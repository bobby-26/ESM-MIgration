using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Data;

public partial class Purchase_PurchaseQuotationFAL : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["APPROVALID"] = Request.QueryString["approvalid"];
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvFal.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }



    protected void gvFal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = PhoenixPurchaseFalApprovalLevel.QuotationFAL(General.GetNullableGuid(ViewState["APPROVALID"].ToString())
                                                            , gvFal.CurrentPageIndex + 1
                                                            , gvFal.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        gvFal.DataSource = dt;
        gvFal.VirtualItemCount = iRowCount;
    }
}