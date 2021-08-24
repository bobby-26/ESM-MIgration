using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Inspection_InspectionShippingKPIEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!Page.IsPostBack)
        {
            ViewState["KPIID"] = General.GetNullableGuid(Request.QueryString["shippingkpiid"]);
            Guid? shippingKpiid = General.GetNullableGuid(ViewState["KPIID"].ToString());


            DataTable dt3 = PheonixDashboardSKKPI.KPIUnitList();

            radcbkpiunitentry.DataSource = dt3;
            radcbkpiunitentry.DataTextField = "FLDUNIT";
            radcbkpiunitentry.DataValueField = "FLDUNITID";
            radcbkpiunitentry.DataBind();

            DataTable dt1 = PheonixDashboardSKKPI.KPIScopeList();

            radcbkpiscopeentry.DataSource = dt1;
            radcbkpiscopeentry.DataTextField = "FLDSCOPE";
            radcbkpiscopeentry.DataValueField = "FLDSCOPEID";
            radcbkpiscopeentry.DataBind();

            DataTable dt2 = PheonixDashboardSKKPI.Departmentlist();

            radcbdept.DataSource = dt2;
            radcbdept.DataTextField = "FLDDEPARTMENTNAME";
            radcbdept.DataValueField = "FLDDEPARTMENTID";
            radcbdept.DataBind();

            DataTable dt = PheonixDashboardSKKPI.KPIEditList(shippingKpiid);
            if (dt.Rows.Count > 0)
            {
                Radtbkpicodeentry.Text = dt.Rows[0]["FLDKPICODE"].ToString();
                Radtbkpinameentry.Text = dt.Rows[0]["FLDKPINAME"].ToString();
                radcbdept.SelectedValue = dt.Rows[0]["FLDDEPARTMENT"].ToString();
                RadRadioButtonkpilevel.SelectedValue = dt.Rows[0]["FLDLEVEL"].ToString();
                radcbkpiunitentry.SelectedValue = dt.Rows[0]["FLDKPIUNIT"].ToString();
                radcbkpiscopeentry.SelectedValue = dt.Rows[0]["FLDKPISCOPE"].ToString();
                Radcbkpiperiodentry.SelectedHard = dt.Rows[0]["FLDKPIPERIOD"].ToString();
                RadtbkPIdescriptionentry.Text = dt.Rows[0]["FLDKPIDESCRIPTION"].ToString();
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
                string Kpicode = General.GetNullableString(Radtbkpicodeentry.Text);
                string Kpiname = General.GetNullableString(Radtbkpinameentry.Text);
                int? kpiunit = General.GetNullableInteger(radcbkpiunitentry.SelectedValue);
                int? kpiscope = General.GetNullableInteger(radcbkpiscopeentry.SelectedValue);
                int? kpiperiod = General.GetNullableInteger(Radcbkpiperiodentry.SelectedHard);
                string description = General.GetNullableString(RadtbkPIdescriptionentry.Text);
                string level = General.GetNullableString(RadRadioButtonkpilevel.SelectedValue);
                int? department = General.GetNullableInteger(radcbdept.SelectedValue);
                Guid? KPIid = General.GetNullableGuid(ViewState["KPIID"].ToString());
                if (!IsValidShippingKPIDetails(Kpicode, Kpiname, kpiunit, kpiscope, kpiperiod))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.KPIUpdate(rowusercode, Kpicode, Kpiname, kpiunit, kpiscope, kpiperiod, description, department, level,  KPIid);


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
    private bool IsValidShippingKPIDetails(string kpicode, string Kpiname, int? kpiunit, int? kpiscope, int? kpiperiod)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (kpicode == null)
        {
            ucError.ErrorMessage = "KPI Code.";
        }
        if (Kpiname == null)
        {
            ucError.ErrorMessage = "KPI Name.";
        }
        if (kpiunit == null)
        {
            ucError.ErrorMessage = "KPI Unit .";
        }
        if (kpiscope == null)
        {
            ucError.ErrorMessage = "KPI Scope .";
        }
        if (kpiperiod == null)
        {
            ucError.ErrorMessage = "KPI Period .";
        }

        return (!ucError.IsError);
    }


}