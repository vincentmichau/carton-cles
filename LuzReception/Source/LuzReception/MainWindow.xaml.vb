Imports LuzReception.Services
Imports LuzReception.Models
Imports LuzReception.DataAccess

Class MainWindow
    Private _reservationService As ReservationService
    Private _operaImportService As OPERAImportService
    Private _currentDate As Date

    Public Sub New()
        InitializeComponent()
        _reservationService = New ReservationService()
        _operaImportService = New OPERAImportService()
        _currentDate = Date.Today
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Initialiser l'interface
        InitializeUI()
        
        ' Mettre à jour l'heure
        Dim timer = New System.Windows.Threading.DispatcherTimer()
        AddHandler timer.Tick, Sub()
            DateTimeDisplay.Text = $"{Date.Today:dddd dd MMMM yyyy} - {DateTime.Now:HH:mm:ss}"
        End Sub
        timer.Interval = TimeSpan.FromSeconds(1)
        timer.Start()

        ' Charger les réservations
        RefreshReservations()
    End Sub

    Private Sub InitializeUI()
        ' Pré-remplir la date d'arrivée avec aujourd'hui
        TxDateArr.Text = Date.Today.ToString("dd/MM/yyyy")
        TxDateDep.Text = Date.Today.AddDays(1).ToString("dd/MM/yyyy")
        DateFilterDisplay.Text = ReservationService.FormatDateFR(Date.Today)
    End Sub

    Private Sub RefreshReservations()
        Try
            Dim reservations = _reservationService.GetReservationsByDate(_currentDate)
            DgReservations.ItemsSource = reservations

            ' Mettre à jour KPIs
            KpiArrivees.Text = reservations.Count.ToString()
            KpiPersonnes.Text = reservations.Sum(Function(r) r.NombreTotalPersonnes).ToString()

            ' Charger aussi le formulaire si une réservation est sélectionnée
            UpdateFormFromSelection()
        Catch ex As Exception
            StatusText.Text = $"Erreur: {ex.Message}"
        End Try
    End Sub

    Private Sub UpdateFormFromSelection()
        ' À implémenter : remplir le formulaire à partir de la sélection
    End Sub

    ' BOUTONS PRINCIPAUX
    Private Sub BtnImportXML_Click(sender As Object, e As RoutedEventArgs)
        Dim openFile = New System.Windows.Forms.OpenFileDialog With {
            .Filter = "CSV Files (*.csv)|*.csv|TSV Files (*.tsv)|*.tsv|All Files (*.*)|*.*",
            .Title = "Importer fichier OPERA"
        }

        If openFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            StatusText.Text = "Importation en cours..."
            
            AddHandler _operaImportService.ProgressChanged, Sub(pct, msg)
                StatusText.Text = $"[{pct}%] {msg}"
            End Sub

            Dim result = _operaImportService.ImportFromFile(openFile.FileName)
            
            If result.Count > 0 Then
                StatusText.Text = $"✓ Import réussi : {result.Count} réservations"
                RefreshReservations()
            Else
                StatusText.Text = "✗ Erreur lors de l'import"
            End If
        End If
    End Sub

    Private Sub BtnReset_Click(sender As Object, e As RoutedEventArgs)
        If MessageBox.Show("Êtes-vous sûr de vouloir supprimer toutes les réservations ?", 
                          "Confirmation Reset", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
            _reservationService.ResetAllReservations()
            RefreshReservations()
            StatusText.Text = "✓ Données réinitialisées"
        End If
    End Sub

    Private Sub BtnQuitter_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    ' NAVIGATION DATES
    Private Sub BtnPrevDate_Click(sender As Object, e As RoutedEventArgs)
        Dim dates = _reservationService.GetDatesWithReservations()
        Dim currentIdx = dates.IndexOf(_currentDate)
        If currentIdx > 0 Then
            _currentDate = dates(currentIdx - 1)
            DateFilterDisplay.Text = ReservationService.FormatDateFR(_currentDate)
            RefreshReservations()
        End If
    End Sub

    Private Sub BtnNextDate_Click(sender As Object, e As RoutedEventArgs)
        Dim dates = _reservationService.GetDatesWithReservations()
        Dim currentIdx = dates.IndexOf(_currentDate)
        If currentIdx < dates.Count - 1 Then
            _currentDate = dates(currentIdx + 1)
            DateFilterDisplay.Text = ReservationService.FormatDateFR(_currentDate)
            RefreshReservations()
        End If
    End Sub

    ' FORMULAIRE RÉCEPTION - Onglet 1
    Private Sub TxDateArr_TextChanged(sender As Object, e As TextChangedEventArgs)
        ' Mettre à jour la préview
        UpdateCardKeyPreview()
        UpdateWelcomePreview()
    End Sub

    Private Sub UpdateCardKeyPreview()
        Try
            Dim nom = TxNomClient.Text.ToUpper()
            Dim prenom = If(String.IsNullOrEmpty(nom), "?", nom)
            Dim civilite = If(CbCivilite.SelectedItem IsNot Nothing, CbCivilite.SelectedItem.ToString(), "MR").Substring(0, Math.Min(3, CbCivilite.SelectedItem.ToString().Length))
            Dim chambre = TxNChambre.Text
            Dim dateArr = TxDateArr.Text
            Dim dateDep = TxDateDep.Text

            CardKeyTitle.Text = $"{civilite}. {nom}"
            CardKeyRoom.Text = chambre
            CardKeyDates.Text = $"{dateArr} - {dateDep}"
        Catch ex As Exception
            StatusText.Text = $"Erreur preview: {ex.Message}"
        End Try
    End Sub

    Private Sub UpdateWelcomePreview()
        Try
            Dim dateArr = TxDateArr.Text
            Dim formule = TxFormulePolitesse.Text
            Dim texte = LanguageDetectionService.GetWelcomeLetterText("FR", "M", "Sing")
            
            WelcomePreview.Text = $"Saint-Jean-de-Luz, le {dateArr}{vbCrLf}{vbCrLf}{formule},{vbCrLf}{vbCrLf}{texte}"
        Catch ex As Exception
            StatusText.Text = $"Erreur welcome: {ex.Message}"
        End Try
    End Sub

    Private Sub BtnEnregistrer_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim res = New Reservation With {
                .NomClient = TxNomClient.Text.ToUpper(),
                .CiviliteClient = If(CbCivilite.SelectedItem IsNot Nothing, CbCivilite.SelectedItem.ToString(), "Monsieur"),
                .NumerosChambre = TxNChambre.Text,
                .DateArrivee = Date.ParseExact(TxDateArr.Text, "dd/MM/yyyy", Nothing),
                .DateDepart = Date.ParseExact(TxDateDep.Text, "dd/MM/yyyy", Nothing),
                .FormulePolitesse = TxFormulePolitesse.Text,
                .LangueAuto = "FR"
            }

            Dim id = _reservationService.SaveReservation(res)
            If id > 0 Then
                StatusText.Text = $"✓ Réservation enregistrée (ID: {id})"
                RefreshReservations()
                ' Effacer le formulaire
                TxNomClient.Clear()
                TxDateArr.Text = Date.Today.ToString("dd/MM/yyyy")
                TxDateDep.Text = Date.Today.AddDays(1).ToString("dd/MM/yyyy")
            End If
        Catch ex As Exception
            StatusText.Text = $"✗ Erreur enregistrement: {ex.Message}"
        End Try
    End Sub

    Private Sub BtnEffacer_Click(sender As Object, e As RoutedEventArgs)
        TxNomClient.Clear()
        TxNChambre.Clear()
        CbCivilite.SelectedIndex = 0
        TxDateArr.Text = Date.Today.ToString("dd/MM/yyyy")
        TxDateDep.Text = Date.Today.AddDays(1).ToString("dd/MM/yyyy")
        TxFormulePolitesse.Clear()
        StatusText.Text = "Formulaire effacé"
    End Sub

End Class
