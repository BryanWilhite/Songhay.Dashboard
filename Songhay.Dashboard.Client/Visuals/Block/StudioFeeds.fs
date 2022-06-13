module Songhay.Dashboard.Client.Visuals.Block.StudioFeeds

open Microsoft.JSInterop
open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.StringUtility

open Songhay.Modules.Bolero.BoleroUtility

open Songhay.Dashboard.Client
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Models
open Songhay.Modules.Bolero.Visuals.Svg

let studioFeedImage (feedName: FeedName, feed: SyndicationFeed) =
    match feedName with
    | CodePen | Flickr ->
        div {
            "card-image" |> toHtmlClass
            figure {
                [ "image"; "is-3by2" ] |> toHtmlClassFromList
                img {
                    attr.alt $"{feed.feedTitle} feed image"
                    attr.src (feed.feedImage |> Option.get)
                }
            }
        }
    | _ -> empty()

let studioFeedIcon (feedName: FeedName) =
    let feedNameMap = Map [
        CodePen,
        Keys.MDI_CODEPEN_24PX.ToAlphanumeric

        Flickr,
        Keys.MDI_RSS_24PX.ToAlphanumeric

        GitHub,
        Keys.MDI_GITHUB_CIRCLE_24PX.ToAlphanumeric

        StackOverflow,
        Keys.MDI_STACK_OVERFLOW_24PX.ToAlphanumeric

        Studio,
        Keys.MDI_RSS_24PX.ToAlphanumeric
    ]

    let svgPathData = svgData[ feedNameMap[ feedName ] ]

    div {
        "media-left" |> toHtmlClass
        figure {
            [ "image"; "is-48x48" ] |> toHtmlClassFromList; "aria-hidden" => "true"
            svgNode (svgViewBoxSquare 24) svgPathData
        }
    }

let studioFeedsNode (feedName: FeedName, feed: SyndicationFeed) =
    let listItem (i: SyndicationFeedItem) =
        li {
            a {
                attr.href i.link; attr.target "_blank"
                text i.title
            }
        }

    div {
        "card" |> App.appBlockChildCssClasses.Prepend |> toHtmlClassFromData
        (feedName, feed) |> studioFeedImage

        div {
            "card-content" |> toHtmlClass

            div {
                "media" |> toHtmlClass

                studioFeedIcon feedName

                div {
                    "media-content" |> toHtmlClass

                    p { [ "title"; "is-4"] |> toHtmlClassFromList; text feed.feedTitle }
                    p { [ "subtitle"; "is-6"] |> toHtmlClassFromList; text (feed.modificationDate.ToString("yyyy-MM-dd")) }
                }
                div {
                    "content" |> toHtmlClass

                    ul {
                        forEach (feed.feedItems |> List.take 10) <| listItem
                    }
                }
            }
        }
    }

let studioFeedsNodes (_: IJSRuntime) (model: Model) : Node list =
    match model.feeds with
    | None ->
        [
            div {
                [ "tile"; "is-child"; "has-text-centered"; "p-6"] |> toHtmlClassFromList

                div {
                    [ "loader"; "m-6" ] |> toHtmlClassFromList
                    attr.title "Loading…"
                }
            }
        ]
    | Some feeds ->
        let l =
            feeds
            |> List.ofArray
            |> List.groupBy (fun (_, feed) -> feed.modificationDate.ToString("yyyy-MM-dd"))
            |> List.sortByDescending fst
            |> List.collect (fun (_, g) -> g |> List.sortBy (fun (_, feed) -> feed.feedTitle |> toBlogSlug))

        [forEach l <| studioFeedsNode]
