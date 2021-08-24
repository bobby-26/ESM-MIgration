using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class OwnerReportUserGroupSectionRights : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Measure", "MEASURE", ToolBarDirection.Right);
        toolbar.AddButton("Section", "SECTION", ToolBarDirection.Right);
                
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
            DataTable ds = PhoenixOwnerReport.UserGroupDashboardSectionRightslist(
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

            if (CommandName.Equals("MEASURE"))
                Response.Redirect("OwnerReportUserGroupMeasureRights.aspx");
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
                string sectionid = (((RadLabel)e.Item.FindControl("lblModuleId")).Text);
                bool ModuleRights = ((RadCheckBox)e.Item.FindControl("chkModuleRights")).Checked == true ? true : false;

                PhoenixOwnerReport.UserGroupdSectionRightsInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     int.Parse(ucUserGroup.SelectedUserGroup),
                     new Guid(sectionid),
                     ModuleRights
                    );

                PhoenixOwnerReport.UserSectionRightsUpdate(
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

            if (e.Item is GridGroupHeaderItem)
            {
                GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
                string[] myArr = item.DataCell.Text.Split(':');
                item.DataCell.Text = myArr[1].Trim();
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