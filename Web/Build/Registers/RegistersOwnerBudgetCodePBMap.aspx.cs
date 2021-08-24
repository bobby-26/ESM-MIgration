using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;


public partial class RegistersOwnerBudgetCodePBMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ResetMenu();
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
            toolbar.AddButton("CBA Contract Wage", "CBA");
            toolbar.AddButton("Standard Wage Components", "SWC");
            toolbar.AddButton("Components Agreed with Crew", "CAWC");
            toolbar.AddButton("Portage Bill Standard Component", "PBSC");
            MenuOwnerBudgetCodeAdmin.AccessRights = this.ViewState;
            MenuOwnerBudgetCodeAdmin.MenuList = toolbar.Show();
            MenuOwnerBudgetCodeAdmin.SelectedMenuIndex = 0;

            ifMoreInfo.Attributes["src"] = "../Registers/RegistersOwnerBudgetCodePBMapContractCBA.aspx";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOwnerBudgetCodeAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("CBA"))
            {
                ViewState["CURRENTTAB"] = "../Registers/RegistersOwnerBudgetCodePBMapContractCBA.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("SWC"))
            {
                ViewState["CURRENTTAB"] = "../Registers/RegistersOwnerBudgetCodePBMapContractESM.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("CAWC"))
            {
                ViewState["CURRENTTAB"] = "../Registers/RegistersOwnerBudgetCodePBMapContractCrew.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("PBSC"))
            {
                ViewState["CURRENTTAB"] = "../Registers/RegistersOwnerBudgetCodePBMapPortageBillStandardComponent.aspx";
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
