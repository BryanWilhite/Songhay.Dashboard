namespace Songhay.Dashboard.Client.Models

open Songhay.Modules.Models

type VersionData =
    {
        id: Identifier
        title: DisplayText
        version: string
    }
