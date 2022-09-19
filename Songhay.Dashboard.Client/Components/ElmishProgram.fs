namespace Songhay.Dashboard.Client.Components

open System

open System.Net
open Microsoft.JSInterop
open Elmish

open Bolero
open Bolero.Html
open Bolero.Remoting.Client

open Songhay.Modules.Models
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.RemoteHandlerUtility
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Player.YouTube
open Songhay.Player.YouTube.Components
open Songhay.Player.YouTube.YtUriUtility

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.Components
open Songhay.Dashboard.Client.ElmishTypes

module ElmishProgram =

    type ContentBlockTemplate = Template<"wwwroot/content-block.html">

    let viewContentBlockTemplate (jsRuntime: IJSRuntime) (model: Model) dispatch =
        let studioPageNode nodes =
            bulmaTile
                TileSizeAuto
                (HasClasses (CssClasses [tileIsAncestor]))
                [
                    bulmaTile
                        TileSizeAuto
                        (HasClasses (CssClasses [tileIsParent; tileIsVertical]))
                        nodes
                ]
        ContentBlockTemplate()
            .Studio(StudioComponent.EComp model dispatch)
            .StudioLinks(StudioLinksComponent.EComp model dispatch)
            .Error(
                cond model.error <| function
                | None -> empty()
                | Some err ->
                    ContentBlockTemplate.ErrorNotification()
                        .Text(err)
                        .Hide(fun _ -> dispatch Message.ClearError)
                        .Elt()
            )
            .Content(
                cond model.page <| function
                | StudioFeedsPage -> studioPageNode (Block.StudioFeeds.studioFeedsNodes jsRuntime model)
                | StudioToolsPage -> studioPageNode [ Block.StudioTools.studioToolsNode ]
            )
            .YouTubeThumbs(
                YtThumbsComponent.EComp (Some "songhay tube") model.ytModel (Message.YouTubeMessage >> dispatch)
            )
            .YouTubeThumbsSet(
                YtThumbsSetComponent.EComp model.ytModel (Message.YouTubeMessage >> dispatch)
            )
            .Elt()

    let update remote (jsRuntime: IJSRuntime) (message: Message) (model: Model) =

        match message with
        | Message.ClearError -> { model with error = None }, Cmd.none
        | Message.Error exn -> { model with error = Some exn.Message }, Cmd.none
        | Message.GetFeeds ->
            let success (result: Result<string, HttpStatusCode>) =
                let dataGetter = Songhay.Dashboard.ServiceHandlerUtility.toAppData
                let feeds = (dataGetter, result) ||> toHandlerOutput None
                GotFeeds feeds
            let uri = App.AppDataLocation |> Uri
            let cmd = Cmd.OfAsync.either remote.getAppData uri success Message.Error
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
                Message.YouTubeMessage ytItemsSuccessMsg

            let failure ex = ((jsRuntime |> Some), ex) ||> ytMsg.failureMessage |> Message.YouTubeMessage

            match ytMsg with
            | YouTubeMessage.CallYtItems ->
                let success (result: Result<string, HttpStatusCode>) =
                    let dataGetter = ServiceHandlerUtility.toYtItems
                    let items = (dataGetter, result) ||> toHandlerOutput None
                    let ytItemsSuccessMsg = YouTubeMessage.CalledYtItems items
                    Message.YouTubeMessage ytItemsSuccessMsg
                let uri = YtIndexSonghayTopTen |> Identifier.Alphanumeric |> getPlaylistUri
                let cmd = Cmd.OfAsync.either remote.getYtItems uri success failure
                ytModel, cmd

            | YouTubeMessage.CallYtIndexAndSet ->
                let success (result: Result<string, HttpStatusCode>) =
                    let dataGetter = ServiceHandlerUtility.toPublicationIndexData
                    let index = (dataGetter, result) ||> toHandlerOutput None
                    let ytItemsSuccessMsg = YouTubeMessage.CalledYtSetIndex index
                    Message.YouTubeMessage ytItemsSuccessMsg
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
                    ytModel, Cmd.ofMsg (Message.YouTubeMessage CallYtIndexAndSet)
                else
                    ytModel, Cmd.none

            | _ -> ytModel, Cmd.none

    let view (jsRuntime: IJSRuntime) (model: Model) dispatch =
        viewContentBlockTemplate jsRuntime model dispatch
