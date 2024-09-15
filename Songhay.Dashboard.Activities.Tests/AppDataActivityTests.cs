using System.IO;
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
        var projectDirectoryInfo = new DirectoryInfo(basePath);

        basePath = projectDirectoryInfo.Parent.FindDirectory("*.Shell").ToReferenceTypeValueOrThrow().FullName;

        ConfigurationRoot = ProgramUtility
            .LoadConfiguration(
                basePath,
                AppDataActivity.ConventionalSettingsFile);

        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(AppDataActivity.ConventionalSettingsFile, optional: false, reloadOnChange: true);

        var programMetadata = new ProgramMetadata();
        builder.Build().Bind(nameof(Songhay.Models.ProgramMetadata), programMetadata);
        Assert.NotNull(programMetadata);
    }

    public AppDataActivityTests(ITestOutputHelper helper) => _testOutputHelper = helper;

    [Fact]
    public void GetMetaSet_Test()
    {
        var activity = new AppDataActivity();
        activity.AddConfiguration(ConfigurationRoot);

        var actual = activity.GetMetaSet();
        Assert.NotNull(actual);

        actual.ForEachInEnumerable(i =>
        {
            _testOutputHelper.WriteLine($"{i.Key}: {i.Value}");
        });
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

        actual.jsonFiles.ForEachInEnumerable(jsonFile =>
        {
            _testOutputHelper.WriteLine($"{nameof(jsonFile)}: {jsonFile}");
        });
        _testOutputHelper.WriteLine($"{nameof(actual.appFileName)}: {actual.appFileName}");
    }

    static readonly IConfigurationRoot ConfigurationRoot;

    readonly ITestOutputHelper _testOutputHelper;
}
