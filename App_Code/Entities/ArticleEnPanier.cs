using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class ArticleEnPanier : SqlEntity
{
    // ID
    [SqlID]
    public long? NoPanier { get; set; }
    // Columns
    public long? NoClient { get; set; }
    public long? NoVendeur { get; set; }
    public long? NoProduit { get; set; }
    public DateTime? DateCreation { get; set; }
    public short? NbItems { get; set; }

    public ArticleEnPanier(SqlDataReader reader) => Init(reader);
}