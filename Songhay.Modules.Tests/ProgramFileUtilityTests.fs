namespace Songhay.Modules.Tests

module ProgramFileUtilityTests =

    open System.IO
    open System.Linq
    open System.Reflection
    open Xunit
    open FsUnit.Xunit
    open FsToolkit.ErrorHandling

    open Songhay.Modules.Models
    open Songhay.Modules.ProgramFileUtility

    [<SkippableTheory>]
    [<InlineData("root1", @"z:\one", "z:|one", true)>]
    [<InlineData(@"z:\root1", @"one", "z:|root1|one", true)>]
    [<InlineData("root2", @"/home/one", "root2|home|one", false)>]
    [<InlineData("path1", @"/two/three/four/", "path1|two|three|four|", false)>]
    [<InlineData("path2", @"\two\three\four", "path2|two|three|four", false)>]
    let ``getCombinedPath test`` (root, path, expectedResult: string, requiresWindows) =

        Skip.If(requiresWindows && isForwardSlashSystem, "OS is not Windows")

        let actual = (root, path) ||> getCombinedPath |> Result.valueOr raiseProgramFileError

        let expected =
            expectedResult.Replace('|', Path.DirectorySeparatorChar)

        expected |> should equal actual

    [<Fact>]
    let ``getParentDirectory test`` () =
        let assembly = Assembly.GetExecutingAssembly()

        let expected =
            assembly
            |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
            |> Result.valueOr raiseProgramFileError

        let actual =
            assembly.Location
            |> getParentDirectory 4
            |> Result.valueOr raiseProgramFileError

        expected |> should equal actual

    [<Fact>]
    let ``getParentDirectoryInfo test`` () =
        let assembly = Assembly.GetExecutingAssembly()

        let expected =
            assembly
            |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
            |> Result.valueOr raiseProgramFileError

        let actual =
            assembly.Location
            |> getParentDirectoryInfo 4
            |> Result.valueOr raiseProgramFileError

        expected |> should equal actual.FullName

    [<Theory>]
    [<InlineData("./one/", "one|")>]
    [<InlineData("../../one/", "one|")>]
    [<InlineData(@".\one/two\", @"one|two|")>]
    [<InlineData(@"..\..\one\", @"one|")>]
    let ``getRelativePath test`` (input, expectedResult: string) =
        let actual =
            input
            |> getRelativePath
            |> Result.valueOr raiseProgramFileError

        let expected =
            expectedResult.Replace('|', Path.DirectorySeparatorChar)

        expected |> should equal actual

    [<Fact>]
    let ``raiseExceptionForExpectedDirectory test`` () =
        let assemblyInfo =
            Assembly.GetExecutingAssembly()
            |> ProgramAssemblyInfo.fromInput

        let dirInfo = DirectoryInfo(assemblyInfo.AssemblyPath)

        let action =
            fun () ->
                $"{dirInfo.GetDirectories().First().FullName}.fubar"
                |> raiseExceptionForExpectedDirectory
                |> ignore

        action
        |> should throw typeof<DirectoryNotFoundException>

    [<Fact>]
    let ``raiseExceptionForExpectedFile test`` () =
        let assemblyInfo =
            Assembly.GetExecutingAssembly()
            |> ProgramAssemblyInfo.fromInput

        let dirInfo = DirectoryInfo(assemblyInfo.AssemblyPath)

        let action =
            fun () ->
                $"{dirInfo.GetFiles().First().FullName}.fubar"
                |> raiseExceptionForExpectedFile
                |> ignore

        action
        |> should throw typeof<FileNotFoundException>

    [<Theory>]
    [<InlineData(@"/\foo\bar\my-file.json", @"foo\bar\my-file.json")>]
    let ``trimLeadingDirectorySeparatorChars test`` (path, expected: string) =
        let actual =
            path |> trimLeadingDirectorySeparatorChars

        expected |> should equal actual
