Namespace LuzReception.Models
    ''' <summary>
    ''' Détermine automatiquement la langue et les caractéristiques grammaticales
    ''' </summary>
    Public Class LanguageDetermination
        Public Property Langue As String ' FR, EN, ES
        Public Property Genre As String ' M, F, Mixte
        Public Property Nombre As String ' Sing, Plur
        Public Property Civilite As String
        Public Property Confidence As Double ' 0-1

        Public Sub New()
            Langue = "FR"
            Genre = "M"
            Nombre = "Sing"
            Civilite = "Monsieur"
            Confidence = 0.5
        End Sub
    End Class

    ''' <summary>
    ''' Dictionnaire de civilités multilingues
    ''' </summary>
    Public Class CivilitesDictionnaire
        Public Shared ReadOnly CivilitesFR As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
            {"M", "Monsieur"},
            {"F", "Madame"},
            {"D", "Mademoiselle"},
            {"MF", "Madame et Monsieur"},
            {"MM", "Messieurs"},
            {"FF", "Mesdames"},
            {"DF", "Mademoiselle et Madame"},
            {"DM", "Mademoiselle et Monsieur"},
            {"FAM", "Famille"}
        }

        Public Shared ReadOnly CiviliteEN As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
            {"M", "Mister"},
            {"F", "Mrs."},
            {"D", "Miss"},
            {"MF", "Mrs. and Mr."},
            {"MM", "Gentlemen"},
            {"FF", "Ladies"},
            {"DF", "Miss and Mrs."},
            {"DM", "Miss and Mr."},
            {"FAM", "Family"}
        }

        Public Shared ReadOnly CivilitesES As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
            {"M", "Señor"},
            {"F", "Señora"},
            {"D", "Señorita"},
            {"MF", "Señora y Señor"},
            {"MM", "Señores"},
            {"FF", "Señoras"},
            {"DF", "Señorita y Señora"},
            {"DM", "Señorita y Señor"},
            {"FAM", "Familia"}
        }
    End Class

    ''' <summary>
    ''' Mappe civilités courtes → détectées
    ''' </summary>
    Public Class CiviliteDetectionMap
        Public Shared Function DetectLangueFromCivilite(civilite As String) As String
            If String.IsNullOrEmpty(civilite) Then Return "FR"

            Dim upper = civilite.ToUpper()

            ' Français
            If upper = "M" OrElse upper = "MONSIEUR" OrElse upper = "MR" Then Return "FR"
            If upper = "MME" OrElse upper = "MADAME" Then Return "FR"
            If upper = "MLLE" OrElse upper = "MADEMOISELLE" Then Return "FR"

            ' Anglais
            If upper = "MR" OrElse upper = "MR." OrElse upper = "SIR" Then Return "EN"
            If upper = "MRS" OrElse upper = "MRS." Then Return "EN"
            If upper = "MISS" Then Return "EN"

            ' Espagnol
            If upper = "SR" OrElse upper = "SEÑOR" Then Return "ES"
            If upper = "SRA" OrElse upper = "SEÑORA" Then Return "ES"
            If upper = "SRTA" OrElse upper = "SEÑORITA" Then Return "ES"

            Return "FR" ' Par défaut français
        End Function

        Public Shared Function DetectGenre(civilite As String) As String
            If String.IsNullOrEmpty(civilite) Then Return "M"

            Dim upper = civilite.ToUpper()
            Dim malePatterns = {"M", "MONSIEUR", "MR", "SIR", "SR", "SEÑOR"}
            Dim femalePatterns = {"F", "MME", "MADAME", "MRS", "MLLE", "MADEMOISELLE", "MISS", "SRA", "SEÑORA", "SRTA", "SEÑORITA"}

            If malePatterns.Any(Function(p) upper.Contains(p)) Then Return "M"
            If femalePatterns.Any(Function(p) upper.Contains(p)) Then Return "F"

            Return "M" ' Par défaut masculin
        End Function
    End Class
End Namespace
