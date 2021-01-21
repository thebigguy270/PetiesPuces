using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Nouveautes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //section du haut
        Panel pnRow1 = pnInfos.DivDyn("", "row ml-3");
        pnRow1.LblDyn("", "Enregistrez vous comme client pour bénéficier des avantages suivants : ", "h5");

        Panel pnRow2 = pnInfos.DivDyn("", "row");
        Panel vide = pnRow2.DivDyn("", "col-1");
        Panel colonne = pnRow2.DivDyn("", "col");
        colonne.LblDyn("", "- Avoir accès aux catalogues de produits", "h4 text-info");

        
        vide = pnRow2.DivDyn("", "col-1");
        colonne = pnRow2.DivDyn("", "col");
        colonne.LblDyn("", "- Avoir accès aux paniers", "h4 text-info");

        pnRow2 = pnInfos.DivDyn("", "row");
        vide = pnRow2.DivDyn("", "col-1");
        colonne = pnRow2.DivDyn("", "col");
        colonne.LblDyn("", "- Passer des commandes", "h4 text-info");

        
        vide = pnRow2.DivDyn("", "col-1");
        colonne = pnRow2.DivDyn("", "col");
        colonne.LblDyn("", "- Rechercher sur le site", "h4 text-info");

        Panel pnRowBtn = pnInfos.DivDyn("", "row ml-3 mt-3");
        Button btn = pnRowBtn.BtnDyn("", "S'inscrire", ouvrirInscription, "btn btn-outline-success classBoutonsMargins100 mr-3");

        //section du bas où il y a 15 articles.

        PPProduits produits = new PPProduits();
        List<Produit> nouvelleListe = produits.Values.OrderByDescending(c => c.DateCreation).Where(x => x.Disponibilité == true && x.NombreItems > 0).ToList();

        int index = 0;
        int index2 = 0;
        Panel rows = null;
        foreach (var produit in nouvelleListe.Take(15))
        {
            if (index % 3 == 0)
            {
                rows = pnNouveautes.DivDyn("", "row mb-3");
                index2++;
            }
            PPVendeurs leVendeur = new PPVendeurs();
            Vendeur monVendeur = leVendeur.Values.Where(c => c.NoVendeur == produit.NoVendeur).First();

            Panel panelCard = rows.DivDyn("", "col-4");
            Panel card = panelCard.DivDyn("", "card");
            Panel panelTitreCategories = card.DivDyn("", "card-header fake-button");
            Table tableT = panelTitreCategories.TableDyn("", "");
            TableHeaderRow headerrow = tableT.ThrDyn();
            TableHeaderCell cellHd = headerrow.ThdDyn();
            cellHd.LblDyn("", " " + monVendeur.NomAffaires, "card-title h5 text-primary");
            TableRow rowt = tableT.TrDyn();
            TableCell cell1 = rowt.TdDyn();
            cell1.ImgDyn("", "~/Pictures/" + produit.Photo, "imgResize");
            TableCell cell2 = rowt.TdDyn();
            cell2.LblDyn("", produit.Nom, "card-title h6");

            rowt.Attributes.Add("data-toggle", "modal");
            rowt.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_myModal" + index.ToString());

            headerrow.Attributes.Add("data-toggle", "modal");
            headerrow.Attributes.Add("data-target", "#Contenu_ContenuPrincipal_myModal" + index.ToString());

            modal(panelAvecModal, "myModal" + index.ToString(), "btnOk"+index.ToString(), "btnAnnulerLiv" + index.ToString(), "Inscrivez-vous!", "Inscrivez-Vous pour avoir accèz aux détails!", true);

            index++;
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
            Button btnOk = new Button();//<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnOk.CssClass = "btn btn-secondary";
            btnOk.Text = "S'inscrire";
            /*btnOk.Command += livrerCommande;*/
            //btnOk.CommandArgument = btnOKID;
            btnOk.Click += new EventHandler(ouvrirInscription);

            panelModalFooter.Controls.Add(btnOk);
            //btnOk.Attributes.Add("data-dismiss", "modal");
            Button btnNon = panelModalFooter.BtnDyn(btnAnnulerID, "Non merci", null, "btn btn-dark"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnNon.Attributes.Add("data-dismiss", "modal");
        }
        else
        {
            Button btnOk = panelModalFooter.BtnDyn(btnOKID, "changez moi", null, "btn btn-secondary"); //<button type = "button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            btnOk.Attributes.Add("data-dismiss", "modal");
        }

    }

    protected void ouvrirInscription(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Inscription.aspx");
    }
}