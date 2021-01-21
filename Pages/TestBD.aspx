<%@ Page Title="Jeu de tests" Language="C#" MasterPageFile="~/Pages/PageMasterAdmin.master" AutoEventWireup="true" CodeFile="TestBD.aspx.cs" Inherits="Pages_Tests" %>
<asp:Content ID="content1" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <asp:Panel CssClass="container-fluid mt-3" runat="server">

        <!-- panel de droite -->
        <asp:Panel runat="server" CssClass="card mb-3">
            <asp:Panel ID="PanelJeuTest" runat="server" CssClass="card">
                <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                    <asp:Label Text="Jeu de tests" runat="server" CssClass="card-title" />
                </asp:Panel>
                <asp:Panel ID="PanelContentJeuTest" CssClass="card-body" runat="server">
                    <asp:Button id="idCreerDatabase" OnClick="CreerDatabase" runat="server" Text="Insérer les éléments dans la base de donnée" CssClass="btn btn-secondary classBoutonLeft classBoutonsMargins"></asp:Button>
                    <asp:Button id="idViderDatabase" OnClick="viderLaDBClick" runat="server" Text="Vider les éléments de la base de donnée" CssClass="btn btn-dark classBoutonsMargins"></asp:Button><br/><br/><br/>
                    <asp:Table ID="GestionInactiviteVendeur" CssClass="table" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Nom de la table</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Nombre d'enregistrements</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell>PPGestionnaires</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblgestionnaires" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPClients</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblclients" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPVendeursClients</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblvendeursclients" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPArticlesEnPanier</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblarticlesenpanier" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPVendeurs</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblvendeurs" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPCommandes</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblcommandes" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPDetailsCommandes</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lbldetailscommandes" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPProduits</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblproduits" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPCategories</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblcategories" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPTypesLivraison</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lbltypeslivraison" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPPoidsLivraisons</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblpoidslivraisons" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPTypesPoids</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lbltypespoids" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPTaxeFederale</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lbltaxefederale" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPTaxeProvinciale</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lbltaxeprovinciale" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>PPHistoriquePaiements</asp:TableCell>
                            <asp:TableCell><asp:Label ID="lblhistoriquepaiements" runat="server"/></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
