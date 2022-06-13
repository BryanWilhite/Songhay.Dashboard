namespace Songhay.Dashboard.Client.Components

open Bolero
open Bolero.Html

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Songhay.Dashboard.Client.ElmishTypes

type StudioComponent() =
    inherit ElmishComponent<Model, Message>()

    static member EComp model dispatch =
        ecomp<StudioComponent, _, _> model dispatch { attr.empty() }

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.View _ _ = Tile.studioComponentNode
