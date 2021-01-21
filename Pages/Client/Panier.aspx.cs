using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Newtonsoft.Json;
using PdfSharp;
using PdfSharp.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

public partial class PanierCode : Page
{
    private SqlInit db = new SqlInit();
    private Dictionary<Vendeur, string> dictVendeurs = new Dictionary<Vendeur, string>();

    /*
     * Init
     */

    /// <summary>
    /// Génerer les paniers
    /// </summary>
    private void GenerePaniers()
    {
        Client client = Session.GetClient();

        var panierArticlesParVendeurs = from article in db.articlesEnPanier.Values
                                        join produit in db.produits.Values on article.NoProduit equals produit.NoProduit
                                        where article.NoClient == client.NoClient
                                        select new
                                        {
                                            Article = article,
                                            Produit = produit
                                        } into articleProduit
                                        group articleProduit by articleProduit.Article.NoVendeur into g
                                        join vendeur in db.vendeurs.Values on g.Key equals vendeur.NoVendeur
                                        select new
                                        {
                                            Vendeur = vendeur,
                                            Articles = g.ToList()
                                        };

        foreach (var group in panierArticlesParVendeurs)
        {
            // Ajouter les vendeurs au DropDownList
            string pnlVendeurId = $"pnlVendeur{group.Vendeur.NoVendeur}";
            dictVendeurs.Add(group.Vendeur, pnlVendeurId);
            // Faire les paniers
            pnlPaniersGeneres.CardDyn(
                pnlVendeurId, "mb-3",
                lbl => lbl.HlinkDYN("", $"~/Pages/Client/Catalogue.aspx?NoVendeur={group.Vendeur.NoVendeur}", group.Vendeur.NomAffaires, "text-white"),
                pnlBody =>
                {
                    Table tablePanier = pnlBody.TableDyn("", "table table-hover fake-button");
                    TableHeaderRow headerRow = tablePanier.ThrDyn();

                    headerRow.ThdDyn("Image");
                    headerRow.ThdDyn("Nom");
                    headerRow.ThdDyn("Description");
                    headerRow.ThdDyn("Poids<br/>(Livre)");
                    headerRow.ThdDyn("Quantité commandée");
                    headerRow.ThdDyn("Prix individuel");
                    headerRow.ThdDyn("Prix");

                    TableHeaderCell cellHeaderSupprimerTout = headerRow.ThdDyn();
                    cellHeaderSupprimerTout.BtnClientDyn("", "Vider le panier", $"supprimerPanier({client.NoClient}, {group.Vendeur.NoVendeur}); return false;", "btn btn-dark btn-block");

                    List<decimal> lstPrixPanier = new List<decimal>();
                    List<decimal> lstPrixPanierAvant = new List<decimal>();
                    List<decimal> lstPoidsPanier = new List<decimal>();

                    foreach (var article in group.Articles)
                    {
                        decimal prixIndividuel = 0;

                        bool prixIndividuelVente = false;
                        if ((article.Produit.PrixVente.HasValue && article.Produit.PrixVente != article.Produit.PrixDemande)
                            && (!article.Produit.DateVente.HasValue || article.Produit.DateVente >= DateTime.Now))
                        {
                                prixIndividuel = article.Produit.PrixVente.Value;
                                prixIndividuelVente = true;
                        }
                        else
                            prixIndividuel = article.Produit.PrixDemande.Value;

                        short nbItems = article.Produit.NombreItems.Value;
                        short nbItemsDansPanier = article.Article.NbItems.Value;

                        TableRow row = tablePanier.TrDyn();

                        string windowLocation = $"window.location.href = '/Pages/Client/InfoProduit.aspx?ID={article.Produit.NoProduit}'";

                        TableCell cellImage = row.TdDyn();
                        cellImage.ImgDyn("", "~/Pictures/" + article.Produit.Photo, "image-panier");
                        cellImage.Attributes.Add("onclick", windowLocation);

                        TableCell cellNom = row.TdDyn(article.Produit.Nom);
                        cellNom.Attributes.Add("onclick", windowLocation);
                        TableCell cellDescription = row.TdDyn(article.Produit.Description);
                        cellDescription.Attributes.Add("onclick", windowLocation);
                        TableCell cellPoids = row.TdDyn(article.Produit.Poids.ToString());
                        cellPoids.Attributes.Add("onclick", windowLocation);

                        TableCell cellQuantite = row.TdDyn();
                        DropDownList ddlQuantite = cellQuantite.DdlDyn("", $"quantiteArticle({article.Article.NoPanier}, this);", "form-control");
                        // Make sure the label doesn't have values (happens when reloading even though it should be replaced)
                        ddlQuantite.Items.Clear();
                        for (int i = 1; i <= nbItems; i++)
                            ddlQuantite.Items.Add(i.ToString());
                        ddlQuantite.SelectedIndex = nbItemsDansPanier - 1;

                        // Prix individuel
                        if (prixIndividuelVente)
                        {
                            TableCell cellPrix = row.TdDyn();
                            cellPrix.CssClass = "text-right";
                            cellPrix.Attributes.Add("onclick", windowLocation);
                            decimal prixDemande = article.Produit.PrixDemande.Value;
                            cellPrix.LblDyn("", prixDemande.ToString("N2") + "$<br/>", "text-secondary");
                            decimal pourcent = ((prixIndividuel / article.Produit.PrixDemande.Value) * 100) - 100;
                            cellPrix.LblDyn("", $"{pourcent.ToString("N2")}%<br/>", "text-success");
                            cellPrix.LblDyn("", prixIndividuel.ToString("N2") + "$", "text-success");
                            decimal prixNbAvant = prixDemande * nbItemsDansPanier;
                            lstPrixPanierAvant.Add(prixDemande * nbItemsDansPanier);

                            TableCell cellPrixNb = row.TdDyn();
                            cellPrixNb.CssClass = "text-right";
                            cellPrixNb.Attributes.Add("onclick", windowLocation);
                            cellPrixNb.LblDyn("", prixNbAvant.ToString("N2") + "$<br/>", "text-secondary");
                            decimal prixNb = prixIndividuel * nbItemsDansPanier;
                            lstPrixPanier.Add(prixNb);
                            cellPrixNb.LblDyn("", prixNb.ToString("N2") + "$", "text-success");
                        }
                        else
                        {
                            TableCell cellPrixIndividuel = row.TdDyn(prixIndividuel.ToString("N2") + "$");
                            cellPrixIndividuel.CssClass = "text-right";
                            cellPrixIndividuel.Attributes.Add("onclick", windowLocation);
                            // Prix 
                            decimal prixNb = prixIndividuel * nbItemsDansPanier;
                            TableCell cellPrix = row.TdDyn(prixNb.ToString("N2") + "$");
                            cellPrix.CssClass = "text-right";
                            cellPrix.Attributes.Add("onclick", windowLocation);
                            lstPrixPanier.Add(prixNb);
                        }

                        TableCell cellSupprimerArticle = row.TdDyn();
                        cellSupprimerArticle.BtnClientDyn("", "Supprimer l'article", $"supprimerArticle({article.Article.NoPanier}); return false;", "btn btn-dark btn-block");
                    }

                    if (lstPrixPanier.Count > 0)
                    {
                        TableRow rowEnd = tablePanier.TrDyn();
                        rowEnd.TdDyn();
                        rowEnd.TdDyn();
                        rowEnd.TdDyn();
                        rowEnd.TdDyn();
                        rowEnd.TdDyn();
                        rowEnd.TdDyn();
                        TableCell cellPrixTotal = rowEnd.TdDyn();
                        cellPrixTotal.CssClass = "text-right";
                        cellPrixTotal.LblDyn("", lstPrixPanier.Sum().ToString("N2") + "$");
                        rowEnd.TdDyn();
                    }

                    decimal poidsMax = group.Vendeur.PoidsMaxLivraison.Value;
                    decimal poidsCommande = group.Articles.Sum(x => x.Produit.Poids * x.Article.NbItems).Value;

                    Panel pnlPoids = pnlBody.DivDyn("", "w-100 text-right");
                    pnlPoids.LblDyn("", $"Poids de la livraison: {poidsCommande} Livre<br/>", "text-secondary");
                    pnlPoids.LblDyn("", $"Poids maximal du vendeur: {group.Vendeur.PoidsMaxLivraison} Livre", "text-secondary");

                    if (poidsMax < poidsCommande)
                        pnlBody.BtnDyn("", "Le panier est trop lourd de " + (poidsCommande - poidsMax), null, "btn btn-danger btn-block").Enabled = false;
                    else
                    {
                        Button btn = pnlBody.BtnDyn("", "Commander le panier", btnCommander_OnClick, "btn btn-secondary btn-block mt-3");
                        btn.Attributes.Add("data-vendeur", group.Vendeur.NoVendeur.ToString());
                    }
                });
        }
    }

