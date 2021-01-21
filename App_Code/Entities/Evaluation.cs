using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Evaluation : SqlEntity
{
    // IDs
    [SqlID]
    public long? NoClient { get; set; }
    [SqlID]
    public long? NoProduit { get; set; }
    // Columns
    public decimal? Cote { get; set; }
    public string Commentaire { get; set; }
    public DateTime? DateMAJ { get; set; }
    public DateTime? DateCreation { get; set; }

    public Evaluation(SqlDataReader reader) => Init(reader);
}