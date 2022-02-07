namespace Songhay.Dashboard.Client

module SyndicationFeedUtility =

    open System
    open System.Text.Json
    open Microsoft.FSharp.Core

    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.ProgramTypeUtility
    open Songhay.Dashboard.Client.Models

    [<Literal>]
    let AtomFeedPropertyName = "feed"

    [<Literal>]
    let RssFeedPropertyName = "rss"

    [<Literal>]
    let SyndicationFeedPropertyName = "feeds"

    let resultError (elementName: string) =
        Error(JsonException $"the expected `{elementName}` element is not here")

    let tryGetJsonElement (rawDocument: string) =
        try
            let document = rawDocument |> JsonDocument.Parse
            Ok document.RootElement
        with
        | exn -> Error exn

    let tryGetProperty (elementName: string) (element: JsonElement) =
        match element.TryGetProperty elementName with
        | false, _ -> resultError elementName
        | true, el -> Ok el

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

    let tryGetAtomChannelItems (element: JsonElement) =

        let toSyndicationFeedItem (el: JsonElement) =

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

            match [ titleResult; linkResult ]
                  |> List.sequenceResultM with
            | Ok [ title; link ] -> Ok { title = title; link = link }
            | Ok _ -> Error(JsonException "The expected number of list items is not here.")
            | Error err -> Error err

        let entryElementResult = element |> tryGetProperty "entry"

        match entryElementResult with
        | Error err -> Error err
        | Ok el ->
            el.EnumerateArray()
            |> Seq.map (fun el -> el |> toSyndicationFeedItem)
            |> List.ofSeq
            |> List.sequenceResultM

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

    let tryGetRssChannelItems (element: JsonElement) =

        let toSyndicationFeedItem (el: JsonElement) =

            let titleResult =
                match el |> tryGetProperty "title" with
                | Error err -> Error err
                | Ok titleElement -> Ok(titleElement.GetString())

            let linkResult =
                match el |> tryGetProperty "link" with
                | Error err -> Error err
                | Ok linkElement -> Ok(linkElement.GetString())

            match [ titleResult; linkResult ]
                  |> List.sequenceResultM with
            | Ok [ title; link ] -> Ok { title = title; link = link }
            | Ok _ -> Error(JsonException "The expected number of list items is not here.")
            | Error err -> Error err

        let itemElementResult =
            match element |> tryGetProperty "channel" with
            | Error err -> Error err
            | Ok channelElement -> channelElement |> tryGetProperty "item"

        match itemElementResult with
        | Error err -> Error err
        | Ok el ->
            el.EnumerateArray()
            |> Seq.map (fun el -> el |> toSyndicationFeedItem)
            |> List.ofSeq
            |> List.sequenceResultM

    let tryGetRssChannelTitle (element: JsonElement) =
        match element |> tryGetProperty "channel" with
        | Error err -> Error err
        | Ok channelElement ->
            match channelElement |> tryGetProperty "title" with
            | Error err -> Error err
            | Ok titleElement -> Ok(titleElement.GetString())

    let tryGetSyndicationFeed (isRssFeed: bool, element: JsonElement) =
        let feedImage: string option = None

        let modificationDateResult =
            element |> tryGetFeedModificationDate isRssFeed

        let feedItemsResult =
            match isRssFeed with
            | true -> tryGetRssChannelItems element
            | _ -> tryGetAtomChannelItems element

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
                    let feedResult = el |> tryGetSyndicationFeed

                    match feedResult with
                    | Error err -> Error err
                    | Ok feed -> Ok(feedName, feed))
        |> List.sequenceResultM
