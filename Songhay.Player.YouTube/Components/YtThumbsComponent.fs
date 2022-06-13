namespace Songhay.Player.YouTube.Components

open System.Collections.Generic
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Player.YouTube
open Songhay.Player.YouTube.Components.YtThumbs

type YtThumbsComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

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
