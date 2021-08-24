using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AdministrationITSupportAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        PhoenixToolbar toolbarbugadd = new PhoenixToolbar();
        toolbarbugadd.AddButton("Save", "SAVE",ToolBarDirection.Right);

        MenuSupportAdd.AccessRights = this.ViewState;
        MenuSupportAdd.MenuList = toolbarbugadd.Show();

    }



    protected void MenuSupportAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (!IsValidBug())
            {
                ucError.Visible = true;
                return;
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                Guid dtkey = Guid.Empty;

                PhoenixAdministrationITSupport.InsertITSupport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , txtLoggedBy.Text
                , txtSystemName.Text
                , Convert.ToInt16(ddlDepartmentList.SelectedValue)
                , Convert.ToInt16(ddlCategoryType.SelectedValue)
                , txtCallType.Text
                , 1
                , txtRemarks.Text
                , ref dtkey);

                ucStatus.Text = "Issue saved";
                String script = "javascript:fnAdminITSupportEdit(); javascript:fnReloadList('code1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        ddlDepartmentList.SelectedValue = -1;
        txtSystemName.Text = "";
        txtCallType.Text = "";
        ddlCategoryType.SelectedValue = -1;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {

    }

    private bool IsValidBug()
    {
        if (General.GetNullableString(ddlDepartmentList.SelectedDepartment) == null)
            ucError.ErrorMessage = "Department is required";
        if (General.GetNullableString(ddlCategoryType.SelectedCategory) == null)
            ucError.ErrorMessage = "Category is required";

        if (General.GetNullableString(txtCallType.Text) == null)
            ucError.ErrorMessage = "Call Type is required";

        return !ucError.IsError;
    }

}
