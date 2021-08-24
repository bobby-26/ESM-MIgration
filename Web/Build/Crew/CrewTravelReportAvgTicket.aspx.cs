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

public partial class CrewTravelReportAvgTicket : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        toolbar.AddFontAwesomeButton("../Crew/CrewTravelReportAvgTicket.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelReportAvgTicketFilter.aspx" + "',false,600,300); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Crew/CrewTravelReportAvgTicket.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        CrewTravelMenu.MenuList = toolbar.Show();
        CrewTravelMenu.AccessRights = this.ViewState;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SUMMARY"] = "1";
            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

    }
    
    protected void CrewTravelMenu_TabStripCommand(object sender, EventArgs e)
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
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentTravelReportAvgTicketFilter = null;

                ShowReport();
                gvCrew.Rebind();
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentTravelReportAvgTicketFilter != null)
        {
            NameValueCollection filter = Filter.CurrentTravelReportAvgTicketFilter;


            ds = PhoenixCrewTravelBI.CrewTravelAvgTicketPrice(
                    General.GetNullableString(filter.Get("ddlYear")),
                    General.GetNullableString(filter.Get("txtAgent")),
                    General.GetNullableString(filter.Get("ddlOrigin")),
                    General.GetNullableString(filter.Get("ddlDestination")),
                     sortexpression, sortdirection,
                     int.Parse(ViewState["PAGENUMBER"].ToString()),
                     gvCrew.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount
                );
        }
        else
        {
            ds = PhoenixCrewTravelBI.CrewTravelAvgTicketPrice(
                     DateTime.Now.Year.ToString(),
                     null,
                     null,
                     null,
                      sortexpression, sortdirection,
                     int.Parse(ViewState["PAGENUMBER"].ToString()),
                     gvCrew.PageSize,
                     ref iRowCount,
                      ref iTotalPageCount);

        }
        if (ds.Tables.Count > 0)
            General.ShowExcel("Crew Travel", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    private void ShowReport()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (Filter.CurrentTravelReportAvgTicketFilter != null)
        {
            NameValueCollection filter = Filter.CurrentTravelReportAvgTicketFilter;


            ds = PhoenixCrewTravelBI.CrewTravelAvgTicketPrice(
                    General.GetNullableString(filter.Get("ddlYear")),
                    General.GetNullableString(filter.Get("txtAgent")),
                    General.GetNullableString(filter.Get("ddlOrigin")),
                    General.GetNullableString(filter.Get("ddlDestination")),
                     sortexpression, sortdirection,
                     int.Parse(ViewState["PAGENUMBER"].ToString()),
                     gvCrew.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount
                );
        }
        else
        {
            ds = PhoenixCrewTravelBI.CrewTravelAvgTicketPrice(
                     DateTime.Now.Year.ToString(),
                     null,
                     null,
                     null,
                      sortexpression, sortdirection,
                     int.Parse(ViewState["PAGENUMBER"].ToString()),
                     gvCrew.PageSize,
                     ref iRowCount,
                      ref iTotalPageCount);

        }

        General.SetPrintOptions("gvCrew", "Crew Travel", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;
        
    }
    private string[] getColumns()
    {
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDAVGAMOUNT"};
        return alColumns;
    }

    private string[] getCaptions()
    {
        string[] alCaptions = { "Origin.", "Destination", "Avg. Price"};
        return alCaptions;
    }
    
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

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}