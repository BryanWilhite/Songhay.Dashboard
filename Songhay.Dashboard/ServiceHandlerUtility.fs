namespace Songhay.Dashboard

open System.Text.Json

open FsToolkit.ErrorHandling

open Songhay.Dashboard.SyndicationFeedUtility

module ServiceHandlerUtility =

    let toAppData (jsonElementResult: Result<JsonElement, JsonException>) =
        jsonElementResult
        |> Result.bind (fun el -> el |> fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
