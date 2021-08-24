using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsRWAApprovalHistoryRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {

                if (Request.QueryString["orderID"] != null && Request.QueryString["orderID"] != string.Empty)
                    ViewState["orderid"] = Request.QueryString["orderID"];
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Reject", "REJECT",ToolBarDirection.Right);

                // MenuHistory.Title = "Approval History " + " - " + strno;
                // MenuHistory.MenuList = header.Show();

                menuRemarks.AccessRights = this.ViewState;
                menuRemarks.MenuList = toolbar.Show();


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void menuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("REJECT"))
            {
                if (!IsValidconfiguration(txtRemarks.Text))

                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsPOStaging.RWAApprovalHistoryRemarksInsert(new Guid(ViewState["orderid"].ToString()),2, General.GetNullableString(txtRemarks.Text));


            }
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidconfiguration(string txtremarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

      
        if (txtremarks.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required.";
        return (!ucError.IsError);
    }

}