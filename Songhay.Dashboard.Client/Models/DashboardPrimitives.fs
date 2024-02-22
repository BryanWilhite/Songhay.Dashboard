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

type DashboardMessage =
    | ClearError
    | Error of exn
    | GetFeeds | GotFeeds of (FeedName * SyndicationFeed)[] option
    | SetPage of DashboardPage
    | SetYouTubeFigureId of string
    | SetYouTubeFigureTitle of string
    | YouTubeFigureResolutionChange of string
    | YouTubeMessage of YouTubeMessage
