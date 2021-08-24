using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionMOCTypeofChangeCategoryEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "Update", ToolBarDirection.Right);

        MenuRegistersTypeofChangeCategoryEdit.AccessRights = this.ViewState;
        MenuRegistersTypeofChangeCategoryEdit.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["subcategoryid"] = "";

            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                ViewState["subcategoryid"] = Request.QueryString["categoryid"].ToString();
            BindMOC();
        }
        MenuRegistersTypeofChangeCategoryEdit.SelectedMenuIndex = 0;
    }

    protected void MenuRegistersTypeofChangeCategoryEdit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UPDATE") && Request.QueryString["categoryid"] !=null)
        {
            string category = Request.QueryString["categoryid"];

            if (!IsValidMOCCategory(txtcode.Text, txtname.Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCCategory.MOCCategoryUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , Guid.Parse(category)
                                                            , txtname.Text, txtcode.Text);
        }

        else
        {
            if (!IsValidMOCCategory(txtcode.Text,txtname.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixInspectionMOCCategory.MOCCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , txtname.Text
                                                            , txtcode.Text);
        }

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
    protected void BindMOC()
    {
        if (ViewState["subcategoryid"] != null && ViewState["subcategoryid"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixInspectionMOCCategory.MOCCategoryMappingEdit(Guid.Parse(ViewState["subcategoryid"].ToString()));

            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtcode.Text = dr["FLDSHORTCODE"].ToString();
                txtcode.ReadOnly = true;
                txtname.Text = dr["FLDMOCCATEGORYNAME"].ToString();
            }
        }
    }

    private bool IsValidMOCCategory(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }


}