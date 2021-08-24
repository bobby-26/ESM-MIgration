using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using SouthNests.Phoenix.CrewReports;
using System.Collections.Specialized;
using System.Configuration;
using Telerik.Web.UI;
public partial class Crew_CrewReportMedicalCaseSummarySearch : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMedicalCaseSummarySearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvMedicalCase')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportMedicalCaseSummarySearch.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , 53, 1, string.Empty).Tables[0]);
                lstStatus.DataSource = ds;

                lstStatus.DataBind();
                lstStatus.Items.Insert(0, new RadListBoxItem("--Select--", ""));
                gvMedicalCase.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvMedicalCase.SelectedIndexes.Clear();
        gvMedicalCase.EditIndexes.Clear();
        gvMedicalCase.DataSource = null;
        gvMedicalCase.Rebind();
    }
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Selected == true && item.Value != "")
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        return strlist.ToString().TrimEnd(',');
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
                ucRank.selectedlist = "";
                ucBatch.SelectedList = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucZone.SelectedZoneValue = "";
                ucManager.SelectedList = "";
                ucPool.SelectedPoolValue = "";
                ucVessel.SelectedVessel = "";
                ucPrincipal.SelectedList = "";
                txtFileNo.Text = "";
                ddlStatus.SelectedValue = "";
                ucDate.Text = null;
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
                if (!IsValidFilter(ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDREFERENCENO", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDSEAMANBOOKNO", "FLDDATEOFILLNESS", "FLDSIGNOFFDATE", "FLDTYPEOFMEDICALCASE", "FLDTYPESOFINJURY", "FLDPENDINGWITH", "FLDFROMDATE", "FLDSTATUS", "FLDREMARKS", "FLDVESSELCBA", "FLDTIMELIMIT", "FLDDATEOFCLOSURE", "FLDTOTALEXPENSE", "FLDCREWDEDUCTABLE", "FLDTOTALCLAIMABLE" };
        string[] alCaptions = { "Sr.No", "Ref Number", "File No", "Crew's Name", "Rank", "Vessel Name", "Vessel Type", "CDC Number", "Illness/Injury Date", "Sign Off Date", "Category Illness/Injury", "Type of Injury /Illness", "Pending With", "From Date", "Status", "Remarks", "Applicable CBA", "Limitation Period", "Date of Closure as per Limitation Period Prescribed in CBA", "Total Cost Incurred", "Deductible Amount", "Claimable Amount" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucPrincipal.SelectedList = ucPrincipal.SelectedList.ToString().Contains("Dummy,") ? ucPrincipal.SelectedList.ToString().Replace("Dummy,", "") : ucPrincipal.SelectedList;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucManager.SelectedList = ucManager.SelectedList.ToString().Contains("Dummy,") ? ucManager.SelectedList.ToString().Replace("Dummy,", "") : ucManager.SelectedList;
        ucBatch.SelectedList = ucBatch.SelectedList.ToString().Contains("Dummy,") ? ucBatch.SelectedList.ToString().Replace("Dummy,", "") : ucBatch.SelectedList;

        ds = PhoenixCrewMedicalCaseSummaryReport.CrewMedicalCaseSummarySearch(General.GetNullableDateTime(ucDate.Text)
            , General.GetNullableDateTime(ucDate1.Text)
            , General.GetNullableString(ucPrincipal.SelectedList)
            , General.GetNullableString(ucRank.selectedlist)
            , General.GetNullableString(ucVesselType.SelectedVesseltype)
            , General.GetNullableString(ucVessel.SelectedVessel)
            , General.GetNullableString(ucPool.SelectedPool)
            , General.GetNullableString(ucZone.selectedlist)
            , General.GetNullableString(ucManager.SelectedList)
            , General.GetNullableInteger(ddlStatus.SelectedValue)
            , General.GetNullableString(StatusSelectedList())
            , General.GetNullableString(ucBatch.SelectedList)
            , General.GetNullableString(txtFileNo.Text.Trim())
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        string fromdates = ucDate.Text;
        string todatess = ucDate1.Text;

        Response.AddHeader("Content-Disposition", "attachment; filename=Medical_Case_Summary.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Medical Case Summary</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
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
                Response.Write("<td align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.Write(string.IsNullOrEmpty(ConfigurationManager.AppSettings["softwarename"]) ? "" : ConfigurationManager.AppSettings["softwarename"]);
        Response.End();
    }
    private void ShowReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDREFERENCENO", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDTYPEDESCRIPTION", "FLDSEAMANBOOKNO", "FLDDATEOFILLNESS", "FLDSIGNOFFDATE", "FLDTYPEOFMEDICALCASE", "FLDTYPESOFINJURY", "FLDPENDINGWITH", "FLDFROMDATE", "FLDSTATUS", "FLDREMARKS", "FLDVESSELCBA", "FLDTIMELIMIT", "FLDDATEOFCLOSURE", "FLDTOTALEXPENSE", "FLDCREWDEDUCTABLE", "FLDTOTALCLAIMABLE" };
            string[] alCaptions = { "Sr.No", "Ref Number", "File No", "Crew's Name", "Rank", "Vessel Name", "Vessel Type", "CDC Number", "Illness/Injury Date", "Sign Off Date", "Category Illness/Injury", "Type of Injury /Illness", "Pending With", "From Date", "Status", "Remarks", "Applicable CBA", "Limitation Period", "Date of Closure as per Limitation Period Prescribed in CBA", "Total Cost Incurred", "Deductible Amount", "Claimable Amount" };


            ucPrincipal.SelectedList = ucPrincipal.SelectedList.ToString().Contains("Dummy,") ? ucPrincipal.SelectedList.ToString().Replace("Dummy,", "") : ucPrincipal.SelectedList;
            ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
            ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
            ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
            ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
            ucManager.SelectedList = ucManager.SelectedList.ToString().Contains("Dummy,") ? ucManager.SelectedList.ToString().Replace("Dummy,", "") : ucManager.SelectedList;
            ucBatch.SelectedList = ucBatch.SelectedList.ToString().Contains("Dummy,") ? ucBatch.SelectedList.ToString().Replace("Dummy,", "") : ucBatch.SelectedList;

            DataSet ds = new DataSet();
            ds = PhoenixCrewMedicalCaseSummaryReport.CrewMedicalCaseSummarySearch(General.GetNullableDateTime(ucDate.Text)
                , General.GetNullableDateTime(ucDate1.Text)
                , General.GetNullableString(ucPrincipal.SelectedList)
                , General.GetNullableString(ucRank.selectedlist)
                , General.GetNullableString(ucVesselType.SelectedVesseltype)
                , General.GetNullableString(ucVessel.SelectedVessel)
                , General.GetNullableString(ucPool.SelectedPool)
                , General.GetNullableString(ucZone.selectedlist)
                , General.GetNullableString(ucManager.SelectedList)
                , General.GetNullableInteger(ddlStatus.SelectedValue)
                , General.GetNullableString(StatusSelectedList())
                , General.GetNullableString(ucBatch.SelectedList)
                , General.GetNullableString(txtFileNo.Text.Trim())
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , gvMedicalCase.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.SetPrintOptions("gvMedicalCase", "Medical Case Summary", alCaptions, alColumns, ds);

            gvMedicalCase.DataSource = ds;
            gvMedicalCase.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMedicalCase_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel lblEmpNo = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
                if (lblEmpNo != null && lnkName != null)
                    lnkName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpNo.Text + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMedicalCase_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvMedicalCase_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMedicalCase.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvMedicalCase_ItemCommand(object sender, GridCommandEventArgs e)
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
