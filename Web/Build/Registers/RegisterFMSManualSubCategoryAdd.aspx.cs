using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegisterFMSManualSubCategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "Add", ToolBarDirection.Right);

            MenuFMSManualSubCategoryAdd.AccessRights = this.ViewState;
            MenuFMSManualSubCategoryAdd.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["subcategoryid"] = null;
                if (Request.QueryString["subcategoryid"] != null && Request.QueryString["subcategoryid"].ToString() != string.Empty)
                    ViewState["subcategoryid"] = Request.QueryString["subcategoryid"].ToString();

                BindFMSCategory();

                if (Request.QueryString["subcategoryid"] != null)
                    BindCategory();
            }
            //BindData();
            MenuFMSManualSubCategoryAdd.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuFMSManualSubCategoryAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidFMSManualSubCategory(ddlCategory.SelectedValue
                   , txtCodeAdd.Text
                   , txtSubCategoryAdd.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (Request.QueryString["subcategoryid"] != null && Request.QueryString["subcategoryid"].ToString() != string.Empty)
                {
                    PhoenixRegisterFMSManual.FMSManualSubCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid(ViewState["subcategoryid"].ToString())
                                                                        , General.GetNullableGuid(ddlCategory.SelectedValue)
                                                                        , txtSubCategoryAdd.Text
                                                                        , txtCodeAdd.Text
                                                                        , int.Parse(txtSortNo.Text));
                }
                else
                {
                    PhoenixRegisterFMSManual.FMSManualSubCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , null
                                                                       , General.GetNullableGuid(ddlCategory.SelectedValue)
                                                                       , txtSubCategoryAdd.Text
                                                                       , txtCodeAdd.Text
                                                                       , int.Parse(txtSortNo.Text));
                }
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
    private bool IsValidFMSManualSubCategory(string category, string shortcode, string subcategory)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (subcategory.Trim().Equals(""))
            ucError.ErrorMessage = "SubCategory is required.";

        return (!ucError.IsError);
    }
    protected void BindFMSCategory()
    {
        string category = Request.QueryString["categoryid"];

        ddlCategory.DataSource = PhoenixRegisterFMSManual.FMSManualCategoryList();
        ddlCategory.DataTextField = "FLDMANUALCATEGORY";
        ddlCategory.DataValueField = "FLDFMSMANUALCATEGORY";
        ddlCategory.DataBind();
        ddlCategory.SelectedValue = category;
    }

    private void BindCategory()
    {
        if (ViewState["subcategoryid"] != null && ViewState["subcategoryid"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixRegisterFMSManual.FMSManualSubCategoryEdit(Guid.Parse(ViewState["subcategoryid"].ToString()));

            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtCodeAdd.Text = dr["FLDSUBCATEGORYCODE"].ToString();
                txtCodeAdd.ReadOnly = true;
                txtSubCategoryAdd.Text = dr["FLDSUBCATEGORYNAME"].ToString();
                ddlCategory.SelectedValue = dr["FLDFMSMANUALCATEGORY"].ToString();
                txtSortNo.Text = dr["FLDSORTORDER"].ToString();
            }
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    protected void BindData()
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

}