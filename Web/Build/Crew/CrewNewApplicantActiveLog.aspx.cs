using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewNewApplicantActiveLog : PhoenixBasePage
{
    private string strEmployeeId = string.Empty; 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection))
                strEmployeeId = Filter.CurrentNewApplicantSelection;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuActivity.AccessRights = this.ViewState;
            MenuActivity.MenuList = toolbar.Show();

            PhoenixToolbar toolbarSub = new PhoenixToolbar();
            toolbarSub.AddFontAwesomeButton("../Crew/CrewNewApplicantActiveLog.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarSub.AddFontAwesomeButton("javascript:CallPrint('gvActivity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuCrewActivityLog.AccessRights = this.ViewState;
            MenuCrewActivityLog.MenuList = toolbarSub.Show();
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["ACTIVITYID"] = null;
                ViewState["LASTACTIVITYDATE"] = string.Empty;
                txtFromDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
                SetEmployeePrimaryDetails();
                gvActivity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Reset()
    {
        ddlActivity.SelectedHard = string.Empty;
        ddlRank.SelectedRank = string.Empty;
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        ddlVessel.SelectedVessel = string.Empty;
        ddlSignOnPort.SelectedValue = string.Empty;
        ddlSignOffPort.SelectedValue = string.Empty;
        ddlSignOnReason.SelectedSignOnReason = string.Empty;
        ddlSignOffReason.SelectedSignOffReason = string.Empty;
        txtRemarks.Text = string.Empty;
        txttravelDays.Text = string.Empty;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDACTIVITYCODE", "FLDACTIVITYNAME", "FLDFROMDATE", "FLDTODATE", "FLDRANKNAME", "FLDVESSELNAME" };
        string[] alCaptions = { "Activity Code", "Activity Name", "From Date", "To Date", "Rank", "Vessel Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        DateTime dLastActiveDate = General.GetNullableDateTime(DateTime.Now.ToString()).Value;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataTable dt = PhoenixCrewActivityLog.SearchCrewActivityLog(int.Parse(strEmployeeId)
                                   , sortexpression, sortdirection
                                   , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                   , ref iRowCount, ref iTotalPageCount, ref dLastActiveDate);

        General.ShowExcel("Crew ActivityLog", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuCrewActivityLog_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void Activity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["ACTIVITYID"] = null;
                Reset();
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidActivity())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ACTIVITYID"] == null)
                {
                    InsertCrewActivity(null, null);
                }
                else
                {
                    PhoenixCrewActivityLog.UpdateCrewActivityLog(int.Parse(ViewState["ACTIVITYID"].ToString()), int.Parse(ddlActivity.SelectedHard), int.Parse(ddlRank.SelectedRank)
                                                               , DateTime.Parse(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
                                                               , General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlSignOnPort.SelectedValue)
                                                               , General.GetNullableInteger(ddlSignOffPort.SelectedValue), General.GetNullableInteger(ddlSignOnReason.SelectedSignOnReason)
                                                               , General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason), txtRemarks.Text
                                                               , General.GetNullableInteger(ucCourse.SelectedCourse)
                                                               , General.GetNullableString(txtCourseVenue.Text)
                                                               , General.GetNullableInteger(ucCountry.SelectedCountry)
                                                               , General.GetNullableInteger(txttravelDays.Text)
                                                               );
                    BindData();
                    gvActivity.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void InsertCrewActivity(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if ((sender != null && ucCM.confirmboxvalue == 1) || sender == null)
            {
                PhoenixCrewActivityLog.InsertCrewActivityLog(int.Parse(strEmployeeId), int.Parse(ddlActivity.SelectedHard), int.Parse(ddlRank.SelectedRank)
                                                                               , DateTime.Parse(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
                                                                               , General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlSignOnPort.SelectedValue)
                                                                               , General.GetNullableInteger(ddlSignOffPort.SelectedValue), General.GetNullableInteger(ddlSignOnReason.SelectedSignOnReason)
                                                                               , General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason), txtRemarks.Text, sender == null ? byte.Parse("0") : byte.Parse("1")
                                                                               , General.GetNullableInteger(ucCourse.SelectedCourse)
                                                                               , General.GetNullableString(txtCourseVenue.Text)
                                                                               , General.GetNullableInteger(ucCountry.SelectedCountry)
                                                                               , General.GetNullableInteger(txttravelDays.Text)
                                                                               );
                Reset();
                BindData();
                gvActivity.Rebind();
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.ToUpper().Contains("OVERLAPPING ACTIVITY CONTINUE"))
            {
                ucConfirm.Text = "Overlapping activity Continue ?";
                ucConfirm.Visible = true;
                ucConfirm.OKText = "Yes";
                ucConfirm.CancelText = "No";
            }
            else
                throw ex;
        }
    }
    public bool IsValidActivity()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        DateTime resultDate;

        if (ddlActivity.SelectedHard.Trim().Equals("") || !Int16.TryParse(ddlActivity.SelectedHard, out result))
            ucError.ErrorMessage = "Activity is required.";
        if (ddlRank.SelectedRank.Trim().Equals("") || !Int16.TryParse(ddlRank.SelectedRank, out result))
            ucError.ErrorMessage = "Rank is required.";
        if (!DateTime.TryParse(txtFromDate.Text, out resultDate))
            ucError.ErrorMessage = "From Date is required.";
        else if (DateTime.TryParse(txtFromDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }
        if (ddlVessel.CssClass == "dropdown_mandatory" && !Int16.TryParse(ddlVessel.SelectedVessel, out result))
            ucError.ErrorMessage = "Vessel is required.";
        if (txtToDate.CssClass == "input_mandatory" && !DateTime.TryParse(txtToDate.Text, out resultDate))
            ucError.ErrorMessage = "To Date is required.";
        return (!ucError.IsError);
    }

    private void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDACTIVITYCODE", "FLDACTIVITYNAME", "FLDFROMDATE", "FLDTODATE", "FLDRANKNAME", "FLDVESSELNAME" };
        string[] alCaptions = { "Activity Code", "Activity Name", "From Date", "To Date", "Rank", "Vessel Name" };
        DateTime dLastActiveDate = General.GetNullableDateTime(DateTime.Now.ToString()).Value;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewActivityLog.SearchCrewActivityLog(int.Parse(strEmployeeId)
                                   , sortexpression, sortdirection
                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                    , gvActivity.PageSize
                                   , ref iRowCount, ref iTotalPageCount, ref dLastActiveDate);
        ViewState["LASTACTIVITYDATE"] = dLastActiveDate;
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        ds.AcceptChanges();
        General.SetPrintOptions("gvActivity", "Crew ActivityLog", alCaptions, alColumns, ds);
        if (dt.Rows.Count > 0)
        {
            gvActivity.DataSource = dt;
            gvActivity.VirtualItemCount = iRowCount;
        }
        else
        {
            gvActivity.DataSource = "";
        }
    }
    
    private void SetActivityDetails(int iActivityLogId)
    {
        DataTable dt = PhoenixCrewActivityLog.ListCrewActivityLog(null, iActivityLogId);
        if (dt.Rows.Count > 0)
        {
            ddlActivity.SelectedHard = dt.Rows[0]["FLDACTIVITYID"].ToString();
            txtFromDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDFROMDATE"].ToString());
            txtToDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDTODATE"].ToString());
            ddlRank.SelectedRank = dt.Rows[0]["FLDRANKID"].ToString();
            ddlVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
            ddlSignOnReason.SelectedSignOnReason = dt.Rows[0]["FLDSIGNONREASON"].ToString();
            ddlSignOnPort.Text = dt.Rows[0]["FLDSIGNONSEAPORT"].ToString();
            ddlSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASON"].ToString();
            ddlSignOffPort.Text = dt.Rows[0]["FLDSIGNOFFSEAPORT"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
            ucCourse.SelectedCourse = dt.Rows[0]["FLDCOURSEID"].ToString();
            txtCourseVenue.Text = dt.Rows[0]["FLDCOURSEVENUE"].ToString();
            ucCountry.SelectedCountry = dt.Rows[0]["FLDCOURSECOUNTRYID"].ToString();
            txttravelDays.Text = dt.Rows[0]["FLDTRAVELDAYS"].ToString();
        }
    }  

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeCode.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPayRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ddlRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
                //txtSignedOff.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDSIGNOFFDATE"].ToString()));
                //txtLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                //txtDOA.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDOA"].ToString()));
                ViewState["VESSELID"] = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlActivity_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlHard ucHard = sender as UserControlHard;
        RadComboBox d = (RadComboBox)ucHard.FindControl("ddlHard");
        if (d.SelectedItem.Text.ToUpper() == "SEA SERVICE")
            ddlVessel.CssClass = "dropdown_mandatory";
        else
            ddlVessel.CssClass = "input";
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        UserControlDate ucDate = sender as UserControlDate;
        DateTime? d = General.GetNullableDateTime(ViewState["LASTACTIVITYDATE"] == null ? string.Empty : ViewState["LASTACTIVITYDATE"].ToString());
        DateTime resultdate;
        if (d.HasValue && d.Value != DateTime.Now)
        {
            if (DateTime.TryParse(txtFromDate.Text, out resultdate) && DateTime.Compare(d.Value, resultdate) > 0)
                txtToDate.CssClass = "input_mandatory";
            else
                txtToDate.CssClass = "input";
        }
    }

    protected void gvActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "ACTIVITYDELETE")
        {            
            PhoenixCrewActivityLog.DelteCrewActivityLog(Convert.ToInt32(((RadLabel)e.Item.FindControl("lblActivityLogId")).Text));            
            BindData();
            gvActivity.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvActivity_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    protected void gvActivity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            RadLabel lblOverLap = (RadLabel)e.Item.FindControl("lblOverLap");
            if (lblOverLap != null && lblOverLap.Text == "1")
            {
                e.Item.CssClass = "rowoverlap";
            }
        }
    }

    protected void gvActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvActivity.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvActivity_EditCommand(object sender, GridCommandEventArgs e)
    {
        string strActivityId = ((RadLabel)e.Item.FindControl("lblActivityLogId")).Text;
        ViewState["ACTIVITYID"] = strActivityId;
        SetActivityDetails(int.Parse(strActivityId));
        ddlActivity_TextChangedEvent(ddlActivity, null);        
    }
}
