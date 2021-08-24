using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;

using Telerik.Web.UI;

public partial class CrewOffshoreDMRMidNightReportTechnical : PhoenixBasePage
{
    public string nextOperational = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();

            toolbarvoyagetap.AddButton("List", "MIDNIGHTREPORTLIST");
            toolbarvoyagetap.AddButton("MidNight Report", "MIDNIGHTREPORT");
            toolbarvoyagetap.AddButton("Tank Plan", "TANKPLAN");
            toolbarvoyagetap.AddButton("HSE", "HSE");
            toolbarvoyagetap.AddButton("Passenger List", "PASSENGERLIST");
            toolbarvoyagetap.AddButton("Crew", "CREW");
            toolbarvoyagetap.AddButton("Technical", "TECHNICAL");


            MenuReportTap.AccessRights = this.ViewState;
            MenuReportTap.MenuList = toolbarvoyagetap.Show();
            MenuReportTap.SelectedMenuIndex = 6;
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            toolbarReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

            PhoenixToolbar toolbarMach = new PhoenixToolbar();
            toolbarMach.AddImageLink("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRequisition.aspx?module=DMR&date=" + txtDate.Text + "')", "Add", "add.png", "ADD");
            MenuMachineryFailure.AccessRights = this.ViewState;
            MenuMachineryFailure.MenuList = toolbarMach.Show();
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarVM = new PhoenixToolbar();
            toolbarVM.AddButton("Save", "SAVE");
            MenuTabSaveVesselMovements.AccessRights = this.ViewState;
            MenuTabSaveVesselMovements.MenuList = toolbarVM.Show();

            PhoenixToolbar toolbarMeteorologyData = new PhoenixToolbar();
            toolbarMeteorologyData.AddButton("Save", "SAVE");
            MenuTabSaveMeteorologyData.AccessRights = this.ViewState;
            MenuTabSaveMeteorologyData.MenuList = toolbarMeteorologyData.Show();

            PhoenixToolbar toolbarFO = new PhoenixToolbar();
            toolbarFO.AddButton("Save", "SAVE");
            MenuTabSaveFO.AccessRights = this.ViewState;
            MenuTabSaveFO.MenuList = toolbarFO.Show();

            PhoenixToolbar toolbarBulks = new PhoenixToolbar();
            toolbarBulks.AddButton("Save", "SAVE");
            MenuTabSaveBulks.AccessRights = this.ViewState;
            MenuTabSaveBulks.MenuList = toolbarBulks.Show();

            if (!IsPostBack)
            {
                BindInstallationType();
                ViewState["COMPANYID"] = "";
                ViewState["REACTIVATED"] = null;
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;

                }
                else
                {
                    ViewState["VESSELID"] = "";
                    ucVessel.Enabled = false;
                }
                ddlETALocation.DataSource = PhoenixRegistersDMRLocation.DMRLocationList();
                ddlETALocation.DataBind();

                ddlETDLocation.DataSource = PhoenixRegistersDMRLocation.DMRLocationList();
                ddlETDLocation.DataBind();

                //ddlETALocation.Items.Insert(0, new ListItem("--Select--", ""));
                //ddlETDLocation.Items.Insert(0, new ListItem("--Select--", ""));

                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    EditMidNightReport();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["MonthlyReportId"] = "";

                DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreInitailMidNightReportYN(int.Parse(Session["VESSELID"].ToString()), General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
                ViewState["INITIALREPORTYN"] = ds.Tables[0].Rows[0]["FLDINITIALREPORTYN"].ToString();
                ViewState["PREVIOUSEREPORTID"] = ds.Tables[0].Rows[0]["FLDPREVIOUSEREPORTID"].ToString();

                EditMidNightReportRunTime(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));
                BindOperationalTimeSummary();
                EditFlowMeterReading();
                BindOperationalTimeSummary();

