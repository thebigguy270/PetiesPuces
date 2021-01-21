<%@ Page Title="Historique des commandes" Language="C#" MasterPageFile="~/Pages/PageMaster.master" AutoEventWireup="true" CodeFile="HistoriqueCommandes.aspx.cs" Inherits="Pages_Client_HistoriqueCommandes" %>

<asp:Content ID="CommandesContent" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <asp:Panel ID="pnlContent" CssClass="container mt-3" runat="server"></asp:Panel>
    <script>
        function ouvrirPDF(texte) {
            window.open('/Factures/' + texte);   
        }
    </script>
</asp:Content>
