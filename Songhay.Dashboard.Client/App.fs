module Songhay.Dashboard.Client.App

open System

open Songhay.Modules.Models
open Songhay.Dashboard.Client.Models

[<Literal>]
let AppTitle = "SonghaySystem(::)"

[<Literal>]
let AppDataLocation = "https://songhaystorage.blob.core.windows.net/studio-dash/app.json"

let appBlockChildCssClasses = [ "tile"; "is-child"; "has-background-greys-dark-tone" ]

let appVersions =
    let boleroVersion = $"{Bolero.Node.Empty.GetType().Assembly.GetName().Version}"
    let dotnetRuntimeVersion = $"{Environment.Version.Major:D}.{Environment.Version.Minor:D2}"

    [
        {
            id = Alphanumeric "mdi_bolero_dance_24px"
            title = DisplayText $"Bolero {boleroVersion}"
            version = boleroVersion
        }
        {
            id = Alphanumeric "mdi_dotnet_24px"
            title = DisplayText $".NET Runtime {dotnetRuntimeVersion}"
            version = dotnetRuntimeVersion
        }
    ]
