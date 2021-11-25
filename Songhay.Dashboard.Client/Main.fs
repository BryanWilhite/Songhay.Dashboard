module Songhay.Dashboard.Client.Main

open Elmish
open Bolero
open Bolero.Remoting.Client
open Bolero.Templating.Client
open Songhay.Dashboard.Client.BoleroTypes
open Songhay.Dashboard.Client.Routing
open Songhay.Dashboard.Client.Templates.MainTemplate

let initModel =
    {
        page = Home
        error = None
    }

let update message model =
    match message with
    | SetPage page -> { model with page = page }, Cmd.none
    | Error exn -> { model with error = Some exn.Message }, Cmd.none
    | ClearError -> { model with error = None }, Cmd.none

let view model dispatch =
    viewMainTemplate model dispatch

type MainApp() =
    inherit ProgramComponent<Model, Message>()

    override this.Program =
        let init = (fun _ -> initModel, Cmd.none)
        Program.mkProgram init update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
