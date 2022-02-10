namespace Songhay.Modules.Models

open System

type SyndicationFeedItem =
    {
        title: string
        link: string
        extract: string option
        publicationDate: DateTime option
    }

type SyndicationFeed =
    {
        feedImage: string option
        feedItems: SyndicationFeedItem list
        feedTitle: string
        modificationDate: DateTime
    }
