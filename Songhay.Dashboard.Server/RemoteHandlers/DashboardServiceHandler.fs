namespace Songhay.Dashboard.Server.RemoteHandlers

open System.Net.Http
open System.Threading.Tasks
open Microsoft.FSharp.Core

open Bolero.Remoting.Server

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.SyndicationFeedUtility
open Songhay.Modules.HttpClientUtility

type DashboardServiceHandler(client: HttpClient) =
    inherit RemoteHandler<DashboardService>()

    static member getOutputResult response =
        task {
            let! jsonResult = response |> tryDownloadToStringAsync
            return
                match jsonResult with
                | Result.Error _ -> None
                | Result.Ok json ->
                    let outputResult = json |> toJsonElement |> fromInput
                    match outputResult with
                    | Ok input -> input |> List.toArray |> Option.ofObj
                    | _ -> None
        }

    override this.Handler =
        {
            getAppData = fun uri -> async {

                let! responseResult =
                    client
                    |> trySendAsync (get uri)
                    |> Async.AwaitTask

                return!
                    match responseResult with
                    | Result.Error _ -> Task.FromResult(None) |> Async.AwaitTask
                    | Result.Ok response -> (DashboardServiceHandler.getOutputResult response) |> Async.AwaitTask 
            }
        }
