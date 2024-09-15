Set-Location $PSScriptRoot

$p = Start-Process dotnet -ArgumentList "Songhay.Dashboard.Shell.dll AppDataActivity" -NoNewWindow -PassThru -Wait

exit $p.ExitCode
