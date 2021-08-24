using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;

public partial class CrewOffshoreDMRMidNightReportHSE : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            //cmdHiddenSubmit.Attributes.Add("style", "display:none");
            toolbarReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

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
            MenuReportTap.SelectedMenuIndex = 3;

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarhse = new PhoenixToolbar();
            toolbarhse.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReportHSE.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuHSEIndicators.AccessRights = this.ViewState;
            MenuHSEIndicators.MenuList = toolbarhse.Show();

            PhoenixToolbar toolbarUnsafe = new PhoenixToolbar();
            toolbarUnsafe.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReportHSE.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuUnsafeActsAdd.AccessRights = this.ViewState;
            MenuUnsafeActsAdd.MenuList = toolbarUnsafe.Show();

            if (!IsPostBack)
            {
                BindInstallationType();
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

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["MonthlyReportId"] = "";

                ddlETALocation.DataSource = PhoenixRegistersDMRLocation.DMRLocationList();
                ddlETALocation.DataBind();

                ddlETDLocation.DataSource = PhoenixRegistersDMRLocation.DMRLocationList();
                ddlETDLocation.DataBind();

                // ddlETALocation.Items.Insert(0, new ListItem("--Select--", ""));
                // ddlETDLocation.Items.Insert(0, new ListItem("--Select--", ""));

                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    EditMidNightReport();
                }

                BindHSEIndicators();
                BindPlannedActivity();
                BindDueAudits();
                BindDueShipTasks();
                BindDueCertificates();
                BindUnsafeActs();
                BindLaggingIndicators();
                ChangeVesselStatus();
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
            txtETDTime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDETDDATE"].ToString());
            //txtETDTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETDDATE"]);
            ddlETDLocation.SelectedValue = ds.Tables[0].Rows[0]["FLDETDLOCATIONID"].ToString();
            txtETADate.Text = ds.Tables[0].Rows[0]["FLDETADATE"].ToString();
            txtETATime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDETADATE"].ToString());
            //txtETATime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETADATE"]);
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
            txtPOBClient.Text = ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString();
            txtPOBService.Text = ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString();
            txtTotalOB.Text = ds.Tables[0].Rows[0]["FLDTOTALONBOARD"].ToString();
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

            txtArrivalDate.Text = ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString();
            txtArrivalTime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString());
            // txtArrivalTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDARRIVALDATE"]);

            txtDepartureDate.Text = ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString();
            txtDepartureTime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString());
            //txtDepartureTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"]);
            txtEstimatedDuration.Text = ds.Tables[0].Rows[0]["FLDESTIMATEDDURATION"].ToString();
            ddlInstalationType.SelectedValue = ds.Tables[0].Rows[0]["FLDINSTALLATIONTYPEID"].ToString();

            ddlCrewListSOBYN.SelectedValue = ds.Tables[0].Rows[0]["FLDCREWLISTDIFFSOBYN"].ToString();
            txtCrewListSOBRemarks.Text = ds.Tables[0].Rows[0]["FLDCREWLISTDIFFSOBREMARKS"].ToString();

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

            if (ds.Tables[0].Rows[0]["FLDINCIDENTYN"].ToString() != string.Empty)
            {
                rblHSEIndicators.SelectedValue = ds.Tables[0].Rows[0]["FLDINCIDENTYN"].ToString();
            }
            if (rblHSEIndicators.SelectedValue == "1")
                MenuHSEIndicators.Visible = true;
            else
                MenuHSEIndicators.Visible = false;

            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataTable dt = PhoenixCrewOffshoreDMRMidNightReport.SearchMidnightReportVesselCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                       , General.GetNullableDateTime(ViewState["REPORTDATE"].ToString())
                                                                       , null, null
                                                                       , 1, 25
                                                                       , ref iRowCount, ref iTotalPageCount);



            if (dt.Rows.Count > 0)
            {

                //txtCrewOff.Text = dt.Rows[0]["FLDCREWOFF"].ToString();
                //txtCrewOn.Text = dt.Rows[0]["FLDCREWON"].ToString();
                //txtPOBCrew.Text = (int.Parse(dt.Rows[0]["FLDCREWOFF"].ToString()) + int.Parse(dt.Rows[0]["FLDCREWON"].ToString())).ToString();
                //txtTotalOB.Text = (int.Parse(dt.Rows[0]["FLDCREWOFF"].ToString()) + int.Parse(dt.Rows[0]["FLDCREWON"].ToString()) + (General.GetNullableInteger(txtPOBClient.Text) == null ? 0 : int.Parse(txtPOBClient.Text)) + (General.GetNullableInteger(txtPOBService.Text) == null ? 0 : int.Parse(txtPOBService.Text))).ToString();
                txtPOB.Text = (int.Parse(dt.Rows[0]["FLDPOWCREW"].ToString()) + int.Parse(dt.Rows[0]["FLDPOBCLIENT"].ToString()) + int.Parse(dt.Rows[0]["FLDPOBSERVICE"].ToString())).ToString();
                txtCrew.Text = dt.Rows[0]["FLDPOWCREW"].ToString();
                //txtMaster.Text = ds.Tables[0].Rows[0]["FLDMASTEROFSHIP"].ToString();
            }
        }
    }

    protected void VesselStatus(object sender, EventArgs e)
    {
        txtETADate.Text = "";
        txtETATime.SelectedTime = null;
        txtETDDate.Text = "";
        txtETDTime.SelectedTime = null;
        txtArrivalDate.Text = "";
        txtArrivalTime.SelectedTime = null;
        txtDepartureDate.Text = "";
        txtDepartureTime.SelectedTime = null;

        ChangeVesselStatus();
    }

    // Look Ahead Planned Activity
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

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveMidnightReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    // Bulks

    // Meteorology Data

    private void UpdateHseIndicatorData()
    {

        foreach (GridDataItem gvr in gvHSEIndicators.Items)
        {
            RadLabel hseindicator = (RadLabel)gvr.FindControl("lblhpiitem");
            if (gvHSEIndicators.Items.Count > 0 && hseindicator.Text == "Last 24 hrs")
            {
                RadLabel eh = (RadLabel)gvr.FindControl("lblEH");
                RadLabel hpi = (RadLabel)gvr.FindControl("lblhpi");
                RadLabel lti = (RadLabel)gvr.FindControl("lbllit");
                RadLabel rwc = (RadLabel)gvr.FindControl("lblrwc");
                RadLabel mtc = (RadLabel)gvr.FindControl("lblmtc");
                RadLabel fac = (RadLabel)gvr.FindControl("lblfac");
                RadLabel EnvironmentalIncident = (RadLabel)gvr.FindControl("lblEnvironmentalIncident");
                UserControlNumber stopcards = (UserControlNumber)gvr.FindControl("ucstopcards");
                RadLabel nearmiss = (RadLabel)gvr.FindControl("lblnearmiss");
                UserControlNumber ExercisesandDrills = (UserControlNumber)gvr.FindControl("ucExercisesandDrills");
                RadLabel NoofRiskAssesment = (RadLabel)gvr.FindControl("lblNoofRiskAssesment");
                UserControlNumber noofsafety = (UserControlNumber)gvr.FindControl("ucnoofsafety");
                UserControlNumber PTWIssued = (UserControlNumber)gvr.FindControl("ucPTWIssued");
                RadLabel UnsafeActs = (RadLabel)gvr.FindControl("lblUnsafeActs");

                int Crew = (General.GetNullableInteger(txtCrew.Text) == null ? 0 : int.Parse(txtCrew.Text));

                double exposurehour = 0;
                if ((rbnhourchange.SelectedValue) == "1" || (rbnhourchange.SelectedValue) == "2")
                {
                    if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "1")
                    {
                        exposurehour = (Crew * 23.5);
                    }
                    if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "2")
                    {
                        exposurehour = (Crew * 23);
                    }
                    if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "1")
                    {
                        exposurehour = (Crew * 24.5);
                    }
                    if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "2")
                    {
                        exposurehour = (Crew * 25);
                    }
                }
                else
                {
                    exposurehour = (Crew * 24);
                }
                eh.Text = exposurehour.ToString();

                if (hseindicator.Text != "")
                {
                    PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReporthseindicatorDataInsert(
                                                                                                        new Guid(Session["MIDNIGHTREPORTID"].ToString()),
                                                                                                        int.Parse(ViewState["VESSELID"].ToString()),
                                                                                                        DateTime.Parse(txtDate.Text),
                                                                                                        hseindicator.Text,
                                                                                                        General.GetNullableInteger(hpi.Text),
                                                                                                        General.GetNullableInteger(lti.Text),
                                                                                                        General.GetNullableInteger(rwc.Text),
                                                                                                        General.GetNullableInteger(mtc.Text),
                                                                                                        General.GetNullableInteger(fac.Text),
                                                                                                        General.GetNullableInteger(EnvironmentalIncident.Text),
                                                                                                        General.GetNullableInteger(stopcards.Text),
                                                                                                        General.GetNullableInteger(nearmiss.Text),
                                                                                                        General.GetNullableInteger(ExercisesandDrills.Text),
                                                                                                        General.GetNullableInteger(NoofRiskAssesment.Text),
                                                                                                        General.GetNullableInteger(noofsafety.Text),
                                                                                                        General.GetNullableInteger(PTWIssued.Text),
                                                                                                        General.GetNullableDecimal(eh.Text),
                                                                                                        General.GetNullableInteger(UnsafeActs.Text));
                }

            }
        }
        BindHSEIndicators();
    }


    // Look Ahead Planned Activity

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
    // Vessel's Movements

    // Ship's Crew

    // HSE Indicators
    private void BindHSEIndicators()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportHSEIndicatorsList(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvHSEIndicators.DataSource = ds;
            gvHSEIndicators.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHSEIndicators_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            TableCell tbHSEIndicators = new TableCell();
            TableCell tbLaggingIndicators = new TableCell();
            TableCell tbLeadingIndicators = new TableCell();

            tbHSEIndicators.ColumnSpan = 1;
            tbLaggingIndicators.ColumnSpan = 7;
            tbLeadingIndicators.ColumnSpan = 7;

            tbHSEIndicators.Text = "HSE Indicators";
            tbLaggingIndicators.Text = "Lagging Indicators";
            tbLeadingIndicators.Text = "Leading Indicators";

            tbHSEIndicators.Attributes.Add("style", "text-align:center;");
            tbLaggingIndicators.Attributes.Add("style", "text-align:center;");
            tbLeadingIndicators.Attributes.Add("style", "text-align:center;");

            gv.Cells.Add(tbHSEIndicators);
            gv.Cells.Add(tbLaggingIndicators);
            gv.Cells.Add(tbLeadingIndicators);

            gvHSEIndicators.Controls[0].Controls.AddAt(0, gv);
        }
    }

    protected void gvHSEIndicators_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            Label lblEH = (Label)e.Row.FindControl("lblEHHeader");
            UserControlToolTip ucEH = (UserControlToolTip)e.Row.FindControl("ucEHHeader");
            lblEH.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucEH.ToolTip + "', 'visible');");
            lblEH.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucEH.ToolTip + "', 'hidden');");

            Label lblhpi = (Label)e.Row.FindControl("lblhpiHeader");
            UserControlToolTip ucthpi = (UserControlToolTip)e.Row.FindControl("uchpiHeader");
            lblhpi.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'visible');");
            lblhpi.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'hidden');");


            Label lbllti = (Label)e.Row.FindControl("lblltiHeader");
            UserControlToolTip uctlti = (UserControlToolTip)e.Row.FindControl("ucltiHeader");
            lbllti.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctlti.ToolTip + "', 'visible');");
            lbllti.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctlti.ToolTip + "', 'hidden');");


            Label lblrwc = (Label)e.Row.FindControl("lblrwcHeader");
            UserControlToolTip uctrwc = (UserControlToolTip)e.Row.FindControl("ucrwcHeader");
            lblrwc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'visible');");
            lblrwc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'hidden');");


            Label lblmtc = (Label)e.Row.FindControl("lblmtcHeader");
            UserControlToolTip uctmtc = (UserControlToolTip)e.Row.FindControl("ucmtcHeader");
            lblmtc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'visible');");
            lblmtc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'hidden');");

            Label lblfac = (Label)e.Row.FindControl("lblfacHeader");
            UserControlToolTip uctfac = (UserControlToolTip)e.Row.FindControl("ucfacHeader");
            lblfac.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctfac.ToolTip + "', 'visible');");
            lblfac.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctfac.ToolTip + "', 'hidden');");

            Label lblEnvInc = (Label)e.Row.FindControl("lblEnvironmentalIncidentHeader");
            UserControlToolTip uctEnvInc = (UserControlToolTip)e.Row.FindControl("ucEnvironmentalIncidentHeader");
            lblEnvInc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'visible');");
            lblEnvInc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'hidden');");


            Label lblnmr = (Label)e.Row.FindControl("lblnearmissHeader");
            UserControlToolTip uctnmr = (UserControlToolTip)e.Row.FindControl("ucnearmissHeader");
            lblnmr.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'visible');");
            lblnmr.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'hidden');");

            Label lblstpcrd = (Label)e.Row.FindControl("lblstopcardsHeader");
            UserControlToolTip uctstpcrd = (UserControlToolTip)e.Row.FindControl("ucstopcardsHeader");
            lblstpcrd.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'visible');");
            lblstpcrd.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'hidden');");

            Label lbled = (Label)e.Row.FindControl("lblExercisesandDrillsHeader");
            UserControlToolTip ucted = (UserControlToolTip)e.Row.FindControl("ucExercisesandDrillsHeader");
            lbled.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucted.ToolTip + "', 'visible');");
            lbled.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucted.ToolTip + "', 'hidden');");

            Label lblra = (Label)e.Row.FindControl("lblNoofRiskAssesmentHeader");
            UserControlToolTip uctra = (UserControlToolTip)e.Row.FindControl("ucNoofRiskAssesmentHeader");
            lblra.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctra.ToolTip + "', 'visible');");
            lblra.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctra.ToolTip + "', 'hidden');");

            Label lblsfty = (Label)e.Row.FindControl("lblnoofsafetyHeader");
            UserControlToolTip uctsfty = (UserControlToolTip)e.Row.FindControl("ucnoofsafetyHeader");
            lblsfty.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'visible');");
            lblsfty.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'hidden');");

            Label lblptw = (Label)e.Row.FindControl("lblPTWIssuedHeader");
            UserControlToolTip uctptw = (UserControlToolTip)e.Row.FindControl("uclblPTWIssuedHeader");
            lblptw.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctptw.ToolTip + "', 'visible');");
            lblptw.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctptw.ToolTip + "', 'hidden');");
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblEH = (Label)e.Row.FindControl("lblEH");
            int Crew = (General.GetNullableInteger(txtCrew.Text) == null ? 0 : int.Parse(txtCrew.Text));
            Label lblHSEIndicator = (Label)e.Row.FindControl("lblhpiitem");

            if (lblHSEIndicator.Text == "Last 24 hrs")
            {
                double exposurehour = 0;
                if ((rbnhourchange.SelectedValue) == "1" || (rbnhourchange.SelectedValue) == "2")
                {
                    if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "1")
                    {
                        exposurehour = (Crew * 23.5);
                    }
                    if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "2")
                    {
                        exposurehour = (Crew * 23);
                    }
                    if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "1")
                    {
                        exposurehour = (Crew * 24.5);
                    }
                    if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "2")
                    {
                        exposurehour = (Crew * 25);
                    }
                }
                else
                {
                    exposurehour = (Crew * 24);
                }
                lblEH.Text = exposurehour.ToString();
                UserControlMaskNumber ucstopcards = (UserControlMaskNumber)e.Row.FindControl("ucstopcards");
                UserControlMaskNumber ucExercisesandDrills = (UserControlMaskNumber)e.Row.FindControl("ucExercisesandDrills");
                UserControlMaskNumber ucnoofsafety = (UserControlMaskNumber)e.Row.FindControl("ucnoofsafety");
                UserControlMaskNumber ucPTWIssued = (UserControlMaskNumber)e.Row.FindControl("ucPTWIssued");

                Label lblstopcards = (Label)e.Row.FindControl("lblstopcards");
                Label lblExercisesandDrills = (Label)e.Row.FindControl("lblExercisesandDrills");
                Label lblnoofsafety = (Label)e.Row.FindControl("lblnoofsafety");
                Label lblPTWIssued = (Label)e.Row.FindControl("lblPTWIssued");

                ucstopcards.Visible = true;
                ucExercisesandDrills.Visible = true;
                ucnoofsafety.Visible = true;
                ucPTWIssued.Visible = true;

                lblstopcards.Visible = false;
                lblExercisesandDrills.Visible = false;
                lblnoofsafety.Visible = false;
                lblPTWIssued.Visible = false;
            }
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
                TextBox tt = (TextBox)gv.FooterRow.FindControl("txtFromTimeAdd");
                tt.Text = "00:00";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void BindUnsafeActs()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportUnsafeActs(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvUnsafe.DataSource = ds;
            gvUnsafe.DataBind();

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
            txtETDTime.Enabled = false;//txtETDTime.ReadOnly = true;
            txtETDDate.CssClass = "readonlytextbox";
            txtETDTime.CssClass = "readonlytextbox";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            txtETATime.Enabled = false;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            txtETATime.CssClass = "readonlytextbox";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            txtArrivalTime.CssClass = "input";
            txtArrivalTime.Enabled = true;//txtArrivalTime.ReadOnly = false;
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            txtDepartureTime.CssClass = "input";
            txtDepartureTime.Enabled = true;//txtDepartureTime.ReadOnly = false;

            txtDepartureDate.Enabled = true;
            txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = true;
            txtArrivalTime.Enabled = true;
            txtETADate.Enabled = true;
            txtETATime.Enabled = true;
            txtETDDate.Enabled = true;
            txtETDTime.Enabled = true;

        }
        if (ddlvesselstatus.SelectedValue == "1")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETDDate.ReadOnly = false;
            txtETDTime.Enabled = true;//txtETDTime.ReadOnly = false;
            txtETDDate.CssClass = "input";
            txtETDTime.CssClass = "input";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            txtETATime.Enabled = false;//txtETATime.ReadOnly = true;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            txtETATime.CssClass = "readonlytextbox";

            ddlETALocation.Text = "";
            ddlETDLocation.Text = "";
            txtETADate.Text = "";
            txtETATime.SelectedTime = null;

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            txtArrivalTime.CssClass = "input";
            txtArrivalTime.Enabled = true;//txtArrivalTime.ReadOnly = false;
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
            txtDepartureTime.CssClass = "readonlytextbox";
            txtDepartureTime.Enabled = false;//txtDepartureTime.ReadOnly = true;

            txtDepartureDate.Enabled = false;
            txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = true;
            txtArrivalTime.Enabled = true;
            txtETADate.Enabled = false;
            txtETATime.Enabled = false;
            txtETDDate.Enabled = true;
            txtETDTime.Enabled = true;
        }
        if (ddlvesselstatus.SelectedValue == "2")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETADate.ReadOnly = false;
            txtETATime.Enabled = true;//txtETATime.ReadOnly = false;
            txtETADate.CssClass = "input";
            txtETATime.CssClass = "input";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETDDate.ReadOnly = true;
            txtETDTime.Enabled = false;//txtETDTime.ReadOnly = true;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETDDate.CssClass = "readonlytextbox";
            txtETDTime.CssClass = "readonlytextbox";

            ddlETALocation.Text = "";
            ddlETDLocation.Text = "";
            txtETDDate.Text = "";
            txtETDTime.SelectedTime = null;

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            txtArrivalTime.CssClass = "readonlytextbox";
            txtArrivalTime.Enabled = false;//txtArrivalTime.ReadOnly = true;
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            txtDepartureTime.CssClass = "input";
            //txtDepartureTime.ReadOnly = false;

            txtDepartureDate.Enabled = true;
            txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = false;
            txtArrivalTime.Enabled = false;
            txtETADate.Enabled = true;
            txtETATime.Enabled = true;
            txtETDDate.Enabled = false;
            txtETDTime.Enabled = false;

        }
        if (ddlvesselstatus.SelectedValue == "3")
        {
            ddlETALocation.Enabled = true;
            txtETADate.ReadOnly = false;
            txtETATime.Enabled = true;// txtETATime.ReadOnly = false;
            ddlETALocation.CssClass = "input_mandatory";
            txtETADate.CssClass = "input_mandatory";
            txtETATime.CssClass = "input_mandatory";
            txtLocation.CssClass = "input_mandatory";
            txtLocation.ReadOnly = false;

            ucPort.Enabled = false;
            txtETDDate.ReadOnly = true;
            txtETDTime.Enabled = false;//txtETDTime.ReadOnly = true;
            ddlETDLocation.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETDDate.CssClass = "readonlytextbox";
            txtETDTime.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";

            ucPort.Text = "";
            txtETDDate.Text = "";
            txtETDTime.SelectedTime = null;
            ddlETDLocation.Text = "";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            txtArrivalTime.CssClass = "readonlytextbox";
            txtArrivalTime.Enabled = false;//txtArrivalTime.ReadOnly = true;
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            txtDepartureTime.CssClass = "input";
            //txtDepartureTime.ReadOnly = false;

            txtDepartureDate.Enabled = true;
            txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = false;
            txtArrivalTime.Enabled = false;
            txtETADate.Enabled = true;
            txtETATime.Enabled = true;
            txtETDDate.Enabled = false;
            txtETDTime.Enabled = false;

        }
        if (ddlvesselstatus.SelectedValue == "4")
        {
            ddlETDLocation.Enabled = true;
            txtETDDate.ReadOnly = false;
            txtETDTime.Enabled = true;//txtETDTime.ReadOnly = false;
            txtETDTime.CssClass = "input";
            txtETDDate.CssClass = "input";
            ddlETDLocation.CssClass = "input_mandatory";
            txtLocation.CssClass = "input_mandatory";
            txtLocation.ReadOnly = false;

            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETADate.ReadOnly = true;
            txtETATime.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            ddlETALocation.Enabled = false;
            ddlETALocation.CssClass = "readonlytextbox";

            ucPort.Text = "";
            txtETATime.SelectedDate = null;
            txtETADate.Text = "";
            ddlETALocation.Text = "";

            ddlInstalationType.CssClass = "input";
            ddlInstalationType.Enabled = true;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            txtArrivalTime.CssClass = "input";
            txtArrivalTime.Enabled = true;//txtArrivalTime.ReadOnly = false;
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
            txtDepartureTime.CssClass = "readonlytextbox";
            //txtDepartureTime.ReadOnly = true;

            txtDepartureDate.Enabled = false;
            txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = true;
            txtArrivalTime.Enabled = true;
            txtETADate.Enabled = false;
            txtETATime.Enabled = false;
            txtETDDate.Enabled = true;
            txtETDTime.Enabled = true;
        }
        if (ddlvesselstatus.SelectedValue == "5")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETDDate.ReadOnly = true;
            txtETDTime.Enabled = false;//txtETDTime.ReadOnly = true;
            txtETDDate.CssClass = "readonlytextbox";
            txtETDTime.CssClass = "readonlytextbox";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            txtETATime.Enabled = false;//txtETATime.ReadOnly = true;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            txtETATime.CssClass = "readonlytextbox";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            txtArrivalTime.CssClass = "readonlytextbox";
            txtArrivalTime.Enabled = false;//txtArrivalTime.ReadOnly = true;
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
            txtDepartureTime.CssClass = "readonlytextbox";
            //txtDepartureTime.ReadOnly = true;

            txtDepartureDate.Enabled = false;
            txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = false;
            txtArrivalTime.Enabled = false;
            txtETADate.Enabled = false;
            txtETATime.Enabled = false;
            txtETDDate.Enabled = false;
            txtETDTime.Enabled = false;
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

    protected void rblHSEIndicators_OnSelectedIndexChanged(Object sender, EventArgs args)
    {
        if (rblHSEIndicators.SelectedValue == "1")
            MenuHSEIndicators.Visible = true;
        else
            MenuHSEIndicators.Visible = false;
    }

    private void BindLaggingIndicators()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportLaggingIndicators(int.Parse(ViewState["VESSELID"].ToString())
                , DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));

            gvLaggingIndicators.DataSource = ds;
            gvLaggingIndicators.DataBind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLaggingIndicators_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //DataRowView drv = (DataRowView)e.Row.DataItem;

                //LinkButton lblReferenceNumber = (LinkButton)e.Row.FindControl("lblReferenceNumber");
                //Label lblReferenceId = (Label)e.Row.FindControl("lblReferenceId");
                //Filter.CurrentIncidentID = lblReferenceId.Text;
                //Filter.CurrentIncidentTab = "INCIDENTDETAILS";
                //lblReferenceNumber.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx'); return false;");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLaggingIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "EDIT")
            {
                GridView _gridView = (GridView)sender;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }

                SaveMidnightReport();

                LinkButton lblReferenceNumber = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblReferenceNumber");
                Label lblReferenceId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblReferenceId");
                Filter.CurrentIncidentID = lblReferenceId.Text;
                Filter.CurrentIncidentTab = "INCIDENTDETAILS";

                Response.Redirect("../Inspection/InspectionIncidentList.aspx", true);

                //lblReferenceNumber.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuHSEIndicators_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
            {
                ucError.HeaderMessage = "Please provide the following required information";
                ucError.ErrorMessage = "Select the Vessel Status";
                ucError.Visible = true;
                return;
            }

            SaveMidnightReport();
            Response.Redirect("../Inspection/InspectionIncidentAdd.aspx?module=DMR&date=" + txtDate.Text, true);

            //toolbarhse.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionIncidentAdd.aspx?module=DMR&date=" + txtDate.Text + "')", "Add", "add.png", "ADD");
        }
    }
    protected void MenuUnsafeActsAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
            {
                ucError.HeaderMessage = "Please provide the following required information";
                ucError.ErrorMessage = "Select the Vessel Status";
                ucError.Visible = true;
                return;
            }

            SaveMidnightReport();

            String scriptpopup = String.Format(
               "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsConditionsAdd.aspx');");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            //toolbarhse.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionIncidentAdd.aspx?module=DMR&date=" + txtDate.Text + "')", "Add", "add.png", "ADD");
        }
    }
    private void SaveMidnightReport()
    {
        if (Session["MIDNIGHTREPORTID"] == null)
        {

            UpdateHseIndicatorData();
            ucStatus.Text = "MidNight Report Created";
        }
        else
        {

            string timeofetd = string.Empty;
            string timeofeta = string.Empty;
            string timeofarraival = string.Empty;
            string timeofdeparture = string.Empty;
            if (txtETDTime.SelectedTime != null) timeofetd = txtETDTime.SelectedTime.Value.ToString();
            if (txtETATime.SelectedTime != null) timeofeta = txtETATime.SelectedTime.Value.ToString();
            if (txtArrivalTime.SelectedTime != null) timeofarraival = txtArrivalTime.SelectedTime.Value.ToString();
            if (txtDepartureTime.SelectedTime != null) timeofdeparture = txtDepartureTime.SelectedTime.Value.ToString();

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportUpdate(new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                , General.GetNullableDateTime(txtETDDate.Text + " " + timeofetd)
                , General.GetNullableDateTime(txtETADate.Text + " " + timeofeta)
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
                , General.GetNullableDateTime(txtArrivalDate.Text + " " + timeofarraival), General.GetNullableDateTime(txtDepartureDate.Text + " " + timeofdeparture)
                , General.GetNullableInteger(ucTea1.Text), General.GetNullableInteger(ucTea2.Text), General.GetNullableInteger(ucSupper.Text)
                , General.GetNullableInteger(ucSupBreakFast.Text), General.GetNullableInteger(ucSupLunch.Text), General.GetNullableInteger(ucSupDinner.Text), General.GetNullableInteger(ucSupSupper.Text)
                , General.GetNullableInteger(ucSupTea1.Text), General.GetNullableInteger(ucSupTea2.Text)
                , null, General.GetNullableInteger(rblHSEIndicators.SelectedValue)
                , General.GetNullableInteger(ddlInstalationType.SelectedValue)
                , General.GetNullableInteger(ddlCrewListSOBYN.SelectedValue), txtCrewListSOBRemarks.Text
             );


            UpdateHseIndicatorData();
            UpdateLookAheadActivity();

            ucStatus.Text = "MidNight Report Updated";
        }
        EditMidNightReport();
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

    protected void gvUnsafe_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {

    }

    //protected void gvUnsafe_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper() == "EDIT")
    //        {
    //            GridView _gridView = (GridView)sender;
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //            if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
    //            {
    //                ucError.HeaderMessage = "Please provide the following required information";
    //                ucError.ErrorMessage = "Select the Vessel Status";
    //                ucError.Visible = true;
    //                return;
    //            }

    //            SaveMidnightReport();

    //            LinkButton lblUnsafeActs = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblUnsafeActs");
    //            Label lblDirectIncidentId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDirectIncidentId");

    //            Response.Redirect("../Inspection/InspectionUnsafeActsConditions.aspx?directincidentid=" + lblDirectIncidentId.Text, true);
    //            //lblReferenceNumber.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx'); return false;");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
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
            //ddlInstalationType.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    // Passenger

    // Operational Time Summary

    //telerik
    protected void ddlvesselstatus_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        txtETADate.Text = "";
        txtETATime.SelectedTime = null;
        txtETDDate.Text = "";
        txtETDTime.SelectedTime = null;
        txtArrivalDate.Text = "";
        txtArrivalTime.SelectedTime = null;
        txtDepartureDate.Text = "";
        txtDepartureTime.SelectedTime = null;

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

    protected void gvUnsafe_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "EDIT")
            {
                GridView _gridView = (GridView)sender;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }

                SaveMidnightReport();

                LinkButton lblUnsafeActs = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblUnsafeActs");
                RadLabel lblDirectIncidentId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDirectIncidentId");

                Response.Redirect("../Inspection/InspectionUnsafeActsConditions.aspx?directincidentid=" + lblDirectIncidentId.Text, true);
                //lblReferenceNumber.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUnsafe_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindUnsafeActs();
    }
    protected void gvHSEIndicators_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            TableCell tbHSEIndicators = new TableCell();
            TableCell tbLaggingIndicators = new TableCell();
            TableCell tbLeadingIndicators = new TableCell();

            tbHSEIndicators.ColumnSpan = 1;
            tbLaggingIndicators.ColumnSpan = 7;
            tbLeadingIndicators.ColumnSpan = 7;

            tbHSEIndicators.Text = "HSE Indicators";
            tbLaggingIndicators.Text = "Lagging Indicators";
            tbLeadingIndicators.Text = "Leading Indicators";

            tbHSEIndicators.Attributes.Add("style", "text-align:center;");
            tbLaggingIndicators.Attributes.Add("style", "text-align:center;");
            tbLeadingIndicators.Attributes.Add("style", "text-align:center;");

            gv.Cells.Add(tbHSEIndicators);
            gv.Cells.Add(tbLaggingIndicators);
            gv.Cells.Add(tbLeadingIndicators);

            gvHSEIndicators.Controls[0].Controls.AddAt(0, gv);
        }

        if (e.Item is GridHeaderItem)
        {

            RadLabel lblEH = (RadLabel)e.Item.FindControl("lblEHHeader");
            UserControlToolTip ucEH = (UserControlToolTip)e.Item.FindControl("ucEHHeader");
            lblEH.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucEH.ToolTip + "', 'visible');");
            lblEH.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucEH.ToolTip + "', 'hidden');");

            RadLabel lblhpi = (RadLabel)e.Item.FindControl("lblhpiHeader");
            UserControlToolTip ucthpi = (UserControlToolTip)e.Item.FindControl("uchpiHeader");
            lblhpi.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'visible');");
            lblhpi.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'hidden');");


            RadLabel lbllti = (RadLabel)e.Item.FindControl("lblltiHeader");
            UserControlToolTip uctlti = (UserControlToolTip)e.Item.FindControl("ucltiHeader");
            lbllti.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctlti.ToolTip + "', 'visible');");
            lbllti.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctlti.ToolTip + "', 'hidden');");


            RadLabel lblrwc = (RadLabel)e.Item.FindControl("lblrwcHeader");
            UserControlToolTip uctrwc = (UserControlToolTip)e.Item.FindControl("ucrwcHeader");
            lblrwc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'visible');");
            lblrwc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'hidden');");


            RadLabel lblmtc = (RadLabel)e.Item.FindControl("lblmtcHeader");
            UserControlToolTip uctmtc = (UserControlToolTip)e.Item.FindControl("ucmtcHeader");
            lblmtc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'visible');");
            lblmtc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'hidden');");

            RadLabel lblfac = (RadLabel)e.Item.FindControl("lblfacHeader");
            UserControlToolTip uctfac = (UserControlToolTip)e.Item.FindControl("ucfacHeader");
            lblfac.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctfac.ToolTip + "', 'visible');");
            lblfac.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctfac.ToolTip + "', 'hidden');");

            RadLabel lblEnvInc = (RadLabel)e.Item.FindControl("lblEnvironmentalIncidentHeader");
            UserControlToolTip uctEnvInc = (UserControlToolTip)e.Item.FindControl("ucEnvironmentalIncidentHeader");
            lblEnvInc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'visible');");
            lblEnvInc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'hidden');");


            RadLabel lblnmr = (RadLabel)e.Item.FindControl("lblnearmissHeader");
            UserControlToolTip uctnmr = (UserControlToolTip)e.Item.FindControl("ucnearmissHeader");
            lblnmr.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'visible');");
            lblnmr.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'hidden');");

            RadLabel lblstpcrd = (RadLabel)e.Item.FindControl("lblstopcardsHeader");
            UserControlToolTip uctstpcrd = (UserControlToolTip)e.Item.FindControl("ucstopcardsHeader");
            lblstpcrd.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'visible');");
            lblstpcrd.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'hidden');");

            RadLabel lbled = (RadLabel)e.Item.FindControl("lblExercisesandDrillsHeader");
            UserControlToolTip ucted = (UserControlToolTip)e.Item.FindControl("ucExercisesandDrillsHeader");
            lbled.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucted.ToolTip + "', 'visible');");
            lbled.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucted.ToolTip + "', 'hidden');");

            RadLabel lblra = (RadLabel)e.Item.FindControl("lblNoofRiskAssesmentHeader");
            UserControlToolTip uctra = (UserControlToolTip)e.Item.FindControl("ucNoofRiskAssesmentHeader");
            lblra.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctra.ToolTip + "', 'visible');");
            lblra.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctra.ToolTip + "', 'hidden');");

            RadLabel lblsfty = (RadLabel)e.Item.FindControl("lblnoofsafetyHeader");
            UserControlToolTip uctsfty = (UserControlToolTip)e.Item.FindControl("ucnoofsafetyHeader");
            lblsfty.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'visible');");
            lblsfty.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'hidden');");

            RadLabel lblptw = (RadLabel)e.Item.FindControl("lblPTWIssuedHeader");
            UserControlToolTip uctptw = (UserControlToolTip)e.Item.FindControl("uclblPTWIssuedHeader");
            lblptw.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctptw.ToolTip + "', 'visible');");
            lblptw.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctptw.ToolTip + "', 'hidden');");

            RadLabel lbluauc = (RadLabel)e.Item.FindControl("lblUnsafeActsHeader");
            UserControlToolTip uctuauc = (UserControlToolTip)e.Item.FindControl("ucUnsafeActsHeader");
            lbluauc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctuauc.ToolTip + "', 'visible');");
            lbluauc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctuauc.ToolTip + "', 'hidden');");

        }
        if (e.Item is GridDataItem)
        {
            {
                RadLabel lblEH = (RadLabel)e.Item.FindControl("lblEH");
                int Crew = (General.GetNullableInteger(txtCrew.Text) == null ? 0 : int.Parse(txtCrew.Text));
                RadLabel lblHSEIndicator = (RadLabel)e.Item.FindControl("lblhpiitem");
                //e.Row.ReadOnly = true;
                if (lblHSEIndicator.Text == "Last 24 hrs")
                {

                    double exposurehour = 0;
                    if ((rbnhourchange.SelectedValue) == "1" || (rbnhourchange.SelectedValue) == "2")
                    {
                        if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "1")
                        {
                            exposurehour = (Crew * 23.5);
                        }
                        if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "2")
                        {
                            exposurehour = (Crew * 23);
                        }
                        if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "1")
                        {
                            exposurehour = (Crew * 24.5);
                        }
                        if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "2")
                        {
                            exposurehour = (Crew * 25);
                        }
                    }
                    else
                    {
                        exposurehour = (Crew * 24);
                    }
                    lblEH.Text = exposurehour.ToString();
                    UserControlNumber ucstopcards = (UserControlNumber)e.Item.FindControl("ucstopcards");
                    UserControlNumber ucExercisesandDrills = (UserControlNumber)e.Item.FindControl("ucExercisesandDrills");
                    UserControlNumber ucnoofsafety = (UserControlNumber)e.Item.FindControl("ucnoofsafety");
                    UserControlNumber ucPTWIssued = (UserControlNumber)e.Item.FindControl("ucPTWIssued");

                    RadLabel lblstopcards = (RadLabel)e.Item.FindControl("lblstopcards");
                    RadLabel lblExercisesandDrills = (RadLabel)e.Item.FindControl("lblExercisesandDrills");
                    RadLabel lblnoofsafety = (RadLabel)e.Item.FindControl("lblnoofsafety");
                    RadLabel lblPTWIssued = (RadLabel)e.Item.FindControl("lblPTWIssued");

                    ucstopcards.Visible = true;
                    ucExercisesandDrills.Visible = true;
                    ucnoofsafety.Visible = true;
                    ucPTWIssued.Visible = true;

                    lblstopcards.Visible = false;
                    lblExercisesandDrills.Visible = false;
                    lblnoofsafety.Visible = false;
                    lblPTWIssued.Visible = false;

                    LinkButton lnkHpi = (LinkButton)e.Item.FindControl("lnkHpi");
                    LinkButton lnkEnvRelease = (LinkButton)e.Item.FindControl("lnkEnvRelease");
                    LinkButton lnkLit = (LinkButton)e.Item.FindControl("lnkLit");
                    LinkButton lnkrwc = (LinkButton)e.Item.FindControl("lnkrwc");
                    LinkButton lnkMtc = (LinkButton)e.Item.FindControl("lnkMtc");
                    LinkButton lnkFac = (LinkButton)e.Item.FindControl("lnkFac");
                    LinkButton lnkNearmiss = (LinkButton)e.Item.FindControl("lnkNearmiss");
                    LinkButton lnkUnsafeActs = (LinkButton)e.Item.FindControl("lnkUnsafeActs");
                    LinkButton lnkNoofRiskAssesment = (LinkButton)e.Item.FindControl("lnkNoofRiskAssesment");

                    RadLabel lblhpi = (RadLabel)e.Item.FindControl("lblhpi");
                    RadLabel lblEnvironmentalIncident = (RadLabel)e.Item.FindControl("lblEnvironmentalIncident");
                    RadLabel lbllit = (RadLabel)e.Item.FindControl("lbllit");
                    RadLabel lblrwc = (RadLabel)e.Item.FindControl("lblrwc");
                    RadLabel lblmtc = (RadLabel)e.Item.FindControl("lblmtc");
                    RadLabel lblfac = (RadLabel)e.Item.FindControl("lblfac");
                    RadLabel lblnearmiss = (RadLabel)e.Item.FindControl("lblnearmiss");
                    RadLabel lblUnsafeActs = (RadLabel)e.Item.FindControl("lblUnsafeActs");
                    RadLabel lblNoofRiskAssesment = (RadLabel)e.Item.FindControl("lblNoofRiskAssesment");

                    lnkHpi.Visible = true;
                    lnkEnvRelease.Visible = true;
                    lnkLit.Visible = true;
                    lnkrwc.Visible = true;
                    lnkMtc.Visible = true;
                    lnkFac.Visible = true;
                    lnkNearmiss.Visible = true;
                    lnkUnsafeActs.Visible = true;
                    lnkNoofRiskAssesment.Visible = true;

                    lblhpi.Visible = false;
                    lblEnvironmentalIncident.Visible = false;
                    lbllit.Visible = false;
                    lblrwc.Visible = false;
                    lblmtc.Visible = false;
                    lblfac.Visible = false;
                    lblnearmiss.Visible = false;
                    lblUnsafeActs.Visible = false;
                    lblNoofRiskAssesment.Visible = false;


                    lnkHpi.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=HPI&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkEnvRelease.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=ENV&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkLit.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=LTI&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkrwc.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=RWC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkMtc.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=MTC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkFac.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=FAC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkNearmiss.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=NM&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkUnsafeActs.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=UAUC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkNoofRiskAssesment.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=RA&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

                }

            }
        }
    }

    protected void gvHSEIndicators_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindHSEIndicators();
    }

    protected void gvLaggingIndicators_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "EDIT")
            {
                GridView _gridView = (GridView)sender;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }

                SaveMidnightReport();

                LinkButton lblReferenceNumber = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblReferenceNumber");
                RadLabel lblReferenceId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblReferenceId");
                Filter.CurrentIncidentID = lblReferenceId.Text;
                Filter.CurrentIncidentTab = "INCIDENTDETAILS";

                Response.Redirect("../Inspection/InspectionIncidentList.aspx", true);

                //lblReferenceNumber.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLaggingIndicators_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindLaggingIndicators();
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

    protected void gvShipTasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDueShipTasks();
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

    protected void gvCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDueCertificates();
    }

}
