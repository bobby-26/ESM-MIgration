using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportReliefPlanningFormat1 : PhoenixBasePage
{

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrewReliefPlanning.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

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
            toolbar1.AddImageButton("../Crew/CrewReportReliefPlanningFormat1.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar1.AddImageLink("javascript:CallPrint('gvCrewReliefPlanning')", "Print Grid", "icon_print.png", "PRINT");
            toolbar1.AddImageButton("../Crew/CrewReportReliefPlanningFormat1.aspx", "Clear Filter", "clear-filter.png", "CLEAR");


            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBER2"] = 1;
                ViewState["SORTEXPRESSION2"] = null;
                ViewState["SORTDIRECTION2"] = null;

                rblFormats.SelectedIndex = 0;
            }

            if (rblFormats.SelectedIndex == 0)
            {
                divGrid.Visible = true;
                divGrid2.Visible = false;

                divPage.Visible = true;
                divPage2.Visible = false;
                

                ShowReport();
                SetPageNavigator();
            }
            else
            {
                divGrid.Visible = false;
                divGrid2.Visible = true;

                divPage.Visible = false;
                divPage2.Visible = true;

                ShowReportFormat2();
                SetPageNavigator2();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void rblFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFormats.SelectedIndex == 0)
        {
            gvCrewReliefPlanning.Visible = true;
            gvCrewReliefPlanningF2.Visible = false;
        }
        else
        {
            gvCrewReliefPlanning.Visible = false;
            gvCrewReliefPlanningF2.Visible = true;
        }
    }

    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                if (rblFormats.SelectedIndex == 0)
                    ShowExcel();
                else
                    ShowExcelF2();
            }

            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucVessel.SelectedVessel = "";
                ucRank.SelectedRankValue = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
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
                if (!IsValidDates(ucFromDate.Text, ucToDate.Text))
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
    private bool IsValidDates(string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following information";

        if (General.GetNullableDateTime(fromdate) == null)
            ucError.ErrorMessage = "From date required";
        if (General.GetNullableDateTime(todate) == null)
            ucError.ErrorMessage = "To date required";
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) > 0)
            ucError.ErrorMessage = "To date should be later than or equal to From date";

        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDOFFSIGNERRANK", "FLDOFFSIGNERNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDREMARKS" };
        string[] alCaptions = { "Vessel Name", "Rank", "Onboard", "Sign On Date", "Relief Date", "Reliever", "DOA", "EOL(Including activities)", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat1(
                    (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                    (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                    (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                    General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                    General.GetNullableDateTime(ucFromDate.Text),
                    General.GetNullableDateTime(ucToDate.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                    General.GetNullableString(ucZone.SelectedZoneValue) == "," ? null : General.GetNullableString(ucZone.SelectedZoneValue));

        string strVesselName = "";

        Response.AddHeader("Content-Disposition", "attachment; filename=Relief Planning.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Relief Planning</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' align='right'>Date:</td><td style='font-family:Arial; font-size:10px;'>" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 1; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (strVesselName != dr[alColumns[0]].ToString())
            {
                Response.Write("<tr>");
                Response.Write("<td colspan=8>");
                Response.Write(dr[alColumns[0]]);
                Response.Write("</td>");
                Response.Write("</tr>");
            }

            strVesselName = dr[alColumns[0]].ToString();
 
            Response.Write("<tr>");
            for (int i = 1; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }


    protected void ShowExcelF2()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDOFFSIGNERRANK", "FLDOFFSIGNERNAME", "FLDOFFSIGNERTOURCOUNT", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDRANKDECIMALEXPERIENCE", "FLDPRPLDECIMALEXPERIENCE", "FLDRELIVERTOURCOUNT" };
        string[] alCaptions = { "Vessel Name", "Rank", "Name", "Tour", "Relief Date", "Reliever Name", "DOA", "COL", "Time In Rank", "Time In BP", "Tour" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat2(
                    (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                    (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                    (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                    General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                    General.GetNullableDateTime(ucFromDate.Text),
                    General.GetNullableDateTime(ucToDate.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                    General.GetNullableString(ucZone.SelectedZoneValue) == "," ? null : General.GetNullableString(ucZone.SelectedZoneValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=Relief Planning.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Relief Planning</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' align='right'>Date:</td><td style='font-family:Arial; font-size:10px;'>" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
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

        string strVesselName = "";
        string strRank = "";
        string strName = "";
        string strtbl;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strtbl = "<tr>";

            for (int i = 0; i < alColumns.Length; i++)
            {
                if (i == 0)
                {
                    if (strVesselName == dr[alColumns[0]].ToString())
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += "</td>";
                    }
                    else
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += dr[alColumns[0]];
                        strtbl += "</td>";
                    }
                }
                else if (i == 1 || i == 2 || i == 3 || i == 6 || i == 7)
                {
                    if (strRank == dr[alColumns[1]].ToString() && strName == dr[alColumns[2]].ToString())
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += "</td>";
                    }
                    else
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += dr[alColumns[i]];
                        strtbl += "</td>";
                    }
                }
                else
                {
                    strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                    strtbl += dr[alColumns[i]];
                    strtbl += "</td>";
                }
            }
            strtbl += "</tr>";

            strVesselName = dr[alColumns[0]].ToString();
            strRank = dr[alColumns[1]].ToString();
            strName = dr[alColumns[2]].ToString(); 

            Response.Write(strtbl);
        }
        
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvCrewReliefPlanning_PreRender(object sender, EventArgs e)
    {
        try
        {
            GridDecorator.MergeRows(gvCrewReliefPlanning);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowReport()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDOFFSIGNERRANK", "FLDOFFSIGNERNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDREMARKS" };
        string[] alCaptions = { "Vessel Name", "Rank", "Onboard", "Sign On Date", "Relief Date", "Reliever", "DOA", "EOL(Including activities)", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat1(
                            (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                            (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                            (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                            General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                            General.GetNullableDateTime(ucFromDate.Text),
                            General.GetNullableDateTime(ucToDate.Text),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            General.ShowRecords(null),
                            ref iRowCount,
                            ref iTotalPageCount,
                            General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                            General.GetNullableString(ucZone.SelectedZoneValue) == "," ? null : General.GetNullableString(ucZone.SelectedZoneValue));

        General.SetPrintOptions("gvCrewReliefPlanning", "Relief Planning", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewReliefPlanning.DataSource = ds;
            gvCrewReliefPlanning.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCrewReliefPlanning);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCrewReliefPlanning.EditIndex = -1;
        gvCrewReliefPlanning.SelectedIndex = -1;
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
        gvCrewReliefPlanning.SelectedIndex = -1;
        gvCrewReliefPlanning.EditIndex = -1;
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
    protected void gvCrewReliefPlanning_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                Label empid = (Label)e.Row.FindControl("lblOffsignerID");
                LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

                Label relieverid = (Label)e.Row.FindControl("lblOnsignerID");
                Label reliever = (Label)e.Row.FindControl("lblRelieverName");
                LinkButton relivername = (LinkButton)e.Row.FindControl("lnkRelieverName");
                if (relieverid.Text.Equals(""))
                {
                    reliever.Visible = true;
                    relivername.Visible = false;
                }
                relivername.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + relieverid.Text + "'); return false;");
            }
        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ShowReport();
        SetPageNavigator();
    }
    protected void gvCrewReliefPlanning_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCrewReliefPlanning.EditIndex = -1;
        gvCrewReliefPlanning.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
    }
    protected void gvCrewReliefPlanning_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            int flag = 1;

            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string currentVesselId = ((Label)gridView.Rows[rowIndex].FindControl("lblVesselId")).Text;
                string previousVesselId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblVesselId")).Text;

                if (currentVesselId != previousVesselId)
                {
                    GridViewRow rownew = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Insert);
                    //create Cell and it will be added to row
                    //int cellcnt = gridView.Columns.Count;
                    TableCell cell = null;
                    for (int i = 0; i <= 1; i++)
                    {
                        cell = new TableCell();
                        rownew.Cells.Add(cell);
                    }
                    rownew.Cells[0].Text = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblVesselName")).Text;
                    rownew.Cells[0].Attributes.Add("style", "text-align:left; font-weight:bold; border-right-color:#ffffff;");
                    rownew.Cells[0].ColumnSpan = 7;
                    //add the row to Gridview and AddAT(Index of where it will be appear
                    gridView.Controls[0].Controls.AddAt(row.RowIndex + 2, rownew);

                    flag = rowIndex;
                }
            }
            if (flag == 0)
            {
                GridViewRow rownew = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = null;
                for (int i = 0; i <= 1; i++)
                {
                    cell = new TableCell();
                    rownew.Cells.Add(cell);
                }
                rownew.Cells[0].Text = ((Label)gridView.Rows[0].FindControl("lblVesselName")).Text;
                rownew.Cells[0].Attributes.Add("style", "text-align:left; font-weight:bold; border-right-color:#ffffff;");
                rownew.Cells[0].ColumnSpan = 7;
                gridView.Controls[0].Controls.AddAt(1, rownew);
            }
        }
    }

    protected void gvCrewReliefPlanningF2_PreRender(object sender, EventArgs e)
    {
        try
        {
            GridDecorator2.MergeRows2(gvCrewReliefPlanningF2);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowReportFormat2()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDOFFSIGNERRANK", "FLDOFFSIGNERNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDREMARKS" };
        string[] alCaptions = { "Rank", "Onboard", "Sign On Date", "Relief Date", "Reliever", "DOA", "EOL(Including activities)", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION2"] == null) ? null : (ViewState["SORTEXPRESSION2"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION2"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION2"].ToString());
        
        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat2(
                            (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                            (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                            (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                            General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                            General.GetNullableDateTime(ucFromDate.Text),
                            General.GetNullableDateTime(ucToDate.Text),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER2"].ToString()),
                            General.ShowRecords(null),
                            ref iRowCount,
                            ref iTotalPageCount,
                            General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                            General.GetNullableString(ucZone.SelectedZoneValue) == "," ? null : General.GetNullableString(ucZone.SelectedZoneValue));

        General.SetPrintOptions("gvCrewReliefPlanningF2", "Relief Planning Format 2", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrewReliefPlanningF2.DataSource = ds;
            gvCrewReliefPlanningF2.DataBind();
            ViewState["ROWSINGRIDVIEW2"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound2(dt, gvCrewReliefPlanningF2);
            ViewState["ROWSINGRIDVIEW2"] = 0;
        }
        ViewState["ROWCOUNT2"] = iRowCount;
        ViewState["TOTALPAGECOUNT2"] = iTotalPageCount;
    }
    protected void cmdGo2_Click(object sender, EventArgs e)
    {
        gvCrewReliefPlanningF2.EditIndex = -1;
        gvCrewReliefPlanningF2.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage2.Text, out result))
        {
            ViewState["PAGENUMBER2"] = Int32.Parse(txtnopage2.Text);

            if ((int)ViewState["TOTALPAGECOUNT2"] < Int32.Parse(txtnopage2.Text))
                ViewState["PAGENUMBER2"] = ViewState["TOTALPAGECOUNT2"];

            if (0 >= Int32.Parse(txtnopage2.Text))
                ViewState["PAGENUMBER2"] = 1;

            if ((int)ViewState["PAGENUMBER2"] == 0)
                ViewState["PAGENUMBER2"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER2"].ToString();
        }
        ShowReportFormat2();
        SetPageNavigator2();
    }

    protected void PagerButtonClick2(object sender, CommandEventArgs ce)
    {
        gvCrewReliefPlanningF2.SelectedIndex = -1;
        gvCrewReliefPlanningF2.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER2"] = (int)ViewState["PAGENUMBER2"] - 1;
        else
            ViewState["PAGENUMBER2"] = (int)ViewState["PAGENUMBER2"] + 1;

        ShowReportFormat2();
        SetPageNavigator2();
    }

    private void SetPageNavigator2()
    {
        cmdPrevious2.Enabled = IsPreviousEnabled2();
        cmdNext2.Enabled = IsNextEnabled2();
        lblPagenumber2.Text = "Page " + ViewState["PAGENUMBER2"].ToString();
        lblPages2.Text = " of " + ViewState["TOTALPAGECOUNT2"].ToString() + " Pages. ";
        lblRecords2.Text = "(" + ViewState["ROWCOUNT2"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled2()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER2"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT2"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled2()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER2"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT2"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void ShowNoRecordsFound2(DataTable dt, GridView gv)
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
    protected void gvCrewReliefPlanningF2_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION2"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION2"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION2"] == null || ViewState["SORTDIRECTION2"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                Label empid = (Label)e.Row.FindControl("lblOffsignerID");
                LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

                Label relieverid = (Label)e.Row.FindControl("lblOnsignerID");
                Label reliever = (Label)e.Row.FindControl("lblRelieverName");
                LinkButton relivername = (LinkButton)e.Row.FindControl("lnkRelieverName");
                if (relieverid.Text.Equals(""))
                {
                    reliever.Visible = true;
                    relivername.Visible = false;
                }
                relivername.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + relieverid.Text + "'); return false;");
            }
        }
    }
    protected void cmdSort2_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ShowReportFormat2();
        SetPageNavigator2();
    }
    protected void gvCrewReliefPlanningF2_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCrewReliefPlanningF2.EditIndex = -1;
        gvCrewReliefPlanningF2.SelectedIndex = -1;
        ViewState["SORTEXPRESSION2"] = se.SortExpression;

        if (ViewState["SORTDIRECTION2"] != null && ViewState["SORTDIRECTION2"].ToString() == "0")
            ViewState["SORTDIRECTION2"] = 1;
        else
            ViewState["SORTDIRECTION2"] = 0;

        ShowReportFormat2();
    }
    protected void gvCrewReliefPlanningF2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    public class GridDecorator2
    {
        public static void MergeRows2(GridView gridView)
        {
            //int newrow = gridView.Rows.Count - 2; 

            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string CurrentVesselId = ((Label)gridView.Rows[rowIndex].FindControl("lblVesselId")).Text;
                string PreviousVesselId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblVesselId")).Text;

                string CurrentRankId = ((Label)gridView.Rows[rowIndex].FindControl("lblRankId")).Text;
                string PreviousRankId = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblRankId")).Text;

                string CurrentOffsignerID = ((Label)gridView.Rows[rowIndex].FindControl("lblOffsignerID")).Text;
                string PreviousOffsignerID = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblOffsignerID")).Text;


                if (CurrentVesselId == PreviousVesselId)
                {
                    //Vessel
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;

                    previousRow.Cells[0].Visible = false;
                }

                if (CurrentRankId == PreviousRankId && CurrentOffsignerID == PreviousOffsignerID)
                {
                    //Rank
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;

                    previousRow.Cells[1].Visible = false;

                    //Name
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                          previousRow.Cells[2].RowSpan + 1;

                    previousRow.Cells[2].Visible = false;

                    //Tour
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                          previousRow.Cells[3].RowSpan + 1;

                    previousRow.Cells[3].Visible = false;

                    //DOA
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                          previousRow.Cells[6].RowSpan + 1;

                    previousRow.Cells[6].Visible = false;

                    //Tour
                    row.Cells[7].RowSpan = previousRow.Cells[7].RowSpan < 2 ? 2 :
                                          previousRow.Cells[7].RowSpan + 1;

                    previousRow.Cells[7].Visible = false;
                }
            }
        }
    }
}
