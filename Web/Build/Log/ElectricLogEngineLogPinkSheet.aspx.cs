using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogEngineLogPinkSheet : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    DateTime currentDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        EngineDockZone.Visible = SessionUtil.CanAccess(this.ViewState, "ENGINE");
        TurboChargerDockZone.Visible = SessionUtil.CanAccess(this.ViewState, "TURBOCHARGER");
        LODockZone.Visible = SessionUtil.CanAccess(this.ViewState, "LO");
        AuxilaryDockZone.Visible = SessionUtil.CanAccess(this.ViewState, "AUXILARY");
        EmergencyDockZone.Visible = SessionUtil.CanAccess(this.ViewState, "EMERGENCY");
        AirCompressorDockZone.Visible = SessionUtil.CanAccess(this.ViewState, "AIRCOMPRESSOR");
        MaintenanceDockZone.Visible = SessionUtil.CanAccess(this.ViewState, "MAINTENANCE");

        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = int.Parse(Filter.SelectedOwnersReportVessel);

        if (Filter.SelectedOwnersReportDate != null)
        {
            currentDate = Convert.ToDateTime(Filter.SelectedOwnersReportDate);
        }
        else
        {
            currentDate = DateTime.Now;
        }


        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ShowHideRowsColumns();
            getEmergencySafetyEquipments();

            lnkEngineDepartmentStaffComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Engine Department Staff', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=EDS');return false; ");
            lnkTurboChargersComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Turbo Chargers', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=TUR');return false; ");
            lnkLOAnalysisComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'L.O. Analysis', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=LOA');return false; ");
            lnkAuxEnginesComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Aux. Engines', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=AES');return false; ");
            lnkEmergencySafetyComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Emergency & Safety Equipment Notes (Dates Tested)', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=ESE');return false; ");
            lnkAirCompressorComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Air Compressor', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=ACM');return false; ");
            lnkMaintenancePositionComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Maintenance Position', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=MAP');return false; ");

            lnkEngineDepartmentStaffInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Engine Department Staff','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=EDS" + "',false, 320, 250,'','',options); return false;");
            lnkTurboChargersInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Turbo Chargers','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=TUR" + "',false, 320, 250,'','',options); return false;");
            lnkLOAnalysisInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','L.O. Analysis','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=LOA" + "',false, 320, 250,'','',options); return false;");
            lnkAuxEnginesInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Aux. Engines','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=AES" + "',false, 320, 250,'','',options); return false;");
            lnkEmergencySafetyInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Emergency & Safety Equipment Notes (Dates Tested)','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=ESE" + "',false, 320, 250,'','',options); return false;");
            lnkAirCompressorInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Air Compressor','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=ACM" + "',false, 320, 250,'','',options); return false;");
            lnkMaintenancePositionInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Maintenance Position','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=MAP" + "',false, 320, 250,'','',options); return false;");
        }
        CheckComments();
    }

    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("EDS", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkEngineDepartmentStaffComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("TUR", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkTurboChargersComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("LOA", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkLOAnalysisComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("AES", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkAuxEnginesComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("ESE", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkEmergencySafetyComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("ACM", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkAirCompressorComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("MAP", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkMaintenancePositionComments.Controls.Add(html);
        }
    }
    private void ShowHideRowsColumns()
    {
        try
        {
            DataSet ds = PhoenixEngineLogAttributes.EngineLogAttributesList(1, vesselId, 0);
            if (ds != null)
            {
                RadGrid grid = null;
                int units = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    switch ((String)row["FLDSHORTCODE"])
                    {
                        case "UPME":
                            grid = gvMaintaincePosition;
                            units = Convert.ToInt32(row["FLDQUANTITY"]);
                            break;
                        case "TUME":
                            grid = gvTurboChargers;
                            units = Convert.ToInt32(row["FLDQUANTITY"]);
                            break;
                        case "AUEN":
                            grid = gvAuxillaryEngine;
                            units = Convert.ToInt32(row["FLDQUANTITY"]);
                            showHideGridColumns(gvEngineLOAnalysis, units);  // FOR LO ANALYSIS SAME UNITS
                            break;
                        case "MACM":
                            grid = gvAirCompressor;
                            units = Convert.ToInt32(row["FLDQUANTITY"]);
                            break;
                    }

                    showHideGridColumns(grid, units);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void showHideGridColumns(RadGrid grid, int units)
    {
        if (grid == null) return;

        for (int i = 1; i <= units; i++)
        {
            if (grid.MasterTableView.GetColumn("unit" + i) != null)
            {
                grid.MasterTableView.GetColumn("unit" + i).Visible = true;
            }
        }
    }

    protected void gvEngineDepartment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gvEngineDepartment.DataSource = PhoenixEngineLog.EngineDepartmentStaffSearch(usercode, vesselId, currentDate.Month, currentDate.Year);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void getEmergencySafetyEquipments()
    {
        try
        {
            DataSet ds = PhoenixEngineLog.EmergencySafertySearch(usercode, vesselId, currentDate.Month, currentDate.Year);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                lblEngineGenerator.Text = ((string)row["FLDEMERGENCYGENERATOR"]);
                lblEmergencyFirePump.Text = ((string)row["FLDEMERGENCYFIREPUMP"]);
                lblEmergencyAirComp.Text = ((string)row["FLDAIRCOMPRESSOR"]);
                lblERFireDetectors1.Text = ((string)row["FLDEERFIREDETECTORS"]);
                lblERFireDetectors2.Text = ((string)row["FLDEERFIREDETECTORS1"]);
                lblQuickClosingValves.Text = ((string)row["FLDQUICKCLOSINGVALVES"]);
                lblEmergencyTrips.Text = ((string)row["FLDEMERGENCYTRIPS"]);
                lblCO2Alarm.Text = ((string)row["FLDCO2ALARM"]);
                lblCo2PressureAlarm.Text = ((string)row["FLDCO2PRESSURE"]);
                lblERFlapsSkyLights.Text = ((string)row["FLDERFLAPS"]);
                lblEmergencySteeringTest.Text = ((string)row["FLDEEMERGENCYSTEERTEST"]);
                lblMEEmergencyControls.Text = ((string)row["FLDMEEMERGENCYCONTROL"]);
                lblBoilerSafetyCutOut.Text = ((string)row["FLDBOILERSAFETYCUT"]);

                ////right section
                lblLifeBoatEngine.Text = ((string)row["FLDLIFEBOAT"]);
                lblLifeBoatDavitSafeties.Text = ((string)row["FLDLIFEBOATDAVIT"]);
                lblFireDoorsWTDoorsClosing.Text = ((string)row["FLDFIREDOOR"]);
                lblCargoCraneSafeties.Text = ((string)row["FLDCARGOCRANESAFETY"]);
                lblProvisionCraneSafeties.Text = ((string)row["FLDPROVISIONCRANE"]);
                lblAccomLadderSafeties.Text = ((string)row["FLDACCOMLADDER"]);
                lblAEShutdownSafeties1.Text = ((string)row["FLDAESHUTDOWN1"]);
                lblAEShutdownSafeties2.Text = ((string)row["FLDAESHUTDOWN2"]);
                lblAEShutdownSafeties3.Text = ((string)row["FLDAESHUTDOWN3"]);
                lblMEShutdownSafeties.Text = ((string)row["FLDMESHUTDOWN"]);
                lblERBilgeAlarams.Text = ((string)row["FLDERBILGE"]);

                lblOtherBilgeAlarm1.Text = ((string)row["FLDOTHERBILGE"]);
                lblOtherBilgeAlarm2.Text = ((string)row["FLDOTHERBILGEALARAM1"]);
                lblOtherBilgeAlarm3.Text = ((string)row["FLDOTHERBILGEALARAM2"]);

            }
            else
            {
                lblEngineGenerator.Text = "NA";
                lblEmergencyFirePump.Text = "NA";
                lblEmergencyAirComp.Text = "NA";
                lblERFireDetectors1.Text = "NA";
                lblERFireDetectors2.Text = "NA";
                lblQuickClosingValves.Text = "NA";
                lblEmergencyTrips.Text = "NA";
                lblCO2Alarm.Text = "NA";
                lblCo2PressureAlarm.Text = "NA";
                lblERFlapsSkyLights.Text = "NA";
                lblEmergencySteeringTest.Text = "NA";
                lblMEEmergencyControls.Text = "NA";
                lblBoilerSafetyCutOut.Text = "NA";

                ////right section
                lblLifeBoatEngine.Text = "NA";
                lblLifeBoatDavitSafeties.Text = "NA";
                lblFireDoorsWTDoorsClosing.Text = "NA";
                lblCargoCraneSafeties.Text = "NA";
                lblProvisionCraneSafeties.Text = "NA";
                lblAccomLadderSafeties.Text = "NA";
                lblAEShutdownSafeties1.Text = "NA";
                lblAEShutdownSafeties2.Text = "NA";
                lblAEShutdownSafeties3.Text = "NA";
                lblMEShutdownSafeties.Text = "NA";
                lblERBilgeAlarams.Text = "NA";

                lblOtherBilgeAlarm1.Text = "NA";
                lblOtherBilgeAlarm2.Text = "NA";
                lblOtherBilgeAlarm3.Text = "NA";
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

    }

    protected void gvMaintaincePosition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gvMaintaincePosition.DataSource = PhoenixEngineLog.MonthlyLogDetailSearch(usercode, vesselId, currentDate.Month, currentDate.Year, "MAINTENANCEPOSISTION");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEngineLOAnalysis_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gvEngineLOAnalysis.DataSource = PhoenixEngineLog.LoAnalysiSearch(usercode, vesselId, currentDate.Month, currentDate.Year);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTurboChargers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gvTurboChargers.DataSource = PhoenixEngineLog.MonthlyLogDetailSearch(usercode, vesselId, currentDate.Month, currentDate.Year, "TURBOCHARGER");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvAuxillaryEngine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gvAuxillaryEngine.DataSource = PhoenixEngineLog.MonthlyLogDetailSearch(usercode, vesselId, currentDate.Month, currentDate.Year, "AUXILLARYENGINE");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAirCompressor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            gvAirCompressor.DataSource = PhoenixEngineLog.MonthlyLogDetailSearch(usercode, vesselId, currentDate.Month, currentDate.Year, "AIRCOMPRESSOR");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdWrapper_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyWrapper();
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdMainteince_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyMaintenance(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdEngineDepartment_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyEngineDepartment(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdLO_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyLO(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdEmergencySafety_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyEmergencySafety(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdTurbo_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyTurboCharge(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdAuxillary_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyAuxillary(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdAirCompressor_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyAirCompressor(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdEngineUnit_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyUnit(vesselId, usercode);
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmEngineUnitWrapper_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixEngineLog.EngineLogMonthlyUnitWrapper();
            ucMessage.HeaderMessage = "Done";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
