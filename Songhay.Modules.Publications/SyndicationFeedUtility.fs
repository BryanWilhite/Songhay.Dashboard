namespace Songhay.Modules.Publications

module SyndicationFeedUtility =

    open System
    open System.Text.Json

    open FsToolkit.ErrorHandling

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

    let isRssFeed (elementName: string) (element: JsonElement) =
        let elementNameNormalized = elementName.ToLowerInvariant()

        element
        |> tryGetProperty SyndicationFeedPropertyName
        |> Result.bind (tryGetProperty elementNameNormalized)
        |> Result.bind (tryGetProperty RssFeedPropertyName)
        |> Result.map (fun _ -> true)
        |> Result.valueOr (fun _ -> false)

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
