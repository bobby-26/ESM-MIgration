using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsRecruitmentStatisticsReport : PhoenixBasePage
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

            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            toolbarmenu.AddFontAwesomeButton("../Crew/CrewReportsRecruitmentStatisticsReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbarmenu.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarmenu.AddFontAwesomeButton("../Crew/CrewReportsRecruitmentStatisticsReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbarmenu.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUMMARY"] = "1";
                txtFromDate.Text = DateTime.Parse(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01").ToString();
                txtToDate.Text = LastDayOfMonthFromDateTime(DateTime.Now).ToString();
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
                txtFromDate.Text = DateTime.Parse(DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/01").ToString();
                txtToDate.Text = LastDayOfMonthFromDateTime(DateTime.Now).ToString();
                ucZone.SelectedZoneValue = "";
                ucBatch.SelectedBatch = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucVessel.SelectedVessel = "";
                lstPool.SelectedPool = "";
                ucRank.selectedlist = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlrefferredby.SelectedQuick = "";
                ShowReport();
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
                if (!IsValidFilter(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    ShowReport();
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
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDAGE", "FLDRANKCODE", "FLDZONE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSOURCEDFROM", "FLDCONVERTEE", "FLDFULLCONTRACT" };
        string[] alCaptions = { "Name", "Age", "Rank", "Zone", "Batch", "Vessel Joined", "SignOn Date", "Source", "Convertee", "Full Contract" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewStatisticsReport.CrewStatisticsReport(
                (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist),
                (lstPool.SelectedPool) == "Dummy" ? null : General.GetNullableString(lstPool.SelectedPool),
                General.GetNullableInteger(ddlrefferredby.SelectedQuick),
                General.GetNullableDateTime(txtFromDate.Text),
                General.GetNullableDateTime(txtToDate.Text),
                sortexpression, sortdirection,
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RecruitmentStatistics.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Recruitment Statistics Report</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length - 1).ToString() + "' align='left'><b>From Date:" + txtFromDate.Text + "</b></td><td><b>To Date:" + txtToDate.Text + "</b></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        if (ViewState["SUMMARY"].ToString() == "1")
        {
            Response.Write("<br />");
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
        else
        {
            if (ds.Tables.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    sb.Append("<table cellspacing=\"2\" cellpadding=\"2\" border=\"1\"> ");
                    sb.Append("<tr><td align='CENTER'><b>ZONE</b></td>");

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        sb.Append("<td colspan='3' align='CENTER'><b>");
                        sb.Append(dr["FLDCOLUMNHEADER"].ToString());
                        sb.Append("</b></td>");
                    }

                    sb.Append("<td align='CENTER'><b>Total</b></td></tr>");
                    sb.Append("<tr><td></td>");
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        sb.Append("<td align='CENTER'><b>Engaged</b></td><td align='CENTER'><b>Re-Engaged</b></td><td align='CENTER'><b>Target</b></td>");
                    }
                    sb.Append("<td></td>");
                    sb.Append("</tr>");

                    //PRINTING         
                    DataTable dt = ds.Tables[3];
                    DataTable tdt = ds.Tables[4];
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        sb.Append("<tr><td align='left'><b>" + dr["FLDROWHEADER"].ToString() + "</b></td>");
                        foreach (DataRow dr1 in ds.Tables[1].Rows)
                        {
                            DataRow[] drv = dt.Select("FLDZONE = " + dr["FLDROWID"].ToString() + " AND FLDRANK=" + dr1["FLDCOLUMNID"].ToString());
                            DataRow[] drv1 = tdt.Select("FLDZONEID = " + dr["FLDROWID"].ToString() + " AND FLDRANKID=" + dr1["FLDCOLUMNID"].ToString());
                            sb.Append("<td align='center'>");
                            sb.Append((drv.Length > 0 ? drv[0]["FLDENGAGED"].ToString() : "0"));
                            sb.Append("</td>");
                            sb.Append("<td align='center'>");
                            sb.Append((drv.Length > 0 ? drv[0]["FLDREENGAGED"].ToString() : "0"));
                            sb.Append("</td>");
                            sb.Append("<td align='center'>");
                            sb.Append((drv1.Length > 0 ? drv1[0]["FLDNOOFVACANT"].ToString() : "0"));
                            sb.Append("</td>");
                        }

                        DataRow[] drv2 = dt.Select("FLDZONE = " + dr["FLDROWID"].ToString() + " AND FLDRANK IS NULL");
                        sb.Append("<td align='right'><b>" + (drv2.Length > 0 ? drv2[0]["FLDTOTAL"].ToString() : "0") + "</b></td></tr>");

                    }
                    sb.Append("<tr><td colspan='" + ((ds.Tables[1].Rows.Count * 3) + 1) + "' align='right'><b>Total</b></td>");
                    DataRow[] drv3 = dt.Select("FLDZONE IS NULL AND FLDRANK IS NULL");

                    sb.Append("<td align='right'><b>");
                    sb.Append((drv3.Length > 0 ? drv3[0]["FLDTOTAL"].ToString() : "0"));
                    sb.Append("</b></td>");

                    sb.Append("</tr>");

                    sb.Append("</table>");

                }
                else
                {


                    sb.Append("<table cellspacing=\"2\" cellpadding=\"2\" border=\"1\"> ");

                    sb.Append("<tr style=\"height:10px;\"><td style=\"height:15px;\"></td></tr>");
                    sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                    sb.Append("</table>");
                }
                Response.Write(sb.ToString());
                Response.End();
            }
        }

    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDAGE", "FLDRANKCODE", "FLDZONE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSOURCEDFROM", "FLDCONVERTEE", "FLDFULLCONTRACT" };
        string[] alCaptions = { "Name", "Age", "Rank", "Zone", "Batch", "Vessel Joined", "SignOn Date", "Source", "Convertee", "Full Contract" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewStatisticsReport.CrewStatisticsReport(
                (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                (ucBatch.SelectedBatch.ToString()) == "Dummy" ? null : General.GetNullableInteger(ucBatch.SelectedBatch.ToString()),
                (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist),
                (lstPool.SelectedPool) == "Dummy" ? null : General.GetNullableString(lstPool.SelectedPool),
                General.GetNullableInteger(ddlrefferredby.SelectedQuick),
                General.GetNullableDateTime(txtFromDate.Text),
                General.GetNullableDateTime(txtToDate.Text),
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);


        General.SetPrintOptions("gvCrew", "Crew Recruitment Statistics Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

     protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewList','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

        }
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
    public bool IsValidFilter(string fromdate, string todate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(fromdate).Equals(null))
        {
            ucError.ErrorMessage = "From Date is Required";
        }
        if (General.GetNullableDateTime(todate).Equals(null))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        return (!ucError.IsError);

    }

    private DateTime LastDayOfMonthFromDateTime(DateTime dateTime)
    {
        DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
        return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
    
}
