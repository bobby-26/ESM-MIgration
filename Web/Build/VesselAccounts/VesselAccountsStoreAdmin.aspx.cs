using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;


public partial class VesselAccountsStoreAdmin :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);  ResetMenu();
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
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Store Item Opening", "OPENING");
            toolbar.AddButton("Store Disposition", "DISPOSITION");
            toolbar.AddButton("Purchase Order UnConfirm", "UCPROVISIONANDBOND");
            toolbar.AddButton("Issue of Bonded Stores", "BONDEDSTOREISSUE");
            toolbar.AddButton("Round Off", "ROUNDOFF");
            toolbar.AddButton("Rob Initialize", "ROB");
            MenuStoreAdmin.AccessRights = this.ViewState;
            MenuStoreAdmin.MenuList = toolbar.Show();
            MenuStoreAdmin.SelectedMenuIndex = 0;

            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsStoreItemOpening.aspx";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStoreAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("OPENING"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreItemOpening.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("DISPOSITION"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreDisposition.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("UCPROVISIONANDBOND"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOrderFormUnConfirm.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("BONDEDSTOREISSUE"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminBondedStoreIssue.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("ROUNDOFF"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsRoundOffUpdate.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("ROB"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOpeningRobExcelUpload.aspx";
            }
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
