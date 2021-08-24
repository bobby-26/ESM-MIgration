using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleComplete : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        if (Request.QueryString["AllowEdit"].Equals("1"))
        {
            SurveyScheduleComplte.AccessRights = this.ViewState;
            SurveyScheduleComplte.MenuList = toolbarmain.Show();
        }
        if (!IsPostBack)
        {
            ViewState["SheduleId"] = Request.QueryString["ScheduleId"];
            ViewState["VesselId"] = Request.QueryString["VesselId"];
            PopulateDate(Request.QueryString["VesselId"], Request.QueryString["ScheduleId"]);
            ucComplteDate.Focus();
        }
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
            txtSurveyorName.Text = ds.Tables[0].Rows[0]["FLDSURVEYORNAME"].ToString();
            txtSurveyorDesignation.Text = ds.Tables[0].Rows[0]["FLDSURVEYORDESIG"].ToString();
            ucSurveyor.SelectedValue = ds.Tables[0].Rows[0]["FLDATTENDINGSUPT"].ToString();
            ucSurveyor.Text = ds.Tables[0].Rows[0]["FLDATTENDINGSUPTNAME"].ToString();
            ddlCompany.SelectedAddress = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
            ucComplteDate.Text = ds.Tables[0].Rows[0]["FLDLASTDONEDATE"].ToString();
        }
    }
    protected void SurveyScheduleComplte_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "SAVE")
            {
                if (!isValidcontent(ucComplteDate.Text, ucSurveyor.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceSurveySchedule.UpdateCompletedDetails(General.GetNullableDateTime(ucComplteDate.Text)
                    , new Guid(ViewState["SheduleId"].ToString())
                    , int.Parse(ViewState["VesselId"].ToString())
                    , General.GetNullableString(txtSurveyorName.Text.Trim())
                    , General.GetNullableString(txtSurveyorDesignation.Text.Trim())
                    , General.GetNullableInteger(ddlCompany.SelectedAddress)
                    , General.GetNullableInteger(ucSurveyor.SelectedValue));
                ucStatus.Text = "Survey Completed Sucessfully";
                ucStatus.Visible = true;
                string script = string.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool isValidcontent(string DoneDate, string Superdent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(DoneDate))
            ucError.ErrorMessage = "Completed Date is Required";
        if (General.GetNullableDateTime(DoneDate) > DateTime.Today)
            ucError.ErrorMessage = "Completed Date should be later than Today's Date";
        if (Superdent.Trim().Equals(""))
            ucError.ErrorMessage = "Attending Superintendent is Required";
        return (!ucError.IsError);
    }
}
