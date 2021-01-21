<%@ Page Title="Profil du client" Language="C#" MasterPageFile="~/Pages/PageMaster.master" AutoEventWireup="true" CodeFile="ProfilClient.aspx.cs" Inherits="ProfilClient" %>

<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server">

    <asp:Panel CssClass="container mt-3" runat="server">
        <asp:Panel ID="Panel1" runat="server" CssClass="card mb-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Profil" runat="server" CssClass="card-title" />
            </asp:Panel>

            <asp:Panel CssClass="card-body" runat="server">
                <asp:Panel ID="pnlReussite" CssClass="alert alert-success" role="alert" Visible="false" runat="server">Les informations ont été sauvegardées avec succès!</asp:Panel>
                <asp:Table CssClass="table-padding w-100" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>Adresse Email:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbAdresseEmail" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Prénom:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbPrenom" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorPrenom" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Nom:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbNom" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorNom" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>No Civique et Rue:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbNoCiviqueRue" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorNoCiviqueRue" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Ville:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbVille" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorVille" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Province:</asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlProvince" CssClass="form-control" runat="server">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Québec" Value="QC"></asp:ListItem>
                                <asp:ListItem Text="Ontario" Value="ON"></asp:ListItem>
                                <asp:ListItem Text="Nouveau-Brunswick" Value="NB"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="invalid-feedback">
                                <asp:Label ID="lblErrorProvince" runat="server"></asp:Label>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Pays:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbPays" Enabled="false" CssClass="form-control" runat="server">Canada</asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Code Postal:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbCodePostal" MaxLength="7" CssClass="form-control" placeholder="A9A 9A9" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorCodePostal" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Téléphone:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbTelephone" CssClass="form-control" placeholder="(999) 999-9999" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorTelephone" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Cellulaire:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbCellulaire" CssClass="form-control" placeholder="(999) 999-9999" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorCellulaire" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br />
                <div class="text-center">
                    <asp:Button ID="btnSauvegarder" Text="Sauvegarder les modifications" CssClass="btn btn-dark classBoutonLeft classBoutonsMargins" OnClick="btnSauvegarder_OnClick" runat="server" />
                    <asp:Button ID="btnAnnuler" Text="Annuler les modifications" CssClass="btn btn-secondary classBoutonsMargins" OnClick="btnAnnuler_OnClick" UseSubmitBehavior="false" runat="server" />
                </div>
            </asp:Panel>
        </asp:Panel>

        <div class="card mb-3">
            <div class="card-header bg-dark-blue">
                <span class="card-title">Changement de mot de passe</span>
            </div>

            <div class="card-body">
                <asp:Panel ID="pnlReussiteMDP" CssClass="alert alert-success" role="alert" Visible="false" runat="server">Les informations ont été sauvegardées avec succès!</asp:Panel>
                <table class="table-padding w-100">
                    <tr>
                        <td>Ancien mot de passe:</td>
                        <td>
                            <asp:TextBox ID="tbAncienMDP" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                            <div class="invalid-feedback">
                                <asp:Label ID="lblErrorAncienMDP" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Nouveau mot de passe:</td>
                        <td>
                            <asp:TextBox ID="tbNouveauMDP" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                            <div class ="invalid-feedback">
                                <asp:Label ID="lblErrorNouveauMDP" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Confirmation mot de passe:</td>
                        <td>
                            <asp:TextBox ID="tbConfirmationMDP" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                            <div class ="invalid-feedback">
                                <asp:Label ID="lblErrorConfirmationMDP" runat="server"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>
                <br />
                <div class="text-center">
                    <asp:Button ID="btnSauvegarderMDP" Text="Sauvegarder les modifications" CssClass="btn btn-dark classBoutonLeft classBoutonsMargins" OnClick="btnSauvegarderMDP_OnClick" runat="server" />
                    <asp:Button ID="btnAnnulerMDP" Text="Annuler les modifications" CssClass="btn btn-secondary classBoutonsMargins" OnClick="btnAnnuler_OnClick" UseSubmitBehavior="false" runat="server" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Label ID="lblTests" runat="server" />

    <script>
        $(document).ready(function () {
            $('#<%= tbTelephone.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbCellulaire.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbCodePostal.ClientID %>').inputmask('a9a 9a9');
        });
    </script>
</asp:Content>
