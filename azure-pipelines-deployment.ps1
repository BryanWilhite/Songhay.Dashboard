$job =
@{
    Name = "job-studio-dash";
    SubscriptionName = "Songhay System";
    WebSiteName = "songhay-system";
    ZipFile = "$env:AGENT_RELEASEDIRECTORY\$env:BUILD_DEFINITIONNAME\drop\Songhay.Dashboard.Shell.zip";
}

Get-AzureSubscription -SubscriptionName $job.SubscriptionName
Select-AzureSubscription -SubscriptionName $job.SubscriptionName

New-AzureWebsiteJob `
    -Name $job.WebSiteName `
    -JobName $job.Name `
    -JobType Triggered `
    -JobFile $job.ZipFile
