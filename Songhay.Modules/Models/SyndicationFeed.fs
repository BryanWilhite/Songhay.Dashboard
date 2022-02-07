namespace Songhay.Modules.Models

open System

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
        modificationDate: DateTime
    }
