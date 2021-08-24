using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;

public partial class PlannedMaintenanceHistoryTemplateReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Template", "TEMPLATE", ToolBarDirection.Right);

            MenuHistoryTemplateReports.AccessRights = this.ViewState;
            MenuHistoryTemplateReports.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateReports.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateReports.aspx", "Clear Filter", " <i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuGrid.AccessRights = this.ViewState;
            MenuGrid.MenuList = toolbar1.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["FORMID"] = string.Empty;
                ViewState["FORMNAME"] = string.Empty;
                if(Request.QueryString["FORMID"] != null)
                    ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                if (Request.QueryString["FORMNAME"] != null)
                    ViewState["FORMNAME"] = Request.QueryString["FORMNAME"].ToString();

                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            MenuGrid.Title = ViewState["FORMNAME"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuHistoryTemplateReports_TabStripCommand(object sender, EventArgs e)

    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper() == "TEMPLATE")
        {
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceHistoryTemplate.aspx");
        }
    }

    protected void Rebind()
    {
        RadGrid1.SelectedIndexes.Clear();
        RadGrid1.EditIndexes.Clear();
        RadGrid1.DataSource = null;
        RadGrid1.Rebind();
    }

    protected void BindDataReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.PMSHistoryTemplateReportSearch(General.GetNullableDateTime(txtFromDate.Text)
                                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                                , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , RadGrid1.CurrentPageIndex + 1
                                                                                , RadGrid1.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);
            RadGrid1.DataSource = dt;
            RadGrid1.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            RadGrid1.DataSource = "";
        }
    }
    protected void gvHistoryTemplateReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label Activeyn = ((Label)e.Row.FindControl("lblActiveyn"));
                Label reportid = ((Label)e.Row.FindControl("lblReportId"));
                ImageButton imgbtn = (ImageButton)e.Row.FindControl("CmdMapping");
                ImageButton imgbtnyn = (ImageButton)e.Row.FindControl("CmdMapped");
                imgbtn.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','PlannedMaintenanceWorkOrderList.aspx?ReportId=" + (drv["FLDREPORTID"].ToString()) + "&Formid=" + (drv["FLDFORMID"].ToString()) + "&Workorderid= " + (null) + "'); return false;");
                if (drv["FLDACTIVEYN"].ToString() == "1")
                {
                    imgbtnyn.Visible = true;
                }
                else
                {
                    imgbtnyn.Visible = false;
                }
                Label lbtn = (Label)e.Row.FindControl("lblcomponentno");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("uclblcomponentname");
                if (lbtn != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }

            }
        }

    }


    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName != "RebindGrid")
            {
                int nCurrentRow = e.Item.RowIndex;
                int formtype = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblFormtype")).Text);
                Guid formid = new Guid(((RadLabel)e.Item.FindControl("lblFormId")).Text);
                Guid Reportid = new Guid(((RadLabel)e.Item.FindControl("lblReportId")).Text);

                if (e.CommandName.ToUpper().Equals("MAPPEDREPORT"))
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderMappingList.aspx?REPORTID=" + Reportid + "&FORMID=" + formid, true);
                }

                if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
                {

                    if (formtype == 1)
                    {
                        PhoenixPMS2XL.Export2XLMEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);

                    }
                    else if (formtype == 2)
                    {
                        PhoenixPMS2XL.Export2XLMEMaintenanceReportEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 3)
                    {
                        PhoenixPMS2XL.Export2MECylinderCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }

                    else if (formtype == 4)
                    {
                        PhoenixPMS2XL.Export2XLMainenginePisonCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 5)
                    {
                        PhoenixPMS2XL.Export2XLMainEngineStuffingBoxReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 6)
                    {
                        PhoenixPMS2XL.Export2XLAuxiliaryEngineReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 7)
                    {
                        PhoenixPMS2XL.Export2XLAERodInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                             , General.GetNullableGuid(Reportid.ToString())
                                                                             , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                             , General.GetNullableGuid(null)
                                                                             , General.GetNullableGuid(null)
                                                                             , formtype);
                    }
                    else if (formtype == 8)
                    {
                        PhoenixPMS2XL.Export2XLAEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , General.GetNullableGuid(Reportid.ToString())
                                                                              , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                              , General.GetNullableGuid(null)
                                                                              , General.GetNullableGuid(null)
                                                                              , formtype);
                    }
                    else if (formtype == 9)
                    {
                        PhoenixPMS2XL.Export2XLMEInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , General.GetNullableGuid(Reportid.ToString())
                                                                              , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                              , General.GetNullableGuid(null)
                                                                              , General.GetNullableGuid(null)
                                                                              , formtype);
                    }
                    else if (formtype == 10)
                    {
                        PhoenixPMS2XL.Export2XLAEDecarbonisationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 11)
                    {
                        PhoenixPMS2XL.Export2XLMEPerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 12)
                    {
                        PhoenixPMS2XL.Export2XLMEPerformanceEOPLReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 13)
                    {
                        PhoenixPMS2XL.Export2XLCargoTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 14)
                    {
                        PhoenixPMS2XL.Export2XLBallastTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
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
                                                                                , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
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
                        PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
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
                Rebind();
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
       
        try
        {
            BindDataReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        LinkButton imgbtn = (LinkButton)e.Item.FindControl("CmdMapping");
        if (imgbtn != null)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel reportID = (RadLabel)item.FindControl("lblReportId");
            RadLabel formID = (RadLabel)item.FindControl("lblFormId");
            imgbtn.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderList.aspx?ReportId=" + reportID.Text + "&Formid=" + formID.Text + "&Workorderid=" + (null) + "'); return false;");
        }
        RadImageButton dwn = (RadImageButton)e.Item.FindControl("cmdDownload");
        if (dwn != null)
        {
            dwn.Visible = SessionUtil.CanAccess(this.ViewState, dwn.CommandName);
        }
        LinkButton excel = (LinkButton)e.Item.FindControl("cmdExcel");
        if (excel != null)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel reportID = (RadLabel)item.FindControl("lblReportId");
            RadLabel workorderid = (RadLabel)item.FindControl("lblWorkorderId");
            excel.Visible = SessionUtil.CanAccess(this.ViewState, excel.CommandName);
            excel.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDoneView.aspx?rid=" + reportID.Text + "&wid=" + workorderid.Text + "'); return false;");
        }

        if (dwn != null && excel != null)
        {
            if (drv["FLDISNEWEXCELFORM"].ToString() == "1")
            {
                dwn.Visible = false;
            }
            else
            {
                excel.Visible = false;
            }
        }

        if (e.Item is GridDataItem)
        {
            LinkButton imgbtnyn = (LinkButton)e.Item.FindControl("CmdMapped");
            string activeYN = Convert.ToString(((RadLabel)e.Item.FindControl("lblActiveyn")).Text);
            if (activeYN == "1")
            {
                imgbtnyn.Visible = true;
            }
            else
            {
                imgbtnyn.Visible = false;
            }
        }

    }

    protected void MenuGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "FIND")
            {
                RadGrid1.CurrentPageIndex = 0;
                //RadGrid1.Rebind();
            }
            if (CommandName.ToUpper() == "CLEAR")
            {
                RadGrid1.CurrentPageIndex = 0;
                txtFromDate.Text = "";
                txtToDate.Text = "";
                //RadGrid1.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
