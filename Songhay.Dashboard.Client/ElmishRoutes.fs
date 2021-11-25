module Songhay.Dashboard.Client.ElmishRoutes

open Bolero
open Songhay.Dashboard.Client.ElmishTypes

let router = Router.infer SetPage (fun model -> model.page)
