using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Inspection_InspectionShippingPIEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
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

            ViewState["SHIPPINGPIID"] = General.GetNullableGuid(Request.QueryString["shippingpiid"]);
            Guid? shippingpiid = General.GetNullableGuid(ViewState["SHIPPINGPIID"].ToString());

            DataTable dt2 = PheonixDashboardSKPI.PIEditList(shippingpiid);
            if (dt2.Rows.Count > 0)
            {
                Radpiidentry.Text = dt2.Rows[0]["FLDPICODE"].ToString();
                pinameentry.Text = dt2.Rows[0]["FLDPINAME"].ToString();
                radcbpiunitentry.SelectedValue = dt2.Rows[0]["FLDPIUNIT"].ToString();
                radcbpiscopeentry.SelectedValue = dt2.Rows[0]["FLDSCOPE"].ToString();
                Radcbpiperiodentry.SelectedHard = dt2.Rows[0]["FLDPERIOD"].ToString();
                if (dt2.Rows[0]["FLDDESCRIPTION"] != null)
                {
                    RadtbPIdescriptionentry.Text = dt2.Rows[0]["FLDDESCRIPTION"].ToString();
                }

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
                string picode = General.GetNullableString(Radpiidentry.Text);
                string piname = General.GetNullableString(pinameentry.Text);
                Guid? shippingpiid = General.GetNullableGuid(ViewState["SHIPPINGPIID"].ToString());

                int? piunit = General.GetNullableInteger(radcbpiunitentry.SelectedValue);
                int? piscope = General.GetNullableInteger(radcbpiscopeentry.SelectedValue);
                int? piperiod = General.GetNullableInteger(Radcbpiperiodentry.SelectedHard);
                string description = General.GetNullableString(RadtbPIdescriptionentry.Text);
                if (!IsValidShippingPIDetails(picode, piname, piunit, piscope, piperiod, description))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKPI.PIUpdate(rowusercode, picode, piname, piunit, piscope, piperiod, description, shippingpiid);


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
    private bool IsValidShippingPIDetails(string picode, string piname, int? piunit, int? piscope, int? piperiod, string description)
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