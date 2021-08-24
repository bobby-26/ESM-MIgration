using System;
using System.Data;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;

public partial class CrewReportBpmsGpd : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportBpmsGpd.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportBpmsGpd.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            ucPool.SelectedPool = "1";
            ucPool.Enabled = false;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
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
                ucDate.Text = "";
                ucRank.selectedlist = "";
                Rebind();
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
                if (!IsValidFilter(ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
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
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDPASSPORTNO", "FLDRANKPOSTEDNAME", "FLDLASTNAME", "FLDFIRSTNAME", "FLDGENDER", "FLDDATEOFBIRTH", "FLDCURRENTBASESALARY", "FLDCURRENCY", "FLDHIREDATE", "FLDRANKWITHBP", "FLDADDRESS1", "FLDADDRESS2", "FLDCITY", "FLDSTATE", "FLDPINCODE", "FLDCOUNTRY", "FLDEMAIL", "FLDHOMENUMBER", "FLDMOBILENO", "FLDSTATUS", "FLDNATIONALITYCODE", "FLDLMCODES", "FLDBONUS", "FLDVPPBONUS", "FLDSPOTBONUS", "FLDSTUDYBONUS", "FLDSUPERIORITYBONUS", "FLDPERFORMANCERATING" };
        string[] alCaptions = { "Emp ID", "National ID(Passport No)", "Rank", "Surname", "First Name", "Gender", "Birthday", "Correct Base Salary", "Currency", "Hire Date", "Date into Current Rank with BP", "Address 1", "Address 2", "City", "State", "Pin Code", "Country", "Email", "Home Number", "Mobile Number (Affix +91)", "Status", "Nationality Code", "LM Codes", "% Bonus", "VPP Bonus", "Spot Bonus", "Study Bonus", "Superiority Bonus", "Performance Rating" };
        string[] filtercolumns = { "FLDEMPLOYEECODE", "FLDPASSPORTNO", "FLDRANKPOSTEDNAME", "FLDLASTNAME", "FLDFIRSTNAME", "FLDGENDER", "FLDDATEOFBIRTH", "FLDCURRENTBASESALARY", "FLDCURRENCY", "FLDHIREDATE", "FLDRANKWITHBP", "FLDADDRESS1", "FLDADDRESS2", "FLDCITY", "FLDSTATE", "FLDPINCODE", "FLDCOUNTRY", "FLDEMAIL", "FLDHOMENUMBER", "FLDMOBILENO", "FLDSTATUS", "FLDNATIONALITYCODE", "FLDLMCODES", "FLDBONUS", "FLDVPPBONUS", "FLDSPOTBONUS", "FLDSTUDYBONUS", "FLDSUPERIORITYBONUS", "FLDPERFORMANCERATING" };
        string[] filtercaptions = { "Emp ID", "National ID(Passport No)", "Rank", "Surname", "First Name", "Gender", "Birthday", "Correct Base Salary", "Currency", "Hire Date", "Date into Current Rank with BP", "Address 1", "Address 2", "City", "State", "Pin Code", "Country", "Email", "Home Number", "Mobile Number (Affix +91)", "Status", "Nationality Code", "LM Codes", "% Bonus", "VPP Bonus", "Spot Bonus", "Study Bonus", "Superiority Bonus", "Performance Rating" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportBpmsGpd.BpmsGpdSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                    , General.GetNullableDateTime(ucDate.Text)
                                                    , ",220,221,"
                                                    , General.GetNullableString("1")
                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , General.ShowRecords(null)
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , sortexpression, sortdirection);

        Response.AddHeader("Content-Disposition", "attachment; filename=BpmsGpd.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Staff Recruited Upto:"+date+"</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
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
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDPASSPORTNO", "FLDRANKPOSTEDNAME", "FLDLASTNAME", "FLDFIRSTNAME", "FLDGENDER", "FLDDATEOFBIRTH", "FLDCURRENTBASESALARY", "FLDCURRENCY", "FLDHIREDATE", "FLDRANKWITHBP", "FLDADDRESS1", "FLDADDRESS2", "FLDCITY", "FLDSTATE", "FLDPINCODE", "FLDCOUNTRY", "FLDEMAIL", "FLDHOMENUMBER", "FLDMOBILENO", "FLDSTATUS", "FLDNATIONALITYCODE", "FLDLMCODES", "FLDBONUS", "FLDVPPBONUS", "FLDSPOTBONUS", "FLDSTUDYBONUS", "FLDSUPERIORITYBONUS", "FLDPERFORMANCERATING" };
        string[] alCaptions = { "Emp ID", "National ID(Passport No)", "Rank", "Surname", "First Name", "Gender", "Birthday", "Correct Base Salary", "Currency", "Hire Date", "Date into Current Rank with BP", "Address 1", "Address 2", "City", "State", "Pin Code", "Country", "Email", "Home Number", "Mobile Number (Affix +91)", "Status", "Nationality Code", "LM Codes", "% Bonus", "VPP Bonus", "Spot Bonus", "Study Bonus", "Superiority Bonus", "Performance Rating" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportBpmsGpd.BpmsGpdSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                    , General.GetNullableDateTime(ucDate.Text)
                                                    , ",220,221,"
                                                    , General.GetNullableString("1")
                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                    , gvCrew.PageSize
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , sortexpression, sortdirection);

        General.SetPrintOptions("gvCrew", "BPMS GPD", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    public bool IsValidFilter(string asondate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(asondate))
        {
            ucError.ErrorMessage = "Date is required";
        }
        else if (DateTime.TryParse(asondate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date should be earlier than current date";
        }

        return (!ucError.IsError);

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
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
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
}
