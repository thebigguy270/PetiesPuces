using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsAdmin())
        {
            Gestionnaire g = Session.GetAdmin();
            PPDestinataires destinataires = new PPDestinataires();
            NomUtilisateur.Text = $"{g.Prenom} {g.Nom}";
            var messagesNonLu = destinataires.CountMessagesNonLu(g.NoAdmin.Value);
            lblBadgeAdmin.Text = messagesNonLu != 0 ? messagesNonLu.ToString() : "";
        }
    }

    protected void deconnecter(object sender, EventArgs e)
    {
        //Session.Deconecte();
        Session.Clear();
        Response.Redirect("~/Pages/Accueil.aspx");
    }
}