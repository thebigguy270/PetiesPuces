<%@ Page Title="Profil du vendeur" Language="C#" MasterPageFile="~/Pages/PageMaster.master" AutoEventWireup="true" CodeFile="ProfilVendeur.aspx.cs" Inherits="ProfilVendeur" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <style>
        .custom-file-input ~ .custom-file-label::after {
            content: "image";
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("input:file").change(function () {

                /** @type {string[]} */

                var fileName = $(this).val().split('\\');
                if (fileName.length > 1) {
                    var monNom = fileName[fileName.length - 1];
                    $("#Contenu_ContenuPrincipal_lbl").html(monNom);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <asp:Panel CssClass="container-fluid" runat="server">
        <asp:Panel runat="server" CssClass="row">
            <asp:Panel runat="server" CssClass="col-xl-8">
                <asp:Panel ID="Panel1" runat="server" CssClass="card mt-3 mb-3">
                    <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                        <asp:Label Text="Profil" runat="server" CssClass="card-title h4" />
                    </asp:Panel>

                    <asp:Panel CssClass="card-body" runat="server">
                        <asp:Panel ID="pnlReussite" CssClass="alert alert-success" role="alert" Visible="false" runat="server">Les informations ont été sauvegardées avec succès!</asp:Panel>

                        <asp:Panel ID="pnlErreur" runat="server"></asp:Panel>

                        <asp:Table CssClass="table-padding w-100" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>Adresse Email:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbAdresseEmail" MaxLength="50" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Nom d'affaires:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="tbNomAffaires" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorNomAffaires" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Prénom:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbPrenom" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorPrenom" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Nom:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbNom" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorNom" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>No civique et Rue:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbRue" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorRue" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Ville:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbVille" MaxLength="50" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorVille" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Province:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control">
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
                                    <asp:TextBox ID="tbPays" Enabled="false" runat="server" CssClass="form-control">Canada</asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Code Postal:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbCodePostal" MaxLength="7" placeholder="A9A 9A9" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorCodePostal" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Téléphone:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbTelephone" placeholder="(999) 999-9999" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorTelephone" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>

                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Cellulaire:</asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="tbCell" placeholder="(999) 999-9999" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:Panel CssClass="invalid-feedback" runat="server">
                                        <asp:Label ID="lblErrorCell" runat="server"></asp:Label>
                                    </asp:Panel>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Prélèvement des taxes de vente:</asp:TableCell>
                                <asp:TableCell CssClass="text-center">
                                    <asp:RadioButton ID="rbOuiTaxes" Text="Oui" GroupName="TaxesGroup" CssClass="mr-3" runat="server" />
                                    <asp:RadioButton ID="rbNonTaxes" Text="Non" GroupName="TaxesGroup" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Poids maximal accepté pour une livraison:</asp:TableCell>
                                <asp:TableCell>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="tbPoidsMax" CssClass="form-control"></asp:TextBox>

                                        <div class="input-group-append">
                                            <span class="input-group-text">Livre</span>
                                        </div>

                                        <asp:Panel CssClass="invalid-feedback" runat="server">
                                            <asp:Label ID="lblErrorPoidsMax" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </div>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    Prix auquel une livraison est gratuite<br />
                                    (Laisser vide si la livraison ne sera jamais gratuite):
                                </asp:TableCell>
                                <asp:TableCell>
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="tbPrixLivGratuite" CssClass="form-control"></asp:TextBox>

                                        <div class="input-group-append">
                                            <span class="input-group-text">$</span>
                                        </div>

                                        <asp:Panel CssClass="invalid-feedback" runat="server">
                                            <asp:Label ID="lblErrorPoidsLivGratuit" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </div>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <br />
                        <div class="text-center">
                            <asp:Button ID="btnSauvegarder" Text="Sauvegarder les modifications" CssClass="btn btn-dark classBoutonLeft classBoutonsMargins" OnClick="btnSauvegarder_OnClick" runat="server" />
                            <asp:Button ID="btnAnnuler" Text="Annuler les modifications" CssClass="btn btn-secondary classBoutonsMargins" OnClick="btnAnnuler_OnClick" runat="server" />
                        </div>
                    </asp:Panel>
                </asp:Panel>

            </asp:Panel>

            <asp:Panel runat="server" CssClass="col-xl-4 mt-3 mb-3">
                <asp:Panel runat="server" CssClass="card">
                    <asp:Panel runat="server" CssClass="card-header bg-dark-blue">
                        <asp:Label runat="server" CssClass="card-tile h4" Text="Personnalisation de votre catalogue"></asp:Label>
                    </asp:Panel>

                    <asp:Panel ID="panelPerso" runat="server" CssClass="card-body">
                        <!-- couleur de fond -->
                        <asp:Panel runat="server" CssClass="row">
                            <asp:Panel runat="server" CssClass="col-4">
                                <asp:Label runat="server" CssClass="h5" Text="Thème : ">

                                </asp:Label>
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="col-8">
                                <asp:TextBox runat="server" ID="couleurFond" CssClass="form-control"
                                    type="color" value="#ffffff" Text="#ffffff" />
                            </asp:Panel>
                        </asp:Panel>

                        <!-- couler du texte -->
                        <asp:Panel runat="server" CssClass="row mt-3">
                            <asp:Panel runat="server" CssClass="col-4">
                                <asp:Label runat="server" CssClass="h5" Text="Texte : ">

                                </asp:Label>
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="col-8">
                                <asp:TextBox runat="server" ID="couleurText" CssClass="form-control"
                                    type="color" value="#000000" Text="#000000" />
                            </asp:Panel>
                        </asp:Panel>

                        <!-- choix du logo -->



                    </asp:Panel>

                </asp:Panel>
            </asp:Panel>

        </asp:Panel>

        <div class="row">
            <div class="col">
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
                                    <div class="invalid-feedback">
                                        <asp:Label ID="lblErrorNouveauMDP" runat="server"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Confirmation mot de passe:</td>
                                <td>
                                    <asp:TextBox ID="tbConfirmationMDP" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                    <div class="invalid-feedback">
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
            </div>
        </div>
    </asp:Panel>

    <script>
        $(document).ready(function () {
            $('#<%= tbTelephone.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbCell.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbCodePostal.ClientID %>').inputmask('a9a 9a9');
            $('#<%= tbPoidsMax.ClientID %>').inputmask({
                mask: '9{1,6}',
                placeholder: '_'
            });
            $('#<%= tbPrixLivGratuite.ClientID %>').inputmask({
                mask: '9{1,5}[.9{1,2}]',
                placeholder: ''
            })
        });
    </script>
</asp:Content>
