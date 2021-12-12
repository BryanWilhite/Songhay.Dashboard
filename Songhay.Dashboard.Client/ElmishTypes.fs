namespace Songhay.Dashboard.Client.ElmishTypes

open Bolero

type Page =
    | [<EndPoint "/">] StudioToolsPage
    | [<EndPoint "/dash">] DashboardPage

type Message =
    | SetPage of Page
    | Error of exn
    | ClearError

type Model =
    {
        page: Page
        error: string option
    }
