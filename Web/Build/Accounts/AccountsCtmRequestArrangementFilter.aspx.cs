using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsCtmRequestArrangementFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtRefNo.Focus();
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtCtmReqNo", txtRefNo.Text);
            criteria.Add("txtCtmETA", txtFromDate.Text);
            criteria.Add("txtCtmETD", txtToDate.Text);
            criteria.Add("txtPaymentVoucherNo", txtPaymentVoucherNo.Text);
            criteria.Add("ddlStatus", ddlStatus.SelectedHard);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ddlport", ddlport.SelectedSeaport);


            Filter.CurrentCtmOfficeFilter = criteria;
        }
        Response.Redirect("AccountsCtmRequestArrangement.aspx?defaultFilter=0", false);
    }
}
