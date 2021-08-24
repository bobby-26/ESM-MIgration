using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionMachineryDamagePreventiveAction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK",ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PREVENTIVEACTIONID"] = null;
                ViewState["MACHINERYDAMAGEID"] = null;
                ViewState["REFFROM"] = null;
                ViewState["VESSELID"] = null;
                ViewState["SHORTCODE"] = "";

                if (Request.QueryString["PREVENTIVEACTIONID"] != null && Request.QueryString["PREVENTIVEACTIONID"].ToString() != string.Empty)
                    ViewState["PREVENTIVEACTIONID"] = Request.QueryString["PREVENTIVEACTIONID"].ToString();

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();

                if (Request.QueryString["MACHINERYDAMAGEID"] != null && Request.QueryString["MACHINERYDAMAGEID"].ToString() != string.Empty)
                    ViewState["MACHINERYDAMAGEID"] = Request.QueryString["MACHINERYDAMAGEID"].ToString();

                lblCompletionDate.Visible = false;
                ucCompletionDate.Visible = false;

                //if (ViewState["PREVENTIVEACTIONID"] == null)
                //    MenuCARGeneral.title = "Add";
                //else
                //    MenuCARGeneral.title = "Edit";

                tdddlSubcategory.Visible = true;
                tdspnPickListDocument.Visible = false;

                BindCategory();
                BindSubcategory();
                BindPreventiveAction();

                if (ViewState["PREVENTIVEACTIONID"] != null && General.GetNullableDateTime(ucTargetDate.Text) != null)
                    SetRights();
            }
            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();

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
    }

    protected void BindPreventiveAction()
    {
        if (ViewState["PREVENTIVEACTIONID"] != null && ViewState["PREVENTIVEACTIONID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionPreventiveAction.EditPreventiveAction(new Guid(ViewState["PREVENTIVEACTIONID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtPreventiveAction.Text = dr["FLDPREVENTIVEACTION"].ToString();
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                BindCategory();
                if (dr["FLDCATEGORYID"] != null && dr["FLDCATEGORYID"].ToString() != "")
                    ddlCategory.SelectedValue = dr["FLDCATEGORYID"].ToString();
                DataSet ds1 = PhoenixInspectionTaskCategory.InspectionTaskCategoryGet(General.GetNullableGuid(ddlCategory.SelectedValue));
                if (ds1.Tables[0].Rows[0]["FLDSHORTCODE"].ToString() == "RCH")
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
            }
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (Request.QueryString["MACHINERYDAMAGEID"].ToString() != string.Empty)
                {
                    Response.Redirect("../Inspection/InspectionMachineryDamageCAR.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString(), false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionMachineryDamageCAR.aspx", false);
                }
            }
            if (CommandName.ToUpper().Equals("SAVE"))
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
        string subcategory = null;
        if (ViewState["SHORTCODE"].ToString() == "RCH")
        {
            subcategory = txtDocumentId.Text;
        }
        else
        {
            subcategory = ddlSubcategory.SelectedValue;
        }
        PhoenixInspectionPreventiveAction.InsertMachineryDamagePreventiveAction(
             PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()),
             General.GetNullableString(txtPreventiveAction.Text),
             int.Parse(ViewState["VESSELID"].ToString()),
             DateTime.Parse(ucTargetDate.Text), null,
             General.GetNullableInteger(ucDept.SelectedDepartment),
             General.GetNullableGuid(ddlCategory.SelectedValue),
             General.GetNullableGuid(subcategory.ToString()),
             General.GetNullableInteger(ucSubDept.SelectedSubDepartment));

        ucStatus.Text = "Preventive Action inserted.";

        Response.Redirect("../Inspection/InspectionMachineryDamageCAR.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&VESSELID=" + ViewState["VESSELID"].ToString(), false);

        BindPreventiveAction();

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdatePreventiveAction()
    {
        if (!IsValidPreventiveAction())
        {
            ucError.Visible = true;
            return;
        }
        string subcategory = null;
        DataSet ds = PhoenixInspectionTaskCategory.InspectionTaskCategoryGet(General.GetNullableGuid(ddlCategory.SelectedValue));
        ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
        if (ViewState["SHORTCODE"].ToString() == "RCH")
        {
            subcategory = txtDocumentId.Text;
        }
        else
        {
            subcategory = ddlSubcategory.SelectedValue;
        }
        PhoenixInspectionPreventiveAction.UpdateMachineryDamagePreventiveAction(
             PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString()),
             General.GetNullableString(txtPreventiveAction.Text),
             DateTime.Parse(ucTargetDate.Text), General.GetNullableDateTime(ucCompletionDate.Text),
             null,
             null,
             General.GetNullableInteger(ucDept.SelectedDepartment),
             General.GetNullableGuid(ddlCategory.SelectedValue),
             General.GetNullableGuid(subcategory.ToString()),
             General.GetNullableInteger(ucSubDept.SelectedSubDepartment));


        ucStatus.Text = "Preventive Action updated.";
        BindPreventiveAction();
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidPreventiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["MACHINERYDAMAGEID"] == null || General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()) == null)
            ucError.ErrorMessage = "Machinery Damage details are not yet added.";

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
}
