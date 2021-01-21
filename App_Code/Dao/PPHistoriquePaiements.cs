using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PPHistoriquePaiements : SqlDAOAttributes<HistoriquePaiement>
{
    public long NextID() => Values.Max(x => x.NoHistorique.Value) + 1;
}
