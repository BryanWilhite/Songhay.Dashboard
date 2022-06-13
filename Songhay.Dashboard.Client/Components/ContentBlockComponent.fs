namespace Songhay.Dashboard.Client.Components

open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Elmish

open Bolero
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open Songhay.Player.YouTube

open Songhay.Dashboard.Client.Components.ElmishProgram
open Songhay.Dashboard.Client.ElmishRoutes
open Songhay.Dashboard.Client.ElmishTypes

type ContentBlockComponent() =
    inherit ProgramComponent<Model, Message>()

    static member val Id = "content-block" with get

    [<Inject>]
    member val JSRuntime = Unchecked.defaultof<IJSRuntime> with get, set

    override this.Program =
        let initModel =
            {
                error = None
                feeds = None
                page = StudioToolsPage
                ytModel = YouTubeModel.initialize
            }
        let init = (fun _ -> initModel, Cmd.ofMsg (Message.YouTubeMessage YouTubeMessage.CallYtItems))
        let update = update (this.Remote<DashboardService>())
        let view = view this.JSRuntime

        Program.mkProgram init update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
