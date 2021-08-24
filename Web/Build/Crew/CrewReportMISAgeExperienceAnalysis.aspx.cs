using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewReportMISAgeExperienceAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISAgeExperienceAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISAgeExperienceAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , 53, 1, "MDL,LSL,NTB,LFT,DTH,DST,EXM,TSP").Tables[0]);
                lstStatus.DataSource = ds;

                lstStatus.DataBind();

                lstStatus.Items.Insert(0, "--Select--");

                foreach (RadListBoxItem item in lstStatus.Items)
                {
                    if (item.Text == "--Select--")
                        item.Value = "";
                }
                lnkDetails.Visible = false;
                //ucDate1.Text = DateTime.Now.ToShortDateString();
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ViewState["RECORDCOUNT"] = null;
                //ShowReport();
            }
            gvCrew.PageSize = 10000;
            lnkDetails.Attributes.Add("onclick", "javascript:openNewWindow('Crew','','" + Session["sitepath"] + "/Crew/CrewReportMISAgeExperienceAnalysisDetails.aspx?fromdate=" +
                General.GetNullableDateTime(ucDate.Text) + "&todate=" + General.GetNullableDateTime(ucDate1.Text) +
                "&currentrank=" + General.GetNullableString(ucCRank.selectedlist) + "&zone=" + General.GetNullableString(ucZone.selectedlist) +
                "&nationality=" + General.GetNullableString(ucNationality.SelectedList) + "&pool=" + General.GetNullableString(ucPool.SelectedPool) +
                "&status=" + General.GetNullableString(StatusSelectedList()) + "&batch=" + General.GetNullableInteger(ucBatch.SelectedBatch) +
                "&GraterAge=" + General.GetNullableInteger(txtAge.Text) + "'); return false;");
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Selected == true && item.Value != "")
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        return strlist.ToString().TrimEnd(',');
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucBatch.SelectedBatch = "";
                ucZone.SelectedZoneValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucPool.SelectedPoolValue = "";
                ucCRank.SelectedRankValue = "";
                lstStatus.SelectedValue = "";
                ucDate.Text = "";
                ucDate1.Text = "";
                lnkDetails.Visible = false;

                ViewState["SHOWREPORT"] = null;
                ShowReport();
                //SetPageNavigator();
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                //if (!IsValidFilter(lstStatus.SelectedValue.ToString()))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                //else
                //{
                if (ucDate.Text == "" || ucDate.Text == null)
                {

                }
                else
                {
                    if (!IsValidDate(ucDate.Text, ucDate1.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                ShowReport();
                //SetPageNavigator();
                //}
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string date = DateTime.Now.ToShortDateString();

        string[] alColumns = { "FLDRANKNAME", "18-25", "26-30", "31-35", "36-40", "41-45", "46-50", "51-55", "56-60", ">60", "FLDHEADCOUNT", "FLDAVGRANKEXP" };
        string[] alCaptions = { "Rank", "18-25", "26-30", "31-35", "36-40", "41-45", "46-50", "51-55", "56-60", ">60", "Head Count", "Avg. Rank Exp." };

        DataSet ds = new DataSet();

        ds = PhoenixCrewReportMIS.CrewReportMISAgeExpAnalysis(
            (ucZone.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist),
            (ucPool.SelectedPool.ToString()) == "Dummy" ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
            (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
            (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
            (ucCRank.selectedlist.ToString()) == "," ? null : General.GetNullableString(ucCRank.selectedlist.ToString()),
            (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList()),
            General.GetNullableDateTime(ucDate.Text),
            General.GetNullableDateTime(ucDate1.Text), General.GetNullableInteger(txtAge.Text));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
                General.ShowExcel("MIS Age Experience Analysis", ds.Tables[0], alColumns, alCaptions, null, null);

        }
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDRANKNAME", "18-25", "26-30", "31-35", "36-40", "41-45", "46-50", "51-55", "56-60", ">60", "FLDHEADCOUNT", "FLDAVGRANKEXP" };
        string[] alCaptions = { "Rank", "18-25", "26-30", "31-35", "36-40", "41-45", "46-50", "51-55", "56-60", ">60", "Head Count", "Avg. Rank Exp." };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISAgeExpAnalysis(
            (ucZone.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist),
            (ucPool.SelectedPool.ToString()) == "Dummy" ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
            (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
            (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
            (ucCRank.selectedlist.ToString()) == "," ? null : General.GetNullableString(ucCRank.selectedlist.ToString()),
            (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList()),
            General.GetNullableDateTime(ucDate.Text),
            General.GetNullableDateTime(ucDate1.Text), General.GetNullableInteger(txtAge.Text));

        //General.SetPrintOptions("gvCrew", "Missing Course", alCaptions, alColumns, ds);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrew.DataSource = ds;
            lnkDetails.Visible = true;
            ViewState["RECORDCOUNT"] = 1;
        }       
        else
        {
            ViewState["RECORDCOUNT"] = null;
        }
    }
    public bool IsValidFilter(string status)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (status.Equals("") || status.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Status";
        }
        return (!ucError.IsError);
    }
    protected void EnableCBT(object sender, EventArgs args)
    {
        if (lstStatus.SelectedValue == PhoenixCommonRegisters.GetHardCode(1, 54, "ONL") || lstStatus.SelectedValue == PhoenixCommonRegisters.GetHardCode(1, 54, "OLP"))
        {
            ucDate.Enabled = false;
            ucDate1.Enabled = false;
            ucDate.CssClass = "readonlytextbox";
            ucDate1.CssClass = "readonlytextbox";
        }
        else
        {
            ucDate.Enabled = true;
            ucDate1.Enabled = true;
            ucDate.CssClass = "input";
            ucDate1.CssClass = "input";
        }
        ShowReport();

    }
    public bool IsValidDate(string fromdate, string todate)
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
        return (!ucError.IsError);
    }   

    int t1825 = 0, t2630 = 0, t3135 = 0, t3640 = 0, t4145 = 0, t4650 = 0, t5155 = 0, t5660 = 0, t60 = 0, thc = 0;
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (ViewState["RECORDCOUNT"] != null)
        //{
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            t1825 += int.Parse(drv["18-25"].ToString() == "" ? "0" : drv["18-25"].ToString());
            t2630 += int.Parse(drv["26-30"].ToString() == "" ? "0" : drv["26-30"].ToString());
            t3135 += int.Parse(drv["31-35"].ToString() == "" ? "0" : drv["31-35"].ToString());
            t3640 += int.Parse(drv["36-40"].ToString() == "" ? "0" : drv["36-40"].ToString());
            t4145 += int.Parse(drv["41-45"].ToString() == "" ? "0" : drv["41-45"].ToString());
            t4650 += int.Parse(drv["46-50"].ToString() == "" ? "0" : drv["46-50"].ToString());
            t5155 += int.Parse(drv["51-55"].ToString() == "" ? "0" : drv["51-55"].ToString());
            t5660 += int.Parse(drv["56-60"].ToString() == "" ? "0" : drv["56-60"].ToString());
            t60 += int.Parse(drv[">60"].ToString() == "" ? "0" : drv[">60"].ToString());
            thc += int.Parse(drv["FLDHEADCOUNT"].ToString() == "" ? "0" : drv["FLDHEADCOUNT"].ToString());
        }

        if (e.Item is GridFooterItem)
        {
            RadLabel lblTotal1825 = (RadLabel)e.Item.FindControl("lblTotal1825");
            if (lblTotal1825 != null) lblTotal1825.Text = t1825.ToString();

            RadLabel lblTotal2630 = (RadLabel)e.Item.FindControl("lblTotal2630");
            if (lblTotal2630 != null) lblTotal2630.Text = t1825.ToString();

            RadLabel lblTotal3135 = (RadLabel)e.Item.FindControl("lblTotal3135");
            if (lblTotal3135 != null) lblTotal3135.Text = t3135.ToString();

            RadLabel lblTotal3640 = (RadLabel)e.Item.FindControl("lblTotal3640");
            if (lblTotal3640 != null) lblTotal3640.Text = t3640.ToString();

            RadLabel lblTotal4145 = (RadLabel)e.Item.FindControl("lblTotal4145");
            if (lblTotal4145 != null) lblTotal4145.Text = t4145.ToString();

            RadLabel lblTotal4650 = (RadLabel)e.Item.FindControl("lblTotal4650");
            if (lblTotal4650 != null) lblTotal4650.Text = t4650.ToString();

            RadLabel lblTotal5155 = (RadLabel)e.Item.FindControl("lblTotal5155");
            if (lblTotal5155 != null) lblTotal5155.Text = t5155.ToString();

            RadLabel lblTotal5660 = (RadLabel)e.Item.FindControl("lblTotal5660");
            if (lblTotal5660 != null) lblTotal5660.Text = t5660.ToString();

            RadLabel lblTotal60 = (RadLabel)e.Item.FindControl("lblTotal60");
            if (lblTotal60 != null) lblTotal60.Text = t60.ToString();

            RadLabel lblTotalHeadCount = (RadLabel)e.Item.FindControl("lblTotalHeadCount");
            if (lblTotalHeadCount != null) lblTotalHeadCount.Text = thc.ToString();
        }
        //}
    }
    protected void gvCrew_ItemCreated(object sender, GridItemEventArgs e)
    {
     
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {         
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
