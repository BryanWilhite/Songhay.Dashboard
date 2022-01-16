namespace Songhay.Modules.Models

open System
open System.IO
open System.Reflection
open System.Text

open Songhay.Modules

type ProgramAssemblyInfo =
    {
        AssemblyCompany: string
        AssemblyCopyright: string
        AssemblyDescription: string
        AssemblyProduct: string
        AssemblyTitle: string
        AssemblyVersion: string
        AssemblyVersionDetail: string
        AssemblyFileName: string
        AssemblyPath: string
    }

    static member fromInput(assembly: Assembly): ProgramAssemblyInfo =
        {
            AssemblyCompany = assembly |> ProgramAssemblyInfo.getAssemblyCompany
            AssemblyCopyright = assembly |> ProgramAssemblyInfo.getAssemblyCopyright
            AssemblyDescription = assembly |> ProgramAssemblyInfo.getAssemblyDescription
            AssemblyProduct = assembly |> ProgramAssemblyInfo.getAssemblyProduct
            AssemblyTitle = assembly |> ProgramAssemblyInfo.getAssemblyTitle
            AssemblyVersion = assembly |> ProgramAssemblyInfo.getAssemblyVersion
            AssemblyVersionDetail = assembly |> ProgramAssemblyInfo.getAssemblyVersionDetail
            AssemblyFileName = assembly |> ProgramAssemblyInfo.getAssemblyFileName
            AssemblyPath = assembly |> ProgramAssemblyInfo.getAssemblyPath
        }

    static member getAssemblyCompany(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyCompanyAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyCompanyAttribute
            attribute.Company
        else String.Empty

    static member getAssemblyCopyright(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyCopyrightAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyCopyrightAttribute
            attribute.Copyright
        else String.Empty

    static member getAssemblyDescription(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyDescriptionAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyDescriptionAttribute
            attribute.Description
        else String.Empty

    static member getAssemblyProduct(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyProductAttribute>, false)
        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyProductAttribute
            attribute.Product
        else String.Empty

    static member getAssemblyTitle(assembly: Assembly) =
        let attributes = assembly.GetCustomAttributes(typeof<AssemblyTitleAttribute>, false)

        if attributes.Length > 0 then
            let attribute = attributes[0] :?> AssemblyTitleAttribute
            let hasTitle = not (String.IsNullOrWhiteSpace(attribute.Title))
            if hasTitle then attribute.Title
            else Path.GetFileNameWithoutExtension(assembly.Location)
        else Path.GetFileNameWithoutExtension(assembly.Location)

    static member getAssemblyVersion(assembly: Assembly) =
        let name = assembly.GetName()
        name.Version.ToString()

    static member getAssemblyVersionDetail(assembly: Assembly) =
        let name = assembly.GetName()
        $"{name.Version.Major:D}.{name.Version.Minor:D2}"

    static member getAssemblyFileName(assembly: Assembly) =
        Path.GetFileName(assembly.Location)

    static member getAssemblyPath(assembly: Assembly) =
        Path.GetDirectoryName(assembly.Location)

    static member getPathFromAssembly path (assembly: Assembly) =
        let pathNormalized =
            path
            |> ProgramFileUtility.ensureRelativePath
            |> ProgramFileUtility.normalizePath

        let root = assembly |> ProgramAssemblyInfo.getAssemblyPath
        let levels = pathNormalized |> ProgramFileUtility.countParentDirectoryChars
        let parentDirectory =
            if (levels > 0) then
                root
                |> ProgramFileUtility.getParentDirectory levels
                |> Option.defaultWith (fun () -> raise (NullReferenceException "The expected parent directory is not here."))
            else
                root

        (parentDirectory, pathNormalized) ||> ProgramFileUtility.getCombinedPath

    member this.getAssemblyInfo =
        StringBuilder()
            .AppendFormat($"{this.AssemblyTitle} {this.AssemblyVersion}")
            .Append(Environment.NewLine)
            .AppendLine(this.AssemblyDescription)
            .AppendLine(this.AssemblyCopyright)
            .ToString()
