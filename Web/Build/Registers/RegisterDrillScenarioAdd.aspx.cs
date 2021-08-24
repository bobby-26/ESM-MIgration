using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI;


public partial class Registers_RegisterDrillScenarioAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripdrillscenarioaddmenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           

            DataTable dt = PhoenixRegisterDrillScenario.drillcheckboxlist();

            radcombodrill.DataSource = dt;
            radcombodrill.DataTextField = "FLDDRILLNAME";
            radcombodrill.DataValueField = "FLDDRILLID";
            radcombodrill.DataBind();

        }
    }
    protected void drillscenarioaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string scenario = General.GetNullableString(Radtxtscenario.Text);
                string scenariodescription = General.GetNullableString(radtbdescription.Text);
                Guid? drillid = General.GetNullableGuid(radcombodrill.SelectedValue);

                if (!IsValidScenarioDetails(scenario, drillid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterDrillScenario.scenarioinsert(rowusercode,
                                                scenario,
                                                scenariodescription,
                                               drillid);

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

    public bool IsValidScenarioDetails(string scenario, Guid? drillid)
    {

        ucError.HeaderMessage = "Provide the following required information";
        if (drillid == null)
        {
            ucError.ErrorMessage = "Drill name.";
        }

        if (scenario == null)
        {
            ucError.ErrorMessage = "Scenario .";

        }

        return (!ucError.IsError);
    }

}