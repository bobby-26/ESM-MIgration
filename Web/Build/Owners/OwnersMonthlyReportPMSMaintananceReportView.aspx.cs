using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportPMSMaintananceReportView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../Owners/OwnersMonthlyReportPMSMaintananceReportView.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvMaintenance')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbarmain.AddFontAwesomeButton("../Owners/OwnersMonthlyReportPMSMaintananceReportView.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.MenuList = toolbarmain.Show();

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            Filter.CurrentOwnerReportFormFilter = null;
        }

    }

    protected void gvMaintenance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        binddata();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void binddata()
    {
        string[] alColumns = { "FLDFORMNO", "FLDFORMNAME", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDLASTREPORTEDDATE" };
        string[] alCaptions = { "Form No.", "Form Name", "Jobcode & Title", "Component No.", "Component Name", "Date" };

        NameValueCollection nvc = Filter.CurrentOwnerReportFormFilter;
        if (nvc == null)
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Add("txtJob", string.Empty);
            criteria.Add("txtComponentNumber", string.Empty);
            criteria.Add("txtComponentName", string.Empty);
            criteria.Add("txtFormName", string.Empty);

            Filter.CurrentOwnerReportFormFilter = criteria;
            nvc = Filter.CurrentOwnerReportFormFilter;
        }



        DataSet ds = PhoenixOwnerReportQuality.OwnersReportMaintenanceFormSearch(General.GetNullableInteger(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            , General.GetNullableString(nvc["txtFormName"] ?? null)
            , General.GetNullableString(nvc["txtComponentNumber"] ?? null)
                    , General.GetNullableString(nvc["txtComponentName"] ?? null)
                    , General.GetNullableString(nvc["txtJob"] ?? null));
        gvMaintenance.DataSource = ds;



        string heading = Request.QueryString["title"];
        if (string.IsNullOrEmpty(heading))
        {
            heading = "Maintenance Form";
        }
        General.SetPrintOptions("gvMaintenance", heading, alCaptions, alColumns, ds);
    }

    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDFORMNO", "FLDFORMNAME", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDLASTREPORTEDDATE" };
            string[] alCaptions = { "Form No.", "Form Name", "Jobcode & Title", "Component No.", "Component Name", "Date" };

            NameValueCollection nvc = Filter.CurrentOwnerReportFormFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }


            DataSet ds = PhoenixOwnerReportQuality.OwnersReportMaintenanceFormSearch(General.GetNullableInteger(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            , General.GetNullableString(nvc["txtFormName"] ?? null)
            , General.GetNullableString(nvc["txtComponentNumber"] ?? null)
                    , General.GetNullableString(nvc["txtComponentName"] ?? null)
                    , General.GetNullableString(nvc["txtJob"] ?? null));
            gvMaintenance.DataSource = ds;

            string heading = Request.QueryString["title"];
            if (string.IsNullOrEmpty(heading))
            {
                heading = "Maintenance Form";
            }
            General.ShowExcel(heading, ds.Tables[0], alColumns, alCaptions, 1, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMaintenance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton comjob = (LinkButton)e.Item.FindControl("cmdComjob");
            if (comjob != null)
            {
                comjob.Visible = SessionUtil.CanAccess(this.ViewState, comjob.CommandName);
                comjob.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceFormComponentJobMapList.aspx?formid=" + drv["FLDFORMID"].ToString() + "'); return false;");
            }
            LinkButton templ = (LinkButton)e.Item.FindControl("cmdExcelTemplate");
            if (templ != null)
            {
                templ.Visible = SessionUtil.CanAccess(this.ViewState, templ.CommandName);
                templ.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryExcelTemplate.aspx?fid=" + drv["FLDFORMID"].ToString() + "'); return false;");
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)  //issue
                {
                    eb.Attributes.Add("style", "display:none");
                }
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null)
                sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }
            LinkButton excel = (LinkButton)e.Item.FindControl("lblFormName");
            if (excel != null)
            {
                Guid workorderid = new Guid(drv["FLDWORKORDERID"].ToString());
                Guid Reportid = new Guid(drv["FLDREPORTID"].ToString());
                if (Convert.ToInt32(drv["FLDJSONREPORTYN"].ToString()) != 0)
                    excel.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDoneView.aspx?rid=" + Reportid.ToString() + "&wid=" + workorderid.ToString() + "&VESSELID=" + Filter.SelectedOwnersReportVessel + "'); return false;");
            }
        }

        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridFilteringItem)
        {

            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = GetFilter("txtJob");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDFORMNAME").CurrentFilterValue = GetFilter("txtFormName");
        }
    }

    protected void gvMaintenance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper() == "FIND")
            {
                GridDataItem item = (GridDataItem)e.Item;

                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("txtJob", string.Empty);
                criteria.Add("txtComponentNumber", string.Empty);
                criteria.Add("txtComponentName", string.Empty);
                criteria.Add("txtFormName", string.Empty);

                Filter.CurrentOwnerReportFormFilter = criteria;
                gvMaintenance.Rebind();
            }

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("txtJob", gvMaintenance.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue);
                criteria.Add("txtComponentNumber", gvMaintenance.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue);
                criteria.Add("txtComponentName", gvMaintenance.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue);
                criteria.Add("txtFormName", gvMaintenance.MasterTableView.GetColumn("FLDFORMNAME").CurrentFilterValue);

                Filter.CurrentOwnerReportFormFilter = criteria;

                gvMaintenance.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                int lbljsonyn = Convert.ToInt32(((RadLabel)e.Item.FindControl("lbljsonyn")).Text);

                if (lbljsonyn == 0)
                {
                    Guid Reportid = new Guid(((RadLabel)e.Item.FindControl("lblReportId")).Text);
                    int formtype = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblFormType")).Text);
                    Guid formid = new Guid(((RadLabel)e.Item.FindControl("lblFormID")).Text);
                    if (formtype == 1)
                    {
                        PhoenixPMS2XL.Export2XLMEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);

                    }
                    else if (formtype == 2)
                    {
                        PhoenixPMS2XL.Export2XLMEMaintenanceReportEOPL(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 3)
                    {
                        PhoenixPMS2XL.Export2MECylinderCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }

                    else if (formtype == 4)
                    {
                        PhoenixPMS2XL.Export2XLMainenginePisonCalibrationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 5)
                    {
                        PhoenixPMS2XL.Export2XLMainEngineStuffingBoxReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 6)
                    {
                        PhoenixPMS2XL.Export2XLAuxiliaryEngineReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                , General.GetNullableGuid(null)
                                                                                , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 7)
                    {
                        PhoenixPMS2XL.Export2XLAERodInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                             , General.GetNullableGuid(Reportid.ToString())
                                                                             , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                             , General.GetNullableGuid(null)
                                                                             , General.GetNullableGuid(null)
                                                                             , formtype);
                    }
                    else if (formtype == 8)
                    {
                        PhoenixPMS2XL.Export2XLAEMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , General.GetNullableGuid(Reportid.ToString())
                                                                              , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                              , General.GetNullableGuid(null)
                                                                              , General.GetNullableGuid(null)
                                                                              , formtype);
                    }
                    else if (formtype == 9)
                    {
                        PhoenixPMS2XL.Export2XLMEInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                              , General.GetNullableGuid(Reportid.ToString())
                                                                              , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                              , General.GetNullableGuid(null)
                                                                              , General.GetNullableGuid(null)
                                                                              , formtype);
                    }
                    else if (formtype == 10)
                    {
                        PhoenixPMS2XL.Export2XLAEDecarbonisationReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 11)
                    {
                        PhoenixPMS2XL.Export2XLMEPerformanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 12)
                    {
                        PhoenixPMS2XL.Export2XLMEPerformanceEOPLReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 13)
                    {
                        PhoenixPMS2XL.Export2XLCargoTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 14)
                    {
                        PhoenixPMS2XL.Export2XLBallastTankInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(Reportid.ToString())
                                                                               , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                               , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                               , formtype);
                    }
                    else if (formtype == 15)
                    {
                        PhoenixPMS2XL.Export2XlRHTurbochargerReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , General.GetNullableGuid(Reportid.ToString())
                                                                        , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                        , null
                                                                        , null
                                                                        , formtype);
                    }
                    else if (formtype == 16)
                    {
                        PhoenixPMS2XL.Export2XlTankerValvesMaintenanceRecord(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                , General.GetNullableGuid(null)
                                                                               , General.GetNullableGuid(null)
                                                                                , formtype);
                    }
                    else if (formtype == 17)
                    {
                        PhoenixPMS2XL.Export2XlCASPACSystemLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableGuid(Reportid.ToString())
                                                                            , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                            , null
                                                                            , null
                                                                            , formtype);
                    }
                    else if (formtype == 18)
                    {
                        PhoenixPMS2XL.Export2XlMainEnginePistonRingGapMeasurementReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , General.GetNullableGuid(Reportid.ToString())
                                                                                            , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                            , null
                                                                                            , null
                                                                                            , formtype);
                    }
                    else if (formtype == 19)
                    {
                        PhoenixPMS2XL.Export2XlValveGreasingandMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                     , General.GetNullableGuid(Reportid.ToString())
                                                                                     , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                     , null
                                                                                     , null
                                                                                     , formtype);
                    }
                    else if (formtype == 20)
                    {
                        PhoenixPMS2XL.Export2XlMainEngineExhaustValveOverhaulReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                     , General.GetNullableGuid(Reportid.ToString())
                                                                                     , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                     , null
                                                                                     , null
                                                                                     , formtype);
                    }
                    else if (formtype == 21)
                    {
                        PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 22)
                    {
                        PhoenixPMS2XL.Export2XlMeggerTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , General.GetNullableGuid(Reportid.ToString())
                                                                 , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                 , null
                                                                 , null
                                                                 , formtype);
                    }
                    else if (formtype == 23)
                    {
                        PhoenixPMS2XL.Export2XlVibrationMonitoringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , General.GetNullableGuid(Reportid.ToString())
                                                                 , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                 , null
                                                                 , null
                                                                 , formtype);

                    }
                    else if (formtype == 24)
                    {
                        PhoenixPMS2XL.Export2XLPOTableWaterTestRecordReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                     , General.GetNullableGuid(Reportid.ToString())
                                                                                     , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                     , null
                                                                                     , null
                                                                                     , formtype);
                    }
                    else if (formtype == 26)
                    {
                        PhoenixPMS2XL.Export2XLScavengeInspectionReportUEC(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }

                    else if (formtype == 27)
                    {
                        PhoenixPMS2XL.Export2XLCargoTankPassivityTestReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 29)
                    {
                        PhoenixPMS2XL.Export2XLGasTankerMaintenanceReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 30)
                    {
                        PhoenixPMS2XL.Export2XLShaftEarthLogReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 31)
                    {
                        PhoenixPMS2XL.Export2XLScavengeInspectionReportSULZER(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 32)
                    {
                        PhoenixPMS2XL.Export2XLHoldConditionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 33)
                    {
                        PhoenixPMS2XL.Export2XLScavengeInspectionReportMANBW(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 34)
                    {
                        PhoenixPMS2XL.Export2XLCrankwebsDeflectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 35)
                    {
                        PhoenixPMS2XL.Export2XLBearingMeasuringReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 36)
                    {
                        PhoenixPMS2XL.Export2XLWeeklySafetyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 37)
                    {
                        PhoenixPMS2XL.Export2XLSimpleJobsReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 38)
                    {
                        PhoenixPMS2XL.Export2XLMonthlyMEperformance(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 39)
                    {
                        PhoenixPMS2XL.Export2XLAEPerformanceReport2(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 40)
                    {
                        PhoenixPMS2XL.Export2XLAEPerformanceReport3(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(Reportid.ToString())
                                                                                    , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                                    , null
                                                                                    , null
                                                                                    , formtype);
                    }
                    else if (formtype == 41)
                    {
                        PhoenixPMS2XL.Export2XLPaintStock(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                , null
                                                                , formtype);
                    }
                    else if (formtype == 42)
                    {
                        PhoenixPMS2XL.Export2XLHoldInspectionReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                , null
                                                                , null
                                                                , formtype);
                    }
                    else if (formtype == 43)
                    {
                        PhoenixPMS2XL.Export2XLPeriodicalCheckListReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(Reportid.ToString())
                                                                , int.Parse(Filter.SelectedOwnersReportVessel)
                                                                , null
                                                                , null
                                                                , formtype);
                    }
                }
                //else
                //{
                //    LinkButton excel = (LinkButton)e.Item.FindControl("lblFormName");
                //    Guid Reportid = new Guid(((RadLabel)e.Item.FindControl("lblReportId")).Text);
                //    Guid workorderid = new Guid(((RadLabel)e.Item.FindControl("lblworkorderid")).Text);
                //    excel.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDoneView.aspx?rid=" + Reportid.ToString() + "&wid=" + workorderid.ToString() + "&VESSELID=" + Filter.SelectedOwnersReportVessel + "'); return true;");
                //}
                Rebind();
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvMaintenance.SelectedIndexes.Clear();
        gvMaintenance.EditIndexes.Clear();
        gvMaintenance.DataSource = null;
        gvMaintenance.Rebind();
    }


    protected void gvMaintenance_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (!IsPostBack)
        {
            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = GetFilter("txtJob");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDFORMNAME").CurrentFilterValue = GetFilter("txtFormName");

            grid.Rebind();
        }
    }

    protected string GetFilter(string filter)
    {
        string value = string.Empty;
        NameValueCollection nvc = Filter.CurrentOwnerReportFormFilter;
        if (nvc != null)
        {
            value = nvc[filter];
        }
        return value;
    }
    protected void SetFilter(string key, string value)
    {
        NameValueCollection nvc = Filter.CurrentOwnerReportFormFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection
            {
                { key, value }
            };
        }
        else
        {
            nvc[key] = value;
        }
        Filter.CurrentOwnerReportFormFilter = nvc;
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOwnerReportFormFilter = null;
                foreach (GridColumn column in gvMaintenance.MasterTableView.Columns)
                {

                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                gvMaintenance.MasterTableView.FilterExpression = string.Empty;
                gvMaintenance.MasterTableView.Rebind();
            }
            if (CommandName.ToUpper() == "EXCEL")
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}