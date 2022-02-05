module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Microsoft.JSInterop
open Bolero.Html

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes

let studioFeedsNode (jsRuntime: IJSRuntime) (model: Model) =
    jsRuntime.InvokeVoidAsync("console.log", "yup", model) |> ignore
    div
        [ attr.classes ([ "card" ] @ App.appBlockChildCssClasses) ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "content"; "has-text-centered" ] ]
                        [ text "[StudioFeeds]" ]
                ]
        ]
