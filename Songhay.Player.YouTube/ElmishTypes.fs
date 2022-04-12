namespace Songhay.Player.YouTube

open Songhay.Modules.Models
open Songhay.Modules.Publications.Models
open Songhay.Player.YouTube.Models

type YouTubeMessage =
    | Error of exn
    | CallYtItems | CalledYtItems of YouTubeItem[] option
    | CallYtSetIndex | CalledYtSetIndex of (ClientId * Name * (DisplayItemModel * ClientId []) []) option
    | CallYtSet | CalledYtSet of (DisplayText * YouTubeItem []) [] option

type YouTubeModel =
    {
        Error: string option
        YtItems: YouTubeItem[] option
        YtSetIndex: (ClientId * Name * (DisplayItemModel * ClientId []) []) option
        YtSetIndexSelectedDocument: ClientId option
        YtSet: (DisplayText * YouTubeItem []) [] option
    }
    static member initialize =
        {
            Error = None
            YtItems = None
            YtSetIndex = None
            YtSetIndexSelectedDocument = None
            YtSet = None
        }

    static member updateModel (message: YouTubeMessage) (model: YouTubeModel) =
        match message with
        | Error exn -> { model with Error = Some exn.Message }
        | CallYtItems -> { model with YtItems = None }
        | CalledYtItems items -> { model with YtItems = items }
        | CallYtSetIndex -> { model with YtSetIndex = None }
        | CalledYtSetIndex index -> { model with YtSetIndex = index }
        | CallYtSet -> { model with YtSet = None }
        | CalledYtSet set -> { model with YtSet = set }
