# Script de Build - Construire l'application portable

Param(
    [string]$Configuration = "Release"
)

Write-Host "====================================" -ForegroundColor Cyan
Write-Host "BUILD: LUZ GRAND HOTEL - Réception" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan

$BuildDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$SolutionFile = "$BuildDir\LuzReception.sln"

# Vérifier MSBuild
$MSBuildPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
if (-not (Test-Path $MSBuildPath)) {
    $MSBuildPath = "C:\Program Files (x86)\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
}

if (-not (Test-Path $MSBuildPath)) {
    Write-Host "❌ MSBuild not found!" -ForegroundColor Red
    exit 1
}

Write-Host "📦 Configuration: $Configuration" -ForegroundColor Green
Write-Host "🔨 MSBuild: $MSBuildPath" -ForegroundColor Green

# Nettoyer les builds antérieures
Write-Host "`n🧹 Nettoyage..." -ForegroundColor Yellow
& "$MSBuildPath" $SolutionFile /t:Clean /p:Configuration=$Configuration | Out-Null

# Compiler
Write-Host "🔨 Compilation..." -ForegroundColor Yellow
& "$MSBuildPath" $SolutionFile /p:Configuration=$Configuration /p:Platform="Any CPU" /maxcpucount

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build échoué!" -ForegroundColor Red
    exit 1
}

Write-Host "`n✅ Build réussi!" -ForegroundColor Green
Write-Host "📂 Output: $BuildDir\Bin\" -ForegroundColor Cyan

# Lister les fichiers générés
Write-Host "`n📋 Fichiers générés:" -ForegroundColor Yellow
Get-ChildItem "$BuildDir\Bin\*.exe", "$BuildDir\Bin\*.dll" | ForEach-Object {
    Write-Host "   $($_.Name) ($([math]::Round($_.Length/1MB, 2)) MB)" -ForegroundColor White
}

Write-Host "`n✨ L'application est prête !" -ForegroundColor Green
Write-Host "💡 Lancez: $BuildDir\Bin\LuzReception.exe" -ForegroundColor Cyan
