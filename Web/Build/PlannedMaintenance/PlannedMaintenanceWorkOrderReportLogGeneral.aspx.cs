using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Common;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportLogGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();        
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuWorkOrderGeneral.AccessRights = this.ViewState;
        MenuWorkOrderGeneral.MenuList = toolbarmain.Show();
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            MenuWorkOrderGeneral.Visible = false;
        if (!IsPostBack)
        {
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
            ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
            ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();
            ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            BindFields();
        }        
    }
    protected void MenuWorkOrderGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() =="SAVE")
            {
                PhoenixPlannedMaintenanceWorkOrderReport.UpdateWorkOrderReportLog(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()).Value
                   , General.GetNullableInteger(ucMainType.SelectedQuick)
                   , General.GetNullableInteger(ucMaintClass.SelectedQuick)
                   , General.GetNullableInteger(ucMainCause.SelectedQuick)
                   , byte.Parse(chkUnexpected.Checked == true ? "1" : "0"  )
                   , byte.Parse(chkDefectList.Checked == true ? "1" : "0"));

                BindFields();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                
                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportLogGeneralSearch(new Guid(Request.QueryString["WORKORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtWorkOrderNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
                    txtWorkOrderTitle.Text = dr["FLDWORKORDERNAME"].ToString();
                    txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                    txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                    txtResponsibility.Text = dr["FLDRESPONSIBILITY"].ToString();
                    txtWorkDoneDate.Text = General.GetDateTimeToString(dr["FLDWORKDONEDATE"].ToString());
                    txtWorkDuration.Text = dr["FLDWORKDURATION"].ToString();
                    txtCounter.Text = dr["FLDCURRENTVALUES"].ToString();
                    txtStartedDate.Text = General.GetDateTimeToString(dr["FLDWORKORDERSTARTEDDATE"].ToString());
                    txtCompletedDate.Text = General.GetDateTimeToString(dr["FLDWORKORDERCOMPLETEDDATE"].ToString());
                    ucMainType.SelectedQuick = dr["FLDWORKMAINTNANCETYPE"].ToString();
                    ucMaintClass.SelectedQuick = dr["FLDWORKMAINTNANCECLASS"].ToString();
                    ucMainCause.SelectedQuick = dr["FLDWORKMAINTNANCECAUSE"].ToString();
                    if (dr["FLDWORKISUNEXPECTED"].ToString().Equals("1"))
                        chkUnexpected.Checked = true;
                    if (dr["FLDISDEFECTYN"].ToString().Equals("1"))
                        chkDefectList.Checked = true;

                    txtVslVerified.Text = dr["FLDVESSELVERFIEDBY"].ToString();
                    txtVslVerifiedDate.Text = General.GetDateTimeToString(dr["FLDVESSELVERIFIEDDATE"].ToString());
                    txtSupntVerified.Text = dr["FLDOFFICEVERIFIEDBY"].ToString();
                    txtSupntVerifiedDate.Text = General.GetDateTimeToString(dr["FLDOFFICEVERIFIEDDATE"].ToString());
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
