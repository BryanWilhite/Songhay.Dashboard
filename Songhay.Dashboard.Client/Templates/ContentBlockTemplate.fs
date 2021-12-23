module Songhay.Dashboard.Client.Templates.ContentBlock

open Bolero
open Bolero.Html
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Templates.SvgSprites
open Songhay.Dashboard.Client.Visuals

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

let viewMainTemplate model dispatch =
    ContentBlockTemplate()
        .SvgSpritesNode(SvgSpritesTemplate().Elt())
        .StudioComponentNode(Tile.studioComponentNode)
        .StudioLinksNode(Tile.studioLinksNode)
        .Content(
            cond model.page <| function
            | StudioFeedsPage -> Tile.studioPageNode Block.StudioFeeds.studioFeedsNode
            | StudioToolsPage -> Tile.studioPageNode Block.StudioTools.studioToolsNode
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
