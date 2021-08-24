using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceJobAsignComponent : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            toolbarmain.AddButton("Save", "SAVE");
            toolbarmain.AddButton("Job", "JOB");
            MenuStockItemGeneral.AccessRights = this.ViewState;   
            MenuStockItemGeneral.MenuList = toolbarmain.Show();          
            txtComponentId.Attributes.Add("style", "visibility:hidden");           
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
   

 
    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                AsignComponentJob();
            }
            if (dce.CommandName.ToUpper().Equals("JOB"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceJob.aspx");
            }
            
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void AsignComponentJob()
    {
        if (!IsValidForm())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixPlannedMaintenanceComponentJob.AsignComponentJob(new Guid(txtComponentId.Text),
            Request.QueryString["JOBIDS"].ToString(),PhoenixSecurityContext.CurrentSecurityContext.VesselID);    
        
       
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
          if( Request.QueryString["JOBIDS"]==null)
              ucError.ErrorMessage = "Jobs are required.";
          if (txtComponentId.Text.Trim().Equals(""))
              ucError.ErrorMessage = "Component is required.";

         return (!ucError.IsError);
    }
    
    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
    }
  
}
