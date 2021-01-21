using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Reflection;

public enum VendeurStatut
{
    None = -99, Disabled = -1, Pending = 0, Enabled = 1
}

public class Vendeur : SqlEntity
{
    // ID
    [SqlID]
    public long? NoVendeur { set; get; }
    // Columns
    public string NomAffaires { set; get; }
    public string Nom { set; get; }
    public string Prenom { set; get; }
    public string Rue { set; get; }
    public string Ville { set; get; }
    public string Province { set; get; }
    public string CodePostal { set; get; }
    public string Pays { set; get; }
    public string Tel1 { set; get; }
    public string Tel2 { set; get; }
    public string AdresseEmail { set; get; }
    public string MotDePasse { set; get; }
    public int? PoidsMaxLivraison { set; get; }
    public decimal? LivraisonGratuite { set; get; }
    public bool? Taxes { set; get; }
    public decimal? Pourcentage { set; get; }
    public string Configuration { set; get; }
    public DateTime? DateCreation { set; get; }
    public DateTime? DateMAJ { set; get; }
    public short? Statut { set; get; }

    public Vendeur(SqlDataReader reader) => Init(reader);

    public VendeurStatut StatutEnum()
    {
        VendeurStatut stat;

        if (Statut != null)
            stat = (VendeurStatut)Statut;
        else
            stat = VendeurStatut.None;

        return stat;
    }
}
