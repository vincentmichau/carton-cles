Namespace LuzReception.DataAccess
    ''' <summary>
    ''' Factory pour gérer les DAOs
    ''' </summary>
    Public Class DAOFactory
        Private Shared _instance As DAOFactory
        Private ReadOnly _connectionString As String
        Private _daoReservation As DAOReservation
        Private _daoSettings As DAOSettings

        Private Sub New()
            _connectionString = DatabaseInitializer.GetConnectionString()
            DatabaseInitializer.Initialize()
        End Sub

        ''' <summary>
        ''' Singleton - récupère l'instance unique
        ''' </summary>
        Public Shared Function GetInstance() As DAOFactory
            If _instance Is Nothing Then
                _instance = New DAOFactory()
            End If
            Return _instance
        End Function

        ''' <summary>
        ''' Récupère le DAO Réservations
        ''' </summary>
        Public Function GetDAOReservation() As DAOReservation
            If _daoReservation Is Nothing Then
                _daoReservation = New DAOReservation(_connectionString)
            End If
            Return _daoReservation
        End Function

        ''' <summary>
        ''' Récupère le DAO Settings
        ''' </summary>
        Public Function GetDAOSettings() As DAOSettings
            If _daoSettings Is Nothing Then
                _daoSettings = New DAOSettings(_connectionString)
            End If
            Return _daoSettings
        End Function

        Public ReadOnly Property ConnectionString As String
            Get
                Return _connectionString
            End Get
        End Property
    End Class
End Namespace
