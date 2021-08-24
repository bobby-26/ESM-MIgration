using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselPositionShiftingReportEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenPick.Attributes.Add("style", "display:none;");
        txtVoyageId.Attributes.Add("style", "visibility:hidden");

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
  

        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "DEPARTUREREPORT");
        toolbarmain.AddButton("Shifting", "DEPARTURE");
        toolbarmain.AddButton("Operations", "OPERATIONS");
        toolbarmain.AddButton("Emission In Port", "MRVSUMMARY");
        MenuDRSubTab.AccessRights = this.ViewState;
        MenuDRSubTab.MenuList = toolbarmain.Show();
        MenuDRSubTab.SelectedMenuIndex = 1;

        PhoenixToolbar toolbar = new PhoenixToolbar();     
        toolbar.AddButton("Send To Office", "SENDTOOFFICE", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        ProjectBilling.AccessRights = this.ViewState;
        ProjectBilling.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["VOYAGEID"] = "";
            ViewState["DEPARTUREPORTCALLID"] = "";
            ViewState["DEBUNKER"] = 0;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "', true); ");
                UcVessel.Enabled = false;
            }

            if ((Request.QueryString["mode"] != null) && (Request.QueryString["mode"].ToString().Equals("NEW")))
            {
                Filter.CurrentVPRSShiftingReportSelection = null;
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "0";
                ucDeparturePort.VesselId = ViewState["VESSELID"].ToString();
                ucNextPort.VesselId = ViewState["VESSELID"].ToString(); 
                Reset();
                btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");

                DataSet dsCurrentvoyage = PhoenixVesselPositionVoyageData.CurrentVoyageData(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dsCurrentvoyage.Tables.Count > 0)
                {
                    if (dsCurrentvoyage.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsCurrentvoyage.Tables[0].Rows[0];

                        txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                        txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                        ucDeparturePort.VoyageId = txtVoyageId.Text;
                        ViewState["VOYAGEID"] = txtVoyageId.Text;
                    }
                }
            }
            ViewState["NOONREPORTID"] = null;
            VesselDepartureEdit();
            ViewState["DEPARTUREPORTCALLID"] = ucDeparturePort.SelectedPortCallValue;
            VesselVoyageDetailsEdit();
        }

    }

    protected void MenuDRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Filter.CurrentVPRSShiftingReportSelection != null)
        {
            if (CommandName.ToUpper().Equals("OPERATIONS"))
            {
                Response.Redirect("../VesselPosition/VesselPositionShiftingReportOperations.aspx");
            }
            if (CommandName.ToUpper().Equals("MRVSUMMARY"))
            {
                Response.Redirect("../VesselPosition/VesselPositionShiftingReportMRVSummary.aspx");
            }
            if (CommandName.ToUpper().Equals("DEPARTUREREPORT"))
            {
                if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                    Response.Redirect("VesselPositionReports.aspx", false);
                else
                    Response.Redirect("VesselPositionShiftingReport.aspx", false);
                //Response.Redirect("../VesselPosition/VesselPositionShiftingReport.aspx");
            }
        }
    }


    protected void LastArrivalPort()
    {
        DataSet ds = PhoenixVesselPositionArrivalReport.LastArrivalPort(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucDeparturePort.Text = dr["FLDSEAPORTNAME"].ToString();
                ucDeparturePort.SelectedValue = dr["FLDLASTPORT"].ToString();
                if (dr["FLDLASTPORTCALLID"].ToString() != "")
                    ucDeparturePort.SelectedPortCallValue = dr["FLDLASTPORTCALLID"].ToString();
            }
        }
    }

    protected void VesselVoyageDetailsEdit()
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.GetVoyageDetailsForDepartureReport(
            General.GetNullableGuid(ViewState["VOYAGEID"] == null ? txtVoyageId.Text : ViewState["VOYAGEID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(ViewState["DEPARTUREPORTCALLID"].ToString()));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (Filter.CurrentVPRSShiftingReportSelection == null)
                {
                    ucNextPort.SelectedValue = dr["FLDNEXTPORT"].ToString();
                    ucNextPort.SelectedPortCallValue = dr["FLDNEXTPORTCALLID"].ToString();
                    ucNextPort.Text = dr["FLDNEXTPORTNAME"].ToString();
                }
            }
        }
    }

    protected void VesselDepartureEdit()
    {
        if (Filter.CurrentVPRSShiftingReportSelection != null)
        {
            DataSet ds = PhoenixVesselPositionDepartureReport.EditDepartureReport(General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ucDeparturePort.VesselId = dr["FLDVESSELID"].ToString();
                    ucDeparturePort.VoyageId = dr["FLDVOYAGEID"].ToString();
                    ucDeparturePort.SelectedValue = dr["FLDPORT"].ToString();
                    ucDeparturePort.SelectedPortCallValue = dr["FLDCURRENTPORTCALLID"].ToString();
                    ucDeparturePort.Text = dr["FLDSEAPORTNAME"].ToString();
                    ViewState["NOONREPORTID"] = dr["FLDNOONREPORTID"].ToString();
                    ViewState["VOYAGEID"] = dr["FLDVOYAGEID"].ToString();
                    ViewState["DEPARTUREPORTCALLID"] = dr["FLDCURRENTPORTCALLID"].ToString();
                    ucNextPort.VesselId = dr["FLDVESSELID"].ToString();
                    ucNextPort.VoyageId = dr["FLDVOYAGEID"].ToString(); 
                    ucNextPort.Text = dr["FLDNEXTPORTNAME"].ToString();
                    ucNextPort.SelectedValue = dr["FLDNEXTPORT"].ToString();
                    ucNextPort.SelectedPortCallValue = dr["FLDNEXTPORTCALLID"].ToString();
                    txtNextPortOperation.SelectedPortactivity = dr["FLDNEXTPORTOPERATION"].ToString();
                    txtRemarks.Text = dr["FLDREMARKS"].ToString();
                    UcVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                    ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                    UcVessel.Enabled = false;
                    txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                    txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                    btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                    txtSBE.Text = dr["FLDSBE"].ToString();
                    if (General.GetNullableDateTime(dr["FLDSBE"].ToString()) != null)
                        txtSBETime.SelectedDate = Convert.ToDateTime(dr["FLDSBE"]);
                    txtSludgeLanded.Text = dr["FLDSLUDGELANDED"].ToString();
                    txtSludgeLandedTime.Text = string.Format("{0:HH:mm}", dr["FLDSLUDGELANDED"]);
                    txtLOSampleLandedTime.Text = string.Format("{0:HH:mm}", dr["FLDLOSAMPLELANDED"]);

                    if (General.GetNullableDateTime(dr["FLDLOSAMPLELANDED"].ToString()) != null)
                    {
                        rbLOSampleLanded.SelectedValue = "1";
                        txtLOSampleLanded.Text = dr["FLDLOSAMPLELANDED"].ToString();
                        txtLOSampleLanded.Enabled = true;
                    }
                    else
                    {
                        rbLOSampleLanded.SelectedValue = "0";
                        txtLOSampleLanded.Text = "";
                        txtLOSampleLanded.Enabled = false;
                    }

                    txtBirthName.Text = dr["FLDBERTHNAME"].ToString();
                    txtArrivedBirthName.Text = dr["FLDARRIVEDBERTHNAME"].ToString();
                    txtPOB.Text = dr["FLDPOB"].ToString();
                    if (General.GetNullableDateTime(dr["FLDPOB"].ToString()) != null)
                        txtPOBTime.SelectedDate = Convert.ToDateTime(dr["FLDPOB"]);
                    txtLLC.Text = dr["FLDLLC"].ToString();
                    if (General.GetNullableDateTime(dr["FLDLLC"].ToString()) != null)
                        txtLLCTime.SelectedDate = Convert.ToDateTime(dr["FLDLLC"]);
                    txtAAW.Text = dr["FLDAAW"].ToString();
                    if (General.GetNullableDateTime(dr["FLDAAW"].ToString()) != null)
                        txtAAWTime.SelectedDate = Convert.ToDateTime(dr["FLDAAW"]);
                    txtDLOSP.Text = dr["FLDDLOSP"].ToString();
                    txtFLA.Text = dr["FLDFLA"].ToString();
                    if (General.GetNullableDateTime(dr["FLDFLA"].ToString()) != null)
                        txtFLATime.SelectedDate = Convert.ToDateTime(dr["FLDFLA"]);
                    txtLGA.Text = dr["FLDLGA"].ToString();
                    if (General.GetNullableDateTime(dr["FLDLGA"].ToString()) != null)
                        txtLGATime.SelectedDate = Convert.ToDateTime(dr["FLDLGA"]);
                    txtFWE.Text = dr["FLDFWE"].ToString();
                    if (General.GetNullableDateTime(dr["FLDFWE"].ToString()) != null)
                        txtFWETime.SelectedDate = Convert.ToDateTime(dr["FLDFWE"]);
                    txtETCD.Text = dr["FLDETD"].ToString();
                    if (General.GetNullableDateTime(dr["FLDETD"].ToString()) != null)
                        txtETCDTime.SelectedDate = Convert.ToDateTime(dr["FLDETD"]);
                    if (General.GetNullableDateTime(dr["FLDDLOSP"].ToString()) != null)
                        txtDLOSPTime.SelectedDate = Convert.ToDateTime(dr["FLDDLOSP"]);

                    txtManoevering.Text = dr["FLDMANOEVERING"].ToString();
                    txtHarbourSteamingDist.Text = dr["FLDHARBOURSTEAMINGDIST"].ToString();
                    txtSludgeLandedQty.Text = dr["FLDSLUDGELANDEDQTY"].ToString();
                    txtBilgeWaterLandedQty.Text = dr["FLDBILGEWATERLANDEDQTY"].ToString();
                    txtBilgeWaterLanded.Text = dr["FLDBILGEWATERLANDED"].ToString();
                    txtBilgeWaterLandedTime.Text = string.Format("{0:HH:mm}", dr["FLDBILGEWATERLANDED"]);
                    txtGarbageLanded.Text = dr["FLDGARBAGELANDED"].ToString();
                    txtGarbageLandedQty.Text = dr["FLDGARBAGELANDEDQTY"].ToString();
                    txtGarbageLandedTime.Text = string.Format("{0:HH:mm}", dr["FLDGARBAGELANDED"]);

                    if (General.GetNullableString(dr["FLDBALLASTYN"].ToString()) != null)
                        rbtnBallastLaden.SelectedValue = dr["FLDBALLASTYN"].ToString();
                    if (dr["FLDCONFIRMEDYN"].ToString() == "1")
                    {
                        ProjectBilling.Visible = false;
                        lblAlertSenttoOFC.Visible = true;
                    }
                    txtEMLogcounteratCOSP.Text = dr["FLDEMLOGCOUNTERATCOSP"].ToString();
                    txtEMLogManoeuveringDistance.Text = dr["FLDEMLOGMANOUVERINGDIST"].ToString();

                    txtUTCSBE.Text = dr["FLDUTCSBE"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCSBE"].ToString()) != null)
                        txtUTCSBETime.SelectedDate = Convert.ToDateTime(dr["FLDUTCSBE"]);
                    txtUTCPOB.Text = dr["FLDUTCPOB"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCPOB"].ToString()) != null)
                        txtUTCPOBTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCPOB"]);
                    txtUTCLLC.Text = dr["FLDUTCLLC"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCLLC"].ToString()) != null)
                        txtUTCPOBTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCLLC"]);
                    txtUTCAAW.Text = dr["FLDUTCAAW"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCAAW"].ToString()) != null)
                        txtUTCAAWTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCAAW"]);
                    txtUTCFLA.Text = dr["FLDUTCFLA"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCFLA"].ToString()) != null)
                        txtUTCFLATime.SelectedDate = Convert.ToDateTime(dr["FLDUTCFLA"]);
                    txtUTCLGA.Text = dr["FLDUTCLGA"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCLGA"].ToString()) != null)
                        txtUTCLGATime.SelectedDate = Convert.ToDateTime(dr["FLDUTCLGA"]);
                    txtUTCFWE.Text = dr["FLDUTCFWE"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCFWE"].ToString()) != null)
                        txtUTCFWETime.SelectedDate = Convert.ToDateTime(dr["FLDUTCFWE"]);
                    txtUTCETCD.Text = dr["FLDUTCETD"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCETD"].ToString()) != null)
                        txtUTCETCDTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCETD"]);
                    txtUTCDLOSP.Text = dr["FLDUTCDLOSP"].ToString();
                    if (General.GetNullableDateTime(dr["FLDUTCDLOSP"].ToString()) != null)
                        txtUTCDLOSPTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCDLOSP"]);

                    txtShipMeanTime.Text = dr["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
                    txtShipMeanTimeSymbol.SelectedValue = dr["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";

                    txtMERevCounter.Text = dr["FLDMEREVCOUNTER"].ToString();

                    chkCounterDefective.Checked = dr["FLDEMLOGCOUNTERDEFECTIVEYN"].ToString().Equals("1") ? true : false;
                    chkRevCounterDefective.Checked = dr["FLDEMREVCOUNTERDEFECTIVEYN"].ToString().Equals("1") ? true : false;
                    chkOffPortLimits.Checked = dr["FLDOFFPORTLIMITYN"].ToString().Equals("1") ? true : false;

                    if (chkCounterDefective.Checked == true)
                    {
                        txtEMLogManoeuveringDistance.CssClass = "input";
                        txtEMLogManoeuveringDistance.Enabled = true;
                    }
                    else
                    {
                        txtEMLogManoeuveringDistance.CssClass = "readonlytextbox";
                        txtEMLogManoeuveringDistance.Enabled = false;
                    }
                }
            }
        }
        else
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                DataSet dsBL = PhoenixVesselPositionNoonReport.NoonReportFirstEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dsBL.Tables.Count > 0)
                {
                    if (dsBL.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsBL.Tables[0].Rows[0];

                        if (General.GetNullableString(dr["FLDBALLASTYN"].ToString()) != null)
                            rbtnBallastLaden.SelectedValue = dr["FLDBALLASTYN"].ToString();

                        if (General.GetNullableString(dr["FLDSHIPMEANTIME"].ToString()) != null)
                        {
                            txtShipMeanTime.Text = dr["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
                            txtShipMeanTimeSymbol.SelectedValue = dr["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";
                        }
                    }
                }
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Reset()
    {
        txtVoyageName.Text = "";
        txtVoyageId.Text = "";
        ViewState["VOYAGEID"] = "";
        ucDeparturePort.SelectedValue = "";
        ucDeparturePort.SelectedPortCallValue = "";
        ucDeparturePort.Text = "";
        ucNextPort.SelectedValue = "";
        ucNextPort.SelectedPortCallValue = "";
        ucNextPort.Text = "";
        txtNextPortOperation.SelectedPortactivity = "";
        txtRemarks.Text = "";
        txtSBE.Text = "";
        txtSBETime.SelectedDate = null;
        txtSludgeLanded.Text = "";
        txtSludgeLandedTime.Text = "";
        txtLOSampleLanded.Text = "";
        txtLOSampleLandedTime.Text = "";
        txtBirthName.Text = "";
        txtArrivedBirthName.Text = "";
        txtPOB.Text = "";
        txtPOBTime.SelectedDate = null;
        txtLLC.Text = "";
        txtLLCTime.SelectedDate = null;
        txtAAW.Text = "";
        txtAAWTime.SelectedDate = null;
        txtDLOSP.Text = "";
        txtETCD.Text = "";
        txtETCDTime.SelectedDate = null;
        txtFLA.Text = "";
        txtFLATime.SelectedDate = null;
        txtLGA.Text = "";
        txtLGATime.SelectedDate = null;
        txtFWE.Text = "";
        txtFWETime.SelectedDate = null;
        txtDLOSPTime.SelectedDate = null;
        txtManoevering.Text = "";
        txtHarbourSteamingDist.Text = "";
        txtSludgeLandedQty.Text = "";
        txtBilgeWaterLandedQty.Text = "";
        txtBilgeWaterLanded.Text = "";
        txtBilgeWaterLandedTime.Text = "";
        txtGarbageLanded.Text = "";
        txtGarbageLandedQty.Text = "";
        txtGarbageLandedTime.Text = "";
    }

    protected void ProjectBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SENDTOOFFICE"))           
            {
                if (Filter.CurrentVPRSShiftingReportSelection != null)
                {
                        if (CommandName.ToUpper().Equals("SENDTOOFFICE"))
                        {
                            UpdateDeparture(1);
                        }
                        else
                        {
                            UpdateDeparture(0);
                        }
                            UpdateDepartureServices();

                        ucStatus.Text = "Shifting report updated";
                }
                else
                {

                        if (CommandName.ToUpper().Equals("SENDTOOFFICE"))
                        {
                            AddDeparture(1);
                        }
                        else
                        {
                            AddDeparture(0);
                        }
                       
                        UpdateDepartureServices();
                        ucStatus.Text = "Shifting report added";
                        Response.Redirect("../VesselPosition/VesselPositionShiftingReportEdit.aspx?VESSELDEPARTUREID=" + Filter.CurrentVPRSShiftingReportSelection, false);
                }
                VesselDepartureEdit();
                RebingvConsumption();
                RebindgvServices();
                RebindgvServices1();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;          
        }
    }

    private void AddDeparture(int confirm)
    {
        string sbetime = txtSBETime.SelectedTime != null ? txtSBETime.SelectedTime.Value.ToString() : "";       
        string sludgelandtime = txtSludgeLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtSludgeLandedTime.Text;
        string lolandtime = txtLOSampleLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtLOSampleLandedTime.Text;
        string pobtime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string llctime = txtLLCTime.SelectedTime != null ? txtLLCTime.SelectedTime.Value.ToString() : "";
        string aawtime = txtAAWTime.SelectedTime != null ? txtAAWTime.SelectedTime.Value.ToString() : "";
        string dlosptime = txtDLOSPTime.SelectedTime != null ? txtDLOSPTime.SelectedTime.Value.ToString() : "";
        string bilgewaterlandedtime = txtBilgeWaterLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeWaterLandedTime.Text;
        string garbagelandedtime = txtGarbageLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtGarbageLandedTime.Text;
        string etcdtime = txtETCDTime.SelectedTime != null ? txtETCDTime.SelectedTime.Value.ToString() : "";
        string flatime = txtFLATime.SelectedTime != null ? txtFLATime.SelectedTime.Value.ToString() : "";
        string lgatime = txtLGATime.SelectedTime != null ? txtLGATime.SelectedTime.Value.ToString() : "";
        string fwetime = txtFWETime.SelectedTime != null ? txtFWETime.SelectedTime.Value.ToString() : "";

        Guid? vesseldepartureid = null;
        Guid? noonreportid = null;

        PhoenixVesselPositionDepartureReport.InsertShiftingReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(txtVoyageId.Text),
            General.GetNullableGuid(ucDeparturePort.SelectedPortCallValue),
            General.GetNullableGuid(ucNextPort.SelectedPortCallValue),
            General.GetNullableDecimal(""),
            txtNextPortOperation.SelectedPortactivity,
            txtRemarks.Text,
            ref vesseldepartureid,
            General.GetNullableDateTime(txtSBE.Text + " " + sbetime),
            General.GetNullableDateTime(txtSludgeLanded.Text + " " + sludgelandtime),
            General.GetNullableDateTime(txtLOSampleLanded.Text == "" ? null : txtLOSampleLanded.Text + " " + lolandtime),
            txtBirthName.Text,
            txtArrivedBirthName.Text,
            General.GetNullableDateTime(txtPOB.Text + " " + pobtime),
            General.GetNullableDateTime(txtLLC.Text + " " + llctime), 
            General.GetNullableDateTime(txtAAW.Text + " " + aawtime),
            General.GetNullableDateTime(txtDLOSP.Text + " " + dlosptime),
            General.GetNullableDateTime(txtETCD.Text + " " + etcdtime),
            General.GetNullableDateTime(txtFLA.Text + " " + flatime),
            General.GetNullableDateTime(txtLGA.Text + " " + lgatime),
            General.GetNullableDateTime(txtFWE.Text + " " + fwetime),
            General.GetNullableDecimal(txtManoevering.Text),
            General.GetNullableDecimal(txtHarbourSteamingDist.Text),
            General.GetNullableDecimal(txtSludgeLandedQty.Text),
            General.GetNullableDecimal(txtBilgeWaterLandedQty.Text),
            General.GetNullableDateTime(txtBilgeWaterLanded.Text + " " + bilgewaterlandedtime),
            General.GetNullableDecimal(txtGarbageLandedQty.Text),
            General.GetNullableDateTime(txtGarbageLanded.Text + " " + garbagelandedtime),
            1,
            General.GetNullableInteger(rbtnBallastLaden.SelectedValue),
            ref noonreportid,confirm,
            General.GetNullableDecimal(txtEMLogcounteratCOSP.Text),
            (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text),
            General.GetNullableDecimal(txtMERevCounter.Text),
            General.GetNullableDecimal(txtEMLogManoeuveringDistance.Text),
            chkCounterDefective.Checked == true ? 1 : 0,
            chkRevCounterDefective.Checked == true ? 1 : 0,
            chkOffPortLimits.Checked == true ? 1 : 0); //0 departure report 1 shifting report

        ViewState["NOONREPORTID"] = noonreportid;

        Filter.CurrentVPRSShiftingReportSelection = vesseldepartureid.ToString();
    }

    private void UpdateDeparture(int confirm)
    {
        string sbetime = txtSBETime.SelectedTime != null ? txtSBETime.SelectedTime.Value.ToString() : "";
        string sludgelandtime = txtSludgeLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtSludgeLandedTime.Text;
        string lolandtime = txtLOSampleLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtLOSampleLandedTime.Text;
        string pobtime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string llctime = txtLLCTime.SelectedTime != null ? txtLLCTime.SelectedTime.Value.ToString() : "";
        string aawtime = txtAAWTime.SelectedTime != null ? txtAAWTime.SelectedTime.Value.ToString() : "";
        string dlosptime = txtDLOSPTime.SelectedTime != null ? txtDLOSPTime.SelectedTime.Value.ToString() : "";
        string bilgewaterlandedtime = txtBilgeWaterLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeWaterLandedTime.Text;
        string garbagelandedtime = txtGarbageLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtGarbageLandedTime.Text;
        string etcdtime = txtETCDTime.SelectedTime != null ? txtETCDTime.SelectedTime.Value.ToString() : "";
        string flatime = txtFLATime.SelectedTime != null ? txtFLATime.SelectedTime.Value.ToString() : "";
        string lgatime = txtLGATime.SelectedTime != null ? txtLGATime.SelectedTime.Value.ToString() : "";
        string fwetime = txtFWETime.SelectedTime != null ? txtFWETime.SelectedTime.Value.ToString() : "";

        Guid? noonreportid = null;

        PhoenixVesselPositionDepartureReport.UpdateShiftingReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(txtVoyageId.Text),
            General.GetNullableGuid(ucDeparturePort.SelectedPortCallValue),
            General.GetNullableGuid(ucNextPort.SelectedPortCallValue),
            General.GetNullableDecimal(""),
            txtNextPortOperation.SelectedPortactivity,
            txtRemarks.Text,
            General.GetNullableDateTime(txtSBE.Text + " " + sbetime),
            General.GetNullableDateTime(txtSludgeLanded.Text + " " + sludgelandtime),
            General.GetNullableDateTime(txtLOSampleLanded.Text),
            txtBirthName.Text, txtArrivedBirthName.Text,
            General.GetNullableDateTime(txtPOB.Text + " " + pobtime),
            General.GetNullableDateTime(txtLLC.Text + " " + llctime), 
            General.GetNullableDateTime(txtAAW.Text + " " + aawtime),
            General.GetNullableDateTime(txtDLOSP.Text + " " + dlosptime),
            General.GetNullableDateTime(txtETCD.Text + " " + etcdtime),
            General.GetNullableDateTime(txtFLA.Text + " " + flatime),
            General.GetNullableDateTime(txtLGA.Text + " " + lgatime),
            General.GetNullableDateTime(txtFWE.Text + " " + fwetime),
            General.GetNullableDecimal(txtManoevering.Text),
            General.GetNullableDecimal(txtHarbourSteamingDist.Text),
            General.GetNullableDecimal(txtSludgeLandedQty.Text),
            General.GetNullableDecimal(txtBilgeWaterLandedQty.Text),
            General.GetNullableDateTime(txtBilgeWaterLanded.Text + " " + bilgewaterlandedtime),
            General.GetNullableDecimal(txtGarbageLandedQty.Text),
            General.GetNullableDateTime(txtGarbageLanded.Text + " " + garbagelandedtime),
            General.GetNullableInteger(rbtnBallastLaden.SelectedValue),
            ref noonreportid,confirm,
            General.GetNullableDecimal(txtEMLogcounteratCOSP.Text),
            (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text),
            General.GetNullableDecimal(txtMERevCounter.Text),
            General.GetNullableDecimal(txtEMLogManoeuveringDistance.Text),
            chkCounterDefective.Checked == true ? 1 : 0,
            chkRevCounterDefective.Checked == true ? 1 : 0,
            chkOffPortLimits.Checked == true ? 1 : 0);

        ViewState["NOONREPORTID"] = noonreportid;
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        RebingvConsumption();
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
        RebingvConsumption();
        ucDeparturePort.VesselId = UcVessel.SelectedVessel;
        ucNextPort.VesselId = UcVessel.SelectedVessel;
        LastArrivalPort();
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        ViewState["VOYAGEID"] = txtVoyageId.Text;
        ucDeparturePort.VoyageId = txtVoyageId.Text;
        ucDeparturePort.Text = "";
        ucDeparturePort.SelectedValue = "";
        ucDeparturePort.SelectedPortCallValue = "";

        ucNextPort.VoyageId = txtVoyageId.Text;
        ucNextPort.Text = "";
        ucNextPort.SelectedValue = "";
        ucNextPort.SelectedPortCallValue = "";

        VesselVoyageDetailsEdit();
        RebingvConsumption();
    }

    protected void ucDeparturePort_Changed(object sender, EventArgs e)
    {
        ViewState["DEPARTUREPORTCALLID"] = ucDeparturePort.SelectedPortCallValue;
        VesselVoyageDetailsEdit();
        RebingvConsumption();
    }

    //////////////////////////////////////// Services..

    protected void gvServices1_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {
                UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrency");
                if (ucCurrency != null)
                {
                    DataRowView dr = (DataRowView)e.Item.DataItem;
                    if (dr != null)
                        ucCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvServices_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblDepServiceId = ((RadLabel)e.Item.FindControl("lblDepServiceId"));
                if(lblDepServiceId.Text != "")
                    PhoenixVesselPositionDepartureReport.DeleteDepartureServices(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(lblDepServiceId.Text));

                RebindgvServices();
                RebindgvServices1();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateDepartureServices()
    {
        foreach (GridDataItem gvr in gvServices.Items)
        {
            if (((RadLabel)gvr.FindControl("lblServiceIdEdit")) != null)
            {
                PhoenixVesselPositionDepartureReport.UpdateDepartureServices(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid((((RadLabel)gvr.FindControl("lblDepServiceIdEdit")).Text)),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                    General.GetNullableGuid(((RadLabel)gvr.FindControl("lblServiceIdEdit")).Text),
                    General.GetNullableInteger(((UserControlMaskNumber)gvr.FindControl("txtQtyEdit")).Text),
                    General.GetNullableInteger(((RadLabel)gvr.FindControl("lblUnitId")).Text),
                     General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtEstCostEdit")).Text),
                    General.GetNullableInteger(((UserControlCurrency)gvr.FindControl("ucCurrency")).SelectedCurrency),
                    ((RadTextBox)gvr.FindControl("txtServiceCompEdit")).Text,
                    ((RadTextBox)gvr.FindControl("txtRemarksEdit")).Text);
            }
        }

        foreach (GridDataItem gvr in gvServices1.Items)
        {
            if (((RadLabel)gvr.FindControl("lblServiceIdEdit")) != null)
            {
                PhoenixVesselPositionDepartureReport.UpdateDepartureServicesDetails(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid((((RadLabel)gvr.FindControl("lblDepServiceIdEdit")).Text)),
                    General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                    General.GetNullableGuid(((RadLabel)gvr.FindControl("lblServiceIdEdit")).Text),
                    ((RadTextBox)gvr.FindControl("txtRemarksEdit")).Text);
            }
        }
    }

    protected void gvServices_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
    
            DataRowView drv = (DataRowView)e.Item.DataItem;


            if (e.Item is GridEditableItem)
            {
                //Label lbtn = (Label)e.Row.FindControl("lblRemarks");
                //UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucRemarksTT");
                //if (lbtn != null)
                //{
                //    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                //}

                //lbtn = (Label)e.Row.FindControl("lblServiceComp");
                //uct = (UserControlToolTip)e.Row.FindControl("ucServiceCompTT");
                //if (lbtn != null)
                //{
                //    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                //}


                if (e.Item is GridEditableItem)
                {
                    UserControlCurrency ucCurrency = (UserControlCurrency)e.Item.FindControl("ucCurrency");
                    if (ucCurrency != null)
                    {
                        DataRowView dr = (DataRowView)e.Item.DataItem;
                        if (dr != null)
                            ucCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void gvConsumption_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell tbOilType = new TableCell();
            TableCell tbrobfwe = new TableCell();
            TableCell tbBunkered = new TableCell();
            TableCell tbSulphur = new TableCell();
            TableCell tbrobsbe = new TableCell();
            TableCell tbrobcosp = new TableCell();
            TableCell tbAtSea = new TableCell();
            TableCell tbAtHourbour = new TableCell();
            TableCell tbInPort = new TableCell();
            TableCell tbLast = new TableCell();
            TableCell tbAction = new TableCell();

            tbOilType.ColumnSpan = 1;
            tbrobfwe.ColumnSpan = 1;
            tbInPort.ColumnSpan = 8;
            tbBunkered.ColumnSpan = 1;
            tbSulphur.ColumnSpan = 1;
            tbrobsbe.ColumnSpan = 1;
            tbrobcosp.ColumnSpan = 1;
            tbAtSea.ColumnSpan = 6;
            tbAtHourbour.ColumnSpan = 8;


            tbOilType.Text = "Item";
            tbrobfwe.Text = "Previous ROB";
            tbInPort.Text = "IN PORT";
            tbBunkered.Text = "Bunkered";
            tbSulphur.Text = "Sulphur %";
            tbrobsbe.Text = "ROB on Dep (SBE)";
            tbAtHourbour.Text = "AT HARBOUR";
            tbrobcosp.Text = "ROB On Arrival FWE";
            tbAction.Text = "Action";

            tbLast.Attributes.Add("style", "text-align:center");
            tbrobfwe.Attributes.Add("style", "text-align:center");
            tbBunkered.Attributes.Add("style", "text-align:center");
            tbSulphur.Attributes.Add("style", "text-align:center");
            tbOilType.Attributes.Add("style", "text-align:center");
            tbAtSea.Attributes.Add("style", "text-align:center");
            tbAtHourbour.Attributes.Add("style", "text-align:center");
            tbInPort.Attributes.Add("style", "text-align:center");
            tbAction.Attributes.Add("style", "text-align:center");
            tbrobsbe.Attributes.Add("style", "text-align:center");
            tbrobcosp.Attributes.Add("style", "text-align:center");

            gv.Cells.Add(tbOilType);
            gv.Cells.Add(tbrobfwe);
            gv.Cells.Add(tbInPort);
            gv.Cells.Add(tbBunkered);
            gv.Cells.Add(tbSulphur);
            gv.Cells.Add(tbrobsbe);
            gv.Cells.Add(tbAtHourbour);
            gv.Cells.Add(tbrobcosp);
            gv.Cells.Add(tbAction);
            gvConsumption.Controls[0].Controls.AddAt(0, gv);
        }

        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvConsumption, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }

    protected void rbLOSampleLanded_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbLOSampleLanded.SelectedValue == "0")
        {
            txtLOSampleLanded.Text = "";
            txtLOSampleLanded.Enabled = false;
        }
        else
        {
            txtLOSampleLanded.Enabled = true;
        }
    }

    protected void gvConsumption_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");

            if (edit != null)
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


                UserControlMaskNumber ucAtSeaAEEdit1 = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaAEEdit");
                if (ucAtSeaAEEdit1 != null)
                {
                    if (drv["FLDSHORTNAME"].ToString() == "AECC")
                        ucAtSeaAEEdit1.Enabled = false;
                    else
                        ucAtSeaAEEdit1.Enabled = true;
                }

                UserControlMaskNumber ucAtSeaOTHEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaOTHEdit");
                if (ucAtSeaOTHEdit != null)
                {
                    if (drv["FLDSHORTNAME"].ToString() == "OTH")
                        ucAtSeaOTHEdit.Enabled = false;
                    else
                        ucAtSeaOTHEdit.Enabled = true;
                }

                LinkButton cmdBunkerAdd = (LinkButton)e.Item.FindControl("cmdBunkerAdd"); 
                
                UserControlMaskNumber txtSulphurPercentageEdit = (UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit");

                if (txtSulphurPercentageEdit != null)
                {
                    if (drv["FLDFUELOIL"].ToString() == "1") txtSulphurPercentageEdit.Enabled = true;
                    else txtSulphurPercentageEdit.Enabled = false;
                }

                if (cmdBunkerAdd != null)
                {
                    RadLabel lblOilConsumptionIdItem = (RadLabel)e.Item.FindControl("lblOilConsumptionIdItem");
                    if (lblOilConsumptionIdItem != null)
                    {
                        if (General.GetNullableDecimal(drv["FLDBUNKEREDQTY"].ToString()) != null && drv["FLDFUELOIL"].ToString() == "1" && drv["FLDBUNKEREDQTY"].ToString() != "0.00")
                        {
                            cmdBunkerAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdBunkerAdd.CommandName);
                            cmdBunkerAdd.Attributes.Add("onclick"
                            , "javascript:Openpopup('codehelp1','','VesselPositionBunkerReceiptMaster.aspx?consumptionid="
                            + lblOilConsumptionIdItem.Text + "&oiltype=" + drv["FLDOILTYPECODE"].ToString() + " ');return true;");
                        }
                        else
                            cmdBunkerAdd.Visible = false;
                    }
                }

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCalculate");
                if (cb != null)
                {
                    RadLabel ldyn = (RadLabel)e.Item.FindControl("lbloilconsumptiononlaterdateyn");
                    if (ldyn != null)
                    {
                        if (ldyn.Text == "1")
                        {
                            cb.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Please confirm you want to proceed ?'); return false;");
                        }
                    }
                }

                UserControlMaskNumber ucAtSeaMEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaMEEdit");
                UserControlMaskNumber ucAtSeaAEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaAEEdit");
                UserControlMaskNumber ucAtSeaBLREdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaBLREdit");
                UserControlMaskNumber ucAtSeaIGGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaIGGEdit");
                UserControlMaskNumber ucAtSeaCARGOENGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOENGEdit");
                UserControlMaskNumber ucAtSeaCARGOHEATINdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOHEATINdit");
                UserControlMaskNumber ucAtSeaTKCLNGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaTKCLNGEdit");
                UserControlMaskNumber ucAtHourbourMEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourMEEdit");
                UserControlMaskNumber ucAtHourbourAEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourAEEdit");
                UserControlMaskNumber ucAtHourbourBLREdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourBLREdit");
                UserControlMaskNumber ucAtHourbourIGGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourIGGEdit");
                UserControlMaskNumber ucAtHourbourCARGOENGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOENGEdit");
                UserControlMaskNumber ucInPortMEEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortMEEdit");
                UserControlMaskNumber ucInPortAEEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortAEEdit");
                UserControlMaskNumber ucInPortBLREdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortBLREdit");
                UserControlMaskNumber ucInPortIGGEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortIGGEdit");
                UserControlMaskNumber ucInPortCARGOENGEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortCARGOENGEdit");
                UserControlMaskNumber ucAtHourbourCARGOHEATINdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOHEATINdit");
                UserControlMaskNumber ucAtHourbourTKCLNGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourTKCLNGEdit");

                if (ucAtSeaMEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtSeaMEEdit.Enabled = false; else ucAtSeaMEEdit.Enabled = true;
                if (ucAtSeaAEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (!drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtSeaAEEdit.Enabled = false; else ucAtSeaAEEdit.Enabled = true;
                if (ucAtSeaBLREdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaBLREdit.Enabled = false; else ucAtSeaBLREdit.Enabled = true;
                if (ucAtSeaIGGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaIGGEdit.Enabled = false; else ucAtSeaIGGEdit.Enabled = true;
                if (ucAtSeaCARGOENGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaCARGOENGEdit.Enabled = false; else ucAtSeaCARGOENGEdit.Enabled = true;
                if (ucAtSeaCARGOHEATINdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaCARGOHEATINdit.Enabled = false; else ucAtSeaCARGOHEATINdit.Enabled = true;
                if (ucAtSeaTKCLNGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaTKCLNGEdit.Enabled = false; else ucAtSeaTKCLNGEdit.Enabled = true;
                if (ucAtHourbourMEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtHourbourMEEdit.Enabled = false; else ucAtHourbourMEEdit.Enabled = true;
                if (ucAtHourbourAEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (!drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtHourbourAEEdit.Enabled = false; else ucAtHourbourAEEdit.Enabled = true;
                if (ucAtHourbourBLREdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourBLREdit.Enabled = false; else ucAtHourbourBLREdit.Enabled = true;
                if (ucAtHourbourIGGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourIGGEdit.Enabled = false; else ucAtHourbourIGGEdit.Enabled = true;
                if (ucAtSeaCARGOENGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourCARGOENGEdit.Enabled = false; else ucAtHourbourCARGOENGEdit.Enabled = true;
                if (ucInPortMEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucInPortMEEdit.Enabled = false; else ucInPortMEEdit.Enabled = true;
                if (ucInPortAEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (!drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucInPortAEEdit.Enabled = false; else ucInPortAEEdit.Enabled = true;
                if (ucInPortBLREdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucInPortBLREdit.Enabled = false; else ucInPortBLREdit.Enabled = true;
                if (ucInPortIGGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucInPortIGGEdit.Enabled = false; else ucInPortIGGEdit.Enabled = true;
                if (ucInPortCARGOENGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucInPortCARGOENGEdit.Enabled = false; else ucInPortCARGOENGEdit.Enabled = true;
                if (ucAtHourbourCARGOHEATINdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourCARGOHEATINdit.Enabled = false; else ucAtHourbourCARGOHEATINdit.Enabled = true;
                if (ucAtHourbourTKCLNGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourTKCLNGEdit.Enabled = false; else ucAtHourbourTKCLNGEdit.Enabled = true; 

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    protected void gvConsumption_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
 
            if (ViewState["NOONREPORTID"] == null || General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) == null)
            {
                ucError.ErrorMessage = "Please click the save button for the 'Shifting Report' and then update the Consumption details.";
                ucError.Visible = true;
                return;
            }

            if (Filter.CurrentVPRSShiftingReportSelection == null)
            {
                ucError.ErrorMessage = "Please fill in the Shifting Report Details and click on the save button.";
                ucError.Visible = true;
                return;
            }

            if (((RadLabel)e.Item.FindControl("lbloilconsumptiononlaterdateyn")).Text == "1")
            {
                ReCalculateROB(e);
            }
            else
            {
                decimal? bunkerqty = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtBunkeredEdit")).Text);
                if (bunkerqty != null && bunkerqty > 0 && ViewState["DEBUNKER"].ToString() == "1")
                {
                    bunkerqty = -1 * bunkerqty;
                }
                PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    new Guid(ViewState["NOONREPORTID"].ToString()),
                    new Guid(((RadLabel)e.Item.FindControl("lblOilTypeid")).Text),
                    "SHIFTING",
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaOTHEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourOTHEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortOTHEdit")).Text),
                    null,
                    null,
                    0, // ROBATNOON YN
                    0, // ROB @ EOSP AND ROB @ FWE YN
                    0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                    0, // RECALCULATE ROB
                    null,
                    null,
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                    null,
                    null
                    );

                PhoenixVesselPositionDepartureReport.UpdateDepartureOilConsumption(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOilConsumptionId")).Text)),
                    General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCodeEdit")).Text), "SHIFTING",
                    bunkerqty,
                    null,
                    null,
                    1, 1,
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit")).Text));

                ViewState["DEBUNKER"] = 0;
                RebingvConsumption();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    protected void gvConsumption_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("CALCULATE"))
            {
                if (ViewState["NOONREPORTID"] == null || General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Please click the save button for the 'Shifting Report' and then update the Consumption details.";
                    ucError.Visible = true;
                    return;
                }

                if (Filter.CurrentVPRSShiftingReportSelection == null)
                {
                    ucError.ErrorMessage = "Please fill in the Shifting Report Details and click on the save button.";
                    ucError.Visible = true;
                    return;
                }

                if (((RadLabel)e.Item.FindControl("lbloilconsumptiononlaterdateyn")).Text == "1")
                {
                    ReCalculateROB(e);
                }
                else
                {
                    decimal? bunkerqty = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtBunkeredEdit")).Text);
                    if (bunkerqty != null && bunkerqty > 0 && ViewState["DEBUNKER"].ToString() == "1")
                    {
                        bunkerqty = -1 * bunkerqty;
                    }
                    PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    new Guid(ViewState["NOONREPORTID"].ToString()),
                    new Guid(((RadLabel)e.Item.FindControl("lblOilTypeid")).Text),
                    "SHIFTING",
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaOTHEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourOTHEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortMEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortAEEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortBLREdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortIGGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortCARGOENGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortOTHEdit")).Text),
                    null,
                    null,
                    0, // ROBATNOON YN
                    0, // ROB @ EOSP AND ROB @ FWE YN
                    0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                    0, // RECALCULATE ROB
                    null,
                    null,
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                    null,
                    null
                    );

                    PhoenixVesselPositionDepartureReport.UpdateDepartureOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid((((RadLabel)e.Item.FindControl("lblOilConsumptionId")).Text)),
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCodeEdit")).Text), "SHIFTING",
                        bunkerqty,
                        null,
                        null,
                        1,1,
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit")).Text));
                    ViewState["DEBUNKER"] = 0;
                    RebingvConsumption();
                }
            }


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (ViewState["NOONREPORTID"] != null && General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) != null)
                {
                    PhoenixVesselPositionNoonReportOilConsumption.DeleteOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()),
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeid")).Text));

                    RadLabel OilConsumptionId = ((RadLabel)e.Item.FindControl("lblOilConsumptionIdItem"));
                    if (OilConsumptionId.Text != "")
                        PhoenixVesselPositionDepartureReport.DeleteDepartureOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid((OilConsumptionId.Text)));

                }

                RebingvConsumption();
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string ss = e.CommandArgument.ToString();
                if (ss == "cmdDeBunker")
                    ViewState["DEBUNKER"] = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ReCalculateROB(GridCommandEventArgs gvr)
    {
        try
        {
            decimal? bunkerqty = General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("txtBunkeredEdit")).Text);
            if (bunkerqty != null && bunkerqty > 0 && ViewState["DEBUNKER"].ToString() == "1")
            {
                bunkerqty = -1 * bunkerqty;
            }

            PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(ViewState["NOONREPORTID"].ToString()),
                new Guid(((RadLabel)gvr.Item.FindControl("lblOilTypeid")).Text),
                "SHIFTING",
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucInPortMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucInPortAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucInPortBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucInPortIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucInPortCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucInPortOTHEdit")).Text),
                null,
                null,
                0, // ROBATNOON YN
                0, // ROB @ EOSP AND ROB @ FWE YN
                0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                0, // RECALCULATE ROB
                null,
                null,
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                null,
                null
                );

            PhoenixVesselPositionDepartureReport.UpdateDepartureOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid((((RadLabel)gvr.Item.FindControl("lblOilConsumptionId")).Text)),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection),
                General.GetNullableGuid(((RadLabel)gvr.Item.FindControl("lblOilTypeCodeEdit")).Text), "SHIFTING",
                bunkerqty,
                null,
                null,
                1, 1,
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.Item.FindControl("txtSulphurPercentageEdit")).Text));

            ucStatus.Text = "ROB Recalculated";
            ViewState["DEBUNKER"] = 0;
            RebingvConsumption();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkCounterDefective_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCounterDefective.Checked == true)
        {
            txtEMLogManoeuveringDistance.CssClass = "input";
            txtEMLogManoeuveringDistance.Enabled = true;
        }
        else
        {
            txtEMLogManoeuveringDistance.CssClass = "readonlytextbox";
            txtEMLogManoeuveringDistance.Enabled = false;
        }
    }

    protected void gvServices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureServices(
            General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection == null ? "" : Filter.CurrentVPRSShiftingReportSelection), 1);

            gvServices.DataSource = ds;
   
    }
    protected void gvConsumption_PreRender(object sender, EventArgs e)
    {
        if (gvConsumption.EditItems.Count > 0)
        {
            foreach (GridDataItem item in gvConsumption.Items)
            {
                if (item.Edit)
                {
                    item["meinport"].ColumnSpan = 8;
                    item["aeinport"].Visible = false;
                    item["blrinport"].Visible = false;
                    item["igginport"].Visible = false;
                    item["cenginport"].Visible = false;
                    item["cthginport"].Visible = false;
                    item["tkclnginport"].Visible = false;
                    item["othinport"].Visible = false;

                    item["meatharbour"].ColumnSpan = 8;
                    item["aeatharbour"].Visible = false;
                    item["blratharbour"].Visible = false;
                    item["iggatharbour"].Visible = false;
                    item["cngatharbour"].Visible = false;
                    item["chtgatharbour"].Visible = false;
                    item["tkclngatharbour"].Visible = false;
                    item["othatharbour"].Visible = false;

                    item["bunker"].ColumnSpan = 3;
                    item["sulphur"].Visible = false;
                    item["robonsbe"].Visible = false;
                }
            }
        }
    }

    protected void gvServices1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dts = PhoenixVesselPositionDepartureReport.ListDepartureServices(
            General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection == null ? "" : Filter.CurrentVPRSShiftingReportSelection), 0);

        gvServices1.DataSource = dts;
    }
    protected void RebindgvServices()
    {
        gvServices.SelectedIndexes.Clear();
        gvServices.EditIndexes.Clear();
        gvServices.DataSource = null;
        gvServices.Rebind();
    }
    protected void RebindgvServices1()
    {
        gvServices1.SelectedIndexes.Clear();
        gvServices1.EditIndexes.Clear();
        gvServices1.DataSource = null;
        gvServices1.Rebind();
    }

    protected void gvConsumption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        int? iVesselId = null;

        if (ViewState["VESSELID"].ToString() != "")
        {
            iVesselId = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        }
        else
            iVesselId = 0;

        string noonreportid = "";

        noonreportid = (ViewState["NOONREPORTID"] != null && General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) != null) ? ViewState["NOONREPORTID"].ToString() : "";

        ds = PhoenixVesselPositionNoonReportOilConsumption.ListOilConsumption(
            iVesselId,
            General.GetNullableGuid(noonreportid), "SHIFTING");

            gvConsumption.DataSource = ds;
 
    }
    protected void RebingvConsumption()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }
}
