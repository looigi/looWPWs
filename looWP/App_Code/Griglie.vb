Imports System.Data

Public Class Griglie
    Private Colonne() As DataColumn
    Private riga As DataRow
    Private dttTabella As New DataTable()
    Private mDBCE As New MetodiDbCE
    Private Rec As Object
    Private QuantiCampi As Integer
    Private AggiunteRighe As Boolean = False

    Public Function ImpostaCampi(Sql As String, Griglia As GridView, Optional NonEseguireSubito As Boolean = False) As Integer
        Dim q As Integer = 0
        Dim nCampi() As String = {}
        Dim Appo As String

        For i As Integer = 0 To Griglia.Columns.Count - 1
            Try
                Appo = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
            Catch ex As Exception
                Appo = ""
            End Try

            If Appo <> "" Then
                ReDim Preserve nCampi(q)
                nCampi(q) = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
                q += 1
            End If
        Next
        QuantiCampi = q - 1

        ReDim Preserve Colonne(UBound(nCampi))
        QuantiCampi = UBound(nCampi)

        For i As Integer = 0 To QuantiCampi
            Colonne(i) = New DataColumn(nCampi(i))

            dttTabella.Columns.Add(Colonne(i))
        Next

        ApreDB()
        Dim RigheInserite As Integer = ImpostaValori(Sql)
        If NonEseguireSubito = False Then
            VisualizzaValori(Griglia)
            ChiudeDB()
        Else
            AggiunteRighe = True
        End If

        Return RigheInserite
    End Function

    Private Function ImpostaValori(Sql As String) As Integer
        Dim Campo As String
        Dim NumeroRighe As Integer

        Rec = mDBCE.RitornaRecordset(Sql)

        Do Until Rec.Eof
            riga = dttTabella.NewRow()
            For i As Integer = 0 To QuantiCampi
                Campo = "" & Rec(i).Value.ToString.Replace("&#39;", "'").Replace("&#224;", "'")

                'If IsDate(Campo) = True And Campo.Length > 6 Then
                '    Dim dCampo As Date = Campo

                '    Campo = ConverteData(dCampo)
                'End If

                riga(i) = Campo
            Next
            dttTabella.Rows.Add(riga)

            NumeroRighe += 1

            Rec.MoveNext()
        Loop

        Rec.Close()

        Return NumeroRighe
    End Function

    Public Sub ImpostaCampiDaQuerySQL(Sql As String, Griglia As GridView, Optional NonEseguireSubito As Boolean = False)
        Dim q As Integer = 0
        Dim nCampi() As String = {}
        Dim Appo As String

        For i As Integer = 0 To Griglia.Columns.Count - 1
            Try
                Appo = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
            Catch ex As Exception
                Appo = ""
            End Try

            If Appo <> "" Then
                ReDim Preserve nCampi(q)
                nCampi(q) = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
                q += 1
            End If
        Next
        QuantiCampi = q - 1

        ReDim Preserve Colonne(UBound(nCampi))
        QuantiCampi = UBound(nCampi)

        For i As Integer = 0 To QuantiCampi
            Colonne(i) = New DataColumn(nCampi(i))

            dttTabella.Columns.Add(Colonne(i))
        Next

        ApreDB()
        ImpostaValoriDaQuerySQL(Sql)
        If NonEseguireSubito = False Then
            VisualizzaValori(Griglia)
            ChiudeDB()
        Else
            AggiunteRighe = True
        End If
    End Sub

    Public Sub PulisceGriglie(NomeGridView As GridView)
        Dim Dati() As String = {}

        ImpostaCampiDaCSV(Dati, NomeGridView)
    End Sub

    Public Sub ImpostaCampiDaCSV(CSV() As String, Griglia As GridView, Optional NonEseguireSubito As Boolean = False)
        Dim q As Integer = 0
        Dim nCampi() As String = {}
        Dim Appo As String

        For i As Integer = 0 To Griglia.Columns.Count - 1
            Try
                Appo = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
            Catch ex As Exception
                Appo = ""
            End Try

            If Appo <> "" Then
                ReDim Preserve nCampi(q)
                nCampi(q) = DirectCast(Griglia.Columns(i), System.Web.UI.WebControls.BoundField).DataField
                q += 1
            End If
        Next
        QuantiCampi = q - 1

        ReDim Preserve Colonne(UBound(nCampi))
        QuantiCampi = UBound(nCampi)

        For i As Integer = 0 To QuantiCampi
            Colonne(i) = New DataColumn(nCampi(i))

            dttTabella.Columns.Add(Colonne(i))
        Next

        ImpostaValoriCSV(CSV)
        If NonEseguireSubito = False Then
            VisualizzaValori(Griglia)
        Else
            AggiunteRighe = True
        End If
    End Sub

    Private Function ApreDB() As Boolean
        Dim u As New Utility
        Dim gf As New GestioneFilesDirectory
        Dim Ritorno As String = ""

        'Dim NomeDB As String = HttpContext.Current.Server.MapPath(".") & "\Db\looWebPlayer.sdf"
        'If mDBCE.ApreConnessione(gf.TornaNomeDirectoryDaPath(NomeDB), gf.TornaNomeFileDaPath(NomeDB)) = "OK" Then
        '    Return True
        'Else
        '    Return False
        'End If

        Dim Connessione As String = u.LeggeImpostazioniDiBase(HttpContext.Current.Server.MapPath("."))

        If Connessione = "" Then
            Return False ' "ERROR: Connessione sito non valida"
        Else
            Dim Conn As Object = u.ApreDB(Connessione)

            If TypeOf (Conn) Is String Then
                Return False ' "ERROR: " & Conn
            Else
                mDBCE = Conn
                Return True
            End If
        End If
    End Function

    Private Sub ChiudeDB()
        riga = Nothing
        dttTabella = Nothing

        mDBCE.ChiudeConnessione()
    End Sub

    Public Sub AggiungeValori(Sql As String)
        Dim Campo As String

        Rec = mDBCE.RitornaRecordset(Sql)

        Do Until Rec.Eof
            riga = dttTabella.NewRow()
            For i As Integer = 0 To QuantiCampi
                Campo = "" & Rec(i).Value

                If IsDate(Campo) = True And Campo.Length > 6 Then
                    Dim dCampo As Date = Campo

                    Campo = ConverteData(dCampo)
                End If

                riga(i) = Campo
            Next
            dttTabella.Rows.Add(riga)

            Rec.MoveNext()
        Loop

        Rec.Close()
    End Sub

    Private Sub ImpostaValoriDaQuerySQL(Sql As String)
        Dim Campo As String

        Rec = mDBCE.RitornaRecordset(Sql)

        Do Until Rec.Eof
            riga = dttTabella.NewRow()
            For i As Integer = 0 To QuantiCampi
                Campo = "" & Rec(i).Value

                If IsDate(Campo) = True And Campo.Length > 6 Then
                    Dim dCampo As Date = Campo

                    Campo = ConverteData(dCampo)
                End If

                riga(i) = Campo
            Next
            dttTabella.Rows.Add(riga)

            Rec.MoveNext()
        Loop

        Rec.Close()
    End Sub

    Private Sub ImpostaValoriCSV(Csv() As String)
        Dim Campo() As String
        Dim sCampo As String

        For k As Integer = 1 To Csv.Length - 1
            Campo = Csv(k).Split(";")

            riga = dttTabella.NewRow()
            For i As Integer = 0 To QuantiCampi
                sCampo = "" & Campo(i)

                If IsDate(Campo) = True And Campo.Length > 6 Then
                    Dim dCampo As Date = sCampo

                    sCampo = ConverteData(dCampo)
                End If

                riga(i) = sCampo
            Next
            dttTabella.Rows.Add(riga)
        Next
    End Sub

    Public Sub VisualizzaValori(grdView As GridView)
        grdView.DataSource = dttTabella
        grdView.DataBind()

        If AggiunteRighe = True Then
            ChiudeDB()
        End If
    End Sub
End Class
