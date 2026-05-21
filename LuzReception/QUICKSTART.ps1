#!/usr/bin/env pwsh
# ============================================================================
# LUZ GRAND HOTEL - Application Réception
# Quickstart Guide - Compiler & Tester l'application
# ============================================================================

Write-Host @"
╔════════════════════════════════════════════════════════════════════════════╗
║                                                                            ║
║         🏨 LUZ GRAND HOTEL - Application Réception & Accueil 🏨          ║
║                                                                            ║
║                    WPF Portable | VB.NET | Import OPERA                   ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝

✨ BIENVENUE !

Cet application a été conçue pour faciliter la gestion des arrivées à la 
réception du LUZ GRAND HOTEL : cartons clé, welcome letters multilingues, 
et bien plus !

"@ -ForegroundColor Cyan

$ProjectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "📂 Répertoire du projet: $ProjectRoot" -ForegroundColor Yellow
Write-Host ""

# Menu principal
do {
    Write-Host "Que voulez-vous faire ?" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  1️⃣  Compiler l'application (Release)" -ForegroundColor White
    Write-Host "  2️⃣  Compiler en Debug (développement)" -ForegroundColor White
    Write-Host "  3️⃣  Ouvrir la solution dans Visual Studio" -ForegroundColor White
    Write-Host "  4️⃣  Lancer l'application" -ForegroundColor White
    Write-Host "  5️⃣  Voir la structure du projet" -ForegroundColor White
    Write-Host "  6️⃣  Voir le fichier README" -ForegroundColor White
    Write-Host "  0️⃣  Quitter" -ForegroundColor White
    Write-Host ""
    
    $choice = Read-Host "Entrez votre choix (0-6)"
    
    switch ($choice) {
        "1" {
            Write-Host "`n🔨 Compilation Release..." -ForegroundColor Green
            & "$ProjectRoot\Build\Build.ps1" -Configuration Release
            break
        }
        "2" {
            Write-Host "`n🔨 Compilation Debug..." -ForegroundColor Green
            & "$ProjectRoot\Build\Build.ps1" -Configuration Debug
            break
        }
        "3" {
            Write-Host "`n🚀 Ouverture Visual Studio..." -ForegroundColor Green
            $sln = "$ProjectRoot\LuzReception.sln"
            if (Test-Path $sln) {
                Start-Process $sln
            } else {
                Write-Host "❌ Fichier .sln non trouvé" -ForegroundColor Red
            }
            break
        }
        "4" {
            Write-Host "`n▶️  Lancement de l'application..." -ForegroundColor Green
            $exe = "$ProjectRoot\Bin\LuzReception.exe"
            if (Test-Path $exe) {
                Start-Process $exe
            } else {
                Write-Host "❌ Fichier .exe non trouvé. Compilez d'abord avec option 1" -ForegroundColor Red
            }
            break
        }
        "5" {
            Write-Host "`n📂 Structure du projet:" -ForegroundColor Green
            Get-ChildItem -Path $ProjectRoot -Recurse -Directory | 
                Select-Object @{Name="Chemin";Expression={$_.FullName.Replace($ProjectRoot, ".")}} |
                Format-Table -AutoSize
            break
        }
        "6" {
            Write-Host "`n📖 Contenu du README:" -ForegroundColor Green
            Get-Content "$ProjectRoot\README.md"
            break
        }
        "0" {
            Write-Host "`n✅ Au revoir !" -ForegroundColor Green
            break
        }
        default {
            Write-Host "❌ Choix invalide" -ForegroundColor Red
        }
    }
    
    if ($choice -ne "0") {
        Write-Host ""
        Read-Host "Appuyez sur ENTRÉE pour continuer"
        Clear-Host
    }
} until ($choice -eq "0")

Write-Host @"

╔════════════════════════════════════════════════════════════════════════════╗
║                                                                            ║
║  💡 Astuces:                                                               ║
║                                                                            ║
║  • Documentation: Consultez README.md pour l'utilisation complète         ║
║  • Tests: Voir Bin/Data/SAMPLE_OPERA.tsv pour fichier test               ║
║  • Développement: Ouvrez la solution dans Visual Studio                   ║
║  • Portabilité: Copiez le dossier Bin/ pour déployer n'importe où       ║
║                                                                            ║
║  📞 Support: Contactez le développeur pour aide/améliorations             ║
║                                                                            ║
╚════════════════════════════════════════════════════════════════════════════╝

"@ -ForegroundColor Cyan
