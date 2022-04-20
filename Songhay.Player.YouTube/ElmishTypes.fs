namespace Songhay.Player.YouTube

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Models
open Songhay.Modules.Publications.Models
open Songhay.Player.YouTube.Models

type YouTubeMessage =
    | Error of exn
    | CallYtItems | CalledYtItems of YouTubeItem[] option
    | CallYtSet
    | CalledYtSet of (DisplayText * YouTubeItem []) [] option
    | CalledYtSetIndex of (ClientId * Name * (DisplayItemModel * ClientId []) []) option
    | CssClass of (EditCommand * Identifier)

type YouTubeModel =
    {
        Error: string option
        YtCssClass: (EditCommand * Identifier) option
        YtItems: YouTubeItem[] option
        YtSet: (DisplayText * YouTubeItem []) [] option
        YtSetIndex: (ClientId * Name * (DisplayItemModel * ClientId []) []) option
        YtSetIndexSelectedDocument: ClientId
        YtSetIsRequested: bool
    }

    static member initialize =
        {
            Error = None
            YtCssClass = None
            YtItems = None
            YtSet = None
            YtSetIndex = None
            YtSetIndexSelectedDocument = "news" |> ClientId.fromString
            YtSetIsRequested = false
        }

    static member updateModel (message: YouTubeMessage) (model: YouTubeModel) =
        match message with
        | Error exn -> { model with Error = Some exn.Message }
        | CallYtItems -> { model with YtItems = None }
        | CalledYtItems items -> { model with YtItems = items }
        | CalledYtSetIndex index -> { model with YtSetIndex = index; YtSetIsRequested = false }
        | CallYtSet -> { model with YtSet = None; YtSetIndex = None; YtSetIsRequested = true }
        | CalledYtSet set -> { model with YtSet = set; YtSetIsRequested = false }
        | CssClass t -> { model with YtCssClass = Some t }
