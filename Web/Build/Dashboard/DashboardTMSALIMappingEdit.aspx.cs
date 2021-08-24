using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;

public partial class Dashboard_DashboardTMSALIMappingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
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

            ViewState["TMSALILINKID"] =Request.QueryString["tmsalilinkid"];

            DataTable dt2 = PhoemixDashboardSKOVMSA.TMSAElementLilinkEdit(General.GetNullableGuid(ViewState["TMSALILINKID"].ToString()));
            RadCombotmsalist.Value = dt2.Rows[0]["FLDTMSAID"].ToString();
            radcobLi.Value = dt2.Rows[0]["FLDLIID"].ToString();

        }
    }

    protected void Tabstripspiaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? liid = General.GetNullableGuid(radcobLi.Value);
                Guid? TMSA2LiLinkid = General.GetNullableGuid(ViewState["TMSALILINKID"].ToString());
                if (!IsValidTMSALIDetails( liid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoemixDashboardSKOVMSA.TMSAElementLIMappingUpdate(rowusercode,  liid, TMSA2LiLinkid);


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

    private bool IsValidTMSALIDetails( Guid? li)
    {
        ucError.HeaderMessage = "Provide the following required information";
       
        if (li == null)
        {
            ucError.ErrorMessage = "LI .";
        }



        return (!ucError.IsError);
    }
}