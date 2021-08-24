using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class AccountsPurchaseInvoiceReportViewHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            string strno = "";
            if (!IsPostBack)
            {
         
                if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                    ViewState["voucherid"] = Request.QueryString["voucherid"];
                else
                    ViewState["voucherid"] = "";
                if (Request.QueryString["vouchernumber"] != null && Request.QueryString["vouchernumber"] != string.Empty)
                    strno = Request.QueryString["vouchernumber"];
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();

            MenuHistory.Title = "Paymentvoucher Report Generated History " + " - " + strno;

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
        if (General.GetNullableGuid(ViewState["voucherid"].ToString()) != null)
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentReportViewHistorySearch(new Guid(ViewState["voucherid"].ToString()));                             
                gvApprovaHistory.DataSource = ds;
        }
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
