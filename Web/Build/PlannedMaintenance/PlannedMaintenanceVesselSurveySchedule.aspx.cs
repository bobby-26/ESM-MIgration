using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveySchedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Postponement", "POSTPONE");
            toolbarmain.AddButton("Certificate List", "CERTIFICATES");
            toolbarmain.AddButton("Survey COC", "SURVEYCOC");
            toolbarmain.AddButton("Report Survey", "COMPLETE");
            if (string.IsNullOrEmpty(Request.QueryString["IsLog"]) || !Request.QueryString["IsLog"].Equals("1"))
                toolbarmain.AddButton("Back", "SCHEDULELIST");
            MenuSurveyScheduleHeader.AccessRights = this.ViewState;
            MenuSurveyScheduleHeader.MenuList = toolbarmain.Show();
            MenuSurveyScheduleHeader.SetTrigger(pnlSurvey);

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["VesselId"] = Request.QueryString["VesselId"];
                ViewState["ScheduleId"] = Request.QueryString["ScheduleId"];
                ViewState["AllowEdit"] = Request.QueryString["AllowEdit"];
                ViewState["Dtkey"] = Request.QueryString["Dtkey"];
                ViewState["IsLog"] = string.IsNullOrEmpty(Request.QueryString["IsLog"]) ? "" : Request.QueryString["IsLog"];
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleGeneral.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&AllowEdit=" + ViewState["AllowEdit"].ToString();
                ViewState["CURRENTINDEX"] = 0;
            }
            MenuSurveyScheduleHeader.SelectedMenuIndex = Int32.Parse(ViewState["CURRENTINDEX"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSurveyScheduleHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleGeneral.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&AllowEdit=" + ViewState["AllowEdit"].ToString()
                    + "&IsLog=" + ViewState["IsLog"].ToString();
                ViewState["CURRENTINDEX"] = 0;
            }
            else if (dce.CommandName.ToUpper().Equals("POSTPONE"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveySchedulePostpone.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&AllowEdit=" + ViewState["AllowEdit"].ToString()
                    + "&IsLog=" + ViewState["IsLog"].ToString();
                ViewState["CURRENTINDEX"] = 1;
            }
            else if (dce.CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateList.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&AllowEdit=" + ViewState["AllowEdit"].ToString()
                    + "&IsLog=" + ViewState["IsLog"].ToString();
                ViewState["CURRENTINDEX"] = 2;
            }
            else if (dce.CommandName.ToUpper().Equals("COMPLETE"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleComplete.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&AllowEdit=" + ViewState["AllowEdit"].ToString()
                    + "&IsLog=" + ViewState["IsLog"].ToString();
                ViewState["CURRENTINDEX"] = 4;
            }
            else if (dce.CommandName.ToUpper().Equals("SURVEYCOC"))
            {
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleCOC.aspx?ScheduleId="
                    + ViewState["ScheduleId"].ToString() + "&VesselId=" + ViewState["VesselId"].ToString() + "&AllowEdit=" + ViewState["AllowEdit"].ToString()
                    + "&IsLog=" + ViewState["IsLog"].ToString();
                ViewState["CURRENTINDEX"] = 3;
            }
            else if (dce.CommandName.ToUpper().Equals("SCHEDULELIST"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
