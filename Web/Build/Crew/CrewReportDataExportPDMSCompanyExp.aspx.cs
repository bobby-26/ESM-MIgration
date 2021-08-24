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

public partial class Crew_CrewReportDataExportPDMSCompanyExp : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Show Report", "REPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbarsub.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDataExportPDMSCompanyExp.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDataExportPDMSCompanyExp.aspx", "Export to Text", "<i class=\"fas fa-list-alt-picklist\"></i>", "TEXT");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucPool.SelectedPool = "1";
                //if (Session["DATAEXPORTADD"] != null)
                //ucPrinicipal.SelectedAddress = Session["DATAEXPORTADD"].ToString();
                
            }
            gvCrew.PageSize = 10000;
            //ShowReport();

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

            if (CommandName.ToUpper().Equals("TEXT"))
            {
                ShowText();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //ucPrinicipal.SelectedAddress = "";
                Session["DATAEXPORTADD"] = null;

                ViewState["PAGENUMBER"] = 1;
               
                gvCrew.SelectedIndexes.Clear();
                gvCrew.EditIndexes.Clear();
                gvCrew.DataSource = null;
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("REPORT"))
        {
            ShowReport();
        }
    }
    private void ShowReport()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEECODE", "FLDCOURSETITLE", "FLDCOURSECERTCODE", "FLDCOURSEINSTITUTE", "FLDCOURSEISSUEDATE", "FLDCOURSEEXPIRYDATE", "FLDCOURSECOUNTRYCODE", "FLDSOURCE" };
        string[] alCaptions = { "Global Emp", "Title", "CertCode", "Institute", "StartDate", "EndDate", "CountryCode", "Source" };

        ds = PhoenixCrewReportsDataExport.DataExportDetailsSearch(ucPool.SelectedPool, 4, null,
                                                           1,
                                                            gvCrew.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);
        //General.SetPrintOptions("gvCrew", "Data Export", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds.Tables[0];
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDCOMPEXPSTARTDATE", "FLDCOMPEXPTODATE", "FLDCOMPEXPRANK", "FLDCOMPVESSEL", "FLDCOMPSIGNOFFREASON", "FLDCOMPENGTYPE", "FLDCOMPVESSELBHP", "FLDCOMPVESSELTYPE", "FLDCOMPVESELCATEGORY", "FLDSOURCE" };
        string[] alCaptions = { "Global Emp", "From Date", "To Date", "Rank", "Vessel", "SignOffReason", "EngType", "Bhp", "Vessel Type", "Vessel Category", "Source" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportsDataExport.DataExportDetailsSearch(ucPool.SelectedPool, 4, null,
                                                            1,
                                                            gvCrew.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=DataExportCompExp.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>Data Export Company Experience</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    protected void ShowText()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDCOMPEXPSTARTDATE", "FLDCOMPEXPTODATE", "FLDCOMPEXPRANK", "FLDCOMPVESSEL", "FLDCOMPSIGNOFFREASON", "FLDCOMPENGTYPE", "FLDCOMPVESSELBHP", "FLDCOMPVESSELTYPE", "FLDCOMPVESELCATEGORY", "FLDSOURCE" };
        string[] alCaptions = { "GlobalEmpCode", "FromDate", "ToDate", "Rank", "Vessel", "SignoffReason", "Engtype", "Bhp", "vsltype", "vslcategory", "Source" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportsDataExport.DataExportDetailsSearch(ucPool.SelectedPool, 4, null,
                                                            1,
                                                            gvCrew.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=DataExportCompExp.txt");
        Response.ContentType = "application/vnd.text";
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write(alCaptions[i]);
            if (i != alCaptions.Length - 1)
                Response.Write("~");
        }
        Response.Write("\r\n");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write(dr[alColumns[i]]);
                if (i != alColumns.Length - 1)
                    Response.Write("~");

            }
            Response.Write("\r\n");
        }
        Response.End();
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
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
    public bool IsValidFilter(string address)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (address.Equals("") || address.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Prinicipal";
        }

        return (!ucError.IsError);
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
}
