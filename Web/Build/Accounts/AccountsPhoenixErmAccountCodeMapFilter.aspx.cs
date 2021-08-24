using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;

public partial class AccountsPhoenixErmAccountCodeMapFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtBudgetId.Attributes.Add("style", "visibility:hidden");
        txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
        txtAccountId.Attributes.Add("style", "visibility:hidden;");
        txtBudgetCode.Attributes.Add("style", "visibility:hidden;");
        txtBudgetName.Attributes.Add("style", "visibility:hidden;");
        txtAccountSource.ReadOnly = true;
        txtAccountUsage.ReadOnly = true;
        
       // if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            toolbar.AddButton("Cancel", "CANCEL");
            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
        }        
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;        
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {            
            criteria.Clear();
            criteria.Add("PhoenixAccountCode", txtAccountCode.Text.Trim());
            criteria.Add("PhoenixAccountDescription", txtAccountDescription.Text.Trim());

            Filter.CurrentPhoenixErmAccountCodeMapSelection = criteria;
            Response.Redirect("../Accounts/AccountsPhoenixErmAccountCodeMap.aspx", false);
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentPhoenixErmAccountCodeMapSelection = criteria;
            Response.Redirect("../Accounts/AccountsPhoenixErmAccountCodeMap.aspx", false);
        }
    } 
}
