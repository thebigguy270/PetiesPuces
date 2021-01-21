using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PPCommandes : SqlDAOAttributes<Commande>
{
    public long NextID() => Values.Max(x => x.NoCommande.Value) + 1;
}
