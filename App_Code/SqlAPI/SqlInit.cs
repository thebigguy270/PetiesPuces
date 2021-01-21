using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class SqlInit
{
    public PPArticlesEnPanier articlesEnPanier = new PPArticlesEnPanier();
    public PPCategories categories = new PPCategories();
    public PPClients clients = new PPClients();
    public PPCommandes commandes = new PPCommandes();
    public PPDestinataires destinataires = new PPDestinataires();
    public PPDetailsCommandes detailsCommandes = new PPDetailsCommandes();
    public PPEvaluations evaluations = new PPEvaluations();
    public PPGestionnaires gestionnaires = new PPGestionnaires();
    public PPHistoriquePaiements historiquePaiements = new PPHistoriquePaiements();
    public PPLieu lieu = new PPLieu();
    public PPMessages messages = new PPMessages();
    public PPPoidsLivraisons poidsLivraisons = new PPPoidsLivraisons();
    public PPProduits produits = new PPProduits();
    public PPTaxeFederale taxeFederale = new PPTaxeFederale();
    public PPTaxeProvinciale taxeProvinciale = new PPTaxeProvinciale();
    public PPTypesLivraison typesLivraison = new PPTypesLivraison();
    public PPTypesPoids typesPoids = new PPTypesPoids();
    public PPVendeurs vendeurs = new PPVendeurs();
    public PPVendeursClients vendeursClients = new PPVendeursClients();
}