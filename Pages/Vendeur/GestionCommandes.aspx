<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestionCommandes.aspx.cs" Inherits="Pages_Vendeur_GestionCommandes"  MasterPageFile="~/Pages/PageMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <script>
        function ouvrirPDF(texte) {
            window.open('/Factures/' + texte);   
        }
    </script>
</asp:Content>
<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    
    <asp:Panel runat="server" ID="panelAvecModal"></asp:Panel>
    <asp:Panel runat="server" CssClass="container">
                <asp:panel ID="pnCommandesClient" runat="server" CssClass="card mt-3 mb-3">
                        <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                            <asp:Label Text="Commandes non traitées" runat="server" CssClass="card-title h4"/>
                        </asp:Panel>
                        <asp:panel CssClass="card-body" runat="server">
                           <asp:Table ID="tableCommandesNonTraitee" CssClass="table table-hover fake-button" runat="server" Visible="true" >
                                <asp:TableHeaderRow>
                      
                                    <asp:TableHeaderCell Text="Nom du client"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Date de la commande"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Valeur Totale"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Poids"></asp:TableHeaderCell>
                        
                                    <asp:TableHeaderCell></asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                            <asp:Panel runat="server" ID="divBTN" CssClass="text-center">
                                <asp:Button ID="btnAnnuler" Text="Accueil" CssClass="btn btn-dark classBoutonsMargins" runat="server" OnClick="retour"/>
                            </asp:Panel>

                        </asp:Panel>

                           
                 </asp:panel>


        <asp:panel ID="pnCommandesTraitees" runat="server" CssClass="card mt-3 mb-3">
                        <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                            <asp:Label Text="Historique des commandes" runat="server" CssClass="card-title h4"/>
                        </asp:Panel>
                        <asp:panel CssClass="card-body" runat="server">
                           <asp:Table ID="tableHistorique" CssClass="table table-hover fake-button" runat="server" Visible="true">
                                <asp:TableHeaderRow>
                      
                                    <asp:TableHeaderCell Text="Nom du client"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Date de la commande"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Valeur Totale"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Text="Poids"></asp:TableHeaderCell>
                        
                                </asp:TableHeaderRow>
                            </asp:Table>
                            <asp:Panel runat="server" ID="divBTN2" CssClass="text-center">
                                <asp:Button ID="btnRetour2" Text="Accueil" CssClass="btn btn-dark classBoutonsMargins" runat="server" OnClick="retour" />
                                </asp:panel>
                        </asp:Panel>

                           
                 </asp:panel>
            </asp:Panel>
</asp:Content>
