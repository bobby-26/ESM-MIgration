using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;

public partial class VesselPositionWedSunReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarsubtap = new PhoenixToolbar();
            toolbarsubtap.AddButton("Deck Dept", "DECK");
            toolbarsubtap.AddButton("Engine Dept", "ENGINE");
            MenuNRSubTab.AccessRights = this.ViewState;
            MenuNRSubTab.MenuList = toolbarsubtap.Show();
            MenuNRSubTab.SelectedMenuIndex = 0;

            ViewMenu();
          
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
                toolbarvoyagetap.AddButton("List", "WEDSUNREPORTLIST");
                toolbarvoyagetap.AddButton("WedSun Report", "WEDSUNREPORT");
                MenuWedSunReportTap.AccessRights = this.ViewState;
                MenuWedSunReportTap.MenuList = toolbarvoyagetap.Show();
                MenuWedSunReportTap.SelectedMenuIndex = 1;

                cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                txtVoyageId.Attributes.Add("style", "visibility:hidden");
                

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                    btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");
                }
                else
                {
                    ViewState["VESSELID"] = "";
                    btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=0', true); ");
                }

                if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString().Equals("NEW"))
                {
                    Session["NOONREPORTID"] = null;
                    txtCurrentDate.Text = General.GetDateTimeToString(System.DateTime.Now);

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
                            }
                        }
                    }
                    ddlCurrentPort.Items.Insert(0, "--Select--");
                    ddlNextPort.Items.Insert(0, "--Select--");
                }
                else
                {
                    BindData();
                    SetFieldRange();
                }

                DataSet Ds = PhoenixRegistersOilType.ListOilType(1, 1);// For only fueloil
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
                    txtTimeofECAEntry.Text = "";
                    ddlECAOilType.SelectedIndex = -1;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCurrentPort()
    {
        if (ViewState["VESSELID"].ToString() != "")
        {
            DataSet PortDs = PhoenixVesselPositionNoonReport.GetVoyagePorts(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["VESSELID"].ToString()),
                                                                            General.GetNullableGuid(txtVoyageId.Text));
            ddlCurrentPort.DataSource = PortDs.Tables[0];
            ddlCurrentPort.DataBind();
            ddlCurrentPort.Items.Insert(0, "--Select--");

            ddlNextPort.DataSource = PortDs.Tables[0];
            ddlNextPort.DataBind();
            ddlNextPort.Items.Insert(0, "--Select--");

            Guid? noonreportid = null;

            if (Session["NOONREPORTID"] != null)
                noonreportid = General.GetNullableGuid(Session["NOONREPORTID"].ToString());

            DataSet ds = PhoenixVesselPositionNoonReport.GetCurrentPort(int.Parse(ViewState["VESSELID"].ToString()), General.GetNullableGuid(txtVoyageId.Text), noonreportid);

            if (Session["NOONREPORTID"] == null && ViewState["OLDNOONREPORTID"] == null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString() != "")
                        ddlCurrentPort.SelectedValue = dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString();
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    if (dt.Rows[0]["FLDNEXTPORTCALLID"].ToString() != "")
                        ddlNextPort.SelectedValue = dt.Rows[0]["FLDNEXTPORTCALLID"].ToString();
                }
            }
        }

        if (ddlVesselStatus.SelectedValue == "INPORT")
        {
            ddlCurrentPort.Enabled = true;
            ddlNextPort.Enabled = true;
        }
        else if (ddlVesselStatus.SelectedValue == "ATANCHOR")
        {
            ddlCurrentPort.Enabled = true;
            ddlNextPort.Enabled = true;
        }
        else if (ddlVesselStatus.SelectedValue == "ATSEA")
        {
            ddlCurrentPort.SelectedIndex = -1;
            ddlCurrentPort.Enabled = false;
            ddlNextPort.Enabled = true;
        }
        else
        {
            ddlCurrentPort.Enabled = false;
            ddlNextPort.Enabled = false;
            ddlCurrentPort.SelectedIndex = -1;
            ddlNextPort.SelectedIndex = -1;
        }
    }

    private void Reset()
    {
        if (ViewState["VESSELID"].ToString() != "")
        {
            Guid? noonreportid = null;

            if (Session["NOONREPORTID"] != null)
                noonreportid = General.GetNullableGuid(Session["NOONREPORTID"].ToString());

            DataSet ds = PhoenixVesselPositionWedSunReport.ResetWedSunReport(int.Parse(ViewState["VESSELID"].ToString()), noonreportid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                ViewState["OLDNOONREPORTID"] = dt.Rows[0]["FLDNOONREPORTID"].ToString();

                txtCurrentDate.Text = General.GetDateTimeToString(System.DateTime.Now);

                txtVoyageId.Text = dt.Rows[0]["FLDVOYAGEID"].ToString();
                txtVoyageName.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
                btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");

                txtETA.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETA"]);
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

                txtShipMeanTime.Text = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
                txtShipMeanTimeSymbol.Text = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";

                txtTimeOfETA.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDETA"]);

                ddlVesselStatus.SelectedValue = dt.Rows[0]["FLDVESSELSTATUS"].ToString();
                txtFullSpeed.Text = dt.Rows[0]["FLDFULLSPEED"].ToString();
                txtFSDistance.Text = dt.Rows[0]["FLDFULLSPEEDDISTANCE"].ToString();
                txtReducedSpeed.Text = dt.Rows[0]["FLDREDUCEDSPEED"].ToString();
                txtRSDistance.Text = dt.Rows[0]["FLDREDUCEDDISTANCE"].ToString();
                txtStopped.Text = dt.Rows[0]["FLDSTOPPED"].ToString();
                txtNoonSpeed.Text = dt.Rows[0]["FLDNOONSPEED"].ToString();
                txtVOCons.Text = dt.Rows[0]["FLDVOYAGEORDERCONS"].ToString();
                txtVOSpeed.Text = dt.Rows[0]["FLDVOYAGEORDERSPEED"].ToString();
                chkIceDeck.Checked = dt.Rows[0]["FLDICINGONDECK"].ToString().Equals("1") ? true : false;

                txtETB.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETB"]);
                txtTimeOfETB.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDETB"]);
                txtETC.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETC"]);
                txtTimeOfETC.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDETC"]);
                txtSwell.Text = dt.Rows[0]["FLDSWELL"].ToString();
                txtBerthLocation.Text = dt.Rows[0]["FLDBERTHLOCATION"].ToString();
                ChkUSWaters.Checked = dt.Rows[0]["FLDISUSWATERS"].ToString().Equals("1") ? true : false;
                ChkThroughHRA.Checked = dt.Rows[0]["FLDISPASSINGTHROUGHHRA"].ToString().Equals("1") ? true : false;

                ChkECAyn.Checked = dt.Rows[0]["FLDECAYN"].ToString().Equals("1") ? true : false;
                string oiltype = dt.Rows[0]["FLDECAOILTYPE"].ToString();
                if (oiltype != "")
                    ddlECAOilType.SelectedValue = oiltype;
                txtECAEntryDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDECAENTERDATE"]);
                txtTimeofECAEntry.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDECAENTERDATE"]);

                BindCurrentPort();

                if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("INPORT"))
                {

                    txtBerthLocation.CssClass = "input";
                    txtBerthLocation.Enabled = true;

                    if (dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString() != "")
                        ddlCurrentPort.SelectedValue = dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString();

                    if (dt.Rows[0]["FLDNEXTPORTCALLID"].ToString() != "")
                        ddlNextPort.SelectedValue = dt.Rows[0]["FLDNEXTPORTCALLID"].ToString();

                    ddlCurrentPort.Enabled = true;
                    ddlNextPort.Enabled = true;

                    txtETA.Text = "";
                    txtETA.CssClass = "readonlytextbox";
                    txtETA.Enabled = false;
                    txtTimeOfETA.Text = "";
                    txtTimeOfETA.CssClass = "readonlytextbox";
                    txtTimeOfETA.Enabled = false;

                    txtETB.Text = "";
                    txtETB.CssClass = "readonlytextbox";
                    txtETB.Enabled = false;
                    txtTimeOfETB.Text = "";
                    txtTimeOfETB.CssClass = "readonlytextbox";
                    txtTimeOfETB.Enabled = false;

                    txtETC.CssClass = "input";
                    txtETC.Enabled = true;
                    txtTimeOfETC.CssClass = "input";
                    txtTimeOfETC.Enabled = true;

                    txtFullSpeed.Text = "";
                    txtFullSpeed.CssClass = "readonlytextbox";
                    txtFullSpeed.Enabled = false;

                    txtReducedSpeed.Text = "";
                    txtReducedSpeed.CssClass = "readonlytextbox";
                    txtReducedSpeed.Enabled = false;

                    txtStopped.Text = "";
                    txtStopped.CssClass = "readonlytextbox";
                    txtStopped.Enabled = false;

                    txtDistanceObserved.Text = "";
                    txtDistanceObserved.CssClass = "readonlytextbox";
                    txtDistanceObserved.Enabled = false;

                    txtLogSpeed.Text = "";
                    txtLogSpeed.CssClass = "readonlytextbox";
                    txtLogSpeed.Enabled = false;

                    txtNoonSpeed.Text = "";
                    txtNoonSpeed.CssClass = "readonlytextbox";
                    txtNoonSpeed.Enabled = false;

                    txtVOSpeed.Text = "";
                    txtVOSpeed.CssClass = "readonlytextbox";
                    txtVOSpeed.Enabled = false;

                    txtCourse.Text = "";
                    txtCourse.CssClass = "readonlytextbox";
                    txtCourse.Enabled = false;

                    txtDraftF.CssClass = "input";
                    txtDraftF.Enabled = true;

                    txtDraftA.CssClass = "input";
                    txtDraftA.Enabled = true;

                    txtDWT.Text = "";
                    txtDWT.CssClass = "readonlytextbox";
                    txtDWT.Enabled = false;

                    txtSeaHeight.Text = "";
                    txtSeaHeight.CssClass = "readonlytextbox";
                    txtSeaHeight.Enabled = false;

                    ucSeaDirection.SelectedDirection = "Dummy";
                    ucSeaDirection.CssClass = "readonlytextbox";
                    ucSeaDirection.Enabled = "false";

                    txtSwell.Text = "";
                    txtSwell.CssClass = "readonlytextbox";
                    txtSwell.Enabled = false;

                    txtCurrentSpeed.Text = "";
                    txtCurrentSpeed.CssClass = "readonlytextbox";
                    txtCurrentSpeed.Enabled = false;

                    ucCurrentDirection.SelectedDirection = "Dummy";
                    ucCurrentDirection.CssClass = "readonlytextbox";
                    ucCurrentDirection.Enabled = "false";

                    txtFSDistance.Text = "";
                    txtFSDistance.CssClass = "readonlytextbox";
                    txtFSDistance.Enabled = false;

                    txtRSDistance.Text = "";
                    txtRSDistance.CssClass = "readonlytextbox";
                    txtRSDistance.Enabled = false;

                    txtVOCons.Text = "";
                    txtVOCons.CssClass = "readonlytextbox";
                    txtVOCons.Enabled = false;
                }
                else if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATANCHOR"))
                {

                    txtBerthLocation.CssClass = "input";
                    txtBerthLocation.Enabled = true;

                    if (dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString() != "")
                        ddlCurrentPort.SelectedValue = dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString();

                    if (dt.Rows[0]["FLDNEXTPORTCALLID"].ToString() != "")
                        ddlNextPort.SelectedValue = dt.Rows[0]["FLDNEXTPORTCALLID"].ToString();

                    ddlCurrentPort.Enabled = true;
                    ddlNextPort.Enabled = true;

                    txtETA.Text = "";
                    txtETA.CssClass = "readonlytextbox";
                    txtETA.Enabled = false;
                    txtTimeOfETA.Text = "";
                    txtTimeOfETA.CssClass = "readonlytextbox";
                    txtTimeOfETA.Enabled = false;

                    txtETB.CssClass = "input";
                    txtETB.Enabled = true;
                    txtTimeOfETB.CssClass = "input";
                    txtTimeOfETB.Enabled = true;

                    txtETC.CssClass = "input";
                    txtETC.Enabled = true;
                    txtTimeOfETC.CssClass = "input";
                    txtTimeOfETC.Enabled = true;

                    txtFullSpeed.Text = "";
                    txtFullSpeed.CssClass = "readonlytextbox";
                    txtFullSpeed.Enabled = false;

                    txtReducedSpeed.Text = "";
                    txtReducedSpeed.CssClass = "readonlytextbox";
                    txtReducedSpeed.Enabled = false;

                    txtStopped.Text = "";
                    txtStopped.CssClass = "readonlytextbox";
                    txtStopped.Enabled = false;

                    txtDistanceObserved.Text = "";
                    txtDistanceObserved.CssClass = "readonlytextbox";
                    txtDistanceObserved.Enabled = false;

                    txtLogSpeed.Text = "";
                    txtLogSpeed.CssClass = "readonlytextbox";
                    txtLogSpeed.Enabled = false;

                    txtNoonSpeed.Text = "";
                    txtNoonSpeed.CssClass = "readonlytextbox";
                    txtNoonSpeed.Enabled = false;

                    txtVOSpeed.Text = "";
                    txtVOSpeed.CssClass = "readonlytextbox";
                    txtVOSpeed.Enabled = false;

                    txtCourse.Text = "";
                    txtCourse.CssClass = "readonlytextbox";
                    txtCourse.Enabled = false;

                    txtDraftF.CssClass = "input";
                    txtDraftF.Enabled = true;

                    txtDraftA.CssClass = "input";
                    txtDraftA.Enabled = true;

                    txtDWT.Text = "";
                    txtDWT.CssClass = "readonlytextbox";
                    txtDWT.Enabled = false;

                    txtSeaHeight.CssClass = "input";
                    txtSeaHeight.Enabled = true;

                    ucSeaDirection.CssClass = "input";
                    ucSeaDirection.Enabled = "true";

                    txtSwell.Text = "";
                    txtSwell.CssClass = "readonlytextbox";
                    txtSwell.Enabled = false;

                    txtCurrentSpeed.CssClass = "input";
                    txtCurrentSpeed.Enabled = true;

                    ucCurrentDirection.CssClass = "input";
                    ucCurrentDirection.Enabled = "true";
                }
                else if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATSEA"))
                {
                    txtBerthLocation.Text = "";
                    txtBerthLocation.CssClass = "readonlytextbox";
                    txtBerthLocation.Enabled = false;

                    ddlCurrentPort.SelectedIndex = -1;

                    if (dt.Rows[0]["FLDNEXTPORTCALLID"].ToString() != "")
                        ddlNextPort.SelectedValue = dt.Rows[0]["FLDNEXTPORTCALLID"].ToString();

                    ddlCurrentPort.Enabled = false;
                    ddlNextPort.Enabled = true;

                    txtETA.CssClass = "input";
                    txtETA.Enabled = true;
                    txtTimeOfETA.CssClass = "input";
                    txtTimeOfETA.Enabled = true;

                    txtETB.Text = "";
                    txtETB.CssClass = "readonlytextbox";
                    txtETB.Enabled = false;
                    txtTimeOfETB.Text = "";
                    txtTimeOfETB.CssClass = "readonlytextbox";
                    txtTimeOfETB.Enabled = false;

                    txtETC.Text = "";
                    txtETC.CssClass = "readonlytextbox";
                    txtETC.Enabled = false;
                    txtTimeOfETC.Text = "";
                    txtTimeOfETC.CssClass = "readonlytextbox";
                    txtTimeOfETC.Enabled = false;

                    txtFullSpeed.CssClass = "input";
                    txtFullSpeed.Enabled = true;

                    txtReducedSpeed.CssClass = "input";
                    txtReducedSpeed.Enabled = true;

                    txtStopped.CssClass = "input";
                    txtStopped.Enabled = true;

                    txtDistanceObserved.CssClass = "input";
                    txtDistanceObserved.Enabled = true;

                    txtLogSpeed.CssClass = "input";
                    txtLogSpeed.Enabled = true;

                    txtNoonSpeed.CssClass = "input";
                    txtNoonSpeed.Enabled = true;

                    txtVOSpeed.CssClass = "input";
                    txtVOSpeed.Enabled = true;

                    txtCourse.CssClass = "input";
                    txtCourse.Enabled = true;

                    txtDraftF.Text = "";
                    txtDraftF.CssClass = "readonlytextbox";
                    txtDraftF.Enabled = false;

                    txtDraftA.Text = "";
                    txtDraftA.CssClass = "readonlytextbox";
                    txtDraftA.Enabled = false;

                    txtDWT.CssClass = "input";
                    txtDWT.Enabled = true;

                    txtSeaHeight.CssClass = "input";
                    txtSeaHeight.Enabled = true;

                    ucSeaDirection.CssClass = "input";
                    ucSeaDirection.Enabled = "true";

                    txtSwell.CssClass = "input";
                    txtSwell.Enabled = true;

                    txtCurrentSpeed.CssClass = "input";
                    txtCurrentSpeed.Enabled = true;

                    ucCurrentDirection.CssClass = "input";
                    ucCurrentDirection.Enabled = "true";
                }

            }
        }
    }

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("COPY"))
            {
                Reset();
            }

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDate())
                {
                    ucError.Visible = true;
                    return;
                }

                if (Session["NOONREPORTID"] == null)
                {
                    AddWedSunReport();
                }
                else
                {
                    UpdateWedSunReport();
                }

                if (Session["NOONREPORTID"] != null)
                {
                    BindData();
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

    private void UpdateWedSunReport()
    {
        try
        {
            if (!IsValidNoonReport())
            {
                ucError.Visible = true;
                return;
            }

            string timeofeta = (txtTimeOfETA.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETA.Text;
            string timeofetb = (txtTimeOfETB.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETB.Text;
            string timeofetc = (txtTimeOfETC.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETC.Text;
            string timeofeca = (txtTimeofECAEntry.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeofECAEntry.Text;

            PhoenixVesselPositionWedSunReport.UpdateWedSunReport(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(Session["NOONREPORTID"].ToString()),
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(txtVoyageId.Text), txtCourse.Text,
                ucLatitude.TextDegree, ucLongitude.TextDegree, ucLatitude.TextMinute, ucLongitude.TextMinute, ucLatitude.TextSecond, ucLongitude.TextSecond, ucLatitude.TextDirection, ucLongitude.TextDirection,
                General.GetNullableInteger(rbtnBallastLaden.SelectedValue),
                General.GetNullableDecimal(txtDWT.Text), General.GetNullableDecimal(txtDraftA.Text),
                CalculateDistanceObserved(txtFSDistance.Text, txtRSDistance.Text),
                CalculateLogSpeed(txtFullSpeed.Text, txtFSDistance.Text, txtReducedSpeed.Text, txtRSDistance.Text), General.GetNullableDecimal(""),
                General.GetNullableGuid(ucWindDirection.SelectedDirection), General.GetNullableDecimal(txtWindForce.Text),
                General.GetNullableDecimal(txtSeaHeight.Text), General.GetNullableGuid(ucSeaDirection.SelectedDirection),
                General.GetNullableDecimal(txtCurrentSpeed.Text), General.GetNullableGuid(ucCurrentDirection.SelectedDirection),
                General.GetNullableDecimal(txtAirTemp.Text),
                //General.GetNullableInteger(CalculateSlip(txtEngineDistance.Text, txtDistanceObserved.Text).ToString()),
                txtRemarks.Text,
                (txtShipMeanTimeSymbol.Text + txtShipMeanTime.Text), General.GetNullableDecimal(txtDraftF.Text),
                General.GetNullableDateTime(txtCurrentDate.Text),
                General.GetNullableDateTime(txtETA.Text + " " + timeofeta),
                General.GetNullableGuid(ddlCurrentPort.SelectedValue),
                General.GetNullableGuid(ddlNextPort.SelectedValue),
                ddlVesselStatus.SelectedValue,
                General.GetNullableDecimal(txtFullSpeed.Text), General.GetNullableDecimal(txtFSDistance.Text),
                General.GetNullableDecimal(txtReducedSpeed.Text), General.GetNullableDecimal(txtRSDistance.Text),
                General.GetNullableDecimal(txtStopped.Text), General.GetNullableDecimal(txtNoonSpeed.Text),
                General.GetNullableDecimal(txtVOSpeed.Text), General.GetNullableDecimal(txtVOCons.Text),
                chkIceDeck.Checked == true ? 1 : 0,
                0
                , General.GetNullableString(txtBerthLocation.Text)
                , General.GetNullableDateTime(txtETB.Text + " " + timeofetb)
                , General.GetNullableDateTime(txtETC.Text + " " + timeofetc)
                , General.GetNullableDecimal(txtSwell.Text)
                , ChkUSWaters.Checked == true ? 1 : 0
                , ChkThroughHRA.Checked == true ? 1 : 0
                , ChkECAyn.Checked == true ? 1 : 0
                , General.GetNullableGuid(ddlECAOilType.SelectedValue)
                , General.GetNullableDateTime(txtECAEntryDate.Text + " " + timeofeca),
                "WEDSUN",null,null,null,null,0);

            PhoenixVesselPositionNoonReport.InsertESIRegister(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ViewState["VESSELID"].ToString()),
                    General.GetNullableGuid(txtVoyageId.Text),
                    General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

            PhoenixVesselPositionNoonReport.UpdateESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(txtVoyageId.Text),
                General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

            ucStatus.Text = "WedSun Report updated.";
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void AddWedSunReport()
    {
        try
        {
            Guid? noonreportid = null;

            if (!IsValidNoonReport())
            {
                ucError.Visible = true;
                return;
            }

            string timeofeta = (txtTimeOfETA.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETA.Text;
            string timeofetb = (txtTimeOfETB.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETB.Text;
            string timeofetc = (txtTimeOfETC.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETC.Text;
            string timeofeca = (txtTimeofECAEntry.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeofECAEntry.Text;


            PhoenixVesselPositionWedSunReport.InsertWedSunReport(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(txtVoyageId.Text), txtCourse.Text,
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
                (txtShipMeanTimeSymbol.Text + txtShipMeanTime.Text), General.GetNullableDecimal(txtDraftF.Text),
                General.GetNullableDateTime(txtCurrentDate.Text),
                General.GetNullableDateTime(txtETA.Text + " " + timeofeta),
                General.GetNullableGuid(ddlCurrentPort.SelectedValue),
                General.GetNullableGuid(ddlNextPort.SelectedValue),
                ddlVesselStatus.SelectedValue,
                General.GetNullableDecimal(txtFullSpeed.Text), General.GetNullableDecimal(txtFSDistance.Text),
                General.GetNullableDecimal(txtReducedSpeed.Text), General.GetNullableDecimal(txtRSDistance.Text),
                General.GetNullableDecimal(txtStopped.Text), General.GetNullableDecimal(txtNoonSpeed.Text),
                General.GetNullableDecimal(txtVOSpeed.Text), General.GetNullableDecimal(txtVOCons.Text),
                chkIceDeck.Checked == true ? 1 : 0,
                0,
                ref noonreportid
                , General.GetNullableString(txtBerthLocation.Text)
                , General.GetNullableDateTime(txtETB.Text + " " + timeofetb)
                , General.GetNullableDateTime(txtETC.Text + " " + timeofetc)
                , General.GetNullableDecimal(txtSwell.Text)
                , ChkUSWaters.Checked == true ? 1 : 0
                , ChkThroughHRA.Checked == true ? 1 : 0
                , ChkECAyn.Checked == true ? 1 : 0
                , General.GetNullableGuid(ddlECAOilType.SelectedValue)
                , General.GetNullableDateTime(txtECAEntryDate.Text + " " + timeofeca),
                "WEDSUN", null, 0, 0);

            Session["NOONREPORTID"] = noonreportid;
            ucStatus.Text = "WedSun Report Created";


            PhoenixVesselPositionNoonReport.InsertESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(txtVoyageId.Text),
                noonreportid);

            PhoenixVesselPositionNoonReport.UpdateESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(txtVoyageId.Text),
                noonreportid);

            Response.Redirect("VesselPositionWedSunReport.aspx?WedSunReportID=" + noonreportid, false);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDate()
    {
        string timeofeta = (txtTimeOfETA.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETA.Text;
        string timeofetb = (txtTimeOfETB.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETB.Text;
        string timeofetc = (txtTimeOfETC.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtTimeOfETC.Text;

        if (txtETA.Text != null)
            if (General.GetNullableDateTime(txtETA.Text + " " + timeofeta) == null)
                ucError.ErrorMessage = "ETA is not Valid time.";
        if (txtETB.Text != null)
            if (General.GetNullableDateTime(txtETB.Text + " " + timeofetb) == null)
                ucError.ErrorMessage = "ETB is not Valid time.";
        if (txtETC.Text != null)
            if (General.GetNullableDateTime(txtETC.Text + " " + timeofetc) == null)
                ucError.ErrorMessage = "ETC is not Valid time.";

        if (ddlVesselStatus.SelectedValue == "INPORT")
        {
            if (ddlCurrentPort.SelectedValue == "--Select--" && ddlNextPort.SelectedValue != "--Select--")
                ucError.ErrorMessage = "Current Port is Required";
            else if (ddlCurrentPort.SelectedValue == ddlNextPort.SelectedValue)
                ucError.ErrorMessage = "Current Port and Next Port Should not be same";
        }
        if (ddlVesselStatus.SelectedValue == "ATANCHOR" || ddlVesselStatus.SelectedValue == "ATSEA")
        {
        }

        return (!ucError.IsError);
    }

    private bool IsValidNoonReport()
    {
        if (General.GetNullableGuid(txtVoyageId.Text) == null)
            ucError.ErrorMessage = "Voyage is required.";

        if (ddlVesselStatus.SelectedValue == "")
            ucError.ErrorMessage = "Vessel's Status is Required";

        if (ViewState["VESSELID"].ToString() == "" || ViewState["VESSELID"].ToString() == "0")
            ucError.ErrorMessage = "Please switch to the particular vessel to add 'Noon Report'.";

        if (ChkECAyn.Checked == true)
        {
            if (General.GetNullableDateTime(txtECAEntryDate.Text) == null)
                ucError.ErrorMessage = "ECA Date is required.";

            if (General.GetNullableGuid(ddlECAOilType.SelectedValue) == null)
                ucError.ErrorMessage = "Fuel used in ECA is required.";
        }

        return (!ucError.IsError);
    }

    protected void WedSunReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("WEDSUNREPORTLIST"))
        {
            Response.Redirect("VesselPositionNoonReportList.aspx", false);
        }
    }

    protected void MenuNRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("ENGINE"))
        {
            if (Session["NOONREPORTID"] != null)
            {
                Response.Redirect("VesselPositionWedSunReportEngine.aspx", false);
            }
            else
            {
                ucError.ErrorMessage = "Please save the Report.";
                ucError.Visible = true;
            }
        }
    }
    
    private void BindData()
    {
        string noonreportid = Session["NOONREPORTID"].ToString();
        DataSet ds = PhoenixVesselPositionWedSunReport.EditWedSunReport(General.GetNullableGuid(noonreportid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            txtCurrentDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDNOONREPORTDATE"]);
            txtVoyageId.Text = dt.Rows[0]["FLDVOYAGEID"].ToString();
            txtVoyageName.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
            btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");

            txtETA.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETA"]);
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
            txtSeaHeight.Text =  dt.Rows[0]["FLDSEAHEIGHT"].ToString();
            ucWindDirection.SelectedDirection = dt.Rows[0]["FLDWINDDIRECTION"].ToString();
            ucCurrentDirection.SelectedDirection = dt.Rows[0]["FLDCURRENTDIRECTION"].ToString();
            ucSeaDirection.SelectedDirection = dt.Rows[0]["FLDSEADIRECTION"].ToString();
            if (General.GetNullableString(dt.Rows[0]["FLDBALLASTYN"].ToString()) != null)
                rbtnBallastLaden.SelectedValue = dt.Rows[0]["FLDBALLASTYN"].ToString();
            txtDraftF.Text = dt.Rows[0]["FLDDRAFTF"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();

            txtShipMeanTime.Text = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
            txtShipMeanTimeSymbol.Text = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";

            txtTimeOfETA.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDETA"]);

            ddlVesselStatus.SelectedValue = dt.Rows[0]["FLDVESSELSTATUS"].ToString();
            txtFullSpeed.Text = dt.Rows[0]["FLDFULLSPEED"].ToString();
            txtFSDistance.Text = dt.Rows[0]["FLDFULLSPEEDDISTANCE"].ToString();
            txtReducedSpeed.Text = dt.Rows[0]["FLDREDUCEDSPEED"].ToString();
            txtRSDistance.Text = dt.Rows[0]["FLDREDUCEDDISTANCE"].ToString();
            txtStopped.Text = dt.Rows[0]["FLDSTOPPED"].ToString();
            txtNoonSpeed.Text = dt.Rows[0]["FLDNOONSPEED"].ToString();
            txtVOCons.Text = dt.Rows[0]["FLDVOYAGEORDERCONS"].ToString();
            txtVOSpeed.Text = dt.Rows[0]["FLDVOYAGEORDERSPEED"].ToString();
            chkIceDeck.Checked = dt.Rows[0]["FLDICINGONDECK"].ToString().Equals("1") ? true : false;

            txtETB.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETB"]);
            txtTimeOfETB.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDETB"]);
            txtETC.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETC"]);
            txtTimeOfETC.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDETC"]);
            txtSwell.Text = dt.Rows[0]["FLDSWELL"].ToString();
            txtBerthLocation.Text = dt.Rows[0]["FLDBERTHLOCATION"].ToString();
            ChkUSWaters.Checked = dt.Rows[0]["FLDISUSWATERS"].ToString().Equals("1") ? true : false;
            ChkThroughHRA.Checked = dt.Rows[0]["FLDISPASSINGTHROUGHHRA"].ToString().Equals("1") ? true : false;

            ChkECAyn.Checked = dt.Rows[0]["FLDECAYN"].ToString().Equals("1") ? true : false;
            string oiltype = dt.Rows[0]["FLDECAOILTYPE"].ToString();
            if (oiltype != "")
                ddlECAOilType.SelectedValue = oiltype;
            txtECAEntryDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDECAENTERDATE"]);
            txtTimeofECAEntry.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDECAENTERDATE"]);

            BindCurrentPort();

            if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("INPORT"))
            {

                txtBerthLocation.CssClass = "input";
                txtBerthLocation.Enabled = true;

                if (dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString() != "")
                {
                    ddlCurrentPort.SelectedValue = dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString();
                    ddlCurrentPort.SelectedItem.Text = dt.Rows[0]["FLDCURRENTPORTNAME"].ToString();
                }

                if (dt.Rows[0]["FLDNEXTPORTCALLID"].ToString() != "")
                {
                    ddlNextPort.SelectedValue = dt.Rows[0]["FLDNEXTPORTCALLID"].ToString();
                    ddlNextPort.SelectedItem.Text = dt.Rows[0]["FLDNEXTPORTNAME"].ToString();
                }

                ddlCurrentPort.Enabled = true;
                ddlNextPort.Enabled = true;

                txtETA.Text = "";
                txtETA.CssClass = "readonlytextbox";
                txtETA.Enabled = false;
                txtTimeOfETA.Text = "";
                txtTimeOfETA.CssClass = "readonlytextbox";
                txtTimeOfETA.Enabled = false;

                txtETB.Text = "";
                txtETB.CssClass = "readonlytextbox";
                txtETB.Enabled = false;
                txtTimeOfETB.Text = "";
                txtTimeOfETB.CssClass = "readonlytextbox";
                txtTimeOfETB.Enabled = false;

                txtETC.CssClass = "input";
                txtETC.Enabled = true;
                txtTimeOfETC.CssClass = "input";
                txtTimeOfETC.Enabled = true;

                txtFullSpeed.Text = "";
                txtFullSpeed.CssClass = "readonlytextbox";
                txtFullSpeed.Enabled = false;

                txtReducedSpeed.Text = "";
                txtReducedSpeed.CssClass = "readonlytextbox";
                txtReducedSpeed.Enabled = false;

                txtStopped.Text = "";
                txtStopped.CssClass = "readonlytextbox";
                txtStopped.Enabled = false;

                txtDistanceObserved.Text = "";
                txtDistanceObserved.CssClass = "readonlytextbox";
                txtDistanceObserved.Enabled = false;

                txtLogSpeed.Text = "";
                txtLogSpeed.CssClass = "readonlytextbox";
                txtLogSpeed.Enabled = false;

                txtNoonSpeed.Text = "";
                txtNoonSpeed.CssClass = "readonlytextbox";
                txtNoonSpeed.Enabled = false;

                txtVOSpeed.Text = "";
                txtVOSpeed.CssClass = "readonlytextbox";
                txtVOSpeed.Enabled = false;

                txtCourse.Text = "";
                txtCourse.CssClass = "readonlytextbox";
                txtCourse.Enabled = false;

                txtDraftF.CssClass = "input";
                txtDraftF.Enabled = true;

                txtDraftA.CssClass = "input";
                txtDraftA.Enabled = true;

                txtDWT.Text = "";
                txtDWT.CssClass = "readonlytextbox";
                txtDWT.Enabled = false;

                txtSeaHeight.Text = "";
                txtSeaHeight.CssClass = "readonlytextbox";
                txtSeaHeight.Enabled = false;

                ucSeaDirection.SelectedDirection = "Dummy";
                ucSeaDirection.CssClass = "readonlytextbox";
                ucSeaDirection.Enabled = "false";

                txtSwell.Text = "";
                txtSwell.CssClass = "readonlytextbox";
                txtSwell.Enabled = false;

                txtCurrentSpeed.Text = "";
                txtCurrentSpeed.CssClass = "readonlytextbox";
                txtCurrentSpeed.Enabled = false;

                ucCurrentDirection.SelectedDirection = "Dummy";
                ucCurrentDirection.CssClass = "readonlytextbox";
                ucCurrentDirection.Enabled = "false";

                txtFSDistance.Text = "";
                txtFSDistance.CssClass = "readonlytextbox";
                txtFSDistance.Enabled = false;

                txtRSDistance.Text = "";
                txtRSDistance.CssClass = "readonlytextbox";
                txtRSDistance.Enabled = false;

                txtVOCons.Text = "";
                txtVOCons.CssClass = "readonlytextbox";
                txtVOCons.Enabled = false;
            }
            else if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATANCHOR"))
            {

                txtBerthLocation.CssClass = "input";
                txtBerthLocation.Enabled = true;

                if (dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString() != "")
                {
                    ddlCurrentPort.SelectedValue = dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString();
                    ddlCurrentPort.SelectedItem.Text = dt.Rows[0]["FLDCURRENTPORTNAME"].ToString();
                }

                if (dt.Rows[0]["FLDNEXTPORTCALLID"].ToString() != "")
                {
                    ddlNextPort.SelectedValue = dt.Rows[0]["FLDNEXTPORTCALLID"].ToString();
                    ddlNextPort.SelectedItem.Text = dt.Rows[0]["FLDNEXTPORTNAME"].ToString();
                }

                ddlCurrentPort.Enabled = true;
                ddlNextPort.Enabled = true;

                txtETA.Text = "";
                txtETA.CssClass = "readonlytextbox";
                txtETA.Enabled = false;
                txtTimeOfETA.Text = "";
                txtTimeOfETA.CssClass = "readonlytextbox";
                txtTimeOfETA.Enabled = false;

                txtETB.CssClass = "input";
                txtETB.Enabled = true;
                txtTimeOfETB.CssClass = "input";
                txtTimeOfETB.Enabled = true;

                txtETC.CssClass = "input";
                txtETC.Enabled = true;
                txtTimeOfETC.CssClass = "input";
                txtTimeOfETC.Enabled = true;

                txtFullSpeed.Text = "";
                txtFullSpeed.CssClass = "readonlytextbox";
                txtFullSpeed.Enabled = false;

                txtReducedSpeed.Text = "";
                txtReducedSpeed.CssClass = "readonlytextbox";
                txtReducedSpeed.Enabled = false;

                txtStopped.Text = "";
                txtStopped.CssClass = "readonlytextbox";
                txtStopped.Enabled = false;

                txtDistanceObserved.Text = "";
                txtDistanceObserved.CssClass = "readonlytextbox";
                txtDistanceObserved.Enabled = false;

                txtLogSpeed.Text = "";
                txtLogSpeed.CssClass = "readonlytextbox";
                txtLogSpeed.Enabled = false;

                txtNoonSpeed.Text = "";
                txtNoonSpeed.CssClass = "readonlytextbox";
                txtNoonSpeed.Enabled = false;

                txtVOSpeed.Text = "";
                txtVOSpeed.CssClass = "readonlytextbox";
                txtVOSpeed.Enabled = false;

                txtCourse.Text = "";
                txtCourse.CssClass = "readonlytextbox";
                txtCourse.Enabled = false;

                txtDraftF.CssClass = "input";
                txtDraftF.Enabled = true;

                txtDraftA.CssClass = "input";
                txtDraftA.Enabled = true;

                txtDWT.Text = "";
                txtDWT.CssClass = "readonlytextbox";
                txtDWT.Enabled = false;

                txtSeaHeight.CssClass = "input";
                txtSeaHeight.Enabled = true;

                ucSeaDirection.CssClass = "input";
                ucSeaDirection.Enabled = "true";

                txtSwell.Text = "";
                txtSwell.CssClass = "readonlytextbox";
                txtSwell.Enabled = false;

                txtCurrentSpeed.CssClass = "input";
                txtCurrentSpeed.Enabled = true;

                ucCurrentDirection.CssClass = "input";
                ucCurrentDirection.Enabled = "true";
            }
            else if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATSEA"))
            {
                txtBerthLocation.Text = "";
                txtBerthLocation.CssClass = "readonlytextbox";
                txtBerthLocation.Enabled = false;

                ddlCurrentPort.SelectedIndex = -1;

                if (dt.Rows[0]["FLDNEXTPORTCALLID"].ToString() != "")
                {
                    ddlNextPort.SelectedValue = dt.Rows[0]["FLDNEXTPORTCALLID"].ToString();
                    ddlNextPort.SelectedItem.Text = dt.Rows[0]["FLDNEXTPORTNAME"].ToString();
                }

                ddlCurrentPort.Enabled = false;
                ddlNextPort.Enabled = true;

                txtETA.CssClass = "input";
                txtETA.Enabled = true;
                txtTimeOfETA.CssClass = "input";
                txtTimeOfETA.Enabled = true;

                txtETB.Text = "";
                txtETB.CssClass = "readonlytextbox";
                txtETB.Enabled = false;
                txtTimeOfETB.Text = "";
                txtTimeOfETB.CssClass = "readonlytextbox";
                txtTimeOfETB.Enabled = false;

                txtETC.Text = "";
                txtETC.CssClass = "readonlytextbox";
                txtETC.Enabled = false;
                txtTimeOfETC.Text = "";
                txtTimeOfETC.CssClass = "readonlytextbox";
                txtTimeOfETC.Enabled = false;

                txtFullSpeed.CssClass = "input";
                txtFullSpeed.Enabled = true;

                txtReducedSpeed.CssClass = "input";
                txtReducedSpeed.Enabled = true;

                txtStopped.CssClass = "input";
                txtStopped.Enabled = true;

                txtDistanceObserved.CssClass = "input";
                txtDistanceObserved.Enabled = true;

                txtLogSpeed.CssClass = "input";
                txtLogSpeed.Enabled = true;

                txtNoonSpeed.CssClass = "input";
                txtNoonSpeed.Enabled = true;

                txtVOSpeed.CssClass = "input";
                txtVOSpeed.Enabled = true;

                txtCourse.CssClass = "input";
                txtCourse.Enabled = true;

                txtDraftF.Text = "";
                txtDraftF.CssClass = "readonlytextbox";
                txtDraftF.Enabled = false;

                txtDraftA.Text = "";
                txtDraftA.CssClass = "readonlytextbox";
                txtDraftA.Enabled = false;

                txtDWT.CssClass = "input";
                txtDWT.Enabled = true;

                txtSeaHeight.CssClass = "input";
                txtSeaHeight.Enabled = true;

                ucSeaDirection.CssClass = "input";
                ucSeaDirection.Enabled = "true";

                txtSwell.CssClass = "input";
                txtSwell.Enabled = true;

                txtCurrentSpeed.CssClass = "input";
                txtCurrentSpeed.Enabled = true;

                ucCurrentDirection.CssClass = "input";
                ucCurrentDirection.Enabled = "true";
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
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

    private int CalculateSlip(string enginedist, string distobserved)
    {
        if ((enginedist != "0") && (enginedist != "") && (enginedist != null))
        {
            if ((distobserved == "0") || (distobserved == "") || (distobserved == null))
                distobserved = "0";
            return Convert.ToInt32((((decimal.Parse(enginedist) - decimal.Parse(distobserved)) / decimal.Parse(enginedist)) * 100));
        }
        else
            return 0;
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        BindCurrentPort();
    }

    protected void VesselStatus_Changed(object sender, EventArgs e)
    {

        BindCurrentPort();

        if (ddlVesselStatus.SelectedValue == "INPORT")
        {

            txtBerthLocation.CssClass = "input";
            txtBerthLocation.Enabled = true;

            txtETA.Text = "";
            txtETA.CssClass = "readonlytextbox";
            txtETA.Enabled = false;
            txtTimeOfETA.Text = "";
            txtTimeOfETA.CssClass = "readonlytextbox";
            txtTimeOfETA.Enabled = false;

            txtETB.Text = "";
            txtETB.CssClass = "readonlytextbox";
            txtETB.Enabled = false;
            txtTimeOfETB.Text = "";
            txtTimeOfETB.CssClass = "readonlytextbox";
            txtTimeOfETB.Enabled = false;

            txtETC.CssClass = "input";
            txtETC.Enabled = true;
            txtTimeOfETC.CssClass = "input";
            txtTimeOfETC.Enabled = true;

            txtFullSpeed.Text = "";
            txtFullSpeed.CssClass = "readonlytextbox";
            txtFullSpeed.Enabled = false;

            txtReducedSpeed.Text = "";
            txtReducedSpeed.CssClass = "readonlytextbox";
            txtReducedSpeed.Enabled = false;

            txtStopped.Text = "";
            txtStopped.CssClass = "readonlytextbox";
            txtStopped.Enabled = false;

            txtDistanceObserved.Text = "";
            txtDistanceObserved.CssClass = "readonlytextbox";
            txtDistanceObserved.Enabled = false;

            txtLogSpeed.Text = "";
            txtLogSpeed.CssClass = "readonlytextbox";
            txtLogSpeed.Enabled = false;

            txtNoonSpeed.Text = "";
            txtNoonSpeed.CssClass = "readonlytextbox";
            txtNoonSpeed.Enabled = false;

            txtVOSpeed.Text = "";
            txtVOSpeed.CssClass = "readonlytextbox";
            txtVOSpeed.Enabled = false;

            txtCourse.Text = "";
            txtCourse.CssClass = "readonlytextbox";
            txtCourse.Enabled = false;

            txtDraftF.CssClass = "input";
            txtDraftF.Enabled = true;

            txtDraftA.CssClass = "input";
            txtDraftA.Enabled = true;

            txtDWT.Text = "";
            txtDWT.CssClass = "readonlytextbox";
            txtDWT.Enabled = false;

            txtSeaHeight.Text = "";
            txtSeaHeight.CssClass = "readonlytextbox";
            txtSeaHeight.Enabled = false;

            ucSeaDirection.SelectedDirection = "Dummy";
            ucSeaDirection.CssClass = "readonlytextbox";
            ucSeaDirection.Enabled = "false";

            txtSwell.Text = "";
            txtSwell.CssClass = "readonlytextbox";
            txtSwell.Enabled = false;

            txtCurrentSpeed.Text = "";
            txtCurrentSpeed.CssClass = "readonlytextbox";
            txtCurrentSpeed.Enabled = false;

            ucCurrentDirection.SelectedDirection = "Dummy";
            ucCurrentDirection.CssClass = "readonlytextbox";
            ucCurrentDirection.Enabled = "false";

            txtFSDistance.Text = "";
            txtFSDistance.CssClass = "readonlytextbox";
            txtFSDistance.Enabled = false;

            txtRSDistance.Text = "";
            txtRSDistance.CssClass = "readonlytextbox";
            txtRSDistance.Enabled = false;

            txtVOCons.Text = "";
            txtVOCons.CssClass = "readonlytextbox";
            txtVOCons.Enabled = false;
        }
        else if (ddlVesselStatus.SelectedValue == "ATANCHOR")
        {

            txtBerthLocation.CssClass = "input";
            txtBerthLocation.Enabled = true;

            txtETA.Text = "";
            txtETA.CssClass = "readonlytextbox";
            txtETA.Enabled = false;
            txtTimeOfETA.Text = "";
            txtTimeOfETA.CssClass = "readonlytextbox";
            txtTimeOfETA.Enabled = false;

            txtETB.CssClass = "input";
            txtETB.Enabled = true;
            txtTimeOfETB.CssClass = "input";
            txtTimeOfETB.Enabled = true;

            txtETC.CssClass = "input";
            txtETC.Enabled = true;
            txtTimeOfETC.CssClass = "input";
            txtTimeOfETC.Enabled = true;

            txtFullSpeed.Text = "";
            txtFullSpeed.CssClass = "readonlytextbox";
            txtFullSpeed.Enabled = false;

            txtReducedSpeed.Text = "";
            txtReducedSpeed.CssClass = "readonlytextbox";
            txtReducedSpeed.Enabled = false;

            txtStopped.Text = "";
            txtStopped.CssClass = "readonlytextbox";
            txtStopped.Enabled = false;

            txtDistanceObserved.Text = "";
            txtDistanceObserved.CssClass = "readonlytextbox";
            txtDistanceObserved.Enabled = false;

            txtLogSpeed.Text = "";
            txtLogSpeed.CssClass = "readonlytextbox";
            txtLogSpeed.Enabled = false;

            txtNoonSpeed.Text = "";
            txtNoonSpeed.CssClass = "readonlytextbox";
            txtNoonSpeed.Enabled = false;

            txtVOSpeed.Text = "";
            txtVOSpeed.CssClass = "readonlytextbox";
            txtVOSpeed.Enabled = false;

            txtCourse.Text = "";
            txtCourse.CssClass = "readonlytextbox";
            txtCourse.Enabled = false;

            txtDraftF.CssClass = "input";
            txtDraftF.Enabled = true;

            txtDraftA.CssClass = "input";
            txtDraftA.Enabled = true;

            txtDWT.Text = "";
            txtDWT.CssClass = "readonlytextbox";
            txtDWT.Enabled = false;

            txtSeaHeight.CssClass = "input";
            txtSeaHeight.Enabled = true;

            ucSeaDirection.CssClass = "input";
            ucSeaDirection.Enabled = "true";

            txtSwell.Text = "";
            txtSwell.CssClass = "readonlytextbox";
            txtSwell.Enabled = false;

            txtCurrentSpeed.CssClass = "input";
            txtCurrentSpeed.Enabled = true;

            ucCurrentDirection.CssClass = "input";
            ucCurrentDirection.Enabled = "true";
        }
        else if (ddlVesselStatus.SelectedValue == "ATSEA")
        {
            txtBerthLocation.Text = "";
            txtBerthLocation.CssClass = "readonlytextbox";
            txtBerthLocation.Enabled = false;

            txtETA.CssClass = "input";
            txtETA.Enabled = true;
            txtTimeOfETA.CssClass = "input";
            txtTimeOfETA.Enabled = true;

            txtETB.Text = "";
            txtETB.CssClass = "readonlytextbox";
            txtETB.Enabled = false;
            txtTimeOfETB.Text = "";
            txtTimeOfETB.CssClass = "readonlytextbox";
            txtTimeOfETB.Enabled = false;

            txtETC.Text = "";
            txtETC.CssClass = "readonlytextbox";
            txtETC.Enabled = false;
            txtTimeOfETC.Text = "";
            txtTimeOfETC.CssClass = "readonlytextbox";
            txtTimeOfETC.Enabled = false;

            txtFullSpeed.CssClass = "input";
            txtFullSpeed.Enabled = true;

            txtReducedSpeed.CssClass = "input";
            txtReducedSpeed.Enabled = true;

            txtStopped.CssClass = "input";
            txtStopped.Enabled = true;

            txtDistanceObserved.CssClass = "input";
            txtDistanceObserved.Enabled = true;

            txtLogSpeed.CssClass = "input";
            txtLogSpeed.Enabled = true;

            txtNoonSpeed.CssClass = "input";
            txtNoonSpeed.Enabled = true;

            txtVOSpeed.CssClass = "input";
            txtVOSpeed.Enabled = true;

            txtCourse.CssClass = "input";
            txtCourse.Enabled = true;

            txtDraftF.Text = "";
            txtDraftF.CssClass = "readonlytextbox";
            txtDraftF.Enabled = false;

            txtDraftA.Text = "";
            txtDraftA.CssClass = "readonlytextbox";
            txtDraftA.Enabled = false;

            txtDWT.CssClass = "input";
            txtDWT.Enabled = true;

            txtSeaHeight.CssClass = "input";
            txtSeaHeight.Enabled = true;

            ucSeaDirection.CssClass = "input";
            ucSeaDirection.Enabled = "true";

            txtSwell.CssClass = "input";
            txtSwell.Enabled = true;

            txtCurrentSpeed.CssClass = "input";
            txtCurrentSpeed.Enabled = true;

            ucCurrentDirection.CssClass = "input";
            ucCurrentDirection.Enabled = "true";
        }
    }

    protected void ddlNextPort_Changed(object sender, EventArgs e)
    {
        if (ddlVesselStatus.SelectedValue == "INPORT")
        {
            if (ddlNextPort.SelectedValue == "--Select--")
            {
                txtETA.Text = "";
                txtETA.CssClass = "readonlytextbox";
                txtETA.Enabled = false;
                txtTimeOfETA.Text = "";
                txtTimeOfETA.CssClass = "readonlytextbox";
                txtTimeOfETA.Enabled = false;
            }
            else
            {
                txtETA.CssClass = "input";
                txtETA.Enabled = true;
                txtTimeOfETA.CssClass = "input";
                txtTimeOfETA.Enabled = true;
            }
        }
    }

    // bug id: 9826 - Noon Report Field range configuration..

    private void SetFieldRange()
    {
        DataSet ds = PhoenixRegistersNoonReportRangeConfig.ListNoonReportRangeConfig(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            1);

        if (ds.Tables.Count > 0)
        {
            decimal? minvalue = null;
            decimal? maxvalue = null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                minvalue = General.GetNullableDecimal(dr["FLDMINVALUE"].ToString());
                maxvalue = General.GetNullableDecimal(dr["FLDMAXVALUE"].ToString());

                switch (dr["FLDCOLUMNNAME"].ToString())
                {
                    case "FLDDISTANCEOBSERVED":
                        {
                            if (General.GetNullableDecimal(txtDistanceObserved.Text) < minvalue || General.GetNullableDecimal(txtDistanceObserved.Text) > maxvalue)
                                txtDistanceObserved.CssClass = "maxhighlight";
                            else
                                txtDistanceObserved.CssClass = "input";
                            break;
                        }
                    case "FLDLOGSPEED":
                        {
                            if (General.GetNullableDecimal(txtLogSpeed.Text) < minvalue || General.GetNullableDecimal(txtLogSpeed.Text) > maxvalue)
                                txtLogSpeed.CssClass = "maxhighlight";
                            else
                                txtLogSpeed.CssClass = "input";
                            break;
                        }
                    case "FLDCOURSE":
                        {
                            if (General.GetNullableDecimal(txtCourse.Text) < minvalue || General.GetNullableDecimal(txtCourse.Text) > maxvalue)
                                txtCourse.CssClass = "maxhighlight";
                            else
                                txtCourse.CssClass = "input";
                            break;
                        }
                    case "FLDDRAFTF":
                        {
                            if (General.GetNullableDecimal(txtDraftF.Text) < minvalue || General.GetNullableDecimal(txtDraftF.Text) > maxvalue)
                                txtDraftF.CssClass = "maxhighlight";
                            else
                                txtDraftF.CssClass = "input";
                            break;
                        }
                    case "FLDDRAFT":
                        {
                            if (General.GetNullableDecimal(txtDraftA.Text) < minvalue || General.GetNullableDecimal(txtDraftA.Text) > maxvalue)
                                txtDraftA.CssClass = "maxhighlight";
                            else
                                txtDraftA.CssClass = "input";
                            break;
                        }
                    case "FLDWINDFORCE":
                        {
                            if (General.GetNullableDecimal(txtWindForce.Text) < minvalue || General.GetNullableDecimal(txtWindForce.Text) > maxvalue)
                                txtWindForce.CssClass = "maxhighlight";
                            else
                                txtWindForce.CssClass = "input";
                            break;
                        }
                    case "FLDSEAHEIGHT":
                        {
                            if (General.GetNullableDecimal(txtSeaHeight.Text) < minvalue || General.GetNullableDecimal(txtSeaHeight.Text) > maxvalue)
                                txtSeaHeight.CssClass = "maxhighlight";
                            else
                                txtSeaHeight.CssClass = "input";
                            break;
                        }
                    case "FLDCURRENTSPEED":
                        {
                            if (General.GetNullableDecimal(txtCurrentSpeed.Text) < minvalue || General.GetNullableDecimal(txtCurrentSpeed.Text) > maxvalue)
                                txtCurrentSpeed.CssClass = "maxhighlight";
                            else
                                txtCurrentSpeed.CssClass = "input";
                            break;
                        }
                    case "FLDAIRTEMP":
                        {
                            if (General.GetNullableDecimal(txtAirTemp.Text) < minvalue || General.GetNullableDecimal(txtAirTemp.Text) > maxvalue)
                                txtAirTemp.CssClass = "maxhighlight";
                            else
                                txtAirTemp.CssClass = "input";
                            break;
                        }
                    case "FLDDWT":
                        {
                            if (General.GetNullableDecimal(txtDWT.Text) < minvalue || General.GetNullableDecimal(txtDWT.Text) > maxvalue)
                                txtDWT.CssClass = "maxhighlight";
                            else
                                txtDWT.CssClass = "input";
                            break;
                        }
                }
            }
        }
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
            txtTimeofECAEntry.Text = "";
            ddlECAOilType.SelectedIndex = -1;
        }
    }

    private void ViewMenu()
    {
        PhoenixToolbar toolbarWedSunReporttap = new PhoenixToolbar();

        toolbarWedSunReporttap.AddButton("Save", "SAVE");
        if (Request.QueryString["mode"] != null)
            toolbarWedSunReporttap.AddButton("Copy", "COPY");
        
        MenuNewSaveTabStrip.AccessRights = this.ViewState;
        MenuNewSaveTabStrip.MenuList = toolbarWedSunReporttap.Show();
    }
}
