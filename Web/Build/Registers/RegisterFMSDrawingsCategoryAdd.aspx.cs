using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegisterFMSDrawingsCategoryAdd : PhoenixBasePage
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
                if (!IsValidFMSManualSubCategory(txtSortNo.Text
                    , txtCodeAdd.Text
                   , txtSubCategoryAdd.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (Request.QueryString["subcategoryid"] != null && Request.QueryString["subcategoryid"].ToString() != string.Empty)
                {
                      PhoenixRegisterFMSDrawing.FMSDrawingCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , new Guid(Request.QueryString["subcategoryid"].ToString())
                                                                         , int.Parse(txtSortNo.Text)
                                                                         , txtCodeAdd.Text
                                                                         , txtSubCategoryAdd.Text);
                }
                else
                {
                    PhoenixRegisterFMSDrawing.FMSDrawingCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , null
                                                                        , int.Parse(txtSortNo.Text)
                                                                        , txtCodeAdd.Text
                                                                        , txtSubCategoryAdd.Text);
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
    private bool IsValidFMSManualSubCategory(string sortorder, string shortcode, string subcategory)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (sortorder.Trim().Equals(""))
            ucError.ErrorMessage = "Sort Order is required.";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Drawing Code is required.";

        if (subcategory.Trim().Equals(""))
            ucError.ErrorMessage = "Drawing Name is required.";

        return (!ucError.IsError);
    }

    private void BindCategory()
    {
        DataSet dt;
        if (ViewState["subcategoryid"] != null && ViewState["subcategoryid"].ToString() != string.Empty)
        {
            dt = PhoenixRegisterFMSDrawing.FMSDrawingCategoryEdit(Guid.Parse(ViewState["subcategoryid"].ToString()));

            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtCodeAdd.Text = dr["FLDDRAWINGCODE"].ToString();
                txtSubCategoryAdd.Text = dr["FLDDRAWINGCATEGORY"].ToString();
                txtSortNo.Text = dr["FLDORDER"].ToString();
            }
        }
    }
    
    protected void BindData()
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindData();
    }

}