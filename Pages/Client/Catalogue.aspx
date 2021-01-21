<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Catalogue.aspx.cs" Inherits="Pages_Vendeur_GestionCatalogue" MasterPageFile="~/Pages/PageMaster.master" %>


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
                <asp:Label Text="Recherche" runat="server" CssClass="card-title h4" />

            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <div class="row">
                    <div class="col-4 mb-3">
                        <asp:DropDownList ID="ddlNbItemPage" CssClass="form-control d-inline" runat="server" OnSelectedIndexChanged="ChangementNbArticlePage" AutoPostBack="true" />
                    </div>
                    <div class="col-4 mb-3">
                        <asp:DropDownList ID="ddlNomVendeur" CssClass="form-control d-inline" runat="server" OnSelectedIndexChanged="ChangementVendeur" AutoPostBack="true" />
                    </div>
                    <div class="col-4 mb-3">
                        <asp:DropDownList ID="ddlRecherche" CssClass="form-control d-inline"  runat="server" AutoPostBack="false"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <asp:TextBox ID="tbRecherche" PlaceHolder="Recherche" CssClass="form-control d-inline" runat="server" MaxLength="50" />
                    </div>
                    <div class="col-6">
                        <asp:Button ID="btnRecherche" Text="Ok" CssClass="form-control d-inline" runat="server" OnClick="Rechercher" />
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>

        <!-- tableau du catalogue -->
        <asp:Panel ID="pnCatalogue" runat="server" CssClass="card">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Image runat="server" ID="imgLogo" class="brand-icon d-inline-block" style="width: 80px;" ImageUrl="~/Static/img/logo.jpg" Visible="false"/>
                <asp:Label ID="lblProduits" Text="Produits du vendeur" runat="server" CssClass="card-title h4" />
                <asp:DropDownList ID="ddlTri" runat="server" AutoPostBack="true" CssClass="form-control float-right w-25 d-inline" OnSelectedIndexChanged="Rechercher" />
                <asp:Label ID="Label1" Text="Tri:   " runat="server" CssClass="card-title h4 float-right pr-3" />

            </asp:Panel>
            <asp:Panel ID="panelTable" CssClass="card-body" runat="server">
            </asp:Panel>
        </asp:Panel>

    </asp:Panel>
    <script>
        function ajouterArticle(noClient, noProduit, idDdl) {

            /** @type {HTMLSelectElement}*/
            let ddlProduit = document.getElementById(idDdl);

            $.ajax({
                url: '/AJAX/TestService.asmx/AjouterPanier',
                method: 'POST',
                data: {
                    NoClient: noClient,
                    NoProduit: noProduit,
                    NbItems: ddlProduit.value
                },
                error: (xhr, status, errorThrown) => {
                    console.log('Error');
                },
                success: (data, status, xhr) => {
                    console.log('success');
                    console.log(data);
                    //location.reload();
                }
            });
        }
    </script>
    <ul id="pagination" class="pagination pagination-lg justify-content-center" runat="server">
    </ul>
</asp:Content>
