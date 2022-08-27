namespace Songhay.Player.YouTube

module ServiceHandlerUtility =

    open System
    open System.Net
    open Microsoft.Extensions.Logging

    open FsToolkit.ErrorHandling

    open Songhay.Modules.StringUtility
    open Songhay.Modules.Bolero.JsonDocumentUtility
    open Songhay.Player.YouTube
    open Songhay.Player.YouTube.DisplayItemModelUtility

    let getYtSetKey seed (uri: Uri) =
        let prefix = seed |> toKabobCase |> Option.defaultValue String.Empty
        $"{prefix}-{uri.Segments |> Array.last}"

    let toPublicationIndexData (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult |> tryGetJsonElement logger
        |> Result.bind (fun el -> el |> Index.fromInput)
        |> Option.ofResult

    let toYtItems (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult |> tryGetJsonElement logger
        |> Result.bind (fun el -> el |> YtItemUtility.fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult

    let toYtSet (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult |> tryGetJsonElement logger
        |> Result.bind (fun el -> el |> ThumbsSet.fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