    private void RemplitDdlVendeurs()
    {
        ddlVendeurs.Items.Add(new ListItem("Tous les vendeurs", "all"));

        foreach (var pnlVendeur in dictVendeurs)
            ddlVendeurs.Items.Add(new ListItem(pnlVendeur.Key.NomAffaires, pnlVendeur.Value));
    }

    private void RemplireInfosClient()
    {
        Client client = Session.GetClient();

        tbAdresseEmail.Text = client.AdresseEmail;
        tbPrenom.Text = client.Prenom;
        tbNom.Text = client.Nom;
        tbNoCiviqueRue.Text = client.Rue;
        tbVille.Text = client.Ville;
        ddlProvince.SelectedValue = client.Province;
        tbPays.Text = client.Pays;
        tbCodePostal.Text = client.CodePostal;
        tbTelephone.Text = client.Tel1;
        tbCellulaire.Text = client.Tel2;
    }

    private void RemplitDdlTypesLivraison()
    {
        foreach (TypeLivraison type in db.typesLivraison.Values)
            ddlOptionsEnvoi.Items.Add(new ListItem(type.Description, type.CodeLivraison.ToString()));
    }

    private decimal MaxTPS()
    {
        return db.taxeFederale
            .Values
            .Where(x => x.DateEffectiveTPS
                        == db.taxeFederale.Values.Max(y => y.DateEffectiveTPS))
            .First().TauxTPS.Value;
    }

