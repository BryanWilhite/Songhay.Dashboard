namespace Songhay.Modules.Bolero.Models

open Bolero
open Bolero.Html

type HtmlChildNodeOrReplaceDefault =
    | ChildNode of Node
    | ReplacementNode of Node

type HtmlNodeOrEmpty =
    | NoNode
    | HasNode of Node

    member this.Value = match this with | NoNode -> empty() | HasNode node -> node

type HtmlNodesOrEmpty =
    | NoNodes
    | HasNodes of Node list

    member this.Value = match this with | NoNodes -> [ empty() ] | HasNodes nodes -> nodes
