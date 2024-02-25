namespace Songhay.Dashboard.Client.Models

open System.Net.Http
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Songhay.Modules.Models
open Songhay.Player.YouTube.Models
open Songhay.Dashboard.Models

type DashboardModel =
    {
        boleroServices: {| remote: DashboardService; jsRuntime: IJSRuntime |}
        error: string option
        feeds: (FeedName * SyndicationFeed)[] option
        page: DashboardPage
        visualStates: AppStateSet<DashboardVisualState>
        ytModel: YouTubeModel
    }

    static member initialize (remote: DashboardService) (httpClient: HttpClient) (jsRuntime: IJSRuntime) (navigationManager: NavigationManager) =
        {
            boleroServices = {| remote = remote; jsRuntime = jsRuntime |}
            error = None
            feeds = None
            page = StudioToolsPage
            visualStates = AppStateSet<DashboardVisualState>.initialize
                .addStates(
                        YouTubeFigureTitle "“It took every shred of grace and grit I had” - Bozoma Saint John",
                        YouTubeFigureId "pkUK5LEZGa8",
                        YouTubeFigureResolution "maxresdefault"
                    )
            ytModel = YouTubeModel.initialize httpClient jsRuntime navigationManager
        }

    member private this.getVisualState (getter: DashboardVisualState -> 'o option) =
        this.visualStates.states
        |> List.ofSeq
        |> List.choose getter
        |> List.head

    member this.getYouTubeFigureTitle() = this.getVisualState(function YouTubeFigureTitle s -> Some s | _ -> None)

    member this.getYouTubeFigureId() = this.getVisualState(function YouTubeFigureId s -> Some s | _ -> None)

    member this.getYouTubeFigureResolution() = this.getVisualState(function YouTubeFigureResolution s -> Some s | _ -> None)

    member this.set nextState =
        match nextState with
        | YouTubeFigureTitle _ ->
            let currentData = this.getYouTubeFigureTitle()
            this.visualStates.removeState(YouTubeFigureTitle currentData).addState(nextState)
        | YouTubeFigureId _ ->
            let currentData = this.getYouTubeFigureId()
            this.visualStates.removeState(YouTubeFigureId currentData).addState(nextState)
        | YouTubeFigureResolution _ ->
            let currentData = this.getYouTubeFigureResolution()
            this.visualStates.removeState(YouTubeFigureResolution currentData).addState(nextState)
