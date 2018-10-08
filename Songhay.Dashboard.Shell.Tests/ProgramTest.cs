using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Songhay.Extensions;
using Songhay.Models;

namespace Songhay.Dashboard.Shell.Tests
{
    [TestClass]
    public class ProgramTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("appFile", @"ClientApp\src\assets\data\app.json")]
        [TestProperty("codepenFile", @"Songhay.Feeds\Songhay.Feeds.Tests\data\codepen.json")]
        [TestProperty("flickrFile", @"Songhay.Feeds\Songhay.Feeds.Tests\data\flickr.json")]
        [TestProperty("githubFile", @"Songhay.Feeds\Songhay.Feeds.Tests\data\github.json")]
        [TestProperty("stackOverflowFile", @"Songhay.Feeds\Songhay.Feeds.Tests\data\stackoverflow.json")]
        [TestProperty("studioFile", @"Songhay.Feeds\Songhay.Feeds.Tests\data\studio.json")]
        [TestProperty("serverMetadataFile", @"json\server-meta.json")]
        public void ShouldGenerateAppData()
        {
            var root = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels : 5);
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());
            var webProjectInfo = this.TestContext.ShouldGetConventionalProjectDirectoryInfo(this.GetType());

            #region test properties:

            var appFile = this.TestContext.Properties["appFile"].ToString();
            appFile = webProjectInfo.FullName.ToCombinedPath(appFile);
            this.TestContext.ShouldFindFile(appFile);

            var codepenFile = this.TestContext.Properties["codepenFile"].ToString();
            codepenFile = root.ToCombinedPath(codepenFile);
            this.TestContext.ShouldFindFile(codepenFile);

            var flickrFile = this.TestContext.Properties["flickrFile"].ToString();
            flickrFile = root.ToCombinedPath(flickrFile);
            this.TestContext.ShouldFindFile(flickrFile);

            var githubFile = this.TestContext.Properties["githubFile"].ToString();
            githubFile = root.ToCombinedPath(githubFile);
            this.TestContext.ShouldFindFile(githubFile);

            var stackOverflowFile = this.TestContext.Properties["stackOverflowFile"].ToString();
            stackOverflowFile = root.ToCombinedPath(stackOverflowFile);
            this.TestContext.ShouldFindFile(stackOverflowFile);

            var studioFile = this.TestContext.Properties["studioFile"].ToString();
            studioFile = root.ToCombinedPath(studioFile);
            this.TestContext.ShouldFindFile(studioFile);

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

            var feeds = new [] { codepenFile, flickrFile, githubFile, stackOverflowFile, studioFile };
            feeds.ForEachInEnumerable(i =>
            {
                var fileName = Path.GetFileNameWithoutExtension(i);
                this.TestContext.WriteLine($"writing {feedsRoot}/{fileName}...");
                var jO_feed = JObject.Parse(File.ReadAllText(i));
                jO[feedsRoot][fileName] = jO_feed;
            });

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