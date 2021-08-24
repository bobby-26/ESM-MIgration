using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        SurveyScheduleGeneral.AccessRights = this.ViewState;
        if (Request.QueryString["AllowEdit"].Equals("1"))
        {
            SurveyScheduleGeneral.MenuList = toolbarmain.Show();
        }
        if (!IsPostBack)
        {
            ViewState["ScheduleId"] = Request.QueryString["ScheduleId"];
            ViewState["VesselId"] = Request.QueryString["VesselId"];
            BindStatus();
            PopulateDate(Request.QueryString["VesselId"], Request.QueryString["ScheduleId"]);
        }
    }
    private void BindStatus()
    {
        ddlStatus.DataSource = PhoenixPlannedMaintenanceSurveySchedule.SurveyStatusList();
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataTextField = "FLDSTATUS";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("--Select--", ""));
    }
    private void PopulateDate(string sVesselId, string sScheduleId)
    {
        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.SurveyScheduleEdit(General.GetNullableInteger(sVesselId), General.GetNullableGuid(sScheduleId));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtSurveyNumber.Text = ds.Tables[0].Rows[0]["FLDSURVEYNUMBER"].ToString();
            txtSurvey.Text = ds.Tables[0].Rows[0]["FLDSURVEYNAME"].ToString();
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            txtSurveyType.Text = ds.Tables[0].Rows[0]["FLDSURVEYTYPENAME"].ToString();
            ucPort.Text = ds.Tables[0].Rows[0]["FLDSEAPORTNAME"].ToString();
            ucPort.SelectedValue = ds.Tables[0].Rows[0]["FLDSURVEYPORT"].ToString();
            ucDueDate.Text = ds.Tables[0].Rows[0]["FLDNEXTDUEDATE"].ToString();
            ucScheduleDate.Text = ds.Tables[0].Rows[0]["FLDSCHEDULEDATE"].ToString();
            ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();
            ucDoneDate.Text = ds.Tables[0].Rows[0]["FLDLASTDONEDATE"].ToString();
            txtSurveyorName.Text = ds.Tables[0].Rows[0]["FLDSURVEYORNAME"].ToString();
            txtSurveyorDesignation.Text = ds.Tables[0].Rows[0]["FLDSURVEYORDESIG"].ToString();
            ucSurveyor.SelectedValue = ds.Tables[0].Rows[0]["FLDATTENDINGSUPT"].ToString();
            ucSurveyor.Text = ds.Tables[0].Rows[0]["FLDATTENDINGSUPTNAME"].ToString();
            ddlCompany.SelectedAddress = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        }
    }
   
    protected void SurveyScheduleGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "SAVE")
            {
                if (!isValidcontent(ucScheduleDate.Text, ddlCompany.SelectedAddress, ucSurveyor.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                //PhoenixPlannedMaintenanceSurveySchedule.UpdateCompletedDetails(General.GetNullableDateTime(ucDoneDate.Text)
                //    , General.GetNullableGuid(ViewState["ScheduleId"].ToString())
                //    , General.GetNullableInteger(ViewState["VesselId"].ToString()));
                PhoenixPlannedMaintenanceSurveySchedule.UpdateSurveySchedule(General.GetNullableInteger(ViewState["VesselId"].ToString())
                    , General.GetNullableGuid(ViewState["ScheduleId"].ToString())
                    , General.GetNullableInteger(ucPort.SelectedValue)
                    , General.GetNullableDateTime(ucScheduleDate.Text)
                    , General.GetNullableString(txtSurveyorName.Text.Trim())
                    , General.GetNullableString(txtSurveyorDesignation.Text.Trim())
                    , General.GetNullableInteger(ddlCompany.SelectedAddress)
                    , General.GetNullableInteger(ucSurveyor.SelectedValue));
                ucStatus.Text = "Details Updated Sucessfully";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool isValidcontent(string ScheduleDate, string Company, string Superdent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(ScheduleDate))
            ucError.ErrorMessage = "Schedule Date is Required";
        //if (Company.Trim().Equals("") || Company.Trim().ToUpper().Equals("DUMMY"))
        //    ucError.ErrorMessage = "Company is Required";
        if (Superdent.Trim().Equals(""))
            ucError.ErrorMessage = "Attending Superintendent is Required";
        //if (General.GetNullableDateTime(ScheduleDate) < DateTime.Today)
        //    ucError.ErrorMessage = "Schedule Date should be Later than Today's Date";
        return (!ucError.IsError);
    }
}
