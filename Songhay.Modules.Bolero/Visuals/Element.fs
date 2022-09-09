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
