# LUZ GRAND HOTEL - Application Réception 🏨

## 📋 Description

Application WPF portable en VB.NET pour gérer les arrivées et les accueils du **LUZ GRAND HOTEL** (5 étoiles, Saint-Jean-de-Luz).

**Fonctionnalités principales :**
- ✅ Import OPERA XML (réservations automatiques)
- ✅ Saisie manuelle des clients
- ✅ Impression cartons clé (A6) personnalisés
- ✅ Impression welcome letters (DL) multilingues (FR/EN/ES)
- ✅ Listings des arrivées (portrait/paysage)
- ✅ Export PDF, DOCX, XLSX
- ✅ Dashboard KPI
- ✅ Autodétermination langue et civilité
- ✅ 100% portable (zéro installation)

## 🚀 Installation & Utilisation

### Prérequis
- Windows 7 SP1 ou supérieur
- .NET Framework 4.8+
- Imprimante (optionnel, export PDF par défaut)

### Lancement
1. Télécharger le dossier `LuzReception/Bin`
2. Double-cliquer sur `LuzReception.exe`
3. C'est tout ! 🎉

### Structure Dossiers
```
LuzReception/
├── Bin/
│   ├── LuzReception.exe (Application principale)
│   ├── LuzReception.*.dll (Modules)
│   ├── Lib/ (Dépendances System.Data.SQLite)
│   ├── Data/ (BD SQLite + modèles par défaut)
│   ├── Fonts/ (Polices MVBoli, Aptos)
│   └── Resources/ (Images, icônes)
└── Source/ (Code VB.NET pour développeurs)
```

## 📖 Guide Utilisateur

### Onglet 1 - Réception (Jour)
**Accueil rapide des clients last-minute**
1. Remplir le formulaire (Nom, Civilité, Chambre, Dates)
2. Vérifier l'aperçu Carton Clé et Welcome Letter
3. Cliquer "Imprimer Carton" ou "Imprimer Welcome"
4. La destination (imprimante/PDF) est mémorisée

**Raccourcis clavier** :
- `F1` → Imprimer Carton
- `F2` → Imprimer Welcome

### Onglet 2 - Réservations (Nuit)
**Gestion batch des arrivées du lendemain**
1. Importer un fichier XML OPERA via `Fichier > Importer XML`
2. Filtrer les arrivées par date (flèches ← →)
3. Sélectionner les réservations (cases à cocher)
4. Cliquer "Imprimer Portrait" ou "Imprimer Paysage"
5. Exporter en PDF, DOCX ou XLSX

### Onglet 3 - Statistiques
Tableau de bord avec :
- Nombre d'arrivées du jour
- Total de personnes
- Taux d'occupation
- Graphique tendance (15 jours)

## 🔧 Architecture

**Pattern MVC + DAO-Factory**

```
LuzReception.exe (Main WPF)
├── LuzReception.Models.dll
│   ├── Reservation
│   ├── Accompagnant
│   ├── LanguageDetermination
│   └── GracefulNessFormula
├── LuzReception.DataAccess.dll
│   ├── DAOReservation (CRUD)
│   ├── DAOSettings (Configuration)
│   ├── DAOFactory (Singleton)
│   └── DatabaseInitializer (SQLite)
├── LuzReception.Services.dll
│   ├── OPERAImportService (Parser XML/CSV)
│   ├── LanguageDetectionService (Auto-détection)
│   └── ReservationService (Métier)
└── LuzReception.Reports.dll
    ├── PrintService (Impression)
    ├── PDFExportService (Export PDF)
    └── ExcelExportService (Export XLSX)
```

**Base de Données SQLite**
- `Reservations` : données clients
- `Accompagnants` : accompagnants
- `AppSettings` : configuration (imprimante, destination, theme, langue)
- `LanguageTexts` : welcome letters multilingues
- `Templates` : modèles d'impression
- `FirstNames` : aide autodétermination langue
- `Themes` : thèmes couleur light/dark

## 🌍 Multilingue

L'application supporte **FR / EN / ES** pour :
- Welcome letters (textes complets adaptés)
- Civilités (Monsieur, Madame, etc.)
- Formules politesse (dames toujours d'abord)
- Interface globale (menu Paramètres > Langue)

## 📋 Format d'Import OPERA

Le fichier XML OPERA doit contenir les colonnes :
- `CONFIRMATION_NO` → N° de réservation
- `FULL_NAME_NO_SHR_IND` → "NOM,Prenom,Civilité"
- `ROOM_NO` → N° de chambre
- `ARRIVAL` (JJ-MM-AAAA) → Date d'arrivée
- `DEPARTURE` (JJ-MM-AAAA) → Date de départ
- `ADULTS`, `CHILDREN`, `PERSONS` → Nombres
- `ACCOMPANYING_NAMES` → "NOM1,Prenom1,Civ1;NOM2,Prenom2,Civ2"
- `BILL_TO_ADDRESS` → Adresse facturation (détection pays)
- `VIP` → Marqueur VIP
- `RI_NAME` → Options parking
- `TRACE_TEXT` → Notes

⚠️ **Fichier expiré après 24h** : système de validation automatique

## 🛠️ Développement

### Compiler
```powershell
cd LuzReception
MSBuild LuzReception.sln /p:Configuration=Release
```

### Structure Code
- `Clean Code` : commentaires en français, noms explicites
- `Tests` : tests unitaires pour import, algorithmes
- `Patterns` : MVC, DAO-Factory, Singleton
- `Robustesse` : gestion erreurs, validation, fallbacks

### Ajouter une Nouvelle Langue
1. Éditer `GracefulNessFormula.vb` → ajouter fonction `GenerateFormula<Langue>`
2. Éditer `LanguageDetectionService.vb` → ajouter détection
3. Ajouter textes welcome en DB via menu Paramètres > Langues

## 📞 Support & Contribution

**Développé pour :** Hôtel 5 étoiles, Saint-Jean-de-Luz, France

**Contributeurs bienvenues !** 
- Bugs & feature requests → GitHub Issues
- Améliorations → Pull Requests

## 📄 Licence

**MIT License** - Libre d'utilisation, modification, distribution

Copyright © 2026 LUZ GRAND HOTEL

---

**Version :** 1.0.0  
**Dernière mise à jour :** Mai 2026  
**Plateforme :** Windows 7 SP1+, .NET Framework 4.8+
