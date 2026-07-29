$ErrorActionPreference = 'Stop'

$sourceFolder = (Get-Item $PSScriptRoot).FullName
$projectFile = Join-Path $sourceFolder 'VAR.Toolbox\VAR.Toolbox.csproj'
$publishDir = Join-Path $sourceFolder 'publish'

Write-Host "Publishing VAR.Toolbox as self-contained..."
Write-Host "  Project: $projectFile"
Write-Host "  Publishing to: $publishDir"
Write-Host ""

# Remove existing publish directory
if (Test-Path $publishDir) {
    Write-Host "Removing existing publish directory..."
    Remove-Item $publishDir -Recurse -Force
}

# Publish self-contained for Windows x64
dotnet publish $projectFile `
    -c Release `
    -r win-x64 `
    -p:SelfContained=true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:PublishReadyToRun=false `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    Write-Host "Publish failed!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Published successfully to: $publishDir" -ForegroundColor Green

$publishFolder = Get-ChildItem $publishDir -File | Where-Object { $_.Name -like '*.exe' }
if ($publishFolder) {
    Write-Host "  Executable: $($publishFolder.FullName)" -ForegroundColor Cyan
}
