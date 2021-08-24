using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportReliefPlanning : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportReliefPlanning.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewReliefPlanning')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportReliefPlanning.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewReliefPlanning.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucVessel.SelectedVessel = "";
                ucRank.SelectedRankValue = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
                ucPool.SelectedPool = "";
                ucZone.SelectedZoneValue = "";
                ShowReport();
                gvCrewReliefPlanning.Rebind();

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
                if (!IsValidDates(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    ShowReport();
                    gvCrewReliefPlanning.Rebind();

                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidDates(string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following information";

        if (General.GetNullableDateTime(fromdate) == null)
            ucError.ErrorMessage = "From date required";
        if (General.GetNullableDateTime(todate) == null)
            ucError.ErrorMessage = "To date required";
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) > 0)
            ucError.ErrorMessage = "To date should be later than or equal to From date";

        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFILENO", "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDBATCHNAME", "FLDVESSELNAME", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDRELIEVERBATCHNAME", "FLDPRPLDECIMALEXPERIENCE", "FLDRANKDECIMALEXPERIENCE", "FLDCOURSECODE", "FLDRELIEVERREMARKS" };
        string[] alCaptions = { "File No", "Name", "Rank", "Batch", "Vessel", "Relief Due", "Reliever", "Reliever Batch", "Experience with Principal(M)", "Rank Experience(M)", "Courses Not done", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReliefPlanningReport.ReliefPlanningReport(
                    (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                    (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                    (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                    General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                    General.GetNullableDateTime(ucFromDate.Text),
                    General.GetNullableDateTime(ucToDate.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                    General.GetNullableString(ucZone.selectedlist) == "," ? null : General.GetNullableString(ucZone.selectedlist));


        Response.AddHeader("Content-Disposition", "attachment; filename=ReliefPlanning.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Relief Planning</center></h5></td></tr>");

        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' align='right'>Date:</td><td style='font-family:Arial; font-size:10px;'>" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
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
                Response.Write("<td style='font-family:Arial; font-size:10px;'>");
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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDBATCHNAME", "FLDVESSELNAME", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDRELIEVERBATCHNAME", "FLDPRPLDECIMALEXPERIENCE", "FLDRANKDECIMALEXPERIENCE", "FLDCOURSECODE", "FLDRELIEVERREMARKS" };
        string[] alCaptions = { "Emp.No", "Name", "Rank", "Batch Name", "Vessel", "Relief Due", "Reliever", "Reliever Batch Name", "Experience with Principal", "Rank Experience", "Courses Not done", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReliefPlanningReport.ReliefPlanningReport(
                            (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                            (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                            (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                            General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                            General.GetNullableDateTime(ucFromDate.Text),
                            General.GetNullableDateTime(ucToDate.Text),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            gvCrewReliefPlanning.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount,
                            General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                            General.GetNullableString(ucZone.selectedlist) == "," ? null : General.GetNullableString(ucZone.selectedlist));

        General.SetPrintOptions("gvCrewReliefPlanning", "Relief Planning", alCaptions, alColumns, ds);

        gvCrewReliefPlanning.DataSource = ds;
        gvCrewReliefPlanning.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrewReliefPlanning_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel empid = (RadLabel)e.Item.FindControl("lblOffsignerID");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

            RadLabel relieverid = (RadLabel)e.Item.FindControl("lblOnsignerID");
            RadLabel reliever = (RadLabel)e.Item.FindControl("lblRelieverName");
            LinkButton relivername = (LinkButton)e.Item.FindControl("lnkRelieverName");
            if (relieverid.Text.Equals(""))
            {
                reliever.Visible = true;
                relivername.Visible = false;
            }
            relivername.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + relieverid.Text + "'); return false;");

        }
    }
    protected void gvCrewReliefPlanning_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrewReliefPlanning_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvCrewReliefPlanning_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewReliefPlanning.CurrentPageIndex + 1;

        ShowReport();
    }
}
