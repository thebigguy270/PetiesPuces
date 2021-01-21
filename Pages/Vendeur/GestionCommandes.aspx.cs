using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Vendeur_GestionCommandes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Session.IsVendeur())
            Response.Redirect(SessionManager.RedirectConnexionLink);

        Vendeur vendeur = Session.GetVendeur();

        //Requetes pour commandes non traitées
        PPCommandes commandes = new PPCommandes();
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

        //commandes non-traitées

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

                TableCell cellNom = new TableCell();
                /* cellNom.Text = "Marcel Leboeuf";
                 cellNom.Attributes.Add("OnClick", "window.location.assign('http://www.google.com');");*/

                string nomPDF = "";

                if (File.Exists(Server.MapPath("~/Factures/" + noCommande.ToString() + ".pdf")))
                {
                    nomPDF = noCommande.ToString() + ".pdf";
                }
                else
                {
                    nomPDF = "defaut.pdf";
                }

                cellNom.Text = nomClient;
                cellNom.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
                row.Cells.Add(cellNom);


                TableCell cellDate = new TableCell();
                cellDate.Text = dateCommande.ToString("yyyy/MM/dd");
                cellDate.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
                row.Cells.Add(cellDate);

                TableCell cellPrix = new TableCell();
                cellPrix.Text = montantCom.ToString("N2") + " $";
                cellPrix.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
                row.Cells.Add(cellPrix);

                TableCell cellStatut = new TableCell();
                cellStatut.Text = poidsCommande.ToString("N2") + " Lbs";
                cellStatut.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");
                row.Cells.Add(cellStatut);


                //modal (il faut faire référence à la div à cet effet dans le front-hand.
                modal(panelAvecModal, "myModal" + indexCom.ToString(), noCommande.ToString(), "btnAnnulerLiv" + indexCom.ToString(), "Livraison", "Voulez-vous vraiment confirmer la livraison de cette commande? (" + noCommande.ToString() + ")", true);

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
            divBTN.Visible = false;
            pnCommandesClient.LblDyn("", "Vous n'avez aucune commande non traité", "h4 text-info ml-3");
        }

        //historique commandes
        var commandesVendeurH = from commande in commandes.Values
                               join monClient in clients.Values on commande.NoClient equals monClient.NoClient
                               where commande.NoVendeur == vendeur.NoVendeur && commande.Statut == "1"
                                orderby commande.DateCommande descending
                               let lstDetails = details.Values.Where(x => x.NoCommande == commande.NoCommande).ToList()
                               select new { Commande = commande, Client = monClient, Details = lstDetails };

        int index = 0;
        if (commandesVendeurH.Count() > 0)
        {



            foreach (var com in commandesVendeurH)
            {
                string nomPDF = "";
                long? noCommande = com.Commande.NoCommande.Value;

                if (File.Exists(Server.MapPath("~/Factures/" + noCommande.ToString() + ".pdf")))
                {
                    nomPDF = noCommande.ToString() + ".pdf";
                }
                else
                {
                    nomPDF = "defaut.pdf";
                }
                TableRow row = new TableRow();
                row.Attributes.Add("OnClick", "ouvrirPDF('" + nomPDF + "');");

                string nomClient = com.Client.Prenom + " " + com.Client.Nom;
                DateTime dateCommande = com.Commande.DateCommande.Value;

                decimal poidsCommande = com.Commande.PoidsTotal.Value;

                PPHistoriquePaiements historique = new PPHistoriquePaiements();
                //requete pour aller cherche le montant dans l'historique
                var montant = from montantHisto in historique.Values
                              where montantHisto.NoCommande == com.Commande.NoCommande
                              select montantHisto.MontantVenteAvantLivraison;

                decimal montantCom = montant.First().Value;

                TableCell cellNom = new TableCell();
                /* cellNom.Text = "Marcel Leboeuf";
                 cellNom.Attributes.Add("OnClick", "window.location.assign('http://www.google.com');");*/
                cellNom.Text = nomClient;
                row.Cells.Add(cellNom);


                TableCell cellDate = new TableCell();
                cellDate.Text = dateCommande.ToString("yyyy/MM/dd");
                row.Cells.Add(cellDate);

                TableCell cellPrix = new TableCell();
                cellPrix.Text = montantCom.ToString("N2") + " $";
                row.Cells.Add(cellPrix);

                TableCell cellStatut = new TableCell();
                cellStatut.Text = poidsCommande.ToString("N2") + " Lbs";
                row.Cells.Add(cellStatut);


                //modal (il faut faire référence à la div à cet effet dans le front-hand.
                modal(panelAvecModal, "myModalH" + index.ToString(), "btnOkLivH" + index.ToString(), "btnAnnulerLivH" + index.ToString(), "Livraison", "Voulez-vous vraiment confirmer la livraison de cette commande? (" + nomClient + ")", true);
                tableHistorique.Rows.Add(row);
                index++;
            }
        }
        else
        {
            tableHistorique.Visible = false;
            divBTN2.Visible = false;
            pnCommandesTraitees.LblDyn("", "Vous n'avez aucune commande dans l'historique", "h4 text-info ml-3");
        }
    }

    protected void ouvrirInfosClients(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Vendeur/InfosClients.aspx");
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
        PPCommandes commandes = new PPCommandes();
        foreach (Commande commande in commandes.Values)
        {
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
    public void retour(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Vendeur/AccueilVendeur.aspx");
    }
}
 