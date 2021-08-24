using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderDefectJobCreate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
            //txtJobId.Attributes.Add("style", "visibility:hidden");
            ViewState["ComponentId"] = null;
            ViewState["WorkorderId"] = null;
            if (Request.QueryString["ComponentId"] != null)
            {
                ViewState["ComponentId"] = Request.QueryString["ComponentId"].ToString();
            }

            if (Request.QueryString["WorkorderId"] != null)
            {
                ViewState["WorkorderId"] = Request.QueryString["WorkorderId"].ToString();
            }

        }
    }

    
    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRequisition(txtTitle.Text, ucPlannedDate.Text,ucDiscipline.SelectedDiscipline,txtJobDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["WorkorderId"] != null && ViewState["ComponentId"] != null)
                {

                    PhoenixPlannedMaintenanceWorkOrderGroup.WorkrequestInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        General.GetNullableString(txtTitle.Text.Trim()),
                        General.GetNullableGuid(ViewState["ComponentId"].ToString()),
                        null, null, null, null, null,
                        General.GetNullableDateTime(ucPlannedDate.Text),
                        General.GetNullableInteger(ucDiscipline.SelectedDiscipline),
                        null, null, null, null, null,
                        1, new Guid(ViewState["WorkorderId"].ToString()), null, null, null, null, null,
                        txtJobDescription.Text,
                        null, null);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"", "CloseModal();", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidRequisition(string componentid, string plannedstartdate,string discipline, string workdetails)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(plannedstartdate).HasValue)
            ucError.ErrorMessage = "Planned Start Date is required";

        if (!General.GetNullableInteger(discipline).HasValue)
            ucError.ErrorMessage = "Planned Start Date is required";

        if (string.IsNullOrEmpty(workdetails))
            ucError.ErrorMessage = "Job code/Work details required ";

        //if (ucMaintClass.SelectedText.ToLower().Contains("docking") && !General.GetNullableInteger(ddlJobType.SelectedValue).HasValue)

        return (!ucError.IsError);
    }
}
