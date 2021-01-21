using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Tests : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PPClients clients = new PPClients();

        //Client client2 = clients.Values.Find(x => x.NoClient == 10600);
        //lblTests.Text = client2.Nom;

        /*clients.Add(new Client(null)
        {
            NoClient = clients.NextId()
        });*/
        //clients.Remove(clients.Values.Find(x => x.NoClient == 10021));

        /*foreach(Client client in clients.Values)
        {
            lblTests.Text += $"{client.NoClient} {client.Nom} {client.Prenom} <br />";
            lblTests.Text += string.Join(", ", client.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }*/

        /*
        PPVendeurs vendeurs = new PPVendeurs();

        foreach (Vendeur vendeur in vendeurs.Vendeurs)
        {
            lblTests.Text += $"{vendeur.NoVendeur} {vendeur.Nom} {vendeur.Prenom} <br />";
            lblTests.Text += string.Join(", ", vendeur.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }

        PPProduits produits = new PPProduits();

        foreach (Produit produit in produits.Produits)
        {
            lblTests.Text += $"{produit.NoProduit} {produit.Nom} <br />";
            lblTests.Text += string.Join(", ", produit.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }*/

        /*PPTaxeFederale taxeFederale = new PPTaxeFederale();

        foreach(TaxeFederale tps in taxeFederale.TaxeFederale)
        {
            lblTests.Text += $"{tps.NoTPS} {tps.TauxTPS} {tps.DateEffectiveTPS} <br />";
            lblTests.Text += string.Join(", ", tps.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }

        PPTaxeProvinciale taxeProvinciale = new PPTaxeProvinciale();

        foreach(TaxeProvinciale tps in taxeProvinciale.TaxeProvinciale)
        {
            lblTests.Text += $"{tps.NoTVQ} {tps.TauxTVQ} {tps.DateEffectiveTVQ} <br />";
            lblTests.Text += string.Join(", ", tps.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }

        PPCategories categories = new PPCategories();

        foreach(Categorie categorie in categories.Categories)
        {
            lblTests.Text += $"{categorie.NoCategorie} {categorie.Description} {categorie.Details} <br />";
            lblTests.Text += string.Join(", ", categorie.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }

        PPHistoriquePaiements historique = new PPHistoriquePaiements();

        foreach(HistoriquePaiement hist in historique.HistoriquePaiements)
        {
            lblTests.Text += string.Join(", ", hist.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }*/

        /*PPCommandes commandes = new PPCommandes();


        foreach(Commande com in commandes.Values)
        {
            lblTests.Text += string.Join(", ", com.GetSqlFormatedValues());
            lblTests.Text += "<br />";
            lblTests.Text += "<br />";
        }*/

        //PPTypesLivraison typesLivraison = new PPTypesLivraison();

        // Ajouter UNE valeur à la fois
        /*typesLivraison.Add(new TypeLivraison(null)
        {
            CodeLivraison = 4,
            Description = "Test"
        });*/

        // Ajouter plusieurs valeurs
        // Version Séparée
        /*TypeLivraison[] typeLivraisons = new TypeLivraison[]
        {
            new TypeLivraison(null)
            {
                CodeLivraison = 4,
                Description = "Test1"
            },
            new TypeLivraison(null)
            {
                CodeLivraison = 5,
                Description = "Test2"
            }
        };

        typesLivraison.AddAll(typeLivraisons);*/

        // Version List
        /*List<TypeLivraison> lstTypeLivraisons = new List<TypeLivraison>();
        lstTypeLivraisons.Add(
            new TypeLivraison(null)
            {
                CodeLivraison = 4,
                Description = "Test1"
            });
        lstTypeLivraisons.Add(
            new TypeLivraison(null)
            {
                CodeLivraison = 5,
                Description = "Test2"
            });

        typesLivraison.AddAll(lstTypeLivraisons);*/

        // Version Compacte
        /*typesLivraison.AddAll(
            new TypeLivraison[]
            {
                new TypeLivraison(null)
                {
                    CodeLivraison = 4,
                    Description = "Test1"
                },
                new TypeLivraison(null)
                {
                    CodeLivraison = 5,
                    Description = "Test2"
                }
            });*/

        //typesLivraison.Remove(typesLivraison.Values.Find(x => x.CodeLivraison == 4));
        //typesLivraison.Remove(typesLivraison.Values.Find(x => x.CodeLivraison == 5));
        //typesLivraison.RemoveById(4);
        //typesLivraison.RemoveById(5);
        //typesLivraison.RemoveByIds(new short[] { 4, 5 });

        PPPoidsLivraisons poidsLivraisons = new PPPoidsLivraisons();

        /*poidsLivraisons.Add(new PoidsLivraison(null)
        {
            CodePoids = 50,
            CodeLivraison = 3,
            Tarif = 666
        });*/

        /*PoidsLivraison newPoids = poidsLivraisons.Values.Find(x => x.CodeLivraison == 3 && x.CodePoids == 50);

        newPoids.Tarif = 1234;

        poidsLivraisons.NotifyUpdated(newPoids);

        poidsLivraisons.Update();*/

        //poidsLivraisons.Remove(poidsLivraisons.Values.Find(x => x.CodeLivraison == 3 && x.CodePoids == 50));

        /*foreach(PoidsLivraison poids in poidsLivraisons.Values)
        {
            lblTests.Text += $"<br/> {poids.CodeLivraison} {poids.CodePoids} {poids.Tarif}";
        }*/

        /*PPEvaluations evaluations = new PPEvaluations();

        foreach (Evaluation ev in evaluations.Values)
            lblTests.Text += $"<br /> {ev.NoClient} {ev.Commentaire}";*/

        //lblTests.Text = clients.GetNameTest();

        /*PPProduits produits = new PPProduits();
        PPCategories categories = new PPCategories();
        PPVendeurs vendeurs = new PPVendeurs();

        var testCat = from produit in produits.Values
                      join vendeur in vendeurs.Values on produit.NoVendeur equals vendeur.NoVendeur
                      select new { Produit = produit, Vendeur = vendeur };

        var ah = from categorie in categories.Values
                 let v = testCat.Where(x => x.Produit.NoCategorie == categorie.NoCategorie).DistinctBy(x => x.Vendeur.NoVendeur)
                 select new { Categorie = categorie, Vendeurs = v };

        foreach(var l in ah)
        {
            lblTests.Text += l.Categorie.Description + "<br/>";
            foreach (var v in l.Vendeurs)
                lblTests.Text += "&nbsp;&nbsp;&nbsp;&nbsp;" + v.Vendeur.NomAffaires + "<br/>";
        }*/

        string test1 = "1.1";
        string test2 = "1,1";

        decimal dec1 = decimal.Parse(test1);
        decimal dec2 = decimal.Parse(test2);

        decimal dec11 = test1.ParseDecimal();
        decimal dec12 = test2.ParseDecimal();
    }
}