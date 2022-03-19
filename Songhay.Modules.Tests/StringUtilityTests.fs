namespace Songhay.Modules.Tests

module StringUtilityTests =

    open System

    open Xunit
    open FsUnit.Xunit

    open Songhay.Modules.StringUtility

    [<Theory>]
    [<InlineData("“How to improve your site’s UX” and other Tweeted Links…", "how-to-improve-your-site-s-ux-and-other-tweeted-links")>]
    [<InlineData("My Angular JS 1.x single-page layout", "my-angular-js-1-x-single-page-layout")>]
    [<InlineData("Put F# on the TODO list?", "put-f-on-the-todo-list")>]
    [<InlineData("Silverlight Page Navigating with MVVM Light Messaging and Songhay NavigationBookmarkData",
        "silverlight-page-navigating-with-mvvm-light-messaging-and-songhay-navigationbookmarkdata")>]
    [<InlineData(null, "None")>]
    [<InlineData("", "None")>]
    [<InlineData("--", "None")>]
    [<InlineData("-----", "None")>]
    [<InlineData("--X--", "x")>]
    let ``toBlogSlug test`` (input: string) (expectedResult: string) =
        match input |> toBlogSlug with
        | Some actual ->
            actual |> should equal expectedResult
        | None ->
            nameof None |> should equal expectedResult

    [<Theory>]
    [<InlineData("OneTwoThree", "one-two-three")>]
    [<InlineData("one-two-three", "one-two-three")>]
    [<InlineData("", "None")>]
    let ``toSnakeCase test`` (input: string) (expectedResult: string) =
        match input |> toSnakeCase with
        | Some actual ->
            actual |> should equal expectedResult
        | None ->
            nameof None |> should equal expectedResult

    [<Fact>]
    let ``tryRegexReplace ArgumentNullException test`` () =
        let test (pattern: string) (replace: string) (input: string) =
            match input |> tryRegexReplace pattern replace defaultRegexOptions with
            | Ok _ -> failwith $"{nameof Result.Ok} is not expected."
            | Error ex -> ex |> should be ofExactType<ArgumentNullException>

        null |> test null null
        "foo" |> test "foo" null
        "foo" |> test null null
        null |> test "foo" null
        null |> test null "foo"
