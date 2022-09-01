namespace Songhay.Modules.Bolero

module RemoteHandlerUtility =

    open System.Net
    open System.Net.Http
    open System.Text.Json
    open System.Threading.Tasks
    open Microsoft.Extensions.Logging

    open Songhay.Modules.HttpResponseMessageUtility
    open Songhay.Modules.Bolero.JsonDocumentUtility

    let tryDownloadToStringAsync (logger: ILogger option) (responseResult: Result<HttpResponseMessage, exn>) =
        task {
            if logger.IsSome then logger.Value.LogInformation("downloading string result...")

            match responseResult with
            | Result.Error err ->
                if logger.IsSome then logger.Value.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                return Result.Error HttpStatusCode.InternalServerError

            | Result.Ok response ->
                let! stringResult = response |> tryDownloadToStringAsync
                return stringResult
        }

    let toHandlerOutput<'TOutput>
        (logger: ILogger option)
        (dataGetter: Result<JsonElement, JsonException> -> 'TOutput option)
        (stringResult: Result<string,HttpStatusCode>) : 'TOutput option =
        stringResult |> tryGetJsonElement logger |> dataGetter

    let toHandlerOutputAsync<'TOutput>
        (logger: ILogger option)
        (dataGetter: Result<JsonElement, JsonException> -> 'TOutput option)
        (responseResult: Result<HttpResponseMessage, exn>) : Task<'TOutput option> =
        task {
            let! stringResult = (logger, responseResult) ||> tryDownloadToStringAsync
            return stringResult |> tryGetJsonElement logger |> dataGetter
        }
