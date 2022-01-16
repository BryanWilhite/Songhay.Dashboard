namespace Songhay.Dashboard.Client.Models

module SyndicationFeedUtility =

    open System
    open System.Text.Json

    open Songhay.Modules.Models

    [<Literal>]
    let AtomFeedPropertyName = "atom"

    [<Literal>]
    let RssFeedPropertyName = "rss"

    [<Literal>]
    let SyndicationFeedPropertyName = "feeds"

    let fromInput(rawFeed: JsonElement): SyndicationFeed =
        let feedImage: string option = Some String.Empty
        let feedItems: SyndicationFeedItem list = []
        let feedTitle: string = String.Empty

        raise (NotImplementedException "fromInput")

    let getAtomChannelTitle(element: JsonElement): string =
        raise (NotImplementedException "getAtomChannelTitle")

    let getAtomChannelItems(element: JsonElement): SyndicationFeedItem list =
        raise (NotImplementedException "getAtomChannelItems")

    let getRssChannelItems(element: JsonElement): SyndicationFeedItem list =
        raise (NotImplementedException "getRssChannelItems")

    let getRssChannelTitle(element: JsonElement): string =
        raise (NotImplementedException "getRssChannelTitle")

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
