namespace Songhay.Modules

module JsonDocumentUtility =

    open System.Linq
    open System.Text.Json

    let resultError (elementName: string) =
        Error(JsonException $"the expected `{elementName}` element is not here")

    let toPropertyName (element: JsonElement) =
        if element.ValueKind <> JsonValueKind.Object then None
        else
            try
                Some (element.EnumerateObject().First().Name)
            with | _ -> None

    let tryGetProperty (elementName: string) (element: JsonElement) =
        match element.TryGetProperty elementName with
        | false, _ -> resultError elementName
        | true, el -> Ok el

    let tryGetRootElement (rawDocument: string) =
        try
            let document = rawDocument |> JsonDocument.Parse
            Ok document.RootElement
        with | exn -> Error exn
