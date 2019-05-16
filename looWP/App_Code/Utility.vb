Public Class Utility
	'Public Function LeggePercorso() As String
	'    Dim NomeFile As String = HttpContext.Current.Server.MapPath(".") & "\pathMp3.txt"

	'    If IO.File.Exists(NomeFile) Then
	'        Dim gf As New GestioneFilesDirectory
	'        Dim Percorso As String = gf.LeggeFileIntero(NomeFile)
	'        gf = Nothing

	'        Return Percorso
	'    Else
	'        Return "ERROR: nessun file configurazione path mp3"
	'    End If
	'End Function

	'Public Function RitornaPercorsi() As String
	'    Dim PathPrinc As String = LeggePercorso()
	'    Dim pp() As String = PathPrinc.Split(";")

	'    Dim PathPrincipale As String = pp(0)
	'    Dim PathMp3_Db As String = pp(1)
	'    Dim PathCompressi As String = pp(2)

	'    If Strings.Right(PathPrincipale, 1) <> "\" Then
	'        PathPrincipale &= "\"
	'    End If

	'    If Strings.Right(PathMp3_Db, 1) <> "\" Then
	'        PathMp3_Db &= "\"
	'    End If

	'    If Strings.Right(PathCompressi, 1) <> "\" Then
	'        PathCompressi &= "\"
	'    End If

	'    Return PathPrincipale & ";" & PathMp3_Db & ";" & PathCompressi
	'End Function

	Public Function RitornaPercorsiDB(Optional path As String = "") As String
		Dim mDBCE As New MetodiDbCE
		Dim gf As New GestioneFilesDirectory
		If path = "" Then
			path = HttpContext.Current.Server.MapPath(".")
		End If
		Dim NomeDB As String = path & "\Db\looWebPlayer.sdf"
		Dim Rit As String = mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
		If Rit <> "OK" Then
			Return Rit
		End If

		Dim rec As Object = mDBCE.RitornaRecordset("Select * From Percorsi")

		Dim PathPrincipale As String = ""
		Dim PathMp3_Db As String = ""
		Dim PathCompressi As String = ""
		Dim Ritorno As String = ""

		If rec Is Nothing Then
			Ritorno = "ERROR: Query non valida"
		Else
			If rec.eof Then
				Ritorno = "ERROR: Nessun valore per la tabella dei percorsi"
			Else
				PathPrincipale = rec(0).Value
				PathMp3_Db = rec(1).Value
				PathCompressi = rec(2).Value

				If Strings.Right(PathPrincipale, 1) <> "\" Then
					PathPrincipale &= "\"
				End If

				If Strings.Right(PathMp3_Db, 1) <> "\" Then
					PathMp3_Db &= "\"
				End If

				If Strings.Right(PathCompressi, 1) <> "\" Then
					PathCompressi &= "\"
				End If
			End If
			Ritorno = PathPrincipale & ";" & PathMp3_Db & ";" & PathCompressi

			rec.close
		End If


		Return Ritorno
	End Function

End Class
