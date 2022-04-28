module Songhay.Player.YouTube.YtThumbs

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Visuals.Block.YtThumbs

type YtThumbsComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    let thumbsContainerRef = HtmlRef()

    let blockWrapperRef = HtmlRef()

    [<Parameter>]
    member val YtThumbsTitle = Unchecked.defaultof<string> with get, set

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.ShouldRender(oldModel, newModel) = oldModel <> newModel

    override this.View model dispatch =
        let title = (this.YtThumbsTitle |> Option.ofObj)
        (title, model) ||> ytThumbsNode dispatch this.JSRuntime thumbsContainerRef blockWrapperRef

let view (attrs: list<Attr>) (model: YouTubeModel) dispatch =
    ecomp<YtThumbsComponent, _, _> attrs model dispatch
