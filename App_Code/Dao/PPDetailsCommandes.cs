using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PPDetailsCommandes : SqlDAOAttributes<DetailsCommandes>
{
    public long NextID() => Values.Max(x => x.NoDetailCommandes.Value) + 1;
}
