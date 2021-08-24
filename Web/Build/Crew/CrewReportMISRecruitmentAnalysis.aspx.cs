using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsMISReports_Recruitment_Analysis_ : PhoenixBasePage
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

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISRecruitmentAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISRecruitmentAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                ViewState["SHOWREPORT"] = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucDate.Text = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPoolValue = "";
                ucFleet.SelectedFleetValue = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucPrincipal.SelectedAddress = "";
                ucManager.SelectedAddress = "";

                ViewState["PAGENUMBER"] = 1;
                gvCrew.CurrentPageIndex = 0;
                ShowReport();
                gvCrew.Rebind();
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
                if (!IsValidFilter(ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    gvCrew.CurrentPageIndex = 0;
                    ShowReport();
                    gvCrew.Rebind();
                }
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKNAME", "FLDNOOFSTAFF", "FLDTOTALCOMPANIES", "FLDAVERAGE" };
        string[] alCaptions = { "Rank", "No.of Staff", "Total Companies Served for Last 3 Yrs", "Avg. Companies Served for Last 3Yrs" };
        string[] filtercolumns = { "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE", "FLDSELECTEDZONE", "FLDSELECTEDPOOL", "FLDSELECTEDFLEET", "FLDSELECTEDPRINCIPAL" };
        string[] filtercaptions = { "From", "To", "Zone", "Pool", "Fleet", "Principal" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixCrewReportMIS.CrewReportMISRecruitment(
                General.GetNullableDateTime(ucDate.Text),
                General.GetNullableDateTime(ucDate1.Text),
                (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist),
                (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                (ucFleet.SelectedList) == "," ? null : General.GetNullableString(ucFleet.SelectedList),
                General.GetNullableInteger(ucPrincipal.SelectedAddress),
                General.GetNullableInteger(ucManager.SelectedAddress),
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MISRecruitmentAnalysisReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Staff Recruited Upto:"+date+"</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKNAME", "FLDNOOFSTAFF", "FLDTOTALCOMPANIES", "FLDAVERAGE" };
        string[] alCaptions = { "Rank", "No.of Staff", "Total Companies Served for Last 3 Yrs", "Avg. Companies Served for Last 3Yrs" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISRecruitment(
               General.GetNullableDateTime(ucDate.Text),
                General.GetNullableDateTime(ucDate1.Text),
                (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist),
                (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                (ucFleet.SelectedList) == "," ? null : General.GetNullableString(ucFleet.SelectedList),
                General.GetNullableInteger(ucPrincipal.SelectedAddress),
                General.GetNullableInteger(ucManager.SelectedAddress),
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
        General.SetPrintOptions("gvCrew", "Crew MIS Recruitment Analysis", alCaptions, alColumns, ds);
        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        
        if (ds.Tables[0].Rows.Count > 1)
        {
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {
            ViewState["ROWSINGRIDVIEW"] = 0;
        }       
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            RadLabel rankid = (RadLabel)e.Item.FindControl("lblRank");
            RadLabel rankname = (RadLabel)e.Item.FindControl("lblRankName");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkDetails");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewReportMISRecruitmentAnalysisSeafarers.aspx?rankid=" + General.GetNullableInteger(rankid.Text)
                + "&fromdate=" + General.GetNullableDateTime(ucDate.Text)
                + "&todate=" + General.GetNullableDateTime(ucDate1.Text)
                + "&zonelist=" + General.GetNullableString(ucZone.selectedlist)
                + "&poollist=" + General.GetNullableString(ucPool.SelectedPool)
                + "&fleetlist=" + General.GetNullableString(ucFleet.SelectedList)
                + "&principal=" + General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString())
                + "&rankname=" + General.GetNullableString(rankname.Text) + "'); return false;");

            LinkButton lbr1 = (LinkButton)e.Item.FindControl("lnkComDeails");
            lbr1.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage1','','" + Session["sitepath"] + "/Crew/CrewReportMISRecruitmentAnalysisCompany.aspx?rankid=" + General.GetNullableInteger(rankid.Text)
                + "&fromdate=" + General.GetNullableDateTime(ucDate.Text)
                + "&todate=" + General.GetNullableDateTime(ucDate1.Text)
                + "&zonelist=" + General.GetNullableString(ucZone.selectedlist)
                + "&poollist=" + General.GetNullableString(ucPool.SelectedPool)
                + "&fleetlist=" + General.GetNullableString(ucFleet.SelectedList)
                + "&principal=" + General.GetNullableInteger(ucPrincipal.SelectedAddress.ToString())
                + "&rankname=" + General.GetNullableString(rankname.Text) + "'); return false;");
                       
        }
    }
    public bool IsValidFilter(string fromdate, string todate)
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
    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
        gvCrew.Rebind();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
