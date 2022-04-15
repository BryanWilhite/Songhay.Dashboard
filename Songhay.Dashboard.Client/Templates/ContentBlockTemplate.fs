module Songhay.Dashboard.Client.Templates.ContentBlock

open Bolero
open Bolero.Html

open Songhay.Player.YouTube

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.Visuals

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

let viewContentBlockTemplate jsRuntime (model: Model) dispatch =
    ContentBlockTemplate()
        .StudioComponentNode(Tile.studioComponentNode)
        .StudioLinksNode(Tile.studioLinksNode)
        .Error(
            cond model.error <| function
            | None -> empty
            | Some err ->
                ContentBlockTemplate.ErrorNotification()
                    .Text(err)
                    .Hide(fun _ -> dispatch Message.ClearError)
                    .Elt()
        )
        .Content(
            cond model.page <| function
            | StudioFeedsPage -> Tile.studioPageNode (Block.StudioFeeds.studioFeedsNodes jsRuntime model)
            | StudioToolsPage -> Tile.studioPageNode [ Block.StudioTools.studioToolsNode() ]
        )
        .YouTubeThumbs(
            YtThumbs.view [ "YtThumbsTitle" => "songhay tube" ] model.ytModel (Message.YouTubeMessage >> dispatch)
        )
        .YouTubeThumbsSet(
            YtThumbsSet.view model.ytModel (Message.YouTubeMessage >> dispatch)
        )
        .Elt()
