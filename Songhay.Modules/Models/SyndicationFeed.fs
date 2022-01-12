namespace Songhay.Modules.Models

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

    static member getAtomChannelTitle(rawFeed: JsonElement): string option =
        Some System.String.Empty

    static member getAtomChannelItems(rawFeed: JsonElement): SyndicationFeedItem list =
        [{title = System.String.Empty; link = System.String.Empty}]

    static member getRssChannelItems(rawFeed: JsonElement): SyndicationFeedItem list =
        [{title = System.String.Empty; link = System.String.Empty}]

    static member getRssChannelTitle(rawFeed: JsonElement): string option =
        Some System.String.Empty
