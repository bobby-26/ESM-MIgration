using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionAuditInterfaceJobMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void gvInspectionJob_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Guid?   CheckItemId = General.GetNullableGuid(Request.QueryString["CHECKITEMID"].ToString());
        int? vesselid = General.GetNullableInteger(Request.QueryString["VesselId"].ToString());

        DataTable dt = PhoenixInspectionAuditInterfaceDetails.InspectionJobMappingList(CheckItemId,vesselid);
        gvInspectionJob.DataSource = dt;

    }


    protected void gvInspectionJob_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lblDueDate = (RadLabel)e.Item.FindControl("lblDueDate");
                if (drv["FLDDUE"].ToString().Equals("1"))
                {
                    lblDueDate.Attributes["style"] = "color:Red !important";
                    lblDueDate.Font.Bold = true;
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
