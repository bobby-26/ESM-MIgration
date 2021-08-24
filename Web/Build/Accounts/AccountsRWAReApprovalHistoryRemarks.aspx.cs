using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsRWAReApprovalHistoryRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["Showall"] = string.Empty;
                if (Request.QueryString["orderID"] != null && Request.QueryString["orderID"] != string.Empty)
                    ViewState["orderid"] = Request.QueryString["orderID"];
                if (Request.QueryString["Showall"] != null && Request.QueryString["Showall"] != string.Empty)
                    ViewState["Showall"] = Request.QueryString["Showall"];

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Approve", "REAPPROVAL", ToolBarDirection.Right);

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
    protected void Rebind()
    {
        gvRevokeHistory.SelectedIndexes.Clear();
        gvRevokeHistory.EditIndexes.Clear();
        gvRevokeHistory.DataSource = null;
        gvRevokeHistory.Rebind();
    }
    protected void menuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("REAPPROVAL"))
            {
                if (!IsValidconfiguration(txtRemarks.Text))

                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsPOStaging.POStagingApprovalStatusUpdate(new Guid(ViewState["orderid"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode,General.GetNullableString(txtRemarks.Text));
                Rebind();
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
    private void BindData()
    {
        DataSet ds = new DataSet();
        if (General.GetNullableGuid(ViewState["orderid"].ToString()) != null)
        {
            ds = PhoenixAccountsPOStaging.RWAApprovalHistory(new Guid(ViewState["orderid"].ToString()), General.GetNullableInteger(ViewState["Showall"].ToString()));

            gvRevokeHistory.DataSource = ds;
        }
    }


    protected void gvRevokeHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}