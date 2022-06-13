namespace Songhay.Dashboard.Client.Components

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open Bolero
open Bolero.Html

open Songhay.Dashboard.Client.ElmishTypes

type StudioLinksComponent() =
    inherit ElmishComponent<Model, Message>()

    static member EComp model dispatch =
        ecomp<StudioLinksComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View _ _ = Tile.studioLinksNode
