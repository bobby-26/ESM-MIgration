using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsRemittanceOnHold : PhoenixBasePage
{
    public int iUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        ViewState["ONHOLD"] = "";
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {

            if (Request.QueryString["REMITTENCEID"] != null)
            {
                ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                RemittanceEdit();
            }
        }

        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        if (ViewState["ONHOLD"].ToString() == "ONHOLD")
        {
            toolbar1.AddButton("Reverse", "REVERSE",ToolBarDirection.Right);
        }
        else
        {
            toolbar1.AddButton("On Hold", "SAVE",ToolBarDirection.Right);
        }

        MenuCreditNote.AccessRights = this.ViewState;
        MenuCreditNote.MenuList = toolbar1.Show();
    }

    protected void MenuCreditNote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (ViewState["Remittenceid"] != null)
                {
                    PhoenixAccountsRemittance.OnHoldRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["Remittenceid"].ToString(), txtRemarks.Text);
                    ucStatus.Text = "Remittance Updated";
                }

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1', null, null);";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (CommandName.ToUpper().Equals("REVERSE"))
        {
            try
            {

                if (ViewState["Remittenceid"] != null)
                {
                    PhoenixAccountsRemittance.ReverseOnHoldRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["Remittenceid"].ToString());
                    ucStatus.Text = "Remittance Updated";
                }

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1', null, null);";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void RemittanceEdit()
    {
        if (ViewState["Remittenceid"] != null)
        {
            DataSet ds = PhoenixAccountsRemittance.Editremittance(ViewState["Remittenceid"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtRemittanceNumber.Text = dr["FLDREMITTANCENUMBER"].ToString();
                txtRemarks.Text = dr["FLDONHOLDREMARKS"].ToString();
                if (dr["FLDREMITTANCESTATUS"].ToString() == "672")
                    ViewState["ONHOLD"] = "ONHOLD";
            }
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
