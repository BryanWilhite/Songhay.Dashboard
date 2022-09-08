namespace Songhay.Dashboard.Client.Components.Block

open Microsoft.JSInterop
open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.StringUtility

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.Component
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Layout
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.Visuals.Colors
open Songhay.Dashboard.Client.ElmishTypes

module StudioFeeds =

    let studioFeedImage (feedName: FeedName, feed: SyndicationFeed) =
        match feedName with
        | CodePen | Flickr ->
            div {
                cardImage |> toHtmlClass
                figure {
                    imageContainer ThreeByTwo |> toHtmlClassFromList
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

        let mediaLeftClasses =
            [ mediaLeft; m (All, L0); m (R, L1) ]
            @ imageContainer (Square Square48)
            |> toHtmlClassFromList

        figure {
            mediaLeftClasses
            "aria-hidden" => "true"
            svgNode (svgViewBoxSquare 24) svgPathData
        }

    let studioFeedsNode (feedName: FeedName, feed: SyndicationFeed) =
        let listItem (i: SyndicationFeedItem) =
            li {
                a {
                    attr.href i.link; attr.target "_blank"
                    text i.title
                }
            }

        let cardContentNodes = [
            bulmaMedia
                NoCssClasses
                (HasNode (studioFeedIcon feedName))
                [
                    Html.p { [ "title"; fontSize Size4 ] |> toHtmlClassFromList; text feed.feedTitle }
                    Html.p { [ "subtitle"; fontSize Size6 ] |> toHtmlClassFromList; text (feed.modificationDate.ToString("yyyy-MM-dd")) }
                ]
            div {
                content |> toHtmlClass

                ul {
                    forEach (feed.feedItems |> List.take 10) <| listItem
                }
            }
        ]

        let codeNode =
            bulmaCard
                (HasClasses (CssClasses [bulmaBackgroundGreyDarkTone]))
                NoNode
                NoNode
                NoNode
                NoCssClasses
                cardContentNodes

        bulmaTile
            TileSizeAuto
            (HasClasses (CssClasses [ tileIsChild ]))
            [ codeNode ]

    let studioFeedsNodes (_: IJSRuntime) (model: Model) : Node list =
        match model.feeds with
        | None ->
            [
                div {
                    [ tile; tileIsChild; elementTextAlign Center; p (All, L6)] |> toHtmlClassFromList

                    div {
                        [ "loader"; m (All, L6) ] |> toHtmlClassFromList
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
