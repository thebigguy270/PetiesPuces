using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_OubliMDP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request["Id"];
        string type = Request["Type"];
        string reussite = Request["Reussite"];
        if (id != null && type != null)
        {
            pnlOubli.Visible = false;
            pnlNouveauMDP.Visible = true;
            pnlReussite.Visible = false;
        }
        else if (reussite != null)
        {
            pnlOubli.Visible = false;
            pnlNouveauMDP.Visible = false;
            pnlReussite.Visible = true;
        }
        else
        {
            pnlOubli.Visible = true;
            pnlNouveauMDP.Visible = false;
            pnlReussite.Visible = false;

            pnlFakeEmail.Visible = false;
            lblFakeEmail.Text = "";
            pnlError.Visible = false;
            lblError.Text = "";
        }
    }

    protected void btnOubliMDP_OnClick(object sender, EventArgs e)
    {
        pnlFakeEmail.Visible = true;

        PPClients clients = new PPClients();
        Client client = clients.Values.Find(x => x.AdresseEmail.ToLower() == tbAdresseEmail.Text.ToLower());
        if (client != null)
        {
            if (client.Nom == null || client.Prenom == null)
                lblFakeEmail.Text = $"{client.AdresseEmail},<br/><br/>";
            else
                lblFakeEmail.Text = $"{client.Nom} {client.Prenom},<br/><br/>";

            lblFakeEmail.Text += "Une demande de changement de mot de passe vous a été envoyé.<br/>";
            lblFakeEmail.Text += $"Cliquez sur <a href=\"/Pages/OubliMDP.aspx?Id={client.NoClient}&Type=client\">ce lien</a> pour le changer.";
        }
        else
        {
            PPVendeurs vendeurs = new PPVendeurs();
            Vendeur vendeur = vendeurs.Values.Find(x => x.AdresseEmail.ToLower() == tbAdresseEmail.Text.ToLower());
            if (vendeur != null)
            {
                lblFakeEmail.Text = $"{vendeur.Nom} {vendeur.Prenom},<br/><br/>";
                lblFakeEmail.Text += "Une demande de changement de mot de passe vous a été envoyé.<br/>";
                lblFakeEmail.Text += $"Cliquez sur <a href=\"/Pages/OubliMDP.aspx?Id={vendeur.NoVendeur}&Type=vendeur\">ce lien</a> pour le changer.";

                Response.Redirect($"~/Pages/OubliMDP.aspx?Id={vendeur.NoVendeur}&Type=vendeur");
            }
            else
            {
                pnlError.Visible = true;
                lblError.Text = "Ce compte n'existe pas";
            }
        }
    }

    protected void btnConfirmerNouveauMDP_OnClick(object sender, EventArgs e)
    {
        tbMDP1.DefaultControl();
        tbMDP2.DefaultControl();

        bool[] errors = new bool[]
        {
            tbMDP1.InvalidateIfEmpty(lblErrorMDP1, "Le mot de passe doit être présent")
                || tbMDP1.CheckMatch(tbMDP2, lblErrorMDP1, lblErrorMDP2, "Les mots de passes ne sont pas identiques"),
            tbMDP2.InvalidateIfEmpty(lblErrorMDP2, "La confirmation de mot de passe doit être présente")
        };

        if (!errors.Contains(true))
        {
            string id = Request["Id"];
            string type = Request["Type"];
            if (type == "client")
            {
                PPClients clients = new PPClients();
                Client client = clients.Values.Find(x => x.NoClient.Value == long.Parse(id));

                client.MotDePasse = tbMDP1.Text;

                clients.NotifyUpdated(client);
                clients.Update();

                Response.Redirect("~/Pages/OubliMDP.aspx?Reussite=true");
            }
            else if (type == "vendeur")
            {
                PPVendeurs vendeurs = new PPVendeurs();
                Vendeur vendeur = vendeurs.Values.Find(x => x.NoVendeur.Value == long.Parse(id));

                vendeur.MotDePasse = tbMDP1.Text;
                vendeurs.NotifyUpdated(vendeur);
                vendeurs.Update();

                Response.Redirect("~/Pages/OubliMDP.aspx?Reussite=true");
            }
        }
    }
}