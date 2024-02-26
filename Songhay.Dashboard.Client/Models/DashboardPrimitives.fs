namespace Songhay.Dashboard.Client.Models

open Songhay.Modules.Models
open Songhay.Player.YouTube.Models
open Songhay.Dashboard.Models

open Bolero

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

type DashboardPage =
    | [<EndPoint "/">] StudioToolsPage
    | [<EndPoint "/feeds">] StudioFeedsPage
    | [<EndPoint "/yt/figure">] YouTubeFigurePage

type YouTubeFigure =
    {
        id: string
        title: string
        resolution: string
    }

type DashboardVisualState =
    | YouTubeFigure of YouTubeFigure

type DashboardMessage =
    | ClearError
    | Error of exn
    | GetFeeds | GotFeeds of (FeedName * SyndicationFeed)[] option
    | SetPage of DashboardPage
    | ChangeVisualState of DashboardVisualState
    | YouTubeMessage of YouTubeMessage
