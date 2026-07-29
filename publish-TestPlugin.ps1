$ErrorActionPreference = 'Stop'

$sourceFolder = (Get-Item $PSScriptRoot).FullName
$toolboxProject = Join-Path $sourceFolder 'VAR.Toolbox\VAR.Toolbox.csproj'
$pluginProject = Join-Path $sourceFolder 'VAR.Toolbox.TestPlugin\VAR.Toolbox.TestPlugin.csproj'
$publishDir = Join-Path $sourceFolder 'publish-TestPlugin'

Write-Host "Publishing VAR.Toolbox.TestPlugin..."
Write-Host ""

# Remove existing publish directory
if (Test-Path $publishDir) {
    Write-Host "Removing existing publish directory..."
    Remove-Item $publishDir -Recurse -Force
}

# Publish plugin
dotnet publish $pluginProject `
    -c Release `
    -r win-x64 `
    -p:SelfContained=false `
    -p:CopyLocalLockFileAssemblies=true `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    Write-Host "Plugin publish failed!" -ForegroundColor Red
    exit 1
}

# Build VAR.Toolbox
Write-Host "Building VAR.Toolbox..."
dotnet build $toolboxProject -c Release -r win-x64

if ($LASTEXITCODE -ne 0) {
    Write-Host "VAR.Toolbox build failed!" -ForegroundColor Red
    exit 1
}

# Find VAR.Toolbox build output
$toolboxBuildDir = Join-Path $sourceFolder 'VAR.Toolbox\bin\Release\net10.0\win-x64'

# Get all files from VAR.Toolbox build
$toolboxFiles = Get-ChildItem $toolboxBuildDir -File
Write-Host ""
Write-Host "Removing files from plugin publish that already exist in VAR.Toolbox..."

# Remove overlapping files from VAR.Toolbox build output
$removedCount = 0
foreach ($pluginFile in (Get-ChildItem $publishDir -File)) {
    $matchingFile = $toolboxFiles | Where-Object { $_.Name -eq $pluginFile.Name }
    if ($matchingFile) {
        Remove-Item $pluginFile.FullName
        Write-Host "  Removed: $($pluginFile.Name)"
        $removedCount++
    }
}

# Remove satellite assemblies from plugin publish that overlap with VAR.Toolbox
$removedSatellites = 0
$toolboxSatelliteFiles = Get-ChildItem $toolboxBuildDir -Filter '*.resources.dll' -Recurse | ForEach-Object { $_.Name }

foreach ($satDir in Get-ChildItem $publishDir -Directory | Where-Object { $_.Name -match '^[a-z]{2}(-[A-Za-z]+)?$' }) {
    foreach ($resFile in Get-ChildItem $satDir.FullName -File -Filter '*.resources.dll') {
        if ($toolboxSatelliteFiles -contains $resFile.Name) {
            Remove-Item $resFile.FullName
            Write-Host "  Removed satellite: $($satDir.Name)/$($resFile.Name)"
            $removedSatellites++
        }
    }
    # Remove empty folders
    if ((Get-ChildItem $satDir.FullName -File).Count -eq 0 -and (Get-ChildItem $satDir.FullName -Directory -Recurse).Count -eq 0) {
        Remove-Item $satDir.FullName -Recurse
        Write-Host "  Removed empty folder: $($satDir.Name)/"
    }
}

Write-Host "  Total removed: $removedCount"
Write-Host ""
Write-Host "Published successfully to: $publishDir" -ForegroundColor Green
Write-Host ""
Write-Host "Copy the contents of $publishDir to the VAR.Toolbox publish directory's plugins folder." -ForegroundColor Cyan
Write-Host ""
Write-Host "Plugin files remaining:" -ForegroundColor Cyan
Get-ChildItem $publishDir -File | ForEach-Object { Write-Host "  $($_.Name)" -ForegroundColor Cyan }

$publishDirDirs = Get-ChildItem $publishDir -Directory
if ($publishDirDirs) {
    Write-Host ""
    Write-Host "Folders remaining:" -ForegroundColor Cyan
    $publishDirDirs | ForEach-Object { Write-Host "  $($_.Name)/" -ForegroundColor Cyan }
}
