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

open Songhay.Dashboard
open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.ElmishTypes

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
                return! (StudioFeedsPage, uri) ||> this.tryDownloadToStringAsync |> Async.AwaitTask
            }

            getYtItems = fun uri -> async {
                return! (CalledYtItems, uri) ||> this.tryDownloadToStringAsync |> Async.AwaitTask
            }

            getYtSetIndex = fun uri -> async {
                return! (CalledYtSetIndex, uri) ||> this.tryDownloadToStringAsync |> Async.AwaitTask
            }

            getYtSet = fun uri -> async {
                let cacheKey = uri |> ServiceHandlerUtility.getYtSetKey (nameof CalledYtSet)
                return! (cacheKey, uri) ||> this.tryDownloadToStringAsync |> Async.AwaitTask
            }
        }
