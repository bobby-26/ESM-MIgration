using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;

public partial class AccountsReportCommittedCostFormat1 : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
               
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";

            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=COMMITTEDCOSTFORMAT1&vessel=null&fromdate=null&todate=null&showmenu=0";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromdate.Text, ucToDate.Text,ucVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=COMMITTEDCOSTFORMAT1&vessel="+ ucVessel.SelectedVessel +"&fromdate="+ ucFromdate.Text +"&todate="+ ucToDate.Text + "&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string fromdate, string todate,string vessel)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To date should be later than 'From Date'";
        }
        if (vessel.Equals("") || vessel.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Vessel is required";
        }
        return (!ucError.IsError);
    }
}
