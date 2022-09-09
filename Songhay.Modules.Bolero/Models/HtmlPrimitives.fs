namespace Songhay.Modules.Bolero.Models

open Bolero
open Bolero.Html

type HtmlAttributesOrEmpty =
    | NoAttributes
    | HasAttributes of Attr list

    member this.Value =
        match this with
        | NoAttributes -> attr.empty()
        | HasAttributes attributes ->
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
