using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI;

public partial class Registers_RegisterTrainingScenarioAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripscenarioaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            

            DataTable dt = PhoenixRegisterTrainingScenario.TrainingList();

            radcombotraining.DataSource = dt;
            radcombotraining.DataTextField = "FLDTRAININGNAME";
            radcombotraining.DataValueField = "FLDTRAININGID";
            radcombotraining.DataBind();

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
                Guid? TrainingId = General.GetNullableGuid(radcombotraining.SelectedValue);

                if (!IsValidScenarioDetails(scenario, TrainingId))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterTrainingScenario.TrainingScenarioInsert(rowusercode,
                                                scenario,
                                                scenariodescription,
                                               TrainingId);

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

    public bool IsValidScenarioDetails(string scenario, Guid? TrainingId)
    {

        ucError.HeaderMessage = "Provide the following required information";
        if (TrainingId == null)
        {
            ucError.ErrorMessage = "Training name.";
        }

        if (scenario == null)
        {
            ucError.ErrorMessage = "Scenario .";

        }

        return (!ucError.IsError);
    }

}