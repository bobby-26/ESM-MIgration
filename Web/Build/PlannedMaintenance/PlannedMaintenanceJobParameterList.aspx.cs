using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class PlannedMaintenanceJobParameterList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["workorderId"] = string.Empty;                
                if (Request.QueryString["WORKORDERID"] != null)
                    ViewState["workorderId"] = Request.QueryString["WORKORDERID"];
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvParameterList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        
        DataTable dt = PhoenixPlannedMaintenanceJobParameter.ListJobParameterValue(int.Parse(ViewState["VESSELID"].ToString()), new Guid(ViewState["workorderId"].ToString()));
        gvParameterList.DataSource = dt;      
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
    }

}
