$ErrorActionPreference='Stop'
<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

function Clean-Solution {
  Write-Host "Cleaning" -ForegroundColor "magenta"
  exec { & dotnet clean --configuration $env:CONFIGURATION }
}
function Restore-Solution {
  Write-Host "Restore" -ForegroundColor "magenta"
  exec { & dotnet restore --no-cache --source "https://api.nuget.org/v3/index.json" --source "https://www.myget.org/F/katalizr/api/v3/index.json" }
}
function Build-Solution {
  Write-Host "Build" -ForegroundColor "magenta"
  exec { & dotnet build -c $env:CONFIGURATION "$env:APPVEYOR_PROJECT_NAME.sln" }
}
function Pack-Solution{
  Write-Host "Pack" -ForegroundColor "magenta"
  exec { & dotnet pack -c $env:CONFIGURATION --include-symbols --no-build -o ..\artifacts "$env:APPVEYOR_PROJECT_NAME\$env:APPVEYOR_PROJECT_NAME.csproj" }
}


function Test-Project{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=0)][string]$ProjectName
    )
    Push-Location  -Path "./$ProjectName"
    exec { & dotnet test --no-build }
    Pop-Location
}

function Test-Solution{
    Write-Host "Testing" -ForegroundColor "magenta"
    Get-ChildItem -Filter *.Tests | ForEach-Object {Test-Project -ProjectName $_.Name}
}

Clean-Solution
Restore-Solution
Build-Solution
Test-Solution
Pack-Solution
