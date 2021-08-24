using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentJobAuditList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;

        DataTable dt = PhoenixPlannedMaintenanceComponentJob.ListComponentJobAduit(new Guid(Request.QueryString["cjobid"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        RadGrid1.DataSource = dt;
        RadGrid1.VirtualItemCount = iRowCount;
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }



    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblJobTitle = (RadLabel)e.Item.FindControl("lblJobTitle");
            UserControlToolTip uctJobTitle = (UserControlToolTip)e.Item.FindControl("ucToolTipJobTitle");
            uctJobTitle.Position = ToolTipPosition.TopCenter;
            uctJobTitle.TargetControlId = lblJobTitle.ClientID;
        }
    }
}



