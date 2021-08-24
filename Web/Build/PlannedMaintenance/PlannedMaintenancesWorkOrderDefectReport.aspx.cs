using System;
using System.Collections.Specialized;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;

public partial class PlannedMaintenancesWorkOrderDefectReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["WorkOrderId"] = null;
                if(Request.QueryString["WorkOrderId"] != null)
                {
                    ViewState["WorkOrderId"] = Request.QueryString["WorkOrderId"].ToString();
                }
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string reqDate = ucDue.Text;
                string reqtitle = txtTitle.Text;
                string reqDetail = txtWorkDetails.Text;

                if(!IsValidJobReport(reqDate, reqDetail, reqtitle))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["WorkOrderId"] != null)
                {
                    PhoenixPlannedMaintenanceWorkOrderGroup.DefectWoUpdate(new Guid(ViewState["WorkOrderId"].ToString()), DateTime.Parse(reqDate), reqDetail, reqtitle); ;
                    ucStatus.Text = "Saved successfully..";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidJobReport(string dueDate,string detail,string title)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(dueDate) == null)
            ucError.ErrorMessage = "Date is required.";
        if (title.Trim() == "")
            ucError.ErrorMessage = "Title is required.";

        if (detail.Trim() == "")
            ucError.ErrorMessage = "Detail is required.";

        return (!ucError.IsError);
    }
 }
