namespace Songhay.Dashboard.Bolero.Tests

module SyndicationFeedTests =

    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers

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

        match result with
        | Error err -> raise err
        | Ok (_, element) -> JsonValueKind.Object |> should equal element.ValueKind

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getAtomChannelTitle test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        match result with
        | Error err -> raise err
        | Ok (_, element) ->
            let titleResult =
                element
                |> SyndicationFeedUtility.getAtomChannelTitle

            match titleResult with
            | Error err -> raise err
            | Ok title ->
                let actual = title |> System.String.IsNullOrWhiteSpace
                actual |> should be False

    [<Theory>]
    [<InlineData(nameof GitHub)>]
    [<InlineData(nameof StackOverflow)>]
    let ``getAtomChannelItems test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        match result with
        | Error _ -> failwith "The expected result is not here"
        | Ok (_, element) ->
            let itemsResult =
                element
                |> SyndicationFeedUtility.getAtomChannelItems

            match itemsResult with
            | Error _ -> failwith "The expected result is not here"
            | Ok actual -> actual |> should not' (be Empty)

    [<Theory>]
    [<InlineData(nameof CodePen)>]
    [<InlineData(nameof Flickr)>]
    [<InlineData(nameof Studio)>]
    let ``getRssChannelTitle test`` elementName =
        let result =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.getFeedElement elementName

        match result with
        | Error _ -> failwith "The expected result is not here"
        | Ok (_, element) ->
            let titleResult =
                element
                |> SyndicationFeedUtility.getRssChannelTitle

            match titleResult with
            | Error _ -> failwith "The expected result is not here"
            | Ok title ->
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

        match result with
        | Error _ -> failwith "The expected result is not here"
        | Ok (_, element) ->
            let itemsResult =
                element
                |> SyndicationFeedUtility.getRssChannelItems

            match itemsResult with
            | Error _ -> failwith "The expected result is not here"
            | Ok actual -> actual |> should not' (be Empty)

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

        match feedElementResult with
        | Error _ -> failwith "The expected result is not here"
        | Ok pair ->
            let feedResult = pair |> SyndicationFeedUtility.toSyndicationFeed

            match feedResult with
            | Error _ -> failwith "The expected result is not here"
            | Ok feed ->
                System.String.IsNullOrWhiteSpace(feed.feedTitle) |> should be False
                feed.feedItems |> should not' (be Empty)
                Assert.All(feed.feedItems,
                    fun i ->
                        System.String.IsNullOrWhiteSpace(i.title) |> should be False
                        System.String.IsNullOrWhiteSpace(i.link) |> should be False
                    )

    [<Fact>]
    let ``fromInput test`` () =
        let actualResult =
            appJsonDocument.RootElement
            |> SyndicationFeedUtility.fromInput

        match actualResult with
        | Error _ -> failwith "The expected result is not here"
        | Ok actual ->
            actual |> should not' (be Empty)
            Assert.All(actual,
                fun (_, feed) ->
                    feed.feedItems |> should not' (be Empty)
                    Assert.All(feed.feedItems,
                            fun i ->
                                System.String.IsNullOrWhiteSpace(i.title) |> should be False
                                System.String.IsNullOrWhiteSpace(i.link) |> should be False
                            )
                )
