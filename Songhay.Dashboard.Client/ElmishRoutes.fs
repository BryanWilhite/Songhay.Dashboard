namespace Songhay.Dashboard.Client

open Bolero

open Songhay.Dashboard.Client.ElmishTypes

module ElmishRoutes =

    let router = Router.infer SetPage (fun model -> model.page)
