using PdfSharp;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using TheArtOfDev.HtmlRenderer.PdfSharp;

public static class FUNCGenererPDF
{
    public static void GenererPDF()
    {
        PPCommandes commandes = new PPCommandes();
        foreach (Commande commande in commandes.Values)
            new GestionPDF(commande).GeneratePDF();
    }
}

public class GestionPDF
{
    private SqlInit db = new SqlInit();
    public readonly Commande commande;
    private readonly Vendeur vendeur;
    private readonly Client client;
    private List<DetailsCommandes> detailsCommande;
    private readonly List<ArticleEnPanier> articleEnPaniersASupprimer = null;
    private readonly HistoriquePaiement historique = null;

    public GestionPDF(Client client, long noVendeur, decimal coutLivraison, short typeLivraison, decimal montantTotAvantTaxes, decimal tps, decimal tvq, decimal poidsTotal, int noAut, decimal fraisMarchand)
    {
        detailsCommande = new List<DetailsCommandes>();
        articleEnPaniersASupprimer = new List<ArticleEnPanier>();

        this.client = client;

        // Ajouter la commande
        vendeur = db.vendeurs.Values.Find(x => x.NoVendeur == noVendeur);

        commande = new Commande(null)
        {
            NoCommande = db.commandes.NextID(),
            NoClient = client.NoClient,
            NoVendeur = noVendeur,
            DateCommande = DateTime.Now,
            CoutLivraison = coutLivraison,
            TypeLivraison = typeLivraison,
            MontantTotAvantTaxes = montantTotAvantTaxes,
            TPS = tps,
            TVQ = tvq,
            PoidsTotal = poidsTotal,
            Statut = "0",
            NoAutorisation = noAut.ToString()
        };

        // Ajouter à l'historique de paiements
        historique = new HistoriquePaiement(null)
        {
            NoHistorique = db.historiquePaiements.NextID(),
            MontantVenteAvantLivraison = montantTotAvantTaxes,
            NoVendeur = noVendeur,
            NoClient = client.NoClient,
            NoCommande = commande.NoCommande,
            DateVente = DateTime.Now,
            NoAutorisation = noAut.ToString(),
            FraisLesi = fraisMarchand,
            Redevance = (montantTotAvantTaxes + coutLivraison + tps + tvq) * (vendeur.Pourcentage / 100),
            FraisLivraison = coutLivraison,
            FraisTPS = tps,
            FraisTVQ = tvq
        };

        // Supprimer les articles dans le panier et les ajouter au détail de la commande
        articleEnPaniersASupprimer = db.articlesEnPanier.Values
                                                        .Where(x => x.NoClient == client.NoClient
                                                            && x.NoVendeur == vendeur.NoVendeur).ToList();

        long nextIdDetailCommandes = db.detailsCommandes.NextID();
        foreach (ArticleEnPanier article in articleEnPaniersASupprimer)
        {
            // Produit correspondant à l'article
            Produit produit = db.produits.Values.Find(x => x.NoProduit == article.NoProduit);

            decimal prixArticle = 0;
            if ((produit.PrixVente.HasValue && produit.PrixVente != produit.PrixDemande)
                && (!produit.DateVente.HasValue || produit.DateVente.Value > DateTime.Now))
                prixArticle = produit.PrixVente.Value;
            else
                prixArticle = produit.PrixDemande.Value;

            // Créer nouveau detail de la commande pour le produit
            DetailsCommandes detail = new DetailsCommandes(null)
            {
                NoDetailCommandes = nextIdDetailCommandes,
                NoCommande = commande.NoCommande,
                NoProduit = article.NoProduit,
                PrixVente = prixArticle,
                Quantité = article.NbItems
            };
            detailsCommande.Add(detail);

            ++nextIdDetailCommandes;

            // Changer le nombre d'items disponibles pour les produits
            produit.NombreItems -= article.NbItems;
            db.produits.NotifyUpdated(produit);
        }
    }

    public GestionPDF(Commande commande)
    {
        this.commande = commande;
        vendeur = db.vendeurs.Values.Find(x => x.NoVendeur == commande.NoVendeur);
        client = db.clients.Values.Find(x => x.NoClient == commande.NoClient);
        detailsCommande = db.detailsCommandes.Values.Where(x => x.NoCommande == commande.NoCommande).ToList();
    }

