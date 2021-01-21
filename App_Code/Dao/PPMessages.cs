using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PPMessages : SqlDAOAttributes<Message>
{
    public long NextId() => Values.Max(x => x.NoMsg.Value) + 1;
}
