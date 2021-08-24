using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;
public partial class AccountsInvoiceQueryClarification : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInvoiceClarification.AccessRights = this.ViewState;
            MenuInvoiceClarification.Title = "Invoice Query";
            MenuInvoiceClarification.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuInvoiceClarification_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(txtClarification.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoiceQuery.InvoiceQueryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["invoicecode"].ToString())
                                                                , "C"
                                                                , txtClarification.Text
                                                                , null
                                                                , null);
                ucStatus.Text = "Clarification Inserted.";
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required.";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
}
