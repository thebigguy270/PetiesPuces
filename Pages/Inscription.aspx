<%@ Page Title="Inscription" Language="C#" MasterPageFile="~/Pages/PageMaster.master" CodeFile="Inscription.aspx.cs" Inherits="InscriptionCode" %>



<asp:Content ID="content1" ContentPlaceHolderID="ContenuPrincipal" runat="server">

    <style>
        .erreur {
            color: red;
            font-size: 12px
        }
    </style>

    <asp:Panel CssClass="container mt-3" runat="server">
        <asp:Panel ID="pnInactiviteClient" runat="server" CssClass="card">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Inscription" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <asp:Panel ID="pnlInscriptionReussie" Visible="false" CssClass="alert alert-success" runat="server">
                    Vous avez été inscrit avec succès
                </asp:Panel>

                <asp:Table ID="Table1" CssClass="table-padding w-100" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label1" runat="server">Courriel:</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="Courriel1" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorCourriel1" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label2" runat="server">Confirmation de courriel:</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="Courriel2" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorCourriel2" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label3" runat="server">Mot de passe</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="MDP" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorMDP" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label4" runat="server">Confirmation de mot de passe</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="MDP2" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorMDP2" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Label runat="server" ID="erreursAdditionnelles" CssClass="erreur" />
                <br />
                <div class="text-center">
                    <asp:Button ID="btnConfirmer" Text="Confirmer l'inscription" CssClass="btn btn-secondary classBoutonLeft classBoutonsMargins" OnClick="btnConfirmer_Click" runat="server" />
                    <asp:HyperLink ID="HyperLink1" CssClass="btn btn-outline-secondary classBoutonsMargins" NavigateUrl="~/Pages/InscriptionVendeur.aspx" runat="server">Je veux être vendeur</asp:HyperLink>
                </div>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>




</asp:Content>
