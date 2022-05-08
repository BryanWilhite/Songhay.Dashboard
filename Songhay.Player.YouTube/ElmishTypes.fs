namespace Songhay.Player.YouTube

open Songhay.Modules.Models
open Songhay.Modules.Publications.Models
open Songhay.Player.YouTube.Models

type YouTubeMessage =
    | Error of exn
    | CallYtItems | CalledYtItems of YouTubeItem[] option
    | CallYtIndexAndSet
    | CallYtSet of DisplayText * ClientId
    | CalledYtSet of (DisplayText * YouTubeItem []) [] option
    | CalledYtSetIndex of (ClientId * Name * (DisplayItemModel * ClientId []) []) option
    | CloseYtSetOverlay
    | OpenYtSetOverlay
    | SelectYtSet

type YouTubeModel =
    {
        Error: string option
        YtItems: YouTubeItem[] option
        YtSet: (DisplayText * YouTubeItem []) [] option
        YtSetIndex: (ClientId * Name * (DisplayItemModel * ClientId []) []) option
        YtSetIndexSelectedDocument: DisplayText * ClientId
        YtSetOverlayIsVisible: bool option
        YtSetIsRequested: bool
        YtSetRequestSelection: bool
    }

    static member initialize =
        {
            Error = None
            YtItems = None
            YtSet = None
            YtSetIndex = None
            YtSetIndexSelectedDocument = (DisplayText "News", "news" |> ClientId.fromString)
            YtSetOverlayIsVisible = None
            YtSetIsRequested = false
            YtSetRequestSelection = false
        }

    static member updateModel (message: YouTubeMessage) (model: YouTubeModel) =
        match message with
        | Error exn -> { model with Error = Some exn.Message }
        | CalledYtItems items -> { model with YtItems = items }
        | CalledYtSet set -> { model with YtSet = set; YtSetIsRequested = false }
        | CalledYtSetIndex index -> { model with YtSetIndex = index; YtSetIsRequested = false }
        | CallYtIndexAndSet -> { model with YtSet = None; YtSetIndex = None; YtSetOverlayIsVisible = Some true; YtSetIsRequested = true }
        | CallYtItems -> { model with YtItems = None }
        | CallYtSet (displayText, id) -> { model with YtSet = None; YtSetIndexSelectedDocument = (displayText, id); YtSetRequestSelection = false }
        | CloseYtSetOverlay -> { model with YtSetOverlayIsVisible = Some false }
        | OpenYtSetOverlay -> { model with YtSetOverlayIsVisible = Some true }
        | SelectYtSet -> { model with YtSetRequestSelection = true }
