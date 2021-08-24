using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsRevenueVoucherFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
      
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);

        ucInvoiceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.VOUCHERSTATUS).ToString();
            MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Voucher";
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtVoucherNumberSearch.Focus();           
        
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
            criteria.Add("txtVoucherNumberSearch", txtVoucherNumberSearch.Text.Trim());
            criteria.Add("txtReferenceNumberSearch", txtReferenceNumberSearch.Text.Trim());
            criteria.Add("txtVoucherFromdateSearch", txtVoucherFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtVoucherTodateSearch.Text);
            criteria.Add("ucInvoiceStatus", ucInvoiceStatus.SelectedHard);
            criteria.Add("txtLongDescription", txtLongDescription.Text);
            Filter.CurrentInvoiceSelection = criteria;
        }
        else if(CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
           // Filter.CurrentInvoiceSelection = criteria;
        }
        Session["New"] = "Y";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}



