using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsAdministration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState); ResetMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Provision Correction", "PROVISION");
            toolbar.AddButton("Crew", "CREWADMIN");
            toolbar.AddButton("Transaction", "LOG");
            toolbar.AddButton("Trash Vessel Data", "TRASH");
            toolbar.AddButton("Stores", "STOREADMIN");
          //  toolbar.AddButton("Phone Card", "PC");
            toolbar.AddButton("Earning/Deduction", "EARNINGDEDUCTION");
            toolbar.AddButton("CTM", "CTM");
            toolbar.AddButton("Portage Bill Correction", "PBCORRECTION");
            MenuAdministration.AccessRights = this.ViewState;
            MenuAdministration.MenuList = toolbar.Show();
            MenuAdministration.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsProvisionNegativeCorrection.aspx";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Administration_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("PROVISION"))
            {
                MenuAdministration.SelectedMenuIndex = 0;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsProvisionNegativeCorrection.aspx";
            }
            else if (CommandName.ToUpper().Equals("CREWADMIN"))
            {
                MenuAdministration.SelectedMenuIndex = 1;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminSignOnOffList.aspx";
            }
            else if (CommandName.ToUpper().Equals("LOG"))
            {
                MenuAdministration.SelectedMenuIndex = 2;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsRemoveTransactionlog.aspx";
            }
            else if (CommandName.ToUpper().Equals("TRASH"))
            {
                MenuAdministration.SelectedMenuIndex = 3;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsTrashVesselDatas.aspx";
            }
            else if (CommandName.ToUpper().Equals("STOREADMIN"))
            {
                MenuAdministration.SelectedMenuIndex = 4;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreItemOpening.aspx";
            }
            //else if (CommandName.ToUpper().Equals("PC"))
            //{
            //    MenuAdministration.SelectedMenuIndex =5;
            //    ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsPhoneCardAdmin.aspx";
            //}
            else if (CommandName.ToUpper().Equals("EARNINGDEDUCTION"))
            {
                MenuAdministration.SelectedMenuIndex = 5;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminEarningDeduction.aspx";
            }
            else if (CommandName.ToUpper().Equals("CTM"))
            {
                MenuAdministration.SelectedMenuIndex = 6;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminCTM.aspx";
            }
            else if (CommandName.ToUpper().Equals("PBCORRECTION"))
            {
                MenuAdministration.SelectedMenuIndex =7;
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsEmployeePortageBillUpdate.aspx";
            }
            ////else if (CommandName.ToUpper().Equals("TABLES"))
            ////{
            ////    ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsTableRecordsSearch.aspx";
            ////}
            ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}
