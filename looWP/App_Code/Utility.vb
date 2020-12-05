Public Class Utility
	Public StringaErrore As String = "ERROR: "

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

	Public Function ApreDB(ByVal Connessione As String) As Object
		' Routine che apre il DB e vede se ci sono errori
		Dim Conn As Object = CreateObject("ADODB.Connection")

		Try
			Conn.Open(Connessione)
			Conn.CommandTimeout = 0
		Catch ex As Exception
			Conn = StringaErrore & " " & ex.Message
		End Try

		Return Conn
	End Function

	Public Function EsegueSql(ByVal Conn As Object, ByVal Sql As String, ByVal Connessione As String) As String
		Dim AperturaManuale As Boolean = ControllaAperturaConnessione(Conn, Connessione)
		Dim Ritorno As String = "*"

		' Routine che esegue una query sul db
		Try
			Conn.Execute(Sql)
		Catch ex As Exception
			Ritorno = StringaErrore & " " & ex.Message
		End Try

		ChiudeDB(AperturaManuale, Conn)

		Return Ritorno
	End Function

	Public Function RitornaPercorsiDB(Optional path As String = "") As String
		'Dim mDBCE As New MetodiDbCE
		'Dim gf As New GestioneFilesDirectory
		'If path = "" Then
		'	path = HttpContext.Current.Server.MapPath(".")
		'End If
		'Dim NomeDB As String = path & "\Db\looWebPlayer.sdf"
		'Dim Rit As String = mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
		'If Rit <> "OK" Then
		'	Return Rit
		'End If

		'Dim rec As Object = mDBCE.RitornaRecordset("Select * From Percorsi")

		Dim Ritorno As String = ""
		Dim Connessione As String = LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

		If Connessione = "" Then
			Ritorno = "ERROR: Connessione non valida"
		Else
			Dim Conn As Object = ApreDB(Connessione)

			If TypeOf (Conn) Is String Then
				Ritorno = "Error:" & Conn
			Else
				'If SoloNuove = "S" Then
				'	Dim m As New mailImap
				'	Dim Ritorno2 As String = m.RitornaMessaggi(Squadra, idAnno, idUtente, Folder)
				'End If

				Dim Rec As Object = HttpContext.Current.Server.CreateObject("ADODB.Recordset")
				Dim Rec2 As Object = HttpContext.Current.Server.CreateObject("ADODB.Recordset")
				Dim Sql As String = "Select * From Percorsi"

				Dim PathPrincipale As String = ""
				Dim PathMp3_Db As String = ""
				Dim PathCompressi As String = ""

				Rec = LeggeQuery(Conn, Sql, Connessione)
				If TypeOf (Rec) Is String Then
					Ritorno = Rec
				Else
					If Rec.Eof Then
						Ritorno = StringaErrore & " Nessuna mail ritornata"
					Else
						PathPrincipale = Rec(0).Value
						PathMp3_Db = Rec(1).Value
						PathCompressi = Rec(2).Value

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

					Rec.close
				End If
			End If
		End If


		Return Ritorno
	End Function

	Private Function ControllaAperturaConnessione(ByRef Conn As Object, ByVal Connessione As String) As Boolean
		Dim Ritorno As Boolean = False

		If Conn Is Nothing Then
			Ritorno = True
			Conn = ApreDB(Connessione)
		End If

		Return Ritorno
	End Function

	Public Sub ChiudeDB(ByVal TipoApertura As Boolean, ByRef Conn As Object)
		If TipoApertura = True Then
			Conn.Close()
		End If
	End Sub

	Public Function LeggeQuery(ByVal Conn As Object, ByVal Sql As String, ByVal Connessione As String) As Object
		Dim AperturaManuale As Boolean = ControllaAperturaConnessione(Conn, Connessione)
		Dim Rec As Object = CreateObject("ADODB.Recordset")

		Try
			Rec.Open(Sql, Conn)
		Catch ex As Exception
			Rec = StringaErrore & " " & ex.Message
		End Try

		ChiudeDB(AperturaManuale, Conn)

		Return Rec
	End Function

	Public Function LeggeImpostazioniDiBase(Percorso As String) As String
		Dim Connessione As String = ""

		' Impostazioni di base
		Dim ListaConnessioni As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings

		If ListaConnessioni.Count <> 0 Then
			' Get the collection elements. 
			For Each Connessioni As ConnectionStringSettings In ListaConnessioni
				Dim Nome As String = Connessioni.Name
				Dim Provider As String = Connessioni.ProviderName
				Dim connectionString As String = Connessioni.ConnectionString

				If Nome = "SQLConnectionStringLOCALE" Then
					Connessione = "Provider=" & Provider & ";" & connectionString
					Connessione = Replace(Connessione, "*^*^*", Percorso & "\")
					Exit For
				End If
			Next
		End If

		Return Connessione
	End Function

End Class
