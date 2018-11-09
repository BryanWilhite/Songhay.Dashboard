trap 
{ 
    Write-Output $_ 
    exit 1 
}

Set-Location $PSScriptRoot

& dotnet Songhay.Dashboard.Shell.dll AppDataActivity --server-assembly-file 'D:\home\site\wwwroot\Songhay.Dashboard.dll'
