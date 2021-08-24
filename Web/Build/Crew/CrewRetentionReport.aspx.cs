using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Crew_CrewRetentionReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewRetentionReportFilter.aspx" + "'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Crew/CrewRetentionReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        
        CrewRetentionMenu.MenuList = toolbar.Show();
        CrewRetentionMenu.AccessRights = this.ViewState;

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Visual", "SHOWVISUAL",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SUMMARY"] = "1";
            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    // tab strip method
    protected void CrewRetentionMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

            if (CommandName.ToUpper().Equals("SHOWVISUAL"))
            {
                //sessionFilterValues();
                Response.Redirect("../Crew/CrewRetentionReportVisual.aspx");
            }
            else if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                Response.Redirect("../Crew/CrewRetentionReport.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // grid Events
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("PAGE"))
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

    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs se)
    {
     
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();

        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 100;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (Filter.CurrentCrewRetentionFilter != null)
        {
            NameValueCollection filter = Filter.CurrentCrewRetentionFilter;

            ds = PhoenixCrewRetentionBI.CrewRetention(
                    General.GetNullableString(filter.Get("ddlYear")),
                    General.GetNullableString(filter.Get("txtFrom")),
                    General.GetNullableString(filter.Get("txtTo")),
                    General.GetNullableString(filter.Get("ucVesselType")),
                    General.GetNullableString(filter.Get("ucPrincipal")),
                    General.GetNullableString(filter.Get("ucRank")),
                    null,
                    1.ToString(),
                    iRowCount.ToString(),
                    ref iRowCount,
                    ref iTotalPageCount
                );
        }
        else
        {
            ds = PhoenixCrewRetentionBI.CrewRetention(
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                       1.ToString(),
                    iRowCount.ToString(),
                       ref iRowCount,
                       ref iTotalPageCount
                   );
        }
        
        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Crew Retention" + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + "Crew Retention" + "</center></h5></td></tr>");
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
    }
    private void ShowReport()
    {  
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        // read from session filter and bind the data
        if (Filter.CurrentCrewRetentionFilter != null)
        {
            NameValueCollection filter = Filter.CurrentCrewRetentionFilter;

            ds = PhoenixCrewRetentionBI.CrewRetention(
                    General.GetNullableString(filter.Get("ddlYear")),
                    General.GetNullableString(filter.Get("txtFrom")),
                    General.GetNullableString(filter.Get("txtTo")),
                    General.GetNullableString(filter.Get("ucVesselType")),
                    General.GetNullableString(filter.Get("ucPrincipal")),
                    General.GetNullableString(filter.Get("ucRank")),
                    null,
                    ViewState["PAGENUMBER"].ToString(),
                    gvCrew.PageSize.ToString(),
                    ref iRowCount,
                    ref iTotalPageCount
                );
        }
        else 
        {
            ds = PhoenixCrewRetentionBI.CrewRetention(
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        ViewState["PAGENUMBER"].ToString(),
                       gvCrew.PageSize.ToString(),
                       ref iRowCount,
                       ref iTotalPageCount
                   );
        }

        General.SetPrintOptions("gvCrew", "Crew Retention", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        if (iRowCount > 0)
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        else
            ViewState["ROWSINGRIDVIEW"] = 0;
    }
    private string[] getColumns()
    {
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME","FLDRANKCODE", "FLDVESSELNAME","FLDVESSELTYPENAME", "FLDPRINCIPALNAME","FLDSIGNONDATE", "FLDSIGNOFFDATE","FLDCONTRACTEDDAYS" };
        return alColumns;
    }

    private string[] getCaptions()
    {
        string[] alCaptions = { "File No.", "Name", "Rank", "Vessel", "Vessel Type", "Principal", "Sign On", "Sign Off", "Contract Days" };
        return alCaptions;
    }

    // popup method
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {        
        ViewState["PAGENUMBER"] = 1;
        gvCrew.CurrentPageIndex = 0;
        ShowReport();
        gvCrew.Rebind();     
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