<%@ Page Language="C#" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8" />
    <title></title>    
</head>
<body>
    <form id="form1" runat="server">
    <h3>Confirmer la commande</h3>
        <asp:Table runat="server" BorderStyle="Solid" GridLines="Both">

        <asp:TableRow><asp:TableHeaderCell>Nom du produit</asp:TableHeaderCell><asp:TableHeaderCell>Quantité commandée</asp:TableHeaderCell><asp:TableHeaderCell>Prix individuel</asp:TableHeaderCell><asp:TableHeaderCell><asp:Button runat="server" Text="Vider le panier" ID="btnToutSupprimer" /></asp:TableHeaderCell></asp:TableRow>
        <asp:TableRow><asp:TableCell> produit</asp:TableCell><asp:TableCell><asp:TextBox runat="server" ID="tbNoQ1" Text="1"></asp:TextBox></asp:TableCell><asp:TableCell>5$</asp:TableCell><asp:TableCell><asp:Button runat="server" ID="btnSupp" Text="Supprimer l'objet" /></asp:TableCell></asp:TableRow>
        </asp:Table>

        <asp:Label runat="server" ID="lblPrixavTaxes" Text="Prix Avant taxes : 5$" /><br />
        <asp:Label runat="server" ID="lblPrixAPT" Text="Prix après : 8$" /><br />
        <asp:Label runat="server" Text="Catégorie de poids:" /> 
        <asp:DropDownList runat="server" ID="ddlPoids">
            <asp:ListItem> Moins de 5 livres</asp:ListItem>
            <asp:ListItem> 5 à moins de 20 livres</asp:ListItem>
            <asp:ListItem> 20 à moins de 100 livres</asp:ListItem>
            <asp:ListItem> 100 livres et plus</asp:ListItem>
        </asp:DropDownList><br />
        <asp:Label runat="server" ID="Label1" Text="Frais livraison: 2$" /><br />
        <asp:Label runat="server" ID="Label2" Text="Prix total: 10$" /><br />
        <asp:Button runat="server" ID="btnCommande" Text="Acheter" />
    </form>
</body>
</html>
