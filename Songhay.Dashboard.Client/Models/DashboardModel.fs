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
        ytFigureThumbRes: string
        ytFigureTitle: string
        ytFigureVideoId: string
        ytModel: YouTubeModel
    }

    static member initialize (remote: DashboardService) (httpClient: HttpClient) (jsRuntime: IJSRuntime) (navigationManager: NavigationManager) =
        {
            boleroServices = {| remote = remote; jsRuntime = jsRuntime |}
            error = None
            feeds = None
            page = StudioToolsPage
            ytFigureTitle = "“It took every shred of grace and grit I had” - Bozoma Saint John"
            ytFigureVideoId = "pkUK5LEZGa8"
            ytFigureThumbRes = "maxresdefault"
            ytModel = YouTubeModel.initialize httpClient jsRuntime navigationManager
        }
