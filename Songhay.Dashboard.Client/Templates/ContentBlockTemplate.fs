module Songhay.Dashboard.Client.Templates.ContentBlock

open Bolero
open Bolero.Html
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Visuals

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

let contentBlock model dispatch =
    Empty //TODO return content

let viewMainTemplate model dispatch =
    ContentBlockTemplate()
        .StudioComponentNode(
            Tile.bulmaParentTile 4 [] //TODO return StudioComponentNode
        )
        .Content(
            cond model.page <| function
            | _ -> contentBlock model dispatch
        )
        .Error(
            cond model.error <| function
            | None -> empty
            | Some err ->
                ContentBlockTemplate.ErrorNotification()
                    .Text(err)
                    .Hide(fun _ -> dispatch ClearError)
                    .Elt()
        )
        .Elt()
