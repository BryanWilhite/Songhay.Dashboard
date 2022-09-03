namespace Songhay.Dashboard.Client.ElmishTypes

open System
open System.Net
open Bolero
open Bolero.Remoting

open Songhay.Modules.Models
open Songhay.Player.YouTube
open Songhay.Dashboard.Models

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
        getAppData: Uri -> Async<Result<string, HttpStatusCode>>
        getYtItems: Uri -> Async<Result<string, HttpStatusCode>>
        getYtSetIndex: Uri -> Async<Result<string, HttpStatusCode>>
        getYtSet: Uri -> Async<Result<string, HttpStatusCode>>
    }

    interface IRemoteService with
        member this.BasePath = "/api"
