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

public partial class CrewReportHeadCountReport : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportHeadCountReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportHeadCountReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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
                ucBatch.SelectedList = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucPrinicipal.SelectedAddress = "";
                ucRank.SelectedRankValue = "";
                lstPool.SelectedPool = "";
                lstZone.SelectedZoneValue = "";
                ucManager.SelectedAddress = "";
                ucDate.Text = "";
                ShowReport();
            }
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
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ViewState["PAGENUMBER"] = 1;
                ShowReport();
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

        string date = General.GetNullableDateTime(ucDate.Text).HasValue ? General.GetDateTimeToString(General.GetNullableDateTime(ucDate.Text).Value) : General.GetDateTimeToString(DateTime.Now);

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDRANKNAME", "FLDACTIVE", "FLDINACTIVE", "FLDINACTIVEEXM", "FLDONBOARD", "FLDONLEAVE", "FLDNTBR", "FLDRSUM" };
        string[] alCaptions = { "Rank", "Active", "InActive", "InActive Exam", "OnBoard", "OnLeave", "NTBR", "Total" };
        string[] filtercaptions = { "Principal", "Rank", "Batch", "Vessel Type" };
        string[] filtercolumns = { "FLDSELECTEDPRINCIPAL", "FLDSELECTEDBATCH", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDRANK" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportHeadCount.HeadAccountSearch((ucBatch.SelectedList) == "," ? null : General.GetNullableString(ucBatch.SelectedList),
                                        (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                                        General.GetNullableInteger(ucPrinicipal.SelectedAddress),
                                        (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                        , General.GetNullableDateTime(ucDate.Text),
                                        General.GetNullableInteger(ucManager.SelectedAddress),
                                        General.GetNullableString(lstPool.SelectedPool),
                                        General.GetNullableString(lstZone.selectedlist),
                                        1,
                                        iRowCount,
                                        ref iRowCount,
                                        ref iTotalPageCount
                                        );

        Response.AddHeader("Content-Disposition", "attachment; filename=Head_Count_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Head Count Report as on " + date + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'>Report Date : " + General.GetDateTimeToString(DateTime.Now) + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b><center>" + alCaptions[i] + "</center></b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                if (dr["FLDRANKID"].ToString().Equals("0"))
                {
                    Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                    Response.Write("<center><b>" + dr[alColumns[i]] + "</b></center>");
                    Response.Write("</td>");
                }
                else
                {
                    Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                    if (alColumns[i].Equals("FLDRANKNAME"))
                    {
                        Response.Write(dr[alColumns[i]]);
                    }
                    else if (alColumns[i].Equals("FLDRSUM"))
                    {
                        Response.Write("<center><b>" + dr[alColumns[i]] + "</b></center>");
                    }
                    else
                    {
                        Response.Write("<center>" + dr[alColumns[i]] + "</center>");
                    }
                    Response.Write("</td>");
                }
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDRANKNAME", "FLDACTIVE", "FLDINACTIVE", "FLDONBOARD", "FLDONLEAVE", "FLDNTBR", "FLDRSUM" };
        string[] alCaptions = { "Rank", "Active", "InActive", "OnBoard", "OnLeave", "NTBR", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportHeadCount.HeadAccountSearch((ucBatch.SelectedList) == "," ? null : General.GetNullableString(ucBatch.SelectedList),
                                (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                                        General.GetNullableInteger(ucPrinicipal.SelectedAddress),
                                        (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                                        General.GetNullableDateTime(ucDate.Text),
                                        General.GetNullableInteger(ucManager.SelectedAddress),
                                        General.GetNullableString(lstPool.SelectedPool),
                                        General.GetNullableString(lstZone.selectedlist),
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvCrew.PageSize,
                                        ref iRowCount,
                                        ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Head Count Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            RadLabel value = (RadLabel)e.Item.FindControl("lblActive");
            RadLabel rankid = (RadLabel)e.Item.FindControl("lblRankId1");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkActive");
            lbr.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportHeadCountReportDetails.aspx?rank=" + rankid.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&principal=" + General.GetNullableString(ucPrinicipal.SelectedAddress) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&type=" + value.Text + "&asondate=" + ucDate.Text + "&manager=" + ucManager.SelectedAddress + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&zone=" + General.GetNullableString(lstZone.selectedlist) + "'); return false;");

            RadLabel value1 = (RadLabel)e.Item.FindControl("lblInactive");
            RadLabel rankid1 = (RadLabel)e.Item.FindControl("lblRankId2");
            LinkButton lbr1 = (LinkButton)e.Item.FindControl("lnkInActive");
            lbr1.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportHeadCountReportDetails.aspx?rank=" + rankid1.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&principal=" + General.GetNullableString(ucPrinicipal.SelectedAddress) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&type=" + value1.Text + "&asondate=" + ucDate.Text + "&manager=" + ucManager.SelectedAddress + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&zone=" + General.GetNullableString(lstZone.selectedlist) + "'); return false;");

            RadLabel value2 = (RadLabel)e.Item.FindControl("lblOnBoard");
            RadLabel rankid2 = (RadLabel)e.Item.FindControl("lblRankId2");
            LinkButton lbr2 = (LinkButton)e.Item.FindControl("lnkOnBoard");
            lbr2.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportHeadCountReportDetails.aspx?rank=" + rankid2.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&principal=" + General.GetNullableString(ucPrinicipal.SelectedAddress) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&type=" + value2.Text + "&asondate=" + ucDate.Text + "&manager=" + ucManager.SelectedAddress + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&zone=" + General.GetNullableString(lstZone.selectedlist) + "'); return false;");

            RadLabel value3 = (RadLabel)e.Item.FindControl("lblOnLeave");
            RadLabel rankid3 = (RadLabel)e.Item.FindControl("lblRankId4");
            LinkButton lbr3 = (LinkButton)e.Item.FindControl("lnkOnLeave");
            lbr3.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportHeadCountReportDetails.aspx?rank=" + rankid3.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&principal=" + General.GetNullableString(ucPrinicipal.SelectedAddress) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&type=" + value3.Text + "&asondate=" + ucDate.Text + "&manager=" + ucManager.SelectedAddress + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&zone=" + General.GetNullableString(lstZone.selectedlist) + "'); return false;");

            RadLabel value4 = (RadLabel)e.Item.FindControl("lblNtbr");
            RadLabel rankid4 = (RadLabel)e.Item.FindControl("lblRankId5");
            LinkButton lbr4 = (LinkButton)e.Item.FindControl("lnkNtbr");
            lbr4.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportHeadCountReportDetails.aspx?rank=" + rankid4.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&principal=" + General.GetNullableString(ucPrinicipal.SelectedAddress) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&type=" + value4.Text + "&asondate=" + ucDate.Text + "&manager=" + ucManager.SelectedAddress + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&zone=" + General.GetNullableString(lstZone.selectedlist) + "'); return false;");

            RadLabel value5 = (RadLabel)e.Item.FindControl("lblInactiveExm");
            RadLabel rankid5 = (RadLabel)e.Item.FindControl("lblRankId6");
            LinkButton lbr5 = (LinkButton)e.Item.FindControl("lnkInActiveExm");
            lbr5.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportHeadCountReportDetails.aspx?rank=" + rankid5.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&principal=" + General.GetNullableString(ucPrinicipal.SelectedAddress) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&type=" + value5.Text + "&asondate=" + ucDate.Text + "&manager=" + ucManager.SelectedAddress + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&zone=" + General.GetNullableString(lstZone.selectedlist) + "'); return false;");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (drv["FLDRANKID"].ToString().Equals("0"))
            {
                lbr.Visible = false;
                lbr1.Visible = false;
                lbr2.Visible = false;
                lbr3.Visible = false;
                lbr4.Visible = false;
                lbr5.Visible = false;

                RadLabel lbrRank = (RadLabel)e.Item.FindControl("lblRankName");
                lbrRank.Visible = false;

                RadLabel lblTRank = (RadLabel)e.Item.FindControl("lblTotal");
                lblTRank.Visible = true;
                lblTRank.Style.Add("font-weight", "bold");

                RadLabel lblTActive = (RadLabel)e.Item.FindControl("lblTActive");
                lblTActive.Visible = true;
                lblTActive.Style.Add("font-weight", "bold");

                RadLabel lblTInactive = (RadLabel)e.Item.FindControl("lblTInactive");
                lblTInactive.Visible = true;
                lblTInactive.Style.Add("font-weight", "bold");

                RadLabel lblTInactiveExm = (RadLabel)e.Item.FindControl("lblTInactiveExm");
                lblTInactiveExm.Visible = true;
                lblTInactiveExm.Style.Add("font-weight", "bold");

                RadLabel lblTOnb = (RadLabel)e.Item.FindControl("lblTOnboard");
                lblTOnb.Visible = true;
                lblTOnb.Style.Add("font-weight", "bold");

                RadLabel lblTOnl = (RadLabel)e.Item.FindControl("lblTOnLeave");
                lblTOnl.Visible = true;
                lblTOnl.Style.Add("font-weight", "bold");

                RadLabel lblTNtrbr = (RadLabel)e.Item.FindControl("lblTNtbr");
                lblTNtrbr.Visible = true;
                lblTNtrbr.Style.Add("font-weight", "bold");

                RadLabel lblTSum = (RadLabel)e.Item.FindControl("lblrTotal");
                lblTSum.Visible = false;

            }
        }
    }

}
