using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class TaxeFederale : SqlEntity
{
    // ID
    [SqlID]
    public byte? NoTPS { set; get; }
    // Columns
    public DateTime? DateEffectiveTPS { set; get; }
    public decimal? TauxTPS { set; get; }

    public TaxeFederale(SqlDataReader reader) => Init(reader);
}