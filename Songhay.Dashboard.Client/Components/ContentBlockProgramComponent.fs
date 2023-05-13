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

module ProgramComponentUtility =

    let uriYtSet model =
        (
            YtIndexSonghay |> Identifier.Alphanumeric,
            snd model.ytModel.YtSetIndexSelectedDocument
        )
        ||> getPlaylistSetUri

    let successYtItems (result: Result<string, HttpStatusCode>) =
        let dataGetter = ServiceHandlerUtility.toYtSet
        let set = (dataGetter, result) ||> toHandlerOutput None
        let ytItemsSuccessMsg = YouTubeMessage.CalledYtSet set

        DashboardMessage.YouTubeMessage ytItemsSuccessMsg

    let failure (jsRuntime: IJSRuntime) (ytMsg: YouTubeMessage) ex =
        ((jsRuntime |> Some), ex)
        ||> ytMsg.failureMessage |> DashboardMessage.YouTubeMessage

    let getCommandForGetFeeds model =
        let success (result: Result<string, HttpStatusCode>) =
            let dataGetter = Songhay.Dashboard.ServiceHandlerUtility.toAppData
            let feeds = (dataGetter, result) ||> toHandlerOutput None
            GotFeeds feeds

        let uri = App.AppDataLocation |> Uri

        Cmd.OfAsync.either model.boleroServices.remote.getAppData uri success DashboardMessage.Error

    let getCommandForCallYtItems model message =
        let success (result: Result<string, HttpStatusCode>) =
            let dataGetter = ServiceHandlerUtility.toYtItems
            let items = (dataGetter, result) ||> toHandlerOutput None
            let ytItemsSuccessMsg = YouTubeMessage.CalledYtItems items
            DashboardMessage.YouTubeMessage ytItemsSuccessMsg

        let failure = failure model.boleroServices.jsRuntime message

        let uri = YtIndexSonghayTopTen |> Identifier.Alphanumeric |> getPlaylistUri

        Cmd.OfAsync.either
            model.boleroServices.remote.getYtItems uri
            success
            failure

    let getCommandForCallYtIndexAndSet model message =
        let success (result: Result<string, HttpStatusCode>) =
            let dataGetter = ServiceHandlerUtility.toPublicationIndexData
            let index = (dataGetter, result) ||> toHandlerOutput None
            let ytItemsSuccessMsg = YouTubeMessage.CalledYtSetIndex index
            DashboardMessage.YouTubeMessage ytItemsSuccessMsg

        let failure = failure model.boleroServices.jsRuntime message

        let uriIdx = YtIndexSonghay |> Identifier.Alphanumeric |> getPlaylistIndexUri

        Cmd.batch [
            Cmd.OfAsync.either model.boleroServices.remote.getYtSetIndex uriIdx success failure
            Cmd.OfAsync.either model.boleroServices.remote.getYtSet (uriYtSet model) successYtItems failure
        ]

    let getCommandForCallYtSet model message =
        let failure = failure model.boleroServices.jsRuntime message

        Cmd.OfAsync.either model.boleroServices.remote.getYtSet (uriYtSet model) successYtItems failure

module pcu = ProgramComponentUtility

type ContentBlockProgramComponent() =
    inherit ProgramComponent<DashboardModel, DashboardMessage>()

    let update message model =

        match message with
        | DashboardMessage.ClearError ->
            let m = { model with error = None }
            m, Cmd.none
        | DashboardMessage.CopyToClipboard data ->
            model.boleroServices.jsRuntime.InvokeVoidAsync("window.navigator.clipboard.writeText", data).AsTask() |> ignore
            model, Cmd.none
        | DashboardMessage.Error exn ->
            let m = { model with error = Some exn.Message }
            m, Cmd.none
        | DashboardMessage.GetFeeds ->
            let m = { model with feeds = None }
            let cmd = pcu.getCommandForGetFeeds model
            m, cmd
        | DashboardMessage.GotFeeds feeds ->
            let m = { model with feeds = feeds }
            m, Cmd.none
        | DashboardMessage.SetPage page ->
            let m = { model with page = page }
            match page with
            | StudioFeedsPage -> m , Cmd.ofMsg GetFeeds
            | _ -> m, Cmd.none
        | DashboardMessage.SetYouTubeFigureId data ->
            let m = { model with ytFigureVideoId = data }
            m, Cmd.none
        | DashboardMessage.SetYouTubeFigureTitle data ->
            let m = { model with ytFigureTitle = data }
            m, Cmd.none
        | DashboardMessage.YouTubeFigureResolutionChange res ->
            let m = { model with ytFigureThumbRes = res }
            m, Cmd.none
        | DashboardMessage.YouTubeMessage ytMsg ->
            let model = { model with ytModel = YouTubeModel.updateModel ytMsg model.ytModel }

            match ytMsg with
            | YouTubeMessage.CallYtItems ->
                let cmd =
                    pcu.getCommandForCallYtItems model ytMsg
                model, cmd
            | YouTubeMessage.CallYtIndexAndSet ->
                let cmdBatch = pcu.getCommandForCallYtIndexAndSet model ytMsg
                model, cmdBatch
            | YouTubeMessage.CallYtSet _ ->
                let cmd = pcu.getCommandForCallYtSet model ytMsg
                model, cmd
            | YouTubeMessage.OpenYtSetOverlay ->
                if model.ytModel.YtSetIndex.IsNone && model.ytModel.YtSet.IsNone then
                    model, Cmd.ofMsg <| DashboardMessage.YouTubeMessage CallYtIndexAndSet
                else
                    model, Cmd.none
            | _ -> model, Cmd.none

    let view model dispatch =
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
        let m = DashboardModel.initialize (this.Remote<DashboardService>()) this.JSRuntime
        let cmd = Cmd.ofMsg (YouTubeMessage.CallYtItems |> DashboardMessage.YouTubeMessage)

        Program.mkProgram (fun _ -> m, cmd) update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
