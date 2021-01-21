<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InfosClients.aspx.cs" Inherits="Pages_Vendeur_InfosClients" MasterPageFile="~/Pages/PageMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">

</asp:Content>

<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server" CssClass="container">        
    <asp:Panel CssClass="container" runat="server">
         <asp:panel ID="Panel1" runat="server" CssClass="card mt-3 mb-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label ID="lblTitre" Text="Profil" runat="server" CssClass="card-title"/>
            </asp:Panel>
        <asp:panel CssClass="card-body" runat="server">
        <asp:Table ID="tableContenu" CssClass="table-padding" runat="server">
              
            </asp:Table>
            <br/>
            <div class="text-center">
                <asp:Button ID="btnSauvegarder" Text="Retourner au catalogue" CssClass="btn btn-secondary" runat="server" OnClick="retour" />
                
                </div>
            </asp:panel>
             </asp:Panel>
    </asp:Panel>
    </asp:Content>
