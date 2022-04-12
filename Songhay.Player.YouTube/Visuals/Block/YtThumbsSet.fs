module Songhay.Player.YouTube.Visuals.Block.YtThumbsSet

open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Visuals.Block.YtThumbsNavigation

let ytThumbsSetNode (jsRuntime: IJSRuntime) (thumbsSetContainerRef: HtmlRef) (model: YouTubeModel) dispatch =
    div
        [ attr.classes [ "rx"; "b-roll" ] ; attr.ref thumbsSetContainerRef ]
        [
            bulmaDropdown jsRuntime model
        ]
