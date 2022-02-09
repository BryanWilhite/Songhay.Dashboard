module Songhay.Dashboard.Client.App

open System

open Songhay.Dashboard.Client.Models

[<Literal>]
let appTitle = "SonghaySystem(::)"

[<Literal>]
let appDataLocation = "https://songhaystorage.blob.core.windows.net/studio-dash/app.json"

let appBlockChildCssClasses = [ "has-background-greys-dark-tone"; "is-child"; "tile" ]

let appVersions =
    let boleroVersion = $"{Bolero.Node.Empty.GetType().Assembly.GetName().Version}"
    let dotnetRuntimeVersion = $"{Environment.Version.Major:D}.{Environment.Version.Minor:D2}"

    [
        {
            id = "mdi_bolero_dance_24px"
            title = $"Bolero {boleroVersion}"
            version = boleroVersion
        }
        {
            id = "mdi_dotnet_24px"
            title = $".NET Runtime {dotnetRuntimeVersion}"
            version = dotnetRuntimeVersion
        }
    ]
