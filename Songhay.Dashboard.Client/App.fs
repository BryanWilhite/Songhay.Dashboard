namespace Songhay.Dashboard.Client

open System

open Songhay.Modules.Bolero.BoleroUtility
open Songhay.Modules.Bolero.Visuals.Svg
open Songhay.Modules.Models
open Songhay.Dashboard.Models

module App =

    [<Literal>]
    let AppTitle = "SonghaySystem(::)"

    [<Literal>]
    let AppDataLocation = "https://songhaystorage.blob.core.windows.net/studio-dash/app.json"

    let appBlockChildCssClasses = CssClasses [ "tile"; "is-child"; "has-background-greys-dark-tone" ]

    let appVersions =
        let boleroVersion = $"{typeof<Bolero.Node>.Assembly.GetName().Version}"
        let dotnetRuntimeVersion = $"{Environment.Version.Major:D}.{Environment.Version.Minor:D2}"

        [
            {
                id = Keys.MDI_BOLERO_DANCE_24PX.ToAlphanumeric
                title = DisplayText $"Bolero {boleroVersion}"
                version = boleroVersion
            }
            {
                id = Keys.MDI_DOTNET_24PX.ToAlphanumeric
                title = DisplayText $".NET Runtime {dotnetRuntimeVersion}"
                version = dotnetRuntimeVersion
            }
        ]

    let appSocialLinks =
        [
            (
                DisplayText "@BryanWilhite on Twitter",
                Uri "https://twitter.com/BryanWilhite",
                Keys.MDI_TWITTER_24PX.ToAlphanumeric
            )
            (
                DisplayText "Bryan Wilhite on LinkedIn",
                Uri "http://www.linkedin.com/in/wilhite",
                Keys.MDI_LINKEDIN_24PX.ToAlphanumeric
            )
            (
                DisplayText "rasx on StackOverflow",
                Uri "http://stackoverflow.com/users/22944/rasx",
                Keys.MDI_STACK_OVERFLOW_24PX.ToAlphanumeric
            )
            (
                DisplayText "BryanWilhite on GitGub",
                Uri "https://github.com/BryanWilhite",
                Keys.MDI_GITHUB_CIRCLE_24PX.ToAlphanumeric
            )
        ]

    let appStudioLinks = [
        (
            DisplayText "Azure DevOps",
            Uri "https://songhay.visualstudio.com/",
            Keys.MDI_VISUAL_STUDIO_24PX.ToAlphanumeric
        )
        (
            DisplayText ">Day Path_",
            Uri "http://songhayblog.azurewebsites.net/",
            Keys.MDI_RSS_24PX.ToAlphanumeric
        )
        (
            DisplayText "Microsoft Azure",
            Uri "https://portal.azure.com/",
            Keys.MDI_AZURE_24PX.ToAlphanumeric
        )
        (
            DisplayText "Microsoft Developer",
            Uri "https://developer.microsoft.com/",
            Keys.MDI_MICROSOFT_24PX.ToAlphanumeric
        )
        (
            DisplayText "OneDrive",
            Uri "https://onedrive.live.com/",
            Keys.MDI_OFFICE_24PX.ToAlphanumeric
        )
    ]
