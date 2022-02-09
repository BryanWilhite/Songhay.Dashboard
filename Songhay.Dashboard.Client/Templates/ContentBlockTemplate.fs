module Songhay.Dashboard.Client.Templates.ContentBlock

open Bolero
open Bolero.Html

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Visuals

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

let viewContentBlockTemplate jsRuntime model dispatch =
    ContentBlockTemplate()
        .StudioComponentNode(Tile.studioComponentNode)
        .StudioLinksNode(Tile.studioLinksNode)
        .Content(
            cond model.page <| function
            | StudioFeedsPage -> Tile.studioPageNode (Block.StudioFeeds.studioFeedsNodes jsRuntime model)
            | StudioToolsPage -> Tile.studioPageNode [ Block.StudioTools.studioToolsNode model]
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
