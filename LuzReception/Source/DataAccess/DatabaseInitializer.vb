Imports System.Data.SQLite
Imports LuzReception.Models

Namespace LuzReception.DataAccess
    ''' <summary>
    ''' Initialise la base de données SQLite et crée les tables
    ''' </summary>
    Public Class DatabaseInitializer
        Private Shared ReadOnly DBPath As String = System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data", "LuzReception.db")

        ''' <summary>
        ''' Initialise la base de données
        ''' </summary>
        Public Shared Sub Initialize()
            Try
                Dim dir = System.IO.Path.GetDirectoryName(DBPath)
                If Not System.IO.Directory.Exists(dir) Then
                    System.IO.Directory.CreateDirectory(dir)
                End If

                Using conn = New SQLiteConnection($"Data Source={DBPath};Version=3;")
                    conn.Open()

                    ' Créer les tables
                    ExecuteCommand(conn, CreateTableReservations())
                    ExecuteCommand(conn, CreateTableAccompagnants())
                    ExecuteCommand(conn, CreateTableAppSettings())
                    ExecuteCommand(conn, CreateTableLanguageTexts())
                    ExecuteCommand(conn, CreateTableTemplates())
                    ExecuteCommand(conn, CreateTableFirstNames())
                    ExecuteCommand(conn, CreateTableThemes())

                    conn.Close()
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur initialisation DB: {ex.Message}")
            End Try
        End Sub

        Private Shared Sub ExecuteCommand(conn As SQLiteConnection, sql As String)
            Try
                Using cmd = New SQLiteCommand(sql, conn)
                    cmd.ExecuteNonQuery()
                End Using
            Catch
                ' Table existe probablement déjà
            End Try
        End Sub

        Private Shared Function CreateTableReservations() As String
            Return "CREATE TABLE IF NOT EXISTS Reservations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                NumeroReservation TEXT UNIQUE,
                NumerosChambre TEXT NOT NULL,
                NomClient TEXT NOT NULL,
                PrenomClient TEXT,
                CiviliteClient TEXT,
                LangueAuto TEXT DEFAULT 'FR',
                GenreGrammatical TEXT DEFAULT 'M',
                NombrePersonnes TEXT DEFAULT 'Sing',
                DateArrivee DATE NOT NULL,
                DateDepart DATE NOT NULL,
                VIP BOOLEAN DEFAULT 0,
                Notes TEXT,
                Traces TEXT,
                Parking TEXT,
                NombreAdultes INTEGER DEFAULT 0,
                NombreEnfants INTEGER DEFAULT 0,
                NombreTotalPersonnes INTEGER DEFAULT 0,
                FormulePolitesse TEXT,
                PaysFacturation TEXT,
                DateModification DATETIME DEFAULT CURRENT_TIMESTAMP,
                UtilisateurModification TEXT
            )"
        End Function

        Private Shared Function CreateTableAccompagnants() As String
            Return "CREATE TABLE IF NOT EXISTS Accompagnants (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ReservationId INTEGER NOT NULL,
                Nom TEXT,
                Prenom TEXT,
                Civilite TEXT,
                Langue TEXT DEFAULT 'FR',
                Genre TEXT DEFAULT 'M',
                FOREIGN KEY(ReservationId) REFERENCES Reservations(Id)
            )"
        End Function

        Private Shared Function CreateTableAppSettings() As String
            Return "CREATE TABLE IF NOT EXISTS AppSettings (
                Cle TEXT PRIMARY KEY,
                Valeur TEXT
            )"
        End Function

        Private Shared Function CreateTableLanguageTexts() As String
            Return "CREATE TABLE IF NOT EXISTS LanguageTexts (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Cle TEXT NOT NULL,
                Langue TEXT NOT NULL,
                Texte TEXT,
                Genre TEXT,
                Nombre TEXT,
                UNIQUE(Cle, Langue, Genre, Nombre)
            )"
        End Function

        Private Shared Function CreateTableTemplates() As String
            Return "CREATE TABLE IF NOT EXISTS Templates (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nom TEXT NOT NULL UNIQUE,
                Type TEXT NOT NULL,
                ContenuJSON TEXT,
                DateCreation DATETIME DEFAULT CURRENT_TIMESTAMP,
                Actif BOOLEAN DEFAULT 1
            )"
        End Function

        Private Shared Function CreateTableFirstNames() As String
            Return "CREATE TABLE IF NOT EXISTS FirstNames (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Prenom TEXT NOT NULL,
                Langue TEXT NOT NULL,
                Genre TEXT NOT NULL
            )"
        End Function

        Private Shared Function CreateTableThemes() As String
            Return "CREATE TABLE IF NOT EXISTS Themes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nom TEXT NOT NULL UNIQUE,
                LightColors TEXT,
                DarkColors TEXT,
                Actif BOOLEAN DEFAULT 1
            )"
        End Function

        Public Shared Function GetConnectionString() As String
            Return $"Data Source={DBPath};Version=3;"
        End Function
    End Class
End Namespace
