<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Home.aspx.vb" Inherits="looWP.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblCompressi" runat="server" Text=""></asp:Label>
    <asp:Button ID="cmdComprime" runat="server" Text="Comprime Mp3" />
    <asp:Button ID="cmdAnnulla" runat="server" Text="Interrompe" />
</asp:Content>
