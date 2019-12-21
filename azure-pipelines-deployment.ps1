#Resource details :
$resourceGroupName = "songhay-system-resources";
$webAppName = "songhay-system"
$webJobName = "job-studio-dash"
$webJobType="triggeredwebjobs"
$deploymentZipFileName="Songhay.Dashboard.Shell.zip"
$Apiversion = "2015-08-01"

#Function to get Publishing credentials for the WebApp :
function Get-PublishingProfileCredentials($resourceGroupName, $webAppName) {

    $resourceType = "Microsoft.Web/sites/config"
    $resourceName = "$webAppName/publishingcredentials"
    $publishingCredentials = Invoke-AzureRmResourceAction `
        -ResourceGroupName $resourceGroupName `
        -ResourceType $resourceType `
        -ResourceName $resourceName `
        -Action list `
        -ApiVersion $Apiversion `
        -Force

    return $publishingCredentials
}

#Pulling authorization access token :
function Get-KuduApiAuthorisationHeaderValue($resourceGroupName, $webAppName) {

    $publishingCredentials = Get-PublishingProfileCredentials $resourceGroupName $webAppName
    return ("Basic {0}" -f [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f 
                    $publishingCredentials.Properties.PublishingUserName, $publishingCredentials.Properties.PublishingPassword))))
}

Write-Host "Preparing to publish WebJob $webJobName..."

Write-Host "Getting KUDU access token..."
$accessToken = Get-KuduApiAuthorisationHeaderValue $resourceGroupName $webAppname

#Generating header to create and publish the Webjob :
$Header = @{
    'Content-Disposition' = 'attachment; attachment; filename=$deploymentZipFileName'
    'Authorization'       = $accessToken
}

$apiUrl = "https://$webAppName.scm.azurewebsites.net/api/$webJobType/$webJobName"

Write-Host "Calling ``$apiUrl``..."
Invoke-RestMethod `
    -Uri $apiUrl `
    -Headers $Header `
    -Method put `
    -InFile "$env:AGENT_RELEASEDIRECTORY\$env:BUILD_DEFINITIONNAME\drop\$deploymentZipFileName" `
    -ContentType 'application/zip'

<#
    📚 https://github.com/projectkudu/kudu/wiki/Deploying-a-WebJob-using-PowerShell-ARM-Cmdlets
#>