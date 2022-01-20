namespace Songhay.Dashboard.Client.Models

open Bolero.Remoting

type DashboardService =
    {
        getAppData: string -> Async<string option>
    }

    interface IRemoteService with
        member this.BasePath = "/api"
