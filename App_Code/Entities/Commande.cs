using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Commande : SqlEntity
{
    // ID
    [SqlID]
    public long? NoCommande { set; get; }
    // Columns
    public long? NoClient { set; get; }
    public long? NoVendeur { set; get; }
    public DateTime? DateCommande { set; get; }
    public decimal? CoutLivraison { set; get; }
    public short? TypeLivraison { set; get; }
    public decimal? MontantTotAvantTaxes { set; get; }
    public decimal? TPS { set; get; }
    public decimal? TVQ { set; get; }
    public decimal? PoidsTotal { set; get; }
    public string Statut { set; get; }
    public string NoAutorisation { set; get; }

    public Commande(SqlDataReader reader) => Init(reader);
}