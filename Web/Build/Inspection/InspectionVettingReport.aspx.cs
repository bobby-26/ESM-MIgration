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

public partial class InspectionVettingReport : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ucInspectionType.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS");
                ucConfirm.Attributes.Add("style", "display:none");
                ucConfirmDelete.Attributes.Add("style", "display:none");

                if (Request.QueryString["SCHEDULEID"] != null && Request.QueryString["SCHEDULEID"].ToString() != string.Empty)
                    ViewState["SCHEDULEID"] = Request.QueryString["SCHEDULEID"].ToString();
                else
                    Reset();

                if (Request.QueryString["reffrom"] != null && Request.QueryString["reffrom"].ToString() != string.Empty)
                    ViewState["reffrom"] = Request.QueryString["reffrom"].ToString();

                if (ViewState["reffrom"] != null && ViewState["reffrom"].ToString() != string.Empty)
                    ddlStatus.Enabled = false;

                ViewState["VESSELID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["INSPECTIONID"] = null;
                ViewState["VALIDYN"] = "0";
                ViewState["VALIDDATIONMSG"] = "";

                if (ViewState["SCHEDULEID"] != null && ViewState["SCHEDULEID"].ToString() != string.Empty)
                {
                    DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["SCHEDULEID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewState["REVIEWID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONID"].ToString();
                }

                BindCompany();
                BindShortCodeList();
                BindInternalAuditor();
                BindInspectionSchedule();
                //BindData();


                if (txtInspection.Text.ToUpper().Contains("SIRE"))
                {
                    rblSire.SelectedValue = "1";
                }
                else
                {
                    rblSire.SelectedValue = "0";
                }
                SetRights();
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            if (Request.QueryString["viewonly"] == null)
            {

                toolbarsub.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionVettingAttachment.aspx?dtkey=" + ViewState["DTKEY"]
                    + "&VESSELID=" + ViewState["VESSELID"]
                    + "&viewonly=0"
                    + "'); return false;", "Attachments", "", "ATTACHMENT", ToolBarDirection.Right);
                toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuInspectionSchedule.AccessRights = this.ViewState;
                MenuInspectionSchedule.MenuList = toolbarsub.Show();
            }
            else
            {
                toolbarsub.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionVettingAttachment.aspx?dtkey=" + ViewState["DTKEY"]
                    + "&VESSELID=" + ViewState["VESSELID"]
                    + "&viewonly=1"
                    + "'); return false;", "Attachments", "", "ATTACHMENT", ToolBarDirection.Right);
                MenuInspectionSchedule.AccessRights = this.ViewState;
                MenuInspectionSchedule.MenuList = toolbarsub.Show();
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

    protected void BindCompany()
    {
        ddlCompany.DataSource = PhoenixInspectionInspectingCompany.ListAuditInspectionCompany(null, General.GetNullableGuid(ViewState["REVIEWID"] != null ? ViewState["REVIEWID"].ToString() : ""));
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void SetRights()
    {
        bool flag = true;
        DataSet ds1 = PhoenixInspectionEventSupdtFeedback.EventSupdtFeedbackEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , 2, General.GetNullableGuid(ViewState["SCHEDULEID"].ToString()));

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

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
        {
            flag = SessionUtil.CanAccess(this.ViewState, "SUPDTEVENTFEEDBACK");
            if (flag == true)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback != 1)
                {
                    flag = false;
                }
            }
        }
        else
        {
            flag = false;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindInspectionSchedule();
    }

    private void BindInspectionSchedule()
    {
        if (ViewState["SCHEDULEID"] != null && ViewState["SCHEDULEID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["SCHEDULEID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucInspectionCategory.SelectedHard = dr["FLDINSPECTIONCATEGORYID"].ToString();
                ucInspectionCategory.Enabled = false;
                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                ddlInspectionShortCodeList.SelectedValue = dr["FLDINSPECTIONID"].ToString();
                ddlInspectionShortCodeList.Enabled = false;
                ViewState["INSPECTIONID"] = dr["FLDINSPECTIONID"].ToString();
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ucPort.SelectedSeaport = dr["FLDPORTID"].ToString();
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                txtCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                if (dr["FLDADDITIONALINSPECTORID"] != null && dr["FLDADDITIONALINSPECTORID"].ToString() != "")
                    ddlAuditorName.SelectedValue = dr["FLDADDITIONALINSPECTORID"].ToString();
                if (dr["FLDISBERTHORANCHORAGE"] != null && dr["FLDISBERTHORANCHORAGE"].ToString() != "")
                    rblLocation.SelectedValue = dr["FLDISBERTHORANCHORAGE"].ToString();
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
                txtCompany.Text = dr["FLDCOMPANYNAME"].ToString();
                txtBerth.Text = dr["FLDBERTH"].ToString();
                txtInspectorName.Text = dr["FLDNAMEOFINSPECTOR"].ToString();
                txtDesignation.Text = dr["FLDINSPECTORDESIGNATIONNAME"].ToString();
                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                txtInspection.Text = dr["FLDINSPECTIONNAME"].ToString();
                txtPlannedDate.Text = dr["FLDPLANNEDDATE"].ToString();
                if (dr["FLDINSPECTINGCOMPANYID"] != null && dr["FLDINSPECTINGCOMPANYID"].ToString() != "")
                {
                    ddlCompany.SelectedValue = dr["FLDINSPECTINGCOMPANYID"].ToString().ToUpper();
                }
                if (dr["FLDISSIREINSPECTION"].ToString() != "")
                    rblSire.SelectedValue = dr["FLDISSIREINSPECTION"].ToString();
                if (dr["FLDVETTINGNILDEFICIENCIES"] != null && dr["FLDVETTINGNILDEFICIENCIES"].ToString() != "" && dr["FLDVETTINGNILDEFICIENCIES"].ToString() == "1")
                {
                    chkNILDeficiencies.Checked = true;
                    gvDeficiency.Enabled = false;
                }
                else
                {
                    chkNILDeficiencies.Checked = false;
                    gvDeficiency.Enabled = true;
                }
                if (dr["FLDISREJECTION"] != null && dr["FLDISREJECTION"].ToString() != "" && dr["FLDISREJECTION"].ToString().ToUpper() == "TRUE")
                {
                    chkRejection.Checked = true;
                }
                else
                {
                    chkRejection.Checked = false;
                }

            }
        }
    }

    protected void SaveVettingDetails()
    {
        if (!IsValidInspectionSchedule())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixInspectionSchedule.UpdateVettingSchedule(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["SCHEDULEID"].ToString())
                , new Guid(ddlInspectionShortCodeList.SelectedValue)
                , Int16.Parse(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ucPort.SelectedSeaport)
                , General.GetNullableInteger(rblLocation.SelectedValue)
                , General.GetNullableString(txtBerth.Text)
                , General.GetNullableDateTime(txtPlannedDate.Text)
                , General.GetNullableDateTime(txtCompletionDate.Text)
                , General.GetNullableInteger(ddlStatus.SelectedHard)
                , General.GetNullableString(txtInspectorName.Text)
                , General.GetNullableString(txtDesignation.Text)
                , General.GetNullableInteger(ddlAuditorName.SelectedValue)
                , General.GetNullableString(txtRemarks.Text)
                , General.GetNullableInteger(rblSire.SelectedValue)
                , General.GetNullableInteger(chkNILDeficiencies.Checked == true ? "1" : "0")
                , General.GetNullableInteger(chkRejection.Checked == true ? "1" : "0")
                , General.GetNullableGuid(ddlCompany.SelectedValue)
                );
        ucStatus.Text = "Information updated successfully.";
        BindShortCodeList();
        BindInternalAuditor();
        BindInspectionSchedule();
        BindData();
    }

    protected void MenuInspectionSchedule_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG")) //planned
                {
                    SaveVettingDetails();
                    String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }
                else
                {
                    if (ViewState["reffrom"] != null && ViewState["reffrom"].ToString() != string.Empty)
                    {
                        SaveVettingDetails();
                        String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                    }
                    else
                    {
                        RadWindowManager1.RadConfirm("All deficiencies should be raised before completing the vetting. Do you want to continue.?", "Confirm", 320, 150, null, "Confirm");
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
            if (!IsValidInspectionSchedule())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionSchedule.UpdateVettingSchedule(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["SCHEDULEID"].ToString())
                    , new Guid(ddlInspectionShortCodeList.SelectedValue)
                    , Int16.Parse(ucVessel.SelectedVessel)
                    , General.GetNullableInteger(ucPort.SelectedSeaport)
                    , General.GetNullableInteger(rblLocation.SelectedValue)
                    , General.GetNullableString(txtBerth.Text)
                    , General.GetNullableDateTime(txtPlannedDate.Text)
                    , General.GetNullableDateTime(txtCompletionDate.Text)
                    , General.GetNullableInteger(ddlStatus.SelectedHard)
                    , General.GetNullableString(txtInspectorName.Text)
                    , General.GetNullableString(txtDesignation.Text)
                    , General.GetNullableInteger(ddlAuditorName.SelectedValue)
                    , General.GetNullableString(txtRemarks.Text)
                    , General.GetNullableInteger(rblSire.SelectedValue)
                    , General.GetNullableInteger(chkNILDeficiencies.Checked == true ? "1" : "0")
                    , General.GetNullableInteger(chkRejection.Checked == true ? "1" : "0")
                    , General.GetNullableGuid(ddlCompany.SelectedValue)
                    );
            ucStatus.Text = "Information updated successfully.";
            BindShortCodeList();
            BindInternalAuditor();
            BindInspectionSchedule();
            BindData();
            SetRights();
            String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);

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

        if (General.GetNullableInteger(ucPort.SelectedSeaport) == null)
            ucError.ErrorMessage = "Port is required.";

        if (General.GetNullableGuid(ddlCompany.SelectedValue) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableInteger(ddlStatus.SelectedHard) == null)
            ucError.ErrorMessage = "Status is required.";
        else if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
        {
            if (General.GetNullableDateTime(txtCompletionDate.Text) == null)
                ucError.ErrorMessage = "Date of Inspection is required.";
            else if (General.GetNullableDateTime(txtCompletionDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Date of Inspection cannot be the future date.";
        }

        if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
        {
            if (General.GetNullableDateTime(txtCompletionDate.Text) != null && General.GetNullableDateTime(txtCompletionDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Date of Inspection cannot be the future date.";
        }

        if (General.GetNullableString(txtInspectorName.Text) == null)
            ucError.ErrorMessage = "Inspector Name is required.";

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
        ucInspectionType.Enabled = true;
        ddlInspectionShortCodeList.Enabled = true;
        ddlInspectionShortCodeList.SelectedIndex = -1;

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
        ddlInspectionShortCodeList.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void InspectionType_Changed(object sender, EventArgs e)
    {
    }

    protected void chkatsea_CheckedChanged(object sender, EventArgs e)
    {

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

    protected void chkAtSeaRequired_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void chkSecond_checkedchanged(object sender, EventArgs e)
    {
        BindInspectionSchedule();
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
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                //  BindValue(nCurrentRow);
                // SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("SUPDTEVENTFEEDBACK"))
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
                        string script = "openNewWindow('Bank','','" + Session["sitepath"] + "/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=2&SOURCEREFERENCEID=" + lblScheduleId.Text + "&OTHERREFERENCEID=" + lblDeficiencyId.Text + "');";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                    }
                }
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
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

                    PhoenixInspectionSchedule.InsertVettingNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["SCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeAdd.SelectedValue),
                        int.Parse(ucVessel.SelectedVessel), General.GetNullableDateTime(txtCompletionDate.Text),
                        int.Parse(status), General.GetNullableString(((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text),
                        General.GetNullableString(((RadTextBox)item.FindControl("txtDescAdd")).Text),
                        4, ref deficiencyid
                        , General.GetNullableGuid(((UserControlInspectionChapter)item.FindControl("ucChapterAdd")).SelectedChapter));
                }
                else if (ddlTypeAdd != null && (ddlTypeAdd.SelectedValue == "3" || ddlTypeAdd.SelectedValue == "4"))
                {
                    if (!IsValidDeficiency(ddlTypeAdd.SelectedValue, ucNCCategoryAdd.SelectedQuick,
                        ((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text,
                        ((RadTextBox)item.FindControl("txtDescAdd")).Text,
                        status))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionSchedule.InsertVettingObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["SCHEDULEID"].ToString()), int.Parse(ucVessel.SelectedVessel),
                        General.GetNullableDateTime(txtCompletionDate.Text),
                        int.Parse(status), General.GetNullableString(((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text),
                        General.GetNullableString(((RadTextBox)item.FindControl("txtDescAdd")).Text),
                        2, ref deficiencyid, 1
                        , General.GetNullableGuid(((UserControlInspectionChapter)item.FindControl("ucChapterAdd")).SelectedChapter)
                        , General.GetNullableInteger(ddlTypeAdd.SelectedValue));
                }

                //String scriptupdate = String.Format("javascript:fnReloadList('Report','IfMoreInfo','keepupopen');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
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
                        ((UserControlHard)e.Item.FindControl("ucStatusEdit")).SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionSchedule.InsertVettingNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["SCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeEdit.SelectedValue),
                        int.Parse(ucVessel.SelectedVessel),
                        General.GetNullableDateTime(lblDate.Text),
                        int.Parse(((UserControlHard)e.Item.FindControl("ucStatusEdit")).SelectedHard),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistRefEdit")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescEdit")).Text),
                        4, ref deficiencyid
                        , General.GetNullableGuid(((UserControlInspectionChapter)e.Item.FindControl("ucChapterEdit")).SelectedChapter));
                }
                else if (ddlTypeEdit != null && (ddlTypeEdit.SelectedValue == "3" || ddlTypeEdit.SelectedValue == "4"))
                {
                    if (!IsValidDeficiency(ddlTypeEdit.SelectedValue, ucNCCategoryEdit.SelectedQuick,
                        ((RadTextBox)e.Item.FindControl("txtChecklistRefEdit")).Text,
                        ((RadTextBox)e.Item.FindControl("txtDescEdit")).Text,
                        ((UserControlHard)e.Item.FindControl("ucStatusEdit")).SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionSchedule.InsertVettingObsDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["SCHEDULEID"].ToString()), int.Parse(ucVessel.SelectedVessel),
                        General.GetNullableDateTime(lblDate.Text),
                        int.Parse(((UserControlHard)e.Item.FindControl("ucStatusEdit")).SelectedHard),
                         General.GetNullableString(((RadTextBox)e.Item.FindControl("txtChecklistRefEdit")).Text),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescEdit")).Text),
                        2, ref deficiencyid, 1
                        , General.GetNullableGuid(((UserControlInspectionChapter)e.Item.FindControl("ucChapterEdit")).SelectedChapter)
                        , General.GetNullableInteger(ddlTypeEdit.SelectedValue));
                }

                ucStatus.Text = "Deficiency is Updated successfully.";
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

                
                ucStatus.Text = "Deficiency is Deleted successfully.";
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
                LinkButton ef = (LinkButton)e.Item.FindControl("imgSupdtEventFeedback");
                if (ef != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ef.CommandName)) ef.Visible = false;
                }
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadComboBox ddltype = (RadComboBox)e.Item.FindControl("ddlTypeEdit");
            UserControlQuick ucNCCategoryEdit = (UserControlQuick)e.Item.FindControl("ucNCCategoryEdit");
            UserControlHard ucStatusEdit = (UserControlHard)e.Item.FindControl("ucStatusEdit");
            UserControlInspectionChapter ucChapterEdit = (UserControlInspectionChapter)e.Item.FindControl("ucChapterEdit");

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
                else if (drv["FLDNONCONFORMITYTYPE"].ToString() == "1")
                {
                    ddltype.SelectedValue = "3";
                }
                else if (drv["FLDNONCONFORMITYTYPE"].ToString() == "2")
                {
                    ddltype.SelectedValue = "4";
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
                ucChapterEdit.InspectionId = ViewState["INSPECTIONID"].ToString();
                ucChapterEdit.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null, null, General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()));
                ucChapterEdit.DataBind();
                ucChapterEdit.SelectedChapter = drv["FLDCHAPTERID"].ToString();
            }

            if (Request.QueryString["viewonly"] != null && Request.QueryString["viewonly"].ToString().Equals("1"))
                gvDeficiency.Columns[9].Visible = false;
            else
                gvDeficiency.Columns[9].Visible = true;

            UserControlToolTip ucDescription = (UserControlToolTip)e.Item.FindControl("ucDescription");
            RadLabel lblDescription = (RadLabel)e.Item.FindControl("lblDescription");
            if (lblDescription != null)
            {
                lblDescription.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDescription.ToolTip + "', 'visible');");
                lblDescription.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDescription.ToolTip + "', 'hidden');");
            }

            RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblScheduleId");
            RadLabel lblDeficiencyId = (RadLabel)e.Item.FindControl("lblDeficiencyId");
            LinkButton imgSupdtEventFeedback = (LinkButton)e.Item.FindControl("imgSupdtEventFeedback");

            bool flag = true;
            if (imgSupdtEventFeedback != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
                {
                    if (imgSupdtEventFeedback != null && lblDeficiencyId != null && lblScheduleId != null)
                    {
                        flag = SessionUtil.CanAccess(this.ViewState, imgSupdtEventFeedback.CommandName);

                        if (imgSupdtEventFeedback != null && lblDeficiencyId != null && lblScheduleId != null)
                        {
                            if (General.GetNullableDateTime(txtCompletionDate.Text) == null)
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
            if (ucDateAdd != null)
                ucDateAdd.Text = txtCompletionDate.Text;

            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName)) cmdAdd.Visible = false;
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

    protected void txtCompletionDate_Changed(object sender, EventArgs e)
    {
        if (ViewState["reffrom"] == null || ViewState["reffrom"].ToString() == "")
        {
            if (General.GetNullableDateTime(txtCompletionDate.Text) != null)
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
            else
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "ASG");
        }
        else
        {
            if (General.GetNullableDateTime(txtCompletionDate.Text) != null)
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
            else
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
        }
    }

    protected void chkNILDeficiencies_CheckedChanged(object sender, EventArgs e)
    {
        if (chkNILDeficiencies.Checked == true)
            gvDeficiency.Enabled = false;
        else
            gvDeficiency.Enabled = true;
    }

    protected void SupdtFeedback_Click(object sender, EventArgs e)
    {
        if (ViewState["VALIDYN"].ToString() != "1")
        {
            ucError.ErrorMessage = ViewState["VALIDDATIONMSG"].ToString();
            ucError.Visible = true;
            return;
        }
        else
        {
            string script = "openNewWindow('Bank','','" + Session["sitepath"] + "/Inspection/InspectionSupdtEventFeedback.aspx?sourcefrom=2&SOURCEREFERENCEID=" + ViewState["SCHEDULEID"].ToString() + "');";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
        }
    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvDeficiency_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
