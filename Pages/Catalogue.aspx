<%@ Page Title="Catalogue" Language="C#" MasterPageFile="~/Pages/PageMaster.master" CodeFile="Catalogue.aspx.cs" Inherits="CatalogueCode" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <style>
        .imgResize {
            height: 96px;
            width: 96px;
        }
    </style>
    <asp:Panel CssClass="container" runat="server">
        <asp:Panel ID="pnInactiviteClient" runat="server" CssClass="card mt-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Catalogue du vendeur X" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">

                <asp:TextBox ID="txtSearchMaster" runat="server" CssClass=""></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Rechercher par..." CssClass="btn btn-secondary" /><br />
                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                    <asp:ListItem Value="Date">Date</asp:ListItem>
                    <asp:ListItem Value="noProd">Numéro de produit</asp:ListItem>
                    <asp:ListItem Value="cat">Catégorie</asp:ListItem>
                    <asp:ListItem Value="desc">Description du produit</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Table runat="server" ID="tableProduits" CssClass="table">
                </asp:Table>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>







</asp:Content>
