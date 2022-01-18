namespace Songhay.Dashboard.Client

module SyndicationFeedUtility =

    open System
    open System.Text.Json

    open Songhay.Modules.Models
    open Songhay.Dashboard.Client.Models

    [<Literal>]
    let AtomFeedPropertyName = "feed"

    [<Literal>]
    let RssFeedPropertyName = "rss"

    [<Literal>]
    let SyndicationFeedPropertyName = "feeds"

    let raiseNullReferenceException (elementName: string) =
        raise (NullReferenceException $"the expected `{elementName}` element is not here")

    let getAtomChannelTitle(element: JsonElement): string =
        match element.TryGetProperty "title" with
        | false, _ -> raiseNullReferenceException "title"
        | true, titleElement ->
            match titleElement.ValueKind with
            | JsonValueKind.String -> titleElement.GetString()
            | JsonValueKind.Object ->
                match titleElement.TryGetProperty "#text" with
                | false, _ -> raiseNullReferenceException "#text"
                | true, textElement -> textElement.GetString()
            | _ -> raise (JsonException $"The expected JSON type of `{nameof titleElement}` is not here.")

    let getAtomChannelItems(element: JsonElement): SyndicationFeedItem list =   
        let entryElement =
            match element.TryGetProperty "entry" with
            | false, _ -> raiseNullReferenceException "entry"
            | true, entry -> entry

        let toSyndicationFeedItem(el: JsonElement) =

            let title =
                match el.TryGetProperty "title" with
                | false, _ -> raiseNullReferenceException "`entry.title`"
                | true, titleElement ->
                    match titleElement.TryGetProperty "#text" with
                    | false, _ -> raiseNullReferenceException "entry.title.#text"
                    | true, textElement -> textElement.GetString()

            let link =
                match el.TryGetProperty "link" with
                | false, _ -> raiseNullReferenceException "entry.link"
                | true, linkElement ->
                    match linkElement.TryGetProperty "@href" with
                    | false, _ -> raiseNullReferenceException "entry.link.@href"
                    | true, hrefElement -> hrefElement.GetString()

            { title = title; link = link }

        entryElement.EnumerateArray()
            |> Seq.map (fun el -> el |> toSyndicationFeedItem)
            |> List.ofSeq

    let getRssChannelItems(element: JsonElement): SyndicationFeedItem list =
        let itemElement =
            match element.TryGetProperty "channel" with
            | false, _ -> raiseNullReferenceException "channel"
            | true, channelElement ->
                match channelElement.TryGetProperty "item" with
                | false, _ -> raiseNullReferenceException "item"
                | true, item -> item

        let toSyndicationFeedItem(el: JsonElement) =

            let title =
                match el.TryGetProperty "title" with
                | false, _ -> raiseNullReferenceException "item.title"
                | true, titleElement -> titleElement.GetString()

            let link =
                match el.TryGetProperty "link" with
                | false, _ -> raiseNullReferenceException "item.link"
                | true, linkElement -> linkElement.GetString()

            { title = title; link = link }

        itemElement.EnumerateArray()
            |> Seq.map (fun el -> el |> toSyndicationFeedItem)
            |> List.ofSeq

    let getRssChannelTitle(element: JsonElement): string =
        match element.TryGetProperty "channel" with
        | false, _ -> raiseNullReferenceException "channel"
        | true, channelElement ->
            match channelElement.TryGetProperty "title" with
            | false, _ -> raiseNullReferenceException "title"
            | true, titleElement -> titleElement.GetString()

    let isRssFeed(elementName: string) (element: JsonElement) =
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
            | false, _ -> raiseNullReferenceException SyndicationFeedPropertyName
            | true, feedsElement ->
                match feedsElement.TryGetProperty elementName with
                | false, _ -> raiseNullReferenceException elementName
                | true, targetElement ->
                    match targetElement.TryGetProperty feedPropertyName with
                    | false, _ -> raiseNullReferenceException feedPropertyName
                    | _, feedElement -> feedElement

        match isRssFeed elementName element with
        | true -> true, getElement RssFeedPropertyName
        | false -> true, getElement AtomFeedPropertyName

    let toSyndicationFeed(isRssFeed: bool, element: JsonElement) =
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

    let fromInput element =
        [
            (
                CodePen,
                element |> getFeedElement ((nameof CodePen).ToLowerInvariant()) |> toSyndicationFeed
            )
            (
                Flickr,
                element |> getFeedElement ((nameof Flickr).ToLowerInvariant()) |> toSyndicationFeed
            )
            (
                GitHub,
                element |> getFeedElement ((nameof GitHub).ToLowerInvariant()) |> toSyndicationFeed
            )
            (
                StackOverflow,
                element |> getFeedElement ((nameof StackOverflow).ToLowerInvariant()) |> toSyndicationFeed
            )
            (
                Studio,
                element |> getFeedElement ((nameof Studio).ToLowerInvariant()) |> toSyndicationFeed
            )
        ]
