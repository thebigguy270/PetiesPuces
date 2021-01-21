using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Vendeur_GestionCatalogue : System.Web.UI.Page
{
    Vendeur vendeur;
    int idProduitGET = 0;
    string couleurTexte;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsVendeur())
            Response.Redirect(SessionManager.RedirectConnexionLink);
        vendeur = Session.GetVendeur();
        string couleurFond = "";
        couleurTexte = "";

        if (vendeur.Configuration != null)
        {
            if (vendeur.Configuration.Contains(";"))
            {
                string[] liste = vendeur.Configuration.Split(';');

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
            bodyAjouter.BackColor = System.Drawing.ColorTranslator.FromHtml(couleurFond);
            panelTable.BackColor = System.Drawing.ColorTranslator.FromHtml(couleurFond);
        }
        paniersAccueilClient(panelTable);
    }

    protected void ouvrirInscriptionProduit(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Vendeur/InscriptionProduit.aspx?T=ajout");
    }

    protected void ouvrirDetailsProduits(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string id = btn.ID.ToString();
        Response.Redirect("~/Pages/Vendeur/InscriptionProduit.aspx?T=details&ID=" + id);
    }

    protected void ouvrirModificationsProduits(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string id = btn.ID.ToString().Split('m')[1];
        Response.Redirect("~/Pages/Vendeur/InscriptionProduit.aspx?T=modification&ID=" + id);
    }

    private void paniersAccueilClient(Panel panel)
    {

       

        PPProduits produits = new PPProduits();
        List<Produit> nouvelleListe = produits.Values.OrderByDescending(x => x.DateCreation.Value).Where(c => c.NoVendeur == vendeur.NoVendeur).ToList();
        int index = 0;
        int index2 = 0;
        Panel rows = null;

        if (nouvelleListe.Count() > 0)
        {



            foreach (var produit in nouvelleListe)
            {
                if (index % 3 == 0)
                {
                    rows = panelTable.DivDyn("", "row");
                    index2++;
                }
                bool inactif = false;
                Panel panelCard = rows.DivDyn("", "col-1200-4 mb-3");
                Panel card = panelCard.DivDyn("", "card");
                string strInactif = "";
                if (!produit.Disponibilité.Value && produit.NombreItems.Value == -1)
                {
                    strInactif = "Inactif";
                    inactif = true;
                }
                else if (!produit.Disponibilité.Value)
                {
                    strInactif = "Indisponible";
                }
                else if (produit.NombreItems == 0)
                {
                    strInactif = "Rupture de stock";
                }

                Panel panelTitreCategories;
                if (inactif)
                {
                    panelTitreCategories = card.DivDyn("", "card-header fake-button bg-warning");
                }
                else
                {
                    panelTitreCategories = card.DivDyn("", "card-header fake-button");
                }
                Table tableT = panelTitreCategories.TableDyn("", "table borderless");
                TableRow rowt = tableT.TrDyn();
                TableCell cell1 = rowt.TdDyn();
                cell1.ImgDyn("", "~/Pictures/" + produit.Photo, "imgResize");
                TableCell cell2 = rowt.TdDyn();
                cell2.LblDyn("", produit.Nom, "card-title h6");
                cell2.ForeColor = System.Drawing.ColorTranslator.FromHtml(couleurTexte);

                if (!produit.Disponibilité.Value && produit.NombreItems.Value == -1)
                {
                    TableCell cell3 = rowt.TdDyn();
                    cell3.LblDyn("", strInactif, "card-title h6 text-danger");
                }
                else if (!produit.Disponibilité.Value || produit.NombreItems.Value == 0)
                {
                    TableCell cell3 = rowt.TdDyn();
                    cell3.LblDyn("", strInactif, "card-title h6 text-danger");
                }

                if (produit.PrixDemande != produit.PrixVente && produit.DateVente >= DateTime.Today)
                {
                    decimal montantRabais = produit.PrixDemande.Value - produit.PrixVente.Value;
                    TableCell cell3ou4 = rowt.TdDyn();
                    cell3ou4.LblDyn("", "rabais!<br/>", "card-title h6 text-success");
                    cell3ou4.LblDyn("", "(" + montantRabais.ToString("N2") + "$)<br/>" + produit.DateVente.Value.ToString("yyyy-MM-dd"), "h6 text-success");
                }
                else if (produit.PrixDemande != produit.PrixVente && produit.DateVente < DateTime.Today)
                {
                    TableCell cell3ou4 = rowt.TdDyn();
                    cell3ou4.LblDyn("", "rabais fini", "card-title h6 text-warning");
                }

                panelTitreCategories.Attributes.Add("data-toggle", "collapse");
                panelTitreCategories.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_collapsePanier" + index.ToString() + index2.ToString());
                panelTitreCategories.Attributes.Add("aria-expanded", "false");
                panelTitreCategories.Attributes.Add("aria-controls", "Contenu_ContenuPrincipal_collapsePanier" + index.ToString() + index2.ToString());

                Panel collapsable;
                //body des categories

                collapsable = card.DivDyn("collapsePanier" + index.ToString() + index2.ToString(), "collapse collapse");

                //colones header
                Panel panelBodyCat = collapsable.DivDyn("", "card-body");

                Table table = panelBodyCat.TableDyn("", "table");
                TableHeaderRow rowTable = table.ThrDyn();
                TableHeaderCell headerCss = rowTable.ThdDyn("Date");
                headerCss = rowTable.ThdDyn("Poids");
                headerCss.CssClass = "text-right";
                headerCss = rowTable.ThdDyn("Prix habituel");
                headerCss.CssClass = "text-right";
                headerCss = rowTable.ThdDyn("Qte");

                TableRow rowTableB = table.TrDyn();

                TableCell celluleCss = rowTableB.TdDyn(produit.DateCreation.Value.ToString("yyyy/MM/dd"));
                celluleCss = rowTableB.TdDyn(produit.Poids.Value.ToString("N2") + " Lbs");
                celluleCss.CssClass = "text-right";
                celluleCss = rowTableB.TdDyn(produit.PrixDemande.Value.ToString("N2") + " $");
                celluleCss.CssClass = "text-right";
                string nbItems = "--";
                if (produit.NombreItems.Value != -1)
                {
                    nbItems = "(" + produit.NombreItems.Value.ToString() + ")";
                }
                rowTableB.TdDyn(nbItems);

                if (!inactif)
                {
                    Panel rowBtnSupprimer = collapsable.DivDyn("", "row ml-3 mb-3");
                    rowBtnSupprimer.BtnDyn(produit.NoProduit.ToString(), "Détails", ouvrirDetailsProduits, "btn btn-outline-info col-3");
                    rowBtnSupprimer.DivDyn("", "col-1");
                    rowBtnSupprimer.BtnDyn("m" + produit.NoProduit.ToString(), "Modifier", ouvrirModificationsProduits, "btn btn-outline-dark col-3");
                    rowBtnSupprimer.DivDyn("", "col-1");
                    Button btn = rowBtnSupprimer.BtnDyn("", "X", null, "btn btn-outline-danger col-3");

                    btn.OnClientClick = "return false;";
                    btn.Attributes.Add("data-toggle", "modal");
                    btn.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_myModal" + index.ToString() + index2.ToString());
                    //Regarder ici si dans panier, alors loader le bon modal.
                    bool panier = false;
                    PPArticlesEnPanier paniers = new PPArticlesEnPanier();
                    List<ArticleEnPanier> trouverSiPanier = paniers.Values.Where(x => x.NoProduit == produit.NoProduit).ToList();
                    if (trouverSiPanier.Count() != 0)
                    {
                        //Dans panier
                        panier = true;
                    }


                    if (!panier)
                    {
                        modal(panelAvecModal, "myModal" + index.ToString() + index2.ToString(), produit.NoProduit.ToString()/*l'ID du btn est le noProduit*/, "idBtnAnnuler" + index.ToString() + index2.ToString(), "Supression", "Voulez vous supprimer ce produit : " + produit.Nom, true);
                    }
                    else
                    {
                        modal(panelAvecModal, "myModal" + index.ToString() + index2.ToString(), produit.NoProduit.ToString()/*l'ID du btn est le noProduit*/, "idBtnAnnuler" + index.ToString() + index2.ToString(), "Attention!", "Ce produit (" + produit.Nom + ") est dans au moins un panier, voulez-vous vraiment le supprimer?", true);
                    }
                }

                index++;
            }
        }
        else
        {
            panelTable.LblDyn("", "Vous n'avez aucun article dans ce moment", "h4 text-info ml-3");
        }



     
    }

    private void click_supprimer_Panier(object sender, EventArgs e)
    {
        panelTable.LblDyn("","faut gérer avec panier la");
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
        if (listeCommande.Count()!=0)
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
            Response.Redirect(Request.RawUrl);

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
            ppM.Add(m);
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
                ppd.Add(d);
                ppd.Update();
                paniers.Remove(article);
            }
            paniers.Update();
            //supprimer le produit en question
            produits.Remove(aSupprimer);
            produits.Update();
            File.Delete(Server.MapPath("~/Pictures/") + aSupprimer.Photo);

            
           



            Response.Redirect(Request.RawUrl);

            
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
            Response.Redirect(Request.RawUrl);
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
            ppM.Add(m);
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
                ppd.Add(d);
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


            Response.Redirect(Request.RawUrl);
            //panelTable.LblDyn("", "Retirer panier et Mettre Qte a 0 et dispo = non");
        }
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

            Button btnOk = panelModalFooter.BtnDyn("s"+btnOKID, "Confirmer", click_supprimer, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            //btnOk.Attributes.Add("data-dismiss", "modal");
            Button btnNon = panelModalFooter.BtnDyn(btnAnnulerID, "Annuler", null, "btn btn-dark"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnNon.Attributes.Add("data-dismiss", "modal");
        }
        else
        {
            Button btnOk = panelModalFooter.BtnDyn("s" + btnOKID, "Confirmer", click_supprimer_Panier, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            //btnOk.Attributes.Add("data-dismiss", "modal");
            Button btnNon = panelModalFooter.BtnDyn(btnAnnulerID, "Annuler", null, "btn btn-dark"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnNon.Attributes.Add("data-dismiss", "modal");
        }

    }

}