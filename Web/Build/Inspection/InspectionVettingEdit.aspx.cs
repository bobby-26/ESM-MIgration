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

public partial class InspectionVettingEdit : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            /*foreach (GridViewRow r in gvDeficiency.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvDeficiency.UniqueID, "Edit$" + r.RowIndex.ToString());
                    
                }

            }*/
            base.Render(writer);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ucInspectionType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS");              

                if (Request.QueryString["SCHEDULEID"] != null && Request.QueryString["SCHEDULEID"].ToString() != string.Empty)
                    ViewState["SCHEDULEID"] = Request.QueryString["SCHEDULEID"].ToString();
                else
                    Reset();

                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                ViewState["VESSELID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["INSPECTIONID"] = null;

                BindShortCodeList();
                BindInternalInspector();
                BindInternalAuditor();
                BindExternalOrganisation();
                BindExternalInspector();
                BindInspectionSchedule();
                //BindData();

                if (Request.QueryString["viewonly"] == null)
                {
                    PhoenixToolbar toolbarsub = new PhoenixToolbar();
                    toolbarsub.AddButton("Save", "SAVE");
                    toolbarsub.AddImageLink("javascript:parent.Openpopup('codehelp2','','../Inspection/InspectionVettingAttachment.aspx?dtkey=" + ViewState["DTKEY"]
                        + "&VESSELID=" + ViewState["VESSELID"]
                        + "&viewonly=0"
                        + "'); return false;", "Attachments", "", "ATTACHMENT");
                    MenuInspectionSchedule.AccessRights = this.ViewState;
                    MenuInspectionSchedule.MenuList = toolbarsub.Show();
                    ucTitle.Text = "Details";
                    ucTitle.ShowMenu = "true";

                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    toolbar.AddButton("List", "LIST");
                    toolbar.AddButton("Details", "DETAILS");
                    MenuInspectionGeneral.AccessRights = this.ViewState;
                    MenuInspectionGeneral.MenuList = toolbar.Show();
                    MenuInspectionGeneral.SelectedMenuIndex = 1;
                }
                else
                {
                    ucTitle.Text = "Vetting";
                    ucTitle.ShowMenu = "false";
                }               
            }

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void MenuInspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionScheduleMaster.aspx", true);
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
        if (ViewState["SCHEDULEID"] != null && ViewState["SCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["SCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtLastDoneDate.Text = dr["FLDLASTDONEDATE"].ToString();
                ucInspectionCategory.SelectedHard = dr["FLDINSPECTIONCATEGORYID"].ToString();
                ucInspectionCategory.Enabled = false;
                ucLastPort.SelectedSeaport = dr["FLDPORTOFLASTINSPECTION"].ToString();
                ddlInspectionShortCodeList.SelectedValue = dr["FLDINSPECTIONID"].ToString();
                ddlInspectionShortCodeList.Enabled = false;
                ViewState["INSPECTIONID"] = dr["FLDINSPECTIONID"].ToString();
                txtDueDate.Text = General.GetDateTimeToString(dr["FLDINSPECTIONDATE"].ToString());
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                txtWindowperiod.Text = dr["FLDWINDOWPERIOD"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtDateRangeFrom.Text = General.GetDateTimeToString(dr["FLDRANGEFROMDATE"].ToString());
                ucPort.SelectedSeaport = dr["FLDPORTID"].ToString();

                ucFromPort.SelectedSeaport = dr["FLDFROMPORTID"].ToString();
                ucToPort.SelectedSeaport = dr["FLDTOPORTID"].ToString();
                txtETA.Text = General.GetDateTimeToString(dr["FLDETA"].ToString());
                txtETD.Text = General.GetDateTimeToString(dr["FLDETD"].ToString());
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                txtCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                
                if (dr["FLDINSPECTIONCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    txtWindowperiod.CssClass = "input";
                    External.Enabled = false;
                    if(General.GetNullableInteger(dr["FLDINTERNALINSPECTORID"].ToString())!=null)
                        ddlInspectorName.SelectedValue = dr["FLDINTERNALINSPECTORID"].ToString();
                    txtOrganization.Text = dr["FLDORGANIZATIONOFINSPECTOR"].ToString();
                }
                else
                {
                    
                    txtWindowperiod.CssClass = "input_mandatory";
                    Internal.Enabled = false;
                    BindExternalInspector();
                    if(General.GetNullableGuid(dr["FLDINSPECTORID"].ToString())!= null)
                        ddlExternalInspectorName.SelectedValue = dr["FLDINSPECTORID"].ToString();
                    txtExternalOrganisationName.Text = dr["FLDCOMPANYNAME"].ToString();
                    if(General.GetNullableInteger(dr["FLDADDITIONALINSPECTORID"].ToString())!=null)
                        ddlAuditorName.SelectedValue = dr["FLDADDITIONALINSPECTORID"].ToString();
                }
                if (dr["FLDFROMPORTID"] != null && dr["FLDFROMPORTID"].ToString() != string.Empty)
                {
                    ucFromPort.Enabled = true;
                    ucFromPort.CssClass = "dropdown_mandatory";
                    ucToPort.Enabled = true;
                    ucToPort.CssClass = "dropdown_mandatory";
                    ucPort.CssClass = "readonlytextbox";
                    ucPort.Enabled = false;
                    chkatsea.Checked = true;
                    rblLocation.Enabled = false;

                }
                else
                {
                    ucFromPort.Enabled = false;
                    ucToPort.Enabled = false;
                    ucPort.Enabled = true ;
                    ucPort.CssClass = "input";
                    ucFromPort.CssClass = "readonlytextbox";
                    ucToPort.CssClass = "readonlytextbox";
                    chkatsea.Checked = false;
                    rblLocation.Enabled = true;
                    rblLocation.SelectedValue = dr["FLDISBERTHORANCHORAGE"].ToString();
                }
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
                String script = String.Format("javascript:fnReloadList('codehelp1');");
                if (!IsValidInspectionSchedule())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionSchedule.UpdateVettingScheduleDetails(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["SCHEDULEID"].ToString())
                        , new Guid(ddlInspectionShortCodeList.SelectedValue)
                        , Int16.Parse(ucVessel.SelectedVessel)
                        , General.GetNullableDateTime(txtLastDoneDate.Text)
                        , General.GetNullableDateTime(txtDueDate.Text)
                        , General.GetNullableInteger(txtWindowperiod.Text)
                        , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                        , General.GetNullableInteger(ucPort.SelectedSeaport)
                        , General.GetNullableDateTime(txtETA.Text)
                        , General.GetNullableDateTime(txtETD.Text)
                        , null
                        , General.GetNullableInteger(ddlInspectorName.SelectedValue)
                        , General.GetNullableGuid(ddlExternalInspectorName.SelectedValue)
                        , General.GetNullableInteger(lblExternalOrganisationId.Text)
                        , General.GetNullableInteger(ddlAuditorName.SelectedValue)
                        , General.GetNullableInteger(ucFromPort.SelectedSeaport)
                        , General.GetNullableInteger(ucToPort.SelectedSeaport)
                        , General.GetNullableInteger(ucLastPort.SelectedSeaport)
                        , General.GetNullableString(txtRemarks.Text)
                        , General.GetNullableDateTime(txtDateRangeFrom.Text)
                        , General.GetNullableInteger(ddlStatus.SelectedHard)
                        , General.GetNullableInteger(rblLocation.SelectedValue)
                        , General.GetNullableDateTime(txtCompletionDate.Text)
                        );
                ucStatus.Text = "'Inspection' Details updated successfully.";
                //String scriptinsert = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            BindShortCodeList();
            BindInternalInspector();
            BindInternalAuditor();
            BindExternalOrganisation();
            BindExternalInspector();
            BindInspectionSchedule();
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

        if (General.GetNullableInteger(ddlStatus.SelectedHard) == null)
            ucError.ErrorMessage = "Status is required.";
        else if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
        {
            if (General.GetNullableDateTime(txtDateRangeFrom.Text) == null)
                ucError.ErrorMessage = "Planned date is required.";
            if (General.GetNullableDateTime(txtCompletionDate.Text) == null)
                ucError.ErrorMessage = "Completion date is required.";
            if (chkatsea.Checked == false)
            {
                if (General.GetNullableInteger(ucPort.SelectedSeaport) == null)
                    ucError.ErrorMessage = "Port of Audit / Inspection is required.";
            }
            if (ucInspectionCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")))
            {
                if (General.GetNullableGuid(ddlExternalInspectorName.SelectedValue) == null)
                    ucError.ErrorMessage = "External Auditor / Inspector is requird.";
                if (General.GetNullableString(txtExternalOrganisationName.Text) == null)
                    ucError.ErrorMessage = "External Auditor / Inspector designation is requird.";
                if (General.GetNullableInteger(ddlAuditorName.Text) == null)
                    ucError.ErrorMessage = "Internal Auditor is required.";
            }
            else if (ucInspectionCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")))
            {
                if (ddlInspectorName.SelectedIndex == 0)
                    ucError.ErrorMessage = "Internal Auditor / Inspector is required.";
            }
        }

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
        txtDueDate.Text = txtRefNo.Text = txtLastDoneDate.Text = "";

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
        }
        else if (ucInspectionCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "EXT")))
        {
            ucDoneType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 5, "VAR");
            ucDoneType.Enabled = false;
            txtWindowperiod.CssClass = "input_mandatory";
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
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREFERENCENUMBER", "FLDTYPE", "FLDDEFICIENCYCATEGORY", "FLDISSUEDDATE", "FLDCHECKLISTREFERENCENUMBER", "FLDDESCRIPTION", "FLDSTATUS" };
            string[] alCaptions = { "Reference Number", "Deficiency Type", "Deficiency Category", "Issued Date", "Checklist Reference Number", "Description", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInspectionAuditSchedule.DeficiencySearch(int.Parse(ViewState["VESSELID"].ToString())
                , new Guid(ViewState["SCHEDULEID"].ToString())
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvDeficiency", "Deficiencies", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDeficiency.DataSource = ds;
                gvDeficiency.DataBind();
                if (ViewState["DEFICIENCYID"] == null || ViewState["DEFICIENCYID"].ToString() == "")
                {
                    ViewState["DEFICIENCYID"] = ds.Tables[0].Rows[0]["FLDDEFICIENCYID"].ToString();
                    gvDeficiency.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvDeficiency);
            }
            //SetTabHighlight();
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDeficiency(string deficiencytype, string deficiecncycategory, string date, string checklistref, string desc, string status)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(deficiencytype) == null)
            ucError.ErrorMessage = "Deficiency Type is required.";

        if (General.GetNullableInteger(deficiecncycategory) == null)
            ucError.ErrorMessage = "Deficiency Category is required.";

        if (General.GetNullableDateTime(date).Equals("") || General.GetNullableDateTime(date).Equals(null))
            ucError.ErrorMessage = "Issue date is required.";

        if (General.GetNullableDateTime(date) > DateTime.Today)
            ucError.ErrorMessage = "Issue date should not be greater than current date.";

        if (General.GetNullableString(checklistref) == null)
            ucError.ErrorMessage = "Checklist Reference Number is required.";

        if (General.GetNullableString(desc) == null)
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableInteger(status) == null)
            ucError.ErrorMessage = "Status is required.";

        return (!ucError.IsError);
    }

    public void setvalue(DropDownList rb, string value)
    {
        foreach (ListItem item in rb.Items)
        {
            if (item.Value.ToString() == value)
                item.Selected = true;
            else
                item.Selected = false;
        }
    }

    private void BindValue(int rowindex)
    {
        try
        {
            ViewState["DEFICIENCYID"] = ((Label)gvDeficiency.Rows[rowindex].FindControl("lblDeficiencyId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvDeficiency_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindValue(nCurrentRow);
            SetRowSelection();
        }
        else if (e.CommandName.ToUpper().Equals("ADD"))
        {
            DropDownList ddlTypeAdd = (DropDownList)_gridView.FooterRow.FindControl("ddlTypeAdd");
            UserControlQuick ucNCCategoryAdd = (UserControlQuick)_gridView.FooterRow.FindControl("ucNCCategoryAdd");
            string status = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");
            Guid? deficiencyid = null;

            if (ddlTypeAdd != null && (ddlTypeAdd.SelectedValue == "1" || ddlTypeAdd.SelectedValue == "2"))
            {
                if (!IsValidDeficiency(ddlTypeAdd.SelectedValue, ucNCCategoryAdd.SelectedQuick,
                    ((UserControlDate)_gridView.FooterRow.FindControl("ucDateAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtChecklistRefAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text,
                    status))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditSchedule.InsertVettingNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["SCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeAdd.SelectedValue),
                    int.Parse(ucVessel.SelectedVessel),
                    General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucDateAdd")).Text),
                    int.Parse(status), 
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtChecklistRefAdd")).Text),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text),
                    4, ref deficiencyid
                    ,General .GetNullableGuid (((UserControlInspectionChapter)_gridView.FooterRow .FindControl ("ucChapterAdd")).SelectedChapter));
            }
            else if (ddlTypeAdd != null && ddlTypeAdd.SelectedValue == "3")
            {
                if (!IsValidDeficiency(ddlTypeAdd.SelectedValue, ucNCCategoryAdd.SelectedQuick,
                    ((UserControlDate)_gridView.FooterRow.FindControl("ucDateAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtChecklistRefAdd")).Text,
                    ((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text,
                    status))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditSchedule.InsertVettingObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["SCHEDULEID"].ToString()), int.Parse(ucVessel.SelectedVessel),
                    General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucDateAdd")).Text),
                    int.Parse(status), 
                     General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtChecklistRefAdd")).Text),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text),
                    2, ref deficiencyid, 1
                    , General.GetNullableGuid(((UserControlInspectionChapter)_gridView.FooterRow.FindControl("ucChapterAdd")).SelectedChapter));
            }
            BindData();
            SetPageNavigator();
        }
    }

    protected void gvDeficiency_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = de.RowIndex;
            Label lblTypeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTypeid"));
            Label lblDeficiencyId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDeficiencyId"));

            if (lblTypeid != null && lblTypeid.Text == "1")
            {
                PhoenixInspectionAuditDirectNonConformity.DeleteAuditDirectNonConformity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDeficiencyid")).Text));
            }
            else if (lblTypeid != null && lblTypeid.Text == "2")
            {
                PhoenixInspectionObservation.DeleteInspectionDirectObservation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDeficiencyid")).Text));
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_RowCancelingEdit(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvDeficiency_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }
            DataRowView drv = (DataRowView)e.Row.DataItem;
            DropDownList ddltype = (DropDownList)e.Row.FindControl("ddlTypeEdit");
            UserControlQuick ucNCCategoryEdit = (UserControlQuick)e.Row.FindControl("ucNCCategoryEdit");
            UserControlHard ucStatusEdit = (UserControlHard)e.Row.FindControl("ucStatusEdit");
            UserControlInspectionChapter ucChapterEdit = (UserControlInspectionChapter)e.Row.FindControl("ucChapterEdit");

            if (ddltype != null)
            {
                if (drv["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MAJ"))
                {
                    ddltype.SelectedValue = "1";
                }
                else if (drv["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MIN"))
                {
                    ddltype.SelectedValue = "2";
                }
                else
                {
                    ddltype.SelectedValue = "3";
                }
                if (ucNCCategoryEdit != null)
                {
                    ucNCCategoryEdit.Visible = true;
                    ucNCCategoryEdit.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                    ucNCCategoryEdit.DataBind();
                    ucNCCategoryEdit.SelectedQuick = drv["FLDDEFICIENCYCATEGORYID"].ToString();
                }
            }
            if (ucStatusEdit != null)
            {
                ucStatusEdit.HardList = PhoenixRegistersHard.ListHard(1, 146, 0, "OPN,CLD,CMP");
                ucStatusEdit.DataBind();
                ucStatusEdit.SelectedHard = drv["FLDSTATUSID"].ToString();
            }
            if (ucChapterEdit != null)
            {
                ucChapterEdit.InspectionId = ViewState["INSPECTIONID"].ToString ();
                ucChapterEdit.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null, null, General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                ucChapterEdit.DataBind();
                ucChapterEdit.SelectedChapter = drv["FLDCHAPTERID"].ToString();
            }

            if (Request.QueryString["viewonly"] != null && Request.QueryString["viewonly"].ToString().Equals("1"))
                gvDeficiency.Columns[8].Visible = false;
            else
                gvDeficiency.Columns[8].Visible = true;
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlTypeAdd = (DropDownList)e.Row.FindControl("ddlTypeAdd");
            UserControlQuick ucNCCategoryAdd = (UserControlQuick)e.Row.FindControl("ucNCCategoryAdd");            
            UserControlInspectionChapter ucChapterAdd = (UserControlInspectionChapter)e.Row.FindControl("ucChapterAdd");
            
            if (ucNCCategoryAdd != null)
            {
                ucNCCategoryAdd.Visible = true;
                ucNCCategoryAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                ucNCCategoryAdd.DataBind();
            }
            
            if (ucChapterAdd != null)
            {
                ucChapterAdd.InspectionId = ViewState["INSPECTIONID"].ToString();
                ucChapterAdd.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null, null, General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                ucChapterAdd.DataBind();
            }
        }
    }

    protected void gvDeficiency_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            DropDownList ddlTypeEdit = (DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlTypeEdit");
            UserControlQuick ucNCCategoryEdit = (UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucNCCategoryEdit");
            Label lblDeficiencyId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDeficiencyId");
            Guid? deficiencyid = new Guid(lblDeficiencyId.Text);

            if (ddlTypeEdit != null && (ddlTypeEdit.SelectedValue == "1" || ddlTypeEdit.SelectedValue == "2"))
            {
                if (!IsValidDeficiency(ddlTypeEdit.SelectedValue, ucNCCategoryEdit.SelectedQuick,
                    ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtChecklistRefEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text,
                    ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucStatusEdit")).SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditSchedule.InsertVettingNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["SCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeEdit.SelectedValue),
                    int.Parse(ucVessel.SelectedVessel),
                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateEdit")).Text),
                    int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucStatusEdit")).SelectedHard),                    
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtChecklistRefEdit")).Text),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text),
                    4, ref deficiencyid
                    , General.GetNullableGuid(((UserControlInspectionChapter)_gridView.Rows[nCurrentRow].FindControl("ucChapterEdit")).SelectedChapter));
            }
            else if (ddlTypeEdit != null && ddlTypeEdit.SelectedValue == "3")
            {
                if (!IsValidDeficiency(ddlTypeEdit.SelectedValue, ucNCCategoryEdit.SelectedQuick,
                    ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtChecklistRefEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text,
                    ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucStatusEdit")).SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditSchedule.InsertVettingObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["SCHEDULEID"].ToString()), int.Parse(ucVessel.SelectedVessel),
                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateEdit")).Text),
                    int.Parse(((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ucStatusEdit")).SelectedHard),
                     General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtChecklistRefEdit")).Text),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text),
                    2, ref deficiencyid, 1
                    , General.GetNullableGuid(((UserControlInspectionChapter)_gridView.Rows[nCurrentRow].FindControl("ucChapterEdit")).SelectedChapter));
            }

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
           && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
           && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDeficiency, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        SetKeyDownScroll(sender, e);
    }

    protected void gvDeficiency_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvDeficiency.SelectedIndex = se.NewSelectedIndex;
        BindValue(se.NewSelectedIndex);
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }

    private void SetRowSelection()
    {
        gvDeficiency.SelectedIndex = -1;
        for (int i = 0; i < gvDeficiency.Rows.Count; i++)
        {
            if (gvDeficiency.DataKeys[i].Value.ToString().Equals(ViewState["DEFICIENCYID"].ToString()))
            {
                gvDeficiency.SelectedIndex = i;
            }
        }
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }

        gvDeficiency.EditIndex = -1;
        gvDeficiency.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvDeficiency.SelectedIndex = -1;
        gvDeficiency.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        gvDeficiency.EditIndex = -1;
        gvDeficiency.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }
}
