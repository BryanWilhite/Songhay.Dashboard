namespace Songhay.Dashboard.Models

open Songhay.Modules.Models

type VersionData =
    {
        id: Identifier
        title: DisplayText
        version: string
    }
