using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsInvoiceLineItemPOReceivedBeforeInvoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");

            MenuRemarks.AccessRights = this.ViewState;
            MenuRemarks.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["InvoiceCode"] = Request.QueryString["InvoiceCode"];
                lblRemarks.Text = "Reason for PO Approved After invoice is Received (" + Request.QueryString["PoNumber"].ToString() + ")";
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixAccountsInvoice.InvoicePOReceivedRemarksUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["InvoiceCode"].ToString())
                    , new Guid(Request.QueryString["OrderID"].ToString())
                    , txtRemarks.Text);

                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
