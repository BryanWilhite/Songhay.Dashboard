namespace Songhay.Dashboard.Server.RemoteHandlers

open System.Net.Http

open Bolero.Remoting.Server

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.SyndicationFeedUtility
open Songhay.Dashboard.Server.OryxUtility

type DashboardServiceHandler(client: HttpClient) =
    inherit RemoteHandler<DashboardService>()

    override this.Handler =
        {
            getAppData = fun uri -> async {
                let ctx = client |> withHttpContext contextGetter
                let request = requestForJson uri.AbsoluteUri
                let! result = request |> runRequest ctx |> Async.AwaitTask

                return
                    match result with
                    | Ok json -> json |> toJsonElement |> fromInput |> List.toArray |> Option.ofObj
                    | _ -> None
            }
        }
