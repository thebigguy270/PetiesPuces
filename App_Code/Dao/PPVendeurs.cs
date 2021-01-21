using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PPVendeurs : SqlDAOAttributes<Vendeur>
{
    public long NextId() => Values.Max(x => x.NoVendeur.Value) + 1;
}
