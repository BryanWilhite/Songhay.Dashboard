namespace Songhay.Dashboard.Server.RemoteHandlers

open Bolero.Remoting.Server
open Songhay.Dashboard.Client.Models

type DashboardServiceHandler() =
    inherit RemoteHandler<DashboardService>()
    override this.Handler =
        {
            getAppData = fun location -> async {
                return Some location
            }
        }
