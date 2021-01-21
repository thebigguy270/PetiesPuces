using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;

public class TypePoids : SqlEntity
{
    // ID
    [SqlID]
    public short? CodePoids { get; set; }
    // Columns
    public decimal? PoidsMin { get; set; }
    public decimal? PoidsMax { get; set; }

    public TypePoids(SqlDataReader reader) => Init(reader);
}