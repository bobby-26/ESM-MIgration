using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsOfficeLeaveWagesAndPerformanceBonus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            
            toolbar.AddButton("Leave Report", "LEAVEREPORT",ToolBarDirection.Right);
            toolbar.AddButton("Portage Bill", "PORTAGEBILL", ToolBarDirection.Right);

            MenuReportLeaveWagesPerformanceBonus.AccessRights = this.ViewState;
            MenuReportLeaveWagesPerformanceBonus.MenuList = toolbar.Show();
            MenuReportLeaveWagesPerformanceBonus.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {

                ifMoreInfo.Attributes["src"] = "../Reports/ReportsView.aspx?applicationcode=7&reportcode=OFFICELEAVEWAGESANDPERFORMANCEBONUS&showmenu=false&showexcel=no&showword=no&vslid=" + Request.QueryString["vslid"].ToString() + "&date=" + Request.QueryString["date"].ToString();

                ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resize();", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReportLeaveWagesPerformanceBonus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PORTAGEBILL"))
            {
                Response.Redirect("AccountsOfficePortageBillList.aspx?" + Request.QueryString.ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
