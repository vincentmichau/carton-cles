Namespace LuzReception.Models
    ''' <summary>
    ''' Génère les formules de politesse personnalisées multilingues
    ''' </summary>
    Public Class GracefulNessFormula
        ''' <summary>
        ''' Génère la formule de politesse en fonction de la langue, du genre et du nombre
        ''' </summary>
        Public Shared Function GenerateFormula(langue As String, genre As String, nombre As String, noms As List(Of String), accompagnants As List(Of Accompagnant)) As String
            If noms Is Nothing OrElse noms.Count = 0 Then Return ""

            Select Case langue.ToUpper()
                Case "FR"
                    Return GenerateFormulaFR(genre, nombre, noms, accompagnants)
                Case "EN"
                    Return GenerateFormulaEN(genre, nombre, noms, accompagnants)
                Case "ES"
                    Return GenerateFormulaES(genre, nombre, noms, accompagnants)
                Case Else
                    Return GenerateFormulaFR(genre, nombre, noms, accompagnants)
            End Select
        End Function

        Private Shared Function GenerateFormulaFR(genre As String, nombre As String, noms As List(Of String), accompagnants As List(Of Accompagnant)) As String
            If accompagnants Is Nothing OrElse accompagnants.Count = 0 Then
                ' Pas d'accompagnants
                Select Case genre
                    Case "M"
                        Return $"Cher Monsieur {noms(0)}"
                    Case "F"
                        Return $"Chère Madame {noms(0)}"
                    Case Else
                        Return $"Cher Monsieur {noms(0)}"
                End Select
            ElseIf accompagnants.Count = 1 Then
                ' 1 accompagnant
                Dim acc = accompagnants(0)
                Dim sameLastName = String.Equals(noms(0), acc.Nom, StringComparison.OrdinalIgnoreCase)

                If sameLastName Then
                    ' Même nom : Chère Madame et Cher Monsieur NOM (dames d'abord)
                    If genre = "MF" OrElse (genre = "M" AndAlso acc.Genre = "F") OrElse (genre = "F" AndAlso acc.Genre = "M") Then
                        Return $"Chère Madame et Cher Monsieur {noms(0)}"
                    ElseIf genre = "MM" Then
                        Return $"Cher Messieurs {noms(0)}"
                    ElseIf genre = "FF" Then
                        Return $"Chère Mesdames {noms(0)}"
                    End If
                Else
                    ' Noms différents
                    If genre = "MF" OrElse (genre = "M" AndAlso acc.Genre = "F") OrElse (genre = "F" AndAlso acc.Genre = "M") Then
                        Return $"Chère Madame {acc.Nom} et Cher Monsieur {noms(0)}"
                    ElseIf genre = "MM" Then
                        Return $"Cher Monsieur {noms(0)} et Cher Monsieur {acc.Nom}"
                    ElseIf genre = "FF" Then
                        Return $"Chère Madame {noms(0)} et Chère Madame {acc.Nom}"
                    End If
                End If
            Else
                ' Plusieurs accompagnants : Famille
                Return $"Chère Famille {noms(0)}"
            End If

            Return $"Cher Monsieur {noms(0)}"
        End Function

        Private Shared Function GenerateFormulaEN(genre As String, nombre As String, noms As List(Of String), accompagnants As List(Of Accompagnant)) As String
            If accompagnants Is Nothing OrElse accompagnants.Count = 0 Then
                Select Case genre
                    Case "M"
                        Return $"Dear Mr. {noms(0)}"
                    Case "F"
                        Return $"Dear Mrs. {noms(0)}"
                    Case Else
                        Return $"Dear Mr. {noms(0)}"
                End Select
            ElseIf accompagnants.Count = 1 Then
                Dim acc = accompagnants(0)
                Dim sameLastName = String.Equals(noms(0), acc.Nom, StringComparison.OrdinalIgnoreCase)

                If sameLastName Then
                    If genre = "MF" OrElse (genre = "M" AndAlso acc.Genre = "F") OrElse (genre = "F" AndAlso acc.Genre = "M") Then
                        Return $"Dear Mrs. and Mr. {noms(0)}"
                    ElseIf genre = "MM" Then
                        Return $"Dear Gentlemen {noms(0)}"
                    ElseIf genre = "FF" Then
                        Return $"Dear Ladies {noms(0)}"
                    End If
                Else
                    If genre = "MF" OrElse (genre = "M" AndAlso acc.Genre = "F") OrElse (genre = "F" AndAlso acc.Genre = "M") Then
                        Return $"Dear Mrs. {acc.Nom} and Mr. {noms(0)}"
                    ElseIf genre = "MM" Then
                        Return $"Dear Mr. {noms(0)} and Mr. {acc.Nom}"
                    ElseIf genre = "FF" Then
                        Return $"Dear Mrs. {noms(0)} and Mrs. {acc.Nom}"
                    End If
                End If
            Else
                Return $"Dear Family {noms(0)}"
            End If

            Return $"Dear Mr. {noms(0)}"
        End Function

        Private Shared Function GenerateFormulaES(genre As String, nombre As String, noms As List(Of String), accompagnants As List(Of Accompagnant)) As String
            If accompagnants Is Nothing OrElse accompagnants.Count = 0 Then
                Select Case genre
                    Case "M"
                        Return $"Estimado Señor {noms(0)}"
                    Case "F"
                        Return $"Estimada Señora {noms(0)}"
                    Case Else
                        Return $"Estimado Señor {noms(0)}"
                End Select
            ElseIf accompagnants.Count = 1 Then
                Dim acc = accompagnants(0)
                Dim sameLastName = String.Equals(noms(0), acc.Nom, StringComparison.OrdinalIgnoreCase)

                If sameLastName Then
                    If genre = "MF" OrElse (genre = "M" AndAlso acc.Genre = "F") OrElse (genre = "F" AndAlso acc.Genre = "M") Then
                        Return $"Estimada Señora y Estimado Señor {noms(0)}"
                    ElseIf genre = "MM" Then
                        Return $"Estimados Señores {noms(0)}"
                    ElseIf genre = "FF" Then
                        Return $"Estimadas Señoras {noms(0)}"
                    End If
                Else
                    If genre = "MF" OrElse (genre = "M" AndAlso acc.Genre = "F") OrElse (genre = "F" AndAlso acc.Genre = "M") Then
                        Return $"Estimada Señora {acc.Nom} y Estimado Señor {noms(0)}"
                    ElseIf genre = "MM" Then
                        Return $"Estimado Señor {noms(0)} y Estimado Señor {acc.Nom}"
                    ElseIf genre = "FF" Then
                        Return $"Estimada Señora {noms(0)} y Estimada Señora {acc.Nom}"
                    End If
                End If
            Else
                Return $"Estimada Familia {noms(0)}"
            End If

            Return $"Estimado Señor {noms(0)}"
        End Function
    End Class
End Namespace
