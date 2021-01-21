<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestionCatalogue.aspx.cs" Inherits="Pages_Vendeur_GestionCatalogue" MasterPageFile="~/Pages/PageMaster.master" %>


<asp:Content runat="server" ContentPlaceHolderID="Head">
    <style>
        .imgResize {
            height: 64px;
            width: 64px;
        }

        .borderless td, .borderless th {
            border: none;
        }
    </style>
</asp:Content>
    <asp:Content ID="contentCatalogueGestion" ContentPlaceHolderID="ContenuPrincipal" runat="server">
        <asp:Panel runat="server" ID="panelAvecModal"></asp:Panel>
         <asp:Panel runat="server" CssClass="container-fluid mt-3 mb-3">

            <!-- Panneau pour ajouter -->
        <asp:Panel ID="pnAjouter" runat="server" CssClass="card mb-3">
        <asp:Panel CssClass="card-header center bg-dark-blue" runat="server">
            <asp:Label Text="Mon catalogue" runat="server" CssClass="card-title h4"/>
        </asp:Panel>
            <asp:panel ID="bodyAjouter" CssClass="card-body" runat="server">
                <asp:Button ID="btnPageAjout" Text="Ajouter un article" runat="server" OnClick="ouvrirInscriptionProduit" CssClass="btn classBoutonsMargins100 btn-outline-success mb-3"/>
                </asp:Panel>
        </asp:Panel>

             <!-- tableau du catalogue -->
        <asp:Panel ID="pnCatalogue" runat="server" CssClass="card">
        <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
            <asp:Label Text="Mes produits" runat="server" CssClass="card-title h4"/>
        </asp:Panel>
            <asp:panel ID="panelTable" CssClass="card-body" runat="server">
            </asp:Panel>
        </asp:Panel>
             
        </asp:Panel>
</asp:Content>
