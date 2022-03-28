module Songhay.Player.YouTube.YtThumbs

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Visuals.Block.YtThumbs

type YtThumbsComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    static member val Id = "yt-thumbs-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.ShouldRender(oldModel, newModel) =
        this.JSRuntime.InvokeVoidAsync("console.log", $"ShouldRender", oldModel, newModel) |> ignore
        oldModel <> newModel

    override this.View model _ =
        ytThumbsNode this.JSRuntime model.YouTubeItems

let updateModel (message: YouTubeMessage) (model: YouTubeModel) =
    match message with
    | Error exn -> { model with Error = Some exn.Message }
    | CallYtItems -> { model with YouTubeItems = None }
    | CalledYtItems items -> { model with YouTubeItems = items }

let view (model: YouTubeModel) dispatch =
    ecomp<YtThumbsComponent, _, _> [] model dispatch
