module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Microsoft.JSInterop
open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.StringUtility

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client.Visuals.Svg

let studioFeedIcon (feedName: FeedName) =
    let data = Map [
        CodePen, "mdi_codepen_24px"
        Flickr, "mdi_rss_24px"
        GitHub, "mdi_github_circle_24px"
        StackOverflow, "mdi_stack_overflow_24px"
        Studio, "mdi_rss_24px"
    ]

    let svgPathData = svgData[data[feedName]]

    div
        [ attr.classes [ "media-left" ] ]
        [
            figure
                [ attr.classes [ "image"; "is-48x48" ]; "aria-hidden" => "true" ]
                [
                    svgNode (svgViewBoxSquare 24) svgPathData
                ]
        ]

let studioFeedsNode (feedName: FeedName, feed: SyndicationFeed) =
    div
        [ attr.classes ([ "card" ] @ App.appBlockChildCssClasses) ]
        [
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [attr.classes [ "media" ]]
                        [
                            studioFeedIcon feedName
                            div
                                [ attr.classes [ "media-content" ] ]
                                [
                                    p [ attr.classes [ "title"; "is-4"] ] [ text feed.feedTitle ]
                                    p
                                        [ attr.classes [ "subtitle"; "is-6"] ]
                                        [ text (feed.modificationDate.ToString("yyyy-MM-dd")) ]
                                ]
                        ]
                    div
                        [ attr.classes [ "content" ] ]
                        [
                            ul
                                []
                                (
                                    feed.feedItems
                                    |> List.take 10
                                    |> List.map (fun i -> li [] [ a [ attr.href i.link ] [ text i.title ] ])
                                )
                        ]
                ]
        ]

let studioFeedsNodes (jsRuntime: IJSRuntime) (model: Model) : Node list =
    jsRuntime.InvokeVoidAsync("console.log", "yup", model) |> ignore
    match model.feeds with
    | None -> [ div [] [ text "loading…" ] ]
    | Some feeds ->
        feeds
        |> List.ofArray
        |> List.groupBy (fun (_, feed) -> feed.modificationDate.ToString("yyyy-MM-dd"))
        |> List.sortByDescending fst
        |> List.collect (fun (_, g) -> g |> List.sortBy (fun (_, feed) -> feed.feedTitle |> toBlogSlug))
        |> List.map studioFeedsNode