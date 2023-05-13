namespace Songhay.Dashboard.Client.Models

open Songhay.Modules.Models
open Songhay.Player.YouTube.Models
open Songhay.Dashboard.Models

type DashboardMessage =
    | ClearError
    | Error of exn
    | GetFeeds | GotFeeds of (FeedName * SyndicationFeed)[] option
    | SetPage of DashboardPage
    | SetYouTubeFigureId of string
    | SetYouTubeFigureTitle of string
    | YouTubeFigureResolutionChange of string
    | YouTubeMessage of YouTubeMessage