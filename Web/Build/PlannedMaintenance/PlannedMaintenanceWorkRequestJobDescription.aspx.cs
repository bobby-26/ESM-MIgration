using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using SouthNests.Phoenix.Framework;

public partial class PlannedMaintenanceWorkRequestJobDescription : PhoenixBasePage
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

            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceJob.WorkRequestJobDescriptionEdit(new Guid(Request.QueryString["WORKORDERID"].ToString()));
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
