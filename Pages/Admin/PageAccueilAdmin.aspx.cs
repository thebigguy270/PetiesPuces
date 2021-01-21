using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    PPClients clients = new PPClients();
    PPVendeurs vendeurs = new PPVendeurs();
    PPVendeursClients vendeursClients = new PPVendeursClients();
    PPCommandes commandes = new PPCommandes();
    PPArticlesEnPanier paniers = new PPArticlesEnPanier();
    PPProduits produits = new PPProduits();
    PPDetailsCommandes detailsCommandes = new PPDetailsCommandes();
    PPCategories categories = new PPCategories();
    protected void Page_Load(object sender, EventArgs e)
    {
        tbDernieresConnexions.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        peuplerDemandeVendeurs();
        int nbTotalVendeurs = vendeurs.Values.Where(x => x.Statut == 1).Count();
        NbTotalVendeurs.Text = "Nombre total de vendeurs: " + nbTotalVendeurs;

        nombreClientParVendeur();
        nombreVisiteClientPourVendeur();
        TotalCommandesClientParVendeur();
        peuplerCategories();
        peuplerRedevancesDues();
        chargerTablesTotal();

        PanelInactiviteClientCatalogue.Visible = false;
        if (!IsPostBack)
        {
            if ((Request.Form["decision"] != null) && (Request.Form["statutvendeur"] != null))
            {
                demandeVendeur(Request.Form["decision"].ToString(), Request.Form["statutvendeur"].ToString());
            }
            
            ddlInactiviteClient.Items.Add(new ListItem("Tous les clients", "0"));
            ddlInactiviteClient.Items.Add(new ListItem("1 mois et plus", "1"));
            ddlInactiviteClient.Items.Add(new ListItem("3 mois et plus", "3"));
            ddlInactiviteClient.Items.Add(new ListItem("6 mois et plus", "6"));
            ddlInactiviteClient.Items.Add(new ListItem("1 an et plus", "12"));
            ddlInactiviteClient.Items.Add(new ListItem("2 ans et plus", "24"));
            ddlInactiviteClient.Items.Add(new ListItem("3 ans et plus", "36"));
            ddlInactiviteClient.Items.Add(new ListItem("Depuis le début", "100"));

            ddlInactiviteVendeur.Items.Add(new ListItem("Tous les vendeurs", "0"));
            ddlInactiviteVendeur.Items.Add(new ListItem("1 mois et plus", "1"));
            ddlInactiviteVendeur.Items.Add(new ListItem("3 mois et plus", "3"));
            ddlInactiviteVendeur.Items.Add(new ListItem("6 mois et plus", "6"));
            ddlInactiviteVendeur.Items.Add(new ListItem("1 an et plus", "12"));
            ddlInactiviteVendeur.Items.Add(new ListItem("2 ans et plus", "24"));
            ddlInactiviteVendeur.Items.Add(new ListItem("3 ans et plus", "36"));
            ddlInactiviteVendeur.Items.Add(new ListItem("Depuis le début", "100"));

            ddlListeDernieresConnexions.Items.Add(new ListItem("10 derniers clients", "10"));
            ddlListeDernieresConnexions.Items.Add(new ListItem("20 derniers clients", "20"));
            ddlListeDernieresConnexions.Items.Add(new ListItem("30 derniers clients", "30"));
            //Gestion accepter/refuser vendeur
            //Client inactif
            peuplerClient(DateTime.Today, false);
            //Vendeur inactif
            peuplerVendeur(DateTime.Today, false);

            listeDernieresConnexions(10);
        }
        else
        {
            if (ddlInactiviteClient.SelectedValue.Equals("0"))
            {
                peuplerClient(DateTime.Today, false);
            }
            else if (ddlInactiviteClient.SelectedValue.Equals("100"))
            {
                peuplerClient(new DateTime(), true);
            }
            else
            {
                DateTime ajd = DateTime.Today.AddMonths(-int.Parse(ddlInactiviteClient.SelectedValue));
                peuplerClient(ajd, false);
            }
            if (ddlInactiviteVendeur.SelectedValue.Equals("0"))
            {
                peuplerVendeur(DateTime.Today, false);
            }
            else if (ddlInactiviteVendeur.SelectedValue.Equals("100"))
            {
                peuplerVendeur(new DateTime(), true);
            }
            else
            {
                DateTime ajd = DateTime.Today.AddMonths(-int.Parse(ddlInactiviteVendeur.SelectedValue));
                peuplerVendeur(ajd, false);
            }
            if (ddlListeDernieresConnexions.SelectedValue.Equals("10"))
            {
                listeDernieresConnexions(10);
            }
            else if (ddlListeDernieresConnexions.SelectedValue.Equals("20"))
            {
                listeDernieresConnexions(20);
            }
            else
            {
                listeDernieresConnexions(30);
            }
        }
    }

    private void nombreVisiteClientPourVendeur()
    {
        TableRow rowHeaderVendeur = new TableRow();

        TableHeaderCell cellHeaderVendeur = new TableHeaderCell();
        cellHeaderVendeur.CssClass = "fake-button";
        cellHeaderVendeur.Text = "Clients/Vendeurs";
        rowHeaderVendeur.Cells.Add(cellHeaderVendeur);

        foreach (Vendeur vendeur in vendeurs.Values) {
            TableHeaderCell cellHeaderVendeurNom = new TableHeaderCell();
            cellHeaderVendeurNom.CssClass = "fake-button";
            cellHeaderVendeurNom.Text = vendeur.NoVendeur.ToString() + " (" + vendeur.Prenom + " " + vendeur.Nom + ")";
            rowHeaderVendeur.Cells.Add(cellHeaderVendeurNom);
        }
        tableVisiteClientPourVendeur.Rows.Add(rowHeaderVendeur);

        foreach (Client client in clients.Values)
        {
            TableRow rowClient = new TableRow();
            TableCell cellClient = new TableCell();
            if (client.Prenom == null && client.Nom == null)
            {
                cellClient.Text = client.NoClient.ToString() + " (" + client.AdresseEmail + ")";
            }
            else
            {
                cellClient.Text = client.NoClient.ToString() + " (" + client.Prenom + " " + client.Nom + ")";
            }
            
            rowClient.Cells.Add(cellClient);
            foreach (Vendeur vendeur in vendeurs.Values) {
                int occurenceClient = 0;
                foreach (VendeurClient vendeurClient in vendeursClients.Values) {
                    if (vendeurClient.NoClient.Equals(client.NoClient) && (vendeurClient.NoVendeur.Equals(vendeur.NoVendeur))) {
                        occurenceClient++;
                    }
                }
                TableCell cellNombreClient = new TableCell();
                cellNombreClient.Text = occurenceClient.ToString();
                rowClient.Cells.Add(cellNombreClient);
            }
            tableVisiteClientPourVendeur.Rows.Add(rowClient);
        }
    }
    protected void peuplerRedevancesDues()
    {
        foreach (Vendeur vendeur in vendeurs.Values)
        {
            Boolean boodejaCommande = false;
            foreach(Commande commande in commandes.Values)
            {
                if (commande.NoVendeur.Equals(vendeur.NoVendeur))
                {
                    boodejaCommande = true;
                    break;
                }
            }
            TableRow rowVendeur = new TableRow();

            rowVendeur.TdDyn(vendeur.NoVendeur.ToString());
            rowVendeur.TdDyn(vendeur.NomAffaires + " (" + vendeur.Prenom + " " + vendeur.Nom + ")");
            TableCell tbcell = rowVendeur.TdDyn();
            TextBox tbRedevance = new TextBox();
            tbRedevance.ID = "RedevancesDues" + vendeur.NoVendeur.ToString();
            tbRedevance.Attributes.Add("type", "number");
            tbRedevance.Attributes.Add("min", "1");
            tbRedevance.Attributes.Add("max", "99");
            tbRedevance.CssClass = "form-control";
            tbRedevance.Attributes.Add("value", vendeur.Pourcentage != null ? vendeur.Pourcentage.ToString() : "");
            if (boodejaCommande)
            {
                tbRedevance.Enabled = false;
            }
            tbcell.Controls.Add(tbRedevance);
            GestionRedevance.Controls.Add(rowVendeur);
        }
    }
    protected void miseAJourRedevances(object sender, EventArgs e)
    {
        foreach (TableRow ctrl2 in GestionRedevance.Controls)
        {
            foreach (TableCell ctrl1 in ctrl2.Controls)
            {
                foreach (var ctrl in ctrl1.Controls)
                {
                    if ((ctrl is TextBox) && (((TextBox)ctrl).Enabled == true))
                    {
                        foreach(Vendeur vendeur in vendeurs.Values)
                        {
                            if (vendeur.NoVendeur.ToString().Equals(((TextBox)ctrl).ID))
                            {
                                vendeur.Pourcentage = decimal.Parse(((TextBox)ctrl).Text.Trim());
                                vendeurs.NotifyUpdated(vendeur);
                                vendeurs.Update();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    protected void peuplerDemandeVendeurs()
    {
        foreach (Vendeur vendeur in vendeurs.Values)
        {
            if (vendeur.Statut == 0)
            {
                Panel panelCard = GestionDemandesVendeurs.DivDyn("demandeVendeur" + vendeur.NoVendeur, "card mt-3");
                Panel panelTitreCategories = panelCard.DivDyn("demandeVendeurTitre" + vendeur.NoVendeur, "card-header fake-button centertitre");
                panelTitreCategories.LblDyn("titre" + vendeur.NoVendeur, vendeur.NomAffaires + " (" + vendeur.Prenom + " " + vendeur.Nom + ")", "card-title h5 text-primary");

                panelTitreCategories.Attributes.Add("data-toggle", "collapse");
                panelTitreCategories.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapseDV" + vendeur.NoVendeur);
                panelTitreCategories.Attributes.Add("aria-expanded", "false");
                panelTitreCategories.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapse" + vendeur.NoVendeur);

                Panel collapsable = panelCard.DivDyn("collapseDV" + vendeur.NoVendeur, "collapse collapse");
                Panel panelBodyCat = collapsable.DivDyn("bodyVendeurDV" + vendeur.NoVendeur, "card-body");

                Panel row = panelBodyCat.DivDyn("bodyVendeurDV2"+vendeur.NoVendeur, "row ml-3 mb-3");

                Table tb = row.TableDyn("tableDV"+vendeur.NoVendeur, "table");
                TableRow tbRowNom = tb.TrDyn();
                tbRowNom.TdDyn("Nom et nom de l'entreprise :");
                tbRowNom.TdDyn(vendeur.NomAffaires + " (" + vendeur.Prenom + " " + vendeur.Nom + ")");

                TableRow tbRowAdresseCourriel = tb.TrDyn();
                tbRowAdresseCourriel.TdDyn("Adresse courriel :");
                tbRowAdresseCourriel.TdDyn(vendeur.AdresseEmail);

                TableRow tbRowLocation = tb.TrDyn();
                tbRowLocation.TdDyn("Adresse complète : ");
                tbRowLocation.TdDyn(vendeur.Rue + " " + vendeur.Ville + " " + vendeur.Pays + "(" + vendeur.CodePostal + ")");

                TableRow tbRowTelephone1 = tb.TrDyn();
                tbRowTelephone1.TdDyn("Numéro de téléphone 1 : ");
                tbRowTelephone1.TdDyn(vendeur.Tel1);

                TableRow tbRowTelephone2 = tb.TrDyn();
                tbRowTelephone2.TdDyn("Numéro de téléphone 2 : ");
                tbRowTelephone2.TdDyn(vendeur.Tel2 != null ? vendeur.Tel2 : "Non Disponible.");

                TableRow tbRowLivraison1 = tb.TrDyn();
                tbRowLivraison1.TdDyn("Poid maximum pour la livraison :");
                tbRowLivraison1.TdDyn(vendeur.PoidsMaxLivraison.ToString() + " lb.");

                TableRow tbRowLivraison2 = tb.TrDyn();
                tbRowLivraison2.TdDyn("Montant minimum pour la livraison gratuite :");
                tbRowLivraison2.TdDyn(string.Format("{0:C}", vendeur.LivraisonGratuite));

                TableRow tbRowTaxes = tb.TrDyn();
                tbRowTaxes.TdDyn("Taxes incluses? :");
                tbRowTaxes.TdDyn(vendeur.Taxes.HasValue ? "Oui" : "Non");

                TableRow tbRowDateCreation = tb.TrDyn();
                tbRowDateCreation.TdDyn("Date de création de compte :");
                tbRowDateCreation.TdDyn(vendeur.DateCreation.ToString());

                TableRow tbRowPourcentage = tb.TrDyn("tbRowRedevance"+vendeur.NoVendeur);
                tbRowPourcentage.TdDyn("Redevance (en %) :");
                TextBox tbRedevance = new TextBox();
                tbRedevance.ID = "tboxRedevance" + vendeur.NoVendeur;
                tbRedevance.Attributes.Add("type", "number");
                tbRedevance.Attributes.Add("min", "1");
                tbRedevance.Attributes.Add("max", "99");
                tbRedevance.Attributes.Add("value", "");
                TableCell tbCellRedevance = new TableCell();
                tbCellRedevance.ID = "tbRedevance" + vendeur.NoVendeur.ToString();
                tbCellRedevance.Controls.Add(tbRedevance);
                tbRowPourcentage.Controls.Add(tbCellRedevance);
                tbRowPourcentage.TdDyn(" %");



                TableRow tbRowChoix = tb.TrDyn();
                TableCell tbCellAccept = tbRowChoix.TdDyn();
                TableCell tbCellRefuse = tbRowChoix.TdDyn();

                tbCellAccept.BtnDyn("btnAccepter" + vendeur.NoVendeur, "Accepter", onclickMail, "btn btn-secondary btn-block");
                tbCellRefuse.BtnDyn("btnRefuser" + vendeur.NoVendeur, "Refuser", onclickMail, "btn btn-dark btn-block");
            }

        }
    }
    protected void peuplerVendeur(DateTime dateAvant, Boolean depuisLeDebut) {
        foreach (Vendeur vendeur in vendeurs.Values.OrderByDescending(v=> DatePlusRecenteVendeur(v)))
        {
            if (vendeur.Statut == 1)
            {
                DateTime datePlusRecente = DatePlusRecenteVendeur(vendeur);
                TableRow rowInactifVendeur = new TableRow();
                TableCell cellNoVendeurInactif = new TableCell();
                TableCell cellNomVendeurInactif = new TableCell();
                TableCell cellNbMoisInactif = new TableCell();
                TableCell cellSupprimerVendeurInactif = new TableCell();

                cellNoVendeurInactif.Text = vendeur.NoVendeur.ToString();
                rowInactifVendeur.Cells.Add(cellNoVendeurInactif);
                if (vendeur.Prenom == null && vendeur.Nom == null) {
                    cellNomVendeurInactif.Text = vendeur.NomAffaires + " (" + vendeur.AdresseEmail + ")";
                }
                else {
                    cellNomVendeurInactif.Text = vendeur.NomAffaires + " (" + vendeur.Prenom + " " + vendeur.Nom + ")";
                }


                rowInactifVendeur.Cells.Add(cellNomVendeurInactif);
                if (datePlusRecente < dateAvant)
                {
                    if (datePlusRecente == new DateTime())
                    {
                        cellNbMoisInactif.Text = "Inactif depuis le début";
                    }
                    else
                    {
                        if ((DateTime.Today - datePlusRecente).TotalDays < 1)
                        {
                            cellNbMoisInactif.Text = "A été actif aujourd'hui";
                        }
                        else
                        {
                            cellNbMoisInactif.Text = "Inactif depuis " + (DateTime.Today - datePlusRecente).TotalDays + " jours";
                        }     
                    }
                    rowInactifVendeur.Cells.Add(cellNbMoisInactif);

                    CheckBox cbSupprimerVendeurInactif = new CheckBox();
                    cbSupprimerVendeurInactif.ID = "vendeur" + vendeur.NoVendeur.ToString();
                    cellSupprimerVendeurInactif.Controls.Add(cbSupprimerVendeurInactif);
                    rowInactifVendeur.Cells.Add(cellSupprimerVendeurInactif);
                    GestionInactiviteVendeur.Rows.Add(rowInactifVendeur);
                }
            }
        }
    }

    protected void peuplerClient(DateTime dateAvant, Boolean depuisLeDebut)
    {
        foreach (Client client in clients.Values.OrderByDescending(c => DatePlusRecenteClient(c)))
        {
            if (client.Statut == 1)
            {
                DateTime datePlusRecente = DatePlusRecenteClient(client);
                TableRow rowInactifClient = new TableRow();
                TableCell cellNoClientInactif = new TableCell();
                TableCell cellNomClientInactif = new TableCell();
                TableCell cellNbMoisInactif = new TableCell();
                TableCell cellSupprimerClientInactif = new TableCell();

                cellNoClientInactif.Text = client.NoClient.ToString();
                rowInactifClient.Controls.Add(cellNoClientInactif);
                if (client.Prenom == null && client.Nom == null)
                {
                    cellNomClientInactif.Text = client.AdresseEmail;
                }
                else
                {
                    cellNomClientInactif.Text = client.Prenom + " " + client.Nom;
                }
                //Il y a eu au moins 1 panier ou commande pour le client
                if (datePlusRecente < dateAvant)
                {

                    if (datePlusRecente == new DateTime())
                    {
                        cellNbMoisInactif.Text = "Inactif depuis le début";
                    }
                    else
                    {
                        cellNbMoisInactif.Text = "Inactif depuis: " + (DateTime.Today - datePlusRecente).TotalDays + " jours";
                    }
                    rowInactifClient.Cells.Add(cellNomClientInactif);
                    rowInactifClient.Cells.Add(cellNbMoisInactif);

                    CheckBox cbSupprimerLeClient = new CheckBox();
                    cellSupprimerClientInactif.Controls.Add(cbSupprimerLeClient);
                    cbSupprimerLeClient.ID = "clientNO" + client.NoClient;
                    rowInactifClient.Cells.Add(cellSupprimerClientInactif);

                    GestionInactiviteClient.Rows.Add(rowInactifClient);
                }
                else if (depuisLeDebut && datePlusRecente.Equals(new DateTime()))
                {
                    cellNbMoisInactif.Text = "Inactif depuis le début";

                    rowInactifClient.Cells.Add(cellNoClientInactif);
                    rowInactifClient.Cells.Add(cellNomClientInactif);
                    rowInactifClient.Cells.Add(cellNbMoisInactif);

                    CheckBox cbSupprimerLeClient = new CheckBox();
                    cellSupprimerClientInactif.Controls.Add(cbSupprimerLeClient);
                    cbSupprimerLeClient.ID = "client" + client.NoClient;
                    rowInactifClient.Cells.Add(cellSupprimerClientInactif);

                    GestionInactiviteClient.Rows.Add(rowInactifClient);
                }
            }
        }
    }
    protected void actionDemandeVendeur(object sender, EventArgs e)
    {
        foreach(Vendeur vendeur in vendeurs.Values)
        {
            if (vendeur.AdresseEmail.Equals(Request.Form["ctl00$ctl00$Contenu$ContenuPrincipal$modalCourrielVendeur"]))
            {
                if (Request.Form["ctl00$ctl00$Contenu$ContenuPrincipal$modalObjet"].Contains("accepté"))
                {
                    vendeur.Pourcentage = int.Parse(Request.Form["ctl00$ctl00$Contenu$ContenuPrincipal$modalTbRedevance"]);
                    vendeur.Statut = 1;
                    vendeurs.NotifyUpdated(vendeur);
                    vendeurs.Update();
            }
                else
                {
                    vendeurs.Remove(vendeur);
                    break;
                }
            }
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Succès", "alert('Le courriel a été envoyé avec succès.');window.location.href = window.location.href;", true);
    }
        protected void onclickMail(object sender, EventArgs e)
    {
        Button btnClick = (Button)sender;
        string noDuVendeur = btnClick.ID;
        Boolean accepte = false;
        if (noDuVendeur.Contains("btnAccepter"))
        {
            accepte = true;
        }
        noDuVendeur = noDuVendeur.Replace("btnAccepter", "").Replace("btnRefuser", "");


        Control con = GestionDemandesVendeurs.FindControl("demandeVendeur"+noDuVendeur);
        Control con2 = con.FindControl("demandeVendeurcollapse" + noDuVendeur);
        Control con3 = con.FindControl("bodyVendeurDV" + noDuVendeur);
        Control con4 = con.FindControl("bodyVendeurDV2" + noDuVendeur);
        Control con5 = con.FindControl("tableDV" + noDuVendeur);
        Control con6 = con5.FindControl("tbRowRedevance" + noDuVendeur);
        Control con7 = con5.FindControl("tbRedevance" + noDuVendeur);
        TextBox tbRedevance= (TextBox)con7.FindControl("tboxRedevance" + noDuVendeur);
        //if (!tbRedevance.Text.Trim().Equals("") && (int.Parse(tbRedevance.Text.Trim()) > 0) && (int.Parse(tbRedevance.Text.Trim()) < 100))
        if (!tbRedevance.Text.Trim().Equals(""))
        {
            Vendeur vendeur = new Vendeur(null);
            foreach (Vendeur vend in vendeurs.Values)
            {
                if (vend.NoVendeur.ToString().Equals(noDuVendeur))
                {
                    vendeur = vend;
                    break;
                }
            }
            modalTbRedevance.Value = tbRedevance.Text;

            modalCourrielVendeur.Text = vendeur.AdresseEmail;
            modalNomVendeur.Text = vendeur.Prenom + " " + vendeur.Nom;

            modalObjet.Text = "Statut de vendeur: " + (accepte ? "accepté" : "refusé");
            modalContenuEmail.Text = (accepte ? "Vous êtes accepté. Le taux de redevance qui vous est attribué est de " + tbRedevance.Text + " %." : "Désolé, votre demande de vendeur est refusée.");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ModalView", "<script>$('#modalDemandeVendeur').modal('show');</script>", false);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Erreur!", "alert('Il faut sélectionner un taux de redevance.');", true);
        }
        /*
        Response.Clear();
        var sb = new System.Text.StringBuilder();
        sb.Append("<html>");
        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
        sb.AppendFormat("<form action='{0}' method='post'>", url);
        sb.AppendFormat("<input type='hidden' name='courriel' value='{0}'>", courriel);
        sb.AppendFormat("<input type='hidden' name='statutvendeur' value='{0}'>", accepte ? "Acceptée" : "Refusée");
        sb.AppendFormat("<input type='hidden' name='sujet' value='Demande de vendeur: {0}'>", accepte ? "Acceptée" : "Refusée");
        sb.AppendFormat("<input type='hidden' name='contenu' value='{0}'>", accepte ? "Félicitations, votre demande de vendeur a été acceptée." : "Désolé, votre demande de vendeur a été refusée.");
        sb.Append("</form>");
        sb.Append("</body>");
        sb.Append("</html>");
        Response.Write(sb.ToString());
        Response.End();
        */
    }
    protected DateTime DatePlusRecenteVendeur(Vendeur vendeur)
    {
        DateTime datePlusRecente = new DateTime();
        PPCommandes commandes = new PPCommandes();
        PPProduits produits = new PPProduits();
        foreach (Commande commande in commandes.Values)
        {
            if (commande.NoVendeur.Equals(vendeur.NoVendeur))
            {
                int result = DateTime.Compare(datePlusRecente, commande.DateCommande ?? new DateTime());
                if (result < 0)
                {
                    datePlusRecente = commande.DateCommande ?? new DateTime();
                }
            }
        }
        foreach (Produit produit in produits.Values)
        {
            if (produit.NoVendeur.Equals(vendeur.NoVendeur))
            {
                int result = DateTime.Compare(datePlusRecente, produit.DateCreation ?? new DateTime());
                if (result < 0)
                {
                    datePlusRecente = produit.DateCreation ?? new DateTime();
                }
            }
        }
        return datePlusRecente;
    }
    protected DateTime DatePlusRecenteClient(Client client) {
        DateTime datePlusRecente = new DateTime();

        foreach (Commande commande in commandes.Values)
        {
            if (commande.NoClient.Equals(client.NoClient))
            {
                int result = DateTime.Compare(datePlusRecente, commande.DateCommande ?? new DateTime());
                if (result < 0)
                {
                    datePlusRecente = commande.DateCommande ?? new DateTime();
                }
            }
        }

        foreach (ArticleEnPanier panier in paniers.Values)
        {
            if (panier.NoClient.Equals(client.NoClient))
            {
                int result = DateTime.Compare(datePlusRecente, panier.DateCreation ?? new DateTime());
                if (result < 0)
                {
                    datePlusRecente = panier.DateCreation ?? new DateTime();
                }
            }
        }
        return datePlusRecente;
    }
    protected void repartitionDemandesVendeurs()
    {
        PPVendeurs vendeurs = new PPVendeurs();
        List<Vendeur> vendeursMois1 = new List<Vendeur>();
        List<Vendeur> vendeursMois3 = new List<Vendeur>();
        List<Vendeur> vendeursMois6 = new List<Vendeur>();
        List<Vendeur> vendeursMois12 = new List<Vendeur>();
        List<Vendeur> vendeursDepuisDebut = new List<Vendeur>();
        foreach (Vendeur vendeur in vendeurs.Values)
        {
            if (vendeur.Statut == 1)
            {
                if (vendeur.DateCreation > DateTime.Now.AddMonths(-1))
                {
                    vendeursMois1.Add(vendeur);
                }
                else if (vendeur.DateCreation > DateTime.Now.AddMonths(-3))
                {
                    vendeursMois3.Add(vendeur);
                }
                else if (vendeur.DateCreation > DateTime.Now.AddMonths(-6))
                {
                    vendeursMois6.Add(vendeur);
                }
                else if (vendeur.DateCreation > DateTime.Now.AddMonths(-12))
                {
                    vendeursMois12.Add(vendeur);
                }
                else if (vendeur.DateCreation.Equals(new DateTime()))
                {
                    vendeursDepuisDebut.Add(vendeur);
                }
            }
        }
        Response.Write(vendeursMois1.Count() + "," + vendeursMois3.Count() + "," + vendeursMois6.Count() + "," + vendeursMois12.Count() + "," + vendeursDepuisDebut.Count());
    }
    protected void repartitionClients()
    {
        int nbClientActif = 0;
        int nbClientPotentiel = 0;
        int nbClientVisiteur = 0;

        foreach (Client client in clients.Values) {
            Boolean booCommande = clientAUneCommande(client);
            Boolean booArticlePanier = clientAUnPanier(client);

            if (booCommande)
            {
                nbClientActif++;
            }
            else if (booArticlePanier)
            {
                nbClientPotentiel++;
            }
            else {
                nbClientVisiteur++;
            }
        }
        Response.Write(nbClientActif + "," + nbClientPotentiel + "," + nbClientVisiteur);
    }
    protected bool clientAUneCommande(Client client)
    {
        Boolean booCommande = false;
        foreach (Commande commande in commandes.Values)
        {
            if (commande.NoClient.Equals(client.NoClient))
            {
                booCommande = true;
            }
        }
        return booCommande;
    }
    protected bool clientAUnPanier(Client client)
    {
        Boolean booArticlePanier = false;
        PPArticlesEnPanier paniers = new PPArticlesEnPanier();
        foreach (ArticleEnPanier panier in paniers.Values)
        {
            if (panier.NoClient.Equals(client.NoClient))
            {
                booArticlePanier = true;
            }
        }
        return booArticlePanier;
    }
    protected void repartitionNouveauxClients() {
        PPClients clients = new PPClients();
        List<Client> clientMois3 = new List<Client>();
        List<Client> clientMois6 = new List<Client>();
        List<Client> clientMois9 = new List<Client>();
        List<Client> clientMois12 = new List<Client>();
        foreach (Client client in clients.Values)
        {
            if (client.Statut == 1)
            {
                if (client.DateCreation > DateTime.Now.AddMonths(-3))
                {
                    clientMois3.Add(client);
                }
                else if (client.DateCreation > DateTime.Now.AddMonths(-6))
                {
                    clientMois6.Add(client);
                }
                else if (client.DateCreation > DateTime.Now.AddMonths(-9))
                {
                    clientMois9.Add(client);
                }
                else if (client.DateCreation > DateTime.Now.AddMonths(-12))
                {
                    clientMois12.Add(client);
                }
            }
        }
        Response.Write(clientMois3.Count() + "," + clientMois6.Count() + "," + clientMois9.Count() + "," + clientMois12.Count());
    }
    protected void nombreConnexionClients()
    {
        foreach (Client client in clients.Values)
        {
            Response.Write(client.NbConnexions + ",");
        }
    }
    protected void nomConnexionClients()
    {
        foreach (Client client in clients.Values)
        {
            Response.Write("\"" + client.NoClient + "\",");
        }
    } protected void listeDernieresConnexions(int nombreDerniereConnexions) {

        List<Client> nouvelleListe = clients.Values.OrderByDescending(c => c.DateDerniereConnexion).ToList();
        int index = 1;
        foreach (Client client in nouvelleListe.Take(nombreDerniereConnexions))
        {
            TableRow rowClient = new TableRow();
            TableCell cellIndex = new TableCell();
            TableCell cellDate = new TableCell();
            TableCell cellNoClient = new TableCell();
            TableCell cellNomClient = new TableCell();
            TableCell cellNbConnexions = new TableCell();
            cellIndex.Text = index.ToString();
            cellDate.Text = client.DateDerniereConnexion.ToString();
            cellNoClient.Text = client.NoClient.ToString();
            if (client.Nom == null && client.Nom == null)
            {
                cellNomClient.Text = client.AdresseEmail;
            }
            else {
                cellNomClient.Text = client.Prenom + " " + client.Nom;
            }

            cellNbConnexions.Text = client.NbConnexions.ToString();
            rowClient.Controls.Add(cellIndex);
            rowClient.Controls.Add(cellDate);
            rowClient.Controls.Add(cellNoClient);
            rowClient.Controls.Add(cellNomClient);
            rowClient.Controls.Add(cellNbConnexions);
            tableDernieresConnexions.Controls.Add(rowClient);
            index++;
        }
    }
    protected void TotalCommandesClientParVendeur()
    {
        foreach (Client client in clients.Values)
        {
            foreach (Vendeur vendeur in vendeurs.Values)
            {
                decimal intMontantBrut = 0;
                decimal intTaxesLivraisont = 0;
                decimal intMontantTotal = 0;
                DateTime derniereCommande = new DateTime();
                Boolean booAuMoinsUneCommande = false;
                foreach (Commande commande in commandes.Values)
                {
                    if (commande.NoVendeur.Equals(vendeur.NoVendeur) && (client.NoClient.Equals(commande.NoClient)))
                    {
                        booAuMoinsUneCommande = true;
                        intMontantBrut += decimal.Parse(commande.MontantTotAvantTaxes.ToString());
                        intTaxesLivraisont += decimal.Parse(commande.TPS.ToString()) + decimal.Parse(commande.TVQ.ToString()) + decimal.Parse(commande.CoutLivraison.ToString());
                        intMontantTotal += decimal.Parse(commande.MontantTotAvantTaxes.ToString()) + decimal.Parse(commande.TPS.ToString()) + decimal.Parse(commande.TVQ.ToString()) + decimal.Parse(commande.CoutLivraison.ToString());
                        if (commande.DateCommande > derniereCommande)
                        {
                            derniereCommande = Convert.ToDateTime(commande.DateCommande);
                        }
                    }
                }
                if (booAuMoinsUneCommande)
                {
                    TableRow rowClient = new TableRow();
                    TableCell cellNoClient = new TableCell();
                    TableCell cellNomClient = new TableCell();
                    TableCell cellBrut = new TableCell();
                    TableCell cellTaxesLivraison = new TableCell();
                    TableCell cellTotal = new TableCell();
                    TableCell cellDateDerniereCommande = new TableCell();
                    TableCell cellNoVendeur = new TableCell();
                    TableCell cellNomVendeur = new TableCell();
                    cellNoClient.Text = client.NoClient.ToString();
                    rowClient.Cells.Add(cellNoClient);
                    if (client.Prenom == null && client.Nom == null)
                    {
                        cellNomClient.Text = "-";
                    }
                    else
                    {
                        cellNomClient.Text = client.Prenom + " " + client.Nom;
                    }
                    rowClient.Cells.Add(cellNomClient);

                    cellBrut.Text = intMontantBrut.ToString("C");
                    rowClient.Cells.Add(cellBrut);

                    cellTaxesLivraison.Text = intTaxesLivraisont.ToString("C");
                    rowClient.Cells.Add(cellTaxesLivraison);

                    cellTotal.Text = intMontantTotal.ToString("C");
                    rowClient.Cells.Add(cellTotal);

                    cellDateDerniereCommande.Text = derniereCommande.ToString();
                    rowClient.Cells.Add(cellDateDerniereCommande);

                    cellNoVendeur.Text = vendeur.NoVendeur.ToString();
                    rowClient.Cells.Add(cellNoVendeur);

                    if (vendeur.Prenom == null && vendeur.Nom == null)
                    {
                        cellNomVendeur.Text = "-";
                    }
                    else
                    {
                        cellNomVendeur.Text = vendeur.Prenom + " " + vendeur.Nom;
                    }
                    rowClient.Cells.Add(cellNomVendeur);

                    tableCommandesClientVendeur.Rows.Add(rowClient);
                }
            }
        }
    }
    protected void nombreClientParVendeur() {
        foreach (Vendeur vendeur in vendeurs.Values)
        {
            int nbClientActif = 0;
            int nbClientPotentiel = 0;
            int nbClientVisiteur = 0;

            foreach (Client client in clients.Values)
            {
                Boolean clientTrouve = false;
                foreach (VendeurClient vendeurClient in vendeursClients.Values)
                {
                    if (client.NoClient.Equals(vendeurClient.NoClient) && (vendeur.NoVendeur.Equals(vendeurClient.NoVendeur))) {
                        clientTrouve = true;
                    }
                }
                if (clientTrouve)
                {
                    Boolean booCommande = false;
                    Boolean booArticlePanier = false;
                    foreach (Commande commande in commandes.Values)
                    {
                        if (commande.NoClient.Equals(client.NoClient) && (commande.NoVendeur.Equals(vendeur.NoVendeur)))
                        {
                            booCommande = true;
                        }
                    }
                    PPArticlesEnPanier paniers = new PPArticlesEnPanier();
                    foreach (ArticleEnPanier panier in paniers.Values)
                    {
                        if (panier.NoClient.Equals(client.NoClient) && (panier.NoVendeur.Equals(vendeur.NoVendeur)))
                        {
                            booArticlePanier = true;
                        }
                    }

                    if (booCommande)
                    {
                        nbClientActif++;
                    }
                    else if (booArticlePanier)
                    {
                        nbClientPotentiel++;
                    }
                    else
                    {
                        nbClientVisiteur++;
                    }
                }
            }
            TableRow rowVendeur = new TableRow();
            TableCell cellNoVendeur = new TableCell();
            TableCell cellNomVendeur = new TableCell();
            TableCell cellActif = new TableCell();
            TableCell cellPotentiel = new TableCell();
            TableCell cellVisiteur = new TableCell();
            TableCell cellTotal = new TableCell();

            cellNoVendeur.Text = vendeur.NoVendeur.ToString();
            rowVendeur.Cells.Add(cellNoVendeur);
            if (vendeur.Prenom == null && vendeur.Nom == null)
            {
                cellNomVendeur.Text = vendeur.AdresseEmail;
            }
            else
            {
                cellNomVendeur.Text = vendeur.Prenom + " " + vendeur.Nom;
            }
            rowVendeur.Cells.Add(cellNomVendeur);

            cellActif.Text = nbClientActif.ToString();
            rowVendeur.Cells.Add(cellActif);

            cellPotentiel.Text = nbClientPotentiel.ToString();
            rowVendeur.Cells.Add(cellPotentiel);

            cellVisiteur.Text = nbClientVisiteur.ToString();
            rowVendeur.Cells.Add(cellVisiteur);

            cellTotal.Text = (nbClientActif + nbClientPotentiel + nbClientVisiteur).ToString();
            rowVendeur.Cells.Add(cellTotal);

            tableCliensPourVendeur.Rows.Add(rowVendeur);
        }
    }
    protected void choixDernieresConnexions(object sender, EventArgs e) {

        tableDernieresConnexions.Rows.Clear();
        TableHeaderRow tableHeaderConnexion = new TableHeaderRow();
        tableHeaderConnexion.CssClass = "fake-button";
        TableHeaderCell tableCellIndex = new TableHeaderCell();
        tableCellIndex.CssClass = "fake-button";
        TableHeaderCell tableCellDate = new TableHeaderCell();
        tableCellDate.CssClass = "fake-button";
        TableHeaderCell tableCellNoClient = new TableHeaderCell();
        tableCellNoClient.CssClass = "fake-button";
        TableHeaderCell tableCellNomClient = new TableHeaderCell();
        tableCellNomClient.CssClass = "fake-button";
        TableHeaderCell tableCellNbConnexions = new TableHeaderCell();
        tableCellNbConnexions.CssClass = "fake-button";
        tableCellIndex.Text = "Index";
        tableCellDate.Text = "Date";
        tableCellNoClient.Text = "Numéro du client";
        tableCellNomClient.Text = "Nom du client";
        tableCellNbConnexions.Text = "Nombre de connexions";
        tableHeaderConnexion.Controls.Add(tableCellIndex);
        tableHeaderConnexion.Controls.Add(tableCellDate);
        tableHeaderConnexion.Controls.Add(tableCellNoClient);
        tableHeaderConnexion.Controls.Add(tableCellNomClient);
        tableHeaderConnexion.Controls.Add(tableCellNbConnexions);
        tableDernieresConnexions.Controls.Add(tableHeaderConnexion);
        int intNbRows = 0;
        int.TryParse(tbDernieresConnexions.Text, out intNbRows);
        listeDernieresConnexions(intNbRows);
    }

    protected void SupprimerLesClients(object sender, EventArgs e)
    {
        List<Client> lstSupprimerClients = new List<Client>();
        List<Client> lstMiseAJourClients = new List<Client>();
        //foreach (TableRow ctrl2 in GestionInactiviteClient.Controls)
        foreach (TableRow row in GestionInactiviteClient.Rows)
        {
            foreach (TableCell cell in row.Cells) {
                foreach (var ctrl in cell.Controls)
                {
                    if ((ctrl is CheckBox) && (((CheckBox)ctrl).Checked == true))
                    {
                        Client client = clients.Values.Find(x => x.NoClient.ToString() == ((CheckBox)ctrl).ID.ToString().Replace("clientNO", ""));
                        if (client != null)
                        {
                            supprimerPanier(client);
                            bool booCommande = clientAUneCommande(client);
                            bool booVisite = false;
                            foreach (VendeurClient vendeurclient in vendeursClients.Values)
                            {
                                if (vendeurclient.NoClient.Equals(client.NoClient))
                                {
                                    booVisite = true;
                                }
                            }
                            if (booVisite || booCommande)
                            {
                                lstMiseAJourClients.Add(client);
                            }
                            else
                            {
                                lstSupprimerClients.Add(client);
                            }
                        }
                    }
                }
            }
        }
        foreach (Client client in lstSupprimerClients)
        {
            clients.Remove(client);
            ajouterClientTableCatalogue(client);
        }
        foreach (Client client in lstMiseAJourClients)
        {
            client.Statut = (short?)ClientStatut.Disabled;
            clients.NotifyUpdated(client);
            ajouterClientTableCatalogue(client);
        }
        clients.Update();
        PanelInactiviteClientCatalogue.Visible = true;

        GestionInactiviteClient.Controls.Clear();
        TableHeaderRow tbHeaderRow = GestionInactiviteClient.ThrDyn();
        tbHeaderRow.CssClass = "fake-button";
        tbHeaderRow.ThdDyn("No du client");
        tbHeaderRow.ThdDyn("Nom du client");
        tbHeaderRow.ThdDyn("Nombre de mois inactif");
        TableHeaderCell tbCheck = tbHeaderRow.ThdDyn("Sélection du ou des comptes");
        tbCheck.CbDyn("", true);
        if (ddlInactiviteClient.SelectedValue.Equals("0"))
        {
            peuplerClient(DateTime.Today.AddMonths(-1), false);
        }
        else if (ddlInactiviteClient.SelectedValue.Equals("100"))
        {
            peuplerClient(new DateTime(), true);
        }
        else
        {
            DateTime ajd = DateTime.Today.AddMonths(-int.Parse(ddlInactiviteClient.SelectedValue));
            peuplerClient(ajd, false);
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Succès", "alert('Les comptes des clients " + string.Join(", ", lstMiseAJourClients.Distinct().Select(v => v.Prenom + " " + v.Nom)) + "ont été supprimés ou désactivés avec succès.')", true);
    }
    protected void ajouterClientTableCatalogue(Client client)
    {
        TableRow tbRow = tableClientCatalogue.TrDyn();
        tbRow.TdDyn(client.NoClient.ToString());
        if ((client.Nom==null||client.Nom.Equals("")) && (client.Prenom==null||client.Prenom.Equals("")))
        {
            tbRow.TdDyn(client.AdresseEmail);
        }
        else
        {
            tbRow.TdDyn(client.Prenom+" "+client.Nom);
        }
        
        int nbVisites = 0;
        foreach(VendeurClient vc in vendeursClients.Values)
        {
            if (vc.NoClient==client.NoClient)
            {
                nbVisites++;
            }
        }
        tbRow.TdDyn(nbVisites.ToString());
    }
    protected void supprimerPanier(Client client)
    {
        List<ArticleEnPanier> lstSupprimerArticles = new List<ArticleEnPanier>();
        foreach (ArticleEnPanier article in paniers.Values)
        {
            if (article.NoClient.Equals(client.NoClient))
            {
                lstSupprimerArticles.Add(article);
            }
        }
        foreach (ArticleEnPanier article in lstSupprimerArticles)
        {
            paniers.Remove(article);
        }
    }
    protected void SupprimerLesVendeurs(object sender, EventArgs e)
    {
        List<Vendeur> lstMiseAJourVendeurs = new List<Vendeur>();
        List<Produit> lstSupprimerProduits = new List<Produit>();
        List<Produit> lstMiseAJourProduits = new List<Produit>();
        List<ArticleEnPanier> lstSupprimerArticleEnPanier = new List<ArticleEnPanier>();
        foreach (TableRow ctrl2 in GestionInactiviteVendeur.Controls)
        {
            foreach (TableCell ctrl1 in ctrl2.Controls)
            {
                foreach (var ctrl in ctrl1.Controls)
                {
                    if ((ctrl is CheckBox) && (((CheckBox)ctrl).Checked == true))
                    {
                        foreach (Vendeur vendeur in vendeurs.Values)
                        {
                            if (vendeur.NoVendeur.ToString().Equals(((CheckBox)ctrl).ID.ToString().Replace("vendeur", "")))
                            {
                                foreach (Produit produit in produits.Values)
                                {
                                    if (produit.NoVendeur.Equals(vendeur.NoVendeur))
                                    {
                                        Boolean booProduitVendu = false;
                                        foreach (Commande commande in commandes.Values)
                                        {
                                            if (commande.NoVendeur.Equals(vendeur.NoVendeur))
                                            {
                                                foreach (DetailsCommandes detailCommandes in detailsCommandes.Values)
                                                {
                                                    if (detailCommandes.NoCommande.Equals(commande.NoCommande))
                                                    {
                                                        if (detailCommandes.NoProduit.Equals(produit.NoProduit))
                                                        {
                                                            booProduitVendu = true;
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        if (booProduitVendu)
                                        {
                                            lstMiseAJourProduits.Add(produit);
                                        }
                                        else
                                        {
                                            lstSupprimerProduits.Add(produit);
                                        }
                                    }
                                }
                                foreach (ArticleEnPanier panier in paniers.Values)
                                {
                                    if (panier.NoVendeur.Equals(vendeur.NoVendeur))
                                    {
                                        lstSupprimerArticleEnPanier.Add(panier);

                                    }
                                }
                                lstMiseAJourVendeurs.Add(vendeur);
                            }
                        }
                    }
                }
            }
        }
        foreach (ArticleEnPanier panier in lstSupprimerArticleEnPanier.Distinct())
        {
            paniers.Remove(panier);
        }
        foreach (Vendeur vendeur in lstMiseAJourVendeurs)
        {
            vendeur.Statut = (short?)VendeurStatut.Disabled;
            vendeurs.NotifyUpdated(vendeur);
        }
        foreach (Produit produit in lstSupprimerProduits.Distinct())
        {
            produits.Remove(produit);
        }
        foreach (Produit produit in lstMiseAJourProduits.Distinct())
        {
            produit.Disponibilité = false;
            produit.NombreItems = (short?)VendeurStatut.Disabled;
            produits.NotifyUpdated(produit);
        }
        vendeurs.Update();
        produits.Update();
        ClientScript.RegisterStartupScript(this.GetType(), "Succès", "alert('Les comptes des vendeurs "+ string.Join(", ", lstMiseAJourVendeurs.Distinct().Select(v=>v.Prenom+" "+v.Nom))+ "ont été supprimés ou désactivés avec succès.');window.location.href = window.location.href;", true);

    }
    protected void demandeVendeur(string choix, string courrielVendeur) {

        Vendeur vendeurChoisi = new Vendeur(null);
        foreach (Vendeur vendeur in vendeurs.Values)
        {
            if (vendeur.AdresseEmail.Trim().Equals(courrielVendeur.Trim()))
            {
                vendeurChoisi = vendeur;
            }
        }
        if (choix.Equals("Annulé"))
        {
            vendeurs.Remove(vendeurChoisi);
        }
        else
        {
            vendeurChoisi.Statut = 1;
            vendeurs.NotifyUpdated(vendeurChoisi);
            vendeurs.Update();
        }
        Response.Redirect(Request.RawUrl);
    }
    protected void peuplerCategories()
    {
        foreach (Categorie categorie in categories.Values)
        {
            Boolean categorieEstVide = true;
            foreach (Produit produit in produits.Values)
            {
                if (produit.NoCategorie.Equals(categorie.NoCategorie))
                {
                    categorieEstVide = false;
                    break;
                }
            }
            TableRow rowCategorie = new TableRow();

            rowCategorie.TdDyn(categorie.NoCategorie.ToString());
            rowCategorie.TdDyn(categorie.Description.ToString());
            TableCell tbcell = rowCategorie.TdDyn();
            if (categorieEstVide)
            {
                tbcell.BtnDyn(categorie.NoCategorie.ToString(), "Supprimer la catégorie", SupprimerCategorie, "btn btn-dark btn-block");
            }
            else
            {
                tbcell.BtnDyn(categorie.NoCategorie.ToString(), "Supprimer la catégorie", SupprimerCategorie, "btn btn-secondary btn-block",false);
            }
            GestionCategories.Controls.Add(rowCategorie);
            
        }
        Panel panelCard = panelGestionCategories.DivDyn("divNouvelleCategorie", "card mt-3");
        Panel panelTitreCategories = panelCard.DivDyn("panelNouvelleCategorie", "card-header fake-button centertitre");
        panelTitreCategories.LblDyn("titreNouvelleCategorie", "Nouvelle catégorie", "card-title h5 text-primary");

        panelTitreCategories.Attributes.Add("data-toggle", "collapse");
        panelTitreCategories.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapseNouvelleCategorie");
        panelTitreCategories.Attributes.Add("aria-expanded", "false");
        panelTitreCategories.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapseNouvelleCategorie");

        Panel collapsable = panelCard.DivDyn("collapseNouvelleCategorie", "collapse collapse");
        Panel panelBodyCat = collapsable.DivDyn("bodyNouvelleCategorie", "card-body");

        Panel row = panelBodyCat.DivDyn("", "row ml-3 mb-3");

        Table tb = row.TableDyn("tableNouvelleCategorie", "table");
        TableHeaderRow tbHeaderRow = tb.ThrDyn();
        tbHeaderRow.CssClass = "fake-button";
        tbHeaderRow.ThdDyn("Nom de la catégorie");
        tbHeaderRow.ThdDyn("Détails de la catégorie");
        TableRow tbRowNom = tb.TrDyn();
        TableCell tbCellDescription = tbRowNom.TdDyn();
        tbCellDescription.TbDyn("tbCategorieDescription","",50,"form-control");
        TableCell tbCellDetails = tbRowNom.TdDyn();
        tbCellDetails.TbDyn("tbCategorieDetails", "", 1000, "form-control");
        Panel panelNouvelleCategorie = (Panel)panelGestionCategories.FindControl("collapseNouvelleCategorie");
        panelNouvelleCategorie.BtnDyn("btnConfirmerNouvelleCategorie", "Ajouter la nouvelle catégorie", AjouterCategorie, "btn btn-secondary btn-block");
    }
    protected void SupprimerCategorie(object sender, EventArgs e)
    {
        string categorieChoisi = ((Button)sender).ID;
        List<Categorie> lstCategorieASupprimer = new List<Categorie>();
        foreach(Categorie categorie in categories.Values)
        {
            if (categorie.NoCategorie.ToString().Equals(categorieChoisi))
            {
                lstCategorieASupprimer.Add(categorie);
                break;
            }
        }
        try
        {
            categories.Remove(lstCategorieASupprimer.FirstOrDefault());
            ClientScript.RegisterStartupScript(this.GetType(), "Succès", "alert('La catégorie a été supprimé avec succès.');window.location.href = window.location.href;", true);
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Erreur!", "alert('La catégorie n'a pas été supprimé.');", true);
        }

    }
    protected void AjouterCategorie(object sender, EventArgs e)
    {
        string descript = ((TextBox)panelGestionCategories.FindControl("tbCategorieDescription")).Text;
        string details = ((TextBox)panelGestionCategories.FindControl("tbCategorieDetails")).Text;

        Boolean booCategorieDejaLa = false;
        foreach(Categorie categorie in categories.Values)
        {
            if (categorie.Description.Equals(descript))
            {
                booCategorieDejaLa = true;
                break;
            }
        }

        if (descript.Trim().Length==0|| details.Trim().Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Erreur!", "alert('La description ou la section détail ne peuvent pas être vides.')", true);
        }
        else if (booCategorieDejaLa)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Erreur!", "alert('La description est déja présente.')", true);
        }
        else
        {
            int intcategorieMax = 10;
            if (categories.Values.Count() > 0)
            {
                intcategorieMax = categories.Values.Max(x => x.NoCategorie.Value) + 10;
            }
            categories.Add(new Categorie(null)
            {
                NoCategorie = intcategorieMax,
                Description = descript,
                Details = details
            });
            ClientScript.RegisterStartupScript(this.GetType(), "Succès", "alert('La catégorie a été ajoutée avec succès.');window.location.href = window.location.href;", true);
        }
    }

    protected void btnEnvoyerATous_Click(object sender, EventArgs e)
    {
        Response.Clear();
        var sb = new System.Text.StringBuilder();
        sb.Append("<html>");
        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
        sb.AppendFormat("<form action='{0}' method='post'>", "/Pages/Courriel.aspx");
        sb.AppendFormat("<input type='hidden' name='messageATous' value='{0}'>","true");
        sb.Append("</form>");
        sb.Append("</body>");
        sb.Append("</html>");
        Response.Write(sb.ToString());
        Response.End();
    }

    private void btnCourrielClient_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        PPClients ppv = new PPClients();
        Client v = ppv.Values.Find(vf => vf.NoClient == long.Parse(btn.CommandArgument));
        Response.Clear();
        var sb = new System.Text.StringBuilder();
        sb.Append("<html>");
        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
        sb.AppendFormat("<form action='{0}' method='post'>", "/Pages/Courriel.aspx");
        sb.AppendFormat("<input type='hidden' name='courrielC' value='{0}'>", v.AdresseEmail);
        sb.AppendFormat("<input type='hidden' name='sujet' value='Sujet du message'>");
        sb.AppendFormat("<input type='hidden' name='contenu' value='Contenu du message'>");
        sb.Append("</form>");
        sb.Append("</body>");
        sb.Append("</html>");
        Response.Write(sb.ToString());
        Response.End();
    }

    private void btnCourrielVendeur_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        PPVendeurs ppv = new PPVendeurs();
        Vendeur v = ppv.Values.Find(vf => vf.NoVendeur == long.Parse(btn.CommandArgument));
        Response.Clear();
        var sb = new System.Text.StringBuilder();
        sb.Append("<html>");
        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
        sb.AppendFormat("<form action='{0}' method='post'>", "/Pages/Courriel.aspx");
        sb.AppendFormat("<input type='hidden' name='courrielV' value='{0}'>", v.AdresseEmail);
        sb.AppendFormat("<input type='hidden' name='sujet' value='Sujet du message'>");
        sb.AppendFormat("<input type='hidden' name='contenu' value='Contenu du message'>");
        sb.Append("</form>");
        sb.Append("</body>");
        sb.Append("</html>");
        Response.Write(sb.ToString());
        Response.End();
    }


    private void chargerTablesTotal()
    {
        PPClients ppc = new PPClients();
        PPVendeurs ppv = new PPVendeurs();

        PPHistoriquePaiements historiquePaiements = new PPHistoriquePaiements();

        foreach(Client c in ppc.Values)
        {
            decimal montant = historiquePaiements.Values
                                .Where(x => x.NoClient == c.NoClient)
                                .Sum(x => x.MontantVenteAvantLivraison).Value;

            TableRow tbR = tableTousClients.TrDyn();
            tbR.TdDyn(c.Nom == null || c.Prenom == null ? "Inconnus" : $"{c.Nom}, {c.Prenom}");
            tbR.TdDyn(c.AdresseEmail);
            tbR.TdDyn(c.DateCreation.Value.ToShortDateString());
            tbR.TdDyn(montant.ToString("N2") + "$");

            Button btnClient = tbR.TdDyn().BtnDyn("", "Courriel", btnCourrielClient_Click, "btn btn-secondary btn-block");
            btnClient.CommandArgument = c.NoClient.ToString();
        }

        foreach (Vendeur v in ppv.Values)
        {
            decimal montant = historiquePaiements.Values
                                .Where(x => x.NoVendeur == v.NoVendeur)
                                .Sum(x => x.MontantVenteAvantLivraison).Value;

            TableRow tbR = tableTousVendeurs.TrDyn();
            tbR.TdDyn(v.Nom + ", " + v.Prenom);
            tbR.TdDyn(v.NomAffaires);
            tbR.TdDyn(v.DateCreation.ToString());
            tbR.TdDyn(montant.ToString("N2") + "$");

            Button btnVendeur = tbR.TdDyn().BtnDyn("", "Courriel", btnCourrielVendeur_Click, "btn btn-secondary btn-block");
            btnVendeur.CommandArgument = v.NoVendeur.ToString();
        }


    }



}
