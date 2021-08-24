using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Crew_CrewReportActivityWiseCourse : PhoenixBasePage
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

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportActivityWiseCourse.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvBatch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportActivityWiseCourse.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["DOCTYPE"] = "";
                //ucPool.Attributes.Add("style", "border-right:white");                
                ucToDate.Text = DateTime.Now.ToShortDateString();
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 53, 1, string.Empty).Tables[0]);
                lstStatus.DataSource = ds;

                lstStatus.DataBind();
                lstStatus.Items.Insert(0, new RadListBoxItem("--Select--", ""));
                gvBatch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindCourse();
            }
            //ShowReport();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCourse()
    {
        lstCourse.Items.Clear();
        RadListBoxItem items = new RadListBoxItem();
        if (ucDocumentType.SelectedHard.Trim().Equals(PhoenixCommonRegisters.GetHardCode(1, 103, "3")))
            lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 103, "3")));
        else
            lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(General.GetNullableString(ViewState["DOCTYPE"].ToString()));
        lstCourse.DataTextField = "FLDDOCUMENTNAME";
        lstCourse.DataValueField = "FLDDOCUMENTID";
        lstCourse.DataBind();
        lstCourse.Items.Insert(0, new RadListBoxItem("--Select--", ""));
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

    private string CourseSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstCourse.Items)
        {
            if (item.Selected == true && item.Value != "")
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        return strlist.ToString().TrimEnd(',');
    }
    protected void DocumentTypeSelection(object sender, EventArgs e)
    {
        ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
        BindCourse();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        string[] alColumns = { "FLDROWNUMBER", "FLDCOURSE", "FLDCOURSEVENUE", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDFROMDATE", "FLDTODATE", "FLDACTIVITYVESSELNAME", "FLDLASTVESSELNAME", "FLDTRAVELDAYS", "FLDREMARKS" };
        string[] alCaptions = { "S.No.", "Course", "Course Venue", "Name", "File No.", "Rank", "From Date", "To Date", "Vessel", "Last Vessel", "Travel Days", "Remarks" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        DataSet ds = new DataSet();
        ds = PhoenixCrewActivityCourseWiseQuery.CrewActivityCourseWiseQuerySearch(General.GetNullableDateTime(ucFromDate.Text)
                , General.GetNullableDateTime(ucToDate.Text)
                , General.GetNullableString(ucVessel.SelectedVessel)
                , General.GetNullableString(ucPool.SelectedPool)
                , General.GetNullableString(ucZone.selectedlist)
                , General.GetNullableString(StatusSelectedList())
                , General.GetNullableString(txtFileNo.Text.Trim())
                , General.GetNullableString(CourseSelectedList())
                , General.GetNullableInteger(ucDocumentType.SelectedHard)
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);
        
        DataTable dt = new DataTable();

        string sortexpression = null;
        int? sortdirection = null;

        dt = ds.Tables[0];

        General.ShowExcel("Activity Course Report", dt, alColumns, alCaptions, sortdirection, sortexpression);

    }

    private void ShowReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDCOURSE", "FLDCOURSEVENUE", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDFROMDATE", "FLDTODATE", "FLDACTIVITYVESSELNAME", "FLDLASTVESSELNAME", "FLDTRAVELDAYS", "FLDREMARKS" };
            string[] alCaptions = { "S.No.", "Course", "Course Venue", "Name", "File No.", "Rank", "From Date", "To Date", "Vessel", "Last Vessel", "Travel Days", "Remarks" };


            ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
            ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
            ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;


            DataSet ds = new DataSet();

            ds = PhoenixCrewActivityCourseWiseQuery.CrewActivityCourseWiseQuerySearch(General.GetNullableDateTime(ucFromDate.Text)
                  , General.GetNullableDateTime(ucToDate.Text)
                , General.GetNullableString(ucVessel.SelectedVessel)
                , General.GetNullableString(ucPool.SelectedPool)
                , General.GetNullableString(ucZone.selectedlist)
                , General.GetNullableString(StatusSelectedList())
                , General.GetNullableString(txtFileNo.Text.Trim())
                , General.GetNullableString(CourseSelectedList())
                , General.GetNullableInteger(ucDocumentType.SelectedHard)
                , gvBatch.CurrentPageIndex+1
                , gvBatch.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.SetPrintOptions("gvBatch", "Activity Course Report", alCaptions, alColumns, ds);

            gvBatch.DataSource = ds;
            gvBatch.VirtualItemCount = iRowCount;
          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
                ucVessel.SelectedVessel = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPoolValue = "";
                lstCourse.SelectedIndex = 0;
                lstStatus.SelectedIndex = 0;
                ucFromDate.Text = null;
                ucToDate.Text = DateTime.Now.ToShortDateString();
                txtFileNo.Text = "";
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
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                gvBatch.CurrentPageIndex = 0;
                ShowReport();
                gvBatch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                gvBatch.CurrentPageIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBatch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBatch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBatch.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}