using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class OwnersMonthlyReportPMSMaintananceReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rdMaintenance.Visible = SessionUtil.CanAccess(this.ViewState, "MAINTENANCEFORM");
        rdDefect.Visible = SessionUtil.CanAccess(this.ViewState, "DEFECT");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            lnkMA.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Maintenance Form', '" + Session["sitepath"] + "/Owners/OwnersMonthlyReportPMSMaintananceReportView.aspx',500,900); return false;");
            lnkDefect.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Defects / Non Routine Jobs', '" + Session["sitepath"] + "/Owners/OwnersReportDefectListRegister.aspx',500,900); return false;");
            lnkException.Attributes.Add("onclick", "javascript:openNewWindow('code1', 'Exception Report', '" + Session["sitepath"] + "/Owners/OwnersWorkOrderReportList.aspx',true); return false;");
            lnkMAComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Maintenance Form', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=MFO');return false; ");
            lnkDefectComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Defects / Non Routine Jobs', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=NRJ');return false; ");
            lnkExceptionComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Exception Report', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=EXC');return false; ");
            CheckComments();
        }
    }
    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("MFO", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkMAComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("NRJ", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkDefectComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("EXC", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkExceptionComments.Controls.Add(html);
        }
    }
    protected void gvDefect_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportDefect(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvDefect.DataSource = dt;
    }

    protected void gvDefect_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string Defectjobid = drv["FLDDEFECTJOBID"].ToString();

            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton verify = (ImageButton)e.Item.FindControl("cmdverify");
            ImageButton workorder = (ImageButton)e.Item.FindControl("cmdWorkorder");
            ImageButton complete = (ImageButton)e.Item.FindControl("cmdComplete");
            ImageButton postpone = (ImageButton)e.Item.FindControl("cmdPostpone");

            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                //edit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectListUpdate.aspx?defectjobId=" + drv["FLDDEFECTJOBID"] + "',false,700,500); ");
            }
            if (verify != null)
            {
                verify.Visible = SessionUtil.CanAccess(this.ViewState, verify.CommandName);
                //verify.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectJobVerification.aspx?defectjobId=" + drv["FLDDEFECTJOBID"] + "&defectno=" + drv["FLDDEFECTNO"] + "',false,500,300); ");
            }
            if (postpone != null)
            {
                postpone.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '" + drv["FLDDEFECTNO"].ToString() + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectReschedule.aspx?dueDate=" + drv["FLDDUEDATE"] + "&defectId=" + drv["FLDDEFECTJOBID"] + "&VesselId="+ Filter.SelectedOwnersReportVessel + "',false,500,400); ");
                postpone.Visible = SessionUtil.CanAccess(this.ViewState, postpone.CommandName);
            }
            if (drv["FLDWORKORDERREQUIRED"].ToString() == "0")
            {
                if (verify != null)
                {
                    verify.Visible = false;
                }
                if (complete != null)
                {
                    complete.Visible = true;
                    complete.Visible = SessionUtil.CanAccess(this.ViewState, complete.CommandName);
                    //complete.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectClose.aspx?DefectId=" + drv["FLDDEFECTJOBID"] + "',false,500,300); ");
                }
            }
            if (drv["FLDWORKORDERREQUIRED"].ToString() == "1")
            {
                if (verify != null)
                {
                    verify.Visible = false;
                }
                if (workorder != null)
                {
                    workorder.Visible = SessionUtil.CanAccess(this.ViewState, workorder.CommandName);
                    //workorder.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODefectJobDetails.aspx?DefectId=" + drv["FLDDEFECTJOBID"] + "&DefectNo=" + drv["FLDDEFECTNO"] + "&ComponentId=" + drv["FLDCOMPONENTID"] + "&Res=" + drv["FLDRESPONSIBILITYID"] + "&Due=" + drv["FLDDUEDATE"] + "',false,800,500); ");
                }
            }

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                //del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }


            if (!string.IsNullOrEmpty(drv["FLDDONEBY"].ToString()))
            {
                edit.Visible = false;
                del.Visible = false;
                verify.Visible = false;
                postpone.Visible = false;
                complete.Visible = false;

            }
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            if (Communication != null)
            {
                int vesselid = int.Parse(Filter.SelectedOwnersReportVessel);
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','" + drv["FLDDEFECTNO"].ToString() + "','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=DEFECT" + "&Referenceid=" + drv["FLDDEFECTJOBID"] + "&Vesselid=" + vesselid + "');");
            }

            ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");
            if (cmdAtt != null)
            {
                cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','" + drv["FLDDEFECTNO"].ToString() + "','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDEFECTJOBID"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&DocSource=DEFECT'); return false;");
                cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
                if (drv["FLDATTACHMENTCOUNT"].ToString() == "0") cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";
            }
        }
    }

    protected void gvMaintenance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixOwnerReportPMS.OwnersReportMaintenanceForm(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        gvMaintenance.DataSource = dt;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
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
    }

    protected void gvMaintenance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.RowIndex;

            int lbljsonyn = Convert.ToInt32(((RadLabel)e.Item.FindControl("lbljsonyn")).Text);
            int formtype = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblFormType")).Text);
            Guid formid = new Guid(((RadLabel)e.Item.FindControl("lblFormID")).Text);
            Guid Reportid = new Guid(((RadLabel)e.Item.FindControl("lblReportId")).Text);

            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                if (lbljsonyn == 0)
                {
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
                //    Guid workorderid = new Guid(((RadLabel)e.Item.FindControl("lblworkorderid")).Text);
                //    //excel.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDoneView.aspx?rid=" + Reportid.ToString() + "&wid=" + workorderid.ToString() + "&VESSELID=" + Filter.SelectedOwnersReportVessel+ "'); return false;");

                //    String scriptpopup = String.Format(
                // "javascript:openNewWindow('codehelp1','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryTemplateDoneView.aspx?rid=" + Reportid.ToString() + "&wid=" + workorderid.ToString() + "&VESSELID=" + Filter.SelectedOwnersReportVessel + "','500','900');");
                //    RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                //    CheckComments();
                //}
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
        DataTable dt = PhoenixOwnerReportPMS.OwnersJobExceptionReport(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            ,null, null, null, null, null, null, null, null);
        RadGrid1.DataSource = dt;
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string name = drv["FLDWORKORDERNUMBER"].ToString() + " - " + drv["FLDWORKORDERNAME"].ToString();
            LinkButton cmdParameters = (LinkButton)e.Item.FindControl("cmdParameters");
            if (cmdParameters != null)
            {
                cmdParameters.Attributes.Add("onclick", "javascript:openNewWindow('Parameters','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderLogParameterList.aspx?&WORKORDERID=" + drv["FLDWORKORDERID"] + "&vesselid=" + Filter.SelectedOwnersReportVessel + "',500,600); return false;");
                if (drv["FLDISJOBPARAMETERWITHINLIMIT"].ToString() == "1")
                {
                    cmdParameters.ToolTip = "All values are within set limits";
                    cmdParameters.Attributes["style"] = "color:green !important";
                }
                else if (drv["FLDISJOBPARAMETERWITHINLIMIT"].ToString() == "0")
                {
                    cmdParameters.ToolTip = "One or more values are outside the set limits";
                    cmdParameters.Attributes["style"] = "color:red !important";
                }
                cmdParameters.Visible = SessionUtil.CanAccess(this.ViewState, cmdParameters.CommandName);
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");

            if (lnkTitle != null)
            {
                lnkTitle.Visible = SessionUtil.CanAccess(this.ViewState, lnkTitle.CommandName);
                if (General.GetNullableGuid(drv["FLDCOMPONENTJOBID"].ToString()) != null)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"].ToString() + "&COMPONENTID=" + drv["FLDCOMPONENTID"].ToString() + "&Cancelledjob=0&vesselid=" + Filter.SelectedOwnersReportVessel + "','','1200','600');return false");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','" + name + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "&vesselid=" + Filter.SelectedOwnersReportVessel + "','','1200','600');return false");
            }
        }
    }
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

    }
}