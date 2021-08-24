using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class UserControlConfirmMessagePurchaseSendMail : System.Web.UI.UserControl
{

    public event EventHandler ConfirmMesage;
    private bool visible = false;
    public int confirmboxvalue = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        IsError = false;       
        if (!IsPostBack)
        {            
            bind();
        }
    }
    public void bind()
    {
        try
        {
            chkOPtion.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 186);
            chkOPtion.DataBind();           
        }
        catch { }
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
            spnErrorMessage.InnerHtml = "<font color='0000CC'><b> " + value + "</b></font><br/>";
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
        int a = 0;
        foreach (ButtonListItem li in chkOPtion.Items)
        {
            if (li.Selected)
            {
                a++;
            }
        }
        if (a < 1)
        {
            spnErrorMessage.InnerHtml = "<font color='Red'> * Please select reason for, do not send Email.</font>";
            IsError = true;
            return;
        }
        if (txtRemarks.Enabled)
        {
            if (string.IsNullOrEmpty(txtRemarks.Text.Trim()))
            {
                spnErrorMessage.InnerHtml = "<font color='Red'> * Remarks is required.</font>";
                IsError = true;
                return;
            }
        }
        ConfirmMesageCommand(e);
        this.Visible = false;
    }

    protected void ImgClose_Click(object sender, EventArgs e)
    {        
      this.Visible = false;        
    }

    protected void chkOPtion_Changed(object sender,EventArgs e)
    {
        int a =0 ;
        string Others=PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 186, "OTS").ToString();

        foreach (ButtonListItem li in chkOPtion.Items)
        {
            if (li.Selected)
            {
                if (li.Value.ToString() == Others)
                    a = 1;
            }
        }
        if(a.ToString() == "1")
        
            txtRemarks.Enabled = true;
        else
            txtRemarks.Enabled = false;

        spnErrorMessage.InnerHtml = "";
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
    public string Reason
    {
        get
        {
            string str = string.Empty;
            foreach (ButtonListItem li in chkOPtion.Items)
            {
                str += (li.Selected ? li.Value + "," : string.Empty);
            }
            return str.TrimEnd(',');
        }
        set
        {
            string reason = value;
            if (!string.IsNullOrEmpty(reason))
            {
                foreach (string val in reason.Split(','))
                {
                    foreach (ButtonListItem li in chkOPtion.Items)
                    {
                        if (li.Value == val.ToString())
                        {
                            li.Selected = true;
                        }
                    }

                }
            }
            else
            {
                foreach (ButtonListItem li in chkOPtion.Items)
                {
                    li.Selected = false;
                }
            }
        }
    }
    public string Remark
    {
        get
        {
            return General.GetNullableString(txtRemarks.Text.Trim());
        }
        set
        {
            txtRemarks.Text = value;
        }
    }
    
}
