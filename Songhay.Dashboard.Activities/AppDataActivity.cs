using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Songhay.Abstractions;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;

namespace Songhay.Dashboard.Activities
{
    public class AppDataActivity : IActivity, IActivityConfigurationSupport
    {
        static AppDataActivity() => TraceSource = TraceSources
            .Instance
            .GetConfiguredTraceSource()
            .WithSourceLevels();

        public void AddConfiguration(IConfigurationRoot? configuration)
        {
            Configuration = configuration;

            _programMetadata = new ProgramMetadata();
            configuration?.Bind(nameof(ProgramMetadata), _programMetadata);
        }

        public string DisplayHelp(ProgramArgs? args) => $@"Updates the conventional static JSON file in Azure Storage for the Web App in this Solution.
Use command-line argument {ProgramArgs.BasePath} to prepend a base path to a configured relative path (e.g. dataRoot=""./my-path"").";

        public void Start(ProgramArgs? args)
        {
            var basePath = args.HasArg(ProgramArgs.BasePath, requiresValue: false) ? args.GetBasePathValue() : Directory.GetCurrentDirectory();

            var metaSet = GetMetaSet();
            var (appFileName, jsonFiles) = GetMetadata(metaSet);

            SetDataRoot(basePath);

            var appFile = GetAppFile(appFileName);
            TraceSource?.TraceVerbose($"downloading to {appFile}...");
            var json = DownloadFileToStringAsync(appFileName).GetAwaiter().GetResult();
            File.WriteAllText(appFile, json);

            var jO = JObject.Parse(json);

            WriteServerMeta(jO);

            DownloadFeedFiles(jsonFiles);

            WriteFeedFiles(jsonFiles, jO);

            WriteAppFile(appFileName, jO);

            UploadAppFile(appFileName);
        }

        /// <summary>
        /// The conventional settings file
        /// </summary>
        internal const string ConventionalSettingsFile = "app-settings.songhay-system.json";

        internal static string GetLocation(string accountName, string containerName, string blobName) =>
            $"https://{accountName}.blob.core.windows.net/{containerName}/{blobName}";

        internal static (string appFileName, string[] jsonFiles) GetMetadata(Dictionary<string, string> metaSet) =>
        (
            metaSet.TryGetValueWithKey("appFileName").ToReferenceTypeValueOrThrow(),
            metaSet.TryGetValueWithKey("jsonFiles").ToReferenceTypeValueOrThrow().Split(',')
        );

        internal IConfigurationRoot? Configuration { get; private set; }

        internal void DownloadFeedFiles(IEnumerable<string> jsonFiles)
        {
            jsonFiles.ForEachInEnumerable(i =>
            {
                var jsonFileName = $"{i}.json";
                var jsonFile = GetAppFile(jsonFileName);
                TraceSource?.TraceInformation($"downloading {jsonFile}.json...");

                TraceSource?.TraceVerbose($"downloading to {jsonFile}...");
                var json = DownloadFileToStringAsync(jsonFileName).GetAwaiter().GetResult();
                File.WriteAllText(jsonFile, json);
            });
        }

        internal async Task<string> DownloadFileToStringAsync(string fileName)
        {
            var metadata = GetCloudStorageMetadata();
            var location = GetLocation(metadata.accountName, metadata.containerName, fileName);

            using var request =
                new HttpRequestMessage(HttpMethod.Get, location)
                    .WithAzureStorageHeaders(
                        DateTime.UtcNow,
                        metadata.apiVersion,
                        metadata.accountName,
                        metadata.accountKey);
            using HttpResponseMessage response = await request.SendAsync();
            var s = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return s;

            var message = $"Request for `{location}` returned status `{response.StatusCode}`.";
            throw new HttpRequestException(message);
        }

        internal string GetAppFile(string appFileName) => ProgramFileUtility.GetCombinedPath(_dataRoot, appFileName);

