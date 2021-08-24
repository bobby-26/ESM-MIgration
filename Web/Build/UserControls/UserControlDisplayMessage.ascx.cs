using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControlDisplayMessage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        pnlErrorMessage.Style["position"] = "absolute";
        pnlErrorMessage.Style["left"] = "350px";
        pnlErrorMessage.Style["filter"] = "alpha(opacity=95)";
        pnlErrorMessage.Style["-moz-opacity"] = ".95";
        pnlErrorMessage.Style["opacity"] = ".95";
        pnlErrorMessage.Style["z-index"] = "99";
        pnlErrorMessage.Style["background-color"] = "White";
        IsError = false;
        spnErrorMessage.InnerHtml = "";
    }
    public bool IsError
    {
        get
        {
            return (bool)ViewState["ERROR"];
        }

        set
        {
            ViewState["ERROR"] = value;
        }
    }

    public string HeaderMessage
    {
        set
        {
            spnHeaderMessage.InnerHtml = "<h4>" + value + "</h4>";
        }
    }

    public string ErrorMessage
    {
        get
        {
            return spnErrorMessage.InnerHtml;
        }
        set
        {
            IsError = true;
            spnErrorMessage.InnerHtml = spnErrorMessage.InnerHtml + "<font color='#0000CC'><b>* " + value + "</b></font><br/>";
        }
    }

    public string Text
    {
        get
        {
            return spnErrorMessage.InnerText;
        }
        set
        {
            IsError = true;
            spnErrorMessage.InnerHtml = spnErrorMessage.InnerHtml +"<font color='#0000CC'><b>* " + value + "</b></font><br/>";
        }
    }

    protected void cmdClose_Click(object sender, EventArgs e)
    {
        spnErrorMessage.InnerHtml = "";
        this.Visible = false;
    }
}
