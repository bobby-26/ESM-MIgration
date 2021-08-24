using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportPMSRunningHoursView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvRunningHours_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportRunningHours(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvRunningHours.DataSource = dt;
    }
}