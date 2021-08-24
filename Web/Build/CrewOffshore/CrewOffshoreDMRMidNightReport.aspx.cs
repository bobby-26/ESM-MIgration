using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Reports;
using System.IO;
using Telerik.Web.UI;

public partial class CrewOffshoreDMRMidNightReport : PhoenixBasePage
{
    public string nextOperational = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //new
            int imasterAccess = 0;
            DataSet dsa = PhoenixCrewOffshoreDMRMidNightReport.DmrVesselMasterAccess(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , ref imasterAccess);
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            toolbarReporttap.AddButton("Copy", "COPY", ToolBarDirection.Right);
            toolbarReporttap.AddButton("Show Report", "REPORT", ToolBarDirection.Right);
            if (imasterAccess == 1)
                toolbarReporttap.AddButton("Send To Office", "SENDTOOFFICE", ToolBarDirection.Right);

            toolbarReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);



            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

            PhoenixToolbar toolbarHSE = new PhoenixToolbar();
            toolbarHSE.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuTabSaveHSEIndicators.AccessRights = this.ViewState;
            MenuTabSaveHSEIndicators.MenuList = toolbarHSE.Show();

            PhoenixToolbar toolbarVM = new PhoenixToolbar();
            toolbarVM.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTabSaveVesselMovements.AccessRights = this.ViewState;
            MenuTabSaveVesselMovements.MenuList = toolbarVM.Show();

            PhoenixToolbar toolbarMeteorologyData = new PhoenixToolbar();
            toolbarMeteorologyData.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTabSaveMeteorologyData.AccessRights = this.ViewState;
            MenuTabSaveMeteorologyData.MenuList = toolbarMeteorologyData.Show();

            PhoenixToolbar toolbarFO = new PhoenixToolbar();
            toolbarFO.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTabSaveFO.AccessRights = this.ViewState;
            MenuTabSaveFO.MenuList = toolbarFO.Show();

            PhoenixToolbar toolbarBulks = new PhoenixToolbar();
            toolbarBulks.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTabSaveBulks.AccessRights = this.ViewState;
            MenuTabSaveBulks.MenuList = toolbarBulks.Show();

            PhoenixToolbar toolbarPassenger = new PhoenixToolbar();
            toolbarPassenger.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTabSavePassenger.AccessRights = this.ViewState;
            MenuTabSavePassenger.MenuList = toolbarPassenger.Show();

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
            MenuReportTap.SelectedMenuIndex = 1;

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRVesselMovements.aspx?reportid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWACADEMIC");
            MenuVesselMovements.AccessRights = this.ViewState;
            MenuVesselMovements.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarMach = new PhoenixToolbar();
            toolbarMach.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRequisition.aspx?module=DMR&date=" + txtDate.Text + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuMachineryFailure.AccessRights = this.ViewState;
            MenuMachineryFailure.MenuList = toolbarMach.Show();

            PhoenixToolbar toolbarhse = new PhoenixToolbar();
            toolbarhse.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReport.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuHSEIndicators.AccessRights = this.ViewState;
            MenuHSEIndicators.MenuList = toolbarhse.Show();

            PhoenixToolbar toolbarUnsafe = new PhoenixToolbar();
            toolbarUnsafe.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDMRMidNightReport.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuUnsafeActsAdd.AccessRights = this.ViewState;
            MenuUnsafeActsAdd.MenuList = toolbarUnsafe.Show();

            PhoenixToolbar toolbarShipCrew = new PhoenixToolbar();
            toolbarShipCrew.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTabShipCrew.AccessRights = this.ViewState;
            MenuTabShipCrew.MenuList = toolbarShipCrew.Show();

            if (!IsPostBack)
            {
                BindInstallationType();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["MonthlyReportId"] = "";

                ViewState["COMPANYID"] = "";
                ViewState["REACTIVATED"] = null;
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                txtVoyageId.Attributes.Add("style", "display:none");

                ddlETALocation.DataSource = PhoenixRegistersDMRLocation.DMRLocationList();
                ddlETALocation.DataBind();

                ddlETDLocation.DataSource = PhoenixRegistersDMRLocation.DMRLocationList();
                ddlETDLocation.DataBind();

                //ddlETALocation.Items.Insert(0, new ListItem("--Select--", ""));
                //ddlETDLocation.Items.Insert(0, new ListItem("--Select--", ""));

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;

                }
                else
                {
                    ViewState["VESSELID"] = "";
                    //ucVessel.Enabled = false;
                    cmdDateBaseSearch.Visible = true;
                }

                DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreInitailMidNightReportYN(int.Parse(Session["VESSELID"].ToString()), General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
                ViewState["INITIALREPORTYN"] = ds.Tables[0].Rows[0]["FLDINITIALREPORTYN"].ToString();
                ViewState["PREVIOUSEREPORTID"] = ds.Tables[0].Rows[0]["FLDPREVIOUSEREPORTID"].ToString();

                if (General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString()) == null)
                {
                    txtDate.Text = General.GetDateTimeToString(System.DateTime.Now);

                }
                else
                {
                    txtDate.Text = ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString();
                }
                DataSet dt = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRCurrentCharterer(int.Parse(Session["VESSELID"].ToString()), General.GetNullableDateTime(txtDate.Text) == null ? System.DateTime.Now : General.GetNullableDateTime(txtDate.Text));
                if (dt.Tables[0].Rows.Count > 0)
                {
                    txtVoyageId.Text = dt.Tables[0].Rows[0]["FLDVOYAGEID"].ToString();
                    txtVoyageName.Text = dt.Tables[0].Rows[0]["FLDVOYAGENO"].ToString();
                }

                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    EditMidNightReport();

                }
                EditMidNightReportRunTime(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));

                EditFlowMeterReading();
                BindOperationalTimeSummary();

                lnkFuelConsShowGraph.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crewoffshore/CrewOffshoreDMRMonthlyDPOperations.aspx?ReportID=" + General.GetNullableString(ViewState["MonthlyReportId"].ToString()) + "&Source=midnightreport'); return false;");
                BindRequisionsandPO();
                BindDueAudits();
                BindDueShipTasks();
                BindDueCertificates();
                BindUnsafeActs();
                BindMachineryFailures();
                BindExternalInspection();
                BindOilLoadandConsumption();
                BindMeteorologyData();
                BindPlannedActivity();
                BindVesselMovements();
                BindCrewData();
                BindHSEIndicators();
                BindOperationalTimeSummary();
                BindWorkOrder();
                BindLaggingIndicators();
                BindPMSoverdue();
                BindDeckCargo();
                BindDeckCargoSummary();
                BindROVoperation();
                BindAnchor();
                //BindPassenger();
                // SetPageNavigator();
                ChangeVesselStatus();
                ChangeFlowmeterTotalEnabedYN();
                CheckPrevioueDefectiveFM();
                ChangeNoFlowmeterEnabedYN(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    SetFieldRange();
                }
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

        if (CommandName.ToUpper().Equals("MIDNIGHTREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMidNightReportList.aspx?VesselId=" + (Request.QueryString["VesselId"] == null ? "" : Request.QueryString["VesselId"].ToString())
                + "&PageNumber=" + (Request.QueryString["PageNumber"] == null ? "" : Request.QueryString["PageNumber"].ToString()), false);
        }
        else
        {
            if (Session["MIDNIGHTREPORTID"] != null)
            {
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
            else
            {
                ucError.ErrorMessage = "Report is not yet Created. Please save the Report first.";
                ucError.Visible = true;
            }

        }

    }

    protected void MenuNRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Session["MIDNIGHTREPORTID"] != null)
        {

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
            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                int confirm;
                if (CommandName.ToUpper() == "SENDTOOFFICE")
                {
                    //EditMidNightReportRemarks();
                    confirm = 1;
                    if (!IsValidData())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    if (!IsValidRemarksData())
                    {
                        return;
                    }
                }
                else
                {
                    confirm = 0;
                    if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                    {
                        ucError.HeaderMessage = "Please provide the following required information";
                        ucError.ErrorMessage = "Select the Vessel Status";
                        ucError.Visible = true;
                        return;
                    }

                }
                SaveMidnightReport(confirm);
                //if (dce.CommandName.ToUpper() == "SENDTOOFFICE")
                //{
                //    GeneratePDF();   
                //}
            }
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                if (Session["MIDNIGHTREPORTID"] != null)
                {
                    string date = txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text;
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=11&reportcode=DMRMIDNIGHTREPORT&midNightReportId=" + Session["MIDNIGHTREPORTID"].ToString()
                                                    + "&VesselID=" + ViewState["VESSELID"].ToString() + "&date=" + date, false);
                }
                else
                {
                    ucError.ErrorMessage = "Midnight Report is not yet Created.";
                    ucError.Visible = true;
                }
            }
            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (Session["MIDNIGHTREPORTID"] == null && ViewState["PREVIOUSEREPORTID"] != null)
                {
                    CopyMidNightReport(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));

                }
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
        DateTime resultdate;
        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableDateTime(txtDate.Text) == null)
            ucError.ErrorMessage = "Report Date is required.";

        if (!string.IsNullOrEmpty(txtDate.Text) && DateTime.TryParse(txtDate.Text, out resultdate))
        {
            if (DateTime.Compare(resultdate, DateTime.Now) > 0)
                ucError.ErrorMessage = "Report Date Should not be later than current date";
        }

        if (General.GetNullableGuid(txtVoyageId.Text) == null)
            ucError.ErrorMessage = "Charter is required.";
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

            if (ddlvesselstatus.SelectedValue == "5" && General.GetNullableInteger(ucPort.SelectedValue) == null)
            {
                ucError.ErrorMessage = "Port is mandatory";
            }
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
            if (General.GetNullableDateTime(txtArrivalDate.Text) > General.GetNullableDateTime(txtDate.Text))
            {
                ucError.ErrorMessage = "Arrival date cannot be later than report date.";
            }
        }
        if (General.GetNullableDateTime(txtDepartureDate.Text) != null && General.GetNullableDateTime(txtDate.Text) != null)
        {
            if (General.GetNullableDateTime(txtDepartureDate.Text) > General.GetNullableDateTime(txtDate.Text))
            {
                ucError.ErrorMessage = "Departure date cannot be later than report date.";
            }
        }
        if (chkDNATest.Checked == true && txtDNATime.Text.Trim() == "__:__")
        {
            ucError.ErrorMessage = "Time of DNA Test is Required";
        }
        //    ucError.ErrorMessage = "Sat C remarks is required.";
        //if (chkCCTV.Checked == false && txtCCTVRemarks.Text == "")
        //    ucError.ErrorMessage = "CCTV remarks is required.";
        //if (chkHiPap.Checked == false && txtHiPapRemarks.Text == "")
        //    ucError.ErrorMessage = "VDR remarks is required.";
        if (rblHSEIndicators.SelectedValue == "")
            ucError.ErrorMessage = "Incident / Near Miss on board is required.";
        if (rblMachineryFailure.SelectedValue == "")
            ucError.ErrorMessage = "Machinery / Equipment failure on board is required.";

        foreach (GridDataItem gvr in gvHSEIndicators.Items)
        {
            RadLabel hseindicator = (RadLabel)gvr.FindControl("lblhpiitem");
            if (gvHSEIndicators.Items.Count > 0 && hseindicator.Text == "Last 24 hrs")

            {
                UserControlMaskNumber ucstopcards = (UserControlMaskNumber)gvr.FindControl("ucstopcards");
                UserControlMaskNumber ucExercisesandDrills = (UserControlMaskNumber)gvr.FindControl("ucExercisesandDrills");
                UserControlMaskNumber ucnoofsafety = (UserControlMaskNumber)gvr.FindControl("ucnoofsafety");
                UserControlMaskNumber ucPTWIssued = (UserControlMaskNumber)gvr.FindControl("ucPTWIssued");

                if (General.GetNullableInteger(ucstopcards.Text) == null)
                    ucError.ErrorMessage = "STOP Cards in HSE is required.";
                if (General.GetNullableInteger(ucExercisesandDrills.Text) == null)
                    ucError.ErrorMessage = "ED in HSE is required.";
                if (General.GetNullableInteger(ucnoofsafety.Text) == null)
                    ucError.ErrorMessage = "HSE Meeting Held in HSE is required.";
                if (General.GetNullableInteger(ucPTWIssued.Text) == null)
                    ucError.ErrorMessage = "PTW Issued in HSE is required.";
            }
        }
        foreach (GridDataItem gvr in gvOperationalTimeSummary.Items)
        {

            RadLabel lblDistanceApplicable = (RadLabel)gvr.FindControl("lblDistanceApplicable");
            UserControlDecimal distance = (UserControlDecimal)gvr.FindControl("ucSeaStreamDistanceEdit");

            if (lblDistanceApplicable.Text == "1" && General.GetNullableDecimal(distance.Text) == null)
                ucError.ErrorMessage = "Operational Summary Distance is Required.";


        }
        if (ddlCrewListSOBYN.SelectedValue == "0" && txtCrewListSOBRemarks.Text == string.Empty)
            ucError.ErrorMessage = "Ship's crew remarks is mandatory.";

        return (!ucError.IsError);
    }
    private bool IsValidRemarksData()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if ((rblSatC.SelectedValue == "0" && txtSatCRemarks.Text == "")
                || (rblCCTV.SelectedValue == "0" && txtCCTVRemarks.Text == "")
                || (rblHiPap.SelectedValue == "0" && txtHiPapRemarks.Text == ""))
        {

            ucError.ErrorMessage = "";
            string satC = "", cctv = "", HiPap = "";
            if (rblSatC.SelectedValue == "0" && txtSatCRemarks.Text == "")
                satC = "no";
            if (rblCCTV.SelectedValue == "0" && txtCCTVRemarks.Text == "")
                cctv = "no";
            if (rblHiPap.SelectedValue == "0" && txtHiPapRemarks.Text == "")
                HiPap = "no";


            String scriptpopup = String.Format(
               "javascript:parent.Openpopup('codehelp1', '', '../CrewOffshore/CrewOffshoreDMRMidNightReportRemarks.aspx?MidNightReportID=" + Session["MIDNIGHTREPORTID"].ToString() +
                            "&SatC=" + satC + "&CCTV=" + cctv + "&HiPap=" + HiPap + "');");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            //EditMidNightReportRemarks();

        }
        return (!ucError.IsError);
    }
    private void EditFlowMeterReading()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportFOFlowmeterReadings(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtme1initialhrs.Text = ds.Tables[0].Rows[0]["FLDME1FIRSTHRS"].ToString();
            txtme1lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME1LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME1FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME1LASTHRS"].ToString());
            lblme1returninitialhrs.Text = ds.Tables[0].Rows[0]["FLDME1RETURNFIRSTHRS"].ToString();
            lblme1returnlasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME1RETURNLASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME1RETURNFIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME1RETURNLASTHRS"].ToString());
            txtme1Total.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME1TOTAL"].ToString()) == null ? "0" : ds.Tables[0].Rows[0]["FLDME1TOTAL"].ToString());
            txtme2initialhrs.Text = ds.Tables[0].Rows[0]["FLDME2FIRSTHRS"].ToString();
            txtme2lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME2LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME2FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME2LASTHRS"].ToString());
            lblme2returninitialhrs.Text = ds.Tables[0].Rows[0]["FLDME2RETURNFIRSTHRS"].ToString();
            lblme2returnlasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME2RETURNLASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDME2RETURNFIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDME2RETURNLASTHRS"].ToString());
            txtme2Total.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDME2TOTAL"].ToString()) == null ? "0" : ds.Tables[0].Rows[0]["FLDME2TOTAL"].ToString());
            txtgrandtotal.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDMEGRANDTOTAL"].ToString()) == null ? "0" : ds.Tables[0].Rows[0]["FLDMEGRANDTOTAL"].ToString());
            txtAE1Consumption.Text = ds.Tables[0].Rows[0]["FLDAE1CONS"].ToString();
            txtAE2Consumption.Text = ds.Tables[0].Rows[0]["FLDAE2CONS"].ToString();

            txtAE1initialhrs.Text = ds.Tables[0].Rows[0]["FLDAE1FIRSTHRS"].ToString();
            txtAE1lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDAE1LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDAE1FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDAE1LASTHRS"].ToString());
            txtAE2initialhrs.Text = ds.Tables[0].Rows[0]["FLDAE2FIRSTHRS"].ToString();
            txtAE2lasthrs.Text = (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDAE2LASTHRS"].ToString()) == null ? ds.Tables[0].Rows[0]["FLDAE2FIRSTHRS"].ToString() : ds.Tables[0].Rows[0]["FLDAE2LASTHRS"].ToString());
            if (Session["MIDNIGHTREPORTID"] != null)
                ucotherConsumption.Text = ds.Tables[0].Rows[0]["FLDOTHERCONS"].ToString();
            ucTotalConsumption.Text = ds.Tables[0].Rows[0]["FLDTOTALCONS"].ToString();

            if (Session["MIDNIGHTREPORTID"] == null)
            {

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
                    txtme1Total.Text = ds.Tables[0].Rows[0]["FLDME1TOTAL"].ToString();
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
                    txtme2Total.Text = ds.Tables[0].Rows[0]["FLDME2TOTAL"].ToString();
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
                    txtAE1Consumption.Text = ds.Tables[0].Rows[0]["FLDAE1CONS"].ToString();
                }
                else
                {
                    chkAE1NoFM.Checked = false;
                }

                if (ds.Tables[0].Rows[0]["FLDAE2NOFM"].ToString() == "1")
                {
                    chkAE2NoFM.Checked = true;
                    txtAE2Consumption.Text = ds.Tables[0].Rows[0]["FLDAE2CONS"].ToString();
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
                    txtme1lasthrs.ReadOnly = true;
                }
                if (chkMEPortNoFM.Checked == false)
                {
                    chkMEPortFlowDetective.Enabled = true;
                    if (chkMEPortFlowDetective.Checked == true)
                    {
                        txtme1lasthrs.CssClass = "readonlytextbox";
                        txtme1lasthrs.ReadOnly = true;
                        txtme1Total.Text = "";
                    }
                    if (chkMEPortFlowDetective.Checked == false)
                    {
                        txtme1lasthrs.CssClass = "input";
                        txtme1lasthrs.ReadOnly =false;
                    }

                }

                if (chkMEStbdNoFM.Checked == true)
                {
                    chkMEStbdFlowDetective.Enabled = false;
                    chkMEStbdFlowDetective.Checked = false;

                    txtme2lasthrs.CssClass = "readonlytextbox";
                    txtme2lasthrs.ReadOnly = true;
                }
                if (chkMEStbdNoFM.Checked == false)
                {
                    chkMEStbdFlowDetective.Enabled = true;
                    if (chkMEStbdFlowDetective.Checked == true)
                    {
                        txtme2lasthrs.CssClass = "readonlytextbox";
                        txtme2lasthrs.ReadOnly = true;
                        txtme2Total.Text = "";
                    }
                    if (chkMEStbdFlowDetective.Checked == false)
                    {
                        txtme2lasthrs.CssClass = "input";
                        txtme2lasthrs.ReadOnly = false;
                    }
                }

                if (chkMEPortreturnNoFM.Checked == true)
                {
                    chkMEPortreturnFlowDetective.Enabled = false;
                    chkMEPortreturnFlowDetective.Checked = false;

                    lblme1returnlasthrs.CssClass = "readonlytextbox";
                    lblme1returnlasthrs.ReadOnly = true;
                }
                if (chkMEPortreturnNoFM.Checked == false)
                {
                    chkMEPortreturnFlowDetective.Enabled = true;
                    if (chkMEPortreturnFlowDetective.Checked == true)
                    {
                        lblme1returnlasthrs.CssClass = "readonlytextbox";
                        lblme1returnlasthrs.ReadOnly = true;
                    }
                    if (chkMEPortreturnFlowDetective.Checked == false)
                    {
                        lblme1returnlasthrs.CssClass = "input";
                        lblme1returnlasthrs.ReadOnly = false;
                    }
                }

                if (chkMEStbdreturnNoFM.Checked == true)
                {
                    chkMEStbdreturnFlowDetective.Enabled = false;
                    chkMEStbdreturnFlowDetective.Checked = false;

                    lblme2returnlasthrs.CssClass = "readonlytextbox";
                    lblme2returnlasthrs.ReadOnly = true;
                }
                if (chkMEStbdreturnNoFM.Checked == false)
                {
                    chkMEStbdreturnFlowDetective.Enabled = true;
                    if (chkMEStbdreturnFlowDetective.Checked == true)
                    {
                        lblme2returnlasthrs.CssClass = "readonlytextbox";
                        lblme2returnlasthrs.ReadOnly = true;
                    }
                    if (chkMEStbdreturnFlowDetective.Checked == false)
                    {
                        lblme2returnlasthrs.CssClass = "input";
                        lblme2returnlasthrs.ReadOnly = false;
                    }
                }

                if (chkAE1NoFM.Checked == true)
                {
                    chkAE1FlowDetective.Enabled = false;
                    chkAE1FlowDetective.Checked = false;

                    txtAE1lasthrs.CssClass = "readonlytextbox";
                    txtAE1lasthrs.ReadOnly = true;
                }
                if (chkAE1NoFM.Checked == false)
                {
                    chkAE1FlowDetective.Enabled = true;
                    if (chkAE1FlowDetective.Checked == true)
                    {
                        txtAE1lasthrs.CssClass = "readonlytextbox";
                        txtAE1lasthrs.ReadOnly = true;
                        txtAE1Consumption.Text = "";
                    }
                    if (chkAE1FlowDetective.Checked == false)
                    {
                        txtAE1lasthrs.CssClass = "input";
                        txtAE1lasthrs.ReadOnly = false;
                    }
                }
                if (chkAE2NoFM.Checked == true)
                {
                    chkAE2FlowDetective.Enabled = false;
                    chkAE2FlowDetective.Checked = false;

                    txtAE2lasthrs.CssClass = "readonlytextbox";
                    txtAE2lasthrs.ReadOnly = true;
                }
                if (chkAE2NoFM.Checked == false)
                {
                    chkAE2FlowDetective.Enabled = true;
                    if (chkAE2FlowDetective.Checked == true)
                    {
                        txtAE2lasthrs.CssClass = "readonlytextbox";
                        txtAE2lasthrs.ReadOnly = true;
                        txtAE2Consumption.Text = "";
                    }
                    if (chkAE2FlowDetective.Checked == false)
                    {
                        txtAE2lasthrs.CssClass = "input";
                        txtAE2lasthrs.ReadOnly = false;
                    }
                    txtAE2lasthrs.CssClass = "input";
                    txtAE2lasthrs.ReadOnly = false;
                }


            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindVesselMovements();
            BindOperationalTimeSummary();
            EditMidNightReportRemarks();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
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
            // txtETDTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETDDATE"]);
            ddlETDLocation.SelectedValue = ds.Tables[0].Rows[0]["FLDETDLOCATIONID"].ToString();
            txtETADate.Text = ds.Tables[0].Rows[0]["FLDETADATE"].ToString();
            txtETATime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDETADATE"].ToString());
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
            txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();



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

            txtGeneralRemarks.Text = ds.Tables[0].Rows[0]["FLDGENERALREMARKS"].ToString();
            txtFlowmeterRemarks.Text = ds.Tables[0].Rows[0]["FLDFLOWMETERREMARKS"].ToString();
            txtCrewRemarks.Text = ds.Tables[0].Rows[0]["FLDCREWREMARKS"].ToString();
            txtMeterologyRemarks.Text = ds.Tables[0].Rows[0]["FLDMETEROLOGYREMARKS"].ToString();
            txtVesselMovementsRemarks.Text = ds.Tables[0].Rows[0]["FLDVESSELMOVEMENTSREMARKS"].ToString();
            txtLookAheadRemarks.Text = ds.Tables[0].Rows[0]["FLDLOOKAHEADREMARKS"].ToString();
            txtbulkRemarks.Text = ds.Tables[0].Rows[0]["FLDBULKREMARKS"].ToString();
            txtPOBClient.Text = ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString();
            txtPOBService.Text = ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString();

            ucPort.SelectedValue = ds.Tables[0].Rows[0]["FLDPORTID"].ToString();
            ucPort.Text = ds.Tables[0].Rows[0]["PORTNAME"].ToString();

            txtArrivalDate.Text = ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString();
            txtArrivalTime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString());
            //txtArrivalTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDARRIVALDATE"]);

            txtDepartureDate.Text = ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString();
            txtDepartureTime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString());

            txtEstimatedDuration.Text = ds.Tables[0].Rows[0]["FLDESTIMATEDDURATION"].ToString();
            ddlInstalationType.SelectedValue = ds.Tables[0].Rows[0]["FLDINSTALLATIONTYPEID"].ToString();
            txtDeckRemarks.Text = ds.Tables[0].Rows[0]["FLDDECKCARGOREMARKS"].ToString();
            txtAnchorRemarks.Text = ds.Tables[0].Rows[0]["FLDANCHORREMARKS"].ToString();
            txtROVRemarks.Text = ds.Tables[0].Rows[0]["FLDROVREMARKS"].ToString();

            //if (ds.Tables[0].Rows[0]["FLDCREWCHANGEALERTYN"] != null && ds.Tables[0].Rows[0]["FLDCREWCHANGEALERTYN"].ToString() == "1")
            //{
            //    lblCrewChangeAlert.Visible = true;
            //    lblFireDrillYN.Visible = true;
            //    chkFireDrillYN.Visible = true;                
            //}
            //chkFireDrillYN.Checked = ds.Tables[0].Rows[0]["FLDFIREDRILLYN"].ToString() == "1" ? true : false;

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

            if (ds.Tables[0].Rows[0]["FLDINCIDENTYN"].ToString() != string.Empty)
            {
                rblHSEIndicators.SelectedValue = ds.Tables[0].Rows[0]["FLDINCIDENTYN"].ToString();
            }
            if (rblHSEIndicators.SelectedValue == "1")
                MenuHSEIndicators.Visible = true;
            else
                MenuHSEIndicators.Visible = false;

            rblSatC.SelectedValue = ds.Tables[0].Rows[0]["FLDSATC"].ToString();
            rblCCTV.SelectedValue = ds.Tables[0].Rows[0]["FLDCCTV"].ToString();
            rblHiPap.SelectedValue = ds.Tables[0].Rows[0]["FLDHIPAP"].ToString();

            if (ds.Tables[0].Rows[0]["FLDDNATESTCONDUCTEDYN"].ToString() == "1")
            {
                chkDNATest.Checked = true;
            }
            else
            {
                chkDNATest.Checked = false;
            }

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
                txtme1lasthrs.ReadOnly = true;
            }
            if (chkMEPortNoFM.Checked == false)
            {
                chkMEPortFlowDetective.Enabled = true;
                if (chkMEPortFlowDetective.Checked == true)
                {
                    txtme1lasthrs.CssClass = "readonlytextbox";
                    txtme1lasthrs.ReadOnly = true;
                    txtme1Total.Text = "";
                }
                if (chkMEPortFlowDetective.Checked == false)
                {
                    txtme1lasthrs.CssClass = "input";
                    txtme1lasthrs.ReadOnly = false;
                }

            }

            if (chkMEStbdNoFM.Checked == true)
            {
                chkMEStbdFlowDetective.Enabled = false;
                chkMEStbdFlowDetective.Checked = false;

                txtme2lasthrs.CssClass = "readonlytextbox";
                txtme2lasthrs.ReadOnly = true;
            }
            if (chkMEStbdNoFM.Checked == false)
            {
                chkMEStbdFlowDetective.Enabled = true;
                if (chkMEStbdFlowDetective.Checked == true)
                {
                    txtme2lasthrs.CssClass = "readonlytextbox";
                    txtme2lasthrs.ReadOnly = true;
                    txtme2Total.Text = "";
                }
                if (chkMEStbdFlowDetective.Checked == false)
                {
                    txtme2lasthrs.CssClass = "input";
                    txtme2lasthrs.ReadOnly = false;
                }
            }

            if (chkMEPortreturnNoFM.Checked == true)
            {
                chkMEPortreturnFlowDetective.Enabled = false;
                chkMEPortreturnFlowDetective.Checked = false;

                lblme1returnlasthrs.CssClass = "readonlytextbox";
                lblme1returnlasthrs.ReadOnly = true;
            }
            if (chkMEPortreturnNoFM.Checked == false)
            {
                chkMEPortreturnFlowDetective.Enabled = true;
                if (chkMEPortreturnFlowDetective.Checked == true)
                {
                    lblme1returnlasthrs.CssClass = "readonlytextbox";
                    lblme1returnlasthrs.ReadOnly = true;
                }
                if (chkMEPortreturnFlowDetective.Checked == false)
                {
                    lblme1returnlasthrs.CssClass = "input";
                    lblme1returnlasthrs.ReadOnly = false;
                }
            }

            if (chkMEStbdreturnNoFM.Checked == true)
            {
                chkMEStbdreturnFlowDetective.Enabled = false;
                chkMEStbdreturnFlowDetective.Checked = false;

                lblme2returnlasthrs.CssClass = "readonlytextbox";
                lblme2returnlasthrs.ReadOnly = true;
            }
            if (chkMEStbdreturnNoFM.Checked == false)
            {
                chkMEStbdreturnFlowDetective.Enabled = true;
                if (chkMEStbdreturnFlowDetective.Checked == true)
                {
                    lblme2returnlasthrs.CssClass = "readonlytextbox";
                    lblme2returnlasthrs.ReadOnly = true;
                }
                if (chkMEStbdreturnFlowDetective.Checked == false)
                {
                    lblme2returnlasthrs.CssClass = "input";
                    lblme2returnlasthrs.ReadOnly = false;
                }
            }

            if (chkAE1NoFM.Checked == true)
            {
                chkAE1FlowDetective.Enabled = false;
                chkAE1FlowDetective.Checked = false;

                txtAE1lasthrs.CssClass = "readonlytextbox";
                txtAE1lasthrs.ReadOnly = true;
            }
            if (chkAE1NoFM.Checked == false)
            {
                chkAE1FlowDetective.Enabled = true;
                if (chkAE1FlowDetective.Checked == true)
                {
                    txtAE1lasthrs.CssClass = "readonlytextbox";
                    txtAE1lasthrs.ReadOnly = true;
                    txtAE1Consumption.Text = "";
                }
                if (chkAE1FlowDetective.Checked == false)
                {
                    txtAE1lasthrs.CssClass = "input";
                    txtAE1lasthrs.ReadOnly = false;
                }
            }
            if (chkAE2NoFM.Checked == true)
            {
                chkAE2FlowDetective.Enabled = false;
                chkAE2FlowDetective.Checked = false;

                txtAE2lasthrs.CssClass = "readonlytextbox";
                txtAE2lasthrs.ReadOnly = true;
            }
            if (chkAE2NoFM.Checked == false)
            {
                chkAE2FlowDetective.Enabled = true;
                if (chkAE2FlowDetective.Checked == true)
                {
                    txtAE2lasthrs.CssClass = "readonlytextbox";
                    txtAE2lasthrs.ReadOnly = true;
                    txtAE2Consumption.Text = "";
                }
                if (chkAE2FlowDetective.Checked == false)
                {
                    txtAE2lasthrs.CssClass = "input";
                    txtAE2lasthrs.ReadOnly = false;
                }
            }

            txtSatCRemarks.Text = ds.Tables[0].Rows[0]["FLDSATCREMARKS"].ToString();
            txtCCTVRemarks.Text = ds.Tables[0].Rows[0]["FLDCCTVREMARKS"].ToString();
            txtHiPapRemarks.Text = ds.Tables[0].Rows[0]["FLDHIPAPREMARKS"].ToString();

            txtDNARemarks.Text = ds.Tables[0].Rows[0]["FLDDNATESTREMARKS"].ToString();
            txtDNATime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDDNATESTDATE"]);
            ddlvesselstatus.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELSTATUS"].ToString();
            rbnhourchange.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGE"].ToString();
            rbnhourvalue.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGEVALUE"].ToString();

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

            if (ds.Tables[0].Rows[0]["FLDCONFIRMEDYN"].ToString() == "1")
                lblAlertSenttoOFC.Visible = true;
            ViewState["REACTIVATED"] = ds.Tables[0].Rows[0]["FLDREACTIVATED"].ToString();
        }
    }
    private void CopyMidNightReport(Guid? midnightreportid)
    {
        if (midnightreportid != null)
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(midnightreportid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["REPORTDATE"] = General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
                ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ddlInstalationType.SelectedValue = ds.Tables[0].Rows[0]["FLDINSTALLATIONTYPEID"].ToString();

                //txtVoyageId.Text = ds.Tables[0].Rows[0]["FLDVOYAGEID"].ToString();
                //txtVoyageName.Text = ds.Tables[0].Rows[0]["FLDVOYAGENO"].ToString();

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
                // txtETDTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETDDATE"]);
                ddlETDLocation.SelectedValue = ds.Tables[0].Rows[0]["FLDETDLOCATIONID"].ToString();
                txtETADate.Text = ds.Tables[0].Rows[0]["FLDETADATE"].ToString();
                txtETATime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDETADATE"].ToString());// String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDETADATE"]);
                //txtPOB.Text = ds.tables[0].rows[0]["fldpob"].tostring();
                //txtcrew.text = ds.Tables[0].Rows[0]["FLDCREW"].ToString();
                txtWaveHeight.Text = ds.Tables[0].Rows[0]["FLDWAVEHEIGHT"].ToString();

                ucLatitude.TextDegree = ds.Tables[0].Rows[0]["FLDLAT1"].ToString();
                ucLatitude.TextMinute = ds.Tables[0].Rows[0]["FLDLAT2"].ToString();
                ucLatitude.TextSecond = ds.Tables[0].Rows[0]["FLDLAT3"].ToString();
                ucLatitude.TextDirection = ds.Tables[0].Rows[0]["FLDLATDIRECTION"].ToString();
                ucLongitude.TextDegree = ds.Tables[0].Rows[0]["FLDLONG1"].ToString();
                ucLongitude.TextMinute = ds.Tables[0].Rows[0]["FLDLONG2"].ToString();
                ucLongitude.TextSecond = ds.Tables[0].Rows[0]["FLDLONG3"].ToString();
                ucLongitude.TextDirection = ds.Tables[0].Rows[0]["FLDLONGDIRECTION"].ToString();

                //txtComments.Text = ds.Tables[0].Rows[0]["FLDCOMMENTS"].ToString();
                //txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();



                //ucMEPort.Text = ds.Tables[0].Rows[0]["FLDMEPORT"].ToString();
                //ucBT1.Text = ds.Tables[0].Rows[0]["FLDBT1"].ToString();
                ucFord.Text = ds.Tables[0].Rows[0]["FLDFORD"].ToString();
                //ucMEStbd.Text = ds.Tables[0].Rows[0]["FLDMESTBD"].ToString();
                //ucBT2.Text = ds.Tables[0].Rows[0]["FLDBT2"].ToString();
                ucMidship.Text = ds.Tables[0].Rows[0]["FLDMIDSHIP"].ToString();
                //ucAEI.Text = ds.Tables[0].Rows[0]["FLDAE1"].ToString();
                //ucST1.Text = ds.Tables[0].Rows[0]["FLDST1"].ToString();
                ucAft.Text = ds.Tables[0].Rows[0]["FLDAFT"].ToString();
                //ucAEII.Text = ds.Tables[0].Rows[0]["FLDAE2"].ToString();
                //ucST2.Text = ds.Tables[0].Rows[0]["FLDST2"].ToString();
                ucAverage.Text = ds.Tables[0].Rows[0]["FLDAVG"].ToString();

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

                //txtPOBClient.Text = ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString();
                //txtPOBService.Text = ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString();

                ucPort.SelectedValue = ds.Tables[0].Rows[0]["FLDPORTID"].ToString();
                ucPort.Text = ds.Tables[0].Rows[0]["PORTNAME"].ToString();

                txtArrivalDate.Text = ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString();
                txtArrivalTime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString());
                //txtArrivalTime.Text = String.Format("{0:HH:mm}", ds.Tables[0].Rows[0]["FLDARRIVALDATE"]);

                txtDepartureDate.Text = ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString();
                txtDepartureTime.SelectedDate = General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString());

                ddlvesselstatus.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELSTATUS"].ToString();
                rbnhourchange.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGE"].ToString();
                rbnhourvalue.SelectedValue = ds.Tables[0].Rows[0]["FLDHOURCHANGEVALUE"].ToString();

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



            }

            ChangeVesselStatus();
            CopyMeteorologyData(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));
            ucStatus.Text = "Previous Midnight Report Copied";
        }
        else
        {
            ucError.ErrorMessage = "Report cannot be copied because there is no Previous reports. ";
            ucError.Visible = true;
        }
    }


    private void EditMidNightReportRemarks()
    {
        if (Session["MIDNIGHTREPORTID"] != null)
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportEdit(General.GetNullableGuid(Session["MIDNIGHTREPORTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtSatCRemarks.Text = ds.Tables[0].Rows[0]["FLDSATCREMARKS"].ToString();
                txtCCTVRemarks.Text = ds.Tables[0].Rows[0]["FLDCCTVREMARKS"].ToString();
                txtHiPapRemarks.Text = ds.Tables[0].Rows[0]["FLDHIPAPREMARKS"].ToString();
            }
        }
    }


    // Bulks
    private void BindOilLoadandConsumption()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOilLoadandConsumptionList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            gvBulks.DataSource = ds;
           // gvBulks.DataBind();


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
            RadLabel lblOpeningStock = (RadLabel)e.Row.FindControl("lblOpeningStocks");
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
            RadLabel lblUnit = (RadLabel)e.Row.FindControl("lblUnit");

            UserControlMaskedTextBox ChartererLoadedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucChartererLoadedEdit");
            UserControlMaskedTextBox ChartererDischargedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucChartererDischargedEdit");
            UserControlMaskedTextBox ConsumedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucConsumedEdit");
            RadLabel lblProductName = (RadLabel)e.Row.FindControl("lblProductName");
            RadLabel lblActiveYN = (RadLabel)e.Row.FindControl("lblActiveYN");
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
            RadLabel lblOpeningStocksHeader = (RadLabel)e.Row.FindControl("lblOpeningStocksHeader");
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
                    string OilLoadedConsumptionId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOilLoadedConsumptionId")).Text;
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

            string OilTypeCode = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOilTypeCode")).Text;
            string LoadedCharterer = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucChartererLoadedEdit")).Text;
            string DischargedCharterer = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucChartererDischargedEdit")).Text;
            string Consumed = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucConsumedEdit")).Text;
            string OilLoadedConsumptionId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOilLoadedConsumptionId")).Text;
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
            UserControlDecimal LoadedCharterer = (UserControlDecimal)gvr.FindControl("ucChartererLoadedEdit");
            UserControlDecimal DischargedCharterer = (UserControlDecimal)gvr.FindControl("ucChartererDischargedEdit");
            UserControlDecimal Consumed = (UserControlDecimal)gvr.FindControl("ucConsumedEdit");
            RadLabel OilLoadedConsumptionId = (RadLabel)gvr.FindControl("lblOilLoadedConsumptionId");
            UserControlDecimal OpeningStock = (UserControlDecimal)gvr.FindControl("txtOpeningStock");
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
            //gvMeteorologyData.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CopyMeteorologyData(Guid? midnightreportid)
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportMeteorologyDataList(midnightreportid);

            gvMeteorologyData.DataSource = ds;
            //gvMeteorologyData.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvMeteorologyData_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        RadLabel lblValueType = (RadLabel)e.Row.FindControl("lblValueType");
    //        RadLabel lblShortname = (RadLabel)e.Row.FindControl("lblShortname");

    //        UserControlMaskedTextBox txtValueDecimal6Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal6Edit");
    //        UserControlMaskedTextBox txtValueDecimal12Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal12Edit");
    //        UserControlMaskedTextBox txtValueDecimal18Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal18Edit");
    //        UserControlMaskedTextBox txtValueDecimal24Edit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimal24Edit");
    //        UserControlMaskedTextBox txtValueDecimalNext24HrsEdit = (UserControlMaskedTextBox)e.Row.FindControl("txtValueDecimalNext24HrsEdit");

    //        UserControlDirection ucDirection6Edit = (UserControlDirection)e.Row.FindControl("ucDirection6Edit");
    //        UserControlDirection ucDirection12Edit = (UserControlDirection)e.Row.FindControl("ucDirection12Edit");
    //        UserControlDirection ucDirection18Edit = (UserControlDirection)e.Row.FindControl("ucDirection18Edit");
    //        UserControlDirection ucDirection24Edit = (UserControlDirection)e.Row.FindControl("ucDirection24Edit");
    //        UserControlDirection ucDirectionNext24HrsEdit = (UserControlDirection)e.Row.FindControl("ucDirectionNext24HrsEdit");

    //        UserControlSeaCondtion ucSeaCondtion6Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion6Edit");
    //        UserControlSeaCondtion ucSeaCondtion12Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion12Edit");
    //        UserControlSeaCondtion ucSeaCondtion18Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion18Edit");
    //        UserControlSeaCondtion ucSeaCondtion24Edit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtion24Edit");
    //        UserControlSeaCondtion ucSeaCondtionNext24HrsEdit = (UserControlSeaCondtion)e.Row.FindControl("ucSeaCondtionNext24HrsEdit");

    //        if (lblValueType.Text == "1")
    //        {
    //            ucDirection6Edit.Visible = false;
    //            ucDirection12Edit.Visible = false;
    //            ucDirection18Edit.Visible = false;
    //            ucDirection24Edit.Visible = false;
    //            ucDirectionNext24HrsEdit.Visible = false;

    //            ucSeaCondtion6Edit.Visible = false;
    //            ucSeaCondtion12Edit.Visible = false;
    //            ucSeaCondtion18Edit.Visible = false;
    //            ucSeaCondtion24Edit.Visible = false;
    //            ucSeaCondtionNext24HrsEdit.Visible = false;

    //            txtValueDecimal6Edit.Visible = true;
    //            if (txtValueDecimal6Edit != null)
    //                txtValueDecimal6Edit.Text = drv["FLDMETEOROLOGYVALUE"].ToString();

    //            if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYVALUE"].ToString()) != null)
    //            {
    //                if ((decimal.Parse(drv["FLDMETEOROLOGYVALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYVALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
    //                    txtValueDecimal6Edit.CssClass = "maxhighlight";
    //            }


    //            txtValueDecimal12Edit.Visible = true;
    //            if (txtValueDecimal12Edit != null)
    //                txtValueDecimal12Edit.Text = drv["FLDMETEOROLOGY1200VALUE"].ToString();

    //            if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGY1200VALUE"].ToString()) != null)
    //            {
    //                if ((decimal.Parse(drv["FLDMETEOROLOGY1200VALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGY1200VALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
    //                    txtValueDecimal12Edit.CssClass = "maxhighlight";
    //            }

    //            txtValueDecimal18Edit.Visible = true;
    //            if (txtValueDecimal18Edit != null)
    //                txtValueDecimal18Edit.Text = drv["FLDMETEOROLOGY1800VALUE"].ToString();

    //            if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGY1800VALUE"].ToString()) != null)
    //            {
    //                if ((decimal.Parse(drv["FLDMETEOROLOGY1800VALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGY1800VALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
    //                    txtValueDecimal18Edit.CssClass = "maxhighlight";
    //            }

    //            txtValueDecimal24Edit.Visible = true;
    //            if (txtValueDecimal24Edit != null)
    //                txtValueDecimal24Edit.Text = drv["FLDMETEOROLOGY2400VALUE"].ToString();

    //            if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGY2400VALUE"].ToString()) != null)
    //            {
    //                if ((decimal.Parse(drv["FLDMETEOROLOGY2400VALUE"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGY2400VALUE"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
    //                    txtValueDecimal24Edit.CssClass = "maxhighlight";
    //            }
    //            txtValueDecimalNext24HrsEdit.Visible = true;
    //            if (txtValueDecimalNext24HrsEdit != null)
    //                txtValueDecimalNext24HrsEdit.Text = drv["FLDMETEOROLOGYNEXT24HRS"].ToString();

    //            if (lblShortname != null)
    //            {
    //                if (lblShortname.Text == "WNS")
    //                {
    //                    if (General.GetNullableDecimal(drv["FLDMINFOREWINDSPEED"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXFOREWINDSPEED"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYNEXT24HRS"].ToString()) != null)
    //                    {
    //                        if ((decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) < (decimal.Parse(drv["FLDMINFOREWINDSPEED"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) > (decimal.Parse(drv["FLDMAXFOREWINDSPEED"].ToString())))
    //                            txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
    //                    }
    //                }
    //                else if (lblShortname.Text == "SWH")
    //                {
    //                    if (General.GetNullableDecimal(drv["FLDMINFORESWELLHEIGHT"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXFORESWELLHEIGHT"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYNEXT24HRS"].ToString()) != null)
    //                    {
    //                        if ((decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) < (decimal.Parse(drv["FLDMINFORESWELLHEIGHT"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) > (decimal.Parse(drv["FLDMAXFORESWELLHEIGHT"].ToString())))
    //                            txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
    //                    }
    //                }
    //                else
    //                {
    //                    if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMETEOROLOGYNEXT24HRS"].ToString()) != null)
    //                    {
    //                        if ((decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(drv["FLDMETEOROLOGYNEXT24HRS"].ToString())) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
    //                            txtValueDecimalNext24HrsEdit.CssClass = "maxhighlight";
    //                    }
    //                }
    //            }
    //        }
    //        if (lblValueType.Text == "2")
    //        {
    //            ucDirection6Edit.Visible = false;
    //            ucDirection12Edit.Visible = false;
    //            ucDirection18Edit.Visible = false;
    //            ucDirection24Edit.Visible = false;
    //            ucDirectionNext24HrsEdit.Visible = false;

    //            ucSeaCondtion6Edit.Visible = true;
    //            if (ucSeaCondtion6Edit != null)
    //                ucSeaCondtion6Edit.SelectedSeaCondition = drv["FLDMETEOROLOGYVALUE"].ToString();

    //            ucSeaCondtion12Edit.Visible = true;
    //            if (ucSeaCondtion12Edit != null)
    //                ucSeaCondtion12Edit.SelectedSeaCondition = drv["FLDMETEOROLOGY1200VALUE"].ToString();

    //            ucSeaCondtion18Edit.Visible = true;
    //            if (ucSeaCondtion18Edit != null)
    //                ucSeaCondtion18Edit.SelectedSeaCondition = drv["FLDMETEOROLOGY1800VALUE"].ToString();

    //            ucSeaCondtion24Edit.Visible = true;
    //            if (ucSeaCondtion24Edit != null)
    //                ucSeaCondtion24Edit.SelectedSeaCondition = drv["FLDMETEOROLOGY2400VALUE"].ToString();

    //            ucSeaCondtionNext24HrsEdit.Visible = true;
    //            if (ucSeaCondtionNext24HrsEdit != null)
    //                ucSeaCondtionNext24HrsEdit.SelectedSeaCondition = drv["FLDMETEOROLOGYNEXT24HRS"].ToString();

    //            txtValueDecimal6Edit.Visible = false;
    //            txtValueDecimal12Edit.Visible = false;
    //            txtValueDecimal18Edit.Visible = false;
    //            txtValueDecimal24Edit.Visible = false;
    //            txtValueDecimalNext24HrsEdit.Visible = false;
    //        }
    //        if (lblValueType.Text == "3")
    //        {
    //            ucDirection6Edit.Visible = true;
    //            if (ucDirection6Edit != null)
    //                ucDirection6Edit.SelectedDirection = drv["FLDMETEOROLOGYVALUE"].ToString();

    //            ucDirection12Edit.Visible = true;
    //            if (ucDirection12Edit != null)
    //                ucDirection12Edit.SelectedDirection = drv["FLDMETEOROLOGY1200VALUE"].ToString();

    //            ucDirection18Edit.Visible = true;
    //            if (ucDirection18Edit != null)
    //                ucDirection18Edit.SelectedDirection = drv["FLDMETEOROLOGY1800VALUE"].ToString();

    //            ucDirection24Edit.Visible = true;
    //            if (ucDirection24Edit != null)
    //                ucDirection24Edit.SelectedDirection = drv["FLDMETEOROLOGY2400VALUE"].ToString();

    //            ucDirectionNext24HrsEdit.Visible = true;
    //            if (ucDirectionNext24HrsEdit != null)
    //                ucDirectionNext24HrsEdit.SelectedDirection = drv["FLDMETEOROLOGYNEXT24HRS"].ToString();

    //            ucSeaCondtion6Edit.Visible = false;
    //            ucSeaCondtion12Edit.Visible = false;
    //            ucSeaCondtion18Edit.Visible = false;
    //            ucSeaCondtion24Edit.Visible = false;
    //            ucSeaCondtionNext24HrsEdit.Visible = false;

    //            txtValueDecimal6Edit.Visible = false;
    //            txtValueDecimal12Edit.Visible = false;
    //            txtValueDecimal18Edit.Visible = false;
    //            txtValueDecimal24Edit.Visible = false;
    //            txtValueDecimalNext24HrsEdit.Visible = false;
    //        }
    //        if (lblShortname != null && lblShortname.Text == "SW")
    //        {
    //            ucSeaCondtionNext24HrsEdit.Visible = false;
    //            ucDirectionNext24HrsEdit.Visible = false;
    //            txtValueDecimalNext24HrsEdit.Visible = false;
    //        }
    //        if (lblShortname != null && lblShortname.Text == "AIR")
    //        {
    //            ucSeaCondtionNext24HrsEdit.Visible = false;
    //            ucDirectionNext24HrsEdit.Visible = false;
    //            txtValueDecimalNext24HrsEdit.Visible = false;
    //        }
    //        if (lblShortname != null && lblShortname.Text == "VIS")
    //        {
    //            ucSeaCondtionNext24HrsEdit.Visible = false;
    //            ucDirectionNext24HrsEdit.Visible = false;
    //            txtValueDecimalNext24HrsEdit.Visible = false;
    //        }
    //        if (lblShortname != null && lblShortname.Text == "BAR")
    //        {
    //            ucSeaCondtionNext24HrsEdit.Visible = false;
    //            ucDirectionNext24HrsEdit.Visible = false;
    //            txtValueDecimalNext24HrsEdit.Visible = false;
    //        }
    //    }

    //}

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
                    string MeteorologyDId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblMeteorologyDId")).Text;
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
            string MeteorologyValue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtMeteorologyValueEdit")).Text;
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

                UserControlMaskNumber txtValueDecimal6Edit = (UserControlMaskNumber)gvr.FindControl("txtValueDecimal6Edit");
                UserControlDirection ucDirection6Edit = (UserControlDirection)gvr.FindControl("ucDirection6Edit");
                UserControlSeaCondtion ucSeaCondtion6Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion6Edit");

                UserControlMaskNumber txtValueDecimal12Edit = (UserControlMaskNumber)gvr.FindControl("txtValueDecimal12Edit");
                UserControlDirection ucDirection12Edit = (UserControlDirection)gvr.FindControl("ucDirection12Edit");
                UserControlSeaCondtion ucSeaCondtion12Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion12Edit");

                UserControlMaskNumber txtValueDecimal18Edit = (UserControlMaskNumber)gvr.FindControl("txtValueDecimal18Edit");
                UserControlDirection ucDirection18Edit = (UserControlDirection)gvr.FindControl("ucDirection18Edit");
                UserControlSeaCondtion ucSeaCondtion18Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion18Edit");

                UserControlMaskNumber txtValueDecimal24Edit = (UserControlMaskNumber)gvr.FindControl("txtValueDecimal24Edit");
                UserControlDirection ucDirection24Edit = (UserControlDirection)gvr.FindControl("ucDirection24Edit");
                UserControlSeaCondtion ucSeaCondtion24Edit = (UserControlSeaCondtion)gvr.FindControl("ucSeaCondtion24Edit");

                UserControlMaskNumber txtValueDecimalNext24HrsEdit = (UserControlMaskNumber)gvr.FindControl("txtValueDecimalNext24HrsEdit");
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
                UserControlMaskNumber stopcards = (UserControlMaskNumber)gvr.FindControl("ucstopcards");
                RadLabel nearmiss = (RadLabel)gvr.FindControl("lblnearmiss");
                UserControlMaskNumber ExercisesandDrills = (UserControlMaskNumber)gvr.FindControl("ucExercisesandDrills");
                RadLabel NoofRiskAssesment = (RadLabel)gvr.FindControl("lblNoofRiskAssesment");
                UserControlMaskNumber noofsafety = (UserControlMaskNumber)gvr.FindControl("ucnoofsafety");
                UserControlMaskNumber PTWIssued = (UserControlMaskNumber)gvr.FindControl("ucPTWIssued");
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

    private bool IsValidMeteorologyData(string MeteorologyValue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (MeteorologyValue.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";

        return (!ucError.IsError);
    }


    // Look Ahead Planned Activity
    private void BindPlannedActivity()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPlannedActivityList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvPlannedActivity.DataSource = ds;
           // gvPlannedActivity.DataBind();

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




    // Vessel's Movements
    private void BindVesselMovements()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportVesselMovementsList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvVesselMovements.DataSource = ds;
           // gvVesselMovements.DataBind(); //dont comment

           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        if(e.Item is GridFooterItem)
        {
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
    }

    //protected void gvVesselMovements_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindVesselMovements();
    //}

    //protected void gvVesselMovements_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvVesselMovements_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = de.NewEditIndex;
    //        BindVesselMovements();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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

    //        string FromTime = (((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFromTime")).Text.Trim() == "__:__") ? string.Empty : ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFromTime")).Text;
    //        string Activity = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text;
    //        string ddlActivity = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlActivityEdit")).SelectedValue;
    //        string ActivityId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblVesselMovementsEditId")).Text;

    //        string fromtime = (FromTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : FromTime;
    //        string TimeDuration = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblTimeDurationEdit")).Text;
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


    //            string TimeDuration = ((RadLabel)_gridView.FooterRow.FindControl("lblTimeDurationAdd")).Text;
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
    //                string VesselMovementsId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblVesselMovementsId")).Text;
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

    protected void gvExternalInspection_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            Table gridTable = (Table)gvExternalInspection.Controls[0];
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                LinkButton db = (LinkButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddlAuditTypeEdit = (DropDownList)e.Row.FindControl("ddlAuditTypeEdit");
                //RadLabel lblTypeOfInspection = (RadLabel)e.Row.FindControl("lblTypeOfInspection");

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

    //protected void gvExternalInspection_RowUpdating(object sender, GridViewUpdateEventArgs e)
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

    //        string lblExternalInspectionIdEdit = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblExternalInspectionIdEdit")).Text;
    //        string txtTypeOfInspectionEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTypeOfInspectionEdit")).Text;
    //        string txtInspectingCompanyEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectingCompanyEdit")).Text;
    //        string txtInspectorNameEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtInspectorNameEdit")).Text;
    //        string txtNumberOfNCEdit = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("txtNumberOfNCEdit")).Text;

    //        if (!IsValidExternalInspection(txtTypeOfInspectionEdit, txtInspectingCompanyEdit, txtInspectorNameEdit, txtNumberOfNCEdit))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
    //                , General.GetNullableGuid(lblExternalInspectionIdEdit), txtTypeOfInspectionEdit, txtInspectingCompanyEdit, txtInspectorNameEdit
    //                , General.GetNullableInteger(txtNumberOfNCEdit));

    //        _gridView.EditIndex = -1;
    //        BindExternalInspection();
    //        EditMidNightReport();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvExternalInspection_RowCommand(object sender, GridViewCommandEventArgs e)
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

    //            string txtTypeOfInspectionAdd = ((TextBox)_gridView.FooterRow.FindControl("txtTypeOfInspectionAdd")).Text;
    //            string txtInspectingCompanyAdd = ((TextBox)_gridView.FooterRow.FindControl("txtInspectingCompanyAdd")).Text;
    //            string txtInspectorNameAdd = ((TextBox)_gridView.FooterRow.FindControl("txtInspectorNameAdd")).Text;
    //            string txtNumberOfNCAdd = ((UserControlMaskedTextBox)_gridView.FooterRow.FindControl("txtNumberOfNCAdd")).Text;


    //            if (!IsValidExternalInspection(txtTypeOfInspectionAdd, txtInspectingCompanyAdd, txtInspectorNameAdd, txtNumberOfNCAdd))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionInsert(int.Parse(ViewState["VESSELID"].ToString()), new Guid(Session["MIDNIGHTREPORTID"].ToString())
    //                    , null, txtTypeOfInspectionAdd, txtInspectingCompanyAdd, txtInspectorNameAdd, General.GetNullableInteger(txtNumberOfNCAdd));

    //            //    string FromTime = (((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text.Trim() == "__:__") ? string.Empty : ((TextBox)_gridView.FooterRow.FindControl("txtFromTimeAdd")).Text;
    //            //    string Activity = ((TextBox)_gridView.FooterRow.FindControl("txtDescriptionAdd")).Text;
    //            //    string ddlActivity = ((DropDownList)_gridView.FooterRow.FindControl("ddlActivityAdd")).SelectedValue;

    //            BindExternalInspection();
    //        }

    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            if (Session["MIDNIGHTREPORTID"] != null)
    //            {
    //                string lblExternalInspectionId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblExternalInspectionId")).Text;
    //                if (lblExternalInspectionId != "")
    //                {
    //                    PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportExternalInspectionDelete(new Guid(Session["MIDNIGHTREPORTID"].ToString()), new Guid(lblExternalInspectionId));
    //                }
    //            }
    //            BindExternalInspection();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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


    // Ship's Crew

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
                                                                       , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                       , ref iRowCount, ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvShipCrew", "Crew List", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvShipCrew.DataSource = dt;
                //gvShipCrew.DataBind();
                txtCrewOff.Text = ds.Tables[0].Rows[0]["FLDCREWOFF"].ToString();
                txtCrewOn.Text = ds.Tables[0].Rows[0]["FLDCREWON"].ToString();
                txtPOBCrew.Text = ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString();
                txtTotalOB.Text = (int.Parse(ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString())).ToString();
                txtPOB.Text = (int.Parse(ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString()) + int.Parse(ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString())).ToString();
                txtPOBCrew.Text = ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString();
                txtPOBClient.Text = ds.Tables[0].Rows[0]["FLDPOBCLIENT"].ToString();
                txtPOBService.Text = ds.Tables[0].Rows[0]["FLDPOBSERVICE"].ToString();
                txtCrew.Text = ds.Tables[0].Rows[0]["FLDPOWCREW"].ToString();
                txtMaster.Text = ds.Tables[0].Rows[0]["FLDMASTEROFSHIP"].ToString();
            }
            else
            {
                gvShipCrew.DataSource = dt;
               // gvShipCrew.DataBind();
            }
            gvShipCrew.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetPageNavigator()
    //{
    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

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
            //gv.DataBind();

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

    //protected void gvShipCrew_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;

    //        RadLabel lblNCCount = (RadLabel)e.Row.FindControl("lblNCCount");
    //        Image imgFlag = (Image)e.Row.FindControl("imgFlag");
    //        Image Imageyellow = (Image)e.Row.FindControl("Imageyellow");
    //        RadLabel lbllastRHRecorddate = (RadLabel)e.Row.FindControl("lblRHDate");
    //        RadLabel lblLevelCode = (RadLabel)e.Row.FindControl("lblLevelCode");


    //        DateTime MidNightReportDate = DateTime.Parse(txtDate.Text);
    //        DateTime RHDate = MidNightReportDate;
    //        if (!string.IsNullOrEmpty(lbllastRHRecorddate.Text))
    //            RHDate = DateTime.Parse(lbllastRHRecorddate.Text);

    //        if (MidNightReportDate.CompareTo(RHDate) == 1)
    //        {
    //            e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
    //            //lbllastRHRecorddate.BackColor = System.Drawing.Color.Red;
    //        }

    //        if (lblLevelCode != null && !string.IsNullOrEmpty(lblLevelCode.Text) && lblLevelCode.Text == "1")
    //        {
    //            Imageyellow.Visible = true;
    //            Imageyellow.ImageUrl = Session["images"] + "/Yellow-symbol.png";
    //        }
    //        if (lblLevelCode != null && !string.IsNullOrEmpty(lblLevelCode.Text) && lblLevelCode.Text == "2")
    //        {
    //            imgFlag.Visible = true;
    //            imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
    //        }


    //        LinkButton lnkNCCount = (LinkButton)e.Row.FindControl("lnkNCCount");

    //        if (lnkNCCount != null && lblNCCount != null)
    //        {
    //            if (lblNCCount.Text == "0" || lblNCCount.Text == string.Empty)
    //            {
    //                lblNCCount.Visible = true;
    //                lnkNCCount.Visible = false;
    //            }
    //            else
    //            {
    //                lblNCCount.Visible = false;
    //                lnkNCCount.Visible = true;
    //            }
    //            //lnkNCCount.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../VesselAccounts/VesselAccountsRHWorkCalenderRemarks.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkNCCount.Attributes.Add("onclick", "parent.Openpopup('MoreInfo', '', '../VesselAccounts/VesselAccountsRHWorkCalenderRemarks.aspx?CalenderId=" + drv["FLDRESTHOURCALENDARID"].ToString() + "&EMPID=" + drv["FLDEMPLOYEEID"].ToString() + "&RHStartId=" + drv["FLDRESTHOURSTARTID"].ToString() + "'); return false;");
    //        }
    //    }
    //}

    // HSE Indicators
    private void BindHSEIndicators()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportHSEIndicatorsList(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvHSEIndicators.DataSource = ds;
            //gvHSEIndicators.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvHSEIndicators_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

    //        TableCell tbHSEIndicators = new TableCell();
    //        TableCell tbLaggingIndicators = new TableCell();
    //        TableCell tbLeadingIndicators = new TableCell();

    //        tbHSEIndicators.ColumnSpan = 1;
    //        tbLaggingIndicators.ColumnSpan = 7;
    //        tbLeadingIndicators.ColumnSpan = 7;

    //        tbHSEIndicators.Text = "HSE Indicators";
    //        tbLaggingIndicators.Text = "Lagging Indicators";
    //        tbLeadingIndicators.Text = "Leading Indicators";

    //        tbHSEIndicators.Attributes.Add("style", "text-align:center;");
    //        tbLaggingIndicators.Attributes.Add("style", "text-align:center;");
    //        tbLeadingIndicators.Attributes.Add("style", "text-align:center;");

    //        gv.Cells.Add(tbHSEIndicators);
    //        gv.Cells.Add(tbLaggingIndicators);
    //        gv.Cells.Add(tbLeadingIndicators);

    //        gvHSEIndicators.Controls[0].Controls.AddAt(0, gv);
    //    }
    //}

    //protected void gvHSEIndicators_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        RadLabel lblEH = (RadLabel)e.Row.FindControl("lblEHHeader");
    //        UserControlToolTip ucEH = (UserControlToolTip)e.Row.FindControl("ucEHHeader");
    //        lblEH.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucEH.ToolTip + "', 'visible');");
    //        lblEH.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucEH.ToolTip + "', 'hidden');");

    //        RadLabel lblhpi = (RadLabel)e.Row.FindControl("lblhpiHeader");
    //        UserControlToolTip ucthpi = (UserControlToolTip)e.Row.FindControl("uchpiHeader");
    //        lblhpi.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'visible');");
    //        lblhpi.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'hidden');");


    //        RadLabel lbllti = (RadLabel)e.Row.FindControl("lblltiHeader");
    //        UserControlToolTip uctlti = (UserControlToolTip)e.Row.FindControl("ucltiHeader");
    //        lbllti.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctlti.ToolTip + "', 'visible');");
    //        lbllti.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctlti.ToolTip + "', 'hidden');");


    //        RadLabel lblrwc = (RadLabel)e.Row.FindControl("lblrwcHeader");
    //        UserControlToolTip uctrwc = (UserControlToolTip)e.Row.FindControl("ucrwcHeader");
    //        lblrwc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'visible');");
    //        lblrwc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'hidden');");


    //        RadLabel lblmtc = (RadLabel)e.Row.FindControl("lblmtcHeader");
    //        UserControlToolTip uctmtc = (UserControlToolTip)e.Row.FindControl("ucmtcHeader");
    //        lblmtc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'visible');");
    //        lblmtc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'hidden');");

    //        RadLabel lblfac = (RadLabel)e.Row.FindControl("lblfacHeader");
    //        UserControlToolTip uctfac = (UserControlToolTip)e.Row.FindControl("ucfacHeader");
    //        lblfac.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctfac.ToolTip + "', 'visible');");
    //        lblfac.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctfac.ToolTip + "', 'hidden');");

    //        RadLabel lblEnvInc = (RadLabel)e.Row.FindControl("lblEnvironmentalIncidentHeader");
    //        UserControlToolTip uctEnvInc = (UserControlToolTip)e.Row.FindControl("ucEnvironmentalIncidentHeader");
    //        lblEnvInc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'visible');");
    //        lblEnvInc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'hidden');");


    //        RadLabel lblnmr = (RadLabel)e.Row.FindControl("lblnearmissHeader");
    //        UserControlToolTip uctnmr = (UserControlToolTip)e.Row.FindControl("ucnearmissHeader");
    //        lblnmr.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'visible');");
    //        lblnmr.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'hidden');");

    //        RadLabel lblstpcrd = (RadLabel)e.Row.FindControl("lblstopcardsHeader");
    //        UserControlToolTip uctstpcrd = (UserControlToolTip)e.Row.FindControl("ucstopcardsHeader");
    //        lblstpcrd.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'visible');");
    //        lblstpcrd.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'hidden');");

    //        RadLabel lbled = (RadLabel)e.Row.FindControl("lblExercisesandDrillsHeader");
    //        UserControlToolTip ucted = (UserControlToolTip)e.Row.FindControl("ucExercisesandDrillsHeader");
    //        lbled.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucted.ToolTip + "', 'visible');");
    //        lbled.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucted.ToolTip + "', 'hidden');");

    //        RadLabel lblra = (RadLabel)e.Row.FindControl("lblNoofRiskAssesmentHeader");
    //        UserControlToolTip uctra = (UserControlToolTip)e.Row.FindControl("ucNoofRiskAssesmentHeader");
    //        lblra.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctra.ToolTip + "', 'visible');");
    //        lblra.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctra.ToolTip + "', 'hidden');");

    //        RadLabel lblsfty = (RadLabel)e.Row.FindControl("lblnoofsafetyHeader");
    //        UserControlToolTip uctsfty = (UserControlToolTip)e.Row.FindControl("ucnoofsafetyHeader");
    //        lblsfty.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'visible');");
    //        lblsfty.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'hidden');");

    //        RadLabel lblptw = (RadLabel)e.Row.FindControl("lblPTWIssuedHeader");
    //        UserControlToolTip uctptw = (UserControlToolTip)e.Row.FindControl("uclblPTWIssuedHeader");
    //        lblptw.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctptw.ToolTip + "', 'visible');");
    //        lblptw.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctptw.ToolTip + "', 'hidden');");

    //        RadLabel lbluauc = (RadLabel)e.Row.FindControl("lblUnsafeActsHeader");
    //        UserControlToolTip uctuauc = (UserControlToolTip)e.Row.FindControl("ucUnsafeActsHeader");
    //        lbluauc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctuauc.ToolTip + "', 'visible');");
    //        lbluauc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctuauc.ToolTip + "', 'hidden');");

    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        RadLabel lblEH = (RadLabel)e.Row.FindControl("lblEH");
    //        int Crew = (General.GetNullableInteger(txtCrew.Text) == null ? 0 : int.Parse(txtCrew.Text));
    //        RadLabel lblHSEIndicator = (RadLabel)e.Row.FindControl("lblhpiitem");
    //        //e.Row.ReadOnly = true;
    //        if (lblHSEIndicator.Text == "Last 24 hrs")
    //        {

    //            double exposurehour = 0;
    //            if ((rbnhourchange.SelectedValue) == "1" || (rbnhourchange.SelectedValue) == "2")
    //            {
    //                if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "1")
    //                {
    //                    exposurehour = (Crew * 23.5);
    //                }
    //                if ((rbnhourchange.SelectedValue) == "1" && (rbnhourvalue.SelectedValue) == "2")
    //                {
    //                    exposurehour = (Crew * 23);
    //                }
    //                if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "1")
    //                {
    //                    exposurehour = (Crew * 24.5);
    //                }
    //                if ((rbnhourchange.SelectedValue) == "2" && (rbnhourvalue.SelectedValue) == "2")
    //                {
    //                    exposurehour = (Crew * 25);
    //                }
    //            }
    //            else
    //            {
    //                exposurehour = (Crew * 24);
    //            }
    //            lblEH.Text = exposurehour.ToString();
    //            UserControlMaskedTextBox ucstopcards = (UserControlMaskedTextBox)e.Row.FindControl("ucstopcards");
    //            UserControlMaskedTextBox ucExercisesandDrills = (UserControlMaskedTextBox)e.Row.FindControl("ucExercisesandDrills");
    //            UserControlMaskedTextBox ucnoofsafety = (UserControlMaskedTextBox)e.Row.FindControl("ucnoofsafety");
    //            UserControlMaskedTextBox ucPTWIssued = (UserControlMaskedTextBox)e.Row.FindControl("ucPTWIssued");

    //            RadLabel lblstopcards = (RadLabel)e.Row.FindControl("lblstopcards");
    //            RadLabel lblExercisesandDrills = (RadLabel)e.Row.FindControl("lblExercisesandDrills");
    //            RadLabel lblnoofsafety = (RadLabel)e.Row.FindControl("lblnoofsafety");
    //            RadLabel lblPTWIssued = (RadLabel)e.Row.FindControl("lblPTWIssued");

    //            ucstopcards.Visible = true;
    //            ucExercisesandDrills.Visible = true;
    //            ucnoofsafety.Visible = true;
    //            ucPTWIssued.Visible = true;

    //            lblstopcards.Visible = false;
    //            lblExercisesandDrills.Visible = false;
    //            lblnoofsafety.Visible = false;
    //            lblPTWIssued.Visible = false;

    //            LinkButton lnkHpi = (LinkButton)e.Row.FindControl("lnkHpi");
    //            LinkButton lnkEnvRelease = (LinkButton)e.Row.FindControl("lnkEnvRelease");
    //            LinkButton lnkLit = (LinkButton)e.Row.FindControl("lnkLit");
    //            LinkButton lnkrwc = (LinkButton)e.Row.FindControl("lnkrwc");
    //            LinkButton lnkMtc = (LinkButton)e.Row.FindControl("lnkMtc");
    //            LinkButton lnkFac = (LinkButton)e.Row.FindControl("lnkFac");
    //            LinkButton lnkNearmiss = (LinkButton)e.Row.FindControl("lnkNearmiss");
    //            LinkButton lnkUnsafeActs = (LinkButton)e.Row.FindControl("lnkUnsafeActs");
    //            LinkButton lnkNoofRiskAssesment = (LinkButton)e.Row.FindControl("lnkNoofRiskAssesment");

    //            RadLabel lblhpi = (RadLabel)e.Row.FindControl("lblhpi");
    //            RadLabel lblEnvironmentalIncident = (RadLabel)e.Row.FindControl("lblEnvironmentalIncident");
    //            RadLabel lbllit = (RadLabel)e.Row.FindControl("lbllit");
    //            RadLabel lblrwc = (RadLabel)e.Row.FindControl("lblrwc");
    //            RadLabel lblmtc = (RadLabel)e.Row.FindControl("lblmtc");
    //            RadLabel lblfac = (RadLabel)e.Row.FindControl("lblfac");
    //            RadLabel lblnearmiss = (RadLabel)e.Row.FindControl("lblnearmiss");
    //            RadLabel lblUnsafeActs = (RadLabel)e.Row.FindControl("lblUnsafeActs");
    //            RadLabel lblNoofRiskAssesment = (RadLabel)e.Row.FindControl("lblNoofRiskAssesment");

    //            lnkHpi.Visible = true;
    //            lnkEnvRelease.Visible = true;
    //            lnkLit.Visible = true;
    //            lnkrwc.Visible = true;
    //            lnkMtc.Visible = true;
    //            lnkFac.Visible = true;
    //            lnkNearmiss.Visible = true;
    //            lnkUnsafeActs.Visible = true;
    //            lnkNoofRiskAssesment.Visible = true;

    //            lblhpi.Visible = false;
    //            lblEnvironmentalIncident.Visible = false;
    //            lbllit.Visible = false;
    //            lblrwc.Visible = false;
    //            lblmtc.Visible = false;
    //            lblfac.Visible = false;
    //            lblnearmiss.Visible = false;
    //            lblUnsafeActs.Visible = false;
    //            lblNoofRiskAssesment.Visible = false;


    //            lnkHpi.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=HPI&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkEnvRelease.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=ENV&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkLit.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=LTI&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkrwc.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=RWC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkMtc.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=MTC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkFac.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=FAC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkNearmiss.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=NM&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkUnsafeActs.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=UAUC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
    //            lnkNoofRiskAssesment.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRhseIncident.aspx?HSEType=RA&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

    //        }

    //    }
    //}


    // Passenger
    private void BindPassenger()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPassengerList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvPassenger.DataSource = ds;
            //gvPassenger.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPassenger_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
    }



    private bool IsValidOperationalTimeSummary(string MeteorologyValue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (MeteorologyValue.Trim().Equals(""))
            ucError.ErrorMessage = "Value is required.";

        return (!ucError.IsError);
    }

    // RequisionsandPO
    private void BindRequisionsandPO()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportRequisionsandPOsList(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));

            gvRequisionsandPO.DataSource = ds;
            //gvRequisionsandPO.DataBind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvRequisionsandPO_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //        DataRowView drv = (DataRowView)e.Row.DataItem;

    //        LinkButton lnk1 = (LinkButton)e.Row.FindControl("lblCount");
    //        RadLabel lblType = (RadLabel)e.Row.FindControl("lblType");
    //        lnk1.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewOffshoreDMRRequisitionandPODetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

    //    }
    //}



    // Operational Time Summary
    private void BindOperationalTimeSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            gvOperationalTimeSummary.DataSource = ds;
          //  gvOperationalTimeSummary.DataBind(); //dont comment

            //decimal total = ((General.GetNullableDecimal(txtgrandtotal.Text) == null ? 0 : decimal.Parse(txtgrandtotal.Text)) +
            //              (General.GetNullableDecimal(txtAE1Consumption.Text) == null ? 0 : decimal.Parse(txtAE1Consumption.Text)) +
            //              (General.GetNullableDecimal(txtAE2Consumption.Text) == null ? 0 : decimal.Parse(txtAE2Consumption.Text)) +
            //              (General.GetNullableDecimal(ucotherConsumption.Text) == null ? 0 : decimal.Parse(ucotherConsumption.Text))
            //             );

            //GridFooterItem footerItem = (GridFooterItem)gvOperationalTimeSummary.MasterTableView.GetItems(GridItemType.Footer)[0];
            //// Button btn = (Button)footerItem.FindControl("Button1");
            //RadLabel TotalFOC = (RadLabel)footerItem.FindControl("lblTotalFOC");
            //RadLabel lblTotalTime = (RadLabel)footerItem.FindControl("lblTotalTime");
            //RadLabel lblTotalDistance = (RadLabel)footerItem.FindControl("lblTotalDistance");
            //if (ds.Tables[0].Rows.Count > 0 && (General.GetNullableDecimal(ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString()) == null ? 0 : decimal.Parse(ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString())) - total != 0)
            //{
            //    TotalFOC.BackColor = System.Drawing.Color.OrangeRed;

            //    TotalFOC.Text = ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString();
            //    lblTotalTime.Text = ds.Tables[0].Rows[0]["FLDTOTALTIMEDURATION"].ToString();
            //    lblTotalDistance.Text = ds.Tables[0].Rows[0]["FLDTOTALDISTANCE"].ToString();
            //    txtDPTime.Text = ds.Tables[0].Rows[0]["FLDTOTALDPTIME"].ToString();
            //    txtDPFuelConsumption.Text = ds.Tables[0].Rows[0]["FLDTOTALDPFUELCONSUPTION"].ToString();
            //}
            //else
            //{
            //    TotalFOC.BorderColor = System.Drawing.Color.Black;
            //}


            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvOperationalTimeSummary_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        UserControlMaskedTextBox ucFuelConsHrEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucFuelConsHrEdit");
    //        UserControlMaskedTextBox ucFuelConsumptionEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucFuelConsumptionEdit");
    //        RadLabel lblTimeDurationEdit = (RadLabel)e.Row.FindControl("lblTimeDurationEdit");
    //        RadLabel lblDistanceApplicable = (RadLabel)e.Row.FindControl("lblDistanceApplicable");
    //        UserControlMaskedTextBox ucSeaStreamDistanceEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucSeaStreamDistanceEdit");
    //        UserControlMaskedTextBox ucSpeedEdit = (UserControlMaskedTextBox)e.Row.FindControl("ucSpeedEdit");

    //        if (ucFuelConsumptionEdit != null && lblTimeDurationEdit != null)
    //        {
    //            if (General.GetNullableDecimal(ucFuelConsumptionEdit.Text) != null && General.GetNullableDecimal(ucFuelConsumptionEdit.Text) != 0)
    //            {
    //                decimal timeDuration = Convert.ToDecimal(lblTimeDurationEdit.Text);
    //                if (ucFuelConsHrEdit != null)
    //                    ucFuelConsHrEdit.Text = (Convert.ToDecimal(ucFuelConsumptionEdit.Text) / (Math.Floor(timeDuration) + ((timeDuration - Math.Floor(timeDuration)) * 100 / 60))).ToString();
    //            }
    //        }

    //        if (General.GetNullableDecimal(ucFuelConsHrEdit.Text) != null)
    //        {
    //            if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && ((decimal.Parse(ucFuelConsHrEdit.Text)) < (decimal.Parse(drv["FLDMINVALUE"].ToString()))))
    //            {
    //                ucFuelConsHrEdit.CssClass = "maxhighlight";
    //            }
    //            if (General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && (decimal.Parse(ucFuelConsHrEdit.Text)) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
    //            {
    //                ucFuelConsHrEdit.CssClass = "maxhighlight";
    //            }
    //        }
    //        //if (General.GetNullableDecimal(drv["FLDMINVALUE"].ToString()) != null && General.GetNullableDecimal(drv["FLDMAXVALUE"].ToString()) != null && General.GetNullableDecimal(ucFuelConsHrEdit.Text) != null)
    //        //{
    //        //    if ((decimal.Parse(ucFuelConsHrEdit.Text)) < (decimal.Parse(drv["FLDMINVALUE"].ToString())) || (decimal.Parse(ucFuelConsHrEdit.Text)) > (decimal.Parse(drv["FLDMAXVALUE"].ToString())))
    //        //        ucFuelConsHrEdit.CssClass = "maxhighlight";
    //        //}
    //        if (lblDistanceApplicable != null && ucSeaStreamDistanceEdit != null)
    //        {
    //            if (lblDistanceApplicable.Text == "0")
    //            {
    //                ucSeaStreamDistanceEdit.CssClass = "readonlytextbox";
    //                ucSeaStreamDistanceEdit.ReadOnly = true;
    //            }
    //            if (ucSeaStreamDistanceEdit != null && General.GetNullableDecimal(ucSeaStreamDistanceEdit.Text) != null)
    //            {
    //                decimal timeDuration = Convert.ToDecimal(lblTimeDurationEdit.Text);
    //                ucSpeedEdit.Text = (Convert.ToDecimal(ucSeaStreamDistanceEdit.Text) / (Math.Floor(timeDuration) + ((timeDuration - Math.Floor(timeDuration)) * 100 / 60))).ToString();

    //            }
    //        }

    //        if (General.GetNullableDecimal(ucSpeedEdit.Text) != null)
    //        {
    //            if (General.GetNullableDecimal(drv["FLDMINVALUESPD"].ToString()) != null && ((decimal.Parse(ucSpeedEdit.Text)) < (decimal.Parse(drv["FLDMINVALUESPD"].ToString()))))
    //            {
    //                ucSpeedEdit.CssClass = "maxhighlight";
    //            }
    //            if (General.GetNullableDecimal(drv["FLDMAXVALUESPD"].ToString()) != null && (decimal.Parse(ucSpeedEdit.Text)) > (decimal.Parse(drv["FLDMAXVALUESPD"].ToString())))
    //            {
    //                ucSpeedEdit.CssClass = "maxhighlight";
    //            }
    //        }
    //    }
    //}

    protected void gvOperationalTimeSummary_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
    }

    //protected void gvOperationalTimeSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            if (Session["MIDNIGHTREPORTID"] != null)
    //            {
    //                string OperationalTimeSummaryId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTimeSummaryId")).Text;
    //                if (OperationalTimeSummaryId != "")
    //                {
    //                    PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryDelete(new Guid(OperationalTimeSummaryId));
    //                }
    //            }
    //            BindOperationalTimeSummary();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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

            string OperationalTaskId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTaskId")).Text;
            string TimeDuration = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblTimeEdit")).Text;
            string FuelConsumption = ((UserControlMaskedTextBox)_gridView.Rows[nCurrentRow].FindControl("ucFuelConsumptionEdit")).Text;
            string OperationalTimeSummaryId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTimeSummaryEditId")).Text;
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
                            //else
                            //    txtAE1Consumption.CssClass = "readonlytextbox";
                            break;
                        }
                    case "FLDAE2FOCONS":
                        {
                            if (General.GetNullableDecimal(txtAE2Consumption.Text) < minvalue || General.GetNullableDecimal(txtAE2Consumption.Text) > maxvalue)
                                txtAE2Consumption.CssClass = "maxhighlight";
                            //else
                            //    txtAE2Consumption.CssClass = "readonlytextbox";
                            break;
                        }
                }
            }
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
            txtETATime.Enabled = false;// txtETATime.ReadOnly = true;
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
            txtDepartureTime.Enabled = true; //txtDepartureTime.ReadOnly = false;

            txtDepartureDate.Enabled = true;
            //txtDepartureTime.Enabled = true;
            txtArrivalDate.Enabled = true;
            //txtArrivalTime.Enabled = true;
            txtETADate.Enabled = true;
            //txtETATime.Enabled = true;
            txtETDDate.Enabled = true;
            // txtETDTime.Enabled = true;

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
            txtETDTime.Enabled = false;//txtETATime.ReadOnly = true;
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
            txtETDTime.Enabled = false;// txtDepartureTime.ReadOnly = true;

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
            txtETDTime.Enabled = false;// txtETDTime.ReadOnly = true;
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
            txtETDTime.Enabled = true;//txtDepartureTime.ReadOnly = false;

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
            txtETDTime.Enabled = false;//txtArrivalTime.ReadOnly = true;
            txtDepartureDate.CssClass = "input";
            txtDepartureDate.ReadOnly = false;
            txtDepartureTime.CssClass = "input";
            txtETDTime.Enabled = true;//txtDepartureTime.ReadOnly = false;

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
            txtETDTime.Enabled = true;// txtETDTime.ReadOnly = false;
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
            txtETATime.SelectedTime = null;
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
        if (ddlvesselstatus.SelectedValue == "5")
        {
            ucPort.Enabled = true;
            ucPort.CssClass = "input_mandatory";
            txtETDDate.ReadOnly = true;
            txtETDTime.Enabled = false;// txtETDTime.ReadOnly = true;
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
            txtDepartureTime.Enabled = false; //txtDepartureTime.ReadOnly = true;

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

    private void BindDueAudits()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportDueAudits(int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));

            gvShipAudit.DataSource = ds;
            //gvShipAudit.DataBind();

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
            //gvShipTasks.DataBind();


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
           // gvCertificates.DataBind();

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
            //gvUnsafe.DataBind();

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
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        DataSet dt = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRCurrentCharterer(int.Parse(Session["VESSELID"].ToString()), General.GetNullableDateTime(txtDate.Text) == null ? System.DateTime.Now : General.GetNullableDateTime(txtDate.Text));
        if (dt.Tables[0].Rows.Count > 0)
        {
            txtVoyageId.Text = dt.Tables[0].Rows[0]["FLDVOYAGEID"].ToString();
            txtVoyageName.Text = dt.Tables[0].Rows[0]["FLDVOYAGENO"].ToString();
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
    protected void rblMachineryFailure_OnSelectedIndexChanged(Object sender, EventArgs args)
    {
        if (rblMachineryFailure.SelectedValue == "1")
            MenuMachineryFailure.Visible = true;
        else
            MenuMachineryFailure.Visible = false;
    }
    protected void rblHSEIndicators_OnSelectedIndexChanged(Object sender, EventArgs args)
    {
        if (rblHSEIndicators.SelectedValue == "1")
            MenuHSEIndicators.Visible = true;
        else
            MenuHSEIndicators.Visible = false;
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
            txtme1lasthrs.ReadOnly = true;
            txtme1Total.Text = "";
        }
        if (chkMEPortFlowDetective.Checked == false)
        {
            txtme1lasthrs.CssClass = "input";
            txtme1lasthrs.ReadOnly = false;
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
            txtme2lasthrs.ReadOnly = true;
            txtme2Total.Text = "";
        }
        if (chkMEStbdFlowDetective.Checked == false)
        {
            txtme2lasthrs.CssClass = "input";
            txtme2lasthrs.ReadOnly = false;
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
            txtAE1lasthrs.ReadOnly = true;
            txtAE1Consumption.Text = "";
        }
        if (chkAE1FlowDetective.Checked == false)
        {
            txtAE1lasthrs.CssClass = "input";
            txtAE1lasthrs.ReadOnly = false;
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
            txtAE2lasthrs.ReadOnly = true;
            txtAE2Consumption.Text = "";
        }
        if (chkAE2FlowDetective.Checked == false)
        {
            txtAE2lasthrs.CssClass = "input";
            txtAE2lasthrs.ReadOnly = false;
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
            txtme1lasthrs.ReadOnly = true;
            txtme1lasthrs.Text = "";
        }
        if (chkMEPortNoFM.Checked == false)
        {
            txtme1lasthrs.CssClass = "input";
            txtme1lasthrs.ReadOnly = false;
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
            lblme1returnlasthrs.ReadOnly = true;
            lblme1returnlasthrs.Text = "";
        }
        if (chkMEPortreturnNoFM.Checked == false)
        {
            lblme1returnlasthrs.CssClass = "input";
            lblme1returnlasthrs.ReadOnly = false;
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
            lblme1returnlasthrs.ReadOnly = true;
        }
        if (chkMEPortreturnFlowDetective.Checked == false)
        {
            lblme1returnlasthrs.CssClass = "input";
            lblme1returnlasthrs.ReadOnly = false;
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
            txtme2lasthrs.ReadOnly = true;
            txtme2lasthrs.Text = "";
        }
        if (chkMEStbdNoFM.Checked == false)
        {
            txtme2lasthrs.CssClass = "input";
            txtme2lasthrs.ReadOnly = false;
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
            lblme2returnlasthrs.ReadOnly = true;
            lblme2returnlasthrs.Text = "";
        }
        if (chkMEStbdreturnNoFM.Checked == false)
        {
            lblme2returnlasthrs.CssClass = "input";
            lblme2returnlasthrs.ReadOnly = false;
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
            lblme2returnlasthrs.ReadOnly = true;

        }
        if (chkMEStbdreturnFlowDetective.Checked == false)
        {
            lblme2returnlasthrs.CssClass = "input";
            lblme2returnlasthrs.ReadOnly = false;
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
            txtAE1lasthrs.ReadOnly = true;
            txtAE1lasthrs.Text = "";
        }
        if (chkAE1NoFM.Checked == false)
        {
            txtAE1lasthrs.CssClass = "input";
            txtAE1lasthrs.ReadOnly = false;
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
            txtAE2lasthrs.ReadOnly = true;
            txtAE2lasthrs.Text = "";
        }
        if (chkAE2NoFM.Checked == false)
        {
            txtAE2lasthrs.CssClass = "input";
            txtAE2lasthrs.ReadOnly = false;
            chkAE2FlowDetective.Enabled = true;
        }
    }

    private void BindWorkOrder()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportWorkOrder(int.Parse(ViewState["VESSELID"].ToString())
                , DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvWorkOrder.DataSource = ds;
            //gvWorkOrder.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvWorkOrder_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            DataRowView drv = (DataRowView)e.Row.DataItem;

    //            LinkButton lblWorkOrderNo = (LinkButton)e.Row.FindControl("lblWorkOrderNo");
    //            RadLabel lblWorkOrderId = (RadLabel)e.Row.FindControl("lblWorkOrderId");
    //            lblWorkOrderNo.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + lblWorkOrderId.Text + "'); return false;");

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void BindLaggingIndicators()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportLaggingIndicators(int.Parse(ViewState["VESSELID"].ToString())
                , DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));
            gvLaggingIndicators.DataSource = ds;
           // gvLaggingIndicators.DataBind();


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
                //RadLabel lblReferenceId = (RadLabel)e.Row.FindControl("lblReferenceId");
                //Filter.CurrentIncidentID = lblReferenceId.Text;
                //Filter.CurrentIncidentTab = "INCIDENTDETAILS";

                //if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                //{
                //    ucError.HeaderMessage = "Please provide the following required information";
                //    ucError.ErrorMessage = "Select the Vessel Status";
                //    ucError.Visible = true;
                //    return;
                //}

                //SaveMidnightReport(0);
                //lblReferenceNumber.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx'); return false;");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvLaggingIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
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

    //            SaveMidnightReport(0);

    //            LinkButton lblReferenceNumber = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblReferenceNumber");
    //            RadLabel lblReferenceId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblReferenceId");
    //            Filter.CurrentIncidentID = lblReferenceId.Text;
    //            Filter.CurrentIncidentTab = "INCIDENTDETAILS";

    //            Response.Redirect("../Inspection/InspectionIncidentList.aspx", true);

    //            //lblReferenceNumber.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../Inspection/InspectionIncidentList.aspx'); return false;");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

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
                txtme1Total.ReadOnly = true;
            }
            else
            {
                txtme1Total.CssClass = "input";
                txtme1Total.ReadOnly = false;
            }
        }
        else
        {
            txtme1Total.CssClass = "input";
            txtme1Total.ReadOnly = false;
        }

        if (chkMEStbdNoFM.Checked == false)
        {
            if (chkMEStbdFlowDetective.Checked == false)
            {
                txtme2Total.CssClass = "readonlytextbox";
                txtme2Total.ReadOnly = true;
            }
            else
            {
                txtme2Total.CssClass = "input";
                txtme2Total.ReadOnly = false;
            }
        }
        else
        {
            txtme2Total.CssClass = "input";
            txtme2Total.ReadOnly = false;
        }

        if (chkAE1NoFM.Checked == false)
        {
            if (chkAE1FlowDetective.Checked == false)
            {
                txtAE1Consumption.CssClass = "readonlytextbox";
                txtAE1Consumption.ReadOnly = true;
            }
            else
            {
                txtAE1Consumption.CssClass = "input";
                txtAE1Consumption.ReadOnly = false;
            }
        }
        else
        {
            txtAE1Consumption.CssClass = "input";
            txtAE1Consumption.ReadOnly = false;
        }

        if (chkAE2NoFM.Checked == false)
        {
            if (chkAE2FlowDetective.Checked == false)
            {
                txtAE2Consumption.CssClass = "readonlytextbox";
                txtAE2Consumption.ReadOnly = true;
            }
            else
            {
                txtAE2Consumption.CssClass = "input";
                txtAE2Consumption.ReadOnly = false;
            }
        }
        else
        {
            txtAE2Consumption.CssClass = "input";
            txtAE2Consumption.ReadOnly = false;
        }


    }
    private void SaveMidnightReport(int confirm)
     {

        if (Session["MIDNIGHTREPORTID"] == null)
        {
            Guid? midnightreportid = null;
            string timeofetd = string.Empty;
            string timeofeta = string.Empty;
            string timeofarraival = string.Empty;
            string timeofdeparture = string.Empty;
            if (txtETDTime.SelectedTime != null) timeofetd = txtETDTime.SelectedTime.Value.ToString();
            if (txtETATime.SelectedTime != null) timeofeta = txtETATime.SelectedTime.Value.ToString();
            if (txtArrivalTime.SelectedTime != null) timeofarraival = txtArrivalTime.SelectedTime.Value.ToString();
            if (txtDepartureTime.SelectedTime != null) timeofdeparture = txtDepartureTime.SelectedTime.Value.ToString();
            // string timeofetd = (txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETDTime.Text;
            //string timeofeta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETATime.Text;

            string DNATestTime = (txtDNATime.Text.Trim() == "__:__") ? string.Empty : txtDNATime.Text;
            string TestTime = (DNATestTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : DNATestTime;

            //string timeofarrival = (txtArrivalTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtArrivalTime.Text;
            //string timeofdeparture = (txtDepartureTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtDepartureTime.Text;



            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportInsert(int.Parse(ViewState["VESSELID"].ToString())
                , DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                , txtSeaCondition.Text, General.GetNullableDecimal(ucSwell.Text), General.GetNullableGuid(ucWindDirection.SelectedDirection), 
                General.GetNullableDecimal(ucWindSpeed.Text), General.GetNullableDecimal(txtWaveHeight.Text), txtLocation.Text,
                General.GetNullableInteger(""), General.GetNullableDateTime(txtETDDate.Text + " " + timeofetd)
                , General.GetNullableInteger(""), General.GetNullableDateTime(txtETADate.Text + " " + timeofeta)
                , ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute, ucLatitude.TextSecond, 
                ucLongitude.TextSecond, ucLatitude.TextDirection, ucLongitude.TextDirection
                , General.GetNullableDecimal(ucAvgSpeed.Text), txtComments.Text, txtRemarks.Text, "", "", 
                General.GetNullableInteger(txtme1initialhrs.Text)
                , General.GetNullableInteger(txtme1lasthrs.Text), General.GetNullableInteger(lblme1returninitialhrs.Text), 
                General.GetNullableInteger(lblme1returnlasthrs.Text)
                , General.GetNullableInteger(txtme2initialhrs.Text), General.GetNullableInteger(txtme2lasthrs.Text), 
                General.GetNullableInteger(lblme2returninitialhrs.Text)
                , General.GetNullableInteger(lblme2returnlasthrs.Text), General.GetNullableInteger(txtAE1Consumption.Text), 
                General.GetNullableInteger(txtAE2Consumption.Text)
                , General.GetNullableDecimal(ucMEPort.Text), General.GetNullableDecimal(ucMEStbd.Text), General.GetNullableDecimal(ucAEI.Text), General.GetNullableDecimal(ucAEII.Text)
                , General.GetNullableDecimal(ucBT1.Text), General.GetNullableDecimal(ucBT2.Text), General.GetNullableDecimal(ucST1.Text), General.GetNullableDecimal(ucST2.Text)
                , General.GetNullableDecimal(ucFord.Text), General.GetNullableDecimal(ucMidship.Text), General.GetNullableDecimal(ucAft.Text), General.GetNullableDecimal(ucAverage.Text)
                , General.GetNullableInteger(ucBreakfast.Text), General.GetNullableInteger(ucLunch.Text), General.GetNullableInteger(ucDinner.Text), General.GetNullableInteger(ucSupper.Text)
                , General.GetNullableString(txtMaster.Text), General.GetNullableString(txtClient.Text)
                , General.GetNullableInteger(txtPOB.Text), General.GetNullableInteger(txtCrew.Text), General.GetNullableInteger(txtPOBMarineCrew.Text)
                , General.GetNullableInteger(txtCrewOff.Text), General.GetNullableInteger(txtCrewOn.Text), General.GetNullableInteger(txtPOBCrew.Text)
                , General.GetNullableInteger(txtPOBClient.Text), General.GetNullableInteger(txtPOBService.Text), General.GetNullableInteger(txtTotalOB.Text)
                , General.GetNullableGuid(txtVoyageId.Text), General.GetNullableGuid(""), General.GetNullableGuid("")
                , General.GetNullableGuid(ddlETALocation.SelectedValue), General.GetNullableGuid(ddlETDLocation.SelectedValue), ref midnightreportid
                , int.Parse(rblSatC.SelectedValue), int.Parse(rblCCTV.SelectedValue), int.Parse(rblHiPap.SelectedValue), General.GetNullableString(txtGeneralRemarks.Text)
                , General.GetNullableString(txtCrewRemarks.Text), General.GetNullableString(txtFlowmeterRemarks.Text), General.GetNullableString(txtMeterologyRemarks.Text)
                , General.GetNullableString(txtVesselMovementsRemarks.Text), General.GetNullableString(txtLookAheadRemarks.Text), General.GetNullableString(txtbulkRemarks.Text), confirm
                , General.GetNullableInteger(ucPort.SelectedValue), chkDNATest.Checked ==true ? 1 : 0, General.GetNullableDateTime(txtDate.Text.ToString() + " " + TestTime)
                , General.GetNullableString(txtDNARemarks.Text), General.GetNullableInteger(ddlvesselstatus.SelectedValue), General.GetNullableDecimal(txtDPTime.Text)
                , General.GetNullableDecimal(txtDPFuelConsumption.Text), General.GetNullableInteger(txtme1Total.Text), General.GetNullableInteger(txtme2Total.Text)
                , General.GetNullableInteger(txtAE1initialhrs.Text), General.GetNullableInteger(txtAE1lasthrs.Text), General.GetNullableInteger(txtAE2initialhrs.Text), General.GetNullableInteger(txtAE2lasthrs.Text)
                , General.GetNullableInteger(rbnhourchange.SelectedValue), General.GetNullableInteger(rbnhourvalue.SelectedValue)
                , General.GetNullableDateTime(txtArrivalDate.Text + " " + timeofarraival), General.GetNullableDateTime(txtDepartureDate.Text +" "+ timeofdeparture)
                , chkMEPortFlowDetective.Checked.Value ? 1 : 0, chkMEStbdFlowDetective.Checked.Value ? 1 : 0, chkAE1FlowDetective.Checked.Value ? 1 : 0, chkAE2FlowDetective.Checked.Value ? 1 : 0
                , txtSatCRemarks.Text, txtCCTVRemarks.Text, txtHiPapRemarks.Text
                , General.GetNullableInteger(ucTea1.Text), General.GetNullableInteger(ucTea2.Text), General.GetNullableInteger(ucSupper.Text)
                , General.GetNullableInteger(ucSupBreakFast.Text), General.GetNullableInteger(ucSupLunch.Text), General.GetNullableInteger(ucSupDinner.Text), General.GetNullableInteger(ucSupSupper.Text)
                , General.GetNullableInteger(ucSupTea1.Text), General.GetNullableInteger(ucSupTea2.Text), General.GetNullableInteger(ucotherConsumption.Text)
                , chkMEPortNoFM.Checked.Value ? 1 : 0, chkMEStbdNoFM.Checked.Value ? 1 : 0, chkMEPortreturnNoFM.Checked.Value ? 1 : 0, chkMEStbdreturnNoFM.Checked.Value ? 1 : 0
                , chkAE1NoFM.Checked.Value ? 1 : 0, chkAE2NoFM.Checked.Value ? 1 : 0, chkMEPortreturnFlowDetective.Checked.Value ? 1 : 0, chkMEStbdreturnFlowDetective.Checked.Value ? 1 : 0
                , General.GetNullableInteger(rblMachineryFailure.SelectedValue), General.GetNullableInteger(rblHSEIndicators.SelectedValue)
                , General.GetNullableDecimal(ucMEPortFirstRunHrs.Text), General.GetNullableDecimal(ucMEStbdFirstRunHrs.Text), General.GetNullableDecimal(ucAEIFirstRunHrs.Text), General.GetNullableDecimal(ucAEIIFirstRunHrs.Text)
                , General.GetNullableDecimal(ucBT1FirstRunHrs.Text), General.GetNullableDecimal(ucBT2FirstRunHrs.Text), General.GetNullableDecimal(ucST1FirstRunHrs.Text), General.GetNullableDecimal(ucST2FirstRunHrs.Text)
                , General.GetNullableDecimal(ucMEPortTotalRunHrs.Text), General.GetNullableDecimal(ucMEStbdTotalRunHrs.Text), General.GetNullableDecimal(ucAEITotalRunHrs.Text), General.GetNullableDecimal(ucAEIITotalRunHrs.Text)
                , General.GetNullableDecimal(ucBT1TotalRunHrs.Text), General.GetNullableDecimal(ucBT2TotalRunHrs.Text), General.GetNullableDecimal(ucST1TotalRunHrs.Text), General.GetNullableDecimal(ucST2TotalRunHrs.Text)
                , General.GetNullableInteger(ddlInstalationType.SelectedValue), txtDeckRemarks.Text, txtAnchorRemarks.Text, txtROVRemarks.Text, General.GetNullableInteger(ddlCrewListSOBYN.SelectedValue), txtCrewListSOBRemarks.Text);

            Session["MIDNIGHTREPORTID"] = midnightreportid;
            UpdateOperationalSummaryData();
            UpdateMeteorologyData();
            UpdateHseIndicatorData();
            UpdateBulks();
            UpdateLookAheadActivity();
            UpdatePMDOverDue();
            UpdateDeckCargo();
            UpdateDeckCargoSummary();
            UpdateROVoperation();
            UpdateAnchor();

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
            //(txtETDTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETDTime.SelectedTime.Value;
            //string timeofeta = (txtETATime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtETATime.Text;

            string DNATestTime = (txtDNATime.Text.Trim() == "__:__") ? string.Empty : txtDNATime.Text;
            string TestTime = (DNATestTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : DNATestTime;
            string s = timeofetd;
            // string timeofarrival = (txtArrivalTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtArrivalTime.Text;
            // string timeofdeparture = (txtDepartureTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtDepartureTime.Text;

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportUpdate(new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()), DateTime.Parse(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                , txtSeaCondition.Text, General.GetNullableDecimal(ucSwell.Text), General.GetNullableGuid(ucWindDirection.SelectedDirection), General.GetNullableDecimal(ucWindSpeed.Text), General.GetNullableDecimal(txtWaveHeight.Text), txtLocation.Text, General.GetNullableInteger(""), General.GetNullableDateTime(txtETDDate.Text + " " + timeofetd)
                , General.GetNullableInteger(""), General.GetNullableDateTime(txtETADate.Text + " " + timeofeta)
                , ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute, ucLatitude.TextSecond, ucLongitude.TextSecond, ucLatitude.TextDirection, ucLongitude.TextDirection
                , General.GetNullableDecimal(ucAvgSpeed.Text), txtComments.Text, txtRemarks.Text, "", "", General.GetNullableInteger(txtme1initialhrs.Text)
                , General.GetNullableInteger(txtme1lasthrs.Text), General.GetNullableInteger(lblme1returninitialhrs.Text), General.GetNullableInteger(lblme1returnlasthrs.Text)
                , General.GetNullableInteger(txtme2initialhrs.Text), General.GetNullableInteger(txtme2lasthrs.Text), General.GetNullableInteger(lblme2returninitialhrs.Text)
                , General.GetNullableInteger(lblme2returnlasthrs.Text), General.GetNullableInteger(txtAE1Consumption.Text), General.GetNullableInteger(txtAE2Consumption.Text)
                , General.GetNullableDecimal(ucMEPort.Text), General.GetNullableDecimal(ucMEStbd.Text), General.GetNullableDecimal(ucAEI.Text), General.GetNullableDecimal(ucAEII.Text)
                , General.GetNullableDecimal(ucBT1.Text), General.GetNullableDecimal(ucBT2.Text), General.GetNullableDecimal(ucST1.Text), General.GetNullableDecimal(ucST2.Text)
                , General.GetNullableDecimal(ucFord.Text), General.GetNullableDecimal(ucMidship.Text), General.GetNullableDecimal(ucAft.Text), General.GetNullableDecimal(ucAverage.Text)
                , General.GetNullableInteger(ucBreakfast.Text), General.GetNullableInteger(ucLunch.Text), General.GetNullableInteger(ucDinner.Text), General.GetNullableInteger(ucSupper.Text)
                , General.GetNullableString(txtMaster.Text), General.GetNullableString(txtClient.Text)
                , General.GetNullableInteger(txtPOB.Text), General.GetNullableInteger(txtCrew.Text), General.GetNullableInteger(txtPOBMarineCrew.Text)
                , General.GetNullableInteger(txtCrewOff.Text), General.GetNullableInteger(txtCrewOn.Text), General.GetNullableInteger(txtPOBCrew.Text)
                , General.GetNullableInteger(txtPOBClient.Text), General.GetNullableInteger(txtPOBService.Text), General.GetNullableInteger(txtTotalOB.Text)
                , General.GetNullableGuid(txtVoyageId.Text), General.GetNullableGuid(""), General.GetNullableGuid("")
                , General.GetNullableGuid(ddlETALocation.SelectedValue), General.GetNullableGuid(ddlETDLocation.SelectedValue)
                , int.Parse(rblSatC.SelectedValue), int.Parse(rblCCTV.SelectedValue), int.Parse(rblHiPap.SelectedValue), General.GetNullableString(txtGeneralRemarks.Text)
                , General.GetNullableString(txtCrewRemarks.Text), General.GetNullableString(txtFlowmeterRemarks.Text), General.GetNullableString(txtMeterologyRemarks.Text)
                , General.GetNullableString(txtVesselMovementsRemarks.Text), General.GetNullableString(txtLookAheadRemarks.Text), General.GetNullableString(txtbulkRemarks.Text), confirm
                , General.GetNullableInteger(ucPort.SelectedValue), chkDNATest.Checked.Value ? 1 : 0, General.GetNullableDateTime(ViewState["REPORTDATE"].ToString() + " " + TestTime)
                , General.GetNullableString(txtDNARemarks.Text), General.GetNullableInteger(ddlvesselstatus.SelectedValue), General.GetNullableDecimal(txtDPTime.Text)
                , General.GetNullableDecimal(txtDPFuelConsumption.Text), General.GetNullableInteger(txtme1Total.Text), General.GetNullableInteger(txtme2Total.Text)
                , General.GetNullableInteger(txtAE1initialhrs.Text), General.GetNullableInteger(txtAE1lasthrs.Text), General.GetNullableInteger(txtAE2initialhrs.Text), General.GetNullableInteger(txtAE2lasthrs.Text)
                , int.Parse(rbnhourchange.SelectedValue == null ? "0" : rbnhourchange.SelectedValue), General.GetNullableInteger(rbnhourvalue.SelectedValue)
                , General.GetNullableDateTime(txtArrivalDate.Text + " " + timeofarraival), General.GetNullableDateTime(txtDepartureDate.Text +" "+timeofdeparture)
                , chkMEPortFlowDetective.Checked.Value ? 1 : 0, chkMEStbdFlowDetective.Checked.Value ? 1 : 0, chkAE1FlowDetective.Checked.Value ? 1 : 0, chkAE2FlowDetective.Checked.Value ? 1 : 0
                , txtSatCRemarks.Text, txtCCTVRemarks.Text, txtHiPapRemarks.Text
                , General.GetNullableInteger(ucTea1.Text), General.GetNullableInteger(ucTea2.Text), General.GetNullableInteger(ucSupper.Text)
                , General.GetNullableInteger(ucSupBreakFast.Text), General.GetNullableInteger(ucSupLunch.Text), General.GetNullableInteger(ucSupDinner.Text), General.GetNullableInteger(ucSupSupper.Text)
                , General.GetNullableInteger(ucSupTea1.Text), General.GetNullableInteger(ucSupTea2.Text), General.GetNullableInteger(ucotherConsumption.Text)
                , chkMEPortNoFM.Checked.Value ? 1 : 0, chkMEStbdNoFM.Checked.Value ? 1 : 0, chkMEPortreturnNoFM.Checked.Value ? 1 : 0, chkMEStbdreturnNoFM.Checked.Value ? 1 : 0
                , chkAE1NoFM.Checked.Value ? 1 : 0, chkAE2NoFM.Checked.Value ? 1 : 0, chkMEPortreturnFlowDetective.Checked.Value ? 1 : 0, chkMEStbdreturnFlowDetective.Checked.Value ? 1 : 0
                , General.GetNullableInteger(rblMachineryFailure.SelectedValue), General.GetNullableInteger(rblHSEIndicators.SelectedValue)
                , General.GetNullableDecimal(ucMEPortFirstRunHrs.Text), General.GetNullableDecimal(ucMEStbdFirstRunHrs.Text), General.GetNullableDecimal(ucAEIFirstRunHrs.Text), General.GetNullableDecimal(ucAEIIFirstRunHrs.Text)
                , General.GetNullableDecimal(ucBT1FirstRunHrs.Text), General.GetNullableDecimal(ucBT2FirstRunHrs.Text), General.GetNullableDecimal(ucST1FirstRunHrs.Text), General.GetNullableDecimal(ucST2FirstRunHrs.Text)
                , General.GetNullableDecimal(ucMEPortTotalRunHrs.Text), General.GetNullableDecimal(ucMEStbdTotalRunHrs.Text), General.GetNullableDecimal(ucAEITotalRunHrs.Text), General.GetNullableDecimal(ucAEIITotalRunHrs.Text)
                , General.GetNullableDecimal(ucBT1TotalRunHrs.Text), General.GetNullableDecimal(ucBT2TotalRunHrs.Text), General.GetNullableDecimal(ucST1TotalRunHrs.Text), General.GetNullableDecimal(ucST2TotalRunHrs.Text)
                , General.GetNullableInteger(ddlInstalationType.SelectedValue), txtDeckRemarks.Text, txtAnchorRemarks.Text, txtROVRemarks.Text
                , General.GetNullableInteger(chkFireDrillYN.Checked == true ? "1" : "0")
                , General.GetNullableInteger(ViewState["REACTIVATED"] == null ? null : ViewState["REACTIVATED"].ToString())
                , General.GetNullableInteger(ddlCrewListSOBYN.SelectedValue), txtCrewListSOBRemarks.Text);

            UpdateMeteorologyData();
            UpdateOperationalSummaryData();
            UpdateHseIndicatorData();
            UpdateBulks();
            UpdateLookAheadActivity();
            UpdatePMDOverDue();
            UpdateDeckCargo();
            UpdateDeckCargoSummary();
            UpdateROVoperation();
            UpdateAnchor();

            if (confirm == 0)
                ucStatus.Text = "MidNight Report Updated";
            if (confirm == 1)
                ucStatus.Text = "MidNight Report Updated.</br>Generate export after 5 min.";
        }
        if (confirm == 1)
        {
            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreActivitityUpdate(new Guid(Session["MIDNIGHTREPORTID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
            //GeneratePDF();
        }

        if (Session["MIDNIGHTREPORTID"] != null)
        {
            EditMidNightReport();
            BindMeteorologyData();
            BindOilLoadandConsumption();
            EditFlowMeterReading();
            BindOperationalTimeSummary();
            SetFieldRange();
            BindPlannedActivity();
            BindVesselMovements();
            BindDeckCargo();
        }
    }
    protected void MenuTabSaveHSEIndicators_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int confirm;
                confirm = 0;
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport(confirm);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTabSaveVesselMovements_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int confirm;
                confirm = 0;
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport(confirm);
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int confirm;
                confirm = 0;
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport(confirm);
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
                int confirm;
                confirm = 0;
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport(confirm);
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int confirm;
                confirm = 0;
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport(confirm);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTabSavePassenger_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int confirm;
                confirm = 0;
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport(confirm);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                    txtme1initialhrs.ReadOnly = false;
                    txtme1initialhrs.CssClass = "input";
                }
                else
                {
                    txtme1initialhrs.ReadOnly = true;
                    txtme1initialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDMEPORTRETURNFLOWDEFECTIVE"].ToString() == "1" && chkMEPortreturnFlowDetective.Checked == false)
                {
                    lblme1returninitialhrs.ReadOnly = false;
                    lblme1returninitialhrs.CssClass = "input";
                }
                else
                {
                    lblme1returninitialhrs.ReadOnly = true;
                    lblme1returninitialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDMESTBDFLOWDEFECTIVE"].ToString() == "1" && chkMEStbdFlowDetective.Checked == false)
                {
                    txtme2initialhrs.ReadOnly = false;
                    txtme2initialhrs.CssClass = "input";
                }
                else
                {
                    txtme2initialhrs.ReadOnly = true;
                    txtme2initialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDMESTBDRETURNFLOWDEFECTIVE"].ToString() == "1" && chkMEStbdreturnFlowDetective.Checked == false)
                {
                    lblme2returninitialhrs.ReadOnly = false;
                    lblme2returninitialhrs.CssClass = "input";
                }
                else
                {
                    lblme2returninitialhrs.ReadOnly = true;
                    lblme2returninitialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDAE1FLOWDEFECTIVE"].ToString() == "1" && chkAE1FlowDetective.Checked == false)
                {
                    txtAE1initialhrs.ReadOnly = false;
                    txtAE1initialhrs.CssClass = "input";
                }
                else
                {
                    txtAE1initialhrs.ReadOnly = true;
                    txtAE1initialhrs.CssClass = "readonlytextbox";
                }
                if (ds.Tables[0].Rows[0]["FLDAE2FLOWDEFECTIVE"].ToString() == "1" && chkAE2FlowDetective.Checked == false)
                {
                    txtAE2initialhrs.ReadOnly = false;
                    txtAE2initialhrs.CssClass = "input";
                }
                else
                {
                    txtAE2initialhrs.ReadOnly = true;
                    txtAE2initialhrs.CssClass = "readonlytextbox";
                }

            }
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

            SaveMidnightReport(0);
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

            SaveMidnightReport(0);

            String scriptpopup = String.Format(
               "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsConditionsAdd.aspx');");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            //toolbarhse.AddImageLink("javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionIncidentAdd.aspx?module=DMR&date=" + txtDate.Text + "')", "Add", "add.png", "ADD");
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
                    txtme1initialhrs.ReadOnly = false;
                }
                if (chkMEPortFlowDetective.Checked == true)
                {
                    txtme1initialhrs.CssClass = "readonlytextbox";
                    txtme1initialhrs.ReadOnly = true;
                }
            }
            if (chkMEPortNoFM.Checked == true)
            {
                txtme1initialhrs.CssClass = "readonlytextbox";
                txtme1initialhrs.ReadOnly = true;
            }

            if (chkMEPortreturnNoFM.Checked == false)
            {
                if (chkMEPortreturnFlowDetective.Checked == false)
                {
                    lblme1returninitialhrs.CssClass = "input";
                    lblme1returninitialhrs.ReadOnly = false;
                }
                if (chkMEPortreturnFlowDetective.Checked == true)
                {
                    lblme1returninitialhrs.CssClass = "readonlytextbox";
                    lblme1returninitialhrs.ReadOnly = true;
                }
            }
            if (chkMEPortreturnNoFM.Checked == true)
            {
                lblme1returninitialhrs.CssClass = "readonlytextbox";
                lblme1returninitialhrs.ReadOnly = true;
            }

            if (chkMEStbdNoFM.Checked == false)
            {
                if (chkMEStbdFlowDetective.Checked == false)
                {
                    txtme2initialhrs.CssClass = "input";
                    txtme2initialhrs.ReadOnly = false;
                }
                if (chkMEStbdFlowDetective.Checked == true)
                {
                    txtme2initialhrs.CssClass = "readonlytextbox";
                    txtme2initialhrs.ReadOnly = true;
                }
            }
            if (chkMEStbdNoFM.Checked == true)
            {
                txtme2initialhrs.CssClass = "readonlytextbox";
                txtme2initialhrs.ReadOnly = true;
            }
            if (chkMEStbdreturnNoFM.Checked == false)
            {
                if (chkMEStbdreturnFlowDetective.Checked == false)
                {
                    lblme2returninitialhrs.CssClass = "input";
                    lblme2returninitialhrs.ReadOnly = false;
                }
                if (chkMEStbdreturnFlowDetective.Checked == true)
                {
                    lblme2returninitialhrs.CssClass = "readonlytextbox";
                    lblme2returninitialhrs.ReadOnly = true;
                }
            }
            if (chkMEStbdreturnNoFM.Checked == true)
            {
                lblme2returninitialhrs.CssClass = "readonlytextbox";
                lblme2returninitialhrs.ReadOnly = true;
            }
            if (chkAE1NoFM.Checked == false)
            {
                if (chkAE1FlowDetective.Checked == false)
                {
                    txtAE1initialhrs.CssClass = "input";
                    txtAE1initialhrs.ReadOnly = false;
                }
                if (chkAE1FlowDetective.Checked == true)
                {
                    txtAE1initialhrs.CssClass = "readonlytextbox";
                    txtAE1initialhrs.ReadOnly = true;
                }
            }
            if (chkAE1NoFM.Checked == true)
            {
                txtAE1initialhrs.CssClass = "readonlytextbox";
                txtAE1initialhrs.ReadOnly = true;
            }
            if (chkAE2NoFM.Checked == false)
            {
                if (chkAE2FlowDetective.Checked == false)
                {
                    txtAE2initialhrs.CssClass = "input";
                    txtAE2initialhrs.ReadOnly = false;
                }
                if (chkAE2FlowDetective.Checked == true)
                {
                    txtAE2initialhrs.CssClass = "readonlytextbox";
                    txtAE2initialhrs.ReadOnly = true;
                }
            }
            if (chkAE2NoFM.Checked == true)
            {
                txtAE2initialhrs.CssClass = "readonlytextbox";
                txtAE2initialhrs.ReadOnly = true;
            }
        }
    }
    //protected void gvCertificates_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        RadLabel expdate = (RadLabel)e.Row.FindControl("lblExpiryDate");
    //        Image imgFlag = (Image)e.Row.FindControl("imgFlag");
    //        DateTime? d = General.GetNullableDateTime(expdate.Text);
    //        if (d.HasValue)
    //        {
    //            TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
    //            if (t.Days <= 15)
    //            {
    //                //e.Row.CssClass = "rowyellow";
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
    //            }
    //            if (t.Days > 15 && t.Days <= 30)
    //            {
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
    //            }
    //            if (t.Days <= 0)
    //            {
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
    //            }
    //        }
    //    }
    //}
    //protected void gvShipAudit_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        RadLabel expdate = (RadLabel)e.Row.FindControl("lblDueDate");
    //        Image imgFlag = (Image)e.Row.FindControl("imgFlag");
    //        DateTime? d = General.GetNullableDateTime(expdate.Text);
    //        if (d.HasValue)
    //        {
    //            TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
    //            if (t.Days <= 15)
    //            {
    //                //e.Row.CssClass = "rowyellow";
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
    //            }
    //            if (t.Days > 15 && t.Days <= 30)
    //            {
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
    //            }
    //            if (t.Days <= 0)
    //            {
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
    //            }
    //        }
    //    }
    //}
    //protected void gvShipTasks_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        RadLabel expdate = (RadLabel)e.Row.FindControl("lblDueDate");
    //        Image imgFlag = (Image)e.Row.FindControl("imgFlag");
    //        DateTime? d = General.GetNullableDateTime(expdate.Text);
    //        if (d.HasValue)
    //        {
    //            TimeSpan t = d.Value - Convert.ToDateTime(txtDate.Text);
    //            if (t.Days <= 15)
    //            {
    //                //e.Row.CssClass = "rowyellow";
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/ORANGE-symbol.png";
    //            }
    //            if (t.Days > 15 && t.Days <= 30)
    //            {
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
    //            }
    //            if (t.Days <= 0)
    //            {
    //                imgFlag.Visible = true;
    //                imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
    //            }
    //        }
    //    }
    //}
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

    //            SaveMidnightReport(0);

    //            LinkButton lblUnsafeActs = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lblUnsafeActs");
    //            RadLabel lblDirectIncidentId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDirectIncidentId");

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
    private void BindPMSoverdue()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportPMSOverdue(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableDateTime(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text)
                , General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            gvPMSoverdue.DataSource = ds;
           // gvPMSoverdue.DataBind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvPMSoverdue_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //        DataRowView drv = (DataRowView)e.Row.DataItem;

    //        LinkButton lnkCount = (LinkButton)e.Row.FindControl("lnkCount");
    //        RadLabel lblType = (RadLabel)e.Row.FindControl("lblType");
    //        lnkCount.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','../CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

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

    private void BindDeckCargo()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRDeckCargoList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString())
                , General.GetNullableDateTime(txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text));

            gvDeckCargo.DataSource = ds;
          //  gvDeckCargo.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateDeckCargo()
    {
        foreach (GridDataItem gvr in gvDeckCargo.Items)
        {

            RadLabel lblDMRItemId = (RadLabel)gvr.FindControl("lblDMRItemId");
            RadLabel lblItemId = (RadLabel)gvr.FindControl("lblItemId");
            RadLabel lblUnitId = (RadLabel)gvr.FindControl("lblUnitId");
            UserControlMaskNumber txtNoFOUnitLoaded = (UserControlMaskNumber)gvr.FindControl("txtNoFOUnitLoaded");
            UserControlMaskNumber txtWtloadedInMT = (UserControlMaskNumber)gvr.FindControl("txtWtloadedInMT");
            UserControlMaskNumber txtNoFOUnitDischarged = (UserControlMaskNumber)gvr.FindControl("txtNoFOUnitDischarged");
            UserControlMaskNumber txtWtDischargedInMT = (UserControlMaskNumber)gvr.FindControl("txtWtDischargedInMT");
            RadLabel lblPreviousROB = (RadLabel)gvr.FindControl("lblPreviousROB");
            RadLabel lblPreviousQuantity = (RadLabel)gvr.FindControl("lblPreviousQuantity");

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRDeckCargoUpdate(General.GetNullableGuid(lblDMRItemId.Text)
                , new Guid(lblItemId.Text)
                , new Guid(Session["MIDNIGHTREPORTID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString())
                , General.GetNullableInteger(lblUnitId.Text)
                , General.GetNullableDecimal(txtNoFOUnitLoaded.Text)
                , General.GetNullableDecimal(txtWtloadedInMT.Text)
                , General.GetNullableDecimal(txtNoFOUnitDischarged.Text)
                , General.GetNullableDecimal(txtWtDischargedInMT.Text)
                , General.GetNullableDecimal(lblPreviousROB.Text)
                 , General.GetNullableDecimal(lblPreviousQuantity.Text));
        }


    }
    //protected void gvDeckCargo_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //}

    private void BindDeckCargoSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRDeckCargoSummaryList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            gvDeckCargoSummary.DataSource = ds;
            //gvDeckCargoSummary.DataBind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateDeckCargoSummary()
    {
        foreach (GridDataItem gvr in gvDeckCargoSummary.Items)
        {

            RadLabel lblOperationName = (RadLabel)gvr.FindControl("lblOperationName");
            RadLabel lblOperationType = (RadLabel)gvr.FindControl("lblOperationType");
            RadLabel lblSummaryId = (RadLabel)gvr.FindControl("lblSummaryId");
            RadLabel lblValue = (RadLabel)gvr.FindControl("lblValue");

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRDeckCargoSummaryUpdate(General.GetNullableGuid(lblSummaryId.Text)
                , new Guid(Session["MIDNIGHTREPORTID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString())
                , int.Parse(lblOperationType.Text)
                , lblOperationName.Text
                , General.GetNullableDecimal(lblValue.Text));
            BindDeckCargoSummary();


        }
    }
    //protected void gvDeckCargoSummary_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {


    //    }
    //}
    //protected void gvROVoperation_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //        DataRowView drv = (DataRowView)e.Row.DataItem;

    //        RadLabel lblValueType = (RadLabel)e.Row.FindControl("lblValueType");
    //        RadLabel lblValue = (RadLabel)e.Row.FindControl("lblValue");
    //        CheckBoxList chkValue = (CheckBoxList)e.Row.FindControl("chkValue");
    //        TextBox txtValueDecimal = (TextBox)e.Row.FindControl("txtValueDecimal");

    //        if (lblValueType.Text == "1")
    //        {
    //            txtValueDecimal.Visible = false;
    //            chkValue.Visible = true;

    //            int iRowCount = 0;
    //            int iTotalPageCount = 0;
    //            DataSet ds = PhoenixRegistersDMRRovType.MDRRovTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
    //            DataTable dt = ds.Tables[0];
    //            chkValue.DataSource = dt;
    //            chkValue.DataBind();

    //            if (chkValue != null)
    //            {
    //                foreach (ListItem item in chkValue.Items)
    //                {
    //                    item.Selected = false;
    //                    if (!string.IsNullOrEmpty(lblValue.Text) && ("," + lblValue.Text + ",").Contains("," + item.Value.ToString() + ","))
    //                        item.Selected = true;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            txtValueDecimal.Visible = true;
    //            chkValue.Visible = false;
    //            txtValueDecimal.Text = lblValue.Text;
    //        }
    //    }
    //}

    private void BindROVoperation()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRRovOperationList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            gvROVoperation.DataSource = ds;
            //gvROVoperation.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateROVoperation()
    {
        foreach (GridDataItem gvr in gvROVoperation.Items)
        {
            RadLabel lblItem = (RadLabel)gvr.FindControl("lblItem");
            RadLabel lblROVOperationId = (RadLabel)gvr.FindControl("lblROVOperationId");
            RadLabel lblROVOperationName = (RadLabel)gvr.FindControl("lblROVOperationName");
            RadLabel lblValueType = (RadLabel)gvr.FindControl("lblValueType");
            RadTextBox txtValueDecimal = (RadTextBox)gvr.FindControl("txtValueDecimal");
            RadCheckBoxList chkValue = (RadCheckBoxList)gvr.FindControl("chkValue");

            string value = "";
            if (lblValueType.Text == "1")
            {
                foreach (ButtonListItem item in chkValue.Items)
                {
                    if (item.Selected)
                    {
                        value = value + item.Value + ",";
                    }
                }
            }
            if (lblValueType.Text == "2")
            {
                value = txtValueDecimal.Text;
            }

            PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRRovOperationUpdate(General.GetNullableGuid(lblROVOperationId.Text)
                , new Guid(Session["MIDNIGHTREPORTID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString())
                , int.Parse(lblROVOperationName.Text)
                , lblItem.Text
                , value
                , int.Parse(lblValueType.Text));
        }


    }

    //protected void gvAnchor_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {

    //            DataRowView drv = (DataRowView)e.Row.DataItem;

    //            RadLabel lblValueType = (RadLabel)e.Row.FindControl("lblValueType");
    //            RadLabel lblValue = (RadLabel)e.Row.FindControl("lblValue");
    //            RadLabel lblValue2 = (RadLabel)e.Row.FindControl("lblValue2");
    //            RadLabel lblSize = (RadLabel)e.Row.FindControl("lblSize");
    //            RadLabel lblLength = (RadLabel)e.Row.FindControl("lblLength");
    //            RadCheckBoxList chkValue = (RadCheckBoxList)e.Row.FindControl("chkValue");
    //            RadTextBox txtValueDecimal = (RadTextBox)e.Row.FindControl("txtValueDecimal");
    //            RadTextBox txtValueTextSize = (RadTextBox)e.Row.FindControl("txtValueTextSize");
    //            RadTextBox txtValueTextLen = (RadTextBox)e.Row.FindControl("txtValueTextLen");
    //            RadRadioButtonList rblValue = (RadRadioButtonList)e.Row.FindControl("rblValue");

    //            if (lblValueType.Text == "1")
    //            {
    //                txtValueDecimal.Visible = false;
    //                chkValue.Visible = true;
    //                txtValueTextSize.Visible = false;
    //                txtValueTextLen.Visible = false;
    //                rblValue.Visible = false;

    //                int iRowCount = 0;
    //                int iTotalPageCount = 0;
    //                DataSet ds = PhoenixRegistersDMRAnchorHandilingType.DMRAnchorHandilingTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
    //                DataTable dt = ds.Tables[0];
    //                chkValue.DataSource = dt;
    //                chkValue.DataBind();

    //                if (chkValue != null)
    //                {
    //                    foreach (ListItem item in chkValue.Items)
    //                    {
    //                        item.Selected = false;
    //                        if (!string.IsNullOrEmpty(lblValue.Text) && ("," + lblValue.Text + ",").Contains("," + item.Value.ToString() + ","))
    //                            item.Selected = true;
    //                    }
    //                }
    //            }
    //            else if (lblValueType.Text == "2")
    //            {
    //                txtValueDecimal.Visible = true;
    //                chkValue.Visible = false;
    //                txtValueTextSize.Visible = false;
    //                txtValueTextLen.Visible = false;
    //                rblValue.Visible = false;

    //                txtValueDecimal.Text = lblValue.Text;
    //            }
    //            else if (lblValueType.Text == "3")
    //            {
    //                txtValueDecimal.Visible = false;
    //                chkValue.Visible = false;
    //                txtValueTextSize.Visible = false;
    //                txtValueTextLen.Visible = false;
    //                rblValue.Visible = true;

    //                rblValue.SelectedValue = lblValue.Text;
    //            }
    //            else if (lblValueType.Text == "4")
    //            {
    //                txtValueDecimal.Visible = false;
    //                chkValue.Visible = false;
    //                txtValueTextSize.Visible = true;
    //                txtValueTextLen.Visible = true;
    //                rblValue.Visible = false;

    //                lblSize.Visible = true;
    //                lblLength.Visible = true;

    //                txtValueTextSize.Text = lblValue.Text;
    //                txtValueTextLen.Text = lblValue2.Text;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void BindAnchor()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRAnchorOperationList(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));
            gvAnchor.DataSource = ds;
          //  gvAnchor.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateAnchor()
    {
        foreach (GridDataItem gvr in gvAnchor.Items)
        {
            RadLabel lblItem = (RadLabel)gvr.FindControl("lblItem");
            RadLabel lblOperationId = (RadLabel)gvr.FindControl("lblOperationId");
            RadLabel lblOperationName = (RadLabel)gvr.FindControl("lblOperationName");
            RadLabel lblValueType = (RadLabel)gvr.FindControl("lblValueType");
            RadTextBox txtValueDecimal = (RadTextBox)gvr.FindControl("txtValueDecimal");
            RadCheckBoxList chkValue = (RadCheckBoxList)gvr.FindControl("chkValue");
            RadTextBox txtValueTextSize = (RadTextBox)gvr.FindControl("txtValueTextSize");
            RadTextBox txtValueTextLen = (RadTextBox)gvr.FindControl("txtValueTextLen");
            RadRadioButtonList rblValue = (RadRadioButtonList)gvr.FindControl("rblValue");

            string value = "";
            string value2 = "";
            if (lblValueType != null)
            {
                if (lblValueType.Text == "1")
                {
                    foreach (ButtonListItem item in chkValue.Items)
                    {
                        if (item.Selected)
                        {
                            value = value + item.Value + ",";
                        }
                    }
                }
                if (lblValueType.Text == "2")
                {
                    value = txtValueDecimal.Text;
                }
                if (lblValueType.Text == "3")
                {
                    value = rblValue.SelectedValue;
                }
                if (lblValueType.Text == "4")
                {
                    value = txtValueTextSize.Text;
                    value2 = txtValueTextLen.Text;
                }

                PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreDMRAnchorOperationUpdate(General.GetNullableGuid(lblOperationId.Text)
                    , new Guid(Session["MIDNIGHTREPORTID"].ToString())
                    , int.Parse(ViewState["VESSELID"].ToString())
                    , int.Parse(lblOperationName.Text)
                    , lblItem.Text
                    , value
                    , value2
                    , int.Parse(lblValueType.Text));
            }
        }

    }


    //Mailing DMR MidNight Report

    protected void GeneratePDF()
    {
        string[] _reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        string _filename = "";
        DataSet ds = new DataSet();

        string date = txtDate.Text == null ? String.Format("{0:dd/MM/yyyy}", System.DateTime.Now) : txtDate.Text;
        string VesselName = ucVessel.SelectedVesselName;

        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("applicationcode", "11");
        nvc.Add("reportcode", "DMRMIDNIGHTREPORTMAIL");
        nvc.Add("midNightReportId", Session["MIDNIGHTREPORTID"].ToString());
        nvc.Add("VesselID", ViewState["VESSELID"].ToString());
        nvc.Add("date", date);
        nvc.Add("VesselName", VesselName);

        Session["PHOENIXREPORTPARAMETERS"] = nvc;
        PhoenixReportClass rpt = new PhoenixReportClass();
        ds = PhoenixReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);

        _reportfile = new string[19];
        _reportfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportsDMRMidnightGeneralMail.rpt");
        _reportfile[1] = "ReportsDMRMidNightHSEIndicators.rpt";
        _reportfile[2] = "ReportsDMRMidNightFOFlowmeterReading.rpt";
        _reportfile[3] = "ReportsDMRMidNightMeteorologyForVDM.rpt";
        _reportfile[4] = "ReportsDMRMidNightVesselsMovements.rpt";
        _reportfile[5] = "ReportsDMRMidNightOperationalSummary.rpt";
        _reportfile[6] = "ReportsDMRMidNightPlannedActivity.rpt";
        _reportfile[7] = "ReportsDMRMidNightBulkForVDM.rpt";
        _reportfile[8] = "ReportsDMRMidNightShipsCrew.rpt";
        _reportfile[9] = "ReportsDMRMidNightRequisitionAndPO.rpt";
        _reportfile[10] = "ReportsDMRMidNightShipAudit.rpt";
        _reportfile[11] = "ReportsDMRMidNightShipTask.rpt";
        _reportfile[12] = "ReportsDMRMidNightCertificatesDue.rpt";
        _reportfile[13] = "ReportsDMRMidNightUnsafeActs.rpt";
        _reportfile[14] = "ReportsDMRMidNightPassengersList.rpt";
        _reportfile[15] = "ReportsDMRMidNightMachineryFailures.rpt";
        _reportfile[16] = "ReportsDMRMidNightExternalInspections.rpt";
        _reportfile[17] = "ReportsDMRMidNightTankPlanSub.rpt";
        _reportfile[18] = "ReportsDMRMidNightLaggingIncidents.rpt";

        //string filename = Path.GetFileName(_filename);
        //string newfullfilename = Path.GetDirectoryName(filename) + ".pdf";
        PhoenixReportClass.ExportReport(_reportfile, _filename, ds);
        //Response.Redirect("ReportsDownload.aspx?filename=" + _filename + "&type=pdf", false);

        PhoenixReportsCommon.SendMail(nvc, ds, _filename);

    }

    protected void ucVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        SwitchSearch();
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        SwitchSearch();
    }


    private void SwitchSearch()
    {
        DataTable dt = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportByVesselAndDate(int.Parse(ucVessel.SelectedVessel), DateTime.Parse(txtDate.Text));
        if (dt.Rows.Count > 0)
        {
            Session["MIDNIGHTREPORTID"] = dt.Rows[0][0].ToString();
            EditMidNightReport();
            EditFlowMeterReading();
            BindOperationalTimeSummary();

            lnkFuelConsShowGraph.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','CrewOffshoreDMRMonthlyDPOperations.aspx?ReportID=" + General.GetNullableString(ViewState["MonthlyReportId"].ToString()) + "&Source=midnightreport'); return false;");
            BindRequisionsandPO();
            BindDueAudits();
            BindDueShipTasks();
            BindDueCertificates();
            BindUnsafeActs();
            BindMachineryFailures();
            BindExternalInspection();
            BindOilLoadandConsumption();
            BindMeteorologyData();
            BindPlannedActivity();
            BindVesselMovements();
            BindCrewData();
            BindHSEIndicators();
            BindWorkOrder();
            BindLaggingIndicators();
            BindPMSoverdue();
            BindDeckCargo();
            BindDeckCargoSummary();
            BindROVoperation();
            BindAnchor();
            //BindPassenger();
            //SetPageNavigator();
            ChangeVesselStatus();
            ChangeFlowmeterTotalEnabedYN();
            CheckPrevioueDefectiveFM();
            ChangeNoFlowmeterEnabedYN(General.GetNullableGuid(ViewState["PREVIOUSEREPORTID"].ToString()));
            SetFieldRange();
        }
        else
        {
            ucError.Text = "No Report found for selected vessel and seleced date";
            ucError.Visible = true;
        }
    }

    protected void MenuTabShipCrew_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int confirm;
                confirm = 0;
                if (General.GetNullableInteger(ddlvesselstatus.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Status";
                    ucError.Visible = true;
                    return;
                }
                SaveMidnightReport(confirm);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    //telerik

    protected void gvVesselMovements_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindVesselMovements();

    }
    protected void ddlvesselstatus_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {

        txtETADate.Text = "";
        txtETATime.SelectedTime = null;// = "";       
        txtETDDate.Text = "";
        txtETDTime.SelectedTime = null;
        txtArrivalDate.Text = "";
        txtArrivalTime.SelectedTime = null;
        txtDepartureDate.Text = "";
        txtDepartureTime.SelectedTime = null;

        ChangeVesselStatus();
    }

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

    protected void gvVesselMovements_InsertCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if (Session["MIDNIGHTREPORTID"] == null)
            {
                ucError.ErrorMessage = "You cannot save. Please save the header details first.";
                ucError.Visible = true;
                return;
            }
            var editableItem = ((GridEditableItem)e.Item);
            string FromTime = (((UserControlMaskedTextBox)editableItem.FindControl("txtFromTime")).TextWithLiterals.Trim() == "__:__") ? string.Empty : ((UserControlMaskedTextBox)editableItem.FindControl("txtFromTime")).TextWithLiterals;
            string Activity = ((RadTextBox)editableItem.FindControl("txtDescriptionEdit")).Text;
            string ddlActivity = ((RadComboBox)editableItem.FindControl("ddlActivityEdit")).SelectedValue;

            string fromtime = (FromTime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : FromTime;


            string TimeDuration = ((RadLabel)editableItem.FindControl("lblTimeDurationEdit")).Text;
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            gvVesselMovements.Rebind();
            BindOperationalTimeSummary();
            gvOperationalTimeSummary.Rebind();
            UpdateOperationalSummaryData();

        }
    }

    protected void gvAnchor_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {


                GridDataItem item = (GridDataItem)e.Item;
                RadLabel lblValueType = (RadLabel)item.FindControl("lblValueType");
                RadLabel lblValue = (RadLabel)item.FindControl("lblValue");
                RadLabel lblValue2 = (RadLabel)item.FindControl("lblValue2");
                RadLabel lblSize = (RadLabel)item.FindControl("lblSize");
                RadLabel lblLength = (RadLabel)item.FindControl("lblLength");
                RadCheckBoxList chkValue = (RadCheckBoxList)item.FindControl("chkValue");
                RadTextBox txtValueDecimal = (RadTextBox)item.FindControl("txtValueDecimal");
                RadTextBox txtValueTextSize = (RadTextBox)item.FindControl("txtValueTextSize");
                RadTextBox txtValueTextLen = (RadTextBox)item.FindControl("txtValueTextLen");
                RadRadioButtonList rblValue = (RadRadioButtonList)item.FindControl("rblValue");

                if (lblValueType.Text == "1")
                {
                    txtValueDecimal.Visible = false;
                    chkValue.Visible = true;
                    txtValueTextSize.Visible = false;
                    txtValueTextLen.Visible = false;
                    rblValue.Visible = false;

                    int iRowCount = 0;
                    int iTotalPageCount = 0;
                    DataSet ds = PhoenixRegistersDMRAnchorHandilingType.DMRAnchorHandilingTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
                    DataTable dt = ds.Tables[0];
                    chkValue.DataSource = dt;
                    chkValue.DataBind();

                    if (chkValue != null)
                    {
                        foreach (ButtonListItem litem in chkValue.Items)
                        {
                            item.Selected = false;
                            if (!string.IsNullOrEmpty(lblValue.Text) && ("," + lblValue.Text + ",").Contains("," + litem.Value.ToString() + ","))
                                item.Selected = true;
                        }
                    }
                }
                else if (lblValueType.Text == "2")
                {
                    txtValueDecimal.Visible = true;
                    chkValue.Visible = false;
                    txtValueTextSize.Visible = false;
                    txtValueTextLen.Visible = false;
                    rblValue.Visible = false;

                    txtValueDecimal.Text = lblValue.Text;
                }
                else if (lblValueType.Text == "3")
                {
                    txtValueDecimal.Visible = false;
                    chkValue.Visible = false;
                    txtValueTextSize.Visible = false;
                    txtValueTextLen.Visible = false;
                    rblValue.Visible = true;

                    rblValue.SelectedValue = lblValue.Text;
                }
                else if (lblValueType.Text == "4")
                {
                    txtValueDecimal.Visible = false;
                    chkValue.Visible = false;
                    txtValueTextSize.Visible = true;
                    txtValueTextLen.Visible = true;
                    rblValue.Visible = false;

                    lblSize.Visible = true;
                    lblLength.Visible = true;

                    txtValueTextSize.Text = lblValue.Text;
                    txtValueTextLen.Text = lblValue2.Text;
                }
                // }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAnchor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindAnchor();
    }

    protected void gvROVoperation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {


            GridDataItem item = (GridDataItem)e.Item;

            RadLabel lblValueType = (RadLabel)item.FindControl("lblValueType");
            RadLabel lblValue = (RadLabel)item.FindControl("lblValue");
            RadCheckBoxList chkValue = (RadCheckBoxList)item.FindControl("chkValue");
            RadTextBox txtValueDecimal = (RadTextBox)item.FindControl("txtValueDecimal");

            if (lblValueType.Text == "1")
            {
                txtValueDecimal.Visible = false;
                chkValue.Visible = true;

                int iRowCount = 0;
                int iTotalPageCount = 0;
                DataSet ds = PhoenixRegistersDMRRovType.MDRRovTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
                DataTable dt = ds.Tables[0];
                chkValue.DataSource = dt;
                chkValue.DataBind();

                if (chkValue != null)
                {
                    foreach (ButtonListItem litem in chkValue.Items)
                    {
                        item.Selected = false;
                        if (!string.IsNullOrEmpty(lblValue.Text) && ("," + lblValue.Text + ",").Contains("," + litem.Value.ToString() + ","))
                            item.Selected = true;
                    }
                }
            }
            else
            {
                txtValueDecimal.Visible = true;
                chkValue.Visible = false;
                txtValueDecimal.Text = lblValue.Text;
            }
        }
    }

    protected void gvROVoperation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindROVoperation();
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

                SaveMidnightReport(0);

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
            GridHeaderItem header = (GridHeaderItem)e.Item;
            header["lblEHHeader"].ToolTip = "Exposure Hour";
            RadLabel lblEH = (RadLabel)e.Item.FindControl("lblEHHeader");
            UserControlToolTip ucEH = (UserControlToolTip)e.Item.FindControl("ucEHHeader");
            if(lblEH !=null) lblEH.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucEH.ToolTip + "', 'visible');");
            if(lblEH != null) lblEH.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucEH.ToolTip + "', 'hidden');");

            RadLabel lblhpi = (RadLabel)e.Item.FindControl("lblhpiHeader");
            UserControlToolTip ucthpi = (UserControlToolTip)e.Item.FindControl("uchpiHeader");
            if(lblhpi!=null)lblhpi.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'visible');");
            if(lblhpi!=null)lblhpi.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucthpi.ToolTip + "', 'hidden');");


            RadLabel lbllti = (RadLabel)e.Item.FindControl("lblltiHeader");
            UserControlToolTip uctlti = (UserControlToolTip)e.Item.FindControl("ucltiHeader");
            if(lbllti!=null)lbllti.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctlti.ToolTip + "', 'visible');");
            if(lbllti!=null)lbllti.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctlti.ToolTip + "', 'hidden');");


            RadLabel lblrwc = (RadLabel)e.Item.FindControl("lblrwcHeader");
            UserControlToolTip uctrwc = (UserControlToolTip)e.Item.FindControl("ucrwcHeader");
            if(lblrwc!=null)lblrwc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'visible');");
            if(lblrwc!=null)lblrwc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctrwc.ToolTip + "', 'hidden');");


            RadLabel lblmtc = (RadLabel)e.Item.FindControl("lblmtcHeader");
            UserControlToolTip uctmtc = (UserControlToolTip)e.Item.FindControl("ucmtcHeader");
            if(lblmtc!=null)lblmtc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'visible');");
            if(lblmtc!=null)lblmtc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctmtc.ToolTip + "', 'hidden');");

            RadLabel lblfac = (RadLabel)e.Item.FindControl("lblfacHeader");
            UserControlToolTip uctfac = (UserControlToolTip)e.Item.FindControl("ucfacHeader");
            if(lblfac!=null)lblfac.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctfac.ToolTip + "', 'visible');");
            if(lblfac!=null)lblfac.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctfac.ToolTip + "', 'hidden');");

            RadLabel lblEnvInc = (RadLabel)e.Item.FindControl("lblEnvironmentalIncidentHeader");
            UserControlToolTip uctEnvInc = (UserControlToolTip)e.Item.FindControl("ucEnvironmentalIncidentHeader");
            if(lblEnvInc!=null)lblEnvInc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'visible');");
            if(lblEnvInc!=null)lblEnvInc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctEnvInc.ToolTip + "', 'hidden');");


            RadLabel lblnmr = (RadLabel)e.Item.FindControl("lblnearmissHeader");
            UserControlToolTip uctnmr = (UserControlToolTip)e.Item.FindControl("ucnearmissHeader");
            if(lblnmr!=null)lblnmr.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'visible');");
            if(lblnmr!=null)lblnmr.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctnmr.ToolTip + "', 'hidden');");

            RadLabel lblstpcrd = (RadLabel)e.Item.FindControl("lblstopcardsHeader");
            UserControlToolTip uctstpcrd = (UserControlToolTip)e.Item.FindControl("ucstopcardsHeader");
            if(lblstpcrd!=null)lblstpcrd.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'visible');");
            if(lblstpcrd!=null)lblstpcrd.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctstpcrd.ToolTip + "', 'hidden');");

            RadLabel lbled = (RadLabel)e.Item.FindControl("lblExercisesandDrillsHeader");
            UserControlToolTip ucted = (UserControlToolTip)e.Item.FindControl("ucExercisesandDrillsHeader");
            if(lbled!=null)lbled.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucted.ToolTip + "', 'visible');");
            if(lbled!=null)lbled.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucted.ToolTip + "', 'hidden');");

            RadLabel lblra = (RadLabel)e.Item.FindControl("lblNoofRiskAssesmentHeader");
            UserControlToolTip uctra = (UserControlToolTip)e.Item.FindControl("ucNoofRiskAssesmentHeader");
            if(lblra!=null)lblra.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctra.ToolTip + "', 'visible');");
            if(lblra!=null)lblra.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctra.ToolTip + "', 'hidden');");

            RadLabel lblsfty = (RadLabel)e.Item.FindControl("lblnoofsafetyHeader");
            UserControlToolTip uctsfty = (UserControlToolTip)e.Item.FindControl("ucnoofsafetyHeader");
            if(lblsfty!=null)lblsfty.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'visible');");
            if(lblsfty!=null)lblsfty.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctsfty.ToolTip + "', 'hidden');");

            RadLabel lblptw = (RadLabel)e.Item.FindControl("lblPTWIssuedHeader");
            UserControlToolTip uctptw = (UserControlToolTip)e.Item.FindControl("uclblPTWIssuedHeader");
            if(lblptw!=null)lblptw.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctptw.ToolTip + "', 'visible');");
            if(lblptw!=null)lblptw.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctptw.ToolTip + "', 'hidden');");

            RadLabel lbluauc = (RadLabel)e.Item.FindControl("lblUnsafeActsHeader");
            UserControlToolTip uctuauc = (UserControlToolTip)e.Item.FindControl("ucUnsafeActsHeader");
            if(lbluauc!=null)lbluauc.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctuauc.ToolTip + "', 'visible');");
            if(lbluauc!=null)lbluauc.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctuauc.ToolTip + "', 'hidden');");

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
                    UserControlMaskNumber ucstopcards = (UserControlMaskNumber)e.Item.FindControl("ucstopcards");
                    UserControlMaskNumber ucExercisesandDrills = (UserControlMaskNumber)e.Item.FindControl("ucExercisesandDrills");
                    UserControlMaskNumber ucnoofsafety = (UserControlMaskNumber)e.Item.FindControl("ucnoofsafety");
                    UserControlMaskNumber ucPTWIssued = (UserControlMaskNumber)e.Item.FindControl("ucPTWIssued");

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


                    lnkHpi.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=HPI&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkEnvRelease.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=ENV&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkLit.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=LTI&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkrwc.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=RWC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkMtc.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=MTC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkFac.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=FAC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkNearmiss.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=NM&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkUnsafeActs.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=UAUC&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");
                    lnkNoofRiskAssesment.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRhseIncident.aspx?HSEType=RA&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

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

                SaveMidnightReport(0);

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

    protected void gvMeteorologyData_ItemCommand(object sender, GridCommandEventArgs e)
    {

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

            UserControlMaskNumber txtValueDecimal6Edit = (UserControlMaskNumber)item.FindControl("txtValueDecimal6Edit");
            UserControlMaskNumber txtValueDecimal12Edit = (UserControlMaskNumber)item.FindControl("txtValueDecimal12Edit");
            UserControlMaskNumber txtValueDecimal18Edit = (UserControlMaskNumber)item.FindControl("txtValueDecimal18Edit");
            UserControlMaskNumber txtValueDecimal24Edit = (UserControlMaskNumber)item.FindControl("txtValueDecimal24Edit");
            UserControlMaskNumber txtValueDecimalNext24HrsEdit = (UserControlMaskNumber)item.FindControl("txtValueDecimalNext24HrsEdit");

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
        if(e.Item is GridFooterItem)
        {
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
            DataSet ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportOperationalTimeSummaryList(General.GetNullableGuid(Session["MIDNIGHTREPORTID"] == null ? "" : Session["MIDNIGHTREPORTID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0 )
            {
                if ((General.GetNullableDecimal(ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString()) == null ? 0 : decimal.Parse(ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString())) - total != 0)
                TotalFOC.BackColor = System.Drawing.Color.OrangeRed;

                TotalFOC.Text = ds.Tables[0].Rows[0]["FLDTOTALFUELOILCONSUMPTION"].ToString();
                lblTotalTime.Text = ds.Tables[0].Rows[0]["FLDTOTALTIMEDURATION"].ToString();
                lblTotalDistance.Text = ds.Tables[0].Rows[0]["FLDTOTALDISTANCE"].ToString();
                txtDPTime.Text = ds.Tables[0].Rows[0]["FLDTOTALDPTIME"].ToString();
                txtDPFuelConsumption.Text = ds.Tables[0].Rows[0]["FLDTOTALDPFUELCONSUPTION"].ToString();
            }
            else
            {
                TotalFOC.BorderColor = System.Drawing.Color.Black;
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
                gvOperationalTimeSummary.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                lblWorkOrderNo.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + lblWorkOrderId.Text + "'); return false;");

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
            lnkCount.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreDMRPMSOverdueDetails.aspx?type=" + General.GetNullableString(lblType.Text) + "&vesselid=" + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&reportdate=" + txtDate.Text + "'); return false;");

        }
    }

    protected void gvPMSoverdue_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPMSoverdue();
    }

    protected void gvExternalInspection_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindExternalInspection();
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
            string txtNumberOfNCEdit = ((UserControlMaskNumber)eeditedItem.FindControl("txtNumberOfNCEdit")).Text;

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
                string txtNumberOfNCAdd = ((UserControlMaskNumber)footerItem.FindControl("txtNumberOfNCAdd")).Text;


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
                gvExternalInspection.Rebind();
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



    protected void gvDeckCargo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDeckCargo();
    }

    protected void gvDeckCargoSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDeckCargoSummary();
    }

    protected void gvBulks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOilLoadandConsumption();
    }

    protected void gvCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDueCertificates();
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

    protected void gvShipAudit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDueAudits();
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

        }
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

    protected void gvPassenger_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        //BindPassenger();
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

    protected void gvBulks_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        //GridView gv = (GridView)sender;

        ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

        if (e.Item is GridDataItem)
        {
            // DataRowView drv = (DataRowView)e.Row.DataItem;
            RadLabel lblOpeningStock = (RadLabel)e.Item.FindControl("lblOpeningStocks");
            UserControlDecimal txtOpeningStock = (UserControlDecimal)e.Item.FindControl("txtOpeningStock");
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
            RadLabel lblUnit = (RadLabel)e.Item.FindControl("lblUnit");

            UserControlDecimal ChartererLoadedEdit = (UserControlDecimal)e.Item.FindControl("ucChartererLoadedEdit");
            UserControlDecimal ChartererDischargedEdit = (UserControlDecimal)e.Item.FindControl("ucChartererDischargedEdit");
            UserControlDecimal ConsumedEdit = (UserControlDecimal)e.Item.FindControl("ucConsumedEdit");
            RadLabel lblProductName = (RadLabel)e.Item.FindControl("lblProductName");
            RadLabel lblActiveYN = (RadLabel)e.Item.FindControl("lblActiveYN");
            if (lblActiveYN != null && lblActiveYN.Text == "0")
            {
                ChartererLoadedEdit.CssClass = "readonlytextbox";
                ChartererLoadedEdit.ReadOnly = "true";
                ChartererDischargedEdit.CssClass = "readonlytextbox";
                ChartererDischargedEdit.ReadOnly = "true";
                ConsumedEdit.CssClass = "readonlytextbox";
                ConsumedEdit.ReadOnly = "true";
                lblOpeningStock.Visible = true;
                txtOpeningStock.Visible = false;
            }
            if (DataBinder.Eval(e.Item.DataItem, "FLDSHORTNAME").ToString() == "FO")
            {
                ConsumedEdit.CssClass = "readonlytextbox";
                ConsumedEdit.ReadOnly = "true";
            }

            if (lblUnit.Text != "LTR")
            {
                //ChartererLoadedEdit.IsInteger = false;
                //ChartererLoadedEdit.DecimalPlace = 2;
                //ChartererDischargedEdit.IsInteger = false;
                //ChartererDischargedEdit.DecimalPlace = 2;
                //ConsumedEdit.IsInteger = false;
                //ConsumedEdit.DecimalPlace = 2;
                //txtOpeningStock.IsInteger = false;
                //txtOpeningStock.DecimalPlace = 2;

            }
            if (lblProductName.Text == "FO (LTR)")
            {
                decimal total = ((General.GetNullableDecimal(txtgrandtotal.Text) == null ? 0 : decimal.Parse(txtgrandtotal.Text)) +
                                 (General.GetNullableDecimal(txtAE1Consumption.Text) == null ? 0 : decimal.Parse(txtAE1Consumption.Text)) +
                                 (General.GetNullableDecimal(txtAE2Consumption.Text) == null ? 0 : decimal.Parse(txtAE2Consumption.Text))
                                 );
                ConsumedEdit.Text = total.ToString();
            }

            Label lblRemainedOnBoard = (Label)e.Item.FindControl("lblRemainedOnBoard");
            if (General.GetNullableDecimal(lblRemainedOnBoard.Text) != null)
            {
                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()) != null && ((decimal.Parse(lblRemainedOnBoard.Text)) < (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMINVALUE").ToString()))))
                {
                    lblRemainedOnBoard.CssClass = "maxhighlight";
                }
                if (General.GetNullableDecimal(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString()) != null && (decimal.Parse(lblRemainedOnBoard.Text)) > (decimal.Parse(DataBinder.Eval(e.Item.DataItem, "FLDMAXVALUE").ToString())))
                {
                    lblRemainedOnBoard.CssClass = "maxhighlight";
                }
            }

        }
        if (e.Item is GridHeaderItem)
        {
            RadLabel lblOpeningStocksHeader = (RadLabel)e.Item.FindControl("lblOpeningStocksHeader");
            if (ViewState["INITIALREPORTYN"].ToString() == "1")
            {
                if (lblOpeningStocksHeader != null)
                    lblOpeningStocksHeader.Text = "Opening Stock";
            }

        }
    }

    protected void gvShipCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvShipCrew_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            // DataRowView drv = (DataRowView)e.Row.DataItem;

            RadLabel lblNCCount = (RadLabel)e.Item.FindControl("lblNCCount");
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            Image Imageyellow = (Image)e.Item.FindControl("Imageyellow");
            RadLabel lbllastRHRecorddate = (RadLabel)e.Item.FindControl("lblRHDate");
            RadLabel lblLevelCode = (RadLabel)e.Item.FindControl("lblLevelCode");
            RadLabel lblonboardoverdue = (RadLabel)e.Item.FindControl("lblonboardoverdue");

            DateTime MidNightReportDate = DateTime.Parse(txtDate.Text);
            DateTime RHDate = MidNightReportDate;
            if (!string.IsNullOrEmpty(lbllastRHRecorddate.Text))
                RHDate = DateTime.Parse(lbllastRHRecorddate.Text);

            if (MidNightReportDate.CompareTo(RHDate) == 1)
            {
                e.Item.Cells[5].BackColor = System.Drawing.Color.Red;
                //lbllastRHRecorddate.BackColor = System.Drawing.Color.Red;
            }
            if (lblonboardoverdue.Text == "1")
            {
                e.Item.Cells[4].BackColor = System.Drawing.Color.Red;
                //lbllastRHRecorddate.BackColor = System.Drawing.Color.Red;
            }


            if (lblLevelCode != null && !string.IsNullOrEmpty(lblLevelCode.Text) && lblLevelCode.Text == "1")
            {
                Imageyellow.Visible = true;
                Imageyellow.ImageUrl = Session["images"] + "/Yellow-symbol.png";
            }
            if (lblLevelCode != null && !string.IsNullOrEmpty(lblLevelCode.Text) && lblLevelCode.Text == "2")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
            }


            LinkButton lnkNCCount = (LinkButton)e.Item.FindControl("lnkNCCount");

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
                lnkNCCount.Attributes.Add("onclick", "parent.Openpopup('MoreInfo', '', '../VesselAccounts/VesselAccountsRHWorkCalenderRemarks.aspx?CalenderId=" + DataBinder.Eval(e.Item.DataItem, "FLDRESTHOURCALENDARID").ToString() + "&EMPID=" + DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEEID").ToString() + "&RHStartId=" + DataBinder.Eval(e.Item.DataItem, "FLDRESTHOURSTARTID").ToString() + "'); return false;");
            }
        }
    }
}
