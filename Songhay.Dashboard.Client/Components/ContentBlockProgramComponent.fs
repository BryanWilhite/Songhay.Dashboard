namespace Songhay.Dashboard.Client.Components

open System
open Microsoft.AspNetCore.Components
open Elmish

open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Templating.Client

open Songhay.Modules.Bolero.BoleroUtility

open Songhay.Player.YouTube.YtUriUtility
open Songhay.Player.YouTube.Components
open Songhay.Player.YouTube.Models

open Songhay.Dashboard.Client.ElmishRoutes
open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Components

module pcu = ProgramComponentUtility

type ContentBlockProgramComponent() =
    inherit ProgramComponent<DashboardModel, DashboardMessage>()

    let update message (model: DashboardModel) =
        match message with
        | ClearError ->
            let m = { model with error = None }
            m, Cmd.none
        | Error exn ->
            let m = { model with error = Some exn.Message }
            m, Cmd.none
        | GetFeeds ->
            let m = { model with feeds = None }
            let cmd = pcu.getCommandForGetFeeds model
            m, cmd
        | GotFeeds feeds ->
            let m = { model with feeds = feeds }
            m, Cmd.none
        | SetPage page ->
            let m = { model with page = page }
            let cmd = pcu.getCommandForSetPage page
            m, cmd
        | ChangeVisualState state ->
            let m = { model with visualStates = model.set state }
            m, Cmd.none
        | DashboardMessage.YouTubeMessage ytMessage ->
            let m = { model with ytModel = YouTubeModel.updateModel ytMessage model.ytModel }
            let cmd = pcu.getCommandForYt m ytMessage
            m, cmd

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
                YtThumbsContainerElmishComponent.EComp (Some "songhay tube") model.ytModel (DashboardMessage.YouTubeMessage >> dispatch)
            )
            .YouTubeThumbsSet(
                YtThumbsSetElmishComponent.EComp model.ytModel (DashboardMessage.YouTubeMessage >> dispatch)
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
    member val ServiceProvider = Unchecked.defaultof<IServiceProvider> with get, set

    override this.CssScope = nameof(ContentBlockProgramComponent)

    override this.Program =
        let m = DashboardModel.initialize (this.Remote<DashboardService>()) this.ServiceProvider
        let cmd = Cmd.ofMsg (DashboardMessage.YouTubeMessage <| YouTubeMessage.CallYtItems YtIndexSonghayTopTen)

        Program.mkProgram (fun _ -> m, cmd) update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif

    override this.ShouldRender(oldModel, newModel) =
        oldModel.visualStates <> newModel.visualStates
        || oldModel.error <> newModel.error
        || oldModel.feeds <> newModel.feeds
        || oldModel.page <> newModel.page
        || oldModel.ytModel <> newModel.ytModel
