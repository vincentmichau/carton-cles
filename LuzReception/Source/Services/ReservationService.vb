Imports LuzReception.Models
Imports LuzReception.DataAccess

Namespace LuzReception.Services
    ''' <summary>
    ''' Service métier pour les réservations
    ''' </summary>
    Public Class ReservationService
        Private ReadOnly _daoFactory As DAOFactory
        Private ReadOnly _daoReservation As DAOReservation

        Public Sub New()
            _daoFactory = DAOFactory.GetInstance()
            _daoReservation = _daoFactory.GetDAOReservation()
        End Sub

        ''' <summary>
        ''' Récupère toutes les réservations
        ''' </summary>
        Public Function GetAllReservations() As List(Of Reservation)
            Return _daoReservation.GetAll()
        End Function

        ''' <summary>
        ''' Récupère les réservations d'une date donnée
        ''' </summary>
        Public Function GetReservationsByDate(dateArrivee As Date) As List(Of Reservation)
            Return _daoReservation.GetByDate(dateArrivee)
        End Function

        ''' <summary>
        ''' Enregistre une nouvelle réservation
        ''' </summary>
        Public Function SaveReservation(reservation As Reservation) As Integer
            If ValidateReservation(reservation) Then
                If reservation.Id = 0 Then
                    ' Nouvelle réservation
                    Return _daoReservation.Insert(reservation)
                Else
                    ' Mise à jour
                    _daoReservation.Update(reservation)
                    Return reservation.Id
                End If
            End If
            Return 0
        End Function

        ''' <summary>
        ''' Valide une réservation
        ''' </summary>
        Private Function ValidateReservation(reservation As Reservation) As Boolean
            If String.IsNullOrEmpty(reservation.NomClient) Then Return False
            If String.IsNullOrEmpty(reservation.NumerosChambre) Then Return False
            If reservation.DateArrivee = Date.MinValue OrElse reservation.DateDepart = Date.MinValue Then Return False
            If reservation.DateDepart < reservation.DateArrivee Then Return False
            If reservation.DateArrivee < Date.Today Then Return False

            ' Valider le numéro de chambre
            If Not IsValidRoomNumber(reservation.NumerosChambre) Then
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' Valide le numéro de chambre
        ''' </summary>
        Public Shared Function IsValidRoomNumber(roomNumber As String) As Boolean
            Try
                Dim num = Integer.Parse(roomNumber)
                ' Valider: 11,12,14,15, 101-117, 201-217, 301-317
                If num = 11 OrElse num = 12 OrElse num = 14 OrElse num = 15 Then Return True
                If num >= 101 AndAlso num <= 117 Then Return True
                If num >= 201 AndAlso num <= 217 Then Return True
                If num >= 301 AndAlso num <= 317 Then Return True
            Catch
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Supprime toutes les réservations
        ''' </summary>
        Public Sub ResetAllReservations()
            _daoReservation.DeleteAll()
        End Sub

        ''' <summary>
        ''' Obtient les dates avec réservations
        ''' </summary>
        Public Function GetDatesWithReservations() As List(Of Date)
            Dim reservations = GetAllReservations()
            Dim dates = (From r In reservations Order By r.DateArrivee Select r.DateArrivee Distinct).ToList()
            Return dates
        End Function

        ''' <summary>
        ''' Formatte une date en texte français
        ''' </summary>
        Public Shared Function FormatDateFR(d As Date) As String
            Dim jours() = {"Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"}
            Dim mois() = {"janvier", "février", "mars", "avril", "mai", "juin", "juillet", "août", "septembre", "octobre", "novembre", "décembre"}
            Return $"{jours(CInt(d.DayOfWeek))} {d:dd} {mois(d.Month - 1)} {d:yyyy}"
        End Function

        ''' <summary>
        ''' Formatte une date en texte anglais
        ''' </summary>
        Public Shared Function FormatDateEN(d As Date) As String
            Dim ordinals = New Dictionary(Of Integer, String) From {
                {1, "st"}, {21, "st"}, {31, "st"},
                {2, "nd"}, {22, "nd"},
                {3, "rd"}, {23, "rd"}
            }
            Dim suffix = If(ordinals.ContainsKey(d.Day), ordinals(d.Day), "th")
            Dim mois() = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"}
            Return $"{mois(d.Month - 1)} {d.Day}{suffix}, {d:yyyy}"
        End Function

        ''' <summary>
        ''' Formatte une date en texte espagnol
        ''' </summary>
        Public Shared Function FormatDateES(d As Date) As String
            Dim mois() = {"enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"}
            Return $"{d:dd} de {mois(d.Month - 1)} de {d:yyyy}"
        End Function
    End Class
End Namespace
