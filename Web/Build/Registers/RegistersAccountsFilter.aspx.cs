using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class RegistersAccountsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ucGroupSearch.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTGROUP).ToString();
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            toolbar.AddButton("Cancel", "CANCEL");
            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtAccountCodeSearch.Focus();
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', 'ifMoreInfo');";
        Script += "</script>" + "\n";


        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {           
            int iChked = chkActiveSearch.Checked ? 1 : 0;
            criteria.Clear();
            criteria.Add("txtAccountCodeSearch", txtAccountCodeSearch.Text);
            criteria.Add("ucGroupSearch", ucGroupSearch.SelectedHard);
            criteria.Add("ddlAccountLevelSearch", ddlAccountLevelSearch.SelectedValue);
            criteria.Add("chkActiveSearch", iChked.ToString());
            Filter.CurrentAccountFilterSelection = criteria;
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentInvoiceSelection = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}



