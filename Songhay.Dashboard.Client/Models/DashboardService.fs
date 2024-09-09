namespace Songhay.Dashboard.Client.Models

open System
open System.Net
open Bolero.Remoting

type DashboardService =
    {
        getAppData: Uri -> Async<Result<string, HttpStatusCode>>
        getYtItems: Uri -> Async<Result<string, HttpStatusCode>>
        getYtSetIndex: Uri -> Async<Result<string, HttpStatusCode>>
        getYtSet: Uri -> Async<Result<string, HttpStatusCode>>
    }

    interface IRemoteService with
        member this.BasePath = "/api"
