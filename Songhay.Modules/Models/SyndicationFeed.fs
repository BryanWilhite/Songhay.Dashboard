namespace Songhay.Modules.Models

open System
open System.Text.Json

type SyndicationFeedItem =
    {
        title: string
        link: string
    }

type SyndicationFeed =
    {
        feedImage: string option
        feedItems: SyndicationFeedItem list
        feedTitle: string
    }

    static member fromInput(rawFeed: JsonElement): SyndicationFeed =
        let feedImage: string option = Some String.Empty
        let feedItems: SyndicationFeedItem list = []
        let feedTitle: string = String.Empty

        raise (NotImplementedException (nameof SyndicationFeed.fromInput))

    static member getAtomChannelTitle(element: JsonElement): string =
        raise (NotImplementedException (nameof SyndicationFeed.getAtomChannelTitle))

    static member getAtomChannelItems(element: JsonElement): SyndicationFeedItem list =
        raise (NotImplementedException (nameof SyndicationFeed.getAtomChannelItems))

    static member getRssChannelItems(element: JsonElement): SyndicationFeedItem list =
        raise (NotImplementedException (nameof SyndicationFeed.getRssChannelItems))

    static member getRssChannelTitle(element: JsonElement): string =
        raise (NotImplementedException (nameof SyndicationFeed.getRssChannelTitle))
