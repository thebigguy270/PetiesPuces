<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InfoProduit.aspx.cs" Inherits="Pages_Client_InfoProduit" MasterPageFile="~/Pages/PageMaster.master"%>

<asp:Content runat="server" ContentPlaceHolderID="Head">

</asp:Content>

<asp:Content ID="InscriptionProduit" ContentPlaceHolderID="ContenuPrincipal" runat="server">
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
                    NbItems:ddlProduit.value
                },
                error: (xhr, status, errorThrown) => {
                    console.log('Error');
                },
                success: (data, status, xhr) => {
                    alert("Le produit a été ajouté au panier");
                    window.location.href = "/Pages/Client/Catalogue.aspx";
                    }
                });
            }
            </script>
     <asp:HiddenField id="hiddenImage" runat="server" value=""/>
    <asp:Panel runat="server" CssClass="container">

        <asp:Panel ID="pnCommandesClient" runat="server" CssClass="card mt-3 mb-3">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label Text="Produit" runat="server" CssClass="card-title h4" />
            </asp:Panel>
            <asp:Panel runat="server" CssClass="row card-body">
                <asp:Panel ID="panelImage" runat="server" CssClass="col-3">
                </asp:Panel>
                <asp:Panel ID="panelDetails" CssClass="col" runat="server">
            </asp:Panel>
        </asp:Panel>
      </asp:Panel>
        </asp:Panel>
</asp:Content>
