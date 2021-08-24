using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionAuditRecordGeneral : PhoenixBasePage
{
    public int? defaultauditytpe = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                if (Request.QueryString["AUDITSCHEDULEID"] != null && Request.QueryString["AUDITSCHEDULEID"].ToString() != string.Empty)
                    ViewState["AUDITSCHEDULEID"] = Request.QueryString["AUDITSCHEDULEID"].ToString();
                else
                    Reset();

                if (Request.QueryString["reffrom"] != null && Request.QueryString["reffrom"].ToString() != string.Empty)
                    ViewState["reffrom"] = Request.QueryString["reffrom"].ToString();

                if (Request.QueryString["viewonly"] != null && Request.QueryString["viewonly"].ToString() != string.Empty)
                    ViewState["viewonly"] = Request.QueryString["viewonly"].ToString();

                if (ViewState["reffrom"] != null && ViewState["reffrom"].ToString() != string.Empty)
                    ddlStatus.Enabled = false;

                if (ViewState["viewonly"] != null && ViewState["viewonly"].ToString() != string.Empty)
                    ddlStatus.Enabled = false;

                ucConfirm.Attributes.Add("style", "display:none");
                ucConfirmDelete.Attributes.Add("style", "display:none");
                gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["INTERNALREVIEWSCHEDULEID"] = null;
                ViewState["EXTERNALREVIEWSCHEDULEID"] = null;
                ViewState["FLDINTERIMAUDITID"] = null;
                ViewState["REVIEWID"] = null;
                ViewState["PSCINSPECTIONYN"] = "0";
                ViewState["VALIDYN"] = "0";
                ViewState["VALIDDATIONMSG"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                

                if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != string.Empty)
                {
                    DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["AUDITSCHEDULEID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewState["REVIEWID"] = ds.Tables[0].Rows[0]["FLDREVIEWID"].ToString();
                }
                BindInternalOrganization();
                BindExternalOrganization();
                BindCompany();
                BindInternalInspector();
                BindInternalAuditor();
                BindAuditSchedule();
                SetWidth();
                SetRights();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["viewonly"] == null)
            {
                
                toolbar.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
                        + "&mod=" + PhoenixModule.QUALITY
                        + "&type=AUDITINSPECTION"
                        + "&cmdname=AUDITINSPECTIONUPLOAD"
                        + "&VESSELID=" + ViewState["VESSELID"]
                        + "'); return true;", "Attachments", "", "ATTACHMENT",ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuInspectionScheduleGeneral.AccessRights = this.ViewState;
                MenuInspectionScheduleGeneral.MenuList = toolbar.Show();
            }
            else
            {
                toolbar.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
                        + "&mod=" + PhoenixModule.QUALITY
                        + "&type=AUDITINSPECTION"
                        + "&cmdname=AUDITINSPECTIONUPLOAD"
                        + "&VESSELID=" + ViewState["VESSELID"]
                        + "&U=1'); return true;", "Attachments", "", "ATTACHMENT", ToolBarDirection.Right);
                MenuInspectionScheduleGeneral.AccessRights = this.ViewState;
                MenuInspectionScheduleGeneral.MenuList = toolbar.Show();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void SetRights()
    {
        bool flag = true;
        DataSet ds1 = PhoenixInspectionEventSupdtFeedback.EventSupdtFeedbackEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
           , 1, General.GetNullableGuid(ViewState["AUDITSCHEDULEID"].ToString()));

        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds1.Tables[0].Rows[0];
            if (dr["FLDVALIDYN"].ToString() == "1")
            {
                ViewState["VALIDYN"] = "1";
            }
            else
            {
                ViewState["VALIDYN"] = "0";
                ViewState["VALIDDATIONMSG"] = dr["FLDVALIDATIONMSG"].ToString();
            }
        }
        flag = SessionUtil.CanAccess(this.ViewState, "SUPDTEVENTFEEDBACK");

        if (flag == true)
        {
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback != 1)
            {
                flag = false;
            }
        }
        if (flag == true)
        {
            imgSupdtEventFeedback.Visible = true;
        }
        else
        {
            imgSupdtEventFeedback.Visible = false;
        }
        if (Request.QueryString["viewonly"] != null)
            imgSupdtEventFeedback.Visible = false;
    }
    protected void MenuInspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionAuditRecordList.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void EnableDisableExternal(bool value, string cssclass)
    {
        txtExternalInspectorName.Enabled = value;
        txtExternalInspectorDesignation.Enabled = value;
        txtExternalOrganisationName.Enabled = value;
        ddlExternalOrganizationName.Enabled = value;
        //ddlAuditorName.Enabled = value;
        ucAuditor.Enabled = value;
        ddlCompany.Enabled = value;

        if (value == false)
        {
            txtExternalInspectorName.Text = "";
            txtExternalInspectorDesignation.Text = "";
            txtExternalOrganisationName.Text = "";
            //ddlAuditorName.SelectedIndex = 0;
            ucAuditor.SelectedValue = "";
            ucAuditor.Text = "";
            ddlCompany.SelectedIndex = 0;
        }

        ddlExternalOrganizationName.CssClass = "readonlytextbox";
        txtExternalInspectorName.CssClass = cssclass;
        //txtExternalInspectorDesignation.CssClass = cssclass;
        //txtExternalOrganisationName.CssClass = cssclass;
        //ddlAuditorName.CssClass = cssclass;
        ucAuditor.CssClass = cssclass;
    }

    protected void EnableDisableInternal(bool value, string cssclass)
    {
        //ddlInspectorName.Enabled = value;
        ucInspector.Enabled = value;
        ddlOrganization.Enabled = value;

        if (value == false)
        {
            //ddlInspectorName.SelectedIndex = 0;
            ucInspector.SelectedValue = "";
            ucInspector.Text = "";
            txtOrganization.Text = "";
        }

        //ddlInspectorName.CssClass = cssclass;
        ucInspector.CssClass = cssclass;
        ddlOrganization.CssClass = "readonlytextbox";
    }

    private void BindAuditSchedule()
    {
        if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["AUDITSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblVesselId.Text = dr["FLDVESSELID"].ToString();
                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtCategory.Text = dr["FLDREVIEWCATEGORYNAME"].ToString();
                txtCategoryid.Text = dr["FLDREVIEWCATEGORYID"].ToString();
                txtInspection.Text = dr["FLDREVIEWSHORTCODE"].ToString();
                txtCompletedDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                if (dr["FLDCREDITEDYN"].ToString() == "0")
                {
                    ChkCredited.Checked = false;
                }
                if (dr["FLDCREDITEDYN"].ToString() == "1")
                {
                    ChkCredited.Checked = true;
                }
                if (dr["FLDINTERNALINSPECTORID"] != null && dr["FLDINTERNALINSPECTORID"].ToString() != "")
                    //ddlInspectorName.SelectedValue = dr["FLDINTERNALINSPECTORID"].ToString();
                    ucInspector.SelectedValue = dr["FLDINTERNALINSPECTORID"].ToString();
                ucInspector.Text = dr["FLDINTERNALINSPECTORNAME"].ToString();
                txtExternalInspectorName.Text = dr["FLDEXTERNALINSPECTORNAME"].ToString();
                txtExternalInspectorDesignation.Text = dr["FLDEXTERNALINSPECTORDESIGNATION"].ToString();
                txtExternalOrganisationName.Text = dr["FLDEXTERNALINSPECTORORGANISATION"].ToString();
                if (dr["FLDADDITIONALAUDITORID"] != null && dr["FLDADDITIONALAUDITORID"].ToString() != "")
                    //ddlAuditorName.SelectedValue = dr["FLDADDITIONALAUDITORID"].ToString();
                    ucAuditor.SelectedValue = dr["FLDADDITIONALAUDITORID"].ToString();
                ucAuditor.Text = dr["FLDATTENDINGSUPTNAME"].ToString();
                ucAnniversaryDate.Text = General.GetDateTimeToString(dr["FLDANNIVERSARYDATE"].ToString());
                if (dr["FLDREVIEWCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    EnableDisableExternal(false, "input");
                    ddlCompany.CssClass = "input";
                    txtExternalOrganisationName.CssClass = "input";
                    ddlOrganization.CssClass = "input_mandatory";
                    ddlOrganization.SelectedValue = dr["FLDINTERNALORGANIZATIONID"].ToString();
                }
                else
                {
                    EnableDisableInternal(false, "input");
                    ddlCompany.CssClass = "input_mandatory";
                    txtExternalOrganisationName.CssClass = "input_mandatory";
                    ddlExternalOrganizationName.CssClass = "input_mandatory";

                    ddlExternalOrganizationName.SelectedValue = dr["FLDEXTERNALORGANIZATIONID"].ToString();
                }
                txtRefNo.Enabled = false;
                ViewState["FLDINTERIMAUDITID"] = dr["FLDINTERIMAUDITID"].ToString();
                ucPort.SelectedValue = dr["FLDPORTID"].ToString();
                ucPort.Text = dr["FLDPORTNAME"].ToString();
                if (dr["FLDISBERTHORANCHORAGE"] != null && dr["FLDISBERTHORANCHORAGE"].ToString() != "")
                    rblLocation.SelectedValue = dr["FLDISBERTHORANCHORAGE"].ToString();

                if (dr["FLDPORTID"] != null && dr["FLDPORTID"].ToString() != "")
                {
                    chkatsea.Checked = false;
                    ucFromPort.Enabled = false;
                    ucToPort.Enabled = false;
                    ucFromPort.SelectedValue = "";
                    ucToPort.SelectedValue = "";
                    ucPort.CssClass = "input_mandatory";
                }
                else
                {
                    chkatsea.Checked = true;
                    ucFromPort.SelectedValue = dr["FLDFROMPORTID"].ToString();
                    ucFromPort.Text = dr["FLDFROMPORTNAME"].ToString();
                    ucToPort.SelectedValue = dr["FLDTOPORTID"].ToString();
                    ucToPort.Text = dr["FLDTOPORTNAME"].ToString();
                    ucFromPort.Enabled = true;
                    ucToPort.Enabled = true;
                    ucFromPort.CssClass = "input_mandatory";
                    ucToPort.CssClass = "input_mandatory";
                    ucPort.Enabled = false;
                    ucPort.SelectedValue = "";
                    ucPort.CssClass = "input";
                    rblLocation.Enabled = false;
                    rblLocation.Items[0].Selected = false;
                    rblLocation.Items[1].Selected = false;

                }
                if (rblLocation.SelectedValue == "1")
                {
                    txtBerth.Enabled = true;
                    txtBerth.CssClass = "input";
                }
                else
                {
                    txtBerth.Enabled = false;
                    txtBerth.Text = "";
                    txtBerth.CssClass = "readonlytextbox";
                }
                txtBerth.Text = dr["FLDBERTH"].ToString();
                ViewState["ATTACHMENTCODE"] = dr["FLDDTKEY"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["REVIEWID"] = dr["FLDREVIEWID"].ToString();
                ViewState["PSCINSPECTIONYN"] = dr["FLDPSCINSPECTIONYN"].ToString();
                if (dr["FLDINSPECTINGCOMPANYID"] != null && dr["FLDINSPECTINGCOMPANYID"].ToString() != "")
                {
                    ddlCompany.SelectedValue = dr["FLDINSPECTINGCOMPANYID"].ToString();
                }
                if (dr["FLDNILDEFICIENCIES"] != null && dr["FLDNILDEFICIENCIES"].ToString() != "" && dr["FLDNILDEFICIENCIES"].ToString() == "1")
                {
                    chkNILDeficiencies.Checked = true;
                    gvDeficiency.Enabled = false;
                }
                else
                {
                    chkNILDeficiencies.Checked = false;
                    gvDeficiency.Enabled = true;
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                {
                    ChkCredited.Visible = false;
                    lblCredited.Visible = false;
                }
                if (dr["FLDISDETENTION"] != null && dr["FLDISDETENTION"].ToString() != "" && dr["FLDISDETENTION"].ToString().ToUpper() == "TRUE")
                {
                    chkDetention.Checked = true;

                }
                else
                {
                    chkDetention.Checked = false;
                }
            }
        }
    }

    protected void SaveReviewDetails()
    {
        if (!IsValidAuditSchedule())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixInspectionAuditSchedule.UpdateReviewSchedule(
             PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
            , General.GetNullableString(txtRemarks.Text)
            , General.GetNullableInteger(ddlStatus.SelectedHard)
            , General.GetNullableDateTime(txtCompletedDate.Text)
            , General.GetNullableInteger(ucPort.SelectedValue)
            , General.GetNullableInteger(rblLocation.SelectedValue)
            , General.GetNullableInteger(ucFromPort.SelectedValue)
            , General.GetNullableInteger(ucToPort.SelectedValue)
            //, General.GetNullableInteger(ddlInspectorName.SelectedValue)
            , General.GetNullableInteger(ucInspector.SelectedValue)
            , General.GetNullableString(txtOrganization.Text)
            , General.GetNullableString(txtExternalInspectorName.Text)
            , General.GetNullableString(txtExternalOrganisationName.Text)
            , General.GetNullableString(txtExternalInspectorDesignation.Text)
            //, General.GetNullableInteger(ddlAuditorName.SelectedValue)
            , General.GetNullableInteger(ucAuditor.SelectedValue)
            , General.GetNullableString(txtBerth.Text)
            , General.GetNullableGuid(ddlCompany.SelectedValue)
            , General.GetNullableInteger(chkNILDeficiencies.Checked == true ? "1" : "0")
            , General.GetNullableString(ucInspector.Text)
            , General.GetNullableString(ucAuditor.Text)
            , General.GetNullableDateTime(ucAnniversaryDate.Text)
            , General.GetNullableInteger(ChkCredited.Checked == true ? "1" : "0")
            , General.GetNullableInteger(chkDetention.Checked == true ? "1" : "0")
            , General.GetNullableInteger(ddlOrganization.SelectedValue)
            , General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue)
            );
        ucStatus.Text = "Information updated.";
        BindAuditSchedule();
        gvDeficiency.Rebind();
        // SetPageNavigator();
    }

    protected void MenuInspectionScheduleGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG")) //planned
                {
                    SaveReviewDetails();
                    String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }
                else
                {
                    if (ViewState["reffrom"] != null && ViewState["reffrom"].ToString() != string.Empty)
                    {
                        SaveReviewDetails();
                        String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                    }
                    else
                    {
                        RadWindowManager1.RadConfirm("All deficiencies should be raised before completing the audit / inspection. Do you want to continue.?", "Confirm", 320, 150, null, "Confirm");
                    }
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
    protected void btnConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int nCurrentRow = int.Parse(ViewState["CURRENTROW"].ToString());

            RadLabel lblTypeid = ((RadLabel)gvDeficiency.Items[nCurrentRow].FindControl("lblTypeid"));
            RadLabel lblDeficiencyId = ((RadLabel)gvDeficiency.Items[nCurrentRow].FindControl("lblDeficiencyId"));

            if (lblTypeid != null && lblTypeid.Text == "1")
            {
                PhoenixInspectionAuditDirectNonConformity.DeleteAuditDirectNonConformity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)gvDeficiency.Items[nCurrentRow].FindControl("lblDeficiencyid")).Text));
            }
            else if (lblTypeid != null && lblTypeid.Text == "2")
            {
                PhoenixInspectionObservation.DeleteInspectionDirectObservation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)gvDeficiency.Items[nCurrentRow].FindControl("lblDeficiencyid")).Text));
            }
            ucStatus.Text = "Deficincy is deleted successfully.";
            gvDeficiency.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidAuditSchedule())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionAuditSchedule.UpdateReviewSchedule(
                 PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
                , General.GetNullableString(txtRemarks.Text)
                , General.GetNullableInteger(ddlStatus.SelectedHard)
                , General.GetNullableDateTime(txtCompletedDate.Text)
                , General.GetNullableInteger(ucPort.SelectedValue)
                , General.GetNullableInteger(rblLocation.SelectedValue)
                , General.GetNullableInteger(ucFromPort.SelectedValue)
                , General.GetNullableInteger(ucToPort.SelectedValue)
                //, General.GetNullableInteger(ddlInspectorName.SelectedValue)
                , General.GetNullableInteger(ucInspector.SelectedValue)
                , General.GetNullableString(txtOrganization.Text)
                , General.GetNullableString(txtExternalInspectorName.Text)
                , General.GetNullableString(txtExternalOrganisationName.Text)
                , General.GetNullableString(txtExternalInspectorDesignation.Text)
                //, General.GetNullableInteger(ddlAuditorName.SelectedValue)
                , General.GetNullableInteger(ucAuditor.SelectedValue)
                , General.GetNullableString(txtBerth.Text)
                , General.GetNullableGuid(ddlCompany.SelectedValue)
                , General.GetNullableInteger(chkNILDeficiencies.Checked == true ? "1" : "0")
                , General.GetNullableString(ucInspector.Text)
                , General.GetNullableString(ucAuditor.Text)
                , General.GetNullableDateTime(ucAnniversaryDate.Text)
                , General.GetNullableInteger(ChkCredited.Checked == true ? "1" : "0")
                , General.GetNullableInteger(chkDetention.Checked == true ? "1" : "0")
                , General.GetNullableInteger(ddlOrganization.SelectedValue)
                , General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue)
                );
            ucStatus.Text = "Information updated.";
            BindAuditSchedule();
            gvDeficiency.Rebind();
            String scriptupdate = String.Format("javascript:fnReloadList('Report',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
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

        if (General.GetNullableInteger(ddlStatus.SelectedHard) == null)
            ucError.ErrorMessage = "Status is required.";
        else if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
        {
            if (General.GetNullableDateTime(txtCompletedDate.Text) == null)
                ucError.ErrorMessage = "Date of Inspection is required.";
            else if (General.GetNullableDateTime(txtCompletedDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Date of Inspection cannot be the future date.";
        }
        if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
        {
            if (General.GetNullableDateTime(txtCompletedDate.Text) != null && General.GetNullableDateTime(txtCompletedDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Date of Inspection cannot be the future date.";
        }
        if (chkatsea.Checked == true)
        {
            if (ucFromPort.SelectedValue.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucFromPort.SelectedValue.ToString()))
                ucError.ErrorMessage = "From Port is required.";
            if (ucToPort.SelectedValue.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucToPort.SelectedValue.ToString()))
                ucError.ErrorMessage = "To Port is required.";
        }
        else
        {
            if (General.GetNullableInteger(ucPort.SelectedValue) == null)
                ucError.ErrorMessage = "Port is required.";
        }
        if (txtCategoryid.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")))
        {
            if (General.GetNullableString(txtExternalInspectorName.Text) == null)
                ucError.ErrorMessage = "External Auditor / Inspector is required.";
            if (General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue) == null)
                ucError.ErrorMessage = "External Auditor / Inspector Organization is required.";
        }
        else if (txtCategoryid.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")))
        {
            if (General.GetNullableString(ucInspector.Text) == null)
                ucError.ErrorMessage = "Internal Auditor / Inspector is required.";

            if (General.GetNullableInteger(ddlOrganization.SelectedValue) == null)
                ucError.ErrorMessage = "Internal Auditor / Inspector Organization is required.";
        }

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["AUDITSCHEDULEID"] = null;
        txtRemarks.Text = "";
    }

    protected void SetWidth()
    {
        RadComboBox ddlListStatus = (RadComboBox)ddlStatus.FindControl("ddlHard");
        Unit ucWidth = new Unit("150px");
        if (ddlListStatus != null)
            ddlListStatus.Attributes.Add("style", "width:95px;");
    }

    protected void chkatsea_CheckedChanged(object sender, EventArgs e)
    {
        if (chkatsea.Checked == true)
        {
            ucPort.SelectedValue = "";
            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            ucFromPort.CssClass = "input_mandatory";
            ucToPort.CssClass = "input_mandatory";
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
            ucFromPort.SelectedValue = "";
            ucToPort.SelectedValue = "";
            rblLocation.SelectedValue = "1";
            rblLocation.Enabled = true;
            txtBerth.CssClass = "input";
            txtBerth.Enabled = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["DEFICIENCYID"] = null;
            gvDeficiency.Rebind();
            //for (int i = 0; i < gvDeficiency.DataKeyNames.Length; i++)
            //{
            //    if (gvDeficiency.DataKeyNames[i] == (ViewState["DEFICIENCYID"] == null ? null : ViewState["DEFICIENCYID"].ToString()))
            //    {
            //        gvDeficiency.SelectedIndex = i;
            //        break;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindInternalInspector()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        //ddlInspectorName.DataSource = ds.Tables[0];
        //ddlInspectorName.DataTextField = "FLDDESIGNATIONNAME";
        //ddlInspectorName.DataValueField = "FLDUSERCODE";
        //ddlInspectorName.DataBind();
        //ddlInspectorName.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindInternalAuditor()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        //ddlAuditorName.DataSource = ds;
        //ddlAuditorName.DataTextField = "FLDDESIGNATIONNAME";
        //ddlAuditorName.DataValueField = "FLDUSERCODE";
        //ddlAuditorName.DataBind();
        //ddlAuditorName.Items.Insert(0, new ListItem("--Select--", "Dummy"));
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

            DataSet ds = PhoenixInspectionAuditSchedule.DeficiencySearch(int.Parse(lblVesselId.Text)
                , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
                , sortexpression, sortdirection,
                gvDeficiency.CurrentPageIndex + 1,
                gvDeficiency.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvDeficiency", "Deficiencies", alCaptions, alColumns, ds);

            gvDeficiency.DataSource = ds;
            gvDeficiency.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDeficiency(string deficiencytype, string deficiecncycategory, string checklistref, string desc, string status)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(deficiencytype) == null)
            ucError.ErrorMessage = "Deficiency Type is required.";

        if (General.GetNullableString(checklistref) == null)
            ucError.ErrorMessage = "Checklist Reference Number is required.";

        if (General.GetNullableString(desc) == null)
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableInteger(status) == null)
            ucError.ErrorMessage = "Status is required.";

        return (!ucError.IsError);
    }

    public void setvalue(RadDropDownList rb, string value)
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
            RadLabel lblDeficiencyId = (RadLabel)gvDeficiency.Items[rowindex].FindControl("lblDeficiencyId");
            if (lblDeficiencyId != null)
            {
                if (General.GetNullableGuid(lblDeficiencyId.Text) != null)
                    ViewState["DEFICIENCYID"] = lblDeficiencyId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        

        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
            }
            if (e.CommandName.ToUpper().Equals("SUPDTEVENTFEEDBACK"))
            {
                if (ViewState["VALIDYN"].ToString() != "1")
                {
                    ucError.ErrorMessage = "This event is older than 30 days, feedback must be given before 30 days.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    LinkButton imgFeedback = (LinkButton)e.Item.FindControl("imgSupdtEventFeedback");
                    RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblScheduleId");
                    RadLabel lblDeficiencyId = (RadLabel)e.Item.FindControl("lblDeficiencyId");

                    if (imgFeedback != null && lblScheduleId != null && lblDeficiencyId != null)
                    {
                        string script = "openNewWindow('Bank','','" + Session["sitepath"] + "/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=1&SOURCEREFERENCEID=" + lblScheduleId.Text + "&OTHERREFERENCEID=" + lblDeficiencyId.Text + "');";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                    }
                }
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)gvDeficiency.MasterTableView.GetItems(GridItemType.Footer)[0];

                RadComboBox ddlTypeAdd = (RadComboBox)item.FindControl("ddlTypeAdd");
                UserControlQuick ucNCCategoryAdd = (UserControlQuick)item.FindControl("ucNCCategoryAdd");
                string status = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");
                Guid? deficiencyid = null;

                if (ddlTypeAdd != null && (ddlTypeAdd.SelectedValue == "1" || ddlTypeAdd.SelectedValue == "2"))
                {
                    if (!IsValidDeficiency(ddlTypeAdd.SelectedValue, ucNCCategoryAdd.SelectedQuick,
                        ((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text,
                        ((RadTextBox)item.FindControl("txtDescAdd")).Text,
                        status))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionAuditSchedule.InsertNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeAdd.SelectedValue),
                        int.Parse(lblVesselId.Text),
                        General.GetNullableDateTime(txtCompletedDate.Text),
                        int.Parse(status),
                        General.GetNullableString(((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text),
                        General.GetNullableString(((RadTextBox)item.FindControl("txtDescAdd")).Text),
                        1, ref deficiencyid,
                        General.GetNullableGuid(((UserControlInspectionChapter)item.FindControl("ucChapterAdd")).SelectedChapter),
                        null,
                        General.GetNullableString(((RadTextBox)item.FindControl("txtKeyAdd")).Text), null,
                        General.GetNullableString(((RadTextBox)item.FindControl("txtItemAdd")).Text)
                        );
                }
                else if (ddlTypeAdd != null && ddlTypeAdd.SelectedValue == "3")
                {
                    if (!IsValidDeficiency(ddlTypeAdd.SelectedValue, ucNCCategoryAdd.SelectedQuick,
                        ((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text,
                        ((RadTextBox)item.FindControl("txtDescAdd")).Text,
                        status))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionAuditSchedule.InsertObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), int.Parse(lblVesselId.Text),
                        General.GetNullableDateTime(txtCompletedDate.Text),
                        int.Parse(status),
                         General.GetNullableString(((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text),
                        General.GetNullableString(((RadTextBox)item.FindControl("txtDescAdd")).Text),
                        1, ref deficiencyid, 1,
                        General.GetNullableGuid(((UserControlInspectionChapter)item.FindControl("ucChapterAdd")).SelectedChapter),
                        null,
                        General.GetNullableString(((RadTextBox)item.FindControl("txtKeyAdd")).Text), null,
                        General.GetNullableString(((RadTextBox)item.FindControl("txtItemAdd")).Text));
                }

                gvDeficiency.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CURRENTROW"] = e.Item.ItemIndex;
                RadLabel lblTypeid = ((RadLabel)e.Item.FindControl("lblTypeid"));
                RadLabel lblDeficiencyId = ((RadLabel)e.Item.FindControl("lblDeficiencyId"));

                if (lblTypeid != null && lblTypeid.Text == "1")
                {
                    PhoenixInspectionAuditDirectNonConformity.DeleteAuditDirectNonConformity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(((RadLabel)e.Item.FindControl("lblDeficiencyid")).Text));
                }
                else if (lblTypeid != null && lblTypeid.Text == "2")
                {
                    PhoenixInspectionObservation.DeleteInspectionDirectObservation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(((RadLabel)e.Item.FindControl("lblDeficiencyid")).Text));
                }
                ucStatus.Text = "Deficiency is deleted successfully.";

                String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);

                gvDeficiency.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadComboBox ddlTypeEdit = (RadComboBox)e.Item.FindControl("ddlTypeEdit");
                UserControlQuick ucNCCategoryEdit = (UserControlQuick)e.Item.FindControl("ucNCCategoryEdit");
                RadLabel lblDeficiencyId = (RadLabel)e.Item.FindControl("lblDeficiencyId");
                RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
                Guid? deficiencyid = new Guid(lblDeficiencyId.Text);

                if (ddlTypeEdit != null && (ddlTypeEdit.SelectedValue == "1" || ddlTypeEdit.SelectedValue == "2"))
                {
                    if (!IsValidDeficiency(ddlTypeEdit.SelectedValue, ucNCCategoryEdit.SelectedQuick,
                        ((RadTextBox)e.Item.FindControl("txtChecklistRefEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDescEdit")).Text,
                        ((RadLabel)e.Item.FindControl("lblStatus")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionAuditSchedule.InsertNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeEdit.SelectedValue),
                        int.Parse(lblVesselId.Text),
                        General.GetNullableDateTime(lblDate.Text),
                        int.Parse(((RadLabel)e.Item.FindControl("lblStatus")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistRefEdit")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescEdit")).Text),
                        1, ref deficiencyid,
                        General.GetNullableGuid(((UserControlInspectionChapter)e.Item.FindControl("ucChapterEdit")).SelectedChapter),
                        null,
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtKeyEdit")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucRCADateEdit")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtItemEdit")).Text));
                }
                else if (ddlTypeEdit != null && ddlTypeEdit.SelectedValue == "3")
                {
                    if (!IsValidDeficiency(ddlTypeEdit.SelectedValue, ucNCCategoryEdit.SelectedQuick,
                        ((RadTextBox)e.Item.FindControl("txtChecklistRefEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDescEdit")).Text,
                        ((RadLabel)e.Item.FindControl("lblStatus")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionAuditSchedule.InsertObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), int.Parse(lblVesselId.Text),
                        General.GetNullableDateTime(lblDate.Text),
                        int.Parse(((RadLabel)e.Item.FindControl("lblStatus")).Text),
                         General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistRefEdit")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescEdit")).Text),
                        1, ref deficiencyid, 1,
                        General.GetNullableGuid(((UserControlInspectionChapter)e.Item.FindControl("ucChapterEdit")).SelectedChapter),
                        null,
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtKeyEdit")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucRCADateEdit")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtItemEdit")).Text));
                }
                ucStatus.Text = "Deficiency is Updated successfully.";


                String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);

                gvDeficiency.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;
            }
            LinkButton ef = (LinkButton)e.Item.FindControl("imgSupdtEventFeedback");
            if (ef != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ef.CommandName)) ef.Visible = false;
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddltype = (RadComboBox)e.Item.FindControl("ddlTypeEdit");
            UserControlQuick ucNCCategoryEdit = (UserControlQuick)e.Item.FindControl("ucNCCategoryEdit");
            UserControlInspectionChapter ucChapterEdit = (UserControlInspectionChapter)e.Item.FindControl("ucChapterEdit");
            RadDropDownList ddlItemEdit = (RadDropDownList)e.Item.FindControl("ddlItemEdit");
            RadLabel lblRCADate = (RadLabel)e.Item.FindControl("lblRCADate");
            if (lblRCADate != null)
            {
                if (lblRCADate.Text == "")
                {
                    if (drv["FLDRCATARGETDATE"].ToString() == "")
                    {
                        lblRCADate.Text = "N/A";
                    }
                    else
                    {
                        lblRCADate.Text = drv["FLDRCATARGETDATE"].ToString();
                    }
                }
            }

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

            if (ucChapterEdit != null)
            {
                ucChapterEdit.InspectionId = ViewState["REVIEWID"].ToString();
                ucChapterEdit.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null, null, General.GetNullableGuid(ViewState["REVIEWID"].ToString()));
                ucChapterEdit.DataBind();
                ucChapterEdit.SelectedChapter = drv["FLDCHAPTERID"].ToString();
            }

            if (ddlItemEdit != null)
            {
                BindVIRItem(ddlItemEdit);
                ddlItemEdit.SelectedValue = drv["FLDITEM"].ToString();
            }

            if (Request.QueryString["viewonly"] != null && Request.QueryString["viewonly"].ToString().Equals("1"))
                gvDeficiency.Columns[11].Visible = false;
            else
                gvDeficiency.Columns[11].Visible = true;

            UserControlToolTip ucDescription = (UserControlToolTip)e.Item.FindControl("ucDescription");
            RadLabel lblDescription = (RadLabel)e.Item.FindControl("lblDescription");
            if (lblDescription != null)
            {
                lblDescription.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDescription.ToolTip + "', 'visible');");
                lblDescription.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDescription.ToolTip + "', 'hidden');");
            }

            UserControlToolTip ucItem = (UserControlToolTip)e.Item.FindControl("ucItem");
            RadLabel lblItem = (RadLabel)e.Item.FindControl("lblItem");
            if (lblItem != null)
            {
                lblItem.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucItem.ToolTip + "', 'visible');");
                lblItem.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucItem.ToolTip + "', 'hidden');");
            }

            RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblScheduleId");
            RadLabel lblDeficiencyId = (RadLabel)e.Item.FindControl("lblDeficiencyId");
            LinkButton imgSupdtEventFeedback = (LinkButton)e.Item.FindControl("imgSupdtEventFeedback");

            bool flag = true;
            if (imgSupdtEventFeedback != null)
            {
                flag = SessionUtil.CanAccess(this.ViewState, imgSupdtEventFeedback.CommandName);
                if (imgSupdtEventFeedback != null && lblDeficiencyId != null && lblScheduleId != null)
                {

                    if (General.GetNullableDateTime(txtCompletedDate.Text) == null)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback != 1)
                        {
                            flag = false;
                        }
                    }
                }

                if (flag == true)
                {
                    imgSupdtEventFeedback.Visible = true;
                }
                else
                {
                    imgSupdtEventFeedback.Visible = false;
                }
                if (Request.QueryString["viewonly"] != null)
                    imgSupdtEventFeedback.Visible = false;
            }

        }

        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlTypeAdd = (RadComboBox)e.Item.FindControl("ddlTypeAdd");
            UserControlQuick ucNCCategoryAdd = (UserControlQuick)e.Item.FindControl("ucNCCategoryAdd");
            UserControlInspectionChapter ucChapterAdd = (UserControlInspectionChapter)e.Item.FindControl("ucChapterAdd");
            UserControlDate ucDateAdd = (UserControlDate)e.Item.FindControl("ucDateAdd");
            RadDropDownList ddlItemAdd = (RadDropDownList)e.Item.FindControl("ddlItemAdd");

            if (ucNCCategoryAdd != null)
            {
                ucNCCategoryAdd.Visible = true;
                ucNCCategoryAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                ucNCCategoryAdd.DataBind();
            }
            if (ucChapterAdd != null)
            {
                ucChapterAdd.InspectionId = ViewState["REVIEWID"].ToString();
                ucChapterAdd.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null, null, General.GetNullableGuid(ViewState["REVIEWID"].ToString()));
                ucChapterAdd.DataBind();
            }
            if (ucDateAdd != null)
                ucDateAdd.Text = txtCompletedDate.Text;

            if (ddlTypeAdd != null)
            {
                if (txtInspection.Text.ToUpper().Contains("VIR"))
                    ddlTypeAdd.SelectedValue = "3";
                else
                    ddlTypeAdd.SelectedValue = "2";

                if (ViewState["PSCINSPECTIONYN"].ToString() == "1")
                {
                    ddlTypeAdd.SelectedValue = "2";
                }
            }

            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName)) cmdAdd.Visible = false;
            }

            if (ddlItemAdd != null)
            {
                BindVIRItem(ddlItemAdd);
            }
        }
    }

    protected void rblLocation_Changed(object sender, EventArgs e)
    {
        if (rblLocation.SelectedValue == "1")
        {
            txtBerth.Enabled = true;
            txtBerth.CssClass = "input";
        }
        else
        {
            txtBerth.Enabled = false;
            txtBerth.Text = "";
            txtBerth.CssClass = "readonlytextbox";
        }
    }

    protected void txtCompletedDate_Changed(object sender, EventArgs e)
    {
        if (ViewState["reffrom"] == null || ViewState["reffrom"].ToString() == "")
        {
            if (General.GetNullableDateTime(txtCompletedDate.Text) != null)
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
            else
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "ASG");
        }
        else
        {
            if (General.GetNullableDateTime(txtCompletedDate.Text) != null)
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
            else
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
        }
    }

    protected void BindInternalOrganization()
    {
        ddlOrganization.DataSource = PhoenixInspectionOrganization.InspectionOrganizationList(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")));
        ddlOrganization.DataTextField = "FLDORGANIZATIONNAME";
        ddlOrganization.DataValueField = "FLDORGANIZATIONID";
        ddlOrganization.DataBind();
        ddlOrganization.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindExternalOrganization()
    {
        ddlExternalOrganizationName.DataSource = PhoenixInspectionOrganization.InspectionOrganizationList(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));
        ddlExternalOrganizationName.DataTextField = "FLDORGANIZATIONNAME";
        ddlExternalOrganizationName.DataValueField = "FLDORGANIZATIONID";
        ddlExternalOrganizationName.DataBind();
        ddlExternalOrganizationName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindCompany()
    {
        ddlCompany.DataSource = PhoenixInspectionInspectingCompany.ListAuditInspectionCompany(null, General.GetNullableGuid(ViewState["REVIEWID"] != null ? ViewState["REVIEWID"].ToString() : ""));
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void chkNILDeficiencies_CheckedChanged(object sender, EventArgs e)
    {
        if (chkNILDeficiencies.Checked == true)
            gvDeficiency.Enabled = false;
        else
            gvDeficiency.Enabled = true;
    }

    protected void BindVIRItem(RadDropDownList ddl)
    {
        ddl.DataSource = PhoenixInspectionVIRItem.InspectionVIRItemTreeList();
        ddl.DataTextField = "FLDITEMNAME";
        ddl.DataValueField = "FLDITEMID";
        ddl.DataBind();
        ddl.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }

    protected void SupdtFeedback_Click(object sender, EventArgs e)
    {
        if (ViewState["VALIDYN"].ToString() != "1")
        {
            ucError.ErrorMessage = ViewState["VALIDDATIONMSG"].ToString();
            ucError.Visible = true;
            return;
        }

        //imgSupdtEventFeedback.Attributes.Add("onclick", "openNewWindow('Bank','','" + Session["sitepath"] + "/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=1&SOURCEREFERENCEID=" + ViewState["AUDITSCHEDULEID"].ToString() + "');return true;");

        else
        {
            string script = "openNewWindow('Bank','','" + Session["sitepath"] + "/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=1&SOURCEREFERENCEID=" + ViewState["AUDITSCHEDULEID"].ToString() + "');";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
        }
    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeficiency.CurrentPageIndex + 1;
        BindData();
    }
}
