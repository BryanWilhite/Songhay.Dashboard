namespace Songhay.Player.YouTube

module ServiceHandlerUtility =

    open System
    open System.Text.Json

    open FsToolkit.ErrorHandling

    open Songhay.Modules.StringUtility
    open Songhay.Player.YouTube
    open Songhay.Player.YouTube.DisplayItemModelUtility

    let getYtSetKey seed (uri: Uri) =
        let prefix = seed |> toKabobCase |> Option.defaultValue String.Empty
        $"{prefix}-{uri.Segments |> Array.last}"

    let toPublicationIndexData (jsonElementResult: Result<JsonElement, JsonException>) =
        jsonElementResult
        |> Result.bind (fun el -> el |> Index.fromInput)
        |> Option.ofResult

    let toYtItems (jsonElementResult: Result<JsonElement, JsonException>) =
        jsonElementResult
        |> Result.bind (fun el -> el |> YtItemUtility.fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult

    let toYtSet (jsonElementResult: Result<JsonElement, JsonException>) =
        jsonElementResult
        |> Result.bind (fun el -> el |> ThumbsSet.fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