                lnkFuelConsShowGraph.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','CrewOffshoreDMRMonthlyReport.aspx?ReportID=" + General.GetNullableString(ViewState["MonthlyReportId"].ToString()) + "'); return false;");
                BindRequisionsandPO();
                BindOperationalTimeSummary();
                BindOilLoadandConsumption();
                BindMeteorologyData();
                BindPlannedActivity();
                BindVesselMovements();
                BindDueAudits();
                BindDueShipTasks();
                BindDueCertificates();
                BindMachineryFailures();
                //BindExternalInspection();
                BindWorkOrder();
                BindPMSoverdue();
                ChangeVesselStatus();
                ChangeFlowmeterTotalEnabedYN();
                CheckPrevioueDefectiveFM();
                ChangeNoFlowmeterEnabedYN(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));
                SetFieldRange();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MIDNIGHTREPORT"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReport.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }

        if (CommandName.ToUpper().Equals("MIDNIGHTREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportList.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("HSE"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportHSE.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("CREW"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportCrew.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("TECHNICAL"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportTechnical.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("TANKPLAN"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportTankPlan.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        if (CommandName.ToUpper().Equals("PASSENGERLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportPassengerList.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
    }

    protected void MenuNRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Session["MIDNIGHTREPORTID"] != null)
        {
            Response.Redirect("VesselPositionNoonReportEngine.aspx", false);
        }
        else
        {
            ucError.ErrorMessage = "Please save the Report.";
            ucError.Visible = true;
        }
    }

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableString(ucLatitude.Text) == null)
            ucError.ErrorMessage = "Latitude is required.";
        if (General.GetNullableString(ucLongitude.Text) == null)
            ucError.ErrorMessage = "Longitude is required.";
        if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Select the Vessel Status";
        }
        else
        {
            if (ddlvesselstatus.SelectedValue == "1" && General.GetNullableInteger(ucPort.SelectedValue) == null)
            {
                ucError.ErrorMessage = "Port is mandatory";
            }
            if (ddlvesselstatus.SelectedValue == "2" && General.GetNullableInteger(ucPort.SelectedValue) == null)
            {
                ucError.ErrorMessage = "Port is mandatory";
            }
            if (ddlvesselstatus.SelectedValue == "2" && General.GetNullableDateTime(txtETADate.Text) == null)
            {
                ucError.ErrorMessage = "ETA Date is mandatory";
            }

            if (ddlvesselstatus.SelectedValue == "3" && txtLocation.Text == string.Empty)
            {
                ucError.ErrorMessage = "Location is mandatory";
            }
            if (ddlvesselstatus.SelectedValue == "3" && General.GetNullableDateTime(txtETADate.Text) == null)
            {
                ucError.ErrorMessage = "ETA Date is mandatory";
            }

            if (ddlvesselstatus.SelectedValue == "4" && txtLocation.Text == string.Empty)
            {
                ucError.ErrorMessage = "Location is mandatory";
            }
            if (General.GetNullableDateTime(txtETADate.Text) != null && General.GetNullableDateTime(txtDate.Text) != null)
            {
                if (General.GetNullableDateTime(txtETADate.Text) < General.GetNullableDateTime(txtDate.Text))
                {
                    ucError.ErrorMessage = "ETA date cannot be less than report date.";
                }
            }
            if (General.GetNullableDateTime(txtETDDate.Text) != null && General.GetNullableDateTime(txtDate.Text) != null)
            {
                if (General.GetNullableDateTime(txtETDDate.Text) < General.GetNullableDateTime(txtDate.Text))
                {
                    ucError.ErrorMessage = "ETD date cannot be less than report date.";
                }
            }
            if (General.GetNullableDateTime(txtArrivalDate.Text) != null && General.GetNullableDateTime(txtDate.Text) != null)
            {
                if (General.GetNullableDateTime(txtArrivalDate.Text) < General.GetNullableDateTime(txtDate.Text))
                {
                    ucError.ErrorMessage = "Arrival date cannot be less than report date.";
                }
            }
            if (General.GetNullableDateTime(txtDepartureDate.Text) != null && General.GetNullableDateTime(txtDate.Text) != null)
            {
                if (General.GetNullableDateTime(txtDepartureDate.Text) < General.GetNullableDateTime(txtDate.Text))
                {
                    ucError.ErrorMessage = "Departure date cannot be less than report date.";
                }
            }
        }
        return (!ucError.IsError);
    }
    private void EditFlowMeterReading()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtme1initialhrs.Text = ds.Tables[0].Rows[0]["FLDME1FIRSTHRS"].ToString();
            txtme1lasthrs.Text = ds.Tables[0].Rows[0]["FLDME1LASTHRS"].ToString();
            lblme1returninitialhrs.Text = ds.Tables[0].Rows[0]["FLDME1RETURNFIRSTHRS"].ToString();
            lblme1returnlasthrs.Text = ds.Tables[0].Rows[0]["FLDME1RETURNLASTHRS"].ToString();
            txtme1Total.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME1TOTAL"].ToString()) == null ? "0" : ds.Tables[0].Rows[0]["FLDME1TOTAL"].ToString());
            txtme2initialhrs.Text = ds.Tables[0].Rows[0]["FLDME2FIRSTHRS"].ToString();
            txtme2lasthrs.Text = ds.Tables[0].Rows[0]["FLDME2LASTHRS"].ToString();
            lblme2returninitialhrs.Text = ds.Tables[0].Rows[0]["FLDME2RETURNFIRSTHRS"].ToString();
            lblme2returnlasthrs.Text = ds.Tables[0].Rows[0]["FLDME2RETURNLASTHRS"].ToString();
            txtme2Total.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME2TOTAL"].ToString()) == null ? "0" : ds.Tables[0].Rows[0]["FLDME2TOTAL"].ToString());
            txtgrandtotal.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDMEGRANDTOTAL"].ToString()) == null ? "0" : ds.Tables[0].Rows[0]["FLDMEGRANDTOTAL"].ToString());
            txtAE1Consumption.Text = ds.Tables[0].Rows[0]["FLDAE1CONS"].ToString();
            txtAE2Consumption.Text = ds.Tables[0].Rows[0]["FLDAE2CONS"].ToString();

            txtAE1initialhrs.Text = ds.Tables[0].Rows[0]["FLDAE1FIRSTHRS"].ToString();
            txtAE1lasthrs.Text = ds.Tables[0].Rows[0]["FLDAE1LASTHRS"].ToString();
            txtAE2initialhrs.Text = ds.Tables[0].Rows[0]["FLDAE2FIRSTHRS"].ToString();
            txtAE2lasthrs.Text = ds.Tables[0].Rows[0]["FLDAE2LASTHRS"].ToString();
            ucotherConsumption.Text = ds.Tables[0].Rows[0]["FLDOTHERCONS"].ToString();
            ucTotalConsumption.Text = ds.Tables[0].Rows[0]["FLDTOTALCONS"].ToString();

        }
    }
    private void EditMidNightReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(new Guid(Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["REPORTDATE"] = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
            ViewState["MonthlyReportId"] = ds.Tables[0].Rows[0]["FLDMONTHLYREPORTID"].ToString();
            Session["MONTHLYREPORTID"] = ds.Tables[0].Rows[0]["FLDMONTHLYREPORTID"].ToString();
            ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            txtDate.Text = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());

            txtVoyageId.Text = ds.Tables[0].Rows[0]["FLDVOYAGEID"].ToString();
            txtVoyageName.Text = ds.Tables[0].Rows[0]["FLDVOYAGENO"].ToString();

            txtClient.Text = ds.Tables[0].Rows[0]["FLDCLIENT"].ToString();
            txtMaster.Text = ds.Tables[0].Rows[0]["FLDMASTER"].ToString();
            txtSeaCondition.Text = ds.Tables[0].Rows[0]["FLDSEACONDITION"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["FLDLOCATION"].ToString();
            ucAvgSpeed.Text = ds.Tables[0].Rows[0]["FLDAVGSPEED"].ToString();
            ucSwell.Text = ds.Tables[0].Rows[0]["FLDSWELL"].ToString();
            ucWindDirection.SelectedDirection = ds.Tables[0].Rows[0]["FLDWINDDIRECTION"].ToString();
            ucWindSpeed.Text = ds.Tables[0].Rows[0]["FLDWINDSPEED"].ToString();
            ddlETALocation.SelectedValue = ds.Tables[0].Rows[0]["FLDETALOCATIONID"].ToString();
            txtETDDate.Text = ds.Tables[0].Rows[0]["FLDETDDATE"].ToString();
            //txtETDTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETDDATE"]);
            ddlETDLocation.SelectedValue = ds.Tables[0].Rows[0]["FLDETDLOCATIONID"].ToString();
            txtETADate.Text = ds.Tables[0].Rows[0]["FLDETADATE"].ToString();
           // txtETATime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETADATE"]);
            txtPOB.Text = ds.Tables[0].Rows[0]["FLDPOB"].ToString();
            txtCrew.Text = ds.Tables[0].Rows[0]["FLDCREW"].ToString();
            txtWaveHeight.Text = ds.Tables[0].Rows[0]["FLDWAVEHEIGHT"].ToString();

            ucLatitude.TextDegree = ds.Tables[0].Rows[0]["FLDLAT1"].ToString();
            ucLatitude.TextMinute = ds.Tables[0].Rows[0]["FLDLAT2"].ToString();
            ucLatitude.TextSecond = ds.Tables[0].Rows[0]["FLDLAT3"].ToString();
            ucLatitude.TextDirection = ds.Tables[0].Rows[0]["FLDLATDIRECTION"].ToString();
            ucLongitude.TextDegree = ds.Tables[0].Rows[0]["FLDLONG1"].ToString();
            ucLongitude.TextMinute = ds.Tables[0].Rows[0]["FLDLONG2"].ToString();
            ucLongitude.TextSecond = ds.Tables[0].Rows[0]["FLDLONG3"].ToString();
            ucLongitude.TextDirection = ds.Tables[0].Rows[0]["FLDLONGDIRECTION"].ToString();

            txtComments.Text = ds.Tables[0].Rows[0]["FLDCOMMENTS"].ToString();
            txtLookAheadRemarks.Text = ds.Tables[0].Rows[0]["FLDLOOKAHEADREMARKS"].ToString();
            ddlvesselstatus.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELSTATUS"].ToString();
            txtGeneralRemarks.Text = ds.Tables[0].Rows[0]["FLDGENERALREMARKS"].ToString();
            ucPort.SelectedValue = ds.Tables[0].Rows[0]["FLDPORTID"].ToString();
            ucPort.Text = ds.Tables[0].Rows[0]["PORTNAME"].ToString();

            txtMeterologyRemarks.Text = ds.Tables[0].Rows[0]["FLDMETEROLOGYREMARKS"].ToString();
            txtVesselMovementsRemarks.Text = ds.Tables[0].Rows[0]["FLDVESSELMOVEMENTSREMARKS"].ToString();
            txtbulkRemarks.Text = ds.Tables[0].Rows[0]["FLDBULKREMARKS"].ToString();
            rbnhourchange.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGE"].ToString();
            rbnhourvalue.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGEVALUE"].ToString();
            txtDPTime.Text = ds.Tables[0].Rows[0]["FLDTOTALDPTIME"].ToString();
            txtDPFuelConsumption.Text = ds.Tables[0].Rows[0]["FLDTOTALDPFUELCONSUMPTION"].ToString();
            txtFlowmeterRemarks.Text = ds.Tables[0].Rows[0]["FLDFLOWMETERREMARKS"].ToString();
            txtCrewRemarks.Text = ds.Tables[0].Rows[0]["FLDCREWREMARKS"].ToString();

            ucBreakfast.Text = ds.Tables[0].Rows[0]["FLDBREAKFAST"].ToString();
            ucLunch.Text = ds.Tables[0].Rows[0]["FLDLUNCH"].ToString();
            ucDinner.Text = ds.Tables[0].Rows[0]["FLDDINNER"].ToString();
            ucSupper.Text = ds.Tables[0].Rows[0]["FLDSUPPER"].ToString();
            ucTea1.Text = ds.Tables[0].Rows[0]["FLDCLIENTTEA1"].ToString();
            ucTea2.Text = ds.Tables[0].Rows[0]["FLDCLIENTTEA2"].ToString();

            ucSupBreakFast.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYBREAKFAST"].ToString();
            ucSupLunch.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYLUNCH"].ToString();
            ucSupDinner.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYDINNER"].ToString();
            ucSupSupper.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYSUPPER"].ToString();
            ucSupTea1.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYTEA1"].ToString();
            ucSupTea2.Text = ds.Tables[0].Rows[0]["FLDSUPERNUMERARYTEA2"].ToString();

            txtPOBClient.Text = ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString();
            txtPOBService.Text = ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString();
            txtTotalOB.Text = ds.Tables[0].Rows[0]["FLDTOTALONBOARD"].ToString();

            ucMEPort.Text = ds.Tables[0].Rows[0]["FLDMEPORT"].ToString();
            ucBT1.Text = ds.Tables[0].Rows[0]["FLDBT1"].ToString();
            ucFord.Text = ds.Tables[0].Rows[0]["FLDFORD"].ToString();
            ucMEStbd.Text = ds.Tables[0].Rows[0]["FLDMESTBD"].ToString();
            ucBT2.Text = ds.Tables[0].Rows[0]["FLDBT2"].ToString();
            ucMidship.Text = ds.Tables[0].Rows[0]["FLDMIDSHIP"].ToString();
            ucAEI.Text = ds.Tables[0].Rows[0]["FLDAE1"].ToString();
            ucST1.Text = ds.Tables[0].Rows[0]["FLDST1"].ToString();
            ucAft.Text = ds.Tables[0].Rows[0]["FLDAFT"].ToString();
            ucAEII.Text = ds.Tables[0].Rows[0]["FLDAE2"].ToString();
            ucST2.Text = ds.Tables[0].Rows[0]["FLDST2"].ToString();
            ucAverage.Text = ds.Tables[0].Rows[0]["FLDAVG"].ToString();
            txtArrivalDate.Text = ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString();
            //txtArrivalTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDARRIVALDATE"]);

            txtDepartureDate.Text = ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString();
            //txtDepartureTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"]);
            txtEstimatedDuration.Text = ds.Tables[0].Rows[0]["FLDESTIMATEDDURATION"].ToString();
            ddlInstalationType.SelectedValue = ds.Tables[0].Rows[0]["FLDINSTALLATIONTYPEID"].ToString();

            ddlCrewListSOBYN.SelectedValue = ds.Tables[0].Rows[0]["FLDCREWLISTDIFFSOBYN"].ToString();
            txtCrewListSOBRemarks.Text = ds.Tables[0].Rows[0]["FLDCREWLISTDIFFSOBREMARKS"].ToString();

            if (ds.Tables[0].Rows[0]["FLDMACHINERYFAILUREYN"].ToString() != string.Empty)
            {
                rblMachineryFailure.SelectedValue = ds.Tables[0].Rows[0]["FLDMACHINERYFAILUREYN"].ToString();
            }
            if (rblMachineryFailure.SelectedValue == "1")
                MenuMachineryFailure.Visible = true;
            else
                MenuMachineryFailure.Visible = false;

            if (ds.Tables[0].Rows[0]["FLDMEPORTFLOWDEFECTIVE"].ToString() == "1")
            {
                chkMEPortFlowDetective.Checked = true;
            }
            else
            {
                chkMEPortFlowDetective.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDMESTBDFLOWDEFECTIVE"].ToString() == "1")
            {
                chkMEStbdFlowDetective.Checked = true;
            }
            else
            {
                chkMEStbdFlowDetective.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDAE1FLOWDEFECTIVE"].ToString() == "1")
            {
                chkAE1FlowDetective.Checked = true;
            }
            else
            {
                chkAE1FlowDetective.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDAE2FLOWDEFECTIVE"].ToString() == "1")
            {
                chkAE2FlowDetective.Checked = true;
            }
            else
            {
                chkAE2FlowDetective.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDME1NOFM"].ToString() == "1")
            {
                chkMEPortNoFM.Checked = true;
            }
            else
            {
                chkMEPortNoFM.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDME1RETURNNOFM"].ToString() == "1")
            {
                chkMEPortreturnNoFM.Checked = true;
            }
            else
            {
                chkMEPortreturnNoFM.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDME2NOFM"].ToString() == "1")
            {
                chkMEStbdNoFM.Checked = true;
            }
            else
            {
                chkMEStbdNoFM.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDME2RETUTNNOFM"].ToString() == "1")
            {
                chkMEStbdreturnNoFM.Checked = true;
            }
            else
            {
                chkMEStbdreturnNoFM.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDAE1NOFM"].ToString() == "1")
            {
                chkAE1NoFM.Checked = true;
            }
            else
            {
                chkAE1NoFM.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDAE2NOFM"].ToString() == "1")
            {
                chkAE2NoFM.Checked = true;
            }
            else
            {
                chkAE2NoFM.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDMEPORTRETURNFLOWDEFECTIVE"].ToString() == "1")
            {
                chkMEPortreturnFlowDetective.Checked = true;
            }
            else
            {
                chkMEPortreturnFlowDetective.Checked = false;
            }

            if (ds.Tables[0].Rows[0]["FLDMESTBDRETURNFLOWDEFECTIVE"].ToString() == "1")
            {
                chkMEStbdreturnFlowDetective.Checked = true;
            }
            else
            {
                chkMEStbdreturnFlowDetective.Checked = false;
            }

            if (chkMEPortNoFM.Checked == true)
            {
                chkMEPortFlowDetective.Enabled = false;
                chkMEPortFlowDetective.Checked = false;

                txtme1lasthrs.CssClass = "readonlytextbox";
                txtme1lasthrs.ReadOnly = "true";
            }
            if (chkMEPortNoFM.Checked == false)
            {
                chkMEPortFlowDetective.Enabled = true;
                if (chkMEPortFlowDetective.Checked == true)
                {
                    txtme1lasthrs.CssClass = "readonlytextbox";
                    txtme1lasthrs.ReadOnly = "true";
                    txtme1Total.Text = "";
                }
                if (chkMEPortFlowDetective.Checked == false)
                {
                    txtme1lasthrs.CssClass = "input";
                    txtme1lasthrs.ReadOnly = "false";
                }

            }

            if (chkMEStbdNoFM.Checked == true)
            {
                chkMEStbdFlowDetective.Enabled = false;
                chkMEStbdFlowDetective.Checked = false;

                txtme2lasthrs.CssClass = "readonlytextbox";
                txtme2lasthrs.ReadOnly = "true";
            }
            if (chkMEStbdNoFM.Checked == false)
            {
                chkMEStbdFlowDetective.Enabled = true;
                if (chkMEStbdFlowDetective.Checked == true)
                {
                    txtme2lasthrs.CssClass = "readonlytextbox";
                    txtme2lasthrs.ReadOnly = "true";
                    txtme2Total.Text = "";
                }
                if (chkMEStbdFlowDetective.Checked == false)
                {
                    txtme2lasthrs.CssClass = "input";
                    txtme2lasthrs.ReadOnly = "false";
                }
            }

            if (chkMEPortreturnNoFM.Checked == true)
            {
                chkMEPortreturnFlowDetective.Enabled = false;
                chkMEPortreturnFlowDetective.Checked = false;

                lblme1returnlasthrs.CssClass = "readonlytextbox";
                lblme1returnlasthrs.ReadOnly = "true";
            }
            if (chkMEPortreturnNoFM.Checked == false)
            {
                chkMEPortreturnFlowDetective.Enabled = true;
                if (chkMEPortreturnFlowDetective.Checked == true)
                {
                    lblme1returnlasthrs.CssClass = "readonlytextbox";
                    lblme1returnlasthrs.ReadOnly = "true";
                }
                if (chkMEPortreturnFlowDetective.Checked == false)
                {
                    lblme1returnlasthrs.CssClass = "input";
                    lblme1returnlasthrs.ReadOnly = "false";
                }
            }

            if (chkMEStbdreturnNoFM.Checked == true)
            {
                chkMEStbdreturnFlowDetective.Enabled = false;
                chkMEStbdreturnFlowDetective.Checked = false;

                lblme2returnlasthrs.CssClass = "readonlytextbox";
                lblme2returnlasthrs.ReadOnly = "true";
            }
            if (chkMEStbdreturnNoFM.Checked == false)
            {
                chkMEStbdreturnFlowDetective.Enabled = true;
                if (chkMEStbdreturnFlowDetective.Checked == true)
                {
                    lblme2returnlasthrs.CssClass = "readonlytextbox";
                    lblme2returnlasthrs.ReadOnly = "true";
                }
                if (chkMEStbdreturnFlowDetective.Checked == false)
                {
                    lblme2returnlasthrs.CssClass = "input";
                    lblme2returnlasthrs.ReadOnly = "false";
                }
            }

            if (chkAE1NoFM.Checked == true)
            {
                chkAE1FlowDetective.Enabled = false;
                chkAE1FlowDetective.Checked = false;

                txtAE1lasthrs.CssClass = "readonlytextbox";
                txtAE1lasthrs.ReadOnly = "true";
            }
            if (chkAE1NoFM.Checked == false)
            {
                chkAE1FlowDetective.Enabled = true;
                if (chkAE1FlowDetective.Checked == true)
                {
                    txtAE1lasthrs.CssClass = "readonlytextbox";
                    txtAE1lasthrs.ReadOnly = "true";
                    txtAE1Consumption.Text = "";
                }
                if (chkAE1FlowDetective.Checked == false)
                {
                    txtAE1lasthrs.CssClass = "input";
                    txtAE1lasthrs.ReadOnly = "false";
                }
            }
            if (chkAE2NoFM.Checked == true)
            {
                chkAE2FlowDetective.Enabled = false;
                chkAE2FlowDetective.Checked = false;

                txtAE2lasthrs.CssClass = "readonlytextbox";
                txtAE2lasthrs.ReadOnly = "true";
            }
            if (chkAE2NoFM.Checked == false)
            {
                chkAE2FlowDetective.Enabled = true;
                if (chkAE2FlowDetective.Checked == true)
                {
                    txtAE2lasthrs.CssClass = "readonlytextbox";
                    txtAE2lasthrs.ReadOnly = "true";
                    txtAE2Consumption.Text = "";
                }
                if (chkAE2FlowDetective.Checked == false)
                {
                    txtAE2lasthrs.CssClass = "input";
                    txtAE2lasthrs.ReadOnly = "false";
                }
                txtAE2lasthrs.CssClass = "input";
                txtAE2lasthrs.ReadOnly = "false";
            }


            if (rbnhourchange.SelectedValue == "0" && rbnhourvalue.SelectedValue == "0")
                ddlAdvanceRetard.SelectedValue = "0";
            if (rbnhourchange.SelectedValue == "1" && rbnhourvalue.SelectedValue == "2")
                ddlAdvanceRetard.SelectedValue = "1";
            if (rbnhourchange.SelectedValue == "1" && rbnhourvalue.SelectedValue == "1")
                ddlAdvanceRetard.SelectedValue = "2";
            if (rbnhourchange.SelectedValue == "2" && rbnhourvalue.SelectedValue == "1")
                ddlAdvanceRetard.SelectedValue = "3";
            if (rbnhourchange.SelectedValue == "2" && rbnhourvalue.SelectedValue == "2")
                ddlAdvanceRetard.SelectedValue = "4";
            ucMEPortFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDMEPORTFIRSTRUNHRS"].ToString();
            ucMEStbdFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDMESTBDFIRSTRUNHRS"].ToString();
            ucAEIFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE1FIRSTRUNHRS"].ToString();
            ucAEIIFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE2FIRSTRUNHRS"].ToString();
            ucBT1FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT1FIRSTRUNHRS"].ToString();
            ucBT2FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT2FIRSTRUNHRS"].ToString();
            ucST1FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDST1FIRSTRUNHRS"].ToString();
            ucST2FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDST2FIRSTRUNHRS"].ToString();

            ucMEPortTotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDME1TOTALRUNHRS"].ToString();
            ucMEStbdTotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDME2TOTALRUNHRS"].ToString();
            ucAEITotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE1TOTALRUNHRS"].ToString();
            ucAEIITotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE2TOTALRUNHRS"].ToString();
            ucBT1TotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT1TOTALRUNHRS"].ToString();
            ucBT2TotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT2TOTALRUNHRS"].ToString();
            ucST1TotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDST1TOTALRUNHRS"].ToString();
            ucST2TotalRunHrs.Text = ds.Tables[0].Rows[0]["FLDST2TOTALRUNHRS"].ToString();            

            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataTable dt = PhoenixCrewOffshoreDMRMidNightReport.SearchMidnightReportVesselCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                       , General.GetNullableDateTime(ViewState["REPORTDATE"].ToString())
                                                                       , null, null
                                                                       , 1, 25
                                                                       , ref iRowCount, ref iTotalPageCount);



            if (dt.Rows.Count > 0)
            {
                txtPOB.Text = (int.Parse(dt.Rows[0]["FLDPOWCREW"].ToString())  + int.Parse(dt.Rows[0]["FLDPOBCLIENT"].ToString()) + int.Parse(dt.Rows[0]["FLDPOBSERVICE"].ToString())).ToString();
                txtCrew.Text = dt.Rows[0]["FLDPOWCREW"].ToString();
            }
            ViewState["REACTIVATED"] = ds.Tables[0].Rows[0]["FLDREACTIVATED"].ToString();
        }
    }
    private void BindRequisionsandPO()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportRequisionsandPOsList(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvRequisionsandPO.DataSource = ds;
            gvRequisionsandPO.DataBind();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRequisionsandPO_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataRowView drv = (DataRowView)e.Row.DataItem;

            LinkButton lnk1 = (LinkButton)e.Row.FindControl("lblCount");
            Label lblType = (Label)e.Row.FindControl("lblType");
            lnk1.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRRequisitionandPODetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            if (gv.ID == "gvVesselMovements")
            {
                UserControlMaskedTextBox tt = (UserControlMaskedTextBox)gv.FooterRow.FindControl("txtFromTimeAdd");
                tt.Text = "00:00";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    // Bulks
    private void BindOilLoadandConsumption()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOilLoadandConsumptionList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvBulks.DataSource = ds;
            gvBulks.DataBind();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulks_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            gvBulks.Controls[0].Controls.AddAt(0, gv);
        }
    }

    protected void gvBulks_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        GridView gv = (GridView)sender;
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            Label lblOpeningStock = (Label)e.Row.FindControl("lblOpeningStocks");
            UserControlMaskedTextBox txtOpeningStock = (UserControlMaskedTextBox)e.Row.FindControl("txtOpeningStock");
            if (ViewState["INITIALREPORTYN"].ToString() == "1")
            {
                if (lblOpeningStock != null)
                    lblOpeningStock.Visible = false;

                if (txtOpeningStock != null)
                    txtOpeningStock.Visible = true;
            }
            if (ViewState["REACTIVATED"] != null && ViewState["REACTIVATED"].ToString() == "1")
            {
                if (lblOpeningStock != null)
                    lblOpeningStock.Visible = false;

                if (txtOpeningStock != null)
                    txtOpeningStock.Visible = true;
            }
            Label lblUnit = (Label)e.Row.FindControl("lblUnit");

            UserControlMaskedTextBox ChartererLoadedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucChartererLoadedEdit");
            UserControlMaskedTextBox ChartererDischargedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucChartererDischargedEdit");
            UserControlMaskedTextBox ConsumedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucConsumedEdit");
            Label lblProductName = (Label)e.Row.FindControl("lblProductName");
            Label lblActiveYN = (Label)e.Row.FindControl("lblActiveYN");
            if (lblActiveYN != null && lblActiveYN.Text == "0")
            {
                ChartererLoadedEdit.CssClass = "readonlytextbox";
                ChartererLoadedEdit.ReadOnly = true;
                ChartererDischargedEdit.CssClass = "readonlytextbox";
                ChartererDischargedEdit.ReadOnly = true;
                ConsumedEdit.CssClass = "readonlytextbox";
                ConsumedEdit.ReadOnly = true;
                lblOpeningStock.Visible = true;
                txtOpeningStock.Visible = false;
            }
            if (drv["FLDSHORTNAME"].ToString() == "FO")
            {
                ConsumedEdit.CssClass = "readonlytextbox";
                ConsumedEdit.ReadOnly = true;
            }

            if (lblUnit.Text != "LTR")
            {

                ChartererLoadedEdit.IsInteger = false;
                ChartererLoadedEdit.DecimalPlace = 2;
                ChartererDischargedEdit.IsInteger = false;
                ChartererDischargedEdit.DecimalPlace = 2;
                ConsumedEdit.IsInteger = false;
                ConsumedEdit.DecimalPlace = 2;
                txtOpeningStock.IsInteger = false;
                txtOpeningStock.DecimalPlace = 2;

            }
            if (lblProductName.Text == "FO (LTR)")
            {
                decimal total = ((General.GetNullableDecimal(txtgrandtotal.Text) == null ? 0 : decimal.Parse(txtgrandtotal.Text)) +
                                 (General.GetNullableDecimal(txtAE1Consumption.Text) == null ? 0 : decimal.Parse(txtAE1Consumption.Text)) +
                                 (General.GetNullableDecimal(txtAE2Consumption.Text) == null ? 0 : decimal.Parse(txtAE2Consumption.Text))
                                 );
                ConsumedEdit.Text = total.ToString();
            }

            Label lblRemainedOnBoard = (Label)e.Row.FindControl("lblRemainedOnBoard");
            if (General.GetNullableDecimal(lblRemainedOnBoard.Text) != null)
            {
                if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && ((decimal.Parse(lblRemainedOnBoard.Text)) < (decimal.Parse(drv["FLDMINVALUE"].ToString()))))
                {
                    lblRemainedOnBoard.CssClass = "maxhighlight";
                }
                if (General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && (decimal.Parse(lblRemainedOnBoard.Text)) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
                {
                    lblRemainedOnBoard.CssClass = "maxhighlight";
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            Label lblOpeningStocksHeader = (Label)e.Row.FindControl("lblOpeningStocksHeader");
            if (ViewState["INITIALREPORTYN"].ToString() == "1")
            {
                if (lblOpeningStocksHeader != null)
                    lblOpeningStocksHeader.Text = "Opening Stock";
            }

        }

    }

    protected void gvBulks_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }

    protected void gvBulks_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    string OilLoadedConsumptionId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilLoadedConsumptionId")).Text;
                    if (OilLoadedConsumptionId != "")
                    {
                        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOilLoadandConsumptionDelete(new Guid(OilLoadedConsumptionId));
                    }
                }
                BindOilLoadandConsumption();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulks_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        int Ei = _gridView.EditIndex;
        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindOilLoadandConsumption();
    }

    protected void gvBulks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindOilLoadandConsumption();
    }

    protected void gvBulks_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }

            string OilTypeCode = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilTypeCode")).Text;
            string LoadedCharterer = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucChartererLoadedEdit")).Text;
            string DischargedCharterer = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucChartererDischargedEdit")).Text;
            string Consumed = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucConsumedEdit")).Text;
            string OilLoadedConsumptionId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOilLoadedConsumptionId")).Text;
            string OpeningStock = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("txtOpeningStock")).Text;


            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOilLoadandConsumptionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString()), new Guid(OilTypeCode)
            , General.GetNullableDecimal(OpeningStock), General.GetNullableDecimal(LoadedCharterer), General.GetNullableDecimal(DischargedCharterer), General.GetNullableDecimal(Consumed), General.GetNullableGuid(OilLoadedConsumptionId));

            _gridView.EditIndex = -1;
            BindOilLoadandConsumption();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void UpdateBulks()
    {
        foreach (GridDataItem gvr in gvBulks.Items)
        {

            RadLabel OilTypeCode = (RadLabel)gvr.FindControl("lblOilTypeCode");
            UserControlNumber LoadedCharterer = (UserControlNumber)gvr.FindControl("ucChartererLoadedEdit");
            UserControlNumber DischargedCharterer = (UserControlNumber)gvr.FindControl("ucChartererDischargedEdit");
            UserControlNumber Consumed = (UserControlNumber)gvr.FindControl("ucConsumedEdit");
            RadLabel OilLoadedConsumptionId = (RadLabel)gvr.FindControl("lblOilLoadedConsumptionId");
            UserControlNumber OpeningStock = (UserControlNumber)gvr.FindControl("txtOpeningStock");
            RadLabel lblActiveYN = (RadLabel)gvr.FindControl("lblActiveYN");
            RadLabel lblOilShortname = (RadLabel)gvr.FindControl("lblOilShortname");
            RadLabel lblUnitName = (RadLabel)gvr.FindControl("lblUnitName");
            RadLabel lblConversionfactorM3 = (RadLabel)gvr.FindControl("lblConversionfactorM3");

            if (lblActiveYN.Text == "1")
            {
                if (OilTypeCode != null && LoadedCharterer != null && DischargedCharterer != null && Consumed != null && OilLoadedConsumptionId != null)
                {
                    if ((OilTypeCode.Text != "") && (LoadedCharterer.Text != "" || DischargedCharterer.Text != "" || Consumed.Text != "" || OpeningStock.Text != ""))
                    {
                        if (lblOilShortname.Text == "FO")
                        {
                            decimal total = ((General.GetNullableDecimal(txtgrandtotal.Text) == null ? 0 : decimal.Parse(txtgrandtotal.Text)) +
                                             (General.GetNullableDecimal(txtAE1Consumption.Text) == null ? 0 : decimal.Parse(txtAE1Consumption.Text)) +
                                             (General.GetNullableDecimal(txtAE2Consumption.Text) == null ? 0 : decimal.Parse(txtAE2Consumption.Text)) +
                                             (General.GetNullableDecimal(ucotherConsumption.Text) == null ? 0 : decimal.Parse(ucotherConsumption.Text))
                                             );
                            Consumed.Text = total.ToString();
                            if (lblUnitName.Text != "LTR" && lblConversionfactorM3.Text != string.Empty)
                                Consumed.Text = (double.Parse(Consumed.Text) * double.Parse(lblConversionfactorM3.Text)).ToString();
                        }

                        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOilLoadandConsumptionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString()), new Guid(OilTypeCode.Text), General.GetNullableDecimal(OpeningStock.Text)
                            , General.GetNullableDecimal(LoadedCharterer.Text), General.GetNullableDecimal(DischargedCharterer.Text), General.GetNullableDecimal(Consumed.Text), General.GetNullableGuid(OilLoadedConsumptionId.Text));

                    }
                }
            }
        }





    }

    // Meteorology Data
    private void BindMeteorologyData()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportMeteorologyDataList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvMeteorologyData.DataSource = ds;
            gvMeteorologyData.DataBind();
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMeteorologyData_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblValueType = (Label)e.Row.FindControl("lblValueType");
            Label lblShortname = (Label)e.Row.FindControl("lblShortname");

            UserControlMaskedTextBox txtValueDecimal6Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal6Edit");
            UserControlMaskedTextBox txtValueDecimal12Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal12Edit");
            UserControlMaskedTextBox txtValueDecimal18Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal18Edit");
            UserControlMaskedTextBox txtValueDecimal24Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal24Edit");
            UserControlMaskedTextBox txtValueDecimalNext24HrsEdit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimalNext24HrsEdit");

            UserControlDirection ucDirection6Edit = (UserControlDirection)e.Row.FindControl("ucDirection6Edit");
            UserControlDirection ucDirection12Edit = (UserControlDirection)e.Row.FindControl("ucDirection12Edit");
            UserControlDirection ucDirection18Edit = (UserControlDirection)e.Row.FindControl("ucDirection18Edit");
            UserControlDirection ucDirection24Edit = (UserControlDirection)e.Row.FindControl("ucDirection24Edit");
            UserControlDirection ucDirectionNext24HrsEdit = (UserControlDirection)e.Row.FindControl("ucDirectionNext24HrsEdit");

            UserControlSeaCondtion ucSeaCondtion6Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion6Edit");
            UserControlSeaCondtion ucSeaCondtion12Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion12Edit");
            UserControlSeaCondtion ucSeaCondtion18Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion18Edit");
            UserControlSeaCondtion ucSeaCondtion24Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion24Edit");
            UserControlSeaCondtion ucSeaCondtionNext24HrsEdit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtionNext24HrsEdit");
            if (lblValueType.Text == "1")
            {
                ucDirection6Edit.Visible = false;
                ucDirection12Edit.Visible = false;
                ucDirection18Edit.Visible = false;
                ucDirection24Edit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;

                ucSeaCondtion6Edit.Visible = false;
                ucSeaCondtion12Edit.Visible = false;
                ucSeaCondtion18Edit.Visible = false;
                ucSeaCondtion24Edit.Visible = false;
                ucSeaCondtionNext24HrsEdit.Visible = false;

                txtValueDecimal6Edit.Visible = true;
                if (txtValueDecimal6Edit != null)
                    txtValueDecimal6Edit.Text = drv["FLDMETEOROLOGYVALUE"].ToString();

                if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYVALUE"].ToString()) != null)
                {
                    if ((decimal.Parse(drv["FLDMETEOROLOGYVALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYVALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
                        txtValueDecimal6Edit.CssClass = "maxhighlight";
                }


                txtValueDecimal12Edit.Visible = true;
                if (txtValueDecimal12Edit != null)
                    txtValueDecimal12Edit.Text = drv["FLDMETEOROLOGY1200VALUE"].ToString();

                if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGY1200VALUE"].ToString()) != null)
                {
                    if ((decimal.Parse(drv["FLDMETEOROLOGY1200VALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGY1200VALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
                        txtValueDecimal12Edit.CssClass = "maxhighlight";
                }

                txtValueDecimal18Edit.Visible = true;
                if (txtValueDecimal18Edit != null)
                    txtValueDecimal18Edit.Text = drv["FLDMETEOROLOGY1800VALUE"].ToString();

                if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGY1800VALUE"].ToString()) != null)
                {
                    if ((decimal.Parse(drv["FLDMETEOROLOGY1800VALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGY1800VALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
                        txtValueDecimal18Edit.CssClass = "maxhighlight";
                }

                txtValueDecimal24Edit.Visible = true;
                if (txtValueDecimal24Edit != null)
                    txtValueDecimal24Edit.Text = drv["FLDMETEOROLOGY2400VALUE"].ToString();

                if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGY2400VALUE"].ToString()) != null)
                {
                    if ((decimal.Parse(drv["FLDMETEOROLOGY2400VALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGY2400VALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
                        txtValueDecimal24Edit.CssClass = "maxhighlight";
                }
                txtValueDecimalNext24HrsEdit.Visible = true;
                if (txtValueDecimalNext24HrsEdit != null)
                    txtValueDecimalNext24HrsEdit.Text = drv["FLDMETEOROLOGYNEXT24HRS"].ToString();

                if (lblShortname != null)
                {
                    if (lblShortname.Text == "WNS")
                    {
                        if (General.GetNullableDecimal(drv["FLDMINFOREWINDSPEED"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXFOREWINDSPEED"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYNEXT24HRS"].ToString()) != null)
                        {
                            if ((decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) < (decimal.Parse(drv["FLDMINFOREWINDSPEED"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) > (decimal.Parse(drv["FLDMAXFOREWINDSPEED"].ToString())))
                                txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
                        }
                    }
                    else if (lblShortname.Text == "SWH")
                    {
                        if (General.GetNullableDecimal(drv["FLDMINFORESWELLHEIGHT"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXFORESWELLHEIGHT"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYNEXT24HRS"].ToString()) != null)
                        {
                            if ((decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) < (decimal.Parse(drv["FLDMINFORESWELLHEIGHT"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) > (decimal.Parse(drv["FLDMAXFORESWELLHEIGHT"].ToString())))
                                txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
                        }
                    }
                    else
                    {
                        if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYNEXT24HRS"].ToString()) != null)
                        {
                            if ((decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
                                txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
                        }
                    }
                }
            }
            if (lblValueType.Text == "2")
            {
                ucDirection6Edit.Visible = false;
                ucDirection12Edit.Visible = false;
                ucDirection18Edit.Visible = false;
                ucDirection24Edit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;

                ucSeaCondtion6Edit.Visible = true;
                if (ucSeaCondtion6Edit != null)
                    ucSeaCondtion6Edit.SelectedSeaCondition = drv["FLDMETEOROLOGYVALUE"].ToString();

                ucSeaCondtion12Edit.Visible = true;
                if (ucSeaCondtion12Edit != null)
                    ucSeaCondtion12Edit.SelectedSeaCondition = drv["FLDMETEOROLOGY1200VALUE"].ToString();

                ucSeaCondtion18Edit.Visible = true;
                if (ucSeaCondtion18Edit != null)
                    ucSeaCondtion18Edit.SelectedSeaCondition = drv["FLDMETEOROLOGY1800VALUE"].ToString();

                ucSeaCondtion24Edit.Visible = true;
                if (ucSeaCondtion24Edit != null)
                    ucSeaCondtion24Edit.SelectedSeaCondition = drv["FLDMETEOROLOGY2400VALUE"].ToString();

                ucSeaCondtionNext24HrsEdit.Visible = true;
                if (ucSeaCondtionNext24HrsEdit != null)
                    ucSeaCondtionNext24HrsEdit.SelectedSeaCondition = drv["FLDMETEOROLOGYNEXT24HRS"].ToString();

                txtValueDecimal6Edit.Visible = false;
                txtValueDecimal12Edit.Visible = false;
                txtValueDecimal18Edit.Visible = false;
                txtValueDecimal24Edit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblValueType.Text == "3")
            {
                ucDirection6Edit.Visible = true;
                if (ucDirection6Edit != null)
                    ucDirection6Edit.SelectedDirection = drv["FLDMETEOROLOGYVALUE"].ToString();

                ucDirection12Edit.Visible = true;
                if (ucDirection12Edit != null)
                    ucDirection12Edit.SelectedDirection = drv["FLDMETEOROLOGY1200VALUE"].ToString();

                ucDirection18Edit.Visible = true;
                if (ucDirection18Edit != null)
                    ucDirection18Edit.SelectedDirection = drv["FLDMETEOROLOGY1800VALUE"].ToString();

                ucDirection24Edit.Visible = true;
                if (ucDirection24Edit != null)
                    ucDirection24Edit.SelectedDirection = drv["FLDMETEOROLOGY2400VALUE"].ToString();

                ucDirectionNext24HrsEdit.Visible = true;
                if (ucDirectionNext24HrsEdit != null)
                    ucDirectionNext24HrsEdit.SelectedDirection = drv["FLDMETEOROLOGYNEXT24HRS"].ToString();

                ucSeaCondtion6Edit.Visible = false;
                ucSeaCondtion12Edit.Visible = false;
                ucSeaCondtion18Edit.Visible = false;
                ucSeaCondtion24Edit.Visible = false;
                ucSeaCondtionNext24HrsEdit.Visible = false;

                txtValueDecimal6Edit.Visible = false;
                txtValueDecimal12Edit.Visible = false;
                txtValueDecimal18Edit.Visible = false;
                txtValueDecimal24Edit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }

            if (lblShortname != null && lblShortname.Text == "SW")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblShortname != null && lblShortname.Text == "AIR")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblShortname != null && lblShortname.Text == "VIS")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblShortname != null && lblShortname.Text == "BAR")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
        }

    }

    protected void gvMeteorologyData_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }

    protected void gvMeteorologyData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    string MeteorologyDId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMeteorologyDId")).Text;
                    if (MeteorologyDId != "")
                    {
                        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportMeteorologyDelete(new Guid(MeteorologyDId));
                    }
                }
                BindMeteorologyData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMeteorologyData_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        int Ei = _gridView.EditIndex;
        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindMeteorologyData();
    }

    protected void gvMeteorologyData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindMeteorologyData();
    }

    protected void gvMeteorologyData_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }

            string MeteorologyId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblMeteorologyId")).Text;
            string MeteorologyValue = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtMeteorologyValueEdit")).Text;
            string MeteorologyDataId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblMeteorologyDataId")).Text;

            if (!IsValidMeteorologyData(MeteorologyValue))
            {
                ucError.Visible = true;
                return;
            }

            //PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportMeteorologyDataInsert(new Guid(MeteorologyId), MeteorologyValue
            //    , new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), General.GetNullableGuid(MeteorologyDataId));

            _gridView.EditIndex = -1;
            BindMeteorologyData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMeteorologyData(string MeteorologyValue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (MeteorologyValue.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";

        return (!ucError.IsError);
    }

    private void UpdateMeteorologyData()
    {
        foreach (GridDataItem gvr in gvMeteorologyData.Items)
        {
            if (gvMeteorologyData.Items.Count > 0)
            {
                RadLabel MeteorologyId = (RadLabel)gvr.FindControl("lblMeteorologyId");
                RadTextBox MeteorologyValue = (RadTextBox)gvr.FindControl("txtMeteorologyValueEdit");
                RadLabel MeteorologyDataId = (RadLabel)gvr.FindControl("lblMeteorologyDataId");
                RadLabel lblValueType = (RadLabel)gvr.FindControl("lblValueType");

                UserControlNumber txtValueDecimal6Edit = (UserControlNumber)gvr.FindControl("txtValueDecimal6Edit");
                UserControlDirection ucDirection6Edit = (UserControlDirection)gvr.FindControl("ucDirection6Edit");
                UserControlSeaCondtion ucSeaCondtion6Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion6Edit");

                UserControlNumber txtValueDecimal12Edit = (UserControlNumber)gvr.FindControl("txtValueDecimal12Edit");
                UserControlDirection ucDirection12Edit = (UserControlDirection)gvr.FindControl("ucDirection12Edit");
                UserControlSeaCondtion ucSeaCondtion12Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion12Edit");

                UserControlNumber txtValueDecimal18Edit = (UserControlNumber)gvr.FindControl("txtValueDecimal18Edit");
                UserControlDirection ucDirection18Edit = (UserControlDirection)gvr.FindControl("ucDirection18Edit");
                UserControlSeaCondtion ucSeaCondtion18Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion18Edit");

                UserControlNumber txtValueDecimal24Edit = (UserControlNumber)gvr.FindControl("txtValueDecimal24Edit");
                UserControlDirection ucDirection24Edit = (UserControlDirection)gvr.FindControl("ucDirection24Edit");
                UserControlSeaCondtion ucSeaCondtion24Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion24Edit");

                UserControlNumber txtValueDecimalNext24HrsEdit = (UserControlNumber)gvr.FindControl("txtValueDecimalNext24HrsEdit");
                UserControlDirection ucDirectionNext24HrsEdit = (UserControlDirection)gvr.FindControl("ucDirectionNext24HrsEdit");
                UserControlSeaCondtion ucSeaCondtionNext24HrsEdit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtionNext24HrsEdit");


                string MeteorologyValue6 = string.Empty;
                string MeteorologyValue12 = string.Empty;
                string MeteorologyValue18 = string.Empty;
                string MeteorologyValue24 = string.Empty;
                string MeteorologyValueNext24Hrs = string.Empty;

                if (lblValueType.Text == "1")
                {
                    MeteorologyValue6 = General.GetNullableDecimal(txtValueDecimal6Edit.Text) == null ? null : txtValueDecimal6Edit.Text;
                    MeteorologyValue12 = General.GetNullableDecimal(txtValueDecimal12Edit.Text) == null ? null : txtValueDecimal12Edit.Text;
                    MeteorologyValue18 = General.GetNullableDecimal(txtValueDecimal18Edit.Text) == null ? null : txtValueDecimal18Edit.Text;
                    MeteorologyValue24 = General.GetNullableDecimal(txtValueDecimal24Edit.Text) == null ? null : txtValueDecimal24Edit.Text;
                    MeteorologyValueNext24Hrs = General.GetNullableDecimal(txtValueDecimalNext24HrsEdit.Text) == null ? null : txtValueDecimalNext24HrsEdit.Text;
                }
                if (lblValueType.Text == "2")
                {
                    MeteorologyValue6 = General.GetNullableGuid(ucSeaCondtion6Edit.SelectedSeaCondition) == null ? null : ucSeaCondtion6Edit.SelectedSeaCondition;
                    MeteorologyValue12 = General.GetNullableGuid(ucSeaCondtion12Edit.SelectedSeaCondition) == null ? null : ucSeaCondtion12Edit.SelectedSeaCondition;
                    MeteorologyValue18 = General.GetNullableGuid(ucSeaCondtion18Edit.SelectedSeaCondition) == null ? null : ucSeaCondtion18Edit.SelectedSeaCondition;
                    MeteorologyValue24 = General.GetNullableGuid(ucSeaCondtion24Edit.SelectedSeaCondition) == null ? null : ucSeaCondtion24Edit.SelectedSeaCondition;
                    MeteorologyValueNext24Hrs = General.GetNullableGuid(ucSeaCondtionNext24HrsEdit.SelectedSeaCondition) == null ? null : ucSeaCondtionNext24HrsEdit.SelectedSeaCondition;
                }
                if (lblValueType.Text == "3")
                {
                    MeteorologyValue6 = General.GetNullableGuid(ucDirection6Edit.SelectedDirection) == null ? null : ucDirection6Edit.SelectedDirection;
                    MeteorologyValue12 = General.GetNullableGuid(ucDirection12Edit.SelectedDirection) == null ? null : ucDirection12Edit.SelectedDirection;
                    MeteorologyValue18 = General.GetNullableGuid(ucDirection18Edit.SelectedDirection) == null ? null : ucDirection18Edit.SelectedDirection;
                    MeteorologyValue24 = General.GetNullableGuid(ucDirection24Edit.SelectedDirection) == null ? null : ucDirection24Edit.SelectedDirection;
                    MeteorologyValueNext24Hrs = General.GetNullableGuid(ucDirectionNext24HrsEdit.SelectedDirection) == null ? null : ucDirectionNext24HrsEdit.SelectedDirection;

                }


                if (MeteorologyId != null && MeteorologyDataId != null)
                {

                    PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportMeteorologyDataInsert(new Guid(MeteorologyId.Text), MeteorologyValue6, MeteorologyValue12, MeteorologyValue18, MeteorologyValue24
                , new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), General.GetNullableGuid(MeteorologyDataId.Text), MeteorologyValueNext24Hrs);


                }
            }
        }
    }

    // Vessel's Movements
    private void BindVesselMovements()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvVesselMovements.DataSource = ds;
            gvVesselMovements.DataBind();

            GridFooterItem footerItem = (GridFooterItem)gvVesselMovements.MasterTableView.GetItems(GridItemType.Footer)[0];
            // Button btn = (Button)footerItem.FindControl("Button1");
            RadComboBox ActivityAdd = (RadComboBox)footerItem.FindControl("ddlActivityAdd");
            if (ActivityAdd != null)
            {
                DataSet ds1 = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
                ActivityAdd.DataSource = ds1;
                ActivityAdd.DataBind();
                //ct.Items.Insert(0, new ListItem("--Select--", ""));

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselMovements_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        Table gridTable = (Table)gvVesselMovements.Controls[0];
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            DropDownList ct = (DropDownList)e.Row.FindControl("ddlActivityEdit");
            if (ct != null)
            {
                DataSet ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
                ct.DataSource = ds;
                ct.DataBind();
                ct.Items.Insert(0, new ListItem("--Select--", ""));
                ct.SelectedValue = drv["FLDOPERATIONALTASKID"].ToString();
            }
            nextOperational = drv["FLDOPERATIONALTASKID"].ToString();
            
            
            //Label lblShortName = (Label)e.Row.FindControl("lblShortName");
            //Label lblTimeDuration = (Label)e.Row.FindControl("lblTimeDuration");
            //string TimeDuration = "";
            //if (lblShortName != null && lblShortName.Text == "SS")
            //{
            //    double exposurehour = 0;
            //    if ((rbnhourchange.SelectedValue) == "1" || (rbnhourchange.SelectedValue) == "2")
            //    {
            //        if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "1")
            //        {
            //            if (lblTimeDuration != null)
            //            {
            //                TimeDuration = (TimeSpan.Parse(lblTimeDuration.Text) - TimeSpan.Parse("00:30:00")).ToString();
            //            }
            //        }
            //        if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "2")
            //        {
            //            if (lblTimeDuration != null)
            //            {
            //                TimeDuration = (TimeSpan.Parse(lblTimeDuration.Text) - TimeSpan.Parse("01:00:00")).ToString();
            //            }
            //        }
            //        if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "1")
            //        {
            //            if (lblTimeDuration != null)
            //            {
            //                TimeDuration = (TimeSpan.Parse(lblTimeDuration.Text) + TimeSpan.Parse("00:30:00")).ToString();
            //            }
            //        }
            //        if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "2")
            //        {
            //            if (lblTimeDuration != null)
            //            {
            //                TimeDuration = (TimeSpan.Parse(lblTimeDuration.Text) + TimeSpan.Parse("01:00:00")).ToString();
            //            }
            //        }
            //        lblTimeDuration.Text = TimeDuration.Substring(0, 5);
            //    }
            //}
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DropDownList at = (DropDownList)e.Row.FindControl("ddlActivityAdd");
            if (at != null)
            {
                DataSet ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
                at.DataSource = ds;
                at.DataBind();
                at.Items.Insert(0, new ListItem("--Select--", ""));
                at.SelectedValue = nextOperational;
            }


        }
    }

    protected void gvVesselMovements_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindVesselMovements();
    }

    protected void gvVesselMovements_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselMovements_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindVesselMovements();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvVesselMovements_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        if (Session["MIDNIGHTREPORTID"] == null)
    //        {
    //            ucError.ErrorMessage = "You cannot save. Please save the header details first.";
    //            ucError.Visible = true;
    //            return;
    //        }

    //        string FromTime = (((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("txtFromTime")).Text.Trim() == "__:__") ? string.Empty : ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("txtFromTime")).Text;
    //        string Activity = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text;
    //        string ddlActivity = ((RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlActivityEdit")).SelectedValue;
    //        string ActivityId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblVesselMovementsEditId")).Text;

    //        string fromtime = (FromTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : FromTime;
    //        string TimeDuration = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTimeDurationEdit")).Text;
    //        if (!IsValidVesselMovements(fromtime, ddlActivity))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
    //                , General.GetNullableGuid(ddlActivity), General.GetNullableString(Activity)
    //                , General.GetNullableDateTime(ViewState["REPORTDATE"].ToString() + " " + fromtime), General.GetNullableGuid(ActivityId)
    //                , General.GetNullableDecimal(TimeDuration));

    //        _gridView.EditIndex = -1;
    //        BindVesselMovements();
    //        BindOperationalTimeSummary();
    //        UpdateOperationalSummaryData();
    //        EditMidNightReport();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private void UpdateOperationalSummaryData()
    {
        foreach (GridDataItem gvr in gvOperationalTimeSummary.Items)
        {

            RadLabel OperationalTaskId = (RadLabel)gvr.FindControl("lblOperationalTaskId");
            RadLabel TimeDuration = (RadLabel)gvr.FindControl("lblTimeDurationEdit");
            UserControlDecimal FuelConsumption = (UserControlDecimal)gvr.FindControl("ucFuelConsumptionEdit");
            RadLabel OperationalTimeSummaryId = (RadLabel)gvr.FindControl("lblOperationalTimeSummaryEditId");
            UserControlDecimal distance = (UserControlDecimal)gvr.FindControl("ucSeaStreamDistanceEdit");

            if (OperationalTaskId != null)
            {
                if (OperationalTaskId.Text != "")
                {
                    PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                        , new Guid(OperationalTaskId.Text), General.GetNullableDecimal(TimeDuration.Text), General.GetNullableDecimal(FuelConsumption.Text)
                        , General.GetNullableGuid(OperationalTimeSummaryId.Text), General.GetNullableDecimal(distance.Text));


                }
            }

        }
        EditMidNightReport();
    }

    //protected void gvVesselMovements_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (Session["MIDNIGHTREPORTID"] == null)
    //            {
    //                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
    //                ucError.Visible = true;
    //                return;
    //            }

    //            string FromTime = (((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text.Trim() == "__:__") ? string.Empty : ((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text;
    //            string Activity = ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text;
    //            string ddlActivity = ((DropDownList)_gridView.FooterRow.FindControl("ddlActivityAdd")).SelectedValue;

    //            string fromtime = (FromTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : FromTime;


    //            string TimeDuration = ((Label)_gridView.FooterRow.FindControl("lblTimeDurationAdd")).Text;
    //            if (!IsValidVesselMovements(fromtime, ddlActivity))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
    //                , General.GetNullableGuid(ddlActivity), General.GetNullableString(Activity)
    //                , General.GetNullableDateTime(ViewState["REPORTDATE"].ToString() + " " + fromtime), General.GetNullableGuid("")
    //                , General.GetNullableDecimal(TimeDuration));

    //            BindVesselMovements();
    //            BindOperationalTimeSummary();
    //            UpdateOperationalSummaryData();
    //        }

    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            if (Session["MIDNIGHTREPORTID"] != null)
    //            {
    //                string VesselMovementsId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselMovementsId")).Text;
    //                if (VesselMovementsId != "")
    //                {
    //                    PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsDelete(new Guid(VesselMovementsId));
    //                }
    //            }
    //            BindVesselMovements();
    //            BindOperationalTimeSummary();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private bool IsValidVesselMovements(string fromtime, string activity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(activity) == null)
            ucError.ErrorMessage = "Activity is required.";
        if (fromtime.Trim().Equals(""))
            ucError.ErrorMessage = "From Time is required.";
        if (General.GetNullableDateTime(ViewState["REPORTDATE"].ToString() + " " + fromtime) == null)
            ucError.ErrorMessage = "Invalid From Time.";
        return (!ucError.IsError);
    }


    // Passenger

    // Operational Time Summary
    private void BindOperationalTimeSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            gvOperationalTimeSummary.DataSource = ds;
            gvOperationalTimeSummary.DataBind();

            decimal total = ((General.GetNullableDecimal(txtgrandtotal.Text) == null ? 0 : decimal.Parse(txtgrandtotal.Text)) +
                          (General.GetNullableDecimal(txtAE1Consumption.Text) == null ? 0 : decimal.Parse(txtAE1Consumption.Text)) +
                          (General.GetNullableDecimal(txtAE2Consumption.Text) == null ? 0 : decimal.Parse(txtAE2Consumption.Text)) +
                          (General.GetNullableDecimal(ucotherConsumption.Text) == null ? 0 : decimal.Parse(ucotherConsumption.Text))
                         );

            GridFooterItem footerItem = (GridFooterItem)gvOperationalTimeSummary.MasterTableView.GetItems(GridItemType.Footer)[0];
            // Button btn = (Button)footerItem.FindControl("Button1");
            RadLabel TotalFOC = (RadLabel)footerItem.FindControl("lblTotalFOC");
            RadLabel lblTotalTime = (RadLabel)footerItem.FindControl("lblTotalTime");
            RadLabel lblTotalDistance = (RadLabel)footerItem.FindControl("lblTotalDistance");
            if ((General.GetNullableDecimal(ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString()) == null ? 0 : decimal.Parse(ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString())) - total != 0)
            {
                TotalFOC.BackColor = System.Drawing.Color.OrangeRed;


            }
            else
            {
                TotalFOC.BorderColor = System.Drawing.Color.Black;
            }


            TotalFOC.Text = ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString();
            lblTotalTime.Text = ds.Tables[0].Rows[0]["FLDTOTALTIMEDURATION"].ToString();
            lblTotalDistance.Text = ds.Tables[0].Rows[0]["FLDTOTALDISTANCE"].ToString();
            txtDPTime.Text = ds.Tables[0].Rows[0]["FLDTOTALDPTIME"].ToString();
            txtDPFuelConsumption.Text = ds.Tables[0].Rows[0]["FLDTOTALDPFUELCONSUPTION"].ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOperationalTimeSummary_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            UserControlMaskedTextBox ucFuelConsHrEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucFuelConsHrEdit");
            UserControlMaskedTextBox ucFuelConsumptionEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucFuelConsumptionEdit");
            Label lblTimeDurationEdit = (Label)e.Row.FindControl("lblTimeDurationEdit");
            Label lblDistanceApplicable = (Label)e.Row.FindControl("lblDistanceApplicable");
            UserControlMaskedTextBox ucSeaStreamDistanceEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucSeaStreamDistanceEdit");
            UserControlMaskedTextBox ucSpeedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucSpeedEdit");
            
            if (ucFuelConsumptionEdit != null && lblTimeDurationEdit != null)
            {
                if (General.GetNullableDecimal(ucFuelConsumptionEdit.Text) != null && General.GetNullableDecimal(ucFuelConsumptionEdit.Text) != 0)
                {
                    decimal timeDuration = Convert.ToDecimal(lblTimeDurationEdit.Text);
                    if (ucFuelConsHrEdit != null)
                        ucFuelConsHrEdit.Text = (Convert.ToDecimal(ucFuelConsumptionEdit.Text) / (Math.Floor(timeDuration) + ((timeDuration - Math.Floor(timeDuration)) * 100 / 60))).ToString();
                }
            }

            if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(ucFuelConsHrEdit.Text) != null)
            {
                if ((decimal.Parse(ucFuelConsHrEdit.Text)) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(ucFuelConsHrEdit.Text)) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
                    ucFuelConsHrEdit.CssClass = "maxhighlight";
            }
            if (lblDistanceApplicable != null && ucSeaStreamDistanceEdit != null)
            {
                if (lblDistanceApplicable.Text == "0")
                {
                    ucSeaStreamDistanceEdit.CssClass = "readonlytextbox";
                    ucSeaStreamDistanceEdit.ReadOnly = true;
                }
                if (ucSeaStreamDistanceEdit != null && General.GetNullableDecimal(ucSeaStreamDistanceEdit.Text) != null)
                {
                    decimal timeDuration = Convert.ToDecimal(lblTimeDurationEdit.Text);
                    ucSpeedEdit.Text = (Convert.ToDecimal(ucSeaStreamDistanceEdit.Text) / (Math.Floor(timeDuration) + ((timeDuration - Math.Floor(timeDuration)) * 100 / 60))).ToString();

                }
            }

            if (General.GetNullableDecimal(ucSpeedEdit.Text) != null)
            {
                if (General.GetNullableDecimal(drv["FLDMINVALUESPD"].ToString()) != null && ((decimal.Parse(ucSpeedEdit.Text)) < (decimal.Parse(drv["FLDMINVALUESPD"].ToString()))))
                {
                    ucSpeedEdit.CssClass = "maxhighlight";
                }
                if (General.GetNullableDecimal(drv["FLDMAXVALUESPD"].ToString()) != null && (decimal.Parse(ucSpeedEdit.Text)) > (decimal.Parse(drv["FLDMAXVALUESPD"].ToString())))
                {
                    ucSpeedEdit.CssClass = "maxhighlight";
                }
            }
        } 
    }

    protected void gvOperationalTimeSummary_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }

    protected void gvOperationalTimeSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    string OperationalTimeSummaryId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTimeSummaryId")).Text;
                    if (OperationalTimeSummaryId != "")
                    {
                        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryDelete(new Guid(OperationalTimeSummaryId));
                    }
                }
                BindOperationalTimeSummary();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationalTimeSummary_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        int Ei = _gridView.EditIndex;
        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindOperationalTimeSummary();
    }

    protected void gvOperationalTimeSummary_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindOperationalTimeSummary();
    }

    protected void gvOperationalTimeSummary_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }

            string OperationalTaskId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTaskId")).Text;
            string TimeDuration = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTimeEdit")).Text;
            string FuelConsumption = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucFuelConsumptionEdit")).Text;
            string OperationalTimeSummaryId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTimeSummaryEditId")).Text;
            string distance = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucSeaStreamDistanceEdit")).Text;

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                , new Guid(OperationalTaskId), General.GetNullableDecimal(TimeDuration), General.GetNullableDecimal(FuelConsumption)
                , General.GetNullableGuid(OperationalTimeSummaryId), General.GetNullableDecimal(distance));

            _gridView.EditIndex = -1;
            BindOperationalTimeSummary();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetFieldRange()
    {
        DataSet ds = PhoenixRegistersDMRRangeConfig.ListDMRRangeConfig(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            1);

        if (ds.Tables[0].Rows.Count > 0)
        {
            decimal? minvalue = null;
            decimal? maxvalue = null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                minvalue = General.GetNullableDecimal(dr["FLDMINVALUE"].ToString());
                maxvalue = General.GetNullableDecimal(dr["FLDMAXVALUE"].ToString());

                switch (dr["FLDCOLUMNNAME"].ToString())
                {
                    //case "FLDSWELL":
                    //    {
                    //        if (General.GetNullableDecimal(ucSwell.Text) < minvalue || General.GetNullableDecimal(ucSwell.Text) > maxvalue)
                    //            ucSwell.CssClass = "maxhighlight";
                    //        else
                    //            ucSwell.CssClass = "input";
                    //        break;
                    //    }
                    case "FLDAVGSPEED":
                        {
                            if (General.GetNullableDecimal(ucAvgSpeed.Text) < minvalue || General.GetNullableDecimal(ucAvgSpeed.Text) > maxvalue)
                                ucAvgSpeed.CssClass = "maxhighlight";
                            else
                                ucAvgSpeed.CssClass = "readonlytextbox";
                            break;
                        }
                    case "FLDPOBCLIENT":
                        {
                            if (General.GetNullableDecimal(txtPOBClient.Text) < minvalue || General.GetNullableDecimal(txtPOBClient.Text) > maxvalue)
                                txtPOBClient.CssClass = "maxhighlight";
                            else
                                txtPOBClient.CssClass = "readonlytextbox";
                            break;
                        }
                    case "FLDTOTALNOOFPERSONS":
                        {
                            if (General.GetNullableDecimal(txtTotalOB.Text) < minvalue || General.GetNullableDecimal(txtTotalOB.Text) > maxvalue)
                                txtTotalOB.CssClass = "maxhighlight";
                            else
                                txtTotalOB.CssClass = "readonlytextbox";
                            break;
                        }

                    //case "FLDFORECASTWINDSPEED":
                    //    {
                    //        if (General.GetNullableDecimal(ucWindSpeedNext.Text) < minvalue || General.GetNullableDecimal(ucWindSpeedNext.Text) > maxvalue)
                    //            ucWindSpeedNext.CssClass = "maxhighlight";
                    //        else
                    //            ucWindSpeedNext.CssClass = "input";
                    //        break;
                    //    }
                    //case "FLDFORECASTSWELLHEIGHT":
                    //    {
                    //        if (General.GetNullableDecimal(ucSwellHeightNext.Text) < minvalue || General.GetNullableDecimal(ucSwellHeightNext.Text) > maxvalue)
                    //            ucSwellHeightNext.CssClass = "maxhighlight";
                    //        else
                    //            ucSwellHeightNext.CssClass = "input";
                    //        break;
                    //    }
                    case "FLDDRAFTAVERAGE":
                        {
                            if (General.GetNullableDecimal(ucAverage.Text) < minvalue || General.GetNullableDecimal(ucAverage.Text) > maxvalue)
                                ucAverage.CssClass = "maxhighlight";
                            else
                                ucAverage.CssClass = "readonlytextbox";
                            break;
                        }
                    case "FLDME1FOCONS":
                        {
                            if (General.GetNullableDecimal(txtme1Total.Text) < minvalue || General.GetNullableDecimal(txtme1Total.Text) > maxvalue)
                                txtme1Total.CssClass = "maxhighlight";
                            else
                                txtme1Total.CssClass = "readonlytextbox";
                            break;
                        }
                    case "FLDME2FOCONS":
                        {
                            if (General.GetNullableDecimal(txtme2Total.Text) < minvalue || General.GetNullableDecimal(txtme2Total.Text) > maxvalue)
                                txtme2Total.CssClass = "maxhighlight";
                            else
                                txtme2Total.CssClass = "readonlytextbox";
                            break;
                        }
                    case "FLDAE1FOCONS":
                        {
                            if (General.GetNullableDecimal(txtAE1Consumption.Text) < minvalue || General.GetNullableDecimal(txtAE1Consumption.Text) > maxvalue)
                                txtAE1Consumption.CssClass = "maxhighlight";
                            else
                                txtAE1Consumption.CssClass = "readonlytextbox";
                            break;
                        }
                    case "FLDAE2FOCONS":
                        {
                            if (General.GetNullableDecimal(txtAE2Consumption.Text) < minvalue || General.GetNullableDecimal(txtAE2Consumption.Text) > maxvalue)
                                txtAE2Consumption.CssClass = "maxhighlight";
                            else
                                txtAE2Consumption.CssClass = "readonlytextbox";
                            break;
                        }
                }
            }
        }
    }

    protected void VesselStatus(object sender, EventArgs e)
    {
        txtETADate.Text = "";
       // txtETATime.Text = "";
        txtETDDate.Text = "";
        //txtETDTime.Text = "";
        txtArrivalDate.Text = "";
       // txtArrivalTime.Text = "";
        txtDepartureDate.Text = "";
        //txtDepartureTime.Text = "";
        ChangeVesselStatus();
    }
    private void BindPlannedActivity()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPlannedActivityList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvPlannedActivity.DataSource = ds;
            gvPlannedActivity.DataBind();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedActivity_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindPlannedActivity();
    }

    protected void gvPlannedActivity_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindPlannedActivity();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateLookAheadActivity()
    {
        foreach (GridDataItem gvr in gvPlannedActivity.Items)
        {
            RadTextBox Activity = (RadTextBox)gvr.FindControl("txtActivityEdit");
            RadLabel ActivityId = (RadLabel)gvr.FindControl("lblPlannedActivityEditId");
            if (Activity != null && ActivityId != null)
            {
                PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPlannedActivityUpdate(int.Parse(ViewState["VESSELID"].ToString())
                                                                                                    , new Guid(Session["MIDNIGHTREPORTID"].ToString()), General.GetNullableString(Activity.Text)
                                                                                                    , General.GetNullableGuid(ActivityId.Text));
            }
        }
    }

    protected void gvPlannedActivity_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }

            BindPlannedActivity();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedActivity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidPlannedActivity(string activity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (activity.Trim().Equals(""))
            ucError.ErrorMessage = "Activity is required.";

        return (!ucError.IsError);
    }
    private void BindDueAudits()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportDueAudits(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvShipAudit.DataSource = ds;
            gvShipAudit.DataBind();

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDueShipTasks()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportDueShipTasks(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvShipTasks.DataSource = ds;
            gvShipTasks.DataBind();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDueCertificates()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportDueCertificates(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvCertificates.DataSource = ds;
            gvCertificates.DataBind();
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindMachineryFailures()
    {
        //try
        //{
        //    DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportMachineryFailures(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        gvMachineryFailure.DataSource = ds;
        //        gvMachineryFailure.DataBind();
        //    }
        //    else
        //    {
        //        ShowNoRecordsFound(ds.Tables[0], gvMachineryFailure);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    private void BindExternalInspection()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspection(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                                       , General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvExternalInspection.DataSource = ds;
            //gvExternalInspection.DataBind();
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void ChangeVesselStatus()
    {
        if (ddlvesselstatus.SelectedValue == "Dummy")
        {
            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETDDate.ReadOnly = true;
            //txtETDTime.ReadOnly = "true";
            txtETDDate.CssClass = "readonlytextbox";
            //txtETDTime.CssClass = "readonlytextbox";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
           // txtETATime.ReadOnly = "true";
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
          //  txtETATime.CssClass = "readonlytextbox";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            //txtArrivalTime.CssClass = "input";
            //txtArrivalTime.ReadOnly = "false";
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
           // txtDepartureTime.CssClass = "input";
           // txtDepartureTime.ReadOnly = "false";

            txtDepartureDate.Enabled = true;
           // txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = true;
            //txtArrivalTime.Enabled = true;
            txtETADate.Enabled = true;
           // txtETATime.Enabled = true;
            txtETDDate.Enabled = true;
           // txtETDTime.Enabled = true;

        }
        if (ddlvesselstatus.SelectedValue == "1")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETDDate.ReadOnly = false;
            //txtETDTime.ReadOnly = "false";
            txtETDDate.CssClass = "input";
            //txtETDTime.CssClass = "input";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            //txtETATime.ReadOnly = "true";
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            //txtETATime.CssClass = "readonlytextbox";

            ddlETALocation.Text = "";
            ddlETDLocation.Text = "";
            txtETADate.Text = "";
            //txtETATime.Text = "";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            //txtArrivalTime.CssClass = "input";
            //txtArrivalTime.ReadOnly = "false";
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
           // txtDepartureTime.CssClass = "readonlytextbox";
            //txtDepartureTime.ReadOnly = "true";

            txtDepartureDate.Enabled = false;
            //txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = true;
           // txtArrivalTime.Enabled = true;
            txtETADate.Enabled = false;
            //txtETATime.Enabled = false;
            txtETDDate.Enabled = true;
           // txtETDTime.Enabled = true;
        }
        if (ddlvesselstatus.SelectedValue == "2")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETADate.ReadOnly = false;
            //txtETATime.ReadOnly = "false";
            txtETADate.CssClass = "input";
            //txtETATime.CssClass = "input";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETDDate.ReadOnly = true;
           // txtETDTime.ReadOnly = "true";
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETDDate.CssClass = "readonlytextbox";
           // txtETDTime.CssClass = "readonlytextbox";

            ddlETALocation.Text = "";
            ddlETDLocation.Text = "";
            txtETDDate.Text = "";
           // txtETDTime.Text = "";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            //txtArrivalTime.CssClass = "readonlytextbox";
           // txtArrivalTime.ReadOnly = "true";
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
           // txtDepartureTime.CssClass = "input";
            //txtDepartureTime.ReadOnly = "false";

            txtDepartureDate.Enabled = true;
            //txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = false;
            //txtArrivalTime.Enabled = false;
            txtETADate.Enabled = true;
           // txtETATime.Enabled = true;
            txtETDDate.Enabled = false;
            //txtETDTime.Enabled = false;

        }
        if (ddlvesselstatus.SelectedValue == "3")
        {
            ddlETALocation.Enabled = true;
            txtETADate.ReadOnly = false;
           // txtETATime.ReadOnly = "false";
            ddlETALocation.CssClass = "input_mandatory";
            txtETADate.CssClass = "input_mandatory";
           // txtETATime.CssClass = "input_mandatory";
            txtLocation.CssClass = "input_mandatory";
            txtLocation.ReadOnly = false;

            ucPort.Enabled = false;
            txtETDDate.ReadOnly = true;
            //txtETDTime.ReadOnly = "true";
            ddlETDLocation.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETDDate.CssClass = "readonlytextbox";
           // txtETDTime.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";

            ucPort.Text = "";
            txtETDDate.Text = "";
            //txtETDTime.Text = "";
            ddlETDLocation.Text = "";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            //txtArrivalTime.CssClass = "readonlytextbox";
            //txtArrivalTime.ReadOnly = "true";
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            //txtDepartureTime.CssClass = "input";
            //txtDepartureTime.ReadOnly = "false";

            txtDepartureDate.Enabled = true;
            //txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = false;
           // txtArrivalTime.Enabled = false;
            txtETADate.Enabled = true;
           // txtETATime.Enabled = true;
            txtETDDate.Enabled = false;
            //txtETDTime.Enabled = false;

        }
        if (ddlvesselstatus.SelectedValue == "4")
        {
            ddlETDLocation.Enabled = true;
            txtETDDate.ReadOnly = false;
           // txtETDTime.ReadOnly = "false";
            //txtETDTime.CssClass = "input";
            txtETDDate.CssClass = "input";
            ddlETDLocation.CssClass = "input_mandatory";
            txtLocation.CssClass = "input_mandatory";
            txtLocation.ReadOnly = false;

            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETADate.ReadOnly = true;
            //txtETATime.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            ddlETALocation.Enabled = false;
            ddlETALocation.CssClass = "readonlytextbox";

            ucPort.Text = "";
           // txtETATime.Text = "";
            txtETADate.Text = "";
            ddlETALocation.Text = "";

            ddlInstalationType.CssClass = "input";
            ddlInstalationType.Enabled = true;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            //txtArrivalTime.CssClass = "input";
            //txtArrivalTime.ReadOnly = "false";
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
           // txtDepartureTime.CssClass = "readonlytextbox";
           // txtDepartureTime.ReadOnly = "true";

            txtDepartureDate.Enabled = false;
           // txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = true;
           // txtArrivalTime.Enabled = true;
            txtETADate.Enabled = false;
            //txtETATime.Enabled = false;
            txtETDDate.Enabled = true;
            //txtETDTime.Enabled = true;
        }
        if (ddlvesselstatus.SelectedValue == "5")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETDDate.ReadOnly = true;
           // txtETDTime.ReadOnly = "true";
            txtETDDate.CssClass = "readonlytextbox";
          //  txtETDTime.CssClass = "readonlytextbox";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            //txtETATime.ReadOnly = "true";
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
           // txtETATime.CssClass = "readonlytextbox";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
           // txtArrivalTime.CssClass = "readonlytextbox";
           // txtArrivalTime.ReadOnly = "true";
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
          //  txtDepartureTime.CssClass = "readonlytextbox";
           // txtDepartureTime.ReadOnly = "true";

            txtDepartureDate.Enabled = false;
           // txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = false;
           // txtArrivalTime.Enabled = false;
            txtETADate.Enabled = false;
           // txtETATime.Enabled = false;
            txtETDDate.Enabled = false;
            //txtETDTime.Enabled = false;
        }
    }

    protected void ddlAdvanceRetard_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAdvanceRetard.SelectedValue == "0")
        {
            rbnhourchange.SelectedValue = "0";
            rbnhourvalue.SelectedValue = "0";
        }
        if (ddlAdvanceRetard.SelectedValue == "1")
        {
            rbnhourchange.SelectedValue = "1";
            rbnhourvalue.SelectedValue = "2";
        }
        if (ddlAdvanceRetard.SelectedValue == "2")
        {
            rbnhourchange.SelectedValue = "1";
            rbnhourvalue.SelectedValue = "1";
        }
        if (ddlAdvanceRetard.SelectedValue == "3")
        {
            rbnhourchange.SelectedValue = "2";
            rbnhourvalue.SelectedValue = "1";
        }
        if (ddlAdvanceRetard.SelectedValue == "4")
        {
            rbnhourchange.SelectedValue = "2";
            rbnhourvalue.SelectedValue = "2";
        }
    }

    private void BindWorkOrder()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportWorkOrder(int.Parse(ViewState["VESSELID"].ToString())
                , DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvWorkOrder.DataSource = ds;
            gvWorkOrder.DataBind();
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataRowView drv = (DataRowView)e.Row.DataItem;

                LinkButton lblWorkOrderNo = (LinkButton)e.Row.FindControl("lblWorkOrderNo");
                Label lblWorkOrderId = (Label)e.Row.FindControl("lblWorkOrderId");
                lblWorkOrderNo.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + lblWorkOrderId.Text + "'); return false;");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rblMachineryFailure_OnSelectedIndexChanged(Object sender, EventArgs args)
    {
        if (rblMachineryFailure.SelectedValue == "1")
            MenuMachineryFailure.Visible = true;
        else
            MenuMachineryFailure.Visible = false;
    }
    protected void gvExternalInspection_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            Table gridTable = (Table)gvExternalInspection.Controls[0];
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddlAuditTypeEdit = (DropDownList)e.Row.FindControl("ddlAuditTypeEdit");
                //Label lblTypeOfInspection = (Label)e.Row.FindControl("lblTypeOfInspection");

                //string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
                //if (ddlAuditTypeEdit != null)
                //{
                //    ddlAuditTypeEdit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                //                                    , null
                //                                    , null
                //                                    , 1
                //                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                //    ddlAuditTypeEdit.DataTextField = "FLDSHORTCODE";
                //    ddlAuditTypeEdit.DataValueField = "FLDINSPECTIONID";
                //    ddlAuditTypeEdit.DataBind();
                //    ddlAuditTypeEdit.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                //    if (lblTypeOfInspection != null)
                //        ddlAuditTypeEdit.SelectedValue = lblTypeOfInspection.Text.ToString();
                //}
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //DropDownList ddlAuditType = (DropDownList)e.Row.FindControl("ddlAuditType");

                //string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
                //if (ddlAuditType != null)
                //{
                //    ddlAuditType.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                //                                    , null
                //                                    , null
                //                                    , 1
                //                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
                //    ddlAuditType.DataTextField = "FLDSHORTCODE";
                //    ddlAuditType.DataValueField = "FLDINSPECTIONID";
                //    ddlAuditType.DataBind();
                //    ddlAuditType.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                //}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExternalInspection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindExternalInspection();
    }

    protected void gvExternalInspection_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExternalInspection_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindExternalInspection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExternalInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }

            string lblExternalInspectionIdEdit = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblExternalInspectionIdEdit")).Text;
            string txtTypeOfInspectionEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTypeOfInspectionEdit")).Text;
            string txtInspectingCompanyEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectingCompanyEdit")).Text;
            string txtInspectorNameEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectorNameEdit")).Text;
            string txtNumberOfNCEdit = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("txtNumberOfNCEdit")).Text;

            if (!IsValidExternalInspection(txtTypeOfInspectionEdit, txtInspectingCompanyEdit, txtInspectorNameEdit, txtNumberOfNCEdit))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                    , General.GetNullableGuid(lblExternalInspectionIdEdit), txtTypeOfInspectionEdit, txtInspectingCompanyEdit, txtInspectorNameEdit
                    , General.GetNullableInteger(txtNumberOfNCEdit));

            _gridView.EditIndex = -1;
            BindExternalInspection();
            EditMidNightReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExternalInspection_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (Session["MIDNIGHTREPORTID"] == null)
                {
                    ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                    ucError.Visible = true;
                    return;
                }

                string txtTypeOfInspectionAdd = ((TextBox)_gridView.FooterRow.FindControl("txtTypeOfInspectionAdd")).Text;
                string txtInspectingCompanyAdd = ((TextBox)_gridView.FooterRow.FindControl("txtInspectingCompanyAdd")).Text;
                string txtInspectorNameAdd = ((TextBox)_gridView.FooterRow.FindControl("txtInspectorNameAdd")).Text;
                string txtNumberOfNCAdd = ((UserControlMaskedTextBox)_gridView.FooterRow.FindControl("txtNumberOfNCAdd")).Text;


                if (!IsValidExternalInspection(txtTypeOfInspectionAdd, txtInspectingCompanyAdd, txtInspectorNameAdd, txtNumberOfNCAdd))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                        , null, txtTypeOfInspectionAdd, txtInspectingCompanyAdd, txtInspectorNameAdd, General.GetNullableInteger(txtNumberOfNCAdd));

                //    string FromTime = (((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text.Trim() == "__:__") ? string.Empty : ((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text;
                //    string Activity = ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text;
                //    string ddlActivity = ((DropDownList)_gridView.FooterRow.FindControl("ddlActivityAdd")).SelectedValue;

                BindExternalInspection();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    string lblExternalInspectionId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblExternalInspectionId")).Text;
                    if (lblExternalInspectionId != "")
                    {
                        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionDelete(new Guid(Session["MIDNIGHTREPORTID"].ToString()), new Guid(lblExternalInspectionId));
                    }
                }
                BindExternalInspection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidExternalInspection(string type, string company, string name, string nc)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (type.Trim().Equals(""))
            ucError.ErrorMessage = "Type of Inspection / Audit is required.";
        if (company.Trim().Equals(""))
            ucError.ErrorMessage = "Inspecting Authority / Company is required.";
        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name of Inspector / Auditor is required.";
        if (nc.Trim().Equals(""))
            ucError.ErrorMessage = "Number of NCs / Observations is required.";
        return (!ucError.IsError);
    }
    protected void chkMEPortFlowDetective_OnCheckedChanged(Object sender, EventArgs args)
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPreviousFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtme1initialhrs.Text = ds.Tables[0].Rows[0]["FLDME1FIRSTHRS"].ToString();
            txtme1lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME1LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME1FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME1LASTHRS"].ToString());
        }
        if (chkMEPortFlowDetective.Checked == true)
        {
            txtme1lasthrs.CssClass = "readonlytextbox";
            txtme1lasthrs.ReadOnly = "true";
            txtme1Total.Text = "";
        }
        if (chkMEPortFlowDetective.Checked == false)
        {
            txtme1lasthrs.CssClass = "input";
            txtme1lasthrs.ReadOnly = "false";
        }
    }
    protected void chkMEStbdFlowDetective_OnCheckedChanged(Object sender, EventArgs args)
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPreviousFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtme2initialhrs.Text = ds.Tables[0].Rows[0]["FLDME2FIRSTHRS"].ToString();
            txtme2lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME2LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME2FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME2LASTHRS"].ToString());
        }
        if (chkMEStbdFlowDetective.Checked == true)
        {
            txtme2lasthrs.CssClass = "readonlytextbox";
            txtme2lasthrs.ReadOnly = "true";
            txtme2Total.Text = "";
        }
        if (chkMEStbdFlowDetective.Checked == false)
        {
            txtme2lasthrs.CssClass = "input";
            txtme2lasthrs.ReadOnly = "false";
        }
    }
    protected void chkAE1FlowDetective_OnCheckedChanged(Object sender, EventArgs args)
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPreviousFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtAE1initialhrs.Text = ds.Tables[0].Rows[0]["FLDAE1FIRSTHRS"].ToString();
            txtAE1lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDAE1LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDAE1FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDAE1LASTHRS"].ToString());
        }
        if (chkAE1FlowDetective.Checked == true)
        {
            txtAE1lasthrs.CssClass = "readonlytextbox";
            txtAE1lasthrs.ReadOnly = "true";
            txtAE1Consumption.Text = "";
        }
        if (chkAE1FlowDetective.Checked == false)
        {
            txtAE1lasthrs.CssClass = "input";
            txtAE1lasthrs.ReadOnly = "false";
        }
    }
    protected void chkAE2FlowDetective_OnCheckedChanged(Object sender, EventArgs args)
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPreviousFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtAE2initialhrs.Text = ds.Tables[0].Rows[0]["FLDAE2FIRSTHRS"].ToString();
            txtAE2lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDAE2LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDAE2FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDAE2LASTHRS"].ToString());
        }
        if (chkAE2FlowDetective.Checked == true)
        {
            txtAE2lasthrs.CssClass = "readonlytextbox";
            txtAE2lasthrs.ReadOnly = "true";
            txtAE2Consumption.Text = "";
        }
        if (chkAE2FlowDetective.Checked == false)
        {
            txtAE2lasthrs.CssClass = "input";
            txtAE2lasthrs.ReadOnly = "false";
        }
    }
    protected void chkMEPortNoFM_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkMEPortNoFM.Checked == true)
        {
            txtme1initialhrs.Text = "";
            chkMEPortFlowDetective.Enabled = false;
            chkMEPortFlowDetective.Checked = false;

            txtme1lasthrs.CssClass = "readonlytextbox";
            txtme1lasthrs.ReadOnly = "true";
            txtme1lasthrs.Text = "";
        }
        if (chkMEPortNoFM.Checked == false)
        {
            txtme1lasthrs.CssClass = "input";
            txtme1lasthrs.ReadOnly = "false";
            chkMEPortFlowDetective.Enabled = true;
        }
    }

    protected void chkMEPortreturnNoFM_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkMEPortreturnNoFM.Checked == true)
        {
            lblme1returninitialhrs.Text = "";
            chkMEPortreturnFlowDetective.Enabled = false;
            chkMEPortreturnFlowDetective.Checked = false;

            lblme1returnlasthrs.CssClass = "readonlytextbox";
            lblme1returnlasthrs.ReadOnly = "true";
            lblme1returnlasthrs.Text = "";
        }
        if (chkMEPortreturnNoFM.Checked == false)
        {
            lblme1returnlasthrs.CssClass = "input";
            lblme1returnlasthrs.ReadOnly = "false";
            chkMEPortreturnFlowDetective.Enabled = true;
        }
    }

    protected void chkMEPortreturnFlowDetective_OnCheckedChanged(Object sender, EventArgs args)
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPreviousFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblme1returninitialhrs.Text = ds.Tables[0].Rows[0]["FLDME1RETURNFIRSTHRS"].ToString();
            lblme1returnlasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME1RETURNLASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME1RETURNFIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME1RETURNLASTHRS"].ToString());
        }
        if (chkMEPortreturnFlowDetective.Checked == true)
        {
            lblme1returnlasthrs.CssClass = "readonlytextbox";
            lblme1returnlasthrs.ReadOnly = "true";
        }
        if (chkMEPortreturnFlowDetective.Checked == false)
        {
            lblme1returnlasthrs.CssClass = "input";
            lblme1returnlasthrs.ReadOnly ="false";
        }
    }

    protected void chkMEStbdNoFM_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkMEStbdNoFM.Checked == true)
        {
            txtme2initialhrs.Text = "";
            chkMEStbdFlowDetective.Enabled = false;
            chkMEStbdFlowDetective.Checked = false;

            txtme2lasthrs.CssClass = "readonlytextbox";
            txtme2lasthrs.ReadOnly = "true";
            txtme2lasthrs.Text = "";
        }
        if (chkMEStbdNoFM.Checked == false)
        {
            txtme2lasthrs.CssClass = "input";
            txtme2lasthrs.ReadOnly = "false";
            chkMEStbdFlowDetective.Enabled = true;
        }
    }

    protected void chkMEStbdreturnNoFM_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkMEStbdreturnNoFM.Checked == true)
        {
            lblme2returninitialhrs.Text = "";
            chkMEStbdreturnFlowDetective.Enabled = false;
            chkMEStbdreturnFlowDetective.Checked = false;

            lblme2returnlasthrs.CssClass = "readonlytextbox";
            lblme2returnlasthrs.ReadOnly = "true";
            lblme2returnlasthrs.Text = "";
        }
        if (chkMEStbdreturnNoFM.Checked == false)
        {
            lblme2returnlasthrs.CssClass = "input";
            lblme2returnlasthrs.ReadOnly = "false";
            chkMEStbdreturnFlowDetective.Enabled = true;
        }
    }

    protected void chkMEStbdreturnFlowDetective_OnCheckedChanged(Object sender, EventArgs args)
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPreviousFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblme2returninitialhrs.Text = ds.Tables[0].Rows[0]["FLDME2RETURNFIRSTHRS"].ToString();
            lblme2returnlasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME2RETURNLASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME2RETURNFIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME2RETURNLASTHRS"].ToString());
        }
        if (chkMEStbdreturnFlowDetective.Checked == true)
        {
            lblme2returnlasthrs.CssClass = "readonlytextbox";
            lblme2returnlasthrs.ReadOnly = "true";

        }
        if (chkMEStbdreturnFlowDetective.Checked == false)
        {
            lblme2returnlasthrs.CssClass = "input";
            lblme2returnlasthrs.ReadOnly = "false";
        }
    }

    protected void chkAE1NoFM_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkAE1NoFM.Checked == true)
        {
            txtAE1initialhrs.Text = "";
            chkAE1FlowDetective.Enabled = false;
            chkAE1FlowDetective.Checked = false;

            txtAE1lasthrs.CssClass = "readonlytextbox";
            txtAE1lasthrs.ReadOnly = "true";
            txtAE1lasthrs.Text = "";
        }
        if (chkAE1NoFM.Checked == false)
        {
            txtAE1lasthrs.CssClass = "input";
            txtAE1lasthrs.ReadOnly = "false";
            chkAE1FlowDetective.Enabled = true;
        }
    }

    protected void chkAE2NoFM_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkAE2NoFM.Checked == true)
        {
            txtAE2initialhrs.Text = "";
            chkAE2FlowDetective.Enabled = false;
            chkAE2FlowDetective.Checked = false;

            txtAE2lasthrs.CssClass = "readonlytextbox";
            txtAE2lasthrs.ReadOnly = "true";
            txtAE2lasthrs.Text = "";
        }
        if (chkAE2NoFM.Checked == false)
        {
            txtAE2lasthrs.CssClass = "input";
            txtAE2lasthrs.ReadOnly = "false";
            chkAE2FlowDetective.Enabled = true;
        }
    }
    
    private void EditMidNightReportRunTime(Guid? midnightreportid)
    {
        if (midnightreportid != null)
        {
            if (Session["MIDNIGHTREPORTID"] == null)
            {
                DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(midnightreportid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ucMEPortFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDMEPORT"].ToString();
                    ucMEStbdFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDMESTBD"].ToString();
                    ucAEIFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE1"].ToString();
                    ucAEIIFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE2"].ToString();
                    ucBT1FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT1"].ToString();
                    ucBT2FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT2"].ToString();
                    ucST1FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDST1"].ToString();
                    ucST2FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDST2"].ToString();

                    ucMEPort.Text = ds.Tables[0].Rows[0]["FLDMEPORT"].ToString();
                    ucMEStbd.Text = ds.Tables[0].Rows[0]["FLDMESTBD"].ToString();
                    ucAEI.Text = ds.Tables[0].Rows[0]["FLDAE1"].ToString();
                    ucAEII.Text = ds.Tables[0].Rows[0]["FLDAE2"].ToString();
                    ucBT1.Text = ds.Tables[0].Rows[0]["FLDBT1"].ToString();
                    ucBT2.Text = ds.Tables[0].Rows[0]["FLDBT2"].ToString();
                    ucST1.Text = ds.Tables[0].Rows[0]["FLDST1"].ToString();
                    ucST2.Text = ds.Tables[0].Rows[0]["FLDST2"].ToString();

                }
            }
        }
        else
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportRuntimeEdit(General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Session["MIDNIGHTREPORTID"] == null)
                {
                    ucMEPort.Text = ds.Tables[0].Rows[0]["FLDMEPORT"].ToString();
                    ucMEStbd.Text = ds.Tables[0].Rows[0]["FLDMESTBD"].ToString();
                    ucAEI.Text = ds.Tables[0].Rows[0]["FLDAE1"].ToString();
                    ucAEII.Text = ds.Tables[0].Rows[0]["FLDAE2"].ToString();
                    ucBT1.Text = ds.Tables[0].Rows[0]["FLDBT1"].ToString();
                    ucBT2.Text = ds.Tables[0].Rows[0]["FLDBT2"].ToString();
                    ucST1.Text = ds.Tables[0].Rows[0]["FLDST1"].ToString();
                    ucST2.Text = ds.Tables[0].Rows[0]["FLDST2"].ToString();

                    ucMEPortFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDMEPORT"].ToString();
                    ucMEStbdFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDMESTBD"].ToString();
                    ucAEIFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE1"].ToString();
                    ucAEIIFirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDAE2"].ToString();
                    ucBT1FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT1"].ToString();
                    ucBT2FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDBT2"].ToString();
                    ucST1FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDST1"].ToString();
                    ucST2FirstRunHrs.Text = ds.Tables[0].Rows[0]["FLDST2"].ToString();
                }
            }
        }
    }
    private void ChangeFlowmeterTotalEnabedYN()
    {
        if (chkMEPortNoFM.Checked == false)
        {
            if (chkMEPortFlowDetective.Checked == false)
            {
                txtme1Total.CssClass = "readonlytextbox";
                txtme1Total.ReadOnly = "true";
            }
            else
            {
                txtme1Total.CssClass = "input";
                txtme1Total.ReadOnly = "false";
            }
        }
        else
        {
            txtme1Total.CssClass = "input";
            txtme1Total.ReadOnly = "false";
        }

        if (chkMEStbdNoFM.Checked == false)
        {
            if (chkMEStbdFlowDetective.Checked == false)
            {
                txtme2Total.CssClass = "readonlytextbox";
                txtme2Total.ReadOnly = "true";
            }
            else
            {
                txtme2Total.CssClass = "input";
                txtme2Total.ReadOnly = "false";
            }
        }
        else
        {
            txtme2Total.CssClass = "input";
            txtme2Total.ReadOnly = "false";
        }

        if (chkAE1NoFM.Checked == false)
        {
            if (chkAE1FlowDetective.Checked == false)
            {
                txtAE1Consumption.CssClass = "readonlytextbox";
                txtAE1Consumption.ReadOnly = "true";
            }
            else
            {
                txtAE1Consumption.CssClass = "input";
                txtAE1Consumption.ReadOnly = "false";
            }
        }
        else
        {
            txtAE1Consumption.CssClass = "input";
            txtAE1Consumption.ReadOnly = "false";
        }

        if (chkAE2NoFM.Checked == false)
        {
            if (chkAE2FlowDetective.Checked == false)
            {
                txtAE2Consumption.CssClass = "readonlytextbox";
                txtAE2Consumption.ReadOnly = "true";
            }
            else
            {
                txtAE2Consumption.CssClass = "input";
                txtAE2Consumption.ReadOnly = "false";
            }
        }
        else
        {
            txtAE2Consumption.CssClass = "input";
            txtAE2Consumption.ReadOnly = "false";
        }
    }
    protected void MenuTabSaveVesselMovements_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTabSaveMeteorologyData_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTabSaveFO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTabSaveBulks_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SaveMidnightReport()
    {
        if (Session["MIDNIGHTREPORTID"] != null)
        {
            //if (!IsValidData())
            //{
            //    ucError.Visible = true;
            //    return;
            //}
            //string timeofetd = (txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETDTime.Text;
            //string timeofeta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETATime.Text;

           // string timeofarrival = (txtArrivalTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtArrivalTime.Text;
           // string timeofdeparture = (txtDepartureTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtDepartureTime.Text;


            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportUpdate(new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                , General.GetNullableDateTime(txtETDDate.Text)
                , General.GetNullableDateTime(txtETADate.Text)
                , General.GetNullableInteger(ucPort.SelectedValue)
                , General.GetNullableGuid(ddlETALocation.SelectedValue), General.GetNullableGuid(ddlETDLocation.SelectedValue)
                , ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute, ucLatitude.TextSecond, ucLongitude.TextSecond, ucLatitude.TextDirection, ucLongitude.TextDirection
                , General.GetNullableDecimal(ucAvgSpeed.Text)
                , General.GetNullableString(txtGeneralRemarks.Text)
                , General.GetNullableString(txtLookAheadRemarks.Text)
                , General.GetNullableString(txtComments.Text)
                , General.GetNullableInteger(ddlvesselstatus.SelectedValue)
                , General.GetNullableString(txtbulkRemarks.Text)
                , General.GetNullableString(txtFlowmeterRemarks.Text)
                , General.GetNullableString(txtVesselMovementsRemarks.Text)
                , General.GetNullableString(txtMeterologyRemarks.Text)
                , General.GetNullableString(txtCrewRemarks.Text)
                , General.GetNullableInteger(rbnhourchange.SelectedValue)
                , General.GetNullableInteger(rbnhourvalue.SelectedValue)
                , General.GetNullableDecimal(txtDPTime.Text)
                , General.GetNullableDecimal(txtDPFuelConsumption.Text)
                , General.GetNullableInteger(ucBreakfast.Text)
                , General.GetNullableInteger(ucLunch.Text)
                , General.GetNullableInteger(ucDinner.Text)
                , null
                , General.GetNullableInteger(txtPOBClient.Text)
                , General.GetNullableInteger(txtPOBService.Text)
                , General.GetNullableInteger(txtTotalOB.Text)
                , General.GetNullableDateTime(txtArrivalDate.Text), General.GetNullableDateTime(txtDepartureDate.Text)
                , General.GetNullableInteger(ucTea1.Text), General.GetNullableInteger(ucTea2.Text), General.GetNullableInteger(ucSupper.Text)
                , General.GetNullableInteger(ucSupBreakFast.Text), General.GetNullableInteger(ucSupLunch.Text), General.GetNullableInteger(ucSupDinner.Text), General.GetNullableInteger(ucSupSupper.Text)
                , General.GetNullableInteger(ucSupTea1.Text), General.GetNullableInteger(ucSupTea2.Text)
                , General.GetNullableInteger(rblMachineryFailure.SelectedValue), null
                , General.GetNullableInteger(ddlInstalationType.SelectedValue)
                , General.GetNullableInteger(ddlCrewListSOBYN.SelectedValue), txtCrewListSOBRemarks.Text
             );
            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportFlowmeterUpdate(new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                 , General.GetNullableInteger(txtme1initialhrs.Text)
                 , General.GetNullableInteger(txtme1lasthrs.Text)
                 , General.GetNullableInteger(lblme1returninitialhrs.Text)
                 , General.GetNullableInteger(lblme1returnlasthrs.Text)
                 , General.GetNullableInteger(txtme2initialhrs.Text)
                 , General.GetNullableInteger(txtme2lasthrs.Text)
                 , General.GetNullableInteger(lblme2returninitialhrs.Text)
                 , General.GetNullableInteger(lblme2returnlasthrs.Text)
                 , General.GetNullableInteger(txtAE1Consumption.Text)
                 , General.GetNullableInteger(txtAE2Consumption.Text)
                 , General.GetNullableDecimal(ucMEPort.Text)
                 , General.GetNullableDecimal(ucMEStbd.Text)
                 , General.GetNullableDecimal(ucAEI.Text)
                 , General.GetNullableDecimal(ucAEII.Text)
                 , General.GetNullableDecimal(ucBT1.Text)
                 , General.GetNullableDecimal(ucBT2.Text)
                 , General.GetNullableDecimal(ucST1.Text)
                 , General.GetNullableDecimal(ucST2.Text)
                 , General.GetNullableDecimal(ucFord.Text)
                 , General.GetNullableDecimal(ucMidship.Text)
                 , General.GetNullableDecimal(ucAft.Text)
                 , General.GetNullableDecimal(ucAverage.Text)
                 , General.GetNullableInteger(txtme1Total.Text)
                 , General.GetNullableInteger(txtme2Total.Text)
                 , General.GetNullableInteger(txtAE1initialhrs.Text)
                 , General.GetNullableInteger(txtAE1lasthrs.Text)
                 , General.GetNullableInteger(txtAE2initialhrs.Text)
                 , General.GetNullableInteger(txtAE2lasthrs.Text)
                 , chkMEPortFlowDetective.Checked.Value ? 1 : 0, chkMEStbdFlowDetective.Checked.Value ? 1 : 0, chkAE1FlowDetective.Checked.Value ? 1 : 0, chkAE2FlowDetective.Checked.Value ? 1 : 0, General.GetNullableInteger(ucotherConsumption.Text)
                 , chkMEPortNoFM.Checked.Value ? 1 : 0, chkMEStbdNoFM.Checked.Value ? 1 : 0, chkMEPortreturnNoFM.Checked.Value ? 1 : 0, chkMEStbdreturnNoFM.Checked.Value ? 1 : 0
                 , chkAE1NoFM.Checked.Value ? 1 : 0, chkAE2NoFM.Checked.Value ? 1 : 0, chkMEPortreturnFlowDetective.Checked.Value ? 1 : 0, chkMEStbdreturnFlowDetective.Checked.Value ? 1 : 0
                 , General.GetNullableDecimal(ucMEPortFirstRunHrs.Text), General.GetNullableDecimal(ucMEStbdFirstRunHrs.Text), General.GetNullableDecimal(ucAEIFirstRunHrs.Text), General.GetNullableDecimal(ucAEIIFirstRunHrs.Text)
                 , General.GetNullableDecimal(ucBT1FirstRunHrs.Text), General.GetNullableDecimal(ucBT2FirstRunHrs.Text), General.GetNullableDecimal(ucST1FirstRunHrs.Text), General.GetNullableDecimal(ucST2FirstRunHrs.Text)
                 , General.GetNullableDecimal(ucMEPortTotalRunHrs.Text), General.GetNullableDecimal(ucMEStbdTotalRunHrs.Text), General.GetNullableDecimal(ucAEITotalRunHrs.Text), General.GetNullableDecimal(ucAEIITotalRunHrs.Text)
                 , General.GetNullableDecimal(ucBT1TotalRunHrs.Text), General.GetNullableDecimal(ucBT2TotalRunHrs.Text), General.GetNullableDecimal(ucST1TotalRunHrs.Text), General.GetNullableDecimal(ucST2TotalRunHrs.Text));
            UpdateOperationalSummaryData();
            UpdateMeteorologyData();
            UpdateBulks();
            UpdateLookAheadActivity();
            UpdatePMDOverDue();

            ucStatus.Text = "MidNight Report Updated";
        }
        EditMidNightReport();
        BindRequisionsandPO();
        BindOilLoadandConsumption();
        BindMeteorologyData();
        BindPlannedActivity();
        BindVesselMovements();
        BindOperationalTimeSummary();
        EditFlowMeterReading();

    }
    private void CheckPrevioueDefectiveFM()
    {
        if (ViewState["PREVIOUSEREPORTID"] != null && ViewState["PREVIOUSEREPORTID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FLDMEPORTFLOWDEFECTIVE"].ToString() == "1" && chkMEPortFlowDetective.Checked == false)
                {
                    txtme1initialhrs.ReadOnly = "false";
                    txtme1initialhrs.CssClass = "input";
                }
                else
                {
                    txtme1initialhrs.ReadOnly = "true";
                    txtme1initialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDMEPORTRETURNFLOWDEFECTIVE"].ToString() == "1" && chkMEPortreturnFlowDetective.Checked == false)
                {
                    lblme1returninitialhrs.ReadOnly = "false";
                    lblme1returninitialhrs.CssClass = "input";
                }
                else
                {
                    lblme1returninitialhrs.ReadOnly = "true";
                    lblme1returninitialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDMESTBDFLOWDEFECTIVE"].ToString() == "1" && chkMEStbdFlowDetective.Checked == false)
                {
                    txtme2initialhrs.ReadOnly = "false";
                    txtme2initialhrs.CssClass = "input";
                }
                else
                {
                    txtme2initialhrs.ReadOnly = "true";
                    txtme2initialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDMESTBDRETURNFLOWDEFECTIVE"].ToString() == "1" && chkMEStbdreturnFlowDetective.Checked == false)
                {
                    lblme2returninitialhrs.ReadOnly = "false";
                    lblme2returninitialhrs.CssClass = "input";
                }
                else
                {
                    lblme2returninitialhrs.ReadOnly = "true";
                    lblme2returninitialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDAE1FLOWDEFECTIVE"].ToString() == "1" && chkAE1FlowDetective.Checked == false)
                {
                    txtAE1initialhrs.ReadOnly = "false";
                    txtAE1initialhrs.CssClass = "input";
                }
                else
                {
                    txtAE1initialhrs.ReadOnly = "true";
                    txtAE1initialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDAE2FLOWDEFECTIVE"].ToString() == "1" && chkAE2FlowDetective.Checked == false)
                {
                    txtAE2initialhrs.ReadOnly = "false";
                    txtAE2initialhrs.CssClass = "input";
                }
                else
                {
                    txtAE2initialhrs.ReadOnly = "true";
                    txtAE2initialhrs.CssClass = "readonlytextbox";
                }
            }
        }
    }

    private void ChangeNoFlowmeterEnabedYN(Guid? prevMidnightId)
    {
        if (prevMidnightId == null)
        {
            if (chkMEPortNoFM.Checked == false)
            {
                if (chkMEPortFlowDetective.Checked == false)
                {
                    txtme1initialhrs.CssClass = "input";
                    txtme1initialhrs.ReadOnly = "false";
                }
                if (chkMEPortFlowDetective.Checked == true)
                {
                    txtme1initialhrs.CssClass = "readonlytextbox";
                    txtme1initialhrs.ReadOnly = "true";
                }
            }
            if (chkMEPortNoFM.Checked == true)
            {
                txtme1initialhrs.CssClass = "readonlytextbox";
                txtme1initialhrs.ReadOnly = "true";
            }

            if (chkMEPortreturnNoFM.Checked == false)
            {
                if (chkMEPortreturnFlowDetective.Checked == false)
                {
                    lblme1returninitialhrs.CssClass = "input";
                    lblme1returninitialhrs.ReadOnly = "false";
                }
                if (chkMEPortreturnFlowDetective.Checked == true)
                {
                    lblme1returninitialhrs.CssClass = "readonlytextbox";
                    lblme1returninitialhrs.ReadOnly = "true";
                }
            }
            if (chkMEPortreturnNoFM.Checked == true)
            {
                lblme1returninitialhrs.CssClass = "readonlytextbox";
                lblme1returninitialhrs.ReadOnly = "true";
            }

            if (chkMEStbdNoFM.Checked == false)
            {
                if (chkMEStbdFlowDetective.Checked == false)
                {
                    txtme2initialhrs.CssClass = "input";
                    txtme2initialhrs.ReadOnly = "false";
                }
                if (chkMEStbdFlowDetective.Checked == true)
                {
                    txtme2initialhrs.CssClass = "readonlytextbox";
                    txtme2initialhrs.ReadOnly = "true";
                }
            }
            if (chkMEStbdNoFM.Checked == true)
            {
                txtme2initialhrs.CssClass = "readonlytextbox";
                txtme2initialhrs.ReadOnly = "true";
            }
            if (chkMEStbdreturnNoFM.Checked == false)
            {
                if (chkMEStbdreturnFlowDetective.Checked == false)
                {
                    lblme2returninitialhrs.CssClass = "input";
                    lblme2returninitialhrs.ReadOnly = "false";
                }
                if (chkMEStbdreturnFlowDetective.Checked == true)
                {
                    lblme2returninitialhrs.CssClass = "readonlytextbox";
                    lblme2returninitialhrs.ReadOnly = "true";
                }
            }
            if (chkMEStbdreturnNoFM.Checked == true)
            {
                lblme2returninitialhrs.CssClass = "readonlytextbox";
                lblme2returninitialhrs.ReadOnly = "true";
            }
            if (chkAE1NoFM.Checked == false)
            {
                if (chkAE1FlowDetective.Checked == false)
                {
                    txtAE1initialhrs.CssClass = "input";
                    txtAE1initialhrs.ReadOnly = "false";
                }
                if (chkAE1FlowDetective.Checked == true)
                {
                    txtAE1initialhrs.CssClass = "readonlytextbox";
                    txtAE1initialhrs.ReadOnly = "true";
                }
            }
            if (chkAE1NoFM.Checked == true)
            {
                txtAE1initialhrs.CssClass = "readonlytextbox";
                txtAE1initialhrs.ReadOnly = "true";
            }
            if (chkAE2NoFM.Checked == false)
            {
                if (chkAE2FlowDetective.Checked == false)
                {
                    txtAE2initialhrs.CssClass = "input";
                    txtAE2initialhrs.ReadOnly = "false";
                }
                if (chkAE2FlowDetective.Checked == true)
                {
                    txtAE2initialhrs.CssClass = "readonlytextbox";
                    txtAE2initialhrs.ReadOnly = "true";
                }
            }
            if (chkAE2NoFM.Checked == true)
            {
                txtAE2initialhrs.CssClass = "readonlytextbox";
                txtAE2initialhrs.ReadOnly = "true";
            }
        }
    }
    protected void gvCertificates_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days <= 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
                }
                if (t.Days > 15 && t.Days <= 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days <= 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
            }
        }
    }
    protected void gvShipAudit_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblDueDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days <= 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
                }
                if (t.Days > 15 && t.Days <= 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days <= 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
            }
        }
    }
    protected void gvShipTasks_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblDueDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days <= 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
                }
                if (t.Days > 15 && t.Days <= 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days <= 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
            }
        }
    }
    private void BindInstallationType()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixRegistersTypeOfInstallation.TypeOfInstallationSearch("", "", "", null, 1, 100, ref iRowCount, ref iTotalPageCount);
            ddlInstalationType.DataSource = ds;
            ddlInstalationType.DataValueField = "FLDTYPEOFINSTALLATIONID";
            ddlInstalationType.DataTextField = "FLDTYPEOFINSTALLATIONNAME";
            ddlInstalationType.DataBind();
           // ddlInstalationType.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindPMSoverdue()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPMSOverdue(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableDateTime(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                , General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            gvPMSoverdue.DataSource = ds;
            gvPMSoverdue.DataBind();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPMSoverdue_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataRowView drv = (DataRowView)e.Row.DataItem;

            LinkButton lnkCount = (LinkButton)e.Row.FindControl("lnkCount");
            RadLabel lblType = (RadLabel)e.Row.FindControl("lblType");
           // lnkCount.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
            lnkCount.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
            //lnkCount.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

        }
    }
    private void UpdatePMDOverDue()
    {
        foreach (GridDataItem gvr in gvPMSoverdue.Items)
        {

            RadLabel lblDMROverDueId = (RadLabel)gvr.FindControl("lblDMROverDueId");
            RadLabel lblName = (RadLabel)gvr.FindControl("lblName");
            RadLabel lblType = (RadLabel)gvr.FindControl("lblType");
            RadLabel lblCount = (RadLabel)gvr.FindControl("lblCount");

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRPmsOverDueUpdate(new Guid(Session["MIDNIGHTREPORTID"].ToString())
                , General.GetNullableGuid(lblDMROverDueId.Text)
                , int.Parse(ViewState["VESSELID"].ToString())
                , int.Parse(lblType.Text)
                , lblName.Text
                , int.Parse(lblCount.Text));


        }
    }

    protected void ddlvesselstatus_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {

        txtETADate.Text = "";
        // txtETATime.Text = "";       
        txtETDDate.Text = "";
        //txtETDTime.Text = "";
        txtArrivalDate.Text = "";
        //txtArrivalTime.Text = "";
        txtDepartureDate.Text = "";
        //txtDepartureTime.Text = "";

        ChangeVesselStatus();

    }
    protected void ddlAdvanceRetard_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlAdvanceRetard.SelectedValue == "0")
        {
            rbnhourchange.SelectedValue = "0";
            rbnhourvalue.SelectedValue = "0";
        }
        if (ddlAdvanceRetard.SelectedValue == "1")
        {
            rbnhourchange.SelectedValue = "1";
            rbnhourvalue.SelectedValue = "2";
        }
        if (ddlAdvanceRetard.SelectedValue == "2")
        {
            rbnhourchange.SelectedValue = "1";
            rbnhourvalue.SelectedValue = "1";
        }
        if (ddlAdvanceRetard.SelectedValue == "3")
        {
            rbnhourchange.SelectedValue = "2";
            rbnhourvalue.SelectedValue = "1";
        }
        if (ddlAdvanceRetard.SelectedValue == "4")
        {
            rbnhourchange.SelectedValue = "2";
            rbnhourvalue.SelectedValue = "2";
        }
    }
    protected void gvVesselMovements_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        if (Session["MIDNIGHTREPORTID"] != null)
        {
            string VesselMovementsId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDVESSELMOVEMENTSID"].ToString();
            if (VesselMovementsId != "")
            {
                PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsDelete(new Guid(VesselMovementsId));
            }
        }
        BindVesselMovements();
        BindOperationalTimeSummary();
    }


    protected void gvVesselMovements_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.RowIndex;

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }

            string FromTime = (((UserControlMaskedTextBox)e.Item.FindControl("txtFromTime")).TextWithLiterals.Trim() == "__:__") ? string.Empty : ((UserControlMaskedTextBox)e.Item.FindControl("txtFromTime")).TextWithLiterals;
            string Activity = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;
            string ddlActivity = ((RadComboBox)e.Item.FindControl("ddlActivityEdit")).SelectedValue;
            string ActivityId = ((RadLabel)e.Item.FindControl("lblVesselMovementsEditId")).Text;

            string fromtime = (FromTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : FromTime;
            string TimeDuration = ((RadLabel)e.Item.FindControl("lblTimeDurationEdit")).Text;
            if (!IsValidVesselMovements(fromtime, ddlActivity))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                    , General.GetNullableGuid(ddlActivity), General.GetNullableString(Activity)
                    , General.GetNullableDateTime(ViewState["REPORTDATE"].ToString() + " " + fromtime), General.GetNullableGuid(ActivityId)
                    , General.GetNullableDecimal(TimeDuration));

            // _gridView.EditIndex = -1;
            BindVesselMovements();
            BindOperationalTimeSummary();
            UpdateOperationalSummaryData();
            EditMidNightReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselMovements_PreRender(object sender, EventArgs e)
    {

    }
    protected void gvVesselMovements_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            GridPagerItem item = (GridPagerItem)e.Item;
        }
        if (e.CommandName.ToString().ToUpper() == "ADD")
        {

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }
            GridFooterItem footerItem = (GridFooterItem)gvVesselMovements.MasterTableView.GetItems(GridItemType.Footer)[0];
            // Button btn = (Button)footerItem.FindControl("Button1");
            RadLabel TotalFOC = (RadLabel)footerItem.FindControl("lblTotalFOC");

            string FromTime = (((UserControlMaskedTextBox)footerItem.FindControl("txtFromTimeAdd")).TextWithLiterals.Trim() == "__:__") ? string.Empty : ((UserControlMaskedTextBox)footerItem.FindControl("txtFromTimeAdd")).TextWithLiterals;
            string Activity = ((RadTextBox)footerItem.FindControl("txtDescriptionAdd")).Text;
            string ddlActivity = ((RadComboBox)footerItem.FindControl("ddlActivityAdd")).SelectedValue;

            string fromtime = (FromTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : FromTime;


            string TimeDuration = ((RadLabel)footerItem.FindControl("lblTimeDurationAdd")).Text;
            if (!IsValidVesselMovements(fromtime, ddlActivity))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                , General.GetNullableGuid(ddlActivity), General.GetNullableString(Activity)
                , General.GetNullableDateTime(ViewState["REPORTDATE"].ToString() + " " + fromtime), General.GetNullableGuid("")
                , General.GetNullableDecimal(TimeDuration));

            BindVesselMovements();
            BindOperationalTimeSummary();
            UpdateOperationalSummaryData();

        }
    }
    protected void gvVesselMovements_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindVesselMovements();

    }

    protected void gvVesselMovements_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            RadComboBox ct = (RadComboBox)item.FindControl("ddlActivityEdit");
            if (ct != null)
            {
                DataSet ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
                ct.DataSource = ds;
                ct.DataBind();
                //ct.Items.Insert(0, new ListItem("--Select--", ""));
                ct.SelectedValue = ((RadLabel)item.FindControl("lblVesselMovementsEditId")).Text;
            }
            // nextOperational = ((RadComboBox)item.FindControl("ddlActivityEdit")).SelectedValue.ToString();//["FLDOPERATIONALTASKID"].ToString();

          
        }


       


    }
    protected void gvMeteorologyData_ItemDataBound(object sender, GridItemEventArgs e)
    {

        // GridDataItem drv = (GridDataItem)e.Item.DataItem;



        if (e.Item is GridDataItem) //if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridDataItem item = (GridDataItem)e.Item;
            ImageButton db = (ImageButton)item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadLabel lblValueType = (RadLabel)item.FindControl("lblValueType");
            RadLabel lblShortname = (RadLabel)item.FindControl("lblShortname");

            UserControlNumber txtValueDecimal6Edit = (UserControlNumber)item.FindControl("txtValueDecimal6Edit");
            UserControlNumber txtValueDecimal12Edit = (UserControlNumber)item.FindControl("txtValueDecimal12Edit");
            UserControlNumber txtValueDecimal18Edit = (UserControlNumber)item.FindControl("txtValueDecimal18Edit");
            UserControlNumber txtValueDecimal24Edit = (UserControlNumber)item.FindControl("txtValueDecimal24Edit");
            UserControlNumber txtValueDecimalNext24HrsEdit = (UserControlNumber)item.FindControl("txtValueDecimalNext24HrsEdit");

            UserControlDirection ucDirection6Edit = (UserControlDirection)item.FindControl("ucDirection6Edit");
            UserControlDirection ucDirection12Edit = (UserControlDirection)item.FindControl("ucDirection12Edit");
            UserControlDirection ucDirection18Edit = (UserControlDirection)item.FindControl("ucDirection18Edit");
            UserControlDirection ucDirection24Edit = (UserControlDirection)item.FindControl("ucDirection24Edit");
            UserControlDirection ucDirectionNext24HrsEdit = (UserControlDirection)item.FindControl("ucDirectionNext24HrsEdit");

            UserControlSeaCondtion ucSeaCondtion6Edit = (UserControlSeaCondtion)item.FindControl("ucSeaCondtion6Edit");
            UserControlSeaCondtion ucSeaCondtion12Edit = (UserControlSeaCondtion)item.FindControl("ucSeaCondtion12Edit");
            UserControlSeaCondtion ucSeaCondtion18Edit = (UserControlSeaCondtion)item.FindControl("ucSeaCondtion18Edit");
            UserControlSeaCondtion ucSeaCondtion24Edit = (UserControlSeaCondtion)item.FindControl("ucSeaCondtion24Edit");
            UserControlSeaCondtion ucSeaCondtionNext24HrsEdit = (UserControlSeaCondtion)item.FindControl("ucSeaCondtionNext24HrsEdit");

            if (lblValueType.Text == "1")
            {
                ucDirection6Edit.Visible = false;
                ucDirection12Edit.Visible = false;
                ucDirection18Edit.Visible = false;
                ucDirection24Edit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;

                ucSeaCondtion6Edit.Visible = false;
                ucSeaCondtion12Edit.Visible = false;
                ucSeaCondtion18Edit.Visible = false;
                ucSeaCondtion24Edit.Visible = false;
                ucSeaCondtionNext24HrsEdit.Visible = false;

                txtValueDecimal6Edit.Visible = true;
                if (txtValueDecimal6Edit != null)
                    txtValueDecimal6Edit.Text = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYVALUE").ToString();// item["FLDMETEOROLOGYVALUE"].ToString();

                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()) != null && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString()) != null && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYVALUE").ToString()) != null)
                {
                    if ((decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYVALUE").ToString())) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString())) || (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYVALUE").ToString())) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString())))
                        txtValueDecimal6Edit.CssClass = "maxhighlight";
                }


                txtValueDecimal12Edit.Visible = true;
                if (txtValueDecimal12Edit != null)
                    txtValueDecimal12Edit.Text = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1200VALUE").ToString();// item["FLDMETEOROLOGY1200VALUE"].ToString();

                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()) != null
                    && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString()) != null
                    && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1200VALUE").ToString()) != null)
                {
                    if ((decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1200VALUE").ToString())) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()))
                        || (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1200VALUE").ToString())) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString())))
                        txtValueDecimal12Edit.CssClass = "maxhighlight";
                }

                txtValueDecimal18Edit.Visible = true;
                if (txtValueDecimal18Edit != null)
                    txtValueDecimal18Edit.Text = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1800VALUE").ToString(); //item["FLDMETEOROLOGY1800VALUE"].ToString();

                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()) != null &&
                    General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString()) != null &&
                    General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1800VALUE").ToString()) != null)
                {
                    if ((decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1800VALUE").ToString())) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()))
                        || (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1800VALUE").ToString())) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString())))
                        txtValueDecimal18Edit.CssClass = "maxhighlight";
                }

                txtValueDecimal24Edit.Visible = true;
                if (txtValueDecimal24Edit != null)
                    txtValueDecimal24Edit.Text = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY2400VALUE").ToString(); //item["FLDMETEOROLOGY2400VALUE"].ToString();

                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()) != null
                    && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString()) != null
                    && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY2400VALUE").ToString()) != null)
                {
                    if ((decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY2400VALUE").ToString())) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()))
                        || (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY2400VALUE").ToString())) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString())))
                        txtValueDecimal24Edit.CssClass = "maxhighlight";
                }
                txtValueDecimalNext24HrsEdit.Visible = true;
                if (txtValueDecimalNext24HrsEdit != null)
                    txtValueDecimalNext24HrsEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString(); //item["FLDMETEOROLOGYNEXT24HRS"].ToString();

                if (lblShortname != null)
                {
                    if (lblShortname.Text == "WNS")
                    {
                        if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINFOREWINDSPEED").ToString()) != null
                            && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXFOREWINDSPEED").ToString()) != null
                            && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString()) != null)
                        {
                            if ((decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString())) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINFOREWINDSPEED").ToString()))
                                || (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString())) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXFOREWINDSPEED").ToString())))
                                txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
                        }
                    }
                    else if (lblShortname.Text == "SWH")
                    {
                        if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINFORESWELLHEIGHT").ToString()) != null
                            && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXFORESWELLHEIGHT").ToString()) != null
                            && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString()) != null)
                        {
                            if ((decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString())) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINFORESWELLHEIGHT").ToString()))
                                || (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString())) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXFORESWELLHEIGHT").ToString())))
                                txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
                        }
                    }
                    else
                    {
                        if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()) != null
                            && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString()) != null
                            && General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString()) != null)
                        {
                            if ((decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString())) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()))
                                || (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString())) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString())))
                                txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
                        }
                    }
                }
            }
            if (lblValueType.Text == "2")
            {
                ucDirection6Edit.Visible = false;
                ucDirection12Edit.Visible = false;
                ucDirection18Edit.Visible = false;
                ucDirection24Edit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;

                ucSeaCondtion6Edit.Visible = true;
                if (ucSeaCondtion6Edit != null)
                    ucSeaCondtion6Edit.SelectedSeaCondition = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYVALUE").ToString();// item["FLDMETEOROLOGYVALUE"].ToString();

                ucSeaCondtion12Edit.Visible = true;
                if (ucSeaCondtion12Edit != null)
                    ucSeaCondtion12Edit.SelectedSeaCondition = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1200VALUE").ToString();  //item["FLDMETEOROLOGY1200VALUE"].ToString();

                ucSeaCondtion18Edit.Visible = true;
                if (ucSeaCondtion18Edit != null)
                    ucSeaCondtion18Edit.SelectedSeaCondition = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1800VALUE").ToString(); //item["FLDMETEOROLOGY1800VALUE"].ToString();

                ucSeaCondtion24Edit.Visible = true;
                if (ucSeaCondtion24Edit != null)
                    ucSeaCondtion24Edit.SelectedSeaCondition = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY2400VALUE").ToString();// item["FLDMETEOROLOGY2400VALUE"].ToString();

                ucSeaCondtionNext24HrsEdit.Visible = true;
                if (ucSeaCondtionNext24HrsEdit != null)
                    ucSeaCondtionNext24HrsEdit.SelectedSeaCondition = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString();// item["FLDMETEOROLOGYNEXT24HRS"].ToString();

                txtValueDecimal6Edit.Visible = false;
                txtValueDecimal12Edit.Visible = false;
                txtValueDecimal18Edit.Visible = false;
                txtValueDecimal24Edit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblValueType.Text == "3")
            {
                ucDirection6Edit.Visible = true;
                if (ucDirection6Edit != null)
                    ucDirection6Edit.SelectedDirection = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYVALUE").ToString();  //item["FLDMETEOROLOGYVALUE"].ToString();

                ucDirection12Edit.Visible = true;
                if (ucDirection12Edit != null)
                    ucDirection12Edit.SelectedDirection = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1200VALUE").ToString();// item["FLDMETEOROLOGY1200VALUE"].ToString();

                ucDirection18Edit.Visible = true;
                if (ucDirection18Edit != null)
                    ucDirection18Edit.SelectedDirection = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY1800VALUE").ToString(); //item["FLDMETEOROLOGY1800VALUE"].ToString();

                ucDirection24Edit.Visible = true;
                if (ucDirection24Edit != null)
                    ucDirection24Edit.SelectedDirection = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGY2400VALUE").ToString();// item["FLDMETEOROLOGY2400VALUE"].ToString();

                ucDirectionNext24HrsEdit.Visible = true;
                if (ucDirectionNext24HrsEdit != null)
                    ucDirectionNext24HrsEdit.SelectedDirection = DataBinder.Eval(e.Item.DataItem, "FLDMETEOROLOGYNEXT24HRS").ToString();// item["FLDMETEOROLOGYNEXT24HRS"].ToString();

                ucSeaCondtion6Edit.Visible = false;
                ucSeaCondtion12Edit.Visible = false;
                ucSeaCondtion18Edit.Visible = false;
                ucSeaCondtion24Edit.Visible = false;
                ucSeaCondtionNext24HrsEdit.Visible = false;

                txtValueDecimal6Edit.Visible = false;
                txtValueDecimal12Edit.Visible = false;
                txtValueDecimal18Edit.Visible = false;
                txtValueDecimal24Edit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblShortname != null && lblShortname.Text == "SW")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblShortname != null && lblShortname.Text == "AIR")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblShortname != null && lblShortname.Text == "VIS")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
            if (lblShortname != null && lblShortname.Text == "BAR")
            {
                ucSeaCondtionNext24HrsEdit.Visible = false;
                ucDirectionNext24HrsEdit.Visible = false;
                txtValueDecimalNext24HrsEdit.Visible = false;
            }
        }
    }

    protected void gvMeteorologyData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindMeteorologyData();
    }
    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem) //if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridDataItem drv = (GridDataItem)e.Item;



                LinkButton lblWorkOrderNo = (LinkButton)drv.FindControl("lblWorkOrderNo");
                RadLabel lblWorkOrderId = (RadLabel)drv.FindControl("lblWorkOrderId");
                lblWorkOrderNo.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + lblWorkOrderId.Text + "'); return false;");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindWorkOrder();
    }

    protected void gvPMSoverdue_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            GridDataItem drv = (GridDataItem)e.Item;

            LinkButton lnkCount = (LinkButton)drv.FindControl("lnkCount");
            RadLabel lblType = (RadLabel)drv.FindControl("lblType");
            //lnkCount.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobAuditList.aspx?cjobid=" + compJobID.Text + "'); return false;");
            lnkCount.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
            //lnkCount.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

        }
    }

 

    protected void gvPMSoverdue_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPMSoverdue();
    }

    protected void gvExternalInspection_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            //RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.RowIndex;

            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }

            string lblExternalInspectionIdEdit = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDEXTERNALINSPECTIONID"].ToString();// "lblExternalInspectionIdEdit")).Text;
            string txtTypeOfInspectionEdit = ((RadTextBox)eeditedItem.FindControl("txtTypeOfInspectionEdit")).Text;
            string txtInspectingCompanyEdit = ((RadTextBox)eeditedItem.FindControl("txtInspectingCompanyEdit")).Text;
            string txtInspectorNameEdit = ((RadTextBox)eeditedItem.FindControl("txtInspectorNameEdit")).Text;
            string txtNumberOfNCEdit = ((UserControlNumber)eeditedItem.FindControl("txtNumberOfNCEdit")).Text;

            if (!IsValidExternalInspection(txtTypeOfInspectionEdit, txtInspectingCompanyEdit, txtInspectorNameEdit, txtNumberOfNCEdit))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                    , General.GetNullableGuid(lblExternalInspectionIdEdit), txtTypeOfInspectionEdit, txtInspectingCompanyEdit, txtInspectorNameEdit
                    , General.GetNullableInteger(txtNumberOfNCEdit));

            // _gridView.EditIndex = -1;
            BindExternalInspection();
            EditMidNightReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExternalInspection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (Session["MIDNIGHTREPORTID"] == null)
                {
                    ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                    ucError.Visible = true;
                    return;
                }

                GridFooterItem footerItem = (GridFooterItem)_gridView.MasterTableView.GetItems(GridItemType.Footer)[0];
                // Button btn = (Button)footerItem.FindControl("Button1");
                RadLabel TotalFOC = (RadLabel)footerItem.FindControl("lblTotalFOC");

                string txtTypeOfInspectionAdd = ((RadTextBox)footerItem.FindControl("txtTypeOfInspectionAdd")).Text;
                string txtInspectingCompanyAdd = ((RadTextBox)footerItem.FindControl("txtInspectingCompanyAdd")).Text;
                string txtInspectorNameAdd = ((RadTextBox)footerItem.FindControl("txtInspectorNameAdd")).Text;
                string txtNumberOfNCAdd = ((UserControlNumber)footerItem.FindControl("txtNumberOfNCAdd")).Text;


                if (!IsValidExternalInspection(txtTypeOfInspectionAdd, txtInspectingCompanyAdd, txtInspectorNameAdd, txtNumberOfNCAdd))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
                        , null, txtTypeOfInspectionAdd, txtInspectingCompanyAdd, txtInspectorNameAdd, General.GetNullableInteger(txtNumberOfNCAdd));

                //    string FromTime = (((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text.Trim() == "__:__") ? string.Empty : ((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text;
                //    string Activity = ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text;
                //    string ddlActivity = ((DropDownList)_gridView.FooterRow.FindControl("ddlActivityAdd")).SelectedValue;

                BindExternalInspection();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    string lblExternalInspectionId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDEXTERNALINSPECTIONID"].ToString();
                    if (lblExternalInspectionId != "")
                    {
                        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionDelete(new Guid(Session["MIDNIGHTREPORTID"].ToString()), new Guid(lblExternalInspectionId));
                    }
                }
                BindExternalInspection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvExternalInspection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindExternalInspection();
    }
    protected void gvOperationalTimeSummary_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem) //if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridDataItem item = (GridDataItem)e.Item;

            ImageButton db = (ImageButton)item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DataRowView drv = (DataRowView)item.DataItem;
            UserControlDecimal ucFuelConsHrEdit = (UserControlDecimal)item.FindControl("ucFuelConsHrEdit");
            UserControlDecimal ucFuelConsumptionEdit = (UserControlDecimal)item.FindControl("ucFuelConsumptionEdit");
            RadLabel lblTimeDurationEdit = (RadLabel)item.FindControl("lblTimeDurationEdit");
            RadLabel lblDistanceApplicable = (RadLabel)item.FindControl("lblDistanceApplicable");
            UserControlDecimal ucSeaStreamDistanceEdit = (UserControlDecimal)item.FindControl("ucSeaStreamDistanceEdit");
            UserControlDecimal ucSpeedEdit = (UserControlDecimal)item.FindControl("ucSpeedEdit");

            if (ucFuelConsumptionEdit != null && lblTimeDurationEdit != null)
            {
                if (General.GetNullableDecimal(ucFuelConsumptionEdit.Text) != null && General.GetNullableDecimal(ucFuelConsumptionEdit.Text) != 0)
                {
                    decimal timeDuration = Convert.ToDecimal(lblTimeDurationEdit.Text);
                    if (ucFuelConsHrEdit != null)
                        ucFuelConsHrEdit.Text = (Convert.ToDecimal(ucFuelConsumptionEdit.Text) / (Math.Floor(timeDuration) + ((timeDuration - Math.Floor(timeDuration)) * 100 / 60))).ToString();
                }
            }

            if (General.GetNullableDecimal(ucFuelConsHrEdit.Text) != null)
            {
                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()) != null
                    && ((decimal.Parse(ucFuelConsHrEdit.Text)) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()))))
                {
                    ucFuelConsHrEdit.CssClass = "maxhighlight";
                }
                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString()) != null
                    && (decimal.Parse(ucFuelConsHrEdit.Text)) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString())))
                {
                    ucFuelConsHrEdit.CssClass = "maxhighlight";
                }
            }
            //if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(ucFuelConsHrEdit.Text) != null)
            //{
            //    if ((decimal.Parse(ucFuelConsHrEdit.Text)) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(ucFuelConsHrEdit.Text)) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
            //        ucFuelConsHrEdit.CssClass = "maxhighlight";
            //}
            if (lblDistanceApplicable != null && ucSeaStreamDistanceEdit != null)
            {
                if (lblDistanceApplicable.Text == "0")
                {
                    ucSeaStreamDistanceEdit.CssClass = "readonlytextbox";
                    ucSeaStreamDistanceEdit.ReadOnly = "true";
                }
                if (ucSeaStreamDistanceEdit != null && General.GetNullableDecimal(ucSeaStreamDistanceEdit.Text) != null)
                {
                    decimal timeDuration = Convert.ToDecimal(lblTimeDurationEdit.Text);
                    ucSpeedEdit.Text = (Convert.ToDecimal(ucSeaStreamDistanceEdit.Text) / (Math.Floor(timeDuration) + ((timeDuration - Math.Floor(timeDuration)) * 100 / 60))).ToString();

                }
            }

            if (General.GetNullableDecimal(ucSpeedEdit.Text) != null)
            {
                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUESPD").ToString()) != null
                    && ((decimal.Parse(ucSpeedEdit.Text)) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUESPD").ToString()))))
                {
                    ucSpeedEdit.CssClass = "maxhighlight";
                }
                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUESPD").ToString()) != null
                    && (decimal.Parse(ucSpeedEdit.Text)) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUESPD").ToString())))
                {
                    ucSpeedEdit.CssClass = "maxhighlight";
                }
            }
        }
    }

    protected void gvOperationalTimeSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOperationalTimeSummary();
    }

    protected void gvOperationalTimeSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    string OperationalTimeSummaryId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTimeSummaryId")).Text;
                    if (OperationalTimeSummaryId != "")
                    {
                        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryDelete(new Guid(OperationalTimeSummaryId));
                    }
                }
                BindOperationalTimeSummary();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBulks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOilLoadandConsumption();
    }


    protected void gvRequisionsandPO_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindRequisionsandPO();
    }

    protected void gvRequisionsandPO_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lnk1 = (LinkButton)e.Item.FindControl("lblCount");
            RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
            lnk1.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRRequisitionandPODetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

            //lnk1.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRRequisitionandPODetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

        }
    }

    protected void gvCertificates_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days <= 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
                }
                if (t.Days > 15 && t.Days <= 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days <= 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
            }
        }
    }

    protected void gvShipAudit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblDueDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days <= 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
                }
                if (t.Days > 15 && t.Days <= 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days <= 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
            }
        }
    }
    protected void gvShipAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDueAudits();
    }
    protected void gvCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDueCertificates();
    }

    protected void gvShipTasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDueShipTasks();
    }

    protected void gvShipTasks_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblDueDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days <= 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
                }
                if (t.Days > 15 && t.Days <= 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days <= 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
            }
        }
    }



}



