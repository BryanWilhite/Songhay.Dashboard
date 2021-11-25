module Songhay.Dashboard.Client.Routing

open Bolero
open Songhay.Dashboard.Client.BoleroTypes

let router = Router.infer SetPage (fun model -> model.page)
