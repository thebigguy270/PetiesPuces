<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Connexion.aspx.cs" Inherits="_Default" MasterPageFile="~/Pages/PageMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <style>
        .titre {
            text-shadow: 3px 6px 2px #333;
            font-size: medium;
        }

        .erreur {
            color: red;
            font-size: 12px
        }
    </style>
</asp:Content>

<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server" CssClass="container">

    <asp:Panel CssClass="container mt-3" runat="server" Width="75%">
        <asp:Panel ID="pnInactiviteClient" runat="server" CssClass="card">

            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Connexion" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server" HorizontalAlign="Center">
                <asp:Table CssClass="table-padding test-table-center mb-3" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>Adresse Email:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbNomU" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Label runat="server" CssClass="erreur">
                                <asp:RequiredFieldValidator ID="valNomU" runat="server"
                                    ControlToValidate="tbNomU"
                                    EnableClientScript="false"
                                    Display="dynamic"
                                    ErrorMessage="Le nom d'utilisateur doit être défini!" />
                            </asp:Label>

                        </asp:TableCell>

                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Mot de passe:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbMDP" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:Label runat="server" CssClass="erreur">
                                <asp:RequiredFieldValidator ID="valMDP" runat="server"
                                    ControlToValidate="tbMDP"
                                    EnableClientScript="false"
                                    Display="dynamic"
                                    ErrorMessage="Le mot de passe doit être défini!" />
                            </asp:Label>
                        </asp:TableCell>


                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="erreur">
                            <asp:Label runat="server" ID="lblErreurSQL"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <div class="text-center">
                    <asp:Button ID="btnSauvegarder" Text="Se Connecter" CssClass="btn btn-secondary classBoutonLeft classBoutonsMargins" OnClick="btnConnexion_Click" runat="server" />
                    <asp:Button ID="btnAnnuler" Text="Oublie de mot de passe" CssClass="btn btn-outline-secondary classBoutonsMargins" OnClick="btnAnnuler_OnClick" runat="server" />
                </div>

            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