    private decimal MaxTVQ()
    {
        return db.taxeProvinciale
            .Values
            .Where(x => x.DateEffectiveTVQ
                        == db.taxeProvinciale.Values.Max(y => y.DateEffectiveTVQ))
            .First().TauxTVQ.Value;
    }

    private void GenererEspaceCommande(Client client)
    {
        pnlPanierLivraison.Controls.Clear();
        long vendeurNo = long.Parse(hidVendeur.Value);

        Vendeur vendeur = db.vendeurs.Values.Find(x => x.NoVendeur == vendeurNo);

        lblNomVendeurPanierLivraison.Text = vendeur.NomAffaires;

        var articlesEnPanierPourVendeur = from article in db.articlesEnPanier.Values
                                          join produit in db.produits.Values on article.NoProduit equals produit.NoProduit
                                          where article.NoVendeur == vendeurNo && article.NoClient == client.NoClient
                                          select new { Article = article, Produit = produit };

        decimal poidsTotal = 0;
        decimal prixTotal = 0;
        decimal prixDiffTotal = 0;
        Table table = pnlPanierLivraison.TableDyn("", "table");
        TableHeaderRow thr = table.ThrDyn();
        thr.ThdDyn("Image");
        thr.ThdDyn("Nom");
        thr.ThdDyn("Quantité");
        thr.ThdDyn("Prix individuel");
        thr.ThdDyn("Prix");

        foreach (var prod in articlesEnPanierPourVendeur)
        {
            decimal prixDemande = prod.Produit.PrixDemande.Value;
            decimal totPrix = 0;

            decimal prixVente = 0;
            decimal diffVente = 0;
            decimal totDiffVente = 0;
            if ((prod.Produit.PrixVente.HasValue && prod.Produit.PrixVente != prod.Produit.PrixDemande)
                && (!prod.Produit.DateVente.HasValue || prod.Produit.DateVente.Value >= DateTime.Now))
            {
                prixVente = prod.Produit.PrixVente.Value;
                diffVente = prixDemande - prixVente;
            }

            TableRow row = table.TrDyn();
            row.TdDyn().ImgDyn("", "~/Pictures/" + prod.Produit.Photo, "image-panier");
            row.TdDyn(prod.Produit.Nom);
            row.TdDyn(prod.Article.NbItems.ToString());
            if (prixVente == 0)
            {
                totPrix = prixDemande * prod.Article.NbItems.Value;
                TableCell cellPrixDemande = row.TdDyn(prixDemande.ToString("N2") + "$");
                cellPrixDemande.CssClass = "text-right";
                TableCell cellPrixTot = row.TdDyn(totPrix.ToString("N2") + "$");
                cellPrixTot.CssClass = "text-right";
            }
            else
            {
                totPrix = prixVente * prod.Article.NbItems.Value;
                totDiffVente = diffVente * prod.Article.NbItems.Value;

                TableCell cellPrix = row.TdDyn();
                cellPrix.CssClass = "text-right";
                cellPrix.LblDyn("", prixVente.ToString("N2") + "$<br/>");
                cellPrix.LblDyn("", "Vous avez sauvé<br/>", "text-success small");
                cellPrix.LblDyn("", $"{diffVente.ToString("N2")}$", "text-success");

                TableCell cellTotal = row.TdDyn();
                cellTotal.CssClass = "text-right";
                cellTotal.LblDyn("", totPrix.ToString("N2") + "$<br/>");
                cellTotal.LblDyn("", "Vous avez sauvé<br/>", "text-success small");
                cellTotal.LblDyn("", $"{totDiffVente.ToString("N2")}$", "text-success");

                prixDiffTotal += totDiffVente;
            }


            prixTotal += totPrix;
            poidsTotal += prod.Article.NbItems.Value * prod.Produit.Poids.Value;
        }

        TableRow rowEnd = table.TrDyn();
        rowEnd.TdDyn();
        rowEnd.TdDyn();
        rowEnd.TdDyn();
        rowEnd.TdDyn();
        TableCell cellPrixTotal = rowEnd.TdDyn();
        cellPrixTotal.CssClass = "text-right";
        cellPrixTotal.LblDyn("", prixTotal.ToString("N2") + "$<br/>");
        if (prixDiffTotal > 0)
        {
            cellPrixTotal.LblDyn("", "Vous avez sauvé<br/>", "text-success small");
            cellPrixTotal.LblDyn("", $"{prixDiffTotal.ToString("N2")}$", "text-success");
        }

        lblPoidsLivraison.Text = poidsTotal.ToString() + " Livre";

        decimal prixLivraison = 0;
        short typeLivraison = short.Parse(ddlOptionsEnvoi.SelectedValue);

        if (prixTotal >= vendeur.LivraisonGratuite.Value && ddlOptionsEnvoi.SelectedIndex == 0)
            lblPrixLivraison.Text = "Gratuit";
        else
        {
            short codePoids = db.typesPoids.Values.Find(x => poidsTotal <= x.PoidsMax.Value).CodePoids.Value;
            prixLivraison = db.poidsLivraisons.Values.Find(x => x.CodeLivraison == typeLivraison && x.CodePoids == codePoids).Tarif.Value;
            lblPrixLivraison.Text = prixLivraison.ToString("N2") + "$";
        }

        decimal prixAvecLivraison = prixTotal + prixLivraison;
        decimal tps = 0;
        decimal tvq = 0;

        if (vendeur.Taxes.Value)
        {
            tps = MaxTPS() / 100;
            if (vendeur.Province == "QC")
                tvq = MaxTVQ() / 100;
        }

        decimal prixTPS = prixAvecLivraison * tps;
        lblPrixTPS.Text = prixTPS.ToString("N2") + "$";

        decimal prixTVQ = prixAvecLivraison * tvq;
        lblPrixTVQ.Text = prixTVQ.ToString("N2") + "$";

        decimal prixAvecTaxes = prixAvecLivraison + prixTPS + prixTVQ;
        lblPrixTotal.Text = prixAvecTaxes.ToString("N2") + "$";
        hidMontantTotal.Value = prixAvecTaxes.ToString();

        // Infos pour la commande
        string jsonInfos = JsonConvert.SerializeObject(new
        {
            vendeur.NoVendeur,
            CoutLivraison = prixLivraison,
            TypeLivraison = typeLivraison,
            MontantTotAvantTaxes = prixTotal,
            TPS = prixTPS,
            TVQ = prixTVQ,
            PoidsTotal = poidsTotal
        });

        hidCommande.Value = jsonInfos;
    }

