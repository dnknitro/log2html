[CmdletBinding()]
Param(
    [switch]$NoInit,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { $host.SetShouldExit(1) }
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

###########################################################################
# CONFIGURATION
###########################################################################

$NuGetVersion = "latest"
$SolutionDirectory = "$PSScriptRoot\src"
$BuildProjectFile = "$PSScriptRoot\.\build\dnkLog4netHtmlReport.build.csproj"
$BuildExeFile = "$PSScriptRoot\.\build\bin\debug\dnkLog4netHtmlReport.build.exe"

$TempDirectory = "$PSScriptRoot\.\.tmp"

$NuGetUrl = "https://dist.nuget.org/win-x86-commandline/$NuGetVersion/nuget.exe"
$NuGetFile = "$TempDirectory\nuget.exe"
$env:NUGET_EXE = $NuGetFile

###########################################################################
# PREPARE BUILD
###########################################################################

function ExecSafe([scriptblock] $cmd) {
    & $cmd
    if ($LastExitCode -ne 0) { throw "The following call failed with exit code $LastExitCode. '$cmd'" }
}

if (!$NoInit) {
    md -force $TempDirectory > $null

    if (!(Test-Path $NuGetFile)) { (New-Object System.Net.WebClient).DownloadFile($NuGetUrl, $NuGetFile) }
    elseif ($NuGetVersion -eq "latest") { & $NuGetFile update -Self }

    ExecSafe { & $NuGetFile restore $BuildProjectFile -SolutionDirectory $SolutionDirectory }
    ExecSafe { & $NuGetFile install Nuke.MSBuildLocator -ExcludeVersion -OutputDirectory $TempDirectory -SolutionDirectory $SolutionDirectory }
}

$MSBuildFile = & "$TempDirectory\Nuke.MSBuildLocator\tools\Nuke.MSBuildLocator.exe"
ExecSafe { & $MSBuildFile $BuildProjectFile /v:minimal /t:Build /nodeReuse:True }

###########################################################################
# EXECUTE BUILD
###########################################################################

& $BuildExeFile $BuildArguments
exit $LASTEXITCODE
