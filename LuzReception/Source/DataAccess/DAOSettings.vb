Imports System.Data.SQLite
Imports LuzReception.Models

Namespace LuzReception.DataAccess
    ''' <summary>
    ''' DAO pour gérer les paramètres applicatifs
    ''' </summary>
    Public Class DAOSettings
        Private _connectionString As String

        Public Sub New(connectionString As String)
            _connectionString = connectionString
        End Sub

        ''' <summary>
        ''' Récupère une valeur de paramètre
        ''' </summary>
        Public Function GetSetting(cle As String, defaultValue As String) As String
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    Dim sql = "SELECT Valeur FROM AppSettings WHERE Cle = @Cle"
                    Using cmd = New SQLiteCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Cle", cle)
                        Dim result = cmd.ExecuteScalar()
                        If result IsNot Nothing Then
                            Return result.ToString()
                        End If
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur GetSetting: {ex.Message}")
            End Try
            Return defaultValue
        End Function

        ''' <summary>
        ''' Enregistre un paramètre
        ''' </summary>
        Public Sub SetSetting(cle As String, valeur As String)
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    ' Essayer UPDATE d'abord
                    Using cmd = New SQLiteCommand("UPDATE AppSettings SET Valeur=@Valeur WHERE Cle=@Cle", conn)
                        cmd.Parameters.AddWithValue("@Valeur", valeur)
                        cmd.Parameters.AddWithValue("@Cle", cle)
                        If cmd.ExecuteNonQuery() = 0 Then
                            ' Si aucune ligne mise à jour, insérer
                            Using cmdIns = New SQLiteCommand("INSERT INTO AppSettings (Cle, Valeur) VALUES (@Cle, @Valeur)", conn)
                                cmdIns.Parameters.AddWithValue("@Cle", cle)
                                cmdIns.Parameters.AddWithValue("@Valeur", valeur)
                                cmdIns.ExecuteNonQuery()
                            End Using
                        End If
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur SetSetting: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Récupère tous les paramètres
        ''' </summary>
        Public Function GetAllSettings() As Dictionary(Of String, String)
            Dim settings = New Dictionary(Of String, String)()
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    Using cmd = New SQLiteCommand("SELECT Cle, Valeur FROM AppSettings", conn)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                settings.Add(reader("Cle").ToString(), reader("Valeur").ToString())
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur GetAllSettings: {ex.Message}")
            End Try
            Return settings
        End Function
    End Class
End Namespace
