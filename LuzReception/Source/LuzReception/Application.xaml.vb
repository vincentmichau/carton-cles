Imports LuzReception.Services
Imports LuzReception.DataAccess

Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        ' Initialiser la base de données
        DatabaseInitializer.Initialize()
        
        ' Pré-charger les services
        Dim svc = New ReservationService()
    End Sub
End Class
