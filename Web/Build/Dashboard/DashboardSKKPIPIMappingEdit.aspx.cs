using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Inspection_InspectionShippingKPIPIMappingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);

        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            DataTable dt = PheonixDashboardSKKPI.KPIList();

            RadComboKpilist.DataSource = dt;
          
            RadComboKpilist.DataBind();

            DataTable dt1 = PheonixDashboardSKPI.PIList();
            Radcombopilist.DataSource = dt1;
          
            Radcombopilist.DataBind();

            ViewState["KPI2PILINKID"] = General.GetNullableGuid(Request.QueryString["shippingkpipilinkid"]);
            Guid? shippingkpipilinkid = General.GetNullableGuid(ViewState["KPI2PILINKID"].ToString());

            DataTable dt2 = PheonixDashboardSKKPI.KPIPIEditList(shippingkpipilinkid);
            if (dt2.Rows.Count > 0)
            {

                RadComboKpilist.Value = dt2.Rows[0]["FLDKPIID"].ToString();
                Radcombopilist.Value = dt2.Rows[0]["FLDPIID"].ToString();

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
                Guid? kpiid = General.GetNullableGuid(RadComboKpilist.Value);
                Guid? piid = General.GetNullableGuid(Radcombopilist.Value);
                Guid? shippingkpipilinkid = General.GetNullableGuid(ViewState["KPI2PILINKID"].ToString());
                if (!IsValidShippingKPIPIDetails(kpiid, piid))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.KPIPIUpdate(rowusercode, kpiid, piid, shippingkpipilinkid);


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

    private bool IsValidShippingKPIPIDetails(Guid? kpi, Guid? pi)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (kpi == null)
        {
            ucError.ErrorMessage = "KPI.";
        }
        if (pi == null)
        {
            ucError.ErrorMessage = "PI .";
        }



        return (!ucError.IsError);
    }
}