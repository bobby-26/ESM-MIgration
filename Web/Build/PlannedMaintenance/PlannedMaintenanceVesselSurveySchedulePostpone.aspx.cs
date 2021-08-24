using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System.Collections.Specialized;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveySchedulePostpone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE");
        if (Request.QueryString["AllowEdit"].Equals("1"))
        {
            SurveySchedulePostpone.AccessRights = this.ViewState;
            SurveySchedulePostpone.MenuList = toolbarmain.Show();
        }
        if (!IsPostBack)
        {
            ViewState["ScheduleId"] = Request.QueryString["ScheduleId"];
            PopulateDate(Request.QueryString["VesselId"], Request.QueryString["ScheduleId"]);
            ucPostponeDate.Focus();
            
        }
    }
    private void PopulateDate(string sVesselId, string sScheduleId)
    {
        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.SurveyScheduleEdit(General.GetNullableInteger(sVesselId), General.GetNullableGuid(sScheduleId));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtRemarks.Text = ds.Tables[0].Rows[0]["FLDPOSTPONEREMARKS"].ToString();
            ucPostponeDate.Text = ds.Tables[0].Rows[0]["FLDPOSTPONEDATE"].ToString();
            txtSurveyNumber.Text = ds.Tables[0].Rows[0]["FLDSURVEYNUMBER"].ToString();
            txtSurvey.Text = ds.Tables[0].Rows[0]["FLDSURVEYNAME"].ToString();
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            txtSurveyType.Text = ds.Tables[0].Rows[0]["FLDSURVEYTYPENAME"].ToString();
        }
    }
    
    protected void SurveySchedulePostpone_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "SAVE")
            {
                if (!isValidcontent(ucPostponeDate.Text, txtRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceSurveySchedule.UpdatePostponeDetails(General.GetNullableDateTime(ucPostponeDate.Text)
                    , General.GetNullableString(txtRemarks.Text)
                    , General.GetNullableGuid(ViewState["ScheduleId"].ToString()));
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
    private bool isValidcontent(string PostponeDate, string Remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(PostponeDate))
            ucError.ErrorMessage = "Postpone Date is Required";
        if (Remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is Required";
        if (General.GetNullableDateTime(PostponeDate) < DateTime.Today)
            ucError.ErrorMessage = "Postpone Date should be later than Today's Date";
            return (!ucError.IsError);
    }
}


