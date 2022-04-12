namespace Songhay.Dashboard.Client.ElmishTypes

open System
open Bolero
open Bolero.Remoting

open Songhay.Modules.Publications.Models
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

type Model =
    {
        error: string option
        feeds: (FeedName * SyndicationFeed)[] option
        page: Page
        ytModel: YouTubeModel
    }

type DashboardService =
    {
        getAppData: Uri -> Async<(FeedName * SyndicationFeed)[] option>
        getYtItems: Uri -> Async<YouTubeItem[] option>
        getYtSetIndex: Uri -> Async<(ClientId * Name * (DisplayItemModel * ClientId []) []) option>
        getYtSet: Uri -> Async<(DisplayText * YouTubeItem []) [] option>
    }

    interface IRemoteService with
        member this.BasePath = "/api"
