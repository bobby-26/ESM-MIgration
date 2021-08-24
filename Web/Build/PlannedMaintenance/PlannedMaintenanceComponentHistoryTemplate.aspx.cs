using System;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentHistoryTemplate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentHistoryTemplate.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentHistoryTemplate.aspx", "Clear Filter", " <i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuComponentHistoryTemplateReports.AccessRights = this.ViewState;
            MenuComponentHistoryTemplateReports.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTID"] = null;

                ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();

                ucHistory.bind();
                gvComponentHistoryTemplateReports.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindDataReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentHistoryTemplateReports_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvComponentHistoryTemplateReports.CurrentPageIndex = 0;
                //gvComponentHistoryTemplateReports.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //gvComponentHistoryTemplateReports.CurrentPageIndex = 0;
                ucHistory.SelectedHistoryTemplate = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ucVessel.SelectedVessel = "";
            }
            gvComponentHistoryTemplateReports.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindDataReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        string vesselid;
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                vesselrow.Visible = true;
                ucVessel.Visible = true;
                vesselid = ucVessel.SelectedVessel;
            }
            else
            {
                vesselrow.Visible = false;
                ucVessel.Visible = false;
                vesselid = (PhoenixSecurityContext.CurrentSecurityContext.VesselID).ToString();
            }

            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.PMSComponentHistoryTemplateReportSearch(General.GetNullableDateTime(txtFromDate.Text)
                                                                                                            , General.GetNullableDateTime(txtToDate.Text)
                                                                                                            , General.GetNullableGuid(ucHistory.SelectedHistoryTemplate)
                                                                                                            , General.GetNullableInteger(vesselid)
                                                                                                            , General.GetNullableGuid(ViewState["COMPONENTID"].ToString())
                                                                                                            , sortexpression
                                                                                                            , sortdirection
                                                                                                            , gvComponentHistoryTemplateReports.CurrentPageIndex + 1
                                                                                                            , gvComponentHistoryTemplateReports.PageSize
                                                                                                            , ref iRowCount
                                                                                                            , ref iTotalPageCount);
            gvComponentHistoryTemplateReports.DataSource = dt;
            gvComponentHistoryTemplateReports.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvComponentHistoryTemplateReports_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                int formtype = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblFormtype")).Text);
                Guid formid = new Guid(((RadLabel)e.Item.FindControl("lblFormId")).Text);
                Guid Reportid = new Guid(((RadLabel)e.Item.FindControl("lblReportId")).Text);
                int Vesselid = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblVesselid")).Text);

                if (formtype == 1)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableGuid(Reportid.ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                            , null
                                                                            , formtype);

                }
                else if (formtype == 2)
                {
                    PhoenixPMS2XL.Export2XLMEMaintenanceReportEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableGuid(Reportid.ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                            , null
                                                                            , formtype);
                }
                else if (formtype == 3)
                {
                    PhoenixPMS2XL.Export2MECylinderCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                            , null
                                                                            , formtype);
                }

                else if (formtype == 4)
                {
                    PhoenixPMS2XL.Export2XLMainenginePisonCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                            , null
                                                                            , formtype);
                }
                else if (formtype == 5)
                {
                    PhoenixPMS2XL.Export2XLMainEngineStuffingBoxReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                            , null
                                                                            , formtype);
                }
                else if (formtype == 6)
                {
                    PhoenixPMS2XL.Export2XLAuxiliaryEngineReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                            , null
                                                                            , formtype);
                }
                else if (formtype == 7)
                {
                    PhoenixPMS2XL.Export2XLAERodInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , General.GetNullableGuid(Reportid.ToString())
                                                                         , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                         , null
                                                                         , null
                                                                         , formtype);
                }
                else if (formtype == 8)
                {
                    PhoenixPMS2XL.Export2XLAEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , General.GetNullableGuid(Reportid.ToString())
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , null
                                                                          , null
                                                                          , formtype);
                }
                else if (formtype == 9)
                {
                    PhoenixPMS2XL.Export2XLMEInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , General.GetNullableGuid(Reportid.ToString())
                                                                          , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                          , null
                                                                          , null
                                                                          , formtype);
                }
                else if (formtype == 10)
                {
                    PhoenixPMS2XL.Export2XLAEDecarbonisationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                           , null
                                                                           , null
                                                                           , formtype);
                }
                else if (formtype == 11)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                           , null
                                                                           , null
                                                                           , formtype);
                }
                else if (formtype == 12)
                {
                    PhoenixPMS2XL.Export2XLMEPerformanceEOPLReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                           , null
                                                                           , null
                                                                           , formtype);
                }
                else if (formtype == 13)
                {
                    PhoenixPMS2XL.Export2XLCargoTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                           , null
                                                                           , null
                                                                           , formtype);
                }
                else if (formtype == 14)
                {
                    PhoenixPMS2XL.Export2XLBallastTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , General.GetNullableGuid(Reportid.ToString())
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                           , null
                                                                           , null
                                                                           , formtype);
                }
                else if (formtype == 15)
                {
                    PhoenixPMS2XL.Export2XlRHTurbochargerReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                    , null
                                                                    , null
                                                                    , formtype);
                }
                else if (formtype == 16)
                {
                    PhoenixPMS2XL.Export2XlTankerValvesMaintenanceRecord(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableGuid(Reportid.ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                           , null
                                                                            , formtype);
                }
                else if (formtype == 17)
                {
                    PhoenixPMS2XL.Export2XlCASPACSystemLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , General.GetNullableGuid(Reportid.ToString())
                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                        , null
                                                                        , null
                                                                        , formtype);
                }
                else if (formtype == 18)
                {
                    PhoenixPMS2XL.Export2XlMainEnginePistonRingGapMeasurementReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , General.GetNullableGuid(Reportid.ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , null
                                                                                        , null
                                                                                        , formtype);
                }
                else if (formtype == 19)
                {
                    PhoenixPMS2XL.Export2XlValveGreasingandMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , General.GetNullableGuid(Reportid.ToString())
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                 , null
                                                                                 , null
                                                                                 , formtype);
                }
                else if (formtype == 20)
                {
                    PhoenixPMS2XL.Export2XlMainEngineExhaustValveOverhaulReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , General.GetNullableGuid(Reportid.ToString())
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                 , null
                                                                                 , null
                                                                                 , formtype);
                }
                else if (formtype == 21)
                {
                    PhoenixPMS2XL.Export2XLOzoneDepletingSubstanceRecordEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , General.GetNullableGuid(Reportid.ToString())
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                 , null
                                                                                 , null
                                                                                 , formtype);
                }
                else if (formtype == 22)
                {
                    PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableGuid(Reportid.ToString())
                                                             , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                             , null
                                                             , null
                                                             , formtype);
                }
                else if (formtype == 23)
                {
                    PhoenixPMS2XL.Export2XlVibrationMonitoringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableGuid(Reportid.ToString())
                                                             , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                             , null
                                                             , null
                                                             , formtype);

                }
                else if (formtype == 24)
                {
                    PhoenixPMS2XL.Export2XLPOTableWaterTestRecordReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                 , General.GetNullableGuid(Reportid.ToString())
                                                                                 , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                 , null
                                                                                 , null
                                                                                 , formtype);
                }
                else if (formtype == 26)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportUEC(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }

                else if (formtype == 27)
                {
                    PhoenixPMS2XL.Export2XLCargoTankPassivityTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 29)
                {
                    PhoenixPMS2XL.Export2XLGasTankerMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 30)
                {
                    PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 31)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportSULZER(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 32)
                {
                    PhoenixPMS2XL.Export2XLHoldConditionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 33)
                {
                    PhoenixPMS2XL.Export2XLScavengeInspectionReportMANBW(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 34)
                {
                    PhoenixPMS2XL.Export2XLCrankwebsDeflectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 35)
                {
                    PhoenixPMS2XL.Export2XLBearingMeasuringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 36)
                {
                    PhoenixPMS2XL.Export2XLWeeklySafetyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 37)
                {
                    PhoenixPMS2XL.Export2XLSimpleJobsReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 38)
                {
                    PhoenixPMS2XL.Export2XLMonthlyMEperformance(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 39)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport2(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }
                else if (formtype == 40)
                {
                    PhoenixPMS2XL.Export2XLAEPerformanceReport3(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , null
                                                                                , null
                                                                                , formtype);
                }

                else if (formtype == 41)
                {
                    PhoenixPMS2XL.Export2XLPaintStock(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(Reportid.ToString())
                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , null
                                                            , formtype);
                }
                else if (formtype == 42)
                {
                    PhoenixPMS2XL.Export2XLHoldInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(Reportid.ToString())
                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , null
                                                            , null
                                                            , formtype);
                }
                else if (formtype == 43)
                {
                    PhoenixPMS2XL.Export2XLPeriodicalCheckListReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(Reportid.ToString())
                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                            , null
                                                            , null
                                                            , formtype);
                }
            }
            BindDataReport();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvComponentHistoryTemplateReports_PreRender(object sender, EventArgs e)
    {
       
    }

    protected void gvComponentHistoryTemplateReports_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataReport();
    }
    protected void gvComponentHistoryTemplateReports_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

}
