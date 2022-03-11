module Songhay.Dashboard.Client.Components.ContentBlock

open System

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Elmish

open Bolero
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.ElmishRoutes
open Songhay.Dashboard.Client.Templates.ContentBlock

let initModel =
    {
        error = None
        feeds = None
        page = StudioToolsPage
        playerYt = None
    }

let update remote (message: Message) (model: Model) =
    match message with
    | Message.ClearError -> { model with error = None }, Cmd.none
    | Message.Error exn -> { model with error = Some exn.Message }, Cmd.none
    | Message.GetFeeds ->
        let uri = App.AppDataLocation |> Uri
        let cmd = Cmd.OfAsync.either remote.getAppData uri GotFeeds Message.Error
        { model with feeds = None }, cmd
    | Message.GotFeeds feeds -> { model with feeds = feeds }, Cmd.none
    | Message.SetPage page ->
        let m = { model with page = page }
        match page with
        | StudioFeedsPage -> m , Cmd.ofMsg GetFeeds
        | _ -> m, Cmd.none
    | YouTubeMessage -> { model with playerYt = model.playerYt }, Cmd.none

let view (jsRuntime: IJSRuntime) (model: Model) dispatch =
    viewContentBlockTemplate jsRuntime model dispatch

type ContentBlockComponent() =
    inherit ProgramComponent<Model, Message>()

    static member val Id = "content-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.Program =
        let init = (fun _ -> initModel, Cmd.none)
        let update = update (this.Remote<DashboardService>())
        let view = view this.JSRuntime

        Program.mkProgram init update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
