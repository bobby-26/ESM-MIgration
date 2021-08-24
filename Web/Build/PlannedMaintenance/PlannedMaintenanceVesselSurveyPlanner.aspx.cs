using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuPlanSurvey.AccessRights = this.ViewState;
            MenuPlanSurvey.MenuList = toolbarsub.Show();
            
           
            if (!IsPostBack)
            {                
                ViewState["VesselId"] = Request.QueryString["vslid"] != null ? Request.QueryString["vslid"] : "";
                ViewState["ScheduleList"] = Request.QueryString["slist"] != null ? Request.QueryString["slist"] : "";
                ViewState["CertificateList"] = Request.QueryString["clist"] != null ? Request.QueryString["clist"] : "";
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void BindData()
    {
        DataTable dt = PhoenixPlannedMaintenanceSurveySchedule.ListVesselCertificateOccasional(PhoenixSecurityContext.CurrentSecurityContext.VesselID, ViewState["CertificateList"].ToString(), null, null, null);
        gvSurvey.DataSource = dt;
        gvSurvey.DataBind();
    }
    protected void MenuPlanSurvey_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails(ucPlanDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceSurveySchedule.PlanSurvey(int.Parse(ViewState["VesselId"].ToString())
                    , General.GetNullableString(ViewState["ScheduleList"].ToString())
                    , General.GetNullableDateTime(ucPlanDate.Text)
                    , General.GetNullableInteger(ddlSurveyPort.SelectedValue)
                    , txtSurveyorName.Text
                    , txtRemarks.Text);

                ucStatus.Text = "Updated Sucessfully";
                ucStatus.Visible = true;
                //string script = string.Format("javascript:fnReloadList('codehelp1');");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }  

    private bool IsValidDetails(string PlanDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(PlanDate))
            ucError.ErrorMessage = "Plan Date is Required";
        if (General.GetNullableDateTime(PlanDate)<DateTime.Today)
            ucError.ErrorMessage = " Plan date should be later then the current date.";
        return (!ucError.IsError);
    }
    private bool IsValidPlanDetails(string PlanDate,string PlanPort)
    {
        PlanPort=(ddlSurveyPort.SelectedValue).ToString();
        if(string.IsNullOrEmpty(PlanDate)&& PlanPort.Equals("Dummy"))
        ucError.ErrorMessage = "Please plan Survey and Save to enter audit data";
        return (!ucError.IsError);
    }

    protected void gvSurvey_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
