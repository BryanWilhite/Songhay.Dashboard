module Songhay.Modules.Bolero.JsRuntimeUtility

open System
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop

open FsToolkit.ErrorHandling

open Bolero

[<Literal>]
let rx = "rx"

[<Literal>]
let CssUtility = "CssUtility"

[<Literal>]
let DomUtility = "DomUtility"

let tryGetElementReference (htmlRef: HtmlRef) =
    match htmlRef.Value with
    | Some elementRef -> Ok elementRef
    | _ -> Error (FormatException "The expected HTML element reference is not here.")

let consoleLogAsync (jsRuntime: IJSRuntime) ([<ParamArray>] args: obj[]) =
    jsRuntime.InvokeVoidAsync("console.log", args).AsTask()

let getComputedStylePropertyValueAsync (htmlRef: HtmlRef) (propertyName: string) (jsRuntime: IJSRuntime) =
    let elementRef = htmlRef |> tryGetElementReference |> Result.valueOr raise
    jsRuntime.InvokeAsync<string>($"{rx}.{CssUtility}.getComputedStylePropertyValue", elementRef, propertyName).AsTask()

let getComputedStylePropertyValueByIdAsync (elementId: string) (propertyName: string) (jsRuntime: IJSRuntime) =
    jsRuntime.InvokeAsync<string>($"{rx}.{CssUtility}.getComputedStylePropertyValueById", elementId, propertyName).AsTask()

let getComputedStylePropertyValueByQueryAsync (query: string) (propertyName: string) (jsRuntime: IJSRuntime) =
    jsRuntime.InvokeAsync<string>($"{rx}.{CssUtility}.getComputedStylePropertyValueByQuery", query, propertyName).AsTask()

let setComputedStylePropertyValueAsync (htmlRef: HtmlRef) (propertyName: string) (propertyValue: string) (jsRuntime: IJSRuntime) =
    let elementRef = htmlRef |> tryGetElementReference |> Result.valueOr raise
    jsRuntime.InvokeVoidAsync($"{rx}.{CssUtility}.setComputedStylePropertyValue", elementRef, propertyName, propertyValue).AsTask()
