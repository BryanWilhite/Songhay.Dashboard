namespace Songhay.Dashboard.Server.Tests

module SyndicationFeedTests =

    open System
    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.ProgramFileUtility

    open Songhay.Dashboard.Models
    open Songhay.Dashboard.Client.SyndicationFeedUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let appJsonDocumentPath =
        "./json/app.json"
        |> tryGetCombinedPath projectDirectoryInfo.FullName
        |> Result.valueOr raiseProgramFileError

    let appJsonDocument =
        JsonDocument.Parse(File.ReadAllText(appJsonDocumentPath))

    [<Fact>]
    let ``app.json should have `feeds` property`` () =
        let actual =
            match appJsonDocument.RootElement.TryGetProperty SyndicationFeedPropertyName with
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
            |> isRssFeed elementName

        match expectedResult with
        | true -> actual |> should be True
        | _ -> actual |> should be False

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof Studio)>]
    [<InlineData(nameof StackOverflow)>]
    let ``tryGetFeedElement test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> tryGetFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise

        element.ValueKind |> should equal JsonValueKind.Object

    [<Theory>]
    [<InlineData(nameof CodePen, true)>]
    [<InlineData(nameof Flickr, true)>]
    [<InlineData(nameof GitHub, false)>]
    [<InlineData(nameof Studio, true)>]
    [<InlineData(nameof StackOverflow, false)>]
    let ``tryGetFeedModificationDate test`` elementName isRssFeed =
        let result =
            appJsonDocument.RootElement
            |> tryGetFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let modificationDateResult = element |> tryGetFeedModificationDate isRssFeed

        modificationDateResult |> should be (ofCase <@ Result<DateTime, JsonException>.Ok @>)
        let modificationDate = modificationDateResult |> Result.valueOr raise

        modificationDate |> should be (greaterThan DateTime.MinValue)

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``tryGetAtomChannelTitle test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> tryGetFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let titleResult = element |> tryGetAtomChannelTitle

        titleResult |> should be (ofCase <@ Result<string, JsonException>.Ok @>)
        let title = titleResult |> Result.valueOr raise
        let actual = title |> String.IsNullOrWhiteSpace

        actual |> should be False

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``tryGetAtomEntries test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> tryGetFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let itemsResult = element |> tryGetAtomEntries

        itemsResult |> should be (ofCase <@ Result<JsonElement list, JsonException>.Ok @>)
        let actual = itemsResult |> Result.valueOr raise

        actual |> should not' (be Empty)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof Studio)>]
    let ``tryGetRssChannelTitle test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> tryGetFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let titleResult = element |> tryGetRssChannelTitle

        titleResult |> should be (ofCase <@ Result<string, JsonException>.Ok @>)
        let title = titleResult |> Result.valueOr raise
        let actual = title |> String.IsNullOrWhiteSpace

        actual |> should be False

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof Studio)>]
    let ``tryGetRssChannelItems test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> tryGetFeedElement elementName

        result |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let _, element = result |> Result.valueOr raise
        let itemsResult = element |> tryGetRssChannelItems

        itemsResult |> should be (ofCase <@ Result<JsonElement list, JsonException>.Ok @>)
        let actual = itemsResult |> Result.valueOr raise

        actual |> should not' (be Empty)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof Studio)>]
    [<InlineData(nameof StackOverflow)>]
    let ``tryGetSyndicationFeed test`` elementName =
        let feedElementResult =
            appJsonDocument.RootElement
            |> tryGetFeedElement elementName

        feedElementResult |> should be (ofCase <@ Result<bool * JsonElement, JsonException>.Ok @>)
        let pair = feedElementResult |> Result.valueOr raise
        let feedResult = pair |> tryGetSyndicationFeed (elementName |> getFeedName)

        feedResult |> should be (ofCase <@ Result<SyndicationFeed, JsonException>.Ok @>)
        let feed = feedResult |> Result.valueOr raise

        String.IsNullOrWhiteSpace(feed.feedTitle) |> should be False

        feed.feedItems |> should not' (be Empty)

        Assert.All(
            feed.feedItems,
            fun i ->
                String.IsNullOrWhiteSpace(i.title)
                |> should be False

                String.IsNullOrWhiteSpace(i.link)
                |> should be False
        )

    [<Fact>]
    let ``fromInput test`` () =
        let actualResult =
            appJsonDocument.RootElement
            |> fromInput

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
                        String.IsNullOrWhiteSpace(i.title)
                        |> should be False

                        String.IsNullOrWhiteSpace(i.link)
                        |> should be False
                )
        )
