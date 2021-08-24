using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCSubcategoryEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "Update", ToolBarDirection.Right);

        MenuRegistersSubCategoryEdit.AccessRights = this.ViewState;
        MenuRegistersSubCategoryEdit.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["mocsubcategoryid"] = "";

            if (Request.QueryString["mocsubcategoryid"] != null && Request.QueryString["mocsubcategoryid"].ToString() != string.Empty)
                ViewState["mocsubcategoryid"] = Request.QueryString["mocsubcategoryid"].ToString();
            BindMOC();
        }
        MenuRegistersSubCategoryEdit.SelectedMenuIndex = 0;
    }

    protected void MenuRegistersSubCategoryEdit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UPDATE") && Request.QueryString["mocsubcategoryid"]!=null)
        {
            string subcategoryid = Request.QueryString["mocsubcategoryid"];
            if (!IsValidCountry(txtShortCodeEdit.Text,
                        txtCategoryEdit.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixInspectionMOC.MOCSubCategoryUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                             , Guid.Parse(subcategoryid)
                                             , txtCategoryEdit.Text
                                             , txtShortCodeEdit.Text);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
    private bool IsValidCountry(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        
        return (!ucError.IsError);
    }
    protected void BindMOC()
    {
        if (ViewState["mocsubcategoryid"] != null && ViewState["mocsubcategoryid"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixInspectionMOC.MOCSubCategorymappingedit(Guid.Parse(ViewState["mocsubcategoryid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtShortCodeEdit.Text = dr["FLDSHORTCODE"].ToString();
                txtCategoryEdit.Text = dr["FLDMOCHARDNAME"].ToString();
            }
        }
    }
}