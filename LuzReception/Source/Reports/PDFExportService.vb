Imports LuzReception.Models

Namespace LuzReception.Reports
    ''' <summary>
    ''' Service d'export PDF (nécessite iTextSharp)
    ''' </summary>
    Public Class PDFExportService
        ''' <summary>
        ''' Exporte les réservations en PDF
        ''' </summary>
        Public Shared Sub ExportReservationsToPDF(reservations As List(Of Reservation), filePath As String, orientation As String)
            Try
                ' Note: Cette implémentation est basique
                ' En production, utiliser iTextSharp ou PdfSharp
                ' Pour l'instant, nous créons un placeholder

                System.IO.File.WriteAllText(filePath, 
                    $"PDF Export - {reservations.Count} réservations{vbCrLf}" &
                    String.Join(vbCrLf, reservations.Select(Function(r) $"{r.NumerosChambre} - {r.NomClient} - {r.DateArrivee:dd/MM/yyyy}")))

            Catch ex As Exception
                Throw New Exception($"Erreur export PDF: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Exporte une welcome letter en PDF
        ''' </summary>
        Public Shared Sub ExportWelcomLetterToPDF(reservation As Reservation, formulePolitesse As String, welcomeText As String, filePath As String)
            Try
                ' Placeholder pour PDF
                Dim content = $"WELCOME LETTER{vbCrLf}" &
                             $"Saint-Jean-de-Luz, le {reservation.DateArrivee:dd MMMM yyyy}{vbCrLf}{vbCrLf}" &
                             $"{formulePolitesse},{vbCrLf}{vbCrLf}" &
                             $"{welcomeText}{vbCrLf}{vbCrLf}" &
                             $"Bien sincèrement,{vbCrLf}" &
                             $"Véronique Allègre-Concédieu"

                System.IO.File.WriteAllText(filePath, content)
            Catch ex As Exception
                Throw New Exception($"Erreur export welcome PDF: {ex.Message}")
            End Try
        End Sub
    End Class
End Namespace
