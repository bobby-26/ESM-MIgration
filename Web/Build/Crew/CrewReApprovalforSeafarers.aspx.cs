using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using Telerik.Web.UI;
public partial class CrewReApprovalforSeafarers : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (Request.QueryString["approvedyn"].ToString() == "0")
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                CrewReApprovalSeafarer.AccessRights = this.ViewState;
                CrewReApprovalSeafarer.MenuList = toolbarmain.Show();
            }
           
            if (!IsPostBack)
            {
                txtCourseId.Attributes.Add("style", "display:none;");
                cmdHiddenPick.Attributes.Add("style", "display:none;");

                BindCategoryDropDown();

                ViewState["CREWPLANID"] = "";
                ViewState["SIGNONOFFID"] = "";
                if (Request.QueryString["docid"] != null && Request.QueryString["docid"] != string.Empty)
                {
                    PoplateEmployeeDetails(Request.QueryString["docid"]);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rblActionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txtCourseId.Text = rblActionType.SelectedValue == "1" ? txtCourseId.Text : "";
            txtCourseName.Text = rblActionType.SelectedValue == "1" ? txtCourseName.Text : "";
            imgShowCourse.Disabled = rblActionType.SelectedValue == "1" ? false : true;

            if (rblActionType.SelectedValue == "2" || rblActionType.SelectedValue == "3")
            {
                txtCouncelledRemarks.CssClass = "input_mandatory";
            }
            else
            {
                txtCouncelledRemarks.CssClass = "";
            }
           
            if (rblActionType.SelectedValue == "1")
            {
                txtCourseName.Enabled = true;
                txtCourseName.CssClass = "input_mandatory";
                txtCourseName.ReadOnly = false;
               
                imgShowCourse.Attributes.Add("onclick", "return showPickList('spnCourse', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCourse.aspx?rankid=" + Request.QueryString["RankId"] + "&vessel=" + Request.QueryString["VesselId"] + "&empid=" + Request.QueryString["EmployeeId"] + "', 'yes'); return true");
            }
            else
            {
                txtCourseName.Enabled = false;
                txtCourseName.CssClass = "";
                imgShowCourse.Attributes.Remove("onclick");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rblStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            rblManagerPrincipal.Enabled = rblStatus.SelectedValue == "1" ? true : false;
            if (rblStatus.SelectedValue == "0")//ReEmployment
            {
                for (int i = 0; i < rblManagerPrincipal.Items.Count; i++)
                {
                    rblManagerPrincipal.Items[i].Selected = false;
                }
                
                ddlManager.Enabled = ucPrinicipal.Enabled = false;
                ddlManager.SelectedAddress = ucPrinicipal.SelectedAddress = string.Empty;
                ddlManager.CssClass = ucPrinicipal.CssClass = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewReApprovalSeafarer_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName == "SAVE")
            {
                if (!IsValidateReApprovalSeafarer())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewReApprovalSeafarer.ReApprovalSeafarerInsert(General.GetNullableInteger(Request.QueryString["EmployeeId"])
                    , General.GetNullableInteger(Request.QueryString["RankId"])
                    , General.GetNullableInteger(Request.QueryString["VesselId"])
                    , General.GetNullableDateTime(Request.QueryString["SignoffDate"])
                    , General.GetNullableInteger(ddlCategory.SelectedValue.Trim())
                    , General.GetNullableInteger(ddlSubCategory.SelectedValue.Trim())
                    , General.GetNullableInteger(rblActionType.SelectedValue.Trim())
                    , General.GetNullableString(txtCourseId.Text.Trim())
                    , General.GetNullableString(txtCouncelledRemarks.Text.Trim())
                    , General.GetNullableInteger(rblStatus.SelectedValue.Trim())
                    , General.GetNullableInteger(rblManagerPrincipal.SelectedValue.Trim())
                    , (rblManagerPrincipal.SelectedValue.Trim() == "1" ? General.GetNullableInteger(ddlManager.SelectedAddress) : General.GetNullableInteger(ucPrinicipal.SelectedAddress))
                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(string.IsNullOrEmpty(ViewState["CREWPLANID"].ToString()) ? "" : ViewState["CREWPLANID"].ToString())
                    , General.GetNullableInteger(string.IsNullOrEmpty(ViewState["SIGNONOFFID"].ToString()) ? "" : ViewState["SIGNONOFFID"].ToString())
                    , General.GetNullableGuid(Request.QueryString["docid"]));
                ucStatus.Text = "Inserted Sucessfully";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void BindCategoryDropDown()
    {
        DataTable dt = PhoenixCrewReApprovalSeafarer.CategoryList();
        if (dt.Rows.Count > 0)
        {
            ddlCategory.DataSource = dt;
            ddlCategory.DataValueField = "FLDCATEGORYID";
            ddlCategory.DataTextField = "FLDCATEGORYNAME";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            ddlCategory_SelectedIndexChanged(null, null);
        }
    }
    private bool IsValidateReApprovalSeafarer()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (ddlCategory.SelectedValue == string.Empty)
            ucError.ErrorMessage = "Category is Required";
        if (ddlSubCategory.SelectedValue == string.Empty)
            ucError.ErrorMessage = "Sub Category is Required";
        if (rblActionType.SelectedValue == string.Empty)
            ucError.ErrorMessage = "Action to be Taken is Required";
        if (rblActionType.SelectedValue == "1" && txtCourseId.Text.Trim() == "")
            ucError.ErrorMessage = "Courses is Required";
        if (rblActionType.SelectedValue == "2" && txtCouncelledRemarks.Text.Trim() == "")
            ucError.ErrorMessage = "Remarks is Required";
        if (rblActionType.SelectedValue == "3" && txtCouncelledRemarks.Text.Trim() == "")
            ucError.ErrorMessage = "Remarks is Required";
        if (rblStatus.SelectedValue == "")
            ucError.ErrorMessage = "Status is Required";
        if (rblStatus.SelectedValue == "1" && ddlManager.SelectedAddress == "" && ucPrinicipal.SelectedAddress == "")
            ucError.ErrorMessage = "Principal or Manager is Required";
        if (rblManagerPrincipal.SelectedValue == "1" && ddlManager.SelectedAddress == "")
            ucError.ErrorMessage = "Manager is Required";
        if (rblManagerPrincipal.SelectedValue == "2" && ucPrinicipal.SelectedAddress == "")
            ucError.ErrorMessage = "Principle is Required";
        if (Request.QueryString["EmployeeId"] == "")
            ucError.ErrorMessage = "Re-Employment Assessment is Required";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;

        if (Filter.CurrentPickListSelection.Keys[1].ToString() == "txtCourseName")
            txtCourseName.Text = Filter.CurrentPickListSelection.Get(1);

        if (Filter.CurrentPickListSelection.Keys[2].ToString() == "txtCourseId")
            txtCourseId.Text = Filter.CurrentPickListSelection.Get(2);
    }
    private void PoplateEmployeeDetails(string DtKey)
    {
        DataSet ds = PhoenixCrewReApprovalSeafarer.SearchSeafarerReApprovalDetails(General.GetNullableGuid(DtKey));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["CREWPLANID"] = dr["FLDCREWPLANID"].ToString().Trim();
            ViewState["SIGNONOFFID"] = dr["FLDSIGNONOFFID"].ToString().Trim();
            ddlCategory.SelectedValue = dr["FLDCATEGORYID"].ToString().Trim();
            ddlCategory_SelectedIndexChanged(null, null);
            ddlSubCategory.SelectedValue = dr["FLDSUBCATEGORYID"].ToString().Trim();
            txtCourseId.Text = dr["FLDRECOMMENDEDCOURSELIST"].ToString().Trim();
            txtCourseName.Text = dr["FLDRECOMMENDEDCOURSENAMELIST"].ToString().Trim();

            if (dr["FLDACTIONTAKEN"].ToString().Trim() != string.Empty)
            {
                rblActionType.SelectedValue = dr["FLDACTIONTAKEN"].ToString().Trim();
                rblActionType_SelectedIndexChanged(null, null);
            }
            else
            {
                for (int i = 0; i < rblActionType.Items.Count; i++)
                {
                    rblActionType.Items[i].Selected = false;
                }
                
                rblActionType_SelectedIndexChanged(null, null);
            }
            if (dr["FLDSTATUS"].ToString().Trim() != string.Empty)
            {
                rblStatus.SelectedValue = dr["FLDSTATUS"].ToString().Trim();
                rblStatus_SelectedIndexChanged(null, null);
            }
            else
            {
                for (int i = 0; i < rblStatus.Items.Count; i++)
                {
                    rblStatus.Items[i].Selected = false;
                }

                rblStatus_SelectedIndexChanged(null, null);
            }
            txtCouncelledRemarks.Text = dr["FLDCOUNSELLINGREMARKS"].ToString().Trim();
            if (rblStatus.SelectedValue == "1")//NTBR
            {
                rblManagerPrincipal.SelectedValue = dr["FLDMANAGERPRINCIPAL"].ToString().Trim();
                rblManagerPrincipal_SelectedIndexChanged(null, null);
            }
            else
            {
                for (int i = 0; i < rblManagerPrincipal.Items.Count; i++)
                {
                    rblManagerPrincipal.Items[i].Selected = false;
                }
              
                rblManagerPrincipal_SelectedIndexChanged(null, null);
            }

            if (rblManagerPrincipal.SelectedValue == "2" && dr["FLDMANAGERPRINCIPALID"].ToString().Trim() != string.Empty)
                ucPrinicipal.SelectedAddress = dr["FLDMANAGERPRINCIPALID"].ToString().Trim();
            else
                ucPrinicipal.SelectedAddress = "";

            if (rblManagerPrincipal.SelectedValue == "1" && dr["FLDMANAGERPRINCIPALID"].ToString().Trim() != string.Empty)
                ddlManager.SelectedAddress = dr["FLDMANAGERPRINCIPALID"].ToString().Trim();
            else
                ddlManager.SelectedAddress = "";

            txtEmployeeFileNo.Text = dr["FLDFILENO"].ToString();
            txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
        }
        else
        {
            ddlCategory.SelectedValue = ddlSubCategory.SelectedValue = string.Empty;
            txtCouncelledRemarks.Text = txtCourseId.Text = txtCourseName.Text = string.Empty;            
            for (int i = 0; i < rblActionType.Items.Count; i++)
            {
                rblActionType.Items[i].Selected = false;
            }
            for (int i = 0; i < rblStatus.Items.Count; i++)
            {
                rblStatus.Items[i].Selected = false;
            }
            for (int i = 0; i < rblManagerPrincipal.Items.Count; i++)
            {
                rblManagerPrincipal.Items[i].Selected = false;
            }

            txtCourseName.Enabled = txtCouncelledRemarks.Enabled = rblManagerPrincipal.Enabled = ucPrinicipal.Enabled = ddlManager.Enabled = false;
            imgShowCourse.Disabled = true;
        }
    }

    protected void rblManagerPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlManager.Enabled = rblManagerPrincipal.SelectedValue == "1" ? true : false;
            ucPrinicipal.Enabled = rblManagerPrincipal.SelectedValue == "2" ? true : false;
            ucPrinicipal.CssClass = rblManagerPrincipal.SelectedValue == "2" ? "input_mandatory" : "";
            ddlManager.CssClass = rblManagerPrincipal.SelectedValue == "1" ? "input_mandatory" : "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSubCategory.DataSource = null;
            ddlSubCategory.DataBind();

            DataTable dt = PhoenixCrewReApprovalSeafarer.SubCategoryList(General.GetNullableInteger(ddlCategory.SelectedValue.Trim()));
            ddlSubCategory.DataSource = dt;
            ddlSubCategory.DataValueField = "FLDSUBCATEGORYID";
            ddlSubCategory.DataTextField = "FLDSUBCATEGORYNAME";
            ddlSubCategory.DataBind();
            ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
