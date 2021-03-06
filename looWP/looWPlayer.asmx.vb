﻿Imports System.Web.Services
Imports System.ComponentModel
Imports System.IO
Imports System.Threading

' Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://looWebPlayer.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class looWPlayer
    Inherits System.Web.Services.WebService

	Public Class MyParameters
		Public Property NomeUtente As String
	End Class

	<WebMethod()>
	Public Function RitornaListaBrani(NomeUtente As String, Artista As String, Album As String, Brano As String,
									  Filtro As String, Refresh As String, Dettagli As String) As String
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Log\")
		Dim l As New Logger
		l.ImpostaFileDiLog(Server.MapPath(".") & "\Log\Logger.txt")

		Dim sArtista As String = Artista.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sAlbum As String = Album.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sBrano As String = Brano.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sFiltro As String = Filtro.Replace("***AND***", "&").Replace("***PI***", "?")

		l.ScriveLogServizio("RitornaListaBrani -> Nome Utente: " & NomeUtente & " - Artista: " & sArtista & " - Album: " & sAlbum & " - Brano: " & sBrano & " - Filtro: " & sFiltro & " - Refresh: " & Refresh & " - Dettagli: " & Dettagli)

		Dim Ritorno As String = ""
		Dim u As New Utility
		Dim Ok As Boolean = False
		Dim PathCartellaOriginale As String = ""

		Dim sPath As String = u.RitornaPercorsiDB
		Dim Path() As String = sPath.Split(";")
		Dim PathMp3_DB As String = Path(1)

		If sArtista <> "" Then
			Path(0) &= sArtista & "\"
		End If
		If sAlbum <> "" Then
			Path(0) &= sAlbum & "\"
		End If
		If sBrano <> "" Then
			Path(0) &= sBrano
		End If

		gf.CreaDirectoryDaPercorso(HttpContext.Current.Server.MapPath(".") & "\Downloads\")

		Dim Search As String = sArtista & ";" & sAlbum & ";" & sBrano & ";" & sFiltro & ";"
		Dim idUtente As Integer = 0

		If LastSearch Is Nothing Then
			LastSearch = New List(Of String)
			LastFileName = New List(Of String)
		End If

		Ok = False
		For Each uu As String In LastSearch
			If uu.ToUpper.Contains(NomeUtente.ToUpper.Trim) Then
				Ok = True
				Exit For
			End If
			idUtente += 1
		Next
		If Not Ok Then
			LastSearch.Add(NomeUtente & ";" & Search)
			LastFileName.Add("")
		End If

		If LastSearch.Item(idUtente) <> Search Then
			LastFileName.Item(idUtente) = ""
			LastSearch.Item(idUtente) = Search
		End If

		If Refresh <> "S" And LastFileName.Item(idUtente) = "" Then
			l.ScriveLogServizio("Nessun refresh e nessuna ricerca. Ricerca file con criteri uguali")
			' Non si è richiesto il refresh e non è mai stata fatta una ricerca. Provo a vedere se esiste un file con gli stessi criteri
			Dim files() As String = IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(".") & "\Downloads\")

			For Each nfile As String In files
				If nfile.Contains(";") Then
					Dim c() As String = nfile.Split(";")

					If c(1).ToUpper.Trim = sArtista.ToUpper.Trim And c(2).ToUpper.Trim = sAlbum.ToUpper.Trim And c(3).ToUpper.Trim = sBrano.ToUpper.Trim And c(4).ToUpper.Trim = sFiltro.ToUpper.Trim Then
						If c(0).ToUpper.Trim <> NomeUtente.ToUpper.Trim Then
							Dim NFileFinale As String = NomeUtente.ToUpper.Trim & ";" & sArtista & ";" & sAlbum & ";" & sBrano & ";" & sFiltro & ";.txt"
							If Not File.Exists(HttpContext.Current.Server.MapPath(".") & "\Downloads\" & NFileFinale) Then
								FileCopy(nfile, HttpContext.Current.Server.MapPath(".") & "\Downloads\" & NFileFinale)
							End If
							LastFileName.Item(idUtente) = NFileFinale
							l.ScriveLogServizio("Trovato: " & NFileFinale)
							Exit For
						End If
					End If
				End If
			Next
		End If

		If Refresh = "S" Or LastFileName.Item(idUtente) = "" Then
			l.ScriveLogServizio("Richiesto refresh")
			Dim CartelleMP3 As New List(Of String)

			l.ScriveLogServizio("Apertura connessione MP3Tag.sdf")

			'Dim mDBCE As New MetodiDbCE
			'Dim NomeDB As String = PathMp3_DB & "MP3Tag.sdf"
			'Dim Rit As String = mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))

			Dim mDBCE As Object
			Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

			If Connessione = "" Then
				l.ScriveLogServizio("ERROR: Connessione non valida")
				Return "ERROR: Connessione non valida"
			Else
				Dim Conn As Object = u.ApreDB(Connessione)

				If TypeOf (Conn) Is String Then
					l.ScriveLogServizio("ERROR: " & Conn)
					Return "ERROR: " & Conn
				Else
					mDBCE = Conn
				End If
			End If

			Dim rec As Object

			Dim PathUtente As String = ""

			l.ScriveLogServizio("Apertura connessione looWebPlayer.sdf")
			'Dim mDBCESito As New MetodiDbCE
			'Dim NomeDBSito As String = HttpContext.Current.Server.MapPath(".") & "\Db\looWebPlayer.sdf"
			'Dim RitSito As String = mDBCESito.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDBSito), gf.TornaNomeFileDaPath(NomeDBSito))
			'If Rit <> "OK" Then
			'	l.ScriveLogServizio("Ritorno non OK: " & Rit)
			'	Return Rit
			'End If

			Dim mDBCESito As Object
			Dim ConnessioneSito As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

			If ConnessioneSito = "" Then
				l.ScriveLogServizio("ERROR: Connessione sito non valida")
				Return "ERROR: Connessione sito non valida"
			Else
				Dim Conn As Object = u.ApreDB(ConnessioneSito)

				If TypeOf (Conn) Is String Then
					l.ScriveLogServizio("ERROR: " & Conn)
					Return "ERROR: " & Conn
				Else
					mDBCESito = Conn
				End If
			End If

			rec = u.LeggeQuery(mDBCESito, "Select * From Utenti Where Utente='" & NomeUtente & "'", ConnessioneSito)
			l.ScriveLogServizio("Query: Select * From Utenti Where Utente='" & NomeUtente & "'")
			If rec Is Nothing Then
				l.ScriveLogServizio("Non trovato nulla")
			Else
				If Not rec.eof Then
					PathUtente = Path(0) & rec("CartellaBase").Value
					CartelleMP3.Add(rec("CartellaBase").Value)

					l.ScriveLogServizio("PathUtente = " & PathUtente)
				End If

				rec.close
			End If
			u.ChiudeDB(True, mdbcesito)

			If PathUtente = "" Then
				PathUtente = Path(0)
			End If
			PathCartellaOriginale = PathUtente.Replace(Path(0), "")

			l.ScriveLogServizio("Scansione directory " & PathUtente)
			gf.ScansionaDirectorySingola(PathUtente)
			Dim Directories() As String = gf.RitornaDirectoryRilevate
			For i As Integer = 1 To gf.RitornaQuanteDirectoryRilevate
				Dim f As String = Directories(i).Replace(PathUtente, "")
				If f <> "" Then
					CartelleMP3.Add(f)
				End If
			Next
			l.ScriveLogServizio("Directory rilevate: " & gf.RitornaQuanteDirectoryRilevate)

			Dim lDirectory As New List(Of String)
			Dim FilesMP3 As New List(Of String)
			Dim Membri As New List(Of String)
			Dim FilesImmagine As New List(Of String)
			Dim FilesVideo As New List(Of String)
			Dim NumeroCartella As Integer = 0
			Dim VecchioArtista As String = ""
			Dim Artisti As New List(Of String)
			Dim NumeroArtisti As Integer = 0
			Dim PathCartella As String = ""

			For Each Cartella As String In CartelleMP3
				If Cartella <> "" Then
					If Strings.Left(Cartella, 1) = "\" Then
						Cartella = Mid(Cartella, 2, Cartella.Length)
					End If

					If PathUtente.Contains("\" & Cartella) Then
						PathCartella = PathUtente
					Else
						PathCartella = PathUtente & "\" & Cartella
					End If

					l.ScriveLogServizio("Lettura " & PathCartella)

					'Dim OkFolder As Boolean = False
					'If Directory.Exists(Path(0) & Cartella) Then
					'    PathCartella = Path(0) & Cartella
					'    PathCartellaOriginale = ""
					'    OkFolder = True
					'Else
					'    For Each Dire As String In DirectoryDiMezzo
					'        If Directory.Exists(Path(0) & Dire & "\" & Cartella) Then
					'            PathCartella = Path(0) & Dire & "\" & Cartella
					'            PathCartellaOriginale = Dire
					'            OkFolder = True
					'            Exit For
					'        End If
					'    Next
					'End If

					'If OkFolder Then
					Dim files() As String = IO.Directory.GetFiles(PathCartella)

					Dim cc2() As String = Cartella.Split("\")
					Dim Art As String = cc2(0)
					'If Art.ToUpper.Contains("MP3") Then
					'    If cc2.Length > 1 Then
					'        Art = cc2(1)
					'    Else
					'        Art = ""
					'    End If
					'End If

					If Art <> VecchioArtista Then
						l.ScriveLogServizio("Nuovo artista: " & Art)
						VecchioArtista = cc2(0)
						Artisti.Add(cc2(0) & ";§")
						NumeroArtisti = Artisti.Count - 1

						Dim rec3 As Object = u.LeggeQuery(mDBCE, "Select * From Members Where Gruppo='" & cc2(0).Replace("'", "''") & "'", Connessione)
						Dim MembriStringa As String = ""
						If rec3 Is Nothing Then
							'Stop
						Else
							Do Until rec3.eof
								MembriStringa &= ";" & NumeroArtisti & ";" & rec3("progressivo").Value.ToString & ";" & rec3("Membro").Value.ToString & ";" & rec3("Durata").Value.ToString & ";" & rec3("Attuale").Value.ToString & "|"

								rec3.movenext
							Loop
							rec3.close
							l.ScriveLogServizio("Nuovo artista. Membri: " & MembriStringa)
						End If
						If MembriStringa = "" Then
							MembriStringa = ";" & NumeroArtisti & ";Nessun membro rilevato;;;|"
							l.ScriveLogServizio("Nuovo artista. Membri: Nessuno")
						End If
						Membri.Add(MembriStringa & ";")

						If Dettagli.ToUpper.Trim = "S" Then
							Dim PathArtista As String = PathCartella
							PathArtista = Mid(PathArtista, 1, PathArtista.IndexOf(VecchioArtista) + VecchioArtista.Length)

							l.ScriveLogServizio("Nuovo artista. Ricerca dettagli path " & PathArtista)

							If Directory.Exists(PathArtista & "\ZZZ-ImmaginiArtista") Then
								Dim filesI() As String = IO.Directory.GetFiles(PathArtista & "\ZZZ-ImmaginiArtista")
								l.ScriveLogServizio("Nuovo artista. Ricerca dettagli. Files rilevati: " & filesI.Length)

								For Each nfile As String In filesI
									nfile = gf.TornaNomeFileDaPath(nfile)

									Dim len As Long = FileLen(PathArtista & "\ZZZ-ImmaginiArtista\" & nfile)
									Dim dat As Date = FileDateTime(PathArtista & "\ZZZ-ImmaginiArtista\" & nfile) ' PathCartella & "\" & nfile)
									Dim sDat As String = Format(dat.Day, "00") & "/" & Format(dat.Month, "00") & "/" & dat.Year & " " & Format(dat.Hour, "00") & ":" & Format(dat.Minute, "00") & ":" & Format(dat.Second, "00")
									If len > 100 Then
										Dim Estensione As String = gf.TornaEstensioneFileDaPath(PathArtista & "\ZZZ-ImmaginiArtista\" & nfile).ToUpper.Trim

										If Estensione = ".JPG" Or Estensione = ".DAT" Then
											FilesImmagine.Add(NumeroCartella & ";" & NumeroArtisti & ";" & "\ZZZ-ImmaginiArtista\" & nfile & ";" & len & ";" & sDat & ";")
											If File.Exists(PathCartella & "\" & nfile) Then
												dat = FileDateTime(PathCartella & "\" & nfile)
												sDat = Format(dat.Day, "00") & "/" & Format(dat.Month, "00") & "/" & dat.Year & " " & Format(dat.Hour, "00") & ":" & Format(dat.Minute, "00") & ":" & Format(dat.Second, "00")
												If len > 100 Then
													Estensione = gf.TornaEstensioneFileDaPath(PathArtista & "\ZZZ-ImmaginiArtista\" & nfile).ToUpper.Trim

													If Estensione = ".JPG" Or Estensione = ".DAT" Then
														FilesImmagine.Add(NumeroCartella & ";" & NumeroArtisti & ";" & "\ZZZ-ImmaginiArtista\" & nfile & ";" & len & ";" & sDat & ";")
													End If

												End If
											End If
										End If
									End If
								Next
							End If
						End If
					End If

					lDirectory.Add(NumeroArtisti & ";" & Cartella.Replace(VecchioArtista & "\", "") & ";")
					NumeroCartella = lDirectory.Count - 1

					l.ScriveLogServizio("Nuovo artista. Ricerca files: " & files.Length)

					For Each nfile As String In files
						nfile = gf.TornaNomeFileDaPath(nfile)
						Dim len As Long = FileLen(PathCartella & "\" & nfile)
						Dim dat As Date = FileDateTime(PathCartella & "\" & nfile)
						Dim sDat As String = Format(dat.Day, "00") & "/" & Format(dat.Month, "00") & "/" & dat.Year & " " & Format(dat.Hour, "00") & ":" & Format(dat.Minute, "00") & ":" & Format(dat.Second, "00")
						If len > 100 Then
							Dim Estensione As String = gf.TornaEstensioneFileDaPath(PathCartella & "\" & nfile).ToUpper.Trim

							If Estensione = ".MP3" Or Estensione = ".WMA" Then
								Dim Altro As String = ""

								If Dettagli.ToUpper.Trim = "S" Then
									Dim rec2 As Object ' = u.LeggeQuery(mDBCE, "Select * From ListaCanzone2 Where Artista='" & cc2(0).Replace("'", "''") & "' And Album='" & cc2(1).Replace("'", "''") & "' And Canzone='" & nfile.Replace("'", "''") & "'", Connessione)
									Dim ssAlbum As String = cc2(1)
									Dim ssAnno As String = ""
									Dim ssTraccia As String = ""
									Dim Canzone As String = nfile
									If ssAlbum.Contains("-") Then
										ssAnno = Mid(ssAlbum, 1, ssAlbum.IndexOf("-"))
										ssAlbum = ssAlbum.Replace(ssAnno & "-", "")
									End If
									If Canzone.Contains("-") Then
										ssTraccia = Mid(Canzone, 1, Canzone.IndexOf("-"))
										Canzone = Canzone.Replace(ssTraccia & "-", "")
									End If
									Dim ssEstensione As String = gf.TornaEstensioneFileDaPath(Canzone)
									Canzone = Canzone.Replace(ssEstensione, "")

									Dim Sql As String = "Select * From ListaCanzone2 " &
										"Where Artista='" & cc2(0).Replace("'", "''") & "' And " &
										"Album='" & ssAlbum.Replace("'", "''") & "' And " &
										"Canzone='" & Canzone.Replace("'", "''") & "' And " &
										"Anno=" & ssAnno & " And " &
										"Traccia=" & ssTraccia & " And " &
										"Estensione='" & ssEstensione.Replace(".", "") & "'"
									rec2 = u.LeggeQuery(mDBCE, Sql, Connessione)
									If rec2 Is Nothing Then
										Altro &= ";;;"
									Else
										If rec2.eof Then
											Altro &= ";;;"
										Else
											Altro &= rec2("Testo").Value.ToString.Replace(";", "**PV**").Replace("§", "**A CAPO**") & ";" & rec2("TestoTradotto").Value.ToString.Replace(";", "**PV**").Replace("§", "**A CAPO**") & ";" & rec2("Ascoltata").Value.ToString & ";" & rec2("Bellezza").Value.ToString
										End If
									End If
								Else
									Altro &= ";;;"
								End If

								FilesMP3.Add(NumeroCartella & ";" & NumeroArtisti & ";" & nfile & ";" & len & ";" & Altro & ";" & sDat & ";")
							Else
								If Estensione = ".MP4" Then
									FilesVideo.Add(NumeroCartella & ";" & NumeroArtisti & ";" & nfile & ";" & len & ";" & sDat & ";")
								Else
									FilesImmagine.Add(NumeroCartella & ";" & NumeroArtisti & ";" & nfile & ";" & len & ";" & sDat & ";")
								End If
							End If
						End If
					Next

					If Dettagli.ToUpper.Trim = "S" Then
						If Directory.Exists(PathCartella & "\VideoYouTube") Then
							Dim filesV() As String = IO.Directory.GetFiles(PathCartella & "\VideoYouTube")

							For Each nfile As String In filesV
								nfile = gf.TornaNomeFileDaPath(nfile)
								Dim len As Long = FileLen(PathCartella & "\VideoYouTube\" & nfile)
								Dim dat As Date = FileDateTime(PathCartella & "\" & nfile)
								Dim sDat As String = Format(dat.Day, "00") & "/" & Format(dat.Month, "00") & "/" & dat.Year & " " & Format(dat.Hour, "00") & ":" & Format(dat.Minute, "00") & ":" & Format(dat.Second, "00")
								If len > 100 Then
									Dim Estensione As String = gf.TornaEstensioneFileDaPath(PathCartella & "\VideoYouTube\" & nfile).ToUpper.Trim

									If Estensione = ".MP4" Then
										FilesVideo.Add(NumeroCartella & ";" & NumeroArtisti & ";" & "\VideoYouTube\" & nfile & ";" & len & ";" & sDat & ";")
									Else
										FilesImmagine.Add(NumeroCartella & ";" & NumeroArtisti & ";" & "\VideoYouTube\" & nfile & ";" & len & ";" & sDat & ";")
									End If
								End If
							Next
						End If
					End If
				End If
				'End If
			Next
			l.ScriveLogServizio("Operazione terminata")

			Ritorno = "***DIRECTORY PRINCIPALE§"
			Ritorno &= PathCartellaOriginale & "§ç"

			Ritorno &= "***DIRECTORIES§"
			Dim message = String.Join("§", lDirectory.ToArray())
			Ritorno &= message
			Ritorno &= "ç"

			Ritorno &= "***ARTISTI§"
			message = String.Join("§", Artisti.ToArray())
			Ritorno &= message
			Ritorno &= "ç"

			Ritorno &= "***MP3§"
			message = String.Join("§", FilesMP3.ToArray())
			Ritorno &= message
			Ritorno &= "ç"

			Ritorno &= "***VIDEO§"
			message = String.Join("§", FilesVideo.ToArray())
			Ritorno &= message
			Ritorno &= "ç"

			Ritorno &= "***IMMAGINI§"
			message = String.Join("§", FilesImmagine.ToArray())
			Ritorno &= message
			Ritorno &= "ç"

			Ritorno &= "***MEMBRI§"
			message = String.Join("§", Membri.ToArray())
			Ritorno &= message
			Ritorno &= "ç"

			Dim NomeFileFinale As String = NomeUtente.ToUpper.Trim & ";" & sArtista & ";" & sAlbum & ";" & sBrano & ";" & sFiltro & ";.txt"

			If File.Exists(HttpContext.Current.Server.MapPath(".") & "\Downloads\" & NomeFileFinale) Then
				Try
					File.Delete(HttpContext.Current.Server.MapPath(".") & "\Downloads\" & NomeFileFinale)
				Catch ex As Exception

				End Try
			End If

			gf.ApreFileDiTestoPerScrittura(HttpContext.Current.Server.MapPath(".") & "\Downloads\" & NomeFileFinale)
			gf.ScriveTestoSuFileAperto(Ritorno)
			gf.ChiudeFileDiTestoDopoScrittura()

			gf = Nothing
			u = Nothing

			Ritorno = "Downloads\" & NomeFileFinale
			LastFileName.Item(idUtente) = Ritorno
		Else
			Ritorno = LastFileName.Item(idUtente)
		End If

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function RitornaDettaglioBrano(Artista As String, Album As String, Brano As String) As String
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Log\")
		Dim l As New Logger
		l.ImpostaFileDiLog(Server.MapPath(".") & "\Log\Logger.txt")

		Dim Ritorno As String = ""
		Dim u As New Utility
		Dim Path() As String = u.RitornaPercorsiDB.Split(";")
		'Dim mDBCE As New MetodiDbCE

		Dim sArtista As String = Artista.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sAlbum As String = Album.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sBrano As String = Brano.Replace("***AND***", "&").Replace("***PI***", "?")

		l.ScriveLogServizio("RitornaDettaglioBrano -> Artista: " & sArtista & " - Album: " & sAlbum & " - Brano: " & sBrano)

		'Dim NomeDB As String = Path(1) & "MP3Tag.sdf"
		'Dim Rit As String = mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
		'If Rit <> "OK" Then
		'	Return Rit
		'End If

		Dim mDBCE As Object
		Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

		If Connessione = "" Then
			l.ScriveLogServizio("ERROR: Connessione sito non valida")
			Return "ERROR: Connessione sito non valida"
		Else
			Dim Conn As Object = u.ApreDB(Connessione)

			If TypeOf (Conn) Is String Then
				l.ScriveLogServizio("ERROR: " & Conn)
				Return "ERROR: " & Conn
			Else
				mDBCE = Conn
			End If
		End If

		Dim rec2 As Object ' = u.LeggeQuery(mDBCE, "Select * From ListaCanzone2 Where Artista='" & sArtista.Replace("'", "''") & "' And Album='" & sAlbum.Replace("'", "''") & "' And Canzone='" & sBrano.Replace("'", "''") & "'", Connessione)
		Dim ssAlbum As String = Album
		Dim ssAnno As String = ""
		Dim ssTraccia As String = ""
		Dim Canzone As String = sBrano
		If ssAlbum.Contains("-") Then
			ssAnno = Mid(ssAlbum, 1, ssAlbum.IndexOf("-"))
			ssAlbum = ssAlbum.Replace(ssAnno & "-", "")
		End If
		If Canzone.Contains("-") Then
			ssTraccia = Mid(Canzone, 1, Canzone.IndexOf("-"))
			Canzone = Canzone.Replace(ssTraccia & "-", "")
		End If
		Dim ssEstensione As String = gf.TornaEstensioneFileDaPath(Canzone)
		Canzone = Canzone.Replace(ssEstensione, "")

		Dim Sql As String = "Select * From ListaCanzone2 " &
										"Where Artista='" & sArtista.Replace("'", "''") & "' And " &
										"Album='" & ssAlbum.Replace("'", "''") & "' And " &
										"Canzone='" & Canzone.Replace("'", "''") & "' And " &
										"Anno=" & ssAnno & " And " &
										"Traccia=" & ssTraccia & " And " &
										"Estensione='" & ssEstensione.Replace(".", "") & "'"
		rec2 = u.LeggeQuery(mDBCE, Sql, Connessione)
		If rec2 Is Nothing Then
			l.ScriveLogServizio("Nessun dettaglio")
			Ritorno &= ";;;;;;"
		Else
			If rec2.eof Then
				l.ScriveLogServizio("Nessun dettaglio")
				Ritorno &= ";;;;;;"
			Else
				l.ScriveLogServizio("Dettaglio rilevato")
				Ritorno &= sArtista & ";" & sAlbum & ";" & sBrano & ";" & rec2("Testo").Value.ToString.Replace(";", "**PV**").Replace("§", "**A CAPO**") & ";" & rec2("TestoTradotto").Value.ToString.Replace(";", "**PV**").Replace("§", "**A CAPO**") & ";" & rec2("Ascoltata").Value.ToString & ";" & rec2("Bellezza").Value.ToString
			End If
		End If
		u.ChiudeDB(True, mDBCE)

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function RitornaMultimediaArtista(PathBase As String, Artista As String) As String
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Log\")
		Dim l As New Logger
		l.ImpostaFileDiLog(Server.MapPath(".") & "\Log\Logger.txt")

		Dim sArtista As String = Artista.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim Ritorno As String = ""
		Dim u As New Utility
		Dim Path() As String = u.RitornaPercorsiDB.Split(";")
		Dim PathArtista As String = Path(0) & PathBase & "\" & sArtista
		Dim FilesImmagine As New List(Of String)
		Dim FilesVideo As New List(Of String)

		PathArtista = PathArtista.Replace("\\", "\")

		l.ScriveLogServizio("RitornaMultimediaArtista -> Artista: " & sArtista & " - Path: " & PathArtista)

		If Directory.Exists(PathArtista & "\ZZZ-ImmaginiArtista") Then
			Dim filesI() As String = IO.Directory.GetFiles(PathArtista & "\ZZZ-ImmaginiArtista")

			l.ScriveLogServizio("Files rilevati: " & filesI.Length)
			For Each nfile As String In filesI
				nfile = gf.TornaNomeFileDaPath(nfile)
				Dim len As Long = FileLen(PathArtista & "\ZZZ-ImmaginiArtista\" & nfile)
				If len > 100 Then
					Dim Estensione As String = gf.TornaEstensioneFileDaPath(PathArtista & "\ZZZ-ImmaginiArtista\" & nfile).ToUpper.Trim

					If Estensione = ".JPG" Or Estensione = ".DAT" Then
						FilesImmagine.Add("\ZZZ-ImmaginiArtista\" & nfile & ";" & len)
					End If
				End If
			Next
		End If

		If Directory.Exists(PathArtista & "\VideoYouTube") Then
			Dim filesV() As String = IO.Directory.GetFiles(PathArtista & "\VideoYouTube")

			l.ScriveLogServizio("Files video rilevati: " & filesV.Length)
			For Each nfile As String In filesV
				nfile = gf.TornaNomeFileDaPath(nfile)
				Dim len As Long = FileLen(PathArtista & "\VideoYouTube\" & nfile)
				If len > 100 Then
					Dim Estensione As String = gf.TornaEstensioneFileDaPath(PathArtista & "\VideoYouTube\" & nfile).ToUpper.Trim

					If Estensione = ".MP3" Or Estensione = ".WMA" Then
						FilesVideo.Add("\VideoYouTube\" & nfile & ";" & len)
					End If
				End If
			Next
		End If
		l.ScriveLogServizio("Operazione eseguita")

		Ritorno &= "***IMMAGINI§"
		For Each f As String In FilesImmagine
			Ritorno &= f & ";§"
		Next
		Ritorno &= "ç"

		Ritorno &= "***VIDEO§"
		For Each f As String In FilesVideo
			Ritorno &= f & ";§"
		Next
		Ritorno &= "ç"

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function RitornaBrano(NomeUtente As String, DirectBase As String, Artista As String, Album As String,
								 Brano As String, Converte As String, Qualita As String, Attendi As Boolean) As String
		Dim gf As New GestioneFilesDirectory
		Dim l As New Logger
		Dim ChiaveAttuale As String = NomeUtente & ";" & Artista & ";" & Album & ";" & Brano & ";" & Converte
		Dim Ritorno As String = ""

		'Thread.Sleep(15000)

		'Ritorno = "ERROR: Errore finto"
		'Return Ritorno

		If ChiaveAttuale = UltimaChiaveBranoMP3 Then
			l.ScriveLogServizio("Brano già in download / conversione")
			Return UltimoRitorno ' "ERROR: brano già in download / conversione"
		End If

		UltimaChiaveBranoMP3 = NomeUtente & ";" & Artista & ";" & Album & ";" & Brano & ";" & Converte

		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Log\")
		l.ImpostaFileDiLog(Server.MapPath(".") & "\Log\Logger.txt")

		Dim StringaConversione As String = "ffmpeg -i 1.mp3 -map 0:a:0 -b:a " & Qualita & "k 2.mp3"

		Dim u As New Utility
		Dim Path() As String = u.RitornaPercorsiDB.Split(";")

		Dim PathCanzone As String

		Dim sArtista As String = Artista.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sAlbum As String = Album.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sBrano As String = Brano.Replace("***AND***", "&").Replace("***PI***", "?")

		l.ScriveLogServizio("RitornaBrano -> Nome Utente: " & NomeUtente & " - Path: " & DirectBase & " - Artista: " & sArtista & " - Album: " & sAlbum & " - Brano: " & sBrano & " - Converte: " & Converte & " - Qualità: " & Qualita)

		PathCanzone = Path(0)
		If DirectBase <> "" Then
			PathCanzone &= DirectBase & "\"
		End If
		If sArtista <> "" And sArtista <> sAlbum And sArtista <> DirectBase Then
			PathCanzone &= sArtista & "\"
		End If
		If sAlbum <> "" And sAlbum <> DirectBase Then
			PathCanzone &= sAlbum & "\"
		End If
		If sBrano <> "" Then
			PathCanzone &= sBrano
		End If

		l.ScriveLogServizio("Path canzone: " & PathCanzone)

		If Converte.ToUpper.Trim = "" Or Converte.ToUpper.Trim = "N" Then
			l.ScriveLogServizio("Conversione: " & Converte)
			If File.Exists(PathCanzone) Then
				Ritorno = "\Normale\" & PathCanzone.Replace(Path(0) & "\", "")

				l.ScriveLogServizio("Brano esistente. Ritorno " & Ritorno)
			Else
				Ritorno = "ERROR: brano non rilevato"

				l.ScriveLogServizio("Brano non esistente")
			End If
		Else
			If Not IsNumeric(Qualita) Then
				Ritorno = "ERROR: qualità non valida -> deve essere numerica e 48 / 96 / 128 /196"

				l.ScriveLogServizio("Qualità non numerica: " & Qualita)
			Else
				If Qualita <> "48" And Qualita <> "96" And Qualita <> "128" And Qualita <> "196" Then
					Ritorno = "ERROR: qualità non valida -> deve essere numerica e 48 / 96 / 128 /196"

					l.ScriveLogServizio("Qualità non valida: " & Qualita)
				Else
					Dim PathCanzoneCompressa As String

					PathCanzoneCompressa = Path(2)
					If DirectBase <> "" Then
						PathCanzoneCompressa &= DirectBase & "\"
					End If
					If sArtista <> "" And sArtista <> sAlbum And sArtista <> DirectBase Then
						PathCanzoneCompressa &= sArtista & "\"
					End If
					If sAlbum <> "" And sAlbum <> DirectBase Then
						PathCanzoneCompressa &= sAlbum & "\"
					End If

					gf.CreaDirectoryDaPercorso(PathCanzoneCompressa.Replace("%20", " "))

					If sBrano <> "" Then
						PathCanzoneCompressa &= sBrano
					End If

					l.ScriveLogServizio("Path canzone compressa:" & PathCanzoneCompressa.Replace("%20", " "))

					If File.Exists(PathCanzoneCompressa.Replace("%20", " ")) Then
						If FileLen(PathCanzoneCompressa.Replace("%20", " ")) < 10000 Then
							l.ScriveLogServizio("Canzone minore di 10K. La elimino")
							File.Delete(PathCanzoneCompressa.Replace("%20", " "))
						End If
					End If

					'If File.Exists(PathCanzoneCompressa.Replace("%20", " ")) Then
					'    Dim Errore As Boolean = False
					'    Dim ss As Integer = 0

					'    Try
					'        Dim Duration As String
					'        Dim w As New WMPLib.WindowsMediaPlayer
					'        Dim m As WMPLib.IWMPMedia = w.newMedia(PathCanzoneCompressa.Replace("%20", " "))
					'        If m IsNot Nothing Then
					'            Duration = m.durationString
					'            If Duration <> "" Then
					'                Dim dd() As String = Duration.Split(":")
					'                If dd.Length > 2 Then
					'                    ss = dd(2)
					'                End If
					'            End If
					'        End If
					'        w.close()
					'    Catch ex As Exception
					'        Errore = True
					'    End Try

					'    If Errore Or ss < 30 Then
					'        File.Delete(PathCanzoneCompressa.Replace("%20", " "))
					'    End If
					'End If

					' Return PathCanzoneCompressa.Replace("%20", " ")

					If File.Exists(PathCanzoneCompressa.Replace("%20", " ")) Then
						Ritorno = "\Compressi\" & PathCanzoneCompressa.Replace(Path(2), "")
						l.ScriveLogServizio("Canzone compressa esistente. La ritorno: " & Ritorno)
					Else
						If File.Exists(PathCanzone.Replace("%20", " ")) Then
							Try
								MkDir(HttpContext.Current.Server.MapPath(".") & "\Temp")
							Catch ex As Exception

							End Try
							gf.CreaAggiornaFile(HttpContext.Current.Server.MapPath(".") & "\Temp\" & NomeUtente & ".txt", Now)
							processoFFMpeg = New Process()
							Dim pi As ProcessStartInfo = New ProcessStartInfo()
							' Qualita = 96 / 128 / 196
							Dim Estensione As String = gf.TornaEstensioneFileDaPath(PathCanzoneCompressa)

							If Not Attendi Then
								PathCanzoneCompressa = PathCanzoneCompressa.Replace(Estensione, "") & "._TMP_" & Estensione
							Else
								PathCanzoneCompressa = PathCanzoneCompressa.Replace(Estensione, "") & Estensione
							End If

							gf.EliminaFileFisico(PathCanzoneCompressa)
							pi.Arguments = "-i " & Chr(34) & PathCanzone & Chr(34) & " -map 0:a:0 -b:a " & Qualita & "k " & Chr(34) & PathCanzoneCompressa.Replace("%20", " ") & Chr(34)
							pi.FileName = HttpContext.Current.Server.MapPath(".") & "\App_Data\ffmpeg.exe"
							pi.WindowStyle = ProcessWindowStyle.Normal
							processoFFMpeg.StartInfo = pi
							processoFFMpeg.Start()

							If Attendi Then
								processoFFMpeg.WaitForExit()
							End If

							NomeCanzoneDaComprimere = PathCanzoneCompressa.Replace("%20", " ")

							l.ScriveLogServizio("Lancio compressione brano: " & NomeCanzoneDaComprimere)
							l.ScriveLogServizio("Parametri: " & pi.Arguments)

							If Not Attendi Then
								trd = New Thread(AddressOf AttesaCompletamento)
								Dim parameters As New MyParameters
								parameters.NomeUtente = NomeUtente
								trd.IsBackground = True
								trd.Start(parameters)
							End If

							'If File.Exists(PathCanzoneCompressa.Replace("%20", " ")) Then
							If Not Attendi Then
								Ritorno = "\Compressi\" & PathCanzoneCompressa.Replace(Path(2), "").Replace("._TMP_", "")
							Else
								Ritorno = "\Compressi\" & PathCanzoneCompressa.Replace(Path(2), "")
							End If

							'Else
							'    Ritorno = "ERROR: brano di destinazione non rilevato"
							'End If
						Else
							Ritorno = "ERROR: brano di origine non rilevato"
							l.ScriveLogServizio("Brano di origine non rilevato: " & PathCanzoneCompressa.Replace("%20", " "))
						End If
					End If
				End If
			End If
		End If
		l.ScriveLogServizio("Fine operazione")

		UltimaChiaveBranoMP3 = ""
		UltimoRitorno = Ritorno

		u = Nothing
		gf = Nothing

		Return Ritorno
	End Function

	Private Sub AttesaCompletamento(ByVal data As Object)
		Dim parameters = CType(data, MyParameters)
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Log\")
		Dim l As New Logger
		l.ImpostaFileDiLog(Server.MapPath(".") & "\Log\Logger.txt")

		l.ScriveLogServizio("Attesa completamento. Inizio")

		Dim Ancora As Boolean = True
		Dim conta As Integer = 0

		While Ancora
			conta += 1
			l.ScriveLogServizio("Attesa completamento. Contatore: " & conta)

			If processoFFMpeg.HasExited Then
				Ancora = False
			End If

			Thread.Sleep(1000)
		End While
		l.ScriveLogServizio("Attesa completamento. Fine")

		File.Copy(NomeCanzoneDaComprimere, NomeCanzoneDaComprimere.Replace("._TMP_", ""))
		l.ScriveLogServizio("Attesa completamento. Pulizia nome canzone: " & NomeCanzoneDaComprimere)
		File.Delete(NomeCanzoneDaComprimere)

		trd.Abort()

		File.Delete(HttpContext.Current.Server.MapPath(".") & "\Temp\" & parameters.NomeUtente & ".txt")
	End Sub

	<WebMethod()>
	Public Function IncrementaAscoltate(NomeUtente As String, Artista As String, Album As String, Brano As String) As String
		Dim u As New Utility
		Dim gf As New GestioneFilesDirectory
		Dim Path() As String = u.RitornaPercorsiDB.Split(";")
		Dim Ritorno As String = ""

		'Dim mDBCE As New MetodiDbCE
		'Dim NomeDB As String = Path(1) & "MP3Tag.sdf"
		Dim Sql As String = "Update ListaCanzone2 Set Ascoltata=Ascoltata+1 Where Artista='" & Artista.Replace("'", "''") & "' And Album='" & Album.Replace("'", "''") & "' And Canzone='" & Brano.Replace("'", "''") & "'"
		Dim mDBCE As New MetodiDbCE
		Dim NomeDB As String = Path(1) & "MP3Tag.sdf"

		Dim ssAlbum As String = Album
		Dim ssAnno As String = ""
		Dim ssTraccia As String = ""
		Dim Canzone As String = Brano
		If ssAlbum.Contains("-") Then
			ssAnno = Mid(ssAlbum, 1, ssAlbum.IndexOf("-"))
			ssAlbum = ssAlbum.Replace(ssAnno & "-", "")
		End If
		If Canzone.Contains("-") Then
			ssTraccia = Mid(Canzone, 1, Canzone.IndexOf("-"))
			Canzone = Canzone.Replace(ssTraccia & "-", "")
		End If
		Dim ssEstensione As String = gf.TornaEstensioneFileDaPath(Canzone)
		Canzone = Canzone.Replace(ssEstensione, "")

		Sql = "Update ListaCanzone2 Set Ascoltata=Ascoltata+1 Where " &
			"Artista='" & Artista.Replace("'", "''") & "' And " &
			"Album='" & ssAlbum.Replace("'", "''") & "' And " &
			"Canzone='" & Canzone.Replace("'", "''") & "' And " &
			"Anno=" & ssAnno & " And " &
			"Traccia=" & ssTraccia & " And " &
			"Estensione='" & ssEstensione & "'"

		Dim Rit As String = ""
		'Rit = mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
		'If Rit <> "OK" Then
		'	Return Rit
		'End If

		'Dim mDBCE As Object
		Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

		If Connessione = "" Then
			Return "ERROR: Connessione sito non valida"
		Else
			Dim Conn As Object = u.ApreDB(Connessione)

			If TypeOf (Conn) Is String Then
				Return "ERROR: " & Conn
			Else
				mDBCE = Conn
			End If
		End If

		Rit = u.EsegueSql(mDBCE, Sql, Connessione)
		If Rit = "OK" Then
			Ritorno = "*"
		Else
			Ritorno = "ERROR: " & Rit
		End If
		u.ChiudeDB(True, mDBCE)

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function SettaStelle(NomeUtente As String, Artista As String, Album As String, Brano As String, Stelle As String) As String
		Dim u As New Utility
		Dim gf As New GestioneFilesDirectory
		Dim Path() As String = u.RitornaPercorsiDB.Split(";")
		Dim Ritorno As String = ""

		'Dim mDBCE As New MetodiDbCE
		'Dim NomeDB As String = Path(1) & "MP3Tag.sdf"
		Dim Sql As String = "Update ListaCanzone2 Set Bellezza=" & Stelle & " Where Artista='" & Artista.Replace("'", "''") & "' And Album='" & Album.Replace("'", "''") & "' And Canzone='" & Brano.Replace("'", "''") & "'"
		Dim mDBCE As New MetodiDbCE
		Dim NomeDB As String = Path(1) & "MP3Tag.sdf"

		Dim ssAlbum As String = Album
		Dim ssAnno As String = ""
		Dim ssTraccia As String = ""
		Dim Canzone As String = Brano
		If ssAlbum.Contains("-") Then
			ssAnno = Mid(ssAlbum, 1, ssAlbum.IndexOf("-"))
			ssAlbum = ssAlbum.Replace(ssAnno & "-", "")
		End If
		If Canzone.Contains("-") Then
			ssTraccia = Mid(Canzone, 1, Canzone.IndexOf("-"))
			Canzone = Canzone.Replace(ssTraccia & "-", "")
		End If
		Dim ssEstensione As String = gf.TornaEstensioneFileDaPath(Canzone)
		Canzone = Canzone.Replace(ssEstensione, "")

		Sql = "Update ListaCanzone2 Set Bellezza=" & Stelle & " Where " &
			"Artista='" & Artista.Replace("'", "''") & "' And " &
			"Album='" & ssAlbum.Replace("'", "''") & "' And " &
			"Canzone='" & Canzone.Replace("'", "''") & "' And " &
			"Traccia=" & ssTraccia & " And " &
			"Anno=" & ssAnno & " And " &
			"Estensione='" & ssEstensione & "'"
		Dim Rit As String = ""
		'Rit = mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
		'If Rit <> "OK" Then
		'	Return Rit
		'End If

		'Dim mDBCE As Object
		Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

		If Connessione = "" Then
			Return "ERROR: Connessione sito non valida"
		Else
			Dim Conn As Object = u.ApreDB(Connessione)

			If TypeOf (Conn) Is String Then
				Return "ERROR: " & Conn
			Else
				mDBCE = Conn
			End If
		End If

		Rit = u.EsegueSql(mDBCE, Sql, Connessione)
		If Rit = "OK" Then
			Ritorno = "*"
		Else
			Ritorno = "ERROR: " & Rit
		End If
		u.ChiudeDB(True, mDBCE)

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function InserisceNuovoUtente(NomeUtente As String, Password As String, Amministratore As String, CartellaBase As String) As String
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Log\")
		Dim l As New Logger
		l.ImpostaFileDiLog(Server.MapPath(".") & "\Log\Logger.txt")

		l.ScriveLogServizio("InserisceNuovoUtente -> NomeUtente: " & NomeUtente & " - Password: " & Password & " - Amministratore: " & Amministratore & " - CartellaBase: " & CartellaBase)
		Dim u As New Utility
		Dim Ritorno As String = ""

		'Dim mDBCE As New MetodiDbCE
		'Dim NomeDB As String = HttpContext.Current.Server.MapPath(".") & "\Db\looWebPlayer.sdf"
		Dim Sql As String = ""
		Dim idUtente As Integer

		Dim Rit As String = "" ' mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
		'If Rit <> "OK" Then
		'	l.ScriveLogServizio("Errore su apertura db: " & Rit)
		'	Return Rit
		'End If

		Dim mDBCE As Object
		Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

		If Connessione = "" Then
			l.ScriveLogServizio("ERROR: Connessione sito non valida")
			Return "ERROR: Connessione sito non valida"
		Else
			Dim Conn As Object = u.ApreDB(Connessione)

			If TypeOf (Conn) Is String Then
				l.ScriveLogServizio("ERROR: " & Conn)
				Return "ERROR: " & Conn
			Else
				mDBCE = Conn
			End If
		End If

		Sql = "Select Max(idUtente)+1 From Utenti"
		Dim rec As Object = u.LeggeQuery(mDBCE, Sql, Connessione)
		If rec Is Nothing Then
			Ritorno = "ERROR: query non valida"
			l.ScriveLogServizio("Query non valida: " & Ritorno)
		Else
			If rec(0).Value Is DBNull.Value Then
				idUtente = 1
			Else
				idUtente = rec(0).Value
			End If
			rec.close

			l.ScriveLogServizio("idUtente: " & idUtente)

			Sql = "Insert Into Utenti Values (" & idUtente & ", '" & NomeUtente.Replace("'", "''") & "', '" & Password.Replace("'", "''") & "', '" & Amministratore.Replace("'", "''") & "', '" & CartellaBase.Replace("'", "''") & "')"
			Rit = u.EsegueSql(mDBCE, Sql, Connessione)
			If Rit = "OK" Then
				Ritorno = "*"
				l.ScriveLogServizio("OK")
			Else
				Ritorno = "ERROR: " & Rit
				l.ScriveLogServizio("Errore: " & Rit)
			End If
		End If
		u.ChiudeDB(True, mDBCE)

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function RitornaDatiUtente(NomeUtente As String) As String
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Log\")
		Dim l As New Logger
		l.ImpostaFileDiLog(Server.MapPath(".") & "\Log\Logger.txt")

		Dim u As New Utility
		Dim Ritorno As String = ""

		l.ScriveLogServizio("Ritorna dati utente: " & NomeUtente)

		'Dim mDBCE As New MetodiDbCE
		'Dim NomeDB As String = HttpContext.Current.Server.MapPath(".") & "\Db\looWebPlayer.sdf"
		Dim Sql As String = "Select * From Utenti Where Upper(LTrim(Rtrim(Utente)))='" & NomeUtente.Replace("'", "''").ToUpper.Trim & "'"
		Dim Rit As String = "" ' mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
		'If Rit <> "OK" Then
		'	l.ScriveLogServizio("Apertura database KO: " & Rit)
		'	Return Rit
		'End If

		Dim mDBCE As Object
		Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

		If Connessione = "" Then
			l.ScriveLogServizio("ERROR: Connessione sito non valida")
			Return "ERROR: Connessione sito non valida"
		Else
			Dim Conn As Object = u.ApreDB(Connessione)

			If TypeOf (Conn) Is String Then
				l.ScriveLogServizio("ERROR: " & Conn)
				Return "ERROR: " & Conn
			Else
				mDBCE = Conn
			End If
		End If

		Dim rec As Object = u.LeggeQuery(mDBCE, Sql, Connessione)
		If rec Is Nothing Then
			Ritorno = "ERROR: query non valida"
			l.ScriveLogServizio("Query non valida: " & Sql)
		Else
			If rec.eof Then
				Ritorno = "ERROR: nessun utente rilevato"
				l.ScriveLogServizio("Nessun utente rilevato")
			Else
				Ritorno = rec("idUtente").Value & ";" & rec("Utente").Value & ";" & rec("Password").Value & ";" & rec("Amministratore").Value & ";" & rec("CartellaBase").Value & ";"
				l.ScriveLogServizio("Utente rilevato: " & Ritorno)
			End If
			rec.close
		End If
		u.ChiudeDB(True, mDBCE)

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function RitornaVersioneApplicazione() As String
		Dim Ritorno As String = ""

		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\NuoveVersioni\")
		Dim NuovaVersione As String = gf.LeggeFileIntero(Server.MapPath(".") & "\NuoveVersioni\Versione.txt")
		If NuovaVersione <> "" Then
			Ritorno = NuovaVersione
		Else
			Ritorno = "ERROR: Nessuna nuova versione rilevata"
		End If

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function RitornaSeDeveAggiornare() As String
		Dim Ritorno As String = ""

		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\Aggiornamenti\")
		Dim VersioneAggiornamento As String = gf.LeggeFileIntero(Server.MapPath(".") & "\Aggiornamenti\UltimoAggiornamento.txt")
		If VersioneAggiornamento <> "" Then
			Ritorno = VersioneAggiornamento
		Else
			Ritorno = "ERROR: Nessun file di aggiornamento rilevato"
		End If

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function CreaAggiornamento() As String
		Dim Ritorno As String = UpdateFileDiAggiornamento(Server.MapPath(".") & "\Aggiornamenti\")
		Return Ritorno
	End Function

	<WebMethod()>
	Public Function EliminaCanzone(PathBase As String, Artista As String, Album As String, Canzone As String) As String
		Dim Ritorno As String = ""

		Dim u As New Utility
		Dim Path() As String = u.RitornaPercorsiDB.Split(";")

		Dim sPath As String = PathBase.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sArtista As String = Artista.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sAlbum As String = Album.Replace("***AND***", "&").Replace("***PI***", "?")
		Dim sBrano As String = Canzone.Replace("***AND***", "&").Replace("***PI***", "?")

		Dim totN As String = Path(0) & PathBase & "\" & sArtista & "\" & sAlbum & "\" & sBrano
		Dim totC As String = Path(2) & PathBase & "\" & sArtista & "\" & sAlbum & "\" & sBrano

		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\DaEliminare\")

		If File.Exists(totN) Then
			'Dim mDBCE As New MetodiDbCE
			'Dim NomeDB As String = Path(1) & "MP3Tag.sdf"
			Dim Sql As String
			Dim Rit As String = ""
			'Rit = mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))

			Dim mDBCE As Object
			Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

			If Connessione = "" Then
				'l.ScriveLogServizio("ERROR: Connessione sito non valida")
				Return "ERROR: Connessione sito non valida"
			Else
				Dim Conn As Object = u.ApreDB(Connessione)

				If TypeOf (Conn) Is String Then
					'l.ScriveLogServizio("ERROR: " & Conn)
					Return "ERROR: " & Conn
				Else
					mDBCE = Conn
				End If
			End If

			If Rit <> "OK" Then
				Ritorno = "ERROR:" & Rit
			Else
				Dim ssAlbum As String = Album
				Dim ssAnno As String = ""
				Dim ssTraccia As String = ""
				Dim ssCanzone As String = Canzone
				If ssAlbum.Contains("-") Then
					ssAnno = Mid(ssAlbum, 1, ssAlbum.IndexOf("-"))
					ssAlbum = ssAlbum.Replace(ssAnno & "-", "")
				End If
				If ssCanzone.Contains("-") Then
					ssTraccia = Mid(ssCanzone, 1, ssCanzone.IndexOf("-"))
					ssCanzone = ssCanzone.Replace(ssTraccia & "-", "")
				End If
				Dim ssEstensione As String = gf.TornaEstensioneFileDaPath(Canzone)
				ssCanzone = ssCanzone.Replace(ssEstensione, "")

				Sql = "Select * From ListaCanzone2 Where " &
					"Artista='" & sArtista.Replace("'", "''") & "' And " &
					"Album='" & ssAlbum.Replace("'", "''") & "' And " &
					"Canzone='" & ssCanzone.Replace("'", "''") & "' And " &
					"Traccia=" & ssTraccia & " And " &
					"Anno=" & ssAnno & " And " &
					"Estensione='" & ssEstensione & "'"

				Dim rec As Object = u.LeggeQuery(mDBCE, Sql, Connessione)
				If rec Is Nothing Then
					Ritorno = "ERROR: query non valida"
				Else
					If Not rec.eof Then
						Dim idBrano As Integer = rec("idCanzone").Value
						rec.close

						gf.ApreFileDiTestoPerScrittura(Server.MapPath(".") & "\DaEliminare\Lista.txt")
						gf.ScriveTestoSuFileAperto(idBrano & ";" & PathBase & ";" & sArtista & ";" & sAlbum & ";" & sBrano & ";|")
						gf.ChiudeFileDiTestoDopoScrittura()

						Sql = "Delete From ListaCanzone2 Where idCanzone = " & idBrano
						Rit = u.EsegueSql(mDBCE, Sql, Connessione)
						If Rit = "OK" Then
							Sql = "Delete From Ascoltate Where idCanzone = " & idBrano
							Rit = u.EsegueSql(mDBCE, Sql, Connessione)

							gf.EliminaFileFisico(totN)

							If File.Exists(totC) Then
								gf.EliminaFileFisico(totC)
							End If

							Ritorno = "*"
						Else
							Ritorno = "ERROR: " & Rit
						End If
						u.ChiudeDB(True, mDBCE)
					End If
				End If

			End If
		Else
			Ritorno = "ERROR: File non trovato: " & totN
		End If

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function RitornaCanzoniDaEliminare() As String
		Dim Ritorno As String = ""
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\DaEliminare\")
		If File.Exists(Server.MapPath(".") & "\DaEliminare\Lista.txt") Then
			Ritorno = gf.LeggeFileIntero(Server.MapPath(".") & "\DaEliminare\Lista.txt")
		Else
			Ritorno = "ERROR: Nessun brano da eliminare"
		End If
		gf = Nothing

		Return Ritorno
	End Function

	<WebMethod()>
	Public Function EliminaListaCanzoniDaEliminare() As String
		Dim Ritorno As String = "*"
		Dim gf As New GestioneFilesDirectory
		gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\DaEliminare\")
		If File.Exists(Server.MapPath(".") & "\DaEliminare\Lista.txt") Then
			gf.EliminaFileFisico(Server.MapPath(".") & "\DaEliminare\Lista.txt")
		End If
		gf = Nothing

		Return Ritorno
	End Function

End Class