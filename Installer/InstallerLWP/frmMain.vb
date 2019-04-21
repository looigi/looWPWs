Imports System.IO
Imports Ionic.Zip

Public Class frmMain
    Private PathIIS As String

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtNomeSito.Text = "LooWebPlayer"
        txtCartellaVirtuale.Text = "Musica"
        txtAppPool.Text = "AppPoolLWP"
        txtPercorsoSito.Text = "D:\Looigi\LooWebPlayer\"
        txtPathCV.Text = "D:\Looigi\MP3\"
        txtPortaSito.Text = "2727"
        txtCVC.Text = "Compressi"
        txtPathCVC.Text = "D:\Looigi\MP3daPassare\"
        txtStringaConnessione.Text = ""
    End Sub

    Private Sub cmdPathSito_Click(sender As Object, e As EventArgs) Handles cmdPathSito.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            txtPercorsoSito.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub cmdPathCV_Click(sender As Object, e As EventArgs) Handles cmdPathCV.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            txtPathCV.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub cmdPathCVC_Click(sender As Object, e As EventArgs) Handles cmdPathCVC.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            txtPathCVC.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Function Controlla()
        Dim Ritorno As String = ""

        If txtNomeSito.Text = "" Then
            Ritorno = "Nome sito non presente"
        Else
            If txtCartellaVirtuale.Text = "" Then
                Ritorno = "Nome cartella virtuale non presente"
            Else
                If txtPathCV.Text = "" Then
                    Ritorno = "Percorso cartella virtuale non presente"
                Else
                    If Not Directory.Exists(txtPathCV.Text) Then
                        Ritorno = "Path cartella virtuale non presente"
                    Else
                        If txtAppPool.Text = "" Then
                            Ritorno = "Nome apppool non presente"
                        Else
                            If txtPercorsoSito.Text = "" Then
                                Ritorno = "Percorso sito non presente"
                            Else
                                If txtPortaSito.Text = "" Then
                                    Ritorno = "Porta sito non presente"
                                Else
                                    If Not IsNumeric(txtPortaSito.Text) Then
                                        Ritorno = "Porta sito non valida"
                                    Else
                                        If Val(txtPortaSito.Text) < 1 Or Val(txtPortaSito.Text) > 65536 Then
                                            Ritorno = "Porta sito non valida"
                                        Else
                                            If txtCVC.Text = "" Then
                                                Ritorno = "Nome cartella virtuale compressi non presente"
                                            Else
                                                If txtPathCVC.Text = "" Then
                                                    Ritorno = "Percorso cartella virtuale compressi non presente"
                                                Else
                                                    If Not Directory.Exists(txtPathCVC.Text) Then
                                                        Ritorno = "Path cartella virtuale compressi non presente"
                                                    Else
                                                        If Not Directory.Exists("c:\windows\system32\inetsrv") And Not Directory.Exists("C:\Windows\SysWOW64\system32\intesrv") Then
                                                            Ritorno = "IIS potrebbe non essere installato nel sistema"
                                                        Else
                                                            If Directory.Exists("c:\windows\system32\inetsrv") Then
                                                                PathIIS = "c:\windows\system32\inetsrv"
                                                            Else
                                                                PathIIS = "C:\Windows\SysWOW64\system32\intesrv"
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If Ritorno = "" Then
            Return True
        Else
            MsgBox(Ritorno, vbExclamation)
            Return False
        End If
    End Function

    Private Sub ScriveOperazione(Cosa As String)
        lblOperazione.Text = Cosa
        Application.DoEvents()
    End Sub

    Private Sub cmdEsegue_Click(sender As Object, e As EventArgs) Handles cmdEsegue.Click
        If Not Controlla() Then
            Exit Sub
        End If

        lblOperazione.Visible = True

        Dim gf As New GestioneFilesDirectory

        If Strings.Right(txtPathCV.Text, 1) <> "\" Then txtPathCV.Text &= "\"
        If Strings.Right(txtPathCVC.Text, 1) <> "\" Then txtPathCVC.Text &= "\"
        If Strings.Right(txtPercorsoSito.Text, 1) <> "\" Then txtPercorsoSito.Text &= "\"

        ScriveOperazione("Elimino cartella sito")
        gf.EliminaAlberoDirectory(txtPercorsoSito.Text, True, True)
        ScriveOperazione("Creo cartella sito")
        gf.CreaDirectoryDaPercorso(txtPercorsoSito.Text)

        ScriveOperazione("Copio files")
        Dim Ritorno As String = gf.CopiaFileFisico(Application.StartupPath & "\Elementi\Pubblicazione.zip", txtPercorsoSito.Text & "Pubblicazione.zip", True)
        If Ritorno.Contains("ERRORE:") Then
            MsgBox(Ritorno, vbExclamation)
        Else
            ScriveOperazione("Decomprimo files")
            Using zip As New ZipFile(txtPercorsoSito.Text & "Pubblicazione.zip")
                zip.ExtractAll(txtPercorsoSito.Text)
            End Using
            If Not File.Exists(txtPercorsoSito.Text & "Home.aspx") And Not File.Exists(txtPercorsoSito.Text & "pathMp3.txt") Then
                MsgBox("Problemi nell'estrazione del file", vbExclamation)
            Else
                ScriveOperazione("Pulisco cartella sito")
                gf.EliminaFileFisico(txtPercorsoSito.Text & "Pubblicazione.zip")

                Dim StringaDaSalvare As String = ""

                StringaDaSalvare &= Mid(txtPathCV.Text, 1, txtPathCV.Text.Length - 1) & ";"
                StringaDaSalvare &= Mid(txtPercorsoSito.Text, 1, txtPercorsoSito.Text.Length - 1) & "DB;"
                StringaDaSalvare &= Mid(txtPathCVC.Text, 1, txtPathCVC.Text.Length - 1)

                ScriveOperazione("Sistemo file di config")
                gf.EliminaFileFisico(txtPercorsoSito.Text & "pathMp3.txt")
                gf.CreaAggiornaFile(txtPercorsoSito.Text & "pathMp3.txt", StringaDaSalvare)

                ScriveOperazione("Creo sito")
                Dim process As Process = Nothing
                Dim processStartInfo As ProcessStartInfo
                processStartInfo = New ProcessStartInfo()
                processStartInfo.FileName = Application.StartupPath & "\crea.bat"
                processStartInfo.Verb = "runas"
                processStartInfo.Arguments = txtNomeSito.Text & " " & Application.StartupPath & "\Out.txt" & " " & txtCartellaVirtuale.Text & " " & txtPathCV.Text & " " &
                    PathIIS & " " & txtPercorsoSito.Text & " " & txtPortaSito.Text & " " & txtCVC.Text & " " & txtPathCVC.Text
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal
                processStartInfo.UseShellExecute = True
                process = Process.Start(processStartInfo)
                process.WaitForExit()

                Dim Risultato As String = gf.LeggeFileIntero(Application.StartupPath & "\Out.txt")
                lblOperazione.Visible = False
                MsgBox(Risultato, vbInformation)
            End If
        End If
        gf = Nothing
    End Sub
End Class
