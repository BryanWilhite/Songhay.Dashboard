module Songhay.Player.YouTube.YtThumbs

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Visuals.Block.YtThumbs

type YtThumbsComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    [<Parameter>]
    member val YtThumbsTitle = Unchecked.defaultof<string> with get, set

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.ShouldRender(oldModel, newModel) = oldModel <> newModel

    override this.View model _ =
        ytThumbsNode this.JSRuntime (this.YtThumbsTitle |> Option.ofObj) model.YouTubeItems

let updateModel (message: YouTubeMessage) (model: YouTubeModel) =
    match message with
    | Error exn -> { model with Error = Some exn.Message }
    | CallYtItems -> { model with YouTubeItems = None }
    | CalledYtItems items -> { model with YouTubeItems = items }

let view (attrs: list<Attr>) (model: YouTubeModel) dispatch =
    ecomp<YtThumbsComponent, _, _> attrs model dispatch
