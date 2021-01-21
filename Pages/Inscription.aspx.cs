using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;


public partial class InscriptionCode : Page
{
    private Regex regexCourriel = new Regex("^[a-zA-Z0-9.!#$%&'*+=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["Reussite"]))
            pnlInscriptionReussie.Visible = true;
    }

    private bool InvalidateIfEmpty(TextBox tb, Label lblError, string message)
    {
        bool ret = false;

        if (tb.Text == "")
        {
            tb.Invalidate();
            lblError.Text = message;
            ret = true;
        }

        return ret;
    }

    private bool CheckFormatCourriel(TextBox tb, Label lblError)
    {
        bool ret = false;

        if (!regexCourriel.IsMatch(tb.Text))
        {
            tb.Invalidate();
            lblError.Text = "Le courriel ne respecte pas le format voulu";
            ret = true;
        }

        return ret;
    }

    private bool CheckMatch(TextBox tb, TextBox tb2, Label lblError, Label lblError2, string message)
    {
        bool ret = false;

        if (tb.Text != tb2.Text)
        {
            tb.Invalidate();
            tb2.Invalidate();
            lblError.Text = message;
            lblError2.Text = message;
            ret = true;
        }

        return ret;
    }

    protected void btnConfirmer_Click(object sender, EventArgs e)
    {
        // Reset everything
        Courriel1.DefaultControl();
        Courriel2.DefaultControl();
        MDP.DefaultControl();
        MDP2.DefaultControl();

        // Check for errors
        bool[] arrError = new bool[]
        {
            InvalidateIfEmpty(Courriel1, lblErrorCourriel1, "Le courriel doit être présent")
                || CheckFormatCourriel(Courriel1, lblErrorCourriel1)
                || CheckMatch(Courriel1, Courriel2, lblErrorCourriel1, lblErrorCourriel2, "Les courriels ne sont pas identiques"),
            InvalidateIfEmpty(Courriel2, lblErrorCourriel2, "La confirmation de courriel doit être présente")
                || CheckFormatCourriel(Courriel2, lblErrorCourriel2),
            InvalidateIfEmpty(MDP, lblErrorMDP, "Le mot de passe doit être présent")
                || CheckMatch(MDP, MDP2, lblErrorMDP, lblErrorMDP2, "Les mots de passes ne sont pas identiques"),
            InvalidateIfEmpty(MDP2, lblErrorMDP2, "La confirmation de mot de passe doit être présente")
        };

        if (!arrError.Contains(true))
        {
            PPClients clients = new PPClients();
            PPVendeurs vendeurs = new PPVendeurs();
            PPGestionnaires gestionnaires = new PPGestionnaires();
            if (clients.Values.Any(x => x.AdresseEmail == Courriel1.Text)
                || vendeurs.Values.Any(x => x.AdresseEmail == Courriel1.Text)
                || gestionnaires.Values.Any(x => x.Email == Courriel1.Text))
            {
                Courriel1.Invalidate();
                lblErrorCourriel1.Text = "Ce courriel est déjà utilisé par un autre utilisateur";
            }
            else
            {
                Client newClient = new Client(null)
                {
                    NoClient = clients.NextId(),
                    AdresseEmail = Courriel1.Text,
                    MotDePasse = MDP.Text,
                    DateCreation = DateTime.Now,
                    NbConnexions = 0,
                    Pays = "Canada"
                };

                clients.Add(newClient);

                Response.Redirect("~/Pages/Inscription.aspx?Reussite=true");
            }
        }
    }
}