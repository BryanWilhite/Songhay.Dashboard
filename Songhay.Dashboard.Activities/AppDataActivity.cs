using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Songhay.Cloud.BlobStorage.Extensions;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Songhay.Dashboard.Activities
{
    public class AppDataActivity : IActivity, IActivityConfigurationSupport
    {
        static AppDataActivity() => traceSource = TraceSources
            .Instance
            .GetTraceSourceFromConfiguredName()
            .WithAllSourceLevels()
            .EnsureTraceSource();

        public void AddConfiguration(IConfigurationRoot configuration)
        {
            this.Configuration = configuration;
        }

        public string DisplayHelp(ProgramArgs args) => $@"Updates the conventional static JSON file in Azure Storage for the Web App in this Solution.
Use command-line argument {ProgramArgs.BasePath} to prepend a base path to a configured relative path (e.g. dataRoot=""./my-path"").";

        public void Start(ProgramArgs args)
        {
            if (!args.HasArg(DashboardActivitiesArgs.ServerAssemblyFile, requiresValue: true)) return;

            this.SetMetadata(args);
            this.SetDataRoot(args);

            var container = this.GetContainerReference();
            var appJson = this.DownloadAppFile(container);
            var jO = JObject.Parse(appJson);

            var serverAssemblyFile = this.GetServerAssemblyFile(args);
            this.WriteServerMeta(jO, serverAssemblyFile);

            this.DownloadFeedFiles(container);
            this.WriteFeedFiles(jO);

            this.WriteAppFile(jO);
            this.UploadAppFile(container);
        }

        internal IConfigurationRoot Configuration { get; private set; }

        internal string DownloadAppFile(CloudBlobContainer container)
        {
            var appFile = this.GetAppFile();

            traceSource.TraceVerbose($"downloading to {appFile}...");

            var @ref = container.GetBlobReference(blobName: this._appFileName);
            @ref.DownloadToFileAsync(appFile, FileMode.Create).Wait();

            if (!File.Exists(appFile)) throw new FileNotFoundException("The expected app file is not here.");

            return File.ReadAllText(appFile);
        }

        internal void DownloadFeedFiles(CloudBlobContainer container)
        {
            var tasks = this._jsonFiles.Select(i =>
            {
                var jsonFile = $"{this._dataRoot}/{i}.json";
                traceSource.TraceInformation($"downloading {jsonFile}.json...");

                var @ref = container.GetBlobReference(blobName: $"{i}.json");
                return @ref.DownloadToFileAsync(jsonFile, FileMode.Create);
            }).ToArray();

            Task.WaitAll(tasks);
        }

        internal string GetAppFile() => $"{this._dataRoot}/{this._appFileName}";

        internal void SetDataRoot(ProgramArgs args)
        {
            var basePath = args.HasArg(ProgramArgs.BasePath, requiresValue: false) ? args.GetBasePathValue() : Directory.GetCurrentDirectory();

            this._dataRoot = basePath.ToCombinedPath(this._metaSet.TryGetValueWithKey("dataRoot", throwException: true));

            if (this._dataRoot.StartsWith("./")) basePath.ToCombinedPath(this._dataRoot);
            this._dataRoot = Path.GetFullPath(this._dataRoot);
            if (!Directory.Exists(this._dataRoot)) Directory.CreateDirectory(this._dataRoot);

            traceSource.TraceVerbose($"verifying {this._dataRoot}...");
            if (!Directory.Exists(this._dataRoot)) throw new DirectoryNotFoundException("The expected data root is not here.");
        }

        internal CloudStorageAccount GetCloudStorageAccount() =>
            this.Configuration.GetCloudStorageAccount("ProgramMetadata:CloudStorageSet:SonghayCloudStorage:general-purpose-v1");

        internal CloudBlobContainer GetContainerReference()
        {
            var blobContainerName = this._metaSet.TryGetValueWithKey("blobContainerName", throwException: true);
            var cloudStorageAccount = this.GetCloudStorageAccount();
            var container = cloudStorageAccount.CreateCloudBlobClient().GetContainerReference(blobContainerName);
            return container;
        }

        internal string GetServerAssemblyFile(ProgramArgs args)
        {
            var serverAssemblyFile = args.GetArgValue(DashboardActivitiesArgs.ServerAssemblyFile);
            traceSource.TraceVerbose($"Looking for {serverAssemblyFile}...");

            if (!File.Exists(serverAssemblyFile)) throw new FileNotFoundException("The expected server assembly file is not here.");

            return serverAssemblyFile;
        }

        internal void SetMetadata(ProgramArgs args)
        {
            var metaSection = this.Configuration.GetSection("meta")?.GetChildren();
            this._metaSet = metaSection.ToDictionary(i => i.Key, i => i.Value);
            this._appFileName = this._metaSet.TryGetValueWithKey("appFileName", throwException: true);
            this._jsonFiles = this._metaSet.TryGetValueWithKey("jsonFiles", throwException: true).Split(',');
        }

        internal void UploadAppFile(CloudBlobContainer container)
        {
            var appFile = this.GetAppFile();

            if (!File.Exists(appFile)) throw new FileNotFoundException("The expected app file is not here.");

            container.UploadBlob(appFile, string.Empty).Wait();
        }

        internal void WriteAppFile(JObject appJO)
        {
            var appFile = this.GetAppFile();

            File.WriteAllText(appFile, appJO.ToString());
        }

        internal void WriteFeedFiles(JObject appJO)
        {
            var feedsRoot = this._metaSet.TryGetValueWithKey("feedsRoot", throwException: true);

            this._jsonFiles.ForEachInEnumerable(i =>
            {
                traceSource.TraceInformation($"writing {feedsRoot}/{i}...");
                var jsonFile = $"{this._dataRoot}/{i}.json";
                if (!File.Exists(jsonFile)) throw new FileNotFoundException("The expected feeds file is not here.");
                var jO_feed = JObject.Parse(File.ReadAllText(jsonFile));
                appJO[feedsRoot][i] = jO_feed;
            });
        }

        internal void WriteServerMeta(JObject appJO, string serverAssemblyFile)
        {
            var serverMetaRoot = this._metaSet.TryGetValueWithKey("serverMetaRoot", throwException: true);

            traceSource.TraceInformation($"writing server {serverMetaRoot}...");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            var serverAssembly = Assembly.LoadFile(serverAssemblyFile);
            var assemblyInfo = new FrameworkAssemblyInfo(serverAssembly);

            var jO = JObject.FromObject(assemblyInfo, JsonSerializer.Create(settings));
            jO = new JObject { { nameof(assemblyInfo), jO } };
            appJO[serverMetaRoot] = jO;
        }

        static readonly TraceSource traceSource;

        Dictionary<string, string> _metaSet;
        string _appFileName;
        string _dataRoot;
        string[] _jsonFiles;
    }
}
