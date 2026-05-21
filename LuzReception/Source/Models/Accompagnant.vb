Namespace LuzReception.Models
    ''' <summary>
    ''' Entité représentant un accompagnant dans une réservation
    ''' </summary>
    Public Class Accompagnant
        Public Property Id As Integer
        Public Property ReservationId As Integer
        Public Property Nom As String
        Public Property Prenom As String
        Public Property Civilite As String
        Public Property Langue As String
        Public Property Genre As String

        Public Overrides Function ToString() As String
            Return $"{Nom} {Prenom} ({Civilite})"
        End Function
    End Class
End Namespace
