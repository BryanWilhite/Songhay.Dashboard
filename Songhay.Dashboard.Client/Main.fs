module Songhay.Dashboard.Client.Main

open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting.Client
open Bolero.Templating.Client

type Page =
    | [<EndPoint "/">] Home

type Model =
    {
        page: Page
        error: string option
    }

let initModel =
    {
        page = Home
        error = None
    }

type Message =
    | SetPage of Page
    | Error of exn
    | ClearError

let update message model =
    match message with
    | SetPage page -> { model with page = page }, Cmd.none
    | Error exn -> { model with error = Some exn.Message }, Cmd.none
    | ClearError -> { model with error = None }, Cmd.none

let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">

let homePage model dispatch =
    Main.Home().Elt()

let menuItem (model: Model) (page: Page) (text: string) =
    Main.MenuItem()
        .Active(if model.page = page then "is-active" else "")
        .Url(router.Link page)
        .Text(text)
        .Elt()

let view model dispatch =
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

type MainApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        let init = (fun _ -> initModel, Cmd.none)
        Program.mkProgram init update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
