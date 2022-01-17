namespace Songhay.Dashboard.Client

module SyndicationFeedUtility =

    open System
    open System.Text.Json

    open Songhay.Modules.Models

    [<Literal>]
    let AtomFeedPropertyName = "feed"

    [<Literal>]
    let RssFeedPropertyName = "rss"

    [<Literal>]
    let SyndicationFeedPropertyName = "feeds"

    let getAtomChannelTitle(element: JsonElement): string =
        match element.TryGetProperty "title" with
        | false, _ -> raise (NullReferenceException "the expected `title` element is not here")
        | true, titleElement ->
            match titleElement.TryGetProperty "#text" with
            | false, _ -> raise (NullReferenceException "the expected `#text` element is not here")
            | true, textElement -> textElement.GetString()

    let getAtomChannelItems(element: JsonElement): SyndicationFeedItem list =   
        let entryElement =
            match element.TryGetProperty "entry" with
            | false, _ -> raise (NullReferenceException "the expected `entry` element is not here")
            | true, entry -> entry

        let toSyndicationFeedItem(el: JsonElement) =

            let title =
                match el.TryGetProperty "title" with
                | false, _ -> raise (NullReferenceException "the expected `entry.title` element is not here")
                | true, titleElement ->
                    match titleElement.TryGetProperty "#text" with
                    | false, _ -> raise (NullReferenceException "the expected `entry.title.#text` element is not here")
                    | true, textElement -> textElement.GetString()

            let link =
                match el.TryGetProperty "link" with
                | false, _ -> raise (NullReferenceException "the expected `entry.link` element is not here")
                | true, linkElement ->
                    match linkElement.TryGetProperty "@href" with
                    | false, _ -> raise (NullReferenceException "the expected `entry.link.@href` element is not here")
                    | true, hrefElement -> hrefElement.GetString()

            { title = title; link = link }

        entryElement.EnumerateArray()
            |> Seq.map (fun el -> el |> toSyndicationFeedItem)
            |> List.ofSeq

    let getRssChannelItems(element: JsonElement): SyndicationFeedItem list =
        let itemElement =
            match element.TryGetProperty "channel" with
            | false, _ -> raise (NullReferenceException "the expected `channel` element is not here")
            | true, channelElement ->
                match channelElement.TryGetProperty "item" with
                | false, _ -> raise (NullReferenceException "the expected `item` element is not here")
                | true, item -> item

        let toSyndicationFeedItem(el: JsonElement) =

            let title =
                match el.TryGetProperty "title" with
                | false, _ -> raise (NullReferenceException "the expected `item.title` element is not here")
                | true, titleElement -> titleElement.GetString()

            let link =
                match el.TryGetProperty "link" with
                | false, _ -> raise (NullReferenceException "the expected `item.link` element is not here")
                | true, linkElement -> linkElement.GetString()

            { title = title; link = link }

        itemElement.EnumerateArray()
            |> Seq.map (fun el -> el |> toSyndicationFeedItem)
            |> List.ofSeq

    let getRssChannelTitle(element: JsonElement): string =
        match element.TryGetProperty "channel" with
        | false, _ -> raise (NullReferenceException "the expected `channel` element is not here")
        | true, channelElement ->
            match channelElement.TryGetProperty "title" with
            | false, _ -> raise (NullReferenceException "the expected `title` element is not here")
            | true, titleElement -> titleElement.GetString()

    let isRssFeed(elementName: string) (element: JsonElement): bool =
        match element.TryGetProperty SyndicationFeedPropertyName with
        | false, _ -> false
        | true, feedsElement ->
            match feedsElement.TryGetProperty elementName with
            | false, _ -> false
            | true, targetElement ->
                match targetElement.TryGetProperty RssFeedPropertyName with
                | false, _ -> false
                | _ -> true

    let getFeedElement(elementName: string) (element: JsonElement) =
        let getElement (feedPropertyName: string) =
            match element.TryGetProperty SyndicationFeedPropertyName with
            | false, _ -> None
            | true, feedsElement ->
                match feedsElement.TryGetProperty elementName with
                | false, _ -> None
                | true, targetElement ->
                    match targetElement.TryGetProperty feedPropertyName with
                    | false, _ -> None
                    | _, feedElement -> Some feedElement

        match isRssFeed elementName element with
        | true -> isRssFeed, getElement RssFeedPropertyName
        | false -> isRssFeed, getElement AtomFeedPropertyName

    let fromInput(isRssFeed: bool, element: JsonElement): SyndicationFeed =
        let feedImage: string option = None

        let feedItems =
            match isRssFeed with
            | true -> getRssChannelItems element
            | _ -> getAtomChannelItems element

        let feedTitle =
            match isRssFeed with
            | true -> getRssChannelTitle element
            | _ -> getAtomChannelTitle element

        { feedImage = feedImage; feedItems = feedItems; feedTitle = feedTitle }
