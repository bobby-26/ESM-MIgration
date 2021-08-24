using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderRoundsReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes["style"] = "display:none";
                DataSet ds = new DataSet();

                ViewState["COMPONENTID"] = null;
                ViewState["WORKORDERID"] = null;
                ViewState["WIEVCOMPONENTID"] = "";
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["COMPONENTJOBID"] = null;
				ViewState["WORKORDERNO"] = null;

                if (Request.QueryString["WORKORDERID"] != null && Request.QueryString["WORKORDERNO"] != null)
				{
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
					ViewState["WORKORDERNO"] = Request.QueryString["WORKORDERNO"].ToString();

					ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportGeneral.aspx?WORKORDERID=";
                    ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportGeneral.aspx?WORKORDERID=" + ViewState["WORKORDERID"] +"&WORKORDERNO=" + ViewState["WORKORDERNO"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportGeneral.aspx";
                }
                ResetMenu(new Guid(Request.QueryString["WORKORDERID"]));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportGeneral.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("RESOURCES"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportDoneBy.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("PARTUSES"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportPartsUsed.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("WORKREPORT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportHistory.aspx?WORKORDERID=";
            }
            else if (CommandName.ToUpper().Equals("HISTORYTEMPLATE"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx?Workorderid=";
            }
            else if (CommandName.ToUpper().Equals("WORKORDER"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceRounds.aspx?WORKORDERID=" + ViewState["WORKORDERID"], false);
            }
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["WORKORDERID"];
            ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resizeFrame();", true);
            //SetTabHighlight();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportGeneral.aspx"))
            {
                MenuWorkOrder.SelectedMenuIndex = 4;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportHistory.aspx"))
            {
                MenuWorkOrder.SelectedMenuIndex = 3;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportComponent.aspx"))
            {
                MenuWorkOrder.SelectedMenuIndex = 2;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportPartsUsed.aspx"))
            {
                MenuWorkOrder.SelectedMenuIndex = 1;
            }
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("PlannedMaintenanceWorkOrderReportDoneBy.aspx"))
            {
                MenuWorkOrder.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        ResetMenu(new Guid(Request.QueryString["WORKORDERID"]));
        ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resizeFrame();", true);
    }
    private void ResetMenu(Guid gWorkOrderId)
    {
        DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(gWorkOrderId);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Back", "WORKORDER", ToolBarDirection.Right);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FLDREPORTUSEDRESOURCES"].ToString() == "1") toolbarmain.AddButton("Resources Used", "RESOURCES",ToolBarDirection.Right);
            if (ds.Tables[0].Rows[0]["FLDREPORTUSEDPARTS"].ToString() == "1") toolbarmain.AddButton("Stock Used", "PARTUSES",ToolBarDirection.Right);
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDREPORTHISTORY"].ToString()))
            {
                if (ds.Tables[0].Rows[0]["FLDHISTORYTEMPLATEMAP"].ToString() == "1") toolbarmain.AddButton("History Template", "HISTORYTEMPLATE",ToolBarDirection.Right);
                if (ds.Tables[0].Rows[0]["FLDHISTORYTEMPLATEMAP"].ToString() == "0") toolbarmain.AddButton("Work Report", "WORKREPORT",ToolBarDirection.Right);
            }
        }
        toolbarmain.AddButton("General", "GENERAL", ToolBarDirection.Right);
        MenuWorkOrder.Title = "Report Work [" + ds.Tables[0].Rows[0]["FLDWORKORDERNUMBER"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDWORKORDERNAME"].ToString() + "]";
        MenuWorkOrder.MenuList = toolbarmain.Show();
        //SetTabHighlight();
    }
}
