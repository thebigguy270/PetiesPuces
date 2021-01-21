<%@ Page Title="Panier" Language="C#" MasterPageFile="~/Pages/PageMaster.master" AutoEventWireup="true" CodeFile="Panier.aspx.cs" Inherits="PanierCode" %>

<asp:Content ID="GestionPanier" ContentPlaceHolderID="ContenuPrincipal" runat="server">

    <asp:HiddenField ID="hidVendeur" runat="server" />
    <asp:HiddenField ID="hidMontantTotal" runat="server" />
    <asp:HiddenField ID="hidCommande" runat="server" />

    <asp:Panel CssClass="container mt-3" runat="server">
        <asp:Panel CssClass="breadcrumb-custom" runat="server">
            <span>
                <asp:Button ID="btnBreadPanier" Text="Panier" Enabled="false" CssClass="btn font-weight-bold" OnClick="btnBreadPanier_OnClick" runat="server" /></span>
            <span>
                <asp:Button ID="btnBreadProfil" Text="Profil" Enabled="false" CssClass="btn" OnClick="btnBreadProfil_OnClick" runat="server" /></span>
            <span>
                <asp:Button ID="btnBreadLivraison" Text="Livraison" Enabled="false" CssClass="btn" UseSubmitBehavior="false" runat="server" /></span>
            <span>
                <asp:Button ID="btnBreadPaiement" Text="Paiement" Enabled="false" CssClass="btn" runat="server" /></span>
            <span>
                <asp:Button ID="btnBreadResume" Text="Résumé" Enabled="false" CssClass="btn" runat="server" /></span>
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlPanier" Visible="false" CssClass="container-fluid" runat="server">
        <asp:DropDownList ID="ddlVendeurs" CssClass="form-control mb-3" AutoPostBack="true" OnSelectedIndexChanged="ddlVendeurs_OnSelectedIndexChanged" runat="server"></asp:DropDownList>
        <asp:Panel ID="pnlPaniersGeneres" runat="server"></asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlProfil" Visible="false" CssClass="container-fluid" runat="server">
        <asp:Panel ID="Panel1" runat="server" CssClass="card">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label Text="Profil" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <asp:Panel ID="pnlErreursProfil" runat="server">
                </asp:Panel>
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
                <asp:Button ID="btnLivraison" Text="Options de livraisons" OnClick="btnLivraison_OnClick" CssClass="btn btn-secondary btn-block" runat="server" />
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlConfirmation" Visible="false" CssClass="container-fluid mt-3 mb-3" runat="server">
        <div class="row">
            <div class="col-lg-7 mb-3">
                <asp:Panel CssClass="card" runat="server">
                    <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                        <asp:Label ID="lblNomVendeurPanierLivraison" CssClass="card-title" runat="server"></asp:Label>
                    </asp:Panel>

                    <asp:Panel ID="pnlPanierLivraison" CssClass="card-body" runat="server"></asp:Panel>
                </asp:Panel>
            </div>

            <div class="col-lg-5">
                <asp:Panel CssClass="card mb-3" runat="server">
                    <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                        <asp:Label CssClass="card-title" runat="server">Livraison</asp:Label>
                    </asp:Panel>

                    <asp:Panel CssClass="card-body" runat="server">
                        <span>Options de livraison:</span><br />
                        <asp:DropDownList ID="ddlOptionsEnvoi" AutoPostBack="true" OnSelectedIndexChanged="ddlOptionsEnvois_OnChange" CssClass="form-control" runat="server"></asp:DropDownList>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="Panel2" runat="server" CssClass="card mb-3">
                    <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                        <asp:Label CssClass="card-title" runat="server">Résumé du prix du panier</asp:Label>
                    </asp:Panel>
                    <asp:Panel CssClass="card-body" runat="server">
                        <asp:Table CssClass="table table-vertical-middle" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>Poids de la livraison:</asp:TableCell>
                                <asp:TableCell CssClass="text-right">
                                    <asp:Label ID="lblPoidsLivraison" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Prix de la livraison:</asp:TableCell>
                                <asp:TableCell CssClass="text-right">
                                    <asp:Label ID="lblPrixLivraison" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>TPS:</asp:TableCell>
                                <asp:TableCell CssClass="text-right">
                                    <asp:Label ID="lblPrixTPS" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>TVQ:</asp:TableCell>
                                <asp:TableCell CssClass="text-right">
                                    <asp:Label ID="lblPrixTVQ" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>Total:</asp:TableCell>
                                <asp:TableCell CssClass="text-right">
                                    <asp:Label ID="lblPrixTotal" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>

                        <input id="btnConfirmerEtPayer" type="button" value="Confirmer et payer" class="btn btn-secondary btn-block" onclick="confirmerEtPayer(this)" />
                    </asp:Panel>
                </asp:Panel>

                <div id="divCardInfosCarte" class="card d-none">
                    <div class="card-header bg-dark-blue">
                        <span class="card-title">Informations de carte de crédit</span>
                    </div>

                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtNomCredit">Nom</label>
                            <input id="txtNomCredit" type="text" class="form-control" value="Nom" />
                            <small class="form-text text-muted">Comme il apparaît sur la carte</small>
                            <div class="invalid-feedback">
                                <span id="spErrNomCredit"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="txtNumeroCredit">Numéro de la carte</label>
                            <input id="txtNumeroCredit" type="text" class="form-control" maxlength="16" value="1111222233334444" />
                            <small class="form-text text-muted">Sans tiret ou espace</small>
                            <div class="invalid-feedback">
                                <span id="spErrNumeroCredit"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col">
                                    <label for="txtDateCredit">Date d'expiration</label>
                                    <input id="txtDateCredit" type="text" class="form-control" maxlength="7" placeholder="mm-aaaa" value="11-2020" />
                                    <div class="invalid-feedback">
                                        <span id="spErrDateCredit"></span>
                                    </div>
                                </div>
                                <div class="col">
                                    <label for="txtSecuriteCredit">Numéro de sécurité</label>
                                    <input id="txtSecuriteCredit" type="text" class="form-control" maxlength="4" placeholder="0000" value="1234" />
                                    <small class="form-text text-muted">3 ou 4 numéros</small>
                                    <div class="invalid-feedback">
                                        <span id="spErrSecuriteCredit"></span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <input id="btnPayerCredit" value="Payer" type="button" class="btn btn-secondary btn-block" onclick="payerLesi()" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlResume" Visible="false" CssClass="container-fluid mb-3" runat="server">
        <asp:Panel ID="Panel3" runat="server" CssClass="card">
            <asp:Panel CssClass="card-header bg-dark-blue" runat="server">
                <asp:Label runat="server" CssClass="card-title">Résultat de la commande</asp:Label>
            </asp:Panel>
            <asp:Panel ID="pnlResumeBody" CssClass="card-body" Height="650px" runat="server">
                <asp:Label ID="lblErrorResultat" Visible="false" runat="server"></asp:Label>
                <iframe id="framePDF" class="w-100 h-100" runat="server"></iframe>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>

    <!-- Modal -->
    <asp:Panel ID="exampleModal" CssClass="modal fade" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" runat="server">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Erreur de commande</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="closeModal();">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblModalText" runat="server"></asp:Label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="closeModal();">Fermer</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div id="divDynHTML" class="d-none"></div>

    <script>
        function closeModal() {
            let modal = document.getElementById('<%= exampleModal.ClientID %>');

            modal.classList.remove('show', 'd-block');

            window.location.href = window.location.href;
        }

        /**
         * @param {number} num
         * @param {number} min
         * @param {number} max
         */
        function between(num, min, max) {
            return num >= min && num <= max;
        }

        function printPdf(pdfNum) {
            let w = window.open('/Factures/' + pdfNum + '.pdf');
            w.print();
        }

        function supprimerArticle(noArticle) {
            $.ajax({
                url: '/AJAX/TestService.asmx/SupprimerArticle',
                method: 'POST',
                data: { DelArticle: noArticle },
                error: (xhr, status, errorThrown) => {
                    console.log('Error');
                },
                success: (data, status, xhr) => {
                    console.log('success');
                    console.log(data);
                    location.reload();
                }
            });
        }

        function supprimerPanier(noClient, noVendeur) {
            $.ajax({
                url: '/AJAX/TestService.asmx/SupprimerPanier',
                method: 'POST',
                data: { NoClient: noClient, NoVendeur: noVendeur },
                error: (xhr, status, errorThrown) => {
                    console.log('Error');
                },
                success: (data, status, xhr) => {
                    console.log('success');
                    console.log(data);
                    location.reload();
                }
            });
        }

        /**
         * @param {number} noArticle
         * @param {HTMLSelectElement} select
         */
        function quantiteArticle(noArticle, select) {
            let nbItems = parseInt(select.selectedOptions[0].value);

            $.ajax({
                url: '/AJAX/TestService.asmx/ChangerQuantite',
                method: 'POST',
                data: { NoPanier: noArticle, NewNbItems: nbItems },
                error: (xhr, status, errorThrown) => {
                    console.log('Error');
                    console.log(xhr)
                    console.log(xhr.responseText)
                    let x = window.open();
                    x.document.open();
                    x.document.write(xhr.responseText);
                    x.document.close();
                    console.log(status)
                    console.log(errorThrown)
                },
                success: (data, status, xhr) => {
                    console.log('success');
                    console.log(data);
                    //location.reload();
                    document.location.reload(true);
                }
            });
        }

        /**
         * @param {HTMLButtonElement} btn
         */
        function confirmerEtPayer(btn) {
            /** @type HTMLDivElement */
            let cardInfosCarte = document.getElementById('divCardInfosCarte');
            cardInfosCarte.classList.remove('d-none');
            btn.classList.add('d-none');

            /** @type HTMLButtonElement */
            let btnLivraison = document.getElementById('<%= btnBreadLivraison.ClientID %>');
            /** @type HTMLButtonElement */
            let btnPaiement = document.getElementById('<%= btnBreadPaiement.ClientID %>');

            btnLivraison.disabled = false;
            btnLivraison.classList.remove('aspNetDisabled', 'font-weight-bold');
            btnLivraison.classList.add('btn-secondary');
            btnLivraison.addEventListener('click', livraisonClick);

            btnPaiement.classList.add('font-weight-bold');
        }

        function livraisonClick() {
            let btn = document.getElementById('<%= btnBreadLivraison.ClientID %>');
            /** @type HTMLDivElement */
            let cardInfosCarte = document.getElementById('divCardInfosCarte');
            cardInfosCarte.classList.add('d-none');

            let btnConfirmerEtPayer = document.getElementById('btnConfirmerEtPayer');
            btnConfirmerEtPayer.classList.remove('d-none');

            /** @type HTMLButtonElement */
            let btnPaiement = document.getElementById('<%= btnBreadPaiement.ClientID %>');
            btnPaiement.classList.remove('font-weight-bold');

            btn.classList.add('aspNetDisabled', 'font-weight-bold');
            btn.classList.remove('btn-secondary');
            btn.disabled = true;
            btn.removeEventListener('click', livraisonClick);
        }

        /**
         * @param {HTMLInputElement} elem
         * @param {HTMLSpanElement} errElem
         * @param {string} error
         */
        function invalidateInput(elem, errElem, error) {
            elem.classList.add('is-invalid');
            errElem.innerHTML = error;
        }

        /**
         * @param {HTMLInputElement} elem
         */
        function resetValidation(elem) {
            elem.classList.remove('is-invalid');
        }

        /**
         * 
         * @param {HTMLInputElement} elem
         * @param {HTMLSpanElement} errElem
         * @param {string} error
         * @returns {boolean}
         */
        function invalidateEmpty(elem, errElem, error) {
            let ret = elem.value == "";

            if (ret)
                invalidateInput(elem, errElem, error);

            return ret;
        }

        /**
         * @param {HTMLInputElement} elem
         * @param {HTMLSpanElement} errElem
         * @param {RegExp} regex
         * @param {string} error
         */
        function invalidateRegex(elem, errElem, regex, error) {
            let ret = !regex.test(elem.value);

            if (ret)
                invalidateInput(elem, errElem, error);

            return ret;
        }

        /**
         * @param {HTMLInputElement} elem
         * @param {HTMLSpanElement} errElem
         * @param {string} error
         */
        function invalidateDate(elem, errElem, error) {
            let dateNow = new Date();
            let moisCourrant = dateNow.getMonth() + 1;
            let anneeCourrante = dateNow.getFullYear();
            let moisEtAnnee = elem.value.split('-');
            let mois = parseInt(moisEtAnnee[0]);
            let annee = parseInt(moisEtAnnee[1]);

            let ret = ((annee == anneeCourrante) && !between(mois, moisCourrant, 12))
                || (!between(mois, 1, 12) || !between(annee, anneeCourrante, 2023));

            if (ret)
                invalidateInput(elem, errElem, error);

            return ret;
        }

        /**
         * @param {HTMLInputElement} elem
         * @param {HTMLSpanElement} errElem
         * @param {string} error
         */
        function invalidateNumeroCarte(elem, errElem, error) {
            let firstFour = parseInt(elem.value.substr(0, 4));
            let ret = firstFour === 0 || (firstFour > 3 && firstFour < 1000) || firstFour > 5000;

            if (ret)
                invalidateInput(elem, errElem, error);

            return ret;
        }

        function payerLesi() {
            let txtNomCredit = document.getElementById('txtNomCredit');
            let txtNumeroCredit = document.getElementById('txtNumeroCredit');
            let txtDateCredit = document.getElementById('txtDateCredit');
            let txtSecuriteCredit = document.getElementById('txtSecuriteCredit');
            let spErrNomCredit = document.getElementById('spErrNomCredit');
            let spErrNumeroCredit = document.getElementById('spErrNumeroCredit');
            let spErrDateCredit = document.getElementById('spErrDateCredit');
            let spErrSecuriteCredit = document.getElementById('spErrSecuriteCredit');
            /** @type HTMLSelectElement */
            let ddlLivraison = document.getElementById('<%= ddlOptionsEnvoi.ClientID %>');

            resetValidation(txtNomCredit);
            resetValidation(txtNumeroCredit);
            resetValidation(txtDateCredit);
            resetValidation(txtSecuriteCredit);

            let errors = [
                invalidateEmpty(txtNomCredit, spErrNomCredit, "Le nom doit être présent"),
                invalidateEmpty(txtNumeroCredit, spErrNumeroCredit, "Le numéro de carte de crédit doit être présent")
                || invalidateRegex(txtNumeroCredit, spErrNumeroCredit, /^\d{16}$/, "Le numéro de carte de crédit doit être composé de 16 chiffres")
                || invalidateNumeroCarte(txtNumeroCredit, spErrNumeroCredit, "Le numéro de carte de crédit doit commencer par 1000 ou plus (4 premiers chiffres)"),
                invalidateEmpty(txtDateCredit, spErrDateCredit, "La date d'expiration doit être présente")
                || invalidateRegex(txtDateCredit, spErrDateCredit, /^\d{2}-\d{4}$/, "La date d'expiration doit suivre le format suivant: mm-aaaa (chiffres seulement)")
                || invalidateDate(txtDateCredit, spErrDateCredit, "La date d'expiration doit être entre aujourd'hui et décembre 2023"),
                invalidateEmpty(txtSecuriteCredit, spErrSecuriteCredit, "Le code de sécurité doit être présent")
                || invalidateRegex(txtSecuriteCredit, spErrSecuriteCredit, /^\d{3,4}$/, "Le code de sécurité doit être entre 000 et 9999")
            ];

            if (!errors.includes(true)) {
                /** @type HTMLInputElement */
                let hidVendeur = document.getElementById('<%= hidVendeur.ClientID %>');
                let hidMontantTotal = document.getElementById('<%= hidMontantTotal.ClientID %>');
                let hidCommande = document.getElementById('<%= hidCommande.ClientID %>');
                let noVendeur = hidVendeur.value;
                let montantTotal = hidMontantTotal.value;

                /** @type HTMLSpanElement */
                let lblNomVendeurPanierLivraison = document.getElementById('<%= lblNomVendeurPanierLivraison.ClientID %>');
                let nomAffaires = lblNomVendeurPanierLivraison.innerHTML;

                post('https://personnel.lmbrousseau.info/lesi-2019/lesi-effectue-paiement.php', {
                    NoVendeur: noVendeur,
                    NomVendeur: nomAffaires,
                    NoCarteCredit: txtNumeroCredit.value,
                    DateExpirationCarteCredit: txtDateCredit.value,
                    NoSecuriteCarteCredit: txtSecuriteCredit.value,
                    MontantPaiement: montantTotal,
                    NomPageRetour: window.location.href,
                    InfoSuppl: hidCommande.value.replace(/"/g, '')
                });
            }
        }

        /**
         * https://stackoverflow.com/questions/133925/javascript-post-request-like-a-form-submit
         * sends a request to the specified url from a form. this will change the window location.
         * @param {string} path the path to send the post request to
         * @param {object} params the paramiters to add to the url
         * @param {string} [method=post] the method to use on the form
         */
        function post(path, params, method) {
            method = method || "post"; // Set method to post by default if not specified.

            // The rest of this code assumes you are not using a library.
            // It can be made less wordy if you use one.
            var form = document.createElement("form");
            form.setAttribute("method", method);
            form.setAttribute("action", path);

            for (var key in params) {
                console.log(key + ': ' + params[key])
                if (params.hasOwnProperty(key)) {
                    var hiddenField = document.createElement("input");
                    hiddenField.setAttribute("type", "text");
                    hiddenField.setAttribute("name", key);
                    hiddenField.setAttribute("id", key);
                    hiddenField.setAttribute("value", params[key]);

                    form.appendChild(hiddenField);
                }
            }

            document.getElementById('divDynHTML').appendChild(form);
            form.submit();
        }

        $(document).ready(function () {
            $('#<%= tbTelephone.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbCellulaire.ClientID %>').inputmask('(999)999-9999');
            $('#<%= tbCodePostal.ClientID %>').inputmask('a9a 9a9');
        });
    </script>
</asp:Content>
