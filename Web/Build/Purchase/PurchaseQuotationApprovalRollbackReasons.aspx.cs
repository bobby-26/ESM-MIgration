using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Data;

public partial class Purchase_PurchaseQuotationApprovalRollbackReasons : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["APPROVALID"] = Request.QueryString["approvalid"];
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvQuotationApprovalRollbackReasons.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void gvQuotationApprovalRollbackReasons_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = PhoenixPurchaseFalApprovalLevel.QuotationFALRollbackReasonSearch(General.GetNullableGuid(ViewState["APPROVALID"].ToString())
                                                            , gvQuotationApprovalRollbackReasons.CurrentPageIndex + 1
                                                            , gvQuotationApprovalRollbackReasons.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        gvQuotationApprovalRollbackReasons.DataSource = dt;
        gvQuotationApprovalRollbackReasons.VirtualItemCount = iRowCount;
    }
}