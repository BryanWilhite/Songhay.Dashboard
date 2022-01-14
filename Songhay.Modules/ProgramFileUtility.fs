namespace Songhay.Modules

module ProgramFileUtility =

    open System
    open System.IO
    open System.Text.RegularExpressions

    let backSlash = '\\'
    let forwardSlash = '/'
    let isForwardSlashSystem = Path.DirectorySeparatorChar.Equals(forwardSlash)

    let countParentDirectoryChars path =
        if String.IsNullOrWhiteSpace path then Unchecked.defaultof<int>
        else
            let parentDirectoryCharsPattern = @$"\.\.\{Path.DirectorySeparatorChar}"
            let matches = Regex.Matches(path, parentDirectoryCharsPattern)
            matches.Count

    let rec findParentDirectoryInfo parentName levels path =
        if (String.IsNullOrWhiteSpace path) then raise (DirectoryNotFoundException "The expected directory is not here.")
        else
            let info = new DirectoryInfo(path)

            let doesNotExist = (not info.Exists)
            let isParent = (info.Name = parentName)
            let hasNullParent = (info.Parent = null)
            let hasTargetParent = not hasNullParent && (info.Parent.Name = parentName)

            match info with
            | _ when doesNotExist || hasNullParent -> None
            | _ when isParent -> Some info
            | _ when hasTargetParent -> Some info.Parent
            | _ ->
                let nextLevels = (abs levels) - 1
                let hasNoMoreLevels = (nextLevels = 0)

                if hasNoMoreLevels then None
                else (parentName, nextLevels, info.Parent.FullName) |||> findParentDirectoryInfo

    let rec getParentDirectoryInfo levels path =
        if (String.IsNullOrWhiteSpace(path)) then None
        else
            let info = new DirectoryInfo(path)

            let isLevelZero = (levels = 0)
            let hasNoParent = (info.Parent = null)

            if isLevelZero || hasNoParent then Some info
            else
                let nextLevels = (abs levels) - 1

                if (nextLevels >= 1) then (nextLevels, info.FullName) ||> getParentDirectoryInfo
                else Some info

    let getParentDirectory levels path =
        let info = (levels, path) ||> getParentDirectoryInfo
        info |> Option.map (fun i -> i.FullName)

    let normalizePath path =
        if (String.IsNullOrWhiteSpace(path)) then path
        elif isForwardSlashSystem  then path.Replace(backSlash, forwardSlash)
        else path.Replace(forwardSlash, backSlash)

    let raiseExceptionForExpectedDirectory path =
        if (String.IsNullOrWhiteSpace(path)) then raise (DirectoryNotFoundException $"The expected directory is not here.")
        elif File.Exists(path) then path
        else raise (DirectoryNotFoundException $"The expected directory, `{path}`, is not here.")

    let raiseExceptionForExpectedFile path =
        if (String.IsNullOrWhiteSpace(path)) then raise (FileNotFoundException $"The expected file is not here.")
        elif File.Exists(path) then path
        else raise (FileNotFoundException $"The expected file, `{path}`, is not here.")

    let  removeBackslashPrefixes (path: string) =
        if String.IsNullOrWhiteSpace(path) then path
        else
            path
                .TrimStart(backSlash)
                .Replace($"..{backSlash}", String.Empty)
                .Replace($".{backSlash}", String.Empty)

    let removeForwardslashPrefixes (path: string) =
        if String.IsNullOrWhiteSpace(path) then path
        else
            path
                .TrimStart(forwardSlash)
                .Replace($"..{forwardSlash}", String.Empty)
                .Replace($".{forwardSlash}", String.Empty)

    let removeConventionalPrefixes path =
        if String.IsNullOrWhiteSpace(path) then path
        elif isForwardSlashSystem then removeForwardslashPrefixes path
        else removeBackslashPrefixes path

    let trimLeadingDirectorySeparatorChars path =
        if String.IsNullOrWhiteSpace(path) then path
        else path.TrimStart(backSlash, forwardSlash)

    let getRelativePath fileSegment =
        if (String.IsNullOrWhiteSpace(fileSegment)) then raise (ArgumentNullException (nameof fileSegment))

        fileSegment
            |> trimLeadingDirectorySeparatorChars
            |> normalizePath
            |> removeBackslashPrefixes
            |> removeForwardslashPrefixes

    let getCombinedPath root path =
        if (String.IsNullOrWhiteSpace(root)) then raise (NullReferenceException $"The expected {nameof(root)} is not here.")
        if (String.IsNullOrWhiteSpace(path)) then raise (NullReferenceException $"The expected {nameof(path)} is not here.")

        let relativePath = path |> getRelativePath

        if Path.IsPathRooted(relativePath) then relativePath
        else Path.Combine(normalizePath(root), relativePath)
