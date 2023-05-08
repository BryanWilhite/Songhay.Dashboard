namespace Songhay.Dashboard.Client.Components

open System
open System.Net
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Elmish

open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open Songhay.Modules.Models
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.RemoteHandlerUtility

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Components
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtUriUtility

open Songhay.Dashboard.Client.ElmishRoutes
open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Components

type ContentBlockProgramComponent() =
    inherit ProgramComponent<DashboardModel, DashboardMessage>()

    let update remote (jsRuntime: IJSRuntime) (message: DashboardMessage) (model: DashboardModel) =

        match message with
        | DashboardMessage.ClearError -> { model with error = None }, Cmd.none
        | DashboardMessage.CopyToClipboard data ->
            jsRuntime.InvokeVoidAsync("window.navigator.clipboard.writeText", data).AsTask() |> ignore
            model, Cmd.none
        | DashboardMessage.Error exn -> { model with error = Some exn.Message }, Cmd.none
        | DashboardMessage.GetFeeds ->
            let success (result: Result<string, HttpStatusCode>) =
                let dataGetter = Songhay.Dashboard.ServiceHandlerUtility.toAppData
                let feeds = (dataGetter, result) ||> toHandlerOutput None
                GotFeeds feeds
            let uri = App.AppDataLocation |> Uri
            let cmd = Cmd.OfAsync.either remote.getAppData uri success DashboardMessage.Error
            { model with feeds = None }, cmd
        | DashboardMessage.GotFeeds feeds -> { model with feeds = feeds }, Cmd.none
        | DashboardMessage.SetPage page ->
            let m = { model with page = page }
            match page with
            | StudioFeedsPage -> m , Cmd.ofMsg GetFeeds
            | _ -> m, Cmd.none
        | DashboardMessage.SetYouTubeFigureId data -> { model with ytFigureVideoId = data }, Cmd.none
        | DashboardMessage.SetYouTubeFigureTitle data -> { model with ytFigureTitle = data }, Cmd.none
        | DashboardMessage.YouTubeFigureResolutionChange res -> { model with ytFigureThumbRes = res }, Cmd.none
        | DashboardMessage.YouTubeMessage ytMsg ->
            let ytModel = {
                model with ytModel = YouTubeModel.updateModel ytMsg model.ytModel
            }
            let uriYtSet =
                (
                    YtIndexSonghay |> Identifier.Alphanumeric,
                    snd ytModel.ytModel.YtSetIndexSelectedDocument
                )
                ||> getPlaylistSetUri

            let successYtItems (result: Result<string, HttpStatusCode>) =
                let dataGetter = ServiceHandlerUtility.toYtSet
                let set = (dataGetter, result) ||> toHandlerOutput None
                let ytItemsSuccessMsg = YouTubeMessage.CalledYtSet set
                DashboardMessage.YouTubeMessage ytItemsSuccessMsg

            let failure ex = ((jsRuntime |> Some), ex) ||> ytMsg.failureMessage |> DashboardMessage.YouTubeMessage

            match ytMsg with
            | YouTubeMessage.CallYtItems ->
                let success (result: Result<string, HttpStatusCode>) =
                    let dataGetter = ServiceHandlerUtility.toYtItems
                    let items = (dataGetter, result) ||> toHandlerOutput None
                    let ytItemsSuccessMsg = YouTubeMessage.CalledYtItems items
                    DashboardMessage.YouTubeMessage ytItemsSuccessMsg
                let uri = YtIndexSonghayTopTen |> Identifier.Alphanumeric |> getPlaylistUri
                let cmd = Cmd.OfAsync.either remote.getYtItems uri success failure
                ytModel, cmd

            | YouTubeMessage.CallYtIndexAndSet ->
                let success (result: Result<string, HttpStatusCode>) =
                    let dataGetter = ServiceHandlerUtility.toPublicationIndexData
                    let index = (dataGetter, result) ||> toHandlerOutput None
                    let ytItemsSuccessMsg = YouTubeMessage.CalledYtSetIndex index
                    DashboardMessage.YouTubeMessage ytItemsSuccessMsg
                let uriIdx = YtIndexSonghay |> Identifier.Alphanumeric |> getPlaylistIndexUri
                let cmdBatch = Cmd.batch [
                    Cmd.OfAsync.either remote.getYtSetIndex uriIdx success failure
                    Cmd.OfAsync.either remote.getYtSet uriYtSet successYtItems failure
                ]
                ytModel, cmdBatch

            | YouTubeMessage.CallYtSet _ ->
                let cmd = Cmd.OfAsync.either remote.getYtSet uriYtSet successYtItems failure
                ytModel, cmd

            | YouTubeMessage.OpenYtSetOverlay ->
                if ytModel.ytModel.YtSetIndex.IsNone && ytModel.ytModel.YtSet.IsNone then
                    ytModel, Cmd.ofMsg <| DashboardMessage.YouTubeMessage CallYtIndexAndSet
                else
                    ytModel, Cmd.none

            | _ -> ytModel, Cmd.none

    let view (_: IJSRuntime) (model: DashboardModel) dispatch =
        ContentBlockTemplate()
            .StudioLinks(StudioLinksComponent.BComp)
            .Error(
                cond model.error <| function
                | None -> empty()
                | Some err ->
                    ContentBlockTemplate.ErrorNotification()
                        .Text(err)
                        .Hide(fun _ -> dispatch DashboardMessage.ClearError)
                        .Elt()
            )
            .Content(
                cond model.page <| function
                | StudioFeedsPage -> PageComponent.BComp <| StudioFeedsElmishComponent.EComp model dispatch
                | StudioToolsPage -> PageComponent.BComp StudioToolsComponent.BComp
                | YouTubeFigurePage -> PageComponent.BComp <| YouTubeFigureElmishComponent.EComp model dispatch
            )
            .YouTubeThumbs(
                YtThumbsComponent.EComp (Some "songhay tube") model.ytModel (DashboardMessage.YouTubeMessage >> dispatch)
            )
            .YouTubeThumbsSet(
                YtThumbsSetComponent.EComp model.ytModel (DashboardMessage.YouTubeMessage >> dispatch)
            )
            .Elt()

    static member val Id = "content-block" with get

    static member PComp =
        (
            ContentBlockProgramComponent.Id
            ,
            div {
                attr.id ContentBlockProgramComponent.Id

                newLine; indent 2
                comp<ContentBlockProgramComponent>
            }
        ) ||> HtmlDocumentComponent.BComp

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.Program =
        let initModel =
            {
                error = None
                feeds = None
                page = StudioToolsPage
                ytFigureTitle = "“It took every shred of grace and grit I had” - Bozoma Saint John"
                ytFigureVideoId = "pkUK5LEZGa8"
                ytFigureThumbRes = "maxresdefault"
                ytModel = YouTubeModel.initialize
            }
        let init = (fun _ -> initModel, Cmd.ofMsg (DashboardMessage.YouTubeMessage YouTubeMessage.CallYtItems))
        let update = update (this.Remote<DashboardService>()) this.JSRuntime
        let view = view this.JSRuntime

        Program.mkProgram init update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
