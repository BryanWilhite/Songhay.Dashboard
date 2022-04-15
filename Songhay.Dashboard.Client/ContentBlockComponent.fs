module Songhay.Dashboard.Client.Components.ContentBlock

open System

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Elmish

open Bolero
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open Songhay.Modules.Models
open Songhay.Player.YouTube
open Songhay.Player.YouTube.YtUriUtility

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.ElmishRoutes
open Songhay.Dashboard.Client.Templates.ContentBlock

let rec update remote (message: Message) (model: Model) =

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
    | Message.YouTubeMessage ytMsg ->

        let ytModel = {
            model with ytModel = YouTubeModel.updateModel ytMsg model.ytModel
        }

        match ytMsg with
        | YouTubeMessage.CallYtItems ->
            let success = fun items ->
                let ytItemsSuccessMsg = YouTubeMessage.CalledYtItems items
                Message.YouTubeMessage ytItemsSuccessMsg

            let failure = fun ex ->
                let ytFailureMsg = YouTubeMessage.Error ex
                Message.YouTubeMessage ytFailureMsg

            let uri = YtIndexSonghayTopTen |> Identifier.Alphanumeric |> getPlaylistUri
            let cmd = Cmd.OfAsync.either remote.getYtItems uri success failure
            ytModel, cmd
        | YouTubeMessage.CallYtSet ->
            let successIdx = fun index ->
                let ytItemsSuccessMsg = YouTubeMessage.CalledYtSetIndex index
                Message.YouTubeMessage ytItemsSuccessMsg

            let success = fun set ->
                let ytItemsSuccessMsg = YouTubeMessage.CalledYtSet set
                Message.YouTubeMessage ytItemsSuccessMsg

            let failure = fun ex ->
                let ytFailureMsg = YouTubeMessage.Error ex
                Message.YouTubeMessage ytFailureMsg

            let uriIdx = YtIndexSonghay |> Identifier.Alphanumeric |> getPlaylistIndexUri

            let uri =
                (
                    YtIndexSonghay |> Identifier.Alphanumeric,
                    ytModel.ytModel.YtSetIndexSelectedDocument
                ) ||> getPlaylistSetUri

            let cmdBatch = Cmd.batch [
                Cmd.OfAsync.either remote.getYtSetIndex uriIdx successIdx failure
                Cmd.OfAsync.either remote.getYtSet uri success failure
            ]

            ytModel, cmdBatch
        | _ -> ytModel, Cmd.none

let view (jsRuntime: IJSRuntime) (model: Model) dispatch =
    viewContentBlockTemplate jsRuntime model dispatch

type ContentBlockComponent() =
    inherit ProgramComponent<Model, Message>()

    static member val Id = "content-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.Program =
        let initModel =
            {
                error = None
                feeds = None
                page = StudioToolsPage
                ytModel = YouTubeModel.initialize
            }
        let init = (fun _ -> initModel, Cmd.ofMsg (Message.YouTubeMessage YouTubeMessage.CallYtItems))
        let update = update (this.Remote<DashboardService>())
        let view = view this.JSRuntime

        Program.mkProgram init update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
