using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Inspection_InspectionShippingKPIScopeEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        {
            ViewState["KPISCOPEID"] = General.GetNullableInteger(Request.QueryString["shippingkpiscopeid"]);
            int? kpiscopeid = General.GetNullableInteger(ViewState["KPISCOPEID"].ToString());

            DataTable dt = PheonixDashboardSKKPI.KPIScopeEditList(kpiscopeid);
            if (dt.Rows.Count > 0)
            {
                Radpiscopecodeentry.Text = dt.Rows[0]["FLDSCOPECODE"].ToString();
                Radpiscopenameentry.Text = dt.Rows[0]["FLDSCOPE"].ToString();

            }
        }
    }
    protected void piaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string scopecode = General.GetNullableString(Radpiscopecodeentry.Text);
                string scope = General.GetNullableString(Radpiscopenameentry.Text);
                int? kpiscopeid = General.GetNullableInteger(ViewState["KPISCOPEID"].ToString());

                if (!IsValidShippingKPIscopeDetails(scopecode, scope))
                {
                    ucError.Visible = true;
                    return;
                }
                PheonixDashboardSKKPI.KPIScopeUpdate(rowusercode, scopecode, scope, kpiscopeid);

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
    private bool IsValidShippingKPIscopeDetails(string scopecode, string scope)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (scopecode == null)
        {
            ucError.ErrorMessage = "Scope Code.";
        }
        if (scope == null)
        {
            ucError.ErrorMessage = "Scope Name.";
        }

        return (!ucError.IsError);
    }
}