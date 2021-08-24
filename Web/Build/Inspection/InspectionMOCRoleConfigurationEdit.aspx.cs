using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;


public partial class InspectionMOCRoleConfigurationEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Save", "Update", ToolBarDirection.Right);
            
            MenuMOCRoleConfigurationEdit.AccessRights = this.ViewState;
            MenuMOCRoleConfigurationEdit.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["approverroleid"] = "";

                if (Request.QueryString["roleapproverroleid"] != null && Request.QueryString["roleapproverroleid"].ToString() != string.Empty)
                    ViewState["approverroleid"] = Request.QueryString["roleapproverroleid"].ToString();
                
                BindMOCRoleConfig();
            }
            MenuMOCRoleConfigurationEdit.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMOCRoleConfigurationEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("UPDATE") && Request.QueryString["roleapproverroleid"] != null)
                {
                string subcategoryid = Request.QueryString["roleapproverroleid"];

                if (!IsValidMOCCategory(txtShortCodeEdit.Text,
                                    txtRoleEdit.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionMOCApproverRole.MOCApproverRoleUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Guid.Parse(lblRoleIdEdit.Text)
                                                                , txtRoleEdit.Text
                                                                , txtShortCodeEdit.Text);
            }
            else
            {
                if (!IsValidMOCCategory(txtShortCodeEdit.Text,
                                    txtRoleEdit.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionMOCApproverRole.MOCApproverRoleInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , txtRoleEdit.Text
                                                                , txtShortCodeEdit.Text);
            }
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void BindData()
    {
    }

    protected void BindMOCRoleConfig()
    {
        if (ViewState["approverroleid"] != null && ViewState["approverroleid"].ToString() != string.Empty)
        {
            txtShortCodeEdit.ReadOnly = true;
            DataTable dt = PhoenixInspectionMOCApproverRole.MOCApproverRoleedit(General.GetNullableGuid(ViewState["approverroleid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                
                lblRoleIdEdit.Text = dr["FLDMOCAPPROVERROLEID"].ToString();
                txtRoleEdit.Text   = dr["FLDMOCAPPROVERROLE"].ToString();
                txtShortCodeEdit.Text = dr["FLDSHORTCODE"].ToString();
            }
        }
    }
    private bool IsValidMOCCategory(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Role is required.";

        return (!ucError.IsError);
    }
}