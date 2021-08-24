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

public partial class InspectionAuditEdit : PhoenixBasePage
{
    public int? defaultauditytpe = null;

    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            foreach (GridViewRow r in gvDeficiency.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Page.ClientScript.RegisterForEventValidation(gvDeficiency.UniqueID, "Edit$" + r.RowIndex.ToString());
                }
            }
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
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                defaultauditytpe = General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "AUD"));
                if (Request.QueryString["AUDITSCHEDULEID"] != null && Request.QueryString["AUDITSCHEDULEID"].ToString() != string.Empty)
                    ViewState["AUDITSCHEDULEID"] = Request.QueryString["AUDITSCHEDULEID"].ToString();
                else
                    Reset();

                ViewState["INTERNALREVIEWSCHEDULEID"] = null;
                ViewState["EXTERNALREVIEWSCHEDULEID"] = null;
                ViewState["FLDINTERIMAUDITID"] = null;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                if (defaultauditytpe != null)
                    ucAudit.InspectionType = defaultauditytpe.ToString();
                BindShortCodeList();
                BindInternalInspector();
                BindInternalAuditor();
                BindExternalOrganisation();
                BindExternalInspector();
                BindAuditSchedule();
                SetWidth();                
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
                ucVessel.bind();
                txtRefNo.Text = dr["FLDREFERENCENUMBER"].ToString();
                txtSerialNumber.Text = dr["FLDSERIALNUMBER"].ToString();
                ucAuditType.SelectedHard = dr["FLDREVIEWTYPEID"].ToString();
                ucAuditCategory.SelectedHard = dr["FLDREVIEWCATEGORYID"].ToString();
                ucExternalAuditType.SelectedHard = dr["FLDEXTERNALAUDITTYPE"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ucLastPort.SelectedSeaport = dr["FLDLASTPORTOFAUDITINSPECTION"].ToString();
                lblInspectionId.Text = dr["FLDREVIEWID"].ToString();
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
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                if (dr["FLDREVIEWCATEGORYID"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 144, "INT"))
                {
                    txtWindowperiod.CssClass = "input";
                    ucExternalAuditType.CssClass = "input";
                    ucExternalAuditType.Enabled = false;
                    External.Enabled = false;
                }
                else
                {
                    txtWindowperiod.CssClass = "input_mandatory";
                    ucExternalAuditType.CssClass = "input";
                    ucExternalAuditType.Enabled = true;
                    Internal.Enabled = false;
                }
                ucAudit.SelectedValue = dr["FLDREVIEWID"].ToString();
                ddlAuditShortCodeList.SelectedValue = dr["FLDREVIEWID"].ToString();
                txtWindowperiod.Enabled = true;
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
                ddlExternalInspectorName.SelectedValue = dr["FLDINSPECTORID"].ToString();
                txtExternalOrganisationName.Text = dr["FLDCOMPANYNAME"].ToString();
                ddlAuditorName.SelectedValue = dr["FLDADDITIONALAUDITORID"].ToString();
                txtReportReceivedDate.Text = dr["FLDREPORTGENERATEDDATE"].ToString();
                ViewState["ATTACHMENTCODE"] = dr["FLDDTKEY"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                if (ddlStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 146, "CMP"))
                {
                    ucAuditCategory.Enabled = false;
                    ucExternalAuditType.Enabled = false;
                    ddlAuditShortCodeList.Enabled = false;
                }
            }
        }
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
        ddlAuditShortCodeList.SelectedIndex = 0;
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
            txtWindowperiod.CssClass = "input_mandatory";
            ucExternalAuditType.Enabled = true;
            Internal.Enabled = false;
            External.Enabled = true;
        }
        else if (ucAuditCategory.SelectedHard.Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 144, "INT")))
        {
            ucExternalAuditType.CssClass = "input";
            txtWindowperiod.CssClass = "input";
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
            ucFromPort.CssClass = "input";
            ucToPort.CssClass = "input";
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
        try
        {
            ViewState["DEFICIENCYID"] = null;
            BindData();
            for (int i = 0; i < gvDeficiency.DataKeyNames.Length; i++)
            {
                if (gvDeficiency.DataKeyNames[i] == (ViewState["DEFICIENCYID"] == null ? null : ViewState["DEFICIENCYID"].ToString()))
                {
                    gvDeficiency.SelectedIndex = i;
                    break;
                }
            }
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
            txtExternalOrganisationName.Text = dr["FLDCOMPANYNAME"].ToString();
        }
    }
    protected void ExternalOrganisation(object sender, EventArgs e)
    {
        BindExternalInspector();
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

            DataSet ds = PhoenixInspectionAuditSchedule.DeficiencySearch(int.Parse(ucVessel.SelectedVessel)
                , new Guid(ViewState["AUDITSCHEDULEID"].ToString())
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
            UserControlQuick ucRiskCategoryEdit = (UserControlQuick)e.Row.FindControl("ucRiskCategoryEdit");
            UserControlHard ucStatusEdit = (UserControlHard)e.Row.FindControl("ucStatusEdit");

            if (ddltype != null)
            {
                if (drv["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MAJ"))
                {
                    ddltype.SelectedValue = "1";
                    if (ucNCCategoryEdit != null)
                    {
                        ucNCCategoryEdit.Visible = true;
                        ucNCCategoryEdit.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                        ucNCCategoryEdit.DataBind();
                        ucNCCategoryEdit.SelectedQuick = drv["FLDDEFICIENCYCATEGORYID"].ToString();
                    }
                    if (ucRiskCategoryEdit != null) ucRiskCategoryEdit.Visible = false;
                }
                else if (drv["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MIN"))
                {
                    ddltype.SelectedValue = "2";
                    if (ucNCCategoryEdit != null)
                    {
                        ucNCCategoryEdit.Visible = true;
                        ucNCCategoryEdit.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                        ucNCCategoryEdit.DataBind();
                        ucNCCategoryEdit.SelectedQuick = drv["FLDDEFICIENCYCATEGORYID"].ToString();
                    }
                    if (ucRiskCategoryEdit != null) ucRiskCategoryEdit.Visible = false;
                }
                else
                {
                    ddltype.SelectedValue = "3";
                    if (ucNCCategoryEdit != null) ucNCCategoryEdit.Visible = false;
                    if (ucRiskCategoryEdit != null)
                    {
                        ucRiskCategoryEdit.Visible = true;
                        ucRiskCategoryEdit.QuickList = PhoenixRegistersQuick.ListQuick(1, 71);
                        ucRiskCategoryEdit.DataBind();
                        ucRiskCategoryEdit.SelectedQuick = drv["FLDDEFICIENCYCATEGORYID"].ToString();
                    }
                }
            }
            if (ucStatusEdit != null)
            {
                ucStatusEdit.HardList = PhoenixRegistersHard.ListHard(1, 146, 0, "OPN,CLD,CMP");
                ucStatusEdit.DataBind();
                ucStatusEdit.SelectedHard = drv["FLDSTATUSID"].ToString();
            }
        }
        /*if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList ddlTypeAdd = (DropDownList)e.Row.FindControl("ddlTypeAdd");
            UserControlQuick ucNCCategoryAdd = (UserControlQuick)e.Row.FindControl("ucNCCategoryAdd");
            UserControlQuick ucRiskCategoryAdd = (UserControlQuick)e.Row.FindControl("ucRiskCategoryAdd");

            ViewState["type"] = ddlTypeAdd.SelectedValue;

            if (ViewState["type"] != null && ucNCCategoryAdd != null && (ViewState["type"].ToString() == "1" || ViewState["type"].ToString() == "2"))
            {
                ucNCCategoryAdd.Visible = true;
                if (General.GetNullableInteger(ucNCCategoryAdd.SelectedQuick) == null)
                {
                    ucNCCategoryAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                    ucNCCategoryAdd.DataBind();
                }
                ucRiskCategoryAdd.Visible = false;
            }
            if (ViewState["type"] != null && ucRiskCategoryAdd != null && (ViewState["type"].ToString() == "3"))
            {
                ucRiskCategoryAdd.Visible = true;
                if (General.GetNullableInteger(ucRiskCategoryAdd.SelectedQuick) == null)
                {
                    ucRiskCategoryAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 71);
                    ucRiskCategoryAdd.DataBind();
                }
                ucNCCategoryAdd.Visible = false;
            }
        }*/

        if (e.Row.RowType == DataControlRowType.Footer)
        {            
            DropDownList ddlTypeAdd = (DropDownList)e.Row.FindControl("ddlTypeAdd");
            UserControlQuick ucNCCategoryAdd = (UserControlQuick)e.Row.FindControl("ucNCCategoryAdd");
            UserControlQuick ucRiskCategoryAdd = (UserControlQuick)e.Row.FindControl("ucRiskCategoryAdd");
            if (!IsPostBack)
            {
                ViewState["type"] = ddlTypeAdd.SelectedValue;
            }
            if (ViewState["type"] != null)
            {
                if (ViewState["type"].ToString() == "3")
                {
                    if (ucRiskCategoryAdd != null)
                    {
                        ucRiskCategoryAdd.Visible = true;
                        ucRiskCategoryAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 71);
                        ucRiskCategoryAdd.DataBind();
                    }
                    if (ucNCCategoryAdd != null) ucNCCategoryAdd.Visible = false;
                }

                else if (ViewState["type"].ToString() == "1" || ViewState["type"].ToString() == "2")
                {
                    if (ucNCCategoryAdd != null)
                    {
                        ucNCCategoryAdd.Visible = true;
                        ucNCCategoryAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 47);
                        ucNCCategoryAdd.DataBind();
                    }
                    if (ucRiskCategoryAdd != null) ucRiskCategoryAdd.Visible = false;
                }
            }
        }
        
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
