using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Description résumée de Accueil
/// </summary>
public partial class AccueilCode : Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        bool connecte = false;
        if (Session.IsClient())
        {
            panelInformations.Visible = true;
            pnClientPanier.Visible = true;
            PanelVendeurs.Visible = true;
            connecte = true;
        }

        PPCategories categories = new PPCategories();
        PPVendeurs vendeurs = new PPVendeurs();
        PPProduits produits = new PPProduits();

        var groupe = from produit in produits.Values
                     group produit by produit.NoCategorie into groupement
                     join categorie in categories.Values on groupement.Key equals categorie.NoCategorie
                     select new { LCategories = groupement.ToList(), Categories = categorie };

        //for pour remplir le pnHaut (Correspond au panel avec le titre : catégorie
        int index = 0;
        int index2 = 0;
        Panel rows = null;
        foreach (var categorie in groupe)
        {
            //requete pour grouper les vendeurs
            var groupement = (from gr in categorie.LCategories.AsEnumerable()
                              group gr by gr.NoVendeur into ok
                              join vendeur in vendeurs.Values on ok.Key equals vendeur.NoVendeur
                              select new { Vendeurs = vendeur }).Where(v => v.Vendeurs.Statut != -1);

            string nomCategorie = categorie.Categories.Description;

            if (index % 2 == 0)
            {
                rows = pnHaut.DivDyn("", "row");
                index2++;
            }

            //header des categories              
            Panel panelCard = rows.DivDyn("", "col-lg-6 mb-3");

            panelCard.CardDynCollapse("", "", index2 == 1 && index == 0 ? "show" : "",
                header =>
                {
                    header.LblDyn("", nomCategorie, "card-title h5 text-primary");
                    header.LblDyn("", $"({groupement.Count()} vendeurs)", "card-title ml-3 h5 text-primary");
                },
                body =>
                {
                    Table table = body.TableDyn("", "table table-hover fake-button");
                    TableHeaderRow rowTable = table.ThrDyn();
                    rowTable.ThdDyn("");
                    rowTable.ThdDyn("Vendeur");
                    rowTable.ThdDyn("Quantité d'articles dans cette catégorie");
                    foreach (var vendeur in groupement)
                    {
                        PPProduits produitsCount = new PPProduits();
                        List<Produit> liste = produits.Values.Where(x => x.NoVendeur == vendeur.Vendeurs.NoVendeur && x.NoCategorie == categorie.Categories.NoCategorie).ToList();
                        //contenu des collapse

                        TableRow nRow = table.TrDyn();
                        if (connecte)
                        {
                            nRow.Attributes.Add("OnClick", "window.location.assign('/Pages/Client/Catalogue.aspx?NoVendeur=" + vendeur.Vendeurs.NoVendeur + "&NoCategorie=" + categorie.Categories.NoCategorie + "');");
                        }
                        else
                        {
                            nRow.Attributes.Add("OnClick", "window.location.assign('/Pages/Nouveautes.aspx');");
                        }
                        string strLogo = "placeholder.png";
                        if (vendeur.Vendeurs.Configuration != null)
                        {
                            strLogo = vendeur.Vendeurs.Configuration.Split(';')[0];
                        }
                        TableCell cell1 = nRow.TdDyn("", "");
                        cell1.ImgDyn("", "~/Logos/" + strLogo, "imgResize");

                        TableCell cell2 = nRow.TdDyn();
                        cell2.LblDyn("", vendeur.Vendeurs.NomAffaires, "text-info h5");

                        TableCell cell3 = nRow.TdDyn();
                        cell3.LblDyn("", "(" + liste.Count() + ")", "");

                    }
                    index++;
                });

        }

        //section client
        if (connecte)
        {
            Client client = Session.GetClient();
            //row de recherche
            //panel pnInfos

            Panel rowInfos = pnInfos.DivDyn("", "row");
            Panel pnPourConn = rowInfos.DivDyn("", "col-md-6");
            pnPourConn.LblDyn("", "Votre nombre de connexions : ", "align-self-center h5");
            pnPourConn.LblDyn("lblNbCo", client.NbConnexions.ToString(), "text-primary align-self-center h5");

            Panel pnDate = rowInfos.DivDyn("", "col-md-6");
            pnDate.LblDyn("", "Dernière connexion : ", "align-self-center h5");
            pnDate.LblDyn("", client.DateDerniereConnexion.Value.ToString("yyyy/MM/dd"), "align-self-center h5 text-primary");



            PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();


            var panierArticlesParVendeurs = (from article in articlesEnPanier.Values
                                             join produit in produits.Values on article.NoProduit equals produit.NoProduit
                                             where article.NoClient == client.NoClient
                                             select new
                                             {
                                                 Article = article,
                                                 Produit = produit
                                             } into articleProduit
                                             group articleProduit by articleProduit.Article.NoVendeur into g
                                             join vendeur in vendeurs.Values on g.Key equals vendeur.NoVendeur
                                             select new
                                             {
                                                 Vendeur = vendeur,
                                                 Articles = g.ToList()
                                             }).Where(v => v.Vendeur.Statut != -1);
            index = 0;
            index2 = 0;
            rows = null;
            foreach (var group in panierArticlesParVendeurs)
            {
                string nomVendeur = group.Vendeur.NomAffaires;
                string noVendeur = group.Vendeur.NoVendeur.ToString();
                decimal montant = 0;
                int nombreArticles = 0;
                DateTime datePanier = DateTime.Now;


                foreach (var article in group.Articles)
                {
                    nombreArticles++;
                    montant += article.Produit.PrixVente.Value * article.Article.NbItems.Value;
                    datePanier = article.Article.DateCreation.Value.Date;
                }

                // Panel card = pnHaut.DivDyn("card" + i.ToString(), "card");
                if (index % 2 == 0)
                {
                    rows = pnPaniers.DivDyn("", "row");
                    index2++;
                }

                //header des categories              
                Panel panelCard = rows.DivDyn("", "col-xl-6 mb-3");

                panelCard.CardDynCollapse("", "", "",
                    header =>
                    {
                        header.LblDyn("", nomVendeur, "card-title h5 text-info");
                        header.LblDyn("", $"valeur de {montant.ToString("N2")}$", "card-title ml-3 h6");
                        header.LblDyn("", $"Articles ({nombreArticles})", "card-title ml-3 h6");
                    },
                    body =>
                    {
                        body.BtnClientDyn("", "Aller au panier", $"panierVendeur({noVendeur}); return false;", "btn btn-secondary btn-block mb-3");

                        //colones header
                        Table table = body.TableDyn("", "table table-hover fake-button");
                        TableHeaderRow rowTable = table.ThrDyn();
                        rowTable.ThdDyn("");
                        rowTable.ThdDyn("Produit");
                        rowTable.ThdDyn("Poids unitaire");
                        rowTable.ThdDyn("Quantite");
                        rowTable.ThdDyn("Prix unitaire");
                        rowTable.ThdDyn("Prix total");

                        foreach (var article in group.Articles)
                        {
                            //contenu des collapse

                            string nomArticle = article.Produit.Nom;
                            decimal montantArticle = article.Produit.PrixVente.Value;
                            decimal poidsArticle = article.Produit.Poids.Value;
                            short nbItems = article.Article.NbItems.Value;
                            decimal prix = article.Produit.PrixVente.Value * article.Article.NbItems.Value;

                            TableRow nRow = table.TrDyn();
                            nRow.Attributes.Add("OnClick", "window.location.assign('/Pages/Client/InfoProduit.aspx?ID=" + article.Produit.NoProduit + "');");

                            TableCell cell1 = nRow.TdDyn("", "");
                            cell1.ImgDyn("", "~/Pictures/" + article.Produit.Photo, "imgResize");

                            TableCell cell2 = nRow.TdDyn();
                            cell2.LblDyn("", nomArticle, "h5");

                            TableCell cell3 = nRow.TdDyn();
                            cell3.LblDyn("", poidsArticle.ToString("N2") + " Lbs", "");

                            TableCell cell4 = nRow.TdDyn();
                            cell4.LblDyn("", "(" + nbItems.ToString() + ")", "");

                            TableCell cell5 = nRow.TdDyn();
                            cell5.CssClass = "moneyDroite";
                            cell5.LblDyn("", montantArticle.ToString("N2") + " $", "moneyDroite");

                            TableCell cell6 = nRow.TdDyn();
                            cell6.CssClass = "moneyDroite";
                            cell6.LblDyn("", prix.ToString("N2") + " $", "text-success moneyDroite");

                        }
                        //prix total
                        Panel rowTotal = body.DivDyn("", "moneyDroite");
                        rowTotal.LblDyn("", "Total de ce panier : ", "h4 mb-3");
                        rowTotal.LblDyn("", montant.ToString("N2") + " $", "h4 text-success mb-3 ml-2");
                        index++;
                    });
            }
            index = 0;
            index2 = 0;
            rows = null;
            foreach (var vendeur in vendeurs.Values.Where(v => v.Statut == 1))
            {
                string nomVendeur = vendeur.NomAffaires;
                if (index % 2 == 0)
                {
                    rows = pnVendeurs.DivDyn("", "row");
                    index2++;
                }
                //header des categories              
                Panel panelCard = rows.DivDyn("", "col-md-6 mb-3");
                Panel card = panelCard.DivDyn("", "card");
                Panel panelTitreCategories = card.DivDyn("", "card-header fake-button");
                var prod = produits.Values.Where(v => v.NoVendeur.Equals(vendeur.NoVendeur)).Count();
                panelTitreCategories.LblDyn("", nomVendeur + " (" + prod + " produits)", "card-title h5 text-primary");
                panelTitreCategories.Attributes.Add("OnClick", "window.location.assign('/Pages/Client/Catalogue.aspx?NoVendeur=" + vendeur.NoVendeur + "');");
                index++;
            }
        }

    }
}