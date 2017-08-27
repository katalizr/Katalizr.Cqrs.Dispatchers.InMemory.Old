<#
.SYNOPSIS
This function converts the provided Pascal cased name to its kebab case equivalent. (e.g. ItemList is converted to item-list)
#>

function ConvertTo-KebabCase {
	param (
		[string]$Name
	)

	if ($Name -eq $null) {
		return $null
  }

	$parts = @()
	$part = ""
	for ($i = 0; $i -lt $Name.Length; $i++) {
		if ([char]::IsUpper($Name[$i]) -and $part.Length -gt 0) {
			$parts += $part.ToLower()
			$part = ""
    }

    if([char]::IsLetterOrDigit($Name[$i])){
      $part += $Name[$i]
    }else{
      if(!$part.EndsWith("-")){
        $part +="-"
      }
    }

	}
	if ($part.Length) {
		$parts += $part.ToLower();
	}

	[string]::Join("-", $parts);
}

$env:version_major = Get-Date -format yyyy
$env:version_minor = Get-Date -format MM
$env:version_build = Get-Date -format dd
$env:version_revision = ([math]::round([int](Get-Date -format hhmmssfff) * 65534 / 235959999,0))
$env:version =  "$env:version_major.$env:version_minor.$env:version_build.$env:version_revision"
$env:author =  [string]::Join(",",$(git log --format='%an' | sort -u))
$env:branch = ConvertTo-KebabCase -Name $env:APPVEYOR_REPO_BRANCH
Update-AppveyorBuild -Version $env:version

if ($env:APPVEYOR_REPO_BRANCH -ne "master"){
  $env:version = "$env:Version-$env:branch"
}