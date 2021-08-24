using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationClosedVesselUpdate : PhoenixBasePage
{
    Guid? RuleAppliedVesselId;

    protected void Page_Load(object sender, EventArgs e)
    {
        RuleAppliedVesselId = new Guid(Request.QueryString["RuleAppliedVesselId"]);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.AccessRights = this.ViewState;
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                int status = Convert.ToInt32(chkStatus.SelectedValue);
                PhoenixInspectionNewRegulation.RegulationAppliedVesselApprove(RuleAppliedVesselId.Value, usercode, status);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearUserInput();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearUserInput()
    {
        chkStatus.SelectedIndex = 0;
    }

}