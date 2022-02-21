namespace Songhay.Modules.Player

module BoleroUtility =

    open System
    open Bolero
    open Bolero.Html

    let newLine = RawHtml $"{Environment.NewLine}"

    let wrapn indentLevel (nodes : Node list) =
        let numberOfSpaces = 4
        let spaceChar = ' '
        let charArray =
            spaceChar
            |> Array.replicate (numberOfSpaces * indentLevel)
        let indent = String(charArray)

        nodes |> List.collect (fun node -> [newLine; text indent; node])
