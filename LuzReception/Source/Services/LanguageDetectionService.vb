Imports LuzReception.Models

Namespace LuzReception.Services
    ''' <summary>
    ''' Service d'autodétermination de la langue et de la civilité
    ''' </summary>
    Public Class LanguageDetectionService
        ''' <summary>
        ''' Détermine automatiquement la langue et les caractéristiques grammaticales
        ''' </summary>
        Public Shared Function DetermineLanguage(civilite As String, paysFacturation As String) As LanguageDetermination
            Dim result = New LanguageDetermination()

            ' Étape 1 : Déterminer la langue à partir de la civilité
            Dim langFromCivilite = CiviliteDetectionMap.DetectLanguageFromCivilite(civilite)
            result.Langue = langFromCivilite
            result.Genre = CiviliteDetectionMap.DetectGenre(civilite)
            result.Confidence = 0.9

            ' Étape 2 : Fallback au pays si civilité incertaine
            If result.Confidence < 0.5 AndAlso Not String.IsNullOrEmpty(paysFacturation) Then
                result.Langue = DetermineLangFromCountry(paysFacturation)
                result.Confidence = 0.6
            End If

            ' Étape 3 : Déterminer la civilité adaptée
            result.Civilite = GetLocalizedCivilite(result.Genre, result.Langue)

            Return result
        End Function

        Private Shared Function DetermineLangFromCountry(pays As String) As String
            If String.IsNullOrEmpty(pays) Then Return "FR"

            Select Case pays.ToUpper()
                Case "FR", "FRANCE"
                    Return "FR"
                Case "GB", "UK", "ENGLAND", "UNITED KINGDOM", "US", "USA", "UNITED STATES"
                    Return "EN"
                Case "ES", "ESPAÑA", "SPAIN", "ESPAGNE"
                    Return "ES"
                Case Else
                    Return "FR" ' Par défaut français
            End Select
        End Function

        Private Shared Function GetLocalizedCivilite(genre As String, langue As String) As String
            Select Case langue.ToUpper()
                Case "FR"
                    Return If(genre = "F", "Madame", "Monsieur")
                Case "EN"
                    Return If(genre = "F", "Mrs.", "Mr.")
                Case "ES"
                    Return If(genre = "F", "Señora", "Señor")
                Case Else
                    Return "Monsieur"
            End Select
        End Function

        ''' <summary>
        ''' Récupère le texte Welcome Letter adaptée à la langue et au genre
        ''' </summary>
        Public Shared Function GetWelcomeLetterText(langue As String, genre As String, nombre As String) As String
            Select Case langue.ToUpper()
                Case "FR"
                    Return GetWelcomeTextFR(genre, nombre)
                Case "EN"
                    Return GetWelcomeTextEN(genre, nombre)
                Case "ES"
                    Return GetWelcomeTextES(genre, nombre)
                Case Else
                    Return GetWelcomeTextFR(genre, nombre)
            End Select
        End Function

        Private Shared Function GetWelcomeTextFR(genre As String, nombre As String) As String
            Dim text = "La direction et l'ensemble du personnel sont sensibles à l'honneur de vous accueillir comme hôte et sont heureux de vous souhaiter un agréable séjour à Saint-Jean-de-Luz."
            text += vbCrLf + vbCrLf + "N'hésitez pas à nous faire part de vos désirs afin de nous aider à mieux vous servir."
            text += vbCrLf + vbCrLf + "Nous vous souhaitons un agréable séjour Luzien !"
            Return text
        End Function

        Private Shared Function GetWelcomeTextEN(genre As String, nombre As String) As String
            Dim text = "The Grand Hotel's management and its entire team are honoured to have you as a guest, and we are also delighted to welcome you in St Jean de Luz."
            text += vbCrLf + vbCrLf + "Do not hesitate to express any special wishes you may have for us to better serve you during your stay."
            text += vbCrLf + vbCrLf + "We wish you a wonderful time in the Basque Country."
            Return text
        End Function

        Private Shared Function GetWelcomeTextES(genre As String, nombre As String) As String
            Dim text = "La Dirección así como todo el personal están muy honrados de tenerle como huésped."
            text += vbCrLf + vbCrLf + "Le deseamos una buena estancia en San Juan de Luz."
            text += vbCrLf + vbCrLf + "No dude en compartir con nosotros sus deseos para procurarle un mejor servicio."
            text += vbCrLf + vbCrLf + "Deseándole un agradable momento en el País Vasco."
            Return text
        End Function
    End Class
End Namespace
