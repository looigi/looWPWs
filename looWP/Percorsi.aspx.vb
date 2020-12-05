Imports System.IO

Public Class Percorsi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CaricaPercorsi()

            divScelta.Visible = False
        End If
    End Sub

    Private Sub CaricaPercorsi()
        Dim u As New Utility
        Dim Path() As String = u.RitornaPercorsiDB.Split(";")

        lblPathMp3.Text = Path(0)
        lblPathDB.Text = Path(1)
        lblPathCompressi.Text = Path(2)
    End Sub

    Protected Sub lstFolder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstFolder.SelectedIndexChanged
        If lstFolder.Text = ".." Then
            If Strings.Right(lblPath.Text, 1) = "\" Then
                lblPath.Text = Mid(lblPath.Text, 1, lblPath.Text.Length - 1)
            End If
            Dim gf As New GestioneFilesDirectory
            Dim Cosa As String = gf.TornaNomeDirectoryDaPath(lblPath.Text)
            If Strings.Right(Cosa, 1) <> "\" And Cosa <> "" Then
                Cosa &= "\"
            End If

            LeggeFolders(Cosa)
        Else
            LeggeFolders(lstFolder.Text)
        End If
    End Sub

    Private Sub LeggeFolders(Inizio As String)
        lstFolder.Items.Clear()

        If Inizio <> "" Then
            lstFolder.Items.Add("..")

            Try
                For Each Dir As String In Directory.GetDirectories(Inizio)
                    lstFolder.Items.Add(Dir)
                Next
            Catch ex As Exception

            End Try
        Else
            Dim allDrives As DriveInfo() = DriveInfo.GetDrives()

            For Each d As DriveInfo In allDrives
                lstFolder.Items.Add(d.Name)
            Next
        End If

        lblPath.Text = Inizio
    End Sub

    Protected Sub cmdPathMP3_Click(sender As Object, e As EventArgs) Handles cmdPathMP3.Click
        LeggeFolders(lblPathMp3.Text)
        hdnQuale.Value = 1

        divScelta.Visible = True
    End Sub

    Protected Sub cmdPathDB_Click(sender As Object, e As EventArgs) Handles cmdPathDB.Click
        LeggeFolders(lblPathDB.Text)
        hdnQuale.Value = 2

        divScelta.Visible = True
    End Sub

    Protected Sub cmdPathCompressi_Click(sender As Object, e As EventArgs) Handles cmdPathCompressi.Click
        LeggeFolders(lblPathCompressi.Text)
        hdnQuale.Value = 3

        divScelta.Visible = True
    End Sub

    Protected Sub cmdOkCartella_Click(sender As Object, e As EventArgs) Handles cmdOkCartella.Click
        Dim u As New Utility
        'Dim u As New Utility
        'Dim gf As New GestioneFilesDirectory
        'Dim Path() As String = u.RitornaPercorsiDB.Split(";")
        Dim Ritorno As String = ""

        'Dim mDBCE As New MetodiDbCE
        'Dim NomeDB As String = HttpContext.Current.Server.MapPath(".") & "\Db\looWebPlayer.sdf"
        'Dim Sql As String = ""
        'mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))
        Dim NomeCampo As String = ""

        Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

        If Connessione = "" Then
            Ritorno = "ERROR: Connessione non valida"
        Else
            Dim Conn As Object = u.ApreDB(Connessione)

            If TypeOf (Conn) Is String Then
                Ritorno = "Error:" & Conn
            Else
                'If SoloNuove = "S" Then
                '	Dim m As New mailImap
                '	Dim Ritorno2 As String = m.RitornaMessaggi(Squadra, idAnno, idUtente, Folder)
                'End If

                Dim Rec As Object = HttpContext.Current.Server.CreateObject("ADODB.Recordset")

                Select Case hdnQuale.Value
                    Case "1"
                        NomeCampo = "Path"
                    Case "2"
                        NomeCampo = "PathDB"
                    Case "3"
                        NomeCampo = "PathCompressi"
                End Select

                Dim Sql As String = "Update Percorsi Set " & NomeCampo & "='" & lblPath.Text.Replace("'", "''") & "'"
                Dim Rit As String = u.EsegueSql(Conn, Sql, Connessione)

                u.ChiudeDB(True, Conn)
            End If
        End If

        CaricaPercorsi()

        hdnQuale.Value = ""
        divScelta.Visible = False
    End Sub

    Protected Sub cmdAnnullaCartella_Click(sender As Object, e As EventArgs) Handles cmdAnnullaCartella.Click
        hdnQuale.Value = ""
        divScelta.Visible = False
    End Sub
End Class