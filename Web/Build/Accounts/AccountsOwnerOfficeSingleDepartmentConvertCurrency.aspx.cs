using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsOwnerOfficeSingleDepartmentConvertCurrency : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "CONVERT",ToolBarDirection.Right);
            MenuSingleDepartment.AccessRights = this.ViewState;
            MenuSingleDepartment.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["OwnerOfficeAllocateId"] = "";
                if (Request.QueryString["OwnerOfficeAllocateId"] != null)
                    ViewState["OwnerOfficeAllocateId"] = Request.QueryString["OwnerOfficeAllocateId"];
                if (Request.QueryString["AllocatedAmount"] != null)
                    ViewState["AllocatedAmount"] = Request.QueryString["AllocatedAmount"];
                if (Request.QueryString["convertedAmount"] != null)
                    ucBankAmount.Text = Request.QueryString["convertedAmount"];
                txtCurrency.Text = Request.QueryString["Currency"];
                txtBankCurrency.Text = Request.QueryString["BankCurrency"];
                ucAmount.Text = ViewState["AllocatedAmount"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSingleDepartment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CONVERT"))
            {
                PhoenixAccountsOwnerOfficeSingleDepartment.FundReceivedVoucherAllocateConvert(new Guid(ViewState["OwnerOfficeAllocateId"].ToString())
                                                                                            , Convert.ToDecimal(ucBankAmount.Text)
                                                                                            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            );

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1');";
                Script += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
