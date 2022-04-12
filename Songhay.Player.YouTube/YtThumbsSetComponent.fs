module Songhay.Player.YouTube.YtThumbsSet

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Player.YouTube.Visuals.Block.YtThumbsSet

type YtThumbsSetComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    let thumbsSetContainerRef = HtmlRef()

    static member val Id = "yt-thumbs-set-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View model dispatch =
        ytThumbsSetNode this.JSRuntime thumbsSetContainerRef model dispatch

let view (model: YouTubeModel) dispatch =
    ecomp<YtThumbsSetComponent, _, _> [] model dispatch
