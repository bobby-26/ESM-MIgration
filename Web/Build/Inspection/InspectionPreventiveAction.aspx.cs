using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class InspectionPreventiveAction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PREVENTIVEACTIONID"] = null;
                ViewState["REFERENCEID"] = null;
                ViewState["REFFROM"] = null;
                ViewState["VESSELID"] = null;
                ViewState["COMPANYID"] = "";
                ViewState["View"] = "";

                if (Request.QueryString["PREVENTIVEACTIONID"] != null && Request.QueryString["PREVENTIVEACTIONID"].ToString() != string.Empty)
                    ViewState["PREVENTIVEACTIONID"] = Request.QueryString["PREVENTIVEACTIONID"].ToString();

                if (Request.QueryString["REFFROM"] != null && Request.QueryString["REFFROM"].ToString() != string.Empty)
                    ViewState["REFFROM"] = Request.QueryString["REFFROM"].ToString();

                if (Request.QueryString["REFERENCEID"] != null && Request.QueryString["REFERENCEID"].ToString() != string.Empty)
                    ViewState["REFERENCEID"] = Request.QueryString["REFERENCEID"].ToString();

                if (Request.QueryString["View"] != null && Request.QueryString["View"].ToString() != string.Empty)
                    ViewState["View"] = Request.QueryString["View"].ToString();

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

               
                lblCompletionDate.Visible = false;
                ucCompletionDate.Visible = false;
                
                //if (ViewState["PREVENTIVEACTIONID"] == null)
                //    MenuCARGeneral.Title = "Preventive Action Add";
                //else
                //    MenuCARGeneral.Title = "Preventive Action Edit";
                txtCAN.Focus();

                tdddlSubcategory.Visible = true;
                tdspnPickListDocument.Visible = false;

                BindVesselid();
                BindCategory();
                BindSubcategory();
                BindVesselTypeList();
                BindVesselList();
                BindPreventiveAction();

                if (ViewState["PREVENTIVEACTIONID"] != null && General.GetNullableDateTime(ucTargetDate.Text) != null)
                {
                    SetRights();
                }
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            if (ViewState["View"].ToString() == "1")
            {
                toolbar.AddButton("Back", "INCIDENTLIST", ToolBarDirection.Right);
            }
            else
            {
                toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            }
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        
    }

    protected void BindVesselid()
    {
        if (ViewState["REFERENCEID"] != null)
        {
            DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(ViewState["REFERENCEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

            ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["REFERENCEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

            ds = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["REFERENCEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VESSELID"] = int.Parse(dr["FLDVESSELID"].ToString());
            }
        }
    }

    protected void BindCategory()
    {
        ddlCategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskCategoryList();
        ddlCategory.DataTextField = "FLDCATEGORYNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindSubcategory()
    {
        ddlSubcategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubcategory.DataTextField = "FLDSUBCATEGORYNAME";
        ddlSubcategory.DataValueField = "FLDSUBCATEGORYID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        DataSet ds = PhoenixInspectionTaskCategory.InspectionTaskCategoryGet(General.GetNullableGuid(ddlCategory.SelectedValue));

        if (ds.Tables[0].Rows.Count > 0)
            ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
        else
            ViewState["SHORTCODE"] = ""; 

        if (ViewState["SHORTCODE"].ToString() == "RCH")
        {
            tdddlSubcategory.Visible = false;
            tdspnPickListDocument.Visible = true;
        }
        else
        {
            tdddlSubcategory.Visible = true;
            tdspnPickListDocument.Visible = false;
            BindSubcategory();
        }
        ddlSubcategory.ClearSelection();
    }

    protected void BindPreventiveAction()
    {
        if (ViewState["PREVENTIVEACTIONID"] != null && ViewState["PREVENTIVEACTIONID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionPreventiveAction.EditPreventiveAction(new Guid(ViewState["PREVENTIVEACTIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCAN.Text = dr["FLDCONTROLACTIONNEEDS"].ToString();
                txtPreventiveAction.Text = dr["FLDPREVENTIVEACTION"].ToString();
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                BindCategory();
                if (dr["FLDCATEGORYID"] != null && dr["FLDCATEGORYID"].ToString() != "")
                    ddlCategory.SelectedValue = dr["FLDCATEGORYID"].ToString();
                DataSet ds1 = PhoenixInspectionTaskCategory.InspectionTaskCategoryGet(General.GetNullableGuid(ddlCategory.SelectedValue));
                if (ds1.Tables[0].Rows.Count > 0)
                    ViewState["SHORTCODE"] = ds1.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                else
                    ViewState["SHORTCODE"] = "";
                if (ViewState["SHORTCODE"].ToString() == "RCH")
                {
                    tdddlSubcategory.Visible = false;
                    tdspnPickListDocument.Visible = true;
                    txtDocumentName.Text = dr["FLDDMSTASKSUBCATEGORYNAME"].ToString();
					txtDocumentId.Text = dr["FLDTASKSUBCATEGORY"].ToString();
                }
                else
                {
                    tdddlSubcategory.Visible = true;
                    tdspnPickListDocument.Visible = false;
                    BindSubcategory();
                    if (dr["FLDSUBCATEGORYID"] != null && dr["FLDSUBCATEGORYID"].ToString() != "")
                        ddlSubcategory.SelectedValue = dr["FLDSUBCATEGORYID"].ToString();
                }
                ucDept.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                ucDept.SelectedDepartment = dr["FLDDEPARTMENT"].ToString();
                BindSubdepartment();
                ucSubDept.SelectedSubDepartment = dr["FLDSUBDEPARTMENTMAPPINGID"].ToString();
                string vessellist = dr["FLDMANUALTASKVESSELLIST"].ToString();
                General.RadBindCheckBoxList(chkVessel, vessellist);
            }
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["REFFROM"].ToString().Equals("nc"))
                {
                    Response.Redirect("../Inspection/InspectionDirectNonConformityCauseAnalysis.aspx?REFFROM=nc&REVIEWDNC=" + ViewState["REFERENCEID"].ToString(), false);
                }
                else if (ViewState["REFFROM"].ToString().Equals("incident"))
                {
                    Response.Redirect("../Inspection/InspectionIncidentCriticalFactor.aspx", false);
                }
                else if (ViewState["REFFROM"].ToString().Equals("obs"))
                {
                    Response.Redirect("../Inspection/InspectionObservationCorrectiveAction.aspx?REFFROM=obs&DIRECTOBSERVATIONID=" + ViewState["REFERENCEID"].ToString(), false);
                }
            }
            else if (CommandName.ToUpper().Equals("INCIDENTLIST"))
            {
                Response.Redirect("../Inspection/InspectionIncidentActionsView.aspx?inspectionincidentid=" + ViewState["REFERENCEID"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["PREVENTIVEACTIONID"] == null)
                    InsertPreventiveAction();
                else
                    UpdatePreventiveAction();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InsertPreventiveAction()
    {
        if (!IsValidPreventiveAction())
        {
            ucError.Visible = true;
            return;
        }
        string vessellist = GetSelectedVessels();
        string subcategory = null;
        DataSet ds = PhoenixInspectionTaskCategory.InspectionTaskCategoryGet(General.GetNullableGuid(ddlCategory.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
            ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
        else
            ViewState["SHORTCODE"] = "";        
        if (General.GetNullableString(ViewState["SHORTCODE"].ToString()) == "RCH")
        {
            subcategory = txtDocumentId.Text;
        }
        else
        {
            subcategory = ddlSubcategory.SelectedValue;
        }
        if (ViewState["REFFROM"].ToString().Equals("incident"))
        {
            PhoenixInspectionPreventiveAction.InsertIncidentPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["REFERENCEID"].ToString()), General.GetNullableString(txtPreventiveAction.Text),
                int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(ucTargetDate.Text), null, null, General.GetNullableInteger(ucDept.SelectedDepartment),
                General.GetNullableGuid(ddlCategory.SelectedValue), General.GetNullableGuid(subcategory.ToString()), null,
                General.GetNullableString(txtCAN.Text), General.GetNullableInteger(ucSubDept.SelectedSubDepartment), General.GetNullableString(vessellist));
        }
        else if (ViewState["REFFROM"].ToString().Equals("nc"))
        {
            PhoenixInspectionPreventiveAction.InsertNCPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["REFERENCEID"].ToString()), General.GetNullableString(txtPreventiveAction.Text),
                DateTime.Parse(ucTargetDate.Text), int.Parse(ViewState["VESSELID"].ToString()), null, General.GetNullableInteger(ucDept.SelectedDepartment),
                General.GetNullableGuid(ddlCategory.SelectedValue), General.GetNullableGuid(subcategory.ToString()), null, General.GetNullableString(txtCAN.Text),
                General.GetNullableInteger(ucSubDept.SelectedSubDepartment), General.GetNullableString(vessellist));
        }
        else if (ViewState["REFFROM"].ToString().Equals("obs"))
        {
            PhoenixInspectionPreventiveAction.InsertObsPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["REFERENCEID"].ToString()), General.GetNullableString(txtPreventiveAction.Text),
                DateTime.Parse(ucTargetDate.Text), int.Parse(ViewState["VESSELID"].ToString()), null, General.GetNullableInteger(ucDept.SelectedDepartment),
                General.GetNullableGuid(ddlCategory.SelectedValue), General.GetNullableGuid(subcategory.ToString()), null, General.GetNullableString(txtCAN.Text),
                General.GetNullableInteger(ucSubDept.SelectedSubDepartment), General.GetNullableString(vessellist));
        }
        ucStatus.Text = "Preventive Action inserted.";

        if (ViewState["REFFROM"].ToString().Equals("nc"))
        {
            Response.Redirect("../Inspection/InspectionDirectNonConformityCauseAnalysis.aspx?REFFROM=nc&REVIEWDNC=" + ViewState["REFERENCEID"].ToString(), false);
        }
        else if (ViewState["REFFROM"].ToString().Equals("incident"))
        {
            Response.Redirect("../Inspection/InspectionIncidentCriticalFactor.aspx", false);
        }
        else if (ViewState["REFFROM"].ToString().Equals("obs"))
        {
            Response.Redirect("../Inspection/InspectionObservationCorrectiveAction.aspx?REFFROM=obs&DIRECTOBSERVATIONID=" + ViewState["REFERENCEID"].ToString(), false);
        }
        //BindPreventiveAction();
        //String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
        //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdatePreventiveAction()
    {
        if (!IsValidPreventiveAction())
        {
            ucError.Visible = true;
            return;
        }
        string vessellist = GetSelectedVessels();
        string subcategory = null;
        DataSet ds = PhoenixInspectionTaskCategory.InspectionTaskCategoryGet(General.GetNullableGuid(ddlCategory.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
            ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
        else
            ViewState["SHORTCODE"] = "";
        if (ViewState["SHORTCODE"].ToString() == "RCH")
        {
            subcategory = txtDocumentId.Text;
        }
        else
        {
            subcategory = ddlSubcategory.SelectedValue;
        }
        if (ViewState["REFFROM"].ToString().Equals("incident"))
        {
            PhoenixInspectionPreventiveAction.UpdateIncidentPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["PREVENTIVEACTIONID"].ToString()), General.GetNullableString(txtPreventiveAction.Text),
                General.GetNullableDateTime(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text), null, null, null,
                General.GetNullableInteger(ucDept.SelectedDepartment), General.GetNullableGuid(ddlCategory.SelectedValue),
                General.GetNullableGuid(subcategory.ToString()), null, General.GetNullableString(txtCAN.Text),
                General.GetNullableInteger(ucSubDept.SelectedSubDepartment), General.GetNullableString(vessellist));
        }
        else if (ViewState["REFFROM"].ToString().Equals("nc"))
        {
            PhoenixInspectionPreventiveAction.UpdateNCPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["PREVENTIVEACTIONID"].ToString()), General.GetNullableString(txtPreventiveAction.Text),
                General.GetNullableDateTime(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text), null, null,
                General.GetNullableInteger(ucDept.SelectedDepartment), General.GetNullableGuid(ddlCategory.SelectedValue),
                General.GetNullableGuid(subcategory.ToString()), null, General.GetNullableString(txtCAN.Text),
                General.GetNullableInteger(ucSubDept.SelectedSubDepartment), General.GetNullableString(vessellist));
        }
        else if (ViewState["REFFROM"].ToString().Equals("obs"))
        {
            PhoenixInspectionPreventiveAction.UpdateObsPreventiveAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["PREVENTIVEACTIONID"].ToString()), General.GetNullableString(txtPreventiveAction.Text),
                General.GetNullableDateTime(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text), null, null,
                General.GetNullableInteger(ucDept.SelectedDepartment), General.GetNullableGuid(ddlCategory.SelectedValue),
                General.GetNullableGuid(subcategory.ToString()), null, General.GetNullableString(txtCAN.Text),
                General.GetNullableInteger(ucSubDept.SelectedSubDepartment), General.GetNullableString(vessellist));

        }
        ucStatus.Text = "Preventive Action updated.";
        BindPreventiveAction();
        String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidPreventiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (string.IsNullOrEmpty(txtCAN.Text))
        //    ucError.ErrorMessage = "Control Action Needs is required.";

        if (string.IsNullOrEmpty(txtPreventiveAction.Text))
            ucError.ErrorMessage = "Preventive Action is required.";

        if (General.GetNullableDateTime(ucTargetDate.Text) == null)
            ucError.ErrorMessage = "Target Date is required.";

        if (General.GetNullableDateTime(ucCompletionDate.Text) != null && General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
            ucError.ErrorMessage = "Completion Date cannot be the future date.";

        return (!ucError.IsError);
    }

    protected void ucDept_TextChangedEvent(object sender, EventArgs e)
    {
        BindSubdepartment();
    }

    protected void BindSubdepartment()
    {
        ucSubDept.DepartmentFilter = ucDept.SelectedDepartment;
        ucSubDept.bind();
    }

    public void SetRights()
    {
        ucTargetDate.Enabled = SessionUtil.CanAccess(this.ViewState, "TARGETDATE");
    }

    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataBindings.DataTextField = "FLDHARDNAME";
        chkVesselType.DataBindings.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }

    protected void BindVesselList()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        chkVessel.Items.Clear();
        chkVessel.DataSource = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null, companyid, null, 1);
        chkVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        chkVessel.DataBindings.DataValueField = "FLDVESSELID";
        chkVessel.DataBind();
    }

    protected void chkVesselType_Changed(object sender, EventArgs e)
    {
        string selectedcategorylist = GetSelectedVesselType();

        foreach (ButtonListItem item in chkVessel.Items)
            item.Selected = false;

        int? companyid = General.GetNullableInteger(ViewState["COMPANYID"].ToString());
        DataSet ds = PhoenixRegistersVessel.ListVessel(null, null, null, null, null, companyid, General.GetNullableString(selectedcategorylist));

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (ButtonListItem item in chkVessel.Items)
                {
                    string vesselid = dr["FLDVESSELID"].ToString();

                    if (item.Value == vesselid && !item.Selected && General.GetNullableString(selectedcategorylist) != null)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
    }

    protected void chkVesselTypeAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkVesselTypeAll.Checked == true)
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = true;

                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = false;

                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected string GetSelectedVesselType()
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (ButtonListItem item in chkVesselType.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strvesseltype.Append(item.Value.ToString());
                strvesseltype.Append(",");
            }
        }

        if (strvesseltype.Length > 1)
            strvesseltype.Remove(strvesseltype.Length - 1, 1);

        string vesseltype = strvesseltype.ToString();
        return vesseltype;
    }
    protected string GetSelectedVessels()
    {
        StringBuilder strVessel = new StringBuilder();
        foreach (ButtonListItem item in chkVessel.Items)
        {
            if (item.Selected == true)
            {
                strVessel.Append(item.Value.ToString());
                strVessel.Append(",");
            }
        }

        if (strVessel.Length > 1)
            strVessel.Remove(strVessel.Length - 1, 1);

        string categoryList = strVessel.ToString();
        return categoryList;
    }
}
