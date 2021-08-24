using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI;

public partial class Registers_RegisterTrainingScenarioEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripscenarioaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
           

            ViewState["TRAININGSCENARIOID"] = General.GetNullableGuid(Request.QueryString["TrainingScenarioId"]);
            Guid? Trainingscenarioid = General.GetNullableGuid(ViewState["TRAININGSCENARIOID"].ToString());

            DataTable dt1 = PhoenixRegisterTrainingScenario.TrainingScenarioEditList(Trainingscenarioid);
            if (dt1.Rows.Count > 0)
            {
                radcombotraining.SelectedValue = dt1.Rows[0]["FLDTRAININGID"].ToString();
                Radtxtscenario.Text = dt1.Rows[0]["FLDSCENARIO"].ToString();
                if (dt1.Rows[0]["FLDDESCRIPTION"] != null)
                {
                    radtbdescription.Text = dt1.Rows[0]["FLDDESCRIPTION"].ToString();
                }
            }

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
            if (CommandName.ToUpper().Equals("UPDATE"))
            {

                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string scenario = General.GetNullableString(Radtxtscenario.Text);
                string scenariodescription = General.GetNullableString(radtbdescription.Text);
                Guid? TrainingId = General.GetNullableGuid(radcombotraining.SelectedValue);
                Guid? Trainingscenarioid = General.GetNullableGuid(ViewState["TRAININGSCENARIOID"].ToString());

                if (!IsValidScenarioDetails(scenario, TrainingId))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterTrainingScenario.TrainingScenarioUpdate(rowusercode,
                                                     Trainingscenarioid,
                                                     scenario
                                                     , scenariodescription
                                                     , TrainingId);

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