using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
public partial class PlannedMaintenanceClassificationSocietyRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Class Codes", "CODES", ToolBarDirection.Left);
        toolbarmain.AddButton("Classification Society", "SOCIETY", ToolBarDirection.Left);

        MenuPlannedMaintenance.AccessRights = this.ViewState;
        MenuPlannedMaintenance.MenuList = toolbarmain.Show();
        MenuPlannedMaintenance.SelectedMenuIndex = 1;
    }

    protected void gvClassMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.ClassificationSocietyList(null);
        gvClassMap.DataSource = ds;
    }

    protected void gvClassMap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.ClassificationSocietyDelete(int.Parse(item.GetDataKeyValue("FLDADDRESSCODE").ToString()));
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (General.GetNullableInteger(((UserControlMultiColumnAddress)item.FindControl("ucAddress")).SelectedValue) == null)
                {
                    ucError.ErrorMessage = "Please select the Classification Society.";
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceGlobalComponent.ClassificationSocietyInsert(int.Parse(((UserControlMultiColumnAddress)item.FindControl("ucAddress")).SelectedValue)
                            , ((UserControlMultiColumnAddress)item.FindControl("ucAddress")).Text);

                gvClassMap.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPlannedMaintenance_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("CODES"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceGlobalComponentClassMap.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}