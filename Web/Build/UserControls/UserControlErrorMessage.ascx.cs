using System;

public partial class UserControls_UserControlErrorMessage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        pnlErrorMessage.Style["position"] = "fixed";
        pnlErrorMessage.Style["left"] = "50%";
        pnlErrorMessage.Style["filter"] = "alpha(opacity=95)";
        pnlErrorMessage.Style["-moz-opacity"] = ".95";
        pnlErrorMessage.Style["opacity"] = ".95";
        pnlErrorMessage.Style["z-index"] = "99";
        pnlErrorMessage.Style["background-color"] = "White";
        pnlErrorMessage.Style["transform"] = "translate(-50%, -50%)";
        pnlErrorMessage.Style["top"] = "30%";
        IsError = false;
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
        get
        {
            return spnHeaderMessage.InnerHtml;
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
            if (this.Visible)
            {
                spnErrorMessage.InnerHtml = "";
                this.Visible = false;
            }
            if(!value.Trim().Equals(""))
                spnErrorMessage.InnerHtml = spnErrorMessage.InnerHtml + "<font color='#ff0000'><b>* " + value + "</b></font><br/>";
        }
    }

    public string clear
    {
        set
        {
            spnErrorMessage.InnerHtml = "";
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
            if (this.Visible)
            {
                spnErrorMessage.InnerHtml = "";
                this.Visible = false;
            }
            IsError = true;
            if (!value.Trim().Equals(""))
                spnErrorMessage.InnerHtml = spnErrorMessage.InnerHtml + "<font color='#ff0000'><b>* " + value + "</b></font><br/>";
        }
    }

    protected void cmdClose_Click(object sender, EventArgs e)
    {
        spnErrorMessage.InnerHtml = "";
        this.Visible = false;
    }
}
