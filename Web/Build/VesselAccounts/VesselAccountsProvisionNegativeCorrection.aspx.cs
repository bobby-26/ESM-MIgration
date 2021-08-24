using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;

public partial class VesselAccountsProvisionNegativeCorrection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuFormCorrection.MenuList = toolbarmain.Show();
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuFormCorrection_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidClosingDate(txtClosingDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.ProvisionNegativeCorrection(int.Parse(ddlVessel.SelectedVessel), DateTime.Parse(txtClosingDate.Text));
                ucStatus.Text = "Provision is Corrected for the given date";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidClosingDate(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlVessel.SelectedVessel == "")
        {
            ucError.ErrorMessage = "Please Select a Vessel";
        }
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Closing Date is required.";
        }


        return (!ucError.IsError);
    }
}
