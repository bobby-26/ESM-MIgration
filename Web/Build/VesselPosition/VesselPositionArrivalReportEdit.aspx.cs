using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselPositionArrivalReportEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            txtVoyageId.Attributes.Add("style", "display:none");
            txtNewVoyageId.Attributes.Add("style", "visibility:hidden");


            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "ARRIVALREPORT");
            toolbarmain.AddButton("Noon to EOSP", "EOSP");
            toolbarmain.AddButton("Passage Summary", "SUMMARY");
            toolbarmain.AddButton("MRV Summary  ", "MRVSUMMARY");
            MenuARSubTab.AccessRights = this.ViewState;
            MenuARSubTab.MenuList = toolbarmain.Show();
            MenuARSubTab.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();  
            toolbar.AddButton("Send To Office", "SENDTOOFFICE", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            ArrivalSave.AccessRights = this.ViewState;
            ArrivalSave.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                    
                    imgShowNewVoyage.Attributes.Add("onclick", "return showPickList('spnNewVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                    btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                }

                if ((Request.QueryString["mode"] != null) && (Request.QueryString["mode"].ToString().Equals("NEW")))
                {
                    Session["VESSELARRIVALID"] = null;
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "0";
                    ddlVoyagePort.VesselId = ViewState["VESSELID"].ToString();
                    imgShowNewVoyage.Attributes.Add("onclick", "return showPickList('spnNewVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                    btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                    
                    DataSet dsCurrentvoyage = PhoenixVesselPositionVoyageData.CurrentVoyageData(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (dsCurrentvoyage.Tables.Count > 0)
                    {
                        if (dsCurrentvoyage.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = dsCurrentvoyage.Tables[0].Rows[0];

                            txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                            txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                            ddlVoyagePort.VoyageId = txtVoyageId.Text;
                        }
                    }
                }
                ViewState["NOONREPORTID"] = null;
                VesselArrivalEdit();
                BindNoonReport();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DEBUNKER"] = 0;

                DataSet Ds = PhoenixRegistersOilType.ListOilType(1, 1);  // For only fueloil
                ddlECAOilType.DataSource = Ds;
                ddlECAOilType.DataBind();
                ddlECAOilType.Items.Insert(0, "--Select--");

                if (ChkECAyn.Checked == true)
                {
                    txtECAEntryDate.Enabled = true;
                    txtTimeofECAEntry.Enabled = true;
                    ddlECAOilType.Enabled = true;

                    txtECAEntryDate.CssClass = "input";
                    txtTimeofECAEntry.CssClass = "input";
                    ddlECAOilType.CssClass = "input";
                }
                else
                {
                    txtECAEntryDate.Enabled = false;
                    txtTimeofECAEntry.Enabled = false;
                    ddlECAOilType.Enabled = false;

                    txtECAEntryDate.CssClass = "readonlytextbox";
                    txtTimeofECAEntry.CssClass = "readonlytextbox";
                    ddlECAOilType.CssClass = "readonlytextbox";

                    txtECAEntryDate.Text = "";
                    txtTimeofECAEntry.SelectedDate = null;
                    ddlECAOilType.SelectedIndex = -1;
                }
            }

            if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString().Equals("NEW"))
            {
                txtESOP.Enabled = true;
                txtESOP.CssClass = "input_mandatory";
                txtESOPTime.Enabled = true;
                txtESOPTime.CssClass = "input_mandatory";
            }
            else
            {
                txtESOP.Enabled = false;
                txtESOP.CssClass = "readonlytextbox";
                txtESOPTime.Enabled = false;
                txtESOPTime.CssClass = "readonlytextbox";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
        ddlVoyagePort.VesselId = UcVessel.SelectedVessel;
    }
    protected void MenuARSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (Session["VESSELARRIVALID"] != null)
        {
            if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                Response.Redirect("../VesselPosition/VesselPositionArrivalReportPassageSummary.aspx");
            }
            if (CommandName.ToUpper().Equals("MRVSUMMARY"))
            {
                Response.Redirect("../VesselPosition/VesselPositionArrivalReportMRVSummary.aspx");
            }
            if (CommandName.ToUpper().Equals("ARRIVALREPORT"))
            {
                if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                    Response.Redirect("VesselPositionReports.aspx", false);
                else
                    Response.Redirect("VesselPositionArrivalReport.aspx", false);
            }
        }
        else
        {
            ucError.HeaderMessage = "Please provide the following required information";
            ucError.ErrorMessage = "Arrival report is not yet Created.Please Create Arrival report first";
            ucError.Visible = true;
            return;
        }
    }

    protected void VesselArrivalEdit()
    {
        if (Session["VESSELARRIVALID"] != null)
        {
            DataSet ds = PhoenixVesselPositionArrivalReport.EditArrivalReport(General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    ddlVoyagePort.VesselId = dr["FLDVESSELID"].ToString();
                    ddlVoyagePort.VoyageId = dr["FLDVOYAGEID"].ToString();
                    ddlVoyagePort.SelectedValue = dr["FLDPORT"].ToString();
                    ddlVoyagePort.SelectedPortCallValue = dr["FLDCURRENTPORTCALLID"].ToString(); 
                    ddlVoyagePort.Text = dr["FLDSEAPORTNAME"].ToString();
                    ViewState["NOONREPORTID"] = dr["FLDNOONREPORTID"].ToString();

                    txtETB.Text = General.GetDateTimeToString(dr["FLDETB"].ToString());
                    txtArrivalDate.Text = General.GetDateTimeToString(dr["FLDDATE"].ToString());
                    txtETD.Text = General.GetDateTimeToString(dr["FLDETD"].ToString());
                    txtRemarks.Text = dr["FLDREMARKS"].ToString();
                    if (General.GetNullableDateTime(dr["FLDETB"].ToString()) != null)
                        txtETBHours.SelectedDate = Convert.ToDateTime(dr["FLDETB"]);
                    if (General.GetNullableDateTime(dr["FLDETD"].ToString()) != null)
                        txtETDHours.SelectedDate = Convert.ToDateTime(dr["FLDETD"]);
                    if (General.GetNullableDateTime(dr["FLDDATE"].ToString()) != null)
                        txtArrivalTime.SelectedDate = Convert.ToDateTime(dr["FLDDATE"].ToString());
                    ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                    btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                    txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                    txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                    UcVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                    UcVessel.Enabled = false;

                    txtNewVoyageId.Text = dr["FLDNEWVOYAGEID"].ToString();
                    txtNewVoyageName.Text = dr["FLDNEWVOYAGENO"].ToString();
                    imgShowNewVoyage.Attributes.Add("onclick", "return showPickList('spnNewVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");

                    if (txtESOP.Text == "")
                    {
                        if (dr["FLDEOSP"].ToString() != "")
                        {
                            DateTime dtEosp = Convert.ToDateTime(General.GetDateTimeToString(dr["FLDEOSP"].ToString()));
                            string StrEosp = dtEosp.ToShortDateString();
                            ViewState["EOSP"] = StrEosp;
                        }
                    }
                    else
                    {
                        ViewState["EOSP"] = txtESOP.Text;
                    }

                    txtESOP.Text = General.GetDateTimeToString(dr["FLDEOSP"].ToString());
                    txtFWE.Text = General.GetDateTimeToString(dr["FLDFWE"].ToString());
                    if (General.GetNullableDateTime(dr["FLDEOSP"].ToString()) != null)
                        txtESOPTime.SelectedDate = Convert.ToDateTime(dr["FLDEOSP"]);
                    if (General.GetNullableDateTime(dr["FLDFWE"].ToString()) != null)
                        txtFWETime.SelectedDate = Convert.ToDateTime(dr["FLDFWE"]);
                    txtPOB.Text = General.GetDateTimeToString(dr["FLDPOB"].ToString());
                    if (General.GetNullableDateTime(dr["FLDPOB"].ToString()) != null)
                        txtPOBTime.SelectedDate = Convert.ToDateTime(dr["FLDPOB"]);
                    txtFLA.Text = General.GetDateTimeToString(dr["FLDFLA"].ToString());
                    if (General.GetNullableDateTime(dr["FLDFLA"].ToString()) != null)
                        txtFLATime.SelectedDate = Convert.ToDateTime(dr["FLDFLA"]);
                    txtLGA.Text = General.GetDateTimeToString(dr["FLDLGA"].ToString());
                    if (General.GetNullableDateTime(dr["FLDLGA"].ToString()) != null)
                        txtLGATime.SelectedDate = Convert.ToDateTime(dr["FLDLGA"]);
                    txtDOP.Text = General.GetDateTimeToString(dr["FLDDOP"].ToString());
                    if (General.GetNullableDateTime(dr["FLDDOP"].ToString()) != null)
                        txtDOPTime.SelectedDate = Convert.ToDateTime(dr["FLDDOP"]);
                    txtNORT.Text = General.GetDateTimeToString(dr["FLDNORT"].ToString());
                    if (General.GetNullableDateTime(dr["FLDNORT"].ToString()) != null)
                        txtNORTTime.SelectedDate = Convert.ToDateTime(dr["FLDNORT"]);
                    txtNORA.Text = General.GetDateTimeToString(dr["FLDNORA"].ToString());
                    if (General.GetNullableDateTime(dr["FLDNORA"].ToString()) != null)
                        txtNORATime.SelectedDate = Convert.ToDateTime(dr["FLDNORA"]);
                    txtFPG.Text = General.GetDateTimeToString(dr["FLDFPG"].ToString());
                    if (General.GetNullableDateTime(dr["FLDFPG"].ToString()) != null)
                        txtFPGTime.SelectedDate = Convert.ToDateTime(dr["FLDFPG"]);
                    txtUTCETB.Text = General.GetDateTimeToString(dr["FLDUTCETB"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCETB"].ToString()) != null)
                        txtUTCETBHours.SelectedDate = Convert.ToDateTime(dr["FLDUTCETB"]);
                    txtUTCETD.Text = General.GetDateTimeToString(dr["FLDUTCETD"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCETD"].ToString()) != null)
                        txtUTCETDHours.SelectedDate = Convert.ToDateTime(dr["FLDUTCETD"]);


                    txtManoevering.Text = dr["FLDARRIVALMANOEVERING"].ToString();
                    txtManoeveringDist.Text = dr["FLDARRIVALMANOEVERINGDIST"].ToString();

                    txtShipMeanTime.Text = dr["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
                    txtShipMeanTimeSymbol.SelectedValue = dr["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";

                    txtUTCDate.Text = General.GetDateTimeToString(dr["FLDUTCDATETIME"].ToString());
                    if (General.GetNullableGuid(dr["FLDUTCDATETIME"].ToString()) != null)
                        txtUTCTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCDATETIME"]);


                    ucECAlatitude.TextDegree = dr["FLDECALAT1"].ToString();
                    ucECAlatitude.TextMinute = dr["FLDECALAT2"].ToString();
                    ucECAlatitude.TextSecond = dr["FLDECALAT3"].ToString();
                    ucECAlatitude.TextDirection = dr["FLDECALATDIRECTION"].ToString();
                    ucECALongitude.TextDegree = dr["FLDECALONG1"].ToString();
                    ucECALongitude.TextMinute = dr["FLDECALONG2"].ToString();
                    ucECALongitude.TextSecond = dr["FLDECALONG3"].ToString();
                    ucECALongitude.TextDirection = dr["FLDECALONGDIRECTION"].ToString();

                    ucFuelCOTime.Text = General.GetDateTimeToString(dr["FLDFUELCOTIME"].ToString());
                    if (General.GetNullableDateTime(dr["FLDFUELCOTIME"].ToString()) != null)
                        txtFuelCOTime.SelectedDate = Convert.ToDateTime(dr["FLDFUELCOTIME"]);

                    txtUTCESOP.Text = General.GetDateTimeToString(dr["FLDUTCEOSP"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCEOSP"].ToString()) != null)
                        txtUTCESOPTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCEOSP"]);
                    txtUTCFWE.Text = General.GetDateTimeToString(dr["FLDUTCFWE"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCFWE"].ToString()) != null)
                        txtUTCFWETime.SelectedDate = Convert.ToDateTime(dr["FLDUTCFWE"]);
                    txtUTCPOB.Text = General.GetDateTimeToString(dr["FLDUTCPOB"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCPOB"].ToString()) != null)
                        txtUTCPOBTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCPOB"]);
                    txtUTCFLA.Text = General.GetDateTimeToString(dr["FLDUTCFLA"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCFLA"].ToString()) != null)
                        txtUTCFLATime.SelectedDate = Convert.ToDateTime(dr["FLDUTCFLA"]);
                    txtUTCLGA.Text = General.GetDateTimeToString(dr["FLDUTCLGA"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCLGA"].ToString()) != null)
                        txtUTCLGATime.SelectedDate = Convert.ToDateTime(dr["FLDUTCLGA"]);
                    txtUTCDOP.Text = General.GetDateTimeToString(dr["FLDUTCDOP"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCDOP"].ToString()) != null)
                        txtUTCDOPTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCDOP"]);
                    txtUTCNORT.Text = General.GetDateTimeToString(dr["FLDUTCNORT"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCNORT"].ToString()) != null)
                        txtUTCNORTTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCNORT"]);
                    txtUTCNORA.Text = General.GetDateTimeToString(dr["FLDUTCNORA"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCNORA"].ToString()) != null)
                        txtUTCNORATime.SelectedDate = Convert.ToDateTime(dr["FLDUTCNORA"]);
                    txtUTCFPG.Text = General.GetDateTimeToString(dr["FLDUTCFPG"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCFPG"].ToString()) != null)
                        txtUTCFPGTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCFPG"]);
                    txtFSW.Text = General.GetDateTimeToString(dr["FLDFSW"].ToString());
                    if (General.GetNullableDateTime(dr["FLDFSW"].ToString()) != null)
                        txtFSWTime.SelectedDate = Convert.ToDateTime(dr["FLDFSW"]);
                    txtUTCFSW.Text = General.GetDateTimeToString(dr["FLDUTCFSW"].ToString());
                    if (General.GetNullableDateTime(dr["FLDUTCFSW"].ToString()) != null)
                        txtUTCFSWTime.SelectedDate = Convert.ToDateTime(dr["FLDUTCFSW"]);


                    if (dr["FLDCONFIRMEDYN"].ToString() == "1")
                    {
                        ArrivalSave.Visible = false;
                        lblAlertSenttoOFC.Visible = true;
                    }

                    txtEMLogcounteratFWE.Text = dr["FLDEMLOGCOUNTERATFWE"].ToString();
                    txtEMLogManoeuveringDistance.Text = dr["FLDEMLOGMANOUVERINGDIST"].ToString();
                    txtEMLogCounteratEOSP.Text = dr["FLDEMLOGCOUNTERATEOSP"].ToString();
                    txtEMLogDistance.Text = dr["FLDEMLOGDISTANCE"].ToString();
                    txtMERevCounter.Text = dr["FLDMEREVCOUNTER"].ToString();
                    txtMErevcounterFWE.Text = dr["FLDMEREVCOUNTERFWE"].ToString();
                    chkOffPortLimits.Checked = dr["FLDOFFPORTLIMITYN"].ToString().Equals("1") ? true : false;
                }
            }
        }
    }

    private decimal? CalculateLogSpeed(string fullspeed, string fullspeeddistance, string reducedspeed, string reduceedspeeddistance)
    {
        decimal? logspeed = 0;
        decimal? fs = General.GetNullableDecimal(fullspeed);
        decimal? rs = General.GetNullableDecimal(reducedspeed);
        decimal? fsdist = General.GetNullableDecimal(fullspeeddistance);
        decimal? rsdist = General.GetNullableDecimal(reduceedspeeddistance);

        decimal? distance = (fsdist == null ? 0 : fsdist) + (rsdist == null ? 0 : rsdist);
        decimal? speed = (fs == null ? 0 : fs) + (rs == null ? 0 : rs);

        if (speed > 0)
            logspeed = distance / speed;

        return logspeed;
    }

    private decimal? CalculateDistanceObserved(string fullspeeddistance, string reduceedspeeddistance)
    {
        decimal? fsdist = General.GetNullableDecimal(fullspeeddistance);
        decimal? rsdist = General.GetNullableDecimal(reduceedspeeddistance);

        decimal? distance = (fsdist == null ? 0 : fsdist) + (rsdist == null ? 0 : rsdist);

        return distance;
    }

    private decimal? CalculateSlip(string enginedist, string distobserved)
    {
        decimal? engdist = General.GetNullableDecimal(enginedist);
        decimal? distobs = General.GetNullableDecimal(distobserved);

        decimal? slip = 0;

        if (engdist != null && engdist != 0)
            slip = ((engdist - (distobs == null ? 0 : distobs)) / engdist) * 100;

        return slip;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Reset()
    {
        ddlVoyagePort.SelectedValue = "";
        ddlVoyagePort.SelectedPortCallValue = "";
        ddlVoyagePort.Text = "";

        txtETB.Text = "";
        txtArrivalDate.Text = "";
        txtETD.Text = "";
        txtAvgSlip.Text = "";
        txtRemarks.Text = "";
        ViewState["NOONREPORTID"] = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        {
            UcVessel.SelectedVessel = "";
            btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=0', true); ");
            txtVoyageId.Text = "";
            txtVoyageName.Text = "";
        }
        else
        {
            UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            UcVessel.Enabled = false;
            btnShowVoyage.Attributes.Add(
                "onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "', true); ");
            txtVoyageId.Text = "";
            txtVoyageName.Text = "";
        }

        txtManoevering.Text = "";
        txtManoeveringDist.Text = "";
    }

    protected void ArrivalSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }

            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                if (Session["VESSELARRIVALID"] != null && Session["VESSELARRIVALID"].ToString() != "")
                {
                    if (CommandName.ToUpper().Equals("SENDTOOFFICE"))
                    {
                        InsertOtherOilConsumption();
                        UpdateArrivalReport(1);
                        PopulateVoyageData();

                    }
                    else
                    {
                        UpdateArrivalReport(0);
                        InsertOtherOilConsumption();
                    }
                    
                }
                else
                {
                    if (CommandName.ToUpper().Equals("SENDTOOFFICE"))
                    {
                        AddArrivalReport(1);
                        PopulateVoyageData();
                    }
                    else
                    {
                        AddArrivalReport(0);
                        InsertOtherOilConsumption();
                    }
                }
                
                VesselArrivalEdit();
                BindNoonReport();
                gvConsumptionRebind();
                gvOtherOilConsRebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void PopulateVoyageData()
    {
        PhoenixVesselPositionArrivalReport.PassageSummaryInsert((PhoenixSecurityContext.CurrentSecurityContext.UserCode), General.GetNullableInteger(ViewState["VESSELID"].ToString()), General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
    }
    private void UpdateArrivalReport(int confirm)
    {
       
        string arrival = txtArrivalTime.SelectedTime != null ? txtArrivalTime.SelectedTime.Value.ToString() : "";
        string etb = txtETBHours.SelectedTime != null ? txtETBHours.SelectedTime.Value.ToString() : "";
        string etd = txtETDHours.SelectedTime != null ? txtETDHours.SelectedTime.Value.ToString() : "";
        string txtESOPt = txtESOPTime.SelectedTime != null ? txtESOPTime.SelectedTime.Value.ToString() : "";
        string txtFWEt = txtFWETime.SelectedTime != null ? txtFWETime.SelectedTime.Value.ToString() : "";

        string POBTime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string FLATime = txtFLATime.SelectedTime != null ? txtFLATime.SelectedTime.Value.ToString() : "";
        string LGATime = txtLGATime.SelectedTime != null ? txtLGATime.SelectedTime.Value.ToString() : "";
        string DOPTime = txtDOPTime.SelectedTime != null ? txtDOPTime.SelectedTime.Value.ToString() : "";
        string NORTTime = txtNORTTime.SelectedTime != null ? txtNORTTime.SelectedTime.Value.ToString() : "";
        string NORATime = txtNORATime.SelectedTime != null ? txtNORATime.SelectedTime.Value.ToString() : "";
        string FPGTime = txtFPGTime.SelectedTime != null ? txtFPGTime.SelectedTime.Value.ToString() : "";
        string timeofFuelCO = txtFuelCOTime.SelectedTime != null ? txtFuelCOTime.SelectedTime.Value.ToString() : "";
        string FSWTime = txtFSWTime.SelectedTime != null ? txtFSWTime.SelectedTime.Value.ToString() : "";


        if (ViewState["NOONREPORTID"] == null || General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) == null)
        {
            AddNoonReport(confirm);
        }
        else
        {
            UpdateNoonReport(confirm);
        }

        PhoenixVesselPositionArrivalReport.UpdateArrivalReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(Session["VESSELARRIVALID"].ToString())
            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(txtVoyageId.Text)
            , General.GetNullableDateTime(txtESOP.Text + " " + txtESOPt)
            , General.GetNullableGuid(ddlVoyagePort.SelectedPortCallValue)
            , General.GetNullableDateTime(txtETB.Text + " " + etb)
            , General.GetNullableDateTime(txtETD.Text + " " + etd)
            , General.GetNullableDateTime(txtESOP.Text + " " + txtESOPt)
            , General.GetNullableDateTime(txtFWE.Text + " " + txtFWEt)
            , General.GetNullableDecimal("txtAtAnchor.Text")
            , txtBerthName.Text
            , General.GetNullableString(txtRemarks.Text)
            , General.GetNullableGuid(ViewState["NOONREPORTID"].ToString())
            , General.GetNullableDateTime(txtPOB.Text + " " + POBTime)
            , General.GetNullableDateTime(txtFLA.Text + " " + FLATime)
            , General.GetNullableDateTime(txtLGA.Text + " " + LGATime)
            , General.GetNullableDateTime(txtDOP.Text + " " + DOPTime)
            , General.GetNullableDateTime(txtNORT.Text + " " + NORTTime)
            , General.GetNullableDateTime(txtNORA.Text + " " + NORATime)
            , General.GetNullableDateTime(txtFPG.Text + " " + FPGTime)
            , General.GetNullableDecimal(txtManoevering.Text)
            , General.GetNullableDecimal(txtManoeveringDist.Text), confirm, (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text)
            , ucECAlatitude.TextDegree, ucECALongitude.TextDegree, ucECAlatitude.TextMinute, ucECALongitude.TextMinute, ucECAlatitude.TextSecond, ucECALongitude.TextSecond, ucECAlatitude.TextDirection, ucECALongitude.TextDirection
            , General.GetNullableDateTime(ucFuelCOTime.Text + " " + timeofFuelCO)
            , General.GetNullableDecimal(txtEMLogcounteratFWE.Text)
            , General.GetNullableDecimal(txtEMLogCounteratEOSP.Text)
            , General.GetNullableDecimal(txtMERevCounter.Text)
            , General.GetNullableDateTime(txtFSW.Text + " " + FSWTime)
            , General.GetNullableDecimal(txtMErevcounterFWE.Text)
            , chkCounterDefective.Checked == true ? 1 : 0
            , chkRevCounterDefective.Checked == true ? 1 : 0
            , General.GetNullableDecimal(txtEMLogDistance.Text)
            , General.GetNullableDecimal(txtEMLogManoeuveringDistance.Text)
            , chkOffPortLimits.Checked == true ? 1 : 0
            );


        if (General.GetNullableGuid(txtNewVoyageId.Text) != null)
        {
            PhoenixVesselPositionVoyageData.UpdateStartNewVoyage(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(txtVoyageId.Text),
                General.GetNullableGuid(txtNewVoyageId.Text),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableInteger(ddlVoyagePort.SelectedValue),
                General.GetNullableDateTime(txtESOP.Text + " " + txtESOPt),
                null,
                General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
        }
        if (confirm == 0)
        {
            ucStatus.Text = "Arrival Report updated.";
        }
        else
        {
            ucStatus.Text = "Arrival Report Sent to Office.";
        }
    }

    private void AddArrivalReport(int confirm)
    {
        string arrival = txtArrivalTime.SelectedTime != null ? txtArrivalTime.SelectedTime.Value.ToString() : "";
        string etb = txtETBHours.SelectedTime != null ? txtETBHours.SelectedTime.Value.ToString() : "";
        string etd = txtETDHours.SelectedTime != null ? txtETDHours.SelectedTime.Value.ToString() : "";
        string txtESOPt = txtESOPTime.SelectedTime != null ? txtESOPTime.SelectedTime.Value.ToString() : "";
        string txtFWEt = txtFWETime.SelectedTime != null ? txtFWETime.SelectedTime.Value.ToString() : "";

        string POBTime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string FLATime = txtFLATime.SelectedTime != null ? txtFLATime.SelectedTime.Value.ToString() : "";
        string LGATime = txtLGATime.SelectedTime != null ? txtLGATime.SelectedTime.Value.ToString() : "";
        string DOPTime = txtDOPTime.SelectedTime != null ? txtDOPTime.SelectedTime.Value.ToString() : "";
        string NORTTime = txtNORTTime.SelectedTime != null ? txtNORTTime.SelectedTime.Value.ToString() : "";
        string NORATime = txtNORATime.SelectedTime != null ? txtNORATime.SelectedTime.Value.ToString() : "";
        string FPGTime = txtFPGTime.SelectedTime != null ? txtFPGTime.SelectedTime.Value.ToString() : "";
        string timeofFuelCO = txtFuelCOTime.SelectedTime != null ? txtFuelCOTime.SelectedTime.Value.ToString() : "";
        string FSWTime = txtFSWTime.SelectedTime != null ? txtFSWTime.SelectedTime.Value.ToString() : "";
        Guid? vesselarrivalid = null;

        //if (!IsValidArrivalReport(ddlVoyagePort.SelectedPortCallValue, txtVoyageId.Text, txtESOP.Text + " " + txtESOPt, txtPOB.Text + " " + POBTime, txtFLA.Text + " " + FLATime, txtLGA.Text + " " + LGATime, txtFWE.Text + " " + txtFWEt, txtFullSpeed.Text, txtFSDistance.Text, txtReducedSpeed.Text, txtRSDistance.Text))
        //{
        //    ucError.Visible = true;
        //    return;
        //}

        if (ViewState["NOONREPORTID"] == null || General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) == null)
        {
            AddNoonReport(confirm);
        }
        else
        {
            UpdateNoonReport(confirm);
        }

        PhoenixVesselPositionArrivalReport.InsertArrivalReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(txtVoyageId.Text)
            , General.GetNullableDateTime(txtESOP.Text + " " + txtESOPt)
            , General.GetNullableGuid(ddlVoyagePort.SelectedPortCallValue)
            , General.GetNullableDateTime(txtETB.Text + " " + etb)
            , General.GetNullableDateTime(txtETD.Text + " " + etd)
            , General.GetNullableDateTime(txtESOP.Text + " " + txtESOPt)
            , General.GetNullableDateTime(txtFWE.Text + " " + txtFWEt)
            , General.GetNullableDecimal("txtAtAnchor.Text")
            , txtBerthName.Text
            , General.GetNullableString(txtRemarks.Text)
            , General.GetNullableGuid(ViewState["NOONREPORTID"].ToString())
            , General.GetNullableDateTime(txtPOB.Text + " " + POBTime)
            , General.GetNullableDateTime(txtFLA.Text + " " + FLATime)
            , General.GetNullableDateTime(txtLGA.Text + " " + LGATime)
            , General.GetNullableDateTime(txtDOP.Text + " " + DOPTime)
            , General.GetNullableDateTime(txtNORT.Text + " " + NORTTime)
            , General.GetNullableDateTime(txtNORA.Text + " " + NORATime)
            , General.GetNullableDateTime(txtFPG.Text + " " + FPGTime)
            , General.GetNullableDecimal(txtManoevering.Text)
            , General.GetNullableDecimal(txtManoeveringDist.Text)
            , ref vesselarrivalid, confirm, (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text)
            , ucECAlatitude.TextDegree, ucECALongitude.TextDegree, ucECAlatitude.TextMinute, ucECALongitude.TextMinute, ucECAlatitude.TextSecond, ucECALongitude.TextSecond, ucECAlatitude.TextDirection, ucECALongitude.TextDirection
            , General.GetNullableDateTime(ucFuelCOTime.Text + " " + timeofFuelCO)
            , General.GetNullableDecimal(txtEMLogcounteratFWE.Text)
            , General.GetNullableDecimal(txtEMLogCounteratEOSP.Text)
            , General.GetNullableDecimal(txtMERevCounter.Text)
            , General.GetNullableDateTime(txtFSW.Text + " " + FSWTime)
            , General.GetNullableDecimal(txtMErevcounterFWE.Text)
            , chkCounterDefective.Checked == true ? 1 : 0
            , chkRevCounterDefective.Checked == true ? 1 : 0
            , General.GetNullableDecimal(txtEMLogDistance.Text)
            , General.GetNullableDecimal(txtEMLogManoeuveringDistance.Text)
            , chkOffPortLimits.Checked == true ? 1 : 0
             );

        Session["VESSELARRIVALID"] = vesselarrivalid;

        if (General.GetNullableGuid(txtNewVoyageId.Text) != null)
        {
            PhoenixVesselPositionVoyageData.UpdateStartNewVoyage(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(txtVoyageId.Text),
                General.GetNullableGuid(txtNewVoyageId.Text),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                General.GetNullableInteger(ddlVoyagePort.SelectedValue),
                General.GetNullableDateTime(txtESOP.Text + " " + txtESOPt),
                null,
                General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
        }

        
        ucStatus.Text = "Arrival Report created.";
        //Response.Redirect("../VesselPosition/VesselPositionArrivalReportEdit.aspx", false);
    }

    private void AddNoonReport(int confirm)
    {
        Guid? noonreportid = null;

        

        string arrival = txtArrivalTime.SelectedTime != null ? txtArrivalTime.SelectedTime.Value.ToString() : "";
        string timeofeca = txtTimeofECAEntry.SelectedTime != null ? txtTimeofECAEntry.SelectedTime.Value.ToString() : "";
        string lastlandsludgetime = txtLastLandSludgeTime.Text.Trim().Equals("__:__") ? string.Empty : txtLastLandSludgeTime.Text;
        string BilgeLandingTime = txtBilgeLandingTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeLandingTime.Text;
        string FSWTime = txtFSWTime.SelectedTime != null ? txtFSWTime.SelectedTime.Value.ToString() : "";

        string EospTime = txtESOPTime.SelectedTime != null ? txtESOPTime.SelectedTime.Value.ToString() : "";
        string FweTime = txtFWETime.SelectedTime != null ? txtFWETime.SelectedTime.Value.ToString() : "";
        string LgaTime = txtLGATime.SelectedTime != null ? txtLGATime.SelectedTime.Value.ToString() : "";
        string FlaTime = txtFLATime.SelectedTime != null ? txtFLATime.SelectedTime.Value.ToString() : "";
        string PobTime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string etd = txtETDHours.SelectedTime != null ? txtETDHours.SelectedTime.Value.ToString() : "";

        PhoenixVesselPositionWedSunReport.InsertWedSunReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(ViewState["VESSELID"].ToString()),
            General.GetNullableGuid(txtVoyageId.Text), txtCourse.Text,
            ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute, ucLatitude.TextSecond, ucLongitude.TextSecond, ucLatitude.TextDirection, ucLongitude.TextDirection,
            General.GetNullableInteger(rbtnBallastLaden.SelectedValue),
            General.GetNullableDecimal(txtDWT.Text), General.GetNullableDecimal(txtDraftA.Text),
            CalculateDistanceObserved(txtFSDistance.Text, txtRSDistance.Text),
            CalculateLogSpeed(txtFullSpeed.Text, txtFSDistance.Text, txtReducedSpeed.Text, txtRSDistance.Text),
            General.GetNullableDecimal(""),
            General.GetNullableGuid(ucWindDirection.SelectedDirection), General.GetNullableDecimal(txtWindForce.Text),
            General.GetNullableDecimal(txtSeaHeight.Text), General.GetNullableGuid(ucSeaDirection.SelectedDirection),
            General.GetNullableDecimal(txtCurrentSpeed.Text), General.GetNullableGuid(ucCurrentDirection.SelectedDirection),
            General.GetNullableDecimal(txtAirTemp.Text), txtRemarks.Text,
            (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text), 
            General.GetNullableDecimal(txtDraftF.Text),
            General.GetNullableDateTime(txtESOP.Text + " " + EospTime),
            General.GetNullableDateTime(""),
            General.GetNullableGuid(ddlVoyagePort.SelectedPortCallValue),
            General.GetNullableGuid(""),
            "",
            General.GetNullableDecimal(txtFullSpeed.Text), General.GetNullableDecimal(txtFSDistance.Text),
            General.GetNullableDecimal(txtReducedSpeed.Text), General.GetNullableDecimal(txtRSDistance.Text),
            General.GetNullableDecimal(txtStopped.Text), General.GetNullableDecimal(txtNoonSpeed.Text),
            General.GetNullableDecimal(txtVOSpeed.Text), General.GetNullableDecimal(txtVOCons.Text),
            chkIceDeck.Checked == true ? 1 : 0,
            1,
            ref noonreportid
            , General.GetNullableString("")
            , General.GetNullableDateTime("")
            , General.GetNullableDateTime(txtETD.Text + " " + etd)
            , General.GetNullableDecimal(txtSwell.Text)
            , 0
            , 0
            , ChkECAyn.Checked == true ? 1 : 0
            , General.GetNullableGuid(ddlECAOilType.SelectedValue)
            , General.GetNullableDateTime(txtECAEntryDate.Text + " " + timeofeca),
            "ARRIVAL",null,confirm
            , General.GetNullableDateTime(txtFWE.Text + " " + FweTime)
            , General.GetNullableDateTime(txtLGA.Text + " " + LgaTime)
            , General.GetNullableDateTime(txtFLA.Text + " " + FlaTime)
            , General.GetNullableDateTime(txtPOB.Text + " " + PobTime)
            , General.GetNullableDecimal(txtEMLogcounteratFWE.Text)
            , General.GetNullableDecimal(txtEMLogCounteratEOSP.Text)
            , General.GetNullableDateTime(txtFSW.Text + " " + FSWTime)
            , General.GetNullableDecimal(txtMERevCounter.Text)
            , General.GetNullableDecimal(txtMErevcounterFWE.Text)
            , chkCounterDefective.Checked==true? 1 : 0
            , chkRevCounterDefective.Checked==true? 1 : 0
            , chkOffPortLimits.Checked == true ? 1 : 0);

        ViewState["NOONREPORTID"] = noonreportid;

        PhoenixVesselPositionWedSunReport.UpdateWedSunReportEngineDept(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["NOONREPORTID"].ToString()),
            int.Parse(ViewState["VESSELID"].ToString()),
            General.GetNullableDecimal(txtSwellTemp.Text),
            General.GetNullableDecimal(txtEngineDistance.Text),
            null, //CalculateSlip(txtEngineDistance.Text, ViewState["DistObserved"].ToString())
            General.GetNullableDecimal(txtERExhTemp.Text),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            txtRemarksCE.Text,
            General.GetNullableDecimal(txtGovernorSetting.Text),
            General.GetNullableDecimal(txtSpeedSetting.Text),
            General.GetNullableDecimal(txtFOInletTemp.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtFuelOilPress.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtBoilerWaterChlorides.Text),
            General.GetNullableDecimal(txtBilgeROB.Text),
            General.GetNullableDecimal(txtSludgeROB.Text),
            General.GetNullableDateTime(txtLastLandSludge.Text + " " + lastlandsludgetime),
            General.GetNullableInteger(txtLastLandingDays.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtMERPM.Text),
            General.GetNullableDateTime(txtBilgeLanding.Text + " " + BilgeLandingTime),
            General.GetNullableInteger(txtBilgeLandingDays.Text),
            General.GetNullableDecimal(txtHFOTankCleaning.Text),
            General.GetNullableDecimal(txtHFOCargoHeating.Text),
            General.GetNullableDecimal(txtGeneralLoadAE1.Text),
            General.GetNullableDecimal(txtGeneralLoadAE2.Text),
            General.GetNullableDecimal(txtGeneralLoadAE3.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE4.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE1OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE2OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE3OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE4OPHrs.Text),
            "ARRIVAL");
    }

    private void UpdateNoonReport(int confirm)
    {
        string timeofeca = txtTimeofECAEntry.SelectedTime != null ? txtTimeofECAEntry.SelectedTime.Value.ToString() : "";
        string lastlandsludgetime = txtLastLandSludgeTime.Text.Trim().Equals("__:__") ? string.Empty : txtLastLandSludgeTime.Text;
        string BilgeLandingTime = txtBilgeLandingTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeLandingTime.Text;
        string FSWTime = txtFSWTime.SelectedTime != null ? txtFSWTime.SelectedTime.Value.ToString() : "";

        string EospTime = txtESOPTime.SelectedTime != null ? txtESOPTime.SelectedTime.Value.ToString() : "";
        string FweTime = txtFWETime.SelectedTime != null ? txtFWETime.SelectedTime.Value.ToString() : "";
        string LgaTime = txtLGATime.SelectedTime != null ? txtLGATime.SelectedTime.Value.ToString() : "";
        string FlaTime = txtFLATime.SelectedTime != null ? txtFLATime.SelectedTime.Value.ToString() : "";
        string PobTime = txtPOBTime.SelectedTime != null ? txtPOBTime.SelectedTime.Value.ToString() : "";
        string etd = txtETDHours.SelectedTime != null ? txtETDHours.SelectedTime.Value.ToString() : "";

        PhoenixVesselPositionWedSunReport.UpdateWedSunReport(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["NOONREPORTID"].ToString()),
            int.Parse(ViewState["VESSELID"].ToString()),
            new Guid(txtVoyageId.Text), txtCourse.Text,
            ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute, ucLatitude.TextSecond, ucLongitude.TextSecond, 
            ucLatitude.TextDirection, ucLongitude.TextDirection,
            General.GetNullableInteger(rbtnBallastLaden.SelectedValue),
            General.GetNullableDecimal(txtDWT.Text), General.GetNullableDecimal(txtDraftA.Text),
            CalculateDistanceObserved(txtFSDistance.Text, txtRSDistance.Text),
            CalculateLogSpeed(txtFullSpeed.Text, txtFSDistance.Text, txtReducedSpeed.Text, txtRSDistance.Text), 
            General.GetNullableDecimal(""),
            General.GetNullableGuid(ucWindDirection.SelectedDirection), General.GetNullableDecimal(txtWindForce.Text),
            General.GetNullableDecimal(txtSeaHeight.Text), General.GetNullableGuid(ucSeaDirection.SelectedDirection),
            General.GetNullableDecimal(txtCurrentSpeed.Text), General.GetNullableGuid(ucCurrentDirection.SelectedDirection),
            General.GetNullableDecimal(txtAirTemp.Text),
            txtRemarks.Text,
            (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text), 
            General.GetNullableDecimal(txtDraftF.Text),
            General.GetNullableDateTime(txtESOP.Text + " " + EospTime),
            General.GetNullableDateTime(""),
            General.GetNullableGuid(ddlVoyagePort.SelectedPortCallValue),
            General.GetNullableGuid(""),
            "",
            General.GetNullableDecimal(txtFullSpeed.Text), General.GetNullableDecimal(txtFSDistance.Text),
            General.GetNullableDecimal(txtReducedSpeed.Text), General.GetNullableDecimal(txtRSDistance.Text),
            General.GetNullableDecimal(txtStopped.Text), General.GetNullableDecimal(txtNoonSpeed.Text),
            General.GetNullableDecimal(txtVOSpeed.Text), General.GetNullableDecimal(txtVOCons.Text),
            chkIceDeck.Checked == true ? 1 : 0,
            1
            , General.GetNullableString("")
            , General.GetNullableDateTime("")
            , General.GetNullableDateTime(txtETD.Text + " " + etd)
            , General.GetNullableDecimal(txtSwell.Text)
            , 0
            , 0
            , ChkECAyn.Checked == true ? 1 : 0
            , General.GetNullableGuid(ddlECAOilType.SelectedValue)
            , General.GetNullableDateTime(txtECAEntryDate.Text + " " + timeofeca),
            "ARRIVAL", null, null, null, null, confirm
            , General.GetNullableDateTime(txtFWE.Text + " " + FweTime)
            , General.GetNullableDateTime(txtLGA.Text + " " + LgaTime)
            , General.GetNullableDateTime(txtFLA.Text + " " + FlaTime)
            , General.GetNullableDateTime(txtPOB.Text + " " + PobTime)
            , General.GetNullableDecimal(txtEMLogcounteratFWE.Text)
            , General.GetNullableDecimal(txtEMLogCounteratEOSP.Text)
            , General.GetNullableDateTime(txtFSW.Text + " " + FSWTime)
            , General.GetNullableDecimal(txtMERevCounter.Text)
            , General.GetNullableDecimal(txtMErevcounterFWE.Text)
            , chkCounterDefective.Checked == true ? 1 : 0
            , chkRevCounterDefective.Checked == true ? 1 : 0
            , chkOffPortLimits.Checked == true ? 1 : 0);

        PhoenixVesselPositionWedSunReport.UpdateWedSunReportEngineDept(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["NOONREPORTID"].ToString()),
            int.Parse(ViewState["VESSELID"].ToString()),
            General.GetNullableDecimal(txtSwellTemp.Text),
            General.GetNullableDecimal(txtEngineDistance.Text),
            null, //CalculateSlip(txtEngineDistance.Text, ViewState["DistObserved"].ToString())
            General.GetNullableDecimal(txtERExhTemp.Text),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            txtRemarksCE.Text,
            General.GetNullableDecimal(txtGovernorSetting.Text),
            General.GetNullableDecimal(txtSpeedSetting.Text),
            General.GetNullableDecimal(txtFOInletTemp.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtFuelOilPress.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtBoilerWaterChlorides.Text),
            General.GetNullableDecimal(txtBilgeROB.Text),
            General.GetNullableDecimal(txtSludgeROB.Text),
            General.GetNullableDateTime(txtLastLandSludge.Text + " " + lastlandsludgetime),
            General.GetNullableInteger(txtLastLandingDays.Text),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            General.GetNullableInteger(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtMERPM.Text),
            General.GetNullableDateTime(txtBilgeLanding.Text + " " + BilgeLandingTime),
            General.GetNullableInteger(txtBilgeLandingDays.Text),
            General.GetNullableDecimal(txtHFOTankCleaning.Text),
            General.GetNullableDecimal(txtHFOCargoHeating.Text),
            General.GetNullableDecimal(txtGeneralLoadAE1.Text),
            General.GetNullableDecimal(txtGeneralLoadAE2.Text),
            General.GetNullableDecimal(txtGeneralLoadAE3.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE4.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE1OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE2OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE3OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE4OPHrs.Text),
            "ARRIVAL");

        //ucStatus.Text = "Noon Report updated.";
    }

    private void BindNoonReport()
    {
        if (ViewState["NOONREPORTID"] != null && General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) != null)
        {
            string noonreportid = ViewState["NOONREPORTID"].ToString();
            DataSet ds = PhoenixVesselPositionWedSunReport.EditWedSunReport(General.GetNullableGuid(noonreportid));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                ucLatitude.TextDegree = dt.Rows[0]["FLDLAT1"].ToString();
                ucLatitude.TextMinute = dt.Rows[0]["FLDLAT2"].ToString();
                ucLatitude.TextSecond = dt.Rows[0]["FLDLAT3"].ToString();
                ucLatitude.TextDirection = dt.Rows[0]["FLDLATDIRECTION"].ToString();
                ucLongitude.TextDegree = dt.Rows[0]["FLDLONG1"].ToString();
                ucLongitude.TextMinute = dt.Rows[0]["FLDLONG2"].ToString();
                ucLongitude.TextSecond = dt.Rows[0]["FLDLONG3"].ToString();
                ucLongitude.TextDirection = dt.Rows[0]["FLDLONGDIRECTION"].ToString();
                txtCourse.Text = dt.Rows[0]["FLDCOURSE"].ToString();
                txtDistanceObserved.Text = dt.Rows[0]["FLDDISTANCEOBSERVED"].ToString();
                txtAirTemp.Text = dt.Rows[0]["FLDAIRTEMP"].ToString();
                txtDWT.Text = dt.Rows[0]["FLDDWT"].ToString();
                txtDraftA.Text = dt.Rows[0]["FLDDRAFT"].ToString();
                txtLogSpeed.Text = dt.Rows[0]["FLDLOGSPEED"].ToString();
                txtWindForce.Text = dt.Rows[0]["FLDWINDFORCE"].ToString();
                txtCurrentSpeed.Text = dt.Rows[0]["FLDCURRENTSPEED"].ToString();
                txtSeaHeight.Text = dt.Rows[0]["FLDSEAHEIGHT"].ToString();
                ucWindDirection.SelectedDirection = dt.Rows[0]["FLDWINDDIRECTION"].ToString();
                ucCurrentDirection.SelectedDirection = dt.Rows[0]["FLDCURRENTDIRECTION"].ToString();
                ucSeaDirection.SelectedDirection = dt.Rows[0]["FLDSEADIRECTION"].ToString();
                if (General.GetNullableString(dt.Rows[0]["FLDBALLASTYN"].ToString()) != null)
                    rbtnBallastLaden.SelectedValue = dt.Rows[0]["FLDBALLASTYN"].ToString();
                txtDraftF.Text = dt.Rows[0]["FLDDRAFTF"].ToString();
                txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
                txtFullSpeed.Text = dt.Rows[0]["FLDFULLSPEED"].ToString();
                txtFSDistance.Text = dt.Rows[0]["FLDFULLSPEEDDISTANCE"].ToString();
                txtReducedSpeed.Text = dt.Rows[0]["FLDREDUCEDSPEED"].ToString();
                txtRSDistance.Text = dt.Rows[0]["FLDREDUCEDDISTANCE"].ToString();
                txtStopped.Text = dt.Rows[0]["FLDSTOPPED"].ToString();
                txtNoonSpeed.Text = dt.Rows[0]["FLDNOONSPEED"].ToString();
                txtVOCons.Text = dt.Rows[0]["FLDVOYAGEORDERCONS"].ToString();
                txtVOSpeed.Text = dt.Rows[0]["FLDVOYAGEORDERSPEED"].ToString();
                chkIceDeck.Checked = dt.Rows[0]["FLDICINGONDECK"].ToString().Equals("1") ? true : false;
                txtSwell.Text = dt.Rows[0]["FLDSWELL"].ToString();
                txtSwellTemp.Text = dt.Rows[0]["FLDSWTEMP"].ToString();
                txtERExhTemp.Text = dt.Rows[0]["FLDERTEMP"].ToString();
                txtEngineDistance.Text = dt.Rows[0]["FLDENGINEDISTANCE"].ToString();
                txtRemarksCE.Text = dt.Rows[0]["FLDREMARKSCE"].ToString();
                txtGovernorSetting.Text = dt.Rows[0]["FLDGOVERNORSETTING"].ToString();
                txtSpeedSetting.Text = dt.Rows[0]["FLDSPEEDSETTING"].ToString();
                txtFOInletTemp.Text = dt.Rows[0]["FLDFOINLETTEMP"].ToString();
                txtFuelOilPress.Text = dt.Rows[0]["FLDFUELOILPRESS"].ToString();
                txtBilgeROB.Text = dt.Rows[0]["FLDBILGETANKROB"].ToString();
                txtSludgeROB.Text = dt.Rows[0]["FLDSLUDGETANKROB"].ToString();
                txtLastLandSludge.Text = dt.Rows[0]["FLDLASTLANDINGSLUDGE"].ToString();
                txtLastLandingDays.Text = dt.Rows[0]["FLDDAYSFROMLASTLANDING"].ToString();
                txtBoilerWaterChlorides.Text = dt.Rows[0]["FLDBOILERCHLORIDES"].ToString();
                txtAvgSlip.Text = dt.Rows[0]["FLDSLIP"].ToString();
                txtMERPM.Text = dt.Rows[0]["FLDMERPM"].ToString();
                txtBilgeLandingDays.Text = dt.Rows[0]["FLDDAYSLASTLANDINGBILGE"].ToString();
                txtHFOTankCleaning.Text = dt.Rows[0]["FLDHFOTANKCLEANING"].ToString();
                txtBilgeLanding.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLASTLANDINGBILGE"].ToString());

                txtLastLandSludgeTime.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGSLUDGE"]);
                txtBilgeLandingTime.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGBILGE"]);

                txtGeneralLoadAE1.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE1"]));
                txtGeneralLoadAE2.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE2"]));
                txtGeneralLoadAE3.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE3"]));
                txtGeneratorLoadAE4.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE4"]));
                
                txtGeneratorLoadAE1OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE1OPHRS"].ToString();
                txtGeneratorLoadAE2OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE2OPHRS"].ToString();
                txtGeneratorLoadAE3OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE3OPHRS"].ToString();
                txtGeneratorLoadAE4OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE4OPHRS"].ToString();

                txtHFOCargoHeating.Text = dt.Rows[0]["FLDHFOCARGOHEATING"].ToString();
                ChkECAyn.Checked = dt.Rows[0]["FLDECAYN"].ToString().Equals("1") ? true : false;
                string oiltype = dt.Rows[0]["FLDECAOILTYPE"].ToString();
                if (oiltype != "")
                    ddlECAOilType.SelectedValue = oiltype;
                txtECAEntryDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDECAENTERDATE"]);
                if (General.GetNullableDateTime(dt.Rows[0]["FLDECAENTERDATE"].ToString()) != null)
                    txtTimeofECAEntry.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDECAENTERDATE"].ToString());
                chkCounterDefective.Checked = dt.Rows[0]["FLDEMLOGCOUNTERDEFECTIVE"].ToString().Equals("1") ? true : false;
                chkRevCounterDefective.Checked = dt.Rows[0]["FLDMEREVCOUNTERDEFECTIVE"].ToString().Equals("1") ? true : false;

                if (chkRevCounterDefective.Checked == true)
                {
                    txtMERPM.CssClass = "input";
                    txtMERPM.Enabled = true;
                }
                else
                {
                    txtMERPM.CssClass = "readonlytextbox";
                    txtMERPM.Enabled = false;
                }
            }
        }
        else
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                DataSet ds = PhoenixVesselPositionNoonReport.NoonReportFirstEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

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
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (ddlVoyagePort.VoyageId != txtVoyageId.Text)
        {
            ddlVoyagePort.SelectedValue = "";
            ddlVoyagePort.SelectedPortCallValue = "";
            ddlVoyagePort.Text = "";
        }
        ddlVoyagePort.VoyageId = txtVoyageId.Text;
    }

    protected void gvConsumption_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("CALCULATE"))
            {
                if (ViewState["NOONREPORTID"] == null)
                {
                    ucError.ErrorMessage = "Please click the save button for the 'Arrival Report' and then update the Consumption details.";
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
                    if(bunkerqty != null && bunkerqty > 0 && ViewState["DEBUNKER"].ToString() == "1")
                    {
                        bunkerqty = -1 *  bunkerqty;
                    }
                    PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    new Guid(ViewState["NOONREPORTID"].ToString()),
                    new Guid(((RadLabel)e.Item.FindControl("lblOilTypeid")).Text),
                    "ARRIVAL",
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
                    1, // ROB @ EOSP AND ROB @ FWE YN
                    1, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                    0, // RECALCULATE ROB
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                    null,
                    null,
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                    bunkerqty,
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit")).Text)
                    );
                    ViewState["DEBUNKER"] = 0;
                    gvConsumptionRebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (ViewState["NOONREPORTID"] == null || General.GetNullableGuid(ViewState["NOONREPORTID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Please click the save button for the 'Arrival Report' and then update the Consumption details.";
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
                        "ARRIVAL",
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
                        1, // ROB @ EOSP AND ROB @ FWE YN
                        1, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                        0, // RECALCULATE ROB
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                        null,
                        null,
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                        bunkerqty,
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit")).Text)
                        );
                    ViewState["DEBUNKER"] = 0;
                    gvConsumptionRebind();
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
                        PhoenixVesselPositionNoonReportOilConsumption.DeleteBunkerOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid((OilConsumptionId.Text)), "ARRIVAL");

                }

                gvConsumptionRebind();
                BindNoonReport();
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

                LinkButton cmdBunkerAdd = (LinkButton)e.Item.FindControl("cmdBunkerAdd");
                UserControlMaskNumber txtSulphurPercentageEdit = (UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit");

                if (txtSulphurPercentageEdit != null)
                {
                    if (drv["FLDFUELOIL"].ToString() == "1") txtSulphurPercentageEdit.Enabled = true; else txtSulphurPercentageEdit.Enabled = false;
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
                if (ucAtSeaAEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (!drv["FLDSHORTNAME"].ToString().ToUpper() .Equals("AECC")) ucAtSeaAEEdit.Enabled = false; else ucAtSeaAEEdit.Enabled = true;
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
            General.GetNullableGuid(noonreportid), "ARRIVAL");
            gvConsumption.DataSource = ds;
    }
    protected void gvConsumptionRebind()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }

    // Other oil consumption..
    protected void gvOtherOilCons_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

        ds = PhoenixVesselPositionNoonReportOilConsumption.ListOtherOilConsumption(
            iVesselId,
            General.GetNullableGuid(noonreportid));
            gvOtherOilCons.DataSource = ds;
    }
    protected void gvOtherOilConsRebind()
    {
        gvOtherOilCons.SelectedIndexes.Clear();
        gvOtherOilCons.EditIndexes.Clear();
        gvOtherOilCons.DataSource = null;
        gvOtherOilCons.Rebind();
    }
  

    private void InsertOtherOilConsumption()
    {
        foreach (GridDataItem gvr in gvOtherOilCons.Items)
        {

            PhoenixVesselPositionNoonReportOilConsumption.InsertOtherOilConsumption(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              int.Parse(ViewState["VESSELID"].ToString()),
              new Guid(ViewState["NOONREPORTID"].ToString()),
              new Guid(((RadLabel)gvr.FindControl("lblOilTypeCodeEdit")).Text),
              "ARRIVAL",
              null,
              General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtROBEdit")).Text),
              General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtProducedEdit")).Text),
              General.GetNullableGuid(((RadLabel)gvr.FindControl("lblOilConsumptionIdEdit")).Text));

        }
        gvOtherOilConsRebind();
    }


    protected void ChkECAyn_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ChkECAyn.Checked == true)
        {
            txtECAEntryDate.Enabled = true;
            txtTimeofECAEntry.Enabled = true;
            ddlECAOilType.Enabled = true;

            txtECAEntryDate.CssClass = "input";
            txtTimeofECAEntry.CssClass = "input";
            ddlECAOilType.CssClass = "input";
        }
        else
        {
            txtECAEntryDate.Enabled = false;
            txtTimeofECAEntry.Enabled = false;
            ddlECAOilType.Enabled = false;

            txtECAEntryDate.CssClass = "readonlytextbox";
            txtTimeofECAEntry.CssClass = "readonlytextbox";
            ddlECAOilType.CssClass = "readonlytextbox";

            txtECAEntryDate.Text = "";
            txtTimeofECAEntry.SelectedDate = null;
            ddlECAOilType.SelectedIndex = -1;
        }
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

    protected void OnTextChange(object sender, EventArgs e)
    {
        if (ViewState["EOSP"] != null)
        {
            if (ViewState["EOSP"].ToString() != txtESOP.Text)
            {
                ViewState["EOSP"] = txtESOP.Text;
                //txtPOB.Text = txtESOP.Text;
                txtFWE.Text = txtESOP.Text;
                //txtDOP.Text = txtESOP.Text;
                //txtNORT.Text = txtESOP.Text;
                //txtNORA.Text = txtESOP.Text;
                //txtFPG.Text = txtESOP.Text;
            }
            
        }
        else
        {
            ViewState["EOSP"] = txtESOP.Text;
            //txtPOB.Text = txtESOP.Text;
            txtFWE.Text = txtESOP.Text;
            //txtDOP.Text = txtESOP.Text;
            //txtNORT.Text = txtESOP.Text;
            //txtNORA.Text = txtESOP.Text;
            //txtFPG.Text = txtESOP.Text;
        }
        txtESOPTime.Focus();
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
                "ARRIVAL",
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
                1, // ROB @ EOSP AND ROB @ FWE YN
                1, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                1, // RECALCULATE ROB
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                null,
                null,
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                bunkerqty,
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("txtSulphurPercentageEdit")).Text)
                );

            ucStatus.Text = "ROB Recalculated";
            ViewState["DEBUNKER"] = 0;

            gvConsumptionRebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkRevCounterDefective_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkRevCounterDefective.Checked == true)
        {
            txtMERPM.CssClass = "input";
            txtMERPM.Enabled = true;
        }
        else
        {
            txtMERPM.CssClass = "readonlytextbox";
            txtMERPM.Enabled = false;
        }

    }
    protected void chkCounterDefective_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCounterDefective.Checked == true)
        {
            txtNoonSpeed.CssClass = "input";
            txtNoonSpeed.Enabled = true;

            txtEMLogDistance.CssClass = "input";
            txtEMLogDistance.Enabled = true;

            txtEMLogManoeuveringDistance.CssClass = "input";
            txtEMLogManoeuveringDistance.Enabled = true;
        }
        else
        {
            txtNoonSpeed.CssClass = "readonlytextbox";
            txtNoonSpeed.Enabled = false;

            txtEMLogDistance.CssClass = "readonlytextbox";
            txtEMLogDistance.Enabled = false;

            txtEMLogManoeuveringDistance.CssClass = "readonlytextbox";
            txtEMLogManoeuveringDistance.Enabled = false;
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
                    item["meatsea"].ColumnSpan = 8;
                    item["aeatsea"].Visible = false;
                    item["blratsea"].Visible = false;
                    item["iggatsea"].Visible = false;
                    item["cengatsea"].Visible = false;
                    item["chtgatsea"].Visible = false;
                    item["tkclngatsea"].Visible = false;
                    item["othatsea"].Visible = false;

                    item["meatharbour"].ColumnSpan = 8;
                    item["aeatharbour"].Visible = false;
                    item["blratharbour"].Visible = false;
                    item["iggatharbour"].Visible = false;
                    item["cengatharbour"].Visible = false;
                    item["chtgatharbour"].Visible = false;
                    item["tkclngatharbour"].Visible = false;
                    item["othatharbour"].Visible = false;

                    item["bunker"].ColumnSpan = 3;
                    item["sulphur"].Visible = false;
                    item["total"].Visible = false;
                }
            }
        }
    }
}
