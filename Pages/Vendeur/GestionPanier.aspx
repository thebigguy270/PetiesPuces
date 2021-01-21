<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestionPanier.aspx.cs" Inherits="Pages_Vendeur_GestionPanier" MasterPageFile="~/Pages/PageMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <style>
        .espace{
            margin-left:10px;
        }

        .imgResize {
            height: 64px;
            width: 64px;
        }
    </style>

    <script>
        function ouvrirDetailProduit(texte) {
            window.location.assign('/Pages/Vendeur/InscriptionProduit.aspx?T=details&ID=' + texte); 
        }
    </script>
</asp:Content>
<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <asp:Panel runat="server" ID="panelAvecModal"></asp:Panel>
        <asp:Panel runat="server" CssClass="container-fluid">
                <asp:panel ID="pnCommandesClient" runat="server" CssClass="card mt-3">
                        <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                            <asp:Label Text="Paniers courrants" runat="server" CssClass="card-title h4"/>
                        </asp:Panel>
                        <asp:panel CssClass="card-body" ID="pnHaut" runat="server">
                            <asp:Panel runat="server" CssClass="row ml-3 mb-3">
                                <asp:Label runat="server" Text="Combien de mois voulez-vous visualiser?" CssClass="labelTest col-3"></asp:Label>
                                <asp:DropDownList ID="ddlChoix" runat="server" AutoPostBack="true" CssClass="form-control col-3 text-secondary"></asp:DropDownList>
                            </asp:Panel>
                                </asp:Panel>
        </asp:panel>

            <!-- panel deux -->
            <asp:panel ID="PanelVieuxPaniers" runat="server" CssClass="card mt-3 mb-3">
                        <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                            <asp:Label Text="Paniers de plus de 6 mois" runat="server" CssClass="card-title h4"/>
                        </asp:Panel>
                        <asp:panel CssClass="card-body" ID="panelVieux" runat="server">
                        </asp:Panel>
        </asp:panel>

        </asp:Panel>
</asp:Content>
