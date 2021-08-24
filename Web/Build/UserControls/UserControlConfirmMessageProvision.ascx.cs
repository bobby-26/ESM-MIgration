using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControlConfirmMessageProvision : System.Web.UI.UserControl
{

    public event EventHandler ConfirmMesage;
    private bool visible = false;
    public int confirmboxvalue = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
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
    }
    public string ActualVictualRate
    {
        set
        {
            txtActual.Text = value;
        }
    }
    public string BudgetedVictualRate
    {
        set
        {
            txtBudgeted.Text = value;
        }
    }
    public string reason
    {
        set
        {
            txtRemarks.Text = value;
        }
    }
    public string ErrorMessage
    {
        get
        {
            return spnErrorMessage.InnerText;
        }
        set
        {
            IsError = true;
            spnErrorMessage.InnerHtml = spnErrorMessage.InnerHtml + "<font color='#0000CC'><b> " + value + "</b></font><br/>";
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
            spnErrorMessage.InnerHtml = "<font color='#0000CC'><b> " + value + "</b></font><br/>";
        }
    }
    protected void ConfirmMesageCommand(EventArgs e)
    {
        if (ConfirmMesage != null)
            ConfirmMesage(this, e);
    }
    protected void cmdYes_Click(object sender, EventArgs e)
    {
        confirmboxvalue = 1;
        ConfirmMesageCommand(e);
        if (!visible)
        {
            spnErrorMessage.InnerHtml = "";
            this.Visible = false;
        }
    }
    protected void cmdNo_Click(object sender, EventArgs e)
    {
        confirmboxvalue = 0;
        ConfirmMesageCommand(e);
        spnErrorMessage.InnerHtml = "";
        this.Visible = false;
    }

    public string OKText
    {
        set
        {
            cmdYes.Text = value;
        }
    }
    public string CancelText
    {
        set
        {
            cmdNo.Text = value;
        }
    }
    public bool YesButtonVisible
    {
        set
        {
            cmdYes.Visible = value;
        }
    }
    public bool ConfirmVisible
    {
        set
        {
            visible = value;
        }
    }
    protected void pnlErrorMessage_Load(object sender, EventArgs e)
    {
        string str = "var dg = document.getElementById(\"ModalBG\");";
        str += "if (dg != null)";
        str += "{";
        str += "dg.style.width = Number((window.innerWidth ? window.innerWidth : (document.documentElement ? document.documentElement.clientWidth : (document.body ? document.body.clientWidth : 0))) + (window.pageXOffset ? window.pageXOffset : (document.documentElement ? document.documentElement.scrollLeft : (document.body ? document.body.scrollLeft : 0)))) + \"px\";";
        str += "dg.style.height = Number((window.innerHeight ? window.innerHeight : (document.documentElement ? document.documentElement.clientHeight : (document.body ? document.body.clientHeight : 0))) + (window.pageYOffset ? window.pageYOffset : (document.documentElement ? document.documentElement.scrollTop : (document.body ? document.body.scrollTop : 0)))) + \"px\";";
        str += "}";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", str, true);
    }
}
