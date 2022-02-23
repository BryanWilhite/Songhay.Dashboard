namespace Songhay.Dashboard.Server.RemoteHandlers

open System
open System.Net.Http
open System.Threading.Tasks
open Microsoft.Extensions.Caching.Memory
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Core

open Bolero.Remoting.Server

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.SyndicationFeedUtility
open Songhay.Modules.HttpClientUtility
open Songhay.Modules.HttpRequestMessageUtility
open Songhay.Modules.HttpResponseMessageUtility
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.Models

type DashboardServiceHandler(client: HttpClient,
                             logger: ILogger<DashboardServiceHandler>,
                             cache: IMemoryCache) =
    inherit RemoteHandler<DashboardService>()

    static member getOutputResult response =
        task {
            let! jsonResult = response |> tryDownloadToStringAsync
            return
                match jsonResult with
                | Result.Error _ -> None
                | Result.Ok json ->
                    match json |> tryGetRootElement with
                    | Result.Error _ -> None
                    | Result.Ok jsonElement ->
                        match jsonElement |> fromInput with
                        | Result.Error _ -> None
                        | Result.Ok input -> input |> List.toArray |> Option.ofObj
        }
    override this.Handler =
        {
            getAppData = fun uri -> async {
                match cache.TryGetValue StudioFeedsPage with
                | true, o ->
                    return o :?> (FeedName * SyndicationFeed)[] option
                | false, _ ->
                    logger.LogInformation($"calling {uri.OriginalString}...")

                    let! responseResult =
                        client
                        |> trySendAsync (get uri)
                        |> Async.AwaitTask

                    let! output =
                        match responseResult with
                        | Result.Error err ->
                            logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                            Task.FromResult(None) |> Async.AwaitTask
                        | Result.Ok response -> (DashboardServiceHandler.getOutputResult response) |> Async.AwaitTask

                    let cacheEntryOptions = MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    cache.Set(StudioFeedsPage, output, cacheEntryOptions) |> ignore

                    return output
            }
        }
