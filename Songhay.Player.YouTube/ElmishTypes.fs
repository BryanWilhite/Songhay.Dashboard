namespace Songhay.Player.YouTube

open Songhay.Modules.Models
open Songhay.Modules.Publications.Models
open Songhay.Player.YouTube.Models

type YouTubeMessage =
    | Error of exn
    | CallYtItems | CalledYtItems of YouTubeItem[] option
    | CallYtIndexAndSet
    | CallYtSet of ClientId
    | CalledYtSet of (DisplayText * YouTubeItem []) [] option
    | CalledYtSetIndex of (ClientId * Name * (DisplayItemModel * ClientId []) []) option
    | SelectYtSet

type YouTubeModel =
    {
        Error: string option
        YtItems: YouTubeItem[] option
        YtSet: (DisplayText * YouTubeItem []) [] option
        YtSetIndex: (ClientId * Name * (DisplayItemModel * ClientId []) []) option
        YtSetIndexSelectedDocument: ClientId
        YtSetIsRequested: bool
        YtSetRequestSelection: bool
    }

    static member initialize =
        {
            Error = None
            YtItems = None
            YtSet = None
            YtSetIndex = None
            YtSetIndexSelectedDocument = "news" |> ClientId.fromString
            YtSetIsRequested = false
            YtSetRequestSelection = false
        }

    static member updateModel (message: YouTubeMessage) (model: YouTubeModel) =
        match message with
        | Error exn -> { model with Error = Some exn.Message }
        | CalledYtItems items -> { model with YtItems = items }
        | CalledYtSet set -> { model with YtSet = set; YtSetIsRequested = false }
        | CalledYtSetIndex index -> { model with YtSetIndex = index; YtSetIsRequested = false }
        | CallYtIndexAndSet -> { model with YtSet = None; YtSetIndex = None; YtSetIsRequested = true }
        | CallYtItems -> { model with YtItems = None }
        | CallYtSet id -> { model with YtSetIndexSelectedDocument = id; YtSetRequestSelection = false }
        | SelectYtSet -> { model with YtSetRequestSelection = true }
