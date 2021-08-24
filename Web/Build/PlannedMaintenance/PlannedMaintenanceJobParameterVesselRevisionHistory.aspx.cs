using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
public partial class PlannedMaintenanceJobParameterVesselRevisionHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = string.Empty;

                if (!string.IsNullOrEmpty(Request.QueryString["VESSELID"]))
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"];
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixPlannedMaintenanceJobParameter.JobParameterForVesselRevisionList(General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            gvJobParameter.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

}
