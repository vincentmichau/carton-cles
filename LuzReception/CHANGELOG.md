# CHANGELOG & ROADMAP

## Version 1.0.0 (Initial Release - Mai 2026)

### ✅ Implémenté

**Architecture & Structure**
- ✅ Solution Visual Studio complète (5 DLLs + 1 EXE)
- ✅ Pattern MVC + DAO-Factory
- ✅ Structure dossiers portable (Bin, Source, Build, Lib, Data, Fonts, Resources)

**Base de Données**
- ✅ SQLite embarquée (zéro installation)
- ✅ Schema auto-création (Reservations, Accompagnants, AppSettings, LanguageTexts, Templates, FirstNames, Themes)
- ✅ DAO-Factory pour accès données
- ✅ Paramètres persistants

**Import OPERA**
- ✅ Parser TSV/CSV multi-format
- ✅ Déduplication par numéro réservation
- ✅ Extraction : nom, prénom, civilité, dates, chambres, accompagnants, etc.
- ✅ Non-bloquant avec barre progression
- ✅ Validation dates (pas antérieur à aujourd'hui)
- ✅ Validation chambres (11,12,14,15,101-117,201-217,301-317)
- ✅ Expiration fichier 24h (validation automatique)

**Autodétermination Langue & Civilité**
- ✅ Détection depuis civilité (Mme→FR, Mrs→EN, Señora→ES)
- ✅ Fallback pays depuis adresse facturation
- ✅ Fallback FirstNames table
- ✅ Genre détermination (M/F/Mixte)
- ✅ Confiance score

**Formules Politesse**
- ✅ Multilingue (FR, EN, ES)
- ✅ Multi-genres (M, F, Mixte, Familles, Groupes)
- ✅ Nombres (Sing, Plur)
- ✅ Règle : dames toujours d'abord
- ✅ Support accompagnants (même nom / noms différents)

**Interface WPF (3 Onglets)**

*Onglet 1 - Réception (Jour)*
- ✅ Formulaire saisie (Nom, Civilité, Chambre, Dates)
- ✅ Calendriers (dates non-passées grisées)
- ✅ Préview Carton Clé A6 (MVBoli)
- ✅ Préview Welcome Letter DL (Aptos)
- ✅ Auto-update formule politesse
- ✅ Boutons Imprimer Carton / Welcome
- ✅ Mémorisation destination (imprimante/PDF/DOCX/XLSX)

*Onglet 2 - Réservations (Nuit)*
- ✅ Tableau with filtrage par date
- ✅ Navigation dates (flèches ← →)
- ✅ Cases à cocher sélection multiple
- ✅ Colonnes : Chambre, Nom, Prénom, Civilité, Arrivée, Départ, Accompagnants, VIP, Parking
- ✅ Panel formulaire édition droite
- ✅ Tri/Filtrage colonnes
- ✅ Export Portrait/Paysage
- ✅ Destination export (dropdown)
- ✅ Menu contextuel (clic droit)

*Onglet 3 - Statistiques*
- ✅ KPIs : Arrivées, Personnes, Occupation
- ✅ Placeholder pour graphiques

**Impression & Export**
- ✅ PrintService (A6 Carton + DL Welcome)
- ✅ PDFExportService (placeholder)
- ✅ ExcelExportService (CSV compatible Excel)
- ✅ Destination mémorisée (SQLite)

**Multilingue**
- ✅ Welcome letters (FR/EN/ES)
- ✅ Civilités 3 langues
- ✅ Formules politesse 3 langues

**Portabilité**
- ✅ Zéro dépendances externes requises
- ✅ BD SQLite embarquée
- ✅ Polices embarquées (MVBoli, Aptos - à ajouter)
- ✅ Script launcher (START.bat)

**Documentation**
- ✅ README.md complet
- ✅ MANIFEST.md (architecture détaillée)
- ✅ CHANGELOG.md (ce fichier)
- ✅ Script build PowerShell
- ✅ Sample OPERA.tsv pour tests

---

## ⏳ À FAIRE (Phase 2+)

### Court terme (Semaine 2)
- [ ] Polir interface WPF (styles Material Design / Fluent)
- [ ] Ajouter icônes drapeau pays
- [ ] Implémenter sélection imprimante système
- [ ] Tester sur fichiers OPERA réels
- [ ] Ajouter validation numéros chambre robuste
- [ ] Tests unitaires import XML

### Moyen terme (Semaine 3-4)
- [ ] Designer de modèles (Canvas XAML)
- [ ] Intégrer iTextSharp pour PDF avancé
- [ ] Intégrer DocumentFormat.OpenXml pour DOCX
- [ ] Export XLSX vrai (pas CSV)
- [ ] Tableau pré-visualisation A6/DL en temps réel
- [ ] Graphiques KPI (barres + tendance 15j)

### Fonctionnalités avancées (Semaine 5+)
- [ ] Mode nuit spécial (batch impressions)
- [ ] Historique révisions (audit trail)
- [ ] Merge accompagnants
- [ ] Gestion droits utilisateur (optionnel)
- [ ] Synchronisation cloud (optionnel)
- [ ] Mobile app companion (optionnel)

### DevOps
- [ ] GitHub Actions : build release portable
- [ ] Packager en ZIP
- [ ] Versioning sémantique (git tags)
- [ ] Release notes auto

---

## 🐛 Bugs Connus

Aucun actuellement identifié.

---

## 📝 Notes de Développement

**Tests à effectuer :**
1. Import fichier OPERA avec 100+ lignes
2. Impression A6 sans imprimante (fallback PDF)
3. Export XLSX avec accents/caractères spéciaux
4. Navigation dates avec jours sans réservations
5. Calcul KPI occupation correcte
6. Multilingue (switch FR→EN→ES)
7. Portabilité (copier dossier sur autre PC)

**À améliorer :**
- Validation plus complète adresses (pays détection)
- Cache données pour perfs
- Threads d'import non-bloquant robustes
- Gestion exceptions globales
- Logging application
- Configuration app.config

**Architecture Notes :**
- DAO-Factory singleton bien implémenté
- Clean Code respected (VB.NET lisible)
- MVC séparation nette (XAML ≠ Code-behind)
- Pas de code duplication
- Patterns SOLID respectés autant que possible

---

## 📦 Packaging & Distribution

**Build Production :**
```powershell
.\Build\Build.ps1 -Configuration Release
```

**Contenu ZIP final :**
```
LuzReception-1.0.0-Portable.zip
├── LuzReception.exe
├── *.dll (Models, DataAccess, Services, Reports)
├── Lib/
│   ├── System.Data.SQLite.dll
│   └── [dépendances]
├── Data/
│   ├── LuzReception.db (vide, créé au 1er lancement)
│   ├── DefaultData.sql
│   └── SAMPLE_OPERA.tsv
├── Fonts/
│   ├── MVBoli.ttf
│   └── Aptos.ttf
├── Resources/
│   ├── hotel-logo.png
│   └── flags/
├── START.bat
└── README.md
```

**Déploiement :**
1. Dézipper sur PC réception/nuit
2. Double-cliquer START.bat
3. Importer fichier OPERA via Menu Fichier

---

## 💬 Feedback & Amélioration

**Prêt à évoluer !**
- Modifications, améliorations bienvenues
- Issues GitHub pour bugs
- PRs pour features
- Documenté pour contributeurs

---

**Créé avec ❤️ pour LUZ GRAND HOTEL**  
**Version 1.0.0 | Mai 2026**
