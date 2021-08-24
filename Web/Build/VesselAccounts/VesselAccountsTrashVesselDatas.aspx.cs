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

public partial class VesselAccountsTrashVesselDatas :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            toolbarmain.AddButton("Bonded Stores", "BOND");
            toolbarmain.AddButton("Provision", "PROVISION");
            toolbarmain.AddButton("Brought Forward", "BROUGHTFORWARD");
            toolbarmain.AddButton("Earning/Deduction", "ED");
            toolbarmain.AddButton("Phone Card", "PC");
            toolbarmain.AddButton("Temp PB", "PB");
            toolbarmain.AddButton("All", "TRASH");
            MenuFormTrash.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuFormTrash_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("TRASH"))
            {
                ucConfirm.Visible = true;
                ucConfirm.Text = "Are you want to trash [" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel datas..";
            }
            if (dce.CommandName.ToUpper().Equals("BOND"))
            {
                ucConfirmBond.Visible = true;
                ucConfirmBond.Text = "Are you want to trash bonded stores data in [" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel..";
            }
            if (dce.CommandName.ToUpper().Equals("PROVISION"))
            {
                ucConfirmProvision.Visible = true;
                ucConfirmProvision.Text = "Are you want to trash Provision data in [" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel..";
            }
            if (dce.CommandName.ToUpper().Equals("BROUGHTFORWARD"))
            {
                ucConfirmPortagebillBroughtforward.Visible = true;
                ucConfirmPortagebillBroughtforward.Text = "Are you want to trash Portagebill Broughtforward data in [" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel..";
            }
            if (dce.CommandName.ToUpper().Equals("ED"))
            {
                ucConfirmPortagebillEarningDeduction.Visible = true;
                ucConfirmPortagebillEarningDeduction.Text = "Are you want to trash Portagebill Earning Deduction data in [" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel..";
            }
            if (dce.CommandName.ToUpper().Equals("PC"))
            {
                ucConfirmPhoneCard.Visible = true;
                ucConfirmPhoneCard.Text = "Are you want to trash Portagebill Phone Card data in [" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel..";
            }
            if (dce.CommandName.ToUpper().Equals("PB"))
            {
                PhoenixVesselAccountsCorrections.TruncateTempPB();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void Trash_Confirm(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                PhoenixVesselAccountsCorrections.DeleteVesselAccountClear(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ucStatus.Text = "[" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel Data has been deleted..";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Trash_ConfirmBond(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                if (!IsValidDate(txtDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.DeleteVesselAccountBondedStores(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtDate.Text));
                ucStatus.Text = "[" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel Bond Data has been deleted..";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Trash_ConfirmProvision(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                if (!IsValidDate(txtDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.DeleteVesselAccountProvision(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtDate.Text));
                ucStatus.Text = "[" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel Provision Data has been deleted..";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Trash_ConfirmPortagebillBroughtforward(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                if (!IsValidDate(txtDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.DeleteVesselAccountPortagebillBroughtforward(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtDate.Text));
                ucStatus.Text = "[" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel Portagebill Broughtforward Data has been deleted..";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Trash_ConfirmPortagebillEarningDeduction(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                if (!IsValidDate(txtDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.DeleteVesselAccountPortagebillEarningDeduction(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtDate.Text));
                ucStatus.Text = "[" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel Portagebill EarningDeduction Data has been deleted..";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Trash_ConfirmPhoneCard(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage uc = (UserControlConfirmMessage)sender;
            if (uc.confirmboxvalue == 1)
            {
                if (!IsValidDate(txtDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.DeleteVesselAccountPhoneCard(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(txtDate.Text));
                ucStatus.Text = "[" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "] Vessel PhoneCard Data has been deleted..";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDate(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Date is required.";
        }


        return (!ucError.IsError);
    }
}
