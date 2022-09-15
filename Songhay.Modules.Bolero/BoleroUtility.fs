namespace Songhay.Modules.Bolero

open System

open Bolero
open Bolero.Html

module BoleroUtility =

    let newLine = rawHtml $"{Environment.NewLine}"

    let indent level =
        let numberOfSpaces = 4
        let spaceChar = ' '
        let charArray =
            spaceChar
            |> Array.replicate (numberOfSpaces * level)

        String(charArray) |> text

    let wrapn indentLevel (nodes : Node list) =
        nodes |> List.collect (fun node -> [newLine; (indentLevel |> indent); node])
