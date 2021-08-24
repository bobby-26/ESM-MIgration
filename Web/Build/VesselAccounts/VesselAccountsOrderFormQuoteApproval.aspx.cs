using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Data;
using Telerik.Web.UI;

public partial class VesselAccountsOrderFormQuoteApproval :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Place Order", "APPROVE",ToolBarDirection.Right);
        MenuApprovalRemarks.AccessRights = this.ViewState;
        MenuApprovalRemarks.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
          
            if (Request.QueryString["ORDERID"] != null && Request.QueryString["ORDERID"].ToString() != string.Empty)
            {
                ViewState["ORDERID"] = Request.QueryString["ORDERID"].ToString();
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
            }
        }
    }

    private void EditBondProvision(Guid gOrderId)
    {
        DataTable dt = PhoenixVesselAccountsOrderForm.EditOrderForm(gOrderId, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtApprovalRemarks.Text = dr["FLDAPPROVALREMARKS"].ToString();
        }
    }

    protected void MenuApprovalRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (General.GetNullableString(txtApprovalRemarks.Text) == null)
            {
                lblMessage.Text = "Remarks is required.";
                return;
            }

            string Script = "";

            if (ViewState["ORDERID"] != null && ViewState["ORDERID"] != null)
            {
                if (CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixVesselAccountsOrderForm.OrderFormQuotationApproval(new Guid(ViewState["ORDERID"].ToString()), txtApprovalRemarks.Text);
                }
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
