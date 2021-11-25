module Songhay.Dashboard.Client.Templates.MainTemplate

open Bolero
open Bolero.Html
open Songhay.Dashboard.Client.BoleroTypes
open Songhay.Dashboard.Client.Routing

type Main = Template<"wwwroot/main.html">

let homePage model dispatch =
    Main.Home().Elt()

let menuItem (model: Model) (page: Page) (text: string) =
    Main.MenuItem()
        .Active(if model.page = page then "is-active" else "")
        .Url(router.Link page)
        .Text(text)
        .Elt()

let viewMainTemplate model dispatch =
    Main()
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
                Main.ErrorNotification()
                    .Text(err)
                    .Hide(fun _ -> dispatch ClearError)
                    .Elt()
        )
        .Elt()
