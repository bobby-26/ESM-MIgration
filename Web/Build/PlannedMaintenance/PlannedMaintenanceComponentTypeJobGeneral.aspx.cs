using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenanceComponentTypeJobGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuComponentTypeGeneral.MenuList = toolbarmain.Show();
            MenuComponentTypeGeneral.SetTrigger(pnlComponentTypeGeneral);

            txtJobId.Attributes.Add("style", "visibility:hidden");
            txtJobId.Attributes.Add("style", "visibility:hidden");
            if (!IsPostBack)
            {
                ucMainType.QuickTypeCode = "32";
                ucMaintClass.QuickTypeCode = "30";
                ucMainCause.QuickTypeCode = "29";
                ucPlanningMethod.HardTypeCode = "5";
                ucCounterType.HardTypeCode = "111";
                ucFrequency.HardTypeCode = "7";
                ucPlanningMethod.SelectedHard = "10"; //Variable
                ucMandatory.YesNoOption = "Yes";
                ucHistory.SelectedHistoryTemplate = "1";
                txtPriority.Text = "3";

                ViewState["WORKORDERID"] = "";
                if (Request.QueryString["COMPONENTTYPEID"] != null)
                    ViewState["COMPONENTTYPEID"] = Request.QueryString["COMPONENTTYPEID"].ToString();
                if (Session["JOBMODE"] != null)
                {
                    Session.Remove("JOBMODE");
                }

                BindFields();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["COMPONENTTYPEJOBID"] != null) && (Request.QueryString["COMPONENTTYPEJOBID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceComponentTypeJob.EditComponentTypeJob(new Guid(Request.QueryString["COMPONENTTYPEJOBID"]));
                DataRow dr = ds.Tables[0].Rows[0];

                txtJobId.Text = dr["FLDJOBID"].ToString();
                txtFrequency.Text = dr["FLDJOBFREQUENCY"].ToString();
                ucFrequency.SelectedHard = dr["FLDJOBFREQUENCYTYPE"].ToString();
                ucHistory.SelectedHistoryTemplate = dr["FLDHISTORYTEMPLATEID"].ToString();
                txtWindow.Text = dr["FLDWINDOW"].ToString();
                ucPlanningMethod.SelectedHard = dr["FLDPLANINGMETHOD"].ToString();
                ucMainType.SelectedQuick = dr["FLDMAINTNANCETYPE"].ToString();
                ucMaintClass.SelectedQuick = dr["FLDMAINTNANCECLASS"].ToString();
                ucMainCause.SelectedQuick = dr["FLDMAINTNANCECAUSE"].ToString();
                ucCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
                ucCounterValues.Text = dr["FLDCOUNTERVALUES"].ToString();
                txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                txtJobName.Text = dr["FLDJOBTITLE"].ToString();
                txtPriority.Text = dr["FLDPRIORITY"].ToString();
                ucDiscipline.SelectedDiscipline = dr["FLDDISCIPLINEID"].ToString();
                if (dr["FLDMANDATORY"].ToString().Equals("Y"))
                    ucMandatory.YesNoOption = "Yes";
                else
                    ucMandatory.YesNoOption = "No";


                ViewState["OPERATIONMODE"] = "EDIT";
            }
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PlannedMaintenanceComponentType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidComponentType(txtJobId.Text, txtFrequency.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixPlannedMaintenanceComponentTypeJob.UpdateComponentTypeJob(new Guid(Request.QueryString["COMPONENTTYPEJOBID"])
                    , General.GetNullableGuid(txtJobId.Text).Value, General.GetNullableGuid(Request.QueryString["COMPONENTTYPEID"]).Value
                    , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                    , General.GetNullableInteger(ucHistory.SelectedHistoryTemplate), General.GetNullableInteger(ucPlanningMethod.SelectedHard)
                    , General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtWindow.Text)
                    , General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick)
                    , General.GetNullableInteger(ucMainCause.SelectedQuick), ucMandatory.YesNoOption
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline)
                    , General.GetNullableInteger(ucCounterType.SelectedHard)
                    , General.GetNullableInteger(ucCounterValues.Text)
                    );
                    //New Work order will generat after work is done

                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixPlannedMaintenanceComponentTypeJob.InsertComponentTypeJob(General.GetNullableGuid(txtJobId.Text).Value
                    , General.GetNullableGuid(Request.QueryString["COMPONENTTYPEID"]).Value
                    , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                   , General.GetNullableInteger(ucHistory.SelectedHistoryTemplate), General.GetNullableInteger(ucPlanningMethod.SelectedHard)
                   , General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtWindow.Text)
                    , General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick)
                    , General.GetNullableInteger(ucMainCause.SelectedQuick), ucMandatory.YesNoOption
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline)
                    , General.GetNullableInteger(ucCounterType.SelectedHard)
                    , General.GetNullableInteger(ucCounterValues.Text)
                    );
                }

                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                txtJobId.Text = "";
                txtFrequency.Text = "";
                ucFrequency.SelectedHard = "";
                ucHistory.SelectedHistoryTemplate = "";
                txtWindow.Text = "";
                ucPlanningMethod.SelectedHard = "10"; //Variable
                ucMandatory.YesNoOption = "Yes";
                ucMainType.SelectedQuick = "";
                ucMaintClass.SelectedQuick = "";
                ucMainCause.SelectedQuick = "";
                txtJobCode.Text = "";
                txtJobName.Text = "";
                txtPriority.Text = "3";
                ucCounterType.SelectedHard = "";
                ucCounterValues.Text = "";
                ucHistory.SelectedHistoryTemplate = "1";
                ViewState["OPERATIONMODE"] = "ADD";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidComponentType(string job, string frequency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (job.Trim().Equals(""))
            ucError.ErrorMessage = "Job is required";

        if (!frequency.Equals("") && !General.GetNullableInteger(ucFrequency.SelectedHard).HasValue)
            ucError.ErrorMessage = "Frequency is required";

        return (!ucError.IsError);
    }

    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        txtJobCode.Text = "";
        txtJobName.Text = "";
        txtJobId.Text = "";
    }
}
