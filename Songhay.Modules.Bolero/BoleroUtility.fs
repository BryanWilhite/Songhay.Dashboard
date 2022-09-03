namespace Songhay.Modules.Bolero

open System
open Bolero
open Bolero.Html

module BoleroUtility =

    type CssClasses =
        | CssClasses of string list

        member this.Value = let (CssClasses l) = this in l

        member this.Append s = CssClasses (this.Value |> List.append([s]))

        member this.AppendList (l: string list) = CssClasses (this.Value |> List.append(l))

        member this.Prepend s = CssClasses ([s] |> List.append(this.Value))

        member this.PrependList (l: string list) = CssClasses (l |> List.append(this.Value))

        member this.ToAttributeValue = this.Value |> List.reduce(fun a b -> $"{a} {b}")

        member this.ToHtmlClassAttribute = attr.``class`` this.ToAttributeValue

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

    let toHtmlClass (s: string) = (CssClasses [s]).ToHtmlClassAttribute

    let toHtmlClassFromData (data: CssClasses) = data.ToHtmlClassAttribute

    let toHtmlClassFromList (list: string list) = (CssClasses list).ToHtmlClassAttribute

    let indent level =
        let numberOfSpaces = 4
        let spaceChar = ' '
        let charArray =
            spaceChar
            |> Array.replicate (numberOfSpaces * level)

        String(charArray) |> text

    let wrapn indentLevel (nodes : Node list) =
        nodes |> List.collect (fun node -> [newLine; (indentLevel |> indent); node])
