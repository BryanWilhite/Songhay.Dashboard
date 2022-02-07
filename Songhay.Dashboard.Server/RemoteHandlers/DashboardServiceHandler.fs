namespace Songhay.Dashboard.Server.RemoteHandlers

open System.Net.Http
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Core

open Bolero.Remoting.Server

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.SyndicationFeedUtility
open Songhay.Modules.HttpClientUtility
open Songhay.Modules.HttpRequestMessageUtility
open Songhay.Modules.HttpResponseMessageUtility

type DashboardServiceHandler(client: HttpClient, logger: ILogger<DashboardServiceHandler>) =
    inherit RemoteHandler<DashboardService>()

    static member getOutputResult response =
        task {
            let! jsonResult = response |> tryDownloadToStringAsync
            return
                match jsonResult with
                | Result.Error _ -> None
                | Result.Ok json ->
                    match json |> tryGetJsonElement with
                    | Result.Error _ -> None
                    | Result.Ok jsonElement ->
                        match jsonElement |> fromInput with
                        | Result.Error _ -> None
                        | Result.Ok input -> input |> List.toArray |> Option.ofObj
        }

    override this.Handler =
        {
            getAppData = fun uri -> async {
                logger.LogInformation($"calling {uri.OriginalString}...")

                let! responseResult =
                    client
                    |> trySendAsync (get uri)
                    |> Async.AwaitTask

                return!
                    match responseResult with
                    | Result.Error err ->
                        logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                        Task.FromResult(None) |> Async.AwaitTask
                    | Result.Ok response -> (DashboardServiceHandler.getOutputResult response) |> Async.AwaitTask 
            }
        }
