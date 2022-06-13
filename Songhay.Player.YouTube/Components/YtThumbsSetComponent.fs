namespace Songhay.Player.YouTube.Components

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Player.YouTube
open Songhay.Player.YouTube.Components.YtThumbsSet

type YtThumbsSetComponent() =
    inherit ElmishComponent<YouTubeModel, YouTubeMessage>()

    static member val Id = "yt-thumbs-set-block" with get

    static member EComp (model: YouTubeModel) dispatch =
        ecomp<YtThumbsSetComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View model dispatch =
        model |> ytThumbsSetNode dispatch this.JSRuntime
