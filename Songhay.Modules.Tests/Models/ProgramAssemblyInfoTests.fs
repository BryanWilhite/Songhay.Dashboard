namespace Songhay.Modules.Models.Tests

module ProgramAssemblyInfoTests =

    open System.IO
    open System.Reflection
    open Xunit

    open Songhay.Modules.Models

    [<Theory>]
    [<InlineData(@"..\..\..\Models\SyndicationFeedTests.fs")>]
    [<InlineData("../../../Models/SyndicationFeedTests.fs")>]
    let ``getPathFromAssembly test`` (path: string) =
        let actual =
            Assembly.GetExecutingAssembly()
            |> ProgramAssemblyInfo.getPathFromAssembly path
        Assert.True(File.Exists(actual))
