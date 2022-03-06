module Songhay.Player.YouTube.YtThumbsSet

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Elmish

open Bolero
open Bolero.Html
open Bolero.Remoting.Client

let initModel =
    {
        Error = None
        YouTubeItems = None
    }

let update message model =
    match message with
    | ClearError -> { model with Error = None }, Cmd.none
    | Error exn -> { model with Error = Some exn.Message }, Cmd.none
    | CallDataStore -> { model with Error = None }, Cmd.none
    | CalledDataStore _ -> { model with Error = None }, Cmd.none

type YtThumbsSetComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    static member val Id = "yt-thumbs-set-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override _.View model dispatch = Empty

let view (model: YouTubeModel) dispatch =
    ecomp<YtThumbsSetComponent, _, _> [] model dispatch
