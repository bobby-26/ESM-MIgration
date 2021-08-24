using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceRequestsApprovedFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            //toolbar.AddButton("Cancel", "CANCEL");

            MenuOrderFormMain.MenuList = toolbar.Show();
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtVoucherNumber", txtVoucherNumeber.Text);
            criteria.Add("txtLicenceRequestNumber", txtLicenceRequestNumber.Text);
            criteria.Add("ucCurrency", ucCurrency.SelectedCurrency);
            criteria.Add("ucAddress",ucAddress.SelectedAddress);
            criteria.Add("ucComapny",ucComapny.SelectedCompany);
            criteria.Add("ucVessel",ucVessel.SelectedVessel);
            criteria.Add("txtCrewName",txtCrewName.Text);
            criteria.Add("ucRank",ucRank.SelectedRank);
            criteria.Add("ucFromDate",ucFromDate.Text);
            criteria.Add("ucToDate",ucToDate.Text);

            Filter.CurrentSelectedLicencePaymentFilter = criteria;
        }

        String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
    }
}
