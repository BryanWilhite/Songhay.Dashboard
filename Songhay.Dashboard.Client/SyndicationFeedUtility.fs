namespace Songhay.Dashboard.Client

open System

open FsToolkit.ErrorHandling

open Songhay.Dashboard.Client.Models

module SyndicationFeedUtility =

    open System.Text.Json
    open Microsoft.FSharp.Core

    open Songhay.Modules.Models

    open Songhay.Modules.JsonDocumentUtility
    open Songhay.Modules.ProgramTypeUtility

    [<Literal>]
    let AtomFeedPropertyName = "feed"

    [<Literal>]
    let RssFeedPropertyName = "rss"

    [<Literal>]
    let SyndicationFeedPropertyName = "feeds"

    let getFeedName (name: string) =
        match name with
        | nameof CodePen -> CodePen
        | nameof Flickr -> Flickr
        | nameof GitHub -> GitHub
        | nameof Studio -> Studio
        | nameof StackOverflow -> StackOverflow
        | _ -> Unknown

    let isRssFeed (elementName: string) (element: JsonElement) =
        let elementNameNormalized = elementName.ToLowerInvariant()

        element
        |> tryGetProperty SyndicationFeedPropertyName
        |> Result.bind (tryGetProperty elementNameNormalized)
        |> Result.bind (tryGetProperty RssFeedPropertyName)
        |> Result.map (fun _ -> true)
        |> Result.valueOr (fun _ -> false)

    let toFeedImage (feedName: FeedName) (headElementResult: Result<JsonElement,JsonException>) =
            match feedName with
            | CodePen ->
                headElementResult
                |> Result.bind (tryGetProperty "link")
                |> Result.either
                    (fun linkProperty -> Some $"{linkProperty.GetString()}/image/large.png")
                    (fun _ -> None)
            | Flickr ->
                headElementResult
                |> Result.bind (tryGetProperty "enclosure")
                |> Result.bind (tryGetProperty "@url")
                |> Result.either
                    (fun urlProperty -> Some (urlProperty.GetString()))
                    (fun _ -> None)
            | _ -> None

    let tryGetFeedElement (elementName: string) (element: JsonElement) =
        let elementNameNormalized = elementName.ToLowerInvariant()

        let getElement (feedPropertyName: string) =
            element |> tryGetProperty SyndicationFeedPropertyName
            |> Result.bind (tryGetProperty elementNameNormalized)
            |> Result.bind (tryGetProperty feedPropertyName)

        match element |> isRssFeed elementNameNormalized with
        | true -> (getElement RssFeedPropertyName) |> Result.map (fun rssElement -> true, rssElement)
        | _ -> (getElement AtomFeedPropertyName) |> Result.map (fun atomElement -> false, atomElement)

    let tryGetFeedModificationDate (isRssFeed: bool) (element: JsonElement) =
        match isRssFeed with
        | false ->
            element |> tryGetProperty "updated" |> Result.map (fun updatedElement -> updatedElement.GetDateTime())
        | true ->
            element
            |> tryGetProperty "channel"
            |> Result.either
                (
                    fun channelElement ->
                        channelElement
                        |> tryGetProperty "pubDate"
                        |> Result.either
                            (
                                fun pubDateElement ->
                                    let dateTimeString = pubDateElement.GetString().Trim()

                                    match tryParseRfc822DateTime dateTimeString with
                                    | Error _ -> resultError "pubDate"
                                    | Ok rfc822DateTime -> Ok rfc822DateTime
                            )
                            (
                                fun _ ->
                                    channelElement
                                    |> tryGetProperty "dc:date"
                                    |> Result.either
                                        (
                                            fun pubDateElement ->
                                                let dateTimeString = pubDateElement.GetString().Trim()

                                                match DateTime.TryParse dateTimeString with
                                                | false, _ -> resultError "dc:date"
                                                | true, dateTime -> Ok dateTime
                                        )
                                        Error
                            )
                )
                Error

    let tryGetSyndicationFeedItem (titleResult: Result<string,JsonException>, linkResult: Result<string,JsonException>) =
        [
            titleResult
            linkResult
        ]
        |> List.sequenceResultM
        |> Result.either
            (
                fun _ ->
                    Ok {
                        title = titleResult |> Result.valueOr raise
                        link = linkResult |> Result.valueOr raise
                        extract = None
                        publicationDate = None
                    }
            )
            Error

    let tryGetAtomSyndicationFeedItem (el: JsonElement) =

        let titleResult =
            el
            |> tryGetProperty "title"
            |> Result.bind (tryGetProperty "#text")
            |> Result.map (fun textElement -> textElement.GetString())

        let linkResult =
            el
            |> tryGetProperty "link"
            |> Result.bind (tryGetProperty "@href")
            |> Result.map (fun hrefElement -> hrefElement.GetString())

        (titleResult, linkResult) |> tryGetSyndicationFeedItem

    let tryGetAtomEntries (element: JsonElement) =
        element
        |> tryGetProperty "entry"
        |> Result.map (fun el -> el.EnumerateArray() |> List.ofSeq)

    let tryGetAtomChannelTitle (element: JsonElement) : Result<string, JsonException> =
        match element |> tryGetProperty "title" with
        | Error err -> Error err
        | Ok titleElement ->
            match titleElement.ValueKind with
            | JsonValueKind.String -> Ok(titleElement.GetString())
            | JsonValueKind.Object ->
                titleElement
                |> tryGetProperty "#text"
                |> Result.map (fun textElement -> textElement.GetString())
            | _ -> resultError (nameof titleElement)

    let tryGetRssSyndicationFeedItem (el: JsonElement) =

        let titleResult =
            el
            |> tryGetProperty "title"
            |> Result.map (fun titleElement -> titleElement.GetString())

        let linkResult =
            el
            |> tryGetProperty "link"
            |> Result.map (fun linkElement -> linkElement.GetString())

        (titleResult, linkResult) |> tryGetSyndicationFeedItem

    let tryGetRssChannelItems (element: JsonElement) =
        element
        |> tryGetProperty "channel"
        |> Result.bind (tryGetProperty "item")
        |> Result.map (fun el -> el.EnumerateArray() |> List.ofSeq)

    let tryGetRssChannelTitle (element: JsonElement) =
        element |> tryGetProperty "channel"
        |> Result.bind (tryGetProperty "title")
        |> Result.map (fun titleElement -> titleElement.GetString())

    let tryGetSyndicationFeed (feedName: FeedName) (isRssFeed: bool, element: JsonElement) =
        let feedElementsResult =
            match isRssFeed with
            | true -> element |> tryGetRssChannelItems
            | false -> element |> tryGetAtomEntries

        let headElementResult = feedElementsResult |> Result.map (fun elements -> elements |> List.head)

        let feedImage = headElementResult |> toFeedImage feedName

        let modificationDateResult = element |> tryGetFeedModificationDate isRssFeed

        let feedItemsResult =
            match isRssFeed with
            | true ->
                feedElementsResult
                |> Result.either
                    (fun elements -> elements |> List.map tryGetRssSyndicationFeedItem |> List.sequenceResultM)
                    Error
            | false ->
                feedElementsResult
                |> Result.either
                    (fun elements -> elements |> List.map tryGetAtomSyndicationFeedItem |> List.sequenceResultM)
                    Error

        let feedTitleResult =
            match isRssFeed with
            | true -> tryGetRssChannelTitle element
            | _ -> tryGetAtomChannelTitle element

        [
            feedItemsResult |> Result.map (fun _ -> true)
            feedTitleResult |> Result.map (fun _ -> true)
            modificationDateResult |> Result.map (fun _ -> true)
        ]
        |> List.sequenceResultM
        |> Result.map ( fun _ ->
                {
                   feedImage = feedImage
                   feedItems = feedItemsResult |> Result.valueOr raise
                   feedTitle = feedTitleResult |> Result.valueOr raise
                   modificationDate = modificationDateResult |> Result.valueOr raise
                }
            )

    let fromInput element =
        [
            CodePen, nameof CodePen
            Flickr, nameof Flickr
            GitHub, nameof GitHub
            StackOverflow, nameof StackOverflow
            Studio, nameof Studio
        ]
        |> List.map
            (
                fun (feedName, elementName) ->
                    element
                    |> tryGetFeedElement elementName
                    |> Result.bind (tryGetSyndicationFeed feedName)
                    |> Result.map (fun feed -> feedName, feed)
            )
        |> List.sequenceResultM
