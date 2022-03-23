namespace Songhay.Dashboard.Client.ElmishTypes

open System
open Bolero
open Bolero.Remoting

open Songhay.Player.YouTube

open Songhay.Modules.Models
open Songhay.Dashboard.Models
open Songhay.Player.YouTube.Models

type Page =
    | [<EndPoint "/">] StudioToolsPage
    | [<EndPoint "/feeds">] StudioFeedsPage

type Message =
    | ClearError
    | Error of exn
    | GetFeeds | GotFeeds of (FeedName * SyndicationFeed)[] option
    | SetPage of Page
    | YouTubeMessage of YouTubeMessage

    member this.ToYouTubeMessage =
        match this with
        | YouTubeMessage m -> Some m
        | _ -> None

type Model =
    {
        error: string option
        feeds: (FeedName * SyndicationFeed)[] option
        page: Page
        ytModel: YouTubeModel option
    }

type DashboardService =
    {
        getAppData: Uri -> Async<(FeedName * SyndicationFeed)[] option>
        getYtItems: Uri -> Async<YouTubeItem[] option>
    }

    interface IRemoteService with
        member this.BasePath = "/api"
