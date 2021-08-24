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

public partial class InspectionAuditScheduleAdd : PhoenixBasePage
{
    public int? defaultauditytpe = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {     
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD"));
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                MenuInspectionScheduleGeneral.MenuList = toolbar.Show();

                if (Request.QueryString["AUDITSCHEDULEID"] != null && Request.QueryString["AUDITSCHEDULEID"].ToString() != string.Empty)
                    ViewState["AUDITSCHEDULEID"] = Request.QueryString["AUDITSCHEDULEID"].ToString();
                else
                    Reset();

                ViewState["INTERNALREVIEWSCHEDULEID"] = null;
                ViewState["EXTERNALREVIEWSCHEDULEID"] = null;
                ViewState["FLDINTERIMAUDITID"] = null;
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                if (defaultauditytpe != null)
                    ucAudit.InspectionType = defaultauditytpe.ToString();                
                BindShortCodeList();
                BindAuditSchedule();
                SetWidth();
                BindInternalInspector();
                BindInternalAuditor();
                BindExternalOrganisation();
                BindExternalInspector();
            }
            ddlInspectorName.DataSourceID = String.Empty;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public void BindShortCodeList()
    {
        int? defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD"));
        DataSet ds = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
            General.GetNullableInteger(ucAuditCategory.SelectedHard), 
            General.GetNullableInteger(ucExternalAuditType.SelectedHard),
            General.GetNullableInteger(ucVessel.SelectedVessel));
        ddlAuditShortCodeList.DataSource = ds;
        ddlAuditShortCodeList.DataTextField = "FLDSHORTCODE";
        ddlAuditShortCodeList.DataValueField = "FLDINSPECTIONID";
        ddlAuditShortCodeList.DataBind();
        ddlAuditShortCodeList.Items.Insert(0, new ListItem("--Select--", "Dummy"));        
    }

    private void BindAuditSchedule()
    {
        if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["AUDITSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtSerialNumber.Text = dr["FLDSERIALNUMBER"].ToString();
                ucAuditType.SelectedHard = dr["FLDREVIEWTYPEID"].ToString();
                ucAuditCategory.SelectedHard = dr["FLDREVIEWCATEGORYID"].ToString();
                ucExternalAuditType.SelectedHard = dr["FLDEXTERNALAUDITTYPE"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ucLastPort.SelectedSeaport = dr["FLDLASTPORTOFAUDITINSPECTION"].ToString();

                int? defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD"));
                DataSet ds1 = PhoenixInspection.ListInspectionShortCode(defaultauditytpe,
                    General.GetNullableInteger(dr["FLDREVIEWCATEGORYID"].ToString()),
                    General.GetNullableInteger(dr["FLDEXTERNALAUDITTYPE"].ToString()),
                    General.GetNullableInteger(dr["FLDVESSELID"].ToString()));
                ddlAuditShortCodeList.DataSource = ds1;
                ddlAuditShortCodeList.DataTextField = "FLDSHORTCODE";
                ddlAuditShortCodeList.DataValueField = "FLDINSPECTIONID";
                ddlAuditShortCodeList.DataBind();
                ddlAuditShortCodeList.Items.Insert(0, new ListItem("--Select--", "Dummy"));               

                if (dr["FLDREVIEWCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    //txtWindowperiod.CssClass = "input";
                    ucExternalAuditType.CssClass = "input";
                    ucExternalAuditType.Enabled = false;
                    ddlInspectorName.CssClass = "input_mandatory";
                    External.Enabled = false;
                }
                else
                {
                    //txtWindowperiod.CssClass = "input_mandatory";
                    ucExternalAuditType.CssClass = "input";
                    ucExternalAuditType.Enabled = true;
                    Internal.Enabled = false;
                    ddlExternalOrganisation.CssClass = "input_mandatory";
                    //ddlExternalInspectorName.CssClass = "input_mandatory";
                }                
                ucAudit.SelectedValue = dr["FLDREVIEWID"].ToString();
                ddlAuditShortCodeList.SelectedValue = dr["FLDREVIEWID"].ToString();
                               
                txtWindowperiod.Enabled = false;
                txtDueDate.Enabled = true;
                txtDueDate.Text = General.GetDateTimeToString(dr["FLDREVIEWSTARTDATE"].ToString());
            
                txtLastDoneDate.Text = General.GetDateTimeToString(dr["FLDLASTDONEDATE"].ToString());                               
                ucwindowperiodtype.SelectedHard = dr["FLDWINDOWPERIODTYPE"].ToString();
                txtWindowperiod.Text = dr["FLDWINDOWPERIOD"].ToString();

                txtRefNo.Enabled = false;
                ucAuditType.Enabled = ucAudit.Enabled = false;
                ViewState["FLDINTERIMAUDITID"] = dr["FLDINTERIMAUDITID"].ToString();
             
                ucPort.SelectedSeaport = dr["FLDPORTID"].ToString();
                txtETA.Text = dr["FLDETA"].ToString();
                txtETD.Text = dr["FLDETD"].ToString();
                txtDateRangeFrom.Text = dr["FLDRANGEFROMDATE"].ToString();
                ddlInspectorName.SelectedValue = dr["FLDINTERNALINSPECTORID"].ToString();
                ddlExternalOrganisation.SelectedValue = dr["FLDINSPECTORCOMPANYID"].ToString();
                //ddlExternalInspectorName.SelectedValue = dr["FLDINSPECTORID"].ToString();                
                ddlAuditorName.SelectedValue = dr["FLDORGANIZATIONOFINSPECTOR"].ToString();
                txtExternalInspectorName.Text = dr["FLDEXTERNALINSPECTORNAME"].ToString();
                txtExternalInspectorDesignation.Text = dr["FLDEXTERNALINSPECTORDESIGNATION"].ToString();
                txtExternalOrganisationName.Text = dr["FLDEXTERNALINSPECTORORGANISATION"].ToString();
            }
        }
    }    

    protected void MenuInspectionScheduleGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            { 
                DateTime? duedate = null;
                DateTime? lastdonedate = General.GetNullableDateTime(txtLastDoneDate.Text);                
                DataTable dt = new DataTable();

                if (IsValidAuditSchedule())
                {
                    if (ViewState["AUDITSCHEDULEID"] == null)
                    {
                        duedate = General.GetNullableDateTime(txtDueDate.Text);
                        //DateTime d = DateTime.Parse(duedate.ToString());
                        
                        Guid? scheduleid = null;

                        if (General.GetNullableGuid(ddlAuditShortCodeList.SelectedValue) != null)
                        {
                            PhoenixInspectionAuditSchedule.InsertAuditSchedule(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , new Guid(ddlAuditShortCodeList.SelectedValue)
                                , Int16.Parse(ucVessel.SelectedVessel)
                                , 0
                                , General.GetNullableDateTime(txtLastDoneDate.Text)
                                , null
                                , null
                                , null
                                , 0
                                , null
                                , null
                                , duedate
                                , General.GetNullableInteger(txtWindowperiod.Text)
                                , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 7, "DAI"))
                                , 0
                                , null
                                , ref scheduleid
                                , General.GetNullableString(txtRemarks.Text)
                                , General.GetNullableInteger(ucLastPort.SelectedSeaport)
                                , General.GetNullableInteger(ucPort.SelectedSeaport)
                                , chkatsea.Checked ? 1 : 0
                                , General.GetNullableInteger(rblLocation.SelectedValue)
                                , General.GetNullableDateTime(txtETA.Text)
                                , General.GetNullableDateTime(txtETD.Text)
                                , General.GetNullableInteger(ddlInspectorName.SelectedValue)
                                , General.GetNullableString(txtOrganization.Text)
                                , General.GetNullableGuid(ddlExternalOrganisation.SelectedValue)
                                , null //General.GetNullableGuid(ddlExternalInspectorName.SelectedValue)
                                , General.GetNullableInteger(ddlAuditorName.SelectedValue)
                                , General.GetNullableString(txtExternalInspectorName.Text)
                                , General.GetNullableString(txtExternalInspectorDesignation.Text)
                                , General.GetNullableString(txtExternalOrganisationName.Text)
                                );

                            BindAuditSchedule();
                            ucStatus.Text = "Schedule is created successfully";
                            Filter.CurrentAuditScheduleId = scheduleid.ToString();
                            String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                        }
                    }                    
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }

            }
            else if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidAuditSchedule()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ucAuditCategory.SelectedHard) == null)
            ucError.ErrorMessage = "Category is required.";        

        if (ddlAuditShortCodeList.SelectedIndex == 0)
            ucError.ErrorMessage = "Audit / Inspection is required.";

        //if (General.GetNullableDateTime(txtDueDate.Text).Equals("") || General.GetNullableDateTime(txtDueDate.Text).Equals(null))
        //    ucError.ErrorMessage = "Due date is required.";

        if (General.GetNullableInteger(ddlStatus.SelectedHard) == null)
            ucError.ErrorMessage = "Status is required.";

        //if (ucAuditCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")))
        //{
        //    if (General.GetNullableString(txtWindowperiod.Text) == null)
        //        ucError.ErrorMessage = "Window Period is required.";
        //}

        if (chkatsea.Checked)
        {
            if (ucFromPort.SelectedSeaport.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucFromPort.SelectedSeaport.ToString()))
                ucError.ErrorMessage = "From Port is required.";
            if (ucToPort.SelectedSeaport.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucToPort.SelectedSeaport.ToString()))
                ucError.ErrorMessage = "To Port is required.";
        }

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["AUDITSCHEDULEID"] = null;
        txtDueDate.Text = txtRefNo.Text = txtLastDoneDate.Text = "";
        ucAuditCategory.SelectedHard = ucAudit.SelectedValue = "";                            
        ucExternalAuditType.Enabled = true;
        ucExternalAuditType.SelectedHard = "";
        txtWindowperiod.Text = "";
        txtWindowperiod.Enabled = true;                
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
        ddlAuditShortCodeList.SelectedIndex = -1;
        ddlAuditShortCodeList.Enabled = true;        
        txtLastDoneDate.Enabled = true;
        txtRemarks.Text = "";
        ucLastPort.SelectedSeaport = "";
        txtSerialNumber.Text = "";
    }

    protected void InspectionType_Changed(object sender, EventArgs e)
    {
        if (ucAuditCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "EXT")))
        {            
            ucExternalAuditType.CssClass = "input";
            //txtWindowperiod.CssClass = "input_mandatory";
            ucExternalAuditType.Enabled = true;
            Internal.Enabled = false;
            External.Enabled = true;
        }
        else if (ucAuditCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "INT")))
        {
            ucExternalAuditType.CssClass = "input";
            //txtWindowperiod.CssClass = "input";
            ucExternalAuditType.SelectedHard = "";
            ucExternalAuditType.Enabled = false;
            Internal.Enabled = true;
            External.Enabled = false;
        }
        
        ucAudit.ExternalAuditType = ucExternalAuditType.SelectedHard;
        ucAudit.InspectionList = PhoenixInspection.ListInspection(
            General.GetNullableInteger(ucAuditType.SelectedHard) == null ? defaultauditytpe : General.GetNullableInteger(ucAuditType.SelectedHard)
            , General.GetNullableInteger(ucAuditCategory.SelectedHard) == null ? 0 : General.GetNullableInteger(ucAuditCategory.SelectedHard)
            , General.GetNullableInteger(ucExternalAuditType.SelectedHard));
        BindShortCodeList();
    }

    protected void ExternalAuditType_Changed(object sender, EventArgs e)
    {
        ucAudit.ExternalAuditType = ucExternalAuditType.SelectedHard;
        ucAudit.InspectionList = PhoenixInspection.ListInspection(
            General.GetNullableInteger(ucAuditType.SelectedHard) == null ? defaultauditytpe : General.GetNullableInteger(ucAuditType.SelectedHard)
            , General.GetNullableInteger(ucAuditCategory.SelectedHard) == null ? 0 : General.GetNullableInteger(ucAuditCategory.SelectedHard)
            , General.GetNullableInteger(ucExternalAuditType.SelectedHard));
        BindShortCodeList();        
    }
       
    protected void ucVessel_changed(object sender, EventArgs e)
    {
        BindShortCodeList();
    }

    protected void SetWidth()
    {
        DropDownList ddlVessel = (DropDownList)ucVessel.FindControl("ddlVessel");
        DropDownList ddlAuditCategory = (DropDownList)ucAuditCategory.FindControl("ddlHard");
        DropDownList ddlExternalAuditType = (DropDownList)ucExternalAuditType.FindControl("ddlHard");
        DropDownList ddlListStatus = (DropDownList)ddlStatus.FindControl("ddlHard");

        Unit ucWidth = new Unit("150px");
        if (ddlVessel != null)
            ddlVessel.Width = Unit.Parse("150");
        if (ddlAuditCategory != null)
            ddlAuditCategory.Attributes.Add("style", "width:150px;");
        if (ddlExternalAuditType != null)
            ddlExternalAuditType.Attributes.Add("style", "width:150px;");
        if (ddlListStatus != null)
            ddlListStatus.Attributes.Add("style", "width:95px;");
        ddlAuditShortCodeList.Attributes.Add("style", "width:150px;");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //if (Filter.CurrentPickListSelection == null)
        //    return;

        //if (Filter.CurrentPickListSelection.Keys[3].ToString() == "txtExternalOrganisation")
        //    //txtOrganisationName.Text = Filter.CurrentPickListSelection.Get(3);
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
        //DataSet ds = PhoenixInspectionAuditSchedule.AuditExternalInspectorSearch(General.GetNullableGuid(ddlExternalInspectorName.SelectedValue));
        //ddlExternalInspectorName.DataSource = ds;
        //ddlExternalInspectorName.DataTextField = "FLDINSPECTORNAME";
        //ddlExternalInspectorName.DataValueField = "FLDINSPECTORID";
        //ddlExternalInspectorName.DataBind();
        //ddlExternalInspectorName.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];
        //    txtExternalOrganisationName.Text = dr["FLDCOMPANYNAME"].ToString();
        //}
    }
    protected void ExternalOrganisation(object sender, EventArgs e)
    {
        BindExternalInspector();
    }

    protected void ExtrenalInspector(object sender, EventArgs e)
    {
        //DataSet ds = PhoenixInspectionAuditSchedule.AuditExternalInspectorSearch(General.GetNullableGuid(ddlExternalInspectorName.SelectedValue));
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];
        //    txtExternalOrganisationName.Text = dr["FLDCOMPANYNAME"].ToString();
        //    lblExternalOrganisationId.Text = dr["FLDCOMPANYID"].ToString();
        //}
    }
}
