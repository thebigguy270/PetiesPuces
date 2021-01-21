using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Destinataire : SqlEntity
{
    // IDs
    [SqlID]
    public int? NoMsg { get; set; }
    [SqlID]
    public int? NoDestinataire { get; set; }
    // Columns
    public short? EtatLu { get; set; }
    public short? Lieu { get; set; }

    public Destinataire(SqlDataReader reader) => Init(reader);
}