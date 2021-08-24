using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFalVesselList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid? Approvalid = General.GetNullableGuid(Request.QueryString["APPROVALID"].ToString());

        ViewState["APPROVALID"] = Approvalid;

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }


    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        Guid? Approvalid = General.GetNullableGuid(ViewState["APPROVALID"].ToString());

        DataTable dt = PhoenixPurchaseFalApprove.PurchaseFalApproveVessellistSearch(
                           Approvalid,
                           gvVesselList.CurrentPageIndex + 1,
                           gvVesselList.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount
                        );

        gvVesselList.DataSource = dt;
        gvVesselList.VirtualItemCount = iRowCount;
    }
}