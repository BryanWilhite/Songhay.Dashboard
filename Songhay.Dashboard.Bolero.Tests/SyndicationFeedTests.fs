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
            |> SyndicationFeedUtility.isRssFeed (elementName)

        match expectedResult with
        | true -> Assert.True(actual)
        | _ -> Assert.False(actual)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof Studio)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getFeedElement test`` (elementName) =
        let _, element =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement (elementName)

        Assert.Equal(JsonValueKind.Object, element.ValueKind)

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getAtomChannelTitle test`` (elementName) =
        let _, element =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement (elementName)

        let actual =
            element
            |> SyndicationFeedUtility.getAtomChannelTitle

        Assert.False(System.String.IsNullOrWhiteSpace(actual))

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getAtomChannelItems test`` (elementName) =
        let _, element =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement (elementName)

        let actual =
            element
            |> SyndicationFeedUtility.getAtomChannelItems

        Assert.NotEmpty(actual)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof Studio)>]
    let ``getRssChannelTitle test`` (elementName) =
        let _, element =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement (elementName)

        let actual =
            element
            |> SyndicationFeedUtility.getRssChannelTitle

        Assert.False(System.String.IsNullOrWhiteSpace(actual))

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof Studio)>]
    let ``getRssChannelItems test`` (elementName) =
        let _, element =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement (elementName)

        let actual =
            element
            |> SyndicationFeedUtility.getRssChannelItems

        Assert.NotEmpty(actual)
