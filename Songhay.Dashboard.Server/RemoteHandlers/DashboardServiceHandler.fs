namespace Songhay.Dashboard.Server.RemoteHandlers

open System.Net.Http

open Bolero.Remoting.Server

open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Server.OryxUtility

type DashboardServiceHandler(client: HttpClient) =
    inherit RemoteHandler<DashboardService>()

    override this.Handler =
        {
            getAppData = fun location -> async {
                let ctx = client |> toHttpContext
                let request = requestForJson location
                let! result = request |> runRequest ctx |> Async.AwaitTask

                return result
            }
        }