    private void ShowSelectedPanier()
    {
        foreach (string pnlId in dictVendeurs.Values)
        {
            Panel pnl = (Panel)pnlPaniersGeneres.FindControl(pnlId);
            pnl.Visible = ddlVendeurs.SelectedValue == "all" || pnlId == ddlVendeurs.SelectedValue;
        }
    }

    private bool VerifierQuantiteArticles(long noVendeur)
    {
        bool ret = false;

        Client client = Session.GetClient();

        List<ArticleEnPanier> articlesEnPanier = db.articlesEnPanier.ArticlesByClientAndVendeur(client.NoClient.Value, noVendeur);

        var articlesProduits = from article in articlesEnPanier
                               join produit in db.produits.Values on article.NoProduit equals produit.NoProduit
                               select new { Article = article, Produit = produit };

        foreach(var article in articlesProduits)
        {
            if (article.Article.NbItems > article.Produit.NombreItems && article.Produit.NombreItems > 0)
            {
                lblModalText.Text += $"Le produit \"{article.Produit.Nom}\" n'est plus disponnible dans les quantités voulues ({article.Article.NbItems}).<br/>Le panier a été mis à jour ({article.Produit.NombreItems}).<br/>";
                article.Article.NbItems = article.Produit.NombreItems;
                db.articlesEnPanier.NotifyUpdatedOutside(article.Article);
                ret = true;
            }
        }

        db.articlesEnPanier.Update();

        return ret;
    }

