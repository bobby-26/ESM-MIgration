using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;
public partial class Crew_CrewReportDepartmentInformation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["VESSEL"] = "";
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDepartmentInformation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDepartmentInformation.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucDate2.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            

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
                ucDate1.Text = "";
                ucDate2.Text = DateTime.Now.ToShortDateString();
                ucPrincipal.SelectedAddress = "";
                ucVesselList.SelectedVessel = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucCountryList.selectedlist = "";
                ucRankList.selectedlist = "";

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
                if (!IsValidFilter(ucDate1.Text, ucDate2.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
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
        string[] alColumns = { "FLDVESSELNAME", "FLDCREWCHANGECOUNT", "FLDONOFF", "FLDPORT", "FLDDATE" };
        string[] alCaptions = { "Vessel", "Number Of Crew Changes", "On\\Off", "Port", "Date"};
        string[] filtercolumns = { "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE", "FLDSELECTEDPRICIPAL", "FLDSELECTEDVESSEL", "FLDSELECTEDVESTYPE", "FLDSELECTEDRANK", "FLDSELECTEDCOUNTRY" };
        string[] filtercaptions = { "From Date", "To Date", "Principal", "Vessel", "Vessel Type", "Rank", "Country" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.CrewMonthlyChangeReport(
                        General.GetNullableDateTime(ucDate1.Text),
                        General.GetNullableDateTime(ucDate2.Text),
                        (ucPrincipal.SelectedAddress) == "" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress),
                        (ucVesselList.SelectedVessel) == "" ? null : General.GetNullableString(ucVesselList.SelectedVessel),
                        (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                        (ucCountryList.selectedlist) == "" ? null : General.GetNullableString(ucCountryList.selectedlist),
                        (ucRankList.selectedlist) == "" ? null : General.GetNullableString(ucRankList.selectedlist),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=MonthlyCrewChangeReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Monthly Crew Change Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");

        ViewState["VESSEL"] = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                if (i == 0 || i == 1)
                {
                    if (ViewState["VESSEL"].ToString() != dr[alColumns[0]].ToString())
                    {
                        ViewState["VESSEL"] = dr[alColumns[0]].ToString();
                        Response.Write("<td>");
                        Response.Write(dr[alColumns[i]]);
                        Response.Write("</td>");

                        i = i + 1;

                        Response.Write("<td>");
                        Response.Write(dr[alColumns[i]]);
                        Response.Write("</td>");
                    }
                    else
                    {
                        Response.Write("<td>");
                        Response.Write("");
                        Response.Write("</td>");

                        i = i + 1;

                        Response.Write("<td>");
                        Response.Write("");
                        Response.Write("</td>");

                    }
                }
                else
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]].ToString());
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
        string[] alColumns = { "FLDVESSELNAME", "FLDCREWCHANGECOUNT", "FLDONOFF", "FLDPORT", "FLDDATE" };
        string[] alCaptions = { "Vessel", "Number Of Crew Changes", "On\\Off", "Port", "Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.CrewMonthlyChangeReport(
                General.GetNullableDateTime(ucDate1.Text),
                General.GetNullableDateTime(ucDate2.Text),
                (ucPrincipal.SelectedAddress) == "" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress),
                (ucVesselList.SelectedVessel) == "" ? null : General.GetNullableString(ucVesselList.SelectedVessel),
                (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                (ucCountryList.selectedlist) == "" ? null : General.GetNullableString(ucCountryList.selectedlist),
                (ucRankList.selectedlist) == "" ? null : General.GetNullableString(ucRankList.selectedlist),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Monthly Crew Change Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVesselName");
            RadLabel lblcrewchangecount = (RadLabel)e.Item.FindControl("lblCount");
            LinkButton lnkOnOffSigner = (LinkButton)e.Item.FindControl("lnkOnOff");
            if (lblVessel != null)
                if (ViewState["VESSEL"].ToString() != lblVessel.Text)
                    ViewState["VESSEL"] = lblVessel.Text;
                else
                {
                    lblVessel.Visible = false;
                    lblcrewchangecount.Visible = false;
                }
            if (lnkOnOffSigner != null)
                lnkOnOffSigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewChangeOnOffSignersDetails.aspx?vesselid=" + drv["FLDVESSELID"].ToString() + "&dateofcrewchange=" + drv["FLDDATE"].ToString() + "&portid=" + drv["FLDPORTID"].ToString() + "'); return false;");
        }
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
