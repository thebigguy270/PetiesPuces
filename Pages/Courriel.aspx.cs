using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

/// <summary>
/// Summary description for Class1
/// </summary>
public partial class CourrielCode : Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["Type"] == null)
        {
            Response.Redirect("Accueil.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["urlPagePrec"] = Request.UrlReferrer.ToString();
            //Si admin accepte vendeur
            if (((Request.Form["courrielC"] != null) && (Request.Form["sujet"] != null) && (Request.Form["contenu"] != null))||(!string.IsNullOrEmpty(postC.Value)))
            {
                rblOrigine.SelectedIndex = 4;
                pnRedactionCourriel.Visible = true;
                pnChoixCourriel.Visible = false;
                btnSupprimerMessage.Visible = false;
                btnRestaurerMessages.Enabled = false;
                
                postC.Value = Request.Form["contenu"].ToString() + "," + Request.Form["sujet"].ToString() + "," + Request.Form["courrielC"].ToString() + ",";
                txtCourrielAEnvoyer.Text = Request.Form["contenu"].ToString();
                txtSujet.Text = Request.Form["sujet"].ToString();
                PPClients pc = new PPClients();
                Client client = pc.Values.Find(pce => pce.AdresseEmail == Request.Form["courrielC"].ToString());
                hidDestinataires.Value += client.NoClient.ToString() + ";";
                if ((string.IsNullOrEmpty(client.Nom)) || (string.IsNullOrEmpty(client.Prenom))){
                    txtDestinataire.Text += client.NoClient + ";";
                }
                else {
                    txtDestinataire.Text += client.Nom + ", " + client.Prenom + ";";
                }
                pnOptions.Visible = false;
                btnBrouillon.Visible = false;
                if (Request.Form["boutonAnnuler"] != null)
                {
                    btnAnnuler.Visible = false;
                }


            }
            else if (((Request.Form["courrielV"] != null) && (Request.Form["sujet"] != null) && (Request.Form["contenu"] != null)) || (!string.IsNullOrEmpty(postV.Value)))
            {
                rblOrigine.SelectedIndex = 4;
                pnRedactionCourriel.Visible = true;
                pnChoixCourriel.Visible = false;
                btnSupprimerMessage.Visible = false;
                btnRestaurerMessages.Enabled = false;
                postV.Value = Request.Form["contenu"].ToString() + "," + Request.Form["sujet"].ToString() + "," + Request.Form["courrielV"].ToString() + ",";
                txtCourrielAEnvoyer.Text = Request.Form["contenu"].ToString();
                txtSujet.Text = Request.Form["sujet"].ToString();
                PPVendeurs pc = new PPVendeurs();
                Vendeur vendeur = pc.Values.Find(pce => pce.AdresseEmail == Request.Form["courrielV"].ToString());
                hidDestinataires.Value += vendeur.NoVendeur.ToString() + ";";
                if ((string.IsNullOrEmpty(vendeur.Nom)) || (string.IsNullOrEmpty(vendeur.Prenom)))
                {
                    txtDestinataire.Text += vendeur.NoVendeur + ";";
                }
                else
                {
                    txtDestinataire.Text += vendeur.NomAffaires + ";";
                }
                pnOptions.Visible = false;
                btnBrouillon.Visible = false;
            }
            

            else if (Request.Form["messageATous"] != null)
            {
                rblOrigine.SelectedIndex = 4;
                pnRedactionCourriel.Visible = true;
                pnChoixCourriel.Visible = false;
                btnSupprimerMessage.Visible = false;
                btnRestaurerMessages.Enabled = false;
                pnOptions.Visible = false;
                btnBrouillon.Visible = false;
                PPClients pc = new PPClients();
                PPVendeurs pv = new PPVendeurs();
                foreach (Client client in pc.Values)
                {
                    if ((client.Nom != null) && (client.Prenom != null))
                        txtDestinataire.Text += client.Nom + ", " + client.Prenom+";";
                    else
                    {
                        txtDestinataire.Text += client.NoClient.ToString()+";";
                    }
                    hidDestinataires.Value += client.NoClient.ToString() + ";";
                }
                foreach (Vendeur vendeur in pv.Values)
                {
                    txtDestinataire.Text += vendeur.NomAffaires + ";";
                    hidDestinataires.Value += vendeur.NoVendeur.ToString() + ";";
                }
                postTous.Value = "true";
            }
            else
            {
                rblOrigine.SelectedIndex = 0;
                pnRedactionCourriel.Visible = false;
                btnRestaurerMessages.Enabled = false;

            }
                TableHeaderRow headerRow = tbCourriels.ThrDyn();
                headerRow.TableSection = TableRowSection.TableHeader;
                headerRow.CssClass += "test";
                 TableHeaderCell thdCB = headerRow.ThdDyn();

                CheckBox cbTous = thdCB.CbDyn("cbTous");
                thdCB.CssClass = "cb";
                cbTous.ClientIDMode = ClientIDMode.Static;
                headerRow.ThdDyn("Envoyé par");
                headerRow.ThdDyn("Sujet");
                headerRow.ThdDyn("Date de réception");
                lblContenu.Text = " Veuillez choisir un courriel dans la liste ";
                chargerCourriel(0);
                PPVendeurs vendeurs = new PPVendeurs();
                PPClients clients = new PPClients();
                PPGestionnaires gestionnaires = new PPGestionnaires();
                PPVendeursClients vendeursClients = new PPVendeursClients();
                lbVendeurs.Items.Clear();
                lbClients.Items.Clear();
                lbAdmins.Items.Clear();
                lbAdmins.SelectedIndex = -1;
                lbClients.SelectedIndex = -1;
                lbVendeurs.SelectedIndex = -1;
                btnModifierBrouillon.CssClass += " invisible";
                btnRepondre.CssClass += " invisible";
                btnTransfert.CssClass += " invisible";
                
                if (!Session.IsVendeur())
                {
                    foreach (Client client in clients.Values)
                    {
                        ListItem li = new ListItem();
                    if ((client.Nom != null) && (client.Prenom != null))
                    {
                        li.Text = client.Nom + ", " + client.Prenom;
                    }
                    else
                    {
                        li.Text = client.NoClient.ToString();
                    }
                        li.Value = client.NoClient.ToString();
                        lbClients.Items.Add(li);
                    }
                    foreach (Vendeur vendeur in vendeurs.Values)
                    {
                        ListItem li = new ListItem();
                        li.Text = vendeur.NomAffaires;
                        li.Value = vendeur.NoVendeur.ToString();
                        lbVendeurs.Items.Add(li);
                    }
                }
                else
                {
                    Vendeur vendeur = Session.GetVendeur();
                    long noCou = (long)vendeur.NoVendeur;
                    foreach (Client client in clients.Values)
                        {
                       
                            if (vendeursClients.Values.Exists(v => (v.NoClient == client.NoClient) && (v.NoVendeur == noCou)))
                            {
                                ListItem li = new ListItem();
                                if ((client.Nom != null) && (client.Prenom != null))
                                {
                                    li.Text = client.Nom + ", " + client.Prenom;
                                }
                                else
                                {
                                    li.Text = client.NoClient.ToString();
                                }
                                li.Value = client.NoClient.ToString();
                                lbClients.Items.Add(li);
                            }
                        }
                
                        ListItem liv = new ListItem();
                        liv.Text = vendeur.NomAffaires;
                        liv.Value = vendeur.NoVendeur.ToString();
                        lbVendeurs.Items.Add(liv);
                }
                foreach (Gestionnaire gestionnaire in gestionnaires.Values)
                {
                    ListItem li = new ListItem();
                    li.Text = gestionnaire.Nom + ", " + gestionnaire.Prenom;
                    li.Value = gestionnaire.NoAdmin.ToString();
                    lbAdmins.Items.Add(li);
                }
                if (Session.IsVendeur())
                {
                    lbAdmins.Visible = true;
                    btnAdmins.Visible = true;
                    rowAdmins.Visible = true;
                    lbClients.Visible = true;
                    lbClients.SelectionMode = ListSelectionMode.Multiple;
                    btnClients.Visible = true;
                    lbVendeurs.Visible = true;
                    lbVendeurs.SelectionMode = ListSelectionMode.Multiple;
                    btnVendeurs.Visible = true;
                }
                else if (Session.IsClient())
                {
                    lbAdmins.Visible = true;
                    lbAdmins.SelectionMode = ListSelectionMode.Multiple;
                    btnAdmins.Visible = true;
                    lbVendeurs.Visible = true;
                    lbVendeurs.SelectionMode = ListSelectionMode.Multiple;
                    btnVendeurs.Visible = true;
                    rowClients.Visible = false;
                }
                else if (Session.IsAdmin())
                {
                    lbAdmins.Visible = true;
                    lbAdmins.SelectionMode = ListSelectionMode.Multiple;
                    lbVendeurs.Visible = true;
                    lbVendeurs.SelectionMode = ListSelectionMode.Multiple;
                    btnVendeurs.Visible = true;
                    lbClients.Visible = true;
                    lbClients.SelectionMode = ListSelectionMode.Multiple;
                    btnClients.Visible = true;
                }
        }
        else
        {
            if (rblOrigine.SelectedIndex == 0)
            {
                hidDestinataires.Value = "";
                txtCourrielAEnvoyer.Text = "";
                txtDestinataire.Text = "";
                txtSujet.Text = "";
                ViewState["NoMsgBrouillon"] = null;
                //ViewState["msgTrRep"] = null;
                tbCourriels.Rows.Clear();
                TableHeaderRow headerRow = tbCourriels.ThrDyn();
                headerRow.TableSection = TableRowSection.TableHeader;
                headerRow.CssClass += "test";
                TableHeaderCell thdCB = headerRow.ThdDyn();
                btnRestaurerMessages.Enabled = false;
                CheckBox cbTous = thdCB.CbDyn("cbTous");
                thdCB.CssClass = "cb";
                cbTous.ClientIDMode = ClientIDMode.Static;
                headerRow.ThdDyn("Envoyé par");
                headerRow.ThdDyn("Sujet");
                headerRow.ThdDyn("Date de réception");
                lblContenu.Text = "Veuillez choisir un message dans la liste ";
                chargerCourriel(rblOrigine.SelectedIndex);
                pnRedactionCourriel.Visible = false;
                pnChoixCourriel.Visible = true;
                btnSupprimerMessage.Visible = true;
            }
            else if (rblOrigine.SelectedIndex == 1)
            {
                hidDestinataires.Value = "";
                txtCourrielAEnvoyer.Text = "";
                txtDestinataire.Text = "";
                txtSujet.Text = "";
                ViewState["NoMsgBrouillon"] = null;
                //ViewState["msgTrRep"] = null;
                tbCourriels.Rows.Clear();
                TableHeaderRow headerRow = tbCourriels.ThrDyn();
                headerRow.TableSection = TableRowSection.TableHeader;
                headerRow.CssClass += "test";
                TableHeaderCell thdCB = headerRow.ThdDyn();
                CheckBox cbTous = thdCB.CbDyn("cbTous");
                thdCB.CssClass = "cb";
                cbTous.ClientIDMode = ClientIDMode.Static;
                headerRow.ThdDyn("Envoyé vers");
                headerRow.ThdDyn("Sujet");
                headerRow.ThdDyn("Date d'envoi");
                lblContenu.Text = "Veuillez choisir un message dans la liste ";
                chargerCourriel(rblOrigine.SelectedIndex);
                pnRedactionCourriel.Visible = false;
                pnChoixCourriel.Visible = true;
                btnSupprimerMessage.Visible = true;
                btnRestaurerMessages.Enabled = false;
            }
            else if (rblOrigine.SelectedIndex == 2)
            {
                hidDestinataires.Value = "";
                txtCourrielAEnvoyer.Text = "";
                txtDestinataire.Text = "";
                txtSujet.Text = "";
                //ViewState["msgTrRep"] = null;
                tbCourriels.Rows.Clear();
                TableHeaderRow headerRow = tbCourriels.ThrDyn();
                headerRow.TableSection = TableRowSection.TableHeader;
                headerRow.CssClass += "test";
                TableHeaderCell thdCB = headerRow.ThdDyn();
                CheckBox cbTous = thdCB.CbDyn("cbTous");
                thdCB.CssClass = "cb";
                cbTous.ClientIDMode = ClientIDMode.Static;
                headerRow.ThdDyn("Envoyé par");
                headerRow.ThdDyn("Envoyé vers");
                headerRow.ThdDyn("Sujet");
                headerRow.ThdDyn("Date de réception");
                lblContenu.Text = "Veuillez choisir un message dans la liste ";
                chargerCourriel(rblOrigine.SelectedIndex);
                pnRedactionCourriel.Visible = false;
                pnChoixCourriel.Visible = true;
                btnSupprimerMessage.Visible = true;
                btnRestaurerMessages.Enabled = true;
            }
            else if (rblOrigine.SelectedIndex == 3)
            {
                hidDestinataires.Value = "";
                txtCourrielAEnvoyer.Text = "";
                txtDestinataire.Text = "";
                txtSujet.Text = "";
                //ViewState["msgTrRep"] = null;
                tbCourriels.Rows.Clear();
                TableHeaderRow headerRow = tbCourriels.ThrDyn();
                headerRow.TableSection = TableRowSection.TableHeader;
                headerRow.CssClass += "test";
                TableHeaderCell thdCB = headerRow.ThdDyn();
                CheckBox cbTous = thdCB.CbDyn("cbTous");
                thdCB.CssClass = "cb";
                cbTous.ClientIDMode = ClientIDMode.Static;
                headerRow.ThdDyn("Envoyé par");
                headerRow.ThdDyn("Sujet");
                headerRow.ThdDyn("Date de réception");
                lblContenu.Text = "Veuillez choisir un message dans la liste ";
                btnSupprimerMessage.Visible = true;
                chargerCourriel(rblOrigine.SelectedIndex);
                pnRedactionCourriel.Visible = false;
                pnChoixCourriel.Visible = true;
                btnRestaurerMessages.Enabled = false;
            }
            else
            {
                btnSupprimerMessage.Visible = false;
                btnRestaurerMessages.Visible = false;
                pnRedactionCourriel.Visible = true;
                pnChoixCourriel.Visible = false;
                string[] strDestinataires = hidDestinataires.Value.Split(';');
                PPDestinataires ppd = new PPDestinataires();
                Array.Resize(ref strDestinataires, strDestinataires.Length - 1);
                PPVendeurs vendeurs = new PPVendeurs();
                PPClients clients = new PPClients();
                PPGestionnaires gestionnaires = new PPGestionnaires();
                for (int i = 0; i < strDestinataires.Length; i++)
                {
                    Vendeur vendeur = vendeurs.Values.Find(ven => ven.NoVendeur == long.Parse(strDestinataires[i]));
                    Gestionnaire admin = gestionnaires.Values.Find(adm => adm.NoAdmin == int.Parse(strDestinataires[i]));
                    Client client = clients.Values.Find(c => c.NoClient == long.Parse(strDestinataires[i]));
                    if (vendeur != null)
                    {
                        lbVendeurs.Items.FindByValue(strDestinataires[i]).Selected = true;
                    }
                    else if (admin != null)
                    {
                        lbAdmins.Items.FindByValue(strDestinataires[i]).Selected = true;
                    }
                    else if (client != null)
                    {
                        lbClients.Items.FindByValue(strDestinataires[i]).Selected = true;
                    }
                }
                   
            }
        }
        string parameter = Request["__EVENTARGUMENT"];
        if (ViewState["NoMsgBrouillon"] != null)
        {
            PPMessages ppmsg = new PPMessages();
            Message msg = ppmsg.Values.Find(m => m.NoMsg == int.Parse(ViewState["NoMsgBrouillon"].ToString()));
            if (!string.IsNullOrEmpty(msg.FichierJoint))
            {
                fichierBrouillon.Value = msg.FichierJoint;
                lblFichierBrouillon.Text = "Ignorez le label, ce fichier suivant a été chargé:" + msg.FichierJoint;

            }
            else
            {
                fichierBrouillon.Value = null;
                lblFichierBrouillon.Text = null;
            }
        }
        else if(ViewState["msgTrRep"] == null)
        {
            fichierBrouillon.Value = null;
            lblFichierBrouillon.Text = null;
        }
    }
    protected void rblOrigine_SelectedIndexChanged(object sender, EventArgs e)
    {
        lienJoint.Visible = false;
        if (rblOrigine.SelectedIndex == 0)
        {
            hidDestinataires.Value = "";
            txtCourrielAEnvoyer.Text = "";
            txtDestinataire.Text = "";
            txtSujet.Text = "";
            //ViewState["NoMsgBrouillon"] = null;
            tbCourriels.Rows.Clear();
            TableHeaderRow headerRow = tbCourriels.ThrDyn();
            headerRow.TableSection = TableRowSection.TableHeader;
            TableHeaderCell thdCB = headerRow.ThdDyn();
            headerRow.CssClass += "test";
            CheckBox cbTous = thdCB.CbDyn("cbTous");
            thdCB.CssClass = "cb";
            cbTous.ClientIDMode = ClientIDMode.Static;
            headerRow.ThdDyn("Envoyé par");
            headerRow.ThdDyn("Sujet");
            headerRow.ThdDyn("Date de réception");
            lblContenu.Text = "Veuillez choisir un message dans la liste ";
           
            chargerCourriel(rblOrigine.SelectedIndex);
            pnChoixCourriel.Visible = true;
            btnSupprimerMessage.Visible = true;
            btnRestaurerMessages.Enabled = false;
        }
        else if (rblOrigine.SelectedIndex == 1)
        {
            hidDestinataires.Value = "";
            txtCourrielAEnvoyer.Text = "";
            txtDestinataire.Text = "";
            txtSujet.Text = "";
            //ViewState["NoMsgBrouillon"] = null;
            tbCourriels.Rows.Clear();
            TableHeaderRow headerRow = tbCourriels.ThrDyn();
            headerRow.TableSection = TableRowSection.TableHeader;
            headerRow.CssClass += "test";
            TableHeaderCell thdCB = headerRow.ThdDyn();
            CheckBox cbTous = thdCB.CbDyn("cbTous");
            thdCB.CssClass = "cb";
            cbTous.ClientIDMode = ClientIDMode.Static;
            headerRow.ThdDyn("Envoyé vers");
            headerRow.ThdDyn("Sujet");
            headerRow.ThdDyn("Date d'envoi");
            lblContenu.Text = "Veuillez choisir un message dans la liste ";
            chargerCourriel(rblOrigine.SelectedIndex);
            pnChoixCourriel.Visible = true;
            btnSupprimerMessage.Visible = true;
            btnRestaurerMessages.Enabled = false;
        }
        else if (rblOrigine.SelectedIndex == 2)
        {
            hidDestinataires.Value = "";
            txtCourrielAEnvoyer.Text = "";
            txtDestinataire.Text = "";
            txtSujet.Text = "";
            //ViewState["NoMsgBrouillon"] = null;
            tbCourriels.Rows.Clear();
            TableHeaderRow headerRow = tbCourriels.ThrDyn();
            headerRow.TableSection = TableRowSection.TableHeader;
            headerRow.CssClass += "test";
            TableHeaderCell thdCB = headerRow.ThdDyn();
            CheckBox cbTous = thdCB.CbDyn("cbTous");
            thdCB.CssClass = "cb";
            cbTous.ClientIDMode = ClientIDMode.Static;
            headerRow.ThdDyn("Envoyé vers");
            headerRow.ThdDyn("Envoyé par");
            headerRow.ThdDyn("Sujet");
            headerRow.ThdDyn("Date de réception");
            lblContenu.Text = "Veuillez choisir un message dans la liste ";
            chargerCourriel(rblOrigine.SelectedIndex);
            pnChoixCourriel.Visible = true;
            btnSupprimerMessage.Visible = true;
            btnRestaurerMessages.Visible = true;
            btnRestaurerMessages.Enabled = true;
        }
        else if (rblOrigine.SelectedIndex == 3)
        {
            hidDestinataires.Value = "";
            txtCourrielAEnvoyer.Text = "";
            txtDestinataire.Text = "";
            txtSujet.Text = "";
            tbCourriels.Rows.Clear();
            TableHeaderRow headerRow = tbCourriels.ThrDyn();
            headerRow.TableSection = TableRowSection.TableHeader;
            headerRow.CssClass += "test";
            TableHeaderCell thdCB = headerRow.ThdDyn();
            CheckBox cbTous = thdCB.CbDyn("cbTous");
            cbTous.ClientIDMode = ClientIDMode.Static;
            thdCB.CssClass = "cb";
            headerRow.ThdDyn("À envoyer vers");
            headerRow.ThdDyn("Sujet");
            headerRow.ThdDyn("Date de réception");
            lblContenu.Text = "Veuillez choisir un message dans la liste ";
            chargerCourriel(rblOrigine.SelectedIndex);
            pnChoixCourriel.Visible = true;
            btnSupprimerMessage.Visible = true;
            btnRestaurerMessages.Enabled = false;
        }
        else
        {
            pnRedactionCourriel.Visible = true;
            pnChoixCourriel.Visible = false;
            string[] strDestinataires = hidDestinataires.Value.Split(';');
            Array.Resize(ref strDestinataires, strDestinataires.Length - 1);
            btnSupprimerMessage.Visible = false;
            btnRestaurerMessages.Visible = false;
        }
    }
    protected void chargerCourriel(int intlieu)
    {
        pnC.Controls.Clear();
        long noCou = 0;
        if (Session.IsClient())
        {
            Client client = Session.GetClient();
            noCou = (long)client.NoClient;
        }
        if (Session.IsVendeur())
        {
            Vendeur vendeur = Session.GetVendeur();
            noCou = (long)vendeur.NoVendeur;
        }
        if (Session.IsAdmin())
        {
            Gestionnaire gestionnaire = Session.GetAdmin();
            noCou = (long)gestionnaire.NoAdmin;
        }
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionInfo"]);

        con.Open();
        string strRequeteInfosCourriel = "";
        if ((rblOrigine.SelectedIndex == 0))
        {
            strRequeteInfosCourriel = "SELECT PPDestinataires.NoDestinataire,PPMessages.NoMsg, PPMessages.NoExpediteur,PPMessages.dateEnvoi, PPMessages.objet,PPDestinataires.EtatLu,PPMessages.DescMsg FROM PPMessages INNER JOIN PPDestinataires ON PPMessages.NoMsg = PPDestinataires.NoMsg WHERE PPDestinataires.NoDestinataire = @noDestinataire AND PPDestinataires.Lieu = @noLieu ORDER BY dateEnvoi DESC";
        }
        else if (rblOrigine.SelectedIndex == 2)
        {
            strRequeteInfosCourriel = "SELECT DISTINCT PPMessages.NoMsg, PPMessages.NoExpediteur,PPMessages.dateEnvoi, PPMessages.objet,PPDestinataires.EtatLu,PPMessages.DescMsg FROM PPMessages INNER JOIN PPDestinataires ON PPMessages.NoMsg = PPDestinataires.NoMsg WHERE (PPMessages.NoExpediteur = @noDestinataire AND PPMessages.Lieu = @noLieu) OR (PPDestinataires.NoDestinataire = @noDestinataire AND PPDestinataires.Lieu = @noLieu) ORDER BY dateEnvoi DESC";
        }
        else
        {
            strRequeteInfosCourriel = "SELECT DISTINCT PPMessages.NoMsg, PPMessages.NoExpediteur,PPMessages.dateEnvoi, PPMessages.objet,PPDestinataires.EtatLu,PPMessages.DescMsg FROM PPMessages INNER JOIN PPDestinataires ON PPMessages.NoMsg = PPDestinataires.NoMsg WHERE PPMessages.NoExpediteur = @noDestinataire AND PPMessages.Lieu = @noLieu ORDER BY dateEnvoi DESC";
        }
        SqlCommand sqlComCourriels = new SqlCommand(strRequeteInfosCourriel, con);
        sqlComCourriels.Parameters.Add(new SqlParameter("noDestinataire", noCou));
        sqlComCourriels.Parameters.Add(new SqlParameter("noLieu", rblOrigine.SelectedIndex + 1));
        DataSet dataSetTable = new DataSet();
        SqlDataAdapter adapCourriel = new SqlDataAdapter(sqlComCourriels);
        adapCourriel.Fill(dataSetTable);
        pnC.Controls.Clear();
        if (dataSetTable.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dataSetTable.Tables[0].Rows.Count; i++)
            {
                //-----Expediteur-----
                String strNomExpediteur = "";
                PPClients clients = new PPClients();
                PPVendeurs vendeurs = new PPVendeurs();
                PPGestionnaires gestionnaires = new PPGestionnaires();
                //Client
                Client clientExp = clients.Values.Find(cli => cli.NoClient.ToString() == dataSetTable.Tables[0].Rows[i]["NoExpediteur"].ToString());
                if (clientExp != null)
                {
                    strNomExpediteur = clientExp.Prenom + " " + clientExp.Nom;
                }
                //Vendeur
                Vendeur vendeurExp = vendeurs.Values.Find(ven => ven.NoVendeur.ToString() == dataSetTable.Tables[0].Rows[i]["NoExpediteur"].ToString());
                if (vendeurExp != null)
                {
                    strNomExpediteur = vendeurExp.NomAffaires;
                }
                //Gestionnaire
                Gestionnaire gestionnaireExp = gestionnaires.Values.Find(ges => ges.NoAdmin.ToString() == dataSetTable.Tables[0].Rows[i]["NoExpediteur"].ToString());
                if (gestionnaireExp != null)
                {
                    strNomExpediteur = gestionnaireExp.Prenom + " " + gestionnaireExp.Nom;
                }
                //Destinataire
                String strNomDestinataire = "";
                PPDestinataires ppdest = new PPDestinataires();
                List<Destinataire> lsD = ppdest.Values.Where(d => d.NoMsg == int.Parse(dataSetTable.Tables[0].Rows[i]["NoMsg"].ToString())).ToList();
                Destinataire dern = lsD.Last();
                foreach (Destinataire m in lsD)
                {
                    Vendeur vendeur = vendeurs.Values.Find(ven => ven.NoVendeur == (long)m.NoDestinataire);
                    Gestionnaire admin = gestionnaires.Values.Find(adm => adm.NoAdmin == (long)m.NoDestinataire);
                    Client client = clients.Values.Find(c => c.NoClient == (long)m.NoDestinataire);
                    if (vendeur != null)
                    {
                        strNomDestinataire += vendeur.NomAffaires;
                    }
                    else if (admin != null)
                    {
                        strNomDestinataire += admin.Prenom + " " + admin.Nom;
                    }
                    else if (client != null)
                    {
                        if (!string.IsNullOrEmpty(client.Prenom) || !string.IsNullOrEmpty(client.Nom))
                            strNomDestinataire += client.Prenom + " " + client.Nom;
                        else
                            strNomDestinataire += client.AdresseEmail;
                    }
                    if (m != dern)
                    {
                        strNomDestinataire += ";";
                    }
                }
                //Table
                TableRow tbRow = tbCourriels.TrDyn();
                tbRow.ID = "tbRow" + i;
                tbRow.ClientIDMode = ClientIDMode.Static;
                String etatLu = dataSetTable.Tables[0].Rows[i]["EtatLu"].ToString();
                if (intlieu == 0) {
                    if (dataSetTable.Tables[0].Rows[i]["EtatLu"].ToString() == "1")
                    {
                        tbRow.CssClass = "rangeeNonLue cliquable";
                    }
                }
                else
                {
                    tbRow.CssClass = "rangeeLue cliquable";
                }
                TableCell tbCoche = tbRow.TdDyn();
                tbCoche.Style.Value = "min-width:16px";
                CheckBox cbDel = tbCoche.CbDyn(dataSetTable.Tables[0].Rows[i]["NoMsg"].ToString());
                cbDel.ClientIDMode = ClientIDMode.Static;
                cbDel.CssClass = "cb";
                TableCell cellN = new TableCell();
                if ((rblOrigine.SelectedIndex == 0))
                {
                    cellN= tbRow.TdDyn(strNomExpediteur);
                }
                else
                {
                    cellN = tbRow.TdDyn(strNomDestinataire);
                }
                cellN.CssClass = "tdInbox tdCellN";
                if(rblOrigine.SelectedIndex == 2)
                {
                    TableCell cellN1 = new TableCell();
                    cellN1= tbRow.TdDyn(strNomExpediteur);
                    cellN1.CssClass = "tdInbox";
                }
                TableCell tdSujet = tbRow.TdDyn();
                tdSujet.CssClass = "tdInbox";
                tdSujet.Text = dataSetTable.Tables[0].Rows[i]["objet"].ToString().Replace("''", "'");
                TableCell tdDate = tbRow.TdDyn();
                tdDate.CssClass = "tdInbox";
                tdDate.Text = dataSetTable.Tables[0].Rows[i]["dateEnvoi"].ToString();
                TableCell tdBtn = tbRow.TdDyn();
                Button btn = pnC.BtnDyn("btn" + i, "btn" + i, rowClick, "btnRangee");
                btn.ClientIDMode = ClientIDMode.Static;
                btn.CommandName = dataSetTable.Tables[0].Rows[i]["NoMsg"].ToString();
                btn.CommandArgument = dataSetTable.Tables[0].Rows[i]["DescMsg"].ToString() + ";" + tbRow.ID;
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = btn.ClientID;
                trigger.EventName = "Click";
                UpdatePanel1.Triggers.Add(trigger);
                string scriptRefresh = "document.getElementById('" + btn.ClientID + "').click();";
                tbRow.Attributes["onclick"] = "document.getElementById('" + btn.ClientID + "').click();";
            }
        }
        con.Close();
    }
    void rowClick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string[] split = btn.CommandArgument.Split(';');
        lblContenu.Text = split[0].Replace("''","'");
        if (rblOrigine.SelectedIndex == 0)
        {
            string strLu = "UPDATE PPDestinataires SET EtatLu = 0 WHERE NoMsg = @noMsg";
            TableRow tbR = ((TableRow)tbCourriels.FindControl(split[1]));
            tbR.CssClass = "rangeeLue cliquable";
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionInfo"]);
            con.Open();
            SqlCommand sqlComCourriels = new SqlCommand(strLu, con);
            sqlComCourriels.Parameters.Add(new SqlParameter("noMsg", int.Parse(btn.CommandName)));
            sqlComCourriels.ExecuteNonQuery();
            con.Close();
        }
        

        ViewState["msgTrRep"] = btn.CommandName;
        PPMessages ppmsg = new PPMessages();
        PPDestinataires ppdst = new PPDestinataires();
        if (ppmsg.Values.Exists(m => m.NoMsg == int.Parse(btn.CommandName)))
        {
            Message msg = ppmsg.Values.Find(m => m.NoMsg == int.Parse(btn.CommandName));
            List<Destinataire> listeD = ppdst.Values.Where(d => d.NoMsg == int.Parse(btn.CommandName)).ToList();
            tbInfos.Controls.Clear();
            TableRow tbRDestinataire = tbInfos.TrDyn();
            //Table Infos
            //Destinataires
            tbRDestinataire.TdDyn("À:");
            TableCell tbCDest = tbRDestinataire.TdDyn();
            Destinataire dern = listeD.Last();
            PPClients clients = new PPClients();
            PPVendeurs vendeurs = new PPVendeurs();
            PPGestionnaires gestionnaires = new PPGestionnaires();
            tbCDest.Text = "";
            foreach (Destinataire d in listeD)
            {
                
                Vendeur vendeur = vendeurs.Values.Find(ven => ven.NoVendeur == (long)d.NoDestinataire);
                Gestionnaire admin = gestionnaires.Values.Find(adm => adm.NoAdmin == (long)d.NoDestinataire);
                Client client = clients.Values.Find(c => c.NoClient == (long)d.NoDestinataire);
                if (vendeur != null)
                {
                    tbCDest.Text += vendeur.NomAffaires;
                }
                else if (admin != null)
                {
                    tbCDest.Text += admin.Prenom + "," + admin.Nom;
                }
                else if (client != null)
                {
                    if(!string.IsNullOrEmpty(client.Prenom)&& !string.IsNullOrEmpty(client.Nom))
                    tbCDest.Text += client.Prenom + "," + client.Nom;
                    else
                    {
                        tbCDest.Text += client.AdresseEmail;
                    }
                }
                if (d != dern)
                {
                    tbCDest.Text += ";";
                }
            }
            //Expéditeur
            Vendeur vendeurExp = vendeurs.Values.Find(ven => ven.NoVendeur == (long)msg.NoExpediteur);
            Gestionnaire adminExp = gestionnaires.Values.Find(adm => adm.NoAdmin == (long)msg.NoExpediteur);
            Client clientExp = clients.Values.Find(c => c.NoClient == (long)msg.NoExpediteur);
            TableRow trExp = tbInfos.TrDyn();
            trExp.TdDyn("De:");
            if (vendeurExp != null)
            {
                trExp.TdDyn(vendeurExp.NomAffaires);
            }
            else if (adminExp != null)
            {
                trExp.TdDyn(adminExp.Prenom + "," + adminExp.Nom);
            }
            else if (clientExp != null)
            {
                trExp.TdDyn(clientExp.Prenom + "," + clientExp.Nom);
            }
            TableRow trSujet = tbInfos.TrDyn();
            trSujet.TdDyn("Objet:");
            trSujet.TdDyn(msg.objet.Replace("''", "'"));
            //Fichier joint
            if (!string.IsNullOrEmpty(msg.FichierJoint))
            {
                    string strChem = Request.Url.GetLeftPart(UriPartial.Authority);
                    lienJoint.Text = msg.FichierJoint;
                    lienJoint.NavigateUrl = strChem + "/Envoyés/" + msg.NoMsg + "/" + msg.FichierJoint;
                    lienJoint.Visible = true;
            }
            else
            {
                    lienJoint.Visible = false;
            }
        }
        if (rblOrigine.SelectedIndex == 3)
        {
            ViewState["NoMsgBrouillon"] = btn.CommandName;
        }
        else
        {
            ViewState["NoMsgBrouillon"] = null;
        }
    }
    protected void btnAnnuler_Click(object sender, EventArgs e)
    {
        //Si accepter vendeur
        if ((postV.Value!= null)|| (postC.Value != null))
        {
            //var url = "Vendeur/GestionPanier.aspx";
            var url = ViewState["urlPagePrec"].ToString();
            Response.Clear();
            var sb = new System.Text.StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            sb.AppendFormat("<form action='{0}' method='post'>", url);
            sb.AppendFormat("<input type='hidden' name='decision' value='Annulé'>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();
        }
        else if ((postTous.Value != null))
        {
            //var url = "Vendeur/GestionPanier.aspx";
            var url = ViewState["urlPagePrec"].ToString();
            Response.Clear();
            var sb = new System.Text.StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            sb.AppendFormat("<form action='{0}' method='post'>", url);
            sb.AppendFormat("<input type='hidden' name='decision' value='Annulé'>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();
        }
        else
        {
            Response.Redirect("/Pages/Courriel.aspx");
        }
    }
    protected void btnClients_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbClients.Items)
            li.Selected = true;
    }

    protected void btnVendeurs_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbVendeurs.Items)
            li.Selected = true;
    }

    protected void btnAdmins_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAdmins.Items)
            li.Selected = true;
    }

    

    protected void btnOk_Click(object sender, EventArgs e)
    {
        txtDestinataire.Text = "";
        hidDestinataires.Value = "";
        
           foreach(ListItem li in lbVendeurs.Items)
           {
                if (li.Selected)
                {
                    txtDestinataire.Text += li.Text + ";";
                    hidDestinataires.Value += li.Value + ";";
                }
           }
           foreach (ListItem li in lbAdmins.Items)
           {
                if (li.Selected)
                {
                    txtDestinataire.Text += li.Text + ";";
                    hidDestinataires.Value += li.Value + ";";
                }
           }
           foreach (ListItem li in lbClients.Items)
           {
                if (li.Selected)
                {
                    txtDestinataire.Text += li.Text + ";";
                    hidDestinataires.Value += li.Value + ";";
                }
           }
        
        ModalPopupExtender1.Hide();
    }



    protected void btnSauvegarder_Click(object sender, EventArgs e)
    {
        if ((txtCourrielAEnvoyer.Text.Trim().Length != 0) && (txtDestinataire.Text.Trim().Length != 0) && (txtSujet.Text.Trim().Length != 0))
        {
            string[] strDestinataires = hidDestinataires.Value.Split(';');

            Array.Resize(ref strDestinataires, strDestinataires.Length - 1);
            int noCou = 0;
            if (Session.IsClient())
            {
                Client client = Session.GetClient();
                noCou = (int)client.NoClient;
            }
            if (Session.IsVendeur())
            {
                Vendeur vendeur = Session.GetVendeur();
                noCou = (int)vendeur.NoVendeur;
            }
            if (Session.IsAdmin())
            {
                Gestionnaire gestionnaire = Session.GetAdmin();
                noCou = (int)gestionnaire.NoAdmin;
            }
            PPMessages mes = new PPMessages();
            int mess = (int)mes.NextId();
            PPMessages messages = new PPMessages();
            String strSujSansG = txtSujet.Text.Replace("'", "''");
            String strM = txtCourrielAEnvoyer.Text.Replace("'", "''");
            Message newMessage = new Message(null)
            {
                NoExpediteur = noCou,
                NoMsg = mess,
                dateEnvoi = DateTime.Now,
                Lieu = 2,
                objet = strSujSansG,
                DescMsg = strM,
            };
            if (this.fuAttachement.HasFile)
            {
                String fichierChemin = "~/Envoyés/"+ mess+"/" + fuAttachement.FileName;
                string dossier = Server.MapPath("~/Envoyés/" + mess + "/");
                if (!Directory.Exists(dossier))
                {
                    Directory.CreateDirectory(dossier);
                }

                this.fuAttachement.SaveAs(dossier + fuAttachement.FileName);
                newMessage.FichierJoint = fuAttachement.FileName;
            }
            
            else if ((!string.IsNullOrEmpty(fichierBrouillon.Value))&&(ViewState["NoMsgBrouillon"]!=null))
            {
                Message messBrouillon = messages.Values.Find(m => m.NoMsg == int.Parse(ViewState["NoMsgBrouillon"].ToString()));
                String strfichierCheminSource = Server.MapPath("~/Envoyés/" + ViewState["NoMsgBrouillon"].ToString() + "/" + fichierBrouillon.Value);
                string dossier = Server.MapPath("~/Envoyés/" + mess + "/");
                if (!Directory.Exists(dossier))
                {
                    Directory.CreateDirectory(dossier);
                }
                
                File.Copy(strfichierCheminSource, dossier + fichierBrouillon.Value);
                newMessage.FichierJoint = fichierBrouillon.Value;
                fichierBrouillon.Value = "";
                lblFichierBrouillon.Text = "";
            }
            else if ((!string.IsNullOrEmpty(fichierBrouillon.Value)) && (!string.IsNullOrEmpty(ViewState["msgTrRep"].ToString())))
            {
                Message messBrouillon = messages.Values.Find(m => m.NoMsg == int.Parse(ViewState["msgTrRep"].ToString()));
                String strfichierCheminSource = Server.MapPath("~/Envoyés/" + ViewState["msgTrRep"].ToString() + "/" + fichierBrouillon.Value);
                string dossier = Server.MapPath("~/Envoyés/" + mess + "/");
                if (!Directory.Exists(dossier))
                {
                    Directory.CreateDirectory(dossier);
                }

                File.Copy(strfichierCheminSource, dossier + fichierBrouillon.Value);
                newMessage.FichierJoint = fichierBrouillon.Value;
                fichierBrouillon.Value = "";
                lblFichierBrouillon.Text = "";
            }

            messages.Add(newMessage);
            for (int i = 0; i < strDestinataires.Length; i++)
            {
                
                PPDestinataires destinataires = new PPDestinataires();
                Destinataire newDestinataire = new Destinataire(null)
                {
                    NoMsg = mess,
                    NoDestinataire = int.Parse(strDestinataires[i]),
                    Lieu = 1,
                    EtatLu = 1
                };
                destinataires.Add(newDestinataire);
            }
            if (!string.IsNullOrEmpty(postC.Value) || !string.IsNullOrEmpty(postV.Value) || !string.IsNullOrEmpty(postTous.Value))
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Succès!", "alert('Le message a été envoyé avec succès.');", true);
                var url = ViewState["urlPagePrec"].ToString();
                Response.Clear();
                var sb = new System.Text.StringBuilder();
                sb.Append("<html>");
                sb.AppendFormat("<body onload='document.forms[0].submit()'>");
                sb.AppendFormat("<form action='{0}' method='post'>", url);
                sb.AppendFormat("<input type='hidden' name='decision' value='Envoyé'>");

                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");
                Response.Write(sb.ToString());
                Response.End();
            }
            else
            {
                ViewState["NoMsgBrouillon"] = null;
                ViewState["msgTrRep"] = null;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Succès!", "alert('Le message a été envoyé avec succès.'); window.location.href = window.location.href;", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Erreur!", "alert('Le contenu du message, la liste de destinataires et le sujet du message ne peuvent pas être vides.')", true);
        }
    }

    protected void btnBrouillon_Click(object sender, EventArgs e)
    {
        if ((txtCourrielAEnvoyer.Text.Trim().Length != 0) && (txtDestinataire.Text.Trim().Length != 0) && (txtSujet.Text.Trim().Length != 0))
        {
            string[] strDestinataires = hidDestinataires.Value.Split(';');

            Array.Resize(ref strDestinataires, strDestinataires.Length - 1);
            int noCou = 0;
            if (Session.IsClient())
            {
                Client client = Session.GetClient();
                noCou = (int)client.NoClient;
            }
            if (Session.IsVendeur())
            {
                Vendeur vendeur = Session.GetVendeur();
                noCou = (int)vendeur.NoVendeur;
            }
            if (Session.IsAdmin())
            {
                Gestionnaire gestionnaire = Session.GetAdmin();
                noCou = (int)gestionnaire.NoAdmin;
            }
            PPMessages mes = new PPMessages();
            int mess = (int)mes.NextId();
            PPMessages messages = new PPMessages();
            String strM = txtCourrielAEnvoyer.Text.Replace("'", "''");
            String strSujSansG = txtSujet.Text.Replace("'", "''");
            Message newMessage = new Message(null)
            {
                NoExpediteur = noCou,
                NoMsg = mess,
                dateEnvoi = DateTime.Now,
                Lieu = 4,
                objet = strSujSansG,
                DescMsg = strM
                
            };
            if (this.fuAttachement.HasFile)
            {
                String fichierChemin = "~/Envoyés/" + mess + "/" + fuAttachement.FileName;
                string dossier = Server.MapPath("~/Envoyés/" + mess + "/");
                if (!Directory.Exists(dossier))
                {
                    Directory.CreateDirectory(dossier);
                }

                this.fuAttachement.SaveAs(dossier + fuAttachement.FileName);
                newMessage.FichierJoint = fuAttachement.FileName;
            }

            messages.Add(newMessage);
            for (int i = 0; i < strDestinataires.Length; i++)
            {

                PPDestinataires destinataires = new PPDestinataires();
                Destinataire newDestinataire = new Destinataire(null)
                {
                    NoMsg = mess,
                    NoDestinataire = int.Parse(strDestinataires[i]),
                    Lieu = 5,
                    EtatLu = 1
                };
                destinataires.Add(newDestinataire);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Succès!", "alert('Le message a été enregistré comme brouillon avec succès.'); window.location.href = window.location.href;", true);
            ClientScript.RegisterStartupScript(this.GetType(), "Succès!", "alert('Le message a été envoyé avec succès.')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Erreur", "alert('Le contenu du message, la liste de destinataires et le sujet du message ne peuvent pas être vides.');", true);
            ClientScript.RegisterStartupScript(this.GetType(), "Erreur!", "alert('Le contenu du message, la liste de destinataires et le sujet du message ne peuvent pas être vides.')", true);
        }
    }



    protected void btnModifierBrouillon_Click(object sender, EventArgs e)
    {
        rblOrigine.SelectedIndex = 4;
        pnRedactionCourriel.Visible = true;
        pnChoixCourriel.Visible = false;
        if (ViewState["NoMsgBrouillon"].ToString() != null)
        {
            //Si brouillon
            PPDestinataires ppdest = new PPDestinataires();
            PPMessages ppmess = new PPMessages();
            Message mess = ppmess.Values.Find(m => m.NoMsg == int.Parse(ViewState["NoMsgBrouillon"].ToString()));
            List<Destinataire> lsD = ppdest.Values.Where(d => d.NoMsg == int.Parse(ViewState["NoMsgBrouillon"].ToString())).ToList();
            Destinataire dern = lsD.Last();
            foreach (Destinataire m in lsD)
            {
                hidDestinataires.Value += m.NoDestinataire.ToString() +";";
                PPClients clients = new PPClients();
                PPVendeurs vendeurs = new PPVendeurs();
                PPGestionnaires gestionnaires = new PPGestionnaires();
                Vendeur vendeur = vendeurs.Values.Find(ven => ven.NoVendeur == (long)m.NoDestinataire);
                Gestionnaire admin = gestionnaires.Values.Find(adm => adm.NoAdmin == (long)m.NoDestinataire);
                Client client = clients.Values.Find(c => c.NoClient == (long)m.NoDestinataire);
                if (vendeur != null)
                {
                    txtDestinataire.Text += vendeur.NomAffaires + ";";
                }
                else if (admin != null)
                {
                    txtDestinataire.Text += admin.Nom + "," + admin.Prenom + ";";
                }
                else if (client != null)
                {
                    txtDestinataire.Text += client.Nom + "," + client.Prenom + ";";
                }

            }
            if (!string.IsNullOrEmpty(mess.FichierJoint))
            {
                fichierBrouillon.Value = mess.FichierJoint;
                lblFichierBrouillon.Text = "Ignorez le fileupload, le fichier " + mess.FichierJoint + " est déja chargé";
            }
            else
            {
                fichierBrouillon.Value = string.Empty;
                lblFichierBrouillon.Text = "";
            }
            txtCourrielAEnvoyer.Text = mess.DescMsg.Replace("''", "'");
            txtSujet.Text = mess.objet.Replace("''", "'");
        }
        else
        {

        }
    }

    protected void btnSupprimerMessage_Click(object sender, EventArgs e)
    {
        int nbCoche = 0;
        for(int i=1;i<tbCourriels.Rows.Count;i++)
        {
            TableRow tbR = tbCourriels.Rows[i];
            CheckBox cb = tbR.Cells[0].Controls[0] as CheckBox;
            int n=0;
            if ((cb.Checked)&&(int.TryParse(cb.ID,out n)))
            {
                nbCoche++;
                int noCou = 0;
                if (Session.IsClient())
                {
                    Client client = Session.GetClient();
                    noCou = (int)client.NoClient;
                }
                if (Session.IsVendeur())
                {
                    Vendeur vendeur = Session.GetVendeur();
                    noCou = (int)vendeur.NoVendeur;
                }
                if (Session.IsAdmin())
                {
                    Gestionnaire gestionnaire = Session.GetAdmin();
                    noCou = (int)gestionnaire.NoAdmin;
                }
                
                Console.WriteLine("Message "+cb.ID);
                PPMessages messages = new PPMessages();
                Message mess = messages.Values.Find(m => m.NoMsg == int.Parse(cb.ID));
                PPDestinataires destinataires = new PPDestinataires();
                List<Destinataire> listeD = destinataires.Values.Where(d => d.NoMsg == int.Parse(cb.ID)).ToList();
               
                    foreach (Destinataire d in listeD)
                    {
                        if (d.NoDestinataire == noCou)
                        {
                            if ((rblOrigine.SelectedIndex == 0) || (rblOrigine.SelectedIndex == 1))
                        {
                                d.Lieu = 3;
                            }
                            else
                            {
                                d.Lieu = 5;
                            }
                            destinataires.NotifyUpdatedOutside(d);
                            destinataires.Update();
                        }
                    }
                if (mess.NoExpediteur == noCou)
                {
                    if ((rblOrigine.SelectedIndex == 0)|| (rblOrigine.SelectedIndex == 1))
                    {
                        mess.Lieu = 3;
                    }
                    else
                    {
                        mess.Lieu = 5;
                    }
                }
                    messages.NotifyUpdatedOutside(mess);
                    messages.Update();
            }
        }
        if (nbCoche > 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Succès!", "alert('Les messages sélectionnés ont été supprimés avec succès.'); window.location.href = window.location.href;", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Erreur", "alert('Aucun message est sélectionné.');", true);
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        rblOrigine.SelectedIndex = 4;
        pnRedactionCourriel.Visible = true;
        pnChoixCourriel.Visible = false;
        if (ViewState["msgTrRep"].ToString() != null)
        {
            //Charger message
            PPDestinataires ppdest = new PPDestinataires();
            PPMessages ppmess = new PPMessages();
            Message mess = ppmess.Values.Find(m => m.NoMsg == int.Parse(ViewState["msgTrRep"].ToString()));
            
            PPClients clients = new PPClients();
            PPVendeurs vendeurs = new PPVendeurs();
            PPGestionnaires gestionnaires = new PPGestionnaires();
            txtCourrielAEnvoyer.Text = "__________________________________" + Environment.NewLine + mess.DescMsg.Replace("''", "'");
            Button btn = (Button)sender;
            if (btn.ID == "btnRepondre")
            {
                hidDestinataires.Value += mess.NoExpediteur.ToString() + ";";
                Vendeur vendeur = vendeurs.Values.Find(ven => ven.NoVendeur == (long)mess.NoExpediteur);
                Gestionnaire admin = gestionnaires.Values.Find(adm => adm.NoAdmin == (long)mess.NoExpediteur);
                Client client = clients.Values.Find(c => c.NoClient == (long)mess.NoExpediteur);
                if (vendeur != null)
                {
                    txtDestinataire.Text += vendeur.NomAffaires + ";";
                }
                else if (admin != null)
                {
                    txtDestinataire.Text += admin.Nom + "," + admin.Prenom + ";";
                }
                else if (client != null)
                {
                    txtDestinataire.Text += client.Nom + "," + client.Prenom + ";";
                }
                    txtSujet.Text = "RE: " + mess.objet.Replace("''", "'");
            }
            
            
            else
            {
                txtSujet.Text = mess.objet.Replace("''", "'");
                 if (!string.IsNullOrEmpty(mess.FichierJoint))
                 {
                 fichierBrouillon.Value = mess.FichierJoint;
                        lblFichierBrouillon.Text = "Ignorez le fileupload, le fichier " + mess.FichierJoint + " est déja chargé";
                 }
                 else
                 {
                        fichierBrouillon.Value = string.Empty;
                        lblFichierBrouillon.Text = "";
                 }
            }
        }
    }

    protected void btnRestaurerMessages_Click(object sender, EventArgs e)
    {
        int nbCoche = 0;
        for (int i = 1; i < tbCourriels.Rows.Count; i++)
        {
            TableRow tbR = tbCourriels.Rows[i];
            CheckBox cb = tbR.Cells[0].Controls[0] as CheckBox;
            int n = 0;
            if ((cb.Checked) && (int.TryParse(cb.ID, out n)))
            {
                nbCoche++;
                int noCou = 0;
                if (Session.IsClient())
                {
                    Client client = Session.GetClient();
                    noCou = (int)client.NoClient;
                }
                if (Session.IsVendeur())
                {
                    Vendeur vendeur = Session.GetVendeur();
                    noCou = (int)vendeur.NoVendeur;
                }
                if (Session.IsAdmin())
                {
                    Gestionnaire gestionnaire = Session.GetAdmin();
                    noCou = (int)gestionnaire.NoAdmin;
                }
                Console.WriteLine("Message " + cb.ID);
                PPMessages messages = new PPMessages();
                Message mess = messages.Values.Find(m => m.NoMsg == int.Parse(cb.ID));
                PPDestinataires destinataires = new PPDestinataires();
                List<Destinataire> listeD = destinataires.Values.Where(d => d.NoMsg == int.Parse(cb.ID)).ToList();
                foreach (Destinataire d in listeD)
                {
                    if (d.NoDestinataire == noCou)
                    {
                        d.Lieu = 1;
                        destinataires.NotifyUpdatedOutside(d);
                        destinataires.Update();
                    }
                }
                if (mess.NoExpediteur == noCou)
                {
                    mess.Lieu = 2;
                }
                messages.NotifyUpdatedOutside(mess);
                messages.Update();
            }
        }
        if (nbCoche > 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Succès!", "alert('Les messages sélectionnés ont été restaurés avec succès.'); window.location.href = window.location.href;", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Erreur", "alert('Aucun message est sélectionné.');", true);
        }
    }
}
