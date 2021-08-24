using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class PlannedMaintenanceGlobalRoutineWorkorderAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkOrderAdd.MenuList = toolbarmain.Show();
    }

    protected void MenuWorkOrderAdd_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidWo())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceGlobalRoutineWorkorder.GlobalRoutineWorkorderinsert(Txttitle.Text.Trim(), int.Parse(UcDiscipline.SelectedDiscipline));

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                              "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidWo()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((General.GetNullableString(Txttitle.Text)) == null)
            ucError.ErrorMessage = "Title is required.";

        if ((General.GetNullableInteger(UcDiscipline.SelectedDiscipline)) == null)
            ucError.ErrorMessage = "Responsibility is required.";

        return (!ucError.IsError);
    }

}