    public GestionPDF GeneratePDF()
    {
        string strPdf = "";
        strPdf += "<style>";
        strPdf += "table { border-collapse: collapse; } table, th, td { border: 1px solid black; } th, td { padding: 10px; }";
        strPdf += "</style>";
        strPdf += $"<h3>Commande no {commande.NoCommande} passée le {commande.DateCommande}</h3>";
        strPdf += "<br/><br/>";
        strPdf += "<table style=\"border: 0px; width: 100%;\">";
        strPdf += "<tr style=\"border: 0px;\"><td style=\"border: 0px;\">";
        strPdf += "<br/><br/>Vendeur:<br/>";
        strPdf += $"{vendeur.NomAffaires}<br/>";
        strPdf += $"{vendeur.Rue}<br/>";
        strPdf += $"{vendeur.Province} {vendeur.Ville}<br/>";
        strPdf += "</td><td style=\"border: 0px;\">";
        strPdf += "<br/><br/>Client:<br/>";
        strPdf += $"{client.Nom} {client.Prenom}<br/>";
        strPdf += client.Rue + "<br/>";
        strPdf += $"{client.Province} {client.Ville}<br/>";
        strPdf += "</td></tr>";
        strPdf += "</table>";
        strPdf += "<br/><br/>";
        strPdf += "<table style=\"width: 100%;\">";
        strPdf += "<tr style=\"font-weight: bold;\">";
        strPdf += "<th style=\"background-color: lightgray;\">Quantité</th>";
        strPdf += "<th style=\"background-color: lightgray;\">Nom du produit</th>";
        strPdf += "<th style=\"background-color: lightgray;\">Poids (en Livre)</th>";
        strPdf += "<th style=\"background-color: lightgray; text-align: right;\">Prix unitaire</th>";
        strPdf += "<th style=\"background-color: lightgray; text-align: right;\">Prix Total</th>";
        strPdf += "</tr>";
        foreach (DetailsCommandes detail in detailsCommande)
        {
            Produit produit = db.produits.Values.Find(x => x.NoProduit == detail.NoProduit);

            decimal prixArticle = 0;
            if ((produit.PrixVente.HasValue && produit.PrixVente != produit.PrixDemande)
                && (!produit.DateVente.HasValue || produit.DateVente.Value > DateTime.Now))
                prixArticle = produit.PrixVente.Value;
            else
                prixArticle = produit.PrixDemande.Value;

            decimal prixNb = prixArticle * detail.Quantité.Value;

            strPdf += "<tr>";
            strPdf += $"<td>{detail.Quantité}</td>";
            strPdf += $"<td>{produit.Nom}</td>";
            strPdf += $"<td>{produit.Poids}</td>";
            strPdf += $"<td style=\"text-align: right;\">{prixArticle.ToString("N2")}$</td>";
            strPdf += $"<td style=\"text-align: right;\">{prixNb.ToString("N2")}$</td>";
            strPdf += "</tr>";
        }

        decimal montantTotAvantTaxes = commande.MontantTotAvantTaxes.Value;
        decimal coutLivraison = commande.CoutLivraison.Value;
        decimal tps = commande.TPS.Value;
        decimal tvq = commande.TVQ.Value;

        strPdf += "</table>";
        strPdf += "<br/><br/>";
        strPdf += "<table>";
        strPdf += "<tr>";
        strPdf += "<td>Total avant taxes</td>";
        strPdf += $"<td style=\"text-align: right\">{montantTotAvantTaxes.ToString("N2")}$</td>";
        strPdf += "</tr>";
        strPdf += "<tr>";
        strPdf += "<td>Prix livraison</td>";
        strPdf += $"<td style=\"text-align: right\">{coutLivraison.ToString("N2")}$</td>";
        strPdf += "</tr>";
        strPdf += "<tr>";
        strPdf += "<td>Tps</td>";
        strPdf += $"<td style=\"text-align: right\">{tps.ToString("N2")}$</td>";
        strPdf += "</tr>";
        strPdf += "<tr>";
        strPdf += "<td>Tvq</td>";
        strPdf += $"<td style=\"text-align: right\">{tvq.ToString("N2")}$</td>";
        strPdf += "</tr>";
        strPdf += "<tr style=\"font-weight: bold;\">";
        strPdf += "<td>Total après taxes</td>";
        strPdf += $"<td style=\"text-align: right\">{(montantTotAvantTaxes + coutLivraison + tps + tvq).ToString("N2")}$</td>";
        strPdf += "</tr>";
        strPdf += "</table>";

        HtmlGenericControl div = new HtmlGenericControl("div")
        {
            InnerHtml = strPdf
        };

        // Générer la facture
        PdfDocument pdf = PdfGenerator.GeneratePdf(strPdf, PageSize.Letter);
        pdf.Save(HttpContext.Current.Server.MapPath($"~/Factures/{commande.NoCommande}.pdf"));

        return this;
    }

    public GestionPDF SaveDB()
    {
        if (articleEnPaniersASupprimer != null && historique != null)
        {
            // Envoyer les changements
            db.commandes.Add(commande);
            db.historiquePaiements.Add(historique);

            db.detailsCommandes.AddAll(detailsCommande);
            db.produits.Update();

            // Supprimer les articles du panier
            db.articlesEnPanier.Remove(articleEnPaniersASupprimer);
        }

        return this;
    }
}