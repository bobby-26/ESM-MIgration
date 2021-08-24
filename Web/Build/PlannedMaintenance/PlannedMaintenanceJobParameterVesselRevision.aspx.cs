using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class PlannedMaintenanceJobParameterVesselRevision : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Submit", "SAVE", ToolBarDirection.Right);

            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"];
                dtRevDate.SelectedDate = DateTime.Now.Date;
                dtRevDate.MaxDate = DateTime.Now.Date;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (string.IsNullOrEmpty(ViewState["VESSELID"].ToString()))
                {
                    return;
                }
                if (!isValidRevision(dtRevDate.SelectedDate, txtReason.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Save();
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "refreshParent();", true);
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Save()
    {

        PhoenixPlannedMaintenanceJobParameter.JobParameterForVesselRevisionInsert(int.Parse(ViewState["VESSELID"].ToString()), dtRevDate.SelectedDate, txtReason.Text);
    }

    private bool isValidRevision(DateTime? revDate, string reason)
    {
        if (revDate == null)
            ucError.ErrorMessage = "Revised Date is required.";
        if (reason.Trim() == "")
            ucError.ErrorMessage = "Reason is required.";

        return (!ucError.IsError);
    }
}
