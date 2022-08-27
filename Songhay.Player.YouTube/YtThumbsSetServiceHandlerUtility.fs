namespace Songhay.Player.YouTube

open System
open System.Net
open Microsoft.Extensions.Logging

open FsToolkit.ErrorHandling

open Songhay.Modules.StringUtility
open Songhay.Modules.Bolero.JsonDocumentUtility
open Songhay.Player.YouTube.DisplayItemModelUtility

module YtThumbsSetServiceHandlerUtility =

    let getYtSetKey seed (uri: Uri) =
        let prefix = seed |> toKabobCase |> Option.defaultValue String.Empty
        $"{prefix}-{uri.Segments |> Array.last}"

    let toYtSet (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult |> tryGetJsonElement logger
        |> Result.bind (fun el -> el |> ThumbsSet.fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
