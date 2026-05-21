Imports LuzReception.Models

Namespace LuzReception.Reports
    ''' <summary>
    ''' Service d'export Excel
    ''' </summary>
    Public Class ExcelExportService
        ''' <summary>
        ''' Exporte les réservations en Excel
        ''' </summary>
        Public Shared Sub ExportReservationsToExcel(reservations As List(Of Reservation), filePath As String)
            Try
                ' Créer un fichier CSV au format Excel
                Dim lines = New List(Of String)

                ' Header
                lines.Add("Chambre,Nom,Prénom,Civilité,Arrivée,Départ,Adultes,Enfants,Total Personnes,VIP,Notes,Parking")

                ' Données
                For Each res In reservations
                    Dim line = $"""{res.NumerosChambre}"",""{res.NomClient}"",""{res.PrenomClient}"",""{res.CiviliteClient}"",""{res.DateArrivee:dd/MM/yyyy}"",""{res.DateDepart:dd/MM/yyyy}"",""{res.NombreAdultes}"",""{res.NombreEnfants}"",""{res.NombreTotalPersonnes}"",""{If(res.VIP, "OUI", "NON")}"",""{res.Notes}"",""{res.Parking}"""
                    lines.Add(line)
                Next

                System.IO.File.WriteAllLines(filePath, lines, System.Text.Encoding.UTF8)

            Catch ex As Exception
                Throw New Exception($"Erreur export Excel: {ex.Message}")
            End Try
        End Sub
    End Class
End Namespace
