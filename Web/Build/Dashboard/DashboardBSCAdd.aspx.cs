using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Dashboard_DashboardBSCAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["STRATEGYID"] = General.GetNullableGuid(Request.QueryString["strategyid"]);
            if (ViewState["STRATEGYID"] != null)
            {
                Guid? BSSMID = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());

                DataTable dt = PhoenixDashboardBSSP.BSSMEditList(BSSMID);
                if (dt.Rows.Count > 0)
                {
                    Radsmnameentry.Text = dt.Rows[0]["FLDBSSMNAME"].ToString();
                    Radsmvisionentry.Text = dt.Rows[0]["FLDVISION"].ToString();

                }
            }

        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        if (ViewState["STRATEGYID"] != null)
        { toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right); }
        else
        { toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right); }

        Tabstripsmaddmenu.MenuList = toolbargrid.Show();

    }

    protected void Tabstripsmaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string Name = General.GetNullableString(Radsmnameentry.Text);
                string Vision = General.GetNullableString(Radsmvisionentry.Text);

                Guid? BSSMID = General.GetNullableGuid(ViewState["STRATEGYID"].ToString());
                if (!IsValidSMDetails(Name, Vision))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSSP.BSSMUpdate(rowusercode, Name, Vision, BSSMID);


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }



            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string Name = General.GetNullableString(Radsmnameentry.Text);
                string Vision = General.GetNullableString(Radsmvisionentry.Text);
                if (!IsValidSMDetails(Name, Vision))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSSP.BSSMInsert(rowusercode, Name, Vision);


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

    private bool IsValidSMDetails(string Name, string Vision )
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (Name == null)
        {
            ucError.ErrorMessage = "Strategy Map Name.";
        }
        if (Vision == null)
        {
            ucError.ErrorMessage = "Strategy Map Vision.";
        }
      

        return (!ucError.IsError);
    }

}