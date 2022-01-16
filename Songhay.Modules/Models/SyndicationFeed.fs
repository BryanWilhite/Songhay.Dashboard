namespace Songhay.Modules.Models

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
