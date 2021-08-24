using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderGroupLogFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuWorkOrderReportLogFilter.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ucCompCategory.QuickTypeCode = "165";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrderReportLogFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtWorkOrderNumber", txtWorkOrderNumber.Text);
                criteria.Add("txtWorkOrderName", txtWorkOrderTitle.Text);
                criteria.Add("txtDateFrom", General.GetNullableDateTime(txtWorkDoneDateFrom.Text).ToString());
                criteria.Add("txtDateTo", General.GetNullableDateTime(txtWorkDoneDateTo.Text).ToString());
                criteria.Add("ucCompCategory", ucCompCategory.SelectedQuick);
                criteria.Add("rcbIsPlanned", chkRoutine.Checked == true ? "0" : string.Empty);
                Filter.CurrentWorkOrderReportLogFilter = criteria;
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}