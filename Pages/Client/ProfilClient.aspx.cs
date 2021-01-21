using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProfilClient : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Session.IsClient())
                Response.Redirect(SessionManager.RedirectConnexionLink);

        if (!Page.IsPostBack)
        {
            Client client = Session.GetClient();

            tbAdresseEmail.Text = client.AdresseEmail;
            tbPrenom.Text = client.Prenom;
            tbNom.Text = client.Nom;
            tbNoCiviqueRue.Text = client.Rue;
            tbVille.Text = client.Ville;
            ddlProvince.SelectedValue = client.Province;
            tbPays.Text = client.Pays;
            tbCodePostal.Text = client.CodePostal;
            tbTelephone.Text = client.Tel1;
            tbCellulaire.Text = client.Tel2;
        }

        if (!string.IsNullOrEmpty(Request.QueryString["Reussite"]))
            pnlReussite.Visible = true;
        if (!string.IsNullOrEmpty(Request.QueryString["ReussiteMDP"]))
            pnlReussiteMDP.Visible = true;
    }

    protected void btnAnnuler_OnClick(object sender, EventArgs e) => Response.Redirect("~/Pages/Accueil.aspx");

    protected void btnSauvegarder_OnClick(object sender, EventArgs e)
    {
        // Reset everything
        tbPrenom.DefaultControl();
        tbNom.DefaultControl();
        tbNoCiviqueRue.DefaultControl();
        tbVille.DefaultControl();
        ddlProvince.DefaultControl();
        tbCodePostal.DefaultControl();
        tbTelephone.DefaultControl();
        tbCellulaire.DefaultControl();

        Client client = Session.GetClient();
        bool[] arrError = new bool[]
        {
            tbPrenom.InvalidateIfEmpty(lblErrorPrenom, "Le prénom doit être présent")
                || tbPrenom.CheckFormatNomPrenom(lblErrorPrenom),
            tbNom.InvalidateIfEmpty(lblErrorNom, "Le nom doit être présent")
                || tbNom.CheckFormatNomPrenom(lblErrorNom),
            tbNoCiviqueRue.InvalidateIfEmpty(lblErrorNoCiviqueRue, "Les informations sur la rue (No Civique et Rue)")
                || tbNoCiviqueRue.CheckFormatNomPrenom(lblErrorNoCiviqueRue),
            tbVille.InvalidateIfEmpty(lblErrorVille, "Le nom de la ville doit être présent")
                || tbVille.CheckFormatNomPrenom(lblErrorVille),
            ddlProvince.InvalidateIfEmpty(lblErrorProvince, "La province doit être selectionnée"),
            tbCodePostal.InvalidateIfEmpty(lblErrorCodePostal, "Le code postal doit être présent")
                || tbCodePostal.CheckContains(lblErrorCodePostal, "Le code postal doit être entré au complet", "_"),
            tbTelephone.InvalidateIfEmpty(lblErrorTelephone, "Le numéro de téléphone doit être présent")
                || tbTelephone.CheckContains(lblErrorTelephone, "Le numéro de téléphone doit être entré au complet", "_"),
            tbCellulaire.CheckContains(lblErrorCellulaire, "Le numéro de téléphone doit être entré au complet", "_")
        };

        if (!arrError.Contains(true))
        {
            // Check if there are any differences
            bool modifications = false;
            if (client.Prenom != tbPrenom.Text)
            {
                client.Prenom = tbPrenom.Text;
                modifications = true;
            }
            if (client.Nom != tbNom.Text)
            {
                client.Nom = tbNom.Text;
                modifications = true;
            }
            if (client.Rue != tbNoCiviqueRue.Text)
            {
                client.Rue = tbNoCiviqueRue.Text;
                modifications = true;
            }
            if (client.Ville != tbVille.Text)
            {
                client.Ville = tbVille.Text;
                modifications = true;
            }
            if (client.Province != ddlProvince.SelectedValue)
            {
                client.Province = ddlProvince.SelectedValue;
                modifications = true;
            }
            if (client.CodePostal != tbCodePostal.Text)
            {
                client.CodePostal = tbCodePostal.Text.ToUpper();
                modifications = true;
            }
            if (client.Tel1 != tbTelephone.Text)
            {
                client.Tel1 = tbTelephone.Text;
                modifications = true;
            }
            if (client.Tel2 != tbCellulaire.Text)
            {
                client.Tel2 = tbCellulaire.Text;
                modifications = true;
            }

            if (modifications)
            {
                client.DateMAJ = DateTime.Now;
                PPClients clients = new PPClients();
                clients.NotifyUpdatedOutside(client);

                clients.Update();

                Response.Redirect("~/Pages/Client/ProfilClient.aspx?Reussite=true");
            }
        }
    }

    protected void btnSauvegarderMDP_OnClick(object sender, EventArgs e)
    {
        tbAncienMDP.DefaultControl();
        tbNouveauMDP.DefaultControl();
        tbConfirmationMDP.DefaultControl();

        Client client = Session.GetClient();
        if (tbAncienMDP.Text != client.MotDePasse)
        {
            tbAncienMDP.Invalidate();
            lblErrorAncienMDP.Text = "L'ancien mot de passe doit correspondre au mot de passe courrant";
        }
        else
        {
            if (!tbNouveauMDP.InvalidateIfEmpty(lblErrorNouveauMDP, "Le mot de passe doit être définit")
                || !tbConfirmationMDP.InvalidateIfEmpty(lblErrorConfirmationMDP, "Le mot de passe doit être définit"))
            {
                if (tbNouveauMDP.Text != tbConfirmationMDP.Text)
                {
                    tbNouveauMDP.Invalidate();
                    tbConfirmationMDP.Invalidate();
                    lblErrorNouveauMDP.Text = "Les mots de passes doivent correspondre";
                }
                else if (tbNouveauMDP.Text == client.MotDePasse)
                {
                    tbNouveauMDP.Invalidate();
                    tbConfirmationMDP.Invalidate();
                    lblErrorNouveauMDP.Text = "Le nouveau mot de passe doit être différent de l'ancien";
                }
                else
                {
                    client.MotDePasse = tbNouveauMDP.Text;

                    PPClients clients = new PPClients();
                    clients.NotifyUpdatedOutside(client);

                    clients.Update();

                    Response.Redirect("~/Pages/Client/ProfilClient.aspx?ReussiteMDP=true");
                }
            }
        }
    }
}