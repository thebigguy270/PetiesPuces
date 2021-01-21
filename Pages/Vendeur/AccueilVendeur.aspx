<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccueilVendeur.aspx.cs" Inherits="AccueilVendeur"  MasterPageFile="~/Pages/PageMaster.master"%>


<asp:Content ContentPlaceHolderID="Head" runat="server">
        <style>
        .imgResize {
            height: 64px;
            width: 64px;
        }

        .table-hover tr:hover {
        background:#00ff00;
        }
    </style>

    <script>
        function ouvrirPDF(texte) {
            window.open('/Factures/' + texte);   
        }

        function ouvrirDetailProduit(texte) {
            window.location.assign('/Pages/Vendeur/InscriptionProduit.aspx?T=details&ID=' + texte); 
        }
    </script>
</asp:Content>



<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server">

<!-- The Modal -->
    <asp:Panel runat="server" ID="panelAvecModal"></asp:Panel>

        <asp:Panel runat="server" CssClass="container-fluid mt-3 mb-3">


                        <!-- Nombre de visites -->
              <asp:panel ID="Panel1" runat="server" CssClass="card mb-3">
            <asp:Panel CssClass="card-header text-center bg-dark-blue" runat="server">
                <asp:Label Text="Vos informations de clientèle" runat="server" CssClass="card-title h4"/>
            </asp:Panel>
             <asp:Panel ID="panelNBVisite" CssClass="card-body text-center" runat="server">

             </asp:Panel>  
             </asp:Panel>



            <!-- Panneau pour les commandes non-traitées -->
            <asp:Panel ID="pnCommandesClient" runat="server" CssClass="card mb-3">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label Text="Commandes non traitées" runat="server" CssClass="card-title h4"/>
            </asp:Panel>
            <asp:panel ID="pnUseless" CssClass="card-body" runat="server">
                <asp:Table ID="tableCommandesNonTraitee" CssClass="table table-hover fake-button" runat="server" Visible="true">
                    <asp:TableHeaderRow>
                      
                        <asp:TableHeaderCell Text="Nom du client"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Text="Date de la commande"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Text="Valeur Totale" CssClass="text-right" ></asp:TableHeaderCell>
                        <asp:TableHeaderCell Text="Poids" CssClass="text-right" ></asp:TableHeaderCell>
                        
                        <asp:TableHeaderCell></asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </asp:panel>
            </asp:Panel>


            <asp:panel ID="pnPaniersVendeur" runat="server" CssClass="card">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label Text="Paniers courants" runat="server" CssClass="card-title h4"/>
            </asp:Panel>
                <asp:panel CssClass="card-body" ID="panelPanier" runat="server">

                </asp:panel>
            </asp:Panel>


    </asp:Panel>
</asp:Content>
