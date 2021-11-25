module Songhay.Dashboard.Client.Templates.ContentBlock

open Bolero
open Bolero.Html
open Bolero.Templating
open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.ElmishRoutes

type ContentBlockTemplate = Template<"wwwroot/content-block.html">

let homePage model dispatch =
    ContentBlockTemplate.Home().Elt()

let menuItem (model: Model) (page: Page) (text: string) =
    ContentBlockTemplate.MenuItem()
        .Active(if model.page = page then "is-active" else "")
        .Url(router.Link page)
        .Text(text)
        .Elt()

let viewMainTemplate model dispatch =
    ContentBlockTemplate()
        .Menu(concat [
            menuItem model Home (nameof Home)
        ])
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
