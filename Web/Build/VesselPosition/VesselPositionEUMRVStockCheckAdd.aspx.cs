using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVStockCheckAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuStockCheck.AccessRights = this.ViewState;
            MenuStockCheck.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStockCheck_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string ReportTime = txtReportTime.SelectedTime != null ? txtReportTime.SelectedTime.Value.ToString() : "";

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? tanksoundinglogid = null;
                PhoenixVesselPositionEUMRVStockCheck.InsertStockcheck(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                General.GetNullableDateTime(txtReportDate.Text + " " + ReportTime),
                General.GetNullableString(ddlLocation.SelectedValue),
                General.GetNullableString(txtOccasion.Text),
                General.GetNullableDecimal(txtDraftf.Text),
                General.GetNullableDecimal(txtDrafta.Text),
                General.GetNullableDecimal(txtList.Text),
                General.GetNullableString(ddlPS.SelectedValue),
                ref tanksoundinglogid
                );

                String script = "javascript:fnReloadList('codehelp1','true');";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
