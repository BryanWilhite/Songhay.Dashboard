namespace Songhay.Modules.Bolero.Models

open Bolero
open Bolero.Html

///<summary>
/// https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Attributes#global_aria_attributes
///</summary>
type AriaGlobal =
    | AriaBusy
    | AriaControls
    | AriaCurrent
    | AriaDescription
    | AriaDisabled
    | AriaErrorMessage
    | AriaHasPopup
    | AriaHidden
    | AriaInvalid
    | AriaKeyShortcuts
    | AriaLabel

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

    member this.ToAttr = (this.AttrName, "true") ||> Attr.Make

type HtmlAttributeOrEmpty =
    | NoAttr
    | HasAttr of Attr

    member this.Value =
        match this with
        | NoAttr -> attr.empty()
        | HasAttr attribute -> attribute

type HtmlAttributesOrEmpty =
    | NoAttrs
    | HasAttrs of Attr list

    member this.Value =
        match this with
        | NoAttrs -> attr.empty()
        | HasAttrs attributes ->
            match attributes with
            | [ a ] -> a
            | _ -> attributes |> List.reduce(fun a b -> attrs.Combine(a, b))

type HtmlChildNodeOrReplaceDefault =
    | ChildNode of Node
    | ReplacementNode of Node

type HtmlElementActiveOrDefault =
    | ActiveState
    | DefaultState

type HtmlNodeOrEmpty =
    | NoNode
    | HasNode of Node

    member this.Value = match this with | NoNode -> empty() | HasNode node -> node

type HtmlNodesOrEmpty =
    | NoNodes
    | HasNodes of Node list

    member this.Value = match this with | NoNodes -> [ empty() ] | HasNodes nodes -> nodes

/// <remarks>
/// — https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a#attr-target
/// </remarks>
type HtmlTargetOrEmpty =
    | DoNotSpecifyTarget
    | TargetSelf
    | TargetBlank
    | TargetParent
    | TargetTop

    member this.Value =
        match this with
        | DoNotSpecifyTarget -> attr.empty()
        | TargetSelf -> attr.target "_self"
        | TargetBlank -> attr.target "_blank"
        | TargetParent -> attr.target "_parent"
        | TargetTop -> attr.target "_top"
