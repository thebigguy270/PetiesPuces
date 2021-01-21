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
        if(Session.GetTypeU() != null)
        {
            PPDestinataires destinataires = new PPDestinataires();
            if (Session.IsVendeur())
            {
                
                pnClient.Visible = false;
                pnVisiteur.Visible = false;
                pnVendeur.Visible = true;
                pnAdmin.Visible = false;
                Vendeur vendeur = Session.GetVendeur();
                string image = "";

                if (vendeur.Configuration != null)
                {
                    if (vendeur.Configuration.Contains(";"))
                    {
                        string[] liste = vendeur.Configuration.Split(';');

                        if (liste.Length == 2)
                        {
                            if (liste[0] != "")
                            {
                                image = "~/Logos/" + liste[0];
                            }


                        }
                        else if (liste.Length == 3)
                        {
                            if (liste[0] != "")
                            {
                                image = "~/Logos/" + liste[0];

                            }

                        }
                    }
                    else
                    {
                        image = "~/Logos/" + vendeur.Configuration;
                    }
                }

                if (image != "")
                {
                    imgLogo.ImageUrl = image;
                }

                lblAccueil.Text = vendeur.NomAffaires;
                

                if (vendeur.Nom == null || vendeur.Prenom == null)
                    lblNomVendeur.Text = vendeur.AdresseEmail;
                else
                    lblNomVendeur.Text = $"{vendeur.Prenom} {vendeur.Nom}";

                hlLogo.NavigateUrl = "~/Pages/Vendeur/AccueilVendeur.aspx";
                var messagesNonLu = destinataires.CountMessagesNonLu(vendeur.NoVendeur.Value);
                lblBadgeVendeur.Text = messagesNonLu != 0 ? messagesNonLu.ToString() : "";
            }
            else if (Session.IsClient())
            {
                pnClient.Visible = true;
                pnVisiteur.Visible = false;
                pnVendeur.Visible = false;
                pnAdmin.Visible = false;
                Client client = Session.GetClient();

                if (client.Nom == null || client.Prenom == null)
                    lblNomClient.Text = client.AdresseEmail;
                else
                    lblNomClient.Text = $"{client.Prenom} {client.Nom}";

                hlLogo.NavigateUrl = "~/Pages/Accueil.aspx";

                var messagesNonLu = destinataires.CountMessagesNonLu(client.NoClient.Value);
                lblBadgeClient.Text = messagesNonLu != 0 ? messagesNonLu.ToString() : "";
            }
            else if(Session.IsAdmin())
            {
                // Admin
                pnAdmin.Visible = true;
                pnClient.Visible = false;
                pnVisiteur.Visible = false;
                pnVendeur.Visible = false;

                Gestionnaire g = Session.GetAdmin();
                if (g.Nom == null || g.Prenom == null)
                    lblNomAdmin.Text = g.Email;
                else
                    lblNomAdmin.Text = $"{g.Prenom} {g.Nom}";

                hlLogo.NavigateUrl = "~/Pages/Admin/PageAccueilAdmin.aspx";
                var messagesNonLu = destinataires.CountMessagesNonLu(g.NoAdmin.Value);
                lblBadgeAdmin.Text = messagesNonLu != 0 ? messagesNonLu.ToString() : "";
            }
            else
            {
                //Juste au cas où
                pnClient.Visible = false;
                pnVisiteur.Visible = true;
                pnVendeur.Visible = false;
                pnAdmin.Visible = false;
                hlLogo.NavigateUrl = "~/Pages/Accueil.aspx";
            }
        }

        else
        {
            pnClient.Visible = false;
            pnVisiteur.Visible = true;
            pnVendeur.Visible = false;
            pnAdmin.Visible = false;
            hlLogo.NavigateUrl = "~/Pages/Accueil.aspx";
        }
    }

    protected void deconnecter(object sender, EventArgs e)
    {
        //Session.Deconecte();
        Session.Clear();
        Response.Redirect("~/Pages/Accueil.aspx");
    }
}