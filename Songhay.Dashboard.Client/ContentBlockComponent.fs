module Songhay.Dashboard.Client.Components.ContentBlock

open Elmish
open Bolero
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open Songhay.Dashboard.Client.ElmishTypes
open Songhay.Dashboard.Client.ElmishRoutes
open Songhay.Dashboard.Client.Models
open Songhay.Dashboard.Client.Templates.ContentBlock

let initModel =
    {
        page = StudioToolsPage
        error = None
    }

let update remote message model =
    match message with
    | SetPage page -> { model with page = page }, Cmd.none
    | Error exn -> { model with error = Some exn.Message }, Cmd.none
    | ClearError -> { model with error = None }, Cmd.none

let view model dispatch =
    viewContentBlockTemplate model dispatch

type ContentBlockComponent() =
    inherit ProgramComponent<Model, Message>()

    static member Id with get() = "content-block"

    override this.Program =
        let init = (fun _ -> initModel, Cmd.none)
        let dashboardService = this.Remote<DashboardService>()
        let update = update dashboardService
        Program.mkProgram init update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
