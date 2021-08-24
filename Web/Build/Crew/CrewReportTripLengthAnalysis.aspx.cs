using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewReportTripLengthAnalysis : PhoenixBasePage
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


            PhoenixToolbar ToolbarALLCrew = new PhoenixToolbar();
            ToolbarALLCrew.AddFontAwesomeButton("../Crew/CrewReportTripLengthAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            ToolbarALLCrew.AddFontAwesomeButton("javascript:CallPrint('gvAllCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            ToolbarALLCrew.AddFontAwesomeButton("../Crew/CrewReportTripLengthAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            ToolbarALLCrew.AddFontAwesomeButton("../Crew/CrewReportTripLengthAnalysis.aspx", "Chart", "<i class=\"fas fa-chart-bar\"></i>", "Chart");
            MenuAllCrew.AccessRights = this.ViewState;
            MenuAllCrew.MenuList = ToolbarALLCrew.Show();

            ShowReportAllCrew();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAllCrew_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowAllCrewExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucDateFrom.Text = "";
                ucDateTo.Text = "";
                ucRank.selectedlist = "";
                ucPool.SelectedPool = "";
                ucVesselType.SelectedVesseltype = "";

                ShowReportAllCrew();
            }
            if (CommandName.ToUpper().Equals("CHART"))
            {
                ChartExcel();
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
                if (!IsValidFilter(ucDateFrom.Text, ucDateTo.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ShowReportAllCrew();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowAllCrewExcel()
    {
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANK", "FLDDURATION" };
        string[] alCaptions = { "Rank", "Trip Length" };

        if (ucDateFrom.Text != null && ucDateTo.Text != null)
        {

            ds = PhoenixCrewReportTripLengthAnalysis.TripLengthAnalysisSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                            , General.GetNullableDateTime(ucDateFrom.Text)
                                                                            , General.GetNullableDateTime(ucDateTo.Text)
                                                                            , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                            , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype), 1);
        }
        else
        {
            ds = PhoenixCrewReportTripLengthAnalysis.TripLengthAnalysisSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                            , General.GetNullableDateTime(ucDateFrom.Text)
                                                                            , General.GetNullableDateTime(ucDateTo.Text)
                                                                            , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                            , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype), 0);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=TripLengthAnalysis.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
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
    private void ShowReportAllCrew()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANK", "FLDDURATION" };
        string[] alCaptions = { "Rank", "Trip Length" };

        if (ucDateFrom.Text != null && ucDateTo.Text != null)
        {

            ds = PhoenixCrewReportTripLengthAnalysis.TripLengthAnalysisSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                            , General.GetNullableDateTime(ucDateFrom.Text)
                                                                            , General.GetNullableDateTime(ucDateTo.Text)
                                                                            , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                            , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype), 1);
        }
        else
        {
            ds = PhoenixCrewReportTripLengthAnalysis.TripLengthAnalysisSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                            , General.GetNullableDateTime(ucDateFrom.Text)
                                                                            , General.GetNullableDateTime(ucDateTo.Text)
                                                                            , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                            , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype), 0);
        }

        General.SetPrintOptions("gvAllCrew", "TRIP LENGTH ANALYSIS", alCaptions, alColumns, ds);

        gvAllCrew.DataSource = ds;
        gvAllCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvAllCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel rankid = (RadLabel)e.Item.FindControl("lblRankId");
            LinkButton lbr1 = (LinkButton)e.Item.FindControl("lnkRank");
            lbr1.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportTripLength.aspx?rank=" + rankid.Text + "&fromdate=" + ucDateFrom.Text + "&todate=" + ucDateTo.Text + "&pool=" + General.GetNullableString(ucPool.SelectedPool) + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "'); return false;");
        }

    }



    protected void ChartExcel()
    {
        DataSet ds = new DataSet();

        if (ucDateFrom.Text != null && ucDateTo.Text != null)
        {

            ds = PhoenixCrewReportTripLengthAnalysis.TripLengthAnalysisSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                            , General.GetNullableDateTime(ucDateFrom.Text)
                                                                            , General.GetNullableDateTime(ucDateTo.Text)
                                                                            , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                            , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype), 1);
        }
        else
        {
            ds = PhoenixCrewReportTripLengthAnalysis.TripLengthAnalysisSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                            , General.GetNullableDateTime(ucDateFrom.Text)
                                                                            , General.GetNullableDateTime(ucDateTo.Text)
                                                                            , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                            , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype), 0);
        }
        DataTable dt1 = ds.Tables[0];
        if (dt1.Rows.Count > 0)
        {
            
            string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Crew/Excel/";
            string filename = strpath + "JuniorTripLength.xls";
            if (ucDateFrom.Text != null && ucDateTo.Text != null)
            {
                PhoenixCrew2XL.Export2ExcelTripLength(filename, ucRank.selectedlist, ucDateFrom.Text, ucDateTo.Text, ucPool.SelectedPool, ucVesselType.SelectedVesseltype, "1");
            }
            else
            {
                PhoenixCrew2XL.Export2ExcelTripLength(filename, ucRank.selectedlist, ucDateFrom.Text, ucDateTo.Text, ucPool.SelectedPool, ucVesselType.SelectedVesseltype, "0");
            }
        }

    }
    protected void gvAllCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvAllCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAllCrew.CurrentPageIndex + 1;

        ShowReportAllCrew();
    }
    protected void gvAllCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

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
}
