using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;


public partial class OptionsModule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("DB Measure", "MEASURE", ToolBarDirection.Right);
        toolbar.AddButton("DB Module", "MODULE", ToolBarDirection.Right);
        toolbar.AddButton("Access", "FUNCTION", ToolBarDirection.Right);
        toolbar.AddButton("Menus", "MENU", ToolBarDirection.Right);
        toolbar.AddButton("Application", "APPLICATION", ToolBarDirection.Right);
        toolbar.AddButton("User Groups", "USERGROUP", ToolBarDirection.Right);

        MenuOptionmodule.MenuList = toolbar.Show();
        MenuOptionmodule.SelectedMenuIndex = 1;

      
    }

   
    protected void Rebind()
    {
        gvModule.SelectedIndexes.Clear();
        gvModule.EditIndexes.Clear();
        gvModule.DataSource = null;
        gvModule.Rebind();
    }

    protected void gvModule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
     
        if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
        {
            DataTable ds = PhoenixDashboardOption.UserGroupDashboardModuleRightslist(
                 PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               Int32.Parse(ucUserGroup.SelectedUserGroup));

            gvModule.DataSource = ds;
        }
    }


    protected void MenuOptionmodule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.Equals("SAVE"))
            {

            }
            else if (CommandName.Equals("USERGROUP"))
                Response.Redirect("OptionsUserGroups.aspx");
            else if (CommandName.Equals("APPLICATION"))
                Response.Redirect("OptionsApplication.aspx");
            else if (CommandName.Equals("MENU"))
                Response.Redirect("OptionsMenuAccess.aspx");
            else if (CommandName.Equals("FUNCTION"))
                Response.Redirect("OptionsFunctionAccess.aspx");
            else if (CommandName.Equals("MODULE"))
                Response.Redirect("OptionsModule.aspx");
            else if (CommandName.Equals("MEASURE"))
                Response.Redirect("OptionsMeasure.aspx");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }




    protected void gvModule_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string ModuleId = (((RadLabel)e.Item.FindControl("lblModuleId")).Text);
                bool ModuleRights = ((RadCheckBox)e.Item.FindControl("chkModuleRights")).Checked == true ? true : false;

                PhoenixDashboardOption.UserGroupDashboardModuleRightsInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     int.Parse(ucUserGroup.SelectedUserGroup),
                     int.Parse(ModuleId),
                     ModuleRights
                    );

                PhoenixDashboardOption.UserGroupDashboardModuleRightsUpdate(
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     int.Parse(ucUserGroup.SelectedUserGroup)
                    );

                Rebind();

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvModule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkModuleRights");
                cb.Enabled = SessionUtil.CanAccess(this.ViewState, "UPDATE");
                cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDRIGHTS").ToString().Equals("1") ? true : false;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }




    protected void ucUserGroup_TextChangedEvent(object sender, EventArgs e)
    {
       
        if (General.GetNullableString(ucUserGroup.SelectedUserGroup) != null)
        {        
            gvModule.Rebind();
        }
    }
}