module Songhay.Modules.Publications.Models

open System
open System.Text.Json

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility
open Songhay.Modules.StringUtility

type PublicationItem =
    | Segment
    | Document
    | Fragment

    static member fromString (s: string) =
        match s.ToLower() with
        | "segment" -> Ok Segment
        | "document" -> Ok Document
        | "fragment" -> Ok Fragment
        | _ -> Error (FormatException("The expected conventional string is not here."))

type Id =
    | Id of Identifier

    static member fromInput (itemType: PublicationItem) (useCamelCase: bool) (element: JsonElement) =
        let elementName =
            match itemType with
            | Segment -> $"{nameof Segment}{nameof Id}" |> toCamelCaseOrDefault useCamelCase
            | Document -> $"{nameof Document}{nameof Id}" |> toCamelCaseOrDefault useCamelCase
            | Fragment -> $"{nameof Fragment}{nameof Id}" |> toCamelCaseOrDefault useCamelCase

        match elementName with
        | None -> JsonException("The expected element-name input is not here") |> Error
        | Some name -> element |> Id.fromInputElementName name

    static member fromInputElementName elementName (element: JsonElement) =
        element
        |> tryGetProperty elementName
        |> Result.bind
            (
                fun el ->
                    match el.ValueKind with
                    | JsonValueKind.String -> el.GetString() |> fun idString -> Identifier.fromString(idString) |> Id |> Ok
                    | JsonValueKind.Number -> el.GetInt32() |> fun id -> Identifier.fromInt32(id) |> Id |> Ok
                    | _ -> JsonException("Only alphanumeric or integer identifiers are supported.") |> Error
            )

    member this.Value = let (Id v) = this in v

type Title =
    | Title of string

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        let elementName = (nameof Title) |> toCamelCaseOrDefault useCamelCase
        match elementName with
        | None -> JsonException("The expected element-name input is not here") |> Error
        | Some name ->
            element
            |> tryGetProperty name
            |> Result.map (fun el -> Title (el.GetString()))

type Name =
    | Name of string option

    static member fromInput (itemType: PublicationItem) (useCamelCase: bool) (element: JsonElement) =
        let elementName =
            match itemType with
            | Segment -> None
            | Document -> $"File{nameof Name}" |> toCamelCaseOrDefault useCamelCase
            | Fragment -> $"{nameof Fragment}{nameof Name}" |> toCamelCaseOrDefault useCamelCase

        match elementName with
        | None when itemType = Segment -> (Name None) |> Ok
        | None -> JsonException("The expected element-name input is not here") |> Error
        | Some name -> element |> Name.fromInputElementName name

    static member fromInputElementName elementName (element: JsonElement) =
        element |> tryGetProperty elementName |> Result.map (fun el -> Name (el.GetString() |> Some))

    member this.Value = let (Name v) = this in v

    member this.toItemName = this.Value |> Option.map ItemName

type Path =
    | Path of string

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        let elementName = (nameof Path) |> toCamelCaseOrDefault useCamelCase
        match elementName with
        | None -> JsonException("The expected element-name input is not here") |> Error
        | Some name ->
            element
            |> tryGetProperty name
            |> Result.map (fun el -> Path (el.GetString()))

type FileName =
    | FileName of string

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        let elementName = (nameof FileName) |> toCamelCaseOrDefault useCamelCase
        match elementName with
        | None -> JsonException("The expected element-name input is not here") |> Error
        | Some name ->
            element
            |> tryGetProperty name
            |> Result.map (fun el -> FileName (el.GetString()))

type IsActive =
    | IsActive of bool

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        let elementName = (nameof IsActive) |> toCamelCaseOrDefault useCamelCase
        match elementName with
        | None -> JsonException("The expected element-name input is not here") |> Error
        | Some name ->
            element
            |> tryGetProperty name
            |> Result.map (fun el -> IsActive (el.GetBoolean()))
