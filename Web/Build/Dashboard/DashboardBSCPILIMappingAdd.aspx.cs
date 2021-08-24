using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;


public partial class Dashboard_DashboardBSCPILIMappingAdd : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            DataTable dt = PheonixDashboardSKPI.PIList();

            Radcombopilist.DataSource = dt;
            Radcombopilist.DataBind();

            DataTable dt1 = PhoenixDashboardBSCLI.LIList();
            radcobLi.DataSource = dt1;
            radcobLi.DataBind();

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
                Guid? piid = General.GetNullableGuid(Radcombopilist.Value);
                Guid? liid = General.GetNullableGuid(radcobLi.Value);

                if (!IsValidShippingPILIDetails(piid, liid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardBSCLI.PILIMappingInsert(rowusercode, piid, liid);


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