using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.StandardForm;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportLogHistoryTemplate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBERWORKORDER"] = 1;
            ViewState["SORTEXPRESSIONWORKORDER"] = null;
            ViewState["SORTDIRECTIONWORKORDER"] = null;
            RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["VESSELID"] = "0";
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        //BindDataWorkOrder();
    }
    protected void BindDataWorkOrder()
    {
        int iRowCountWorkOrder = 0;
        int iTotalPageCountWorkOrder = 0;
        string sortexpression = (ViewState["SORTEXPRESSIONWORKORDER"] == null) ? null : (ViewState["SORTEXPRESSIONWORKORDER"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONWORKORDER"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONWORKORDER"].ToString());
        ViewState["WORKORDERID"]=  Request.QueryString["WORKORDERID"].ToString();
        try
        {
            DataSet ds;
            if (Request.QueryString["vesselid"] != null)
            {
                ds = PhoenixPlannedMaintenanceHistoryTemplate.MaintenancelogWorkReportSearch(new Guid(ViewState["WORKORDERID"].ToString()), sortexpression, sortdirection
                                                                                , RadGrid1.CurrentPageIndex + 1
                                                                                , RadGrid1.PageSize
                                                                                , ref iRowCountWorkOrder
                                                                                , ref iTotalPageCountWorkOrder
                                                                                , int.Parse(ViewState["VESSELID"].ToString()));
            }
            else
            {
                ds = PhoenixPlannedMaintenanceHistoryTemplate.MaintenancelogWorkReportSearch(new Guid(ViewState["WORKORDERID"].ToString()), sortexpression, sortdirection
                                                                                    , RadGrid1.CurrentPageIndex + 1
                                                                                    , RadGrid1.PageSize
                                                                                    , ref iRowCountWorkOrder
                                                                                    , ref iTotalPageCountWorkOrder);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                RadGrid1.DataSource = ds;
                RadGrid1.VirtualItemCount = iRowCountWorkOrder;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                RadGrid1.DataSource = "";
            }
            ViewState["ROWCOUNTWORKORDER"] = iRowCountWorkOrder;
            ViewState["TOTALPAGECOUNTWORKORDER"] = iTotalPageCountWorkOrder;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    //private void SetPageNavigatorWorkOrder()
    //{
    //    cmdPreviousWorkOrder.Enabled = IsPreviousEnabledWorkOrder();
    //    cmdNextWorkOrder.Enabled = IsNextEnabledWorkOrder();
    //    lblPagenumberWorkOrder.Text = "Page " + ViewState["PAGENUMBERWORKORDER"].ToString();
    //    lblPagesWorkOrder.Text = " of " + ViewState["TOTALPAGECOUNTWORKORDER"].ToString() + " Pages. ";
    //    lblRecordsWorkOrder.Text = "(" + ViewState["ROWCOUNTWORKORDER"].ToString() + " records found)";
    //}

    protected void gvWorkReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
            {
                return;
            }
            GridView _gridViewWorkReport = (GridView)sender;
            int nCurrentRow = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                int formtype = Convert.ToInt32(((Label)_gridViewWorkReport.Rows[nCurrentRow].FindControl("lblFormtype")).Text);
                Guid formid = new Guid(((Label)_gridViewWorkReport.Rows[nCurrentRow].FindControl("lblFormId")).Text);
                Guid Reportid = new Guid(((Label)_gridViewWorkReport.Rows[nCurrentRow].FindControl("lblReportId")).Text);
                //int Vesselid = Convert.ToInt32(((Label)_gridViewWorkReport.Rows[nCurrentRow].FindControl("lblVesselid")).Text);
                Guid Componentid = new Guid(((Label)_gridViewWorkReport.Rows[nCurrentRow].FindControl("lblComponentIdGV2")).Text);
                Guid WorkOrderid = new Guid(((Label)_gridViewWorkReport.Rows[nCurrentRow].FindControl("lblWorkOrderIdGV2")).Text);
                int vesselid = int.Parse(ViewState["VESSELID"].ToString());
                if (formtype == 8)
                {
                    PhoenixPMS2XL.Export2XLAEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 3)
                {
                    PhoenixPMS2XL.Export2MECylinderCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 1)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 2)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReportEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 6)
                {
                    PhoenixPMS2XL.Export2XLAuxiliaryEngineReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 7)
                {
                    PhoenixPMS2XL.Export2XLAERodInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 4)
                {
                    PhoenixPMS2XL.Export2XLMainenginePisonCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 5)
                {
                    PhoenixPMS2XL.Export2XLMainEngineStuffingBoxReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 9)
                {
                    PhoenixPMS2XL.Export2XLMEInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 10)
                {
                    PhoenixPMS2XL.Export2XLAEDecarbonisationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 11)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 12)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceEOPLReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 13)
                {
                    PhoenixPMS2XL.Export2XLCargoTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 14)
                {
                    PhoenixPMS2XL.Export2XLBallastTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 15)
                {
                    PhoenixPMS2XL.Export2XlRHTurbochargerReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 16)
                {
                    PhoenixPMS2XL.Export2XlTankerValvesMaintenanceRecord(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 17)
                {
                    PhoenixPMS2XL.Export2XlCASPACSystemLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 18)
                {
                    PhoenixPMS2XL.Export2XlMainEnginePistonRingGapMeasurementReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 19)
                {
                    PhoenixPMS2XL.Export2XlValveGreasingandMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 20)
                {
                    PhoenixPMS2XL.Export2XlMainEngineExhaustValveOverhaulReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 21)
                {
                    PhoenixPMS2XL.Export2XLOzoneDepletingSubstanceRecordEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 22)
                {
                    PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 23)
                {
                    PhoenixPMS2XL.Export2XlVibrationMonitoringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);

                }
                else if (formtype == 24)
                {
                    PhoenixPMS2XL.Export2XLPOTableWaterTestRecordReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 26)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportUEC(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 27)
                {
                    PhoenixPMS2XL.Export2XLCargoTankPassivityTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 29)
                {
                    PhoenixPMS2XL.Export2XLGasTankerMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 30)
                {
                    PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 31)
                {
                    PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 32)
                {
                    PhoenixPMS2XL.Export2XLHoldConditionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 33)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportMANBW(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 34)
                {
                    PhoenixPMS2XL.Export2XLCrankwebsDeflectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 35)
                {
                    PhoenixPMS2XL.Export2XLBearingMeasuringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 36)
                {
                    PhoenixPMS2XL.Export2XLWeeklySafetyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 37)
                {
                    PhoenixPMS2XL.Export2XLSimpleJobsReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 38)
                {
                    PhoenixPMS2XL.Export2XLMonthlyMEperformance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 39)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport2(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 40)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport3(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 41)
                {
                    PhoenixPMS2XL.Export2XLPaintStock(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid,  WorkOrderid, formtype);
                }
                else if (formtype == 42)
                {
                    PhoenixPMS2XL.Export2XLHoldInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 43)
                {
                    PhoenixPMS2XL.Export2XLPeriodicalCheckListReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    /////
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
            {
                return;
            }
            //GridView _gridViewWorkReport = (GridView)sender;
            //int nCurrentRow = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                int formtype = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblFormtype")).Text);
                Guid formid = new Guid(((RadLabel)e.Item.FindControl("lblFormId")).Text);
                Guid Reportid = new Guid(((RadLabel)e.Item.FindControl("lblReportId")).Text);
                //int Vesselid = Convert.ToInt32(((Label)_gridViewWorkReport.Rows[nCurrentRow].FindControl("lblVesselid")).Text);
                Guid Componentid = new Guid(((RadLabel)e.Item.FindControl("lblComponentIdGV2")).Text);
                Guid WorkOrderid = new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderIdGV2")).Text);
                int vesselid = int.Parse(ViewState["VESSELID"].ToString());
                if (formtype == 8)
                {
                    PhoenixPMS2XL.Export2XLAEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 3)
                {
                    PhoenixPMS2XL.Export2MECylinderCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 1)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 2)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReportEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 6)
                {
                    PhoenixPMS2XL.Export2XLAuxiliaryEngineReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 7)
                {
                    PhoenixPMS2XL.Export2XLAERodInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 4)
                {
                    PhoenixPMS2XL.Export2XLMainenginePisonCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 5)
                {
                    PhoenixPMS2XL.Export2XLMainEngineStuffingBoxReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 9)
                {
                    PhoenixPMS2XL.Export2XLMEInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 10)
                {
                    PhoenixPMS2XL.Export2XLAEDecarbonisationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 11)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 12)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceEOPLReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 13)
                {
                    PhoenixPMS2XL.Export2XLCargoTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 14)
                {
                    PhoenixPMS2XL.Export2XLBallastTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 15)
                {
                    PhoenixPMS2XL.Export2XlRHTurbochargerReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 16)
                {
                    PhoenixPMS2XL.Export2XlTankerValvesMaintenanceRecord(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 17)
                {
                    PhoenixPMS2XL.Export2XlCASPACSystemLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 18)
                {
                    PhoenixPMS2XL.Export2XlMainEnginePistonRingGapMeasurementReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 19)
                {
                    PhoenixPMS2XL.Export2XlValveGreasingandMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 20)
                {
                    PhoenixPMS2XL.Export2XlMainEngineExhaustValveOverhaulReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 21)
                {
                    PhoenixPMS2XL.Export2XLOzoneDepletingSubstanceRecordEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 22)
                {
                    PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 23)
                {
                    PhoenixPMS2XL.Export2XlVibrationMonitoringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);

                }
                else if (formtype == 24)
                {
                    PhoenixPMS2XL.Export2XLPOTableWaterTestRecordReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 26)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportUEC(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 27)
                {
                    PhoenixPMS2XL.Export2XLCargoTankPassivityTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 29)
                {
                    PhoenixPMS2XL.Export2XLGasTankerMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 30)
                {
                    PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 31)
                {
                    PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 32)
                {
                    PhoenixPMS2XL.Export2XLHoldConditionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 33)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportMANBW(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 34)
                {
                    PhoenixPMS2XL.Export2XLCrankwebsDeflectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 35)
                {
                    PhoenixPMS2XL.Export2XLBearingMeasuringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 36)
                {
                    PhoenixPMS2XL.Export2XLWeeklySafetyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 37)
                {
                    PhoenixPMS2XL.Export2XLSimpleJobsReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 38)
                {
                    PhoenixPMS2XL.Export2XLMonthlyMEperformance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 39)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport2(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 40)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport3(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 41)
                {
                    PhoenixPMS2XL.Export2XLPaintStock(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, WorkOrderid, formtype);
                }
                else if (formtype == 42)
                {
                    PhoenixPMS2XL.Export2XLHoldInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 43)
                {
                    PhoenixPMS2XL.Export2XLPeriodicalCheckListReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, vesselid, Componentid, WorkOrderid, formtype);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataWorkOrder();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        RadImageButton dwn = (RadImageButton)e.Item.FindControl("cmdDownload");
        if (dwn != null)
        {
            dwn.Visible = SessionUtil.CanAccess(this.ViewState, dwn.CommandName);
        }
        LinkButton excel = (LinkButton)e.Item.FindControl("cmdExcel");
        if (excel != null)
        {
            excel.Visible = SessionUtil.CanAccess(this.ViewState, excel.CommandName);
            excel.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDoneView.aspx?rid=" + drv["FLDREPORTID"].ToString() + "&wid=" + ViewState["WORKORDERID"].ToString() + "&vessselid="+ ViewState["VESSELID"] + "'); return false;");
        }
        if (dwn != null && excel != null)
        {
            if (drv["FLDEXCELJSONREPORT"].ToString() != string.Empty)
            {
                dwn.Visible = false;
            }
            else
            {
                excel.Visible = false;
            }
        }
    }
}
