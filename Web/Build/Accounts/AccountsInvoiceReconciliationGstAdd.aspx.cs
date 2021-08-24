using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using System.Data;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;


public partial class Accounts_AccountsInvoiceReconciliationGstAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuOfficeFilterMain.Title = "Add GST";
        if (!IsPostBack)
        {
            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["ORDERID"].ToString();
            }
            else
            {
                ViewState["orderid"] = "";
            }
        }

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (string.IsNullOrEmpty(ucValue.Text))
            {
                ucError.ErrorMessage = "Value has to be entered";
                ucError.Visible = true;
                return;
            }
            else
            {
                try
                {
                    PhoenixAccountsPOStaging.InvoiceReconciliationStagingGstAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), "GST", General.GetNullableDecimal(ucValue.Text));

                    String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
        }
    }    
}
