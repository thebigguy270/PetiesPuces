using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

[WebService(Namespace = "http://424p.cgodin.qc.ca/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[ScriptService]
public class TestService : System.Web.Services.WebService
{
    public TestService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public void SupprimerArticle(long DelArticle)
    {
        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        bool del = articlesEnPanier.RemoveById(DelArticle);

        Context.Response.Clear();
        Context.Response.ContentType = "application/text";
        Context.Response.AddHeader("content-length", del.ToString().Length.ToString());
        Context.Response.Flush();

        Context.Response.Write(del);
    }

    [WebMethod]
    public void SupprimerPanier(long NoClient, long NoVendeur)
    {
        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        bool del = articlesEnPanier.RemoveByClientAndVendeur(NoClient, NoVendeur);

        Context.Response.Clear();
        Context.Response.ContentType = "application/text";
        Context.Response.AddHeader("content-length", del.ToString().Length.ToString());
        Context.Response.Flush();

        Context.Response.Write(del);
    }

    [WebMethod]
    public void ChangerQuantite(long NoPanier, short NewNbItems)
    {
        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
        ArticleEnPanier article = articlesEnPanier.Values.Find(x => x.NoPanier == NoPanier);
        article.NbItems = NewNbItems;

        articlesEnPanier.NotifyUpdated(article);
        articlesEnPanier.Update();
    }

    [WebMethod]
    public void AjouterPanier(long NoClient, long NoProduit, short NbItems)
    {
        string ret = "OK";

        PPProduits produits = new PPProduits();
        PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();

        Produit produit = produits.Values.Find(x => x.NoProduit == NoProduit);

        ArticleEnPanier article = articlesEnPanier.Values.Find(x => x.NoProduit == produit.NoProduit && x.NoClient == NoClient);

        if (article != null)
        {
            int newNbItems = article.NbItems.Value + NbItems;
            if (newNbItems <= produit.NombreItems)
            {
                article.NbItems = (short)newNbItems;

                articlesEnPanier.NotifyUpdated(article);
                articlesEnPanier.Update();
            }
            else
            {
                ret = "ERREUR;Impossible d'ajouter au panier";
            }
        }
        else
        {
            ArticleEnPanier newArticle = new ArticleEnPanier(null)
            {
                NoPanier = articlesEnPanier.NextId(NoClient, produit.NoVendeur.Value),
                NoClient = NoClient,
                NoVendeur = produit.NoVendeur,
                NoProduit = produit.NoProduit,
                DateCreation = DateTime.Now,
                NbItems = NbItems
            };

            articlesEnPanier.Add(newArticle);
        }

        Context.Response.Clear();
        Context.Response.ContentType = "application/text";
        Context.Response.AddHeader("content-length", ret.Length.ToString());
        Context.Response.Flush();

        Context.Response.Write(ret);
    }
}
