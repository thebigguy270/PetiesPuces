using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PPDestinataires : SqlDAOAttributes<Destinataire>
{
    public int CountMessagesNonLu(long noDest)
        => Values.Where(x => x.NoDestinataire.Value == noDest && x.EtatLu == 1 && x.Lieu == 1).Count();
}
