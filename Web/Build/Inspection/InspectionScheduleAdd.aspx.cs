using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class Inspection_InspectionScheduleAdd : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucInspectionType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS");

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddButton("Save", "SAVE");
                MenuInspectionSchedule.MenuList = toolbarsub.Show();

                if (Request.QueryString["INSPECTIONSCHEDULEID"] != null && Request.QueryString["INSPECTIONSCHEDULEID"].ToString() != string.Empty)
                    ViewState["INSPECTIONSCHEDULEID"] = Request.QueryString["INSPECTIONSCHEDULEID"].ToString();
                else
                    Reset();

                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                BindShortCodeList();
                //BindInspectionSchedule();
                BindInternalInspector();
                BindInternalAuditor();
                BindExternalOrganisation();
                BindExternalInspector();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindInspectionSchedule()
    {
        if (ViewState["INSPECTIONSCHEDULEID"] != null && ViewState["INSPECTIONSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["INSPECTIONSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;

                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                ucInspectionType.SelectedHard = dr["FLDINSPECTIONTYPEID"].ToString();
                ucInspectionCategory.SelectedHard = dr["FLDINSPECTIONCATEGORYID"].ToString();
                ddlInspectionShortCodeList.SelectedValue = dr["FLDINSPECTIONID"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                if (dr["FLDINSPECTIONCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                    txtWindowperiod.CssClass = "input";
                else
                    txtWindowperiod.CssClass = "input_mandatory";
                txtLastDoneDate.Text = General.GetDateTimeToString(dr["FLDLASTDONEDATE"].ToString());
                ucDoneType.SelectedHard = dr["FLDISFIXED"].ToString();
                txtWindowperiod.Text = dr["FLDWINDOWPERIOD"].ToString();
                txtDueDate.Text = General.GetDateTimeToString(dr["FLDINSPECTIONSTARTDATE"].ToString());
                if (General.GetNullableString(dr["FLDLASTDONEDATE"].ToString()) == null) //&& rdoAutomatic.Checked
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDINSPECTIONSTARTDATE"].ToString());
                ucInspectionType.Enabled = ucDoneType.Enabled = false;
            }
        }
    }


    protected void MenuInspectionSchedule_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                DataTable dt = new DataTable();
                //double frequency;
                //DateTime? duedate = null;
                String script = String.Format("javascript:fnReloadList('codehelp1');");

                if (!IsValidInspectionSchedule())
                {
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidReSchedule())
                {
                    ucError.Visible = true;
                    return;
                }
                
                if (ViewState["INSPECTIONSCHEDULEID"] == null)
                {
                    PhoenixInspectionSchedule.InsertInspectionScheduleDetails(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ddlInspectionShortCodeList.SelectedValue)
                        , Int16.Parse(ucVessel.SelectedVessel)
                        , General.GetNullableDateTime(txtLastDoneDate.Text)
                        , General.GetNullableDateTime(txtDueDate.Text)
                        , General.GetNullableInteger(txtWindowperiod.Text)
                        , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                        , General.GetNullableInteger(ucPort.SelectedSeaport)
                        , General.GetNullableDateTime(txtETA.Text)
                        , General.GetNullableDateTime(txtETD.Text)
                        ,null
                        , General .GetNullableInteger (ddlInspectorName.SelectedValue)
                        , General.GetNullableGuid(ddlExternalInspectorName.SelectedValue)
                        , General .GetNullableInteger (lblExternalOrganisationId .Text)
                        , General .GetNullableInteger (ddlAuditorName.SelectedValue)
                        , General.GetNullableInteger(ucFromPort.SelectedSeaport)
                        , General.GetNullableInteger(ucToPort.SelectedSeaport)
                        , General .GetNullableInteger (ucLastPort .SelectedSeaport)
                        , General.GetNullableString(txtRemarks.Text)
                        , General.GetNullableDateTime(txtDateRangeFrom.Text)
                        , General.GetNullableInteger(rblLocation.SelectedValue)
                        );

                    ucStatus.Text = "Inspection is scheduled successfully.";
                    Session["NewSchedule"] = "Y";
                    String scriptinsert = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                }
                else
                {
                
                    //String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInspectionSchedule()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        //string timeofincident = txtTimeOfIncident.Text.Trim() == "__:__" ? string.Empty : txtTimeOfIncident.Text;       

        if (ucInspectionType.SelectedHard.Equals("") || ucInspectionType.SelectedHard.Equals("Dummy"))
            ucError.ErrorMessage = "Inspection type is required.";

        if (ucInspectionCategory.SelectedHard.Equals("") || ucInspectionCategory.SelectedHard.Equals("Dummy"))
            ucError.ErrorMessage = "Inspection category is required.";

        if (ddlInspectionShortCodeList.SelectedIndex == 0)
            ucError.ErrorMessage = "Inspection name is required.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null || General.GetNullableInteger(ucVessel.SelectedVessel) == 0)
            ucError.ErrorMessage = "'Vessel' is required.";

        if (General.GetNullableDateTime(txtDueDate.Text).Equals("") || General.GetNullableDateTime(txtDueDate.Text).Equals(null))
            ucError.ErrorMessage = "'Due date' is required.";

        if (General.GetNullableDateTime(txtLastDoneDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Last done date should not be the future date.";

       

        if (ucInspectionCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            if (txtWindowperiod.Text.Equals(""))
                ucError.ErrorMessage = "Window Period value is required.";
        }
        if (chkatsea.Checked)
        {
            if (ucFromPort.SelectedSeaport.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucFromPort.SelectedSeaport.ToString()))
                ucError.ErrorMessage = "From Port is required.";
            if (ucToPort.SelectedSeaport.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucToPort.SelectedSeaport.ToString()))
                ucError.ErrorMessage = "To Port is required.";
        }
        return (!ucError.IsError);
    }

    private bool IsValidReSchedule()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["INSPECTIONSCHEDULEID"] = null;
        txtDueDate.Text = txtRefNo.Text = txtLastDoneDate.Text  = "";

        ucInspectionCategory.Enabled = true;
        ucInspectionCategory.SelectedHard = "";
        ucInspectionType.Enabled = true;
        txtWindowperiod.Text = "";
        txtWindowperiod.Enabled = true;
        
        ucDoneType.Enabled = true;
        ddlInspectionShortCodeList.Enabled = true;
        ddlInspectionShortCodeList.SelectedIndex = -1;
        txtLastDoneDate.Enabled = txtDueDate.Enabled = true;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            ucVessel.Enabled = false;
        }
        else
        {
            ucVessel.SelectedVessel = "";
            ucVessel.Enabled = true;
        }

        txtLastDoneDate.CssClass = "input";
        txtWindowperiod.CssClass = "input_mandatory";
        txtRemarks.Text = "";
    }
    public void BindShortCodeList()
    {
        int? defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS"));
        DataSet ds = PhoenixInspection.ListInspectionShortCode(defaultauditytpe, General.GetNullableInteger(ucInspectionCategory.SelectedHard), null, null);
        ddlInspectionShortCodeList.Items.Add("select");
        ddlInspectionShortCodeList.DataSource = ds;
        ddlInspectionShortCodeList.DataTextField = "FLDSHORTCODE";
        ddlInspectionShortCodeList.DataValueField = "FLDINSPECTIONID";
        ddlInspectionShortCodeList.DataBind();
        ddlInspectionShortCodeList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void InspectionType_Changed(object sender, EventArgs e)
    {
        BindShortCodeList();
        if (ucInspectionCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "INT")))
        {
            ucDoneType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 5, "VAR");
            ucDoneType.Enabled = false;
            txtWindowperiod.CssClass = "input";

            Internal.Enabled = true;
            External.Enabled = false;

        }
        else if (ucInspectionCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "EXT")))
        {
            ucDoneType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 5, "VAR");
            ucDoneType.Enabled = false;
            txtWindowperiod.CssClass = "input_mandatory";

            Internal.Enabled = false;
            External.Enabled = true;
        }
    }
    protected void chkatsea_CheckedChanged(object sender, EventArgs e)
    {
        if (chkatsea.Checked == true)
        {
            ucPort.SelectedSeaport = "";
            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            ucFromPort.CssClass = "dropdown_mandatory";
            ucToPort.CssClass = "dropdown_mandatory";
            ucFromPort.Enabled = true;
            ucToPort.Enabled = true;
            rblLocation.Enabled = false;
            rblLocation.Items[0].Selected = false;
            rblLocation.Items[1].Selected = false;
        }
        else
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "dropdown_mandatory";
            ucFromPort.CssClass = "readonlytextbox";
            ucToPort.CssClass = "readonlytextbox";
            ucFromPort.Enabled = false;
            ucToPort.Enabled = false;
            ucFromPort.SelectedSeaport = "";
            ucToPort.SelectedSeaport = "";
            rblLocation.SelectedValue = "1";
            rblLocation.Enabled = true;
        }
    }
    protected void ExtrenalInspector(object sender, EventArgs e)
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditExternalInspectorSearch(General.GetNullableGuid(ddlExternalInspectorName.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtExternalOrganisationName.Text = dr["FLDCOMPANYNAME"].ToString();
            lblExternalOrganisationId.Text = dr["FLDCOMPANYID"].ToString();
        }
    }
    protected void BindInternalInspector()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlInspectorName.DataSource = ds.Tables[0];
        ddlInspectorName.DataTextField = "FLDDESIGNATIONNAME";
        ddlInspectorName.DataValueField = "FLDUSERCODE";
        ddlInspectorName.DataBind();
        ddlInspectorName.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindInternalAuditor()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlAuditorName.DataSource = ds;
        ddlAuditorName.DataTextField = "FLDDESIGNATIONNAME";
        ddlAuditorName.DataValueField = "FLDUSERCODE";
        ddlAuditorName.DataBind();
        ddlAuditorName.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindExternalOrganisation()
    {
        DataSet ds = PhoenixInspectionSchedule.ListInspectionCompany(null);
        ddlExternalOrganisation.DataSource = ds;
        ddlExternalOrganisation.DataTextField = "FLDCOMPANYNAME";
        ddlExternalOrganisation.DataValueField = "FLDCOMPANYID";
        ddlExternalOrganisation.DataBind();
        ddlExternalOrganisation.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindExternalInspector()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditExternalInspectorSearch(General.GetNullableGuid(ddlExternalInspectorName.SelectedValue));
        ddlExternalInspectorName.DataSource = ds;
        ddlExternalInspectorName.DataTextField = "FLDINSPECTORNAME";
        ddlExternalInspectorName.DataValueField = "FLDINSPECTORID";
        ddlExternalInspectorName.DataBind();
        ddlExternalInspectorName.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            //txtExternalOrganisationName.Text = dr["FLDCOMPANYNAME"].ToString();
        }
    }
    protected void chkAtSeaRequired_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void chkSecond_checkedchanged(object sender, EventArgs e)
    {
        BindInspectionSchedule();
    }
    protected void BindAutomaticDetails()
    {
        if (ViewState["INSPECTIONSCHEDULEID"] != null && ViewState["INSPECTIONSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["INSPECTIONSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtLastDoneDate.Text = General.GetDateTimeToString(dr["FLDLASTDONEDATE"].ToString());
                
                ucDoneType.SelectedHard = dr["FLDISFIXED"].ToString();
                txtWindowperiod.Text = dr["FLDWINDOWPERIOD"].ToString();
                //if (rdoAutomatic.Checked)
                //txtDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                //if (rdoMannual.Checked)
                txtDueDate.Text = General.GetDateTimeToString(dr["FLDINSPECTIONSTARTDATE"].ToString());
                if (General.GetNullableString(dr["FLDLASTDONEDATE"].ToString()) == null) //&& rdoAutomatic.Checked
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDINSPECTIONSTARTDATE"].ToString());
                if (dr["FLDINSPECTIONCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                    txtWindowperiod.CssClass = "input";
                else
                    txtWindowperiod.CssClass = "input_mandatory";
            }
        }
    }
    protected void BindManualDetails()
    {
        if (ViewState["INSPECTIONSCHEDULEID"] != null && ViewState["INSPECTIONSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["INSPECTIONSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //if (rdoAutomatic.Checked)
                //txtDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                //if (rdoMannual.Checked)
                txtDueDate.Text = General.GetDateTimeToString(dr["FLDINSPECTIONSTARTDATE"].ToString());
                if (General.GetNullableString(dr["FLDLASTDONEDATE"].ToString()) == null) //&& rdoAutomatic.Checked
                    txtDueDate.Text = General.GetDateTimeToString(dr["FLDINSPECTIONSTARTDATE"].ToString());
            }
        }
    }
}
