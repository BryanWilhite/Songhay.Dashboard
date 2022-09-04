namespace Songhay.Player.YouTube.Components

open System.Collections.Generic
open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Web
open Microsoft.JSInterop

open FsToolkit.ErrorHandling
open Humanizer

open Bolero
open Bolero.Html
open Elmish

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.StringUtility
open Songhay.Modules.Bolero.JsRuntimeUtility
open Songhay.Modules.Bolero.Visuals.Bulma.Block
open Songhay.Modules.Bolero.Visuals.Svg

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Models
open Songhay.Player.YouTube.YtItemUtility

type SlideDirection = | Left | Right

type YtThumbsComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    [<Literal>] // see `$var-thumbs-container-wrapper-left` in `Songhay.Player.YouTube/src/scss/you-tube-css-variables.scss`
    static let CssVarThumbsContainerWrapperLeft = "--thumbs-container-wrapper-left"

    [<Literal>] // see `$thumbnail-margin-right` in `Songhay.Player.YouTube/src/scss/you-tube-thumbs.scss`
    static let ThumbnailMarginRight = 4

    static let click = GlobalEventHandlers.OnClick

    static let getYtThumbsAnchor (item: YouTubeItem) =
        let limit = 60
        let caption =
            if item.snippet.title.Length > limit then
                $"{item.snippet.title.Substring(0, limit)}…"
            else
                item.snippet.title

        a {
            attr.href (item.tryGetUri |> Result.valueOr raise)
            attr.target "_blank"
            attr.title item.snippet.title

            text caption
        }

    static let getYtThumbsTitle (dispatch: Dispatch<YouTubeMessage>) (_: IJSRuntime)
        (itemsTitle: string option) (model: YouTubeModel) =

        let items = model.YtItems

        if items.IsNone then
            rawHtml "&#160;"
        else
            if itemsTitle.IsNone then
                let pair = items.Value |> Array.head |> getYtItemsPair
                a { attr.href (fst pair); attr.target "_blank"; text (snd pair) }
            else
                a {
                    attr.href "#" ; attr.title $"{itemsTitle.Value}: show curated YouTube™ channels"
                    click.PreventDefault
                    on.click (fun _ -> YouTubeMessage.OpenYtSetOverlay |> dispatch)
                    text itemsTitle.Value
                }

    ///<remarks>
    /// this member is needed to ‘jumpstart’ CSS animations
    /// without this member, the first interop with CSS will not function as expected
    ///</remarks>
    static let initAsync (initCache: Dictionary<GlobalEventHandlers, bool>) (blockWrapperRef: HtmlRef) (jsRuntime: IJSRuntime) =
        task {
            if not initCache[OnLoad] then
                let! wrapperLeftStr = jsRuntime |> getComputedStylePropertyValueAsync blockWrapperRef "left"

                jsRuntime
                |> setComputedStylePropertyValueAsync blockWrapperRef CssVarThumbsContainerWrapperLeft wrapperLeftStr
                |> ignore

            initCache[OnLoad] <- true
        }

    static let ytThumbnailsNode (_: IJSRuntime) (blockWrapperRef: HtmlRef) (items: YouTubeItem[] option) =

        let toSpan (item: YouTubeItem) =
            let duration =
                match item.tryGetDuration with
                | Ok ts -> ts.ToString() |> text
                | _ -> text ":00"

            span {
                a
                    {
                        attr.href (item.tryGetUri |> Result.valueOr raise).OriginalString
                        attr.target "_blank"
                        attr.title item.snippet.title

                        img
                            {
                                attr.src item.snippet.thumbnails.medium.url
                                attr.width item.snippet.thumbnails.medium.width
                                attr.height item.snippet.thumbnails.medium.height
                            }
                    }
                span { [ "published-at"; "is-size-6" ] |> toHtmlClassFromList; item.getPublishedAt.Humanize() |> text }
                span { [ "caption"; "has-text-weight-semibold"; "is-size-5" ] |> toHtmlClassFromList; item |> getYtThumbsAnchor }
                span { [ "duration"; "is-size-6" ] |> toHtmlClassFromList ; span { duration } }
            }

        cond items.IsSome <| function
            | true -> div { attr.ref blockWrapperRef; forEach items.Value <| toSpan }
            | false -> (6, 3) ||> bulmaLoader

    static let ytThumbsNode (dispatch: Dispatch<YouTubeMessage>) (jsRuntime: IJSRuntime)
        (initCache: Dictionary<GlobalEventHandlers, bool>) (thumbsContainerRef: HtmlRef) (blockWrapperRef: HtmlRef)
        (itemsTitle: string option) (model: YouTubeModel) =

        let items = model.YtItems
        let slideAsync (direction: SlideDirection) (_: MouseEventArgs) =
            async {
                if items.IsNone then ()
                else
                    jsRuntime |> initAsync initCache blockWrapperRef |> ignore
                    let! wrapperContainerWidthStr =
                        jsRuntime
                        |> getComputedStylePropertyValueAsync thumbsContainerRef "width"
                        |> Async.AwaitTask
                    let! wrapperLeftStr =
                        jsRuntime
                        |> getComputedStylePropertyValueAsync blockWrapperRef "left"
                        |> Async.AwaitTask

                    let wrapperContainerWidth = wrapperContainerWidthStr |> toNumericString |> Option.defaultValue "0" |> int
                    let wrapperLeft = wrapperLeftStr |> toNumericString |> Option.defaultValue "0" |> int

                    let cannotSlideLeft =
                        let itemsHead = items.Value |> Array.head
                        let fixedBlockWidth = itemsHead.snippet.thumbnails.medium.width + ThumbnailMarginRight
                        let totalWidth = fixedBlockWidth * (items.Value |> Array.length)
                        let slideLeftLength = abs(wrapperLeft) + wrapperContainerWidth

                        slideLeftLength >= totalWidth

                    let getSlideRightLength =
                        let l = abs wrapperLeft
                        if l > wrapperContainerWidth then wrapperContainerWidth
                        else l

                    let nextLeft =
                        match direction with
                        | Left ->
                            if cannotSlideLeft then wrapperLeft
                            else wrapperLeft - wrapperContainerWidth
                        | Right ->
                            if wrapperLeft >= 0 then wrapperLeft
                            else wrapperLeft + getSlideRightLength

                    jsRuntime
                    |> setComputedStylePropertyValueAsync blockWrapperRef CssVarThumbsContainerWrapperLeft $"{nextLeft}px"
                    |> ignore
            }

        div {
            [ "rx"; "b-roll" ] |> toHtmlClassFromList

            nav {
                [ "level"; "video"; "thumbs"; "header" ] |> toHtmlClassFromList
                div {
                    "level-left" |> toHtmlClass

                    span {
                        [ "level-item"; "image"; "is-48x48" ] |> toHtmlClassFromList
                        svgNode (svgViewBoxSquare 24) svgData[Keys.MDI_YOUTUBE_24PX.ToAlphanumeric]
                    }
                    span {
                        [ "level-item"; "is-size-2" ] |> toHtmlClassFromList
                        (jsRuntime, itemsTitle, model) |||> getYtThumbsTitle dispatch
                    }
                }
            }
            div {
                [ "video"; "thumbs"; "thumbs-container" ] |> toHtmlClassFromList
                attr.ref thumbsContainerRef

                items |> ytThumbnailsNode jsRuntime blockWrapperRef

                a {
                    attr.href "#"; [ "command"; "left"; "image"; "is-48x48" ] |> toHtmlClassFromList
                    click.PreventDefault
                    on.async.click (slideAsync Right)
                    svgNode (svgViewBoxSquare 24) svgData[Keys.MDI_ARROW_LEFT_DROP_CIRCLE_24PX.ToAlphanumeric]
                }
                a {
                    attr.href "#"; [ "command"; "right"; "image"; "is-48x48" ] |> toHtmlClassFromList
                    click.PreventDefault
                    on.async.click (slideAsync Left)
                    svgNode (svgViewBoxSquare 24) svgData[Keys.MDI_ARROW_RIGHT_DROP_CIRCLE_24PX.ToAlphanumeric]
                }
            }
        }

    let blockWrapperRef = HtmlRef()
    let initCache = Dictionary<GlobalEventHandlers, bool>()
    let thumbsContainerRef = HtmlRef()

    static member EComp (title: string option) (model: YouTubeModel) dispatch =
        ecomp<YtThumbsComponent, _, _> model dispatch {
            if title.IsSome then
                "YtThumbsTitle" => title.Value
            else
                attr.empty()
        }

    [<Parameter>]
    member val YtThumbsTitle = Unchecked.defaultof<string> with get, set

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.ShouldRender(oldModel, newModel) = oldModel.YtItems <> newModel.YtItems

    override this.View model dispatch =
        if not(initCache.ContainsKey(OnLoad)) then initCache.Add(OnLoad, false)
        let title = (this.YtThumbsTitle |> Option.ofObj)
        (title, model)
        ||> ytThumbsNode dispatch this.JSRuntime initCache thumbsContainerRef blockWrapperRef
