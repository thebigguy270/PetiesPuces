using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public static class DynamicControls
{
    public static WebControl B(this Control conteneur, string id) => (WebControl)conteneur.FindControl(id);

    public static Panel DivDyn(this Control conteneur, string id, string strStyle = "")
    {
        Panel panel = new Panel();
        panel.ID = id;
        panel.CssClass = strStyle;

        conteneur.Controls.Add(panel);

        return panel;
    }

    public static void BrDyn(this Control conteneur, int nb = 1)
    {
        for (int i = 0; i < nb; i++)
            conteneur.Controls.Add(new HtmlGenericControl("br"));
    }

    public static Button BtnDyn(this Control conteneur, string id, string text, EventHandler nomFonction, string strStyle = "")
    {
        Button btn = new Button
        {
            ID = id,
            Text = text,
            CssClass = strStyle
        };
        btn.Click += nomFonction;

        conteneur.Controls.Add(btn);

        return btn;
    }
    public static Button BtnDyn(this Control conteneur, string id, string text, EventHandler nomFonction, string strStyle = "",Boolean enabled=true)
    {
        Button btn = new Button
        {
            ID = id,
            Text = text,
            CssClass = strStyle,
            Enabled=enabled
        };
        btn.Click += nomFonction;

        conteneur.Controls.Add(btn);

        return btn;
    }
    public static Button BtnClientDyn(this Control conteneur, string id, string text, string onClientClick, string strStyle = "")
    {
        Button btn = new Button
        {
            ID = id,
            Text = text,
            OnClientClick = onClientClick,
            UseSubmitBehavior = false,
            CssClass = strStyle
        };

        conteneur.Controls.Add(btn);

        return btn;
    }

    public static CheckBox CbDyn(this Control conteneur, string id)
    {
        CheckBox cb = new CheckBox
        {
            ID = id
        };

        conteneur.Controls.Add(cb);

        return cb;
    }
    public static CheckBox CbDyn(this Control conteneur, string id,Boolean dowhat)
    {
        CheckBox cb = new CheckBox
        {
            ID = id
    };
        cb.Attributes.Add("onclick", "CheckClients(this)");
        conteneur.Controls.Add(cb);

        return cb;
    }
    public static ImageButton ImgBtnDyn(this Control control, string id, string imgUrl, ImageClickEventHandler handler, string style = "")
    {
        ImageButton imgBtn = new ImageButton();

        imgBtn.ID = id;
        imgBtn.ImageUrl = imgUrl;
        imgBtn.Click += handler;
        imgBtn.CssClass = style;

        control.Controls.Add(imgBtn);

        return imgBtn;
    }
    public static ImageButton ImgBtnDyn(this Control control, string id, string imgUrl, string jsEvent = "", string style = "")
    {
        ImageButton imgBtn = new ImageButton();

        imgBtn.ID = id;
        imgBtn.ImageUrl = imgUrl;
        imgBtn.OnClientClick = jsEvent;
        imgBtn.CssClass = style;

        control.Controls.Add(imgBtn);

        return imgBtn;
    }

    public static Image ImgDyn(this Control control, string id, string imgUrl, string style = "")
    {
        Image img = new Image();

        img.ID = id;
        img.ImageUrl = imgUrl;
        img.CssClass = style;

        control.Controls.Add(img);

        return img;
    }

    public static Label LblDyn(this Control conteneur, string id, string text, string style = "")
    {
        Label lbl = new Label();
        lbl.ID = id;
        lbl.Text = text;
        lbl.CssClass = style;

        conteneur.Controls.Add(lbl);

        return lbl;
    }

    public static RangeValidator RangeValidatorDyn(this Control conteneur,
        string id, string controlToValidate,
        string min, string max, ValidationDataType type)
    {
        RangeValidator rangeValidator = new RangeValidator();

        rangeValidator.ID = id;
        rangeValidator.ControlToValidate = controlToValidate;
        rangeValidator.MinimumValue = min;
        rangeValidator.MaximumValue = max;
        rangeValidator.Type = type;

        conteneur.Controls.Add(rangeValidator);

        return rangeValidator;
    }

    public static RequiredFieldValidator RequiredFieldValidatorDyn(this Control conteneur,
        string id, string controlToValidate)
    {
        RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();

        requiredFieldValidator.ID = id;
        requiredFieldValidator.ControlToValidate = controlToValidate;

        conteneur.Controls.Add(requiredFieldValidator);

        return requiredFieldValidator;
    }

    public static TextBox TbDyn(this Control conteneur,
        string id, string val, int maxLen, string style)
    {
        TextBox textBox = new TextBox();

        textBox.ID = id;
        textBox.Text = val;
        textBox.MaxLength = maxLen;
        textBox.CssClass = style;

        conteneur.Controls.Add(textBox);

        return textBox;
    }
    public static TextBox TbDyn(this Control conteneur,
        string id, string val, int maxLen, string style,Boolean active=true)
    {
        TextBox textBox = new TextBox();

        textBox.ID = id;
        textBox.Text = val;
        textBox.MaxLength = maxLen;
        textBox.CssClass = style;
        textBox.Enabled = active;
        conteneur.Controls.Add(textBox);

        return textBox;
    }
    public static TextBox TbDyn(this Control conteneur,
       string id, string val)
    {
        TextBox textBox = new TextBox();

        textBox.ID = id;
        textBox.Text = val;

        conteneur.Controls.Add(textBox);

        return textBox;
    }
    public static Table TableDyn(this Control conteneur,
        string id, string style)
    {
        Table table = new Table
        {
            ID = id,
            CssClass = style
        };

        conteneur.Controls.Add(table);

        return table;
    }

    public static TableHeaderRow ThrDyn(this Table table)
    {
        TableHeaderRow row = new TableHeaderRow();

        table.Rows.Add(row);

        return row;
    }

    public static TableRow TrDyn(this Table table)
    {
        TableRow row = new TableRow();

        table.Rows.Add(row);

        return row;
    }
    public static TableRow TrDyn(this Table table,string strid)
    {
        TableRow row = new TableRow();
        row.ID = strid;
        table.Rows.Add(row);

        return row;
    }
    public static TableHeaderCell ThdDyn(this TableHeaderRow row)
    {
        TableHeaderCell cell = new TableHeaderCell();

        row.Cells.Add(cell);

        return cell;
    }

    public static TableHeaderCell ThdDyn(this TableHeaderRow row,
        string text)
    {
        TableHeaderCell cell = new TableHeaderCell
        {
            Text = text
        };

        row.Cells.Add(cell);

        return cell;
    }

    public static TableCell TdDyn(this TableRow row)
    {
        TableCell cell = new TableCell();

        row.Cells.Add(cell);

        return cell;
    }

    public static TableCell TdDyn(this TableRow row, string text)
    {
        TableCell cell = new TableCell
        {
            Text = text
        };

        row.Cells.Add(cell);

        return cell;
    }
    public static TableCell TdDyn(this TableRow row, string text, string style)
    {
        TableCell cell = new TableCell
        {
            Text = text,
            CssClass = style
        };

        row.Cells.Add(cell);

        return cell;
    }
    /* public static TableCell TdDyn(this TableRow row,
    string id, string style)
    {
        TableCell cell = new TableCell
        {
            ID = id,
            CssClass = style
        };

        row.Cells.Add(cell);

        return cell;
    }*/
    public static HyperLink HlinkDYN(this Control conteneur, string id,string url,string txt, string css = "")
    {
        HyperLink hlink = new HyperLink
        {
            ID = id,
            NavigateUrl = url,
            Text = txt,
            CssClass = css
        };
        conteneur.Controls.Add(hlink);
        return hlink;
    }

    public static DropDownList DdlDyn(this Control conteneur, string id, string jsOnChange, string css = "")
    {
        DropDownList ddl = new DropDownList()
        {
            ID = id,
            CssClass = css,
        };
        ddl.Attributes.Add("OnChange", jsOnChange);

        conteneur.Controls.Add(ddl);

        return ddl;
    }
}