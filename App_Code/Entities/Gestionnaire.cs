using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Gestionnaire : SqlEntity
{
    // ID
    [SqlID]
    public int? NoAdmin { get; set; }
    // Columns
    public string Email { get; set; }
    public string MotDePasse { get; set; }
    public DateTime? DateCreation { get; set; }
    public string Prenom { get; set; }
    public string Nom { get; set; }

    public Gestionnaire(SqlDataReader reader) => Init(reader);
}