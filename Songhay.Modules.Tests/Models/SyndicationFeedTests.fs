namespace Songhay.Modules.Models.Tests

module SyndicationFeedTests =

    open System.IO
    open System.Reflection
    open System.Text.Json
    open Xunit

    open Songhay.Modules
    open Songhay.Modules.Models

    let projectDirectoryInfo =
        Assembly.GetExecutingAssembly()
        |> ProgramAssemblyInfo.getPathFromAssembly "../../../"
        |> DirectoryInfo

    let appJsonDocumentPath =
        "./json/app.json"
        |> ProgramFileUtility.getCombinedPath projectDirectoryInfo.FullName

    let appJsonDocument = JsonDocument.Parse(File.ReadAllText(appJsonDocumentPath))

    [<Fact>]
    let ``app.json should have `feeds` property`` () =
        let result =
            match appJsonDocument.RootElement.TryGetProperty "feeds" with
            | true, _ -> true
            | _ -> false

        Assert.True(result)
