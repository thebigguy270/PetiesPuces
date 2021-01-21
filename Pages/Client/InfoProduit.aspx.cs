using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Client_InfoProduit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsClient())
            Response.Redirect(SessionManager.RedirectConnexionLink);
        if (!IsPostBack)
        {
            var url = Request.UrlReferrer;
            if (url != null)
                ViewState["PreviousPageUrl"] = url.ToString();
        }
        long produitARG;
        // se faire passer la valeur du produit par ce tag [ID] i.e : ?ID=noProduit
        if (long.TryParse(Request.QueryString["ID"], out produitARG)){
            //long produitARG = 1200001;
            PPProduits produits = new PPProduits();
            Produit monProduit = new Produit(null);
            foreach (Produit prod in produits.Values.Where(x => x.NoProduit == (long?)produitARG))
            {
                monProduit = produits.Values.Where(x => x.NoProduit == (long?)produitARG).ToList().First();
            }

            if (monProduit.NoProduit != null)
            {
                panelImage.Visible = true;
                Image img = panelImage.ImgDyn("", "~/Pictures/" + monProduit.Photo, "img-fluid");


                Table table = panelDetails.TableDyn("", "table table-striped");
                TableRow rowDet = table.TrDyn();
                TableCell colDet1 = rowDet.TdDyn();
                TableCell colDet2 = rowDet.TdDyn();

                //nom
                colDet1.LblDyn("", "Nom du produit", "text-info h3");
                colDet2.LblDyn("", monProduit.Nom, "h3 text-success");

                PPCategories categos = new PPCategories();
                Categorie categ = new Categorie(null);
                foreach (Categorie cat in categos.Values.Where(x => x.NoCategorie == monProduit.NoCategorie))
                {
                    categ = cat;
                }
                if (categ.NoCategorie != null)
                {
                    //Catégorie
                    rowDet = table.TrDyn();
                    colDet1 = rowDet.TdDyn();
                    colDet2 = rowDet.TdDyn();

                    colDet1.LblDyn("", "Catégorie", "text-info h5");
                    colDet2.LblDyn("", categ.Description, "h5");

                    //Description
                    rowDet = table.TrDyn();
                    colDet1 = rowDet.TdDyn();
                    colDet2 = rowDet.TdDyn();

                    colDet1.LblDyn("", "Description du produit", "text-info h5");
                    colDet2.LblDyn("", monProduit.Description, "h5");

                    //Prix demandé
                    rowDet = table.TrDyn();
                    colDet1 = rowDet.TdDyn();
                    colDet2 = rowDet.TdDyn();

                    colDet1.LblDyn("", "Prix demandé", "text-info h5");
                    colDet2.LblDyn("", monProduit.PrixDemande.Value.ToString("N2") + " $", "h5");

                    //Prix Vente
                    rowDet = table.TrDyn();
                    colDet1 = rowDet.TdDyn();
                    colDet2 = rowDet.TdDyn();

                    colDet1.LblDyn("", "Prix de vente", "text-info h5");
                    if (monProduit.PrixVente.Value != monProduit.PrixDemande.Value)
                    {
                        colDet2.LblDyn("", monProduit.PrixVente.Value.ToString("N2") + " $", "h5");
                    }
                    else
                    {
                        colDet2.LblDyn("", "Pas en vente", "h5");
                    }

                    //qte
                    rowDet = table.TrDyn();
                    colDet1 = rowDet.TdDyn();
                    colDet2 = rowDet.TdDyn();

                    colDet1.LblDyn("", "Quantité disponible", "text-info h5");
                    if (monProduit.NombreItems == 0)
                    {
                        colDet2.LblDyn("", "Aucun article disponible", "h5");
                    }
                    else
                    {
                        colDet2.LblDyn("", monProduit.NombreItems.ToString(), "h5");
                    }


                    //Poids
                    rowDet = table.TrDyn();
                    colDet1 = rowDet.TdDyn();
                    colDet2 = rowDet.TdDyn();

                    colDet1.LblDyn("", "Poids du produit", "text-info h5");
                    colDet2.LblDyn("", monProduit.Poids.Value.ToString("N2") + " Lbs", "h5");

                    //Date de vente
                    rowDet = table.TrDyn();
                    colDet1 = rowDet.TdDyn();
                    colDet2 = rowDet.TdDyn();

                    string dateVente = "indéfénie";
                    if (monProduit.DateVente.HasValue)
                    {
                        dateVente = monProduit.DateVente.Value.ToString("yyyy/MM/dd");
                    }

                    colDet1.LblDyn("", "Fin de la vente", "text-info h5");
                    colDet2.LblDyn("", dateVente, "h5");

                    if (monProduit.NombreItems != 0)
                    {
                        rowDet = table.TrDyn();

                        colDet1 = rowDet.TdDyn();
                        colDet1.LblDyn("", "Quantité à commander: ", "text-info h5");
                        colDet2 = rowDet.TdDyn();
                        DropDownList ddl = new DropDownList();
                        for (int i = 0; i < monProduit.NombreItems.Value; i++)
                        {
                            ddl.Items.Add((i + 1).ToString());
                        }
                        ddl.SelectedIndex = 0;
                        ddl.CssClass = "form-control w-25 d-inline align-middle mr-3";
                        colDet2.Controls.Add(ddl);
                        TableCell colDet3 = rowDet.TdDyn();
                        Button btn = colDet2.BtnClientDyn("", "Ajouter au panier", $"ajouterArticle({Session.GetClient().NoClient},{monProduit.NoProduit},'{ddl.ClientID}');return false;", "btn btn-success d-inline");
                    }
                    panelDetails.BtnDyn("", "Retour", retour, "btn  btn-outline-dark classBoutonsMargins100");


                    Button btnContactV = panelDetails.BtnDyn("", "Contacter le vendeur", btnCourrielVendeur_Click, "btn btn-secondary classBoutonsMargins100");
                    btnContactV.CommandArgument = monProduit.NoVendeur.ToString();
                    btnContactV.CommandName = monProduit.Nom;
                }
            }
        }
    }
    protected void retour(object sender, EventArgs e)
    {
        Session["fileUpload"] = null;
        var previous = ViewState["PreviousPageUrl"];
        if (previous != null)
            Response.Redirect(previous.ToString());
        else
            Response.Redirect("~/Pages/Accueil.aspx");
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

        sb.AppendFormat("<input type='hidden' name='sujet' value='{0}'>", btn.CommandName);
        sb.AppendFormat("<input type='hidden' name='contenu' value='Contenu du message'>");
        sb.Append("</form>");
        sb.Append("</body>");
        sb.Append("</html>");
        Response.Write(sb.ToString());
        Response.End();
    }


}