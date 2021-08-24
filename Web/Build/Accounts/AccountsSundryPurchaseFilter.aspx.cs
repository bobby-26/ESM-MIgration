using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsSundryPurchaseFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuOfficeFilterMain.Title = "Sundry Purchase Filter";
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
       
        txtRefNo.Focus();
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtRefNo", txtRefNo.Text);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            criteria.Add("ddlStockType", ddlStockType.SelectedHard);
            criteria.Add("ddlStatus", ddlStatus.SelectedHard);

            Filter.CurrentSundryPurchaseFilter = criteria;
        }
        Response.Redirect("AccountsSundryPurchase.aspx", false);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }   
}
