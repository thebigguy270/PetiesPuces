<%@ Page Language="C#" MasterPageFile="~/Pages/PageMaster.master" AutoEventWireup="true" CodeFile="~/Pages/InscriptionVendeur.aspx.cs" Inherits="Pages_InscriptionVendeur" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContenuPrincipal" runat="server">
    <style>
        .erreur {
            color: red;
            font-size: 12px
        }
    </style>


    <asp:Panel CssClass="container mt-3" Width="70%" runat="server">
        <asp:Panel ID="pnInactiviteClient" runat="server" CssClass="card">
            <asp:Panel CssClass="card-header" runat="server" BackColor="#606060" ForeColor="White">
                <asp:Label Text="Inscription d'un vendeur" runat="server" CssClass="card-title" />
            </asp:Panel>

            <asp:Panel CssClass="card-body" runat="server">
                <asp:Panel ID="pnlInscriptionReussie" Visible="false" CssClass="alert alert-success" runat="server">
                    Vous avez été inscrit avec succès.<br />
                    Cependant vous devez attendre d'être accepté par le gestionnaire avant de pouvoir accéder à votre compte.
                </asp:Panel>

                <asp:Table ID="Table1" CssClass="table-padding w-100 table-1stcolumn-50" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label1" runat="server">Courriel:</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="Courriel1" CssClass="form-control" MaxLength="100" runat="server"></asp:TextBox>
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
                            <asp:TextBox ID="Courriel2" CssClass="form-control" MaxLength="100" runat="server"></asp:TextBox>
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
                            <asp:TextBox ID="MDP" TextMode="Password" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
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
                            <asp:TextBox ID="MDP2" TextMode="Password" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorMDP2" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Nom d'affaires:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbNomAffaires" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorNomAffaires" runat="server"></asp:Label>
                            </asp:Panel>
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
                        <asp:TableCell>Rue:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbRue" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorRue" runat="server"></asp:Label>
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
                                <asp:ListItem Text="Québec" Value="QC"></asp:ListItem>
                                <asp:ListItem Text="Ontario" Value="ON"></asp:ListItem>
                                <asp:ListItem Text="Nouveau-Brunswick" Value="NB"></asp:ListItem>
                            </asp:DropDownList>
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
                            <asp:TextBox ID="tbTelephone" MaxLength="20" CssClass="form-control" placeholder="(___) ___-____" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorTelephone" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Cellulaire:</asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="tbTelephone2" MaxLength="20" CssClass="form-control" placeholder="(___) ___-____" runat="server"></asp:TextBox>
                            <asp:Panel CssClass="invalid-feedback" runat="server">
                                <asp:Label ID="lblErrorTelephone2" runat="server"></asp:Label>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Prélèvement des taxes de vente:</asp:TableCell>
                        <asp:TableCell CssClass="text-center">
                            <asp:RadioButton ID="rbTaxesOui" GroupName="rbgTaxes" Text="Oui" CssClass="mr-3" Checked="true" runat="server" />
                            <asp:RadioButton ID="rbTaxesNon" GroupName="rbgTaxes" Text="Non" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Poids maximal accepté pour une livraison:</asp:TableCell>
                        <asp:TableCell>
                            <div class="input-group">
                                <asp:TextBox ID="tbPoidsMax" CssClass="form-control" placeholder="999999" runat="server"></asp:TextBox>

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
                        <asp:TableCell>Prix auquel une livraison est gratuite<br />(Laisser vide si la livraison ne sera jamais gratuite):</asp:TableCell>
                        <asp:TableCell>
                            <div class="input-group">
                                <asp:TextBox ID="tbPrixLivGratuite" CssClass="form-control" placeholder="99999.99" runat="server"></asp:TextBox>

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
                    <asp:Button ID="btnConfirmer" Text="Confirmer l'inscription" OnClick="btnConfirmer_Click" CssClass="btn btn-secondary classBoutonLeft classBoutonsMargins" runat="server" />
                    <asp:Button ID="btnRetour" Text="Retour" OnClick="btnRetour_Click" CssClass="btn btn-dark classBoutonsMargins" runat="server" />
                </div>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>

    <script>
        $(document).ready(function () {
            $('#<%= tbTelephone.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbTelephone2.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbCodePostal.ClientID %>').inputmask('a9a 9a9');
            $('#<%= tbPoidsMax.ClientID %>').inputmask({
                mask: '9{1,6}',
                greedy: false,
                placeholder: ''
            });
            $('#<%= tbPrixLivGratuite.ClientID %>').inputmask({
                mask: '9{1,5}[.9{1,2}]',
                placeholder: ''
            })
        })
    </script>
</asp:Content>
