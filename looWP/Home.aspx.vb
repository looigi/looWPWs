Imports System.IO
Imports System.Threading
Imports System.Windows.Forms

Public Class Home
	Inherits System.Web.UI.Page

	Private trd As Thread
	Protected Shared content As String
	Protected Shared inProcess As Boolean = False

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		cmdAnnulla.Visible = False
		cmdComprime.Visible = True
		lblCompressi.Text = ""
	End Sub

	Private Sub ComprimeFiles()
		Dim u As New Utility
		Dim Path() As String = u.RitornaPercorsiDB(Server.MapPath(".")).Split(";")
		Dim PathMP3 As String = Path(0)
		Dim PathDB As String = Path(1)
		Dim PathCompressi As String = Path(2)
		Dim gf As New GestioneFilesDirectory

		' ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script" & Now.ToString.Trim, "changeText('Lettura directory');", True)
		content = "Lettura directory"

		gf.ScansionaDirectorySingola(PathMP3)
		Dim filetti() As String = gf.RitornaFilesRilevati
		Dim qFiletti As Integer = gf.RitornaQuantiFilesRilevati

		For i As Integer = 1 To qFiletti
			If Not InterrompiElaborazione Then
				If filetti(i).ToUpper.Contains(".MP3") Or filetti(i).ToUpper.Contains(".WMA") Then
					Dim Dire As String = filetti(i).Replace(PathMP3, "")
					Dire = gf.TornaNomeDirectoryDaPath(Dire)
					Dim NomeMp3Compresso As String = PathCompressi & Dire & "\" & gf.TornaNomeFileDaPath(filetti(i))
					If Not File.Exists(NomeMp3Compresso) Then
						' hdnText.Value = "Compressione " & i & "/" & qFiletti & ": " & Dire & "\" & gf.TornaNomeFileDaPath(filetti(i))
						content = "Compressione " & i & "/" & qFiletti & ": " & Dire & "\" & gf.TornaNomeFileDaPath(filetti(i))
						' ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script" & Now.ToString.Trim, "changeText('Compressione " & i & "/" & qFiletti & ": " & Dire & "\" & gf.TornaNomeFileDaPath(filetti(i)) & "');", True)
						gf.CreaDirectoryDaPercorso(PathCompressi & Dire & "\")

						processoFFMpeg = New Process()
						Dim pi As ProcessStartInfo = New ProcessStartInfo()
						' Qualita = 96 / 128 / 196
						Dim Estensione As String = gf.TornaEstensioneFileDaPath(NomeMp3Compresso)
						NomeMp3Compresso = NomeMp3Compresso.Replace(Estensione, "") & Estensione
						gf.EliminaFileFisico(NomeMp3Compresso)
						pi.Arguments = "-i " & Chr(34) & filetti(i) & Chr(34) & " -map 0:a:0 -b:a 96k " & Chr(34) & NomeMp3Compresso.Replace("%20", " ") & Chr(34)
						pi.FileName = Server.MapPath(".") & "\App_Data\ffmpeg.exe"
						pi.WindowStyle = ProcessWindowStyle.Normal
						processoFFMpeg.StartInfo = pi
						processoFFMpeg.Start()
						processoFFMpeg.WaitForExit()

						Quanti += 1
					End If
				End If
			Else
				Exit For
			End If
		Next

		cmdAnnulla.Visible = False
		cmdComprime.Visible = True

		gf = Nothing

		'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script" & Now, "changeText('Fine processo');", True)
		content = "Fine processo"
		trd.Abort()

		inProcess = False
	End Sub

	Protected Sub Timer1_Tick(sender As Object, e As EventArgs)
		If inProcess Then
			lblCompressi.Text = content
			If Not cmdAnnulla.Visible Then
				cmdAnnulla.Visible = True
				cmdComprime.Visible = False
			End If
		Else
			Timer1.Enabled = False
		End If
	End Sub

	Protected Sub cmdComprime_Click(sender As Object, e As EventArgs) Handles cmdComprime.Click
		InterrompiElaborazione = False

		' ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script" & Now.ToString.Trim, "changeText('Inizio processo');", True)
		'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script1", "startText();", True)
		'hdnText.Value = "Inizio processo"

		inProcess = True
		content = "Inizio processo"
		Timer1.Enabled = True

		trd = New Thread(AddressOf ComprimeFiles)
		trd.IsBackground = True
		trd.Start()
	End Sub

	Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
		'ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script2", "stopText();", True)
		cmdAnnulla.Visible = False
		cmdComprime.Visible = True
		inProcess = False
		InterrompiElaborazione = True
		trd.Abort()
	End Sub
End Class