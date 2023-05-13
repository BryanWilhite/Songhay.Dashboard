namespace Songhay.Dashboard.Client

open System
open System.Net
open Microsoft.JSInterop
open Elmish

open Bolero.Remoting.Client

open Songhay.Modules.Models
open Songhay.Modules.Bolero.RemoteHandlerUtility

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtUriUtility

open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client

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

    let getCommandForSetPage page =
        match page with
        | StudioFeedsPage -> Cmd.ofMsg GetFeeds
        | _ -> Cmd.none

    let getCommandForYt model message =
        match message with
        | YouTubeMessage.CallYtItems ->
            getCommandForCallYtItems model message
        | YouTubeMessage.CallYtIndexAndSet ->
            getCommandForCallYtIndexAndSet model message
        | YouTubeMessage.CallYtSet _ ->
            getCommandForCallYtSet model message
        | YouTubeMessage.OpenYtSetOverlay ->
            if model.ytModel.YtSetIndex.IsNone && model.ytModel.YtSet.IsNone then
                Cmd.ofMsg <| DashboardMessage.YouTubeMessage CallYtIndexAndSet
            else
                Cmd.none
        | _ -> Cmd.none

