<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ModificaUtenti.aspx.vb" Inherits="looWP.ModificaUtenti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function MostraPopup(Scritta) {
            var o = document.getElementById("ctl00_ContentPlaceHolder1_lblTestoPopup");
            o.innerHTML = Scritta;

            $('#popupDiv').modal('show');
        }

        function ChiudePopup() {
            $('#popupDiv').modal('hide');
        }

        function MostraPopupSceltaCartella() {
            $('#popupSceltaCartella').modal('show');
        }

        function ChiudePopupSceltaCartella() {
            $('#popupSceltaCartella').modal('hide');
        }

        function MostraPopupEliminazione() {
            $('#popupConfermaElim').modal('show');
        }

        function ChiudePopupEliminazione() {
            $('#popupConfermaElim').modal('hide');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-6">
        <asp:HiddenField ID="hdnIdUtente" runat="server" />
        <asp:HiddenField ID="hdnNomeUtente" runat="server" />
        <asp:GridView ID="grdUtenti" runat="server" AutoGenerateColumns="False" 
            CssClass="griglia" CellPadding ="0" CellSpacing ="10" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
                PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/Icone/icona_PRIMO-RECORD.png" 
                PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/Icone/icona_ULTIMO-RECORD.png" AllowPaging="True" PageSize="5">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                    <ItemTemplate>
                        <asp:ImageButton ID="imgImmagine" runat="server" Width="70" Height="70" CssClass ="ImmagineGriglia" ></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="idUtente" HeaderText="N°" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia-sinistra" />
                </asp:BoundField>
                <asp:BoundField DataField="Utente" HeaderText="Utente" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia-sinistra" />
                </asp:BoundField>
                <asp:BoundField DataField="Password" HeaderText="Password" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia-sinistra" />
                </asp:BoundField>
                <asp:BoundField DataField="Amministratore" HeaderText="Ammin." >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia-sinistra" />
                </asp:BoundField>
                <asp:BoundField DataField="CartellaBase" HeaderText="Root dir" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia-sinistra" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                    <ItemTemplate>
                        <asp:ImageButton ID="imgSeleziona" runat="server" OnClick ="VisualizzaDettaglio2" Width="40" Height="40" ImageUrl="~/App_Themes/Standard/Images/Icone/icona_MODIFICA-TAG.png" ToolTip="Seleziona l'utente"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                    <ItemTemplate>
                        <asp:ImageButton ID="imgElimina" runat="server" OnClick ="EliminaDettaglio2" Width="40" Height="40" ImageUrl="~/App_Themes/Standard/Images/Icone/icona_ELIMINA-TAG.png" ToolTip="Elimina l'utente"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns> 
        </asp:GridView>
    </div>
    <div class="col-6">
        <div class="input-group input-group-sm mb-3">
            <asp:Image ID="imgUtente" runat="server" Width="150px" Height="150px" ImageUrl ="~/App_Themes/Standard/Images/Sconosciuto.png" />&nbsp;
            <asp:FileUpload ID="FileUpload1" runat="server" />
        </div>
        <div class="input-group input-group-sm mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text" id="inputGroup-sizing-sm">Utente</span>
            </div>
            <asp:TextBox ID="txtUtente" runat="server" class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
        </div>
        <div class="input-group input-group-sm mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text" id="inputGroup-sizing-sm">Password</span>
            </div>
            <asp:TextBox ID="txtPassword" runat="server" class="form-control" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
        </div>
        <div class="input-group input-group-sm mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text" id="inputGroup-sizing-sm">Amministratore</span>
            </div>
            <asp:CheckBox ID="chkAmministratore" runat="server" class="form-control" aria-label="Text input with checkbox" />
        </div>
        <div class="input-group input-group-sm mb-3">
            <div class="input-group-prepend">
                <span class="input-group-text" id="inputGroup-sizing-sm">Path</span>
            </div>
            <asp:Label ID="lblPath" runat="server" Text="Label" class="form-control" aria-label="Text input with checkbox"></asp:Label>
            <asp:Button ID="cmdPath" runat="server" Text="..." class="btn btn-primary"/>
        </div>
        <hr />
        <div class="input-group input-group-sm mb-3">
            <asp:Button ID="cmdPulisce" runat="server" Text="Pulisce" class="btn btn-primary"/>&nbsp;
            <asp:Button ID="cmdSalva" runat="server" Text="Salva" class="btn btn-primary"/>
        </div>
    </div>

    <div id="popupDiv" class="modal fade bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" style="padding: 5px;">
      <div class="modal-dialog modal-sm">
        <div class="modal-content">
            Loo's Web Player
            <hr />
            <asp:Label ID="lblTestoPopup" runat="server" Text="Label" class="form-control" aria-label="Text input with checkbox"></asp:Label>
            <hr />
            <div class="col-12" style="text-align: right;">
                <asp:Button ID="cmdChiude" runat="server" Text="Ok" class="btn btn-primary" OnClientClick="ChiudePopup();" Width="100px"/>
            </div>
        </div>
      </div>
    </div>

    <div id="popupSceltaCartella" class="modal fade bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" style="padding: 5px;">
      <div class="modal-dialog modal-sm">
        <div class="modal-content">
            Loo's Web Player
            <hr />
            <asp:Label ID="Label1" runat="server" Text="Scelta cartella utente" class="form-control" aria-label="Text input with checkbox"></asp:Label>
            <hr />
            <asp:ListBox ID="lstFolder" runat="server"></asp:ListBox>            
            <hr />
            <div class="col-12" style="text-align: right;">
                <asp:Button ID="cmdAnnullaCartella" runat="server" Text="Annulla" class="btn btn-primary" OnClientClick="ChiudePopupSceltaCartella();" Width="150px"/>
                <asp:Button ID="cmdOkCartella" runat="server" Text="Ok" class="btn btn-primary" Width="100px"/>
            </div>
        </div>
      </div>
    </div>

    <div id="popupConfermaElim" class="modal fade bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" style="padding: 5px;">
      <div class="modal-dialog modal-sm">
        <div class="modal-content">
            Loo's Web Player
            <hr />
            <asp:Label ID="Label2" runat="server" Text="Confermi l'eliminazione ?" class="form-control" aria-label="Text input with checkbox"></asp:Label>
            <hr />
            <div class="col-12" style="text-align: right;">
                <asp:Button ID="cmdAnnullaEliminazione" runat="server" Text="Annulla" class="btn btn-primary" OnClientClick="ChiudePopupEliminazione();" Width="150px"/>
                <asp:Button ID="cmdOkEliminazione" runat="server" Text="Ok" class="btn btn-primary" Width="100px"/>
            </div>
        </div>
      </div>
    </div>

</asp:Content>
