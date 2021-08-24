using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;


public partial class PlannedMaintenanceRescheduleFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO");
            MenuMaintenanceRescheduleFilter.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
               ckPlaning.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.PLANNING));
               ckPlaning.DataBind();
               chkClasses.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixQuickTypeCode.JOBCLASS));
               chkClasses.DataBind();
               chkStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.WORKORDERSTATUS));
               chkStatus.DataBind();
                
                //ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STOCKCLASS).ToString();
               ucMainType.QuickTypeCode = "32";
               ucMaintClass.QuickTypeCode = "30";
               ucMainCause.QuickTypeCode = "29";
            }
 
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMaintenanceRescheduleFilter_TabStripCommand(object sender, EventArgs e)
    {

        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GO"))
            {
                string ststus = "";
                string planning = "";
                string jobclass = "";
                foreach (ListItem item in chkStatus.Items)
                {
                    if (item.Selected)
                        ststus = ststus + item.Value + ",";                   
                }
                foreach (ListItem item in ckPlaning.Items)
                {
                    if (item.Selected)
                        planning = planning + item.Value + ",";
                }
                foreach (ListItem item in chkClasses.Items)
                {
                    if (item.Selected)
                        jobclass = jobclass + item.Value + ",";
                }
                ststus = ststus.TrimEnd(new Char[] { ',' });
            
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtWorkOrderNumber", txtWorkOrderNumber.Text);
                criteria.Add("txtWorkOrderName", txtTitle.Text);
                criteria.Add("txtComponentNumber", txtComponentNumber.Text.TrimEnd("000.00.00".ToCharArray()));
                criteria.Add("txtComponentName", txtComponentName.Text.Trim());
                criteria.Add("ucRank", ucRank.SelectedRank);
                criteria.Add("txtDateFrom", txtDateFrom.Text);
                criteria.Add("txtDateTo", txtDateTo.Text);
                criteria.Add("status", ststus.TrimEnd(new Char[] { ',' }));
                criteria.Add("planning", planning.TrimEnd(new Char[]{','}));
                criteria.Add("jobclass", jobclass.TrimEnd(new Char[] { ',' }));
                criteria.Add("ucMainType", ucMainType.SelectedQuick);
                criteria.Add("ucMainCause", ucMainCause.SelectedQuick);
                criteria.Add("ucMaintClass", ucMaintClass.SelectedQuick);
                criteria.Add("chkUnexpected", (chkUnexpected.Checked).ToString());
                criteria.Add("txtPriority", txtPriority.Text);
                Filter.CurrentWorkOrderFilter = criteria;
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReschedule.aspx");
            }

           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
