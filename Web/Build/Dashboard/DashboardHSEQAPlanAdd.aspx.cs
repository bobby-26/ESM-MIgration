using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.ShippingKPI;
using System.Globalization;
using Telerik.Web.UI;

public partial class Dashboard_DashboardHSEQAPlanAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddButton("Save", "save", ToolBarDirection.Right);

        Tabstriphseqaschedulereportmenu.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            DataTable dt1 = PhoenixDashboardHSEQAPlanner.LIList();
            radcobLi.DataSource = dt1;
            radcobLi.DataBind();


        }

    }

    protected void Tabstriphseqaschedulereportmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? liid = General.GetNullableGuid(radcobLi.Value);
                DateTime? planneddate = General.GetNullableDateTime(radplanneddate.Text);
                int? Actionby = General.GetNullableInteger(radcbactionby.SelectedValue);

                PhoenixDashboardHSEQAPlanner.HSEQAPlanInsert(rowusercode, liid, planneddate, Actionby);
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }



    protected void radcobLi_SelectedIndexChanged(object sender, RadMultiColumnComboBoxSelectedIndexChangedEventArgs e)
    {

        Guid? liid = General.GetNullableGuid(radcobLi.Value);

        DataTable dt2 = PhoenixDashboardHSEQAPlanner.PlannerActionby(liid);
        radcbactionby.DataSource = dt2;
        radcbactionby.DataValueField = "FLDUSERCODE";
        radcbactionby.DataTextField = "FLDUSERNAME";
        radcbactionby.DataBind();
    }
}