using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public static class BreadcrumbsButtons
{
    public static void PreviousBread(this Button btn, Button btnPrev)
    {
        // Current
        btn.CssClass = "btn";

        // Destination
        btnPrev.Enabled = false;
        btnPrev.CssClass = "btn font-weight-bold";
    }

    public static void NextBread(this Button btn, Button btnNext)
    {
        // Current
        btn.Enabled = true;
        btn.CssClass = "btn btn-secondary";

        // Destination
        btnNext.CssClass = "btn font-weight-bold";
    }

    public static void DisableBread(this Button btn)
    {
        btn.Enabled = false;
        btn.CssClass = "btn";
    }
}