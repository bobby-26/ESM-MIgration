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
public partial class InspectionAuditManualAdd : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionScheduleGeneral.AccessRights = this.ViewState;
            MenuInspectionScheduleGeneral.MenuList = toolbar.Show();
            //ucConfirm.Visible = false;               

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                {
                    ChkCredited.Visible = false;
                    lblCredited.Visible = false;
                }

                VesselConfiguration();
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }

                ViewState["AUDITSCHEDULEID"] = "";
                ViewState["REVIEWID"] = "";
                //if (Request.QueryString["AUDITSCHEDULEID"] != null && Request.QueryString["AUDITSCHEDULEID"].ToString() != string.Empty)
                //    ViewState["AUDITSCHEDULEID"] = Request.QueryString["AUDITSCHEDULEID"].ToString();
                //else
                //    Reset();

                ddlStatus.Enabled = false;
                ddlStatus.DataBind();
                ddlStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");

                ViewState["CATEGORYID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindInternalOrganization();
                BindExternalOrganization();
                BindInspection();
                BindInternalInspector();
                BindInternalAuditor();
                BindAuditSchedule();
                BindCompany();
                SetWidth();
            }

           // BindData();
            // SetPageNavigator();
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
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ucAuditCategory.SelectedHard = dr["FLDREVIEWCATEGORYID"].ToString();
                BindInspection();
                ddlAudit.SelectedValue = dr["FLDREVIEWID"].ToString();
                txtCompletedDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
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
                if (dr["FLDREVIEWCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    if (ddlOrganization.Items.Equals((dr["FLDINTERNALORGANIZATIONID"].ToString()) != null))
                        ddlOrganization.SelectedValue = dr["FLDINTERNALORGANIZATIONID"].ToString();
                    EnableDisableExternal(false, "input");
                }
                else
                {
                    EnableDisableInternal(false, "input");
                    if (ddlExternalOrganizationName.Items.Equals((dr["FLDEXTERNALORGANIZATIONID"].ToString()) != null))
                        ddlExternalOrganizationName.SelectedValue = dr["FLDEXTERNALORGANIZATIONID"].ToString();
                }
                txtRefNo.Enabled = false;
                ucPort.SelectedSeaport = dr["FLDPORTID"].ToString();
                if (dr["FLDISBERTHORANCHORAGE"] != null && dr["FLDISBERTHORANCHORAGE"].ToString() != "")
                    rblLocation.SelectedValue = dr["FLDISBERTHORANCHORAGE"].ToString();

                if (dr["FLDPORTID"] != null && dr["FLDPORTID"].ToString() != "")
                {
                    chkatsea.Checked = false;
                    ucFromPort.Enabled = false;
                    ucToPort.Enabled = false;
                    ucFromPort.SelectedSeaport = "";
                    ucToPort.SelectedSeaport = "";
                    ucPort.CssClass = "input_mandatory";
                }
                else
                {
                    chkatsea.Checked = true;
                    ucFromPort.SelectedSeaport = dr["FLDFROMPORTID"].ToString();
                    ucToPort.SelectedSeaport = dr["FLDTOPORTID"].ToString();
                    ucFromPort.Enabled = true;
                    ucToPort.Enabled = true;
                    ucFromPort.CssClass = "input_mandatory";
                    ucToPort.CssClass = "input_mandatory";
                    ucPort.Enabled = false;
                    ucPort.SelectedSeaport = "";
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


    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        BindInspection();
        if (ucAuditCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            EnableDisableExternal(true, "input_mandatory");
            EnableDisableInternal(false, "input");
        }
        else if (ucAuditCategory.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
        {
            EnableDisableExternal(false, "input");
            EnableDisableInternal(true, "input_mandatory");
        }
        BindCompany();
        ViewState["CATEGORYID"] = ucAuditCategory.SelectedHard;
    }

    protected void BindInspection()
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
        ddlAudit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 1
                                        , 0
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlAudit_Changed(object sender, EventArgs e)
    {
        string type = "";
        if (General.GetNullableGuid(ddlAudit.SelectedValue) != null)
        {
            DataSet ds = PhoenixInspection.EditInspection(new Guid(ddlAudit.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
                type = ds.Tables[0].Rows[0]["FLDINSPECTIONCATEGORYID"].ToString();
        }
        ViewState["CATEGORYID"] = type;
        if (type == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            EnableDisableExternal(true, "input_mandatory");
            EnableDisableInternal(false, "input");
            BindCompany();
        }
        else
        {
            EnableDisableExternal(false, "input");
            EnableDisableInternal(true, "input_mandatory");
        }
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

        ddlExternalOrganizationName.CssClass = cssclass;
        txtExternalInspectorName.CssClass = cssclass;
        //txtExternalInspectorDesignation.CssClass = cssclass;
        //txtExternalOrganisationName.CssClass = cssclass;
        //ddlAuditorName.CssClass = "input";
        ucAuditor.CssClass = "input";
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
        ddlOrganization.CssClass = cssclass;
    }

    protected void SaveReviewDetails()
    {
        BindData();
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
                if (!IsValidAuditSchedule())
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? scheduleid = null;
                if (ViewState["AUDITSCHEDULEID"] != null && ViewState["AUDITSCHEDULEID"].ToString() != "")
                    scheduleid = new Guid(ViewState["AUDITSCHEDULEID"].ToString());
                PhoenixInspectionAuditSchedule.ReviewScheduleManualInsert(
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(ucVessel.SelectedVessel)
                    , new Guid(ddlAudit.SelectedValue)
                    , General.GetNullableInteger(ucFromPort.SelectedSeaport)
                    , General.GetNullableInteger(ucToPort.SelectedSeaport)
                    , General.GetNullableInteger(ucPort.SelectedSeaport)
                    , General.GetNullableInteger(rblLocation.SelectedValue)
                    , General.GetNullableString(txtBerth.Text)
                    , General.GetNullableDateTime(txtCompletedDate.Text)
                    , General.GetNullableString(txtRemarks.Text)
                    //, General.GetNullableInteger(ddlInspectorName.SelectedValue)
                    , General.GetNullableInteger(ucInspector.SelectedValue)
                    , General.GetNullableString(txtExternalInspectorName.Text)
                    , General.GetNullableString(txtOrganization.Text)
                    , General.GetNullableString(txtExternalOrganisationName.Text)
                    , General.GetNullableString(txtExternalInspectorDesignation.Text)
                    //, General.GetNullableInteger(ddlAuditorName.SelectedValue)
                    , General.GetNullableInteger(ucAuditor.SelectedValue)
                    , General.GetNullableGuid(ddlCompany.SelectedValue)
                    , General.GetNullableInteger(chkNILDeficiencies.Checked == true ? "1" : "0")
                    , ref scheduleid
                    , General.GetNullableString(ucInspector.Text)
                    , General.GetNullableString(ucAuditor.Text)
                    , General.GetNullableInteger(ChkCredited.Checked == true ? "1" : "0")
                    , General.GetNullableInteger(ddlOrganization.SelectedValue)
                    , General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue)
                    );
                Filter.CurrentAuditScheduleId = scheduleid.ToString();
                ViewState["AUDITSCHEDULEID"] = scheduleid.ToString();
                ucStatus.Text = "Information updated.";
                BindAuditSchedule();
                BindData();
                //  SetPageNavigator();
                String scriptupdate = String.Format("javascript:fnReloadList('codehelp1',null,'keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
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

        if (General.GetNullableGuid(ddlAudit.SelectedValue) == null)
            ucError.ErrorMessage = "Audit / Inspection is required.";

        if (General.GetNullableDateTime(txtCompletedDate.Text) == null)
            ucError.ErrorMessage = "Date of Inspection is required.";
        else if (General.GetNullableDateTime(txtCompletedDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Date of Inspection cannot be the future date.";

        if (chkatsea.Checked == true)
        {
            if (ucFromPort.SelectedSeaport.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucFromPort.SelectedSeaport.ToString()))
                ucError.ErrorMessage = "From Port is required.";
            if (ucToPort.SelectedSeaport.ToUpper().ToString() == "DUMMY" || string.IsNullOrEmpty(ucToPort.SelectedSeaport.ToString()))
                ucError.ErrorMessage = "To Port is required.";
        }
        else
        {
            if (General.GetNullableInteger(ucPort.SelectedSeaport) == null)
                ucError.ErrorMessage = "Port is required.";
        }
        if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            if (General.GetNullableString(txtExternalInspectorName.Text) == null)
                ucError.ErrorMessage = "External Auditor / Inspector is required.";

            if (General.GetNullableInteger(ddlExternalOrganizationName.SelectedValue) == null)
                ucError.ErrorMessage = "External Auditor / Inspector Organization is required.";
        }
        else if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
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
            ucPort.SelectedSeaport = "";
            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            ucFromPort.CssClass = "input_mandatory";
            ucToPort.CssClass = "input_mandatory";
            ucFromPort.Enabled = true;
            ucToPort.Enabled = true;
            rblLocation.Enabled = false;
            rblLocation.Items[0].Selected = false;
            rblLocation.Items[1].Selected = false;
            txtBerth.CssClass = "readonlytextbox";
            txtBerth.Text = "";
            txtBerth.Enabled = false;
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
            txtBerth.CssClass = "input";
            txtBerth.Enabled = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["DEFICIENCYID"] = null;
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

            DataSet ds = PhoenixInspectionAuditSchedule.DeficiencySearch(General.GetNullableInteger(ucVessel.SelectedVessel)
                    , General.GetNullableGuid(ViewState["AUDITSCHEDULEID"].ToString())
                    , sortexpression, sortdirection,
                    (int)ViewState["PAGENUMBER"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

            General.SetPrintOptions("gvDeficiency", "Deficiencies", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["DEFICIENCYID"] == null || ViewState["DEFICIENCYID"].ToString() == "")
                {
                    ViewState["DEFICIENCYID"] = ds.Tables[0].Rows[0]["FLDDEFICIENCYID"].ToString();

                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
            }
            gvDeficiency.DataSource = ds;
            //   gvDeficiency.DataBind();
            gvDeficiency.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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

        if (ViewState["AUDITSCHEDULEID"] == null || ViewState["AUDITSCHEDULEID"].ToString() == "")
            ucError.ErrorMessage = "Audit/Inspection details should be recorded and saved first before adding deficiencies.";
        else
        {
            if (General.GetNullableInteger(deficiencytype) == null)
                ucError.ErrorMessage = "Deficiency Type is required.";

            if (General.GetNullableString(checklistref) == null)
                ucError.ErrorMessage = "Checklist Reference Number is required.";

            if (General.GetNullableString(desc) == null)
                ucError.ErrorMessage = "Description is required.";

            if (General.GetNullableInteger(status) == null)
                ucError.ErrorMessage = "Status is required.";
        }

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

    //protected void gvDeficiency_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //RadGrid _gridView = (RadGrid)sender;
        //int nCurrentRow = e.Item.RowIndex;
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                // BindValue(nCurrentRow);
                ///// SetRowSelection();
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

                    PhoenixInspectionAuditSchedule.InsertNCDeficiency(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), General.GetNullableInteger(ddlTypeAdd.SelectedValue),
                        int.Parse(ucVessel.SelectedVessel),
                        General.GetNullableDateTime(txtCompletedDate.Text),
                        int.Parse(status),
                        General.GetNullableString(((RadTextBox)item.FindControl("txtChecklistRefAdd")).Text),
                        General.GetNullableString(((RadTextBox)item.FindControl("txtDescAdd")).Text),
                        1, ref deficiencyid,
                        General.GetNullableGuid(((UserControlInspectionChapter)item.FindControl("ucChapterAdd")).SelectedChapter),
                        null,
                        General.GetNullableString(((RadTextBox)item.FindControl("txtKeyAdd")).Text), null,
                        General.GetNullableString(((RadTextBox)item.FindControl("txtItemAdd")).Text));
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
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), int.Parse(ucVessel.SelectedVessel),
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
                String scriptupdate = String.Format("javascript:fnReloadList('codehelp1','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
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
                ucStatus.Text = "Deficiency is deleted successfully.";
                String scriptupdate = String.Format("javascript:fnReloadList('codehelp1','IfMoreInfo','keepupopen');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            else
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
                        int.Parse(ucVessel.SelectedVessel),
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
                        new Guid(ViewState["AUDITSCHEDULEID"].ToString()), int.Parse(ucVessel.SelectedVessel),
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
            }
            gvDeficiency.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvDeficiency_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = de.RowIndex;
    //        RadLabel lblTypeid = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblTypeid"));
    //        RadLabel lblDeficiencyId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDeficiencyId"));

    //        if (lblTypeid != null && lblTypeid.Text == "1")
    //        {
    //            PhoenixInspectionAuditDirectNonConformity.DeleteAuditDirectNonConformity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDeficiencyid")).Text));
    //        }
    //        else if (lblTypeid != null && lblTypeid.Text == "2")
    //        {
    //            PhoenixInspectionObservation.DeleteInspectionDirectObservation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDeficiencyid")).Text));
    //        }
    //        BindData();
    //        //SetPageNavigator();

    //        String scriptupdate = String.Format("javascript:fnReloadList('codehelp1','IfMoreInfo','keepupopen');");
    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvDeficiency_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;

    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;

    //        BindData();
    //        //SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvDeficiency_RowCancelingEdit(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void gvDeficiency_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            // if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
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
                if (ViewState["REVIEWID"] != null && ViewState["REVIEWID"].ToString() != "")
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
                if (ViewState["REVIEWID"] != null && ViewState["REVIEWID"].ToString() != "")
                    ucChapterAdd.InspectionId = ViewState["REVIEWID"].ToString();
                ucChapterAdd.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(null, null, General.GetNullableGuid(ViewState["REVIEWID"].ToString()));
                ucChapterAdd.DataBind();
            }
            if (ucDateAdd != null)
                ucDateAdd.Text = txtCompletedDate.Text;

            if (ddlTypeAdd != null)
            {
                if (ddlAudit.SelectedValue.ToUpper().Contains("VIR"))
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
        // gvDeficiency.SelectedIndex = se.NewSelectedIndex;
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

    protected void BindCompany()
    {
        ddlCompany.Items.Clear();
        if (General.GetNullableGuid(ddlAudit.SelectedValue) != null)
        {
            ddlCompany.DataSource = PhoenixInspectionInspectingCompany.ListAuditInspectionCompany(null, General.GetNullableGuid(ddlAudit.SelectedValue));
            ddlCompany.DataTextField = "FLDCOMPANYNAME";
            ddlCompany.DataValueField = "FLDCOMPANYID";
            ddlCompany.DataBind();
        }
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

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeficiency.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
