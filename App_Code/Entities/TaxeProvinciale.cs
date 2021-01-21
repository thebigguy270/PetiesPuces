using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class TaxeProvinciale : SqlEntity
{
    // ID
    [SqlID]
    public byte? NoTVQ { set; get; }
    // Columns
    public DateTime? DateEffectiveTVQ { set; get; }
    public decimal? TauxTVQ { set; get; }

    public TaxeProvinciale(SqlDataReader reader) => Init(reader);
}