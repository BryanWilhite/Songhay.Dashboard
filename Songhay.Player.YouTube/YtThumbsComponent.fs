module Songhay.Player.YouTube.YtThumbs

open System
open Elmish
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Models

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtUriUtility
open Songhay.Player.YouTube.Visuals.Block.YtThumbs

type YtThumbsComponent() =
    inherit ElmishComponent<YouTubeItem[] option, YouTubeMessage>()

    static member val Id = "yt-thumbs-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View model _ =
        ytThumbsNode this.JSRuntime model

let update (getterAsync: Uri -> Async<YouTubeItem[] option>) (message: YouTubeMessage) (model: YouTubeModel) =
    match message with
    | Error exn -> { model with Error = Some exn.Message }, Cmd.none
    | CallYtItems ->
        let uri = YtIndexSonghayTopTen |> Identifier.Alphanumeric |> getPlaylistUri
        let cmd = Cmd.OfAsync.either getterAsync uri CalledYtItems YouTubeMessage.Error
        { model with YouTubeItems = None }, cmd
    | CalledYtItems items -> { model with YouTubeItems = items }, Cmd.none

let view (model: YouTubeItem[] option) dispatch =
    ecomp<YtThumbsComponent, _, _> [] model dispatch
