using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Songhay.Extensions;
using Songhay.Models;
using System.IO;

namespace Songhay.Dashboard.Shell.Tests
{
    [TestClass]
    public class ProgramTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("appFile", @"ClientApp\src\assets\data\app.json")]
        [TestProperty("indexFile", @"json\index.json")]
        [TestProperty("serverMetadataFile", @"json\server-meta.json")]
        public void ShouldGenerateAppData()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());
            var webProjectInfo = this.TestContext.ShouldGetConventionalProjectDirectoryInfo(this.GetType());

            #region test properties:

            var appFile = this.TestContext.Properties["appFile"].ToString();
            appFile = Path.Combine(webProjectInfo.FullName, appFile);
            this.TestContext.ShouldFindFile(appFile);

            var indexFile = this.TestContext.Properties["indexFile"].ToString();
            indexFile = Path.Combine(projectDirectoryInfo.FullName, indexFile);
            this.TestContext.ShouldFindFile(indexFile);

            var serverMetadataFile = this.TestContext.Properties["serverMetadataFile"].ToString();
            serverMetadataFile = Path.Combine(projectDirectoryInfo.FullName, serverMetadataFile);
            this.TestContext.ShouldFindFile(serverMetadataFile);

            #endregion

            var serverMetaRoot = "serverMeta";
            var indexRoot = "index";

            var jO = JObject.Parse(File.ReadAllText(appFile));

            var jO_serverMetadata = JObject.Parse(File.ReadAllText(serverMetadataFile));

            var jA_index = JArray.Parse(File.ReadAllText(indexFile));

            jO[serverMetaRoot] = jO_serverMetadata;
            jO[indexRoot] = jA_index;

            File.WriteAllText(appFile, jO.ToString());
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
    }
}
