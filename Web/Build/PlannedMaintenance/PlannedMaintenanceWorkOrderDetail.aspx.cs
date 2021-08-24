using System;
using System.Data;
using System.Web;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuDetail.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            BindFields();
        }
    }

    private void BindFields()
    {
        try
        {

            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                int vesselid = Request.QueryString["vesselid"] != null ? int.Parse(Request.QueryString["vesselid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                DataSet ds = PhoenixPlannedMaintenanceWorkOrder.EditWorkOrder(new Guid(Request.QueryString["WORKORDERID"].ToString()), vesselid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtDetail.Content = HttpUtility.HtmlDecode(dr["FLDDETAILS"].ToString());
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


    protected void MenuDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
                {
                    PhoenixPlannedMaintenanceWorkOrder.UpdateDetailsWorkOrder
                    (
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["WORKORDERID"].ToString()),
                         txtDetail.Content.ToString()
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
