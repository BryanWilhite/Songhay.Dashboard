namespace Songhay.Dashboard.Server.RemoteHandlers

open System
open System.Net
open System.Net.Http
open System.Text.Json
open Microsoft.Extensions.Caching.Memory
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core

open FsToolkit.ErrorHandling

open Bolero.Remoting.Server

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.SyndicationFeedUtility
open Songhay.Modules.HttpClientUtility
open Songhay.Modules.HttpRequestMessageUtility
open Songhay.Modules.HttpResponseMessageUtility
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.Models

type DashboardServiceHandler(client: HttpClient, logger: ILogger<DashboardServiceHandler>, cache: IMemoryCache) =
    inherit RemoteHandler<DashboardService>()

    static member getOutputResultAsync (logger: ILogger) (responseResult: Result<HttpResponseMessage,exn>) =
        task {
            logger.LogInformation("processing result...")

            match responseResult with
            | Result.Error err ->
                logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                return None

            | Result.Ok response ->

                let! jsonResult = response |> tryDownloadToStringAsync

                return
                    jsonResult
                    |> Result.mapError
                           (
                                fun code ->
                                    logger.LogError($"The expected {nameof HttpStatusCode}, `{code},` is not here.")
                                    exn (code.ToString())
                           )
                    |> Result.bind (fun json -> json |> tryGetRootElement)
                    |> Result.mapError
                           (
                               fun err ->
                                   logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                                   JsonException err.Message
                            )
                    |> Result.bind (fun el -> el |> fromInput)
                    |> Result.map (fun input -> input |> List.toArray)
                    |> Option.ofResult
        }

    override this.Handler =
        {
            getAppData = fun uri -> async {
                match cache.TryGetValue StudioFeedsPage with
                | true, o -> return o :?> (FeedName * SyndicationFeed)[] option
                | false, _ ->
                    logger.LogInformation($"calling {uri.OriginalString}...")

                    let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

                    let! output =
                        responseResult
                        |> (DashboardServiceHandler.getOutputResultAsync logger)
                        |> Async.AwaitTask

                    let cacheEntryOptions = MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    cache.Set(StudioFeedsPage, output, cacheEntryOptions) |> ignore

                    return output
            }
        }
