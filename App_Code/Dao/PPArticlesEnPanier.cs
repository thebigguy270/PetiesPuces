using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PPArticlesEnPanier : SqlDAOAttributes<ArticleEnPanier>
{
    public List<ArticleEnPanier> ArticlesByClientAndVendeur(long NoClient, long NoVendeur)
        => Values.Where(x => x.NoClient == NoClient && x.NoVendeur == NoVendeur).ToList();

    public long NextId(List<ArticleEnPanier> lstPanier)
    {
        long ret;

        if (lstPanier.Count == 0)
        {
            var maxAll = Values.Max(x => x.NoPanier.Value);
            long maxPanier = (long)Math.Floor(maxAll / 1000.00);
            ret = ((maxPanier + 1) * 1000) + 1;
        }
        else
            ret = lstPanier.Max(x => x.NoPanier.Value) + 1;

        return ret;
    }

    public long NextId(long NoClient, long NoVendeur) => NextId(ArticlesByClientAndVendeur(NoClient, NoVendeur));

    public bool RemoveById(long id)
    {
        ArticleEnPanier article = Values.Find(x => x.NoPanier == id);
        bool ret = article != null;
        if (ret)
            Remove(article);

        return ret;
    }

    public bool RemoveByClientAndVendeur(long noClient, long noVendeur)
    {
        List<ArticleEnPanier> lstDelete = Values.FindAll(x => x.NoClient == noClient && x.NoVendeur == noVendeur);
        bool del = lstDelete.Count > 0;

        if (del)
            del = Remove(lstDelete);

        return del;
    }
}
