using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Categorie : SqlEntity
{
    // ID
    [SqlID]
    public int? NoCategorie { get; set; }
    // Columns
    public string Description { get; set; }
    public string Details { get; set; }

    public Categorie(SqlDataReader reader) => Init(reader);
}