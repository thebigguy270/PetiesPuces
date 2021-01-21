using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Message : SqlEntity
{
    // ID
    [SqlID]
    public int? NoMsg { get; set; }
    // Columns
    public int? NoExpediteur { get; set; }
    public string DescMsg { get; set; }
    public string FichierJoint { get; set; }
    public short? Lieu { get; set; }
    public DateTime? dateEnvoi { get; set; }
    public string objet { get; set; }

    public Message(SqlDataReader reader) => Init(reader);
    

}