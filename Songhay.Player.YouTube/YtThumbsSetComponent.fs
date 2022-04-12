module Songhay.Player.YouTube.YtThumbsSet

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

type YtThumbsSetComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    static member val Id = "yt-thumbs-set-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override _.View model dispatch = Empty

let view (model: YouTubeModel) dispatch =
    ecomp<YtThumbsSetComponent, _, _> [] model dispatch
