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
using Telerik.Web.UI;

public partial class InspectionAuditOfficeRecordGeneral : PhoenixBasePage
{
    public int? defaultauditytpe = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucConfirm.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                if (Request.QueryString["AUDITSCHEDULEID"] != null && Request.QueryString["AUDITSCHEDULEID"].ToString() != string.Empty)
                    ViewState["AUDITSCHEDULEID"] = Request.QueryString["AUDITSCHEDULEID"].ToString();
                else
                    Reset();

                if (Request.QueryString["reffrom"] != null && Request.QueryString["reffrom"].ToString() != string.Empty)
                    ViewState["reffrom"] = Request.QueryString["reffrom"].ToString();

                if (ViewState["reffrom"] != null && ViewState["reffrom"].ToString() != string.Empty)
                    ddlStatus.Enabled = false;

                ViewState["INTERNALREVIEWSCHEDULEID"] = null;
                ViewState["EXTERNALREVIEWSCHEDULEID"] = null;
                ViewState["FLDINTERIMAUDITID"] = null;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = 0; //for office

                BindInternalOrganization();
                BindExternalOrganization();
                BindInternalInspector();
                BindInternalAuditor();
                BindCompany();
                BindAuditSchedule();
                SetWidth();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["viewonly"] == null)
            {
               
                toolbar.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
                        + "&mod=" + PhoenixModule.QUALITY
                        + "&type=AUDITINSPECTION"
                        + "&cmdname=AUDITINSPECTIONUPLOAD"
                        + "&VESSELID=" + ViewState["VESSELID"]
                        + "'); return true;", "Attachments", "", "ATTACHMENT", ToolBarDirection.Right);
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
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuInspectionScheduleGeneral.AccessRights = this.ViewState;
                MenuInspectionScheduleGeneral.MenuList = toolbar.Show();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
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

    //protected void MenuInspectionGeneral_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    try
    //    {
    //        if (dce.CommandName.ToUpper().Equals("LIST"))
    //        {
    //            Response.Redirect("../Inspection/InspectionAuditRecordList.aspx", true);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}

    protected void EnableDisableExternal(bool value, string cssclass)
    {
        txtExternalInspectorName.Enabled = value;
        txtExternalInspectorDesignation.Enabled = value;
        txtExternalOrganisationName.Enabled = value;
        ddlAuditorName.Enabled = value;
        ddlCompany.Enabled = value;
        ddlExternalOrganizationName.Enabled = value;
        if (value == false)
        {
            txtExternalInspectorName.Text = "";
            txtExternalInspectorDesignation.Text = "";
            txtExternalOrganisationName.Text = "";
            ddlAuditorName.SelectedIndex = 0;
            ddlCompany.SelectedIndex = 0;
        }
        txtExternalInspectorName.CssClass = cssclass;
        ddlAuditorName.CssClass = cssclass;
        ddlExternalOrganizationName.CssClass = "readonlytextbox";
    }

    protected void EnableDisableInternal(bool value, string cssclass)
    {
        ddlInspectorName.Enabled = value;
        ddlOrganization.Enabled = value;

        if (value == false)
        {
            ddlInspectorName.SelectedIndex = 0;
            txtOrganization.Text = "";
        }

        ddlInspectorName.CssClass = cssclass;
        ddlOrganization.CssClass = "readonlytextbox";
    }

    private void BindAuditSchedule()
    {
        if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditOfficeSchedule.EditAuditOfficeSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["AUDITSCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                lblCompanyId.Text = dr["FLDCOMPANYID"].ToString();
                txtCompany.Text = dr["FLDCOMPANYNAME"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtCategory.Text = dr["FLDREVIEWCATEGORYNAME"].ToString();
                txtCategoryid.Text = dr["FLDREVIEWCATEGORYID"].ToString();
                txtInspection.Text = dr["FLDREVIEWSHORTCODE"].ToString();
                txtCompletedDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                if (dr["FLDINTERNALINSPECTORID"] != null && dr["FLDINTERNALINSPECTORID"].ToString() != "")
                    ddlInspectorName.SelectedValue = dr["FLDINTERNALINSPECTORID"].ToString();
                txtExternalInspectorName.Text = dr["FLDEXTERNALINSPECTORNAME"].ToString();
                txtExternalInspectorDesignation.Text = dr["FLDEXTERNALINSPECTORDESIGNATION"].ToString();
                txtExternalOrganisationName.Text = dr["FLDEXTERNALINSPECTORORGANISATION"].ToString();
                if (dr["FLDADDITIONALAUDITORID"] != null && dr["FLDADDITIONALAUDITORID"].ToString() != "")
                    ddlAuditorName.SelectedValue = dr["FLDADDITIONALAUDITORID"].ToString();
                if (dr["FLDREVIEWCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    EnableDisableExternal(false, "input");
                    ddlOrganization.CssClass = "input_mandatory";
                    ddlOrganization.SelectedValue = dr["FLDINTERNALORGANIZATIONID"].ToString();
                }
                else
                {
                    EnableDisableInternal(false, "input");
                    ddlExternalOrganizationName.CssClass = "input_mandatory";
                    ddlExternalOrganizationName.SelectedValue = dr["FLDEXTERNALORGANIZATIONID"].ToString();
                }
                txtRefNo.Enabled = false;
                ViewState["FLDINTERIMAUDITID"] = dr["FLDINTERIMAUDITID"].ToString();

                ViewState["ATTACHMENTCODE"] = dr["FLDDTKEY"].ToString();
                ViewState["FLDCOMPANYID"] = dr["FLDCOMPANYID"].ToString();
                ViewState["REVIEWID"] = dr["FLDREVIEWID"].ToString();
                //BindCompany();
                if (dr["FLDINSPECTINGCOMPANYID"] != null && dr["FLDINSPECTINGCOMPANYID"].ToString() != "")
                    ddlCompany.SelectedValue = dr["FLDINSPECTINGCOMPANYID"].ToString();
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
        PhoenixInspectionAuditOfficeSchedule.UpdateReviewOfficeSchedule(
             PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
            , General.GetNullableString(txtRemarks.Text)
            , General.GetNullableInteger(ddlStatus.SelectedHard)
            , General.GetNullableDateTime(txtCompletedDate.Text)
            , General.GetNullableInteger(ddlInspectorName.SelectedValue)
            , General.GetNullableString(txtOrganization.Text)
            , General.GetNullableString(txtExternalInspectorName.Text)
            , General.GetNullableString(txtExternalOrganisationName.Text)
            , General.GetNullableString(txtExternalInspectorDesignation.Text)
            , General.GetNullableInteger(ddlAuditorName.SelectedValue)
            , General.GetNullableGuid(ddlCompany.SelectedValue)
            , General.GetNullableInteger(chkNILDeficiencies.Checked == true ? "1" : "0")
            , General.GetNullableInteger(ddlOrganization.SelectedValue)
            , General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue)
            );
        ucStatus.Text = "Information updated.";
        BindAuditSchedule();
        gvDeficiency.Rebind();
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

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
             if (!IsValidAuditSchedule())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionAuditOfficeSchedule.UpdateReviewOfficeSchedule(
                                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
                                                                                 , General.GetNullableString(txtRemarks.Text)
                                                                                 , General.GetNullableInteger(ddlStatus.SelectedHard)
                                                                                 , General.GetNullableDateTime(txtCompletedDate.Text)
                                                                                 , General.GetNullableInteger(ddlInspectorName.SelectedValue)
                                                                                 , General.GetNullableString(txtOrganization.Text)
                                                                                 , General.GetNullableString(txtExternalInspectorName.Text)
                                                                                 , General.GetNullableString(txtExternalOrganisationName.Text)
                                                                                 , General.GetNullableString(txtExternalInspectorDesignation.Text)
                                                                                 , General.GetNullableInteger(ddlAuditorName.SelectedValue)
                                                                                 , General.GetNullableGuid(ddlCompany.SelectedValue)
                                                                                 , General.GetNullableInteger(chkNILDeficiencies.Checked == true ? "1" : "0")
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
        if (txtCategoryid.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")))
        {
            if (General.GetNullableString(txtExternalInspectorName.Text) == null)
                ucError.ErrorMessage = "External Auditor / Inspector is required.";

            if (General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue) == null)
                ucError.ErrorMessage = "External Auditor / Inspector Organization is required.";
        }
        else if (txtCategoryid.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 144, "INT")))
        {
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

        //DropDownList ddlListStatus = (DropDownList)ddlStatus.FindControl("ddlHard");
        UserControlHard ddlListStatus = (UserControlHard)ddlStatus.FindControl("ddlStatus");
        Unit ucWidth = new Unit("150px");
        if (ddlListStatus != null)
            ddlListStatus.Attributes.Add("style", "width:95px;");
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
        ddlInspectorName.DataSource = ds.Tables[0];
        ddlInspectorName.DataTextField = "FLDDESIGNATIONNAME";
        ddlInspectorName.DataValueField = "FLDUSERCODE";
        ddlInspectorName.DataBind();
        ddlInspectorName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindInternalAuditor()
    {
        DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
        ddlAuditorName.DataSource = ds;
        ddlAuditorName.DataTextField = "FLDDESIGNATIONNAME";
        ddlAuditorName.DataValueField = "FLDUSERCODE";
        ddlAuditorName.DataBind();
        ddlAuditorName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
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

            DataSet ds = PhoenixInspectionAuditSchedule.DeficiencySearch(0
                , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvDeficiency", "Deficiencies", alCaptions, alColumns, ds);

            gvDeficiency.DataSource = ds;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            gvDeficiency.VirtualItemCount = iRowCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
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
            ViewState["DEFICIENCYID"] = ((RadLabel)gvDeficiency.Items[rowindex].FindControl("lblDeficiencyId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.RowIndex;

        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                // BindValue(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadComboBox ddlTypeAdd = (RadComboBox)e.Item.FindControl("ddlTypeAdd");
                UserControlQuick ucNCCategoryAdd = (UserControlQuick)e.Item.FindControl("ucNCCategoryAdd");
                string status = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");
                Guid? deficiencyid = null;

                if (ddlTypeAdd != null && (ddlTypeAdd.SelectedValue == "1" || ddlTypeAdd.SelectedValue == "2"))
                {
                    if (!IsValidDeficiency(ddlTypeAdd.SelectedValue, ucNCCategoryAdd.SelectedQuick,
                        ((RadTextBox)e.Item.FindControl("txtChecklistRefAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDescAdd")).Text,
                        status))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionAuditSchedule.InsertNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeAdd.SelectedValue),
                        0, /*for company*/
                        General.GetNullableDateTime(txtCompletedDate.Text),
                        int.Parse(status),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistRefAdd")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescAdd")).Text),
                        1, ref deficiencyid,
                        General.GetNullableGuid(((UserControlInspectionChapter)e.Item.FindControl("ucChapterAdd")).SelectedChapter),
                        null,
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtKeyAdd")).Text), null,
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtItemAdd")).Text)
                        );
                }
                else if (ddlTypeAdd != null && ddlTypeAdd.SelectedValue == "3")
                {
                    if (!IsValidDeficiency(ddlTypeAdd.SelectedValue, ucNCCategoryAdd.SelectedQuick,
                        ((RadTextBox)e.Item.FindControl("txtChecklistRefAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDescAdd")).Text,
                        status))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionAuditSchedule.InsertObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), 0,
                        General.GetNullableDateTime(txtCompletedDate.Text),
                        int.Parse(status),
                         General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistRefAdd")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescAdd")).Text),
                        1, ref deficiencyid, 1,
                        General.GetNullableGuid(((UserControlInspectionChapter)e.Item.FindControl("ucChapterAdd")).SelectedChapter),
                        null,
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtKeyAdd")).Text), null,
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtItemAdd")).Text));
                }
             
                String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
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
             
                String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
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
                        int.Parse(ViewState["VESSELID"].ToString()),
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
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()),
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
             
                String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            BindData();
            gvDeficiency.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvDeficiency_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
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
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddltype = (RadComboBox)e.Item.FindControl("ddlTypeEdit");
            UserControlQuick ucNCCategoryEdit = (UserControlQuick)e.Item.FindControl("ucNCCategoryEdit");
            UserControlInspectionChapter ucChapterEdit = (UserControlInspectionChapter)e.Item.FindControl("ucChapterEdit");
            DropDownList ddlItemEdit = (DropDownList)e.Item.FindControl("ddlItemEdit");
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
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlTypeAdd = (RadComboBox)e.Item.FindControl("ddlTypeAdd");
            UserControlQuick ucNCCategoryAdd = (UserControlQuick)e.Item.FindControl("ucNCCategoryAdd");
            UserControlInspectionChapter ucChapterAdd = (UserControlInspectionChapter)e.Item.FindControl("ucChapterAdd");
            UserControlDate ucDateAdd = (UserControlDate)e.Item.FindControl("ucDateAdd");
            DropDownList ddlItemAdd = (DropDownList)e.Item.FindControl("ddlItemAdd");

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

    protected void gvDeficiency_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
           && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
           && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDeficiency, "Select$" + e.Row.RowIndex.ToString(), false);
        }
        //SetKeyDownScroll(sender, e);
    }

    protected void gvDeficiency_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        //  gvDeficiency.SelectedIndex = se.NewSelectedIndex;
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
        //gvDeficiency.SelectedIndex = -1;
        //for (int i = 0; i < gvDeficiency.Rows.Count; i++)
        //{
        //    if (gvDeficiency.DataKeys[i].Value.ToString().Equals(ViewState["DEFICIENCYID"].ToString()))
        //    {
        //        gvDeficiency.SelectedIndex = i;
        //    }
        //}
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

    protected void BindCompany()
    {
        ddlCompany.DataSource = PhoenixInspectionInspectingCompany.ListAuditInspectionCompany();
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

    protected void BindVIRItem(DropDownList ddl)
    {
        ddl.DataSource = PhoenixInspectionVIRItem.InspectionVIRItemTreeList();
        ddl.DataTextField = "FLDITEMNAME";
        ddl.DataValueField = "FLDITEMID";
        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
}
