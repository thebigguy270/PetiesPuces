using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Vendeur_InscriptionProduit : System.Web.UI.Page
{

    TextBox tbNom;
    TextBox tb;
    FileUpload fileUpload;
    Label lbl; //le lbl du file upload
    TextBox tbPrix;
    TextBox tbQte;
    TextBox tbPoids;
    TextBox tbPrixV;
    RadioButtonList rbList;
    TextBox tbDate;
    DropDownList ddlDesc;
    Vendeur vendeur;
    Label lblErrNom;
    Label lblErrPrix;
    Label lblErrPrixV;
    Label lblErrQte;
    Label lblErrPoids;
    Label lblErrDate;
    Label lblTele;
    Panel pn2;
    Button btnCollapse;
    Panel collapsable;
    Panel panelBodyCat;
    Panel group;
    Panel prepend;
    Panel customFile;
    ListItem rb1;
    ListItem rb2;
    Produit monProduit_Modif;
    protected void Page_Load(object sender, EventArgs e)
    {

        

        if (!Session.IsVendeur())
            Response.Redirect(SessionManager.RedirectConnexionLink);

       if (!IsPostBack)
        {
            var url = Request.UrlReferrer;
            if (url != null)
                ViewState["PreviousPageUrl"] = url.ToString();

        }

        vendeur = Session.GetVendeur();
        string affichage = Request.QueryString["T"];
        switch (affichage)
        {
            case "ajout":
                //type
                Panel row = panel.DivDyn("", "row mb-3");
                Panel col1 = row.DivDyn("", "col-6");
                Panel col2 = row.DivDyn("", "col-6");

               

                col1.LblDyn("", "Catégorie du produit : ", "h4 text-info");
                ddlDesc = new DropDownList();
                ddlDesc.CssClass = "form-control";
                col2.Controls.Add(ddlDesc);

                //populer le ddl
                if (!IsPostBack)
                {
                    PPCategories categories = new PPCategories();
                    List<Categorie> listeCate = categories.Values.OrderBy(x => x.Description).ToList();
                    foreach (var catego in listeCate)
                    {
                        ddlDesc.Items.Add(new ListItem(catego.Description, catego.NoCategorie.ToString()));
                    }
                }



                //nom
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Nom du produit : ", "h4 text-info");
                tbNom = col2.TbDyn("", "", 50, "form-control");
                tbNom.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrNom = pn2.LblDyn("", "", "");

                //informations
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Description : ", "h4 text-info");
                btnCollapse = col2.BtnDyn("", "Cliquez ici", null, "form-control");

                btnCollapse.OnClientClick = "return false;";
                btnCollapse.Attributes.Add("data-toggle", "collapse");
                btnCollapse.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapse");
                btnCollapse.Attributes.Add("aria-expanded", "false");
                btnCollapse.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapse");

                collapsable = col2.DivDyn("collapse", "collapse collapse");
                panelBodyCat = collapsable.DivDyn("", "");

                tb = panelBodyCat.TbDyn("", "", 65535, "form-control");
                tb.TextMode = TextBoxMode.MultiLine;
                tb.Height = 250;

                //Image 
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Image du produit : ", "h4 text-info");
                group = col2.DivDyn("", "input-group");
                prepend = group.DivDyn("", "input-group-prepend");
                lblTele = prepend.LblDyn("", "Téléverser", "input-group-text");
                customFile = group.DivDyn("", "custom-file");
                fileUpload = new FileUpload();
                fileUpload.CssClass = "custom-file-input";
                fileUpload.ID = "inputGroupFile02";
                fileUpload.Attributes.Add("aria-describedby", "fileHelp");
                customFile.Controls.Add(fileUpload);
                lbl = customFile.LblDyn("lbl", "Aucun fichier", "custom-file-label h6");

                if (IsPostBack)
                {
                    if (Session["fileUpload"] == null && fileUpload.HasFile)
                    {
                        Session["fileUpload"] = fileUpload;
                        lbl.Text = fileUpload.FileName;
                    }
                    // Next time submit and Session has values but FileUpload is Blank
                    // Return the values from session to FileUpload
                    else if (Session["fileUpload"] != null && (!fileUpload.HasFile))
                    {
                        fileUpload = (FileUpload) Session["fileUpload"];
                        lbl.Text = fileUpload.FileName;
                    }
                    // Now there could be another sictution when Session has File but user want to change the file
                    // In this case we have to change the file in session object
                    else if (fileUpload.HasFile)
                    {
                        Session["fileUpload"] = fileUpload;
                        lbl.Text = fileUpload.FileName;
                    }
                }

                //Prix demandé
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Prix demandé : ", "h4 text-info");
                tbPrix = col2.TbDyn("", "");
                tbPrix.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbPrix.TextMode = TextBoxMode.Number;
                tbPrix.CssClass = "form-control";
                tbPrix.Attributes["min"]= "0";
                tbPrix.Attributes["max"] = "200000";
                tbPrix.Attributes["step"] = "0.01";

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrPrix = pn2.LblDyn("", "", "");

                //Prix Vente
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Prix de vente (si rabais) : ", "h4 text-info");
                tbPrixV = col2.TbDyn("", "");
                tbPrixV.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbPrixV.TextMode = TextBoxMode.Number;
                tbPrixV.CssClass = "form-control";
                tbPrixV.Attributes["min"] = "0";
                tbPrixV.Attributes["max"] = "200000";
                tbPrixV.Attributes["step"] = "0.01";

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrPrixV = pn2.LblDyn("", "", "");

                //Nombre d'items;
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Quantité disponnible : ", "h4 text-info");
                tbQte = col2.TbDyn("", "");
                tbQte.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbQte.TextMode = TextBoxMode.Number;
                tbQte.CssClass = "form-control";
                tbQte.Attributes["min"] = "0";
                tbQte.Attributes["max"] = "30000";

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrQte = pn2.LblDyn("", "", "");

                //Poids;
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Poids : ", "h4 text-info");
                tbPoids = col2.TbDyn("", "");
                tbPoids.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbPoids.TextMode = TextBoxMode.Number;
                tbPoids.CssClass = "form-control";
                tbPoids.Attributes["min"] = "0";
                tbPoids.Attributes["max"] = "999999.9";
                tbPoids.Attributes["step"] = "0.1";

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrPoids = pn2.LblDyn("", "", "");

                //Dispônnibilité
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Disponibilité : ", "h4 text-info");

                rbList = new RadioButtonList();
                rbList.RepeatDirection = RepeatDirection.Horizontal;
                rbList.CssClass = "d-flex justify-content-center h5";
                rbList.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                rb1 = new ListItem();
                ListItem rb2 = new ListItem();
                rb1.Attributes["class"] = "mr-3 ml-3";
                rb1.Text = "Disponible";
                rb2.Text = "Non disponible";  
                rbList.Items.Add(rb1);
                rbList.Items.Add(rb2);
                col2.Controls.Add(rbList);
                if (!IsPostBack)
                {
                    rb1.Selected = true;
                }

                //Date de vente
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Date de la fin de la vente (si rabais) : ", "h4 text-info");
                tbDate = col2.TbDyn("", "");
                tbDate.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbDate.TextMode = TextBoxMode.Date;
                tbDate.CssClass = "form-control";

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrDate = pn2.LblDyn("", "", "");

                //btn d'options
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.BtnDyn("", "Confirmer", click_confirmer, "btn btn-outline-secondary classBoutonsMargins100");
                col2.BtnDyn("", "Annuler", retour, "btn btn-outline-dark classBoutonsMargins100");

                break;
            case "modification":
                long produitARG_Modif = long.Parse(Request.QueryString["ID"]);

                PPProduits produits_Modif = new PPProduits();

                if (produits_Modif.Values.Where(x => x.NoProduit == (long?)produitARG_Modif && x.NoVendeur == vendeur.NoVendeur).ToList().Count() > 0)
                {
                    monProduit_Modif = produits_Modif.Values.Where(x => x.NoProduit == (long?)produitARG_Modif && x.NoVendeur == vendeur.NoVendeur).ToList().First();
                }
                else
                {
                    Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");
                }

                

                panelImage.Visible = true;
                Image imgM = panelImage.ImgDyn("", "~/Pictures/" + monProduit_Modif.Photo, "img-fluid");
                panelImage.LblDyn("", "image actuelle", "text-info h5 d-flex justify-content-center");
                panelImage.LblDyn("", "vous verrez les modifications après enregistrement", "text-secondary h6 d-flex justify-content-center");

                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

               

                col1.LblDyn("", "Catégorie du produit : ", "h4 text-info");
                ddlDesc = new DropDownList();
                ddlDesc.CssClass = "form-control";
                col2.Controls.Add(ddlDesc);

                //populer le ddl
                if (!IsPostBack)
                {
                    PPCategories categories = new PPCategories();
                    List<Categorie> listeCate = categories.Values.OrderBy(x => x.Description).ToList();
                    foreach (var catego in listeCate)
                    {
                        ddlDesc.Items.Add(new ListItem(catego.Description, catego.NoCategorie.ToString()));
                    }
                    ddlDesc.SelectedValue = monProduit_Modif.NoCategorie.ToString();
                }

                //nom
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Nom du produit : ", "h4 text-info");
                tbNom = col2.TbDyn("", "", 50, "form-control");
                tbNom.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbNom.Text = monProduit_Modif.Nom;
                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrNom = pn2.LblDyn("", "", "");

                //informations
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Description : ", "h4 text-info");
                btnCollapse = col2.BtnDyn("", "Cliquez ici", null, "form-control");

                btnCollapse.OnClientClick = "return false;";
                btnCollapse.Attributes.Add("data-toggle", "collapse");
                btnCollapse.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapse");
                btnCollapse.Attributes.Add("aria-expanded", "false");
                btnCollapse.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapse");

                collapsable = col2.DivDyn("collapse", "collapse collapse");
                panelBodyCat = collapsable.DivDyn("", "");

                tb = panelBodyCat.TbDyn("", "", 65535, "form-control");
                tb.TextMode = TextBoxMode.MultiLine;

                if (monProduit_Modif.Description != "N/A")
                {
                    tb.Text = monProduit_Modif.Description;
                }
                
                tb.Height = 250;

                //Image 
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Image du produit : ", "h4 text-info");
                group = col2.DivDyn("", "input-group");
                prepend = group.DivDyn("", "input-group-prepend");
                lblTele = prepend.LblDyn("", "Téléverser", "input-group-text");
                customFile = group.DivDyn("", "custom-file");
                fileUpload = new FileUpload();
                fileUpload.CssClass = "custom-file-input";
                fileUpload.ID = "inputGroupFile02";
                fileUpload.Attributes.Add("aria-describedby", "fileHelp");
                customFile.Controls.Add(fileUpload);

                if (monProduit_Modif.Photo != null)
                {
                    lbl = customFile.LblDyn("lbl", monProduit_Modif.Photo, "custom-file-label h6");
                }
                else
                {
                    lbl = customFile.LblDyn("lbl", "Aucun fichier", "custom-file-label h6");
                }
                

                if (IsPostBack)
                {
                    if (Session["fileUpload"] == null && fileUpload.HasFile)
                    {
                        Session["fileUpload"] = fileUpload;
                        lbl.Text = fileUpload.FileName;
                    }
                    // Next time submit and Session has values but FileUpload is Blank
                    // Return the values from session to FileUpload
                    else if (Session["fileUpload"] != null && (!fileUpload.HasFile))
                    {
                        fileUpload = (FileUpload)Session["fileUpload"];
                        lbl.Text = fileUpload.FileName;
                    }
                    // Now there could be another sictution when Session has File but user want to change the file
                    // In this case we have to change the file in session object
                    else if (fileUpload.HasFile)
                    {
                        Session["fileUpload"] = fileUpload;
                        lbl.Text = fileUpload.FileName;
                    }
                }

                //Prix demandé
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Prix demandé : ", "h4 text-info");
                tbPrix = col2.TbDyn("", "");
                tbPrix.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbPrix.TextMode = TextBoxMode.Number;
                tbPrix.CssClass = "form-control";
                tbPrix.Attributes["min"] = "0";
                tbPrix.Attributes["step"] = "0.01";
                tbPrix.Attributes["max"] = "200000";
                tbPrix.Text = monProduit_Modif.PrixDemande.Value.ToString("N2").Replace(",", "");

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrPrix = pn2.LblDyn("", "", "");

                //Prix Vente
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Prix de vente (si rabais) : ", "h4 text-info");
                tbPrixV = col2.TbDyn("", "");
                tbPrixV.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbPrixV.TextMode = TextBoxMode.Number;
                tbPrixV.CssClass = "form-control";
                tbPrixV.Attributes["min"] = "0";
                tbPrixV.Attributes["step"] = "0.01";
                tbPrixV.Attributes["max"] = "200000";
                if (monProduit_Modif.PrixVente.Value != monProduit_Modif.PrixDemande.Value)
                {
                    tbPrixV.Text = monProduit_Modif.PrixVente.Value.ToString("N2").Replace(",", "");
                }

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrPrixV = pn2.LblDyn("", "", "");
                //Nombre d'items;
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Quantité disponnible : ", "h4 text-info");
                tbQte = col2.TbDyn("", "");
                tbQte.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbQte.TextMode = TextBoxMode.Number;
                tbQte.CssClass = "form-control";
                tbQte.Attributes["min"] = "0";
                tbQte.Attributes["max"] = "30000";
                tbQte.Text = monProduit_Modif.NombreItems.Value.ToString();

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrQte = pn2.LblDyn("", "", "");

                //Poids;
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Poids : ", "h4 text-info");
                tbPoids = col2.TbDyn("", "");
                tbPoids.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbPoids.TextMode = TextBoxMode.Number;
                tbPoids.CssClass = "form-control";
                tbPoids.Attributes["min"] = "0";
                tbPoids.Attributes["max"] = "999999.9";
                tbPoids.Attributes["step"] = "0.1";
                
                tbPoids.Text = monProduit_Modif.Poids.Value.ToString();

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrPoids = pn2.LblDyn("", "", "");

                //Dispônnibilité
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Disponibilité : ", "h4 text-info");

                rbList = new RadioButtonList();
                rbList.RepeatDirection = RepeatDirection.Horizontal;
                rbList.CssClass = "d-flex justify-content-center h5";
                rbList.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                rb1 = new ListItem();
                rb2 = new ListItem();
                rb1.Attributes["class"] = "mr-3 ml-3";
                rb1.Text = "Disponible";
                rb2.Text = "Non disponible";
                rbList.Items.Add(rb1);
                rbList.Items.Add(rb2);
                col2.Controls.Add(rbList);

                if (monProduit_Modif.Disponibilité.Value)
                {
                    rb1.Selected = true;
                }
                else
                {
                    rb2.Selected = true;
                }

                //Date de vente
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.LblDyn("", "Date de la fin de la vente (si rabais) : ", "h4 text-info");
                tbDate = col2.TbDyn("", "");
                tbDate.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                tbDate.TextMode = TextBoxMode.Date;
                tbDate.CssClass = "form-control";

                if (monProduit_Modif.DateVente.HasValue)
                {
                    tbDate.Text = monProduit_Modif.DateVente.Value.ToString("yyyy-MM-dd");
                }

                pn2 = col2.DivDyn("", "invalid-feedback");
                lblErrDate = pn2.LblDyn("", "", "");

                //btn d'options
                row = panel.DivDyn("", "row mb-3");
                col1 = row.DivDyn("", "col-6");
                col2 = row.DivDyn("", "col-6");

                col1.BtnDyn("", "Confirmer", click_confirmer_Modif, "btn btn-outline-secondary classBoutonsMargins100");
                col2.BtnDyn("", "Annuler", retour, "btn btn-outline-dark classBoutonsMargins100");

                break;
            case "details":
                long produitARG = long.Parse(Request.QueryString["ID"]);
                PPProduits produits = new PPProduits();
                Produit monProduit = null; //= produits.Values.Where(x => x.NoProduit == (long?) produitARG).ToList().First();


                if (produits.Values.Where(x => x.NoProduit == (long?)produitARG&& x.NoVendeur == vendeur.NoVendeur).ToList().Count() > 0)
                {
                    monProduit = produits.Values.Where(x => x.NoProduit == (long?)produitARG && x.NoVendeur == vendeur.NoVendeur).ToList().First();
                }
                else
                {
                    Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");
                }



                panelImage.Visible = true;
                Image img = panelImage.ImgDyn("", "~/Pictures/" + monProduit.Photo, "img-fluid");


                Table table = panel.TableDyn("", "table table-striped");
                TableRow rowDet = table.TrDyn();
                TableCell colDet1 = rowDet.TdDyn();
                TableCell colDet2 = rowDet.TdDyn();

                //nom
                colDet1.LblDyn("", "Nom du produit", "text-info h3");
                colDet2.LblDyn("", monProduit.Nom, "h3 text-success");

                PPCategories categos = new PPCategories();
                Categorie categ = categos.Values.Where(x => x.NoCategorie == monProduit.NoCategorie).ToList().First();

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

                //Prix
                rowDet = table.TrDyn();
                colDet1 = rowDet.TdDyn();
                colDet2 = rowDet.TdDyn();

                colDet1.LblDyn("", "Prix demandé", "text-info h5");
                colDet2.LblDyn("", monProduit.PrixDemande.Value.ToString("N2") + " $", "h5");

                //Prix 2
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
                colDet2.LblDyn("", monProduit.NombreItems.ToString(), "h5");

                //Poids
                rowDet = table.TrDyn();
                colDet1 = rowDet.TdDyn();
                colDet2 = rowDet.TdDyn();

                colDet1.LblDyn("", "Poids du produit", "text-info h5");
                colDet2.LblDyn("", monProduit.Poids.Value.ToString("N2") + " Lbs", "h5");

                //Disponibilité
                rowDet = table.TrDyn();
                colDet1 = rowDet.TdDyn();
                colDet2 = rowDet.TdDyn();

                string dispo = "Non";
                if (monProduit.Disponibilité.Value)
                {
                    dispo = "Oui";
                }
                colDet1.LblDyn("", "Disponible", "text-info h5");
                colDet2.LblDyn("", dispo, "h5");

                //Date de vente
                rowDet = table.TrDyn();
                colDet1 = rowDet.TdDyn();
                colDet2 = rowDet.TdDyn();

                string dateVente = "N/A";
                if (monProduit.DateVente.HasValue)
                {
                    dateVente = monProduit.DateVente.Value.ToString("yyyy/MM/dd");
                }

                colDet1.LblDyn("", "Date de mise en vente", "text-info h5");
                colDet2.LblDyn("", dateVente, "h5");

                //Date de vente
                rowDet = table.TrDyn();
                colDet1 = rowDet.TdDyn();
                colDet2 = rowDet.TdDyn();

                colDet1.LblDyn("", "Date de création", "text-info h5");
                colDet2.LblDyn("", monProduit.DateCreation.Value.ToString("yyyy/MM/dd"), "h5");

                //Date de vente
                rowDet = table.TrDyn();
                colDet1 = rowDet.TdDyn();
                colDet2 = rowDet.TdDyn();

                colDet1.LblDyn("", "Date de mise à jour", "text-info h5");
                colDet2.LblDyn("", monProduit.DateMAJ.Value.ToString("yyyy/MM/dd"), "h5");

                
                panel.BtnDyn("", "Retour", retour, "btn btn-outline-dark classBoutonsMargins100");

                Button btn = panel.BtnDyn("", "Supprimer", null, "btn btn-outline-danger classBoutonsMargins100 mt-3");

                btn.OnClientClick = "return false;";
                btn.Attributes.Add("data-toggle", "modal");
                btn.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_myModal");
                //Regarder ici si dans panier, alors loader le bon modal.
                bool panier = false;
                PPArticlesEnPanier paniers = new PPArticlesEnPanier();
                List<ArticleEnPanier> trouverSiPanier = paniers.Values.Where(x => x.NoProduit == monProduit.NoProduit).ToList();
                if (trouverSiPanier.Count() != 0)
                {
                    //Dans panier
                    panier = true;
                }


                if (!panier)
                {
                    modal(panelAvecModal, "myModal", monProduit.NoProduit.ToString()/*l'ID du btn est le noProduit*/, "idBtnAnnuler", "Supression", "Voulez vous supprimer ce produit : " + monProduit.Nom, true);
                }
                else
                {
                    modal(panelAvecModal, "myModal", monProduit.NoProduit.ToString()/*l'ID du btn est le noProduit*/, "idBtnAnnuler", "Attention!", "Ce produit (" + monProduit.Nom + ") est dans au moins un panier, voulez-vous vraiment le supprimer?", true);
                }

                break;

        }
    }

    protected bool validationFichier(string source)
    {
        bool retour = false;
        if (source == "ajout")
        {
            if (lbl.Text.Equals("Aucun fichier") || Path.GetFileName(fileUpload.FileName).Split('.')[1] != "jpg" && Path.GetFileName(fileUpload.FileName).Split('.')[1].ToUpper() != "PNG" && Path.GetFileName(fileUpload.FileName).Split('.')[1].ToUpper() != "JPEG")
            {
                lbl.CssClass = "text-danger custom-file-label h6";
                lblTele.CssClass = "text-danger input-group-text";
                retour = true;
            }
            else
            {
                lbl.CssClass = "custom-file-label h6";
                lblTele.CssClass = "input-group-text";
                retour = false;
            }
        }
        else if (source == "modif")
        {
            if (lbl.Text != monProduit_Modif.Photo)
            {
                if (Path.GetFileName(fileUpload.FileName).Split('.')[1] != "jpg" && Path.GetFileName(fileUpload.FileName).Split('.')[1].ToUpper() != "PNG" && Path.GetFileName(fileUpload.FileName).Split('.')[1].ToUpper() != "JPEG")
                {
                    lbl.CssClass = "text-danger custom-file-label h6";
                    lblTele.CssClass = "text-danger input-group-text";
                    retour = true;
                }
                    
            }
        }
        
        return retour;
    }

    protected bool ValidationDeDate()
    {
        bool retour = false;

        if (tbDate.Text.Trim() != "" && tbPrixV.Text.Trim() != "")
        {
            if (DateTime.Parse(tbDate.Text) < DateTime.Today)
            {
                tbDate.Invalidate();
                lblErrDate.Text = "La date ne peut être inférieure à celle d'aujourd'hui";
                retour = true;
            }
        }else if (tbDate.Text.Trim() != "" && tbPrixV.Text.Trim() == "")
        {
            tbDate.Invalidate();
            lblErrDate.Text = "Si vous entrez une date, vous devez avoir un prix de vente";
            retour = true;
        }else if (tbDate.Text.Trim() == "" && tbPrixV.Text.Trim() != "")
        {
            tbDate.Invalidate();
            lblErrDate.Text = "Vous devez avoir une date de vente si vous avez un prix de vente";
            retour = true;
        }
        return retour;
    }

    protected bool validationRabais()
    {
        bool retour = false;
        if (tbPrixV.Text != "")
        {
            if (decimal.Parse(tbPrixV.Text) >= decimal.Parse(tbPrix.Text))
            {
                tbPrixV.Invalidate();
                lblErrPrixV.Text = "Le prix avec rabais doit être plus bas que le montant demandé";
                retour = true;

            }
        }
        return retour;
    }

    protected bool validationNomProduit(string nom)
    {
        bool retour = false;

        if (!Regex.IsMatch(nom, ""))
        {
            retour = true;
        }



        return retour;
    }

    protected void click_confirmer(object sender, EventArgs e)
    {
        tbNom.DefaultControl();
        //Session["fileUpload"] = lbl.Text;
        bool[] arrError = new bool[]
        {
            tbNom.InvalidateIfEmpty(lblErrNom, "Le nom ne peut être vide"),
            tbPrix.InvalidateIfEmpty(lblErrPrix, "Le prix demandé est obligatoire"),
            tbPoids.InvalidateIfEmpty(lblErrPoids, "Le poids est obligatoire"),
            tbQte.InvalidateIfEmpty(lblErrQte, "La quantité est obligatoire"),
            validationFichier("ajout"),
            ValidationDeDate(),
            validationRabais(),
            validationNomProduit(tbNom.Text)
        };

        if (!arrError.Contains(true))
        {
            string nom = tbNom.Text.Trim();
            string date = tbDate.Text;
            string categorie = ddlDesc.SelectedValue;
            string prix = tbPrix.Text;
            string poids = tbPoids.Text;
            string quantite = tbQte.Text;
            string dispo = rbList.SelectedValue;
            string description = tb.Text;
            string prixDeVente = tbPrixV.Text;
            bool bit = false;
            string nomFile = "";

            if (tbDate.Text == "")
            {
                date = null;
            }

            if (description.Trim() == "")
            {
                description = "N/A";
            }

            if (prixDeVente == "")
            {
                prixDeVente = prix;
            }

            PPProduits produits = new PPProduits();

            Produit trouverID = produits.Values.Where(x => x.NoVendeur == vendeur.NoVendeur).OrderBy(x => x.NoProduit).ToList().Last();
            long nextID = trouverID.NoProduit.Value + 1;

            if (fileUpload.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(fileUpload.FileName); //le upload marche (donc il restera a le faire comme du monde, pour le moment on n'a que le nom du fichier (devra etre l'ID)
                    //Session["fileUpload"] = filename;
                    nomFile = nextID + "." + filename.Split('.')[1];
                    fileUpload.SaveAs(Server.MapPath("~/Pictures/") + nomFile);
                    
                }
                catch (Exception ex)
                {
                    //mettre une erreur?
                }
            }

            if (dispo.Equals("Disponible"))
            {
                bit = true;
            }
            panel.LblDyn("", prix);
            Produit newProduit = new Produit(null)
            {
                NoProduit = nextID,
                NoVendeur = vendeur.NoVendeur,
                NoCategorie = (int?)int.Parse(categorie),
                Nom = nom,
                Description = description,
                Photo = nomFile,
                PrixDemande = (decimal?)decimal.Parse(prix),
                NombreItems = (short?)short.Parse(quantite),
                Disponibilité = bit,
                DateVente = date == null ? null : (DateTime?)DateTime.Parse(date),
                PrixVente = (decimal?)decimal.Parse(prixDeVente),
                Poids = (decimal?)decimal.Parse(poids),
                DateCreation = (DateTime?)DateTime.Now,
                DateMAJ = (DateTime?)DateTime.Now
            };
            produits.Add(newProduit);

            Session["fileUpload"] = null;
            Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");
        }
    }

    protected void click_confirmer_Modif(object sender, EventArgs e)
    {
        tbNom.DefaultControl();
        //Session["fileUpload"] = lbl.Text;
        bool[] arrError = new bool[]
        {
            tbNom.InvalidateIfEmpty(lblErrNom, "Le nom ne peut être vide"),
            tbPrix.InvalidateIfEmpty(lblErrPrix, "Le prix demandé est obligatoire"),
            tbPoids.InvalidateIfEmpty(lblErrPoids, "Le poids est obligatoire"),
            tbQte.InvalidateIfEmpty(lblErrQte, "La quantité est obligatoire"),
            validationFichier("modif"),
            ValidationDeDate(),
            validationRabais()
        };

        if (!arrError.Contains(true))
        {
            string nom = tbNom.Text.Trim();
            string date = tbDate.Text;
            string categorie = ddlDesc.SelectedValue;
            string prix = tbPrix.Text;
            string poids = tbPoids.Text;
            string quantite = tbQte.Text;
            string dispo = rbList.SelectedValue;
            string description = tb.Text;
            string prixDeVente = tbPrixV.Text;
            bool bit = false;
            string nomFile = "";

            bool modification = false;

            //nom
            if (monProduit_Modif.Nom != nom)
            {
                monProduit_Modif.Nom = nom;
                modification = true;
            }

            //Prix de vente
            if (prixDeVente != "")
            {
                if (monProduit_Modif.PrixVente.Value != decimal.Parse(prixDeVente))
                {
                    monProduit_Modif.PrixVente = decimal.Parse(prixDeVente);
                    modification = true;
                }
                else if (monProduit_Modif.PrixVente.Value != monProduit_Modif.PrixDemande.Value && monProduit_Modif.PrixVente.Value != decimal.Parse(prixDeVente))
                {
                    monProduit_Modif.PrixVente = decimal.Parse(prixDeVente);
                    modification = true;
                }
            }
            else
            {
                if (monProduit_Modif.PrixVente.Value != monProduit_Modif.PrixDemande.Value)
                {
                    if (monProduit_Modif.PrixDemande.Value == decimal.Parse(prix))
                    {
                        monProduit_Modif.PrixVente = monProduit_Modif.PrixDemande;
                        modification = true;
                    }
                    else
                    {
                        monProduit_Modif.PrixVente = decimal.Parse(prix);
                        modification = true;
                    }
                   
                }else if (monProduit_Modif.PrixDemande.Value != decimal.Parse(prix))
                {
                    monProduit_Modif.PrixVente = decimal.Parse(prix);
                    modification = true;
                }
            }

            //prix demandé
            if (monProduit_Modif.PrixDemande.Value != decimal.Parse(prix))
            {
                monProduit_Modif.PrixDemande = decimal.Parse(prix);
                modification = true;
            }

            //poids
            if (monProduit_Modif.Poids.Value != decimal.Parse(poids))
            {
                monProduit_Modif.Poids = decimal.Parse(poids);
                modification = true;
            }

            //quantite
            if (monProduit_Modif.NombreItems.Value != short.Parse(quantite))
            {
                monProduit_Modif.NombreItems = short.Parse(quantite);
                modification = true;
            }

            //description

          /*  if (monProduit_Modif.Description != description)
            {
                if (description.Trim() == "")
                {
                    monProduit_Modif.Description = "N/A";
                    modification = true;
                }
                else
                {
                    monProduit_Modif.Description = description;
                    modification = true;
                }
            }*/

            //essais 2 

            if (monProduit_Modif.Description == "N/A" && description != "")
            {
                monProduit_Modif.Description = description;
                modification = true;
            }
            else if (monProduit_Modif.Description != "N/A" && description != "" && monProduit_Modif.Description != description)
            {
                monProduit_Modif.Description = description;
                modification = true;
            }else if (monProduit_Modif.Description != "N/A" && description == "")
            {
                monProduit_Modif.Description = "N/A";
                modification = true;
            }

            //date
            if (date != "")
            {
                if (monProduit_Modif.DateVente != DateTime.Parse(date))
                {
                    monProduit_Modif.DateVente = DateTime.Parse(date);
                    modification = true;
                }
            }
            else
            {
                if (monProduit_Modif.DateVente != null)
                {
                    monProduit_Modif.DateVente = null;
                }
            }
            

            //categorie

            if (monProduit_Modif.NoCategorie.Value != int.Parse(ddlDesc.SelectedValue))
            {
                monProduit_Modif.NoCategorie = int.Parse(ddlDesc.SelectedValue);
                modification = true;
            }

            //dispo            
            if (rbList.SelectedValue.ToString() == "Disponible")
            {
                bit = true;
            }
            else
            {
                bit = false;
            }
            bool indispo = false;
            if (monProduit_Modif.Disponibilité.Value != bit)
            {
                monProduit_Modif.Disponibilité = bit;
                indispo = true;
                modification = true;
            }

            //image
            nomFile = lbl.Text;
            if (nomFile != monProduit_Modif.Photo)
            {
                if (fileUpload.HasFile)
                {
                    try
                    {
                        string filename = Path.GetFileName(fileUpload.FileName); //le upload marche (donc il restera a le faire comme du monde, pour le moment on n'a que le nom du fichier (devra etre l'ID)                                                                                
                        nomFile = monProduit_Modif.NoProduit.ToString() + "." + filename.Split('.')[1];
                        File.Delete(Server.MapPath("~/Pictures/" + monProduit_Modif.Photo));
                        fileUpload.SaveAs(Server.MapPath("~/Pictures/") + nomFile);
                        
                    }
                    catch (Exception ex)
                    {
                        //mettre une erreur?
                    }
                }
                monProduit_Modif.Photo = nomFile;
                modification = true;
            }
             
            if (modification)
            {
                //panier mais pas commande
                PPMessages ppM = new PPMessages();
                PPDestinataires ppd = new PPDestinataires();
                PPVendeurs ppv = new PPVendeurs();
                long noM = ppM.NextId();
                //@Marc ici on envoie un courriel a tous les clients ayant eu un panier pour le produit
                int noV = int.Parse(vendeur.NoVendeur.ToString());
                Vendeur venMess = ppv.Values.Find(v => v.NoVendeur == vendeur.NoVendeur);
                Message m = new Message(null)
                {
                    NoMsg = (int)noM,
                    DescMsg = $"Un des produits dans votre panier ({monProduit_Modif.Nom}) de {venMess.NomAffaires} fut modifié.",
                    objet = "Un des produits dans votre panier fut modifié.",
                    Lieu = 2,
                    dateEnvoi = DateTime.Now,
                    NoExpediteur = noV
                };
                ppM.Add(m);
                ppM.Update();

                PPArticlesEnPanier articlesMessages = new PPArticlesEnPanier();
                List<ArticleEnPanier> listeMessage = articlesMessages.Values.Where(x => x.NoProduit == monProduit_Modif.NoProduit).ToList();
                foreach (ArticleEnPanier article in listeMessage)
                {
                    Destinataire d = new Destinataire(null)
                    {
                        NoDestinataire = (int)article.NoClient,
                        NoMsg = (int)noM,
                        EtatLu = 0,
                        Lieu = 1
                    };
                    ppd.Add(d);
                    ppd.Update();
                }



                monProduit_Modif.DateMAJ = DateTime.Today;
                PPProduits mesProduits = new PPProduits();
                mesProduits.NotifyUpdatedOutside(monProduit_Modif);
                mesProduits.Update();

                //supprimer le panier avec un article non dispo
                if (indispo)
                {
                    PPArticlesEnPanier articlesPaniers = new PPArticlesEnPanier();

                    List<ArticleEnPanier> paniersSup = articlesPaniers.Values.Where(x => x.NoProduit == monProduit_Modif.NoProduit).ToList();
                    foreach (var pan in paniersSup)
                    {
                        articlesPaniers.Remove(pan);
                    }
                    articlesPaniers.Update();
                }


                Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");
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
            Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");

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
            //gérer la suppression ici

            Button btnOk = panelModalFooter.BtnDyn("s" + btnOKID, "Confirmer", click_supprimer, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            //btnOk.Attributes.Add("data-dismiss", "modal");
            Button btnNon = panelModalFooter.BtnDyn(btnAnnulerID, "Annuler", null, "btn btn-dark"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnNon.Attributes.Add("data-dismiss", "modal");
        }
        else
        {
            Button btnOk = panelModalFooter.BtnDyn("s" + btnOKID, "Confirmer", null, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            //btnOk.Attributes.Add("data-dismiss", "modal");
            Button btnNon = panelModalFooter.BtnDyn(btnAnnulerID, "Annuler", null, "btn btn-dark"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnNon.Attributes.Add("data-dismiss", "modal");
        }

    }

    private void click_supprimer(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        PPProduits produits = new PPProduits();
        Produit aSupprimer = produits.Values.Where(x => x.NoProduit == long.Parse(btn.ID.ToString().Split('s')[1])).ToList().First();
        //je peux récupérer l'id a supprimer avec le code ci-haut
        bool panier = false;
        bool commande = false;
        string sortie = aSupprimer.NoProduit.ToString() + " ---> ";
        PPArticlesEnPanier paniers = new PPArticlesEnPanier();
        List<ArticleEnPanier> listePanier = paniers.Values.Where(x => x.NoProduit == aSupprimer.NoProduit).ToList();

        if (listePanier.Count() != 0)
        {
            //Dans panier
            panier = true;
        }

        PPDetailsCommandes detailsCommandes = new PPDetailsCommandes();
        List<DetailsCommandes> listeCommande = detailsCommandes.Values.Where(x => x.NoProduit == aSupprimer.NoProduit).ToList();
        if (listeCommande.Count() != 0)
        {
            //Dans commande
            commande = true;
        }

        if (!panier && !commande)
        {
            //null part
            //panelTable.LblDyn("", "On peut supprimer le produit");
            produits.Remove(aSupprimer);
            produits.Update();
            File.Delete(Server.MapPath("~/Pictures/") + aSupprimer.Photo);
            var previous = ViewState["PreviousPageUrl"];
            if (previous != null)
                Response.Redirect(previous.ToString());
            else
                Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");

        }
        else if (panier && !commande)
        {
            //panier mais pas commande
            PPMessages ppM = new PPMessages();
            PPDestinataires ppd = new PPDestinataires();
            PPVendeurs ppv = new PPVendeurs();
            long noM = ppM.NextId();
            //@Marc ici on envoie un courriel a tous les clients ayant eu un panier pour le produit
            int noV = int.Parse(Session.GetVendeur().NoVendeur.ToString());
            Vendeur venMess = ppv.Values.Find(v => v.NoVendeur == aSupprimer.NoVendeur);
            Message m = new Message(null)
            {
                NoMsg = (int)noM,
                DescMsg = $"Un des produits dans votre panier ({aSupprimer.Nom}) de {venMess.NomAffaires} fut supprimé.",
                objet = "Un des produits dans votre panier fut supprimé.",
                Lieu = 2,
                dateEnvoi = DateTime.Now,
                NoExpediteur = noV
            };
            ppM.Update();


            //supprimer les paniers liés a ce produit
            foreach (ArticleEnPanier article in listePanier)
            {
                Destinataire d = new Destinataire(null)
                {
                    NoDestinataire = (int)article.NoClient,
                    NoMsg = (int)noM,
                    EtatLu = 0,
                    Lieu = 1
                };
                ppd.Update();
                paniers.Remove(article);
            }
            paniers.Update();
            //supprimer le produit en question
            produits.Remove(aSupprimer);
            produits.Update();
            File.Delete(Server.MapPath("~/Pictures/") + aSupprimer.Photo);

            var previous = ViewState["PreviousPageUrl"];
            if (previous != null)
                Response.Redirect(previous.ToString());
            else
                Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");


            //panelTable.LblDyn("", "Retirer des paniers, aviser le vendeur : nb Panier : " + listePanier.Count().ToString());
        }
        else if (!panier && commande)
        {
            //a revoir car pas vraiment supprimé... lol
            //dans commande mais pas panier
            aSupprimer.Disponibilité = false;
            aSupprimer.NombreItems = -1;
            produits.NotifyUpdatedOutside(aSupprimer);
            produits.Update();
            var previous = ViewState["PreviousPageUrl"];
            if (previous != null)
                Response.Redirect(previous.ToString());
            else
                Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");
            //panelTable.LblDyn("", "Reste au catalogue, qte devient 0 et dispo = non");
        }
        else if (panier && commande)
        {
            //partout
            PPMessages ppM = new PPMessages();
            PPDestinataires ppd = new PPDestinataires();
            PPVendeurs ppv = new PPVendeurs();
            long noM = ppM.NextId();

            int noV = int.Parse(Session.GetVendeur().NoVendeur.ToString());
            Vendeur venMess = ppv.Values.Find(v => v.NoVendeur == aSupprimer.NoVendeur);
            Message m = new Message(null)
            {
                NoMsg = (int)noM,
                DescMsg = $"Un des produits dans votre panier ({aSupprimer.Nom}) de {venMess.NomAffaires} fut supprimé.",
                objet = "Un des produits dans votre panier fut supprimé.",
                Lieu = 2,
                dateEnvoi = DateTime.Now,
                NoExpediteur = noV
            };
            ppM.Update();


            //supprime les paniers
            foreach (ArticleEnPanier article in listePanier)
            {
                Destinataire d = new Destinataire(null)
                {
                    NoDestinataire = (int)article.NoClient,
                    NoMsg = (int)noM,
                    EtatLu = 0,
                    Lieu = 1
                };
                ppd.Update();
                paniers.Remove(article);
            }
            paniers.Update();

            //change le statut a cause des commandes
            aSupprimer.Disponibilité = false;
            aSupprimer.NombreItems = -1;
            produits.NotifyUpdatedOutside(aSupprimer);
            produits.Update();

            //@Marc ici on envoie un courriel a tous les clients ayant eu un panier pour le produit


            var previous = ViewState["PreviousPageUrl"];
            if (previous != null)
                Response.Redirect(previous.ToString());
            else
                Response.Redirect("~/Pages/Vendeur/GestionCatalogue.aspx");
            //panelTable.LblDyn("", "Retirer panier et Mettre Qte a 0 et dispo = non");
        }
    }

}

