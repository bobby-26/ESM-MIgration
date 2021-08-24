using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using Telerik.Web.UI;

public partial class PlannedMaintenanceRemarksPopup : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWorkOrderReschedule.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                string woid = Request.QueryString["woid"];
                ViewState["WOID"] = !string.IsNullOrEmpty(woid) ? woid : string.Empty;
                string woreschid = Request.QueryString["woreschid"];
                ViewState["WORESCHID"] = !string.IsNullOrEmpty(woreschid) ? woreschid : string.Empty;
                string vlsid = Request.QueryString["vslid"];
                ViewState["VSLID"] = !string.IsNullOrEmpty(vlsid) ? vlsid : string.Empty;
                BindField();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindField()
    {
        
    }
    private bool IsValidPostponement()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtRemarks.Text) == null)
            ucError.ErrorMessage = "Remarks is required.";
        
        return (!ucError.IsError);
    }

    protected void MenuWorkOrderReschedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if(!IsValidPostponement())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleRequestCancel(int.Parse(ViewState["VSLID"].ToString()), new Guid(ViewState["WOID"].ToString()), General.GetNullableGuid(ViewState["WORESCHID"].ToString()), txtRemarks.Text);
                string script = "refresh();";
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
