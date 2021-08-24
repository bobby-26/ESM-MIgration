using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;

public partial class Dashboard_DashboardBSCPILIMappingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PI2LILINKID"] = Request.QueryString["shippingpililinkid"];
            Guid? pi2lilinkid = General.GetNullableGuid(ViewState["PI2LILINKID"].ToString());
            DataTable dt = PheonixDashboardSKPI.PIList();
            Radcombopilist.DataSource = dt;
            Radcombopilist.DataBind();

            DataTable dt1 = PhoenixDashboardBSCLI.LIList();
            radcobLi.DataSource = dt1;
            radcobLi.DataBind();

            DataTable dt2 = PhoenixDashboardBSCLI.PILIMappingEditList(pi2lilinkid);

            Radcombopilist.Value = dt2.Rows[0]["FLDPIID"].ToString();
            radcobLi.Value = dt2.Rows[0]["FLDLIID"].ToString();
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
                Guid? piid = General.GetNullableGuid(Radcombopilist.Value);
                Guid? liid = General.GetNullableGuid(radcobLi.Value);
                Guid? pi2lilinkid = General.GetNullableGuid(ViewState["PI2LILINKID"].ToString());
                if (!IsValidShippingPILIDetails(piid, liid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSCLI.PILIMappingUpdate(rowusercode, piid, liid , pi2lilinkid);


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

    private bool IsValidShippingPILIDetails(Guid? pi, Guid? li)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (pi == null)
        {
            ucError.ErrorMessage = "PI.";
        }
        if (li == null)
        {
            ucError.ErrorMessage = "LI .";
        }



        return (!ucError.IsError);
    }
}