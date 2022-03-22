namespace Songhay.Player.YouTube

open System
open Bolero.Remoting

open Songhay.Player.YouTube.Models

type YouTubeModel =
    {
        Error: string option
        YouTubeItems: YouTubeItem[] option
    }

type YouTubeMessage =
    | ClearError
    | Error of exn
    | CallDataStore | CalledDataStore of YouTubeItem[] option

type YtThumbsService =
    {
        getYtItems: Uri -> Async<YouTubeItem[] option>
    }

    interface IRemoteService with
        member this.BasePath = "/api/yt"
