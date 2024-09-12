namespace Songhay.Dashboard.Client.Components

open Microsoft.AspNetCore.Components

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models
open Songhay.Modules.Bolero.Visuals.Bulma.CssClass
open Songhay.Modules.Bolero.Visuals.Bulma.Layout

type PageComponent() =
    inherit Component()

    static member BComp (pageNode: Node) =
        comp<PageComponent> {
            "PageNode" => pageNode
        }

    [<Parameter>]
    member val PageNode = empty() with get, set

    override this.Render() =
        bulmaTile
            HSizeAuto
            (HasClasses <| CssClasses [tileIsAncestor])
            (bulmaTile
                HSizeAuto
                (HasClasses <| CssClasses [tileIsParent; tileIsVertical])
                this.PageNode)
