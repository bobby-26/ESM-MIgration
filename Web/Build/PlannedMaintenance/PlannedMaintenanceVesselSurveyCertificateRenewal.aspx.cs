using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class PlannedMaintenanceVesselSurveyCertificateRenewal : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();            
            toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCertificatesRenewal.AccessRights = this.ViewState;
            MenuCertificatesRenewal.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {                
                ViewState["TEMPLATEID"] = "";
                ViewState["ATTACHMENTTYPE"] = "SURVEYCERTIFICATE";
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["DTKEY"] = Request.QueryString["dtkey"];
                ViewState["CID"] = Request.QueryString["cid"];
                ViewState["VSLID"] = Request.QueryString["vslid"] != null ? Request.QueryString["vslid"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                EditVesselCertificate();            
                
                txtNumber.Focus();                                
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindSurveyType(RadComboBox ddl)
    {
        try
        {
            ddl.Items.Clear();
            ddl.DataSource = PhoenixRegisterVesselSurveyConfiguration.ListTemplateSurveyType(General.GetNullableGuid(ViewState["TEMPLATEID"].ToString()));
            ddl.DataValueField = "FLDSURVEYTYPEID";
            ddl.DataTextField = "FLDSURVEYTYPENAME";
            ddl.DataBind();
            ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
            
    private void EditVesselCertificate()
    {
        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.EditVesselCertificate(int.Parse(ViewState["VSLID"].ToString())
            , General.GetNullableInteger(ViewState["CID"].ToString()).Value
            , General.GetNullableGuid(ViewState["DTKEY"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCertificate.Text = dr["FLDCERTIFICATENAME"].ToString().Trim();
            txtCertificate.ToolTip = dr["FLDCERTIFICATENAME"].ToString().Trim();
            txtNumber.Text = dr["FLDCERTIFICATENO"].ToString().Trim();
            ucIssueDate.Text = dr["FLDDATEOFISSUE"].ToString().Trim();
            ucExpiryDate.Text = dr["FLDDATEOFEXPIRY"].ToString().Trim();
            ucIssuingAuthority.Text = dr["FLDISSUINGAUTHORITYNAME"].ToString().Trim();
            ucIssuingAuthority.SelectedValue = dr["FLDISSUINGAUTHORITY"].ToString().Trim();
            ddlPort.SelectedValue = dr["FLDISSUEDPORT"].ToString().Trim();
            ddlPort.Text = dr["FLDSEAPORTNAME"].ToString().Trim();
            txtSurveyorName.Text = dr["FLDSURVEYORNAME"].ToString().Trim();
            ucRemarks.SelectedQuick = dr["FLDREMARKS"].ToString().Trim();
            txtCategory.Text = dr["FLDCATEGORYNAME"].ToString().Trim();
            ucDoneDate.Text = dr["FLDLASTDONEDATE"].ToString().Trim();
            txtSurveyType.Text = dr["FLDSURVEYTYPENAME"].ToString().Trim();
            //chkAuditLog.Checked = dr["FLDUPDATEAUDITLOG"].ToString().Trim().Equals("1") ? true : false;
            txtRemarks.Text = dr["FLDCERTIFICATEREMARKS"].ToString().Trim();
            ChkNotApplicable.Checked = dr["FLDNOTAPPLICABLE"].ToString().Trim().Equals("1") ? true : false;
            txtReason.Text = dr["FLDNOTAPPLICABLEREASON"].ToString().Trim();
            chkVerifyYN.Checked = dr["FLDATTACHCORRECTYN"].ToString().Trim().Equals("1") ? true : false;
            ddlSurveyPort.SelectedValue = dr["FLDPLANPORT"].ToString().Trim();
            ddlSurveyPort.Text = dr["FLDSURVEYPORTNAME"].ToString().Trim();
            chkNoExpiry.Checked = dr["FLDNOEXPIRY"].ToString().Trim().Equals("1") ? true : false;
            ucExpiryDate.ReadOnly = chkNoExpiry.Checked;
            ucExpiryDate.Text = chkNoExpiry.Checked ? "" : dr["FLDDATEOFEXPIRY"].ToString().Trim();
            txtPlanDate.Text = dr["FLDPLANDATE"].ToString();
            ucExpiryDate.CssClass = chkNoExpiry.Checked ? "readonlytextbox" : "input";
            ucLastSurveyDate.Text = dr["FLDLASTSURVEYDATE"].ToString();
            txtInitialDate.Text = dr["FLDINITIALDATE"].ToString();
            txtPlanRemarks.Text = dr["FLDPLANREMARKS"].ToString();

            ViewState["AUDITREQ"] = dr["FLDAUDITREQUIREDYN"].ToString();
            ViewState["INITIALDTKEY"] = dr["FLDINITIALDTKEY"].ToString();
            ViewState["TEMPLATEID"] = dr["FLDTEMPLATEID"].ToString();
            ViewState["BEFORE"] = dr["FLDWINDOWBEFORE"].ToString();
            ViewState["AFTER"] = dr["FLDWINDOWAFTER"].ToString();
            ViewState["RESTARTYN"] = dr["FLDRESTARTYN"].ToString();
            BindSurveyType(ddlLastSurveyType);

            if (dr["FLDCATEGORYCODE"].ToString() == "SC")
            {
                ViewState["ATTACHMENTTYPE"] = "STATUTORYCERTIFICATE";
            }
            else
            {
                ViewState["ATTACHMENTTYPE"] = "SURVEYCERTIFICATE";
            }

            if (!string.IsNullOrEmpty(dr["FLDFILEPATH"].ToString()))
            {
                ViewState["FILEPATH"] = dr["FLDFILEPATH"].ToString();
            }
            if (dr["FLDAUDITREQUIREDYN"].ToString() == "1")
            {
                ddlLastSurveyType.SelectedValue = dr["FLDLASTAUDIT"].ToString().ToUpper();               
            }
            else
            {                               
                ddlLastSurveyType.SelectedValue = dr["FLDLASTSURVEYTYPEID"].ToString();                
            }
            if (dr["FLDUSEANNIVERSARYDATE"].ToString() == "1")
            {
                txtInitialDate.CssClass = "readonlytextbox";
                txtInitialDate.Enabled = false;
            }
            else
            {
                txtInitialDate.CssClass = "input";
                txtInitialDate.Enabled = true;
            }
            if (ViewState["DTKEY"].ToString() != string.Empty)
            {
                DataTable dta = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["DTKEY"].ToString()), ViewState["ATTACHMENTTYPE"].ToString());
                ViewState["ATTDTKEY"] = string.Empty;
                if (dta.Rows.Count > 0)
                    ViewState["ATTDTKEY"] = dta.Rows[0]["FLDDTKEY"].ToString();
                else
                {
                    DataTable atd = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["DTKEY"].ToString()), "SURVEYCERTIFICATE");
                    if (atd.Rows.Count > 0)
                        ViewState["ATTDTKEY"] = atd.Rows[0]["FLDDTKEY"].ToString();
                }
            }
            if (dr["FLDLASTSCHEDULEID"].ToString() != string.Empty)
            {
                ddlLastSurveyType.Enabled = false;
                ddlLastSurveyType.CssClass = "readonlytextbox";
                ucLastSurveyDate.Enabled = false;
                ucLastSurveyDate.CssClass = "readonlytextbox";
            }
           
            if (dr["FLDSEQUENCE"].ToString().Equals("1"))
            {
                ddlLastSurveyType.Enabled = false;
                ddlLastSurveyType.CssClass = "readonlytextbox";
                ucLastSurveyDate.Enabled = false;
                ucLastSurveyDate.CssClass = "readonlytextbox";                
            }

            DisableLastSurvey(dr["FLDNOEXPIRY"].ToString(), dr["FLDTEMPLATEID"].ToString(), ViewState["RESTARTYN"].ToString(), txtInitialDate.Text);
    
        }
    }

    protected void MenuCertificatesRenewal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails(txtNumber.Text, ucIssueDate.Text, ucExpiryDate.Text, ucIssuingAuthority.SelectedValue,
                    ChkNotApplicable.Checked, txtReason.Text.Trim(), ucDoneDate.Text, ucLastSurveyDate.Text, ddlLastSurveyType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                Guid? g = null;
                if (ViewState["DTKEY"].ToString() != string.Empty)
                {
                    g = new Guid(ViewState["DTKEY"].ToString());

                    if (General.GetNullableDateTime(txtInitialDate.Text).HasValue)
                    {
                        PhoenixPlannedMaintenanceSurveySchedule.InsertVesselCertificateInitialAudit(int.Parse(ViewState["VSLID"].ToString())
                                                                                               , General.GetNullableInteger(ViewState["CID"].ToString()).Value
                                                                                               , General.GetNullableGuid(ViewState["INITIALDTKEY"].ToString())
                                                                                               , DateTime.Parse(txtInitialDate.Text)
                                                                                               , null//ViewState["AUDITREQ"].ToString() == "1" ? General.GetNullableGuid(ddlLastSurveyType.SelectedValue) : null
                                                                                               , null//ViewState["AUDITREQ"].ToString() != "1" ? General.GetNullableInteger(ddlLastSurveyType.SelectedValue) : null
                                                                                               );
                    }

                    PhoenixPlannedMaintenanceSurveySchedule.UpdateVesselSurveyCertificate(int.Parse(ViewState["VSLID"].ToString())
                                                               , General.GetNullableInteger(ViewState["CID"].ToString())
                                                               , General.GetNullableString(txtNumber.Text.Trim())
                                                               , General.GetNullableDateTime(ucIssueDate.Text)
                                                               , General.GetNullableDateTime(ucExpiryDate.Text)
                                                               , General.GetNullableInteger(ucIssuingAuthority.SelectedValue)
                                                               , General.GetNullableInteger(ddlPort.SelectedValue)
                                                               , General.GetNullableInteger(ucRemarks.SelectedQuick)
                                                               , General.GetNullableString(txtRemarks.Text.Trim())
                                                               , General.GetNullableInteger("0")
                                                               , General.GetNullableInteger((ChkNotApplicable.Checked ? "1" : "0"))
                                                               , General.GetNullableString(txtReason.Text.Trim())
                                                               , General.GetNullableInteger((chkVerifyYN.Checked ? "1" : "0"))
                                                               , General.GetNullableInteger((chkNoExpiry.Checked ? "1" : "0"))
                                                               , General.GetNullableDateTime(ucLastSurveyDate.Text)
                                                               , ViewState["AUDITREQ"].ToString() != "1" ? General.GetNullableInteger(ddlLastSurveyType.SelectedValue) : null
                                                               , ViewState["AUDITREQ"].ToString() == "1" ? General.GetNullableGuid(ddlLastSurveyType.SelectedValue) : null
                                                               , ref g
                                                               , General.GetNullableInteger(ddlSurveyPort.SelectedValue)
                                                               , General.GetNullableDateTime(txtPlanDate.Text)
                                                               , txtSurveyorName.Text.Trim()
                                                               , General.GetNullableDateTime(ucDoneDate.Text)
                                                               , txtPlanRemarks.Text
                                                               );

                    if (ViewState["ATTDTKEY"].ToString() != string.Empty && PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE") // Replace old attachment in offshore
                    {
                        PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["ATTDTKEY"].ToString()), PhoenixModule.PLANNEDMAINTENANCE);
                    }
                    else
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files, g.Value, PhoenixModule.PLANNEDMAINTENANCE, null, null, string.Empty, ViewState["ATTACHMENTTYPE"].ToString(), int.Parse(ViewState["VSLID"].ToString()));
                    }
                }
                else if (ViewState["DTKEY"].ToString() == string.Empty)
                {
                    if (General.GetNullableDateTime(txtInitialDate.Text).HasValue)
                    {
                        PhoenixPlannedMaintenanceSurveySchedule.InsertVesselCertificateInitialAudit(int.Parse(ViewState["VSLID"].ToString())
                                                                                                , General.GetNullableInteger(ViewState["CID"].ToString()).Value
                                                                                                , General.GetNullableGuid(ViewState["INITIALDTKEY"].ToString())
                                                                                                , DateTime.Parse(txtInitialDate.Text)
                                                                                                , null//ViewState["AUDITREQ"].ToString() == "1" ? General.GetNullableGuid(ddlLastSurveyType.SelectedValue) : null
                                                                                                , null//ViewState["AUDITREQ"].ToString() != "1" ? General.GetNullableInteger(ddlLastSurveyType.SelectedValue) : null
                                                                                                );
                    }

                    PhoenixPlannedMaintenanceSurveySchedule.InsertVesselSurveyCertificate(int.Parse(ViewState["VSLID"].ToString())
                                                              , General.GetNullableInteger(ViewState["CID"].ToString())
                                                              , General.GetNullableString(txtNumber.Text.Trim())
                                                              , General.GetNullableDateTime(ucIssueDate.Text)
                                                              , General.GetNullableDateTime(ucExpiryDate.Text)
                                                              , General.GetNullableInteger(ucIssuingAuthority.SelectedValue)
                                                              , General.GetNullableInteger(ddlPort.SelectedValue)
                                                              , General.GetNullableInteger(ucRemarks.SelectedQuick)
                                                              , General.GetNullableString(txtRemarks.Text.Trim())
                                                              , General.GetNullableInteger("0")
                                                              , General.GetNullableInteger((ChkNotApplicable.Checked ? "1" : "0"))
                                                              , General.GetNullableString(txtReason.Text.Trim())
                                                              , General.GetNullableInteger((chkVerifyYN.Checked ? "1" : "0"))
                                                              , General.GetNullableInteger((chkNoExpiry.Checked ? "1" : "0"))
                                                              , General.GetNullableDateTime(ucLastSurveyDate.Text)
                                                              , ViewState["AUDITREQ"].ToString() != "1" ? General.GetNullableInteger(ddlLastSurveyType.SelectedValue) : null
                                                              , ViewState["AUDITREQ"].ToString() == "1" ? General.GetNullableGuid(ddlLastSurveyType.SelectedValue) : null
                                                              , ref g
                                                              , General.GetNullableInteger(ddlSurveyPort.SelectedValue)
                                                              , General.GetNullableDateTime(txtPlanDate.Text)
                                                              , txtSurveyorName.Text.Trim()
                                                              , General.GetNullableDateTime(ucDoneDate.Text)
                                                              , txtPlanRemarks.Text
                                                              );

                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, g.Value, PhoenixModule.PLANNEDMAINTENANCE, null, null, string.Empty, ViewState["ATTACHMENTTYPE"].ToString(), int.Parse(ViewState["VSLID"].ToString()));
                }

                ViewState["DTKEY"] = g;
                //EditVesselCertificate();
                ucStatus.Text = "Certificate Details Updated Sucessfully";
                ucStatus.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtCertificate.Text = string.Empty;
                txtCertificate.ToolTip = string.Empty;
                txtNumber.Text = string.Empty;
                ucIssueDate.Text = string.Empty;
                ucExpiryDate.Text = string.Empty;
                ucIssuingAuthority.Text = string.Empty;
                ucIssuingAuthority.SelectedValue = string.Empty;
                ddlPort.SelectedValue = string.Empty;
                ddlPort.Text = string.Empty;
                txtSurveyorName.Text = string.Empty;
                ucRemarks.SelectedQuick = string.Empty;
                txtCategory.Text = string.Empty;
                ucDoneDate.Text = string.Empty;
                txtSurveyType.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                ChkNotApplicable.Checked = false;
                txtReason.Text = string.Empty;
                chkVerifyYN.Checked = false;
                ddlSurveyPort.SelectedValue = string.Empty;
                ddlSurveyPort.Text = string.Empty;
                chkNoExpiry.Checked = false;
                ucExpiryDate.ReadOnly = false;
                ucExpiryDate.Text = string.Empty;
                txtPlanDate.Text = string.Empty;
                ucExpiryDate.CssClass = "input";
                ucLastSurveyDate.Text = string.Empty;
                txtPlanRemarks.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDetails(
        string certificateno
        , string dateofissue
        , string dateofexpiry
        , string issuingauthority
        , bool VesselNotApplicable
        , string Reason
        , string DoneDate
        , string LastSurveyDate
        , string LastSurveyType)
    {
        DateTime resultDate;
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (certificateno.Trim().Equals("") && !VesselNotApplicable)
            ucError.ErrorMessage = "Certificate Number is required.";

        if (!DateTime.TryParse(dateofissue, out resultDate) && !VesselNotApplicable)
            ucError.ErrorMessage = "Issue Date is required.";

        if (!Int16.TryParse(issuingauthority, out resultInt) && !VesselNotApplicable)
            ucError.ErrorMessage = "Issuing Authority is required.";
        if (dateofissue != null && !ChkNotApplicable.Checked)
        {
            if (DateTime.Parse(dateofissue) > DateTime.Today)
            {
                ucError.ErrorMessage = "Issue Date should be earlier than Current Date.";
            }
        }
        if (dateofissue != null && dateofexpiry != null && !VesselNotApplicable)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'.";
        }
        if (VesselNotApplicable && Reason.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Reason why not applicable is required.";
        }
        else if (VesselNotApplicable && (General.GetNullableDateTime(txtPlanDate.Text).HasValue 
                                        || !txtSurveyorName.Text.Trim().Equals("") 
                                        || General.GetNullableInteger(ddlSurveyPort.SelectedValue).HasValue))
        {
            ucError.ErrorMessage = "Not applicable certificate cannot be planned.";
        }
        else if (VesselNotApplicable && (General.GetNullableDateTime(ucLastSurveyDate.Text).HasValue                                       
                                       || General.GetNullableInteger(ddlLastSurveyType.SelectedValue).HasValue))
        {
            ucError.ErrorMessage = "Not applicable certificate cannot have Last Survey.";
        }
        if (DoneDate != null && !VesselNotApplicable)
        {
            if (DateTime.Parse(DoneDate) > DateTime.Today)
            {
                ucError.ErrorMessage = "Done Date should be earlier then Current Date.";
            }
        }
        if (General.GetNullableDateTime(txtPlanDate.Text).HasValue
            && General.GetNullableDateTime(ViewState["BEFORE"].ToString()).HasValue
            && General.GetNullableDateTime(ViewState["AFTER"].ToString()).HasValue
            && !VesselNotApplicable)
        {
            if (DateTime.Parse(txtPlanDate.Text) < DateTime.Parse(ViewState["BEFORE"].ToString()) || DateTime.Parse(txtPlanDate.Text) > DateTime.Parse(ViewState["AFTER"].ToString()))
            {
                ucError.ErrorMessage = "Planned Date should fall between Window(Range) " + General.GetDateTimeToString(ViewState["BEFORE"].ToString()) + " - " + General.GetDateTimeToString(ViewState["AFTER"].ToString()) + " Date.";
            }
        }

        if (General.GetNullableDateTime(txtInitialDate.Text).HasValue && !DateTime.TryParse(txtInitialDate.Text, out resultDate) && !VesselNotApplicable)
            ucError.ErrorMessage = "Anniversary /Initial Audit Date is required.";

        if (!General.GetNullableInteger(ucRemarks.SelectedQuick).HasValue && !VesselNotApplicable)
            ucError.ErrorMessage = "Certificate Status is required.";
        if (General.GetNullableString(ddlLastSurveyType.SelectedValue) != null || General.GetNullableDateTime(ucLastSurveyDate.Text).HasValue)
        {
            if (General.GetNullableString(ddlLastSurveyType.SelectedValue) == null && ddlLastSurveyType.CssClass == "input_mandatory" && !VesselNotApplicable)
                ucError.ErrorMessage = "Last Audit/Survey Type  is required.";

            if (General.GetNullableDateTime(ucLastSurveyDate.Text) == null && ucLastSurveyDate.CssClass == "input_mandatory" && !VesselNotApplicable)
                ucError.ErrorMessage = "Last Audit/Survey Date is required.";
            if (General.GetNullableDateTime(ucLastSurveyDate.Text).HasValue && ucLastSurveyDate.CssClass == "input_mandatory"
                    && (DateTime.Parse(ucLastSurveyDate.Text) > DateTime.Today) && !VesselNotApplicable)
                ucError.ErrorMessage = "Last Audit/Survey Date should be earlier than Current Date.";
        }
        return (!ucError.IsError);
    }
    protected void chkNoExpiry_CheckedChange(object sender, EventArgs e)
    {
        ucExpiryDate.Text = chkNoExpiry.Checked ? "" : "";
        ucExpiryDate.ReadOnly = chkNoExpiry.Checked;
        ucExpiryDate.CssClass = chkNoExpiry.Checked ? "readonlytextbox" : "input";
        DisableLastSurvey(chkNoExpiry.Checked ? "1" : "0", ViewState["TEMPLATEID"].ToString(), ViewState["RESTARTYN"].ToString(), txtInitialDate.Text);
    }
   
    private void DisableLastSurvey(string noexpiry, string validtycycle, string restartyn, string initialdate)
    {
        if (noexpiry.Equals("1") || validtycycle.Equals("") || restartyn.Equals("1") || restartyn.Equals("2") || restartyn.Equals("3") || !General.GetNullableDateTime(initialdate).HasValue)
        {
            ddlLastSurveyType.Enabled = false;
            ddlLastSurveyType.CssClass = "readonlytextbox";

            ucLastSurveyDate.Enabled = false;
            ucLastSurveyDate.CssClass = "readonlytextbox";
        }
        else
        {
            ddlLastSurveyType.Enabled = true;
            ddlLastSurveyType.CssClass = "input_mandatory";

            ucLastSurveyDate.Enabled = true;
            ucLastSurveyDate.CssClass = "input_mandatory";
        }
    }

}
