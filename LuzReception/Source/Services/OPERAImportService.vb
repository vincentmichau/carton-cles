Imports System.Text.RegularExpressions
Imports LuzReception.Models
Imports LuzReception.DataAccess

Namespace LuzReception.Services
    ''' <summary>
    ''' Service pour parser et importer les fichiers OPERA XML/CSV
    ''' </summary>
    Public Class OPERAImportService
        Public Event ProgressChanged(percentage As Integer, message As String)

        ''' <summary>
        ''' Parse un fichier OPERA TSV/CSV
        ''' </summary>
        Public Function ImportFromFile(filePath As String) As List(Of Reservation)
            Dim reservations = New List(Of Reservation)()

            Try
                If Not System.IO.File.Exists(filePath) Then
                    RaiseEvent ProgressChanged(0, "Fichier non trouvé")
                    Return reservations
                End If

                ' Vérifier l'âge du fichier (max 24h)
                Dim fileInfo = New System.IO.FileInfo(filePath)
                Dim fileAge = DateTime.Now - fileInfo.LastWriteTime
                If fileAge.TotalHours > 24 Then
                    RaiseEvent ProgressChanged(0, "Fichier expiré (>24h)")
                    Return reservations
                End If

                RaiseEvent ProgressChanged(10, "Lecture du fichier...")
                Dim lines = System.IO.File.ReadAllLines(filePath)

                If lines.Length = 0 Then
                    RaiseEvent ProgressChanged(0, "Fichier vide")
                    Return reservations
                End If

                ' Détecter le séparateur (TAB ou virgule)
                Dim separator = If(lines(0).Contains(vbTab), vbTab, ",")
                Dim headers = lines(0).Split(New String() {separator}, StringSplitOptions.None)

                ' Trouver les index des colonnes
                Dim colMap = GetColumnMap(headers)
                RaiseEvent ProgressChanged(20, "Parsing des colonnes...")

                ' Parser les lignes
                Dim dedupe = New Dictionary(Of String, Reservation)()
                Dim lineCount = lines.Length - 1

                For i = 1 To lines.Length - 1
                    Try
                        Dim line = lines(i)
                        If String.IsNullOrWhiteSpace(line) Then Continue For

                        Dim fields = line.Split(New String() {separator}, StringSplitOptions.None)
                        Dim res = ParseLine(fields, colMap)

                        If res IsNot Nothing Then
                            ' Dédupliquer par numéro de réservation
                            If Not dedupe.ContainsKey(res.NumeroReservation) Then
                                dedupe(res.NumeroReservation) = res
                            End If
                        End If

                        ' Mise à jour progress
                        Dim progress = 20 + (i * 60 / lineCount)
                        If i Mod 10 = 0 Then
                            RaiseEvent ProgressChanged(progress, $"Parsing ligne {i}/{lineCount}")
                        End If
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine($"Erreur ligne {i}: {ex.Message}")
                    End Try
                Next

                reservations = dedupe.Values.ToList()
                RaiseEvent ProgressChanged(85, $"{reservations.Count} réservations parsées")

                ' Insérer en DB
                RaiseEvent ProgressChanged(90, "Insertion en base...")
                Dim daoFactory = DAOFactory.GetInstance()
                Dim daoRes = daoFactory.GetDAOReservation()

                ' Effacer les anciennes données
                daoRes.DeleteAll()

                For Each res In reservations
                    daoRes.Insert(res)
                Next

                RaiseEvent ProgressChanged(100, "Import terminé avec succès")

            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur import: {ex.Message}")
                RaiseEvent ProgressChanged(0, $"Erreur: {ex.Message}")
            End Try

            Return reservations
        End Function

        Private Function GetColumnMap(headers As String()) As Dictionary(Of String, Integer)
            Dim map = New Dictionary(Of String, Integer)()
            For i = 0 To headers.Length - 1
                map(headers(i).Trim().ToUpper()) = i
            Next
            Return map
        End Function

        Private Function ParseLine(fields As String(), colMap As Dictionary(Of String, Integer)) As Reservation
            Try
                ' Récupérer les champs critiques
                Dim numChambre = GetFieldValue(fields, colMap, "ROOM_NO")
                Dim fullName = GetFieldValue(fields, colMap, "FULL_NAME_NO_SHR_IND")
                Dim arrival = GetFieldValue(fields, colMap, "ARRIVAL")
                Dim departure = GetFieldValue(fields, colMap, "DEPARTURE")

                If String.IsNullOrWhiteSpace(numChambre) OrElse String.IsNullOrWhiteSpace(fullName) Then
                    Return Nothing
                End If

                ' Parser le nom
                Dim nameParts = ParseFullName(fullName)
                Dim numRes = GetFieldValue(fields, colMap, "CONFIRMATION_NO")

                ' Parser les dates
                Dim dateArr = ParseDate(arrival)
                Dim dateDep = ParseDate(departure)

                If dateArr = Date.MinValue OrElse dateDep = Date.MinValue Then
                    Return Nothing
                End If

                ' Autres champs
                Dim vip = GetFieldValue(fields, colMap, "VIP") = "VIP"
                Dim notes = GetFieldValue(fields, colMap, "TRACE_TEXT")
                Dim parking = GetFieldValue(fields, colMap, "RI_NAME")
                Dim adults = ParseInt(GetFieldValue(fields, colMap, "ADULTS"))
                Dim children = ParseInt(GetFieldValue(fields, colMap, "CHILDREN"))
                Dim persons = ParseInt(GetFieldValue(fields, colMap, "PERSONS"))
                Dim paysFacturation = ParseCountry(GetFieldValue(fields, colMap, "BILL_TO_ADDRESS"))
                Dim accompagnants = ParseAccompagnants(GetFieldValue(fields, colMap, "ACCOMPANYING_NAMES"))

                ' Autodétermination langue/civilité
                Dim langDetect = LanguageDetectionService.DetermineLanguage(nameParts.Civilite, paysFacturation)

                ' Créer la réservation
                Dim res = New Reservation With {
                    .NumeroReservation = numRes,
                    .NumerosChambre = numChambre,
                    .NomClient = nameParts.Nom,
                    .PrenomClient = nameParts.Prenom,
                    .CiviliteClient = nameParts.Civilite,
                    .LangueAuto = langDetect.Langue,
                    .GenreGrammatical = langDetect.Genre,
                    .NombrePersonnes = If(accompagnants.Count > 0, "Plur", "Sing"),
                    .DateArrivee = dateArr,
                    .DateDepart = dateDep,
                    .VIP = vip,
                    .Notes = notes,
                    .Parking = parking,
                    .NombreAdultes = adults,
                    .NombreEnfants = children,
                    .NombreTotalPersonnes = persons,
                    .PaysFacturation = paysFacturation,
                    .Accompagnants = accompagnants
                }

                ' Générer formule politesse
                Dim noms = New List(Of String) From {res.NomClient}
                res.FormulePolitesse = GracefulNessFormula.GenerateFormula(res.LangueAuto, res.GenreGrammatical, res.NombrePersonnes, noms, accompagnants)

                Return res
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Erreur ParseLine: {ex.Message}")
                Return Nothing
            End Try
        End Function

        Private Function GetFieldValue(fields As String(), colMap As Dictionary(Of String, Integer), colName As String) As String
            Dim upper = colName.ToUpper()
            If colMap.ContainsKey(upper) Then
                Dim idx = colMap(upper)
                If idx >= 0 AndAlso idx < fields.Length Then
                    Return fields(idx).Trim()
                End If
            End If
            Return ""
        End Function

        Private Function ParseFullName(fullName As String) As (Nom As String, Prenom As String, Civilite As String)
            ' Format: "NOM,Prenom,Civilité"
            Dim parts = fullName.Split(New String() {","}, StringSplitOptions.None)
            Dim nom = If(parts.Length > 0, parts(0).Trim().ToUpper(), "")
            Dim prenom = If(parts.Length > 1, parts(1).Trim(), "")
            Dim civilite = If(parts.Length > 2, parts(2).Trim(), "")

            ' Capitaliser le prénom
            If Not String.IsNullOrEmpty(prenom) Then
                prenom = Char.ToUpper(prenom(0)) + If(prenom.Length > 1, prenom.Substring(1).ToLower(), "")
            End If

            Return (nom, prenom, civilite)
        End Function

        Private Function ParseDate(dateStr As String) As Date
            Try
                If String.IsNullOrWhiteSpace(dateStr) Then Return Date.MinValue
                ' Format attendu: JJ-MM-AAAA
                Dim parts = dateStr.Trim().Split("-"c)
                If parts.Length = 3 Then
                    Dim day = Integer.Parse(parts(0))
                    Dim month = Integer.Parse(parts(1))
                    Dim year = Integer.Parse(parts(2))
                    Return New Date(year, month, day)
                End If
            Catch
            End Try
            Return Date.MinValue
        End Function

        Private Function ParseInt(value As String) As Integer
            Try
                If String.IsNullOrWhiteSpace(value) Then Return 0
                Return Integer.Parse(value.Trim())
            Catch
                Return 0
            End Try
        End Function

        Private Function ParseCountry(address As String) As String
            ' Simple extraction du dernier pays connu
            If String.IsNullOrEmpty(address) Then Return "FR"
            If address.ToUpper().Contains("FRANCE") Then Return "FR"
            If address.ToUpper().Contains("USA") OrElse address.ToUpper().Contains("NEW YORK") Then Return "US"
            If address.ToUpper().Contains("ENGLAND") OrElse address.ToUpper().Contains("UK") Then Return "GB"
            If address.ToUpper().Contains("ESPAÑA") OrElse address.ToUpper().Contains("MADRID") Then Return "ES"
            Return "FR"
        End Function

        Private Function ParseAccompagnants(accompNames As String) As List(Of Accompagnant)
            Dim result = New List(Of Accompagnant)()
            If String.IsNullOrEmpty(accompNames) Then Return result

            ' Format attendu : "NOM,Prenom,Civilité;NOM2,Prenom2,Civilité2"
            Dim accompList = accompNames.Split(";"c)
            For Each acc In accompList
                If Not String.IsNullOrWhiteSpace(acc) Then
                    Dim parts = acc.Split(New String() {","}, StringSplitOptions.None)
                    If parts.Length >= 1 Then
                        Dim nom = parts(0).Trim().ToUpper()
                        Dim prenom = If(parts.Length > 1, parts(1).Trim(), "")
                        Dim civilite = If(parts.Length > 2, parts(2).Trim(), "")

                        If Not String.IsNullOrEmpty(prenom) Then
                            prenom = Char.ToUpper(prenom(0)) + If(prenom.Length > 1, prenom.Substring(1).ToLower(), "")
                        End If

                        Dim accCompagnant = New Accompagnant With {
                            .Nom = nom,
                            .Prenom = prenom,
                            .Civilite = civilite,
                            .Langue = CiviliteDetectionMap.DetectLanguageFromCivilite(civilite),
                            .Genre = CiviliteDetectionMap.DetectGenre(civilite)
                        }
                        result.Add(accCompagnant)
                    End If
                End If
            Next

            Return result
        End Function
    End Class
End Namespace
