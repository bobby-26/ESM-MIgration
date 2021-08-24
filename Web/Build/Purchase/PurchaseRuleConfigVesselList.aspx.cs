using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class PurchaseRuleConfigVesselList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid? Configid = General.GetNullableGuid(Request.QueryString["CONFIGID"].ToString());

        ViewState["CONFIGID"] = Configid;

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvvessellist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void gvvessellist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        Guid? Configid = General.GetNullableGuid(ViewState["CONFIGID"].ToString());


        DataTable dt = PhoenixPurchaseRules.PurchaseRulesConfigVessellistSearch
                        (
                           Configid,
                           gvvessellist.CurrentPageIndex + 1,
                           gvvessellist.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount
                        );
                                                   
        gvvessellist.DataSource = dt;
        gvvessellist.VirtualItemCount = iRowCount;
    }
}