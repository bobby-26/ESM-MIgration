using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI;


public partial class Registers_RegisterDrillScenarioEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripdrillscenarioaddmenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
          

            ViewState["DRILLSCENARIOID"] = General.GetNullableGuid(Request.QueryString["drillscenarioid"]);
            Guid? drillscenarioid = General.GetNullableGuid(ViewState["DRILLSCENARIOID"].ToString());

            DataTable dt1 = PhoenixRegisterDrillScenario.scenarioeditlist(drillscenarioid);
            if (dt1.Rows.Count > 0)
            {
                radcombodrill.SelectedValue = dt1.Rows[0]["FLDDRILLID"].ToString();
                Radtxtscenario.Text = dt1.Rows[0]["FLDSCENARIO"].ToString();
                if (dt1.Rows[0]["FLDDESCRIPTION"] != null)
                {
                    radtbdescription.Text = dt1.Rows[0]["FLDDESCRIPTION"].ToString();
                }
            }

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
            if (CommandName.ToUpper().Equals("UPDATE"))
            {

                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string scenario = General.GetNullableString(Radtxtscenario.Text);
                string scenariodescription = General.GetNullableString(radtbdescription.Text);
                Guid? drillid = General.GetNullableGuid(radcombodrill.SelectedValue);
                Guid? drillscenarioid = General.GetNullableGuid(ViewState["DRILLSCENARIOID"].ToString());

                if (!IsValidScenarioDetails(scenario, drillid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterDrillScenario.scenarioupdate(rowusercode,
                                                     drillscenarioid,
                                                     scenario
                                                     , scenariodescription
                                                     , drillid);

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
