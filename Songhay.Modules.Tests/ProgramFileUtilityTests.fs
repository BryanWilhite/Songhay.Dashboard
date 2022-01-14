namespace Songhay.Modules.Tests

module ProgramFileUtilityTests =

    open System.IO
    open Xunit

    open Songhay.Modules

    [<SkippableTheory>]
    [<InlineData("root1", @"z:\one", "z:|one", true)>]
    [<InlineData(@"z:\root1", @"one", "z:|root1|one", true)>]
    [<InlineData("root2", @"/home/one", "root2|home|one", false)>]
    [<InlineData("path1", @"/two/three/four/", "path1|two|three|four|", false)>]
    [<InlineData("path2", @"\two\three\four", "path2|two|three|four", false)>]
    let ``getCombinedPath test`` (root, path, expectedResult: string, requiresWindows) =

        Skip.If(requiresWindows && ProgramFileUtility.isForwardSlashSystem, "OS is not Windows")

        let actual = (root, path) ||> ProgramFileUtility.getCombinedPath
        Assert.Equal(expectedResult.Replace('|', Path.DirectorySeparatorChar), actual)
