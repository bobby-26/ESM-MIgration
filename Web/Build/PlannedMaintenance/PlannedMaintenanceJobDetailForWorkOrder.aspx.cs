using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJobDetailForWorkOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindFields();
        }
    }

    private void BindFields()
    {
        try
        {

            if ((Request.QueryString["JOBID"] != null) && (Request.QueryString["JOBID"] != ""))
            {
                string str = Request.QueryString["JOBID"].ToString();
                DataSet ds = PhoenixPlannedMaintenanceJob.EditJob(new Guid(Request.QueryString["JOBID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtJobDetail.Content = dr["FLDJOBDETAILS"].ToString();
                    ViewState["dtkey"] = dr["FLDDTKEY"].ToString();                    
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
