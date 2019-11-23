<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Home.aspx.vb" Inherits="looWP.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Timer runat="server" ID="Timer1" Interval="1000" Enabled="false" OnTick="Timer1_Tick" />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
            <asp:Label ID="lblCompressi" runat="server" Text=""></asp:Label>
            <br />

            <asp:Button ID="cmdAggiornaVersione" runat="server" Text="Aggiornamento versione libreria" />
            <asp:Button ID="cmdComprime" runat="server" Text="Comprime Mp3" />
            <asp:Button ID="cmdAnnulla" runat="server" Text="Interrompe" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
