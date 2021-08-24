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

public partial class PlannedMaintenanceComponentJobGeneralOffice : PhoenixBasePage
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
                ViewState["VESSELID"] = null;
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
                else
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["COMPONENTID"]))
                {
                    ViewState["OPERATIONMODE"] = "EDIT";
                }
                BindFields();
                chkPostOverHaulCheck_CheckedChanged(null, null);
                imgShowRA.Attributes.Add("onclick", "return showPickList('spnRA', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListMachineryRA.aspx?catid=3&vesselid=" + ViewState["VESSELID"].ToString() + "&status=4,5', true); ");
                imgJob.Attributes.Add("onclick", "return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] +"/Common/CommonPickListJob.aspx', true); ");
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (ViewState["OPERATIONMODE"].ToString() == "EDIT")
            {
                toolbarmain.AddButton("Generate Job", "GWO", ToolBarDirection.Right);
            }
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

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["COMPONENTJOBID"] != null) && (Request.QueryString["COMPONENTJOBID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceComponentJob.EditComponentJob(new Guid(Request.QueryString["COMPONENTJOBID"]), int.Parse( ViewState["VESSELID"].ToString()));
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
                //ucHistory.SelectedHistoryTemplateName = "SIMPLE JOBS TEMPLATE";
                txtNextDueDate.Text = string.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                foreach (ButtonListItem li in cblJobStatus.Items) li.Selected = (li.Value == "35" ? true : false);
                ucPlanningMethod.SelectedHard = "10"; //Variable
                ViewState["OPERATIONMODE"] = "ADD";
            }
            if (txtRAId.Text == "")
            {
                cmdRA.Visible = false;
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
                foreach (ButtonListItem li in cblJobStatus.Items)
                {
                    jobstatus += (li.Selected ? li.Value + "," : string.Empty);
                }
                jobstatus = jobstatus.TrimEnd(',');

                int chklist = chkCheckList.Checked == true ? 1 : 0;

                int ChkRa = chkRAMandatory.Checked == true ? 1 : 0;
                int ChkAtt = chkAttachment.Checked == true ? 1 : 0;
                int? VslVerification = General.GetNullableInteger(ucRank.SelectedRank);
                int? SupntVerification = chkSupntVerification.Checked == true ? 1 : General.GetNullableInteger("");

                if(SupntVerification == 1 && VslVerification ==  null)
                {
                    ucError.ErrorMessage = "Vessel verification is Required";
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixPlannedMaintenanceComponentJobChecklist.UpdateComponentJobRevision(int.Parse(ViewState["VESSELID"].ToString())
                    , new Guid(Request.QueryString["COMPONENTJOBID"])
                    , General.GetNullableGuid(txtJobId.Text).Value
                    , General.GetNullableInteger(txtFrequency.Text)
                    , General.GetNullableInteger(ucFrequency.SelectedHard)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline)
                    , General.GetNullableInteger(ucHistory.SelectedHistoryTemplate)
                    , General.GetNullableInteger(ucPlanningMethod.SelectedHard)
                    , General.GetNullableInteger(txtPriority.Text)
                    , General.GetNullableInteger(txtWindow.Text)
                    , txtlastDoneDate.SelectedDate
                    , General.GetNullableInteger(ucMainType.SelectedQuick)
                    , General.GetNullableInteger(ucMaintClass.SelectedQuick)
                    , General.GetNullableInteger(ucMainCause.SelectedQuick)
                    , General.GetNullableDecimal(txtDuration.Text)
                    , ucMandatory.YesNoOption
                    , General.GetNullableInteger(ucCounterType.SelectedHard)
                    , General.GetNullableInteger(ucCounterFrequency.Text)
                    , General.GetNullableDecimal(txtLastDoneHours.Text)
                    , null/*General.GetNullableInteger(txtWindowsHrs.Text)*/
                    , jobstatus
                    , General.GetNullableString(txtInstructions.Text)
                    , null
                    , chklist
                    , ChkRa
                    , General.GetNullableGuid(txtRAId.Text)
                    , ChkAtt
                    , VslVerification
                    , SupntVerification
                    , General.GetNullableString(txtInstructions.Text)
                    , General.GetNullableInteger(chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value ? "1" : "0")
                    , txtDetilsofCheck.Text
                    , General.GetNullableInteger(rblInterval.SelectedValue)
                    , General.GetNullableInteger(txtInterValue.Text)
                    );
                }
                else if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixPlannedMaintenanceComponentJobChecklist.InsertComponentJob(int.Parse(ViewState["VESSELID"].ToString())
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
                    , null/*General.GetNullableInteger(txtWindowsHrs.Text)*/
                    , jobstatus, txtReference.Text, null, chklist, ChkRa, General.GetNullableGuid(txtRAId.Text), ChkAtt
                    , VslVerification, SupntVerification, General.GetNullableString(txtInstructions.Text)
                    , General.GetNullableInteger(chkPostOverHaulCheck.Checked.HasValue && chkPostOverHaulCheck.Checked.Value ? "1" : "0")
                    , txtDetilsofCheck.Text
                    , General.GetNullableInteger(rblInterval.SelectedValue)
                    , General.GetNullableInteger(txtInterValue.Text)
                    );
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            else if (CommandName.ToUpper().Equals("GWO"))
            {
                if (ConfigurationManager.AppSettings.Get("PhoenixTelerik") != null && ConfigurationManager.AppSettings.Get("PhoenixTelerik").ToString() == "1")
                {
                    PhoenixPlannedMaintenanceWorkOrder.InsertNewWorkOrderAutomatic(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Request.QueryString["COMPONENTJOBID"])
                        , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()));
                }
                else
                {
                    PhoenixPlannedMaintenanceWorkOrder.InsertWorkOrderAutomatic(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Request.QueryString["COMPONENTJOBID"])
                    , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()));
                }
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
                //ucHistory.SelectedHistoryTemplateName = "SIMPLE JOBS TEMPLATE";
                foreach (ButtonListItem li in cblJobStatus.Items) li.Selected = (li.Value == "35" ? true : false);
                //txtWindowsHrs.Text = "";
                txtReference.Text = "";
                txtRANumber.Text = "";
                txtRA.Text = "";
                txtRAId.Text = "";
                txtRaType.Text = "";
                chkRAMandatory.Checked = false;
                ViewState["OPERATIONMODE"] = "ADD";
                chkSupntVerification.Checked = false;
                ucRank.SelectedRank = "";
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

        if (General.GetNullableInteger(frequency).HasValue && General.GetNullableInteger(frequency).Value > 0 && !General.GetNullableInteger(ucFrequency.SelectedHard).HasValue)
            ucError.ErrorMessage = "Frequency is required";

        if ((frequency.Equals("") || frequency.Equals("0")) && (countertype.Equals("Dummy") || countervalue == "" || countervalue == "0"))
            //if ((frequency.Equals("") || frequency.Equals("0")) && (countertype.Equals("Dummy")))
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
                DataTable dt = PhoenixPlannedMaintenanceComponentCounters.GetCounterRunningHours(new Guid(Request.QueryString["COMPONENTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
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
        if (txtlastDoneDate.SelectedDate != null)
        {
            CalculateNextDueHours();
            //if (!txtlastDoneDate.Text.Trim().Equals(""))
                CalculateNextDueDate();
        }
    }
    private void CalculateNextDueHours()
    {
        Decimal? lastdonehours = null;
        Decimal? countervalue = null;
        lastdonehours = General.GetNullableDecimal(txtLastDoneHours.Text);
        if (ucCounterFrequency.Text.Equals(""))
            ucCounterFrequency.Text = "0";
        countervalue = General.GetNullableDecimal(ucCounterFrequency.Text);
        if (lastdonehours.HasValue)
            ucNextDueHours.Text = Convert.ToString(lastdonehours + countervalue);
    }
    private void CalculateNextDueDate()
    {
        DateTime? calendarBasedNextDueDate = null;
        DateTime? counterBasedNextDueDate = null;

        string LastDoneDate = General.GetDateTimeToString(txtlastDoneDate.SelectedDate);

        if (!LastDoneDate.Equals(""))
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
        txtFrequency.Text = "";
        ucFrequency.SelectedHard = "";
        ucCounterType.SelectedHard = "";
        ucCounterFrequency.Text = "0";
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (txtJobId.Text != "" && ViewState["OPERATIONMODE"].ToString() == "ADD")
        {
            DataSet ds = PhoenixPlannedMaintenanceJob.EditJob(new Guid(txtJobId.Text));
            DataRow dr = ds.Tables[0].Rows[0];
            ucFrequency.SelectedHard = dr["FLDFREQUENCYTYPE"].ToString();
            txtFrequency.Text = dr["FLDFREQUENCY"].ToString();
            ucCounterType.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
            ucCounterFrequency.Text = dr["FLDCOUNTERVALUES"].ToString();
            ucRank.SelectedRank = dr["FLDVESSELVERIFYREQUIRED"].ToString();
            chkSupntVerification.Checked = dr["FLDOFFICEVERIFYREQUIRED"].ToString() == "1" ? true : false;

            if (ucCounterFrequency.Text == "")
                ucCounterFrequency.Text = "0";
        }

        if (txtRAId.Text == "")
            cmdRA.Visible = false;
        else
        {
            cmdRA.Visible = true;
            chkRAMandatory.Checked = true;
        }
    }

    protected void imgShowRA_Click(object sender, ImageClickEventArgs e)
    {
        if (!IsValidComponent(txtJobId.Text, txtFrequency.Text, ucCounterType.SelectedHard, ucCounterFrequency.Text, txtPriority.Text))
        {
            ucError.Visible = true;
            return;
        }
        //string script = string.Format("javascript:showPickList('spnRA', 'codehelp1', '', '../Common/CommonPickListMachineryRACopyforPmsJob.aspx?catid=3&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&status=4,5&ComponentId=" + ViewState["COMPONENTID"].ToString()+"&JobId="+ txtJobId.Text +"', true);");
        string script = string.Format("javascript:showPickList('spnRA', 'codehelp1', '', '../Common/CommonPickListMachineryRACopyforPmsJob.aspx?framename=ifMoreInfo&catid=3&vesselid=0&status=3&ComponentId=" + ViewState["COMPONENTID"].ToString() + "&JobId=" + txtJobId.Text + "', true);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void cmdRA_Click(object sender, ImageClickEventArgs e)
    {
        if (txtRAId.Text != "")
        {
            string scriptpopup = string.Format("javascript:parent.Openpopup('codehelp1', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + txtRAId.Text + "&showmenu=0&showexcel=NO');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
        }
    }

    protected void txtlastDoneDate_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
    {
        CalculateNextDueDate();
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
}
