namespace Songhay.Player.YouTube

open System.Net
open System.Text.Json
open Microsoft.Extensions.Logging

open FsToolkit.ErrorHandling

open Songhay.Modules.JsonDocumentUtility
open Songhay.Player.YouTube.DisplayItemModelUtility

module YtThumbsSetServiceHandlerUtility =

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
