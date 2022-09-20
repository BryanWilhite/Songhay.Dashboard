namespace Songhay.Dashboard.Client.Components.Block

open System
open Microsoft.JSInterop
open Bolero
open Bolero.Html

open Songhay.Modules.Models
open Songhay.Modules.StringUtility

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.BodyElement
open Songhay.Modules.Bolero.Visuals.Bulma.Component
open Songhay.Modules.Bolero.Visuals.Bulma.Element
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Layout

open Songhay.Dashboard.Models
open Songhay.Dashboard.Client.App.Colors
open Songhay.Dashboard.Client.ElmishTypes

module StudioFeeds =

    let studioFeedImage (feedName: FeedName, feed: SyndicationFeed) =
        match feedName with
        | CodePen | Flickr ->
            bulmaCardImageContainer
                (HasClasses (CssClasses (imageContainer ThreeByTwo)))
                (imageElement
                    NoCssClasses
                    NoAttrs
                    $"{feed.feedTitle} feed image"
                    (feed.feedImage |> Option.get |> Uri))
        | _ -> empty()

    let studioFeedIcon (feedName: FeedName) =
        let feedNameCollection =
            [
                CodePen,
                SonghaySvgKeys.MDI_CODEPEN_24PX.ToAlphanumeric

                Flickr,
                SonghaySvgKeys.MDI_RSS_24PX.ToAlphanumeric

                GitHub,
                SonghaySvgKeys.MDI_GITHUB_CIRCLE_24PX.ToAlphanumeric

                StackOverflow,
                SonghaySvgKeys.MDI_STACK_OVERFLOW_24PX.ToAlphanumeric

                Studio,
                SonghaySvgKeys.MDI_RSS_24PX.ToAlphanumeric
            ] |> dict

        let streamGeometry = SonghaySvgData.Get(feedNameCollection[feedName])

        bulmaMediaLeft
            (HasClasses (CssClasses ([ mediaLeft; m (All, L0); m (R, L1) ] @ imageContainer (Square Square48))))
            (HasAttrs AriaHidden.ToAttr)
            ((bulmaIconSvgViewBox Square24, streamGeometry) ||> svgElement)

    let studioFeedsNode (feedName: FeedName, feed: SyndicationFeed) =
        let listItem (i: SyndicationFeedItem) =
            li {
                anchorElement
                    NoCssClasses
                    (i.link |> Uri)
                    TargetBlank
                    NoAttrs
                    (text i.title)
            }

        let cardContentNodes =
            concat {
                bulmaMedia
                    NoCssClasses
                    (HasNode (studioFeedIcon feedName))
                    (concat {
                        paragraphElement
                            (HasClasses (CssClasses (title (HasFontSize Size4))))
                            NoAttrs
                            (text feed.feedTitle)
                        paragraphElement
                            (HasClasses (CssClasses (subtitle (HasFontSize Size6))))
                            NoAttrs
                            (text (feed.modificationDate.ToString("yyyy-MM-dd")))
                    })
                bulmaContent
                    NoCssClasses
                    (ul {
                        forEach (feed.feedItems |> List.take 10) <| listItem
                    })
            }

        let cardNode =
            bulmaCard
                (HasClasses (CssClasses [bulmaBackgroundGreyDarkTone]))
                (HasNode (studioFeedImage (feedName, feed)))
                NoNode
                NoNode
                NoCssClasses
                cardContentNodes

        bulmaTile
            TileSizeAuto
            (HasClasses (CssClasses [ tileIsChild ]))
            cardNode

    let studioFeedsNodes (_: IJSRuntime) (model: Model) : Node =
        match model.feeds with
        | None ->
            div {
                [ tile; tileIsChild; elementTextAlign Center; p (All, L6)] |> CssClasses.toHtmlClassFromList

                bulmaLoader (HasClasses (CssClasses [ m (All, L6) ]))
            }
        | Some feeds ->
            let l =
                feeds
                |> List.ofArray
                |> List.groupBy (fun (_, feed) -> feed.modificationDate.ToString("yyyy-MM-dd"))
                |> List.sortByDescending fst
                |> List.collect (fun (_, g) -> g |> List.sortBy (fun (_, feed) -> feed.feedTitle |> toBlogSlug))

            forEach l <| studioFeedsNode
