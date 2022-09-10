namespace Songhay.Modules.Bolero.Visuals

open System
open Microsoft.AspNetCore.Components

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models

module Element =

    let anchorElement (moreClasses: CssClassesOrEmpty) (href: Uri) (target: HtmlTargetOrEmpty) (childNodes: Node list) =
        a {
            moreClasses.Value

            attr.href href.OriginalString
            target.Value

            forEach childNodes <| id
        }

    let buttonElement (callback: Web.MouseEventArgs -> unit) (cssClasses: CssClassesOrEmpty) =
        button {
            cssClasses.Value

            on.click callback
        }

    let htmlComment (comment: string) = rawHtml $"<!-- {comment} -->"

    let imageElement
        (cssClasses: CssClassesOrEmpty)
        (moreAttrs: HtmlAttributesOrEmpty)
        (alt: string)
        (uri: Uri) =
        img {
            cssClasses.Value

            attr.src uri.OriginalString
            attr.alt alt
            moreAttrs.Value
        }

    ///<remarks>
    /// Remember that the specified child <see cref="Node" /> can be <see cref="rawHtml" />.
    ///</remarks>
    let paragraphElement
        (cssClasses: CssClassesOrEmpty)
        (moreAttrs: HtmlAttributesOrEmpty)
        (childNode: Node) =
        p {
            cssClasses.Value
            moreAttrs.Value

            childNode
        }
