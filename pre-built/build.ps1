# build.ps1 - builds and packages the screensaver as an installable .scr
# Run this from the project root (the folder containing Screensaver.csproj),
# in PowerShell, on Windows with the .NET 8 SDK installed.

$ErrorActionPreference = "Stop"

$projectDir = $PSScriptRoot
$csproj     = Join-Path $projectDir "Screensaver.csproj"
$publishDir = Join-Path $projectDir "publish\Screensaver"

if (-not (Test-Path $csproj)) {
    throw "Can't find Screensaver.csproj next to build.ps1 ($csproj). Run this script from the project folder."
}

Write-Host "==> Cleaning previous output..." -ForegroundColor Cyan
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }

Write-Host "==> Publishing self-contained win-x64 build (this also runs 'dotnet restore')..." -ForegroundColor Cyan
dotnet publish $csproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -o $publishDir

$exePath = Join-Path $publishDir "Screensaver.exe"
$scrPath = Join-Path $publishDir "Screensaver.scr"

if (-not (Test-Path $exePath)) {
    throw "Screensaver.exe not found in $publishDir - publish failed."
}

Write-Host "==> Renaming Screensaver.exe -> Screensaver.scr..." -ForegroundColor Cyan
if (Test-Path $scrPath) { Remove-Item $scrPath -Force }
Rename-Item -Path $exePath -NewName "Screensaver.scr"

Write-Host ""
Write-Host "Done. The whole folder below is your ready-to-install screensaver:" -ForegroundColor Green
Write-Host "  $publishDir"
Write-Host ""
Write-Host "IMPORTANT: the whole folder (Screensaver.scr + all .dll files next to" -ForegroundColor Yellow
Write-Host "it + the web\ folder) must stay together - the .scr file alone is not enough." -ForegroundColor Yellow
Write-Host ""
Write-Host "Next steps: see install.md"