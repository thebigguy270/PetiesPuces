using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Pages_Vendeur_GestionCatalogue : System.Web.UI.Page
{
    PPVendeurs vendeurs = new PPVendeurs();
    PPCategories categories = new PPCategories();
    PPVendeursClients rencontres = new PPVendeursClients();
    string couleurTexte = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        tbRecherche.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        if (!Session.IsClient())
            Response.Redirect(SessionManager.RedirectConnexionLink);


        if (!IsPostBack)
        {
            if (int.TryParse(Request.QueryString["NoVendeur"],out int noVendeur))
            {
                Vendeur vendeurSelectionne = null;
                foreach(Vendeur vendeur in vendeurs.Values)
                {
                    if (vendeur.NoVendeur == noVendeur)
                    {
                        vendeurSelectionne = vendeur;
                    }
                }
                if (vendeurSelectionne != null) {
                    string couleurFond = "";
                    if (vendeurSelectionne.Configuration != null)
                    {
                        if (vendeurSelectionne.Configuration.Contains(";"))
                        {
                            string[] liste = vendeurSelectionne.Configuration.Split(';');

                            if (liste.Length == 2)
                            {
                                if (liste[1] != "")
                                {
                                    couleurFond = liste[1];
                                }
                            }
                            else if (liste.Length == 3)
                            {
                                if (liste[1] != "")
                                {
                                    couleurFond = liste[1];
                                }

                                if (liste[2] != "")
                                {
                                    couleurTexte = liste[2];
                                }
                            }
                        }
                    }
                    if (couleurFond != "")
                    {
                        panelTable.BackColor = System.Drawing.ColorTranslator.FromHtml(couleurFond);
                    }
                }
                bool BooJourDejaLa = false;
                //Response.Write(DateTime.Now);
                foreach (VendeurClient vc in rencontres.Values.Where(r => r.NoClient.Equals(Session.GetClient().NoClient)&&r.NoVendeur==noVendeur))
                {
                    if (DateTime.Now.Date.Equals(vc.DateVisite.GetValueOrDefault(DateTime.Now).Date))
                    {
                        BooJourDejaLa = true;
                        //Response.Write(DateTime.Now.Date + "==" + vc.DateVisite.GetValueOrDefault(DateTime.Now).Date + "::" + BooJourDejaLa);
                    }
                    else
                    {
                        //Response.Write("<br/>"+vc.DateVisite.ToString());
                    }
                }
                if (!BooJourDejaLa)
                {
                    rencontres.Add(new VendeurClient(null)
                    {
                        NoVendeur = noVendeur,
                        NoClient = Session.GetClient().NoClient,
                        DateVisite = DateTime.Now
                    });
                }
            }
           
            ddlNbItemPage.Items.Add(new ListItem("5 articles", "5"));
            ddlNbItemPage.Items.Add(new ListItem("10 articles", "10"));
            ddlNbItemPage.Items.Add(new ListItem("15 articles", "15"));
            ddlNbItemPage.Items.Add(new ListItem("20 articles", "20"));
            ddlNbItemPage.Items.Add(new ListItem("25 articles", "25"));
            ddlNbItemPage.Items.Add(new ListItem("50 articles", "50"));
            ddlNbItemPage.Items.Add(new ListItem("tous articles", "0"));
            foreach(Vendeur vend in vendeurs.Values)
            {
                if (vend.Statut == 1)
                {
                    ddlNomVendeur.Items.Add(new ListItem(vend.NomAffaires, vend.NoVendeur.ToString()));
                } 
            }
            ddlNomVendeur.Items.Add(new ListItem("Tous les vendeurs", ""));

            ddlRecherche.Items.Add(new ListItem("Date de parution (aaaa-mm-jj)", "0"));
            ddlRecherche.Items.Add(new ListItem("Numéro de produit", "1"));
            ddlRecherche.Items.Add(new ListItem("Catégorie particulière de produit", "2"));
            ddlRecherche.Items.Add(new ListItem("Description du produit", "3"));



            ddlTri.Items.Add(new ListItem("Catégorie et description", "0"));
            ddlTri.Items.Add(new ListItem("Numéro de produit", "1"));
            ddlTri.Items.Add(new ListItem("Catégorie", "2"));
            ddlTri.Items.Add(new ListItem("Date de parution", "3"));

            getVariablesGET(15);
        }
    }

    private void PageVendeur(Panel panel, Vendeur vendeur=null, String categorie=null, int nbArticleParPage = 15, int page = 1,int TypeRecherche=0,string rechercher="",Boolean effectuerRecherche=false)
    {
        lblProduits.Text = vendeur==null? "Produits de tous les vendeurs": "Produits du vendeur " + vendeur.NomAffaires;
        PPProduits produits = new PPProduits();
        IEnumerable<Produit> enumProds = produits.Values.Where(p => p.NombreItems != -1 || p.Disponibilité == true);
        enumProds = vendeur == null ? enumProds : enumProds.Where(c => c.NoVendeur == vendeur.NoVendeur);
        enumProds = categorie == null ? enumProds : enumProds.Where(c => c.NoCategorie.ToString().Equals(categorie));
        if (effectuerRecherche)
        {
            switch (TypeRecherche)
            {
                case 0:
                    enumProds = enumProds.Where(p => p.DateCreation.ToString().Contains(rechercher));
                    break;
                case 1:
                    enumProds = enumProds.Where(p => p.NoProduit.ToString().Contains(rechercher));
                    break;
                case 2:
                    //la recherche s'effectue sur seulement 1 catégorie.
                    List<Categorie> cat = categories.Values.Where(c => c.Description.Contains(rechercher)).ToList();
                    if (cat.Count == 1)
                    {
                        enumProds = enumProds.Where(p => p.NoCategorie.Equals(cat.First().NoCategorie));
                    }
                    break;
                case 3:
                    enumProds = enumProds.Where(p => p.Description.Contains(rechercher));
                    break;
            }
        }
        if (int.TryParse(Request.QueryString["Tri"], out int noTri))
        {
            ddlTri.SelectedIndex = noTri;
        }
        switch (ddlTri.SelectedIndex)
        {
            case 0:
                enumProds = enumProds.OrderBy(c => c.NoCategorie).ThenBy(c=>c.Description);
                break;
            case 1:
                enumProds = enumProds.OrderBy(c => c.NoProduit);
                break;
            case 2:
                enumProds = enumProds.OrderBy(c => c.NoCategorie);
                break;
            case 3:
                enumProds = enumProds.OrderBy(c => c.DateCreation);
                break;
        }
        List <Produit> nouvelleListe = enumProds.ToList();

        int index2 = 0;
        Panel rows = panelTable.DivDyn("", "");
        Double nbPagesMax = 1;
        if (nbArticleParPage != 0)
        {
            nbPagesMax = Math.Ceiling(Double.Parse((nouvelleListe.Count() / nbArticleParPage).ToString())) + 1;
        }
        ddlNbItemPage.SelectedValue = nbArticleParPage.ToString();
        if (vendeur != null)
        {
            ddlNomVendeur.SelectedValue = vendeur.NoVendeur.ToString();
            string image = "";

            if (vendeur.Configuration != null)
            {
                if (vendeur.Configuration.Contains(";"))
                {
                    string[] liste = vendeur.Configuration.Split(';');

                    if (liste.Length == 2)
                    {
                        if (liste[0] != "")
                        {
                            image = "~/Logos/" + liste[0];
                        }


                    }
                    else if (liste.Length == 3)
                    {
                        if (liste[0] != "")
                        {
                            image = "~/Logos/" + liste[0];

                        }

                    }
                }
                else
                {
                    image = "~/Logos/" + vendeur.Configuration;
                }
            }

            if (image != "")
            {
                imgLogo.ImageUrl = image;
                imgLogo.Visible = true;
            }
        }
        else
        {
            ddlNomVendeur.SelectedValue = "";
        }
        
        for (int nbPage = 1; nbPage <= nbPagesMax; nbPage++) {
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes["class"] = "page-item "+((nbPage == page) ? "disabled" : "");
            pagination.Controls.Add(li);

            HtmlGenericControl anchor = new HtmlGenericControl("a");
            anchor.Attributes["class"] = "page-link";
            string strUrl = "/Pages/Client/Catalogue.aspx?";
            if (vendeur != null) {
                strUrl += "NoVendeur=" + vendeur.NoVendeur+ "&";
            }
            if (categorie != null)
            {
                strUrl += "NoCategorie=" + categorie + "&";
            }
            strUrl += "NbArticles=" + nbArticleParPage.ToString()+"&Page=" + nbPage.ToString();
            if (effectuerRecherche)
            {
                strUrl += "&TypeRecherche=" + Request.QueryString["TypeRecherche"] + "&Recherche=" + Request.QueryString["Recherche"];
            }
            anchor.Attributes.Add("href", strUrl + "&Tri=" + ddlTri.SelectedIndex);
            anchor.InnerText = nbPage.ToString();
            li.Controls.Add(anchor);
        }
        int nbArticleAffiche = 0;
        for (int indexItem = 0; indexItem < nouvelleListe.Count(); indexItem++)
        {
            if (nbArticleParPage==0||(indexItem>=((page-1)*nbArticleParPage)&&indexItem< (page * nbArticleParPage)))
            {

                if (nbArticleAffiche % 3 == 0)
                {
                    rows = panelTable.DivDyn("", "row");
                    index2++;
                }
                string strCategorie = "";
                foreach (Categorie cat in categories.Values)
                {
                    if (cat.NoCategorie.Equals(nouvelleListe[indexItem].NoCategorie))
                    {
                        strCategorie = cat.Description;
                        break;
                    }
                }
                Panel panelCard = rows.DivDyn("", "col-fhd-4 mb-3");
                Panel card = panelCard.DivDyn("", "card");
                Panel panelTitreCategories = card.DivDyn("", "card-header fake-button");
                Table tableT = panelTitreCategories.TableDyn("", "");
                TableRow rowt = tableT.TrDyn();
                TableCell cell1 = rowt.TdDyn();
                cell1.ImgDyn("", "~/Pictures/" + nouvelleListe[indexItem].Photo, "imgResize");
                TableCell cell2 = rowt.TdDyn();
                cell2.LblDyn("", " "+nouvelleListe[indexItem].Nom, "card-title h6");
                cell2.LblDyn("", " (#"+nouvelleListe[indexItem].NoProduit.ToString()+")  <br/>Catégorie: "+strCategorie , "card-title h6");
                if (couleurTexte != "")
                {
                    cell2.ForeColor = System.Drawing.ColorTranslator.FromHtml(couleurTexte);
                }
                bool EstEnRupture = nouvelleListe[indexItem].NombreItems == 0 ? true : false;
                if (EstEnRupture)
                {
                    cell2.LblDyn("", " Rupture de stock!", "card-title h6 text-uppercase font-weight-bold text-danger");
                }
                panelTitreCategories.Attributes.Add("data-toggle", "collapse");
                panelTitreCategories.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapsePanier" + indexItem.ToString() + index2.ToString());
                panelTitreCategories.Attributes.Add("aria-expanded", "false");
                panelTitreCategories.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapsePanier" + indexItem.ToString() + index2.ToString());

                Panel collapsable;
                //body des categories

                collapsable = card.DivDyn("collapsePanier" + indexItem.ToString() + index2.ToString(), "collapse collapse");

                //colones header
                Panel panelBodyCat = collapsable.DivDyn("", "card-body");

                HyperLink hlink = new HyperLink();
                hlink.NavigateUrl = "/Pages/Client/InfoProduit.aspx?ID=" + nouvelleListe[indexItem].NoProduit;
                hlink.Text = "Détails du produit";
                hlink.Attributes.Add("role", "button");
                hlink.CssClass = "btn btn-info w-100";
                panelBodyCat.Controls.Add(hlink);
                //panelBodyCat.LblDyn("", "Catégorie: " + strCategorie);
                Table table = panelBodyCat.TableDyn("", "table");
                TableHeaderRow rowTable = table.ThrDyn();


                rowTable.ThdDyn("Date de parution");
                rowTable.ThdDyn("Poids");
                rowTable.ThdDyn("Prix");
                rowTable.ThdDyn("Qte");

                TableRow rowTableB = table.TrDyn();

                rowTableB.TdDyn(nouvelleListe[indexItem].DateCreation.GetValueOrDefault().Date.ToShortDateString());
                rowTableB.TdDyn(nouvelleListe[indexItem].Poids.Value.ToString("N2") + " Lbs");
                rowTableB.TdDyn(nouvelleListe[indexItem].PrixDemande.Value.ToString("N2") + " $");
                rowTableB.TdDyn(nouvelleListe[indexItem].NombreItems.Value.ToString());

                if (!EstEnRupture)
                {
                    TableRow rowTableC = table.TrDyn();

                    TableCell tbcell1 = rowTableC.TdDyn();
                    tbcell1.LblDyn("", "Quantité à commander: ");
                    TableCell tbcell2 = rowTableC.TdDyn();
                    DropDownList ddl = new DropDownList();
                    for (int i = 0; i < nouvelleListe[indexItem].NombreItems.Value; i++)
                    {
                        ddl.Items.Add((i + 1).ToString());
                    }
                    ddl.SelectedIndex = 0;
                    ddl.CssClass = "form-control";
                    tbcell2.Controls.Add(ddl);
                    TableCell tbcell3 = rowTableC.TdDyn();

                    Button btn = tbcell3.BtnClientDyn("", "Ajouter au panier", $"ajouterArticle({Session.GetClient().NoClient},{nouvelleListe[indexItem].NoProduit},'{ddl.ClientID}');return false;", "btn btn-success");
                    btn.Attributes.Add("data-toggle", "modal");
                    btn.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_myModal" + indexItem.ToString() + index2.ToString());
                }
                nbArticleAffiche++;
            }
        }
        if (nouvelleListe.Count() == 0)
        {
            panelTable.TbDyn("", "Il n'y a pas de produit qui correspond aux critères de recherche",100, "w-100 text-center");
        }

    }
    protected void getVariablesGET(int nbArticlesParPage)
    {
        String noVendeur = Request.QueryString["NoVendeur"];
        String cat = Request.QueryString["NoCategorie"];
        int intPage = 1; 
        int TypeRecherche = 0;
        string strRecherche = "";
        bool effectuerUneRecherche = false;
        if (Request.QueryString["NbArticles"] != null)
        {
            if (!Request.QueryString["NbArticles"].Trim().Equals(""))
                int.TryParse(Request.QueryString["NbArticles"],out nbArticlesParPage); 
        }
        if (Request.QueryString["Page"] != null)
        {
            int.TryParse(Request.QueryString["Page"],out intPage);
        }
        if (Request.QueryString["TypeRecherche"] != null)
        {
           if (int.TryParse(Request.QueryString["TypeRecherche"], out TypeRecherche))
            {
                if (TypeRecherche >= 0 && TypeRecherche <= 3)
                {
                    if (Request.QueryString["Recherche"] != null)
                    {
                        strRecherche = Request.QueryString["Recherche"];
                        effectuerUneRecherche = true;
                    }
                }
            }
        }

        Vendeur vendeur = null;
        foreach (Vendeur vend in vendeurs.Values)
        {
            if (vend.NoVendeur.ToString().Equals(noVendeur))
            {
                vendeur = vend;
            }
        }
        panelTable.Controls.Clear();
        pagination.Controls.Clear();
        //Response.Write(nbArticlesParPage.ToString());
        if (effectuerUneRecherche)
        {
            PageVendeur(panelTable, vendeur, cat, nbArticlesParPage, intPage,TypeRecherche,strRecherche,true);
        }
        else
        {
            PageVendeur(panelTable, vendeur, cat, nbArticlesParPage, intPage);
        }
        
    }
    protected void ChangementNbArticlePage(object sender, EventArgs e)
    {
        Response.Redirect("/Pages/Client/Catalogue.aspx?NoVendeur=" + Request.QueryString["NoVendeur"] + "&NbArticles=" + ddlNbItemPage.SelectedValue + "&Page=1&TypeRecherche=" + ddlRecherche.SelectedValue + "&Recherche=" + tbRecherche.Text);
    }
    protected void ChangementVendeur(object sender, EventArgs e)
    {

        Response.Redirect("/Pages/Client/Catalogue.aspx?NoVendeur=" + ddlNomVendeur.SelectedValue + "&NbArticles=" + Request.QueryString["NbArticles"] + "&Page=1&TypeRecherche=" + ddlRecherche.SelectedValue + "&Recherche=" + tbRecherche.Text);
    }

    protected void Rechercher(object sender, EventArgs e)
    {
        Response.Redirect("/Pages/Client/Catalogue.aspx?NoVendeur=" + Request.QueryString["NoVendeur"] + "&NbArticles=" + Request.QueryString["NbArticles"] + "&Page=1&TypeRecherche="+ddlRecherche.SelectedValue+"&Recherche="+tbRecherche.Text+"&Tri="+ddlTri.SelectedIndex);
    }
}