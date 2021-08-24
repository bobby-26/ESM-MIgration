using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Inspection_InspectionShippingKPIUnitEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        {
            ViewState["KPIUNITID"] = General.GetNullableInteger(Request.QueryString["shippingkpiunitid"]);
            int? kpiunitid = General.GetNullableInteger(ViewState["KPIUNITID"].ToString());

            DataTable dt = PheonixDashboardSKKPI.KPIUnitEditList(kpiunitid);
            if (dt.Rows.Count > 0)
            {
                Radpiunitcodeentry.Text = dt.Rows[0]["FLDUNITCODE"].ToString();
                Radpiunitnameentry.Text = dt.Rows[0]["FLDUNIT"].ToString();

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
                string unitcode = General.GetNullableString(Radpiunitcodeentry.Text);
                string unit = General.GetNullableString(Radpiunitnameentry.Text);

                if (!IsValidShippingKPIUnitDetails(unitcode, unit))
                {
                    ucError.Visible = true;
                    return;
                }

                int? kpiunitid = General.GetNullableInteger(ViewState["KPIUNITID"].ToString());

                PheonixDashboardSKKPI.KPIUnitUpdate(rowusercode, unitcode, unit, kpiunitid);

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

    private bool IsValidShippingKPIUnitDetails(string unitcode, string unit)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (unitcode == null)
        {
            ucError.ErrorMessage = "Unit Code.";
        }
        if (unit == null)
        {
            ucError.ErrorMessage = "Unit Name.";
        }

        return (!ucError.IsError);
    }
}