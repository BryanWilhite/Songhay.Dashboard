namespace Songhay.Modules

module ProgramFileUtility =

    open System
    open System.IO
    open System.Text.RegularExpressions

    type ProgramFileError = ProgramFileError of exn

    let backSlash = '\\'
    let forwardSlash = '/'
    let isForwardSlashSystem = Path.DirectorySeparatorChar.Equals(forwardSlash)

    let countParentDirectoryChars path =
        if String.IsNullOrWhiteSpace path then Unchecked.defaultof<int>
        else
            let parentDirectoryCharsPattern = @$"\.\.\{Path.DirectorySeparatorChar}"
            let matches = Regex.Matches(path, parentDirectoryCharsPattern)
            matches.Count

    let rec tryFindParentDirectoryInfo parentName levels path =
        if (String.IsNullOrWhiteSpace path) then
            Error (ProgramFileError (DirectoryNotFoundException "The expected directory is not here."))
        else
            let info = DirectoryInfo(path)

            let doesNotExist = (not info.Exists)
            let isParent = (info.Name = parentName)
            let hasNullParent = (info.Parent = null)
            let hasTargetParent = not hasNullParent && (info.Parent.Name = parentName)

            match info with
            | _ when doesNotExist || hasNullParent ->
                Error (ProgramFileError (DirectoryNotFoundException "Directory does not exist."))
            | _ when isParent -> Ok info
            | _ when hasTargetParent -> Ok info.Parent
            | _ ->
                let nextLevels = (abs levels) - 1
                let hasNoMoreLevels = (nextLevels = 0)

                if hasNoMoreLevels then Error (ProgramFileError (DirectoryNotFoundException "Has no more levels"))
                else (parentName, nextLevels, info.Parent.FullName) |||> tryFindParentDirectoryInfo

    let rec tryGetParentDirectoryInfo levels path =
        if (String.IsNullOrWhiteSpace(path)) then
            Error (ProgramFileError (DirectoryNotFoundException "The expected path is not here."))
        else
            let info = DirectoryInfo(path)
            match levels with
            | _ when (abs levels) = 0 -> Ok info
            | _ ->
                match info with
                | _ when info = null -> Ok info
                | _ ->
                    let nextLevels = levels - 1
                    match nextLevels with
                    | _ when nextLevels >= 1 -> tryGetParentDirectoryInfo nextLevels info.Parent.FullName
                    | _ -> Ok info.Parent

    let rec tryGetParentDirectory levels path =
        if (String.IsNullOrWhiteSpace(path)) then Error (ProgramFileError (NullReferenceException "The expected path is not here."))
        else
            match levels with
            | _ when (abs levels) = 0 -> Ok path
            | _ ->
                let info = Directory.GetParent(path)
                match info with
                | _ when info = null -> Ok path
                | _ ->
                    let nextLevels = levels - 1
                    match nextLevels with
                    | _ when nextLevels >= 1 -> tryGetParentDirectory nextLevels info.FullName
                    | _ -> Ok info.FullName

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

    let raiseProgramFileError (ProgramFileError e) = raise e

    let  removeBackslashPrefixes (path: string) =
        if String.IsNullOrWhiteSpace(path) then path
        else
            path
                .TrimStart(backSlash)
                .Replace($"..{backSlash}", String.Empty)
                .Replace($".{backSlash}", String.Empty)

    let removeForwardSlashPrefixes (path: string) =
        if String.IsNullOrWhiteSpace(path) then path
        else
            path
                .TrimStart(forwardSlash)
                .Replace($"..{forwardSlash}", String.Empty)
                .Replace($".{forwardSlash}", String.Empty)

    let removeConventionalPrefixes path =
        if String.IsNullOrWhiteSpace(path) then path
        elif isForwardSlashSystem then removeForwardSlashPrefixes path
        else removeBackslashPrefixes path

    let trimLeadingDirectorySeparatorChars path =
        if String.IsNullOrWhiteSpace(path) then path
        else path.TrimStart(backSlash, forwardSlash)

    let tryRelativePath path =
        if (String.IsNullOrWhiteSpace(path)) then
            Error (ProgramFileError (NullReferenceException "The expected path is not here."))
        else
            path
                |> trimLeadingDirectorySeparatorChars
                |> fun p ->
                    if Path.IsPathRooted(p) then
                        Error (ProgramFileError (FormatException "The expected relative path is not here."))
                    else Ok p

    let tryGetRelativePath path =
        if (String.IsNullOrWhiteSpace(path)) then
            Error (ProgramFileError (NullReferenceException "The expected path is not here."))
        else
            path
                |> trimLeadingDirectorySeparatorChars
                |> normalizePath
                |> removeBackslashPrefixes
                |> removeForwardSlashPrefixes
                |> Ok

    let tryGetCombinedPath root path =
        if (String.IsNullOrWhiteSpace(root)) then
            Error (ProgramFileError (NullReferenceException $"The expected {nameof(root)} is not here."))
        else if (String.IsNullOrWhiteSpace(path)) then
            Error (ProgramFileError (NullReferenceException $"The expected {nameof(path)} is not here."))
        else
            let relativePathResult = path |> tryGetRelativePath
            match relativePathResult with
            | Error err -> Error err
            | Ok relativePath ->
                if Path.IsPathRooted(relativePath) then Ok relativePath
                else Ok (Path.Combine(normalizePath(root), relativePath))
