using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Songhay.Cloud.BlobStorage.Extensions;
using Songhay.Dashboard.Activities;
using Songhay.Extensions;
using Songhay.Models;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Songhay.Dashboard.Shell.Tests
{
    [TestClass]
    public class ProgramTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            var projectInfo = this.TestContext.ShouldGetSiblingDirectoryInfoByName(this.GetType(), typeof(Program).Namespace);

            var builder = new ConfigurationBuilder();
            cloudStorageAccount = builder.ToCloudStorageAccount(projectInfo.FullName,
                "app-settings.songhay-system.json",
                "SonghayCloudStorage",
                "general-purpose-v1");
        }

        [TestMethod]
        [TestProperty("serverAssemblyFile", @"bin\Release\netcoreapp2.0\Songhay.Dashboard.dll")]
        public async Task ShouldGenerateAppData()
        {
            var shellDirectoryInfo = this.TestContext.ShouldGetSiblingDirectoryInfoByName(this.GetType(), typeof(Program).Namespace);
            var webDirectoryInfo = this.TestContext.ShouldGetConventionalProjectDirectoryInfo(this.GetType());
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            #region test properties:

            var serverAssemblyFile = this.TestContext.Properties["serverAssemblyFile"].ToString();
            serverAssemblyFile = webDirectoryInfo.FullName.ToCombinedPath(serverAssemblyFile);
            this.TestContext.ShouldFindFile(serverAssemblyFile);

            #endregion

            var configuration = Program.LoadConfiguration(shellDirectoryInfo.FullName);
            var metaSection = configuration.GetSection("meta")?.GetChildren();
            Assert.IsTrue(metaSection.Any(), "The expected section is not here.");

            var metaSet = metaSection.ToDictionary(i => i.Key, i => i.Value);
            var meta = new
            {
                appFileName = metaSet.TryGetValueWithKey("appFileName", throwException: true),
                blobContainerName = metaSet.TryGetValueWithKey("blobContainerName", throwException: true),
                dataRoot = projectDirectoryInfo.FullName.ToCombinedPath(metaSet.TryGetValueWithKey("dataRoot", throwException: true)),
                feedsRoot = metaSet.TryGetValueWithKey("feedsRoot", throwException: true),
                jsonFiles = metaSet.TryGetValueWithKey("jsonFiles", throwException: true).Split(','),
                serverMetaRoot = metaSet.TryGetValueWithKey("serverMetaRoot", throwException: true)
            };

            this.TestContext.ShouldFindDirectory(meta.dataRoot);

            var activitiesGetter = Program.GetActivitiesGetter(new[] { nameof(AppDataActivity) });
            var activity = activitiesGetter.GetActivity();
            Assert.IsNotNull(activity, "The expected Activity is not here.");
            Assert.IsTrue(activity is AppDataActivity, "The expected Activity type is not here.");

            var appFile = $"{meta.dataRoot}/{meta.appFileName}";
            this.TestContext.ShouldFindFile(appFile);

            var jO = JObject.Parse(File.ReadAllText(appFile));

            this.TestContext.WriteLine($"writing {meta.serverMetaRoot}...");
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            var serverAssembly = Assembly.LoadFile(serverAssemblyFile);
            var assemblyInfo = new FrameworkAssemblyInfo(serverAssembly);
            var jO_serverMetadata = JObject.FromObject(assemblyInfo, JsonSerializer.Create(settings));
            jO[meta.serverMetaRoot] = jO_serverMetadata;

            var container = cloudStorageAccount.CreateCloudBlobClient().GetContainerReference(meta.blobContainerName);
            var tasks = meta.jsonFiles.Select(i =>
            {
                var jsonFile = $"{meta.dataRoot}/{i}.json";
                this.TestContext.ShouldFindFile(jsonFile);
                this.TestContext.WriteLine($"downloading {jsonFile}.json...");
                var @ref = container.GetBlobReference(blobName: $"{i}.json");
                return @ref.DownloadToFileAsync(jsonFile, FileMode.Create);
            }).ToArray();

            Task.WaitAll(tasks);

            meta.jsonFiles.ForEachInEnumerable(i =>
            {
                this.TestContext.WriteLine($"writing {meta.feedsRoot}/{i}...");
                var jsonFile = $"{meta.dataRoot}/{i}.json";
                this.TestContext.ShouldFindFile(jsonFile);
                var jO_feed = JObject.Parse(File.ReadAllText(jsonFile));
                jO[meta.feedsRoot][i] = jO_feed;
            });

            File.WriteAllText(appFile, jO.ToString());
            await container.UploadBlob(appFile, string.Empty);
        }

        static CloudStorageAccount cloudStorageAccount;
    }
}