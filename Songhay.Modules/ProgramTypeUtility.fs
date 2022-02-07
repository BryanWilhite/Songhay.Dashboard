namespace Songhay.Modules

module ProgramTypeUtility =

    open System
    open System.Globalization

    let tryParseRfc822DateTime (value: string): Result<DateTime, exn> =
        if(String.IsNullOrWhiteSpace(value)) then Error (NullReferenceException "The expected date-time value is not here.")
        else
            let dateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat
            let formats = [|
                dateTimeFormat.RFC1123Pattern
                "ddd',' d MMM yyyy HH:mm:ss zzz"
                "ddd',' dd MMM yyyy HH:mm:ss zzz"
            |]
            match DateTime.TryParseExact (value, formats, dateTimeFormat, DateTimeStyles.AssumeUniversal) with
            | false, _ -> Error (FormatException "The expected date-time format is not here.")
            | true, dateTimeValue -> Ok dateTimeValue
