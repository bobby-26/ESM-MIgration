using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselPositionDepartureReportEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            txtVoyageId.Attributes.Add("style", "visibility:hidden");
            txtNewVoyageId.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "DEPARTUREREPORT");
            toolbarmain.AddButton("Departure Report", "DEPARTURE");
            toolbarmain.AddButton("Operations", "OPERATIONS");
            toolbarmain.AddButton("Emission In Port", "MRVSUMMARY");
            MenuDRSubTab.AccessRights = this.ViewState;
            MenuDRSubTab.MenuList = toolbarmain.Show();
            MenuDRSubTab.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send To Office", "SENDTOOFFICE",ToolBarDirection.Right);
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
                    imgShowNewVoyage.Attributes.Add("onclick", "return showPickList('spnNewVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&voyageid=" + txtVoyageId.Text + "', true); ");

                    UcVessel.Enabled = false;
                }

                if ((Request.QueryString["mode"] != null) && (Request.QueryString["mode"].ToString().Equals("NEW")))
                {
                    Filter.CurrentVPRSDepartureReportSelection = null;
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "0";
                    ucDeparturePort.VesselId = ViewState["VESSELID"].ToString();
                    Reset();
                    txtCOSP.Text = DateTime.UtcNow.ToShortDateString();
                    btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                    imgShowNewVoyage.Attributes.Add("onclick", "return showPickList('spnNewVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&voyageid=" + txtVoyageId.Text + "', true); ");

                    DataSet dsCurrentvoyage = PhoenixVesselPositionVoyageData.CurrentVoyageData(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (dsCurrentvoyage.Tables.Count > 0)
                    {
                        if (dsCurrentvoyage.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = dsCurrentvoyage.Tables[0].Rows[0];

                            txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                            txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                            ucDeparturePort.VoyageId = txtVoyageId.Text;
                        }
                    }
                }

                ViewState["NOONREPORTID"] = null;
                VesselDepartureEdit();
                ViewState["DEPARTUREPORTCALLID"] = ucDeparturePort.SelectedPortCallValue;
                VesselVoyageDetailsEdit();
            }

            if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString().Equals("NEW"))
            {
                txtCOSP.Enabled = true;
                txtCOSP.CssClass = "input";
                txtCOSPTime.Enabled = true;
                txtCOSPTime.CssClass = "input";
            }
            else
            {
                txtCOSP.Enabled = false;
                txtCOSP.CssClass = "readonlytextbox";
                txtCOSPTime.Enabled = false;
                txtCOSPTime.CssClass = "readonlytextbox";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Filter.CurrentVPRSDepartureReportSelection != null)
        {
            if (CommandName.ToUpper().Equals("OPERATIONS"))
            {
                Response.Redirect("../VesselPosition/VesselPositionDepartureReportOperations.aspx");
            }

            if (CommandName.ToUpper().Equals("MRVSUMMARY"))
            {
                Response.Redirect("../VesselPosition/VesselPositionDepartureReportMRVSummary.aspx");
            }
        }
        if (CommandName.ToUpper().Equals("DEPARTUREREPORT"))
        {
            if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                Response.Redirect("VesselPositionReports.aspx", false);
            else
                Response.Redirect("VesselPositionDepartureReport.aspx", false);
            //Response.Redirect("../VesselPosition/VesselPositionDepartureReport.aspx");
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
        string VoyageId = (txtNewVoyageId.Text == "" ? null : txtNewVoyageId.Text) ?? (ViewState["VOYAGEID"].ToString() == "" ? null : ViewState["VOYAGEID"].ToString()) ?? (txtVoyageId.Text == "" ? null : txtVoyageId.Text);  
        DataSet ds = PhoenixVesselPositionDepartureReport.GetVoyageDetailsForDepartureReport(
            General.GetNullableGuid(VoyageId),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(ViewState["DEPARTUREPORTCALLID"].ToString()));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (Filter.CurrentVPRSDepartureReportSelection == null)
                {
                    ucNextPort.SelectedValue = dr["FLDNEXTPORT"].ToString();
                    ucNextPort.SelectedPortCallValue = dr["FLDNEXTPORTCALLID"].ToString();
                    ucNextPort.Text = dr["FLDNEXTPORTNAME"].ToString();
                }
            }
        }

        if (txtVoyageId.Text != "")
        {
            ucNextPort.VoyageId = (txtNewVoyageId.Text == "" ? txtVoyageId.Text : txtNewVoyageId.Text);
            ucNextPort.VesselId = (ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "");
            ucNextPort.bind();
        }
    }

    protected void VesselDepartureEdit()
    {
        try
        {
            if (Filter.CurrentVPRSDepartureReportSelection != null)
            {
                DataSet ds = PhoenixVesselPositionDepartureReport.EditDepartureReport(General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection));
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

                        txtDate.Text = dr["FLDVESSELDEPARTUREDATE"].ToString();
                        if (General.GetNullableDateTime(dr["FLDVESSELDEPARTUREDATE"].ToString()) != null)
                            txtDepTime.SelectedDate = Convert.ToDateTime(dr["FLDVESSELDEPARTUREDATE"]);
                        ViewState["VOYAGEID"] = dr["FLDVOYAGEID"].ToString();
                        ViewState["DEPARTUREPORTCALLID"] = dr["FLDCURRENTPORTCALLID"].ToString();
                        txtETA.Text = dr["FLDETA"].ToString();
                        if (General.GetNullableDateTime(dr["FLDETA"].ToString()) != null)
                            txtTimeOfETA.SelectedDate = Convert.ToDateTime(dr["FLDETA"]);
                        ucNextPort.Text = dr["FLDNEXTPORTNAME"].ToString();
                        ucNextPort.SelectedValue = dr["FLDNEXTPORT"].ToString();
                        ucNextPort.SelectedPortCallValue = dr["FLDNEXTPORTCALLID"].ToString(); 
                        txtDistance.Text = dr["FLDDISTANCETONEXTPORT"].ToString();
                        txtNextPortOperation.SelectedPortactivity = dr["FLDNEXTPORTOPERATION"].ToString();
                        txtRemarks.Text = dr["FLDREMARKS"].ToString();
                        UcVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                        ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                        UcVessel.Enabled = false;

                        txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                        txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                        btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");

                        txtNewVoyageId.Text = dr["FLDNEWVOYAGEID"].ToString();
                        txtNewVoyageName.Text = dr["FLDNEWVOYAGENO"].ToString();
                        imgShowNewVoyage.Attributes.Add("onclick", "return showPickList('spnNewVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&voyageid=" + txtVoyageId.Text + "', true); ");

                        txtSBE.Text = dr["FLDSBE"].ToString();
                        if (General.GetNullableDateTime(dr["FLDSBE"].ToString()) != null)
                            txtSBETime.SelectedDate = Convert.ToDateTime(dr["FLDSBE"]);
                        txtCOSP.Text = dr["FLDCOSP"].ToString();
                        if (General.GetNullableDateTime(dr["FLDCOSP"].ToString()) != null)
                            txtCOSPTime.SelectedDate = Convert.ToDateTime(dr["FLDCOSP"]);
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

                        ucLatitude.TextDegree = dr["FLDLAT1"].ToString();
                        ucLatitude.TextMinute = dr["FLDLAT2"].ToString();
                        ucLatitude.TextSecond = dr["FLDLAT3"].ToString();
                        ucLatitude.TextDirection = dr["FLDLATDIRECTION"].ToString();
                        ucLongitude.TextDegree = dr["FLDLONG1"].ToString();
                        ucLongitude.TextMinute = dr["FLDLONG2"].ToString();
                        ucLongitude.TextSecond = dr["FLDLONG3"].ToString();
                        ucLongitude.TextDirection = dr["FLDLONGDIRECTION"].ToString();
                        txtoffhiredelay.Text = dr["FLDOFFHIREDELAY"].ToString();

                        if (General.GetNullableString(dr["FLDBALLASTYN"].ToString()) != null)
                            rbtnBallastLaden.SelectedValue = dr["FLDBALLASTYN"].ToString();
                        if (dr["FLDCONFIRMEDYN"].ToString() == "1")
                        {
                            ProjectBilling.Visible = false;
                            lblAlertSenttoOFC.Visible = true ;
                        }

                        if (General.GetNullableGuid(dr["FLDNEWVOYAGEID"].ToString()) != null)
                        {
                            imgShowNewVoyage.Visible = false;
                        }

                        txtShipMeanTime.Text = dr["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
                        txtShipMeanTimeSymbol.SelectedValue = dr["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";

                        txtUTCDate.Text = General.GetDateTimeToString(dr["FLDUTCDATETIME"].ToString());
                        if (General.GetNullableDateTime(dr["FLDUTCDATETIME"].ToString()) != null)
                            txtUTCTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCDATETIME"]);

                        txtUTCPOB.Text = dr["FLDUTCPOB"].ToString();
                        if (General.GetNullableDateTime(dr["FLDUTCPOB"].ToString()) != null)
                            txtUTCPOBTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCPOB"]);
                        txtUTCLLC.Text = dr["FLDUTCLLC"].ToString();
                        if (General.GetNullableDateTime(dr["FLDUTCLLC"].ToString()) != null)
                            txtUTCLLCTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCLLC"]);
                        txtUTCAAW.Text = dr["FLDUTCAAW"].ToString();
                        if (General.GetNullableDateTime(dr["FLDUTCAAW"].ToString()) != null)
                            txtUTCAAWTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCAAW"]);
                        txtUTCDLOSP.Text = dr["FLDUTCDLOSP"].ToString();
                        if (General.GetNullableDateTime(dr["FLDUTCDLOSP"].ToString()) != null)
                            txtUTCDLOSPTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCDLOSP"]);
                        txtUTCCOSP.Text = dr["FLDUTCDATETIME"].ToString();
                        if (General.GetNullableDateTime(dr["FLDUTCDATETIME"].ToString()) != null)
                            txtUTCCOSPTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCDATETIME"]);
                        txtUTCSBE.Text = dr["FLDUTCSBE"].ToString();
                        if (General.GetNullableDateTime(dr["FLDUTCSBE"].ToString()) != null)
                            txtUTCSBETime.SelectedDate = Convert.ToDateTime(dr["FLDUTCSBE"]);
                        txtEMLogcounteratCOSP.Text = dr["FLDEMLOGCOUNTERATCOSP"].ToString();
                        txtEMLogManoeuveringDistance.Text = dr["FLDEMLOGMANOUVERINGDIST"].ToString();
                        txtEMLogcounteratSBE.Text = dr["FLDEMLOGCOUNTERATSBE"].ToString();
                        txtRFA.Text = dr["FLDRFA"].ToString();
                        if (General.GetNullableDateTime(dr["FLDRFA"].ToString()) != null)
                            txtRFATime.SelectedDate = Convert.ToDateTime(dr["FLDRFA"]);
                        txtUTCRFA.Text = dr["FLDUTCRFA"].ToString();
                        if (General.GetNullableDateTime(dr["FLDUTCRFA"].ToString()) != null)
                            txtUTCRFATime.SelectedDate = Convert.ToDateTime(dr["FLDUTCRFA"]);

                        txtMERevCounter.Text = dr["FLDMEREVCOUNTER"].ToString();

                        chkCounterDefective.Checked = dr["FLDEMLOGCOUNTERDEFECTIVEYN"].ToString().Equals("1") ? true : false;
                        chkRevCounterDefective.Checked = dr["FLDEMREVCOUNTERDEFECTIVEYN"].ToString().Equals("1") ? true : false;

                        ChkOptimumSpeed.Checked = dr["FLDOPTIMUMSPEEDYN"].ToString().Equals("1") ? true : false;
                        chkOptimumTrim.Checked = dr["FLDOPTIMUMTIRMYN"].ToString().Equals("1") ? true : false;
                        chkMostEfficientRoute.Checked = dr["FLDMOSTEFFICIENTROUTEYN"].ToString().Equals("1") ? true : false;
                        chkCargoStowage.Checked = dr["FLDCARGOSTOWAGE"].ToString().Equals("1") ? true : false;
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

                    DataSet ds = PhoenixVesselPositionDepartureReport.EditFirstDepartureReport(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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

                            //txtDate.Text = dr["FLDDEPARTUREDATE"].ToString();

                            txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                            txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();

                            txtBirthName.Text = dr["FLDNAMEOFBERTH"].ToString();

                            ucLatitude.TextDegree = dr["FLDLAT1"].ToString();
                            ucLatitude.TextMinute = dr["FLDLAT2"].ToString();
                            ucLatitude.TextSecond = dr["FLDLAT3"].ToString();
                            ucLatitude.TextDirection = dr["FLDLATDIRECTION"].ToString();
                            ucLongitude.TextDegree = dr["FLDLONG1"].ToString();
                            ucLongitude.TextMinute = dr["FLDLONG2"].ToString();
                            ucLongitude.TextSecond = dr["FLDLONG3"].ToString();
                            ucLongitude.TextDirection = dr["FLDLONGDIRECTION"].ToString();

                            //txtPOB.Text = dr["FLDDEPARTUREDATE"].ToString();
                            txtSBE.Text = dr["FLDDEPARTUREDATE"].ToString();
                            //txtLLC.Text = dr["FLDDEPARTUREDATE"].ToString();
                            //txtAAW.Text = dr["FLDDEPARTUREDATE"].ToString();
                            //txtDLOSP.Text = dr["FLDDEPARTUREDATE"].ToString();
                            txtCOSP.Text = dr["FLDDEPARTUREDATE"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Reset()
    {
        txtDate.Text = "";
        txtVoyageName.Text = "";
        txtVoyageId.Text = "";
        ViewState["VOYAGEID"] = "";
        ucDeparturePort.SelectedValue = "";
        ucDeparturePort.SelectedPortCallValue = "";
        ucDeparturePort.Text = "";
        txtETA.Text = "";
        txtDepTime.SelectedDate = null;
        ucNextPort.SelectedValue = "";
        ucNextPort.SelectedPortCallValue = "";
        ucNextPort.Text = "";
        txtDistance.Text = "";
        txtNextPortOperation.SelectedPortactivity = "";
        txtRemarks.Text = "";
        txtSBE.Text = "";
        txtSBETime.SelectedDate = null;
        txtCOSP.Text = DateTime.UtcNow.ToShortDateString();
        txtCOSPTime.SelectedDate = null;
        txtSludgeLanded.Text = "";
        txtSludgeLandedTime.Text = "";
        txtLOSampleLanded.Text = "";
        txtLOSampleLandedTime.Text = "";

        txtBirthName.Text = "";
        txtPOB.Text = "";
        txtPOBTime.SelectedDate = null;
        txtLLC.Text = "";
        txtLLCTime.SelectedDate = null;
        txtAAW.Text = "";
        txtAAWTime.SelectedDate = null;
        txtDLOSP.Text = "";
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

        ucLatitude.TextDegree = "";
        ucLatitude.TextMinute = "";
        ucLatitude.TextSecond = "";
        ucLatitude.TextDirection = "";
        ucLongitude.TextDegree = "";
        ucLongitude.TextMinute = "";
        ucLongitude.TextSecond = "";
        ucLongitude.TextDirection = "";

    }

    protected void ProjectBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SENDTOOFFICE"))           
            {
                
                if (Filter.CurrentVPRSDepartureReportSelection != null)
                {
                    string depTime = txtCOSPTime.SelectedTime != null ? txtCOSPTime.SelectedTime.Value.ToString() : "";
                    string sbeTime = txtSBETime.SelectedTime != null ? txtSBETime.SelectedTime.Value.ToString() : "";
                    string POBTime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
                    string LLCTime = txtLLCTime.SelectedTime != null ? txtLLCTime.SelectedTime.Value.ToString() : "";
                    string AAWTime = txtAAWTime.SelectedTime != null ? txtAAWTime.SelectedTime.Value.ToString() : "";
                    string DLOSPTime = txtDLOSPTime.SelectedTime != null ? txtDLOSPTime.SelectedTime.Value.ToString() : "";

                    //if (!IsValidDepartureReport(UcVessel.SelectedVessel, txtVoyageId.Text, ucDeparturePort.SelectedPortCallValue, txtCOSP.Text + " " + depTime, txtSBE.Text + " " + sbeTime, txtPOB.Text + " " + POBTime, txtLLC.Text + " " + LLCTime, txtAAW.Text + " " + AAWTime, txtDLOSP.Text + " " + DLOSPTime))
                    //{
                    //    ucError.Visible = true;
                    //    return;
                    //}
                    //else
                    //{
                    if (CommandName.ToUpper().Equals("SENDTOOFFICE"))
                    {
                        UpdateDeparture(1);
                        InsertVoyageData();
                    }
                    else
                    {
                        UpdateDeparture(0);
                    }
                        UpdateDepartureServices();
                        InsertOtherOilConsumption();
                        ucStatus.Text = "Departure report updated";

                    //}
                }
                else
                {
                    string depTime = txtCOSPTime.SelectedTime != null ? txtCOSPTime.SelectedTime.Value.ToString() : "";
                    string sbeTime = txtSBETime.SelectedTime != null ? txtSBETime.SelectedTime.Value.ToString() : "";
                    string POBTime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
                    string LLCTime = txtLLCTime.SelectedTime != null ? txtLLCTime.SelectedTime.Value.ToString() : "";
                    string AAWTime = txtAAWTime.SelectedTime != null ? txtAAWTime.SelectedTime.Value.ToString() : "";
                    string DLOSPTime = txtDLOSPTime.SelectedTime != null ? txtDLOSPTime.SelectedTime.Value.ToString() : "";

                    //if (!IsValidDepartureReport(UcVessel.SelectedVessel, txtVoyageId.Text, ucDeparturePort.SelectedPortCallValue, txtCOSP.Text + " " + depTime, txtSBE.Text + " " + sbeTime, txtPOB.Text + " " + POBTime, txtLLC.Text + " " + LLCTime, txtAAW.Text + " " + AAWTime, txtDLOSP.Text + " " + DLOSPTime))
                    //{
                    //    ucError.Visible = true;
                    //    return;
                    //}
                    //else
                    //{
                        if (CommandName.ToUpper().Equals("SENDTOOFFICE"))
                        {
                            AddDeparture(1);
                            InsertVoyageData();
                        }
                        else
                        {
                            AddDeparture(0); 
                        }
                    
                    UpdateDepartureServices();
                    InsertOtherOilConsumption();
                    ucStatus.Text = "Departure report added";
                    Response.Redirect("../VesselPosition/VesselPositionDepartureReportEdit.aspx?VESSELDEPARTUREID=" + Filter.CurrentVPRSDepartureReportSelection, false);
                    //}
                }
                VesselDepartureEdit();
                RebindgvServices();
                RebindgvServices1();
                RebindgvConsumption();
                RebindgvFW();
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
    private void InsertVoyageData()
    {
        PhoenixVesselPositionEUMRVSummaryReport.EUReportingInsert(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
             General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection));

    }
    private void AddDeparture(int confirm)
    {
        string timeofETA = txtTimeOfETA.SelectedTime != null ? txtTimeOfETA.SelectedTime.Value.ToString() : "";
        string sbetime = txtSBETime.SelectedTime != null ? txtSBETime.SelectedTime.Value.ToString() : "";  
        string depTime = txtDepTime.SelectedTime != null ? txtDepTime.SelectedTime.Value.ToString() : "";
        string cosptime = txtCOSPTime.SelectedTime != null ? txtCOSPTime.SelectedTime.Value.ToString() : "";
        string sludgelandtime = txtSludgeLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtSludgeLandedTime.Text;
        string lolandtime = txtLOSampleLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtLOSampleLandedTime.Text;

        string pobtime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string llctime = txtLLCTime.SelectedTime != null ? txtLLCTime.SelectedTime.Value.ToString() : "";
        string aawtime = txtAAWTime.SelectedTime != null ? txtAAWTime.SelectedTime.Value.ToString() : "";
        string dlosptime = txtDLOSPTime.SelectedTime != null ? txtDLOSPTime.SelectedTime.Value.ToString() : "";
        string rfatime = txtRFATime.SelectedTime != null ? txtRFATime.SelectedTime.Value.ToString() : "";
        string bilgewaterlandedtime = txtBilgeWaterLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeWaterLandedTime.Text;
        string garbagelandedtime = txtGarbageLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtGarbageLandedTime.Text;


        Guid? vesseldepartureid = null;
        Guid? noonreportid = null;

        PhoenixVesselPositionDepartureReport.InsertDepartureReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(txtVoyageId.Text),
            General.GetNullableDateTime(txtCOSP.Text + " " + cosptime),
            General.GetNullableGuid(ucDeparturePort.SelectedPortCallValue),
            General.GetNullableDateTime(txtETA.Text + " " + timeofETA),
            General.GetNullableGuid(ucNextPort.SelectedPortCallValue),
            General.GetNullableDecimal(txtDistance.Text),
            txtNextPortOperation.SelectedPortactivity,
            txtRemarks.Text,
            ref vesseldepartureid,
            General.GetNullableDateTime(txtSBE.Text + " " + sbetime),
            General.GetNullableDateTime(txtCOSP.Text + " " + cosptime),
            General.GetNullableDateTime(txtSludgeLanded.Text + " " + sludgelandtime),
            General.GetNullableDateTime(txtLOSampleLanded.Text),
            txtBirthName.Text,
            ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute,
            ucLatitude.TextSecond, ucLongitude.TextSecond, ucLatitude.TextDirection, ucLongitude.TextDirection,
            General.GetNullableDateTime(txtPOB.Text + " " + pobtime),
            General.GetNullableDateTime(txtLLC.Text + " " + llctime), General.GetNullableDateTime(txtAAW.Text + " " + aawtime),
            General.GetNullableDateTime(txtDLOSP.Text + " " + dlosptime),
            General.GetNullableDecimal(txtManoevering.Text),
            General.GetNullableDecimal(txtHarbourSteamingDist.Text),
            General.GetNullableDecimal(txtSludgeLandedQty.Text),
            General.GetNullableDecimal(txtBilgeWaterLandedQty.Text),
            General.GetNullableDateTime(txtBilgeWaterLanded.Text + " " + bilgewaterlandedtime),
            General.GetNullableDecimal(txtGarbageLandedQty.Text),
            General.GetNullableDateTime(txtGarbageLanded.Text + " " + garbagelandedtime),
            0, //0 departure report 1 shifting report
            General.GetNullableInteger(rbtnBallastLaden.SelectedValue),
            ref noonreportid,
            General.GetNullableDecimal(txtoffhiredelay.Text),confirm,
            General.GetNullableInteger(rbLOSampleLanded.SelectedValue.ToString()),
            (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text),
            General.GetNullableDecimal(txtEMLogcounteratCOSP.Text),
            General.GetNullableDateTime(txtRFA.Text + " " + rfatime),
            General.GetNullableDecimal(txtMERevCounter.Text),
            General.GetNullableDecimal(txtEMLogcounteratSBE.Text),
            General.GetNullableDecimal(txtEMLogManoeuveringDistance.Text),
            chkCounterDefective.Checked == true ? 1 : 0,
            chkRevCounterDefective.Checked == true ? 1 : 0,
            ChkOptimumSpeed.Checked == true ? 1 : 0,
            chkOptimumTrim.Checked == true ? 1 : 0,
            chkMostEfficientRoute.Checked == true ? 1 : 0,
            chkCargoStowage.Checked == true ? 1 : 0,
            chkOffPortLimits.Checked == true ? 1 : 0); 

        ViewState["NOONREPORTID"] = noonreportid;

        Filter.CurrentVPRSDepartureReportSelection = vesseldepartureid.ToString();

        if (General.GetNullableGuid(txtNewVoyageId.Text) != null)
        {
            PhoenixVesselPositionVoyageData.UpdateStartNewVoyage(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(txtVoyageId.Text),
                General.GetNullableGuid(txtNewVoyageId.Text),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableInteger(ucDeparturePort.SelectedValue),
                General.GetNullableDateTime(txtCOSP.Text + " " + cosptime),
                General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                null);
        }
    }

    private void UpdateDeparture(int confirm)
    {
        string timeofETA = txtTimeOfETA.SelectedTime != null ? txtTimeOfETA.SelectedTime.Value.ToString() : "";
        string sbetime = txtSBETime.SelectedTime != null ? txtSBETime.SelectedTime.Value.ToString() : "";
        string depTime = txtDepTime.SelectedTime != null ? txtDepTime.SelectedTime.Value.ToString() : "";
        string cosptime = txtCOSPTime.SelectedTime != null ? txtCOSPTime.SelectedTime.Value.ToString() : "";
        string sludgelandtime = txtSludgeLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtSludgeLandedTime.Text;
        string lolandtime = txtLOSampleLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtLOSampleLandedTime.Text;

        string pobtime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string llctime = txtLLCTime.SelectedTime != null ? txtLLCTime.SelectedTime.Value.ToString() : "";
        string aawtime = txtAAWTime.SelectedTime != null ? txtAAWTime.SelectedTime.Value.ToString() : "";
        string dlosptime = txtDLOSPTime.SelectedTime != null ? txtDLOSPTime.SelectedTime.Value.ToString() : "";
        string rfatime = txtRFATime.SelectedTime != null ? txtRFATime.SelectedTime.Value.ToString() : "";
        string bilgewaterlandedtime = txtBilgeWaterLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeWaterLandedTime.Text;
        string garbagelandedtime = txtGarbageLandedTime.Text.Trim().Equals("__:__") ? string.Empty : txtGarbageLandedTime.Text;

        Guid? noonreportid = null;

        PhoenixVesselPositionDepartureReport.UpdateDepartureReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(txtVoyageId.Text),
            General.GetNullableDateTime(txtCOSP.Text + " " + cosptime),
            General.GetNullableGuid(ucDeparturePort.SelectedPortCallValue),
            General.GetNullableDateTime(txtETA.Text + " " + timeofETA),
            General.GetNullableGuid(ucNextPort.SelectedPortCallValue),
            General.GetNullableDecimal(txtDistance.Text),
            txtNextPortOperation.SelectedPortactivity,
            txtRemarks.Text,
            General.GetNullableDateTime(txtSBE.Text + " " + sbetime),
            General.GetNullableDateTime(txtCOSP.Text + " " + cosptime),
            General.GetNullableDateTime(txtSludgeLanded.Text + " " + sludgelandtime),
            General.GetNullableDateTime(txtLOSampleLanded.Text),
            txtBirthName.Text,
            ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute,
            ucLatitude.TextSecond, ucLongitude.TextSecond, ucLatitude.TextDirection, ucLongitude.TextDirection,
            General.GetNullableDateTime(txtPOB.Text + " " + pobtime),
            General.GetNullableDateTime(txtLLC.Text + " " + llctime), General.GetNullableDateTime(txtAAW.Text + " " + aawtime),
            General.GetNullableDateTime(txtDLOSP.Text + " " + dlosptime),
            General.GetNullableDecimal(txtManoevering.Text),
            General.GetNullableDecimal(txtHarbourSteamingDist.Text),
            General.GetNullableDecimal(txtSludgeLandedQty.Text),
            General.GetNullableDecimal(txtBilgeWaterLandedQty.Text),
            General.GetNullableDateTime(txtBilgeWaterLanded.Text + " " + bilgewaterlandedtime),
            General.GetNullableDecimal(txtGarbageLandedQty.Text),
            General.GetNullableDateTime(txtGarbageLanded.Text + " " + garbagelandedtime),
            General.GetNullableInteger(rbtnBallastLaden.SelectedValue),
            ref noonreportid, General.GetNullableDecimal(txtoffhiredelay.Text),confirm,
            General.GetNullableInteger(rbLOSampleLanded.SelectedValue.ToString()),
            (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text),
            General.GetNullableDecimal(txtEMLogcounteratCOSP.Text),
            General.GetNullableDateTime(txtRFA.Text + " " + rfatime),
            General.GetNullableDecimal(txtMERevCounter.Text),
            General.GetNullableDecimal(txtEMLogcounteratSBE.Text),
            General.GetNullableDecimal(txtEMLogManoeuveringDistance.Text),
            chkCounterDefective.Checked == true ? 1 : 0,
            chkRevCounterDefective.Checked == true ? 1 : 0,
            ChkOptimumSpeed.Checked == true ? 1 : 0,
            chkOptimumTrim.Checked == true ? 1 : 0,
            chkMostEfficientRoute.Checked == true ? 1 : 0,
            chkCargoStowage.Checked == true ? 1 : 0,
            chkOffPortLimits.Checked == true ? 1 : 0);

        ViewState["NOONREPORTID"] = noonreportid;

        if (General.GetNullableGuid(txtNewVoyageId.Text) != null)
        {
            PhoenixVesselPositionVoyageData.UpdateStartNewVoyage(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(txtVoyageId.Text),
                General.GetNullableGuid(txtNewVoyageId.Text),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableInteger(ucDeparturePort.SelectedValue),
                General.GetNullableDateTime(txtCOSP.Text + " " + cosptime),
                General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                null);
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        RebindgvConsumption();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
        RebindgvConsumption();
        ucDeparturePort.VesselId = UcVessel.SelectedVessel;
        LastArrivalPort();
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        ViewState["VOYAGEID"] = txtVoyageId.Text;
        if (ucDeparturePort.VoyageId != txtVoyageId.Text)
        {
            ucDeparturePort.Text = "";
            ucDeparturePort.SelectedValue = "";
            ucDeparturePort.SelectedPortCallValue = "";
        }
        ucDeparturePort.VoyageId = txtVoyageId.Text;

        VesselVoyageDetailsEdit();
        RebindgvConsumption();

        //if (ucNextPort.VoyageId != txtNewVoyageId.Text)
        //{
            ucNextPort.Text = "";
            ucNextPort.SelectedValue = "";
            ucNextPort.SelectedPortCallValue = "";

        //}
        ucNextPort.VoyageId = (txtNewVoyageId.Text == "" ? txtVoyageId.Text : txtNewVoyageId.Text);
        ucNextPort.VesselId = UcVessel.SelectedVessel;
        ucNextPort.bind();
    }

    protected void ucDeparturePort_Changed(object sender, EventArgs e)
    {
        ViewState["DEPARTUREPORTCALLID"] = ucDeparturePort.SelectedPortCallValue;
        VesselVoyageDetailsEdit();
        RebindgvConsumption();
    }

    //private bool IsValidDepartureReport(string vesselid, string voyageid, string port,string departuredate,string SBE,string POB,string LLC,string AAW,string DLOSP)
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";

    //    if (General.GetNullableInteger(UcVessel.SelectedVessel) == null)
    //        ucError.ErrorMessage = "Vessel is required.";

    //    if (General.GetNullableGuid(voyageid) == null)
    //        ucError.ErrorMessage = "Voyage is required.";

    //    if (General.GetNullableGuid(port) == null)
    //        ucError.ErrorMessage = "Port is required.";

    //    if (General.GetNullableDateTime(departuredate) == null)
    //        ucError.ErrorMessage = "Commencement of Sea Passage (COSP) is required.";

    //    if (rbLOSampleLanded.SelectedValue == "1" && General.GetNullableDateTime(txtLOSampleLanded.Text) == null)
    //        ucError.ErrorMessage = "LO Sample Landed is required.";

    //    if (General.GetNullableDateTime(SBE) == null)
    //    {
    //        ucError.ErrorMessage = "Standby Engine(SBE) is required.";

    //    }
    //    if (General.GetNullableDateTime(SBE) != null && General.GetNullableDateTime(LLC) != null && General.GetNullableDateTime(departuredate) != null)
    //    {
    //        if (General.GetNullableDateTime(LLC) < General.GetNullableDateTime(SBE))
    //        {
    //            ucError.ErrorMessage = "All Gone and Clear (LLC) Time should be later than Standby Engine(SBE) Time.";
    //        }
    //        if (General.GetNullableDateTime(LLC) > General.GetNullableDateTime(departuredate))
    //        {
    //            ucError.ErrorMessage = "Commencement of Sea Passage (COSP) Time should be later than All Gone and Clear (LLC) Time.";
    //        }
    //    }
    //    if (General.GetNullableDateTime(SBE) != null && General.GetNullableDateTime(departuredate) != null)
    //    {
    //        if (General.GetNullableDateTime(departuredate) < General.GetNullableDateTime(SBE))
    //        {
    //            ucError.ErrorMessage = "Commencement of Sea Passage(COSP) Time should be later than Standby Engine(SBE) Time.";
    //        }
    //    }
    //    if (General.GetNullableDateTime(LLC) != null && General.GetNullableDateTime(DLOSP) != null)
    //    {
    //        if (General.GetNullableDateTime(DLOSP) < General.GetNullableDateTime(LLC))
    //        {
    //            ucError.ErrorMessage = "Dropping of Last Outward Sea Pilot (DLOSP)  Time should be later than All Gone and Clear (LLC) Time.";
    //        }
    //    }
    //    if (General.GetNullableDateTime(SBE) != null && General.GetNullableDateTime(DLOSP) != null && General.GetNullableDateTime(departuredate) != null)
    //    {
    //        if (General.GetNullableDateTime(DLOSP) < General.GetNullableDateTime(SBE))
    //        {
    //            ucError.ErrorMessage = "Dropping of Last Outward Sea Pilot (DLOSP) Time should be later than Standby Engine(SBE) Time.";
    //        }
    //        if (General.GetNullableDateTime(LLC) > General.GetNullableDateTime(departuredate))
    //        {
    //            ucError.ErrorMessage = "Commencement of Sea Passage (COSP) Time should be later than Dropping of Last Outward Sea Pilot (DLOSP) Time  .";
    //        }
    //    }
       


    //    return (!ucError.IsError);
    //}

    //////////////////////////////////////// Services..
    protected void gvServices_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string DepServiceId = (((RadLabel)e.Item.FindControl("lblDepServiceId")).Text);
                if (DepServiceId != "")
                    PhoenixVesselPositionDepartureReport.DeleteDepartureServices(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(DepServiceId));

                RebindgvServices();
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
                    General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
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
                    General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                    General.GetNullableGuid(((RadLabel)gvr.FindControl("lblServiceIdEdit")).Text),
                    ((RadTextBox)gvr.FindControl("txtRemarksEdit")).Text);
            }
        }

    }


    private void InsertOtherOilConsumption()
    {
        foreach (GridDataItem gvr in gvFW.Items)
        {

            PhoenixVesselPositionNoonReportOilConsumption.InsertOtherOilConsumption(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              int.Parse(ViewState["VESSELID"].ToString()),
              new Guid(ViewState["NOONREPORTID"].ToString()),
              new Guid(((RadLabel)gvr.FindControl("lblOilTypeCodeEdit")).Text),
              "DEPARTURE",
              null,
              General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtROBOnDepartureEdit")).Text),
              General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtReceivedEdit")).Text),
              General.GetNullableGuid(((RadLabel)gvr.FindControl("lblNoonOilConsumptionId")).Text));

            PhoenixVesselPositionDepartureReport.UpdateDepartureWaterConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid((((RadLabel)gvr.FindControl("lblOilConsumptionId")).Text)),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                General.GetNullableGuid(((RadLabel)gvr.FindControl("lblOilTypeCodeEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtReceivedEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtROBOnDepartureEdit")).Text));
        }
    }

    protected void gvServices_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {

                    ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

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

    protected void gvServices1_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

          
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ucNextPort_Changed(object sender, EventArgs e)
    {
       
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
            tbrobcosp.Text = "ROB on Dep (COSP)";
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
                    ucError.ErrorMessage = "Please click the save button for the 'Departure Report' and then update the Consumption details.";
                    ucError.Visible = true;
                    return;
                }

                if (Filter.CurrentVPRSDepartureReportSelection == null)
                {
                    ucError.ErrorMessage = "Please fill in the Departure Report Details and click on the save button.";
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
                    "DEPARTURE",
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
                        General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCodeEdit")).Text), "DEPARTURE",
                        bunkerqty,
                        null,
                        null,
                        1,1,
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit")).Text));
                    ViewState["DEBUNKER"] = 0;
                    RebindgvConsumption();
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

                RebindgvConsumption();
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
                            cmdBunkerAdd.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+ "/VesselPosition/VesselPositionBunkerReceipt.aspx?consumptionid="
                            + lblOilConsumptionIdItem.Text + "&oiltype=" + drv["FLDOILTYPECODE"].ToString() + "'); return true;");
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
 
    protected void gvConsumption_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (ViewState["NOONREPORTID"] == null || General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) == null)
            {
                ucError.ErrorMessage = "Please click the save button for the 'Departure Report' and then update the Consumption details.";
                ucError.Visible = true;
                return;
            }

            if (Filter.CurrentVPRSDepartureReportSelection == null)
            {
                ucError.ErrorMessage = "Please fill in the Departure Report Details and click on the save button.";
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
                    "DEPARTURE",
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
                    General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeCodeEdit")).Text), "DEPARTURE",
                    bunkerqty,
                    null,
                    null,
                    1, 1,
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit")).Text));
                ViewState["DEBUNKER"] = 0;
                RebindgvConsumption();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ReCalculateROB(GridCommandEventArgs cRow)
    {
        try
        {
            decimal? bunkerqty = General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("txtBunkeredEdit")).Text);
            if (bunkerqty != null && bunkerqty > 0 && ViewState["DEBUNKER"].ToString() == "1")
            {
                bunkerqty = -1 * bunkerqty;
            }

            PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(ViewState["NOONREPORTID"].ToString()),
                new Guid(((RadLabel)cRow.Item.FindControl("lblOilTypeid")).Text),
                "DEPARTURE",
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortOTHEdit")).Text),
                null,
                null,
                0, // ROBATNOON YN
                0, // ROB @ EOSP AND ROB @ FWE YN
                0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                0, // RECALCULATE ROB
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                null,
                null,
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                null,
                null
                );

            PhoenixVesselPositionDepartureReport.UpdateDepartureOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid((((RadLabel)cRow.Item.FindControl("lblOilConsumptionId")).Text)),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection),
                General.GetNullableGuid(((RadLabel)cRow.Item.FindControl("lblOilTypeCodeEdit")).Text), "DEPARTURE",
                bunkerqty,
                null,
                null,
                1, 1,
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("txtSulphurPercentageEdit")).Text));

            ucStatus.Text = "ROB Recalculated";
            ViewState["DEBUNKER"] = 0;
            RebindgvConsumption();
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

    protected void gvServices_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureServices(
           General.GetNullableInteger(ViewState["VESSELID"].ToString())
           , General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection == null ? "" : Filter.CurrentVPRSDepartureReportSelection), 1);

            gvServices.DataSource = ds;
    }
    protected void RebindgvServices()
    {
        gvServices.SelectedIndexes.Clear();
        gvServices.EditIndexes.Clear();
        gvServices.DataSource = null;
        gvServices.Rebind();
    }

    protected void gvServices1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dts = PhoenixVesselPositionDepartureReport.ListDepartureServices(
           General.GetNullableInteger(ViewState["VESSELID"].ToString())
           , General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection == null ? "" : Filter.CurrentVPRSDepartureReportSelection), 0);

            gvServices1.DataSource = dts;
    }
    protected void RebindgvServices1()
    {
        gvServices1.SelectedIndexes.Clear();
        gvServices1.EditIndexes.Clear();
        gvServices1.DataSource = null;
        gvServices1.Rebind();
    }

    protected void gvFW_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.ListDepartureFreshWaterConsumption(
           General.GetNullableInteger(ViewState["VESSELID"].ToString())
           , General.GetNullableGuid(Filter.CurrentVPRSDepartureReportSelection == null ? "" : Filter.CurrentVPRSDepartureReportSelection)
           , General.GetNullableGuid(ViewState["VOYAGEID"].ToString())
           , General.GetNullableInteger(""));

            gvFW.DataSource = ds;

    }
    protected void RebindgvFW()
    {
        gvFW.SelectedIndexes.Clear();
        gvFW.EditIndexes.Clear();
        gvFW.DataSource = null;
        gvFW.Rebind();
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
            General.GetNullableGuid(noonreportid), "DEPARTURE");
            gvConsumption.DataSource = ds;

    }
    protected void RebindgvConsumption()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }
}
