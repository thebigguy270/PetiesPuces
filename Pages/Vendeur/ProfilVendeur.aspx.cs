using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class ProfilVendeur : System.Web.UI.Page
{
    Vendeur vendeur;
    FileUpload fileUpload;
    string config = "";
    string nomImageDEjaLa = "";
    Panel group;
    Panel prepend;
    Panel customFile;
    Label lblTele;

    Label lbl; //le lbl du file upload
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsVendeur())
            Response.Redirect(SessionManager.RedirectConnexionLink);


        //Image 
        Panel row = panelPerso.DivDyn("", "row mt-3");
        Panel col1 = row.DivDyn("", "col-4");
        Panel col2 = row.DivDyn("", "col-8");

        col1.LblDyn("", "Logo : ", "h5");
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

        vendeur = Session.GetVendeur();

        if (vendeur.Configuration != null)
        {
            if (vendeur.Configuration.Contains(";"))
            {
                string[] liste = vendeur.Configuration.Split(';');

                if (liste.Length == 2)
                {
                    if (liste[0] != "")
                    {
                        //lbl.Text = liste[0];
                        nomImageDEjaLa = liste[0];
                    }
                    else
                    {
                        //lbl.Text = "Aucun fichier";
                        nomImageDEjaLa = "Aucun fichier";
                    }

                }
                else if (liste.Length == 3)
                {
                    if (liste[0] != "")
                    {
                        //lbl.Text = liste[0];
                        nomImageDEjaLa = liste[0];
                    }
                    else
                    {
                        //lbl.Text = "Aucun fichier";
                        nomImageDEjaLa = "Aucun fichier";
                    }
                }
            }
            else
            {
                // lbl.Text = vendeur.Configuration;
                nomImageDEjaLa = vendeur.Configuration;

            }
        }
        else
        {
            //lbl.Text = "Aucun fichier";
            nomImageDEjaLa = "Aucun fichier";
        }



        if (!Page.IsPostBack)
        {
            tbAdresseEmail.Text = vendeur.AdresseEmail;
            tbNomAffaires.Text = vendeur.NomAffaires;
            tbPrenom.Text = vendeur.Prenom;
            tbNom.Text = vendeur.Nom;
            tbRue.Text = vendeur.Rue;
            tbVille.Text = vendeur.Ville;
            ddlProvince.SelectedValue = vendeur.Province;
            tbPays.Text = vendeur.Pays;
            tbCodePostal.Text = vendeur.CodePostal;
            tbTelephone.Text = vendeur.Tel1;
            tbCell.Text = vendeur.Tel2;


            if (vendeur.Configuration != null)
            {
                if (vendeur.Configuration.Contains(";"))
                {
                    string[] liste = vendeur.Configuration.Split(';');

                    if (liste.Length == 2)
                    {
                        if (liste[0] != "")
                        {
                            lbl.Text = liste[0];
                            nomImageDEjaLa = liste[0];
                        }
                        else
                        {
                            lbl.Text = "Aucun fichier";
                            nomImageDEjaLa = "Aucun fichier";
                        }

                        if (liste[1] != "")
                        {
                            couleurFond.Text = liste[1];
                        }

                    }
                    else if (liste.Length == 3)
                    {
                        if (liste[0] != "")
                        {
                            lbl.Text = liste[0];
                            nomImageDEjaLa = liste[0];
                        }
                        else
                        {
                            lbl.Text = "Aucun fichier";
                            nomImageDEjaLa = "Aucun fichier";
                        }

                        if (liste[1] != "")
                        {
                            couleurFond.Text = liste[1];
                        }

                        couleurText.Text = liste[2];
                    }
                }
                else
                {
                    lbl.Text = vendeur.Configuration;
                    nomImageDEjaLa = vendeur.Configuration;

                }
            }
            else
            {
                lbl.Text = "Aucun fichier";
                nomImageDEjaLa = "Aucun fichier";
            }


            if (vendeur.Taxes.HasValue)
            {
                if (vendeur.Taxes.Value)
                    rbOuiTaxes.Checked = true;
                else
                    rbNonTaxes.Checked = true;
            }

            tbPoidsMax.Text = vendeur.PoidsMaxLivraison.Value.ToString();
            tbPrixLivGratuite.Text = vendeur.LivraisonGratuite.Value.ToString("N2");
        }

        if (nomImageDEjaLa != "Aucun fichier")
        {
            Panel rowPourImage = panelPerso.DivDyn("", "row mt-3");
            Panel colImage = rowPourImage.DivDyn("", "col-3");
            Panel colText = rowPourImage.DivDyn("", "col-9");
            colImage.ImgDyn("", "~/Logos/" + nomImageDEjaLa, "img-fluid");
            colText.LblDyn("", "Ceci est votre logo présentement visible par les clients<br />", "h5 text-info");
            colText.LblDyn("", "Les modifications seront effectuées après la sauvegarde", "h6 text-muted");
        }

        /*  if (IsPostBack)
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
          }*/

        if (!string.IsNullOrEmpty(Request.QueryString["Reussite"]))
            pnlReussite.Visible = true;
        if (!string.IsNullOrEmpty(Request.QueryString["ReussiteMDP"]))
            pnlReussiteMDP.Visible = true;
    }

    protected void btnSauvegarder_OnClick(object sender, EventArgs e)
    {
        tbPoidsMax.DefaultControl();
        tbNomAffaires.DefaultControl();
        tbNom.DefaultControl();
        tbPrenom.DefaultControl();
        tbPrixLivGratuite.DefaultControl();
        tbRue.DefaultControl();
        tbCodePostal.DefaultControl();
        tbTelephone.DefaultControl();
        tbCell.DefaultControl();
        ddlProvince.DefaultControl();
        tbVille.DefaultControl();

        string couleurBG = couleurFond.Text;
        string couleurTEXT = couleurText.Text;

        int poidsMax = -1;
        decimal prixLivGratuite = -1;
        bool[] arrError = new bool[]
        {
            // Nom d'affaires
            tbNomAffaires.InvalidateIfEmpty(lblErrorNomAffaires, "Le nom d'affaires doit être présent"),
            tbPrenom.InvalidateIfEmpty(lblErrorPrenom, "Le prénom doit être présent"),
            tbNom.InvalidateIfEmpty(lblErrorNom, "Le nom doit être présent"),
            tbRue.InvalidateIfEmpty(lblErrorRue, "Les informations sur la rue (No Civique et Rue)"),
            ddlProvince.InvalidateIfEmpty(lblErrorProvince, "La province doit être selectionnée"),
            tbVille.InvalidateIfEmpty(lblErrorVille, "Le nom de la ville doit être présent"),
            tbCodePostal.InvalidateIfEmpty(lblErrorCodePostal, "Le code postal doit être présent"),
            tbTelephone.InvalidateIfEmpty(lblErrorTelephone, "Le numéro de téléphone doit être présent")
                || tbTelephone.CheckContains(lblErrorTelephone, "Le numéro de téléphone doit être entré au complet", "_"),
            tbCell.CheckContains(lblErrorCell, "Le numéro de téléphone doit être entré au complet", "_"),
            // Poids Max
            tbPoidsMax.InvalidateIfEmpty(lblErrorPoidsMax, "Le poids maximal pour une livraison doit être présent")
                || tbPoidsMax.CheckInt(lblErrorPoidsMax, "Le poids maximal doit être un nombre décimal", out poidsMax)
                || tbPoidsMax.CheckIntOver0(lblErrorPoidsMax, poidsMax),
            // Poids livraison gratuite
            tbPrixLivGratuite.CheckDecimal(lblErrorPoidsLivGratuit, "Le prix auquel une livraison devient gratuite doit être un nombre décimal", out prixLivGratuite)
                || tbPrixLivGratuite.CheckDecimalOver0(lblErrorPoidsLivGratuit, prixLivGratuite),
            validationFichier(),
        };

        if (!arrError.Contains(true))
        {
            bool modifications = false;

            bool modificationFile = upload(vendeur);
            bool modificationCouleur = modifCouleur(vendeur, "fond");
            bool modificationTexte = modifCouleur(vendeur, "text");

            if (vendeur.NomAffaires != tbNomAffaires.Text)
            {
                vendeur.NomAffaires = tbNomAffaires.Text;
                modifications = true;
            }
            if (vendeur.Prenom != tbPrenom.Text)
            {
                vendeur.Prenom = tbPrenom.Text;
                modifications = true;
            }
            if (vendeur.Nom != tbNom.Text)
            {
                vendeur.Nom = tbNom.Text;
                modifications = true;
            }
            if (vendeur.Rue != tbRue.Text)
            {
                vendeur.Rue = tbRue.Text;
                modifications = true;
            }
            if (vendeur.Ville != tbVille.Text)
            {
                vendeur.Ville = tbVille.Text;
                modifications = true;
            }
            if (vendeur.Province != ddlProvince.SelectedValue)
            {
                vendeur.Province = ddlProvince.SelectedValue;
                modifications = true;
            }
            if (vendeur.CodePostal != tbCodePostal.Text)
            {
                vendeur.CodePostal = tbCodePostal.Text;
                modifications = true;
            }
            if (vendeur.Tel1 != tbTelephone.Text)
            {
                vendeur.Tel1 = tbTelephone.Text;
                modifications = true;
            }
            if (vendeur.Tel2 != tbCell.Text)
            {
                vendeur.Tel2 = tbCell.Text;
                modifications = true;
            }
            if (vendeur.Taxes.HasValue)
            {
                if (vendeur.Taxes.Value && rbNonTaxes.Checked)
                {
                    vendeur.Taxes = false;
                    modifications = true;
                }
                else if (!vendeur.Taxes.Value && rbOuiTaxes.Checked)
                {
                    vendeur.Taxes = true;
                    modifications = true;
                }
            }
            if (vendeur.PoidsMaxLivraison != poidsMax)
            {
                vendeur.PoidsMaxLivraison = poidsMax;
                modifications = true;
            }
            if (vendeur.LivraisonGratuite != prixLivGratuite)
            {
                vendeur.LivraisonGratuite = prixLivGratuite;
                modifications = true;
            }

            if (modifications || modificationFile || modificationCouleur || modificationTexte)
            {
                PPVendeurs vendeurs = new PPVendeurs();
                vendeurs.NotifyUpdatedOutside(vendeur);

                vendeurs.Update();
                Response.Redirect("~/Pages/Vendeur/AccueilVendeur.aspx");
                //Response.Redirect("~/Pages/Vendeur/ProfilVendeur.aspx?Reussite=true");
            }
        }
    }

    protected bool modifCouleur(Vendeur vendeur, string type)
    {
        string image = "";
        string couleur = "";
        string texte = "";

        if (vendeur.Configuration != null)
        {
            if (vendeur.Configuration.Contains(";"))
            {
                string[] liste = vendeur.Configuration.Split(';');

                if (liste.Length == 2)
                {
                    image = liste[0];
                    couleur = liste[1];
                }
                else if (liste.Length == 3)
                {
                    image = liste[0];
                    couleur = liste[1];
                    texte = liste[2];
                }
            }
            else
            {
                image = vendeur.Configuration;
            }
        }



        bool retour = false;


        if (type == "fond")
        {
            if (couleurFond.Text != "#ffffff" || couleur != "") // marche si pas image, marche si image, marche si change couleur
            {
                if (couleur != couleurFond.Text)
                {
                    if (image == "")
                    {
                        vendeur.Configuration = ";" + couleurFond.Text;
                        retour = true;
                    }
                    else
                    {
                        if (texte != "")
                        {
                            vendeur.Configuration = image + ";" + couleurFond.Text + ";" + texte;
                            retour = true;
                        }
                        else
                        {
                            vendeur.Configuration = image + ";" + couleurFond.Text;
                            retour = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (couleurText.Text != "#000000" || texte != "") // marche si pas image, marche si image, marche si change couleur
            {
                if (texte != couleurText.Text)
                {
                    if (image == "" && couleur == "")
                    {
                        vendeur.Configuration = ";;" + couleurText.Text;
                        retour = true;
                    }
                    else if (image == "" && couleur != "")
                    {
                        vendeur.Configuration = ";" + couleur + ";" + couleurText.Text;
                        retour = true;
                    }
                    else if (image != "" && couleur == "")
                    {
                        vendeur.Configuration = image + ";" + ";" + couleurText.Text;
                        retour = true;
                    }
                    else
                    {
                        vendeur.Configuration = image + ";" + couleur + ";" + couleurText.Text;
                        retour = true;
                    }
                }
            }
        }

        return retour;
    }

    protected bool upload(Vendeur vendeur)
    {
        bool retour = false;
        //récupération et téléchargement de l'image dans nos ressources
        if (fileUpload.HasFile)
        {
            try
            {   //dans le cas où la config est vide.        
                if (vendeur.Configuration == null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName); //récupérer le nom de l'image.
                    string nom = vendeur.NoVendeur.ToString() + "." + filename.Split('.')[1];
                    vendeur.Configuration = nom;
                    fileUpload.SaveAs(Server.MapPath("~/Logos/") + nom);
                    retour = true;

                }
                else
                {
                    string[] liste = vendeur.Configuration.Split(';');
                    string filename = Path.GetFileName(fileUpload.FileName);
                    panelPerso.LblDyn("", filename, "text-success");
                    if (liste[0] != filename)
                    {

                        string rebuild = "";
                        for (int i = 0; i < liste.Length; i++)
                        {
                            if (i == 0)
                            {
                                string nom = vendeur.NoVendeur.ToString() + "." + filename.Split('.')[1];
                                rebuild = nom;
                                File.Delete(Server.MapPath("~/Logos/" + liste[0]));
                                fileUpload.SaveAs(Server.MapPath("~/Logos/") + nom);

                                retour = true;
                            }
                            else
                            {
                                if (i - 1 == 0)
                                {
                                    if (i < liste.Length - 1)
                                    {
                                        rebuild += ";" + liste[i] + ";";
                                    }
                                    else
                                    {
                                        rebuild += ";" + liste[i];
                                    }
                                }
                                else if (i < liste.Length - 1)
                                {
                                    rebuild += liste[i] + ";";
                                }
                                else
                                {
                                    rebuild += liste[i];
                                }
                            }
                        }

                        vendeur.Configuration = rebuild;
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                //mettre une erreur?
            }
        }
        return retour;
    }

    protected void btnAnnuler_OnClick(object sender, EventArgs e)
    {
        Session["fileUpload"] = null;
        Response.Redirect("~/Pages/Vendeur/AccueilVendeur.aspx");
    }

    protected bool validationFichier()
    {
        bool retour = false;

        if (lbl.Text != nomImageDEjaLa)
        {
            if (Path.GetFileName(fileUpload.FileName).Split('.')[1] != "jpg" && Path.GetFileName(fileUpload.FileName).Split('.')[1].ToUpper() != "PNG" && Path.GetFileName(fileUpload.FileName).Split('.')[1].ToUpper() != "JPEG")
            {
                lbl.CssClass = "text-danger custom-file-label h6";
                lblTele.CssClass = "text-danger input-group-text";
                retour = true;
            }
            else
            {
                lbl.CssClass = "custom-file-label h6";
                lblTele.CssClass = "input-group-text";
            }

        }

        return retour;
    }

    protected void btnSauvegarderMDP_OnClick(object sender, EventArgs e)
    {
        tbAncienMDP.DefaultControl();
        tbNouveauMDP.DefaultControl();
        tbConfirmationMDP.DefaultControl();

        Vendeur vendeur = Session.GetVendeur();
        if (tbAncienMDP.Text != vendeur.MotDePasse)
        {
            tbAncienMDP.Invalidate();
            lblErrorAncienMDP.Text = "L'ancien mot de passe doit correspondre au mot de passe courrant";
        }
        else
        {
            if (!tbNouveauMDP.InvalidateIfEmpty(lblErrorNouveauMDP, "Le mot de passe doit être définit")
                || !tbConfirmationMDP.InvalidateIfEmpty(lblErrorConfirmationMDP, "Le mot de passe doit être définit"))
            {
                if (tbNouveauMDP.Text != tbConfirmationMDP.Text)
                {
                    tbNouveauMDP.Invalidate();
                    tbConfirmationMDP.Invalidate();
                    lblErrorNouveauMDP.Text = "Les mots de passes doivent correspondre";
                }
                else if (tbNouveauMDP.Text == vendeur.MotDePasse)
                {
                    tbNouveauMDP.Invalidate();
                    tbConfirmationMDP.Invalidate();
                    lblErrorNouveauMDP.Text = "Le nouveau mot de passe doit être différent de l'ancien";
                }
                else
                {
                    vendeur.MotDePasse = tbNouveauMDP.Text;

                    PPVendeurs vendeurs = new PPVendeurs();
                    vendeurs.NotifyUpdatedOutside(vendeur);

                    vendeurs.Update();

                    Response.Redirect("~/Pages/Vendeur/ProfilVendeur.aspx?ReussiteMDP=true");
                }
            }
        }
    }
}