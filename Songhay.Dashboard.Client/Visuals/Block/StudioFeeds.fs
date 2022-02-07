module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Microsoft.JSInterop
open Bolero
open Bolero.Html

open Songhay.Modules.Models

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Models

let studioFeedsNode (feedName: FeedName, feed: SyndicationFeed) =
    div
        [ attr.classes ([ "card" ] @ App.appBlockChildCssClasses) ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "media-content" ] ]
                        [
                            p [ attr.classes [ "title"; "is-4"] ] [ text feed.feedTitle ]
                            p [ attr.classes [ "subtitle"; "is-6"] ] [ text (feed.modificationDate.ToString("yyyy-MM-dd")) ]
                        ]
                    div
                        [ attr.classes [ "content" ] ]
                        [ text "[StudioFeeds]" ]
                ]
        ]

let studioFeedsNodes (jsRuntime: IJSRuntime) (model: Model) : Node list =
    jsRuntime.InvokeVoidAsync("console.log", "yup", model) |> ignore
    match model.feeds with
    | None -> [ div [] [ text "loading…" ] ]
    | Some feeds ->
        feeds
        |> List.ofArray
        |> List.sortByDescending (fun (_, feed) -> feed.modificationDate)
        |> List.map studioFeedsNode