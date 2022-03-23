namespace Songhay.Player.YouTube

open Songhay.Player.YouTube.Models

type YouTubeModel =
    {
        Error: string option
        YouTubeItems: YouTubeItem[] option
    }

type YouTubeMessage =
    | Error of exn
    | CallYtItems | CalledYtItems of YouTubeItem[] option
