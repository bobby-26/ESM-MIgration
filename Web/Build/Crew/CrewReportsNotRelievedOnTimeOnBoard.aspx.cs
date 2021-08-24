using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportsNotRelievedOnTimeOnBoard : PhoenixBasePage
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
            MenuReport.SelectedMenuIndex = 0;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsNotRelievedOnTimeOnBoard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsNotRelievedOnTimeOnBoard.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SHOWREPORT"] = null;
                ucAsOnDate.Text = DateTime.Now.ToShortDateString();
                Filter.CurrentReliefDelayedReportOnboard = null;
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
            else if (CommandName.ToUpper().Equals("DELAYEDRELIEFONLEAVE"))
            {
                Response.Redirect("../Crew/CrewReportsNotRelievedOnTimeOnLeave.aspx");
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
                ucAsOnDate.Text = DateTime.Now.ToShortDateString();
                Filter.CurrentReliefDelayedReportOnboard = null;
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPool = "";
                ucZone.SelectedZoneValue = "";
                ucVessel.SelectedVessel = "";

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
                if (!IsValidFilter())
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
                    criteria.Add("ucAsOnDate", ucAsOnDate.Text);

                    Filter.CurrentReliefDelayedReportOnboard = criteria;

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
        string[] alCaptions = { "Rank", "D(0 TO +7)", "D(+8 TO +15)", "D(+16 TO +30)", "D(+31 TO +37)", "D(+38 TO +45)", "D(> +45)", "Total" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnboard;

        ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnBoardSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["ucAsOnDate"] : ucAsOnDate.Text)
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                                                                , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype));

        General.ShowExcel("Relief Delayed(Onboard)", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKNAME", "FLDD1", "FLDD2", "FLDD3", "FLDD4", "FLDD5", "FLDD6", "FLDTOTAL" };
        string[] alCaptions = { "Rank", "D(0 TO +7)", "D(+8 TO +15)", "D(+16 TO +30)", "D(+31 TO +37)", "D(+38 TO +45)", "D(> +45)", "Total" };

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentReliefDelayedReportOnboard;

        ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTimeOnBoardSearch(General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucManager"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucRank"] : null)
                                                                , General.GetNullableString(nvc != null ? nvc["ucBatch"] : null)
                                                                , General.GetNullableDateTime(nvc != null ? nvc["ucAsOnDate"] : ucAsOnDate.Text)
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvCrew.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                                                                , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype));

        General.SetPrintOptions("gvCrew", "Relief delayed(Onboard)", alCaptions, alColumns, ds);

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
                title = "Relief Delayed (ONB) for " + lblRank.Text + " - D(0 TO +7)";
                lnkD1.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&date=" + ucAsOnDate.Text + "&type=1&from=1&title=" + title + "'); return false;");
            }

            LinkButton lnkD2 = (LinkButton)e.Item.FindControl("lnkD2");
            if (lnkD2 != null)
            {
                title = "Relief Delayed (ONB) for " + lblRank.Text + " - D(+8 TO +15)";
                lnkD2.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&date=" + ucAsOnDate.Text + "&type=2&from=1&title=" + title + "'); return false;");
            }

            LinkButton lnkD3 = (LinkButton)e.Item.FindControl("lnkD3");
            if (lnkD3 != null)
            {
                title = "Relief Delayed (ONB) for " + lblRank.Text + " - D(+16 TO +30)";
                lnkD3.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&date=" + ucAsOnDate.Text + "&type=3&from=1&title=" + title + "'); return false;");
            }

            LinkButton lnkD4 = (LinkButton)e.Item.FindControl("lnkD4");
            if (lnkD4 != null)
            {
                title = "Relief Delayed (ONB) for " + lblRank.Text + " - D(+31 TO +37)";
                lnkD4.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&date=" + ucAsOnDate.Text + "&type=4&from=1&title=" + title + "'); return false;");
            }

            LinkButton lnkD5 = (LinkButton)e.Item.FindControl("lnkD5");
            if (lnkD5 != null)
            {
                title = "Relief Delayed (ONB) for " + lblRank.Text + " - D(+38 TO +45)";
                lnkD5.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&date=" + ucAsOnDate.Text + "&type=5&from=1&title=" + title + "'); return false;");
            }

            LinkButton lnkD6 = (LinkButton)e.Item.FindControl("lnkD6");
            if (lnkD6 != null)
            {
                title = "Relief Delayed (ONB) for " + lblRank.Text + " - D(> +45)";
                lnkD6.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&date=" + ucAsOnDate.Text + "&type=6&from=1&title=" + title + "'); return false;");
            }

            LinkButton lnkTotal = (LinkButton)e.Item.FindControl("lnkTotal");
            if (lnkTotal != null)
            {
                title = "Relief Delayed (ONB) for " + lblRank.Text;
                lnkTotal.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewReportsNotRelievedOnTimeDetails.aspx?rank=" + lblRankId.Text + "&date=" + ucAsOnDate.Text + "&type=0&from=1&title=" + title + "'); return false;");
            }
        }
    }

    public bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(ucAsOnDate.Text) == null)
        {
            ucError.ErrorMessage = "Onboard as on date is required";
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
