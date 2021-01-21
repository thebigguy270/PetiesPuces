using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

public static class SessionManager
{
    public static readonly string Connecte = "Connecte";
    public static readonly string Type = "Type";
    public static readonly string RedirectConnexionLink = "~/Pages/Connexion.aspx";

    public static void SetClient(this HttpSessionState session, Client client)
    {
        session[Connecte] = client;
        session[Type] = "Client";
    }

    public static void SetVendeur(this HttpSessionState session, Vendeur vendeur)
    {
        session[Connecte] = vendeur;
        session[Type] = "Vendeur";
    }

    public static void SetAdmin(this HttpSessionState session, Gestionnaire admin)
    {
        //session[Connecte] = "Admin";
        session[Connecte] = admin;
        session[Type] = "Admin";
    }

    public static bool IsClient(this HttpSessionState session)
        => session[Connecte] != null && session[Connecte].GetType() == typeof(Client);

    public static bool IsVendeur(this HttpSessionState session)
        => session[Connecte] != null && session[Connecte].GetType() == typeof(Vendeur);

    public static bool IsAdmin(this HttpSessionState session)
        => session[Connecte] != null && session[Connecte].GetType() == typeof(Gestionnaire);

    public static Client GetClient(this HttpSessionState session)
        => (Client)session[Connecte];

    public static Vendeur GetVendeur(this HttpSessionState session)
        => (Vendeur)session[Connecte];

    public static Gestionnaire GetAdmin(this HttpSessionState session)
        => (Gestionnaire)session[Connecte];

    public static string GetTypeU(this HttpSessionState session)
        => session[Type] == null ? null : (string)session[Type];

    public static void Deconecte(this HttpSessionState session)
    {
        session[Type] = null;
        session[Connecte] = null;
    }
}