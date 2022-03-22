namespace Songhay.Dashboard.Server.RemoteHandlers

open System
open System.Net.Http
open Microsoft.Extensions.Caching.Memory
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Core

open FsToolkit.ErrorHandling

open Bolero.Remoting.Server

open Songhay.Modules.HttpClientUtility
open Songhay.Modules.HttpRequestMessageUtility
open Songhay.Modules.Bolero.RemoteHandlerUtility

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtThumbsServiceHandlerUtility

type YtThumbsServiceHandler(client: HttpClient, logger: ILogger<DashboardServiceHandler>, cache: IMemoryCache) =
    inherit RemoteHandler<YtThumbsService>()

    override this.Handler =
        {
            getYtItems = fun uri -> async {
                match cache.TryGetValue CalledDataStore with
                | true, o -> return o :?> YouTubeItem[] option
                | false, _ ->
                    logger.LogInformation($"calling {uri.OriginalString}...")

                    let! responseResult = client |> trySendAsync (get uri) |> Async.AwaitTask

                    let! output =
                        responseResult
                        |> (toHandlerOutputAsync logger toDomainData)
                        |> Async.AwaitTask

                    let cacheEntryOptions = MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    cache.Set(CalledDataStore, output, cacheEntryOptions) |> ignore

                    return output
            }
        }
