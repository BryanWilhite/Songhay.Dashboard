namespace Songhay.Modules

module StringUtility =

    open System
    open System.Text.RegularExpressions

    let defaultRegexOptions = RegexOptions.IgnoreCase

    let regexReplace (pattern: string) (replace: string) (options: RegexOptions) (input: string) =
        Regex.Replace(input, pattern, replace, options)

    let tryRegexReplace (pattern: string) (replace: string) (options: RegexOptions) (input: string) =
        try
            Ok (input |> regexReplace pattern replace options)
        with
        | ex -> Error ex

    let toLowerInvariant (input: string) =
        if String.IsNullOrWhiteSpace input then None
        else Some (input.ToLowerInvariant())

    let toBlogSlug (input: string) =
        if String.IsNullOrWhiteSpace input then None
        else
            let removeEntities s =
                s
                |> regexReplace @"\&\w+\;" String.Empty defaultRegexOptions
                |> regexReplace @"\&\#\d+\;" String.Empty defaultRegexOptions

            let replaceNonAlphanumericWithHyphen s =
                s
                |> regexReplace "[^a-z^0-9]" "-" defaultRegexOptions

            let rec removeDoubleHyphens s =
                let pattern = "--"
                if Regex.IsMatch(s, pattern) then
                    s
                    |> regexReplace pattern "-" defaultRegexOptions
                    |> removeDoubleHyphens
                else
                    s

            let removeTrailingAndLeadingHyphens s =
                s
                |> regexReplace "^-|-$" String.Empty defaultRegexOptions

            input
                .Replace("&amp;", "and")
                |> removeEntities
                |> replaceNonAlphanumericWithHyphen
                |> removeDoubleHyphens
                |> removeTrailingAndLeadingHyphens
                |> toLowerInvariant

    let toSnakeCase (input: string) =
        if String.IsNullOrWhiteSpace input then None
        else
            let processChar i c =
                if (i > 0) && Char.IsUpper(c) then $"-{Char.ToLower(c)}"
                else $"{Char.ToLower(c)}"
            let stringArray = input.ToCharArray() |> Array.mapi processChar
            Some (String.Join(String.Empty, stringArray))
