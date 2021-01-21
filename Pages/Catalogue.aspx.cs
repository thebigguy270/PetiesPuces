
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary>
/// Description résumée de Catalogue
/// </summary>
public partial class CatalogueCode : Page
{
    private void TableCatalogueHeader(Table table)
    {
        TableHeaderRow headerRow = table.ThrDyn();

        headerRow.ThdDyn("Numéro");
        headerRow.ThdDyn("Image");
        headerRow.ThdDyn("Catégorie");
        headerRow.ThdDyn("Nom");
        headerRow.ThdDyn("Prix demandé");
        headerRow.ThdDyn("Quantité");

    }
    private void TableCatalogueRangees(Table table)
    {
        Image img = new Image();
        img.ImageUrl = "~/Static/img/placeholder.png";
        img.CssClass = "imgResize";
         TableRow tbR = table.TrDyn();
        tbR.TdDyn("1");
        TableCell cellImg = tbR.TdDyn();
        cellImg.CssClass = "imgResize";
        cellImg.ImgDyn("image", "~/Static/img/placeholder.png","imgResize");
        tbR.TdDyn("Une catégorie");
        tbR.TdDyn("Une Chose");
        tbR.TdDyn("12$");
        tbR.TdDyn("12");
        TableCell cellAjPanier = tbR.TdDyn();
        cellAjPanier.BtnDyn("btnAjDnasPanier", "Ajouter dans le panier", null, "btn btn-secondary");
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        TableCatalogueHeader(tableProduits);
        TableCatalogueRangees(tableProduits);
    }

    protected void ouvrirPageAjoutProduit(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/Client/InscriptionProduit.aspx");
    }
}