using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public static class BootstrapServerExtensions
{
    private static readonly Regex regexCourriel = new Regex("^[a-zA-Z0-9.!#$%&'*+=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
    private static readonly Regex regexNomPrenom = new Regex(@"^(\w|\d)+((\ |')(\w|\d)+)*$");

    //public static 

    public static void AddError(this Panel pnl, string error) => pnl.LblDyn("", error, "alert alert-danger");

    public static void Invalidate(this TextBox tb) => tb.CssClass = "form-control is-invalid";
    public static void Invalidate(this DropDownList tb) => tb.CssClass = "form-control is-invalid";

    public static void Validate(this TextBox tb) => tb.CssClass = "form-control is-valid";
    public static void Validate(this DropDownList tb) => tb.CssClass = "form-control is-valid";

    public static void DefaultControl(this TextBox tb) => tb.CssClass = "form-control";
    public static void DefaultControl(this DropDownList tb) => tb.CssClass = "form-control";

    public static bool InvalidateIfEmpty(this DropDownList ddl, Label lblError, string message)
    {
        bool ret = ddl.SelectedValue == "";

        if (ret)
        {
            ddl.Invalidate();
            lblError.Text = message;
        }

        return ret;
    }

    public static bool InvalidateIfEmpty(this TextBox tb, Label lblError, string message)
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

    public static bool CheckFormat(this TextBox tb, Label lblError, string message, Regex regex)
    {
        bool ret = !regex.IsMatch(tb.Text);

        if (ret)
        {
            tb.Invalidate();
            lblError.Text = message;
        }

        return ret;
    }

    public static bool CheckFormatCourriel(this TextBox tb, Label lblError)
        => tb.CheckFormat(lblError, "Le courriel ne respecte pas le format voulu", regexCourriel);

    public static bool CheckFormatNomPrenom(this TextBox tb, Label lblError)
        => tb.CheckFormat(lblError, "Le champ doit respecter le format voulu", regexNomPrenom);

    public static bool CheckMatch(this TextBox tb, TextBox tb2, Label lblError, Label lblError2, string message)
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

    public static bool CheckInt(this TextBox tb, Label lblError, string message, out int outValue)
    {
        bool ret = !int.TryParse(tb.Text, out outValue);

        if (ret)
        {
            tb.Invalidate();
            lblError.Text = message;
        }

        return ret;
    }

    public static bool CheckIntOver0(this TextBox tb, Label lblError, int value)
    {
        bool ret = value < 0;

        if (ret)
        {
            tb.Invalidate();
            lblError.Text = "Le nombre entré doit être plus grand ou égal (>=) à 0";
        }

        return ret;
    }

    public static bool CheckDecimal(this TextBox tb, Label lblError, string message, out decimal outValue)
    {
        bool ret = !decimal.TryParse(tb.Text, out outValue);

        if (ret)
        {
            tb.Invalidate();
            lblError.Text = message;
        }

        return ret;
    }

    public static bool CheckDecimalOver0(this TextBox tb, Label lblError, decimal value)
    {
        bool ret = value < 0;

        if (ret)
        {
            tb.Invalidate();
            lblError.Text = "Le nom entré doit être plus grand ou égal (>=) à 0";
        }

        return ret;
    }

    public static bool CheckLength(this TextBox tb, Label lblError, string message, int len)
    {
        bool ret = tb.Text.Length != len;

        if (ret)
        {
            tb.Invalidate();
            lblError.Text = message;
        }

        return ret;
    }

    public static bool CheckContains(this TextBox tb, Label lblError, string message, string cont)
    {
        bool ret = tb.Text.Contains(cont);

        if (ret)
        {
            tb.Invalidate();
            lblError.Text = message;
        }

        return ret;
    }

    public static void CardDyn(this Panel pan, string mainId, string mainCss, Action<Label> lblTitleHandler, Action<Panel> pnlBodyHandler)
    {
        Panel card = pan.DivDyn(mainId, $"card {mainCss}");

        // Header
        Panel cardHeader = card.DivDyn("", "card-header bg-dark-blue");
        Label lblTitle = cardHeader.LblDyn("", "", "card-title");
        lblTitleHandler(lblTitle);

        // Body
        Panel body = card.DivDyn("", "card-body");
        pnlBodyHandler(body);
    }

    public static void CardDynCollapse(this Panel pan, string mainId, string cardCss, string collCss, Action<Panel> pnlTitleHandler, Action<Panel> pnlBodyHandler)
    {
        Panel card = pan.DivDyn(mainId, $"card {cardCss}");

        Panel cardHeader = card.DivDyn("", "card-header fake-button");
        pnlTitleHandler(cardHeader);

        Panel collapse = card.DivDyn("", $"collapse {collCss}");

        cardHeader.Attributes.Add("data-toggle", "collapse");
        cardHeader.Attributes.Add("data-target", $"#{collapse.ClientID}");
        cardHeader.Attributes.Add("aria-expanded", "false");
        cardHeader.Attributes.Add("aria-controls", collapse.ClientID);

        Panel body = collapse.DivDyn("", "card-body");
        pnlBodyHandler(body);
    }
}