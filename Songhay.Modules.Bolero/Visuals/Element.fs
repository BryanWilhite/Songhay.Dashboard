namespace Songhay.Modules.Bolero.Visuals

open System
open Microsoft.AspNetCore.Components

open Bolero
open Bolero.Html

open Songhay.Modules.Bolero.Models

///<summary>
/// Shared functions for generating HTML elements.
///</summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML
/// </remarks>
module Element =

    ///<summary>
    /// Returns the HTML anchor element, <c>a</c>, adorned with any CSS classes.
    ///</summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a
    /// </remarks>
    let anchorElement (moreClasses: CssClassesOrEmpty) (href: Uri) (target: HtmlTargetOrEmpty) (childNodes: Node list) =
        a {
            moreClasses.Value

            attr.href href.OriginalString
            target.Value

            forEach childNodes <| id
        }

    ///<summary>
    /// Returns the HTML <c>button</c> element,
    /// declaring an eventing 🗲🐎 callback and adorned with any CSS classes.
    ///</summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button
    /// </remarks>
    let buttonElement (cssClasses: CssClassesOrEmpty) (callback: Web.MouseEventArgs -> unit) =
        button {
            cssClasses.Value

            on.click callback
        }

    ///<summary>
    /// Returns the HTML <c>footer</c> element, adorned with any CSS classes.
    ///</summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML/Element/footer
    /// </remarks>
    let footerElement (moreClasses: CssClassesOrEmpty) (childNodes: Node list) =
        footer {
            moreClasses.Value

            forEach childNodes <| id
        }

    ///<summary>
    /// Returns an HTML comment.
    ///</summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Learn/HTML/Introduction_to_HTML/Getting_started#html_comments
    /// </remarks>
    let htmlComment (comment: string) = rawHtml $"<!-- {comment} -->"

    ///<summary>
    /// Returns the HTML image embed element, <c>img</c>, adorned with any CSS classes and any attributes.
    ///</summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML/Element/img
    /// </remarks>
    let imageElement
        (cssClasses: CssClassesOrEmpty)
        (moreAttrs: HtmlAttributesOrEmpty)
        (alt: string)
        (src: Uri) =
        img {
            cssClasses.Value

            attr.src src.OriginalString
            attr.alt alt
            moreAttrs.Value
        }

    ///<summary>
    /// Returns the HTML paragraph element, <c>p</c>, adorned with any CSS classes and any attributes.
    ///</summary>
    /// <remarks>
    /// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML/Element/p
    /// 
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
