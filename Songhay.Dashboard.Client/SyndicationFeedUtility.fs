namespace Songhay.Dashboard.Client

open System
open Songhay.Dashboard.Client.Models

module SyndicationFeedUtility =

    open System.Text.Json
    open Microsoft.FSharp.Core

    open FsToolkit.ErrorHandling

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

        let testResult =
            element
            |> tryGetProperty SyndicationFeedPropertyName
            |> Result.bind (tryGetProperty elementNameNormalized)
            |> Result.bind (tryGetProperty RssFeedPropertyName)
            |> Result.map (fun _ -> true)

        match testResult with
        | Error _ -> false
        | Ok test -> test

    let tryGetFeedElement (elementName: string) (element: JsonElement) =
        let elementNameNormalized = elementName.ToLowerInvariant()

        let getElement (feedPropertyName: string) =
            element |> tryGetProperty SyndicationFeedPropertyName
            |> Result.bind (tryGetProperty elementNameNormalized)
            |> Result.bind (tryGetProperty feedPropertyName)

        match isRssFeed elementNameNormalized element with
        | true -> (getElement RssFeedPropertyName) |> Result.map (fun rssElement -> true, rssElement)
        | _ -> (getElement AtomFeedPropertyName) |> Result.map (fun atomElement -> false, atomElement)

    let tryGetFeedModificationDate (isRssFeed: bool) (element: JsonElement) =
        match isRssFeed with
        | false ->
            element |> tryGetProperty "updated" |> Result.map (fun updatedElement -> updatedElement.GetDateTime())
        | true ->
            match element |> tryGetProperty "channel" with
            | Error err -> Error err
            | Ok channelElement ->
                match channelElement |> tryGetProperty "pubDate" with
                | Error _ ->
                    match channelElement |> tryGetProperty "dc:date" with
                    | Error err -> Error err
                    | Ok pubDateElement ->
                        let dateTimeString = pubDateElement.GetString().Trim()

                        match DateTime.TryParse dateTimeString with
                        | false, _ -> resultError "dc:date"
                        | true, dateTime -> Ok dateTime
                | Ok pubDateElement ->
                    let dateTimeString = pubDateElement.GetString().Trim()

                    match tryParseRfc822DateTime dateTimeString with
                    | Error _ -> resultError "pubDate"
                    | Ok rfc822DateTime -> Ok rfc822DateTime

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

        match [ titleResult; linkResult ] |> List.sequenceResultM with
        | Ok [ title; link ] -> Ok { title = title; link = link; extract = None; publicationDate = None }
        | Ok _ -> Error(JsonException "The expected number of list items is not here.")
        | Error err -> Error err

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
                match titleElement |> tryGetProperty "#text" with
                | Error err -> Error err
                | Ok textElement -> Ok(textElement.GetString())
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

        match [ titleResult; linkResult ] |> List.sequenceResultM with
        | Ok [ title; link ] -> Ok { title = title; link = link; extract = None; publicationDate = None }
        | Ok _ -> Error(JsonException "The expected number of list items is not here.")
        | Error err -> Error err

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

        let feedImage: string option =
            let headElementResult = feedElementsResult |> Result.map (fun elements -> elements |> List.head)

            match feedName with
            | CodePen ->
                match headElementResult |> Result.bind (tryGetProperty "link") with
                | Ok linkProperty -> Some $"{linkProperty.GetString()}/image/large.png"
                | _ -> None
            | Flickr ->
                match headElementResult |> Result.bind (tryGetProperty "enclosure") with
                | Ok enclosureProperty ->
                    match enclosureProperty |> tryGetProperty "@url" with
                    | Ok urlProperty -> Some (urlProperty.GetString())
                    | _ -> None
                | _ -> None
            | _ -> None

        let modificationDateResult =
            element |> tryGetFeedModificationDate isRssFeed

        let feedItemsResult =
            match isRssFeed with
            | true ->
                match feedElementsResult with
                | Error err -> Error err
                | Ok elements -> elements |> List.map tryGetRssSyndicationFeedItem |> List.sequenceResultM
            | false ->
                match feedElementsResult with
                | Error err -> Error err
                | Ok elements -> elements |> List.map tryGetAtomSyndicationFeedItem |> List.sequenceResultM

        let feedTitleResult =
            match isRssFeed with
            | true -> tryGetRssChannelTitle element
            | _ -> tryGetAtomChannelTitle element

        match modificationDateResult with
        | Error err -> Error err
        | Ok modificationDate ->
            match feedItemsResult with
            | Error err -> Error err
            | Ok feedItems ->
                feedTitleResult |> Result.map
                    (
                       fun feedTitle ->
                           {
                               feedImage = feedImage
                               feedItems = feedItems
                               feedTitle = feedTitle
                               modificationDate = modificationDate
                           }
                    )

    let fromInput element =
        [ CodePen, nameof CodePen
          Flickr, nameof Flickr
          GitHub, nameof GitHub
          StackOverflow, nameof StackOverflow
          Studio, nameof Studio ]
        |> List.map
            (fun (feedName, elementName) ->
                element |> tryGetFeedElement elementName
                |> Result.bind (tryGetSyndicationFeed feedName)
                |> Result.map (fun feed -> feedName, feed)
            )
        |> List.sequenceResultM
