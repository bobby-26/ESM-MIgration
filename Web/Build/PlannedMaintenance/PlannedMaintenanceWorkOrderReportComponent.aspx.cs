using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderReportComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            //toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx", "Refresh", "refresh.png", "REFRESH");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportComponent.aspx", "Refresh", "<i class=\"fas fa-sync\"></i>", "REFRESH");
            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
          
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["WORKORDERID"] = null;
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                ViewState["PAGENUMBERWORKORDER"] = 1;
                ViewState["SORTEXPRESSIONWORKORDER"] = null;
                ViewState["SORTDIRECTIONWORKORDER"] = null;
                gvHistoryTemplate.PageSize = General.ShowRecords(null);
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

            if (CommandName.ToUpper().Equals("REFRESH"))
			{
                gvWorkReport.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
       
        DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.WorkOrderHistoryTemplate(new Guid(ViewState["WORKORDERID"].ToString())
                                                    , gvHistoryTemplate.CurrentPageIndex + 1
                                                    , gvHistoryTemplate.PageSize
                                                    , ref iRowCount, ref iTotalPageCount);
        gvHistoryTemplate.DataSource = dt;
        gvHistoryTemplate.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void BindDataWorkOrder()
    {
        int iRowCountWorkOrder = 0;
        int iTotalPageCountWorkOrder = 0;
        string sortexpression = (ViewState["SORTEXPRESSIONWORKORDER"] == null) ? null : (ViewState["SORTEXPRESSIONWORKORDER"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONWORKORDER"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONWORKORDER"].ToString());
        try
        {

            DataSet ds = PhoenixPlannedMaintenanceHistoryTemplate.MaintenancelogWorkReportSearch(new Guid(ViewState["WORKORDERID"].ToString())
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvWorkReport.CurrentPageIndex + 1
                                                                                , gvWorkReport.PageSize
                                                                                , ref iRowCountWorkOrder
                                                                                , ref iTotalPageCountWorkOrder);


            gvWorkReport.DataSource = ds;
            gvWorkReport.VirtualItemCount = iRowCountWorkOrder;
            ViewState["ROWCOUNT"] = iRowCountWorkOrder;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvWorkReport_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
            {
                return;
            }
            
            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {                
                int formtype = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblFormtype")).Text);
                Guid formid = new Guid(((RadLabel)e.Item.FindControl("lblFormId")).Text);
                Guid Reportid = new Guid(((RadLabel)e.Item.FindControl("lblReportId")).Text);
                Guid Componentid = new Guid(((RadLabel)e.Item.FindControl("lblComponentIdGV2")).Text);
                Guid WorkOrderid = new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderIdGV2")).Text);


                if (formtype == 1)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 2)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReportEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 3)
                {
                    PhoenixPMS2XL.Export2MECylinderCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 4)
                {
                    PhoenixPMS2XL.Export2XLMainenginePisonCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 5)
                {
                    PhoenixPMS2XL.Export2XLMainEngineStuffingBoxReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 6)
                {
                    PhoenixPMS2XL.Export2XLAuxiliaryEngineReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 7)
                {
                    PhoenixPMS2XL.Export2XLAERodInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 8)
                {
                    PhoenixPMS2XL.Export2XLAEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 9)
                {
                    PhoenixPMS2XL.Export2XLMEInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 10)
                {
                    PhoenixPMS2XL.Export2XLAEDecarbonisationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 11)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 12)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceEOPLReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 13)
                {
                    PhoenixPMS2XL.Export2XLCargoTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 14)
                {
                    PhoenixPMS2XL.Export2XLBallastTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 15)
                {
                    PhoenixPMS2XL.Export2XlRHTurbochargerReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 16)
                {
                    PhoenixPMS2XL.Export2XlTankerValvesMaintenanceRecord(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);

                }
                else if (formtype == 17)
                {
                    PhoenixPMS2XL.Export2XlCASPACSystemLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);

                }
                else if (formtype == 18)
                {
                    PhoenixPMS2XL.Export2XlMainEnginePistonRingGapMeasurementReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);

                }
                else if (formtype == 19)
                {

                    PhoenixPMS2XL.Export2XlValveGreasingandMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 20)
                {

                    PhoenixPMS2XL.Export2XlMainEngineExhaustValveOverhaulReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 21)
                {
                    PhoenixPMS2XL.Export2XLOzoneDepletingSubstanceRecordEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 22)
                {
                    PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 23)
                {
                    PhoenixPMS2XL.Export2XlVibrationMonitoringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);

                }
                else if (formtype == 24)
                {
                    PhoenixPMS2XL.Export2XLPOTableWaterTestRecordReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 26)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportUEC(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 27)
                {
                    PhoenixPMS2XL.Export2XLCargoTankPassivityTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 29)
                {
                    PhoenixPMS2XL.Export2XLGasTankerMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 30)
                {
                    PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 31)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportSULZER(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 32)
                {
                    PhoenixPMS2XL.Export2XLHoldConditionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 33)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportMANBW(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 34)
                {
                    PhoenixPMS2XL.Export2XLCrankwebsDeflectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 35)
                {
                    PhoenixPMS2XL.Export2XLBearingMeasuringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 36)
                {
                    PhoenixPMS2XL.Export2XLWeeklySafetyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 37)
                {
                    PhoenixPMS2XL.Export2XLSimpleJobsReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 38)
                {
                    PhoenixPMS2XL.Export2XLMonthlyMEperformance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 39)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport2(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 40)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport3(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 41)
                {
                    PhoenixPMS2XL.Export2XLPaintStock(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, WorkOrderid, formtype);
                }
                else if (formtype == 42)
                {
                    PhoenixPMS2XL.Export2XLHoldInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }
                else if (formtype == 43)
                {
                    PhoenixPMS2XL.Export2XLPeriodicalCheckListReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reportid, PhoenixSecurityContext.CurrentSecurityContext.VesselID, Componentid, WorkOrderid, formtype);
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
      
    }

    protected void gvWorkReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataWorkOrder();
    }
    protected void gvWorkReport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        ImageButton dwn = (ImageButton)e.Item.FindControl("cmdDownload");
        if (dwn != null)
        {
            dwn.Visible = SessionUtil.CanAccess(this.ViewState, dwn.CommandName);
        }
        LinkButton excel = (LinkButton)e.Item.FindControl("cmdExcel");
        if (excel != null)
        {
            excel.Visible = SessionUtil.CanAccess(this.ViewState, excel.CommandName);
            excel.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDoneView.aspx?rid=" + drv["FLDREPORTID"].ToString() + "&wid=" + ViewState["WORKORDERID"].ToString() + "&frm=" + Request.QueryString["frm"] + "'); return false;");
        }
        if(dwn != null && excel != null)
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

    protected void gvHistoryTemplate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EXCEL"))
            {
                RadLabel type = ((RadLabel)e.Item.FindControl("lblFormType"));
                Guid Componentid = new Guid(((RadLabel)e.Item.FindControl("lblComponentId")).Text);
                Guid WorkOrderid = new Guid(((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text);


                if (type.Text == "1")
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , Componentid
                                                                            , WorkOrderid
                                                                            , Convert.ToInt32(type.Text));

                }
                else if (type.Text == "2")
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReportEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , Componentid
                                                                            , WorkOrderid
                                                                            , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "3")
                {
                    PhoenixPMS2XL.Export2MECylinderCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , Componentid
                                                                            , WorkOrderid
                                                                            , Convert.ToInt32(type.Text));
                }

                else if (type.Text == "4")
                {
                    PhoenixPMS2XL.Export2XLMainenginePisonCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , Componentid
                                                                            , WorkOrderid
                                                                            , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "5")
                {
                    PhoenixPMS2XL.Export2XLMainEngineStuffingBoxReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , Componentid
                                                                            , WorkOrderid
                                                                            , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "6")
                {
                    PhoenixPMS2XL.Export2XLAuxiliaryEngineReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , Componentid
                                                                            , WorkOrderid
                                                                            , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "7")
                {
                    PhoenixPMS2XL.Export2XLAERodInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , null
                                                                         , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                         , Componentid
                                                                         , WorkOrderid
                                                                         , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "8")
                {
                    PhoenixPMS2XL.Export2XLAEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , null
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , Componentid
                                                                          , WorkOrderid
                                                                          , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "9")
                {
                    PhoenixPMS2XL.Export2XLMEInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , null
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , Componentid
                                                                          , WorkOrderid
                                                                          , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "10")
                {
                    PhoenixPMS2XL.Export2XLAEDecarbonisationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , null
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , Componentid
                                                                          , WorkOrderid
                                                                          , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "11")
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , null
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , Componentid
                                                                          , WorkOrderid
                                                                          , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "12")
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceEOPLReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , null
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , Componentid
                                                                          , WorkOrderid
                                                                          , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "13")
                {
                    PhoenixPMS2XL.Export2XLCargoTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , null
                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                        , Componentid
                                                                        , WorkOrderid
                                                                        , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "14")
                {
                    PhoenixPMS2XL.Export2XLBallastTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , null
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , Componentid
                                                                          , WorkOrderid
                                                                          , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "15")
                {
                    PhoenixPMS2XL.Export2XlRHTurbochargerReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , null
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                    , Componentid
                                                                    , WorkOrderid
                                                                    , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "16")
                {
                    PhoenixPMS2XL.Export2XlTankerValvesMaintenanceRecord(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , null
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , Componentid
                                                                            , WorkOrderid
                                                                            , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "17")
                {
                    PhoenixPMS2XL.Export2XlCASPACSystemLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , null
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                    , Componentid
                                                                    , WorkOrderid
                                                                    , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "18")
                {
                    PhoenixPMS2XL.Export2XlMainEnginePistonRingGapMeasurementReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , null
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , Componentid
                                                                                        , WorkOrderid
                                                                                        , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "19")
                {
                    PhoenixPMS2XL.Export2XlValveGreasingandMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , null
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                 , Componentid
                                                                                 , WorkOrderid
                                                                                 , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "20")
                {
                    PhoenixPMS2XL.Export2XlMainEngineExhaustValveOverhaulReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(null)
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt32(type.Text));
                }
                else if (type.Text == "21")
                {
                    PhoenixPMS2XL.Export2XLOzoneDepletingSubstanceRecordEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "22")
                {
                    PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "23")
                {
                    PhoenixPMS2XL.Export2XlVibrationMonitoringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null//Reportid.ToString()
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));

                }
                else if (type.Text == "24")
                {
                    PhoenixPMS2XL.Export2XLPOTableWaterTestRecordReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "25")
                {
                    PhoenixPMS2XL.Export2XLTankInspectionPhotoTemplate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "26")
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportUEC(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }

                else if (type.Text == "27")
                {
                    PhoenixPMS2XL.Export2XLCargoTankPassivityTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "28")
                {
                    PhoenixPMS2XL.Export2XLScavengePhotoTemplate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "29")
                {
                    PhoenixPMS2XL.Export2XLGasTankerMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "30")
                {
                    PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "31")
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportSULZER(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "32")
                {
                    PhoenixPMS2XL.Export2XLHoldConditionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "33")
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportMANBW(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "34")
                {
                    PhoenixPMS2XL.Export2XLCrankwebsDeflectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "35")
                {
                    PhoenixPMS2XL.Export2XLBearingMeasuringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "36")
                {
                    PhoenixPMS2XL.Export2XLWeeklySafetyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "37")
                {
                    PhoenixPMS2XL.Export2XLSimpleJobsReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "38")
                {
                    PhoenixPMS2XL.Export2XLMonthlyMEperformance(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "39")
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport2(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "40")
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport3(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , Componentid
                                                                                , WorkOrderid
                                                                                , Convert.ToInt16(type.Text));
                }
                else if (type.Text == "41")
                {
                    PhoenixPMS2XL.Export2XLPaintStock(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , null
                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , WorkOrderid
                                                            , Convert.ToInt16(type.Text));
                }

                else if (type.Text == "42")
                {
                    PhoenixPMS2XL.Export2XLHoldInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , null
                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , Componentid
                                                            , WorkOrderid
                                                            , Convert.ToInt16(type.Text));
                }

                else if (type.Text == "43")
                {
                    PhoenixPMS2XL.Export2XLPeriodicalCheckListReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , null
                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , Componentid
                                                            , WorkOrderid
                                                            , Convert.ToInt16(type.Text));
                }

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvHistoryTemplate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvHistoryTemplate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        LinkButton excel = (LinkButton)e.Item.FindControl("cmdExcel");       
        if (excel != null)
        {
            excel.Visible = SessionUtil.CanAccess(this.ViewState, excel.CommandName);
            excel.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDone.aspx?fid=" + drv["FLDFORMID"].ToString() + "&wid="+ViewState["WORKORDERID"].ToString()+ "&frm=" + Request.QueryString["frm"] + "'); return false;");
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvWorkReport.Rebind();
        string script = "refreshParent();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
}
