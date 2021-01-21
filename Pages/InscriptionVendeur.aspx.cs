using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_InscriptionVendeur : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["RefUrl"] = Request.UrlReferrer.ToString();
            if (Session.IsClient())
            {
                Client client = Session.GetClient();
                tbPrenom.Text = client.Prenom;
                tbNom.Text = client.Nom;
                tbRue.Text = client.Rue;
                tbVille.Text = client.Ville;
                ddlProvince.SelectedValue = client.Province;
                tbCodePostal.Text = client.CodePostal;
                tbTelephone.Text = client.Tel1;
                tbTelephone2.Text = client.Tel2;
            }
        }
    }

    protected void btnConfirmer_Click(object sender, EventArgs e)
    {
        // Reset everything
        Courriel1.DefaultControl();
        Courriel2.DefaultControl();
        MDP.DefaultControl();
        MDP2.DefaultControl();
        tbNomAffaires.DefaultControl();
        tbPrenom.DefaultControl();
        tbNom.DefaultControl();
        tbRue.DefaultControl();
        tbVille.DefaultControl();
        tbCodePostal.DefaultControl();
        tbTelephone.DefaultControl();
        tbTelephone2.DefaultControl();
        tbPoidsMax.DefaultControl();
        tbPrixLivGratuite.DefaultControl();

        int poidsMax = -1;
        decimal prixLivGratuit = -1;

        // Check for errors
        bool[] arrError = new bool[]
        {
            // Courriel
            Courriel1.InvalidateIfEmpty(lblErrorCourriel1, "Le courriel doit être présent")
                || Courriel1.CheckFormatCourriel(lblErrorCourriel1)
                || Courriel1.CheckMatch(Courriel2, lblErrorCourriel1, lblErrorCourriel2, "Les courriels ne sont pas identiques"),
            Courriel2.InvalidateIfEmpty(lblErrorCourriel2, "La confirmation de courriel doit être présente")
                || Courriel2.CheckFormatCourriel(lblErrorCourriel2),
            // Mot de passe
            MDP.InvalidateIfEmpty(lblErrorMDP, "Le mot de passe doit être présent")
                || MDP.CheckMatch(MDP2, lblErrorMDP, lblErrorMDP2, "Les mots de passes ne sont pas identiques"),
            MDP2.InvalidateIfEmpty(lblErrorMDP2, "La confirmation de mot de passe doit être présente"),
            // Nom d'affaires
            tbNomAffaires.InvalidateIfEmpty(lblErrorNomAffaires, "Le nom d'affaires doit être présent"),
            tbPrenom.InvalidateIfEmpty(lblErrorPrenom, "Le prénom doit être présent")
                || tbPrenom.CheckFormatNomPrenom(lblErrorPrenom),
            tbNom.InvalidateIfEmpty(lblErrorNom, "Le nom doit être présent")
                || tbNom.CheckFormatNomPrenom(lblErrorNom),
            tbRue.InvalidateIfEmpty(lblErrorRue, "Les informations sur la rue (No Civique et Rue)")
                || tbRue.CheckFormatNomPrenom(lblErrorRue),
            tbVille.InvalidateIfEmpty(lblErrorVille, "Le nom de la ville doit être présent")
                || tbVille.CheckFormatNomPrenom(lblErrorVille),
            tbCodePostal.InvalidateIfEmpty(lblErrorCodePostal, "Le code postal doit être présent")
                || tbCodePostal.CheckContains(lblErrorCodePostal, "Le code postal doit être entré au complet", "_"),
            tbTelephone.InvalidateIfEmpty(lblErrorTelephone, "Le numéro de téléphone doit être présent")
                || tbTelephone.CheckContains(lblErrorTelephone, "Le numéro de téléphone doit être entré au complet", "_"),
            tbTelephone2.CheckContains(lblErrorTelephone2, "Le numéro de téléphone doit être entré au complet", "_"),
            // Poids Max
            tbPoidsMax.InvalidateIfEmpty(lblErrorPoidsMax, "Le poids maximal pour une livraison doit être présent")
                || tbPoidsMax.CheckInt(lblErrorPoidsMax, "Le poids maximal doit être un nomber entier", out poidsMax)
                || tbPoidsMax.CheckIntOver0(lblErrorPoidsMax, poidsMax),
            // Poids livraison gratuite
            //tbPrixLivGratuite.InvalidateIfEmpty(lblErrorPoidsLivGratuit, "Le poids auquel une livraison devient gratuite doit être présent")
            tbPrixLivGratuite.Text != "" && (
                tbPrixLivGratuite.CheckDecimal(lblErrorPoidsLivGratuit, "Le prix auquel une livraison devient gratuite doit être un nombre décimal", out prixLivGratuit)
                    || tbPrixLivGratuite.CheckDecimalOver0(lblErrorPoidsLivGratuit, prixLivGratuit)
            )
        };

        if (!arrError.Contains(true))
        {
            PPVendeurs vendeurs = new PPVendeurs();
            PPClients clients = new PPClients();
            PPGestionnaires gestionnaires = new PPGestionnaires();

            bool errorsDB = false;

            if (vendeurs.Values.Any(x => x.AdresseEmail == Courriel1.Text))
            {
                Courriel1.Invalidate();
                lblErrorCourriel1.Text = "Ce courriel est déjà utilisé par un vendeur";
                errorsDB = true;
            }
            else if (clients.Values.Any(x => x.AdresseEmail == Courriel1.Text))
            {
                Courriel1.Invalidate();
                lblErrorCourriel1.Text = "Ce courriel est déjà utilisé par un client";
                errorsDB = true;
            }
            else if (gestionnaires.Values.Any(x => x.Email == Courriel1.Text))
            {
                Courriel1.Invalidate();
                lblErrorCourriel1.Text = "Ce courriel est déjà utilisé par un gestionnaire";
                errorsDB = true;
            }

            if (vendeurs.Values.Any(x => x.NomAffaires == tbNomAffaires.Text))
            {
                tbNomAffaires.Invalidate();
                lblErrorNomAffaires.Text = "Ce nom d'affaires est déjà utilisé par un autre vendeur";
                errorsDB = true;
            }

            if (!errorsDB)
            {
                Vendeur newVendeur = new Vendeur(null)
                {
                    NoVendeur = vendeurs.NextId(),
                    NomAffaires = tbNomAffaires.Text,
                    Nom = tbNom.Text,
                    Prenom = tbPrenom.Text,
                    Rue = tbRue.Text,
                    Ville = tbVille.Text,
                    Province = ddlProvince.SelectedValue,
                    CodePostal = tbCodePostal.Text.ToUpper(),
                    Pays = tbPays.Text,
                    Tel1 = tbTelephone.Text,
                    Tel2 = tbTelephone2.Text.ToStringEmptyNull(),
                    AdresseEmail = Courriel1.Text,
                    MotDePasse = MDP.Text,
                    PoidsMaxLivraison = poidsMax,
                    LivraisonGratuite = prixLivGratuit <= 0 ? null : (decimal?)prixLivGratuit,
                    Taxes = rbTaxesOui.Checked,
                    DateCreation = DateTime.Now,
                    Statut = 0
                };

                vendeurs.Add(newVendeur);

                Response.Redirect("~/Pages/InscriptionVendeur.aspx?Reussite=true");
            }
        }
    }

    protected void btnRetour_Click(object sender, EventArgs e)
    {
        object refUrl = ViewState["RefUrl"];
        if (refUrl != null)
            Response.Redirect((string)refUrl);

    }
}