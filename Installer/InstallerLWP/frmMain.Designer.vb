<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtNomeSito = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPortaSito = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCartellaVirtuale = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPercorsoSito = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPathCV = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtAppPool = New System.Windows.Forms.TextBox()
        Me.cmdPathSito = New System.Windows.Forms.Button()
        Me.cmdPathCV = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.cmdPathCVC = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtPathCVC = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtCVC = New System.Windows.Forms.TextBox()
        Me.cmdEsegue = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtStringaConnessione = New System.Windows.Forms.TextBox()
        Me.lblOperazione = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtNomeSito
        '
        Me.txtNomeSito.Location = New System.Drawing.Point(144, 13)
        Me.txtNomeSito.Name = "txtNomeSito"
        Me.txtNomeSito.Size = New System.Drawing.Size(100, 20)
        Me.txtNomeSito.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Nome sito"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Porta sito"
        '
        'txtPortaSito
        '
        Me.txtPortaSito.Location = New System.Drawing.Point(144, 67)
        Me.txtPortaSito.Name = "txtPortaSito"
        Me.txtPortaSito.Size = New System.Drawing.Size(100, 20)
        Me.txtPortaSito.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(109, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Nome cartella virtuale"
        '
        'txtCartellaVirtuale
        '
        Me.txtCartellaVirtuale.Location = New System.Drawing.Point(144, 93)
        Me.txtCartellaVirtuale.Name = "txtCartellaVirtuale"
        Me.txtCartellaVirtuale.Size = New System.Drawing.Size(100, 20)
        Me.txtCartellaVirtuale.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(68, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Percorso sito"
        '
        'txtPercorsoSito
        '
        Me.txtPercorsoSito.Location = New System.Drawing.Point(144, 39)
        Me.txtPercorsoSito.Name = "txtPercorsoSito"
        Me.txtPercorsoSito.Size = New System.Drawing.Size(394, 20)
        Me.txtPercorsoSito.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 122)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(123, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Percorso cartella virtuale"
        '
        'txtPathCV
        '
        Me.txtPathCV.Location = New System.Drawing.Point(144, 119)
        Me.txtPathCV.Name = "txtPathCV"
        Me.txtPathCV.Size = New System.Drawing.Size(394, 20)
        Me.txtPathCV.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 148)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Nome App Pool"
        '
        'txtAppPool
        '
        Me.txtAppPool.Location = New System.Drawing.Point(144, 145)
        Me.txtAppPool.Name = "txtAppPool"
        Me.txtAppPool.Size = New System.Drawing.Size(100, 20)
        Me.txtAppPool.TabIndex = 10
        '
        'cmdPathSito
        '
        Me.cmdPathSito.Location = New System.Drawing.Point(544, 39)
        Me.cmdPathSito.Name = "cmdPathSito"
        Me.cmdPathSito.Size = New System.Drawing.Size(29, 20)
        Me.cmdPathSito.TabIndex = 12
        Me.cmdPathSito.Text = "..."
        Me.cmdPathSito.UseVisualStyleBackColor = True
        '
        'cmdPathCV
        '
        Me.cmdPathCV.Location = New System.Drawing.Point(544, 119)
        Me.cmdPathCV.Name = "cmdPathCV"
        Me.cmdPathCV.Size = New System.Drawing.Size(29, 20)
        Me.cmdPathCV.TabIndex = 13
        Me.cmdPathCV.Text = "..."
        Me.cmdPathCV.UseVisualStyleBackColor = True
        '
        'cmdPathCVC
        '
        Me.cmdPathCVC.Location = New System.Drawing.Point(544, 202)
        Me.cmdPathCVC.Name = "cmdPathCVC"
        Me.cmdPathCVC.Size = New System.Drawing.Size(29, 20)
        Me.cmdPathCVC.TabIndex = 18
        Me.cmdPathCVC.Text = "..."
        Me.cmdPathCVC.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 205)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(126, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Perc. cart. virt. compressi"
        '
        'txtPathCVC
        '
        Me.txtPathCVC.Location = New System.Drawing.Point(144, 202)
        Me.txtPathCVC.Name = "txtPathCVC"
        Me.txtPathCVC.Size = New System.Drawing.Size(394, 20)
        Me.txtPathCVC.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 179)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(129, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Nome cart. virt. compressi"
        '
        'txtCVC
        '
        Me.txtCVC.Location = New System.Drawing.Point(144, 176)
        Me.txtCVC.Name = "txtCVC"
        Me.txtCVC.Size = New System.Drawing.Size(100, 20)
        Me.txtCVC.TabIndex = 14
        '
        'cmdEsegue
        '
        Me.cmdEsegue.Location = New System.Drawing.Point(492, 231)
        Me.cmdEsegue.Name = "cmdEsegue"
        Me.cmdEsegue.Size = New System.Drawing.Size(81, 38)
        Me.cmdEsegue.TabIndex = 19
        Me.cmdEsegue.Text = "Esegue"
        Me.cmdEsegue.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 231)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 13)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Stringa connessione"
        Me.Label9.Visible = False
        '
        'txtStringaConnessione
        '
        Me.txtStringaConnessione.Location = New System.Drawing.Point(142, 228)
        Me.txtStringaConnessione.Name = "txtStringaConnessione"
        Me.txtStringaConnessione.Size = New System.Drawing.Size(344, 20)
        Me.txtStringaConnessione.TabIndex = 20
        Me.txtStringaConnessione.Visible = False
        '
        'lblOperazione
        '
        Me.lblOperazione.AutoSize = True
        Me.lblOperazione.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblOperazione.Location = New System.Drawing.Point(12, 257)
        Me.lblOperazione.Name = "lblOperazione"
        Me.lblOperazione.Size = New System.Drawing.Size(103, 13)
        Me.lblOperazione.TabIndex = 22
        Me.lblOperazione.Text = "Stringa connessione"
        Me.lblOperazione.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(583, 279)
        Me.Controls.Add(Me.lblOperazione)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtStringaConnessione)
        Me.Controls.Add(Me.cmdEsegue)
        Me.Controls.Add(Me.cmdPathCVC)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtPathCVC)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtCVC)
        Me.Controls.Add(Me.cmdPathCV)
        Me.Controls.Add(Me.cmdPathSito)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtAppPool)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtPathCV)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtPercorsoSito)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtCartellaVirtuale)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtPortaSito)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNomeSito)
        Me.Name = "frmMain"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtNomeSito As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtPortaSito As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCartellaVirtuale As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtPercorsoSito As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPathCV As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtAppPool As TextBox
    Friend WithEvents cmdPathSito As Button
    Friend WithEvents cmdPathCV As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents cmdPathCVC As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtPathCVC As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtCVC As TextBox
    Friend WithEvents cmdEsegue As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents txtStringaConnessione As TextBox
    Friend WithEvents lblOperazione As Label
End Class
