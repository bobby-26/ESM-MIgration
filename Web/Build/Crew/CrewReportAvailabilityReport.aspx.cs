using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewReports;
using System.Data;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewReportAvailabilityReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewReportAvailabilityReport.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            //ViewState["PAGEURL"] = "../Crew/CrewReportAvailabilityReportFormat1.aspx";
            
            if (!IsPostBack)
            {
                rblFormats.SelectedIndex = 0;
                ucDate1.Text = DateTime.Now.ToShortDateString();
            }
            if (rblFormats.SelectedIndex == 0)
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewReportAvailabilityReportFormat1.aspx";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "CrewReportAvailabilityReportFormat2.aspx";
            }
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
                if (!IsValidFilter(ucDate.Text, ucDate1.Text,rblFormats.SelectedIndex.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ucRank",(ucRank.selectedlist) == "," ? null : ucRank.selectedlist);
                    criteria.Add("ucNationality",(ucNationality.SelectedList)== "Dummy" ? null : ucNationality.SelectedList);
                    criteria.Add("ucZone", (ucZone.selectedlist) == "Dummy" ? null : ucZone.selectedlist);
                    criteria.Add("ucPool", (ucPool.SelectedPool) == "," ? null : ucPool.SelectedPool);
                    criteria.Add("ucName", txtName.Text);
                    criteria.Add("ucDate", ucDate.Text);
                    criteria.Add("ucDate1", ucDate1.Text);
                    criteria.Add("ucPrincipal", ucPrinicpal.SelectedAddress);
                    criteria.Add("newappln", chkNewApp.Checked ? "1" : "0");
                    criteria.Add("inactive", chkInactive.Checked ? "1" : "0");
                    Filter.CurrentAvailabilityFilter = criteria;
                    if (rblFormats.SelectedIndex == 0)
                    {
                        ifMoreInfo.Attributes["src"] = "../Crew/CrewReportAvailabilityReportFormat1.aspx" ;
                    }
                    else
                    {
                        ifMoreInfo.Attributes["src"] = "CrewReportAvailabilityReportFormat2.aspx" ;
                    }
                }
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucRank.SelectedRankValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPoolValue = "";
                txtName.Text = "";
                ucDate.Text = "";
                ucPrinicpal.SelectedAddress = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                Filter.CurrentAvailabilityFilter = null;
                if (rblFormats.SelectedIndex == 0)
                {
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewReportAvailabilityReportFormat1.aspx";
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "CrewReportAvailabilityReportFormat2.aspx";
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    public bool IsValidFilter(string fromdate, string todate,string format)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        if (format.Equals("")||format.Equals("-1"))
        {
            ucError.ErrorMessage = "Select Format";
        }
        return (!ucError.IsError);
    }
    public void rblFormats_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
