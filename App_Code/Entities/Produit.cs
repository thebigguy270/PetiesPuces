using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

public class Produit : SqlEntity
{
    // ID
    [SqlID]
    public long? NoProduit { get; set; }
    // Columns
    public long? NoVendeur { get; set; }
    public int? NoCategorie { get; set; }
    public string Nom { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public decimal? PrixDemande { get; set; }
    public short? NombreItems { get; set; }
    // Oui, y'a un accent dans la base de donnée
    public bool? Disponibilité { get; set; }
    public DateTime? DateVente { get; set; }
    public decimal? PrixVente { get; set; }
    public decimal? Poids { get; set; }
    public DateTime? DateCreation { get; set; }
    public DateTime? DateMAJ { get; set; }

    public Produit(SqlDataReader reader) => Init(reader);
}
