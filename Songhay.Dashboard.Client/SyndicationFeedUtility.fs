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
            match element
                  |> tryGetProperty SyndicationFeedPropertyName with
            | Error err -> Error err
            | Ok feedsElement ->
                match feedsElement
                      |> tryGetProperty elementNameNormalized with
                | Error err -> Error err
                | Ok targetElement ->
                    match targetElement
                          |> tryGetProperty RssFeedPropertyName with
                    | Error err -> Error err
                    | _ -> Ok true

        match testResult with
        | Error _ -> false
        | Ok test -> test

    let tryGetFeedElement (elementName: string) (element: JsonElement) =
        let elementNameNormalized = elementName.ToLowerInvariant()

        let getElement (feedPropertyName: string) =
            match element
                  |> tryGetProperty SyndicationFeedPropertyName with
            | Error err -> Error err
            | Ok feedsElement ->
                match feedsElement
                      |> tryGetProperty elementNameNormalized with
                | Error err -> Error err
                | Ok targetElement ->
                    match targetElement |> tryGetProperty feedPropertyName with
                    | Error err -> Error err
                    | Ok feedElement -> Ok feedElement

        match isRssFeed elementNameNormalized element with
        | true ->
            match getElement RssFeedPropertyName with
            | Error err -> Error err
            | Ok rssElement -> Ok(true, rssElement)
        | _ ->
            match getElement AtomFeedPropertyName with
            | Error err -> Error err
            | Ok atomElement -> Ok(false, atomElement)

    let tryGetFeedModificationDate (isRssFeed: bool) (element: JsonElement) =
        match isRssFeed with
        | false ->
            match element |> tryGetProperty "updated" with
            | Error err -> Error err
            | Ok updatedElement -> Ok(updatedElement.GetDateTime())
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
            match el |> tryGetProperty "title" with
            | Error err -> Error err
            | Ok titleElement ->
                match titleElement |> tryGetProperty "#text" with
                | Error err -> Error err
                | Ok textElement -> Ok(textElement.GetString())

        let linkResult =
            match el |> tryGetProperty "link" with
            | Error err -> Error err
            | Ok linkElement ->
                match linkElement |> tryGetProperty "@href" with
                | Error err -> Error err
                | Ok hrefElement -> Ok(hrefElement.GetString())

        match [ titleResult; linkResult ] |> List.sequenceResultM with
        | Ok [ title; link ] -> Ok { title = title; link = link; extract = None; publicationDate = None }
        | Ok _ -> Error(JsonException "The expected number of list items is not here.")
        | Error err -> Error err

    let tryGetAtomEntries (element: JsonElement) =
        match element |> tryGetProperty "entry" with
        | Error err -> Error err
        | Ok el -> Ok(el.EnumerateArray() |> List.ofSeq)

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
            match el |> tryGetProperty "title" with
            | Error err -> Error err
            | Ok titleElement -> Ok(titleElement.GetString())

        let linkResult =
            match el |> tryGetProperty "link" with
            | Error err -> Error err
            | Ok linkElement -> Ok(linkElement.GetString())

        match [ titleResult; linkResult ] |> List.sequenceResultM with
        | Ok [ title; link ] -> Ok { title = title; link = link; extract = None; publicationDate = None }
        | Ok _ -> Error(JsonException "The expected number of list items is not here.")
        | Error err -> Error err

    let tryGetRssChannelItems (element: JsonElement) =

        let itemElementResult =
            match element |> tryGetProperty "channel" with
            | Error err -> Error err
            | Ok channelElement -> channelElement |> tryGetProperty "item"

        match itemElementResult with
        | Error err -> Error err
        | Ok el -> Ok(el.EnumerateArray() |> List.ofSeq)

    let tryGetRssChannelTitle (element: JsonElement) =
        match element |> tryGetProperty "channel" with
        | Error err -> Error err
        | Ok channelElement ->
            match channelElement |> tryGetProperty "title" with
            | Error err -> Error err
            | Ok titleElement -> Ok(titleElement.GetString())

    let tryGetSyndicationFeed (feedName: FeedName) (isRssFeed: bool, element: JsonElement) =
        let feedElementsResult =
            match isRssFeed with
            | true -> element |> tryGetRssChannelItems
            | false -> element |> tryGetAtomEntries

        let feedImage: string option =
            match feedName with
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
                match feedTitleResult with
                | Error err -> Error err
                | Ok feedTitle ->
                    Ok
                        { feedImage = feedImage
                          feedItems = feedItems
                          feedTitle = feedTitle
                          modificationDate = modificationDate }

    let fromInput element =
        [ CodePen, nameof CodePen
          Flickr, nameof Flickr
          GitHub, nameof GitHub
          StackOverflow, nameof StackOverflow
          Studio, nameof Studio ]
        |> List.map
            (fun (feedName, elementName) ->
                match element |> tryGetFeedElement elementName with
                | Error err -> Error err
                | Ok el ->
                    let feedResult = el |> tryGetSyndicationFeed feedName

                    match feedResult with
                    | Error err -> Error err
                    | Ok feed -> Ok(feedName, feed))
        |> List.sequenceResultM
