namespace Songhay.Modules.Bolero

open System

open Bolero
open Bolero.Html

/// <summary>
/// Utility functions mostly for <see cref="Node" />.
/// </summary>
module BoleroUtility =

    /// <summary>
    /// Wraps <see cref="Environment.NewLine" /> to format markup.
    /// </summary>
    let newLine = rawHtml $"{Environment.NewLine}"

    /// <summary>
    /// Return spaces of indentation to format markup.
    /// </summary>
    let indent level =
        let numberOfSpaces = 4
        let spaceChar = ' '
        let charArray =
            spaceChar
            |> Array.replicate (numberOfSpaces * level)

        String(charArray) |> text

    /// <summary>
    /// Inject a new line and indentation
    /// between the specified list of <see cref="Node" />.
    /// </summary>
    let wrapn indentLevel (nodes : Node list) =
        nodes |> List.collect (fun node -> [newLine; (indentLevel |> indent); node])
