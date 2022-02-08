namespace Songhay.Modules.Tests

module StringUtilityTests =

    open Xunit
    open FsUnit.Xunit

    open Songhay.Modules.StringUtility

    [<Theory>]
    [<InlineData("“How to improve your site’s UX” and other Tweeted Links…", "how-to-improve-your-site-s-ux-and-other-tweeted-links")>]
    [<InlineData("My Angular JS 1.x single-page layout", "my-angular-js-1-x-single-page-layout")>]
    [<InlineData("Put F# on the TODO list?", "put-f-on-the-todo-list")>]
    [<InlineData("Silverlight Page Navigating with MVVM Light Messaging and Songhay NavigationBookmarkData",
        "silverlight-page-navigating-with-mvvm-light-messaging-and-songhay-navigationbookmarkdata")>]
    let ``toBlogSlug test`` (input: string) (expectedResult: string) =
        match input |> toBlogSlug with
        | Some actual ->
            actual |> should equal expectedResult
        | None ->
            failwith "`None` is not expected."
