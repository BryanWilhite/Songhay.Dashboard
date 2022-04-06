namespace Songhay.Modules.Bolero

module BoleroUtility =

    open System
    open Bolero
    open Bolero.Html

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

        member this.toLower =
            match this with
            | OnBlur -> (nameof OnBlur).ToLowerInvariant()
            | OnChange -> (nameof OnChange).ToLowerInvariant()
            | OnClick -> (nameof OnClick).ToLowerInvariant()
            | OnDblClick -> (nameof OnDblClick).ToLowerInvariant()
            | OnFocus -> (nameof OnFocus).ToLowerInvariant()
            | OnKeyDown -> (nameof OnKeyDown).ToLowerInvariant()
            | OnKeyUp -> (nameof OnKeyUp).ToLowerInvariant()

        member this.PreventDefault =
            on.preventDefault this.toLower true

    let newLine = RawHtml $"{Environment.NewLine}"

    let wrapn indentLevel (nodes : Node list) =
        let numberOfSpaces = 4
        let spaceChar = ' '
        let charArray =
            spaceChar
            |> Array.replicate (numberOfSpaces * indentLevel)
        let indent = String(charArray)

        nodes |> List.collect (fun node -> [newLine; text indent; node])
