Imports System.Data.SQLite
Imports LuzReception.Models

Namespace LuzReception.DataAccess
    ''' <summary>
    ''' DAO pour gérer les réservations
    ''' </summary>
    Public Class DAOReservation
        Private _connectionString As String

        Public Sub New(connectionString As String)
            _connectionString = connectionString
        End Sub

        ''' <summary>
        ''' Insère une nouvelle réservation
        ''' </summary>
        Public Function Insert(reservation As Reservation) As Integer
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    Dim sql = "INSERT INTO Reservations (NumeroReservation, NumerosChambre, NomClient, PrenomClient, 
                               CiviliteClient, LangueAuto, GenreGrammatical, NombrePersonnes, DateArrivee, 
                               DateDepart, VIP, Notes, Traces, Parking, NombreAdultes, NombreEnfants, 
                               NombreTotalPersonnes, FormulePolitesse, PaysFacturation, DateModification, UtilisateurModification)
                               VALUES (@NumRes, @NumChamb, @Nom, @Prenom, @Civilite, @Langue, @Genre, @Nombre, 
                               @DateArr, @DateDep, @VIP, @Notes, @Traces, @Parking, @Adults, @Children, 
                               @Total, @Formule, @Pays, @DateMod, @Utilisateur);"

                    Using cmd = New SQLiteCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@NumRes", If(reservation.NumeroReservation, ""))
                        cmd.Parameters.AddWithValue("@NumChamb", reservation.NumerosChambre)
                        cmd.Parameters.AddWithValue("@Nom", reservation.NomClient)
                        cmd.Parameters.AddWithValue("@Prenom", If(reservation.PrenomClient, ""))
                        cmd.Parameters.AddWithValue("@Civilite", If(reservation.CiviliteClient, ""))
                        cmd.Parameters.AddWithValue("@Langue", reservation.LangueAuto)
                        cmd.Parameters.AddWithValue("@Genre", reservation.GenreGrammatical)
                        cmd.Parameters.AddWithValue("@Nombre", reservation.NombrePersonnes)
                        cmd.Parameters.AddWithValue("@DateArr", reservation.DateArrivee)
                        cmd.Parameters.AddWithValue("@DateDep", reservation.DateDepart)
                        cmd.Parameters.AddWithValue("@VIP", If(reservation.VIP, 1, 0))
                        cmd.Parameters.AddWithValue("@Notes", If(reservation.Notes, ""))
                        cmd.Parameters.AddWithValue("@Traces", If(reservation.Traces, ""))
                        cmd.Parameters.AddWithValue("@Parking", If(reservation.Parking, ""))
                        cmd.Parameters.AddWithValue("@Adults", reservation.NombreAdultes)
                        cmd.Parameters.AddWithValue("@Children", reservation.NombreEnfants)
                        cmd.Parameters.AddWithValue("@Total", reservation.NombreTotalPersonnes)
                        cmd.Parameters.AddWithValue("@Formule", If(reservation.FormulePolitesse, ""))
                        cmd.Parameters.AddWithValue("@Pays", If(reservation.PaysFacturation, ""))
                        cmd.Parameters.AddWithValue("@DateMod", reservation.DateModification)
                        cmd.Parameters.AddWithValue("@Utilisateur", If(reservation.UtilisateurModification, Environment.UserName))

                        cmd.ExecuteNonQuery()
                    End Using

                    ' Récupérer l'ID généré
                    Using cmdId = New SQLiteCommand("SELECT last_insert_rowid();", conn)
                        Return CInt(cmdId.ExecuteScalar())
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur Insert Reservation: {ex.Message}")
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Récupère toutes les réservations
        ''' </summary>
        Public Function GetAll() As List(Of Reservation)
            Dim reservations = New List(Of Reservation)()
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    Dim sql = "SELECT * FROM Reservations ORDER BY DateArrivee DESC"
                    Using cmd = New SQLiteCommand(sql, conn)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                reservations.Add(MapRowToReservation(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur GetAll: {ex.Message}")
            End Try
            Return reservations
        End Function

        ''' <summary>
        ''' Récupère les réservations d'une date donnée
        ''' </summary>
        Public Function GetByDate(dateArrivee As Date) As List(Of Reservation)
            Dim reservations = New List(Of Reservation)()
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    Dim sql = "SELECT * FROM Reservations WHERE DATE(DateArrivee) = @Date ORDER BY NumerosChambre"
                    Using cmd = New SQLiteCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Date", dateArrivee.ToString("yyyy-MM-dd"))
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                reservations.Add(MapRowToReservation(reader))
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur GetByDate: {ex.Message}")
            End Try
            Return reservations
        End Function

        ''' <summary>
        ''' Met à jour une réservation
        ''' </summary>
        Public Sub Update(reservation As Reservation)
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    Dim sql = "UPDATE Reservations SET NumerosChambre=@NumChamb, NomClient=@Nom, 
                               PrenomClient=@Prenom, CiviliteClient=@Civilite, LangueAuto=@Langue, 
                               GenreGrammatical=@Genre, NombrePersonnes=@Nombre, DateArrivee=@DateArr, 
                               DateDepart=@DateDep, VIP=@VIP, Notes=@Notes, Traces=@Traces, Parking=@Parking,
                               NombreAdultes=@Adults, NombreEnfants=@Children, NombreTotalPersonnes=@Total,
                               FormulePolitesse=@Formule, PaysFacturation=@Pays, DateModification=@DateMod,
                               UtilisateurModification=@Utilisateur WHERE Id=@Id"

                    Using cmd = New SQLiteCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@Id", reservation.Id)
                        cmd.Parameters.AddWithValue("@NumChamb", reservation.NumerosChambre)
                        cmd.Parameters.AddWithValue("@Nom", reservation.NomClient)
                        cmd.Parameters.AddWithValue("@Prenom", If(reservation.PrenomClient, ""))
                        cmd.Parameters.AddWithValue("@Civilite", If(reservation.CiviliteClient, ""))
                        cmd.Parameters.AddWithValue("@Langue", reservation.LangueAuto)
                        cmd.Parameters.AddWithValue("@Genre", reservation.GenreGrammatical)
                        cmd.Parameters.AddWithValue("@Nombre", reservation.NombrePersonnes)
                        cmd.Parameters.AddWithValue("@DateArr", reservation.DateArrivee)
                        cmd.Parameters.AddWithValue("@DateDep", reservation.DateDepart)
                        cmd.Parameters.AddWithValue("@VIP", If(reservation.VIP, 1, 0))
                        cmd.Parameters.AddWithValue("@Notes", If(reservation.Notes, ""))
                        cmd.Parameters.AddWithValue("@Traces", If(reservation.Traces, ""))
                        cmd.Parameters.AddWithValue("@Parking", If(reservation.Parking, ""))
                        cmd.Parameters.AddWithValue("@Adults", reservation.NombreAdultes)
                        cmd.Parameters.AddWithValue("@Children", reservation.NombreEnfants)
                        cmd.Parameters.AddWithValue("@Total", reservation.NombreTotalPersonnes)
                        cmd.Parameters.AddWithValue("@Formule", If(reservation.FormulePolitesse, ""))
                        cmd.Parameters.AddWithValue("@Pays", If(reservation.PaysFacturation, ""))
                        cmd.Parameters.AddWithValue("@DateMod", Now)
                        cmd.Parameters.AddWithValue("@Utilisateur", Environment.UserName)

                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur Update: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Supprime toutes les réservations (Reset)
        ''' </summary>
        Public Sub DeleteAll()
            Try
                Using conn = New SQLiteConnection(_connectionString)
                    conn.Open()
                    Using cmd = New SQLiteCommand("DELETE FROM Reservations; DELETE FROM Accompagnants;", conn)
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur DeleteAll: {ex.Message}")
            End Try
        End Sub

        Private Function MapRowToReservation(reader As SQLiteDataReader) As Reservation
            Dim res = New Reservation With {
                .Id = CInt(reader("Id")),
                .NumeroReservation = reader("NumeroReservation").ToString(),
                .NumerosChambre = reader("NumerosChambre").ToString(),
                .NomClient = reader("NomClient").ToString(),
                .PrenomClient = reader("PrenomClient").ToString(),
                .CiviliteClient = reader("CiviliteClient").ToString(),
                .LangueAuto = reader("LangueAuto").ToString(),
                .GenreGrammatical = reader("GenreGrammatical").ToString(),
                .NombrePersonnes = reader("NombrePersonnes").ToString(),
                .DateArrivee = CDate(reader("DateArrivee")),
                .DateDepart = CDate(reader("DateDepart")),
                .VIP = CBool(reader("VIP")),
                .Notes = reader("Notes").ToString(),
                .Traces = reader("Traces").ToString(),
                .Parking = reader("Parking").ToString(),
                .NombreAdultes = CInt(reader("NombreAdultes")),
                .NombreEnfants = CInt(reader("NombreEnfants")),
                .NombreTotalPersonnes = CInt(reader("NombreTotalPersonnes")),
                .FormulePolitesse = reader("FormulePolitesse").ToString(),
                .PaysFacturation = reader("PaysFacturation").ToString()
            }
            Return res
        End Function
    End Class
End Namespace
