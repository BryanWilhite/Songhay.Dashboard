namespace Songhay.Player.YouTube

open System
open System.Net
open System.Text.Json
open Microsoft.Extensions.Logging

open FsToolkit.ErrorHandling

open Songhay.Modules
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.StringUtility
open Songhay.Player.YouTube.DisplayItemModelUtility

module YtThumbsSetServiceHandlerUtility =

    let getYtSetKey seed (uri: Uri) =
        let prefix = seed |> toKabobCase |> Option.defaultValue String.Empty
        $"{prefix}-{uri.Segments |> Array.last}"

    let toDomainData (logger: ILogger) (jsonResult: Result<string, HttpStatusCode>) =
        jsonResult
        |> Result.mapError
               (
                    fun code ->
                        logger.LogError($"The expected {nameof HttpStatusCode}, `{code},` is not here.")
                        exn $"{nameof HttpStatusCode}: {code.ToString()}"
               )
        |> Result.bind (fun json -> json |> tryGetRootElement)
        |> Result.mapError
               (
                   fun err ->
                       logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                       JsonException err.Message
                )
        |> Result.bind (fun el -> el |> ThumbsSet.fromInput)
        |> Result.map (fun input -> input |> List.toArray)
        |> Option.ofResult
