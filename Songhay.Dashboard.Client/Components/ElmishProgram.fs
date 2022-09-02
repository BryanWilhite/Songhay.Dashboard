namespace Songhay.Dashboard.Client.Components

open System

open System.Net
open Microsoft.JSInterop
open Elmish

open Bolero
open Bolero.Html
open Bolero.Remoting.Client

open Songhay.Modules.Models
open Songhay.Modules.Bolero.RemoteHandlerUtility
open Songhay.Player.YouTube
open Songhay.Player.YouTube.Components
open Songhay.Player.YouTube.YtUriUtility

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Components
open Songhay.Dashboard.Client.Visuals

module ElmishProgram =

    type ContentBlockTemplate = Template<"wwwroot/content-block.html">

    let viewContentBlockTemplate (jsRuntime: IJSRuntime) (model: Model) dispatch =
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
                | StudioFeedsPage -> Tile.studioPageNode (Block.StudioFeeds.studioFeedsNodes jsRuntime model)
                | StudioToolsPage -> Tile.studioPageNode [ Block.StudioTools.studioToolsNode() ]
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

                    jsRuntime |> Songhay.Modules.Bolero.JsRuntimeUtility.consoleErrorAsync [|
                        $"{nameof YouTubeMessage}.{nameof CallYtItems} failure:", ex
                    |] |> ignore

                    Message.YouTubeMessage ytFailureMsg

                let uri = YtIndexSonghayTopTen |> Identifier.Alphanumeric |> getPlaylistUri
                let cmd = Cmd.OfAsync.either remote.getYtItems uri success failure

                ytModel, cmd

            | YouTubeMessage.CallYtIndexAndSet ->
                let successIdx = fun (result: Result<string, HttpStatusCode>) ->
                    let dataGetter = ServiceHandlerUtility.toPublicationIndexData
                    let index = (dataGetter, result) ||> toHandlerOutput None
                    let ytItemsSuccessMsg = YouTubeMessage.CalledYtSetIndex index
                    Message.YouTubeMessage ytItemsSuccessMsg

                let success = fun set ->
                    let ytItemsSuccessMsg = YouTubeMessage.CalledYtSet set
                    Message.YouTubeMessage ytItemsSuccessMsg

                let failure = fun ex ->
                    let ytFailureMsg = YouTubeMessage.Error ex

                    jsRuntime |> Songhay.Modules.Bolero.JsRuntimeUtility.consoleErrorAsync [|
                        $"{nameof YouTubeMessage}.{nameof CallYtIndexAndSet} failure:", ex
                    |] |> ignore

                    Message.YouTubeMessage ytFailureMsg

                let uriIdx = YtIndexSonghay |> Identifier.Alphanumeric |> getPlaylistIndexUri

                let uri =
                    (
                        YtIndexSonghay |> Identifier.Alphanumeric,
                        snd ytModel.ytModel.YtSetIndexSelectedDocument
                    ) ||> getPlaylistSetUri

                let cmdBatch = Cmd.batch [
                    Cmd.OfAsync.either remote.getYtSetIndex uriIdx successIdx failure
                    Cmd.OfAsync.either remote.getYtSet uri success failure
                ]

                ytModel, cmdBatch

            | YouTubeMessage.CallYtSet _ ->
                let success = fun set ->
                    let ytItemsSuccessMsg = YouTubeMessage.CalledYtSet set
                    Message.YouTubeMessage ytItemsSuccessMsg

                let failure = fun ex ->
                    let ytFailureMsg = YouTubeMessage.Error ex

                    jsRuntime |> Songhay.Modules.Bolero.JsRuntimeUtility.consoleErrorAsync [|
                        $"{nameof YouTubeMessage}.{nameof CallYtSet} failure:", ex
                    |] |> ignore

                    Message.YouTubeMessage ytFailureMsg

                let uri =
                    (
                        YtIndexSonghay |> Identifier.Alphanumeric,
                        snd ytModel.ytModel.YtSetIndexSelectedDocument
                    ) ||> getPlaylistSetUri
                let cmd = Cmd.OfAsync.either remote.getYtSet uri success failure

                ytModel, cmd

            | YouTubeMessage.OpenYtSetOverlay ->
                if ytModel.ytModel.YtSetIndex.IsNone && ytModel.ytModel.YtSet.IsNone then
                    ytModel, Cmd.ofMsg (Message.YouTubeMessage CallYtIndexAndSet)
                else
                    ytModel, Cmd.none

            | _ -> ytModel, Cmd.none

    let view (jsRuntime: IJSRuntime) (model: Model) dispatch =
        viewContentBlockTemplate jsRuntime model dispatch
