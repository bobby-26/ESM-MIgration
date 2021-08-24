using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsRWAApprovalHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                // string strno = "";
                ViewState["Showall"] = string.Empty;
                if (Request.QueryString["orderID"] != null && Request.QueryString["orderID"] != string.Empty)
                    ViewState["orderid"] = Request.QueryString["orderID"];
                if (Request.QueryString["Showall"] != null && Request.QueryString["Showall"] != string.Empty)
                    ViewState["Showall"] = Request.QueryString["Showall"];

                PhoenixToolbar header = new PhoenixToolbar();
               
               // MenuHistory.Title = "Approval History " + " - " + strno;
               // MenuHistory.MenuList = header.Show();

            }
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
        if (General.GetNullableGuid(ViewState["orderid"].ToString()) != null)
        {
            ds = PhoenixAccountsPOStaging.RWAApprovalHistory(new Guid(ViewState["orderid"].ToString()),General.GetNullableInteger(ViewState["Showall"].ToString()));

            gvRevokeHistory.DataSource = ds;
        }
    }
   
   
    protected void gvRevokeHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
