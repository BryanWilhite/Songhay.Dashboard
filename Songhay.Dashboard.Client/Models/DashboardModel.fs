namespace Songhay.Dashboard.Client.Models

open Songhay.Modules.Models
open Songhay.Player.YouTube.Models
open Songhay.Dashboard.Models

type DashboardModel =
    {
        error: string option
        feeds: (FeedName * SyndicationFeed)[] option
        page: DashboardPage
        ytFigureThumbRes: string
        ytFigureTitle: string
        ytFigureVideoId: string
        ytModel: YouTubeModel
    }
