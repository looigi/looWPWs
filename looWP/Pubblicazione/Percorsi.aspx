<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Percorsi.aspx.vb" Inherits="looWP.Percorsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="input-group input-group-sm mb-3">
        <div class="input-group-prepend">
            <span class="input-group-text" id="inputGroup-sizing-sm">Path MP3</span>
        </div>
        <asp:Label ID="lblPathMp3" runat="server" Text="Label" class="form-control" aria-label="Text input with checkbox"></asp:Label>
        <asp:Button ID="cmdPathMP3" runat="server" Text="..." class="btn btn-primary"/>
    </div>
    <div class="input-group input-group-sm mb-3">
        <div class="input-group-prepend">
            <span class="input-group-text" id="inputGroup-sizing-sm">Path DB Mp3</span>
        </div>
        <asp:Label ID="lblPathDB" runat="server" Text="Label" class="form-control" aria-label="Text input with checkbox"></asp:Label>
        <asp:Button ID="cmdPathDB" runat="server" Text="..." class="btn btn-primary"/>
    </div>
    <div class="input-group input-group-sm mb-3">
        <div class="input-group-prepend">
            <span class="input-group-text" id="inputGroup-sizing-sm">Path MP3 compressi</span>
        </div>
        <asp:Label ID="lblPathCompressi" runat="server" Text="Label" class="form-control" aria-label="Text input with checkbox"></asp:Label>
        <asp:Button ID="cmdPathCompressi" runat="server" Text="..." class="btn btn-primary"/>
    </div>

    <div id="divScelta" runat="server" style="position: absolute; left: 35%; top: 25%; width: 30%; height: auto; display: block; padding: 3px; border: 1px solid #000000; background-color: #ffd800; overflow: auto;">
        <asp:HiddenField ID="hdnQuale" runat="server" />
        Loo's Web Player
        <hr />
        <asp:ListBox ID="lstFolder" runat="server" AutoPostBack="True" style="width: 99%; margin-left: 2px; height: 300px"></asp:ListBox>            
        <hr />
        <asp:Label ID="lblPath" runat="server" Text="Label" class="col-form-label"></asp:Label>
        <hr />
        <div class="col-12" style="text-align: right;">
            <asp:Button ID="cmdAnnullaCartella" runat="server" Text="Annulla" class="btn btn-primary" Width="150px"/>
            <asp:Button ID="cmdOkCartella" runat="server" Text="Ok" class="btn btn-primary" Width="100px"/>
        </div>
    </div>
</asp:Content>
