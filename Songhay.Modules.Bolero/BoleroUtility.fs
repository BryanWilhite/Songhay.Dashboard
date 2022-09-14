namespace Songhay.Modules.Bolero

open System

open Bolero
open Bolero.Html

module BoleroUtility =

    ///<summary>
    /// Defines selected event names of the <c>GlobalEventHandlers</c> mixin.
    ///</summary>
    /// <remarks>
    /// See https://developer.mozilla.org/en-US/docs/Web/API/GlobalEventHandlers
    /// </remarks>
    type GlobalEventHandlers =
        | OnBlur
        | OnChange
        | OnClick
        | OnDblClick
        | OnFocus
        | OnKeyDown
        | OnKeyUp
        | OnLoad
        | OnLoadEnd

        member this.toLower =
            match this with
            | OnBlur -> (nameof OnBlur).ToLowerInvariant()
            | OnChange -> (nameof OnChange).ToLowerInvariant()
            | OnClick -> (nameof OnClick).ToLowerInvariant()
            | OnDblClick -> (nameof OnDblClick).ToLowerInvariant()
            | OnFocus -> (nameof OnFocus).ToLowerInvariant()
            | OnKeyDown -> (nameof OnKeyDown).ToLowerInvariant()
            | OnKeyUp -> (nameof OnKeyUp).ToLowerInvariant()
            | OnLoad -> (nameof OnLoad).ToLowerInvariant()
            | OnLoadEnd -> (nameof OnLoadEnd).ToLowerInvariant()

        member this.PreventDefault =
            on.preventDefault this.toLower true

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
