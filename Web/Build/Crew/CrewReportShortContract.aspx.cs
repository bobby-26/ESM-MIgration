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

public partial class CrewReportShortContract : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportShortContract.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportShortContract.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

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
                ucDate.Text = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucZone.SelectedZoneValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucBatch.SelectedBatch = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucVessel.SelectedVessel = "";
                ucRank.SelectedRankValue = "";
                lstPool.SelectedPool = "";
                ShowReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDZONE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSOURCEDFROM", "FLDENGAGED", "FLDPROMO", "FLDCONVERTEE", "FLDFULLCONTRACT", "FLDSIGNONREMARKS" };
        string[] alCaptions = { "S.No.", "Name", "Rank", "Zone", "Batch", "Vessel Joined", "SignOn Date", "Source", "Engaged/Re-Engaged", "On Promotion", "Convertee", "Full Contract", "Remarks" };
        string[] FilterColumns = { "FLDFROMDATE", "FLDTODATE", "FLDSELECTEDNATIONALITY", "FLDSELECTEDBATCH", "FLDSELECTEDZONE", "FLDSELECTEDVESSEL", "FLDSELECTEDPRINCIPAL", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDRANK" };
        string[] FilterCaptions = { "From Date", "To Date", "Nationality", "Batch", "Zone", "Vessel", "Principal", "Vessel Type", "Rank" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportsShortContract.CrewShortContractReport(
                     (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress),
                     (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                     (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                     (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                     (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                     (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                     (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                     General.GetNullableDateTime(ucDate.Text),
                     General.GetNullableDateTime(ucDate1.Text),
                     lstPool.SelectedPool,
                     sortexpression, sortdirection,
                     Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                     iRowCount,
                     ref iRowCount,
                     ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewShortContract.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Short Contract Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
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
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDZONE", "FLDBATCH", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSOURCEDFROM", "FLDENGAGED", "FLDPROMO", "FLDCONVERTEE", "FLDFULLCONTRACT", "FLDSIGNONREMARKS" };
        string[] alCaptions = { "S.No.", "Name", "Rank", "Zone", "Batch", "Vessel Joined", "SignOn Date", "Source", "Engaged/Re-Engaged", "On Promotion", "Convertee", "Full Contract", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCrewReportsShortContract.CrewShortContractReport(
                     (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucPrincipal.SelectedAddress),
                     (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                     (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                     (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                     (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                     (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                     (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                     General.GetNullableDateTime(ucDate.Text),
                     General.GetNullableDateTime(ucDate1.Text),
                     lstPool.SelectedPool,
                     sortexpression, sortdirection,
                     Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                     gvCrew.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Crew Short Contract", alCaptions, alColumns, ds);

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
            lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewDetails','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
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
}
