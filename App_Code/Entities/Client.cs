using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Reflection;

public enum ClientStatut
{
    None = -99, Disabled = -1, Enabled = 1
}

public class Client : SqlEntity
{
    // ID
    [SqlID]
    public long? NoClient { get; set; }
    // Columns
    public string AdresseEmail { get; set; }
    public string MotDePasse { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Rue { get; set; }
    public string Ville { get; set; }
    public string Province { get; set; }
    public string CodePostal { get; set; }
    public string Pays { get; set; }
    public string Tel1 { get; set; }
    public string Tel2 { get; set; }
    public DateTime? DateCreation { get; set; }
    public DateTime? DateMAJ { get; set; }
    public short? NbConnexions { get; set; }
    public DateTime? DateDerniereConnexion { get; set; }
    public short? Statut { get; set; }

    public Client(SqlDataReader reader) => Init(reader);

    public ClientStatut StatutEnum()
    {
        ClientStatut stat;

        if (Statut != null)
            stat = (ClientStatut)Statut;
        else
            stat = ClientStatut.None;

        return stat;
    }
}
