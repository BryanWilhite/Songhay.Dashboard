namespace Songhay.Player.YouTube

open Songhay.Modules.Models
open Songhay.Modules.Publications.Models
open Songhay.Player.YouTube.Models

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

type YouTubeMessage =
    | Error of exn
    | CallYtItems | CalledYtItems of YouTubeItem[] option
    | CallYtSetIndex | CalledYtSetIndex of (ClientId * Name * (DisplayItemModel * ClientId []) []) option
    | CallYtSet | CalledYtSet of (DisplayText * YouTubeItem []) [] option
