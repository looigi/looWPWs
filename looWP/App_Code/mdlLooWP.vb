Imports System.Threading

Module mdlLooWP
    Public LastFileName As List(Of String)
    Public LastSearch As List(Of String)
    Public trd As Thread
    Public NomeCanzoneDaComprimere As String
    Public processoFFMpeg As System.Diagnostics.Process
    Public UltimaChiaveBranoMP3 As String = ""
	Public UltimoRitorno As String = ""
	Public InterrompiElaborazione As Boolean
	Public Quanti As Integer = 0

	Public Function ConverteData(Datella As Date) As String
        Return Datella.Year & "-" & Format(Datella.Month, "00") & "-" & Format(Datella.Day, "00") & " " & Format(Datella.Hour, "00") & ":" & Format(Datella.Minute, "00") & ":" & Format(Datella.Second, "00") & ".000"
    End Function

    Public Function MetteMaiuscole(Cosa As String) As String
        Dim Ritorno As String = Cosa.ToLower.Trim

        If Ritorno <> "" Then
            If Asc(Mid(Ritorno, 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, 1, 1)) <= Asc("z") Then
                Ritorno = Chr(Asc(Mid(Ritorno, 1, 1)) - 32) & Mid(Ritorno, 2, Len(Ritorno))
            End If
            Ritorno = Ritorno.Replace("  ", " ")
            For i As Integer = 2 To Len(Ritorno)
                If Mid(Ritorno, i, 1) = " " Then
                    If Asc(Mid(Ritorno, i + 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, i + 1, 1)) <= Asc("z") Then
                        Ritorno = Mid(Ritorno, 1, i) & Chr(Asc(Mid(Ritorno, i + 1, 1)) - 32) & Mid(Ritorno, i + 2, Len(Ritorno))
                    End If
                End If
            Next
        End If

        Return Ritorno
    End Function

    Public Function SistemaTestoPerDB(Cosa As String) As String
        Dim Ritorno As String = Cosa

        Ritorno = Ritorno.Replace("&#39;", "'").Replace("&#224;", "'")
        Ritorno = Ritorno.Replace("'", "''")

        Return Ritorno
    End Function

End Module
