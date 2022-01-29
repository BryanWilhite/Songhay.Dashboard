namespace Songhay.Dashboard.Server.RemoteHandlers

open System.Net.Http

open Bolero.Remoting.Server

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.SyndicationFeedUtility
open Songhay.Modules.HttpClientUtility

type DashboardServiceHandler(client: HttpClient) =
    inherit RemoteHandler<DashboardService>()

    override this.Handler =
        {
            getAppData = fun uri -> async {

                let! response = client |> getAsync uri |> Async.AwaitTask
                let! jsonResult = response |> tryDownloadToStringAsync |> Async.AwaitTask

                return
                    match jsonResult with
                    | Result.Error _ -> None
                    | Result.Ok json ->
                        let inputResult = json |> toJsonElement |> fromInput
                        match inputResult with
                        | Ok input -> input |> List.toArray |> Option.ofObj
                        | _ -> None
            }
        }