    private void ShowModal() => exampleModal.CssClass += " show d-block";

    /*
     * Load
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsClient())
            Response.Redirect(SessionManager.RedirectConnexionLink);

        string noAutorisation = Request["NoAutorisation"];

        bool errQuantite = false;
        foreach (var art in db.articlesEnPanier.Values.Where(x => x.NoClient == Session.GetClient().NoClient).GroupBy(x => x.NoVendeur))
        {
            if (VerifierQuantiteArticles(art.Key.Value))
            {
                ShowModal();
                errQuantite = true;
            }
        }

        if (noAutorisation == null)
        {
            pnlResume.Visible = false;

            string noVendeur = Request["NoVendeur"];

            GenerePaniers();

            if (!Page.IsPostBack)
            {
                pnlPanier.Visible = true;
                RemplitDdlVendeurs();
                RemplireInfosClient();
                RemplitDdlTypesLivraison();

                if (noVendeur != null)
                {
                    ddlVendeurs.SelectedValue = $"pnlVendeur{noVendeur}";
                    ShowSelectedPanier();
                }
            }
            else
            {
                if (hidVendeur.Value != "")
                    GenererEspaceCommande(Session.GetClient());
            }
        }
        else if(!errQuantite)
        {
            pnlPanier.Visible = false;
            pnlResume.Visible = true;
            string dateAutorisation = Request["DateAutorisation"];
            string fraisMarchand = Request["FraisMarchand"];
            string infoSuppl = Request["InfoSuppl"];

            int noAut = int.Parse(noAutorisation);

            btnBreadPanier.CssClass = "btn";
            btnBreadResume.CssClass = "btn font-weight-bold";

            if (noAut == 0)
            {
                lblErrorResultat.Visible = true;
                lblErrorResultat.Text = "Commande annulé";
            }
            else if (noAut == 1)
            {
                lblErrorResultat.Visible = true;
                lblErrorResultat.Text = "Date d'expiration de la carte de crédit dépassée";
            }
            else if (noAut == 2)
            {
                lblErrorResultat.Visible = true;
                lblErrorResultat.Text = "Limite de crédit dépassée";
            }
            else if (noAut == 3)
            {
                lblErrorResultat.Visible = true;
                lblErrorResultat.Text = "Contacez le 514-626-2666";
            }
            else if (noAut == 9999)
            {
                lblErrorResultat.Visible = true;
                lblErrorResultat.Text = "Erreur(s) de validation";
            }
            else
            {
                dynamic infos = JsonConvert.DeserializeObject(infoSuppl);

                GestionPDF pdf = new GestionPDF(Session.GetClient(),
                    (long)infos.NoVendeur, (decimal)infos.CoutLivraison, (short)infos.TypeLivraison,
                    (decimal)infos.MontantTotAvantTaxes, (decimal)infos.TPS, (decimal)infos.TVQ, (decimal)infos.PoidsTotal,
                    noAut, decimal.Parse(fraisMarchand))
                    .GeneratePDF()
                    .SaveDB();

                framePDF.Src = $"/Factures/{pdf.commande.NoCommande}.pdf";
            }
        }
    }

    /*
     * Events
     */

