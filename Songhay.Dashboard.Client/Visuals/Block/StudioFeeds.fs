module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Microsoft.JSInterop
open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.StringUtility

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Models
open Songhay.Modules.Bolero.Visuals.Svg

let studioFeedImage (feedName: FeedName, feed: SyndicationFeed) =
    match feedName with
    | CodePen | Flickr ->
        div
            [ attr.classes [ "card-image" ] ]
            [
                figure
                    [ attr.classes [ "image"; "is-3by2" ] ]
                    [
                        img [
                            attr.alt $"{feed.feedTitle} feed image"
                            attr.src (feed.feedImage |> Option.get)
                        ]
                    ]
            ]
    | _ -> empty

let studioFeedIcon (feedName: FeedName) =
    let feedNameMap = Map [
        CodePen,
        Alphanumeric "mdi_codepen_24px"

        Flickr,
        Alphanumeric "mdi_rss_24px"

        GitHub,
        Alphanumeric "mdi_github_circle_24px"

        StackOverflow,
        Alphanumeric "mdi_stack_overflow_24px"

        Studio,
        Alphanumeric "mdi_rss_24px"
    ]

    let svgPathData = svgData[ feedNameMap[ feedName ] ]

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
            (feedName, feed) |> studioFeedImage
            div
                [ attr.classes [ "card-content" ] ]
                [
                    div
                        [ attr.classes [ "media" ] ]
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

let studioFeedsNodes (_: IJSRuntime) (model: Model) : Node list =
    match model.feeds with
    | None ->
        [
            div
                [ attr.classes [ "tile"; "is-child"; "has-text-centered"; "p-6"] ]
                [
                    div [ attr.classes [ "loader"; "m-6" ]; attr.title "Loading…" ] []
                ]
        ]
    | Some feeds ->
        feeds
        |> List.ofArray
        |> List.groupBy (fun (_, feed) -> feed.modificationDate.ToString("yyyy-MM-dd"))
        |> List.sortByDescending fst
        |> List.collect (fun (_, g) -> g |> List.sortBy (fun (_, feed) -> feed.feedTitle |> toBlogSlug))
        |> List.map studioFeedsNode
