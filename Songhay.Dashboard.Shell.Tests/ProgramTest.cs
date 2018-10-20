using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Songhay.Cloud.BlobStorage.Extensions;
using Songhay.Extensions;
using Songhay.Models;

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
        [TestProperty("appFile", @"json\app.json")]
        [TestProperty("blobContainerName", "studio-dash")]
        [TestProperty("jsonFiles", "codepen,flickr,github,stackoverflow,studio")]
        [TestProperty("jsonRoot", "json")]
        [TestProperty("serverMetadataFile", @"json\server-meta.json")]
        public async Task ShouldGenerateAppData()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            #region test properties:

            var appFile = this.TestContext.Properties["appFile"].ToString();
            appFile = projectDirectoryInfo.FullName.ToCombinedPath(appFile);
            this.TestContext.ShouldFindFile(appFile);

            var blobContainerName = this.TestContext.Properties["blobContainerName"].ToString();

            var jsonFiles = this.TestContext.Properties["jsonFiles"].ToString().Split(',');

            var jsonRoot = this.TestContext.Properties["jsonRoot"].ToString();
            jsonRoot = projectDirectoryInfo.FullName.ToCombinedPath(jsonRoot);
            this.TestContext.ShouldFindDirectory(jsonRoot);

            var serverMetadataFile = this.TestContext.Properties["serverMetadataFile"].ToString();
            serverMetadataFile = projectDirectoryInfo.FullName.ToCombinedPath(serverMetadataFile);
            this.TestContext.ShouldFindFile(serverMetadataFile);

            #endregion

            var serverMetaRoot = "serverMeta";
            var feedsRoot = "feeds";

            var jO = JObject.Parse(File.ReadAllText(appFile));

            this.TestContext.WriteLine($"writing {serverMetaRoot}...");
            var jO_serverMetadata = JObject.Parse(File.ReadAllText(serverMetadataFile));
            jO[serverMetaRoot] = jO_serverMetadata;

            var container = cloudStorageAccount.CreateCloudBlobClient().GetContainerReference(blobContainerName);
            var tasks = jsonFiles.Select(i =>
            {
                var jsonFile = $"{jsonRoot}/{i}.json";
                this.TestContext.WriteLine($"downloading {jsonFile}.json...");
                var @ref = container.GetBlobReference(blobName: $"{i}.json");
                return @ref.DownloadToFileAsync(jsonFile, FileMode.Create);
            }).ToArray();

            Task.WaitAll(tasks);

            jsonFiles.ForEachInEnumerable(i =>
            {
                this.TestContext.WriteLine($"writing {feedsRoot}/{i}...");
                var jsonFile = $"{jsonRoot}/{i}.json";
                this.TestContext.ShouldFindFile(jsonFile);
                var jO_feed = JObject.Parse(File.ReadAllText(jsonFile));
                jO[feedsRoot][i] = jO_feed;
            });

            File.WriteAllText(appFile, jO.ToString());
            await container.UploadBlob(appFile, string.Empty);
        }

        [TestMethod]
        [TestProperty("serverMetadataFile", @"json\server-meta.json")]
        public void ShouldGenerateServerMetadata()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            #region test properties:

            var serverMetadataFile = this.TestContext.Properties["serverMetadataFile"].ToString();
            serverMetadataFile = projectDirectoryInfo.FullName.ToCombinedPath(serverMetadataFile);
            this.TestContext.ShouldFindFile(serverMetadataFile);

            #endregion

            var jO = JObject.Parse(File.ReadAllText(serverMetadataFile));
            var assemblyInfo = new FrameworkAssemblyInfo(typeof(Program).Assembly);

            this.TestContext.WriteLine($"assemblyInfo: {assemblyInfo}");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            jO[nameof(assemblyInfo)] = JObject.FromObject(assemblyInfo, JsonSerializer.Create(settings));

            File.WriteAllText(serverMetadataFile, jO.ToString());
        }

        static CloudStorageAccount cloudStorageAccount;
    }
}