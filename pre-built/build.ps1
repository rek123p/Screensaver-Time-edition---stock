# build.ps1 - builds and packages the screensaver as an installable .scr
# Run this from the project root (the folder containing Screensaver.csproj),
# in PowerShell, on Windows with the .NET 8 SDK installed.

$ErrorActionPreference = "Stop"

$projectDir = $PSScriptRoot
$csproj     = Join-Path $projectDir "Screensaver.csproj"
$publishDir = Join-Path $projectDir "publish\Screensaver"

if (-not (Test-Path $csproj)) {
    throw "Nie widze Screensaver.csproj obok build.ps1 ($csproj). Uruchom ten skrypt z folderu projektu."
}

Write-Host "==> Czyszcze poprzedni output..." -ForegroundColor Cyan
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }

Write-Host "==> Publikuje self-contained build win-x64 (to samo w sobie robi 'dotnet restore')..." -ForegroundColor Cyan
dotnet publish $csproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=false `
    -o $publishDir

$exePath = Join-Path $publishDir "Screensaver.exe"
$scrPath = Join-Path $publishDir "Screensaver.scr"

if (-not (Test-Path $exePath)) {
    throw "Nie znalazlem Screensaver.exe w $publishDir - publish sie nie powiodl."
}

Write-Host "==> Zmieniam Screensaver.exe -> Screensaver.scr..." -ForegroundColor Cyan
if (Test-Path $scrPath) { Remove-Item $scrPath -Force }
Rename-Item -Path $exePath -NewName "Screensaver.scr"

Write-Host ""
Write-Host "Gotowe. Caly folder ponizej to Twoj gotowy do instalacji wygaszacz:" -ForegroundColor Green
Write-Host "  $publishDir"
Write-Host ""
Write-Host "WAZNE: caly folder (Screensaver.scr + wszystkie .dll obok + folder web\) musi" -ForegroundColor Yellow
Write-Host "zostac razem w jednym miejscu - .scr sam w sobie nie wystarczy." -ForegroundColor Yellow
Write-Host ""
Write-Host "Dalsze kroki: patrz README-INSTALACJA.md"