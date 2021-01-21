using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Client_HistoriqueCommandes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsClient())
            Response.Redirect(SessionManager.RedirectConnexionLink);

        Client client = Session.GetClient();

        PPCommandes commandes = new PPCommandes();
        PPDetailsCommandes detailsCommandes = new PPDetailsCommandes();
        PPVendeurs vendeurs = new PPVendeurs();
        PPTypesLivraison typesLivraison = new PPTypesLivraison();

        var listeCommandes = from commande in commandes.Values
                             join vendeur in vendeurs.Values on commande.NoVendeur equals vendeur.NoVendeur
                             join typeLivraison in typesLivraison.Values on commande.TypeLivraison equals typeLivraison.CodeLivraison
                             where commande.NoClient == client.NoClient
                             let listeDetails = detailsCommandes.Values.Where(x => x.NoCommande == commande.NoCommande)
                             select new { Commande = commande, TypeLivraison = typeLivraison , Vendeur = vendeur, Details = listeDetails };

        pnlContent.CardDyn(
            "", "mb-3",
            lbl => lbl.Text = "Historique des commandes",
            body =>
            {
                Table table = body.TableDyn("", "table table-padding");
                TableHeaderRow hrow = table.ThrDyn();
                hrow.ThdDyn("No Commande");
                hrow.ThdDyn("Nom du vendeur");
                hrow.ThdDyn("Date de la commande");
                hrow.ThdDyn("Prix total avant taxes");

                foreach (var commande in listeCommandes)
                {
                    TableRow row = table.TrDyn();

                    TableCell cellBtnCommande = row.TdDyn();
                    string path = $"Factures/{commande.Commande.NoCommande}.pdf";
                    if (File.Exists(Server.MapPath($"~/{path}")))
                        cellBtnCommande.BtnClientDyn("", commande.Commande.NoCommande.ToString(), $"window.open('/{path}'); return false;", "btn btn-outline-secondary btn-block");
                    else
                        cellBtnCommande.BtnClientDyn("", commande.Commande.NoCommande.ToString(), $"window.open('/Factures/defaut.pdf'); return false", "btn btn-outline-secondary btn-block");

                    TableCell cellLinkVendeur = row.TdDyn();
                    cellLinkVendeur.HlinkDYN("", $"~/Pages/Client/Catalogue.aspx?NoVendeur={commande.Vendeur.NoVendeur}", commande.Vendeur.NomAffaires);

                    row.TdDyn(commande.Commande.DateCommande.ToString());
                    row.TdDyn(commande.Commande.MontantTotAvantTaxes.Value.ToString("N2") + "$");
                }
            });

    }
}