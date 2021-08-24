using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewReportNotRelievedOnTime : PhoenixBasePage
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
            MenuReport.SelectedMenuIndex = 2;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportNotRelievedOnTime.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportNotRelievedOnTime.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
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
            ucDate1.Text = DateTime.Now.ToShortDateString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DELAYEDRELIEFONLEAVE"))
            {
                Response.Redirect("../Crew/CrewReportsNotRelievedOnTimeOnLeave.aspx");
                MenuReport.SelectedMenuIndex = 1;
            }
            else if (CommandName.ToUpper().Equals("DELAYEDRELIEFONBOARD"))
            {
                Response.Redirect("../Crew/CrewReportsNotRelievedOnTimeOnBoard.aspx");
                MenuReport.SelectedMenuIndex = 0;
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
                ucZone.selectedlist = "";
                ucRank.SelectedRankValue = "";
                ucDate.Text = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPool = "";
                ucPrincipal.SelectedValue = "";
                ucPrincipal.SelectedList = "";
                txtSkipDays.Text = "";
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
                if (!IsValidFilter(ucDate.Text, ucDate1.Text))
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
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDDATETOBERELIEVED", "FLDFINALLYRELIEVED", "FLDDURATION", "FLDSIGNOFFREMARKS", "FLDRELIEVERNAME", "FLDRCREMARKS", "FLDDELAYEARLYREASON", "FLDZONE" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "Vessel", "Sign On Date", "Relief Due Date", "Relieved Date", "Duration", "Sign off Remarks", "Relieved by/Planned relief", "Remarks", "Reason(Delayed/Early Relief)", "Zone" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTime(
                       General.GetNullableDateTime(ucDate.Text),
                       General.GetNullableDateTime(ucDate1.Text),
                       ucRank.selectedlist.Replace("Dummy", "").TrimStart(','),
                        ucZone.selectedlist.Replace("Dummy", "").TrimStart(','),
                         (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                        (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList),
                       (txtSkipDays.Text) == "" ? 0 : General.GetNullableInteger(txtSkipDays.Text),
                       sortexpression, sortdirection,
                       1,
                       iRowCount,
                       ref iRowCount,
                       ref iTotalPageCount,
                       ucPool.SelectedPool.Replace("Dummy", "").TrimStart(','));

        General.ShowExcel("Relief Delayed", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDDATETOBERELIEVED", "FLDFINALLYRELIEVED", "FLDDURATION", "FLDSIGNOFFREMARKS", "FLDRELIEVERNAME", "FLDRCREMARKS", "FLDDELAYEARLYREASON", "FLDZONE" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "Vessel", "Sign On Date", "Relief Due Date", "Relieved Date", "Duration", "Sign off Remarks", "Relieved by/Planned relief", "Remarks", "Reason(Delayed/Early Relief)", "Zone" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixCrewNotRelievedOnTime.CrewNotRelievedOnTime(
                   General.GetNullableDateTime(ucDate.Text),
                   General.GetNullableDateTime(ucDate1.Text),
                   ucRank.selectedlist.Replace("Dummy", "").TrimStart(','),
                   ucZone.selectedlist.Replace("Dummy", "").TrimStart(','),
                    (ucVesselType.SelectedVesseltype) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                    (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList),
                   (txtSkipDays.Text) == "" ? 0 : General.GetNullableInteger(txtSkipDays.Text),
                   sortexpression, sortdirection,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvCrew.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount,
                   ucPool.SelectedPool.Replace("Dummy", "").TrimStart(','));

        General.SetPrintOptions("gvCrew", "Crew Not Relieved On Time", alCaptions, alColumns, ds);

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


            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

            RadLabel relieverid = (RadLabel)e.Item.FindControl("lblOnsignerID");
            RadLabel reliever = (RadLabel)e.Item.FindControl("lblRelieverName");
            LinkButton relivername = (LinkButton)e.Item.FindControl("lnkRelieverName");
            if (reliever.Text.Equals("NOT PLANNED"))
            {
                reliever.Visible = true;
                relivername.Visible = false;
            }
            relivername.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + relieverid.Text + "'); return false;");

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblSignOffRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucSignOffRemarks");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
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
