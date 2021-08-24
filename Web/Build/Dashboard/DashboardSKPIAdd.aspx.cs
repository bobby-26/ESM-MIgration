using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Inspection_InspectionShippingPIAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!Page.IsPostBack)
        { 
        DataTable dt = PheonixDashboardSKPI.PIUnitList();

        radcbpiunitentry.DataSource = dt;
        radcbpiunitentry.DataTextField = "FLDUNIT";
        radcbpiunitentry.DataValueField = "FLDUNITID";
        radcbpiunitentry.DataBind();

        DataTable dt1 = PheonixDashboardSKPI.PIScopeList();

        radcbpiscopeentry.DataSource = dt1;
        radcbpiscopeentry.DataTextField = "FLDSCOPE";
        radcbpiscopeentry.DataValueField = "FLDSCOPEID";
        radcbpiscopeentry.DataBind();
        }

    }

    protected void piaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string picode = General.GetNullableString(Radpiidentry.Text);
                string piname = General.GetNullableString(pinameentry.Text);

                int? piunit = General.GetNullableInteger(radcbpiunitentry.SelectedValue);
                int? piscope = General.GetNullableInteger(radcbpiscopeentry.SelectedValue);
                int? piperiod = General.GetNullableInteger(Radcbpiperiodentry.SelectedHard);
                string description = General.GetNullableString(RadtbPIdescriptionentry.Text);
                if (!IsValidShippingPIDetails(picode, piname, piunit, piscope, piperiod))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKPI.PIInsert(rowusercode, picode, piname, piunit, piscope, piperiod, description);


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
    private bool IsValidShippingPIDetails(string picode, string piname, int? piunit, int? piscope, int? piperiod)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (picode == null)
        {
            ucError.ErrorMessage = "PI ID.";
        }
        if (piname == null)
        {
            ucError.ErrorMessage = "PI Name.";
        }
        if (piunit == null)
        {
            ucError.ErrorMessage = "PI Unit.";
        }
        if (piscope == null)
        {
            ucError.ErrorMessage = "PI Scope.";
        }
        if (piperiod == null)
        {
            ucError.ErrorMessage = "PI Period.";
        }

        return (!ucError.IsError);
    }
}