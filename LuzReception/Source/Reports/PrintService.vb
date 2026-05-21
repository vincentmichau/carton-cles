Imports System.Drawing
Imports System.Drawing.Printing
Imports LuzReception.Models

Namespace LuzReception.Reports
    ''' <summary>
    ''' Service d'impression pour cartons clé et welcome letters
    ''' </summary>
    Public Class PrintService
        ''' <summary>
        ''' Imprime un carton clé
        ''' </summary>
        Public Shared Sub PrintCardKey(reservation As Reservation, printerName As String)
            Try
                Dim pd = New PrintDocument()
                pd.PrinterSettings.PrinterName = printerName
                pd.DefaultPageSettings.PaperSize = New PaperSize("A6", 423, 298) ' A6 en 100èmes de mm

                ' Ajouter l'événement d'impression
                AddHandler pd.PrintPage, Sub(sender, e)
                    ' Font
                    Dim fontTitle = New Font("MVBoli", 10, FontStyle.Bold)
                    Dim fontRoom = New Font("MVBoli", 36, FontStyle.Bold)
                    Dim fontDates = New Font("MVBoli", 9)

                    ' Positions (coordonnées absolues pour A6)
                    Dim x = 10, y = 10

                    ' Civilité courte + NOM
                    Dim civiliteCourtLabel = GetCiviliteCourtLabel(reservation.CiviliteClient)
                    e.Graphics.DrawString($"{civiliteCourtLabel}. {reservation.NomClient}", fontTitle, Brushes.Black, x, y)

                    ' N° chambre (centré)
                    Dim roomRect = New RectangleF(x, y + 50, 200, 100)
                    Dim sf = New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    e.Graphics.DrawString(reservation.NumerosChambre, fontRoom, Brushes.Black, roomRect, sf)

                    ' Dates
                    e.Graphics.DrawString($"{reservation.DateArrivee:dd/MM/yyyy} - {reservation.DateDepart:dd/MM/yyyy}", fontDates, Brushes.Black, x, y + 150)

                    e.HasMorePages = False
                End Sub

                pd.Print()
            Catch ex As Exception
                Throw New Exception($"Erreur impression carton: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Imprime une welcome letter
        ''' </summary>
        Public Shared Sub PrintWelcomeLetter(reservation As Reservation, formulePolitesse As String, welcomeText As String, printerName As String)
            Try
                Dim pd = New PrintDocument()
                pd.PrinterSettings.PrinterName = printerName
                pd.DefaultPageSettings.PaperSize = New PaperSize("DL", 220, 110) ' Enveloppe DL en mm

                AddHandler pd.PrintPage, Sub(sender, e)
                    Dim fontHeader = New Font("Aptos", 9)
                    Dim fontFormula = New Font("Aptos", 11, FontStyle.Bold)
                    Dim fontBody = New Font("Aptos", 11)
                    Dim fontSignature = New Font("Aptos", 9)

                    ' Marges
                    Dim left = 50, top = 50, right = 150

                    ' En-tête : Ville, Date
                    Dim city = "Saint-Jean-de-Luz"
                    Dim dateStr = $"le {reservation.DateArrivee:dd MMMM yyyy}"
                    e.Graphics.DrawString($"{city}, {dateStr}", fontHeader, Brushes.Black, right, top)

                    ' Formule politesse
                    e.Graphics.DrawString(formulePolitesse, fontFormula, Brushes.Black, left, top + 80)

                    ' Corps de lettre
                    Dim bodyRect = New RectangleF(left, top + 120, 800, 200)
                    e.Graphics.DrawString(welcomeText, fontBody, Brushes.Black, bodyRect)

                    ' Signature
                    e.Graphics.DrawString("Bien sincèrement,", fontSignature, Brushes.Black, left, top + 320)
                    e.Graphics.DrawString("Véronique Allègre-Concédieu", New Font("Aptos", 10, FontStyle.Bold), Brushes.Black, left, top + 360)

                    e.HasMorePages = False
                End Sub

                pd.Print()
            Catch ex As Exception
                Throw New Exception($"Erreur impression welcome: {ex.Message}")
            End Try
        End Sub

        Private Shared Function GetCiviliteCourtLabel(civilite As String) As String
            If String.IsNullOrEmpty(civilite) Then Return "M"
            Dim upper = civilite.ToUpper()
            If upper.Contains("MME") OrElse upper.Contains("MADAME") OrElse upper.Contains("MRS") Then Return "Mme"
            If upper.Contains("MLLE") OrElse upper.Contains("MADEMOISELLE") OrElse upper.Contains("MISS") Then Return "Mlle"
            Return "M"
        End Function
    End Class
End Namespace
