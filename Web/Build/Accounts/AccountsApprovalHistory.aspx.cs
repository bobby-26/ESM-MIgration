using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsApprovalHistory : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["DOCUMENTID"] = "";
                if (Request.QueryString["docid"] != null && Request.QueryString["docid"] != "")
                {
                    ViewState["DOCUMENTID"] = Request.QueryString["docid"];
                }
                ViewState["POTYPE"] = "";
                if (Request.QueryString["potype"] != null && Request.QueryString["potype"] != "")
                {
                    ViewState["POTYPE"] = Request.QueryString["potype"];
                }
                else
                {
                    ViewState["POTYPE"] = "PurchasePO";
                }

                gvApproval.PageSize = General.ShowRecords(gvApproval.PageSize);
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        DataTable dt = new DataTable();
        if (ViewState["POTYPE"].ToString() == "DirectPO")
        {
            dt = PhoenixAccountsAdvancePayment.DirectPOApprovalHistoryList(General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()));
        }
        else if (ViewState["POTYPE"].ToString() == "BondProvision")
        {
            dt = PhoenixAccountsAdvancePayment.BondProvisionApprovalHistoryList(General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()));
        }
        else
        {
            dt = PhoenixAccountsAdvancePayment.AdvancePaymentHistoryList(General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()));
        }

        gvApproval.DataSource = dt;

        //if (dt.Rows.Count > 0)
        //{
        //    gvApproval.DataSource = dt;
        //    gvApproval.DataBind();
        //}
       
    } 
}
