using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentJobGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            ucCounterFrequency_TextChanged(null, null);
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                Response.Redirect("PlannedMaintenanceComponentJobGeneralOffice.aspx?" + Request.QueryString.ToString(), true);
            }
            txtJobId.Attributes.Add("style", "visibility:hidden");
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
                ViewState["WORKORDERID"] = "";
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["COMPONENTID"]))
                {
                    ViewState["OPERATIONMODE"] = "EDIT";
                    BindFields();
                }
                else
                {
                    BindFields();
                }
                chkPostOverHaulCheck_CheckedChanged(null, null);
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();                       
            if (ViewState["OPERATIONMODE"].ToString() == "EDIT") toolbarmain.AddButton("Generate WO", "GWO", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuComponentGeneral.AccessRights = this.ViewState;  
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
            if ((Request.QueryString["COMPONENTJOBID"] != null) && (Request.QueryString["COMPONENTJOBID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(Request.QueryString["COMPONENTJOBID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];

                txtJobId.Text = dr["FLDJOBID"].ToString();
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
                //txtWindowsHrs.Text = dr["FLDWINDOWHOURS"].ToString(); 
                //if (dr["FLDCOUNTERTYPE"].ToString() != "")
                {

                    //ucCounterType.Enabled = true;
                    //ucCounterFrequency.ReadOnly = false;
                    //txtLastDoneHours.ReadOnly = false;
                    //ucNextDueHours.Enabled = true;
                    ucCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
                    ucCounterFrequency.Text = dr["FLDCOUNTERVALUES"].ToString();
                    string ldh = dr["FLDLASTDONEHOURS"].ToString();
                    txtLastDoneHours.Text = ldh != string.Empty && ldh.IndexOf('.') > -1 ? ldh.Substring(0, ldh.IndexOf('.')) : string.Empty;
                    ucNextDueHours.Text = dr["FLDNEXTDUEHOURS"].ToString();
                    txtAverage.Text = dr["FLDAVERAGE"].ToString();
                    txtCounterDate.Text = dr["FLDREADINGDATE"].ToString();
                    txtCounterValue.Text = dr["FLDREADINGVALUE"].ToString();
                }
                //else
                //{
                //    ucCounterType.Enabled  = false;
                //    ucCounterFrequency.Enabled  = false;
                //    txtLastDoneHours.Enabled  = false;
                //    ucNextDueHours.Enabled  = false;
                //}


                txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                txtJobName.Text = dr["FLDJOBTITLE"].ToString();
                txtPriority.Text = dr["FLDPRIORITY"].ToString();
                txtReference.Text = dr["FLDREFERENCE"].ToString();
                ViewState["WORKORDERID"] = dr["FLDWORKORDERID"].ToString();
                if (dr["FLDMANDATORY"].ToString().Equals("Y"))
                    ucMandatory.YesNoOption = "Yes";
                else
                    ucMandatory.YesNoOption = "No";
                foreach (ListItem li in cblJobStatus.Items)
                {
                    li.Selected = ("," + dr["FLDJOBSTATUS"].ToString() + ",").Contains("," + li.Value + ",") ? true : false;
                }
                chkCheckList.Checked = dr["FLDCHECKLIST"].ToString() == "1" ? true : false;
                chkRAMandatory.Checked = dr["FLDISRAMANDATORY"].ToString() == "1" ? true : false;
                chkAttachment.Checked = dr["FLDATTACHMENTREQUIRED"].ToString() == "1" ? true : false;
                txtInstructions.Visible = dr["FLDATTACHMENTREQUIRED"].ToString() == "1" ? true : false;
                txtInstructions.Text = dr["FLDATTINSTRUCTIONS"].ToString();
                ucRank.SelectedRank = dr["FLDVSLVERIFYREQUIRED"].ToString();
                chkSupntVerification.Checked = dr["FLDSUPNTVERIFYREQUIRED"].ToString() == "1" ? true : false;
                chkPostOverHaulCheck.Checked = dr["FLDISPOSTOVERHAULCHECK"].ToString() == "1";
                rblInterval.SelectedValue = dr["FLDOVERHAULINTERVAL"].ToString();
                txtDetilsofCheck.Text = dr["FLDPOSTOVERHAULCHECKDESC"].ToString();
                txtInterValue.Text = dr["FLDOVERHAULINTERVALVALUE"].ToString();
                DataTable dt = PhoenixPlannedMaintenanceComponentJob.compJobRAEdit(new Guid(Request.QueryString["COMPONENTJOBID"]));
                if (dt.Rows.Count > 0)
                {
                    DataRow drr = dt.Rows[0];
                    txtRAId.Text = drr["FLDRAID"].ToString();
                    txtRANumber.Text = drr["FLDRAREFNO"].ToString();
                    txtRA.Text = drr["FLDRISKASSESSMENT"].ToString();
                    txtRaType.Text = drr["FLDTYPE"].ToString();
                }
                ViewState["OPERATIONMODE"] = "EDIT";
            }
            else
            {
                txtPriority.Text = "3";
                ucMandatory.YesNoOption = "Yes";
                ucHistory.SelectedHistoryTemplate = "1";
                txtNextDueDate.Text = string.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                foreach (ListItem li in cblJobStatus.Items) li.Selected = (li.Value == "35" ? true : false);
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidComponent(txtJobId.Text, txtFrequency.Text, ucCounterType.SelectedHard, ucCounterFrequency.Text, txtPriority.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string jobstatus = string.Empty;
                foreach (ListItem li in cblJobStatus.Items)
                {
                    jobstatus += (li.Selected ? li.Value + "," : string.Empty);
                }
                jobstatus = jobstatus.TrimEnd(',');

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixPlannedMaintenanceComponentJob.UpdateComponentJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , new Guid(Request.QueryString["COMPONENTJOBID"]), General.GetNullableGuid(txtJobId.Text).Value
                    , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(ucHistory.SelectedHistoryTemplate)
                    , General.GetNullableInteger(ucPlanningMethod.SelectedHard), General.GetNullableInteger(txtPriority.Text)
                    , General.GetNullableInteger(txtWindow.Text), txtlastDoneDate.SelectedDate
                    , General.GetNullableInteger(ucMainType.SelectedQuick)
                    , General.GetNullableInteger(ucMaintClass.SelectedQuick), General.GetNullableInteger(ucMainCause.SelectedQuick)
                    , General.GetNullableDecimal(txtDuration.Text), ucMandatory.YesNoOption
                    , General.GetNullableInteger(ucCounterType.SelectedHard), General.GetNullableInteger(ucCounterFrequency.Text)
                    , General.GetNullableDecimal(txtLastDoneHours.Text)
                    , null/*General.GetNullableInteger(txtWindowsHrs.Text)*/
                    , jobstatus, txtReference.Text
                    , General.GetNullableInteger(chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value ? "1" : "0")
                    , txtDetilsofCheck.Text
                    , General.GetNullableInteger(rblInterval.SelectedValue)
                    , General.GetNullableInteger(txtInterValue.Text)
                    );
                }
                else if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixPlannedMaintenanceComponentJob.InsertComponentJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(txtJobId.Text), General.GetNullableGuid(Request.QueryString["COMPONENTID"])
                    , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(ucHistory.SelectedHistoryTemplate)
                    , General.GetNullableInteger(ucPlanningMethod.SelectedHard), General.GetNullableInteger(txtPriority.Text)
                    , General.GetNullableInteger(txtWindow.Text), txtlastDoneDate.SelectedDate
                    , General.GetNullableInteger(ucMainType.SelectedQuick)
                    , General.GetNullableInteger(ucMaintClass.SelectedQuick), General.GetNullableInteger(ucMainCause.SelectedQuick)
                    , General.GetNullableDecimal(txtDuration.Text), ucMandatory.YesNoOption
                    , General.GetNullableInteger(ucCounterType.SelectedHard), General.GetNullableInteger(ucCounterFrequency.Text)
                    , General.GetNullableDecimal(txtLastDoneHours.Text)
                    , null/*General.GetNullableInteger(txtWindowsHrs.Text)*/ , jobstatus, General.GetNullableString(txtInstructions.Text)
                    , General.GetNullableInteger(chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value ? "1" : "0")
                    , txtDetilsofCheck.Text
                    , General.GetNullableInteger(rblInterval.SelectedValue)
                    , General.GetNullableInteger(txtInterValue.Text));
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            else if (CommandName.ToUpper().Equals("GWO"))
            {
                PhoenixPlannedMaintenanceWorkOrder.InsertWorkOrderAutomatic(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(Request.QueryString["COMPONENTJOBID"])
                    , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()));
                ucCounterFrequency_TextChanged(null, null);
                ucStatus.Text = "Work Order Generated";
                BindFields();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                txtJobId.Text = "";
                txtFrequency.Text = "";
                ucFrequency.SelectedHard = "";
                ucDiscipline.SelectedDiscipline = "";
                ucHistory.SelectedHistoryTemplate = "";
                txtWindow.Text = "";
                txtlastDoneDate.SelectedDate = null;
                txtNextDueDate.Text = string.Format("{0:dd/MMM/yyyy}", DateTime.Now);
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
                txtLastDoneHours.Text = "";
                ucNextDueHours.Text = "";
                txtAverage.Text = "";
                txtCounterDate.Text = "";
                txtCounterValue.Text = "";
                ucMandatory.YesNoOption = "Yes";
                ucHistory.SelectedHistoryTemplate = "1";
                foreach (ListItem li in cblJobStatus.Items) li.Selected = (li.Value == "35" ? true : false);
                //txtWindowsHrs.Text = "";
                txtReference.Text = "";
                txtInterValue.Text = "";
                rblInterval.SelectedIndex = -1;
                txtDetilsofCheck.Text = "";
                chkPostOverHaulCheck.Checked = false;
                ViewState["OPERATIONMODE"] = "ADD";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidComponent(string job, string frequency, string countertype, string countervalue, string piriority)
    {
        ucError.HeaderMessage = "Please provide the following required information";

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

        if (txtlastDoneDate.SelectedDate.HasValue && DateTime.Compare(txtlastDoneDate.SelectedDate.Value, DateTime.Now) > 0)
            ucError.ErrorMessage = "Last Done Date should be earlier than Current Date.";
        if (chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value)
        {
            if (General.GetNullableInteger(txtInterValue.Text) == null)
                ucError.ErrorMessage = "Interval after the Overhaul is required.";

            if (General.GetNullableInteger(rblInterval.SelectedValue) == null)
                ucError.ErrorMessage = "Interval is required.";

            if (General.GetNullableString(txtDetilsofCheck.Text) == null)
                ucError.ErrorMessage = "Details of Check is required.";
        }
        return (!ucError.IsError);
    }

    protected void ucCounterType_OnTextChangedEvent(object sender, EventArgs e)
    {
        if (ucCounterType.SelectedHard != "Dummy" && ucCounterType.SelectedHard != "")
        {
            if (ViewState["OPERATIONMODE"].ToString() == "ADD")
            {
                DataTable dt = PhoenixPlannedMaintenanceComponentCounters.GetCounterRunningHours(new Guid(Request.QueryString["COMPONENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dt.Rows.Count > 0)
                    txtLastDoneHours.Text = dt.Rows[0][0].ToString();
            }
            CalculateNextDueHours();
            CalculateNextDueDate();
        }
    }

    protected void LastDoneDateCalculation(object sender, EventArgs e)
    {
        CalculateNextDueDate();
    }
    protected void ucCounterFrequency_TextChanged(object sender, EventArgs e)
    {
        CalculateNextDueHours();
        if (txtlastDoneDate.SelectedDate.HasValue)
            CalculateNextDueDate();
    }
    private void CalculateNextDueHours()
    {
        decimal? lastdonehours = General.GetNullableDecimal(txtLastDoneHours.Text);
        if (ucCounterFrequency.Text.Equals(""))
            ucCounterFrequency.Text = "0";
        decimal? countervalue = General.GetNullableDecimal(ucCounterFrequency.Text);
        if (lastdonehours.HasValue)
            ucNextDueHours.Text = Convert.ToString(lastdonehours + countervalue);
    }
    private void CalculateNextDueDate()
    {
        DateTime? calendarBasedNextDueDate = null;
        DateTime? counterBasedNextDueDate = null;
        if (txtlastDoneDate.SelectedDate.HasValue)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(ucFrequency.SelectedHard))
                str = ucFrequency.SelectedName.Substring(0, 1);

            int intfrequency = 0;
            if (txtFrequency.Text == "0" || txtFrequency.Text == "")
            {
                calendarBasedNextDueDate = null;
            }
            else
            {
                if (!txtFrequency.Text.Equals(""))
                    intfrequency = int.Parse(txtFrequency.Text);

                if (str.ToUpper().Equals("M"))
                    calendarBasedNextDueDate = txtlastDoneDate.SelectedDate.Value.AddMonths(intfrequency);
                else if (str.ToUpper().Equals("W"))
                    calendarBasedNextDueDate = txtlastDoneDate.SelectedDate.Value.AddDays(intfrequency * 7);
                else if (str.ToUpper().Equals("D"))
                    calendarBasedNextDueDate = txtlastDoneDate.SelectedDate.Value.AddDays(intfrequency);
            }

        }
        if (General.GetNullableInteger(ucCounterType.SelectedHard) != null && (ucCounterFrequency.Text != "0" || ucCounterFrequency.Text.Trim().Equals("")))
        {
            decimal? countervalue;
            decimal? averagecountervalue;
            DateTime? couterDate;
            int days;
            countervalue = General.GetNullableDecimal(ucCounterFrequency.Text);
            averagecountervalue = General.GetNullableDecimal(txtAverage.Text);
            couterDate = General.GetNullableDateTime(txtCounterDate.Text);
            if (averagecountervalue != null && averagecountervalue != 0)
            {
                int nxtduehrs = 0;
                int.TryParse(ucNextDueHours.Text, out nxtduehrs);
                days = Convert.ToInt32((countervalue.Value) / averagecountervalue.Value);
                if (txtlastDoneDate.SelectedDate.HasValue)
                    counterBasedNextDueDate = txtlastDoneDate.SelectedDate.Value.AddDays(days);
            }
        }
        if (calendarBasedNextDueDate != null && counterBasedNextDueDate != null)
        {
            if (calendarBasedNextDueDate < counterBasedNextDueDate)
                txtNextDueDate.Text = General.GetDateTimeToString(calendarBasedNextDueDate.ToString());
            else
                txtNextDueDate.Text = General.GetDateTimeToString(counterBasedNextDueDate.ToString());
        }
        else if (counterBasedNextDueDate == null)
            txtNextDueDate.Text = General.GetDateTimeToString(calendarBasedNextDueDate.ToString());
        else
            txtNextDueDate.Text = General.GetDateTimeToString(counterBasedNextDueDate.ToString());
    }

    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        txtJobCode.Text = "";
        txtJobName.Text = "";
        txtJobId.Text = "";
    }
    protected void cmdRA_Click(object sender, ImageClickEventArgs e)
    {
        if (txtRAId.Text != "")
        {
            string scriptpopup = string.Format("javascript:parent.Openpopup('codehelp1', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + txtRAId.Text + "&showmenu=0&showexcel=NO');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
        }
    }
    protected void chkAttachment_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAttachment.Checked == true)
        {
            txtInstructions.Visible = true;
        }
        else txtInstructions.Visible = false;
    }
    protected void chkPostOverHaulCheck_CheckedChanged(object sender, EventArgs e)
    {
        txtDetilsofCheck.Enabled = false;
        rblInterval.Enabled = false;
        lblIntervalValue.Enabled = false;
        if (chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value)
        {
            txtDetilsofCheck.Enabled = true;
            rblInterval.Enabled = true;
            lblIntervalValue.Enabled = true;
        }
    }

    protected void txtlastDoneDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        CalculateNextDueDate();
    }
}
