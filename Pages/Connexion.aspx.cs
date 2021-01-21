using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.IsClient())
            Response.Redirect("~/Pages/Accueil.aspx");
        else if (Session.IsVendeur())
            Response.Redirect("~/Pages/AccueilVendeur.aspx");
        else if (Session.IsAdmin())
            Response.Redirect("~/Pages/PageAccueilAdmin.aspx");
    }

    protected void btnConnexion_Click(object sender, EventArgs e)
    {
        if (valMDP.IsValid && valNomU.IsValid)
        {
            // Essayer de se connecter en tant que client en premier
            PPClients clients = new PPClients();

            Client client = clients.Values.Find(cli => cli.AdresseEmail.ToLower() == tbNomU.Text.ToLower());
            if (client != null)
            {
                if (client.MotDePasse == tbMDP.Text)
                {
                    if (client.StatutEnum() == ClientStatut.Disabled)
                            lblErreurSQL.Text = "Ce compte est désactivé";
                    else
                    {
                        client.NbConnexions++;
                        client.DateDerniereConnexion = DateTime.Now;

                        clients.NotifyUpdated(client);
                        clients.Update();

                        Session.SetClient(client);
                        Response.Redirect("~/Pages/Accueil.aspx");
                    }
                }
                else
                    lblErreurSQL.Text = "L'adresse email ou le mot de passe n'est pas correcte";
            }
            else
            {
                // Essayer en tant que vendeur si le client n'existe pas
                PPVendeurs vendeurs = new PPVendeurs();

                Vendeur vendeur = vendeurs.Values.Find(ven => ven.AdresseEmail.ToLower() == tbNomU.Text.ToLower());
                if (vendeur != null)
                {
                    if (vendeur.MotDePasse == tbMDP.Text)
                    {
                        if (vendeur.StatutEnum() == VendeurStatut.Disabled)
                            lblErreurSQL.Text = "Ce compte est désactivé";
                        else if (vendeur.StatutEnum() == VendeurStatut.Pending)
                            lblErreurSQL.Text = "Ce compte n'a pas encore été accepté par le gestionnaire";
                        else
                        {
                            Session.SetVendeur(vendeur);
                            Response.Redirect("~/Pages/Vendeur/AccueilVendeur.aspx");
                        }
                    }
                    else
                        lblErreurSQL.Text = "L'adresse email ou le mot de passe n'est pas correcte";
                }
                else
                {
                    // Essayer en tant que gestionnaire comme dernier recours
                    PPGestionnaires gestionnaires = new PPGestionnaires();

                    Gestionnaire admin = gestionnaires.Values.Find(adm => adm.Email.ToLower() == tbNomU.Text.ToLower());
                    if (admin != null)
                    {
                        if (admin.MotDePasse == tbMDP.Text)
                        {
                            Session.SetAdmin(admin);
                            Response.Redirect("~/Pages/Admin/PageAccueilAdmin.aspx");
                        }
                        else
                            lblErreurSQL.Text = "L'adresse email ou le mot de passe n'est pas correcte";
                    }
                    else
                        lblErreurSQL.Text = "L'utilisateur n'existe pas";
                }
            }
        }
    }

    protected void btnAnnuler_OnClick(object sender, EventArgs e) => Response.Redirect("~/Pages/OubliMDP.aspx");
}