namespace Songhay.Modules.Publications

open System
open System.Text.Json

open FsToolkit.ErrorHandling

open Songhay.Modules.Models
open Songhay.Modules.Publications.Models

module DisplayItemModelUtility =
    let tryGetDisplayItemModel
        displayTextGetter
        (resourceIndicatorGetter: (PublicationItem -> JsonElement -> Result<Uri option, JsonException>) option)
        (itemType: PublicationItem) (element: JsonElement): Result<DisplayItemModel, JsonException> =

        let idResult = (itemType, element) ||> Id.fromInput
        let nameResult = (itemType, element) ||> Name.fromInput
        let displayTextResult =  (itemType, element) ||> displayTextGetter
        let resourceIndicatorResult =
            match resourceIndicatorGetter with
            | Some getter -> ((itemType, element) ||> getter)
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
                            itemName = Some (ItemName (nameResult |> Result.valueOr raise).Value)
                            displayText = Some (displayTextResult |> Result.valueOr raise)
                            resourceIndicator = (resourceIndicatorResult |> Result.valueOr raise)
                        }
               )
               Result.Error
