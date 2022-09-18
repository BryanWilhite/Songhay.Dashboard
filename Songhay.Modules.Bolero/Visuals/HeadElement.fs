namespace Songhay.Modules.Bolero.Visuals

open System
open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models
open Songhay.Modules.MimeTypes
open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.BodyElement

///<summary>
/// Shared functions for generating selected HTML elements as a child of <c>head</c>.
///</summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML/Element/head
/// </remarks>
module HeadElement =

    let baseElement (href: string option) = ``base`` { attr.href (href |> Option.defaultWith (fun _ -> "/")) }
 
    let atomAlternateLinkElement (appTitle: string) (href: string) =
        link { attr.rel "alternate"; attr.``type`` ApplicationAtomXml; attr.title appTitle; attr.href href }

    let linkRelElement (rel: HtmlLinkedDocumentRelationship) (moreAttributes: HtmlAttributesOrEmpty) (href: Uri) =
        match rel with
        | RelIcon ->
            link { attr.rel RelIcon.Value; attr.``type`` "image/x-icon"; moreAttributes.Value; attr.href href }
        | RelBookmark | RelExternal | RelNoFollow | RelNoOpener | RelNoReferrer | RelTag
            -> htmlComment $"ERROR: the `link` element does not support `{rel.Value}`."
        | _ -> link { attr.rel rel.Value; moreAttributes.Value; attr.href href }

    let titleElement (appTitle: string) = title { text appTitle }

    let headElement (childElements: Node list) = head { forEach ((2, childElements) ||> wrapn) <| id }
