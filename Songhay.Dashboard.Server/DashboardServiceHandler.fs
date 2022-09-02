namespace Songhay.Dashboard.Server

open System
open System.Net
open System.Net.Http
open Microsoft.Extensions.Caching.Memory
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Core

open FsToolkit.ErrorHandling

open Bolero.Remoting.Server

open Songhay.Modules.HttpClientUtility
open Songhay.Modules.HttpRequestMessageUtility
open Songhay.Modules.Models
open Songhay.Modules.Bolero.RemoteHandlerUtility

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models

open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Server

type DashboardServiceHandler(client: HttpClient, logger: ILogger<DashboardServiceHandler>, cache: IMemoryCache) =
    inherit RemoteHandler<DashboardService>()

    let upcastLogger = (logger :> ILogger) |> Some 

    member this.tryDownloadToStringAsync (cacheKey: obj) (uri: Uri) =
        task {
                match cache.TryGetValue CalledYtSetIndex with
                | true, o -> return o :?> Result<string, HttpStatusCode>
                | false, _ -> 
                    if upcastLogger.IsSome then upcastLogger.Value.LogInformation($"calling {uri.OriginalString}...")

                    let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

                    let! output =
                        (upcastLogger, responseResult) ||> tryDownloadToStringAsync
                        |> Async.AwaitTask

                    let cacheEntryOptions = MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    cache.Set(cacheKey, output, cacheEntryOptions) |> ignore

                    return output
        }

    override this.Handler =
        {
            getAppData = fun uri -> async {
                match cache.TryGetValue StudioFeedsPage with
                | true, o -> return o :?> (FeedName * SyndicationFeed)[] option
                | false, _ ->
                    logger.LogInformation($"calling {uri.OriginalString}...")

                    let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

                    let dataGetter = ServiceHandlerUtility.toAppData

                    let! output =
                        responseResult
                        |> ((Some (logger :> ILogger), dataGetter) ||> toHandlerOutputAsync)
                        |> Async.AwaitTask

                    let cacheEntryOptions = MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    cache.Set(StudioFeedsPage, output, cacheEntryOptions) |> ignore

                    return output
            }

            getYtItems = fun uri -> async {
                match cache.TryGetValue CalledYtItems with
                | true, o -> return o :?> YouTubeItem[] option
                | false, _ ->
                    logger.LogInformation($"calling {uri.OriginalString}...")

                    let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

                    let dataGetter = ServiceHandlerUtility.toYtItems

                    let! output =
                        responseResult
                        |> ((Some (logger :> ILogger), dataGetter) ||> toHandlerOutputAsync)
                        |> Async.AwaitTask

                    let cacheEntryOptions = MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    cache.Set(CalledYtItems, output, cacheEntryOptions) |> ignore

                    return output
            }

            getYtSetIndex = fun uri -> async {
                return! (CalledYtSetIndex, uri) ||> this.tryDownloadToStringAsync |> Async.AwaitTask
            }

            getYtSet = fun uri -> async {
                let cacheKey = uri |> ServiceHandlerUtility.getYtSetKey (nameof CalledYtSet)
                match cache.TryGetValue cacheKey with
                | true, o -> return o :?> (DisplayText * YouTubeItem []) [] option
                | false, _ ->
                    logger.LogInformation($"calling {uri.OriginalString}...")

                    let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

                    let dataGetter = ServiceHandlerUtility.toYtSet

                    let! output =
                        responseResult
                        |> ((Some (logger :> ILogger), dataGetter) ||> toHandlerOutputAsync)
                        |> Async.AwaitTask

                    let cacheEntryOptions = MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    cache.Set(cacheKey, output, cacheEntryOptions) |> ignore

                    return output
            }
        }
