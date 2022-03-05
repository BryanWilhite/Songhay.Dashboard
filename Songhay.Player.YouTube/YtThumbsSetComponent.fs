module Songhay.Player.YouTube.YtThumbsSet

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Elmish

open Bolero
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

let view (jsRuntime: IJSRuntime) model dispatch =
    Empty

type YtThumbsSetComponent() =
    inherit ProgramComponent<YouTubeModel, YouTubeMessage>()

    static member val Id = "yt-thumbs-set-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.Program =
        let init = (fun _ -> initModel, Cmd.none)
        let view = view this.JSRuntime

        Program.mkProgram init update view
