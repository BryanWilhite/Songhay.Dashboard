namespace Songhay.Dashboard.Client.Models

open Bolero

open Songhay.Modules.Models

open Songhay.Player.YouTube.Models

open Songhay.Dashboard.Models

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

/// Routing endpoints definition.
type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/counter">] Counter
    | [<EndPoint "/data">] Data

/// The Elmish application's update messages.
type Message =
    | SetPage of Page
    | Increment
    | Decrement
    | SetCounter of int
    | GetBooks
    | GotBooks of Book[]
    | SetUsername of string
    | SetPassword of string
    | GetSignedInAs
    | RecvSignedInAs of option<string>
    | SendSignIn
    | RecvSignIn of option<string>
    | SendSignOut
    | RecvSignOut
    | Error of exn
    | ClearError
