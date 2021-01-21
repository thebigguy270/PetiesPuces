using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;

public class TypeLivraison : SqlEntity
{
    // ID
    [SqlID]
    public short? CodeLivraison { get; set; }
    // Column
    public string Description { get; set; }

    public TypeLivraison(SqlDataReader reader) => Init(reader);
}