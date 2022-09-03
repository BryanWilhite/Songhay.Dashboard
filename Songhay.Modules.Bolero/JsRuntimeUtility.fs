namespace Songhay.Modules.Bolero

open System
open Microsoft.JSInterop

open FsToolkit.ErrorHandling

open Bolero

module JsRuntimeUtility =

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

    ///<summary>
    /// Calls the methods of the JavaScript <c>console</c> object
    /// via the <see cref="IJSRuntime.InvokeVoidAsync" /> method.
    /// See  https://developer.mozilla.org/en-US/docs/Web/API/console ]
    ///</summary>
    /// <remarks>
    /// The specified <c>args</c> object array has its elements converted to strings
    /// before it is passed to the <see cref="IJSRuntime.InvokeVoidAsync" /> method.
    /// </remarks>
    let callConsoleMethodAsync (methodName: string) (args: obj[]) (jsRuntime: IJSRuntime) =
        let toStringArray (oArray: obj[]) =
            oArray |> Array.map(fun o -> if o :? string then o else $"{o}" )

        jsRuntime.InvokeVoidAsync($"console.{methodName}", args |> toStringArray).AsTask()

    ///<summary>
    /// Calls the <c>console.debug</c> method
    /// via the <see cref="callConsoleMethodAsync" /> function.
    /// See [ https://developer.mozilla.org/en-US/docs/Web/API/console/debug ]
    ///</summary>
    let consoleDebugAsync (args: obj[]) (jsRuntime: IJSRuntime) = jsRuntime |> callConsoleMethodAsync "debug" args

    ///<summary>
    /// Calls the <c>console.error</c> method
    /// via the <see cref="callConsoleMethodAsync" /> function.
    /// See [ https://developer.mozilla.org/en-US/docs/Web/API/console/error ]
    ///</summary>
    let consoleErrorAsync (args: obj[]) (jsRuntime: IJSRuntime) = jsRuntime |> callConsoleMethodAsync "error" args

    ///<summary>
    /// Calls the <c>console.info</c> method
    /// via the <see cref="callConsoleMethodAsync" /> function.
    /// See [ https://developer.mozilla.org/en-US/docs/Web/API/console/info ]
    ///</summary>
    let consoleInfoAsync (args: obj[]) (jsRuntime: IJSRuntime) = jsRuntime |> callConsoleMethodAsync "info" args

    ///<summary>
    /// Calls the <c>console.log</c> method
    /// via the <see cref="callConsoleMethodAsync" /> function.
    /// See [ https://developer.mozilla.org/en-US/docs/Web/API/console/log ]
    ///</summary>
    let consoleLogAsync (args: obj[]) (jsRuntime: IJSRuntime) = jsRuntime |> callConsoleMethodAsync "log" args

    ///<summary>
    /// Calls the <c>console.warn</c> method
    /// via the <see cref="callConsoleMethodAsync" /> function.
    /// See [ https://developer.mozilla.org/en-US/docs/Web/API/console/warn ]
    ///</summary>
    let consoleWarnAsync (args: obj[]) (jsRuntime: IJSRuntime) = jsRuntime |> callConsoleMethodAsync "warn" args

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
