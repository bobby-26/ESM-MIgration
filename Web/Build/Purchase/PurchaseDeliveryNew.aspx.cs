using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PurchaseDeliveryNew : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuStockItemGeneral.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                //if (Filter.CurrentVesselConfiguration == null ||Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                //{
                //    if (Filter.LastVesselSelected != null)
                //    {
                //        NameValueCollection nvc = Filter.LastVesselSelected;
                //        int vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());
                //        ucVessel.SelectedVessel = vesselid.ToString();
                //    }
                //}
                //else
                //{
                //    ucVessel.SelectedVessel = Filter.CurrentVesselConfiguration.ToString();
                //    ucVessel.Enabled = false;                  

                //}

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        Filter.CurrentPurchaseStockType = ddlStockType.SelectedValue;        
        Filter.CurrentPurchaseVesselSelection = (General.GetNullableInteger(ucVessel.SelectedVessel) == null)? -1 : int.Parse(ucVessel.SelectedVessel);      
    }
    
    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return; 
                }
                InsertDelivery();
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void InsertDelivery()
    {
        PhoenixPurchaseDelivery.Insertdelivery(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ucVessel.SelectedVessel),null, null);                 
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required";      
        return (!ucError.IsError);
    }
}
