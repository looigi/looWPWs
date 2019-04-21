'------------------------------------------------------------------------------
' <generato automaticamente>
'     Codice generato da uno strumento.
'
'     Le modifiche a questo file possono causare un comportamento non corretto e verranno perse se
'     il codice viene rigenerato. 
' </generato automaticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class ModificaUtenti
    
    '''<summary>
    '''Controllo hdnIdUtente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdnIdUtente As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo hdnNomeUtente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdnNomeUtente As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo grdUtenti.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdUtenti As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo imgUtente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgUtente As Global.System.Web.UI.WebControls.Image
    
    '''<summary>
    '''Controllo FileUpload1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents FileUpload1 As Global.System.Web.UI.WebControls.FileUpload
    
    '''<summary>
    '''Controllo txtUtente.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtUtente As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo txtPassword.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents txtPassword As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Controllo chkAmministratore.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkAmministratore As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo lblPath.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblPath As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo cmdPath.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdPath As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo cmdPulisce.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdPulisce As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo cmdSalva.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdSalva As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo lblTestoPopup.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblTestoPopup As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo cmdChiude.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdChiude As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo Label1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo lstFolder.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lstFolder As Global.System.Web.UI.WebControls.ListBox
    
    '''<summary>
    '''Controllo cmdAnnullaCartella.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdAnnullaCartella As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo cmdOkCartella.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdOkCartella As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo Label2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo cmdAnnullaEliminazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdAnnullaEliminazione As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo cmdOkEliminazione.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdOkEliminazione As Global.System.Web.UI.WebControls.Button
End Class
