using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccueilVendeur : System.Web.UI.Page
{
    PPClients clients = new PPClients();
    Vendeur vendeur;
    PPVendeursClients vendeursClients = new PPVendeursClients();
    PPCommandes commandes = new PPCommandes();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Session.IsVendeur())
            Response.Redirect(SessionManager.RedirectConnexionLink);

        vendeur = Session.GetVendeur();
        
        //Requetes pour commandes non traitées
        //PPCommandes commandes = new PPCommandes();
        PPDetailsCommandes details = new PPDetailsCommandes();
        PPClients clients = new PPClients();

        var commandesVendeur = from commande in commandes.Values
                               join monClient in clients.Values on commande.NoClient equals monClient.NoClient
                               where commande.NoVendeur == vendeur.NoVendeur && commande.Statut == "0"
                               orderby commande.DateCommande ascending
                               let lstDetails = details.Values.Where(x => x.NoCommande == commande.NoCommande).ToList()
                               select new { Commande = commande, Client = monClient, Details = lstDetails };

        //Commandes
        int indexCom = 0;

        if (commandesVendeur.Count() > 0)
        {

        

        foreach (var com in commandesVendeur)
        {
            TableRow row = new TableRow();
            long? noCommande = com.Commande.NoCommande;
            string nomClient = com.Client.Prenom + " " + com.Client.Nom;
            DateTime dateCommande = com.Commande.DateCommande.Value;
            
            decimal poidsCommande = com.Commande.PoidsTotal.Value;

            PPHistoriquePaiements historique = new PPHistoriquePaiements();
            //requete pour aller cherche le montant dans l'historique
            var montant = from montantHisto in historique.Values
                          where montantHisto.NoCommande == com.Commande.NoCommande
                          select montantHisto.MontantVenteAvantLivraison;

            decimal montantCom = montant.First().Value;

            string nomPDF = "";

            if (File.Exists(Server.MapPath("~/Factures/" + noCommande.ToString() + ".pdf")))
            {
                nomPDF = noCommande.ToString()+".pdf";
            }
            else
            {
                nomPDF = "defaut.pdf";
            }

                TableCell cellNom = new TableCell();
            /* cellNom.Text = "Marcel Leboeuf";
             cellNom.Attributes.Add("OnClick", "window.location.assign('http://www.google.com');");*/
            //cellNom.BtnDyn("", nomClient, ouvrirInfosClients, "table btn btn-outline-secondary text-left");
            cellNom.Text = nomClient;
            cellNom.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
            row.Cells.Add(cellNom);


            TableCell cellDate = new TableCell();
            cellDate.Text = dateCommande.ToString("yyyy/MM/dd");
            cellDate.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
            row.Cells.Add(cellDate);

            TableCell cellPrix = new TableCell();
            cellPrix.CssClass = "text-right";
            cellPrix.Text = montantCom.ToString("N2") + " $";
            
            cellPrix.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
            row.Cells.Add(cellPrix);

            TableCell cellStatut = new TableCell();
            cellStatut.CssClass = "text-right";
            cellStatut.Text = poidsCommande.ToString("N2") + " Lbs";
            cellStatut.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
            row.Cells.Add(cellStatut);


            //modal (il faut faire référence à la div à cet effet dans le front-hand.
            modal(panelAvecModal, "myModal" + indexCom.ToString(),noCommande.ToString(), "btnAnnulerLiv" + indexCom.ToString(), "Livraison", "Voulez-vous vraiment confirmer la livraison de cette commande? (" + noCommande.ToString() + ")", true);

            TableCell cellBtn = row.TdDyn();
            Button btnLivrer = cellBtn.BtnDyn("", "Prêt à livrer", null, "btn btn-secondary btn-block btn-sm");

            btnLivrer.OnClientClick = "return false;";
            btnLivrer.Attributes.Add("data-toggle", "modal");
            btnLivrer.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_myModal" + indexCom.ToString());

            row.Cells.Add(cellBtn);

            tableCommandesNonTraitee.Rows.Add(row);
            indexCom++;
        }
        }
        else
        {
            tableCommandesNonTraitee.Visible = false;
            pnUseless.LblDyn("", "Vous n'avez aucune commande non traités", "h4 text-info");
        }







        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        PPProduits produits = new PPProduits();

        var yes = from article in articlesEnPanier.Values
                      //join vendeur in vendeurs.Values on article.NoVendeur equals vendeur.NoVendeur
                  join produit in produits.Values on article.NoProduit equals produit.NoProduit
                  where article.NoVendeur == vendeur.NoVendeur
                  orderby article.DateCreation ascending
                  select new { Article = article, Produit = produit };

        var grouped = from article in yes
                      group article by article.Article.NoClient into g
                      join client in clients.Values on g.Key equals client.NoClient
                      select new
                      {
                          NoClient = g.Key,
                          Client = client,
                          Articles = g.ToList()
                      };

        int index = 0;

        if (grouped.Count() > 0)
        {


            foreach (var group in grouped)
            {

                string nomClient = "Anonyme";
                string noClient = group.Client.NoClient.ToString();
                decimal montant = 0;
                int nombreArticles = 0;
                DateTime datePanier = DateTime.Now;

                if (group.Client.Prenom != null && group.Client.Nom != null)
                {
                    nomClient = group.Client.Prenom + " " + group.Client.Nom;
                }

                foreach (var article in group.Articles)
                {
                    nombreArticles++;
                    montant += article.Produit.PrixDemande.Value * article.Article.NbItems.Value;
                    datePanier = article.Article.DateCreation.Value.Date;
                }

                Panel pnCard = panelPanier.DivDyn("", "card mb-3");
                Panel pnHeader = pnCard.DivDyn("", "card-header fake-button");

                pnHeader.LblDyn("", "(" + noClient + ") ", "card-title h5 text-info");
                pnHeader.LblDyn("", nomClient, "card-title h5 text-info");
                pnHeader.LblDyn("", "valeur de " + montant.ToString("N2") + " $", "ml-3 card-title h6");
                pnHeader.LblDyn("", "Articles (" + nombreArticles.ToString() + ")", "ml-3 card-title h6");
                pnHeader.LblDyn("", datePanier.ToString("yyyy/MM/dd"), "ml-3 card-title h6 text-success");

                pnHeader.Attributes.Add("data-toggle", "collapse");
                pnHeader.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapsePanierBas" + index.ToString());
                pnHeader.Attributes.Add("aria-expanded", "false");
                pnHeader.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapsePanierBas" + index.ToString());


                Panel collapsable = pnCard.DivDyn("collapsePanierBas" + index.ToString(), "collapse collapse");
                Panel panelBodyCat = collapsable.DivDyn("", "card-body");

                //prix
                Panel rowTotal = panelBodyCat.DivDyn("", "row ml-3");
                rowTotal.LblDyn("", "total de ce panier : ", "h4 mb-3");
                rowTotal.LblDyn("", montant.ToString("N2") + " $", "h4 text-success mb-3 ml-2");


                Table table = panelBodyCat.TableDyn("", "table table-hover fake-button");
                TableHeaderRow rowTable = table.ThrDyn();
                rowTable.ThdDyn("");
                rowTable.ThdDyn("Produit");
                TableHeaderCell headerCss = rowTable.ThdDyn("Prix unitaire");
                headerCss.CssClass = "moneyDroite";
                headerCss = rowTable.ThdDyn("Poids unitaire");
                headerCss.CssClass = "moneyDroite";
                headerCss = rowTable.ThdDyn("Qte.");
                headerCss.CssClass = "text-center";
                headerCss = rowTable.ThdDyn("Prix total (sans rabais)");
                headerCss.CssClass = "moneyDroite";

                /* for (int j = 0; j < 5; j++)
                 {*/
                foreach (var article in group.Articles)
                {
                    string lblRabais = "";
                    string nomArticle = article.Produit.Nom;
                    decimal montantArticle = -1;
                    if (article.Produit.PrixVente != article.Produit.PrixDemande && article.Produit.DateVente >= DateTime.Today)
                    {
                        decimal montantRabais = article.Produit.PrixDemande.Value - article.Produit.PrixVente.Value;
                        lblRabais = "<br/>rabais! (" + montantRabais.ToString("N2") + "$) finit le " + article.Produit.DateVente.Value.ToString("yyyy-MM-dd");
                        montantArticle = article.Produit.PrixDemande.Value;
                    }
                    else
                    {
                        montantArticle = article.Produit.PrixDemande.Value;
                    }
                    decimal poidsArticle = article.Produit.Poids.Value;
                    short nbItems = article.Article.NbItems.Value;
                    decimal prix = article.Produit.PrixDemande.Value * article.Article.NbItems.Value;


                    TableRow nRow = table.TrDyn();
                    nRow.Attributes.Add("OnClick", "ouvrirDetailProduit('" + article.Produit.NoProduit.ToString() + "');");

                    TableCell cell1 = nRow.TdDyn("", "");
                    cell1.ImgDyn("", "~/Pictures/" + article.Produit.Photo, "imgResize");

                    TableCell cell2 = nRow.TdDyn("", "");
                    cell2.LblDyn("", nomArticle, "");

                    TableCell cell3 = nRow.TdDyn("", "moneyDroite");
                    cell3.LblDyn("", montantArticle.ToString("N2") + " $ ", "");
                    cell3.LblDyn("", lblRabais, "text-success");

                    TableCell cell4 = nRow.TdDyn("", "moneyDroite");
                    cell4.LblDyn("", poidsArticle.ToString("N2") + " Lbs", "");

                    TableCell cell5 = nRow.TdDyn("", "text-center");
                    cell5.LblDyn("", "(" + nbItems.ToString() + ")", "");

                    TableCell cell6 = nRow.TdDyn("", "moneyDroite");
                    cell6.LblDyn("", prix.ToString("N2") + " $", "text-success");

                }

                Panel rowBtnSupprimer = collapsable.DivDyn("", "row ml-3 mr-3 mb-3");
                Button btnClient = rowBtnSupprimer.BtnDyn("btnCourriel" + index.ToString(), "Courriel", btnCourriel_Click, "btn btn-outline-dark btn-block");
                btnClient.CommandArgument = (group.NoClient).ToString();
                index++;
            }
        }
        else
        {
            panelPanier.LblDyn("", "Vous n'avez aucun panier présentement", "h4 text-info");
        }

        panelNBVisite.LblDyn("", "Vous êtes présentement à ", "h5");

        //requete trouver nbVisites
        int visites = 0;
        PPVendeursClients clientsVendeur = new PPVendeursClients();
        VendeurClient client2 = clientsVendeur.Values.Find(x => x.NoVendeur == vendeur.NoVendeur);

        foreach (VendeurClient vc in clientsVendeur.Values)
        {
            if (vc.NoVendeur == vendeur.NoVendeur)
            {
                visites++;
            }
        }
        panelNBVisite.LblDyn("", visites.ToString(), "text-primary h4");
        panelNBVisite.LblDyn("", " visites de clients", "h5");

        nombreClientParVendeur();

        //ajouter les stats ici


    }

    protected void nombreClientParVendeur()
    {
        Table tableCliensPourVendeur = panelNBVisite.TableDyn("", "table table-active mt-3");
        TableHeaderRow thr = tableCliensPourVendeur.ThrDyn();
        thr.CssClass = "text-primary";
        thr.ThdDyn("Clients actifs");
        thr.ThdDyn("Clients potentiels");
        thr.ThdDyn("Clients visiteurs");
        TableHeaderCell cellh = thr.ThdDyn("Nombre total de clients");
        cellh.CssClass = "h4 text-success";

        int nbClientActif = 0;
        int nbClientPotentiel = 0;
        int nbClientVisiteur = 0;

        foreach (Client client in clients.Values)
        {
            Boolean clientTrouve = false;
            foreach (VendeurClient vendeurClient in vendeursClients.Values)
            {
                if (client.NoClient.Equals(vendeurClient.NoClient) && (vendeur.NoVendeur.Equals(vendeurClient.NoVendeur)))
                {
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
        rowVendeur.CssClass = "text-primary";
        TableCell cellActif = new TableCell();
        TableCell cellPotentiel = new TableCell();
        TableCell cellVisiteur = new TableCell();
        TableCell cellTotal = new TableCell();
        cellTotal.CssClass= "h5 text-success";
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

    private void btnCourriel_Click(object sender, EventArgs e)
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

    private void TabView(object sender, EventArgs e)
    {
        Button imgbtnsender = (Button)(sender);
        Control table = panelPanier.FindControl(imgbtnsender.CommandArgument);
        if (table.Visible == false)
        {
            table.Visible = true;
        }
        else
        {
            table.Visible = false;
        }

    }
    /* Fonction pour créer un modal complet : PanelParent, est le panel du aspx, 
     *                                        2 string d'idBoutons, laisser deuxieme a "" si on veut 1 boutton et mettre false pour avecConfirmation
                                              Message, message du body du modal.*/
    private void modal(Panel panelParent, string idModal, string btnOKID, string btnAnnulerID, string titre, string message, bool avecConfirmation)
    {
        Panel panelModal = panelParent.DivDyn(idModal, "modal"); //< div class="modal" id="myModal">
        Panel panelModalDialogue = panelModal.DivDyn("", "modal-dialog"); //<div class="modal-dialog">
        Panel panelModalContent = panelModalDialogue.DivDyn("", "modal-content"); //<div class="modal-content">

        Panel panelCardHeader = panelModalContent.DivDyn("", "card-header"); //<div class="card-header">
        Label lblHeader = panelCardHeader.LblDyn("", titre, "h5"); //<h4 class="modal-title">Livraison</h4>

        Panel panelModalBody = panelModalContent.DivDyn("", "modal-body"); //<div class="modal-body">
        Label lblMessage = panelModalBody.LblDyn("", message, ""); // Voulez vous vraiment confirmer la livraison de cette commande?

        Panel panelModalFooter = panelModalContent.DivDyn("", "modal-footer"); //<div class="modal-footer">

        if (avecConfirmation)
        {
            Button btnOk = new Button();//<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnOk.CssClass = "btn btn-secondary";
            btnOk.Text = "Prêt à livrer";
            btnOk.Command += livrerCommande;
            btnOk.CommandArgument = btnOKID;
            //btnOk.Attributes.Add("CustomParameter", btnOKID);

            panelModalFooter.Controls.Add(btnOk);
            //btnOk.Attributes.Add("data-dismiss", "modal");
            Button btnNon = panelModalFooter.BtnDyn(btnAnnulerID, "Annuler", null, "btn btn-dark"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnNon.Attributes.Add("data-dismiss", "modal");
        }
        else
        {
            Button btnOk = panelModalFooter.BtnDyn(btnOKID, "fermer", null, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnOk.Attributes.Add("data-dismiss", "modal");
        }
       
    }
    protected void livrerCommande(object sender, CommandEventArgs e)
    {
        //PPCommandes commandes = new PPCommandes();
        foreach (Commande commande in commandes.Values){
            if (commande.NoCommande.ToString().Equals(e.CommandArgument.ToString()))
            {
                Response.Write("ici");
                commande.Statut = "1";
                commandes.NotifyUpdated(commande);
            }
        }
        commandes.Update();
        Response.Write(e.CommandArgument.ToString());
        Response.Redirect(Request.RawUrl);
    }
    protected void ouvrirInfosClients(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Vendeur/InfosClients.aspx");
    }

}