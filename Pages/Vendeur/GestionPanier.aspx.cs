using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Vendeur_GestionPanier : System.Web.UI.Page
{
    Vendeur vendeur;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Session.IsVendeur())
            Response.Redirect(SessionManager.RedirectConnexionLink);

        if (!IsPostBack)
        {
            ddlChoix.Items.Add(new ListItem("1 mois", "1"));
            ddlChoix.Items.Add(new ListItem("2 mois", "2"));
            ddlChoix.Items.Add(new ListItem("3 mois", "3"));
            ddlChoix.Items.Add(new ListItem("6 mois", "6"));

            ddlChoix.SelectedValue = "6";
        }
        //for pour remplir les paniers
        vendeur = Session.GetVendeur();

        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        PPProduits produits = new PPProduits();
        PPClients clients = new PPClients();

        int nbMois = int.Parse(ddlChoix.SelectedValue);
        DateTime min = DateTime.Today.AddMonths(-1 * nbMois);

        var yes = from article in articlesEnPanier.Values
                      //join vendeur in vendeurs.Values on article.NoVendeur equals vendeur.NoVendeur
                  join produit in produits.Values on article.NoProduit equals produit.NoProduit
                  where article.NoVendeur == vendeur.NoVendeur// && article.DateCreation.Value.Date > min 
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
        int index2 = 0;
        Panel rows = null;

        /* if (grouped.Count() == 0)
         {
             rows = pnHaut.DivDyn("", "row mb-3");
             rows.DivDyn("", "col-2");
             Panel pnCol = rows.DivDyn("", "");
             pnCol.LblDyn("lblVide", "Vous n'avez aucun panier pour cette période de temps : " + ddlChoix.SelectedItem, "h4 text-info");
         }
         else { */

        bool auMoinsUNPanier = false;
        foreach (var group in grouped)
        {
            bool dateChecker = false;
            foreach (var ap in group.Articles)
            {
                if (ap.Article.DateCreation.Value.Date >= min)
                {
                    dateChecker = true;
                    auMoinsUNPanier = true;
                }
            }

            if (dateChecker)
            {
                string nomClient = "Annonyme";
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

                rows = pnHaut.DivDyn("", "row mb-3");
                /* if (index % 2 == 0)
                 {
                     rows = pnHaut.DivDyn("", "row mb-3");
                     index2++;
                 }*/


                //header des categories              
                Panel panelCard = rows.DivDyn("", "col-12");
                Panel card = panelCard.DivDyn("", "card");
                Panel panelTitreCategories = card.DivDyn("", "card-header fake-button");
                panelTitreCategories.LblDyn("", nomClient, "card-title h5 text-info");
                panelTitreCategories.LblDyn("", "valeur de " + montant.ToString("N2") + " $", "ml-3 card-title h6");
                panelTitreCategories.LblDyn("", "Articles (" + nombreArticles.ToString() + ")", "ml-3 card-title h6");
                panelTitreCategories.LblDyn("", datePanier.ToString("yyyy/MM/dd"), "ml-3 card-title h6 text-success");

                //Assigner les options de collapse au header
                panelTitreCategories.Attributes.Add("data-toggle", "collapse");
                panelTitreCategories.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapsePanier" + index.ToString() + index2.ToString());
                panelTitreCategories.Attributes.Add("aria-expanded", "false");
                panelTitreCategories.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapsePanier" + index.ToString() + index2.ToString());


                Panel collapsable;
                //body des categories

                collapsable = card.DivDyn("collapsePanier" + index.ToString() + index2.ToString(), "collapse collapse");


                Panel panelBodyCat = collapsable.DivDyn("", "card-body");

                //prix total
                Panel rowTotal = panelBodyCat.DivDyn("", "row ml-3");
                rowTotal.LblDyn("", "total de ce panier : ", "h4 mb-3");
                rowTotal.LblDyn("", montant.ToString("N2") + " $", "h4 text-success mb-3 ml-2");

                //colones header

                Table table = panelBodyCat.TableDyn("", "table table-hover fake-button");
                TableHeaderRow rowTable = table.ThrDyn();
                TableHeaderCell CellCss = rowTable.ThdDyn("");

                rowTable.ThdDyn("Produit");
                CellCss = rowTable.ThdDyn("Prix unitaire");
                CellCss.CssClass = "text-right";
                CellCss = rowTable.ThdDyn("Poids unitaire");
                CellCss.CssClass = "text-right";
                CellCss = rowTable.ThdDyn("Qte");
                CellCss.CssClass = "text-center";
                CellCss = rowTable.ThdDyn("Prix total (sans rabais)");
                CellCss.CssClass = "text-right";

                foreach (var article in group.Articles)
                {
                    //contenu des collapse

                    string nomArticle = article.Produit.Nom;
                    string lblRabais = "";
                    decimal montantArticle = -1;
                    if (article.Produit.PrixVente != article.Produit.PrixDemande && article.Produit.DateVente >= DateTime.Today)
                    {
                        decimal montantRabais = article.Produit.PrixDemande.Value - article.Produit.PrixVente.Value;
                        lblRabais = "rabais! (" + montantRabais.ToString("N2") + "$) finit le " + article.Produit.DateVente.Value.ToString("yyyy-MM-dd");
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

                    TableCell cell2 = nRow.TdDyn();
                    cell2.LblDyn("", nomArticle, "");

                    TableCell cell3 = nRow.TdDyn("", "text-right");
                    cell3.LblDyn("", montantArticle.ToString("N2") + " $ ", "");
                    cell3.BrDyn();
                    cell3.LblDyn("", lblRabais, "text-success");

                    TableCell cell4 = nRow.TdDyn("", "text-right");
                    cell4.LblDyn("", poidsArticle.ToString("N2") + " Lbs", "");

                    TableCell cell5 = nRow.TdDyn("", "text-center");
                    cell5.LblDyn("", "(" + nbItems.ToString() + ")", "");

                    TableCell cell6 = nRow.TdDyn("", "text-right");
                    cell6.LblDyn("", prix.ToString("N2") + " $", "text-success");

                }
                Panel rowBtnSupprimer = collapsable.DivDyn("", "row ml-3 mr-3 mb-3");
                Button btnClient = rowBtnSupprimer.BtnDyn("btn" + group.NoClient, "Courriel", btnCourriel_Click, "btn btn-outline-dark btn-block");
                btnClient.CommandArgument = (group.NoClient).ToString();
            }
            index++;
        }

        if (!auMoinsUNPanier)
        {
            rows = pnHaut.DivDyn("", "row mb-3");
            rows.DivDyn("", "col-2");
            Panel pnCol = rows.DivDyn("", "");
            pnCol.LblDyn("lblVide", "Vous n'avez aucun panier pour cette période de temps : " + ddlChoix.SelectedItem, "h4 text-info");
        }
        // }

        //paniers du bas
        PPArticlesEnPanier articlesEnPanier2 = new PPArticlesEnPanier();
        PPProduits produits2 = new PPProduits();
        PPClients clients2 = new PPClients();

        rows = null;
        min = DateTime.Today.AddMonths(-6);
        var yes2 = from article in articlesEnPanier2.Values
                       //join vendeur in vendeurs.Values on article.NoVendeur equals vendeur.NoVendeur
                   join produit in produits2.Values on article.NoProduit equals produit.NoProduit
                   where article.NoVendeur == vendeur.NoVendeur //&& article.DateCreation.Value.Date < min // Mettre la variable pour gérer les mois
                   orderby article.DateCreation ascending
                   select new { Article = article, Produit = produit };

        var groupedBas = from article in yes2
                         group article by article.Article.NoClient into g
                         join client in clients2.Values on g.Key equals client.NoClient
                         select new
                         {
                             NoClient = g.Key,
                             Client = client,
                             Articles = g.ToList()
                         };


        index = 0;
        index2 = 0;

        bool auMoinsUNPanierH = false;

        if (groupedBas.Count() <= 0)
        {
            auMoinsUNPanierH = true;
        }

        foreach (var group in groupedBas)
        {

            bool dateChecker = false;
            foreach (var ap in group.Articles)
            {
                if (ap.Article.DateCreation.Value.Date > min)
                {
                    dateChecker = true;
                }
            }

            if (!dateChecker)
            {
                auMoinsUNPanierH = true;
                string nomClient = "Annonyme";
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
                rows = panelVieux.DivDyn("", "row mb-3");
                /*  if (index % 2 == 0)
                  {
                      rows = panelVieux.DivDyn("", "row mb-3");
                      index2++;
                  }*/

                //header des categories              
                Panel panelCard = rows.DivDyn("", "col-12");
                Panel card = panelCard.DivDyn("", "card");
                Panel panelTitreCategories = card.DivDyn("", "card-header fake-button");

                panelTitreCategories.LblDyn("", nomClient, "card-title h5 text-info");
                panelTitreCategories.LblDyn("", "valeur de " + montant.ToString("N2") + " $", "ml-3 card-title h6");
                panelTitreCategories.LblDyn("", "Articles (" + nombreArticles.ToString() + ")", "ml-3 card-title h6");
                panelTitreCategories.LblDyn("", datePanier.ToString("yyyy/MM/dd"), "ml-3 card-title h6 text-danger");


                //Assigner les options de collapse au header
                panelTitreCategories.Attributes.Add("data-toggle", "collapse");
                panelTitreCategories.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapsePanierBas" + index.ToString() + index2.ToString());
                panelTitreCategories.Attributes.Add("aria-expanded", "false");
                panelTitreCategories.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapsePanierBas" + index.ToString() + index2.ToString());


                Panel collapsable;
                //body des categories

                collapsable = card.DivDyn("collapsePanierBas" + index.ToString() + index2.ToString(), "collapse collapse");


                Panel panelBodyCat = collapsable.DivDyn("", "card-body");

                //prix total
                Panel rowTotal = panelBodyCat.DivDyn("", "row ml-3");
                rowTotal.LblDyn("", "total de ce panier : ", "h4 mb-3");
                rowTotal.LblDyn("", montant.ToString("N2") + " $", "h4 text-success mb-3 ml-2");

                //colones header
                Table table = panelBodyCat.TableDyn("", "table table-hover fake-button");
                TableHeaderRow rowTable = table.ThrDyn();
                TableHeaderCell CellCss = rowTable.ThdDyn("");

                rowTable.ThdDyn("Produit");
                CellCss = rowTable.ThdDyn("Prix unitaire");
                CellCss.CssClass = "text-right";
                CellCss = rowTable.ThdDyn("Poids unitaire");
                CellCss.CssClass = "text-right";
                CellCss = rowTable.ThdDyn("Qte");
                CellCss.CssClass = "text-center";
                CellCss = rowTable.ThdDyn("Prix total (sans rabais)");
                CellCss.CssClass = "text-right";

                foreach (var article in group.Articles)
                {
                    //contenu des collapse

                    string nomArticle = article.Produit.Nom;
                    string lblRabais = "";
                    decimal montantArticle = -1;
                    if (article.Produit.PrixVente != article.Produit.PrixDemande && article.Produit.DateVente >= DateTime.Today)
                    {
                        decimal montantRabais = article.Produit.PrixDemande.Value - article.Produit.PrixVente.Value;
                        lblRabais = "rabais! (" + montantRabais.ToString("N2") + "$) finit le" + article.Produit.DateVente.Value.ToString("yyyy-MM-dd");
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

                    TableCell cell2 = nRow.TdDyn();
                    cell2.LblDyn("", nomArticle, "h6");

                    TableCell cell3 = nRow.TdDyn("", "text-right");
                    cell3.LblDyn("", montantArticle.ToString("N2") + " $ ", "");
                    cell3.BrDyn();
                    cell3.LblDyn("", lblRabais, "text-success");

                    TableCell cell4 = nRow.TdDyn("", "text-right");
                    cell4.LblDyn("", poidsArticle.ToString("N2") + " Lbs", "");

                    TableCell cell5 = nRow.TdDyn("", "text-center");
                    cell5.LblDyn("", "(" + nbItems.ToString() + ")", "");

                    TableCell cell6 = nRow.TdDyn("", "text-right");
                    cell6.LblDyn("", prix.ToString("N2") + " $", "text-success");

                }
                Panel rowBtnSupprimer = collapsable.DivDyn("", "row ml-3 mr-3 mb-3");
                Button btnClient = rowBtnSupprimer.BtnDyn("", "Courriel", btnCourriel_Click, "btn btn-outline-dark col-5");
                btnClient.CommandArgument = group.NoClient.ToString();
                rowBtnSupprimer.DivDyn("", "col-2");
                Button btn = rowBtnSupprimer.BtnDyn("", "Supprimer", null, "btn btn-outline-danger col-5");

                btn.OnClientClick = "return false;";
                btn.Attributes.Add("data-toggle", "modal");
                btn.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_myModal" + index.ToString() + index2.ToString());

                modal(panelAvecModal, "myModal" + index.ToString() + index2.ToString(), group.NoClient.ToString(), "idBtnAnnuler" + index.ToString() + index2.ToString(), "Supression", "Voulez vous supprimer ce panier", true);
            }
            index++;
        }

        if (!auMoinsUNPanierH)
        {
            panelVieux.LblDyn("", "Vous n'avez aucun panier vieux de plus de 6 mois", "text-info h4");
        }
        /* }
         else
         {
             panelVieux.LblDyn("", "Vous n'avez aucun panier vieux de plus de 6 mois", "text-info h4");
         }*/

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
            Button btnOk = panelModalFooter.BtnDyn(btnOKID, "Confirmer", click_supprimer_confirme, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                                                                                                         // btnOk.Attributes.Add("data-dismiss", "modal");
            Button btnNon = panelModalFooter.BtnDyn(btnAnnulerID, "Annuler", null, "btn btn-dark"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnNon.Attributes.Add("data-dismiss", "modal");
        }
        else
        {
            Button btnOk = panelModalFooter.BtnDyn(btnOKID, "fermer", null, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnOk.Attributes.Add("data-dismiss", "modal");
        }
    }

    private void click_supprimer_confirme(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string id = btn.ID;
        PPArticlesEnPanier articles = new PPArticlesEnPanier();

        List<ArticleEnPanier> listePanier = articles.Values.Where(x => x.NoVendeur == vendeur.NoVendeur && x.NoClient == int.Parse(id)).ToList();

        /*foreach (ArticleEnPanier article in listePanier)
        {
            articles.Remove(article);
        }
        articles.Update();*/

        articles.Remove(listePanier);
        PPClients ppc = new PPClients();
        Client cl = ppc.Values.Find(c => c.NoClient == int.Parse(id));

        Response.Clear();
        var sb = new System.Text.StringBuilder();
        sb.Append("<html>");
        sb.AppendFormat("<body onload='document.forms[0].submit()'>");
        sb.AppendFormat("<form action='{0}' method='post'>", "/Pages/Courriel.aspx");
        sb.AppendFormat("<input type='hidden' name='courrielC' value='{0}'>", cl.AdresseEmail);
        sb.AppendFormat("<input type='hidden' name='sujet' value='Votre panier a été supprimé.'>");
        sb.AppendFormat("<input type='hidden' name='contenu' value='Raison:'>");
        sb.AppendFormat("<input type='hidden' name='boutonAnnuler' value='Raison:'>");
        sb.Append("</form>");
        sb.Append("</body>");
        sb.Append("</html>");
        Response.Write(sb.ToString());
        Response.End();
    }
}