    protected void ddlVendeurs_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVendeurs.SelectedValue == "all")
            Response.Redirect("~/Pages/Client/Panier.aspx");
        else
            Response.Redirect($"~/Pages/Client/Panier.aspx?NoVendeur={dictVendeurs.Where(x => x.Value == ddlVendeurs.SelectedValue).First().Key.NoVendeur}");
    }

    private string CurrentPage()
    {
        string ret = "";

        if (btnBreadPanier.CssClass.Contains("font"))
            ret = "Panier";
        else if (btnBreadProfil.CssClass.Contains("font"))
            ret = "Profil";
        else if (btnBreadLivraison.CssClass.Contains("font"))
            ret = "Livraison";
        else if (btnBreadPaiement.CssClass.Contains("font"))
            ret = "Paiement";
        else if (btnBreadResume.CssClass.Contains("font"))
            ret = "Resume";

        return ret;
    }

    protected void btnRetourAccueil_OnClick(object sender, EventArgs e) => Response.Redirect("~/Pages/Accueil.aspx");

    /*
     * Breadcrumbs
     */
    protected void btnBreadPanier_OnClick(object sender, EventArgs e)
    {
        //string noVendeur = Request["NoVendeur"];
        //Response.Redirect("~/Pages/Client/Panier.aspx");
        Response.Redirect(Request.RawUrl);
    }

    protected void btnBreadProfil_OnClick(object sender, EventArgs e)
    {
        if (CurrentPage() == "Livraison")
        {
            pnlConfirmation.Visible = false;

            btnBreadLivraison.PreviousBread(btnBreadProfil);
        }

        pnlProfil.Visible = true;
    }

    /* 
     * Next buttons
     */
    protected void btnCommander_OnClick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        hidVendeur.Value = btn.Attributes["data-vendeur"];
        pnlPanier.Visible = false;
        pnlProfil.Visible = true;

        btnBreadPanier.NextBread(btnBreadProfil);
    }

    protected void btnLivraison_OnClick(object sender, EventArgs e)
    {
        // Reset everything
        tbPrenom.DefaultControl();
        tbNom.DefaultControl();
        tbNoCiviqueRue.DefaultControl();
        tbVille.DefaultControl();
        ddlProvince.DefaultControl();
        tbCodePostal.DefaultControl();
        tbTelephone.DefaultControl();
        tbCellulaire.DefaultControl();

        Client client = Session.GetClient();
        bool[] arrError = new bool[]
        {
            tbPrenom.InvalidateIfEmpty(lblErrorPrenom, "Le prénom doit être présent")
                || tbPrenom.CheckFormatNomPrenom(lblErrorPrenom),
            tbNom.InvalidateIfEmpty(lblErrorNom, "Le nom doit être présent")
                || tbNom.CheckFormatNomPrenom(lblErrorNom),
            tbNoCiviqueRue.InvalidateIfEmpty(lblErrorNoCiviqueRue, "Les informations sur la rue (No Civique et Rue)")
                || tbNoCiviqueRue.CheckFormatNomPrenom(lblErrorNoCiviqueRue),
            tbVille.InvalidateIfEmpty(lblErrorVille, "Le nom de la ville doit être présent")
                || tbVille.CheckFormatNomPrenom(lblErrorVille),
            ddlProvince.InvalidateIfEmpty(lblErrorProvince, "La province doit être selectionnée"),
            tbCodePostal.InvalidateIfEmpty(lblErrorCodePostal, "Le code postal doit être présent")
                || tbCodePostal.CheckContains(lblErrorCodePostal, "Le code postal doit être entré au complet", "_"),
            tbTelephone.InvalidateIfEmpty(lblErrorTelephone, "Le numéro de téléphone doit être présent")
                || tbTelephone.CheckContains(lblErrorTelephone, "Le numéro de téléphone doit être entré au complet", "_"),
            tbCellulaire.CheckContains(lblErrorCellulaire, "Le numéro de téléphone doit être entré au complet", "_")
        };

        if (!arrError.Contains(true))
        {
            // Check if there are any differences
            bool modifications = false;
            if (client.Prenom != tbPrenom.Text)
            {
                client.Prenom = tbPrenom.Text;
                modifications = true;
            }
            if (client.Nom != tbNom.Text)
            {
                client.Nom = tbNom.Text;
                modifications = true;
            }
            if (client.Rue != tbNoCiviqueRue.Text)
            {
                client.Rue = tbNoCiviqueRue.Text;
                modifications = true;
            }
            if (client.Ville != tbVille.Text)
            {
                client.Ville = tbVille.Text;
                modifications = true;
            }
            if (client.Province != ddlProvince.SelectedValue)
            {
                client.Province = ddlProvince.SelectedValue;
                modifications = true;
            }
            if (client.CodePostal != tbCodePostal.Text)
            {
                client.CodePostal = tbCodePostal.Text;
                modifications = true;
            }
            if (client.Tel1 != tbTelephone.Text)
            {
                client.Tel1 = tbTelephone.Text;
                modifications = true;
            }
            if (client.Tel2 != tbCellulaire.Text)
            {
                client.Tel2 = tbCellulaire.Text;
                modifications = true;
            }

            if (modifications)
            {
                db.clients.NotifyUpdatedOutside(client);

                db.clients.Update();
            }

            tbPrenom.DefaultControl();
            tbNom.DefaultControl();
            pnlProfil.Visible = false;

            pnlConfirmation.Visible = true;

            btnBreadProfil.NextBread(btnBreadLivraison);

            GenererEspaceCommande(client);
        }
    }

    protected void ddlOptionsEnvois_OnChange(object sender, EventArgs e)
    {
    }
}
