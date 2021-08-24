using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceComponentJobCRAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucOMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
                ucOMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
                ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();
                ucOMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();
                ucPlanningMethod.HardTypeCode = ((int)(PhoenixHardTypeCode.PLANNINGMETHOD)).ToString(); //"5";
                ucOPlanningMethod.HardTypeCode = ((int)(PhoenixHardTypeCode.PLANNINGMETHOD)).ToString(); //"5";
                ucCounterType.HardTypeCode = "111";
                ucOCounterType.HardTypeCode = "111";
                ucFrequency.HardTypeCode = ((int)(PhoenixHardTypeCode.PERIODICFREQUENCY)).ToString();// "7";
                ucOFrequency.HardTypeCode = ((int)(PhoenixHardTypeCode.PERIODICFREQUENCY)).ToString();// "7";

                cblJobStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 13, 0, "NON,AVA,IUE,REP");
                cblJobStatus.DataBind();

                cblOJobStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 13, 0, "NON,AVA,IUE,REP");
                cblOJobStatus.DataBind();

                ViewState["REQUESTID"] = Request.QueryString["REQUESTID"];

                BindFields();
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
          
            MenuComponentGeneral.MenuList = toolbarmain.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields(DataRow dr)
    {
        if (dr.Table.Columns.Contains("FLDREQUESTTYPE"))
            ddlChangeReqType.SelectedValue = dr["FLDREQUESTTYPE"].ToString();
        if (dr.Table.Columns.Contains("FLDREMARKS"))
            txtRemarksChange.Text = dr["FLDREMARKS"].ToString();
        txtComponentJobId.SelectedValue = dr["FLDCOMPONENTJOBID"].ToString();
        txtComponentJobId.Text = dr["FLDJOBTITLE"].ToString();
        //txtCompJobCode.Text = dr["FLDJOBCODE"].ToString();
        //txtCompJobName.Text = dr["FLDJOBTITLE"].ToString();
        txtJobId.Text = dr["FLDJOBID"].ToString();
        txtFrequency.Text = dr["FLDJOBFREQUENCY"].ToString();
        ucFrequency.SelectedHard = dr["FLDJOBFREQUENCYTYPE"].ToString();
        ucDiscipline.SelectedDiscipline = dr["FLDJOBDISCIPLINE"].ToString();
        txtWindow.Text = dr["FLDWINDOW"].ToString();
        ucPlanningMethod.SelectedHard = dr["FLDPLANINGMETHOD"].ToString();
        txtDuration.Text = dr["FLDJOBDURATION"].ToString();
        ucMainType.SelectedQuick = dr["FLDMAINTNANCETYPE"].ToString();
        ucMaintClass.SelectedQuick = dr["FLDMAINTNANCECLASS"].ToString();
        ucMainCause.SelectedQuick = dr["FLDMAINTNANCECAUSE"].ToString();

        ucCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
        ucCounterFrequency.Text = dr["FLDCOUNTERVALUES"].ToString();


        txtJobCode.Text = dr["FLDJOBCODE"].ToString();
        txtJobName.Text = dr["FLDJOBTITLE"].ToString();
        txtPriority.Text = dr["FLDPRIORITY"].ToString();
        txtReference.Text = dr["FLDREFERENCE"].ToString();

        if (dr["FLDMANDATORY"].ToString().Equals("1"))
            ucMandatory.YesNoOption = "YES";
        else
            ucMandatory.YesNoOption = "NO";
        foreach (RadListBoxItem li in cblJobStatus.Items)
        {
            li.Checked = ("," + dr["FLDJOBSTATUS"].ToString() + ",").Contains("," + li.Value + ",") ? true : false;
        }
    }
    private void BindFields()
    {
        try
        {
            if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
            {
                DataSet ds = PhoenixPlannedMaintenanceComponentJobCR.EditComponentJobChangeRequest(new Guid(ViewState["REQUESTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];

                BindFields(dr);
                if (General.GetNullableGuid(dr["FLDCOMPONENTJOBID"].ToString()).HasValue)
                {
                    ds = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(dr["FLDCOMPONENTJOBID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        dr = ds.Tables[0].Rows[0];

                        SetComponetJobActualValues(dr);
                    }
                }
            }
            else
            {
                txtPriority.Text = "3";
                ucMandatory.YesNoOption = "YES";
                foreach (RadListBoxItem li in cblJobStatus.Items) li.Checked = (li.Value == "35" ? true : false);
                ucPlanningMethod.SelectedHard = "10"; //Variable             
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidComponent(txtComponentJobId.SelectedValue, txtJobId.Text
                    , txtFrequency.Text, ucCounterType.SelectedHard, ucCounterFrequency.Text, txtPriority.Text, ddlChangeReqType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                string jobstatus = string.Empty;
                foreach (RadListBoxItem li in cblJobStatus.Items)
                {
                    jobstatus += (li.Checked ? li.Value + "," : string.Empty);
                }
                jobstatus = jobstatus.TrimEnd(',');

                if (ViewState["REQUESTID"] != null)
                {
                    PhoenixPlannedMaintenanceComponentJobCR.UpdateComponentJobChangeRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , General.GetNullableGuid(ViewState["REQUESTID"].ToString()).Value
                   , General.GetNullableGuid(txtJobId.Text).Value, General.GetNullableGuid(txtComponentJobId.SelectedValue).Value
                   , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                   , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null
                   , General.GetNullableInteger(ucPlanningMethod.SelectedHard), General.GetNullableInteger(txtPriority.Text)
                   , General.GetNullableInteger(txtWindow.Text)
                   , General.GetNullableInteger(ucMainType.SelectedQuick)
                   , General.GetNullableInteger(ucMaintClass.SelectedQuick), General.GetNullableInteger(ucMainCause.SelectedQuick)
                   , General.GetNullableDecimal(txtDuration.Text), byte.Parse(ucMandatory.YesNoOption.ToUpper() == "YES" ? "1" : "0")
                   , General.GetNullableInteger(ucCounterType.SelectedHard), General.GetNullableInteger(ucCounterFrequency.Text)
                   , null/*General.GetNullableInteger(txtWindowsHrs.Text)*/
                   , jobstatus, txtReference.Text
                   , byte.Parse(ddlChangeReqType.SelectedValue)
                   , txtRemarksChange.Text);

                    /*Change req history update */
                    PhoenixPlannedMaintenanceComponentJobCR.CompJobChangeRequestHistoryInsert(new Guid(ViewState["REQUESTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    Response.Redirect("PlannedMaintenanceComponentJobCRList.aspx", false);
                }
                else if (ViewState["REQUESTID"] == null)
                {
                    PhoenixPlannedMaintenanceComponentJobCR.InsertComponentJobChangeRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , General.GetNullableGuid(txtJobId.Text).Value, General.GetNullableGuid(txtComponentJobId.SelectedValue).Value
                   , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                   , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null
                   , General.GetNullableInteger(ucPlanningMethod.SelectedHard), General.GetNullableInteger(txtPriority.Text)
                   , General.GetNullableInteger(txtWindow.Text)
                   , General.GetNullableInteger(ucMainType.SelectedQuick)
                   , General.GetNullableInteger(ucMaintClass.SelectedQuick), General.GetNullableInteger(ucMainCause.SelectedQuick)
                   , General.GetNullableDecimal(txtDuration.Text), byte.Parse(ucMandatory.YesNoOption.ToUpper() == "YES" ? "1" : "0")
                   , General.GetNullableInteger(ucCounterType.SelectedHard), General.GetNullableInteger(ucCounterFrequency.Text)
                   , null/*General.GetNullableInteger(txtWindowsHrs.Text)*/
                   , jobstatus, txtReference.Text
                   , byte.Parse(ddlChangeReqType.SelectedValue)
                   , txtRemarksChange.Text);
                }
                Response.Redirect("PlannedMaintenanceComponentJobCRList.aspx", false);

            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["REQUESTID"] = null;

                txtJobId.Text = "";
                txtFrequency.Text = "";
                ucFrequency.SelectedHard = "";
                ucDiscipline.SelectedDiscipline = "";
                txtWindow.Text = "";
                ucPlanningMethod.SelectedHard = "10"; //Variable
                txtDuration.Text = "";
                ucMainType.SelectedQuick = "";
                ucMaintClass.SelectedQuick = "";
                ucMainCause.SelectedQuick = "";
                txtJobCode.Text = "";
                txtJobName.Text = "";
                txtPriority.Text = "3";
                ucCounterType.SelectedHard = "";
                ucCounterFrequency.Text = "0";
                ucMandatory.YesNoOption = "YES";
                foreach (RadListBoxItem li in cblJobStatus.Items) li.Checked = (li.Value == "35" ? true : false);
                txtReference.Text = "";
                txtComponentJobId.Text = "";
                //txtCompJobName.Text = "";
                //txtCompJobCode.Text = "";
                txtRemarksChange.Text = "";
                ddlChangeReqType.SelectedIndex = -1;

                txtOJobId.Text = "";
                txtOFrequency.Text = "";
                ucOFrequency.SelectedHard = "";
                ucODiscipline.SelectedDiscipline = "";
                txtOWindow.Text = "";
                ucOPlanningMethod.SelectedHard = "10"; //Variable
                txtODuration.Text = "";
                ucOMainType.SelectedQuick = "";
                ucOMaintClass.SelectedQuick = "";
                ucOMainCause.SelectedQuick = "";
                txtOJobCode.Text = "";
                txtOJobName.Text = "";
                txtOPriority.Text = "3";
                ucOCounterType.SelectedHard = "";
                ucOCounterFrequency.Text = "0";
                txtOHistoryMandatory.Text = "";
                foreach (RadListBoxItem li in cblOJobStatus.Items) li.Checked = (li.Value == "35" ? true : false);
                txtOReference.Text = "";

            }
            else if(CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("PlannedMaintenanceComponentJobCRList.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidComponent(string componentjobid, string job, string frequency, string countertype, string countervalue, string piriority, string requesttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableGuid(componentjobid).HasValue)
            ucError.ErrorMessage = "Component Job is requierd";

        if (job.Trim().Equals(""))
            ucError.ErrorMessage = "Job is required";

        if (!frequency.Equals("") && !General.GetNullableInteger(ucFrequency.SelectedHard).HasValue)
            ucError.ErrorMessage = "Frequency is required";

        if ((frequency.Equals("") || frequency.Equals("0")) && (countertype.Equals("Dummy") || countervalue == "" || countervalue == "0"))
            ucError.ErrorMessage = "Either Frequency or Counter type needs to be specified";

        if (piriority.Trim().Equals("") || (int.Parse(piriority) > 3 || int.Parse(piriority) < 1))
            ucError.ErrorMessage = "Job Priority is required(1-3)";

        if (!General.GetNullableInteger(ucPlanningMethod.SelectedHard).HasValue)
            ucError.ErrorMessage = "Planning Method is requierd";

        if (!General.GetNullableInteger(requesttype).HasValue)
            ucError.ErrorMessage = "Request Type is mandatory.";

        return (!ucError.IsError);
    }

    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        txtJobCode.Text = "";
        txtJobName.Text = "";
        txtJobId.Text = "";
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtComponentJobId.SelectedValue).HasValue)
        {
            DataSet ds = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(txtComponentJobId.SelectedValue), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                SetComponetJobActualValues(dr);

                BindFields(dr);
            }
        }
    }
    protected void ddlChangeReqType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ClearRequestValues();
        if (ddlChangeReqType.SelectedValue == "0" || ddlChangeReqType.SelectedValue == "")
        {
            divOldValues.Visible = false;
        }
        else
        {
            divOldValues.Visible = true;
        }
    }
    private void SetComponetJobActualValues(DataRow dr)
    {

        txtOJobId.Text = dr["FLDJOBID"].ToString();
        txtOFrequency.Text = dr["FLDJOBFREQUENCY"].ToString();
        ucOFrequency.SelectedHard = dr["FLDJOBFREQUENCYTYPE"].ToString();
        ucODiscipline.SelectedDiscipline = dr["FLDJOBDISCIPLINE"].ToString();
        txtOWindow.Text = dr["FLDWINDOW"].ToString();
        ucOPlanningMethod.SelectedHard = dr["FLDPLANINGMETHOD"].ToString();
        txtODuration.Text = dr["FLDJOBDURATION"].ToString();
        ucOMainType.SelectedQuick = dr["FLDMAINTNANCETYPE"].ToString();
        ucOMaintClass.SelectedQuick = dr["FLDMAINTNANCECLASS"].ToString();
        ucOMainCause.SelectedQuick = dr["FLDMAINTNANCECAUSE"].ToString();

        ucOCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
        ucOCounterFrequency.Text = dr["FLDCOUNTERVALUES"].ToString();


        txtOJobCode.Text = dr["FLDJOBCODE"].ToString();
        txtOJobName.Text = dr["FLDJOBTITLE"].ToString();
        txtOPriority.Text = dr["FLDPRIORITY"].ToString();
        txtOReference.Text = dr["FLDREFERENCE"].ToString();

        if (dr["FLDMANDATORY"].ToString().Equals("Y"))
            txtOHistoryMandatory.Text = "Yes";
        else
            txtOHistoryMandatory.Text = "No";
        foreach (RadListBoxItem li in cblOJobStatus.Items)
        {
            li.Checked = ("," + dr["FLDJOBSTATUS"].ToString() + ",").Contains("," + li.Value + ",") ? true : false;
        }

    }
}