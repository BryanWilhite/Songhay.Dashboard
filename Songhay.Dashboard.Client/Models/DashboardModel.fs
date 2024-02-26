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
                        YouTubeFigure {
                            id =  "pkUK5LEZGa8"
                            title =  "“It took every shred of grace and grit I had” - Bozoma Saint John"
                            resolution = "maxresdefault" 
                        }
                    )
            ytModel = YouTubeModel.initialize httpClient jsRuntime navigationManager
        }

    member private this.getVisualState (getter: DashboardVisualState -> 'o option) =
        this.visualStates.states
        |> List.ofSeq
        |> List.choose getter
        |> List.head

    member this.getYouTubeFigure() = this.getVisualState(function YouTubeFigure ytf -> Some ytf)

    member this.set nextState =
        match nextState with
        | YouTubeFigure _ ->
            let currentData = this.getYouTubeFigure()
            this.visualStates.removeState(YouTubeFigure currentData).addState(nextState)
