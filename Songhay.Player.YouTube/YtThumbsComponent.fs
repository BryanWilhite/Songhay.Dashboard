module Songhay.Player.YouTube.YtThumbs

open Elmish
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.Visuals.Block.YtThumbs

type YtThumbsComponent() =
    inherit ElmishComponent<YouTubeItem[] option, YouTubeMessage>()

    static member val Id = "yt-thumbs-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View model _ =
        ytThumbsNode this.JSRuntime model

let update (jsRuntime: IJSRuntime) (message: YouTubeMessage) (model: YouTubeModel) =

    jsRuntime.InvokeVoidAsync("console.log", nameof message, message) |> ignore

    match message with
    | Error exn -> { model with Error = Some exn.Message }, Cmd.none
    | CallYtItems -> { model with YouTubeItems = None }, Cmd.none
    | CalledYtItems items -> { model with YouTubeItems = items }, Cmd.none

let view (model: YouTubeItem[] option) dispatch =
    ecomp<YtThumbsComponent, _, _> [] model dispatch
