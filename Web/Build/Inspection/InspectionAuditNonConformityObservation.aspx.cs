using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;

public partial class InspectionAuditNonConformityObservation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtSupt.Attributes.Add("style", "visibility:hidden");
        txtSuptEmailHidden.Attributes.Add("style", "visibility:hidden");
        if (!IsPostBack)
        {
            ViewState["OBSID"] = null;
            imgShowSupt.Attributes.Add("onclick", "return showPickList('spnPickListSupt', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
            + PhoenixModule.QUALITY + "', true);");

            if (Request.QueryString["RecordResponseId"] != null && Request.QueryString["RecordResponseId"].ToString() != string.Empty)
            {
                ViewState["RecordResponseId"] = Request.QueryString["RecordResponseId"].ToString();
                EditAuditSchedule();
                EditObservation();                
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
            {
                if (ViewState["OBSID"] != null && !string.IsNullOrEmpty(ViewState["OBSID"].ToString()))
                    toolbar.AddImageLink("javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                                        + PhoenixModule.QUALITY + "&type=" + "INSPECTIONOBS" + "&cmdname=OBSUPLOAD&U=1&VESSELID=" + ViewState["VESID"] + "'); return false;", "Attachments", "", "ATTACHMENTS");
                MenuInspectionObsComments.MenuList = toolbar.Show();
            }
            else
            {
                toolbar.AddButton("Save", "SAVE");
                toolbar.AddButton("Verify", "VERIFY");
                if (ViewState["OBSID"] != null && !string.IsNullOrEmpty(ViewState["OBSID"].ToString()))
                    toolbar.AddImageLink("javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                                        + PhoenixModule.QUALITY + "&type=" + "INSPECTIONOBS" + "&cmdname=OBSUPLOAD&VESSELID=" + ViewState["VESID"] + "'); return false;", "Attachments", "", "ATTACHMENTS");
                MenuInspectionObsComments.MenuList = toolbar.Show();
            }
        }
        BindPreventiveAction();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void MenuInspectionObsComments_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {                
                if (!IsValidObservation())
                {
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentAuditScheduleId != null && Filter.CurrentAuditScheduleId.ToString() != string.Empty)
                {
                    string raisedfrom = "1"; //audit/inspection
                    PhoenixInspectionAuditNonConformity.InsertNonConformityObservation(
                                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , new Guid(Filter.CurrentAuditScheduleId.ToString())
                                                                            , new Guid(ViewState["RecordResponseId"].ToString())
                                                                            , int.Parse(ViewState["VESSELID"].ToString())                                                                            
                                                                            , txtObservation.Text.Trim()
                                                                            , General.GetNullableString(txtComments.Text)
                                                                            , General.GetNullableString(txtOwnerComments.Text)
                                                                            , General.GetNullableString(txtOperatorComments.Text)
                                                                            , int.Parse(ucRiskCategory.SelectedQuick)
                                                                            , General.GetNullableDateTime(txtOperatorCommentsDate.Text)
                                                                            , null
                                                                            , null
                                                                            , null
                                                                            , null
                                                                            , null
                                                                            , int.Parse(raisedfrom)
                                                                            ,General.GetNullableInteger(ucVerficationLevel.SelectedHard)
                                                                            );
                    ucStatus.Text = "Observation details updated.";
                }
                EditObservation();

            }
            if (dce.CommandName.ToUpper().Equals("VERIFY"))
            {
                if (!IsValidObservation())
                {
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentAuditScheduleId != null && Filter.CurrentAuditScheduleId.ToString() != string.Empty)
                {
                    string raisedfrom = "1"; //audit/inspection
                    PhoenixInspectionAuditNonConformity.InsertNonConformityObservation(
                                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , new Guid(Filter.CurrentAuditScheduleId.ToString())
                                                                            , new Guid(ViewState["RecordResponseId"].ToString())
                                                                            , int.Parse(ViewState["VESSELID"].ToString())
                                                                            , txtObservation.Text.Trim()
                                                                            , General.GetNullableString(txtComments.Text)
                                                                            , General.GetNullableString(txtOwnerComments.Text)
                                                                            , General.GetNullableString(txtOperatorComments.Text)
                                                                            , int.Parse(ucRiskCategory.SelectedQuick)
                                                                            , General.GetNullableDateTime(txtOperatorCommentsDate.Text)
                                                                            , null
                                                                            , null
                                                                            , null
                                                                            , null
                                                                            , null
                                                                            , int.Parse(raisedfrom)
                                                                            , General.GetNullableInteger(ucVerficationLevel.SelectedHard)
                                                                            );                    
                }
                
                if (!IsValidVerification())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionAuditNonConformity.ReviewActionVerificationUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(Filter.CurrentAuditScheduleId.ToString())
                                                                , new Guid(ViewState["RecordResponseId"].ToString())
                                                                , new Guid(lblObservationid.Text)
                                                                , General.GetNullableDateTime(txtVerificationDate.Text)
                                                                , General.GetNullableInteger(txtSupt.Text)
                                                                , 2
                                                                //, General.GetNullableInteger(ucVerficationLevel.SelectedHard)
                                                                );
               
                ucStatus.Text = "Observation has been verified.";
                EditObservation();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidVerification()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVerficationLevel.SelectedHard) == null)
            ucError.ErrorMessage = "Verification level is required.";

        if (string.IsNullOrEmpty(txtSuptName.Text))
            ucError.ErrorMessage = "'Verification by Superintendent' is required.";

        if (string.IsNullOrEmpty(txtVerificationDate.Text))
            ucError.ErrorMessage = "'Verification Date' is required.";

        if (!string.IsNullOrEmpty(txtVerificationDate.Text))
        {
            if (General.GetNullableDateTime(txtVerificationDate.Text) != null && General.GetNullableDateTime(txtVerificationDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Verification Date can not be the future date.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidObservation()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucRiskCategory.SelectedQuick) == null)
            ucError.ErrorMessage = "Risk category is required.";

        if (txtObservation.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Observation is required.";

        if (!string.IsNullOrEmpty(txtOperatorCommentsDate.Text))
        {
            if (General.GetNullableDateTime(txtOperatorCommentsDate.Text) != null && General.GetNullableDateTime(txtOperatorCommentsDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Operator Comments submitted on can not be the future date.";
        }
                        
        return (!ucError.IsError);
    }

    protected void EditObservation()
    {
        DataSet ds = PhoenixInspectionAuditNonConformity.EditNonConformityObservation(new Guid(ViewState["RecordResponseId"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtObservation.Text = dr["FLDOBSERVATION"].ToString();
            lblObservationid.Text = dr["FLDREVIEWNCOBSERVATIONID"].ToString();
            txtVerificationDate.Text = dr["FLDCLOSEOUTVERIFIEDDATE"].ToString();
            txtSupt.Text = dr["FLDCLOSEOUTVERIFIEDBY"].ToString();
            txtSuptName.Text = dr["FLDCLOSEOUTVERIFIEDBYNAME"].ToString();
            txtSuptDesignation.Text = dr["FLDCLOSEOUTVERIFIEDBYDESIGNATION"].ToString();
            ViewState["OBSID"] = dr["FLDREVIEWNCOBSERVATIONID"].ToString();
            ViewState["VESID"] = dr["FLDVESSELID"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();
            ucRiskCategory.SelectedQuick = dr["FLDRISKCATEGORYID"].ToString();
            txtComments.Text = dr["FLDCOMMENTS"].ToString();
            txtOperatorComments.Text = dr["FLDOPERATORRESPONSE"].ToString();
            txtOwnerComments.Text = dr["FLDOWNERCOMMENTS"].ToString();
            txtOperatorCommentsDate.Text = General.GetDateTimeToString(dr["FLDOPERATORCOMMENTSSUBMITTEDON"].ToString());
            txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
        }
        ds = null;
        ds = PhoenixInspectionAuditRecordAndResponse.EditAuditRecordAndResponse(new Guid(ViewState["RecordResponseId"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtChecklistRefNo.Text = ds.Tables[0].Rows[0]["FLDCHECKLISTREFERENCENUMBER"].ToString();
        }

        if (ViewState["DTKEY"] != null && !string.IsNullOrEmpty(ViewState["DTKEY"].ToString()))
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
            {
                if (ViewState["OBSID"] != null && !string.IsNullOrEmpty(ViewState["OBSID"].ToString()))
                    toolbar.AddImageLink("javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                                        + PhoenixModule.QUALITY + "&type=" + "INSPECTIONOBS" + "&cmdname=OBSUPLOAD&U=1&VESSELID=" + ViewState["VESID"] + "'); return false;", "Attachments", "", "ATTACHMENTS");
                MenuInspectionObsComments.MenuList = toolbar.Show();
            }
            else
            {
                toolbar.AddButton("Save", "SAVE");
                toolbar.AddButton("Verify", "VERIFY");
                if (ViewState["OBSID"] != null && !string.IsNullOrEmpty(ViewState["OBSID"].ToString()))
                    toolbar.AddImageLink("javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&mod="
                                        + PhoenixModule.QUALITY + "&type=" + "INSPECTIONOBS" + "&cmdname=OBSUPLOAD&VESSELID=" + ViewState["VESID"] + "'); return false;", "Attachments", "", "ATTACHMENTS");
                MenuInspectionObsComments.MenuList = toolbar.Show();
            }
        }
    }

    protected void EditAuditSchedule()
    {
        if (Filter.CurrentAuditScheduleId != null && Filter.CurrentAuditScheduleId.ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditSchedule.EditAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Filter.CurrentAuditScheduleId.ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["AUDITDATE"] = dr["FLDRANGEFROMDATE"].ToString();
                DateTime d = DateTime.Parse(ViewState["AUDITDATE"].ToString());
                //txtTargetDate.Text = String.Format("{0:dd/MMM/yyyy}", d.AddMonths(3));
            }
        }
    }

    protected void chkPAExtensionRequiredEdit_checkedChanged(object sender, EventArgs e)
    {
        CheckBox chkextrequired = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chkextrequired.Parent.Parent;
        UserControlDate targetdate = (UserControlDate)row.Cells[1].FindControl("ucPATargetDateEdit");
        TextBox extreason = (TextBox)row.Cells[4].FindControl("txtPAExtensionReasonEdit");

        if (chkextrequired.Checked)
        {
            if (targetdate != null)
            {
                targetdate.Enabled = true;
                targetdate.CssClass = "input_mandatory";
            }
            if (extreason != null)
            {
                extreason.Enabled = true;
                extreason.CssClass = "input_mandatory";
            }
        }
        else
        {
            if (targetdate != null)
            {
                targetdate.Enabled = false;
                targetdate.CssClass = "input";
                if (ViewState["OLDPATARGETDATE"] != null)
                    targetdate.Text = ViewState["OLDPATARGETDATE"].ToString();
            }
            if (extreason != null)
            {
                extreason.Enabled = false;
                extreason.CssClass = "input";
                if (ViewState["OLDPAEXTENSIONREASON"] != null)
                    extreason.Text = ViewState["OLDPAEXTENSIONREASON"].ToString();
            }
        }
    }

    protected void gvPreventiveAction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindPreventiveAction();
    }

    protected void gvPreventiveAction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (nCurrentRow != -1)
            {
                CheckBox c = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkPAExtensionRequiredEdit");
                UserControlDate targetdate = (UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit");
                TextBox extreason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit");

                if (c != null && c.Checked == false)
                {
                    if (targetdate != null)
                    {
                        targetdate.Enabled = false;
                        targetdate.CssClass = "input";
                        if (ViewState["OLDPATARGETDATE"] != null)
                            targetdate.Text = ViewState["OLDPATARGETDATE"].ToString();
                    }
                    if (extreason != null)
                    {
                        extreason.Enabled = false;
                        extreason.CssClass = "input";
                        if (ViewState["OLDPAEXTENSIONREASON"] != null)
                            extreason.Text = ViewState["OLDPAEXTENSIONREASON"].ToString();
                    }
                }
            }

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreventiveActionAddition(((TextBox)_gridView.FooterRow.FindControl("txtPreventActAdd")).Text,
                                                       ((UserControlDate)_gridView.FooterRow.FindControl("ucPATargetDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["OBSID"] == null || string.IsNullOrEmpty(ViewState["OBSID"].ToString()))
                {
                    ucError.Text = "Observation Details should be recorded first";
                    ucError.Visible = true;
                    return;
                }
                int vesselid = 0;
                DataSet ds = PhoenixInspectionAuditNonConformity.EditNonConformityObservation(new Guid(ViewState["RecordResponseId"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                    vesselid = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

                string createdfrom = "1";

                PhoenixInspectionObservation.ObservationActionToBeTakenInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , ViewState["RecordResponseId"] != null ? General.GetNullableGuid(ViewState["RecordResponseId"].ToString()) : null
                    , ((TextBox)_gridView.FooterRow.FindControl("txtPreventActAdd")).Text
                    , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("ucPATargetDateAdd")).Text)
                    , vesselid
                    , General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtPICAdd")).Text)
                    , General.GetNullableInteger(((UserControlInspectionDepartment)_gridView.FooterRow.FindControl("ucDeptAdd")).SelectedDepartment)
                    , General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucCategoryAdd")).SelectedQuick)
                    , General.GetNullableInteger(((UserControlQuick)_gridView.FooterRow.FindControl("ucSubCategoryAdd")).SelectedQuick)
                    , General.GetNullableInteger(createdfrom)
                    );

                BindPreventiveAction();
                ((TextBox)_gridView.FooterRow.FindControl("txtPreventActAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionObservation.ObservationActionToBeTakenDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsPreventActid")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreventiveAction_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindPreventiveAction();
    }

    protected void gvPreventiveAction_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindPreventiveAction();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtPreventActEdit")).Focus();

            UserControlDate ucPATargetDateEdit = (UserControlDate)_gridView.Rows[de.NewEditIndex].FindControl("ucPATargetDateEdit");
            TextBox txtPAExtensionReasonEdit = (TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtPAExtensionReasonEdit");
            if (ucPATargetDateEdit != null)
                ViewState["OLDPATARGETDATE"] = ucPATargetDateEdit.Text;
            if (txtPAExtensionReasonEdit != null)
                ViewState["OLDPAEXTENSIONREASON"] = txtPAExtensionReasonEdit.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreventiveAction_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            CheckBox chkextrequired = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkPAExtensionRequiredEdit");

            if (!IsValidPreventiveAction(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPreventActEdit")).Text
                                        , chkextrequired.Checked
                                        , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit")).Text
                                        , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPACompletionDateEdit")).Text
                                        , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            string createdfrom = "1";

            PhoenixInspectionObservation.ObservationActionToBeTakenUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsPreventActidEdit")).Text)
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPreventActEdit")).Text
                , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit")).Text)
                , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPACompletionDateEdit")).Text)
                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit")).Text
                , General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPICEdit")).Text)
                , General.GetNullableInteger(((UserControlInspectionDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDeptEdit")).SelectedDepartment)
                , General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucCategoryEdit")).SelectedQuick)
                , General.GetNullableInteger(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucSubCategoryEdit")).SelectedQuick)
                , General.GetNullableInteger(createdfrom)
                );

            if (chkextrequired != null && chkextrequired.Checked == true)
            {
                int vesselid = 0;

                DataSet ds = PhoenixInspectionObservation.EditInspectionObservation(new Guid(ViewState["RecordResponseId"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                    vesselid = int.Parse(ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());

                PhoenixInspectionSchedule.ReScheduleHistoryInsert(
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsPreventActidEdit")).Text)
                                                                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPAExtensionReasonEdit")).Text
                                                                , General.GetNullableDateTime(ViewState["OLDPATARGETDATE"].ToString())
                                                                , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucPATargetDateEdit")).Text)
                                                                , vesselid
                                                                );
            }

            gvPreventiveAction.EditIndex = -1;
            BindPreventiveAction();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreventiveAction_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvPreventiveAction.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvPreventiveAction_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPreventiveAction, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvPreventiveAction_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
                {
                    if (db != null)
                        db.Visible = false;
                }
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
            {
                if (eb != null)
                    eb.Visible = false;
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton cmdReschedule = (ImageButton)e.Row.FindControl("cmdReschedule");
            Label preactionid = (Label)e.Row.FindControl("lblInsPreventActid");
            if (cmdReschedule != null)
            {
                cmdReschedule.Visible = SessionUtil.CanAccess(this.ViewState, cmdReschedule.CommandName);
                if (preactionid != null)
                    cmdReschedule.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionNonConformityExtensionReason.aspx?ncid=" + preactionid.Text + "');return true;");
            }

            DataRowView dr = (DataRowView)e.Row.DataItem;
            UserControlInspectionDepartment ucDeptEdit = (UserControlInspectionDepartment)e.Row.FindControl("ucDeptEdit");
            if (ucDeptEdit != null)
            {
                ucDeptEdit.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                ucDeptEdit.DataBind();
                ucDeptEdit.SelectedDepartment = dr["FLDDEPARTMENT"].ToString();

                HtmlImage imgPICEdit = (HtmlImage)e.Row.FindControl("imgPICEdit");
                if (imgPICEdit != null && General.GetNullableInteger(dr["FLDDEPARTMENT"].ToString()) != null)
                    imgPICEdit.Attributes.Add("onclick", "return showPickList('spnPICEdit', 'codehelp1', '', '../Common/CommonPickListInspectionUser.aspx?departmentid=" + General.GetNullableInteger(dr["FLDDEPARTMENT"].ToString()) + "', true);");
            }
            UserControlQuick ucCategoryEdit = (UserControlQuick)e.Row.FindControl("ucCategoryEdit");
            if (ucCategoryEdit != null)
            {
                ucCategoryEdit.bind();
                ucCategoryEdit.SelectedQuick = dr["FLDCATEGORY"].ToString();
            }
            UserControlQuick ucSubCategoryEdit = (UserControlQuick)e.Row.FindControl("ucSubCategoryEdit");
            if (ucSubCategoryEdit != null)
            {
                ucSubCategoryEdit.bind();
                ucSubCategoryEdit.SelectedQuick = dr["FLDSUBCATEGORY"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlInspectionDepartment ucDeptAdd = (UserControlInspectionDepartment)e.Row.FindControl("ucDeptAdd");
            if (ucDeptAdd != null)
            {
                ucDeptAdd.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                ucDeptAdd.DataBind();
            }
            UserControlQuick ucCategoryAdd = (UserControlQuick)e.Row.FindControl("ucCategoryAdd");
            if (ucCategoryAdd != null)
                ucCategoryAdd.bind();
            UserControlQuick ucSubCategoryAdd = (UserControlQuick)e.Row.FindControl("ucSubCategoryAdd");
            if (ucSubCategoryAdd != null)
                ucSubCategoryAdd.bind();
        }
    }

    private void BindPreventiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionObservation.ObservationActionToBeTakenList(new Guid(ViewState["RecordResponseId"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPreventiveAction.DataSource = ds;
                gvPreventiveAction.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPreventiveAction);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidPreventiveAction(string action, bool extrequired, string targetdate, string completiondate, string extreason)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(action))
            ucError.ErrorMessage = "Preventive Action is required.";

        if (General.GetNullableDateTime(targetdate) == null)
            ucError.ErrorMessage = "Target Date is required.";

        if (extrequired)
        {
            if (string.IsNullOrEmpty(extreason))
                ucError.ErrorMessage = "Reschedule Reason is required.";

            else if (General.GetNullableString(extreason) == General.GetNullableString(ViewState["OLDPAEXTENSIONREASON"].ToString()))
                ucError.ErrorMessage = "You have not modified the Reschedule Reason.";

            if (General.GetNullableDateTime(targetdate) == General.GetNullableDateTime(ViewState["OLDPATARGETDATE"].ToString()))
                ucError.ErrorMessage = "You have not modified the 'Target Date'.";
        }

        if (General.GetNullableDateTime(completiondate) != null && General.GetNullableDateTime(completiondate) > DateTime.Today)
            ucError.ErrorMessage = "Completion Date can not be the future date.";

        return (!ucError.IsError);
    }

    private bool IsValidPreventiveActionAddition(string action, string targetdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(action))
            ucError.ErrorMessage = "Preventive Action is required.";

        if (string.IsNullOrEmpty(targetdate))
            ucError.ErrorMessage = "Target Date is required.";

        else if (General.GetNullableDateTime(targetdate) == null)
            ucError.ErrorMessage = "Please enter a valid Target Date.";

        return (!ucError.IsError);
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void departmentEdit_Changed(object sender, EventArgs e)
    {
        UserControlInspectionDepartment ucDeptEdit = (UserControlInspectionDepartment)sender;
        GridViewRow row = (GridViewRow)ucDeptEdit.Parent.Parent;
        //UserControlInspectionDepartment ucDeptEdit = (UserControlInspectionDepartment)row.Cells[1].FindControl("ucDeptEdit");
        HtmlImage imgPICEdit = (HtmlImage)row.Cells[2].FindControl("imgPICEdit");

        if (ucDeptEdit != null)
        {
            //txtPICEdit.Text = "";
            if (imgPICEdit != null)
            {
                imgPICEdit.Attributes.Add("onclick", "return showPickList('spnPICEdit', 'codehelp1', '', '../Common/CommonPickListInspectionUser.aspx?departmentid=" + General.GetNullableInteger(ucDeptEdit.SelectedDepartment) + "', true);");
            }
        }
    }

    protected void departmentAdd_Changed(object sender, EventArgs e)
    {
        UserControlInspectionDepartment ucDeptAdd = (UserControlInspectionDepartment)sender;
        GridViewRow row = (GridViewRow)ucDeptAdd.Parent.Parent;
        HtmlImage imgPICAdd = (HtmlImage)row.Cells[2].FindControl("imgPICAdd");

        if (ucDeptAdd != null)
        {
            //txtPICAdd.Text = "";
            if (imgPICAdd != null)
            {
                imgPICAdd.Attributes.Add("onclick", "return showPickList('spnPICAdd', 'codehelp1', '', '../Common/CommonPickListInspectionUser.aspx?departmentid=" + General.GetNullableInteger(ucDeptAdd.SelectedDepartment) + "', true);");
            }
        }
    }

}
