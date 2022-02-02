namespace Songhay.Dashboard.Bolero.Tests

module SyndicationFeedTests =

    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Dashboard.Client
    open Songhay.Dashboard.Client.Models
    open Songhay.Modules.ProgramFileUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let appJsonDocumentPath =
        "./json/app.json"
        |> getCombinedPath projectDirectoryInfo.FullName
        |> Result.valueOr raiseProgramFileError

    let appJsonDocument =
        JsonDocument.Parse(File.ReadAllText(appJsonDocumentPath))

    [<Fact>]
    let ``app.json should have `feeds` property`` () =
        let actual =
            match appJsonDocument.RootElement.TryGetProperty SyndicationFeedUtility.SyndicationFeedPropertyName with
            | true, _ -> true
            | _ -> false

        actual |> should be True

    [<Theory>]
    [<InlineData(nameof CodePen, true)>]
    [<InlineData(nameof Flickr, true)>]
    [<InlineData(nameof GitHub, false)>]
    [<InlineData(nameof Studio, true)>]
    [<InlineData(nameof StackOverflow, false)>]
    let ``isRssFeed test`` (elementName: string, expectedResult) =
        let actual =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.isRssFeed elementName

        match expectedResult with
        | true -> actual |> should be True
        | _ -> actual |> should be False

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof Studio)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getFeedElement test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise

        element.ValueKind |> should equal JsonValueKind.Object

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getAtomChannelTitle test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let titleResult = element |> SyndicationFeedUtility.getAtomChannelTitle

        titleResult |> should be (ofCase <@ Result<string, JsonException>.Ok @>)
        let title = titleResult |> Result.valueOr raise
        let actual = title |> System.String.IsNullOrWhiteSpace

        actual |> should be False

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getAtomChannelItems test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let itemsResult = element |> SyndicationFeedUtility.getAtomChannelItems

        itemsResult |> should be (ofCase <@ Result<SyndicationFeedItem list, JsonException>.Ok @>)
        let actual = itemsResult |> Result.valueOr raise

        actual |> should not' (be Empty)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof Studio)>]
    let ``getRssChannelTitle test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let titleResult = element |> SyndicationFeedUtility.getRssChannelTitle

        titleResult |> should be (ofCase <@ Result<string, JsonException>.Ok @>)
        let title = titleResult |> Result.valueOr raise
        let actual = title |> System.String.IsNullOrWhiteSpace

        actual |> should be False

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof Studio)>]
    let ``getRssChannelItems test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let itemsResult = element |> SyndicationFeedUtility.getRssChannelItems

        itemsResult |> should be (ofCase <@ Result<SyndicationFeedItem list, JsonException>.Ok @>)
        let actual = itemsResult |> Result.valueOr raise

        actual |> should not' (be Empty)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof Studio)>]
    [<InlineData(nameof StackOverflow)>]
    let ``toSyndicationFeed test`` elementName =
        let feedElementResult =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        feedElementResult |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let pair = feedElementResult |> Result.valueOr raise
        let feedResult = pair |> SyndicationFeedUtility.toSyndicationFeed

        feedResult |> should be (ofCase <@ Result<SyndicationFeed, JsonException>.Ok @>)
        let feed = feedResult |> Result.valueOr raise

        System.String.IsNullOrWhiteSpace(feed.feedTitle) |> should be False

        feed.feedItems |> should not' (be Empty)

        Assert.All(
            feed.feedItems,
            fun i ->
                System.String.IsNullOrWhiteSpace(i.title)
                |> should be False

                System.String.IsNullOrWhiteSpace(i.link)
                |> should be False
        )

    [<Fact>]
    let ``fromInput test`` () =
        let actualResult =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.fromInput

        actualResult |> should be (ofCase <@ Result<(FeedName * SyndicationFeed) list, JsonException>.Ok @>)
        let actual = actualResult |> Result.valueOr raise

        actual |> should not' (be Empty)

        Assert.All(
            actual,
            fun (_, feed) ->
                feed.feedItems |> should not' (be Empty)

                Assert.All(
                    feed.feedItems,
                    fun i ->
                        System.String.IsNullOrWhiteSpace(i.title)
                        |> should be False

                        System.String.IsNullOrWhiteSpace(i.link)
                        |> should be False
                )
        )
