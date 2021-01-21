using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;

public class Lieu : SqlEntity
{
    // ID
    [SqlID]
    public short? NoLieu { get; set; }
    // Column
    public string Description { get; set; }

    public Lieu(SqlDataReader reader) => Init(reader);
}