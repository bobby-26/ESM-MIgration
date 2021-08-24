using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsProjectBillingItemFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Invoice";
        MenuOfficeFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["SelectedVesselList"] = "";
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
            criteria.Add("txtProjectBillingName", txtProjectBillingName.Text);
            criteria.Add("ucProjectBillingGroups", ucProjectBillingGroups.SelectedQuick);
            criteria.Add("ucCurrencyCode", ucCurrencyCode.SelectedCurrency);
            criteria.Add("ucBillingUnit", ucBillingUnit.SelectedUnit);


            Filter.CurrentSelectedProjectBillingItem = criteria;

            String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentSelectedProjectBillingItem = criteria;

            String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }

    }
}



