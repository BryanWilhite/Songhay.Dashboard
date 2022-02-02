namespace Songhay.Modules.Models.Tests

module ProgramAssemblyInfoTests =

    open System.IO
    open System.Reflection
    open Xunit
    open FsUnit.Xunit
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.ProgramFileUtility

    [<Theory>]
    [<InlineData(@"..\..\..\Models\ProgramAssemblyInfoTests.fs")>]
    [<InlineData("../../../Models/ProgramAssemblyInfoTests.fs")>]
    let ``getPathFromAssembly test`` (path: string) =
        let actual =
            Assembly.GetExecutingAssembly()
            |> ProgramAssemblyInfo.getPathFromAssembly path
            |> Result.valueOr raiseProgramFileError
            |> File.Exists

        actual |> should be True
