using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Tests : System.Web.UI.Page
{
    DateTime? nullableDate = null;
    int? nullableInt = null;
    protected void CreerDatabase(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        XDocument docCollege = XDocument.Load(Server.MapPath("~/LayoutXml.xml"));
        XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";
        foreach (XElement worksheet in docCollege.Descendants(ss + "Worksheet"))
        {
            if (worksheet.Attribute(ss + "Name").Value.Equals("PPVendeurs"))
            {
                remplirVendeur(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPClients"))
            {
                remplirClients(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPCategories"))
            {
                remplirCategories(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPProduits"))
            {
                remplirProduits(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPArticlesEnPanier"))
            {
                remplirArticlesEnPanier(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPTypesLivraison"))
            {
                remplirTypesLivraison(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPTypesPoids"))
            {
                remplirTypesPoids(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPPoidsLivraisons"))
            {
                remplirPoidsLivraisons(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPCommandes"))
            {
                remplirCommandes(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPDetailsCommandes"))
            {
                remplirDetailsCommandes(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPHistoriquePaiements"))
            {
                remplirHistoriquePaiements(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPVendeursClients"))
            {
                remplirVendeursClients(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPTaxeProvinciale"))
            {
                remplirTaxeProvinciale(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPTaxeFederale"))
            {
                remplirTaxeFederale(ss, worksheet);
            }
            else if (worksheet.Attribute(ss + "Name").Value.Equals("PPGestionnaires"))
            {
                remplirGestionnaires(ss, worksheet);
            }
            
        }
        afficherLesDonnees();
        FUNCGenererPDF.GenererPDF();
    }
    protected void remplirVendeur(XNamespace ss, XElement worksheet)
    {
        PPVendeurs vendeurs = new PPVendeurs();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }
            //Response.Write(arrayStr.Count());
            if (arrayStr.Count() == 21)
            {
                vendeurs.Add(new Vendeur(null)
                {
                    NoVendeur = Convert.ToInt64(arrayStr[0]),
                    NomAffaires = arrayStr[1],
                    Nom = arrayStr[2],
                    Prenom = arrayStr[3],
                    Rue = arrayStr[4],
                    Ville = arrayStr[5],
                    Province = arrayStr[6] == "NULL" ? "" : arrayStr[6],
                    CodePostal = arrayStr[7],
                    Pays = arrayStr[8] == "NULL" ? "":arrayStr[8],
                    Tel1 = arrayStr[9],
                    Tel2 = arrayStr[10],
                    AdresseEmail = arrayStr[11],
                    MotDePasse = arrayStr[12],
                    PoidsMaxLivraison = arrayStr[13] == "NULL" ? nullableInt : Convert.ToInt32(arrayStr[13]),
                    LivraisonGratuite = arrayStr[14] == "NULL" ? nullableInt : Convert.ToInt32(arrayStr[14]),
                    Taxes = (arrayStr[15] == "1" ? true : false),
                    Pourcentage = arrayStr[16] == "NULL" ? nullableInt:Convert.ToInt32(arrayStr[16]),
                    Configuration = arrayStr[17]=="NULL" ? null : arrayStr[17],
                    DateCreation =  DateTime.Parse(arrayStr[18]),
                    DateMAJ = arrayStr[19] == "NULL" ? nullableDate : DateTime.Parse(arrayStr[19]),
                    Statut = Convert.ToInt16(arrayStr[20])
                });
            }
            /*foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                //Response.Write(cell.Value);
            }*/
        }
    }
    protected void remplirClients(XNamespace ss, XElement worksheet)
    {
        PPClients clients = new PPClients();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }
            if (arrayStr.Count() == 17)
            {
                clients.Add(new Client(null)
                {
                    NoClient = Convert.ToInt64(arrayStr[0]),
                    AdresseEmail = arrayStr[1],
                    MotDePasse = arrayStr[2],
                    Nom = (arrayStr[3] == "NULL" ? null : arrayStr[3]),
                    Prenom = (arrayStr[4] == "NULL" ? null : arrayStr[4]),
                    Rue = (arrayStr[5] == "NULL" ? null : arrayStr[5]),
                    Ville = (arrayStr[6] == "NULL" ? null : arrayStr[6]),
                    Province = (arrayStr[7] == "NULL" ? null : arrayStr[7]),
                    CodePostal = (arrayStr[8] == "NULL" ? null : arrayStr[8]),
                    Pays = (arrayStr[9] == "NULL" ? null : arrayStr[9]),
                    Tel1 = (arrayStr[10] == "NULL" ? null : arrayStr[10]),
                    Tel2 = (arrayStr[11] == "NULL" ? null : arrayStr[11]),
                    DateCreation = DateTime.Parse(arrayStr[12]),
                    DateMAJ = (arrayStr[13] == "NULL" ? nullableDate : Convert.ToDateTime(arrayStr[13])),
                    NbConnexions = Convert.ToInt16(arrayStr[14]),
                    DateDerniereConnexion = DateTime.Parse(arrayStr[15]),
                    Statut = Convert.ToInt16(arrayStr[16])
                });
            }

        }
    }
    protected void remplirCategories(XNamespace ss, XElement worksheet)
    {
        PPCategories categories = new PPCategories();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 3)
            {
                categories.Add(new Categorie(null)
                {
                    NoCategorie = Convert.ToInt32(arrayStr[0]),
                    Description = arrayStr[1],
                    Details = arrayStr[2]
                });
            }

        }
    }
    protected void remplirProduits(XNamespace ss, XElement worksheet)
    {
        PPProduits produits = new PPProduits();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }
            DateTime? nullableDate = null;
            if (arrayStr.Count() == 14)
            {

                produits.Add(new Produit(null)
                {
                    NoProduit = Convert.ToInt64(arrayStr[0]),
                    NoVendeur = Convert.ToInt64(arrayStr[1]),
                    NoCategorie = Convert.ToInt32(arrayStr[2]),
                    Nom = arrayStr[3],
                    Description = arrayStr[4],
                    Photo = arrayStr[5],
                    PrixDemande = Convert.ToDecimal(arrayStr[6]),
                    NombreItems = Convert.ToInt16(arrayStr[7]),
                    Disponibilité = (arrayStr[8] == "1" ? true : false),
                    DateVente = (arrayStr[9] == "NULL" ? nullableDate : Convert.ToDateTime(arrayStr[9])),
                    PrixVente = Convert.ToDecimal(arrayStr[10]),
                    Poids = Convert.ToDecimal(arrayStr[11]),
                    DateCreation = DateTime.Parse(arrayStr[12]),
                    DateMAJ = (arrayStr[13] == "NULL" ? nullableDate : Convert.ToDateTime(arrayStr[13])),
                });
            }
        }
    }
    protected void remplirArticlesEnPanier(XNamespace ss, XElement worksheet)
    {
        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 6)
            {
                articlesEnPanier.Add(new ArticleEnPanier(null)
                {
                    NoPanier = Convert.ToInt64(arrayStr[0]),
                    NoClient = Convert.ToInt64(arrayStr[1]),
                    NoVendeur = Convert.ToInt64(arrayStr[2]),
                    NoProduit = Convert.ToInt64(arrayStr[3]),
                    DateCreation = Convert.ToDateTime(arrayStr[4]),
                    NbItems = Convert.ToInt16(arrayStr[5])
                });
            }

        }
    }
    protected void remplirCommandes(XNamespace ss, XElement worksheet)
    {
        PPCommandes commandes = new PPCommandes();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }
            if (arrayStr.Count() == 12)
            {

                commandes.Add(new Commande(null)
                {
                    NoCommande = Convert.ToInt64(arrayStr[0]),
                    NoClient = Convert.ToInt64(arrayStr[1]),
                    NoVendeur = Convert.ToInt32(arrayStr[2]),
                    DateCommande = DateTime.Parse(arrayStr[3]),
                    CoutLivraison = Convert.ToDecimal(arrayStr[4]),
                    TypeLivraison = Convert.ToInt16(arrayStr[5]),
                    MontantTotAvantTaxes = Convert.ToDecimal(arrayStr[6]),
                    TPS = Convert.ToDecimal(arrayStr[7]),
                    TVQ = Convert.ToDecimal(arrayStr[8]),
                    PoidsTotal = Convert.ToDecimal(arrayStr[9]),
                    Statut = arrayStr[10],
                    NoAutorisation = arrayStr[11]
                });
            }
        }
    }
    protected void remplirTypesLivraison(XNamespace ss, XElement worksheet)
    {
        PPTypesLivraison typesLivraison = new PPTypesLivraison();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 2)
            {
                typesLivraison.Add(new TypeLivraison(null)
                {
                    CodeLivraison = Convert.ToInt16(arrayStr[0]),
                    Description = arrayStr[1]
                });
            }

        }
    }
    protected void remplirTypesPoids(XNamespace ss, XElement worksheet)
    {
        PPTypesPoids typesPoids = new PPTypesPoids();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 3)
            {
                typesPoids.Add(new TypePoids(null)
                {
                    CodePoids = Convert.ToInt16(arrayStr[0]),
                    PoidsMin = Convert.ToDecimal(arrayStr[1]),
                    PoidsMax = Convert.ToDecimal(arrayStr[2])
                });
            }

        }
    }
    protected void remplirPoidsLivraisons(XNamespace ss, XElement worksheet)
    {
        PPPoidsLivraisons poidsLivraisons = new PPPoidsLivraisons();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 3)
            {
                poidsLivraisons.Add(new PoidsLivraison(null)
                {
                    CodeLivraison = Convert.ToInt16(arrayStr[0]),
                    CodePoids = Convert.ToInt16(arrayStr[1]),
                    Tarif = Convert.ToDecimal(arrayStr[2])
                });
            }

        }
    }
    protected void remplirDetailsCommandes(XNamespace ss, XElement worksheet)
    {
        PPDetailsCommandes detailsCommandes = new PPDetailsCommandes();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }
            if (arrayStr.Count() == 5)
            {

                detailsCommandes.Add(new DetailsCommandes(null)
                {
                    NoDetailCommandes = Convert.ToInt64(arrayStr[0]),
                    NoCommande = Convert.ToInt64(arrayStr[1]),
                    NoProduit = Convert.ToInt32(arrayStr[2]),
                    PrixVente = Convert.ToDecimal(arrayStr[3]),
                    Quantité = Convert.ToInt16(arrayStr[4])
                });
            }
        }
    }
    protected void remplirHistoriquePaiements(XNamespace ss, XElement worksheet)
    {
        PPHistoriquePaiements historiquePaiements = new PPHistoriquePaiements();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }
            if (arrayStr.Count() == 12)
            {
                historiquePaiements.Add(new HistoriquePaiement(null)
                {
                    NoHistorique = Convert.ToInt64(arrayStr[0]),
                    MontantVenteAvantLivraison = Convert.ToDecimal(arrayStr[1]),
                    NoVendeur = Convert.ToInt32(arrayStr[2]),
                    NoClient = Convert.ToInt64(arrayStr[3]),
                    NoCommande = Convert.ToInt64(arrayStr[4]),
                    DateVente = Convert.ToDateTime(arrayStr[5]),
                    NoAutorisation = arrayStr[6],
                    FraisLesi = Convert.ToDecimal(arrayStr[7]),
                    Redevance = Convert.ToDecimal(arrayStr[8]),
                    FraisLivraison = Convert.ToDecimal(arrayStr[9]),
                    FraisTPS = Convert.ToDecimal(arrayStr[10]),
                    FraisTVQ = Convert.ToDecimal(arrayStr[11])
                });
            }
        }
    }
    protected void remplirVendeursClients(XNamespace ss, XElement worksheet)
    {
        PPVendeursClients vendeursClients = new PPVendeursClients();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 3)
            {
                vendeursClients.Add(new VendeurClient(null)
                {
                    NoVendeur = Convert.ToInt16(arrayStr[0]),
                    NoClient = Convert.ToInt16(arrayStr[1]),
                    DateVisite = Convert.ToDateTime(arrayStr[2])
                });
            }

        }
    }
    protected void remplirTaxeProvinciale(XNamespace ss, XElement worksheet)
    {
        PPTaxeProvinciale taxeProvinciale = new PPTaxeProvinciale();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 3)
            {
                taxeProvinciale.Add(new TaxeProvinciale(null)
                {
                    NoTVQ = Convert.ToByte(arrayStr[0]),
                    DateEffectiveTVQ = Convert.ToDateTime(arrayStr[1]),
                    TauxTVQ = Convert.ToDecimal(arrayStr[2])
                });
            }

        }
    }
    protected void remplirTaxeFederale(XNamespace ss, XElement worksheet)
    {
        PPTaxeFederale taxeFederale = new PPTaxeFederale();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 3)
            {
                taxeFederale.Add(new TaxeFederale(null)
                {
                    NoTPS = Convert.ToByte(arrayStr[0]),
                    DateEffectiveTPS = Convert.ToDateTime(arrayStr[1]),
                    TauxTPS = Convert.ToDecimal(arrayStr[2])
                });
            }

        }
    }
    protected void remplirGestionnaires(XNamespace ss, XElement worksheet)
    {
        PPGestionnaires gestionnaires = new PPGestionnaires();
        foreach (XElement row in worksheet.Descendants(ss + "Row"))
        {
            List<String> arrayStr = new List<string>();
            foreach (XElement cell in row.Descendants(ss + "Data"))
            {
                arrayStr.Add(cell.Value);
            }

            if (arrayStr.Count() == 6)
            {
                gestionnaires.Add(new Gestionnaire(null)
                {
                    NoAdmin = Convert.ToInt32(arrayStr[0]),
                    Email = arrayStr[1],
                    MotDePasse = arrayStr[2],
                    DateCreation = Convert.ToDateTime(arrayStr[3]),
                    Prenom = arrayStr[4],
                    Nom = arrayStr[5]
                });
            }

        }
    }
    protected void ViderBD()
    {
        PPGestionnaires gestionnaires = new PPGestionnaires();
        gestionnaires.RemoveAll();
        PPDetailsCommandes detailsCommandes = new PPDetailsCommandes();
        detailsCommandes.RemoveAll();
        PPCommandes commandes = new PPCommandes();
        commandes.RemoveAll();
        PPPoidsLivraisons poidLivraison = new PPPoidsLivraisons();
        poidLivraison.RemoveAll();
        PPTypesLivraison typeLivraison = new PPTypesLivraison();
        typeLivraison.RemoveAll();
        PPTypesPoids typePoids = new PPTypesPoids();
        typePoids.RemoveAll();
        PPVendeursClients vendeursClients = new PPVendeursClients();
        vendeursClients.RemoveAll();
        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        articlesEnPanier.RemoveAll();
        PPClients clients = new PPClients();
        clients.RemoveAll();
        PPProduits produits = new PPProduits();
        produits.RemoveAll();
        PPVendeurs vendeurs = new PPVendeurs();
        vendeurs.RemoveAll();
        PPCategories categories = new PPCategories();
        categories.RemoveAll();
        PPHistoriquePaiements historique = new PPHistoriquePaiements();
        historique.RemoveAll();
        PPTaxeFederale taxeFed = new PPTaxeFederale();
        taxeFed.RemoveAll();
        PPTaxeProvinciale taxeProv = new PPTaxeProvinciale();
        taxeProv.RemoveAll();
        afficherLesDonnees();
    }
    protected void viderLaDBClick(object sender, EventArgs e)
    {
        ViderBD();
        afficherLesDonnees();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsAdmin())
            Response.Redirect(SessionManager.RedirectConnexionLink);

        afficherLesDonnees();
    }
    protected void afficherLesDonnees()
    {
        PPGestionnaires gestionnaires = new PPGestionnaires();
        lblgestionnaires.Text = gestionnaires.Values.Count.ToString() + " donnée(s)";
        PPDetailsCommandes detailsCommandes = new PPDetailsCommandes();
        lbldetailscommandes.Text = detailsCommandes.Values.Count.ToString() + " donnée(s)";
        PPCommandes commandes = new PPCommandes();
        lblcommandes.Text = commandes.Values.Count.ToString() + " donnée(s)";
        PPPoidsLivraisons poidLivraison = new PPPoidsLivraisons();
        lblpoidslivraisons.Text = poidLivraison.Values.Count.ToString() + " donnée(s)";
        PPTypesLivraison typeLivraison = new PPTypesLivraison();
        lbltypeslivraison.Text = typeLivraison.Values.Count.ToString() + " donnée(s)";
        PPTypesPoids typePoids = new PPTypesPoids();
        lbltypespoids.Text = typePoids.Values.Count.ToString() + " donnée(s)";
        PPVendeursClients vendeursClients = new PPVendeursClients();
        lblvendeursclients.Text = vendeursClients.Values.Count.ToString() + " donnée(s)";
        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        lblarticlesenpanier.Text = articlesEnPanier.Values.Count.ToString() + " donnée(s)";
        PPClients clients = new PPClients();
        lblclients.Text = clients.Values.Count.ToString() + " donnée(s)";
        PPProduits produits = new PPProduits();
        lblproduits.Text = produits.Values.Count.ToString() + " donnée(s)";
        PPVendeurs vendeurs = new PPVendeurs();
        lblvendeurs.Text = vendeurs.Values.Count.ToString() + " donnée(s)";
        PPCategories categories = new PPCategories();
        lblcategories.Text = categories.Values.Count.ToString() + " donnée(s)";
        PPHistoriquePaiements historique = new PPHistoriquePaiements();
        lblhistoriquepaiements.Text = historique.Values.Count.ToString() + " donnée(s)";
        PPTaxeFederale taxeFed = new PPTaxeFederale();
        lbltaxefederale.Text = taxeFed.Values.Count.ToString() + " donnée(s)";
        PPTaxeProvinciale taxeProv = new PPTaxeProvinciale();
        lbltaxeprovinciale.Text = taxeProv.Values.Count.ToString() + " donnée(s)";
        if ((gestionnaires.Values.Count > 0) || (detailsCommandes.Values.Count > 0) || (commandes.Values.Count > 0) || (poidLivraison.Values.Count > 0) || (typeLivraison.Values.Count > 0) ||
            (typePoids.Values.Count > 0) || (vendeursClients.Values.Count > 0) || (articlesEnPanier.Values.Count > 0) || (clients.Values.Count > 0) || (produits.Values.Count > 0) ||
            (vendeurs.Values.Count > 0) || (categories.Values.Count > 0) || (historique.Values.Count > 0) || (taxeFed.Values.Count > 0) || (taxeProv.Values.Count > 0))
        {
            idCreerDatabase.Enabled = false;
            idViderDatabase.Enabled = true;
        }
        else
        {
            idCreerDatabase.Enabled = true;
            idViderDatabase.Enabled = false;
        }
    }
}