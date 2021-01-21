using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class VendeurClient : SqlEntity
{
    // IDs
    [SqlID]
    public long? NoVendeur { get; set; }
    [SqlID]
    public long? NoClient { get; set; }
    [SqlID]
    public DateTime? DateVisite { get; set; }

    public VendeurClient(SqlDataReader reader) => Init(reader);
}