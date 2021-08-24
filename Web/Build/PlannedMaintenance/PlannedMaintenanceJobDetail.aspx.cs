using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJobDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuJobDetail.MenuList = toolbarmain.Show();
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

    protected void MenuJobDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["JOBID"] != null) && (Request.QueryString["JOBID"] != ""))
                {
                    PhoenixPlannedMaintenanceJob.UpdateJobDetails
                    (
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["JOBID"].ToString()),
                         txtJobDetail.Content.ToString()
                    );

                    ucStatus.Text = "Details Saved.";
                 
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
