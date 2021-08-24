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
public partial class Crew_CrewTravelReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        
        toolbar.AddFontAwesomeButton("../Crew/CrewTravelReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelReportFilter.aspx" + "'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");


        CrewTravelMenu.MenuList = toolbar.Show();
        CrewTravelMenu.AccessRights = this.ViewState;

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        //toolbar1.AddButton("Show Report", "SHOWREPORT");

        toolbar1.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SUMMARY"] = "1";
            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            SetDetails();

        }
        //  ShowReport();
    }

    public void SetDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewTravelBI.LastScheduled();

            if (dt.Rows.Count > 0)
            {
                txtDate.Text = dt.Rows[0]["FLDLASTRUNDATE"].ToString();
             
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // tab strip method
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
                Response.Redirect("../Crew/CrewTravelReportVisual.aspx");
            }
            else if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                Response.Redirect("../Crew/CrewTravelReport.aspx");
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
                ViewState["PAGENUMBER"] = null;
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
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentCrewTravelFilter != null)
        {
            NameValueCollection filter = Filter.CurrentCrewTravelFilter;

            ds = PhoenixCrewTravelBI.CrewTravel(
                    General.GetNullableString(filter.Get("ddlYear")),
                    General.GetNullableString(filter.Get("ddlQuarter")),
                    General.GetNullableString(filter.Get("ddlMonth")),
                    General.GetNullableString(filter.Get("txtFileNumber")),
                    General.GetNullableString(filter.Get("txtPassportno")),
                    General.GetNullableString(filter.Get("txtTicketno")),
                    General.GetNullableString(filter.Get("txtRequisition")),
                    General.GetNullableString(filter.Get("txtAgent")),
                    General.GetNullableString(filter.Get("ucZone")),
                    General.GetNullableString(filter.Get("ucRank")),
                    General.GetNullableString(filter.Get("ddlDesignation")),
                    General.GetNullableString(filter.Get("ucVessel")),
                    null,
                    General.GetNullableString(filter.Get("ddlOrigin")),
                    General.GetNullableString(filter.Get("ddlDestination")),
                    General.GetNullableString(filter.Get("ddlOfficeCrew")),
                    General.GetNullableString(filter.Get("ddlTravelreason")),
                    1.ToString(),
                    iRowCount.ToString(),
                    ref iRowCount,
                    ref iTotalPageCount
                );
        }
        else
        {
            ds = PhoenixCrewTravelBI.CrewTravel(
                        DateTime.Now.Year.ToString(),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
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

        Response.AddHeader("Content-Disposition", "attachment; filename=" + pageTitle.Text + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + pageTitle.Text + "</center></h5></td></tr>");
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
        DataSet ds = new DataSet();
        ViewState["SHOWREPORT"] = 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = getCaptions();
        string[] alColumns = getColumns();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        // read from session filter and bind the data
        if (Filter.CurrentCrewTravelFilter != null)
        {
            NameValueCollection filter = Filter.CurrentCrewTravelFilter;

            ds = PhoenixCrewTravelBI.CrewTravel(
                    General.GetNullableString(filter.Get("ddlYear")),
                    General.GetNullableString(filter.Get("ddlQuarter")),
                    General.GetNullableString(filter.Get("ddlMonth")),
                    General.GetNullableString(filter.Get("txtFileNumber")),
                    General.GetNullableString(filter.Get("txtPassportno")),
                    General.GetNullableString(filter.Get("txtTicketno")),
                    General.GetNullableString(filter.Get("txtRequisition")),
                    General.GetNullableString(filter.Get("txtAgent")),
                    General.GetNullableString(filter.Get("ucZone")),
                    General.GetNullableString(filter.Get("ucRank")),
                    General.GetNullableString(filter.Get("ddlDesignation")),
                    General.GetNullableString(filter.Get("ucVessel")),
                    null,
                    General.GetNullableString(filter.Get("ddlOrigin")),
                    General.GetNullableString(filter.Get("ddlDestination")),
                    General.GetNullableString(filter.Get("ddlOfficeCrew")),
                    General.GetNullableString(filter.Get("ddlTravelreason")),
                    ViewState["PAGENUMBER"].ToString(),
                    gvCrew.PageSize.ToString(),
                    ref iRowCount,
                    ref iTotalPageCount
                );
        }
        else
        {
            ds = PhoenixCrewTravelBI.CrewTravel(
                        DateTime.Now.Year.ToString(),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
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

        General.SetPrintOptions("gvCrew", "Crew Travel", alCaptions, alColumns, ds);

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
        string[] alColumns = { "FLDEMPLOYEECODE", "FLDPASSENGERNAME", "FLDRANKCODE", "FLDZONECODE", "FLDPASSPORTNO", "FLDREQUISITIONNO", "FLDTICKETNO", "FLDORIGIN", "FLDDESTINATION", "FLDREQUESTEDDATE", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDTRAVELREASON", "FLDVESSELNAME", "FLDPAYMENTMODENAME", "FLDCLASS", "FLDAGENTNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDTAX", "FLDTOTALAMOUNT", "FLDCOMMITTEDYN", "FLDTICKETCANCELLEDYN", "FLDCOMMITTEDDATE" };
        return alColumns;
    }

    private string[] getCaptions()
    {
        string[] alCaptions = { "File No.", "Name", "Rank", "Zone", "Passport No.", "Requisition No.", "Ticket No.", "Origin", "Destination", "Requested", "Departure", "Arrival", "Travel Reason", "Vessel", "Payment Mode", "Class", "Agent", "Currency", "Amount", "Tax", "Total", "Confirmed", "Cancelled", "Committed Date" };
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

    // printing 
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