using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportCrewSign : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
    }

    protected void gvSignoff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnersReportCrew.SignoffListView(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvSignoff.DataSource = dt;
    }

    protected void gvSignoff_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}