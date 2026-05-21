Namespace LuzReception.Models
    ''' <summary>
    ''' Entité représentant une réservation
    ''' </summary>
    Public Class Reservation
        Public Property Id As Integer
        Public Property NumeroReservation As String
        Public Property NumerosChambre As String
        Public Property NomClient As String
        Public Property PrenomClient As String
        Public Property CiviliteClient As String
        Public Property LangueAuto As String
        Public Property GenreGrammatical As String ' M, F, Mixte
        Public Property NombrePersonnes As String ' Sing, Plur
        Public Property DateArrivee As Date
        Public Property DateDepart As Date
        Public Property VIP As Boolean
        Public Property Notes As String
        Public Property Traces As String
        Public Property Parking As String
        Public Property NombreAdultes As Integer
        Public Property NombreEnfants As Integer
        Public Property NombreTotalPersonnes As Integer
        Public Property FormulePolitesse As String
        Public Property Accompagnants As List(Of Accompagnant)
        Public Property PaysFacturation As String
        Public Property DateModification As Date
        Public Property UtilisateurModification As String

        Public Sub New()
            Accompagnants = New List(Of Accompagnant)()
            DateModification = Now
        End Sub

        Public Overrides Function ToString() As String
            Return $"{NomClient} {PrenomClient} - Chambre {NumerosChambre}"
        End Function
    End Class
End Namespace
