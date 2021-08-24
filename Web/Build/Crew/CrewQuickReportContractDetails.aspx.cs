using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PhoenixCrewQuickReports;
using Telerik.Web.UI;
using System.Web;

public partial class CrewQuickReportContractDetails : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrew.Rows)
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

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Employee wise", "EMP"); toolbar1.AddButton("Vessel wise", "VESSEL");
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbar1.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SHOWREPORT"] = null;
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
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EMP"))
            {
                Response.Redirect("..\\Crew\\CrewQuickReportContractDetails.aspx", false);
            }
            if (dce.CommandName.ToUpper().Equals("VESSEL"))
            {
                Response.Redirect("..\\Crew\\CrewContractHistory.aspx", false);
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["SHOWREPORT"] = null;
                txtFileNo.Text = "";
                txtname.Text = "";
                txtpassportno.Text = "";
                txtSeamanBookNo.Text = "";
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
                ViewState["PAGENUMBER"] = 1;
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDRANKCODE", "FLDBATCH", "FLDZONENAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDLASTNOTESDESCRIPTION", "FLDDOA", "FLDLASTCONTACTDATE", "FLDSTATUS", "FLDSEAFARERREQUIREMENT" };
        string[] alCaptions = { "SNo.", "File No.", "	Name", "Passport No.", "Seaman Book No.", "Rank", "Batch", "Zone", "Last Vessel", "Sign-Off Date", "Present Vessel", "Sign-On Date", "Remarks", "Availability", "Last Contact", "Status", "Requirement" };
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixCrewQuickReports.CrewQuickEmployeeSearch(General.GetNullableString(txtname.Text.Trim())
            , General.GetNullableString(txtFileNo.Text.Trim())
            , General.GetNullableString(txtpassportno.Text.Trim())
            , General.GetNullableString(txtSeamanBookNo.Text.Trim())
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=EmpolyeeSearchQuickReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Empolyee Search</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDRANKCODE", "FLDBATCH", "FLDZONENAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDLASTNOTESDESCRIPTION", "FLDDOA", "FLDLASTCONTACTDATE", "FLDSTATUS", "FLDSEAFARERREQUIREMENT" };
        string[] alCaptions = { "SNo.", "File No.", "	Name", "Passport No.", "Seaman Book No.", "Rank", "Batch", "Zone", "Last Vessel", "Sign-Off Date", "Present Vessel", "Sign-On Date", "Remarks", "Availability", "Last Contact", "Status", "Requirement" };

        DataSet ds = PhoenixCrewQuickReports.CrewQuickEmployeeSearch(General.GetNullableString(txtname.Text.Trim())
             , General.GetNullableString(txtFileNo.Text.Trim())
             , General.GetNullableString(txtpassportno.Text.Trim())
             , General.GetNullableString(txtSeamanBookNo.Text.Trim())
             , Int32.Parse(ViewState["PAGENUMBER"].ToString())
             , General.ShowRecords(null)
             , ref iRowCount
             , ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Empolyee Search", alCaptions, alColumns, ds);

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

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                Label empid = (Label)e.Row.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                ImageButton ph = (ImageButton)e.Row.FindControl("cmdPortageBillHistory");
                if (ph != null)
                {
                    ph.Visible = SessionUtil.CanAccess(this.ViewState, ph.CommandName);
                    ph.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Crew/CrewEmployeePortageBillList.aspx?empid=" + empid.Text + "');return false;");
                }
                ImageButton ch = (ImageButton)e.Row.FindControl("cmdContractHistory");
                if (ch != null)
                {
                    ch.Visible = SessionUtil.CanAccess(this.ViewState, ch.CommandName);
                    ch.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Crew/CrewContractHistory.aspx?empid=" + empid.Text + "');return false;");
                }
                Image lbtn = (Image)e.Row.FindControl("imgToolTip");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTip");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
        }
    }
}
