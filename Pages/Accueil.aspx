<%@ Page Title="Accueil" Language="C#" MasterPageFile="~/Pages/PageMaster.master" CodeFile="Accueil.aspx.cs" Inherits="AccueilCode" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <style>
        .imgResize {
            height: 64px;
            /*width: 64px;*/
        }

        .coleurBG {
            background-color: #f7f7f7;
        }

        .moneyDroite {
            text-align: right;
        }
    </style>
    <asp:Panel CssClass="container-fluid mt-3" runat="server">

        <!-- panel de droite -->

            <asp:Panel runat="server" CssClass="mb-3" ID="panelInformations" Visible="false">
                <asp:Panel ID="Panel3" runat="server" CssClass="card">
                    <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                        <asp:Label Text="Vos informations" runat="server" CssClass="card-title h4" />
                    </asp:Panel>
                    <asp:Panel ID="pnInfos" CssClass="card-body" runat="server">
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>

        </asp:Panel>

        <asp:Panel ID="pnClientPanier" runat="server" CssClass="row" Visible="false">
            <asp:Panel runat="server" CssClass="col-12 mb-3">
                <asp:Panel ID="pnInactiviteClient" runat="server" CssClass="card">
                    <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                        <asp:Label Text="Résumé des paniers" runat="server" CssClass="card-title h4" />
                    </asp:Panel>
                    <asp:Panel ID="pnPaniers" CssClass="card-body" runat="server">
                        <!-- <asp:Table ID="GestionInactiviteClient" CssClass="table" runat="server">
                    </asp:Table> -->

                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>



        <asp:Panel ID="Panel1" runat="server" CssClass="card mb-3" >
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label Text="Catégories" runat="server" CssClass="card-title h4" />
            </asp:Panel>
            <asp:Panel ID="pnHaut" CssClass="card-body" runat="server">
            </asp:Panel>
        </asp:Panel>

        <asp:Panel ID="PanelVendeurs" runat="server" CssClass="card mb-3" Visible="false">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label Text="Vendeurs" runat="server" CssClass="card-title h4" />
            </asp:Panel>
            <asp:Panel ID="pnVendeurs" CssClass="card-body" runat="server">
            </asp:Panel>
        </asp:Panel>



    </asp:Panel>

    <script>
        function panierVendeur(noVendeur) {
            window.location.href = '/Pages/Client/Panier.aspx?NoVendeur=' + noVendeur;
        }
    </script>
</asp:Content>
