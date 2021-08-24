using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionTrainingdoneedit : PhoenixBasePage
{ protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["TRAININGSCHEDULEID"] = General.GetNullableGuid(Request.QueryString["trainingscheduleid"]);
            Guid? Trainingscheduleid = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());
            DataTable dt = PhoenixInspectionTrainingSchedule.TrainingScheduleEditList(Trainingscheduleid);
            if (dt.Rows.Count > 0)
            {
                radduedate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDTRAININGONBOARDDUEDATE"].ToString()); ;
                radlastdonedate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDTRAININGONBOARDLASTDONEDATE"].ToString());

            }
        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "SAVE", ToolBarDirection.Right);

        Tabstriptrainingschedule.MenuList = toolbargrid.Show();
    }

    protected void Tabstriptrainingschedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? Trainingscheduleid = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());

                DateTime? duedate = General.GetNullableDateTime(radduedate.Text);
                DateTime? Lastdonedate = General.GetNullableDateTime(radlastdonedate.Text);

                if (!IsValidTrainingDetails(duedate, Lastdonedate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionTrainingSchedule.TrainingScheduleLastdoneUpdate(rowusercode, Trainingscheduleid, Lastdonedate, duedate);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidTrainingDetails(DateTime? duedate, DateTime? Lastdonedate)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (duedate == null)
        {
            ucError.ErrorMessage = " Due Date";
        }

        if (Lastdonedate == null)
        {
            ucError.ErrorMessage = " Last Done Date";
        }


        return (!ucError.IsError);
    }
}