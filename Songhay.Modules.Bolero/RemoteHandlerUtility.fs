namespace Songhay.Modules.Bolero

module RemoteHandlerUtility =

    open System.Net.Http
    open System.Text.Json
    open System.Threading.Tasks
    open Microsoft.Extensions.Logging

    open Songhay.Modules.HttpResponseMessageUtility
    open Songhay.Modules.Bolero.JsonDocumentUtility

    let toHandlerOutputAsync<'TOutput>
        (logger: ILogger)
        (dataGetter: Result<JsonElement, JsonException> -> 'TOutput option)
        (responseResult: Result<HttpResponseMessage, exn>) : Task<'TOutput option> =
        task {
            logger.LogInformation("processing result...")

            match responseResult with
            | Result.Error err ->
                logger.LogError(err.Message, err.GetType().FullName, err.Source, err.StackTrace)
                return None

            | Result.Ok response ->
                let! jsonResult = response |> tryDownloadToStringAsync
                return jsonResult |> tryGetJsonElement logger |> dataGetter
        }
