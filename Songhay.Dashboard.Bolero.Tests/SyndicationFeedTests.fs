namespace Songhay.Dashboard.Bolero.Tests

module SyndicationFeedTests =

    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit

    open Songhay.Modules
    open Songhay.Modules.Models
    open Songhay.Dashboard.Client
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
    [<InlineData(nameof CodePen, true)>]
    [<InlineData(nameof Flickr, true)>]
    [<InlineData(nameof GitHub, false)>]
    [<InlineData(nameof Studio, true)>]
    [<InlineData(nameof StackOverflow, false)>]
    let ``isRssFeed test`` (elementName: string, expectedResult) =
        let actual =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.isRssFeed (elementName.ToLowerInvariant())

        match expectedResult with
        | true -> Assert.True(actual)
        | _ -> Assert.False(actual)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof Studio)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getFeedElement test`` (elementName: string) =
        let actual =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement (elementName.ToLowerInvariant())

        Assert.True(Option.isSome (snd actual))
