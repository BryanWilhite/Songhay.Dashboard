namespace Songhay.Player.YouTube.Models

open Songhay.Modules.StringUtility

/// <summary>
/// styling variables for this App
/// </summary>
type YouTubeCssVariable =
    | ThumbsHeaderLinkColor of string
    | ThumbsHeaderLinkTextDecoration of string
    | ThumbsSetHeaderColor of string
    | ThumbsSetBackgroundColor of string
    | ThumbsSetHeaderPosition of string
    | ThumbsSetPaddingTop of string

    member this.Name =
        let name =
            match this with
            | ThumbsHeaderLinkColor _ -> nameof ThumbsHeaderLinkColor |> toKabobCase
            | ThumbsHeaderLinkTextDecoration _ -> nameof ThumbsHeaderLinkTextDecoration |> toKabobCase
            | ThumbsSetHeaderColor _ -> nameof ThumbsSetHeaderColor |> toKabobCase
            | ThumbsSetBackgroundColor _ -> nameof ThumbsSetBackgroundColor |> toKabobCase
            | ThumbsSetHeaderPosition _ -> nameof ThumbsSetHeaderPosition |> toKabobCase
            | ThumbsSetPaddingTop _ -> nameof ThumbsSetPaddingTop |> toKabobCase
        $"--{name}"

    member this.Value =
        match this with
        | ThumbsHeaderLinkColor v -> v
        | ThumbsHeaderLinkTextDecoration v -> v
        | ThumbsSetHeaderColor v -> v
        | ThumbsSetBackgroundColor v -> v
        | ThumbsSetHeaderPosition v -> v
        | ThumbsSetPaddingTop v -> v

    member this.Pair =
        let n = this.Name
        let v = this.Value
        n, v