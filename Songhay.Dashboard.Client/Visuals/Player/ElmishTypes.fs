namespace Songhay.Player

open Songhay.Player.Models

type Model =
    {
        Error: string option
        YouTubeItems: YouTubeItem[] option
    }

type Message =
    | ClearError
    | Error of exn
    | CallDataStore | CalledDataStore of YouTubeItem[] option
