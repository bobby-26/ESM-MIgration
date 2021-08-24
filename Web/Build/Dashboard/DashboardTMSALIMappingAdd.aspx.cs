using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;


public partial class Dashboard_DashboardTMSALIMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            DataTable dt = PhoemixDashboardSKOVMSA.TMSAElementList();

            RadCombotmsalist.DataSource = dt;

            RadCombotmsalist.DataBind();

            DataTable dt1 = PhoenixDashboardBSCLI.LIList();
            radcobLi.DataSource = dt1;
            radcobLi.DataBind();

        }
    }

    protected void Tabstripspiaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? TMSAid = General.GetNullableGuid(RadCombotmsalist.Value);
                Guid? liid = General.GetNullableGuid(radcobLi.Value);

                if (!IsValidTMSALIDetails(TMSAid, liid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoemixDashboardSKOVMSA.TMSAElementLIMappingInsert(rowusercode, TMSAid, liid);


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

    private bool IsValidTMSALIDetails(Guid? TMSAId, Guid? li)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (TMSAId == null)
        {
            ucError.ErrorMessage = "TMSA Element.";
        }
        if (li == null)
        {
            ucError.ErrorMessage = "LI .";
        }



        return (!ucError.IsError);
    }
}