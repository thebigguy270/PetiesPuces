<%@ Page Title="Page d'accueil admin" Language="C#" MasterPageFile="~/Pages/PageMasterAdmin.master" AutoEventWireup="true" CodeFile="PageAccueilAdmin.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="GestionPrincipal" ContentPlaceHolderID="ContenuPrincipal" runat="server">

    <script>
        <!--Source https://stackoverflow.com/questions/1543017/javascript-check-all-checkboxes-in-a-table-asp-net -->
    function CheckClients(elem) {
        var div = document.getElementById("<% = GestionInactiviteClient.ClientID %>");
            var chk = div.getElementsByTagName('input');
            for (var i = 0; i < chk.length; i++) {
                if (chk[i].type === 'checkbox') {
                    chk[i].checked = elem.checked;
                }
            }
        }
        function CheckVendeurs(elem) {
            var div = document.getElementById("<% = GestionInactiviteVendeur.ClientID %>");
            var chk = div.getElementsByTagName('input');
            for (var i = 0; i < chk.length; i++) {
                if (chk[i].type === 'checkbox') {
                    chk[i].checked = elem.checked;
                }
            }
        }
        $(document).ready(function () {
            $('th').click(function () {
                var table = $(this).parents('table').eq(0)
                var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()))
                this.asc = !this.asc
                if (!this.asc) { rows = rows.reverse() }
                for (var i = 0; i < rows.length; i++) { table.append(rows[i]) }
            })
            function comparer(index) {
                return function (a, b) {
                    var valA = getCellValue(a, index), valB = getCellValue(b, index)
                    return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.toString().localeCompare(valB)
                }
            }
            function getCellValue(row, index) { return $(row).children('td').eq(index).text() }
        });
    </script>
    <asp:panel cssclass="container-fluid" runat="server">
        <!-- Début du code CSS prit de https://getbootstrap.com/docs/4.0/components/modal/  -->
        <asp:Panel runat="server">
            <div class="modal fade" id="modalDemandeVendeur" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
              <div class="modal-dialog" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Confirmer et envoyer le courriel</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                  </div>
                  <div class="modal-body" style="text-align:center">
                    <asp:Label id="modalNomVendeur" Text="Nom du vendeur" runat="server"/><br/>
                      <asp:HiddenField id="modalTbRedevance" runat="server"/>
                      <asp:TextBox id="modalCourrielVendeur" Text="Courriel du vendeur" runat="server" CssStyle="form-control-plaintext" ReadOnly="true" Columns="30" Rows="1" /><br/>
                      <asp:TextBox id="modalObjet" Text="Objet" runat="server" CssStyle="form-control-plaintext" ReadOnly="true" Columns="30" Rows="1"/><br/>
                    <asp:TextBox id="modalContenuEmail" runat="server" TextMode="Multiline" Columns="50" Rows="3"/><br/>
                  </div>
                  <div class="modal-footer">
                      <asp:Button CssClass="btn btn-primary" Text="Confirmer l'ajout du vendeur" runat="server" onClick="actionDemandeVendeur"/>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Annuler</button>
                  </div>
                </div>
              </div>
            </div>
        </asp:Panel>
