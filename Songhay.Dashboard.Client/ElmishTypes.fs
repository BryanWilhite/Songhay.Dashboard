namespace Songhay.Dashboard.Client.ElmishTypes

open System
open Bolero
open Bolero.Remoting

open Songhay.Player.YouTube

open Songhay.Modules.Models
open Songhay.Dashboard.Models

type Page =
    | [<EndPoint "/">] StudioToolsPage
    | [<EndPoint "/feeds">] StudioFeedsPage

type Message =
    | ClearError
    | Error of exn
    | GetFeeds | GotFeeds of (FeedName * SyndicationFeed)[] option
    | SetPage of Page
    | YouTubeMessage

type Model =
    {
        error: string option
        feeds: (FeedName * SyndicationFeed)[] option
        page: Page
        playerYt: YouTubeModel option
    }

type DashboardService =
    {
        getAppData: Uri -> Async<(FeedName * SyndicationFeed)[] option>
    }

    interface IRemoteService with
        member this.BasePath = "/api"
