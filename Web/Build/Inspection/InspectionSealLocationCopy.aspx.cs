using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionSealLocationCopy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ucConfirm.Visible = false;
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Copy", "COPY", ToolBarDirection.Right);
                MenuCopy.MenuList = toolbar.Show();
               // MenuCopy.SetTrigger(pnlSealLocationCopy);
                txtSource.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuCopy_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (!IsValidCopy())
                {
                    ucError.Visible = true;
                    return;
                }
                int result = 0;
                PhoenixInspectionSealLocation.CheckIfSealLocationExists(int.Parse(ucVessel.SelectedVessel), ref result);
                if (result == 1)
                {
                    ucConfirm.Visible = true;
                    ucConfirm.Text = "Destination vessel has existing Seal locations. Pls confirm if you wish to overwrite the Seal locations";
                    return;
                }
                else
                {
                    PhoenixInspectionSealLocation.SealLocationCopyWithOverwrite(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ucVessel.SelectedVessel));
                    ucStatus.Text = "Seal Locations copied successfully.";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCopy()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if(General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Destination Vessel is required.";

        return (!ucError.IsError);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                PhoenixInspectionSealLocation.SealLocationCopyWithOverwrite(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ucVessel.SelectedVessel));
                ucStatus.Text = "Seal Locations copied successfully.";
            }
            else
            {
                //PhoenixInspectionSealLocation.SealLocationCopyWithoutOverwrite(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ucVessel.SelectedVessel));
                //ucStatus.Text = "Seal Locations copied successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