<!-- fin du code CSS prit de https://getbootstrap.com/docs/4.0/components/modal/  -->

        <asp:Panel ID="pnlDemandeVendeurs" runat="server" CssClass="card mt-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Gestion des demandes de vendeurs" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel id="GestionDemandesVendeurs" CssClass="card-body" runat="server">

            </asp:Panel>
        </asp:Panel>

        <br />
        <br />
        <asp:Panel ID="PanelInactiviteClientCatalogue" runat="server" CssClass="card mt-3" visible="false">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Nombre de visite de catalogue" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <asp:Panel runat="server" CssClass="scrollClass">
                    <asp:Table ID="tableClientCatalogue" CssClass="table" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell CssClass="fake-button">No du client</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nom ou courriel</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nombre de visite de catalogue</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>

        <br />
        <br />
        <asp:Panel ID="pnInactiviteClient" runat="server" CssClass="card mt-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Gestion de l'inactivité des clients" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <asp:Label Text="Période d'inactivité des clients" runat="server" />
                <asp:DropDownList ID="ddlInactiviteClient" CssClass="mydropdownlist" runat="server" AutoPostBack="true" />
                <br />
                <br />
                <asp:Panel runat="server" CssClass="scrollClass">
                    <asp:Table ID="GestionInactiviteClient" CssClass="table" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell CssClass="fake-button">No du client</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nom du client</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nombre de mois inactif</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button"><input type="checkbox" onclick="CheckClients(this)" />
                                Sélection du ou des comptes</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Button ID="SupprimerClients" Text="Supprimer le ou les clients sélectionné(s)" runat="server" CssClass="btn btn-dark btn-block" OnClick="SupprimerLesClients" />
            </asp:Panel>
        </asp:Panel>
        <br />
        <br />

        <asp:Panel ID="pnInactiviteVendeur" runat="server" CssClass="card mt-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Gestion de l'inactivité des vendeurs" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <asp:Label Text="Période d'inactivité des vendeurs" runat="server" />
                <asp:DropDownList ID="ddlInactiviteVendeur" CssClass="mydropdownlist" runat="server" AutoPostBack="true" />
                <br />
                <br />
                <asp:Panel runat="server" CssClass="scrollClass">
                    <asp:Table ID="GestionInactiviteVendeur" CssClass="table" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell CssClass="fake-button">No du vendeur</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nom du vendeur</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nombre de mois inactif</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button"><input type="checkbox" onclick="CheckVendeurs(this)" />
                                Sélection du ou des comptes</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Button ID="SupprimerVendeurs" Text="Supprimer le ou les vendeurs sélectionné(s)" runat="server" CssClass="btn btn-dark btn-block" OnClick="SupprimerLesVendeurs"/>
            </asp:Panel>
        </asp:Panel>
         <asp:Panel ID="PanneauGestionRedevance" runat="server" CssClass="card mt-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Gestion des redevances" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <asp:Panel runat="server" CssClass="scrollClass">
                    <asp:Table ID="GestionRedevance" CssClass="table" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell CssClass="fake-button">Numéro du vendeur</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nom du vendeur</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Pourcentage de redevance</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
                <asp:Button ID="btnAppliquerRedevances" Text="Appliquer les modifications des redevances" runat="server" CssClass="btn btn-secondary btn-block" OnClick="miseAJourRedevances"/>

            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="PannelGestionCategorie" runat="server" CssClass="card mt-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Gestion des catégories" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel ID="panelGestionCategories" CssClass="card-body" runat="server">
                <asp:Panel runat="server" CssClass="scrollClass">
                    <asp:Table ID="GestionCategories" CssClass="table" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell CssClass="fake-button">Numéro de la catégorie</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Nom de la catégorie</asp:TableHeaderCell>
                            <asp:TableHeaderCell CssClass="fake-button">Suppression de la catégorie</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>

        <br />
        <br />
        <asp:Panel ID="statistiques" runat="server" CssClass="card mt-3">
            <asp:Panel CssClass="card-header" runat="server">
                <asp:Label Text="Statistiques" runat="server" CssClass="card-title" />
            </asp:Panel>
            <asp:Panel CssClass="card-body" runat="server">
                <asp:Label ID="NbTotalVendeurs" runat="server" />
                <canvas id="listeVendeurs" style="width: 70%; height: 70%"></canvas>
                <br/><br/>
                <canvas id="nombreClients" style="width: 70%; height: 70%"></canvas>
                <br/><br/>
                <canvas id="listeClients" style="width: 70%; height: 70%"></canvas>
                <br/><br/>
                <canvas id="connexionClients" style="width: 70%; height: 70%"></canvas>
                <script>
                    var ctx = document.getElementById("listeVendeurs");
                    var myChart = new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: ["1 Mois", "3 Mois", "6 Mois", "12 Mois", "Depuis le début"],
                            datasets: [{
                                label: 'Répartition des nouveaux vendeurs',
                                data: [<% repartitionDemandesVendeurs();%>],
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    'rgba(54, 162, 235, 0.2)',
                                    'rgba(255, 159, 64, 0.2)',
                                    'rgba(75, 192, 192, 0.2)',
                                    'rgba(153, 102, 255, 0.2)'
                                ],
                                borderColor: [
                                    'rgba(255,99,132,1)',
                                    'rgba(54, 162, 235, 1)',
                                    'rgba(255, 159, 64, 1)',
                                    'rgba(75, 192, 192, 1)',
                                    'rgba(153, 102, 255, 1)'
                                ],
                                borderWidth: 1
                            }]
                        },
                    });
                    var ctx = document.getElementById("nombreClients");
                    var myChart = new Chart(ctx, {
                        type: 'pie',
                        data: {
                            labels: ["Actif", "Potentiel", "Visiteur"],
                            datasets: [{
                                label: 'Répartition des clients',
                                data: [<% repartitionClients();%>],
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    'rgba(54, 162, 235, 0.2)',
                                    'rgba(255, 159, 64, 0.2)'
                                ],
                                borderColor: [
                                    'rgba(255,99,132,1)',
                                    'rgba(54, 162, 235, 1)',
                                    'rgba(255, 159, 64, 1)'
                                ],
                                borderWidth: 1
                            }]
                        },
                    });
                    var ctx = document.getElementById("listeClients");
                    var myChart = new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: ["3 Mois", "6 Mois", "9 Mois", "12 Mois"],
                            datasets: [{
                                label: 'Répartition des nouveaux Clients',
                                data: [<% repartitionNouveauxClients();%>],
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    'rgba(54, 162, 235, 0.2)',
                                    'rgba(255, 159, 64, 0.2)',
                                    'rgba(75, 192, 192, 0.2)'
                                ],
                                borderColor: [
                                    'rgba(255,99,132,1)',
                                    'rgba(54, 162, 235, 1)',
                                    'rgba(255, 159, 64, 1)',
                                    'rgba(75, 192, 192, 1)'
                                ],
                                borderWidth: 1
                            }]
                        },
                    });

                    var ctx = document.getElementById("connexionClients");
                    var myChart = new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: [<% nomConnexionClients();%>],
                            datasets: [{
                                label: 'Nombre de connexions des clients',
                                data: [<% nombreConnexionClients();%>],
                                borderColor: 'rgba(255,99,132,1)',
                                borderWidth: 1
                            }]
                        },
                    });
                </script>
                <asp:Panel ID="PanelListeDernieresConnexions" runat="server" CssClass="card mt-3">
                    <asp:Panel CssClass="card-header" runat="server">
                        <asp:Label Text="Liste des dernières connexions de clients" runat="server" CssClass="card-title" />
                    </asp:Panel>
                    <asp:Panel CssClass="card-body" runat="server">
                         <asp:DropDownList ID="ddlListeDernieresConnexions" CssClass="mydropdownlist" runat="server" AutoPostBack="true" />
                        <asp:TextBox id="tbDernieresConnexions" PlaceHolder="Entrer le nombre de dernières connexions :" runat="server" AutoPostBack="false"/>
                        <asp:Button runat="server" OnClick="choixDernieresConnexions" Text="OK"/>
                        <asp:Panel runat="server" CssClass="scrollClass">
                            <asp:Table ID="tableDernieresConnexions" CssClass="table" runat="server">
                                 <asp:TableHeaderRow TableSection="TableHeader">
                                    <asp:TableHeaderCell CssClass="fake-button">Index</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Date</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Numéro du client</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Nom du client</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Nombre de connexions</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                
                <br/><br/>
                <asp:Panel ID="PanelCommandes" runat="server" CssClass="card mt-3">
                    <asp:Panel CssClass="card-header" runat="server">
                        <asp:Label Text="Liste du total des commandes d'un client par vendeur" runat="server" CssClass="card-title" />
                    </asp:Panel>
                    <asp:Panel CssClass="card-body" runat="server">
                        <asp:Panel runat="server" CssClass="scrollClass">
                            <asp:Table ID="tableCommandesClientVendeur" CssClass="table" runat="server">
                                <asp:TableHeaderRow TableSection="TableHeader">
                                    <asp:TableHeaderCell CssClass="fake-button">No du client</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Nom du client</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Montant brut</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Taxes et livraison</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Montant total</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Date de dernière commande</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">No du vendeur</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Nom du vendeur</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <br/><br/>
                <asp:Panel ID="PanelNombreClient" runat="server" CssClass="card mt-3">
                    <asp:Panel CssClass="card-header" runat="server">
                        <asp:Label Text="Nombre de clients pour un vendeur" runat="server" CssClass="card-title" />
                    </asp:Panel>
                    <asp:Panel CssClass="card-body" runat="server">
                        <asp:Panel runat="server" CssClass="scrollClass">
                            <asp:Table ID="tableCliensPourVendeur" CssClass="table" runat="server">
                                <asp:TableHeaderRow TableSection="TableHeader">
                                    <asp:TableHeaderCell CssClass="fake-button">No du vendeur</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Nom du vendeur</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Clients actifs</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Clients potentiels</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Clients visiteurs</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="fake-button">Nombre total de clients</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="PanelVisiteClientPourVendeur" runat="server" CssClass="card mt-3">
                    <asp:Panel CssClass="card-header" runat="server">
                        <asp:Label Text="Nombre de visites d'un client pour un vendeur" runat="server" CssClass="card-title" />
                    </asp:Panel>
                    <asp:Panel CssClass="card-body" runat="server">
                         <asp:Panel runat="server" CssClass="scrollClass">
                            <asp:Table ID="tableVisiteClientPourVendeur" CssClass="table table-responsive" runat="server">
                            </asp:Table>
                             </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="pnTousClients" runat="server" CssClass="card mt-3">
                    <asp:Panel CssClass="card-header" runat="server">
                        <asp:Label Text="Clients et vendeurs" runat="server" CssClass="card-title" />
                    </asp:Panel>
                    <asp:Panel CssClass="card-body" runat="server">
                         <asp:Panel runat="server" CssClass="scrollClass" >
                             <h3>Tous les clients</h3>
                            <asp:Table ID="tableTousClients" CssClass="table w-100" runat="server">
                            <asp:TableHeaderRow TableSection="TableHeader">
                                <asp:TableHeaderCell CssClass="fake-button">
                                Nom, Prénom
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="fake-button">
                                    Adresse courriel
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="fake-button">
                                    Date d'Inscription
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="fake-button">
                                    Montant total avant livraison
                                </asp:TableHeaderCell>
                                 <asp:TableHeaderCell CssClass="fake-button">
                                    Envoyer un message
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            </asp:Table>
                             <br />
                             
                        <h3>Tous les vendeurs</h3>
                            <br />
                             <asp:Table ID="tableTousVendeurs" CssClass="table w-100" runat="server">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell TableSection="TableHeader" CssClass="fake-button">
                                Nom, Prénom
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="fake-button">
                                    Nom d'affaires
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="fake-button">
                                    Date d'Inscription
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="fake-button">
                                    Montant total avant livraison
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="fake-button">
                                    Envoyer un message
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                             </asp:Table>
                             </asp:Panel>
                        <asp:Button ID="btnEnvoyerATous" runat="server" Text="Envoyer un message à tous les clients et vendeurs" CssClass="btn btn-block btn-secondary" OnClick="btnEnvoyerATous_Click" />
                    </asp:Panel>
                </asp:Panel>
                
            </asp:Panel>
        </asp:Panel>
    </asp:panel>
</asp:Content>
