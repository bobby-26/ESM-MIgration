using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Dashboard;

public partial class Accounts_AccountsDashboardPortageBillViewHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
           // string strno = "";
            if (!IsPostBack)
            {
                ViewState["voucherid"] = "";
                ViewState["Month"] = "";
                ViewState["Year"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != string.Empty)
                    ViewState["vesselid"] = Request.QueryString["vesselid"];

                if (Request.QueryString["Month"] != null && Request.QueryString["Month"] != string.Empty)
                    ViewState["Month"] = Request.QueryString["Month"];

                if (Request.QueryString["Year"] != null && Request.QueryString["Year"] != string.Empty)
                    ViewState["Year"] = Request.QueryString["Year"];

            }
            PhoenixToolbar toolbar = new PhoenixToolbar();

           // MenuHistory.Title = "Portage Bill Not Yet Finalized  " + " - " + strno;

            MenuHistory.AccessRights = this.ViewState;
            MenuHistory.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataSet ds = new DataSet();
        //if (General.GetNullableGuid(ViewState["voucherid"].ToString()) != null)
        //{
            ds = PhoenixDashboardAccounts.ListAccountsSendMailHistory(
                                                                 General.GetNullableInteger(ViewState["Month"].ToString())
                                                                 , General.GetNullableInteger(ViewState["Year"].ToString())
                                                                 ,General.GetNullableInteger(ViewState["vesselid"].ToString())
                                                                   );
            gvApprovaHistory.DataSource = ds;
        //}
    }
    protected void MenuHistory_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    }

    protected void gvApprovaHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
