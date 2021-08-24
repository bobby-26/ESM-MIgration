using System;
using System.Data;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web;
public partial class Crew_CrewReportMedicalCost : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMedicalCost.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMedicalCost.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ViewState["TTO"] = PhoenixCommonRegisters.GetHardCode(1, 53, "TTO");
                ucTesttodate.Text = DateTime.Now.ToShortDateString();

                NameValueCollection nvc = Filter.MedicalCostFilter;
                if (nvc != null)
                {

                    ucTestfromdate.Text = nvc.Get("ucTestfromdate");
                    ucTesttodate.Text = nvc.Get("ucTesttodate");
                    if (nvc.Get("ucManager").Equals("Dummy"))
                    {
                        ucManager.SelectedAddress = "";
                    }
                    else
                    {
                        ucManager.SelectedAddress = nvc.Get("ucManager");
                    }

                    if (nvc.Get("ucPrincipal").Equals("Dummy"))
                    {
                        ucPrincipal.SelectedAddress = "";
                    }
                    else
                    {
                        ucPrincipal.SelectedAddress = nvc.Get("ucPrincipal");
                    }
                    if (nvc.Get("ucVesselType").Equals("Dummy"))
                    {
                        ucVesselType.SelectedVesseltype = "";
                    }
                    else
                    {
                        ucVesselType.SelectedVesseltype = nvc.Get("ucVesselType");
                    }
                    if (nvc.Get("ucVesselList").Equals("Dummy"))
                    {
                        ucVesselList.SelectedVessel = "";
                    }
                    else
                    {
                        ucVesselList.SelectedVessel = nvc.Get("ucVesselList");
                    }

                    if (nvc.Get("ucPool").Equals("Dummy"))
                    {
                        ucPool.SelectedPoolValue = "";
                    }
                    else
                    {
                        ucPool.SelectedPoolValue = nvc.Get("ucPool").ToString();
                    }
                    if (nvc.Get("ucRank").Equals("Dummy"))
                    {
                        ucRank.SelectedValue = "";
                    }
                    else
                    {
                        ucRank.SelectedRankValue = nvc.Get("ucRank").ToString();
                    }
                    if (nvc.Get("ucZone").Equals("Dummy"))
                    {
                        ucZone.SelectedZoneValue = "";
                    }
                    else
                    {
                        ucZone.SelectedZoneValue = nvc.Get("ucZone").ToString();
                    }
                }
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
                ViewState["SHOWREPORT"] = null;
                ucTestfromdate.Text = null;
                ucTesttodate.Text = DateTime.Now.ToShortDateString();
                ucZone.SelectedZoneValue = "";
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPoolValue = "";
                ucManager.SelectedAddress = "";
                ucPrincipal.SelectedAddress = "";
                ucRank.SelectedRankValue = "";
                ucVesselList.SelectedVessel = "";


                Filter.MedicalCostFilter = null;

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
                if (!IsValidFilter(ucTestfromdate.Text, ucTesttodate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ucManager", ucManager.SelectedAddress);
                    criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
                    criteria.Add("ucTestfromdate", ucTestfromdate.Text);
                    criteria.Add("ucTesttodate", ucTesttodate.Text);
                    criteria.Add("ucZone", ucZone.SelectedZoneValue);
                    criteria.Add("ucRank", ucRank.SelectedRankValue);
                    criteria.Add("ucVesselType", ucVesselType.SelectedVesselTypeValue);
                    criteria.Add("ucVesselList", ucVesselList.SelectedVessel);
                    criteria.Add("ucPool", ucPool.SelectedPoolValue);
                    Filter.MedicalCostFilter = criteria;
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
        NameValueCollection nvc = Filter.MedicalCostFilter;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDMEDICAL", "FLDCLUB", "FLDDEDUCTIBLES", "FLDCOST" };
        string[] alCaptions = { "Sr.No", "Name Of Vessel", "Medical", "P&I Club", "P&I Deductibles", "Cost" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (ucTestfromdate.Text == null)
        {
            ds = PhoenixCrewMedicalCostReport.CrewMedicalCostReport(
                    (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselList")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucRank")) : null)
                    , (nvc != null ? General.GetNullableInteger(nvc.Get("ucManager")) : null)
                    , (nvc != null ? General.GetNullableString(nvc.Get("ucPrincipal")) : null)
                    , General.GetNullableDateTime(null)
                    , General.GetNullableDateTime(null)
                    , sortexpression, sortdirection
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCrewMedicalCostReport.CrewMedicalCostReport(
                   (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselList")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucRank")) : null)
                   , (nvc != null ? General.GetNullableInteger(nvc.Get("ucManager")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucPrincipal")) : null)
                   , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucTestfromdate")) : null)
                   , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucTesttodate")) : null)
                   , sortexpression, sortdirection
                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                   , iRowCount
                   , ref iRowCount
                   , ref iTotalPageCount);

        }

        string fromdates = ucTestfromdate.Text;
        string todatess = ucTesttodate.Text;
        Response.AddHeader("Content-Disposition", "attachment; filename=Medical_Cost_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Medical Cost Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "  To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td align='left'>");
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
        NameValueCollection nvc = Filter.MedicalCostFilter;
        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDMEDICAL", "FLDCLUB", "FLDDEDUCTIBLES", "FLDCOST" };
        string[] alCaptions = { "Sr.No", "Name Of Vessel", "Medical", "P&I Club", "P&I Deductibles", "Cost" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 25;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();
        if (ucTestfromdate.Text == null)
        {
            ds = PhoenixCrewMedicalCostReport.CrewMedicalCostReport(
                   (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselList")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucRank")) : null)
                   , (nvc != null ? General.GetNullableInteger(nvc.Get("ucManager")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucPrincipal")) : null)
                   , General.GetNullableDateTime(null)
                   , General.GetNullableDateTime(null)
                   , sortexpression, sortdirection
                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                   , gvCrew.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixCrewMedicalCostReport.CrewMedicalCostReport(
                   (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselList")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucRank")) : null)
                   , (nvc != null ? General.GetNullableInteger(nvc.Get("ucManager")) : null)
                   , (nvc != null ? General.GetNullableString(nvc.Get("ucPrincipal")) : null)
                   , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucTestfromdate")) : null)
                   , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucTesttodate")) : null)
                   , sortexpression, sortdirection
                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                   , gvCrew.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount);
        }


        General.SetPrintOptions("gvCrew", "Medical Cost Report", alCaptions, alColumns, ds);

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

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("GETVESSEL"))
            {
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                Response.Redirect("..\\Crew\\CrewReportMedicalCostEmployeeWise.aspx?vesid=" + vesselid + "&p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes", false);

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
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
}
