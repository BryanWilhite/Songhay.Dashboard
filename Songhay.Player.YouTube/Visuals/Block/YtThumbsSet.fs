module Songhay.Player.YouTube.Visuals.Block.YtThumbsSet

open Microsoft.JSInterop

open Bolero
open Bolero.Html
open Elmish

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Visuals.Block.YtThumbsNavigation

let ytThumbsSetNode (jsRuntime: IJSRuntime) (thumbsSetContainerRef: HtmlRef) (model: YouTubeModel) =
    div
        [ attr.classes [ "rx"; "b-roll"; "overlay" ] ; attr.ref thumbsSetContainerRef ]
        [
            bulmaDropdown jsRuntime model
        ]
