module Songhay.Dashboard.Client.Templates.ContentBlock

open Bolero
open Bolero.Html
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Visuals
open Songhay.Dashboard.Client.Templates.SvgSprites

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

let viewMainTemplate model dispatch =
    ContentBlockTemplate()
        .SvgSpritesNode(SvgSpritesTemplate().Elt())
        .StudioComponentNode(Tile.studioComponentNode)
        .Content(
            cond model.page <| function
            | _ -> Empty // TODO: define pages
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