using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Configuration;
using Telerik.Web.UI.Calendar;

public partial class PlannedMaintenanceJobSummaryComponentJobGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
                ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();
                ucPlanningMethod.HardTypeCode = ((int)(PhoenixHardTypeCode.PLANNINGMETHOD)).ToString(); //"5";
                ucCounterType.HardTypeCode = "111";
                ucFrequency.HardTypeCode = ((int)(PhoenixHardTypeCode.PERIODICFREQUENCY)).ToString();// "7";

                cblJobStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 13, 0, "NON,AVA,IUE,REP");
                cblJobStatus.DataBind();

                BindFields();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            MenuComponentGeneral.MenuList = toolbarmain.Show();
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
            if (!string.IsNullOrEmpty(Request.QueryString["COMPONENTJOBID"]) && !string.IsNullOrEmpty(Request.QueryString["VESSELID"]))
            {
                DataSet ds = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(Request.QueryString["COMPONENTJOBID"]),int.Parse(Request.QueryString["VESSELID"]));
                DataRow dr = ds.Tables[0].Rows[0];
                MenuComponentGeneral.Title = "Component : " + dr["FLDCOMPONENTNUMBER"].ToString();
                txtFrequency.Text = dr["FLDJOBFREQUENCY"].ToString();
                ucFrequency.SelectedHard = dr["FLDJOBFREQUENCYTYPE"].ToString();
                ucDiscipline.SelectedDiscipline = dr["FLDJOBDISCIPLINE"].ToString();
                ucHistory.SelectedHistoryTemplate = dr["FLDHISTORYTEMPLATEID"].ToString();
                txtWindow.Text = dr["FLDWINDOW"].ToString();
                txtlastDoneDate.SelectedDate = General.GetNullableDateTime(dr["FLDJOBLASTDONEDATE"].ToString());
                txtNextDueDate.Text = General.GetDateTimeToString(dr["FLDJOBNEXTDUEDATE"].ToString());
                ucPlanningMethod.SelectedHard = dr["FLDPLANINGMETHOD"].ToString();
                txtDuration.Text = dr["FLDJOBDURATION"].ToString();
                ucMainType.SelectedQuick = dr["FLDMAINTNANCETYPE"].ToString();
                ucMaintClass.SelectedQuick = dr["FLDMAINTNANCECLASS"].ToString();
                ucMainCause.SelectedQuick = dr["FLDMAINTNANCECAUSE"].ToString();
                chkCheckList.Checked = dr["FLDCHECKLIST"].ToString() == "1" ? true : false;
                chkRAMandatory.Checked = dr["FLDISRAMANDATORY"].ToString() == "1" ? true : false;
                chkAttachment.Checked = dr["FLDATTACHMENTREQUIRED"].ToString() == "1" ? true : false;
                txtInstructions.Visible= dr["FLDATTACHMENTREQUIRED"].ToString() == "1" ? true : false;
                txtInstructions.Text = dr["FLDATTINSTRUCTIONS"].ToString();
                ucCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
                ucCounterFrequency.Text = dr["FLDCOUNTERVALUES"].ToString();
                string ldh = dr["FLDLASTDONEHOURS"].ToString();
                txtLastDoneHours.Text = ldh != string.Empty && ldh.IndexOf('.') > -1 ? ldh.Substring(0, ldh.IndexOf('.')) : string.Empty;
                ucNextDueHours.Text = dr["FLDNEXTDUEHOURS"].ToString();
                txtAverage.Text = dr["FLDAVERAGE"].ToString();
                txtCounterDate.Text = dr["FLDREADINGDATE"].ToString();
                txtCounterValue.Text = dr["FLDREADINGVALUE"].ToString();
                txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                txtJobName.Text = dr["FLDJOBTITLE"].ToString();
                txtPriority.Text = dr["FLDPRIORITY"].ToString();
                txtReference.Text = dr["FLDREFERENCE"].ToString();
                ucRank.SelectedRank = dr["FLDVSLVERIFYREQUIRED"].ToString();
                chkSupntVerification.Checked = dr["FLDSUPNTVERIFYREQUIRED"].ToString() == "1" ? true : false;
                ViewState["WORKORDERID"] = dr["FLDWORKORDERID"].ToString();
                if (dr["FLDMANDATORY"].ToString().Equals("Y"))
                    ucMandatory.YesNoOption = "Yes";
                else
                    ucMandatory.YesNoOption = "No";
                foreach (ButtonListItem li in cblJobStatus.Items)
                {
                    li.Selected = ("," + dr["FLDJOBSTATUS"].ToString() + ",").Contains("," + li.Value + ",") ? true : false;
                }
                chkPostOverHaulCheck.Checked = dr["FLDISPOSTOVERHAULCHECK"].ToString() == "1" ? true : false;
                rblInterval.SelectedValue = dr["FLDOVERHAULINTERVAL"].ToString();
                txtDetilsofCheck.Text = dr["FLDPOSTOVERHAULCHECKDESC"].ToString();
                txtInterValue.Text = dr["FLDOVERHAULINTERVALVALUE"].ToString();
                DataTable dt = PhoenixPlannedMaintenanceComponentJob.compJobRAEdit(new Guid(Request.QueryString["COMPONENTJOBID"]));
                if (dt.Rows.Count > 0)
                {
                    DataRow drr = dt.Rows[0];
                    txtRANumber.Text = drr["FLDRAREFNO"].ToString();
                    txtRA.Text = drr["FLDRISKASSESSMENT"].ToString();
                }
                
                ViewState["OPERATIONMODE"] = "EDIT";
            }
            else
            {
                txtPriority.Text = "3";
                ucMandatory.YesNoOption = "Yes";
                //ucHistory.SelectedHistoryTemplateName = "SIMPLE JOBS TEMPLATE";
                txtNextDueDate.Text = string.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                foreach (ButtonListItem li in cblJobStatus.Items) li.Selected = (li.Value == "35" ? true : false);
                ucPlanningMethod.SelectedHard = "10"; //Variable
                ViewState["OPERATIONMODE"] = "ADD";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PlannedMaintenanceComponent_TabStripCommand(object sender, EventArgs e)
    {

    }

}
