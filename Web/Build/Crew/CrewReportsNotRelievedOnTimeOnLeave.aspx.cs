using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportsNotRelievedOnTimeOnLeave : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Relief delayed(ONB)", "DELAYEDRELIEFONBOARD", ToolBarDirection.Right);
            toolbar.AddButton("Relief delayed(ONL)", "DELAYEDRELIEFONLEAVE", ToolBarDirection.Right);
            toolbar.AddButton("Crew Delayed Relief", "CREWDELAYEDRELIEF", ToolBarDirection.Right);

            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbar.Show();
            MenuReport.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsNotRelievedOnTimeOnLeave.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsNotRelievedOnTimeOnLeave.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SHOWREPORT"] = null;
                ucToDate.Text = General.GetDateTimeToString(DateTime.Now.ToShortDateString());
                Filter.CurrentReliefDelayedReportOnleave = null;
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
    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CREWDELAYEDRELIEF"))
            {
                Response.Redirect("../Crew/CrewReportNotRelievedOnTime.aspx");
            }
            else if (CommandName.ToUpper().Equals("DELAYEDRELIEFONBOARD"))
            {
                Response.Redirect("../Crew/CrewReportsNotRelievedOnTimeOnBoard.aspx");
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

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["SHOWREPORT"] = null;
                ucPrincipal.SelectedList = "";
                ucPrincipal.SelectedValue = "";
                ucRank.selectedlist = "";
                ucRank.SelectedRankValue = "";
                ucManager.SelectedList = "";
                ucBatch.SelectedList = "";
                ucVesselType.SelectedVesseltype = "";
                ucFromDate.Text = "";
                ucToDate.Text = DateTime.Now.ToShortDateString();
                ucPool.SelectedPool = "";
                ucZone.SelectedZoneValue = "";
                Filter.CurrentReliefDelayedReportOnleave = null;
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
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    NameValueCollection criteria = new NameValueCollection();

                    criteria.Clear();
                    criteria.Add("ucPrincipal", ucPrincipal.SelectedList);
                    criteria.Add("ucManager", ucManager.SelectedList);
                    criteria.Add("ucRank", ucRank.selectedlist);
                    criteria.Add("ucBatch", ucBatch.SelectedList);
                    criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
                    criteria.Add("ucFromDate", ucFromDate.Text);
                    criteria.Add("ucToDate", ucToDate.Text);
                    Filter.CurrentReliefDelayedReportOnleave = criteria;
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
        string[] alColumns = { "FLDRANKNAME", "FLDD1", "FLDD2", "FLDD3", "FLDD4", "FLDD5", "FLDD6", "FLDTOTAL" };
        string[] alCaptions = { "Rank", "D(-30 TO -15)", "D(-14 TO 0)", "D(+1 TO +7)", "D(+8 TO +15)", "D(+16 TO +30)", "D(>+30)", "Total" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnleave;

        ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnLeaveSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucVesselType"] : null)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["ucFromDate"] : null)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["ucToDate"] : null)
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel));

        General.ShowExcel("Relief Delayed(Onleave)", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKNAME", "FLDD1", "FLDD2", "FLDD3", "FLDD4", "FLDD5", "FLDD6", "FLDTOTAL" };
        string[] alCaptions = { "Rank", "D(-30 TO -15)", "D(-14 TO 0)", "D(+1 TO +7)", "D(+8 TO +15)", "D(+16 TO +30)", "D(>+30)", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnleave;

        ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnLeaveSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucVesselType"] : null)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["ucFromDate"] : null)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["ucToDate"] : null)
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvCrew.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel));

        General.SetPrintOptions("gvCrew", "Relief delayed(Onleave)", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblRankId = (RadLabel)e.Item.FindControl("lblRankId");
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            LinkButton lnkD1 = (LinkButton)e.Item.FindControl("lnkD1");
            string title = "";
            if (lnkD1 != null)
            {
                title = "Relief Delayed (ONL) for " + lblRank.Text + " - D(-30 TO -15)";
                lnkD1.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&type=1&from=0&title=" + title + "'); return false;");
            }

            LinkButton lnkD2 = (LinkButton)e.Item.FindControl("lnkD2");
            if (lnkD2 != null)
            {
                title = "Relief Delayed (ONL) for " + lblRank.Text + " - D(-14 TO 0)";
                lnkD2.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&type=2&from=0&title=" + title + "'); return false;");
            }

            LinkButton lnkD3 = (LinkButton)e.Item.FindControl("lnkD3");
            if (lnkD3 != null)
            {
                title = "Relief Delayed (ONL) for " + lblRank.Text + " - D(+1 TO +7)";
                lnkD3.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&type=3&from=0&title=" + title + "'); return false;");
            }

            LinkButton lnkD4 = (LinkButton)e.Item.FindControl("lnkD4");
            if (lnkD4 != null)
            {
                title = "Relief Delayed (ONL) for " + lblRank.Text + " - D(+8 TO +15)";
                lnkD4.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&type=4&from=0&title=" + title + "'); return false;");
            }

            LinkButton lnkD5 = (LinkButton)e.Item.FindControl("lnkD5");
            if (lnkD5 != null)
            {
                title = "Relief Delayed (ONL) for " + lblRank.Text + " - D(+16 TO +30)";
                lnkD5.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&type=5&from=0&title=" + title + "'); return false;");
            }

            LinkButton lnkD6 = (LinkButton)e.Item.FindControl("lnkD6");
            if (lnkD6 != null)
            {
                title = "Relief Delayed (ONL) for " + lblRank.Text + " - D(>+30)";
                lnkD6.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&type=6&from=0&title=" + title + "'); return false;");
            }

            LinkButton lnkTotal = (LinkButton)e.Item.FindControl("lnkTotal");
            if (lnkTotal != null)
            {
                title = "Relief Delayed (ONL) for " + lblRank.Text;
                lnkTotal.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&type=0&from=0&title=" + title + "'); return false;");
            }
        }
    }

    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(fromdate).Equals(null))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (General.GetDateTimeToString(todate).Equals(null))
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

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
}
