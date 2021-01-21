using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Vendeur_InfosClients : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        fillingRow(tableContenu, "Numéro", "10000");
        fillingRow(tableContenu, "Nom", "Martine Beaupré");
        fillingRow(tableContenu, "Province", "Québec");
        fillingRow(tableContenu, "Téléphone", "(450) 444-1231");
        fillingRow(tableContenu, "Courriel", "mbeaupre@gmail.com");
    }

    public void retour(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Vendeur/GestionCommandes.aspx");
    }

    public void fillingRow(Table tableRef, string description, string info)
    {
        TableRow row= tableRef.TrDyn();
        TableCell cell1 = row.TdDyn();
        cell1.Text = description;
        cell1.CssClass = "text-primary";

        TableCell cell2 = row.TdDyn();
        cell2.Text = info;
    }
}