using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportLogFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Go", "GO", ToolBarDirection.Right);
                MenuWorkOrderReportLogFilter.MenuList = toolbarmain.Show();
                chkClasses.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixQuickTypeCode.JOBCLASS));
                chkClasses.DataBind();

                //ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STOCKCLASS).ToString();
                ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
                ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();
            }

            img1.Attributes.Add("onclick", "javascript:return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx', true);");
            
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
                string jobclass = "";
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtWorkOrderNumber", txtWorkOrderNumber.Text);
                criteria.Add("txtWorkOrderName", txtWorkOrderTitle.Text);
                criteria.Add("txtComponentNumber", txtComponentNumber.Text.TrimEnd("000.00.00".ToCharArray()));
                criteria.Add("txtComponentName", txtComponentName.Text.Trim());
                criteria.Add("txtDateFrom", txtWorkDoneDateFrom.Text);
                criteria.Add("txtDateTo", txtWorkDoneDateTo.Text);
                criteria.Add("totalduration", string.Empty);
                criteria.Add("ucrank", ucWorkDoneBy.SelectedDiscipline);
                criteria.Add("chkoverdue", chkPlanning.Checked == true ? "19" : "");
                //criteria.Add("planning", planning.TrimEnd(new Char[] { ',' }));
                foreach (ButtonListItem item in chkClasses.Items)
                {
                    if (item.Selected)
                        jobclass = jobclass + item.Value + ",";
                }
                criteria.Add("ucMainType", ucMainType.SelectedQuick);
                criteria.Add("ucMainCause", ucMainCause.SelectedQuick);
                criteria.Add("ucMaintClass", ucMaintClass.SelectedQuick);               
                criteria.Add("ddlJobType", ddlJobType.SelectedValue);                
                criteria.Add("jobclass", jobclass.TrimEnd(','));
                //criteria.Add("txtJobId", txtJobId.Text);
                criteria.Add("txtClassCode", txtClassCode.Text);
                criteria.Add("ucFrequency", ucFrequency.SelectedHard);
                criteria.Add("txtFrequencyFrom", txtFrequencyFrom.Text);
                //criteria.Add("txtFrequencyTo", txtFrequencyTo.Text);
                //criteria.Add("ucCounterType", ucCounterType.SelectedHard);
                criteria.Add("ucCounterFrequencyFrom", ucCounterFrequencyFrom.Text);
                //criteria.Add("ucCounterFrequencyTo", ucCounterFrequencyTo.Text);
                criteria.Add("txtPriority", txtPriority.Text);
                criteria.Add("chkPostponed", (chkPostponed.Checked == true ? "1" : string.Empty));
                criteria.Add("chkRaJob", (chkRaJob.Checked == true ? "1" : string.Empty));
                criteria.Add("chkCritical", (chkCritical.Checked == true ? "1" : string.Empty));
                
                Filter.CurrentWorkOrderReportLogFilter = criteria;
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

}
