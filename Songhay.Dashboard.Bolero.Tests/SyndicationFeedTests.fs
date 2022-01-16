namespace Songhay.Dashboard.Bolero.Tests

module SyndicationFeedTests =

    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit

    open Songhay.Modules
    open Songhay.Modules.Models
    open Songhay.Dashboard.Client.Models

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> DirectoryInfo

    let appJsonDocumentPath =
        "./json/app.json"
        |> ProgramFileUtility.getCombinedPath projectDirectoryInfo.FullName

    let appJsonDocument = JsonDocument.Parse(File.ReadAllText(appJsonDocumentPath))

    [<Fact>]
    let ``app.json should have `feeds` property`` () =
        let result =
            match appJsonDocument.RootElement.TryGetProperty SyndicationFeedUtility.SyndicationFeedPropertyName with
            | true, _ -> true
            | _ -> false

        Assert.True(result)

    [<Theory>]
    [<InlineData("codepen", true)>]
    [<InlineData("flickr", true)>]
    [<InlineData("github", false)>]
    [<InlineData("studio", true)>]
    [<InlineData("stackoverflow", false)>]
    let ``isRssFeed test`` (elementName, expectedResult) =
        let actual =  appJsonDocument.RootElement |> SyndicationFeedUtility.isRssFeed elementName
        match expectedResult with
        | true -> Assert.True(actual)
        | _ -> Assert.False(actual)
