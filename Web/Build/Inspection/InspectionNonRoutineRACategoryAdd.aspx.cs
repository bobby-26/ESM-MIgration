using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class InspectionNonRoutineRACategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuNonRoutineRACategoryAdd.AccessRights = this.ViewState;
            MenuNonRoutineRACategoryAdd.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                ViewState["CATEGORYID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != string.Empty)
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                cbActive.Checked = true;
                BindCatagory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuNonRoutineRACategoryAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() != string.Empty)
                    UpdateCategory();
                else
                    InsertCategory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void InsertCategory()
    {
        if (!IsValidNonROutineRACategory())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixInspectionNonRoutineRACategory.InsertNonRoutineRACategory(General.GetNullableString(txtShortcode.Text.Trim()), General.GetNullableString(txtName.Text.Trim()),cbActive.Checked.Equals(true) ? 1 : 0);

        ucStatus.Text = "Information updated.";

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateCategory()
    {
        if (!IsValidNonROutineRACategory())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixInspectionNonRoutineRACategory.UpdateNonRoutineRACategory(int.Parse(ViewState["CATEGORYID"].ToString()),
                 General.GetNullableString(txtShortcode.Text.Trim()), General.GetNullableString(txtName.Text.Trim()), cbActive.Checked.Equals(true) ? 1 : 0);

        ucStatus.Text = "Information updated.";

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
    public bool IsValidNonROutineRACategory()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtShortcode.Text) == null)
            ucError.ErrorMessage = "Short Code is required";

        if (General.GetNullableString(txtName.Text) == null)
            ucError.ErrorMessage = "Name is required";

        return (!ucError.IsError);

    }
    protected void BindCatagory()
    {
        if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixInspectionNonRoutineRACategory.EditNonRoutineRACategory(int.Parse(ViewState["CATEGORYID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtShortcode.Text = dr["FLDSHORTCODE"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                cbActive.Checked = dr["FLDACTIVEYN"].ToString().Equals("1") ? true : false;
            }
        }
    }
}