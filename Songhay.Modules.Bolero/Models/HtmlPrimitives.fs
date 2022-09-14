namespace Songhay.Modules.Bolero.Models

open Bolero
open Bolero.Html

///<summary>
/// Defines a subset of the Global WAI-ARIA attributes.
/// 📖 https://www.w3.org/WAI/standards-guidelines/aria/
///</summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes#global_aria_attributes
/// </remarks>
type AriaGlobal =
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaBusy
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaControls
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaCurrent
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaDescription
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaDisabled
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaErrorMessage
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaHasPopup
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaHidden
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaInvalid
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaKeyShortcuts
    ///<summary> Global WAI-ARIA attribute </summary>
    | AriaLabel

    ///<summary>Returns the <see cref="string" /> representation of the WAI-ARIA attribute.</summary>
    member this.AttrName =
        match this with
        | AriaBusy -> "aria-busy"
        | AriaControls -> "aria-controls"
        | AriaCurrent -> "aria-current"
        | AriaDescription -> "aria-description"
        | AriaDisabled -> "aria-disabled"
        | AriaErrorMessage -> "aria-errormessage"
        | AriaHasPopup -> "aria-haspopup"
        | AriaHidden -> "aria-hidden"
        | AriaInvalid -> "aria-invalid"
        | AriaKeyShortcuts -> "aria-keyshortcuts"
        | AriaLabel -> "aria-label"

    ///<summary>
    /// Returns an <see cref="Attr" /> representing a CSS declaration
    /// like <c>aria-busy="true"</c>.
    /// </summary>
    member this.ToAttr = (this.AttrName, "true") ||> Attr.Make

///<summary>
/// Defines a type representing an <see cref="Attr" />
/// or a type representing the non-presence of <see cref="Attr" />.
/// </summary>
type HtmlAttributeOrEmpty =
    ///<summary> the non-presence of <see cref="Attr" /> </summary>
    | NoAttr
    ///<summary> the presence of <see cref="Attr" /> </summary>
    | HasAttr of Attr

    ///<summary>Returns <see cref="Attr" /> or <see cref="attr.empty" />.</summary>
    member this.Value =
        match this with
        | NoAttr -> attr.empty()
        | HasAttr attribute -> attribute

///<summary>
/// Defines a type representing a list of <see cref="Attr" />
/// or a type representing the non-presence of this list.
/// </summary>
type HtmlAttributesOrEmpty =
    ///<summary> the non-presence of a list of <see cref="Attr" /> </summary>
    | NoAttrs
    ///<summary> the presence of a list of <see cref="Attr" /> </summary>
    | HasAttrs of Attr list

    ///<summary>
    /// Returns <see cref="Attr" />,
    /// representing a combination of a list of <see cref="Attr" />
    /// or <see cref="attr.empty" />.
    /// </summary>
    member this.Value =
        match this with
        | NoAttrs -> attr.empty()
        | HasAttrs attributes ->
            match attributes with
            | [ a ] -> a
            | _ -> attributes |> List.reduce(fun a b -> attrs.Combine(a, b))

///<summary>
/// Defines rules around how to handle a <see cref="Node" />.
/// </summary>
type HtmlChildNodeOrReplaceDefault =
    ///<summary> the <see cref="Node" /> is regarded as a child </summary>
    | ChildNode of Node
    ///<summary> the <see cref="Node" /> is regarded as a replacement </summary>
    | ReplacementNode of Node

///<summary>
/// Defines whether an HTML element is active.
/// This is useful for an HTML element representing a selectable item.
/// </summary>
type HtmlElementActiveOrDefault =
    ///<summary> the HTML element is active </summary>
    | ActiveState
    ///<summary> the HTML element is not active </summary>
    | DefaultState

///<summary>
/// Defines a type representing an <see cref="Node" />
/// or a type representing the non-presence of <see cref="Node" />.
/// </summary>
type HtmlNodeOrEmpty =
    ///<summary> the non-presence of <see cref="Node" /> </summary>
    | NoNode
    ///<summary> the presence of <see cref="Node" /> </summary>
    | HasNode of Node

    ///<summary>Returns <see cref="Node" /> or <see cref="empty" />.</summary>
    member this.Value = match this with | NoNode -> empty() | HasNode node -> node

///<summary>
/// Defines a type representing a list of <see cref="Node" />
/// or a type representing the non-presence of this list.
/// </summary>
type HtmlNodesOrEmpty =
    ///<summary> the non-presence of a list of <see cref="Node" /> </summary>
    | NoNodes
    ///<summary> the presence of a list of <see cref="Node" /> </summary>
    | HasNodes of Node list

    ///<summary>
    /// Passes through a list of <see cref="Node" /> or a list with one empty <see cref="Node" />.
    /// </summary>
    member this.Value = match this with | NoNodes -> [ empty() ] | HasNodes nodes -> nodes

///<summary>
/// Enumerates the names of “where to display the linked URL”
/// for the <c>target</c> attribute of the anchor element (<c>a</c>)
/// or to not specify this.
/// </summary>
/// <remarks>
/// 📖 https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a#attr-target
/// </remarks>
type HtmlTargetOrEmpty =
    ///<summary> do not specify where to display the linked URL </summary>
    | DoNotSpecifyTarget
    ///<summary> the <c>_self</c> target </summary>
    | TargetSelf
    ///<summary> the <c>_blank</c> target </summary>
    | TargetBlank
    ///<summary> the <c>_parent</c> target </summary>
    | TargetParent
    ///<summary> the <c>_top</c> target </summary>
    | TargetTop

    ///<summary>Returns the <see cref="string" /> representation of the target.</summary>
    member this.Value =
        match this with
        | DoNotSpecifyTarget -> attr.empty()
        | TargetSelf -> attr.target "_self"
        | TargetBlank -> attr.target "_blank"
        | TargetParent -> attr.target "_parent"
        | TargetTop -> attr.target "_top"
