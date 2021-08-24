using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsBankStatementExcelUploadFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);


        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Bank Statement excel upload filter";
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

            txtBankAccount.Focus();
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtBankAccount", txtBankAccount.Text.Trim());
            criteria.Add("txtAccDesc", txtAccDesc.Text.Trim());
            criteria.Add("ucCurrency", ucCurrency.SelectedCurrency);
            criteria.Add("ddlType", ddlType.SelectedValue);
            criteria.Add("ucMonth", ucMonth.SelectedHard);
            criteria.Add("chkExclPostedBankStmt", chkExclPostedBankStmt.Checked==true ? "1" : "0");
            criteria.Add("chkExclArchivedBankStmt", chkExcludeArchived.Checked == true ? "1" : "0");

            Filter.CurrentBankStatementUploadSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
           // Filter.CurrentBankStatementUploadSelection = criteria;
        }
        Response.Redirect("../Accounts/AccountsBankStatementExcelUpload.aspx", false);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
