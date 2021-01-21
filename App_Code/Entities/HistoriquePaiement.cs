using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;

public class HistoriquePaiement : SqlEntity
{
    // ID
    [SqlID]
    public long? NoHistorique { get; set; }
    // Columns
    public decimal? MontantVenteAvantLivraison { get; set; }
    // ---
    // Foreign IDs (Not connected)
    public long? NoVendeur { get; set; }
    public long? NoClient { get; set; }
    public long? NoCommande { get; set; }
    // ---
    public DateTime? DateVente { get; set; }
    public string NoAutorisation { get; set; }
    public decimal? FraisLesi { get; set; }
    public decimal? Redevance { get; set; }
    public decimal? FraisLivraison { get; set; }
    public decimal? FraisTPS { get; set; }
    public decimal? FraisTVQ { get; set; }

    public HistoriquePaiement(SqlDataReader reader) => Init(reader);
}