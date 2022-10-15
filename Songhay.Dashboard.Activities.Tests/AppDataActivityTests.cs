using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Songhay.Extensions;
using Songhay.Models;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Dashboard.Activities.Tests;

public class AppDataActivityTests
{
    static AppDataActivityTests()
    {
        var basePath = ProgramAssemblyUtility.GetPathFromAssembly(typeof(AppDataActivityTests).Assembly, "../../../");
        Assert.True(Directory.Exists(basePath), $"The expected directory, `{basePath}`, is not here.");
        ProjectDirectoryInfo = new DirectoryInfo(basePath);

        basePath = ProjectDirectoryInfo.Parent.FindDirectory("*.Shell").ToReferenceTypeValueOrThrow().FullName;

        ConfigurationRoot = ProgramUtility
            .LoadConfiguration(
                basePath,
                AppDataActivity.ConventionalSettingsFile);

        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(AppDataActivity.ConventionalSettingsFile, optional: false, reloadOnChange: true);

        ProgramMetadata = new ProgramMetadata();
        builder.Build().Bind(nameof(Songhay.Models.ProgramMetadata), ProgramMetadata);
        Assert.NotNull(ProgramMetadata);
    }

    public AppDataActivityTests(ITestOutputHelper helper) => _testOutputHelper = helper;

    [Fact]
    public async Task DownloadAppFileAsync_Test()
    {
        var activity = new AppDataActivity();
        activity.AddConfiguration(ConfigurationRoot);
        var set = activity.GetMetaSet();
        var appMetadata = AppDataActivity.GetMetadata(set);

        var metadata = activity.GetCloudStorageMetadata();
        var location = AppDataActivity.GetLocation(metadata.accountName, metadata.containerName, appMetadata.appFileName);

        using var request =
            new HttpRequestMessage(HttpMethod.Get, location)
                .WithAzureStorageHeaders(
                    DateTime.UtcNow,
                    metadata.apiVersion,
                    metadata.accountName,
                    metadata.accountKey);
        using HttpResponseMessage response = await request.SendAsync();
        var actual = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var message = $"Request for `{location}` returned status `{response.StatusCode}`.";
            throw new HttpRequestException(message);
        }

        Assert.False(string.IsNullOrWhiteSpace(actual));
    }

    [Fact]
    public void GetCloudStorageMetadata_Test()
    {
        var activity = new AppDataActivity();
        activity.AddConfiguration(ConfigurationRoot);

        var actual = activity.GetCloudStorageMetadata();

        Assert.False(string.IsNullOrWhiteSpace(actual.accountName));
        Assert.False(string.IsNullOrWhiteSpace(actual.accountKey));
        Assert.False(string.IsNullOrWhiteSpace(actual.containerName));
        Assert.False(string.IsNullOrWhiteSpace(actual.apiVersion));
    }

    [Fact]
    public void GetMetaSet_Test()
    {
        var activity = new AppDataActivity();
        activity.AddConfiguration(ConfigurationRoot);

        var actual = activity.GetMetaSet();
        Assert.NotNull(actual);
    }

    [Fact]
    public void GetMetadata_Test()
    {
        var activity = new AppDataActivity();
        activity.AddConfiguration(ConfigurationRoot);
        var set = activity.GetMetaSet();

        var actual = AppDataActivity.GetMetadata(set);
        Assert.NotNull(actual.jsonFiles);
        Assert.NotNull(actual.appFileName);
    }

    static readonly DirectoryInfo ProjectDirectoryInfo;
    static readonly ProgramMetadata ProgramMetadata;
    static readonly IConfigurationRoot ConfigurationRoot;

    readonly ITestOutputHelper _testOutputHelper;
}
