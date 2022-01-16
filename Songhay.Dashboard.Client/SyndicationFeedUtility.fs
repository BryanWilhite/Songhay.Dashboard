namespace Songhay.Dashboard.Client

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

    let getFeedElement(elementName: string) (element: JsonElement): JsonElement =
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
        | true ->
            ()
        | false ->
            ()

        raise (NotImplementedException "getFeedElement")
