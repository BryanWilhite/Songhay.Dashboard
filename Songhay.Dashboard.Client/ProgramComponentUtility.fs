namespace Songhay.Dashboard.Client

open System
open System.Net
open Microsoft.JSInterop
open Elmish

open Songhay.Modules.Models
open Songhay.Modules.Bolero.RemoteHandlerUtility

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtUriUtility

open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client

module ProgramComponentUtility =

    let jsRuntime = Songhay.Modules.Bolero.ServiceProviderUtility.getIJSRuntime()

    let ytSetUri model =
        (
            YtIndexSonghay |> Identifier.Alphanumeric,
            model.ytModel.getSelectedDocumentClientId()
        )
        ||> getPlaylistSetUri

    let ytItemsSuccessMsg (result: Result<string, HttpStatusCode>) =
        let dataGetter = ServiceHandlerUtility.toYtSet
        let set = (dataGetter, result) ||> toHandlerOutput None
        let ytItemsSuccessMsg = YouTubeMessage.CalledYtSet set

        DashboardMessage.YouTubeMessage ytItemsSuccessMsg

    let ytFailureMsg (jsRuntime: IJSRuntime) (ytMsg: YouTubeMessage) ex =
        ((jsRuntime |> Some), ex)
        ||> ytMsg.failureMessage |> DashboardMessage.YouTubeMessage

    let getCommandForGetFeeds model =
        let success (result: Result<string, HttpStatusCode>) =
            let dataGetter = Songhay.Dashboard.ServiceHandlerUtility.toAppData
            let feeds = (dataGetter, result) ||> toHandlerOutput None
            GotFeeds feeds

        let uri = App.AppDataLocation |> Uri

        Cmd.OfAsync.either model.boleroServices.remote.getAppData uri success DashboardMessage.Error

    let getCommandForCallYtItems model message key =
        let success (result: Result<string, HttpStatusCode>) =
            let dataGetter = ServiceHandlerUtility.toYtItems
            let items = (dataGetter, result) ||> toHandlerOutput None
            let msg = YouTubeMessage.CalledYtItems items
            DashboardMessage.YouTubeMessage msg

        let failure = ytFailureMsg jsRuntime message

        let uri = key |> Identifier.Alphanumeric |> getPlaylistUri

        Cmd.OfAsync.either
            model.boleroServices.remote.getYtItems uri
            success
            failure

    let getCommandForCallYtIndexAndSet model message =
        let successMsg (result: Result<string, HttpStatusCode>) =
            let dataGetter = ServiceHandlerUtility.toPublicationIndexData
            let index = (dataGetter, result) ||> toHandlerOutput None
            let msg = YouTubeMessage.CalledYtSetIndex index
            DashboardMessage.YouTubeMessage msg

        let failureMsg = ytFailureMsg jsRuntime message

        let uriIdx = YtIndexSonghay |> Identifier.Alphanumeric |> getPlaylistIndexUri
        let uri = ytSetUri model

        Cmd.batch [
            Cmd.OfAsync.either model.boleroServices.remote.getYtSetIndex uriIdx successMsg failureMsg
            Cmd.OfAsync.either model.boleroServices.remote.getYtSet uri ytItemsSuccessMsg failureMsg
        ]

    let getCommandForCallYtSet model message =
        let failure = ytFailureMsg jsRuntime message
        let uri = ytSetUri model

        Cmd.OfAsync.either model.boleroServices.remote.getYtSet uri ytItemsSuccessMsg failure

    let getCommandForSetPage page =
        match page with
        | StudioFeedsPage -> Cmd.ofMsg GetFeeds
        | _ -> Cmd.none

    let getCommandForYt model message =
        match message with
        | YouTubeMessage.CallYtItems key ->
            getCommandForCallYtItems model message key
        | YouTubeMessage.CallYtIndexAndSet ->
            getCommandForCallYtIndexAndSet model message
        | YouTubeMessage.CallYtSet _ ->
            getCommandForCallYtSet model message
        | YouTubeMessage.OpenYtSetOverlay ->
            if model.ytModel.ytSetIndex.IsNone && model.ytModel.ytSet.IsNone then
                Cmd.ofMsg <| DashboardMessage.YouTubeMessage CallYtIndexAndSet
            else
                Cmd.none
        | _ -> Cmd.none
