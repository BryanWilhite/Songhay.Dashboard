namespace Songhay.Modules.Publications

open System
open System.Text.Json

open FsToolkit.ErrorHandling

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.Publications.Models

module DisplayItemModelUtility =
    let defaultDisplayTextGetter
        (fragmentElementName: string option)
        (itemType: PublicationItem)
        (useCamelCase: bool)
        (jsonElement: JsonElement) =
        let displayTextResult elementName =
            jsonElement |> tryGetProperty elementName |> Result.map (fun el -> Some (el.GetString() |> DisplayText))
        match itemType with
        | Segment ->
            let elementName = $"{nameof Segment}{nameof Name}" |> getElementName useCamelCase
            elementName |> displayTextResult
        | Document ->
            let elementName = $"{nameof Title}" |> getElementName useCamelCase
            elementName |> displayTextResult
        | Fragment ->
            match fragmentElementName with
            | Some name -> name |> getElementName useCamelCase |> displayTextResult
            | _ -> Ok None

    let tryGetDisplayItemModel
        (displayTextGetter: PublicationItem -> bool -> JsonElement -> Result<DisplayText option, JsonException>)
        (resourceIndicatorGetter: (PublicationItem -> bool -> JsonElement -> Result<Uri option, JsonException>) option)
        (itemType: PublicationItem)
        (useCamelCase: bool)
        (element: JsonElement)
        : Result<DisplayItemModel, JsonException> =

        let idResult = (useCamelCase, element) ||> Id.fromInput itemType
        let nameResult = (useCamelCase, element) ||> Name.fromInput itemType
        let displayTextResult =  (useCamelCase, element) ||> displayTextGetter itemType
        let resourceIndicatorResult =
            match resourceIndicatorGetter with
            | Some getter -> ((useCamelCase, element) ||> getter itemType)
            | _ -> Ok None

        [
            idResult |> Result.map (fun _ -> true)
            nameResult  |> Result.map (fun _ -> true)
            displayTextResult  |> Result.map (fun _ -> true)
            resourceIndicatorResult  |> Result.map (fun _ -> true)
        ]
        |> List.sequenceResultM
        |> Result.either
              (
                   fun _ ->
                        Ok {
                            id = (idResult |> Result.valueOr raise).Value
                            itemName = (nameResult |> Result.valueOr raise).toItemName
                            displayText = (displayTextResult |> Result.valueOr raise)
                            resourceIndicator = (resourceIndicatorResult |> Result.valueOr raise)
                        }
               )
               Result.Error
