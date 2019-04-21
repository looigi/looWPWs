Imports System.IO
Imports PardesiServices.WinControls

Public Class ModificaUtenti
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            CaricaUtenti()
        End If
    End Sub

    Private Sub CaricaUtenti()
        Dim Gr As New Griglie
        Dim Sql As String = ""
        Dim dg As New GridView

        dg = grdUtenti

        Sql = "Select * From Utenti Order By Utente"

        Dim QuanteRighe As Integer = Gr.ImpostaCampi(Sql, dg)

        Gr = Nothing

        PulisceCampi()
    End Sub

    Private Sub PulisceCampi()
        hdnIdUtente.Value = ""
        txtUtente.Text = ""
        txtPassword.Text = ""
        chkAmministratore.Checked = False
        lblPath.Text = ""
        imgUtente.ImageUrl = "App_Themes\Standard\Images\Sconosciuto.png"
        hdnNomeUtente.Value = ""
    End Sub

    Private Sub grdUtenti_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdUtenti.PageIndexChanging
        grdUtenti.PageIndex = e.NewPageIndex
        grdUtenti.DataBind()

        CaricaUtenti()
    End Sub

    Private Sub grdUtenti_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUtenti.RowDataBound
        'If e.Row.RowType = DataControlRowType.Header Then
        '    e.Row.Cells(2).Visible = False
        '    e.Row.Cells(6).Visible = False
        'End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Immagine As ImageButton = DirectCast(e.Row.FindControl("imgImmagine"), ImageButton)
            Dim NomeImmagine As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Utenti\" & e.Row.Cells(2).Text & ".jpg"

            If File.Exists(NomeImmagine) = True Then
                NomeImmagine = "App_Themes\Standard\Images\Utenti\" & e.Row.Cells(2).Text & ".jpg"
            Else
                NomeImmagine = "App_Themes\Standard\Images\Sconosciuto.png"
            End If

            Immagine.ImageUrl = NomeImmagine
        End If
    End Sub

    Protected Sub VisualizzaDettaglio2(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        hdnIdUtente.Value = row.Cells(1).Text
        hdnNomeUtente.Value = row.Cells(2).Text

        Dim Immagine As ImageButton = DirectCast(row.FindControl("imgImmagine"), ImageButton)
        Dim NomeImmagine As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Utenti\" & row.Cells(2).Text & ".jpg"

        If File.Exists(NomeImmagine) = True Then
            NomeImmagine = "App_Themes\Standard\Images\Utenti\" & row.Cells(2).Text & ".jpg"
        Else
            NomeImmagine = "App_Themes\Standard\Images\Sconosciuto.png"
        End If
        imgUtente.ImageUrl = NomeImmagine

        txtUtente.Text = row.Cells(2).Text
        txtPassword.Text = row.Cells(3).Text
        If row.Cells(4).Text = "S" Then
            chkAmministratore.Checked = True
        Else
            chkAmministratore.Checked = False
        End If
        lblPath.Text = row.Cells(5).Text
    End Sub


    Protected Sub EliminaDettaglio2(sender As Object, e As EventArgs)
        Dim row As GridViewRow = DirectCast(DirectCast(sender, ImageButton).NamingContainer, GridViewRow)
        hdnIdUtente.Value = row.Cells(1).Text

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "MostraPopupEliminazione();", True)
    End Sub

    Protected Sub cmdPath_Click(sender As Object, e As EventArgs) Handles cmdPath.Click
        Dim u As New Utility
        Dim Path() As String = u.RitornaPercorsiDB.Split(";")

        lstFolder.Items.Clear()
        lstFolder.Items.Add("\")
        For Each Dir As String In Directory.GetDirectories(Path(0))
            lstFolder.Items.Add(Dir)
        Next

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "MostraPopupSceltaCartella();", True)
    End Sub

    Protected Sub cmdPulisce_Click(sender As Object, e As EventArgs) Handles cmdPulisce.Click
        PulisceCampi()
    End Sub

    Protected Sub cmdSalva_Click(sender As Object, e As EventArgs) Handles cmdSalva.Click
        If txtUtente.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "MostraPopup('Inserire il nome utente');", True)
            Exit Sub
        End If
        If txtPassword.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "MostraPopup('Inserire la password');", True)
            Exit Sub
        End If
        If lblPath.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "MostraPopup('Inserire il path');", True)
            Exit Sub
        End If

        Dim u As New Utility
        Dim gf As New GestioneFilesDirectory
        Dim Path() As String = u.RitornaPercorsiDB.Split(";")
        Dim Ritorno As String = ""

        Dim Amministratore As String = "N"

        If chkAmministratore.Checked Then
            Amministratore = "S"
        End If

        Dim mDBCE As New MetodiDbCE
        Dim NomeDB As String = HttpContext.Current.Server.MapPath(".") & "\Db\looWebPlayer.sdf"
        Dim Sql As String = ""
        Dim Messaggio As String = ""
        mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))

        If hdnIdUtente.Value = "" Then
            Dim idUtente As Integer

            Sql = "Select Max(idUtente)+1 From Utenti"
            Dim rec As Object = mDBCE.RitornaRecordset(Sql)
            If rec Is Nothing Then
                Ritorno = "ERROR: query non valida"
            Else
                If rec(0).Value Is DBNull.Value Then
                    idUtente = 1
                Else
                    idUtente = rec(0).Value
                End If
                rec.close

                Sql = "Insert Into Utenti Values ( " &
                    " " & idUtente & ", " &
                    "'" & txtUtente.Text.Replace("'", "''") & "', " &
                    "'" & txtPassword.Text.Replace("'", "''") & "', " &
                    "'" & Amministratore & "', " &
                    "'" & lblPath.Text.Replace("'", "''") & "' " &
                    ")"
                Messaggio = "Nuovo utente inserito"
            End If
        Else
            Sql = "Update Utenti Set " &
                "Utente='" & txtUtente.Text.Replace("'", "''") & "', " &
                "Password='" & txtPassword.Text.Replace("'", "''") & "', " &
                "Amministratore='" & Amministratore & "', " &
                "CartellaBase='" & lblPath.Text.Replace("'", "''") & "' " &
                "Where idUtente=" & hdnIdUtente.Value
            Messaggio = "Utente modificato"
        End If

        Dim Rit As String = mDBCE.EsegueSQL(Sql)

        If Rit <> "OK" Then
            Messaggio = Rit
        End If

        mDBCE.ChiudeConnessione()

        If FileUpload1.HasFile Then
            Dim NomeImmagine As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Utenti\" & txtUtente.Text & ".jpg"
            gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\App_Themes\Standard\Images\Utenti\")
            FileUpload1.SaveAs(NomeImmagine)
        Else
            If hdnNomeUtente.Value <> txtUtente.Text Then
                Dim NomeImmagineVecchia As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Utenti\" & hdnNomeUtente.Value & ".jpg"
                Dim NomeImmagineNuova As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Utenti\" & txtUtente.Text & ".jpg"

                Rename(NomeImmagineVecchia, NomeImmagineNuova)
            End If
        End If

        CaricaUtenti()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "MostraPopup('" & Messaggio & "');", True)
    End Sub

    Protected Sub cmdOkCartella_Click(sender As Object, e As EventArgs) Handles cmdOkCartella.Click
        Dim Cartella As String = lstFolder.Text

        If Cartella = "" Then
            Exit Sub
        End If

        Dim u As New Utility
        Dim gf As New GestioneFilesDirectory
        Dim Path() As String = u.RitornaPercorsiDB.Split(";")
        lblPath.Text = Cartella.Replace(Path(0), "")

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ChiudePopupSceltaCartella();", True)
    End Sub

    Protected Sub cmdOkEliminazione_Click(sender As Object, e As EventArgs) Handles cmdOkEliminazione.Click
        Dim u As New Utility
        Dim gf As New GestioneFilesDirectory
        Dim Path() As String = u.RitornaPercorsiDB.Split(";")
        Dim Ritorno As String = ""

        Dim mDBCE As New MetodiDbCE
        Dim NomeDB As String = HttpContext.Current.Server.MapPath(".") & "\Db\looWebPlayer.sdf"
        Dim Sql As String = ""
        mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB))

        Sql = "Delete From Utenti Where idUtente=" & hdnIdUtente.Value
        Dim Rit As String = mDBCE.EsegueSQL(Sql)

        mDBCE.ChiudeConnessione()

        CaricaUtenti()

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "ChiudePopupEliminazione();", True)
    End Sub

End Class