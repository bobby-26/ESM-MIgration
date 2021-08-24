using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Configuration;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportSeafarersOnBoardOnLeave : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportSeafarersOnBoardOnLeave.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportSeafarersOnBoardOnLeave.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
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
                ucDate.Text = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPool = "";
                ucPrinicipal.SelectedAddress = "";
                ucManager.SelectedValue = "";
                ddlrefferredby.SelectedQuick = "";
                rbtnOnBoardOnLeave.SelectedValue = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucVessel.SelectedVessel = "";
                ucRank.SelectedRankValue = "";
                ucNationality.SelectedNationalityValue = "";
                chkIncludeInactive.Checked = false;
                ucFleet.SelectedList = "";
                ucManager.SelectedList = "";
                ShowReport();
                

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
                if (!IsValidFilter(ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    ShowReport();
                    
                }               
                    
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidFilter(string testfromdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        

        if (General.GetNullableDateTime(testfromdate) == null)
        {
            ucError.ErrorMessage = "As On Date is required.";
        }
        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKNAME", "FLDBATCH", "FLDCOUNTRYNAME", "FLDCIVILSTATUS", "FLDSTATUS", "FLDPRESENTVESSEL", "FLDPRESENTVESSELSIGNONDATE", "FLDPRESENTVESSELSIGNOFFDATE", "FLDPRESENTVESSELDUEOFF", "FLDPRESENTVESSELTYPE", "FLDPRESENTVESSELDURATION", "FLDLASTVESSEL", "FLDLASTVESSELSIGNONDATE", "FLDLASTVESSELSIGNOFFDATE", "FLDLASTVESSELDUEOFF", "FLDLASTVESSELDURATION", "FLDLASTVESSELTYPE", "FLDDATEOFJOINING", "FLDEMAIL" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "Nationality", "Civil Status", "Present Status", "Present Vessel", "From Date", "To Date", "Due Off", "Vessel Type", "Days OnBoard", "Last Vessel", "From Date", "To Date", "Due Off", "Days OnBoard", "Vessel Type", "Joined Date" , "Email"};
        
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixCrewSeafarersOnBoardOnLeaveReport.CrewSeafarersOnBoardOnLeaveReport((ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                                        , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                                        , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                                                        , (ucNationality.SelectedList) == "," ? null : General.GetNullableString(ucNationality.SelectedList)
                                                                                        , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                                        , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                                        , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                                                                                        , (ucManager.SelectedList) == "," ? null : General.GetNullableString(ucManager.SelectedList)                                     
																						, General.GetNullableDateTime(ucDate.Text)
																						, chkIncludeInactive.Checked == true ? 0 : 1
                                                                                        , General.GetNullableString(rbtnOnBoardOnLeave.SelectedValue)
                                                                                        , General.GetNullableInteger(ddlrefferredby.SelectedQuick)
                                                                                        , null
                                                                                        , 1
                                                                                        , 1
                                                                                        , iRowCount
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount
                                                                                        , sortexpression, sortdirection
                                                                                        , (ucFleet.SelectedFleetValue) == "," ? null : General.GetNullableString(ucFleet.SelectedFleetValue));

        Response.AddHeader("Content-Disposition", "attachment; filename=SeafarersOnBoardOnLeave.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");        
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.Write("<br/>");
        Response.Write("<br/>");
        Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        Response.End();
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;
        

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDROW", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKNAME", "FLDBATCH", "FLDCOUNTRYNAME", "FLDCIVILSTATUS", "FLDSTATUS", "FLDPRESENTVESSEL", "FLDPRESENTVESSELSIGNONDATE", "FLDPRESENTVESSELSIGNOFFDATE", "FLDPRESENTVESSELDUEOFF", "FLDPRESENTVESSELTYPE", "FLDPRESENTVESSELDURATION", "FLDLASTVESSEL", "FLDLASTVESSELSIGNONDATE", "FLDLASTVESSELSIGNOFFDATE", "FLDLASTVESSELDUEOFF", "FLDLASTVESSELDURATION", "FLDLASTVESSELTYPE", "FLDDATEOFJOINING", "FLDEMAIL" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "Nationality", "Civil Status", "Present Status", "Present Vessel", "From Date", "To Date", "Due Off", "Vessel Type", "Days OnBoard", "Last Vessel", "From Date", "To Date", "Due Off", "Days OnBoard", "Vessel Type", "Joined Date", "Email" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewSeafarersOnBoardOnLeaveReport.CrewSeafarersOnBoardOnLeaveReport((ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                                        , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                                        , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                                                        , (ucNationality.SelectedList) == "," ? null : General.GetNullableString(ucNationality.SelectedList)
                                                                                        , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                                        , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                                        , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                                                                                        , (ucManager.SelectedList) == "," ? null : General.GetNullableString(ucManager.SelectedList)
                                                                                        , General.GetNullableDateTime(ucDate.Text)
                                                                                        , chkIncludeInactive.Checked ==true ? 0:1
                                                                                        , General.GetNullableString(rbtnOnBoardOnLeave.SelectedValue)
                                                                                        , General.GetNullableInteger(ddlrefferredby.SelectedQuick)
                                                                                        , null
                                                                                        , 1
                                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                        , gvCrew.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount
                                                                                        , sortexpression, sortdirection
                                                                                        , (ucFleet.SelectedFleetValue) == "," ? null : General.GetNullableString(ucFleet.SelectedFleetValue));

        General.SetPrintOptions("gvCrew", "Seafarers OnBoard OnLeave", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpId");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

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
