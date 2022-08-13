module Songhay.Modules.Publications.Models

open System
open System.Text.Json

open Songhay.Modules.Models
open Songhay.Modules.JsonDocumentUtility

let getElementName (useCamelCase: bool) (input: string) =
    let toCamelCase (input: string) = //TODO: move to `Songhay.Modules`
        $"{input[0].ToString().ToLowerInvariant()}{input[1..]}"
    if useCamelCase then input |> toCamelCase else input

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
            | Segment -> $"{nameof Segment}{nameof Id}" |> getElementName useCamelCase
            | Document -> $"{nameof Document}{nameof Id}" |> getElementName useCamelCase
            | Fragment -> $"{nameof Fragment}{nameof Id}" |> getElementName useCamelCase
        let getId id = Id(Identifier.fromInt32(id))
        let getIdString idString = Id(Identifier.fromString(idString))

        element
        |> tryGetProperty elementName
        |> Result.map
               (
                   fun el ->
                        match el.TryGetInt32() with
                        | false, _ -> el.GetString() |> getIdString
                        | true, id -> id |> getId
               )

    member this.Value = let (Id v) = this in v

type Title =
    | Title of string

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        element
        |> tryGetProperty ((nameof Title) |> getElementName useCamelCase)
        |> Result.map (fun el -> Title (el.GetString()))

type Name =
    | Name of string option

    static member fromInput (itemType: PublicationItem) (useCamelCase: bool)  (element: JsonElement) =
        let elementName =
            match itemType with
            | Segment -> None
            | Document -> $"File{nameof Name}" |> getElementName useCamelCase |> Some
            | Fragment -> $"{nameof Fragment}{nameof Name}" |> getElementName useCamelCase |> Some
        match elementName with
        | Some name -> element |> tryGetProperty name |> Result.map (fun el -> Name (el.GetString() |> Some))
        | _ -> (Name None) |> Ok

    member this.Value = let (Name v) = this in v

    member this.toItemName = this.Value |> Option.map ItemName

type Path =
    | Path of string

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        element
        |> tryGetProperty ((nameof Path) |> getElementName useCamelCase)
        |> Result.map (fun el -> Path (el.GetString()))

type FileName =
    | FileName of string

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        element
        |> tryGetProperty ((nameof FileName) |> getElementName useCamelCase)
        |> Result.map (fun el -> FileName (el.GetString()))

type IsActive =
    | IsActive of bool

    static member fromInput (useCamelCase: bool) (element: JsonElement) =
        element
        |> tryGetProperty ((nameof IsActive) |> getElementName useCamelCase)
        |> Result.map (fun el -> IsActive (el.GetBoolean()))
