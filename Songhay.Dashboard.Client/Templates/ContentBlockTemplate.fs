module Songhay.Dashboard.Client.Templates.ContentBlock

open Bolero
open Bolero.Html
open Songhay.Dashboard.Client.ElmishTypes

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

let homePage model dispatch =
    ContentBlockTemplate.Home().Elt()

let viewMainTemplate model dispatch =
    ContentBlockTemplate()
        .Body(
            cond model.page <| function
            | Home -> homePage model dispatch
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
