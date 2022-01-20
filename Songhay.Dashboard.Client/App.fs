module Songhay.Dashboard.Client.App

open System

open Songhay.Dashboard.Client.Models

[<Literal>]
let appTitle = "SonghaySystem(::)"

[<Literal>]
let appDataLocation = "https://songhaystorage.blob.core.windows.net/studio-dash/app.json"

[<Literal>]
let appSvgViewBox = "0 0 24 24"

let appBlockChildCssClasses = [ "has-background-greys-dark-tone"; "is-child"; "tile" ]

let appSocialLinks =
    [
        {
            title = "@BryanWilhite on Twitter"
            href = "https://twitter.com/BryanWilhite"
            id = "mdi_twitter_24px"
            viewBox = appSvgViewBox
        }
        {
            title = "Bryan Wilhite on LinkedIn"
            href = "http://www.linkedin.com/in/wilhite"
            id = "mdi_linkedin_24px"
            viewBox = appSvgViewBox
        }
        {
            title = "rasx on StackOverflow"
            href = "http://stackoverflow.com/users/22944/rasx"
            id = "mdi_stack_overflow_24px"
            viewBox = appSvgViewBox
        }
        {
            title = "BryanWilhite on GitGub"
            href = "https://github.com/BryanWilhite"
            id = "mdi_github_circle_24px"
            viewBox = appSvgViewBox
        }
    ]

let appStudioLinks = [
    {
        title = "Azure DevOps"
        href = "https://songhay.visualstudio.com/"
        id = "mdi_visual_studio_24px"
        viewBox = appSvgViewBox
    }
    {
        title = ">Day Path_"
        href = "http://songhayblog.azurewebsites.net/"
        id = "mdi_rss_24px"
        viewBox = appSvgViewBox
    }
    {
        title = "Microsoft Azure"
        href = "https://portal.azure.com/"
        id = "mdi_azure_24px"
        viewBox = appSvgViewBox
    }
    {
        title = "Microsoft Developer"
        href = "https://developer.microsoft.com/"
        id = "mdi_microsoft_24px"
        viewBox = appSvgViewBox
    }
    {
        title = "OneDrive"
        href = "https://onedrive.live.com/"
        id = "mdi_office_24px"
        viewBox = appSvgViewBox
    }
]

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
