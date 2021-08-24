using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;


public partial class Dashboard_DashboardSKKPITargetEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabkpi.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            DataTable dt = PheonixDashboardSKKPI.KPIList();
            radcobkpi.DataSource = dt;
            radcobkpi.DataBind();
            ViewState["KPITARGETID"] = General.GetNullableGuid(Request.QueryString["kpitargetid"]);
            Guid? kpitargetid = General.GetNullableGuid(ViewState["KPITARGETID"].ToString());

            DataTable dt2 = PheonixDashboardSKKPI.ObjectiveOwnerList();
            Radcombodesignationlist.DataSource = dt2;
            Radcombodesignationlist.DataBind();

            DataTable dt1 = PhoenixDashboardSKKPITarget.KPITargetEditList(kpitargetid);

            if (dt1.Rows.Count > 0)
            {
                radcbyear.SelectedYear = int.Parse((dt1.Rows[0]["FLDYEAR"].ToString()));
                radcobkpi.Value = General.GetNullableString(dt1.Rows[0]["FLDKPIID"].ToString());
                radtbkpiminimumentry.Text = General.GetNullableString(dt1.Rows[0]["FLDKPIMINVALUE"].ToString());
                radtbkpitargetvalueentry.Text = General.GetNullableString(dt1.Rows[0]["FLDKPITARGETVALUE"].ToString());
                Radcombodesignationlist.Value = dt1.Rows[0]["FLDOBJOWNER"].ToString();
                radtbkpirefno.Text = dt1.Rows[0]["FLDREFERENCENO"].ToString();
            }
            



        }
    }
    protected void KPI_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                Guid? kpiid = General.GetNullableGuid(radcobkpi.Value);
                Decimal? KPIminimum = General.GetNullableDecimal(radtbkpiminimumentry.Text);
                Decimal? KPItarget = General.GetNullableDecimal(radtbkpitargetvalueentry.Text);
                Guid? kpitargetid = General.GetNullableGuid(ViewState["KPITARGETID"].ToString());
                int? objectiveowner = General.GetNullableInteger(Radcombodesignationlist.Value);
                string referencenumber = General.GetNullableString(radtbkpirefno.Text);
                if (!IsValidShippingKPITargetDetails(year, kpiid, KPIminimum, KPItarget))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardSKKPITarget.KPITargetUpdate(rowusercode, KPIminimum, KPItarget, objectiveowner, referencenumber,kpitargetid);


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

    private bool IsValidShippingKPITargetDetails(int? year, Guid? kpiid, Decimal? KPIminimum, Decimal? KPItarget)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (year == null)
        {
            ucError.ErrorMessage = "Year.";
        }
        if (kpiid == null)
        {
            ucError.ErrorMessage = "KPI Name.";
        }
        if (KPIminimum == null)
        {
            ucError.ErrorMessage = "KPI Minimum Value.";
        }
        if (KPItarget == null)
        {
            ucError.ErrorMessage = "KPI Target Value.";
        }



        return (!ucError.IsError);
    }

}