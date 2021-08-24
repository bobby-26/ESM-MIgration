using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class AccountsCreateDefaultAllotment : PhoenixBasePage
{
    string header = "Please provide the following required information", error = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Create Default Allotment", "CREATEDEFAULTALLOTMENT", ToolBarDirection.Right);
        MenuDefaultAllotment.AccessRights = this.ViewState;
        MenuDefaultAllotment.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            //for (int i = DateTime.Now.Year; i >= 2005; i--)
            //{
            //    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
            //}

            //ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            //ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

        }

    }

    protected void ddlvessel_textchangedevent(object sender, System.EventArgs e)
    {

    }

    protected void MenuDefaultAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("CREATEDEFAULTALLOTMENT"))
        {
            if (IsValidDefaultAllotment(ref header, ref error))
            {
                RadWindowManager1.RadAlert(error, 400, 175, header, null);
                //ucError.Visible = true;
                return;
            }
            try
            {

                PhoenixAccountsAllotment.DefaultAllotmentValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ddlVessel.SelectedVessel), int.Parse(ddlMonth.SelectedMonth), int.Parse(ddlyear.SelectedYearinstr));
            }

            catch (Exception ex)
            {

                ucConfirm.Visible = true;
                RadWindowManager1.RadConfirm(ex.Message, "ucConfirm", 500, 250, null, "Confirm");
                return;

                //ucError.ErrorMessage = ex.Message;
                //ucError.Visible = true;
            }
            //DefaultAllotmentGenerate(
            //   int.Parse(ddlVessel.SelectedVessel),
            //   int.Parse(ddlMonth.SelectedMonth),
            //   int.Parse(ddlyear.SelectedYearinstr));
        }
    }

    private void DefaultAllotmentGenerate(int VesselId, int Month, int Year)
    {

        PhoenixAccountsAllotment.DefaultAllotmentGenerate(VesselId, Month, Year);

    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            DefaultAllotmentGenerate(
                int.Parse(ddlVessel.SelectedVessel),
                int.Parse(ddlMonth.SelectedMonth),
                int.Parse(ddlyear.SelectedYearinstr));
        }
        catch (Exception ex)
        {
            //ucError.ErrorMessage = ex.Message;
            //ucError.Visible = true;
            RadWindowManager1.RadAlert(ex.Message, 400, 175, header, null);
            //ucError.Visible = true;
            return;
        }
    }

    private bool IsValidDefaultAllotment(ref string headermessage, ref string errormessage)
    {
        //ucError.HeaderMessage = "Please provide the following required information";
        errormessage = "";
        //ucError.HeaderMessage = "Please provide the following required information";

        if ((General.GetNullableInteger(ddlVessel.SelectedVessel) == null))
            errormessage = errormessage + "Vessel is required.</br>";

        if ((General.GetNullableInteger(ddlMonth.SelectedMonth) == null))
           errormessage = errormessage + "Month is required.</br>";

        if ((General.GetNullableInteger(ddlyear.SelectedYearinstr) == null))
           errormessage = errormessage + "Year is required.</br>";

        if (!errormessage.Length.Equals(0))
        {
            return true;
        }
        else
            return false;

    }
}