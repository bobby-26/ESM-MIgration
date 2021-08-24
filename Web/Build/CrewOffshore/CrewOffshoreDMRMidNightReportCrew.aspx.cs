using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshoreDMRMidNightReportCrew : PhoenixBasePage
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
            MenuReportTap.SelectedMenuIndex = 5;

            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                BindInstallationType();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CPAGENUMBER"] = 1;
                ViewState["CSORTEXPRESSION"] = null;
                ViewState["CSORTDIRECTION"] = null;
                ViewState["OPAGENUMBER"] = 1;
                ViewState["OSORTEXPRESSION"] = null;
                ViewState["OSORTDIRECTION"] = null;
                ViewState["TPAGENUMBER"] = 1;
                ViewState["TSORTEXPRESSION"] = null;
                ViewState["TSORTDIRECTION"] = null;
                ViewState["MPAGENUMBER"] = 1;
                ViewState["MSORTEXPRESSION"] = null;
                ViewState["MSORTDIRECTION"] = null;
                ViewState["LPAGENUMBER"] = 1;
                ViewState["LSORTEXPRESSION"] = null;
                ViewState["LSORTDIRECTION"] = null;
                ViewState["VESSELMANDATORY"] = null;
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

                // ddlETALocation.Items.Insert(0, new ListItem("--Select--", ""));
                // ddlETDLocation.Items.Insert(0, new ListItem("--Select--", ""));


                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    EditMidNightReport();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }
            //ShowReport();
            BindPlannedActivity();
            // BindCrewData();
            SetFieldRange();
            //SetPageNavigator();
            ChangeVesselStatus();
            gvShipCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvCrewOtherDoc.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvCrewCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvCrewMedical.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvCrewTravel.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

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
    private void EditMidNightReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(new Guid(Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["REPORTDATE"] = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
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
            // txtETDTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETDDATE"]);
            ddlETDLocation.SelectedValue = ds.Tables[0].Rows[0]["FLDETDLOCATIONID"].ToString();
            txtETADate.Text = ds.Tables[0].Rows[0]["FLDETADATE"].ToString();
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
            txtMeterologyRemarks.Text = ds.Tables[0].Rows[0]["FLDMETEROLOGYREMARKS"].ToString();
            txtVesselMovementsRemarks.Text = ds.Tables[0].Rows[0]["FLDVESSELMOVEMENTSREMARKS"].ToString();
            txtbulkRemarks.Text = ds.Tables[0].Rows[0]["FLDBULKREMARKS"].ToString();
            rbnhourchange.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGE"].ToString();
            rbnhourvalue.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGEVALUE"].ToString();
            txtDPTime.Text = ds.Tables[0].Rows[0]["FLDTOTALDPTIME"].ToString();
            txtDPFuelConsumption.Text = ds.Tables[0].Rows[0]["FLDTOTALDPFUELCONSUMPTION"].ToString();
            txtFlowmeterRemarks.Text = ds.Tables[0].Rows[0]["FLDFLOWMETERREMARKS"].ToString();
            txtCrewRemarks.Text = ds.Tables[0].Rows[0]["FLDCREWREMARKS"].ToString();
            ucLifeBoatCapacity.Text = ds.Tables[0].Rows[0]["FLDLIFEBOATCAPACITY"].ToString();
            ucPassenger.Text = ds.Tables[0].Rows[0]["FLDPASSENGERCOUNT"].ToString();

            txtArrivalDate.Text = ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString();
            // txtArrivalTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDARRIVALDATE"]);

            txtDepartureDate.Text = ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString();
            // txtDepartureTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"]);
            txtEstimatedDuration.Text = ds.Tables[0].Rows[0]["FLDESTIMATEDDURATION"].ToString();
            ddlInstalationType.SelectedValue = ds.Tables[0].Rows[0]["FLDINSTALLATIONTYPEID"].ToString();

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

            ddlCrewListSOBYN.SelectedValue = ds.Tables[0].Rows[0]["FLDCREWLISTDIFFSOBYN"].ToString();
            txtCrewListSOBRemarks.Text = ds.Tables[0].Rows[0]["FLDCREWLISTDIFFSOBREMARKS"].ToString();

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
                //if (!IsValidData())
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                // string timeofetd = (txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETDTime.Text;
                //  string timeofeta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETATime.Text;

                //string timeofarrival = (txtArrivalTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtArrivalTime.Text;
                //string timeofdeparture = (txtDepartureTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtDepartureTime.Text;

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
                                 , null, null
                                 , General.GetNullableInteger(ddlInstalationType.SelectedValue)
                                 , General.GetNullableInteger(ddlCrewListSOBYN.SelectedValue), txtCrewListSOBRemarks.Text
                               );

                UpdateLookAheadActivity();

                ucStatus.Text = "MidNight Report Updated";


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
        ucError.HeaderMessage = "Please provide the following required information"; if (General.GetNullableString(ucLatitude.Text) == null)
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

    private void ChangeVesselStatus()
    {
        if (ddlvesselstatus.SelectedValue == "Dummy")
        {
            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETDDate.ReadOnly = true;
            //txtETDTime.ReadOnly = true;
            txtETDDate.CssClass = "readonlytextbox";
            // txtETDTime.CssClass = "readonlytextbox";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            // txtETATime.ReadOnly = true;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            // txtETATime.CssClass = "readonlytextbox";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            // txtArrivalTime.CssClass = "input";
            // txtArrivalTime.ReadOnly = false;
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            //txtDepartureTime.CssClass = "input";
            // txtDepartureTime.ReadOnly = false;

            txtDepartureDate.Enabled = true;
            // txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = true;
            // txtArrivalTime.Enabled = true;
            txtETADate.Enabled = true;
            // txtETATime.Enabled = true;
            txtETDDate.Enabled = true;
            //txtETDTime.Enabled = true;

        }
        if (ddlvesselstatus.SelectedValue == "1")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETDDate.ReadOnly = false;
            // txtETDTime.ReadOnly = false;
            txtETDDate.CssClass = "input";
            // txtETDTime.CssClass = "input";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            // txtETATime.ReadOnly = true;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            // txtETATime.CssClass = "readonlytextbox";

            ddlETALocation.Text = "";
            ddlETDLocation.Text = "";
            txtETADate.Text = "";
            // txtETATime.Text = "";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "input";
            txtArrivalDate.ReadOnly = false;
            //txtArrivalTime.CssClass = "input";
            // txtArrivalTime.ReadOnly = false;
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
            //  txtDepartureTime.CssClass = "readonlytextbox";
            // txtDepartureTime.ReadOnly = true;

            txtDepartureDate.Enabled = false;
            // txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = true;
            // txtArrivalTime.Enabled = true;
            txtETADate.Enabled = false;
            // txtETATime.Enabled = false;
            txtETDDate.Enabled = true;
            // txtETDTime.Enabled = true;
        }
        if (ddlvesselstatus.SelectedValue == "2")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETADate.ReadOnly = false;
            // txtETATime.ReadOnly = false;
            txtETADate.CssClass = "input";
            // txtETATime.CssClass = "input";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETDDate.ReadOnly = true;
            // txtETDTime.ReadOnly = true;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETDDate.CssClass = "readonlytextbox";
            // txtETDTime.CssClass = "readonlytextbox";

            ddlETALocation.Text = "";
            ddlETDLocation.Text = "";
            txtETDDate.Text = "";
            //txtETDTime.Text = "";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            // txtArrivalTime.CssClass = "readonlytextbox";
            // txtArrivalTime.ReadOnly = true;
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            // txtDepartureTime.CssClass = "input";
            // txtDepartureTime.ReadOnly = false;

            txtDepartureDate.Enabled = true;
            // txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = false;
            // txtArrivalTime.Enabled = false;
            txtETADate.Enabled = true;
            // txtETATime.Enabled = true;
            txtETDDate.Enabled = false;
            // txtETDTime.Enabled = false;

        }
        if (ddlvesselstatus.SelectedValue == "3")
        {
            ddlETALocation.Enabled = true;
            txtETADate.ReadOnly = false;
            // txtETATime.ReadOnly = false;
            ddlETALocation.CssClass = "input_mandatory";
            txtETADate.CssClass = "input_mandatory";
            // txtETATime.CssClass = "input_mandatory";
            txtLocation.CssClass = "input_mandatory";
            txtLocation.ReadOnly = false;

            ucPort.Enabled = false;
            txtETDDate.ReadOnly = true;
            // txtETDTime.ReadOnly = true;
            ddlETDLocation.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETDDate.CssClass = "readonlytextbox";
            //  txtETDTime.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";

            ucPort.Text = "";
            txtETDDate.Text = "";
            // txtETDTime.Text = "";
            ddlETDLocation.Text = "";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            // txtArrivalTime.CssClass = "readonlytextbox";
            // txtArrivalTime.ReadOnly = true;
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            // txtDepartureTime.CssClass = "input";
            // txtDepartureTime.ReadOnly = false;

            txtDepartureDate.Enabled = true;
            // txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = false;
            //txtArrivalTime.Enabled = false;
            txtETADate.Enabled = true;
            // txtETATime.Enabled = true;
            txtETDDate.Enabled = false;
            // txtETDTime.Enabled = false;

        }
        if (ddlvesselstatus.SelectedValue == "4")
        {
            ddlETDLocation.Enabled = true;
            txtETDDate.ReadOnly = false;
            // txtETDTime.ReadOnly = false;
            // txtETDTime.CssClass = "input";
            txtETDDate.CssClass = "input";
            ddlETDLocation.CssClass = "input_mandatory";
            txtLocation.CssClass = "input_mandatory";
            txtLocation.ReadOnly = false;

            ucPort.Enabled = false;
            ucPort.CssClass = "readonlytextbox";
            txtETADate.ReadOnly = true;
            // txtETATime.CssClass = "readonlytextbox";
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
            // txtArrivalTime.CssClass = "input";
            // txtArrivalTime.ReadOnly = false;
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
            // txtDepartureTime.CssClass = "readonlytextbox";
            // txtDepartureTime.ReadOnly = true;

            txtDepartureDate.Enabled = false;
            //  txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = true;
            // txtArrivalTime.Enabled = true;
            txtETADate.Enabled = false;
            //txtETATime.Enabled = false;
            txtETDDate.Enabled = true;
            //  txtETDTime.Enabled = true;
        }
        if (ddlvesselstatus.SelectedValue == "5")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETDDate.ReadOnly = true;
            //  txtETDTime.ReadOnly = true;
            txtETDDate.CssClass = "readonlytextbox";
            //  txtETDTime.CssClass = "readonlytextbox";
            txtLocation.CssClass = "readonlytextbox";
            txtLocation.ReadOnly = true;
            txtLocation.Text = "";

            ddlETALocation.Enabled = false;
            ddlETDLocation.Enabled = false;
            txtETADate.ReadOnly = true;
            // txtETATime.ReadOnly = true;
            ddlETALocation.CssClass = "readonlytextbox";
            ddlETDLocation.CssClass = "readonlytextbox";
            txtETADate.CssClass = "readonlytextbox";
            //txtETATime.CssClass = "readonlytextbox";

            ddlInstalationType.SelectedValue = "0";
            ddlInstalationType.CssClass = "readonlytextbox";
            ddlInstalationType.Enabled = false;

            txtArrivalDate.CssClass = "readonlytextbox";
            txtArrivalDate.ReadOnly = true;
            //txtArrivalTime.CssClass = "readonlytextbox";
            //txtArrivalTime.ReadOnly = true;
            txtDepartureDate.CssClass = "readonlytextbox";
            txtDepartureDate.ReadOnly = true;
            //txtDepartureTime.CssClass = "readonlytextbox";
            //txtDepartureTime.ReadOnly = true;

            txtDepartureDate.Enabled = false;
            //txtDepartureTime.Enabled = false;
            txtArrivalDate.Enabled = false;
            //txtArrivalTime.Enabled = false;
            txtETADate.Enabled = false;
            // txtETATime.Enabled = false;
            txtETDDate.Enabled = false;
            // txtETDTime.Enabled = false;
        }
    }
    protected void VesselStatus(object sender, EventArgs e)
    {
        txtETADate.Text = "";
        //txtETATime.Text = "";
        txtETDDate.Text = "";
        // txtETDTime.Text = "";
        txtArrivalDate.Text = "";
        // txtArrivalTime.Text = "";
        txtDepartureDate.Text = "";
        //txtDepartureTime.Text = "";

        ChangeVesselStatus();
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

    private void BindCrewData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSIGNONRANKNAME", "FLDNAME" };
        string[] alCaptions = { "Rank", "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataTable dt = PhoenixCrewOffshoreDMRMidNightReport.SearchMidnightReportVesselCrewList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                       , General.GetNullableDateTime(txtDate.Text)
                                                                       , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], gvShipCrew.PageSize
                                                                       , ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvShipCrew", "Crew List", alCaptions, alColumns, ds);
            gvShipCrew.DataSource = dt;
            gvShipCrew.VirtualItemCount = iRowCount;
            if (dt.Rows.Count > 0)
            {

                txtCrewOff.Text = ds.Tables[0].Rows[0]["FLDCREWOFF"].ToString();
                txtCrewOn.Text = ds.Tables[0].Rows[0]["FLDCREWON"].ToString();
                txtPOBCrew.Text = ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString();
                txtTotalOB.Text = (int.Parse(ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString())).ToString();
                txtPOB.Text = (int.Parse(ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString())).ToString();
                txtPOBClient.Text = ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString();
                txtPOBService.Text = ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString();
                txtPOBCrew.Text = ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString();
                txtCrew.Text = ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString();

                //txtMaster.Text = ds.Tables[0].Rows[0]["FLDMASTEROFSHIP"].ToString();
            }

            if (General.GetNullableInteger(txtPOBCrew.Text) > General.GetNullableInteger(ucLifeBoatCapacity.Text))
                ucLifeBoatCapacity.CssClass = "maxhighlight";

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetPageNavigatorLicense()
    //{
    //    //try
    //    //{
    //    //    cmdLicensePrevious.Enabled = IsPreviousEnabledLicense();
    //    //    cmdLicenseNext.Enabled = IsNextEnabledLicense();
    //    //    lblLicensePagenumber.Text = "Page " + ViewState["LPAGENUMBER"].ToString();
    //    //    lblLicensePages.Text = " of " + ViewState["LTOTALPAGECOUNT"].ToString() + " Pages. ";
    //    //    lblLicenseRecords.Text = "(" + ViewState["LROWCOUNT"].ToString() + " records found)";
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    ucError.ErrorMessage = ex.Message;
    //    //    ucError.Visible = true;
    //    //}
    //}
    //private Boolean IsPreviousEnabledLicense()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["LPAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["LTOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabledLicense()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["LPAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["LTOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        BindCrewData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvShipCrew.SelectedIndex = -1;
    //        gvShipCrew.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        BindCrewData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
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

    protected void gvShipCrew_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            Label lblNCCount = (Label)e.Row.FindControl("lblNCCount");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            if (lblNCCount != null && !string.IsNullOrEmpty(lblNCCount.Text) && int.Parse(lblNCCount.Text) > 0)
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
            }

            LinkButton lnkNCCount = (LinkButton)e.Row.FindControl("lnkNCCount");
           if (lnkNCCount != null && lblNCCount != null)
            {
                if (lblNCCount.Text == "0" || lblNCCount.Text == string.Empty)
                {
                    lblNCCount.Visible = true;
                    lnkNCCount.Visible = false;
                }
                else
                {
                    lblNCCount.Visible = false;
                    lnkNCCount.Visible = true;
                }
                //lnkNCCount.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../VesselAccounts/VesselAccountsRHWorkCalenderRemarks.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                lnkNCCount.Attributes.Add("onclick", "parent.Openpopup('MoreInfo', '', '../VesselAccounts/VesselAccountsRHWorkCalenderRemarks.aspx?CalenderId=" + drv["FLDRESTHOURCALENDARID"].ToString() + "&EMPID=" + drv["FLDEMPLOYEEID"].ToString() + "&RHStartId=" + drv["FLDRESTHOURSTARTID"].ToString() + "'); return false;");
            }
        }
    }

    // Passenger

    // Operational Time Summary
    private void ShowReport()
    {
        try
        {
            BindLicenceData();
            BindCourseData();
            BindOtherDocumentData();
            BindTravelDocumentData();
            BindMedicalData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindLicenceData()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["LSORTEXPRESSION"] == null) ? null : (ViewState["LSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["LSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["LSORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewReportExpiringLicense(
                           General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                           General.GetNullableDateTime(ViewState["REPORTDATE"].ToString()),
                           220,
                           60,
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["LPAGENUMBER"].ToString()),
                           gvCrew.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;
        // gvCrew.DataBind();

        ViewState["LROWCOUNT"] = iRowCount;
        ViewState["LTOTALPAGECOUNT"] = iTotalPageCount;
        //SetPageNavigatorLicense();
    }
    private void BindCourseData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["CSORTEXPRESSION"] == null) ? null : (ViewState["CSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["CSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["CSORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewReportExpiringCourse(
                           General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                           General.GetNullableDateTime(ViewState["REPORTDATE"].ToString()),
                           220,
                           60,
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["CPAGENUMBER"].ToString()),
                           gvCrewCourse.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount);

        gvCrewCourse.DataSource = ds;
        // gvCrewCourse.DataBind();
        gvCrewCourse.VirtualItemCount = iRowCount;

        ViewState["CROWCOUNT"] = iRowCount;
        ViewState["CTOTALPAGECOUNT"] = iTotalPageCount;
        //SetPageNavigatorCourse();
    }
    private void BindOtherDocumentData()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["OSORTEXPRESSION"] == null) ? null : (ViewState["OSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["OSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["OSORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewReportExpiringOtherDocument(
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                         General.GetNullableDateTime(ViewState["REPORTDATE"].ToString()),
                         220,
                         60,
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["OPAGENUMBER"].ToString()),
                        gvCrewOtherDoc.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);

        gvCrewOtherDoc.DataSource = ds;
        gvCrewOtherDoc.VirtualItemCount = iRowCount;
        //gvCrewOtherDoc.DataBind();


        ViewState["OROWCOUNT"] = iRowCount;
        ViewState["OTOTALPAGECOUNT"] = iTotalPageCount;
        //SetPageNavigatorOtherDoc();
    }
    private void BindTravelDocumentData()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["TSORTEXPRESSION"] == null) ? null : (ViewState["TSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["TSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["TSORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewReportExpiringTravel(
                           General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                           General.GetNullableDateTime(ViewState["REPORTDATE"].ToString()),
                           220,
                           60,
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["TPAGENUMBER"].ToString()),
                           gvCrewTravel.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount);

        gvCrewTravel.DataSource = ds;
        gvCrewTravel.VirtualItemCount = iRowCount;
        //gvCrewTravel.DataBind();

        ViewState["TROWCOUNT"] = iRowCount;
        ViewState["TTOTALPAGECOUNT"] = iTotalPageCount;
        //SetPageNavigatorTrvel();
    }
    private void BindMedicalData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string sortexpression = (ViewState["MSORTEXPRESSION"] == null) ? null : (ViewState["MSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["MSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["MSORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewReportExpiringMedical(
                           General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                           General.GetNullableDateTime(ViewState["REPORTDATE"].ToString()),
                           220,
                           60,
                           sortexpression, sortdirection,
                           Int32.Parse(ViewState["MPAGENUMBER"].ToString()),
                          gvCrewMedical.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount);
        gvCrewMedical.DataSource = ds;
        //gvCrewMedical.DataBind();
        gvCrewMedical.VirtualItemCount = iRowCount;


        ViewState["MROWCOUNT"] = iRowCount;
        ViewState["MTOTALPAGECOUNT"] = iTotalPageCount;
        // SetPageNavigatorMedical();
    }

    //protected void cmdLicenceGo_Click(object sender, EventArgs e)
    //{
    //    gvCrew.EditIndex = -1;
    //    gvCrew.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopageLicense.Text, out result))
    //    {
    //        ViewState["LPAGENUMBER"] = Int32.Parse(txtnopageLicense.Text);

    //        if ((int)ViewState["LTOTALPAGECOUNT"] < Int32.Parse(txtnopageLicense.Text))
    //            ViewState["LPAGENUMBER"] = ViewState["LTOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopageLicense.Text))
    //            ViewState["LPAGENUMBER"] = 1;

    //        if ((int)ViewState["LPAGENUMBER"] == 0)
    //            ViewState["LPAGENUMBER"] = 1;

    //        txtnopageLicense.Text = ViewState["LPAGENUMBER"].ToString();
    //    }
    //    BindLicenceData();
    //}
    //protected void LicensePagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCrew.SelectedIndex = -1;
    //    gvCrew.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["LPAGENUMBER"] = (int)ViewState["LPAGENUMBER"] - 1;
    //    else
    //        ViewState["LPAGENUMBER"] = (int)ViewState["LPAGENUMBER"] + 1;

    //    BindLicenceData();
    //}
    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}
    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}
    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    protected void gvCrew_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["LSORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["LSORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["LSORTDIRECTION"] == null || ViewState["LSORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //        && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {
        //        Label empid = (Label)e.Row.FindControl("lblEmpNo");
        //        LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
        //        Label lblempcode = (Label)e.Row.FindControl("lblEmpCode");
        //        if (lblempcode.Text != "")
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&licence=1&cid=2'); return false;");
        //        }
        //        else
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&licence=1'); return false;");
        //        }
        //    }
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }

            }
        }

    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindLicenceData();
    }
    protected void gvCrew_Sorting(object sender, GridViewSortEventArgs se)
    {
        // gvCrew.EditIndex = -1;
        // gvCrew.SelectedIndex = -1;
        ViewState["LSORTEXPRESSION"] = se.SortExpression;

        if (ViewState["LSORTDIRECTION"] != null && ViewState["LSORTDIRECTION"].ToString() == "0")
            ViewState["LSORTDIRECTION"] = 1;
        else
            ViewState["LSORTDIRECTION"] = 0;

        BindLicenceData();
    }
    protected void gvCrew_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    public bool IsValidFilter(string vessellist, string rblselectfrom, string days)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["VESSELMANDATORY"] != null && ViewState["VESSELMANDATORY"].ToString().Equals("1"))
        {
            if (vessellist.Equals("") || vessellist.Equals("Dummy"))
            {
                ucError.ErrorMessage = "Select Vessel";
            }
        }
        if (rblselectfrom.Equals("") || rblselectfrom.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Status";
        }
        if (days.Equals(""))
        {
            ucError.ErrorMessage = "Enter Documents Expiring in Next";
        }
        return (!ucError.IsError);
    }

    //protected void cmdGoCourse_Click(object sender, EventArgs e)
    //{
    //    gvCrewCourse.EditIndex = -1;
    //    gvCrewCourse.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopageCourse.Text, out result))
    //    {
    //        ViewState["CPAGENUMBER"] = Int32.Parse(txtnopageCourse.Text);

    //        if ((int)ViewState["CTOTALPAGECOUNT"] < Int32.Parse(txtnopageCourse.Text))
    //            ViewState["CPAGENUMBER"] = ViewState["CTOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopageCourse.Text))
    //            ViewState["CPAGENUMBER"] = 1;

    //        if ((int)ViewState["CPAGENUMBER"] == 0)
    //            ViewState["CPAGENUMBER"] = 1;

    //        txtnopageCourse.Text = ViewState["CPAGENUMBER"].ToString();
    //    }
    //    BindCourseData();
    //}
    //protected void CoursePagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCrewCourse.SelectedIndex = -1;
    //    gvCrewCourse.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["CPAGENUMBER"] = (int)ViewState["CPAGENUMBER"] - 1;
    //    else
    //        ViewState["CPAGENUMBER"] = (int)ViewState["CPAGENUMBER"] + 1;

    //    BindCourseData();
    //}
    //private void SetPageNavigatorCourse()
    //{
    //    cmdPreviousCourse.Enabled = IsPreviousEnabledCourse();
    //    cmdNextCourse.Enabled = IsNextEnabledCourse();
    //    lblPagenumberCourse.Text = "Page " + ViewState["CPAGENUMBER"].ToString();
    //    lblPagesCourse.Text = " of " + ViewState["CTOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecordsCourse.Text = "(" + ViewState["CROWCOUNT"].ToString() + " records found)";
    //}
    private Boolean IsPreviousEnabledCourse()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["CPAGENUMBER"];
        iTotalPageCount = (int)ViewState["CTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }
    private Boolean IsNextEnabledCourse()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["CPAGENUMBER"];
        iTotalPageCount = (int)ViewState["CTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void gvCrewCourse_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["CSORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["CSORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["CSORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //        && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {
        //        Label empid = (Label)e.Row.FindControl("lblEmpNo");
        //        LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
        //        Label lblempcode = (Label)e.Row.FindControl("lblEmpCode");
        //        if (lblempcode.Text != "")
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&course=1'); return false;");
        //        }
        //        else
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&course=1'); return false;");
        //        }
        //    }
        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
            }
        }

    }
    protected void cmdSortCourse_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindCourseData();
    }
    //protected void gvCrewCourse_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvCrewCourse.EditIndex = -1;
    //    gvCrewCourse.SelectedIndex = -1;
    //    ViewState["CSORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["CSORTDIRECTION"] != null && ViewState["CSORTDIRECTION"].ToString() == "0")
    //        ViewState["CSORTDIRECTION"] = 1;
    //    else
    //        ViewState["CSORTDIRECTION"] = 0;

    //    BindCourseData();
    //}
    protected void gvCrewCourse_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    //protected void cmdGoOtherDoc_Click(object sender, EventArgs e)
    //{
    //   // gvCrewOtherDoc.EditIndex = -1;
    //   // gvCrewOtherDoc.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopageOtherDoc.Text, out result))
    //    {
    //        ViewState["OPAGENUMBER"] = Int32.Parse(txtnopageOtherDoc.Text);

    //        if ((int)ViewState["OTOTALPAGECOUNT"] < Int32.Parse(txtnopageOtherDoc.Text))
    //            ViewState["OPAGENUMBER"] = ViewState["OTOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopageOtherDoc.Text))
    //            ViewState["OPAGENUMBER"] = 1;

    //        if ((int)ViewState["OPAGENUMBER"] == 0)
    //            ViewState["OPAGENUMBER"] = 1;

    //        txtnopageOtherDoc.Text = ViewState["OPAGENUMBER"].ToString();
    //    }
    //    BindOtherDocumentData();
    //}
    protected void OtherDocPagerButtonClick(object sender, CommandEventArgs ce)
    {
        // gvCrewOtherDoc.SelectedIndex = -1;
        // gvCrewOtherDoc.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["OPAGENUMBER"] = (int)ViewState["OPAGENUMBER"] - 1;
        else
            ViewState["OPAGENUMBER"] = (int)ViewState["OPAGENUMBER"] + 1;

        BindOtherDocumentData();
    }
    //private void SetPageNavigatorOtherDoc()
    //{
    //    cmdPreviousOtherDoc.Enabled = IsPreviousEnabledOtherDoc();
    //    cmdNextOtherDoc.Enabled = IsNextEnabledOtherDoc();
    //    lblPagenumberOtherDoc.Text = "Page " + ViewState["OPAGENUMBER"].ToString();
    //    lblPagesOtherDoc.Text = " of " + ViewState["OTOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecordsOtherDoc.Text = "(" + ViewState["OROWCOUNT"].ToString() + " records found)";
    //}
    private Boolean IsPreviousEnabledOtherDoc()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["OPAGENUMBER"];
        iTotalPageCount = (int)ViewState["OTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }
    private Boolean IsNextEnabledOtherDoc()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["OPAGENUMBER"];
        iTotalPageCount = (int)ViewState["OTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void gvCrewOtherDoc_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["OSORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["OSORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["OSORTDIRECTION"] == null || ViewState["OSORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //        && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {
        //        Label empid = (Label)e.Row.FindControl("lblEmpNo");
        //        LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
        //        Label lblempcode = (Label)e.Row.FindControl("lblEmpCode");
        //        if (lblempcode.Text != "")
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&Otherdocuments=1'); return false;");
        //        }
        //        else
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&Otherdocuments=1'); return false;");
        //        }
        //    }
        //}

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
            }
        }
    }
    protected void cmdSortOtherDoc_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindOtherDocumentData();
    }
    //protected void gvCrewOtherDoc_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    //gvCrewOtherDoc.EditIndex = -1;
    //    //gvCrewOtherDoc.SelectedIndex = -1;
    //    ViewState["OSORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["OSORTDIRECTION"] != null && ViewState["OSORTDIRECTION"].ToString() == "0")
    //        ViewState["OSORTDIRECTION"] = 1;
    //    else
    //        ViewState["OSORTDIRECTION"] = 0;

    //    BindOtherDocumentData();
    //}
    protected void gvCrewOtherDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    //protected void cmdGoTravel_Click(object sender, EventArgs e)
    //{
    //    gvCrewTravel.EditIndex = -1;
    //    gvCrewTravel.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopageTravel.Text, out result))
    //    {
    //        ViewState["TPAGENUMBER"] = Int32.Parse(txtnopageTravel.Text);

    //        if ((int)ViewState["TTOTALPAGECOUNT"] < Int32.Parse(txtnopageTravel.Text))
    //            ViewState["TPAGENUMBER"] = ViewState["TTOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopageTravel.Text))
    //            ViewState["TPAGENUMBER"] = 1;

    //        if ((int)ViewState["TPAGENUMBER"] == 0)
    //            ViewState["TPAGENUMBER"] = 1;

    //        txtnopageTravel.Text = ViewState["TPAGENUMBER"].ToString();
    //    }
    //    BindTravelDocumentData();
    //}
    //protected void TravelPagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCrewTravel.SelectedIndex = -1;
    //    gvCrewTravel.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["TPAGENUMBER"] = (int)ViewState["TPAGENUMBER"] - 1;
    //    else
    //        ViewState["TPAGENUMBER"] = (int)ViewState["TPAGENUMBER"] + 1;

    //    BindTravelDocumentData();
    //}
    //private void SetPageNavigatorTrvel()
    //{
    //    cmdPreviousTravel.Enabled = IsPreviousEnabledTravel();
    //    cmdNextTravel.Enabled = IsNextEnabledTravel();
    //    lblPagenumberTravel.Text = "Page " + ViewState["TPAGENUMBER"].ToString();
    //    lblPagesTravel.Text = " of " + ViewState["TTOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecordsTravel.Text = "(" + ViewState["TROWCOUNT"].ToString() + " records found)";
    //}
    private Boolean IsPreviousEnabledTravel()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["TPAGENUMBER"];
        iTotalPageCount = (int)ViewState["TTOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }
    private Boolean IsNextEnabledTravel()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["TPAGENUMBER"];
        iTotalPageCount = (int)ViewState["TTOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void gvCrewTravel_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["TSORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["TSORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["TSORTDIRECTION"] == null || ViewState["TSORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //        && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {
        //        Label empid = (Label)e.Row.FindControl("lblEmpNo");
        //        LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
        //        Label lblempcode = (Label)e.Row.FindControl("lblEmpCode");
        //        if (lblempcode.Text != "")
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&documents=1'); return false;");
        //        }
        //        else
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&documents=1'); return false;");
        //        }
        //    }
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
            }
        }
    }
    protected void cmdSortTravel_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindTravelDocumentData();
    }
    protected void gvCrewTravel_Sorting(object sender, GridViewSortEventArgs se)
    {
        // gvCrew.EditIndex = -1;
        //gvCrew.SelectedIndex = -1;
        ViewState["TSORTEXPRESSION"] = se.SortExpression;

        if (ViewState["TSORTDIRECTION"] != null && ViewState["TSORTDIRECTION"].ToString() == "0")
            ViewState["TSORTDIRECTION"] = 1;
        else
            ViewState["TSORTDIRECTION"] = 0;

        BindTravelDocumentData();
    }
    protected void gvCrewTravel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }



    //protected void cmdGoMedical_Click(object sender, EventArgs e)
    //{
    //    gvCrewMedical.EditIndex = -1;
    //    gvCrewMedical.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopageMedical.Text, out result))
    //    {
    //        ViewState["MPAGENUMBER"] = Int32.Parse(txtnopageMedical.Text);

    //        if ((int)ViewState["MTOTALPAGECOUNT"] < Int32.Parse(txtnopageMedical.Text))
    //            ViewState["MPAGENUMBER"] = ViewState["MTOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopageMedical.Text))
    //            ViewState["MPAGENUMBER"] = 1;

    //        if ((int)ViewState["MPAGENUMBER"] == 0)
    //            ViewState["MPAGENUMBER"] = 1;

    //        txtnopageMedical.Text = ViewState["MPAGENUMBER"].ToString();
    //    }
    //    BindMedicalData();
    //}
    //protected void MedicalPagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCrewMedical.SelectedIndex = -1;
    //    gvCrewMedical.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["MPAGENUMBER"] = (int)ViewState["MPAGENUMBER"] - 1;
    //    else
    //        ViewState["MPAGENUMBER"] = (int)ViewState["MPAGENUMBER"] + 1;

    //    BindMedicalData();
    //}
    //private void SetPageNavigatorMedical()
    //{
    //    cmdPreviousMedical.Enabled = IsPreviousEnabledMedical();
    //    cmdNextMedical.Enabled = IsNextEnabledMedical();
    //    lblPagenumberMedical.Text = "Page " + ViewState["MPAGENUMBER"].ToString();
    //    lblPagesMedical.Text = " of " + ViewState["MTOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecordsMedical.Text = "(" + ViewState["MROWCOUNT"].ToString() + " records found)";
    //}
    //private Boolean IsPreviousEnabledMedical()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["MPAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["MTOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}
    //private Boolean IsNextEnabledMedical()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["MPAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["MTOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    protected void gvCrewMedical_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["MSORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["MSORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["MSORTDIRECTION"] == null || ViewState["MSORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //        && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {
        //        Label empid = (Label)e.Row.FindControl("lblEmpNo");
        //        LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
        //        Label lblempcode = (Label)e.Row.FindControl("lblEmpCode");
        //        if (lblempcode.Text != "")
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "&med=1'); return false;");
        //        }
        //        else
        //        {
        //            lbr.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "&med=1'); return false;");
        //        }
        //    }
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label expdate = (Label)e.Row.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Row.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
            }
        }
    }
    protected void cmdSortMedical_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindMedicalData();
    }
    //protected void gvCrewMedical_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    gvCrewMedical.EditIndex = -1;
    //    gvCrewMedical.SelectedIndex = -1;
    //    ViewState["MSORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["MSORTDIRECTION"] != null && ViewState["MSORTDIRECTION"].ToString() == "0")
    //        ViewState["MSORTDIRECTION"] = 1;
    //    else
    //        ViewState["MSORTDIRECTION"] = 0;

    //    BindMedicalData();
    //}
    protected void gvCrewMedical_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

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
                    case "FLDPOBCLIENT":
                        {
                            if (General.GetNullableDecimal(txtPOBClient.Text) < minvalue || General.GetNullableDecimal(txtPOBClient.Text) > maxvalue)
                                txtPOBClient.CssClass = "maxhighlight";
                            else
                                txtPOBClient.CssClass = "readonlytextbox";
                            break;
                        }
                        //case "FLDTOTALNOOFPERSONS":
                        //    {
                        //        if (General.GetNullableDecimal(txtTotalOB.Text) < minvalue || General.GetNullableDecimal(txtTotalOB.Text) > maxvalue)
                        //            txtTotalOB.CssClass = "maxhighlight";
                        //        else
                        //            txtTotalOB.CssClass = "readonlytextbox";
                        //        break;
                        //    }                  
                }
            }
        }
    }
    protected void ddlvesselstatus_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
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


    protected void gvShipCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipCrew.CurrentPageIndex + 1;
            BindCrewData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }

            }
        }
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["LPAGENUMBER"] = ViewState["LPAGENUMBER"] != null ? ViewState["LPAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            BindLicenceData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }

            }
        }
    }

    protected void gvCrewCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["CPAGENUMBER"] = ViewState["CPAGENUMBER"] != null ? ViewState["CPAGENUMBER"] : gvCrewCourse.CurrentPageIndex + 1;
            BindCourseData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewOtherDoc_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
            }
        }
    }

    protected void gvCrewOtherDoc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["OPAGENUMBER"] = ViewState["OPAGENUMBER"] != null ? ViewState["OPAGENUMBER"] : gvCrewOtherDoc.CurrentPageIndex + 1;
            BindOtherDocumentData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewTravel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
            }
        }
    }

    protected void gvCrewTravel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["TPAGENUMBER"] = ViewState["TPAGENUMBER"] != null ? ViewState["TPAGENUMBER"] : gvCrewTravel.CurrentPageIndex + 1;
            BindTravelDocumentData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void gvCrewMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel expdate = (RadLabel)e.Item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
                if (t.Days >= 0 && t.Days < 15)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                if (t.Days >= 15 && t.Days < 30)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                if (t.Days >= 30 && t.Days < 60)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
            }
        }
    }

    protected void gvCrewMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["MPAGENUMBER"] = ViewState["MPAGENUMBER"] != null ? ViewState["MPAGENUMBER"] : gvCrewMedical.CurrentPageIndex + 1;
            BindMedicalData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

   
    protected void gvShipCrew_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblonboardoverdue = (RadLabel)e.Item.FindControl("lblonboardoverdue");

            if (lblonboardoverdue!=null && lblonboardoverdue.Text == "1")
            {
                e.Item.Cells[4].BackColor = System.Drawing.Color.Red;
                //lbllastRHRecorddate.BackColor = System.Drawing.Color.Red;
            }
        }
    }
}
