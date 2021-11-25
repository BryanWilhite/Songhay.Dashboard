namespace Songhay.Dashboard.Client.BoleroTypes

open Bolero

type Page =
    | [<EndPoint "/">] Home

type Message =
    | SetPage of Page
    | Error of exn
    | ClearError

type Model =
    {
        page: Page
        error: string option
    }
