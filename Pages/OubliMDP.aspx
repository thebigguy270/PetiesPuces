<%@ Page Title="Inscription" Language="C#" MasterPageFile="~/Pages/PageMaster.master" CodeFile="OubliMDP.aspx.cs" Inherits="Pages_OubliMDP" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <div class="container">
        <asp:Panel ID="pnlOubli" runat="server">
            <div class="card mt-3">
                <div class="card-header">
                    <span class="card-title">Oubli de mot de passe</span>
                </div>

                <div class="card-body">
                    <asp:Panel ID="pnlError" Visible="false" CssClass="alert alert-danger" runat="server">
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </asp:Panel>
                    <table class="w-100">
                        <tr>
                            <td>Adresse email:</td>
                            <td>
                                <asp:TextBox ID="tbAdresseEmail" CssClass="form-control" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnOubliMDP" Text="Mot de passe oublié" OnClick="btnOubliMDP_OnClick" CssClass="mt-2 btn btn-secondary btn-block" runat="server" />
                            </td>
                        </tr>
                    </table>

                    <asp:Panel ID="pnlFakeEmail" Visible="false" CssClass="mt-4 border border-dark rounded p-2" runat="server">
                        <asp:Label ID="lblFakeEmail" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlNouveauMDP" Visible="false" runat="server">
            <div class="card mt-3">
                <div class="card-header">
                    <span class="card-title">Modifier mot de passe</span>
                </div>

                <div class="card-body">
                    <table class="w-100">
                        <tr>
                            <td>Nouveau mot de passe:</td>
                            <td>
                                <asp:TextBox ID="tbMDP1" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                <div class="invalid-feedback">
                                    <asp:Label ID="lblErrorMDP1" runat="server"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>Confirmation du nouveau mot de passe:</td>
                            <td>
                                <asp:TextBox ID="tbMDP2" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                <div class="invalid-feedback">
                                    <asp:Label ID="lblErrorMDP2" runat="server"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnConfirmerNouveauMDP" Text="Changer de mot de passe" CssClass="mt-2 btn btn-secondary btn-block" OnClick="btnConfirmerNouveauMDP_OnClick" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlReussite" CssClass="mt-3" Visible="false" runat="server">
            <div class="alert alert-success">
                <span>Changement de mot de passe réussit</span>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
