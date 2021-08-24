using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportMISPrincipalwiseReturneeRate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISPrincipalwiseReturneeRate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMISPrincipalwiseReturneeRate.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucManager.SelectedAddress = "898";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                Guidlines();
                //ShowReport();
                gvCrew1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Guidlines()
    {
        ucToolTipNW.Text = "<table> <tr><td> &nbsp;1.&nbsp;Last sign on date of seafarers during the given period will be compared to the previous sign on date for this report  <br/>&nbsp;2.&nbsp;Returnee Rate = (All joiners - New joiners)/(All joiners)</td> </tr></tr></table>";

        imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
        imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");
        
        ucToolTipNW.Position = ToolTipPosition.TopCenter;
        ucToolTipNW.TargetControlId = imgnotes.ClientID;        
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
                ucDate.Text = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPoolValue = "";
                ucFleet.SelectedFleetValue = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucPrinicpal.SelectedAddress = "";
                ucRank.SelectedRankValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucVesselType.SelectedVesseltype = "";
                ViewState["PAGENUMBER"] = 1;
                gvCrew1.CurrentPageIndex = 0;
                ShowReport();
                gvCrew1.Rebind();
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
                if (!IsValidFilter(ucDate.Text, ucDate1.Text, ucPrinicpal.SelectedAddress.ToString(),ucManager.SelectedAddress.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    gvCrew1.CurrentPageIndex = 0;
                    ShowReport();
                    gvCrew1.Rebind();
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
        string[] alColumns = { "FLDRANKNAME", "FLDALLJOINERS", "FLDNEW", "FLDREJOINER", "FLDRETURNEERATE" };
        string[] alCaptions = { "Rank", "All Joiners", "New Joiners", "Re Joiners","Returnee Rate" };
        string[] filtercaptions = {"From", "To", "Principal", "Nationality", "Zone", "Pool", "Rank", "Emp.Fleet" };
        string[] filtercolumns = { "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE", "FLDSELECTEDPRICIPAL", "FLDSELECTEDNATIONALITY",
                                     "FLDSELECTEDZONE", "FLDSELECTEDPOOL", "FLDSELECTEDRANK", "FLDSELECTEDFLEET" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixCrewReportMIS.CrewReportMISPrincipalwiseReturneeRate(
                (ucZone.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                (ucPool.SelectedPool.ToString()) == "Dummy" ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
                (ucRank.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                (ucPrinicpal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrinicpal.SelectedAddress.ToString()),
                General.GetNullableDateTime(ucDate.Text),
                General.GetNullableDateTime(ucDate1.Text),
                (ucFleet.SelectedList.ToString().ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucFleet.SelectedList.ToString()),
                General.GetNullableInteger(rblPrincipalManager.SelectedValue),
                General.GetNullableInteger(ucManager.SelectedAddress),
                (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList),
                sortexpression, sortdirection,
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount,
               (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype));

        Response.AddHeader("Content-Disposition", "attachment; filename=MISPrincipalwiseReturneeRate.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Principalwise Returnee Rate</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b><center>" + alCaptions[i] + "</center></b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                Response.Write("<center>"+ dr[alColumns[i]]+ "</center>");
                Response.Write("</td>");
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
        string[] alColumns = { "FLDRANKNAME", "FLDALLJOINERS", "FLDNEW", "FLDREJOINER", "FLDRETURNEERATE" };
        string[] alCaptions = { "Rank", "All Joiners", "New Joiners", "Re Joiners", "Returnee Rate" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMIS.CrewReportMISPrincipalwiseReturneeRate(
                (ucZone.selectedlist.ToString()) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist.ToString()),
                (ucPool.SelectedPool.ToString()) == "Dummy" ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                (ucNationality.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality.SelectedList.ToString()),
                (ucRank.selectedlist.ToString()) == "," ? null : General.GetNullableString(ucRank.selectedlist.ToString()),
                (ucPrinicpal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrinicpal.SelectedAddress.ToString()),
                General.GetNullableDateTime(ucDate.Text),
                General.GetNullableDateTime(ucDate1.Text),
                (ucFleet.SelectedList.ToString().ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucFleet.SelectedList.ToString()),
                General.GetNullableInteger(rblPrincipalManager.SelectedValue),
                General.GetNullableInteger(ucManager.SelectedAddress),
                (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrew1.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
               (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype));

        General.SetPrintOptions("gvCrew1", "Principalwise Returnee Rate", alCaptions, alColumns, ds);

        gvCrew1.DataSource = ds;
        gvCrew1.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (iRowCount > 0)
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        else
            ViewState["ROWSINGRIDVIEW"] = 0;
    }

    protected void gvCrew1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {

            RadLabel value = (RadLabel)e.Item.FindControl("lblNewJoiners");
            RadLabel rankid = (RadLabel)e.Item.FindControl("lblrankid1");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkNewJoiners");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewReportMISPrincipalwiseReturneeRateDetails.aspx?zone=" + General.GetNullableString(ucZone.selectedlist) + "&pool=" + General.GetNullableDateTime(ucPool.SelectedPool) + "&rank=" + rankid.Text + "&nationality=" + General.GetNullableString(ucNationality.SelectedList) + "&principal=" + General.GetNullableString(ucPrinicpal.SelectedAddress) + "&fromdate=" + General.GetNullableDateTime(ucDate.Text) + "&todate=" + General.GetNullableDateTime(ucDate1.Text) + "&fleet=" + General.GetNullableString(ucFleet.SelectedList) + "&value=" + value.Text + "&pmtype=" + rblPrincipalManager.SelectedValue + "&manager=" + ucManager.SelectedAddress + "&batch=" + ucBatch.SelectedList + "&vsltype=" + ucVesselType.SelectedVesseltype + "'); return false;");

            RadLabel value1 = (RadLabel)e.Item.FindControl("lblAllJoiners");
            RadLabel rankid1 = (RadLabel)e.Item.FindControl("lblrankid");
            LinkButton lbr1 = (LinkButton)e.Item.FindControl("lnkAllJoiners");
            lbr1.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewReportMISPrincipalwiseReturneeRateDetails.aspx?zone=" + General.GetNullableString(ucZone.selectedlist) + "&pool=" + General.GetNullableDateTime(ucPool.SelectedPool) + "&rank=" + rankid1.Text + "&nationality=" + General.GetNullableString(ucNationality.SelectedList) + "&principal=" + General.GetNullableString(ucPrinicpal.SelectedAddress) + "&fromdate=" + General.GetNullableDateTime(ucDate.Text) + "&todate=" + General.GetNullableDateTime(ucDate1.Text) + "&fleet=" + General.GetNullableString(ucFleet.SelectedList) + "&value=" + value1.Text + "&pmtype=" + rblPrincipalManager.SelectedValue + "&manager=" + ucManager.SelectedAddress + "&batch=" + ucBatch.SelectedList + "&vsltype=" + ucVesselType.SelectedVesseltype + "'); return false;");


            RadLabel value2 = (RadLabel)e.Item.FindControl("lblReJoiners");
            RadLabel rankid2 = (RadLabel)e.Item.FindControl("lblrankid2");
            LinkButton lbr2 = (LinkButton)e.Item.FindControl("lnkReJoiners");

            lbr2.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewReportMISPrincipalwiseReturneeRateDetails.aspx?zone=" + General.GetNullableString(ucZone.selectedlist) + "&pool=" + General.GetNullableDateTime(ucPool.SelectedPool) + "&rank=" + rankid2.Text + "&nationality=" + General.GetNullableString(ucNationality.SelectedList) + "&principal=" + General.GetNullableString(ucPrinicpal.SelectedAddress) + "&fromdate=" + General.GetNullableDateTime(ucDate.Text) + "&todate=" + General.GetNullableDateTime(ucDate1.Text) + "&fleet=" + General.GetNullableString(ucFleet.SelectedList) + "&value=" + value2.Text + "&pmtype=" + rblPrincipalManager.SelectedValue + "&manager=" + ucManager.SelectedAddress + "&batch=" + ucBatch.SelectedList + "&vsltype=" + ucVesselType.SelectedVesseltype + "'); return false;");
        }
    }
    public bool IsValidFilter(string fromdate, string todate,string principal,string manager)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required.";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date.";
        }
        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required.";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'.";
        }

        if (rblPrincipalManager.SelectedValue == "2")
        {
            if (principal.Equals("") || principal.ToUpper().Equals("DUMMY"))
                ucError.ErrorMessage = "Principal is required.";
        }
        if (rblPrincipalManager.SelectedValue == "1")
        {
            if (manager.Equals("") || manager.ToUpper().Equals("DUMMY"))
                ucError.ErrorMessage = "Manager is Required.";
        }
        
        return (!ucError.IsError);
    }
    protected void PrincipalManagerClick(object sender, EventArgs e)
    {
        if (rblPrincipalManager.SelectedValue == "2")
        {
            ucManager.Visible = false;
            ucPrinicpal.Visible = true;
        }
        else
        {
            ucManager.Visible = true;
            ucPrinicpal.Visible = false;
        }
    }
    protected void gvCrew1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {           
            if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew1.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
