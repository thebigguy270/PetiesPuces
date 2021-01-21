using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;

public class PoidsLivraison : SqlEntity
{
    // IDs
    [SqlID]
    public short? CodeLivraison { get; set; }
    [SqlID]
    public short? CodePoids { get; set; }
    // Column
    public decimal? Tarif { get; set; }

    public PoidsLivraison(SqlDataReader reader) => Init(reader);
}