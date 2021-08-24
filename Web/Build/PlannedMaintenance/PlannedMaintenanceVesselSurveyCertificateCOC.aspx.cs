using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class PlannedMaintenanceVesselSurveyCertificateCOC : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);

            MenuCertificatesCOC.AccessRights = this.ViewState;
            MenuCertificatesCOC.MenuList = toolbarmain.Show();


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["VesselId"] = Request.QueryString["vslid"];
                ViewState["DTKEY"] = Request.QueryString["dtkey"];
                ViewState["CID"] = Request.QueryString["cid"];
                gvCertificatesCOC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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

    private void EditVesselCertificate()
    {
        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.EditVesselCertificate(int.Parse(ViewState["VesselId"].ToString())
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
            chkAttachYN.Checked = dr["FLDATTACHCORRECTYN"].ToString().Trim().Equals("1") ? true : false;
            ddlSurveyPort.SelectedValue = dr["FLDPLANPORT"].ToString().Trim();
            ddlSurveyPort.Text = dr["FLDSURVEYPORTNAME"].ToString().Trim();
            chkNoExpiry.Checked = dr["FLDNOEXPIRY"].ToString().Trim().Equals("1") ? true : false;
            ucExpiryDate.ReadOnly = chkNoExpiry.Checked;
            ucExpiryDate.Text = chkNoExpiry.Checked ? "" : dr["FLDDATEOFEXPIRY"].ToString().Trim();
            txtPlanDate.Text = dr["FLDPLANDATE"].ToString();
            ucExpiryDate.CssClass = chkNoExpiry.Checked ? "readonlytextbox" : "input";
            txtInitialDate.Text = dr["FLDINITIALDATE"].ToString();
            txtPlanRemarks.Text = dr["FLDPLANREMARKS"].ToString();

            ucLastSurveyDate.Text = dr["FLDLASTSURVEYDATE"].ToString();
            ViewState["ScheduleId"] = dr["FLDSCHEDULEID"].ToString();
            ViewState["CertificateId"] = dr["FLDCERTIFICATEID"].ToString();
            ViewState["AUDITREQ"] = dr["FLDAUDITREQUIREDYN"].ToString();
            txtLastSurveyAudit.Text = dr["FLDLASTSURVEYAUDITNAME"].ToString();
            ViewState["LASTSURVEY"] = dr["FLDLASTSURVEYTYPEID"].ToString();
            ViewState["LASTAUDIT"] = dr["FLDLASTAUDIT"].ToString();
        }
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixPlannedMaintenanceVesselCertificateCOC.CertificateCOCSearch(
                General.GetNullableInteger(ViewState["VesselId"].ToString())
                , General.GetNullableInteger(ViewState["CertificateId"].ToString())
                 , General.GetNullableGuid(ViewState["ScheduleId"].ToString())
               , gvCertificatesCOC.CurrentPageIndex + 1
               , gvCertificatesCOC.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );


            gvCertificatesCOC.DataSource = ds.Tables[0];
            gvCertificatesCOC.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            if (!IsPostBack)
            {
                //((TextBox)gvCertificatesCOC.FooterRow.FindControl("txtItemAdd")).Focus();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCertificatesCOC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails(txtNumber.Text, ucIssueDate.Text, ucExpiryDate.Text, ucIssuingAuthority.SelectedValue,
                    ChkNotApplicable.Checked, txtReason.Text.Trim(), ucDoneDate.Text, ucLastSurveyDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (General.GetNullableDateTime(ucDoneDate.Text).HasValue)
                {
                    ucConfirm.Visible = true;
                    ucConfirm.Text = "Please ensure that you have updated the attachment with the revised Certificate.";
                }
                else
                {
                    Save();
                }
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
        )
    {
        DateTime resultDate;
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (certificateno.Trim().Equals(""))
            ucError.ErrorMessage = "Certificate Number is required.";

        if (!DateTime.TryParse(dateofissue, out resultDate))
            ucError.ErrorMessage = "Issue Date is required.";

        if (!Int16.TryParse(issuingauthority, out resultInt))
            ucError.ErrorMessage = "Issuing Authority is required.";
        if (dateofissue != null)
        {
            if (DateTime.Parse(dateofissue) > DateTime.Today)
            {
                ucError.ErrorMessage = "Issue Date should be earlier then Current Date.";
            }
        }
        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater then 'Issue Date'.";
        }
        if (VesselNotApplicable && Reason.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Reason for not applicable is required.";
        }
        if (DoneDate != null)
        {
            if (DateTime.Parse(DoneDate) > DateTime.Today)
            {
                ucError.ErrorMessage = "Done Date should be earlier then Current Date.";
            }
        }

        return (!ucError.IsError);
    }



    private void UpdateVesselCertificatesCOC(string COCId, string VesselId, string CertificateId, string ScheduleId
        , string Item, string Description, string DueDate, string Status)
    {
        PhoenixPlannedMaintenanceVesselCertificateCOC.UpdateCertificateCOC(General.GetNullableGuid(COCId)
            , General.GetNullableInteger(VesselId)
            , General.GetNullableInteger(CertificateId)
            , General.GetNullableGuid(ScheduleId)
            , General.GetNullableString(Item)
            , General.GetNullableString(Description)
            , General.GetNullableDateTime(DueDate)
            , General.GetNullableInteger(Status));
    }
    private void InsertVesselCertificatesCOC(string VesselId, string CertificateId, string ScheduleId
        , string Item, string Description, string DueDate)
    {
        PhoenixPlannedMaintenanceVesselCertificateCOC.InsertCertificateCOC(General.GetNullableInteger(VesselId)
           , General.GetNullableInteger(CertificateId)
           , General.GetNullableGuid(ScheduleId)
           , General.GetNullableString(Item)
           , General.GetNullableString(Description)
           , General.GetNullableDateTime(DueDate));
    }
    private bool IsValidCOC(string Item, string Description, string DueDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (Item.Trim().Equals(""))
            ucError.ErrorMessage = "COC is required.";
        if (Description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (DueDate == null)
            ucError.ErrorMessage = "Due Date is required.";
        //else if (DateTime.TryParse(DueDate, out resultDate) && DateTime.Parse(DueDate) < DateTime.Today)
        //{
        //    ucError.ErrorMessage = "Due Date Should be Later then Current Date";
        //}
        return (!ucError.IsError);
    }
       
    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {

            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                Save();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Save()
    {
        try
        {
            Guid? g = null;
            if (ViewState["DTKEY"].ToString() != string.Empty)
            {
                g = new Guid(ViewState["DTKEY"].ToString());
                PhoenixPlannedMaintenanceSurveySchedule.ReportVesselSurveyCertificate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                               , General.GetNullableInteger(ViewState["CID"].ToString()).Value
                                                                               , ref g
                                                                               , General.GetNullableInteger(ddlSurveyPort.SelectedValue)
                                                                               , General.GetNullableDateTime(txtPlanDate.Text)
                                                                               , txtSurveyorName.Text.Trim()
                                                                               , General.GetNullableDateTime(ucDoneDate.Text)
                                                                               , General.GetNullableString(txtPlanRemarks.Text.Trim())
                                                                               );

            }

            ViewState["DTKEY"] = g;
            EditVesselCertificate();
            ucStatus.Text = "Certificate Details Updated Sucessfully";
            ucStatus.Visible = true;
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            //    "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', " + (General.GetNullableDateTime(ucDoneDate.Text).HasValue ? "null" : "'keepopen'") + ");", true);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCertificatesCOC_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCertificatesCOC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                LinkButton cp = (LinkButton)e.Item.FindControl("cmdComplete");
                if (cp != null) cp.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

                if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                }

                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
                LinkButton cmdComplete = (LinkButton)e.Item.FindControl("cmdComplete");
                LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                RadLabel lblItem = (RadLabel)e.Item.FindControl("lblItem");
                RadLabel lblDescription = (RadLabel)e.Item.FindControl("lblDescription");
                LinkButton cmdSvyAtt = (LinkButton)e.Item.FindControl("cmdSvyAtt");
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDtkey");
                RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                RadLabel lblCocId = (RadLabel)e.Item.FindControl("lblCocID");

                if (lblIsAtt != null && lblIsAtt.Text != "YES")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    cmdSvyAtt.Controls.Add(html);
                }
                if (cmdComplete != null && lblStatus != null)
                    cmdComplete.Visible = lblStatus.Text.Trim().Equals("3") ? false : true;
                if (cmdDelete != null)
                    cmdDelete.Visible = lblStatus.Text.Trim().Equals("3") ? false : true;
                if (cmdEdit != null)
                    cmdEdit.Visible = lblStatus.Text.Trim().Equals("3") ? false : true;
                if (cmdSvyAtt != null)
                    cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] +"/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.Trim() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCOC');");

                if(cmdComplete !=null)
                {
                    cmdComplete.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyCOCComplete.aspx?CertificateCOCId=" + lblCocId.Text.Trim() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCOC');");
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCertificatesCOC_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadGrid _gridView = (RadGrid)sender;
            GridFooterItem fitem = e.Item as GridFooterItem;
            GridDataItem item = e.Item as GridDataItem;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCOC((fitem.FindControl("txtItemAdd") as RadTextBox).Text
                  , (fitem.FindControl("txtDescAdd") as RadTextBox).Text
                  , (fitem.FindControl("ucDueDateAdd") as UserControlDate).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertVesselCertificatesCOC(ViewState["VesselId"].ToString()
                                 , ViewState["CertificateId"].ToString()
                                 , ViewState["ScheduleId"].ToString()
                                 , (fitem.FindControl("txtItemAdd") as RadTextBox).Text
                                 , (fitem.FindControl("txtDescAdd") as RadTextBox).Text
                                 , (fitem.FindControl("ucDueDateAdd") as UserControlDate).Text
                                 );


                gvCertificatesCOC.Rebind();

            }


            else if (e.CommandName.Trim().ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceVesselCertificateCOC.DeleteCertificateCOC(new Guid((item.FindControl("lblCocID") as RadLabel).Text));
                gvCertificatesCOC.Rebind();

            }
            else if (e.CommandName.Trim().ToUpper().Equals("UPDATE"))
            {
                if (!IsValidCOC((item.FindControl("txtItemEdit") as RadTextBox).Text
                    , (item.FindControl("txtDescEdit") as RadTextBox).Text
                    , (item.FindControl("ucDueDateEdit") as UserControlDate).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateVesselCertificatesCOC(
                   (item.FindControl("lblCocID") as RadLabel).Text
                   , ViewState["VesselId"].ToString()
                   , ViewState["CertificateId"].ToString()
                   , ViewState["ScheduleId"].ToString()
                   , (item.FindControl("txtItemEdit") as RadTextBox).Text
                   , (item.FindControl("txtDescEdit") as RadTextBox).Text
                   , (item.FindControl("ucDueDateEdit") as UserControlDate).Text
                    , (item.FindControl("lblStatus") as RadLabel).Text
                   );

                gvCertificatesCOC.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
