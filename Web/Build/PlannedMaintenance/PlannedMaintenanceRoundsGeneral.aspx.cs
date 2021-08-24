using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceRoundsGeneral : PhoenixBasePage
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
                ucFrequency.HardTypeCode = ((int)(PhoenixHardTypeCode.PERIODICFREQUENCY)).ToString();// "7";

                cblJobStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 13, 0, "NON,AVA,IUE,REP");
                cblJobStatus.DataBind();
                ViewState["WORKORDERID"] = "";


                if (Session["JOBMODE"] != null)
                {
                    if (Session["JOBMODE"].Equals("ViewJob"))
                    {
                        ViewState["OPERATIONMODE"] = "EDIT";
                        BindFields();
                    }
                    else
                    {
                        txtPriority.Text = "3";
                        txtNextDueDate.Text = string.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                        foreach (ButtonListItem li in cblJobStatus.Items) li.Selected = (li.Value == "35" ? true : false);
                        ucPlanningMethod.SelectedHard = "10"; //Variable
                        ViewState["OPERATIONMODE"] = "ADD";
                    }
                    Session.Remove("JOBMODE");
                }
                else
                {
                    BindFields();
                }
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                if (ViewState["OPERATIONMODE"].ToString() == "EDIT")
                {
                    toolbarmain.AddButton("Gen. Work Order", "GWO", ToolBarDirection.Right);
                }
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuComponentGeneral.MenuList = toolbarmain.Show();
            }

            imgJob.Attributes.Add("onclick", "javascript:return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJob.aspx', true);");
            
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

                txttitle.Text = dr["FLDTITLE"].ToString();
                txtJobId.Text = dr["FLDJOBID"].ToString();
                txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                txtJobName.Text = dr["FLDJOBTITLE"].ToString();                
                txtFrequency.Text = dr["FLDJOBFREQUENCY"].ToString();
                ucFrequency.SelectedHard = dr["FLDJOBFREQUENCYTYPE"].ToString();
                ucDiscipline.SelectedDiscipline = dr["FLDJOBDISCIPLINE"].ToString();
                //ucHistory.SelectedHistoryTemplate = dr["FLDHISTORYTEMPLATEID"].ToString();
                txtWindow.Text = dr["FLDWINDOW"].ToString();
                txtlastDoneDate.Text = General.GetDateTimeToString(dr["FLDJOBLASTDONEDATE"].ToString());
                txtNextDueDate.Text = General.GetDateTimeToString(dr["FLDJOBNEXTDUEDATE"].ToString());
                ucPlanningMethod.SelectedHard = dr["FLDPLANINGMETHOD"].ToString();
                txtDuration.Text = dr["FLDJOBDURATION"].ToString();
                ucMainType.SelectedQuick = dr["FLDMAINTNANCETYPE"].ToString();
                ucMaintClass.SelectedQuick = dr["FLDMAINTNANCECLASS"].ToString();
                ucMainCause.SelectedQuick = dr["FLDMAINTNANCECAUSE"].ToString();               
                txtPriority.Text = dr["FLDPRIORITY"].ToString();
                ViewState["WORKORDERID"] = dr["FLDWORKORDERID"].ToString();
                if (dr["FLDMANDATORY"].ToString().Equals("1"))
                    ucMandatory.YesNoOption = "Yes";
                else
                    ucMandatory.YesNoOption = "No";
                foreach (ButtonListItem li in cblJobStatus.Items)
                {
                    li.Selected = ("," + dr["FLDJOBSTATUS"].ToString() + ",").Contains("," + li.Value + ",") ? true : false;
                }

                ViewState["OPERATIONMODE"] = "EDIT";
            }
            else
            {
                txtPriority.Text = "3";
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRounds(txtJobId.Text, txtFrequency.Text, txtPriority.Text, ucDiscipline.SelectedDiscipline,txttitle.Text))
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

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixPlannedMaintenanceComponentJob.UpdateComponentJobRevision(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , new Guid(Request.QueryString["COMPONENTJOBID"]), General.GetNullableGuid(txtJobId.Text).Value
                    , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null //General.GetNullableInteger(ucHistory.SelectedHistoryTemplate)
                    , General.GetNullableInteger(ucPlanningMethod.SelectedHard), General.GetNullableInteger(txtPriority.Text)
                    , General.GetNullableInteger(txtWindow.Text), General.GetNullableDateTime(txtlastDoneDate.Text)
                    , General.GetNullableInteger(ucMainType.SelectedQuick)
                    , General.GetNullableInteger(ucMaintClass.SelectedQuick), General.GetNullableInteger(ucMainCause.SelectedQuick)
                    , General.GetNullableDecimal(txtDuration.Text), (ucMandatory.YesNoOption.Equals("YES") ? "1" : (ucMandatory.YesNoOption.Equals("NO") ? "0" : string.Empty))
                    , null, null
                    , null
                    , null/*General.GetNullableInteger(txtWindowsHrs.Text)*/
                    , jobstatus, txtReference.Text, txttitle.Text.Trim());
                }
                else if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixPlannedMaintenanceRounds.InsertComponentJob(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(txtJobId.Text), null
                    , General.GetNullableInteger(txtFrequency.Text), General.GetNullableInteger(ucFrequency.SelectedHard)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null //General.GetNullableInteger(ucHistory.SelectedHistoryTemplate)
                    , General.GetNullableInteger(ucPlanningMethod.SelectedHard), General.GetNullableInteger(txtPriority.Text)
                    , General.GetNullableInteger(txtWindow.Text), General.GetNullableDateTime(txtlastDoneDate.Text)
                    , General.GetNullableInteger(ucMainType.SelectedQuick)
                    , General.GetNullableInteger(ucMaintClass.SelectedQuick), General.GetNullableInteger(ucMainCause.SelectedQuick)
                    , General.GetNullableDecimal(txtDuration.Text), (ucMandatory.YesNoOption.Equals("Y") ? "1" : (ucMandatory.YesNoOption.Equals("N") ? "0" : string.Empty))
                    , null, null
                    , null
                    , null, jobstatus, txtReference.Text,txttitle.Text.Trim());
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            else if (CommandName.ToUpper().Equals("GWO"))
            {
                PhoenixPlannedMaintenanceWorkOrder.InsertWorkOrderAutomatic(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , new Guid(Request.QueryString["COMPONENTJOBID"])
                                                                            , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()));                
                BindFields();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                txttitle.Text = "";
                txtFrequency.Text = "";
                ucFrequency.SelectedHard = "";
                ucDiscipline.SelectedDiscipline = "";
                //ucHistory.SelectedHistoryTemplate = "";
                txtWindow.Text = "";
                txtlastDoneDate.Text = "";
                txtNextDueDate.Text = string.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                ucPlanningMethod.SelectedHard = "10"; //Variable
                txtDuration.Text = "";
                ucMainType.SelectedQuick = "";
                ucMaintClass.SelectedQuick = "";
                ucMainCause.SelectedQuick = "";
                txtJobCode.Text = "";
                txtJobId.Text = "";
                txtJobName.Text = "";
                txtPriority.Text = "3";               
                foreach (ButtonListItem li in cblJobStatus.Items) li.Selected = (li.Value == "35" ? true : false);
                //txtWindowsHrs.Text = "";
                ViewState["OPERATIONMODE"] = "ADD";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidRounds(string jobid, string frequency, string piriority, string responsibility,string title)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required";

        if (jobid.Trim().Equals(""))
            ucError.ErrorMessage = "Job is required";
       
        if ((frequency.Equals("") || frequency.Equals("0")))
            ucError.ErrorMessage = "Frequency needs to be specified";

        if (piriority.Trim().Equals("") || (int.Parse(piriority) > 3 || int.Parse(piriority) < 1))
            ucError.ErrorMessage = "Job Priority is required(1-3)";

        if (!General.GetNullableInteger(ucPlanningMethod.SelectedHard).HasValue)
            ucError.ErrorMessage = "Planning Method is requierd";

        if (!General.GetNullableInteger(responsibility).HasValue)
            ucError.ErrorMessage = "Responsibility is requierd";

        return (!ucError.IsError);
    }

    protected void LastDoneDateCalculation(object sender, EventArgs e)
    {
        CalculateNextDueDate();
    }
    
    private void CalculateNextDueDate()
    {
        DateTime? calendarBasedNextDueDate = null;
        DateTime? counterBasedNextDueDate = null;
        if (txtlastDoneDate.Text != null)
        {
            if (!txtlastDoneDate.Text.Equals(""))
            {
                string str = ucFrequency.SelectedName.Substring(0, 1);
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
                        calendarBasedNextDueDate = Convert.ToDateTime(txtlastDoneDate.Text).AddMonths(intfrequency);
                    else if (str.ToUpper().Equals("W"))
                        calendarBasedNextDueDate = Convert.ToDateTime(txtlastDoneDate.Text).AddDays(intfrequency * 7);
                    else if (str.ToUpper().Equals("D"))
                        calendarBasedNextDueDate = Convert.ToDateTime(txtlastDoneDate.Text).AddDays(intfrequency);
                }

            }
        }
        
        if (calendarBasedNextDueDate != null && counterBasedNextDueDate != null)
        {
            if (calendarBasedNextDueDate < counterBasedNextDueDate)
                txtNextDueDate.Text = General.GetDateTimeToString(calendarBasedNextDueDate.ToString());
            else
                txtNextDueDate.Text = General.GetDateTimeToString(counterBasedNextDueDate.ToString());
        }
        else if (calendarBasedNextDueDate != null)
            txtNextDueDate.Text = General.GetDateTimeToString(calendarBasedNextDueDate.ToString());
        else
            txtNextDueDate.Text = General.GetDateTimeToString(counterBasedNextDueDate.ToString());
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (txtJobId.Text != "")
        {
            DataSet ds = PhoenixPlannedMaintenanceJob.EditJob(new Guid(txtJobId.Text));
            DataRow dr = ds.Tables[0].Rows[0];
            ucFrequency.SelectedHard = dr["FLDFREQUENCYTYPE"].ToString();
            txtFrequency.Text = dr["FLDFREQUENCY"].ToString();
           
        }
    }

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtJobCode.Text = "";
        txtJobName.Text = "";
        txtJobId.Text = "";
        txtFrequency.Text = "";
        ucFrequency.SelectedHard = "";
    }
}
