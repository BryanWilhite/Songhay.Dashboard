namespace Songhay.Modules.Publications.Tests

module DisplayItemModelUtilityTests =

    open System
    open System.IO
    open System.Reflection
    open System.Text.Json

    open Xunit

    open Xunit
    open FsUnit.Xunit
    open FsUnit.CustomMatchers
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.JsonDocumentUtility
    open Songhay.Modules.ProgramFileUtility
    open Songhay.Modules.Publications.Models
    open Songhay.Modules.Publications.DisplayItemModelUtility

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> Result.valueOr raiseProgramFileError
        |> DirectoryInfo

    let getJsonDocument (fileName: string) =
        let path =
            $"./json/{fileName}"
            |> tryGetCombinedPath projectDirectoryInfo.FullName
            |> Result.valueOr raiseProgramFileError
        JsonDocument.Parse(File.ReadAllText(path))

    [<Theory>]
    [<InlineData("Segment", true,"segment-without-documents.json")>]
    let ``tryGetDisplayItemModel test`` (itemTypeString: string, shouldUseCamelCase: bool, fileName: string) =
        let jsonDocument = fileName |> getJsonDocument
        let displayTextGetter =
            fun itemType useCamelCase (jsonElement: JsonElement) ->
                let elementName =
                    match itemType with
                    | Segment -> $"{nameof Segment}{nameof Name}" |> getElementName useCamelCase
                    | Document -> "Title" |> getElementName useCamelCase
                    | Fragment -> "ClientId" |> getElementName useCamelCase

                jsonElement |> tryGetProperty elementName |> Result.map (fun el -> el.GetString() |> DisplayText)

        let result =
            (shouldUseCamelCase, jsonDocument.RootElement)
            ||> tryGetDisplayItemModel
                displayTextGetter
                None
                (itemTypeString |> PublicationItem.fromString |> Result.valueOr raise)

        result |> should be (ofCase <@ Result<DisplayItemModel, JsonException>.Ok @>)
