<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Nouveautes.aspx.cs" Inherits="Pages_Nouveautes" MasterPageFile="~/Pages/PageMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <style>
        .imgResize {
            height: 64px;
            width: 64px;
        }
    </style>
</asp:Content>
    <asp:Content ID="contentCatalogueGestion" ContentPlaceHolderID="ContenuPrincipal" runat="server">
        <asp:Panel runat="server" ID="panelAvecModal"></asp:Panel>
         <asp:Panel runat="server" CssClass="container-fluid mt-3 mb-3">

            <!-- Panneau pour ajouter -->
        <asp:Panel ID="pnAjouter" runat="server" CssClass="card mb-3">
        <asp:Panel CssClass="card-header center bg-dark-blue" runat="server">
            <asp:Label Text="Bienvenue sur Les Petites Puces!, voici quelques informations pour vous." runat="server" CssClass="card-title h4"/>
        </asp:Panel>
            <asp:panel CssClass="card-body" runat="server" ID="pnInfos">
                </asp:Panel>
        </asp:Panel>

             <!-- tableau du catalogue -->
        <asp:Panel ID="pnCatalogue" runat="server" CssClass="card">
        <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
            <asp:Label Text="Voici les NOUVEAUTÉS de nos vendeurs!" runat="server" CssClass="card-title h4"/>
        </asp:Panel>
            <asp:panel ID="pnNouveautes" CssClass="card-body" runat="server">
            </asp:Panel>
        </asp:Panel>
             
        </asp:Panel>
</asp:Content>