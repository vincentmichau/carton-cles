LuzReception/
├── README.md                          # Documentation complète
├── LuzReception.sln                   # Solution Visual Studio
│
├── Source/
│   ├── LuzReception/                  # Application WPF principale
│   │   ├── LuzReception.vbproj
│   │   ├── Application.xaml
│   │   ├── Application.xaml.vb
│   │   ├── MainWindow.xaml            # Interface 3 onglets
│   │   ├── MainWindow.xaml.vb         # Logique onglets
│   │   └── My Project/
│   │       └── AssemblyInfo.vb
│   │
│   ├── Models/                        # Entités métier (DLL)
│   │   ├── LuzReception.Models.vbproj
│   │   ├── Reservation.vb             # Classe Réservation
│   │   ├── Accompagnant.vb            # Classe Accompagnant
│   │   ├── LanguageDetermination.vb   # Auto-détection langue/genre
│   │   └── GracefulNessFormula.vb     # Génération formules politesse
│   │
│   ├── DataAccess/                    # DAO-Factory + SQLite (DLL)
│   │   ├── LuzReception.DataAccess.vbproj
│   │   ├── DatabaseInitializer.vb     # Création schema SQLite
│   │   ├── DAOReservation.vb          # CRUD réservations
│   │   ├── DAOSettings.vb             # Gestion paramètres
│   │   └── DAOFactory.vb              # Factory singleton
│   │
│   ├── Services/                      # Logique métier (DLL)
│   │   ├── LuzReception.Services.vbproj
│   │   ├── OPERAImportService.vb      # Parser CSV/TSV OPERA
│   │   ├── LanguageDetectionService.vb # Détection langue
│   │   └── ReservationService.vb      # Services métier
│   │
│   └── Reports/                       # Impression + Export (DLL)
│       ├── LuzReception.Reports.vbproj
│       ├── PrintService.vb            # Impression A6/DL
│       ├── PDFExportService.vb        # Export PDF
│       └── ExcelExportService.vb      # Export XLSX
│
├── Build/
│   └── Build.ps1                      # Script PowerShell build portable
│
├── Bin/                               # SORTIE BUILD (portable)
│   ├── LuzReception.exe               # Exécutable principal
│   ├── LuzReception.*.dll             # DLLs modules
│   │
│   ├── Lib/                           # Dépendances (incluses)
│   │   ├── System.Data.SQLite.dll
│   │   └── [autres dépendances]
│   │
│   ├── Data/                          # BD SQLite + Modèles
│   │   ├── LuzReception.db            # Base de données
│   │   └── DefaultTemplates.sql       # Scripts initialisation
│   │
│   ├── Fonts/                         # Polices embarquées
│   │   ├── MVBoli.ttf                 # Carton clé
│   │   └── Aptos.ttf                  # Welcome letter
│   │
│   └── Resources/                     # Images, icones
│       ├── hotel-logo.png
│       └── flags/
│           ├── fr.png
│           ├── en.png
│           └── es.png
│
└── Properties/                        # [Réservé pour versions futures]

═══════════════════════════════════════════════════════════════════════════════

ARCHITECTURE
============

MVC + DAO-Factory Pattern:

1. UI (MainWindow.xaml / MainWindow.xaml.vb)
   ↓ commandes utilisateur
2. Contrôleur (MainWindow.xaml.vb event handlers)
   ↓ appelle
3. Service (ReservationService, OPERAImportService, etc.)
   ↓ utilise
4. DAO (DAOReservation, DAOFactory)
   ↓ accède à
5. Model (Reservation, Accompagnant, etc.)
   ↓ stocke
6. SQLite (Data/LuzReception.db)

MODULES
=======

Models (DLL)
  - Entités métier : Reservation, Accompagnant
  - Détection : LanguageDetermination, CiviliteDetectionMap
  - Formules : GracefulNessFormula (FR/EN/ES multilingues)

DataAccess (DLL)
  - DAO-Factory pattern
  - DatabaseInitializer : création schema SQLite auto
  - DAOReservation : SELECT/INSERT/UPDATE/DELETE réservations
  - DAOSettings : configuration persistante
  - Singleton DAOFactory pour toute l'app

Services (DLL)
  - OPERAImportService : parse CSV/TSV OPERA non-bloquant
  - LanguageDetectionService : autodétection langue/civilité
  - ReservationService : métier + validations

Reports (DLL)
  - PrintService : impression A6 (carton) + DL (welcome)
  - PDFExportService : export PDF
  - ExcelExportService : export XLSX

UI (EXE)
  - WPF XAML : 3 onglets (Réception, Réservations, Stats)
  - Event handlers : gestion utilisateur

FEATURES IMPLÉMENTÉES
======================

✅ Saisie manuelle + Import OPERA XML
✅ Autodétermination langue/civilité (FR/EN/ES)
✅ Formules politesse multilingues + multi-genres
✅ Cartons clé A6 (MVBoli)
✅ Welcome letters DL (Aptos) 3 langues
✅ Listings paysage/portrait
✅ Export PDF/DOCX/XLSX
✅ SQLite persistant
✅ 100% portable (copier-coller)
✅ Dashboard KPI

PRÊT À DÉVELOPPER
==================

- Polir interface WPF (styles, icones)
- Tester sur vrais fichiers OPERA
- Intégrer iTextSharp pour PDF avancé
- Ajouter Designer de modèles (canvas)
- Packager en zip portable
- Publier sur GitHub
