using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web;
using Telerik.Web.UI;
public partial class Crew_CrewReportsCrewListForaPeriod : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsCrewListForaPeriod.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsCrewListForaPeriod.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            ucVessel.VesselsOnly = true;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
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
                ucDate.Text = null;
                ucVessel.SelectedVessel = "";
                ucZone.selectedlist = "";
                ucPool.SelectedPool = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucBatchList.SelectedList = "";
                ucRank.SelectedRankValue = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
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
                if (!IsValidFilter(ucVessel.SelectedVessel, ucDate.Text, ucDate1.Text))
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
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPRESENTRANKCODE", "FLDBTOD", "FLDSIGNONDATE", "FLDTRIPLENGTH", "FLDSIGNOFFDATE", "FLDETOD", "FLDDURATION", "FLDSIGNOFFREMARKS", "FLDVESSELNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDDATEOFBIRTH", "FLDDATEOFJOINING", "FLDEMPLOYEESTATUS" };
        string[] alCaptions = { "Sl.No", "File No.", "Batch", "Name", "Sailed Rank", "Present Rank", "Btod", "From", "Trip Length", "To", "Etod", "Duration", "Remarks", "Vessel", "Passport No", "Cdc No", "D.O.B", "Dt.FirstJoin", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixCrewList.CrewListSearch(
                       (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel),
                       General.GetNullableDateTime(ucDate.Text),
                       General.GetNullableDateTime(ucDate1.Text),
                       General.GetNullableString(ucZone.selectedlist == "Dummy" ? null : ucZone.selectedlist),
                       General.GetNullableString(ucPool.SelectedPool == "," ? null : ucPool.SelectedPool),
                       (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                       (ucBatchList.SelectedList) == "," ? null : General.GetNullableString(ucBatchList.SelectedList),
                       (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                       sortexpression, sortdirection,
                       (int)ViewState["PAGENUMBER"],
                       iRowCount,
                       ref iRowCount,
                       ref iTotalPageCount);

        DataTable dt = ds.Tables[0];
        string fromdate = dt.Rows[0]["FLDFROMPERIOD"].ToString();
        string todate = dt.Rows[0]["FLDTOPERIOD"].ToString();
        string selectedvessels = dt.Rows[0]["FLDSELECTEDVESSELS"].ToString();

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewListForPeriod.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew List For Vessel: " + selectedvessels + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "' >From:" + General.GetDateTimeToString(fromdate) + " To:" + General.GetDateTimeToString(todate) + "</td></tr>");
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
        //int counter = 1;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;'>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            //counter += 1;
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDPRESENTRANKCODE", "FLDBTOD", "FLDSIGNONDATE", "FLDTRIPLENGTH", "FLDSIGNOFFDATE", "FLDETOD", "FLDDURATION", "FLDSIGNOFFREMARKS", "FLDVESSELNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDDATEOFBIRTH", "FLDDATEOFJOINING", "FLDEMPLOYEESTATUS" };
        string[] alCaptions = { "Sl.No", "File No.", "Batch", "Name", "Sailed Rank", "Present Rank", "Btod", "From", "Trip Length", "To", "Etod", "Duration", "Remarks", "Vessel", "Passport No", "Cdc No", "D.O.B", "Dt.FirstJoin", "Status" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewList.CrewListSearch(
                       (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel),
                       General.GetNullableDateTime(ucDate.Text),
                       General.GetNullableDateTime(ucDate1.Text),
                       General.GetNullableString(ucZone.selectedlist == "Dummy" ? null : ucZone.selectedlist),
                       General.GetNullableString(ucPool.SelectedPool == "," ? null : ucPool.SelectedPool),
                       (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                       (ucBatchList.SelectedList) == "," ? null : General.GetNullableString(ucBatchList.SelectedList),
                       (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvCrew.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "CrewListForAPeriod", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if ("933" != drv["FLDEMPLOYEESTATUSCODE"].ToString())
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            else
                lbr.Enabled = false;
        }


    }
    public bool IsValidFilter(string vessellist, string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessellist.Equals("") || vessellist.Equals("Dummy") || vessellist.Equals(","))
        {
            ucError.ErrorMessage = "Select Vessel";
        }

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

