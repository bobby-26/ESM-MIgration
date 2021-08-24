using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;


public partial class PlannedMaintenanceComponentJobDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtComponentDetail.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Preview;
            BindFields();
        }
    }

    private void BindFields()
    {
        try
        {

            if ((Request.QueryString["JOBID"] != null) && (Request.QueryString["JOBID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceJob.EditJob(new Guid(Request.QueryString["JOBID"].ToString()));               
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtComponentDetail.Content = dr["FLDJOBDETAILS"].ToString();                    
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
