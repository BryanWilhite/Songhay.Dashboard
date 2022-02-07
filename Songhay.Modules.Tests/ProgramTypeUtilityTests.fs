namespace Songhay.Modules.Tests

module ProgramTypeUtilityTests =

    open System

    open Xunit
    open FsUnit.Xunit

    open Songhay.Modules.ProgramTypeUtility

    [<Theory>]
    [<InlineData("Fri, 22 Mar 2019 18:56:07 -0700", true)>]
    [<InlineData("Sat, 05 Dec 2020 13:30:25 -0800", true)>]
    [<InlineData("2021-06-06T06:21:07Z", false)>]
    [<InlineData("Mon, 24 May 2021 11:50:26 -0700", true)>]
    let ``tryParseRfc822DateTime test`` (input: string) (expectedResult: bool) =
        match tryParseRfc822DateTime input with
        | Ok fromRfc822DateTime ->
            expectedResult |> should be True
            fromRfc822DateTime |> should be (greaterThan DateTime.MinValue)
        | Error ex ->
            expectedResult |> should be False
            ex |> should be ofExactType<FormatException>
