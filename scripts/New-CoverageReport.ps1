$tools = dotnet tool list -g
$isReportGeneratorInstalled = $tools -like '*dotnet-reportgenerator-globaltool*'
if ($isReportGeneratorInstalled) {
	dotnet tool update dotnet-reportgenerator-globaltool -g
} else {
	dotnet tool install dotnet-reportgenerator-globaltool -g
}

$currentDir = Get-Location
$rootDir = "$PSScriptRoot\.."
$slnDir = "$rootDir\src"
$reportDir = "$rootDir\CoverageReport"
$testResultDirs = "$rootDir\*\TestResults"


if (Test-Path $reportDir) {
	Remove-Item $reportDir -Recurse
}

Get-ChildItem $slnDir -Recurse -File -Filter *.Tests.csproj | ForEach-Object {
	$csprojPath = $_.FullName
	Invoke-Expression "dotnet test $csprojPath --collect:""XPlat Code Coverage"" /p:CoverletOutputFormat=cobertura" | Out-Null
}

$reportList = (Get-ChildItem $slnDir -Recurse -File -Filter *coverage.cobertura.xml).FullName -join ";"
Invoke-Expression "ReportGenerator -reporttypes:Html -classfilters: -assemblyfilters: ""-targetdir:$reportDir"" ""-reports:$reportList"""


& $reportDir\index.html

Set-Location $currentDir