        internal (string accountName, string accountKey, string containerName, string apiVersion) GetCloudStorageMetadata()
        {
            var storageMeta = _programMetadata?
                .CloudStorageSet
                .TryGetValueWithKey("SonghayCloudStorage")
                .ToReferenceTypeValueOrThrow();

            var connectionString = storageMeta
                .TryGetValueWithKey("general-purpose-v1")
                .ToReferenceTypeValueOrThrow();

            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            var values = new[] { "AccountName", "AccountKey" }.Select(key =>
            {
                if (!builder.ContainsKey(key))
                {
                    throw new NullReferenceException($"The expected connection string key, `{key}`, is not here.");
                }

                var value = builder[key] as string;

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NullReferenceException($"The expected connection string value for key, `{key}`, is not here.");
                }

                return value;

            }).ToArray();

            if (values.Length < 2) throw new DataException("The expected connection string values are not here.");

            (string accountName, string accountKey, string containerName, string apiVersion) storageMetadata;

            storageMetadata.accountName = values[0];
            storageMetadata.accountKey = values[1];
            storageMetadata.containerName = GetMetaSet().TryGetValueWithKey("blobContainerName").ToReferenceTypeValueOrThrow();
            storageMetadata.apiVersion = "2019-12-12";

            return storageMetadata;
        }

        internal void SetDataRoot(string? basePath)
        {
            _dataRoot = ProgramFileUtility.GetCombinedPath(basePath, _metaSet.TryGetValueWithKey("dataRoot", throwException: true));

            if (_dataRoot.StartsWith("./")) ProgramFileUtility.GetCombinedPath(basePath, _dataRoot);
            _dataRoot = Path.GetFullPath(_dataRoot);
            if (!Directory.Exists(_dataRoot)) Directory.CreateDirectory(_dataRoot);

            TraceSource?.TraceVerbose($"verifying {_dataRoot}...");
            if (!Directory.Exists(_dataRoot)) throw new DirectoryNotFoundException("The expected data root is not here.");
        }

        internal Dictionary<string, string> GetMetaSet()
        {
            if (_metaSet != null) return _metaSet;

            var metaSection = Configuration?.GetSection("meta")?.GetChildren();
            _metaSet = metaSection
                .ToReferenceTypeValueOrThrow()
                .ToDictionary(i => i.Key, i => i.Value);

            return _metaSet;
        }

        internal void UploadAppFile(string appFileName)
        {
            var appFile = GetAppFile(appFileName);

            if (!File.Exists(appFile)) throw new FileNotFoundException("The expected app file is not here.");

            //container.UploadBlobAsync(appFile, string.Empty).Wait();
        }

        internal void WriteAppFile(string appFileName, JObject appJo)
        {
            var appFile = GetAppFile(appFileName);

            File.WriteAllText(appFile, appJo.ToString());
        }

        internal void WriteFeedFiles(string[] jsonFiles, JObject appJO)
        {
            var feedsRoot = _metaSet.TryGetValueWithKey("feedsRoot").ToReferenceTypeValueOrThrow();

            jsonFiles.ForEachInEnumerable(i =>
            {
                TraceSource?.TraceInformation($"writing {feedsRoot}/{i}...");
                var jsonFile = ProgramFileUtility.GetCombinedPath(_dataRoot, $"{i}.json");
                if (!File.Exists(jsonFile)) throw new FileNotFoundException("The expected feeds file is not here.");
                var jOFeed = JObject.Parse(File.ReadAllText(jsonFile));
                appJO[feedsRoot][i] = jOFeed;
            });
        }

        internal void WriteServerMeta(JObject appJo)
        {
            var serverMetaRoot = _metaSet.TryGetValueWithKey("serverMetaRoot", throwException: true);

            TraceSource?.TraceInformation($"writing server {serverMetaRoot}...");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            var assemblyInfo = new ProgramAssemblyInfo(GetType().Assembly);

            var jO = JObject.FromObject(assemblyInfo, JsonSerializer.Create(settings));
            jO = new JObject { { nameof(assemblyInfo), jO } };
            appJo[serverMetaRoot] = jO;
        }

        static readonly TraceSource? TraceSource;

        static ProgramMetadata? _programMetadata;

        Dictionary<string, string>? _metaSet;
        string? _dataRoot;
    }
}
