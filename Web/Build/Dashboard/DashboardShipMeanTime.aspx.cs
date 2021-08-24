using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Dashboard_DashboardShipMeanTime : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarsubtap = new PhoenixToolbar();
        toolbarsubtap.AddButton("Save", "SAVE", ToolBarDirection.Right);     

        MenuSMT.AccessRights = this.ViewState;
        MenuSMT.MenuList = toolbarsubtap.Show();
    }
    protected void MenuSMT_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string script = "parent.frames[1].$find(\"RadAjaxManager1\").ajaxRequest(\"SMT\");top.closeTelerikWindow('smt');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}