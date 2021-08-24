using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFalLevelUserMappingList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvUserList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void gvUserList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixPurchaseFalLevel.PurchaseFalLevelUserMappingList
                        (
                          General.GetNullableGuid(Request.QueryString["LEVELMAPPINGID"].ToString()),
                          General.GetNullableGuid(Request.QueryString["LEVELID"].ToString()),
                           gvUserList.CurrentPageIndex + 1,
                           gvUserList.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount
                        );

        gvUserList.DataSource = dt;
        gvUserList.VirtualItemCount = iRowCount;
    }
}
