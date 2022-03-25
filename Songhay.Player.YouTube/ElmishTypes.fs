namespace Songhay.Player.YouTube

open Songhay.Player.YouTube.Models
type YouTubeModel =
    {
        Error: string option
        YouTubeItems: YouTubeItem[] option
    }
    static member initialize =
        { Error = None; YouTubeItems = None }

type YouTubeMessage =
    | Error of exn
    | CallYtItems | CalledYtItems of YouTubeItem[] option
