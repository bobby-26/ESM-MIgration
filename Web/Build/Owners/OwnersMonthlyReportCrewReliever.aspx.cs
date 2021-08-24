using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportCrewReliever : PhoenixBasePage
{ 
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
    }

    protected void gvReleif_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = new DataTable();
        dt = PhoenixOwnersReportCrew.ReliefPlanView(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            , gvReleif.CurrentPageIndex + 1
            , gvReleif.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        gvReleif.DataSource = dt;
        gvReleif.VirtualItemCount = iRowCount;
    }

    protected void gvReleif_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            Label lblRemarks = (Label)e.Item.FindControl("lblRemarks");
            LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            if (imgRemarks != null)
            {
                if (lblRemarks != null)
                {
                    if (lblRemarks.Text != "")
                    {
                        imgRemarks.Visible = true;

                        if (uct != null)
                        {
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = imgRemarks.ClientID;
                            //imgRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                            //imgRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                        }
                    }
                    else
                        imgRemarks.Visible = false;
                }
            }
        }
    }
}