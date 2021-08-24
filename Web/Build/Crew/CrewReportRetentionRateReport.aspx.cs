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

public partial class CrewReportRetentionRateReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Crew/CrewReportRetentionRateReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageLink("javascript:CallPrint('gvCrew')", "Print Grid", "icon_print.png", "PRINT");
            toolbar1.AddImageButton("../Crew/CrewReportRetentionRateReport.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Guidlines();
                PrincipalManagerClick(null, null);
            }
            ShowReport();
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Guidlines()
    {
        ucToolTipNW.Text = @"<table>
        <tr>
            <td>
                Please note:
            </td>
        </tr>
        <tr>
            <td>
                Corrected leavers-Number of seafarers who have left the organisation on their own
                accord(Total Leavers-Dismissed-Transferred to office)
            </td>
        </tr>
        <tr>
            <td>
                Head count-is the number of active seafarers in that rank on the “From” date of
                the filter
            </td>
        </tr>
        <tr>
            <td>
                All attrition-Total Leavers/Head count represented in percentage
            </td>
        </tr>
        <tr>
            <td>
                Corrected attrition-Corrected Leavers/Head Count represented in percentage
            </td>
        </tr>
        <tr>
            <td>
                All retention-100-All attrition represented in percentage
            </td>
        </tr>
        <tr>
            <td>
                Corrected retention-100 –Corrected attrition
            </td>
        </tr>
    </table>";
        imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
        imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucBatch.SelectedList = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucPrincipal.SelectedList = "";
                ucManager.SelectedList = "";
                ucRank.SelectedRankValue = "";
                lstPool.SelectedPool = "";
                ucDateFrom.Text = "";
                ucDateTo.Text = "";

                ShowReport();
                SetPageNavigator();
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
                if (!IsValidFilter(ucDateFrom.Text, ucDateTo.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    ShowReport();
                    SetPageNavigator();
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

        string[] alColumns = { "FLDRANKNAME", "FLDDISMISSED", "FLDRESIGNED", "FLDTTO", "FLDTOTALLEAVERS", "FLDCORRECTEDLEAVERS", "FLDHEADCOUNT", "FLDALLATTRITION", "FLDCORRECTEDATTRITION", "FLDALLRETENTION", "FLDCORRECTEDRETENTION" };
        string[] alCaptions = { "Rank", "Dismissed", "Resigned", "Transfer To Office", "Total Leavers", "Corrected Leavers", "Head Count", "All Attrition", "Corrected Attrition", "All Retention", "Corrected Retention" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportRetentionRate.RetentionRateSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                                                , (ucBatch.SelectedList) == "," ? null : General.GetNullableString(ucBatch.SelectedList)
                                                                , General.GetNullableDateTime(ucDateFrom.Text)
                                                                , General.GetNullableDateTime(ucDateTo.Text)
                                                                , (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                , (lstPool.SelectedPool) == "" ? null : General.GetNullableString(lstPool.SelectedPool)
                                                                , (ucManager.SelectedList) == "" ? null : General.GetNullableString(ucManager.SelectedList)
                                                                , (ucZone.selectedlist) == "" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RetentionRateReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Retention Rate Report as on " + date + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
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
                    Response.Write("<center>" + dr[alColumns[i]] + "</center>");
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

        string[] alColumns = { "FLDRANKNAME", "FLDDISMISSED", "FLDRESIGNED", "FLDTTO", "FLDTOTALLEAVERS", "FLDCORRECTEDLEAVERS", "FLDHEADCOUNT", "FLDALLATTRITION", "FLDCORRECTEDATTRITION", "FLDALLRETENTION", "FLDCORRECTEDRETENTION" };
        string[] alCaptions = { "Rank", "Dismissed", "Resigned", "Transfer To Office", "Total Leavers", "Corrected Leavers", "Head Count", "All Attrition", "Corrected Attrition", "All Retention", "Corrected Retention" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportRetentionRate.RetentionRateSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                                                , (ucBatch.SelectedList) == "," ? null : General.GetNullableString(ucBatch.SelectedList)
                                                                , General.GetNullableDateTime(ucDateFrom.Text)
                                                                , General.GetNullableDateTime(ucDateTo.Text)
                                                                , (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                , (lstPool.SelectedPool) == "" ? null : General.GetNullableString(lstPool.SelectedPool)
                                                                , (ucManager.SelectedList) == "" ? null : General.GetNullableString(ucManager.SelectedList)
                                                                , (ucZone.selectedlist) == "" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Retention Rate Report", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrew.DataSource = ds;
            gvCrew.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCrew);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void gvCrew_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataRowView drv = (DataRowView)e.Row.DataItem;

            LinkButton lnk1 = (LinkButton)e.Row.FindControl("lnkDismissed");
            if (drv["FLDRANKID"].ToString() != "0")
                lnk1.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportRetentionRateReportDetails.aspx?principal=" + General.GetNullableString(ucPrincipal.SelectedList) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&fromdate=" + ucDateFrom.Text + "&todate=" + ucDateTo.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&manager=" + General.GetNullableString(ucManager.SelectedList) + "&rank=" + drv["FLDRANKID"].ToString() + "&zone=" + General.GetNullableString(ucZone.selectedlist) + "&type=1'); return false;");
            else
            {
                lnk1.Enabled = false;
                lnk1.Font.Bold = true;
            }


            LinkButton lnk2 = (LinkButton)e.Row.FindControl("lnkResigned");
            if (drv["FLDRANKID"].ToString() != "0")
                lnk2.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportRetentionRateReportDetails.aspx?principal=" + General.GetNullableString(ucPrincipal.SelectedList) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&fromdate=" + ucDateFrom.Text + "&todate=" + ucDateTo.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&manager=" + General.GetNullableString(ucManager.SelectedList) + "&rank=" + drv["FLDRANKID"].ToString() + "&zone=" + General.GetNullableString(ucZone.selectedlist) + "&type=2'); return false;");
            else
            {
                lnk2.Enabled = false;
                lnk2.Font.Bold = true;
            }
            LinkButton lnk3 = (LinkButton)e.Row.FindControl("lnkTTO");
            if (drv["FLDRANKID"].ToString() != "0")
                lnk3.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportRetentionRateReportDetails.aspx?principal=" + General.GetNullableString(ucPrincipal.SelectedList) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&fromdate=" + ucDateFrom.Text + "&todate=" + ucDateTo.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&manager=" + General.GetNullableString(ucManager.SelectedList) + "&rank=" + drv["FLDRANKID"].ToString() + "&zone=" + General.GetNullableString(ucZone.selectedlist) + "&type=3'); return false;");
            else
            {
                lnk3.Enabled = false;
                lnk3.Font.Bold = true;
            }
            LinkButton lnk4 = (LinkButton)e.Row.FindControl("lnkTotalLeavers");
            if (drv["FLDRANKID"].ToString() != "0")
                lnk4.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportRetentionRateReportDetails.aspx?principal=" + General.GetNullableString(ucPrincipal.SelectedList) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&fromdate=" + ucDateFrom.Text + "&todate=" + ucDateTo.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&manager=" + General.GetNullableString(ucManager.SelectedList) + "&rank=" + drv["FLDRANKID"].ToString() + "&zone=" + General.GetNullableString(ucZone.selectedlist) + "&type=4'); return false;");
            else
            {
                lnk4.Enabled = false;
                lnk4.Font.Bold = true;
            }
            LinkButton lnk5 = (LinkButton)e.Row.FindControl("lnkCorrectedLeavers");
            if (drv["FLDRANKID"].ToString() != "0")
                lnk5.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportRetentionRateReportDetails.aspx?principal=" + General.GetNullableString(ucPrincipal.SelectedList) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&fromdate=" + ucDateFrom.Text + "&todate=" + ucDateTo.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&manager=" + General.GetNullableString(ucManager.SelectedList) + "&rank=" + drv["FLDRANKID"].ToString() + "&zone=" + General.GetNullableString(ucZone.selectedlist) + "&type=5'); return false;");
            else
            {
                lnk5.Enabled = false;
                lnk5.Font.Bold = true;
            }
            LinkButton lnk6 = (LinkButton)e.Row.FindControl("lnkHeadCount");
            if (drv["FLDRANKID"].ToString() != "0")
                lnk6.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportRetentionRateReportDetails.aspx?principal=" + General.GetNullableString(ucPrincipal.SelectedList) + "&batch=" + General.GetNullableString(ucBatch.SelectedList) + "&fromdate=" + ucDateFrom.Text + "&todate=" + ucDateTo.Text + "&vesseltype=" + General.GetNullableString(ucVesselType.SelectedVesseltype) + "&pool=" + General.GetNullableString(lstPool.SelectedPool) + "&manager=" + General.GetNullableString(ucManager.SelectedList) + "&rank=" + drv["FLDRANKID"].ToString() + "&zone=" + General.GetNullableString(ucZone.selectedlist) + "&type=6'); return false;");
            else
            {
                lnk6.Enabled = false;
                lnk6.Font.Bold = true;
            }
        }
    }
    protected void PrincipalManagerClick(object sender, EventArgs e)
    {
        if (rblPrincipalManager.SelectedValue == "1")
        {
            ucManager.Visible = false;
            lblManager.Visible = false;
            ucManager.SelectedList = "";
            lblPrincipal.Visible = true;
            ucPrincipal.Visible = true;
            //dvAddressType.Attributes["class"] = "input_mandatory";
            //cblAddressType.Enabled = true;
            //ddlManager.Visible = false;
        }
        else
        {
            lblPrincipal.Visible = false;
            ucPrincipal.Visible = false;
            ucPrincipal.SelectedList = "";
            ucManager.Visible = true;
            lblManager.Visible = true;
        }
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCrew.EditIndex = -1;
        gvCrew.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        ShowReport();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCrew.SelectedIndex = -1;
        gvCrew.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        ShowReport();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
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
