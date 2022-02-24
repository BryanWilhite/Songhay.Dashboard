namespace Songhay.Player.YouTube

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
