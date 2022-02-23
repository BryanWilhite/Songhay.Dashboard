module Songhay.Dashboard.Client.App

open System

open Songhay.Modules.Models
open Songhay.Dashboard.Models

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

let appSocialLinks =
    [
        (
            DisplayText "@BryanWilhite on Twitter",
            Uri "https://twitter.com/BryanWilhite",
            Alphanumeric "mdi_twitter_24px"
        )
        (
            DisplayText "Bryan Wilhite on LinkedIn",
            Uri "http://www.linkedin.com/in/wilhite",
            Alphanumeric "mdi_linkedin_24px"
        )
        (
            DisplayText "rasx on StackOverflow",
            Uri "http://stackoverflow.com/users/22944/rasx",
            Alphanumeric "mdi_stack_overflow_24px"
        )
        (
            DisplayText "BryanWilhite on GitGub",
            Uri "https://github.com/BryanWilhite",
            Alphanumeric "mdi_github_circle_24px"
        )
    ]

let appStudioLinks = [
    (
        DisplayText "Azure DevOps",
        Uri "https://songhay.visualstudio.com/",
        Alphanumeric "mdi_visual_studio_24px"
    )
    (
        DisplayText ">Day Path_",
        Uri "http://songhayblog.azurewebsites.net/",
        Alphanumeric "mdi_rss_24px"
    )
    (
        DisplayText "Microsoft Azure",
        Uri "https://portal.azure.com/",
        Alphanumeric "mdi_azure_24px"
    )
    (
        DisplayText "Microsoft Developer",
        Uri "https://developer.microsoft.com/",
        Alphanumeric "mdi_microsoft_24px"
    )
    (
        DisplayText "OneDrive",
        Uri "https://onedrive.live.com/",
        Alphanumeric "mdi_office_24px"
    )
]
