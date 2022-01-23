namespace Songhay.Modules.Models.Tests

module ProgramAssemblyInfoTests =

    open System.IO
    open System.Reflection
    open Xunit
    open FsUnit.Xunit

    open Songhay.Modules.Models

    [<Theory>]
    [<InlineData(@"..\..\..\Models\ProgramAssemblyInfoTests.fs")>]
    [<InlineData("../../../Models/ProgramAssemblyInfoTests.fs")>]
    let ``getPathFromAssembly test`` (path: string) =
        let actual =
            Assembly.GetExecutingAssembly()
            |> ProgramAssemblyInfo.getPathFromAssembly path
            |> File.Exists

        actual |> should be True
