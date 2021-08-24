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

public partial class Crew_CrewReportDataExportPDMSNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar3 = new PhoenixToolbar();
            toolbar3.AddButton("Format 1", "FORMAT1",ToolBarDirection.Left);
            toolbar3.AddButton("Format 2", "FORMAT2", ToolBarDirection.Left);
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar3.Show();

            CrewMenu.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Show Report", "REPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbarsub.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDataExportPDMSNew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDataExportPDMSNew.aspx", "Export to Text", "<i class=\"fas fa-list-alt-picklist\"></i>", "TEXT");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucPool.SelectedPool = "1";
                //if (Session["DATAEXPORTADD"] != null)
                //    ucPrinicipal.SelectedAddress = Session["DATAEXPORTADD"].ToString();
                
            }
            gvCrew.PageSize = 10000;
            //  ShowReport();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORMAT1"))
            {
                Response.Redirect("../Crew/CrewReportDataExportPDMSPersonal.aspx", true);
            }

            if (CommandName.ToUpper().Equals("FORMAT2"))
            {
                Response.Redirect("../Crew/CrewReportDataExportPDMSNew.aspx", true);
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
            ViewState["PAGENUMBER"] = 1;
            
            ShowReport();
        }
    }
    private void ShowReport()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEECODE", "FLDLASTNAME", "FLDFIRSTNAME", "FLDDATEOFBIRTH", "FLDMIDDLENAME", "FLDTITLE", "FLDDATEOFJOINING", "FLDSHORTNAME", "FLDNATIONCODE", "FLDNATIONCODE2", "FLDPROMOTIONDATE", "FLDRANKEXPERIANCE", "FLDTOTALRANKEXPERIENCE", "FLDRANKNAME", "FLDINACTIVE", "FLDINACTIVEDATE", "FLDSOURCE", "FLDLEAVEENDDATE", "FLDRELIEFDUEDATE", "FLDINACTIVEREASON" };
        string[] alCaptions = { "Global Emp", "Last Name", "First Name", "Birth Date", "Middle Name", "PreTitleCode", "Hire Date", "Gender COde", "NationCode", "NationCode2", "PromotedDate", "RankExp", "TotalExperience", "Rank", "Inactive", "InactiveDate", "Source", "LeaveEndDate", "DueOffDate", "InactiveReason" };


        ds = PhoenixCrewReportsDataExport.DataExportPersonalNew(ucPool.SelectedPool);
        //General.SetPrintOptions("gvCrew", "Data Export", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDLASTNAME", "FLDFIRSTNAME", "FLDDATEOFBIRTH", "FLDMIDDLENAME", "FLDTITLE", "FLDDATEOFJOINING", "FLDSHORTNAME", "FLDNATIONCODE", "FLDNATIONCODE2", "FLDPROMOTIONDATE", "FLDRANKEXPERIANCE", "FLDTOTALRANKEXPERIENCE", "FLDRANKNAME", "FLDINACTIVE", "FLDINACTIVEDATE", "FLDSOURCE", "FLDLEAVEENDDATE", "FLDRELIEFDUEDATE", "FLDINACTIVEREASON" };
        string[] alCaptions = { "Global Emp", "Last Name", "First Name", "Birth Date", "Middle Name", "PreTitleCode", "Hire Date", "Gender COde", "NationCode", "NationCode2", "PromotedDate", "RankExp", "TotalExperience", "Rank", "Inactive", "InactiveDate", "Source", "LeaveEndDate", "DueOffDate", "InactiveReason" };

        ds = PhoenixCrewReportsDataExport.DataExportPersonalNew(ucPool.SelectedPool);

        Response.AddHeader("Content-Disposition", "attachment; filename=DataExportPersonal.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>Data Export Personal</center></h5></td></tr>");
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
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDLASTNAME", "FLDFIRSTNAME", "FLDDATEOFBIRTH", "FLDMIDDLENAME", "FLDTITLE", "FLDDATEOFJOINING", "FLDSHORTNAME", "FLDNATIONCODE", "FLDNATIONCODE2", "FLDPROMOTIONDATE", "FLDRANKEXPERIANCE", "FLDTOTALRANKEXPERIENCE", "FLDRANKNAME", "FLDINACTIVE", "FLDINACTIVEDATE", "FLDSOURCE", "FLDLEAVEENDDATE", "FLDRELIEFDUEDATE", "FLDINACTIVEREASON" };
        string[] alCaptions = { "Global Emp", "Last Name", "First Name", "Birth Date", "Middle Name", "PreTitleCode", "Hire Date", "Gender COde", "NationCode", "NationCode2", "PromotedDate", "RankExp", "TotalExperience", "Rank", "Inactive", "InactiveDate", "Source", "LeaveEndDate", "DueOffDate", "InactiveReason" };

        ds = PhoenixCrewReportsDataExport.DataExportPersonalNew(ucPool.SelectedPool);

        Response.AddHeader("Content-Disposition", "attachment; filename=DataExportPersonal.txt");
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
