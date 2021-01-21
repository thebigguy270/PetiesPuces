using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class DetailsCommandes : SqlEntity
{
    // ID
    [SqlID]
    public long? NoDetailCommandes { get; set; }
    // Columns
    public long? NoCommande { get; set; }
    public long? NoProduit { get; set; }
    public decimal? PrixVente { get; set; }
    public short? Quantité { get; set; }

    public DetailsCommandes(SqlDataReader reader) => Init(reader);
}