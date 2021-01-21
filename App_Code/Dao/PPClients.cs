using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

using System.Collections.Specialized;

public class PPClients : SqlDAOAttributes<Client>
{
    public long NextId() => Values.Max(x => x.NoClient.Value) + 1;
}
