namespace Songhay.Dashboard.Client.Models

open Bolero.Remoting

type DashboardService =
    {
        getAppData: string -> Async<Result<string, exn>>
    }

    interface IRemoteService with
        member this.BasePath = "/api"
