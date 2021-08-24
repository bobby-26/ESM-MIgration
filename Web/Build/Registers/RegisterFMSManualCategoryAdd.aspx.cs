using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegisterFMSManualCategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "Update", ToolBarDirection.Right);

        MenuRegistersTypeofChangeCategoryAdd.AccessRights = this.ViewState;
        MenuRegistersTypeofChangeCategoryAdd.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["categoryid"] = null;
            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                ViewState["categoryid"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["categoryid"] != null)
                BindCategory();
        }
        MenuRegistersTypeofChangeCategoryAdd.SelectedMenuIndex = 0;
    }

    private void BindCategory()
    {
        if (ViewState["categoryid"] != null && ViewState["categoryid"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixRegisterFMSManual.FMSManualCategoryEdit(Guid.Parse(ViewState["categoryid"].ToString()));

            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtcode.Text = dr["FLDMANUALCODE"].ToString();
                txtcode.ReadOnly = true;
                txtname.Text = dr["FLDMANUALCATEGORY"].ToString();
            }
        }
    }

    protected void MenuRegistersTypeofChangeCategoryAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UPDATE"))
        {
            if (!IsValidMOCCategory(txtcode.Text, txtname.Text))
            {
                ucError.Visible = true;
                return;
            }

            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
            {
                PhoenixRegisterFMSManual.FMSManualCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["categoryid"].ToString()), txtname.Text, txtcode.Text);
            }
            else
            {
                PhoenixRegisterFMSManual.FMSManualCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, txtname.Text, txtcode.Text);
            }
        }

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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