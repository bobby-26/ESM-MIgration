using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class AccountsInvoicePaymentVoucherApprovalRevokalHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                string strno = "";
                if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                    ViewState["voucherid"] = Request.QueryString["voucherid"];
                else
                    ViewState["voucherid"] = "";
                if (Request.QueryString["vouchernumber"] != null && Request.QueryString["vouchernumber"] != string.Empty)
                    strno = Request.QueryString["vouchernumber"];
                MenuHistory.Title = "Approval History " + " - " + strno;
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            MenuHistory.AccessRights = this.ViewState;
            MenuHistory.MenuList = toolbarmain.Show();
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
        if (General.GetNullableGuid(ViewState["voucherid"].ToString()) != null)
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherApprovalHistory(new Guid(ViewState["voucherid"].ToString()));

                gvRevokeHistory.DataSource = ds;
           
        }
    }    
    protected void MenuHistory_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;        
    }

    private void BindApprovalData()
    {
        DataSet ds = new DataSet();
        if (General.GetNullableGuid(ViewState["voucherid"].ToString()) != null)
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherApprovalHistorySearch(new Guid(ViewState["voucherid"].ToString()));

                gvApprovaHistory.DataSource = ds;

        }
    }
    protected void gvRevokeHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvApprovaHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindApprovalData();
    }
}
