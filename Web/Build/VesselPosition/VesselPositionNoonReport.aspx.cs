using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselPositionNoonReport : PhoenixBasePage
{
    DataSet dsPDF = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsubtap = new PhoenixToolbar();
            toolbarsubtap.AddButton("List", "NOONREPORTLIST", ToolBarDirection.Left);
            toolbarsubtap.AddButton("Deck Dept", "DECK", ToolBarDirection.Left);
            toolbarsubtap.AddButton("Engine Dept", "ENGINE", ToolBarDirection.Left);
            
            MenuNRSubTab.AccessRights = this.ViewState;
            MenuNRSubTab.MenuList = toolbarsubtap.Show();
            MenuNRSubTab.SelectedMenuIndex = 1;

            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            txtVoyageId.Attributes.Add("style", "display:none;");
            

            if (!IsPostBack)
            {
                ViewState["REPORTSTATUS"] = "";
                ViewState["OFFPORTLIMITYN"] = "";
                ViewState["REVIEWDYN"] = "";
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

                DataSet Ds = PhoenixRegistersOilType.ListOilType(1, 1); // For only fueloil
                ddlECAOilType.DataSource = Ds;
                ddlECAOilType.DataBind();
                ddlECAOilType.Items.Insert(0, "--Select--");

                if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToString().Equals("NEW"))
                {
                    Session["NOONREPORTID"] = null;

                    DataSet dsReportDate = PhoenixVesselPositionNoonReport.GetNoonReportDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (dsReportDate.Tables.Count > 0)
                        txtCurrentDate.Text = dsReportDate.Tables[0].Rows[0]["FLDREPORTDATE"].ToString();

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

                                chkOffPortLimits.Checked = dr["FLDOFFPORTLIMITYN"].ToString().Equals("1") ? true : false;
                                ViewState["OFFPORTLIMITYN"] = dr["FLDOFFPORTLIMITYN"].ToString();

                                    if (General.GetNullableString(dr["FLDSHIPMEANTIME"].ToString()) != null)
                                {
                                    txtShipMeanTime.Text = dr["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
                                    txtShipMeanTimeSymbol.SelectedValue = dr["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";
                                }

                                txtEntryDate.Text = dr["FLDUSWATERENTRYDATE"].ToString();

                                if (General.GetNullableDateTime(dr["FLDUSWATERENTRYDATE"].ToString()) != null)
                                    cmdPickListEntry.OnClientClick = "";

                                txtShipCalendarIdEntry.Text = dr["FLDSHIPCALENDEERENTRYID"].ToString();
                                txtEntryHour.Text = dr["FLDUSWATERENTRYHOUR"].ToString();

                                if (General.GetNullableString(dr["FLDLASTECAENTERDATE"].ToString()) != null)
                                {
                                    txtECAEntryDate.Enabled = false;
                                    txtTimeofECAEntry.Enabled = false;

                                    txtECAEntryDate.CssClass = "readonlytextbox";
                                    txtTimeofECAEntry.CssClass = "readonlytextbox";

                                    txtExitECADate.Enabled = true;
                                    txtExitECATime.Enabled = true;

                                    txtExitECADate.CssClass = "input";
                                    txtExitECATime.CssClass = "input";

                                    txtECAEntryDate.Text = General.GetDateTimeToString(dr["FLDLASTECAENTERDATE"].ToString());
                                    if (General.GetNullableDateTime(dr["FLDLASTECAENTERDATE"].ToString()) != null)
                                        txtTimeofECAEntry.SelectedDate = Convert.ToDateTime(dr["FLDLASTECAENTERDATE"]);

                                }
                                else
                                {
                                    txtECAEntryDate.Enabled = true;
                                    txtTimeofECAEntry.Enabled = true;

                                    txtECAEntryDate.CssClass = "input";
                                    txtTimeofECAEntry.CssClass = "input";

                                    txtExitECADate.Enabled = false;
                                    txtExitECATime.Enabled = false;

                                    txtExitECADate.CssClass = "readonlytextbox";
                                    txtExitECATime.CssClass = "readonlytextbox";

                                }
                                if (General.GetNullableGuid(dr["OILTYPE"].ToString()) != null)
                                {
                                    ddlECAOilType.SelectedValue = dr["OILTYPE"].ToString();
                                }
                            }
                        }
                    }

                    ddlCurrentPort.Items.Insert(0, "--Select--");
                    ddlNextPort.Items.Insert(0, "--Select--");

                    DataSet dsCurrentvoyage = PhoenixVesselPositionVoyageData.CurrentVoyageData(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    if (dsCurrentvoyage.Tables.Count > 0)
                    {
                        if (dsCurrentvoyage.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = dsCurrentvoyage.Tables[0].Rows[0];

                            txtVoyageName.Text = dr["FLDVOYAGENO"].ToString();
                            txtVoyageId.Text = dr["FLDVOYAGEID"].ToString();
                        }
                    }
                }
                else
                {
                    BindData();
                    SetFieldRange();
                }
                
  
                if (txtCurrentDate.Text == string.Empty || General.GetNullableDateTime(txtCurrentDate.Text) == null)
                {
                    txtCurrentDate.Enabled = true;
                    txtCurrentDate.CssClass = "input";
                }
                else
                {
                    txtCurrentDate.Enabled = false;
                    txtCurrentDate.CssClass = "readonlytextbox";
                }
            }
            ViewMenu();
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
                    if(dt.Rows[0]["FLDCURRENTPORTCALLID"].ToString() != "")
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
        else if (ddlVesselStatus.SelectedValue == "ATSEA" || ddlVesselStatus.SelectedValue == "DRIFTING")
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
            Guid? noonreportid  = null;

            if (Session["NOONREPORTID"] != null)
                noonreportid = General.GetNullableGuid(Session["NOONREPORTID"].ToString());
            DataSet ds;
            string CurrentDate = string.Format("{0:ddd}", System.DateTime.Now);

            ds = PhoenixVesselPositionNoonReport.ResetNoonReport(int.Parse(ViewState["VESSELID"].ToString()), noonreportid);
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                ViewState["OLDNOONREPORTID"] = dt.Rows[0]["FLDNOONREPORTID"].ToString();

                //txtCurrentDate.Text = General.GetDateTimeToString(System.DateTime.Now);

                txtVoyageId.Text = dt.Rows[0]["FLDVOYAGEID"].ToString();
                txtVoyageName.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
                btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");

                txtETA.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETA"]);
                if (General.GetNullableDateTime(dt.Rows[0]["FLDETA"].ToString()) != null)
                    txtTimeOfETA.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDETA"]);

                txtETB.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETB"]);
                if (General.GetNullableDateTime(dt.Rows[0]["FLDETB"].ToString()) != null)
                    txtTimeOfETB.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDETB"]);


                txtETC.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETC"]);
                if (General.GetNullableDateTime(dt.Rows[0]["FLDETC"].ToString()) != null)
                    txtTimeOfETC.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDETC"]);

                ucLatitude.TextDegree = dt.Rows[0]["FLDLAT1"].ToString();
                ucLatitude.TextMinute = dt.Rows[0]["FLDLAT2"].ToString();
                ucLatitude.TextSecond = dt.Rows[0]["FLDLAT3"].ToString();
                ucLatitude.TextDirection = dt.Rows[0]["FLDLATDIRECTION"].ToString();

                ucLongitude.TextDegree = dt.Rows[0]["FLDLONG1"].ToString();
                ucLongitude.TextMinute = dt.Rows[0]["FLDLONG2"].ToString();
                ucLongitude.TextSecond = dt.Rows[0]["FLDLONG3"].ToString();
                ucLongitude.TextDirection = dt.Rows[0]["FLDLONGDIRECTION"].ToString();

                txtCourse.Text = dt.Rows[0]["FLDCOURSE"].ToString();
                //txtDistanceObserved.Text = dt.Rows[0]["FLDDISTANCEOBSERVED"].ToString();
                txtAirTemp.Text = dt.Rows[0]["FLDAIRTEMP"].ToString();
                txtDWT.Text = dt.Rows[0]["FLDDWT"].ToString();
                txtDraftA.Text = dt.Rows[0]["FLDDRAFT"].ToString();
               // txtLogSpeed.Text = dt.Rows[0]["FLDLOGSPEED"].ToString();
                txtWindForce.Text = dt.Rows[0]["FLDWINDFORCE"].ToString();
                txtCurrentSpeed.Text = dt.Rows[0]["FLDCURRENTSPEED"].ToString();
                txtSeaHeight.Text = dt.Rows[0]["FLDSEAHEIGHT"].ToString();
                ucWindDirection.SelectedDirection = dt.Rows[0]["FLDWINDDIRECTION"].ToString();
                ucCurrentDirection.SelectedDirection = dt.Rows[0]["FLDCURRENTDIRECTION"].ToString();
                ucSeaDirection.SelectedDirection = dt.Rows[0]["FLDSEADIRECTION"].ToString();
                if (General.GetNullableString(dt.Rows[0]["FLDBALLASTYN"].ToString()) != null)
                    rbtnBallastLaden.SelectedValue = dt.Rows[0]["FLDBALLASTYN"].ToString();
                txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
                txtDraftF.Text = dt.Rows[0]["FLDDRAFTF"].ToString();
                txtSwell.Text = dt.Rows[0]["FLDSWELL"].ToString();
                txtBerthLocation.Text = dt.Rows[0]["FLDBERTHLOCATION"].ToString();

                txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();

                txtShipMeanTime.Text = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
                txtShipMeanTimeSymbol.SelectedValue = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";

                ddlVesselStatus.SelectedValue = dt.Rows[0]["FLDVESSELSTATUS"].ToString();
               // txtFullSpeed.Text = dt.Rows[0]["FLDFULLSPEED"].ToString();
               // txtFSDistance.Text = dt.Rows[0]["FLDFULLSPEEDDISTANCE"].ToString();
               // txtReducedSpeed.Text = dt.Rows[0]["FLDREDUCEDSPEED"].ToString();
                //txtRSDistance.Text = dt.Rows[0]["FLDREDUCEDDISTANCE"].ToString();
               // txtStopped.Text = dt.Rows[0]["FLDSTOPPED"].ToString();
               // txtNoonSpeed.Text = dt.Rows[0]["FLDNOONSPEED"].ToString();
                txtVOCons.Text = dt.Rows[0]["FLDVOYAGEORDERCONS"].ToString();
                txtVOSpeed.Text = dt.Rows[0]["FLDVOYAGEORDERSPEED"].ToString();
                chkIceDeck.Checked = dt.Rows[0]["FLDICINGONDECK"].ToString().Equals("1") ? true : false;
                ChkUSWaters.Checked = dt.Rows[0]["FLDISUSWATERS"].ToString().Equals("1") ? true : false;
                ChkThroughHRA.Checked = dt.Rows[0]["FLDISPASSINGTHROUGHHRA"].ToString().Equals("1") ? true : false;

                ChkECAyn.Checked = dt.Rows[0]["FLDECAYN"].ToString().Equals("1") ? true : false;
                string oiltype = dt.Rows[0]["FLDECAOILTYPE"].ToString();
                if (oiltype != "")
                    ddlECAOilType.SelectedValue = oiltype;
                txtECAEntryDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDECAENTERDATE"]);
                if (General.GetNullableDateTime(dt.Rows[0]["FLDECAENTERDATE"].ToString()) != null)
                    txtTimeofECAEntry.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDECAENTERDATE"]);

                txtDistancetogo.Text = dt.Rows[0]["FLDDISTANCETOGO"].ToString();
                txtVoDOCons.Text = dt.Rows[0]["FLDVOYAGEORDERDOCONS"].ToString();

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
                    txtTimeOfETA.SelectedDate = null;
                    txtTimeOfETA.CssClass = "readonlytextbox";
                    txtTimeOfETA.Enabled = false;

                    txtNoonSpeed.Text = "";
                    txtNoonSpeed.CssClass = "readonlytextbox";
                    txtNoonSpeed.Enabled = false;

                    txtVoyageAverageSpeed.Text = "";
                    txtVoyageAverageSpeed.CssClass = "readonlytextbox";
                    txtVoyageAverageSpeed.Enabled = false;

                    txtEMLogCounter.Text = "";
                txtEMLogCounter.CssClass = "readonlytextbox";
                txtEMLogCounter.Enabled = false;

                    txtETB.Text = "";
                    txtETB.CssClass = "readonlytextbox";
                    txtETB.Enabled = false;
                    txtTimeOfETB.SelectedDate = null;
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

                    //txtDistanceObserved.Text = "";
                    //txtDistanceObserved.CssClass = "readonlytextbox";
                    //txtDistanceObserved.Enabled = false;

                    txtLogSpeed.Text = "";
                    if (txtLogSpeed.CssClass != "maxhighlight")
                        txtLogSpeed.CssClass = "readonlytextbox";
                    txtLogSpeed.Enabled = false;

                    //txtVOSpeed.Text = "";
                    //txtVOSpeed.CssClass = "readonlytextbox";
                    //txtVOSpeed.Enabled = false;

                    //txtVoDOCons.Text = "";
                    //txtVoDOCons.CssClass = "readonlytextbox";
                    //txtVoDOCons.Enabled = false;

                    txtCourse.Text = "";
                    txtCourse.CssClass = "readonlytextbox";
                    txtCourse.Enabled = false;

                    txtDraftF.CssClass = "input";
                    txtDraftF.Enabled = true;

                    txtDraftA.CssClass = "input";
                    txtDraftA.Enabled = true;

                    //txtDWT.Text = "";
                    //txtDWT.CssClass = "readonlytextbox";
                    //txtDWT.Enabled = false;

                    txtSeaHeight.Text = "";
                    txtSeaHeight.CssClass = "readonlytextbox";
                    txtSeaHeight.Enabled = false;

                    ucSeaDirection.SelectedDirection = "Dummy";
                    ucSeaDirection.CssClass = "readonlytextbox";
                    ucSeaDirection.Enabled = "false";

                    //txtSwell.Text = "";
                    //txtSwell.CssClass = "readonlytextbox";
                    //txtSwell.Enabled = false;

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

                    //txtVOCons.Text = "";
                    //txtVOCons.CssClass = "readonlytextbox";
                    //txtVOCons.Enabled = false;
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

                    txtTimeOfETA.SelectedDate = null;
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

                    //txtDistanceObserved.Text = "";
                    //txtDistanceObserved.CssClass = "readonlytextbox";
                    //txtDistanceObserved.Enabled = false;

                    txtLogSpeed.Text = "";
                    if (txtLogSpeed.CssClass != "maxhighlight")
                        txtLogSpeed.CssClass = "readonlytextbox";
                    txtLogSpeed.Enabled = false;

                    txtNoonSpeed.Text = "";
                    txtNoonSpeed.CssClass = "readonlytextbox";
                    txtNoonSpeed.Enabled = false;

                    //txtVOSpeed.Text = "";
                    //txtVOSpeed.CssClass = "readonlytextbox";
                    //txtVOSpeed.Enabled = false;

                    //txtVoDOCons.Text = "";
                    //txtVoDOCons.CssClass = "readonlytextbox";
                    //txtVoDOCons.Enabled = false;

                    txtCourse.Text = "";
                    txtCourse.CssClass = "readonlytextbox";
                    txtCourse.Enabled = false;

                    txtDraftF.CssClass = "input";
                    txtDraftF.Enabled = true;

                    txtDraftA.CssClass = "input";
                    txtDraftA.Enabled = true;

                    //txtDWT.Text = "";
                    //txtDWT.CssClass = "readonlytextbox";
                    //txtDWT.Enabled = false;

                    txtSeaHeight.CssClass = "input";
                    txtSeaHeight.Enabled = true;

                    ucSeaDirection.CssClass = "input";
                    ucSeaDirection.Enabled = "true";

                    //txtSwell.Text = "";
                    //txtSwell.CssClass = "readonlytextbox";
                    //txtSwell.Enabled = false;

                    txtCurrentSpeed.CssClass = "input";
                    txtCurrentSpeed.Enabled = true;

                    ucCurrentDirection.CssClass = "input";
                    ucCurrentDirection.Enabled = "true";
                }
                else if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATSEA") || dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("DRIFTING"))
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
                    txtTimeOfETB.SelectedDate = null;
                    txtTimeOfETB.CssClass = "readonlytextbox";
                    txtTimeOfETB.Enabled = false;

                    txtETC.Text = "";
                    txtETC.CssClass = "readonlytextbox";
                    txtETC.Enabled = false;
                    txtTimeOfETC.SelectedDate = null;
                    txtTimeOfETC.CssClass = "readonlytextbox";
                    txtTimeOfETC.Enabled = false;

                    txtFullSpeed.CssClass = "input";
                    txtFullSpeed.Enabled = true;

                    txtReducedSpeed.CssClass = "input";
                    txtReducedSpeed.Enabled = true;

                    txtStopped.CssClass = "input";
                    txtStopped.Enabled = true;

                    //txtDistanceObserved.CssClass = "input";
                    //txtDistanceObserved.Enabled = true;
                    if (txtLogSpeed.CssClass != "maxhighlight")
                        txtLogSpeed.CssClass = "input";
                    txtLogSpeed.Enabled = true;

                    txtNoonSpeed.CssClass = "input";
                    txtNoonSpeed.Enabled = true;

                    txtVOSpeed.CssClass = "input";
                    txtVOSpeed.Enabled = true;

                    txtVoDOCons.CssClass = "input";
                    txtVoDOCons.Enabled = true;

                    txtCourse.CssClass = "input";
                    txtCourse.Enabled = true;

                    //txtDraftF.Text = "";
                    txtDraftF.CssClass = "input";
                    txtDraftF.Enabled = true;

                    //txtDraftA.Text = "";
                    txtDraftA.CssClass = "input";
                    txtDraftA.Enabled = true;

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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("COPY"))
            {
                Reset();
            }

            if (CommandName.ToUpper().Equals("SAVE") || CommandName.ToUpper().Equals("SENDTOOFFICE"))
            {
                     
                if (CommandName.ToUpper() == "SENDTOOFFICE")
                {
                    if (Session["NOONREPORTID"] == null)
                    {
                        AddNoonReport(1);
                    }
                    else
                    {
                        UpdateNoonReport(1);
                    }
                   
                }
                else
                {
                    if (Session["NOONREPORTID"] == null)
                    {
                        AddNoonReport(0);
                    }
                    else
                    {
                        UpdateNoonReport(0);
                    }
                }
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {
                ConvertToPdf(PrepareHtmlDoc());
            }
            if (CommandName.ToUpper().Equals("REVIEW"))
            {
                String scriptpopup = String.Format(
                 "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/VesselPosition/VesselPositionNoonReportReview.aspx?VesselId="
             + ViewState["VESSELID"].ToString() + "&NoonReportID=" + Session["NOONREPORTID"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {            
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }       
    }

    private void UpdateNoonReport(int confirm)
    {
        try
        {
            string timeofeta = txtTimeOfETA.SelectedTime != null ? txtTimeOfETA.SelectedTime.Value.ToString() : "";
            string timeofetb = txtTimeOfETB.SelectedTime != null ? txtTimeOfETB.SelectedTime.Value.ToString() : "";
            string timeofetc = txtTimeOfETC.SelectedTime != null ? txtTimeOfETC.SelectedTime.Value.ToString() : "";
            string timeofeca = txtTimeofECAEntry.SelectedTime != null ? txtTimeofECAEntry.SelectedTime.Value.ToString() : "";
            string timeofFuelCO = txtFuelCOTime.SelectedTime != null ? txtFuelCOTime.SelectedTime.Value.ToString() : "";
            string timeofexiteta = txtExitECATime.SelectedTime != null ? txtExitECATime.SelectedTime.Value.ToString() : "";

            DateTime CurrentDate = DateTime.Parse(txtCurrentDate.Text);

            PhoenixVesselPositionNoonReport.UpdateNoonReport(
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
                 (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text), General.GetNullableDecimal(txtDraftF.Text),
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
                 , General.GetNullableDateTime(txtECAEntryDate.Text + " " + timeofeca)
                 , General.GetNullableDecimal(txtDistancetogo.Text)
                 , General.GetNullableDecimal(txtVoyageAverageSpeed.Text)
                 , General.GetNullableInteger(ChkOilMajorCargoOnboard.Checked == true ? "1" : "0")
                 , General.GetNullableString(ddlOilMajor.SelectedValue)
                 , confirm
                 , ucECAlatitude.TextDegree, ucECALongitude.TextDegree, ucECAlatitude.TextMinute, ucECALongitude.TextMinute, ucECAlatitude.TextSecond, ucECALongitude.TextSecond, ucECAlatitude.TextDirection, ucECALongitude.TextDirection
                 , General.GetNullableDateTime(ucFuelCOTime.Text + " " + timeofFuelCO)
                 , General.GetNullableDecimal(txtEMLogCounter.Text)
                 , chkCounterDefective.Checked == true ? 1 : 0
                 , General.GetNullableDecimal(txtEMLogDistance.Text)
                 , General.GetNullableDecimal(txtVoDOCons.Text)
                 , General.GetNullableDateTime(txtExitECADate.Text + " " + timeofexiteta)
                 , chkOffPortLimits.Checked == true ? 1 : 0
                 , General.GetNullableDateTime(txtEntryDate.Text)
                 , General.GetNullableDecimal(txtEntryHour.Text)
                 , General.GetNullableDateTime(txtExitDate.Text)
                 , General.GetNullableDecimal(txtExitHour.Text)
                 , General.GetNullableInteger(txtShipCalendarIdEntry.Text)
                 , General.GetNullableInteger(txtShipCalendarIdExit.Text)
                 );
            if (confirm == 0)
            {
                ucStatus.Text = "Noon Report updated.";
            }
            else
            {
                ucStatus.Text = "Noon Report Sent to office.";
            }

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

            if (Session["NOONREPORTID"] != null)
            {
                BindData();
                SetFieldRange();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void AddNoonReport(int confirm)
    {
        try
        {
            Guid? noonreportid = null;

            //if (!IsValidNoonReport())
            //{
            //    ucError.Visible = true;
            //    return;
            //}

            string timeofeta = txtTimeOfETA.SelectedTime != null ? txtTimeOfETA.SelectedTime.Value.ToString() : "";
            string timeofetb = txtTimeOfETB.SelectedTime != null ? txtTimeOfETB.SelectedTime.Value.ToString() : "";
            string timeofetc = txtTimeOfETC.SelectedTime != null ? txtTimeOfETC.SelectedTime.Value.ToString() : "";
            string timeofeca = txtTimeofECAEntry.SelectedTime != null ? txtTimeofECAEntry.SelectedTime.Value.ToString() : "";
            string timeofFuelCO = txtFuelCOTime.SelectedTime != null ? txtFuelCOTime.SelectedTime.Value.ToString() : "";
            string timeofexiteta = txtExitECATime.SelectedTime != null ? txtExitECATime.SelectedTime.Value.ToString() : "";

            //DateTime CurrentDate = DateTime.Parse(txtCurrentDate.Text);

            PhoenixVesselPositionNoonReport.InsertNoonReport(
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
                     (txtShipMeanTimeSymbol.SelectedValue + txtShipMeanTime.Text), General.GetNullableDecimal(txtDraftF.Text),
                     General.GetNullableDateTime(txtCurrentDate.Text),
                     General.GetNullableDateTime(txtETA.Text + " " + timeofeta),
                     General.GetNullableGuid(ddlCurrentPort.SelectedValue),
                     General.GetNullableGuid(ddlNextPort.SelectedValue),
                     General.GetNullableString(ddlVesselStatus.SelectedValue),
                     General.GetNullableDecimal(txtFullSpeed.Text), General.GetNullableDecimal(txtFSDistance.Text),
                     General.GetNullableDecimal(txtReducedSpeed.Text), General.GetNullableDecimal(txtRSDistance.Text),
                     General.GetNullableDecimal(txtStopped.Text), General.GetNullableDecimal(txtNoonSpeed.Text),
                     General.GetNullableDecimal(txtVOSpeed.Text), General.GetNullableDecimal(txtVOCons.Text),
                     chkIceDeck.Checked == true ? 1 : 0,
                     0
                     , ref noonreportid
                     , General.GetNullableString(txtBerthLocation.Text)
                     , General.GetNullableDateTime(txtETB.Text + " " + timeofetb)
                     , General.GetNullableDateTime(txtETC.Text + " " + timeofetc)
                     , General.GetNullableDecimal(txtSwell.Text)
                     , ChkUSWaters.Checked == true ? 1 : 0
                     , ChkThroughHRA.Checked == true ? 1 : 0
                     , ChkECAyn.Checked == true ? 1 : 0
                     , General.GetNullableGuid(ddlECAOilType.SelectedValue)
                     , General.GetNullableDateTime(txtECAEntryDate.Text + " " + timeofeca)
                     , General.GetNullableDecimal(txtDistancetogo.Text)
                     , confirm
                     , chkIDLCrossing.Checked == true ? 1 : 0
                     , General.GetNullableInteger(ddlIDLDirection.SelectedValue)
                     , ucECAlatitude.TextDegree, ucECALongitude.TextDegree, ucECAlatitude.TextMinute, ucECALongitude.TextMinute, ucECAlatitude.TextSecond, ucECALongitude.TextSecond, ucECAlatitude.TextDirection, ucECALongitude.TextDirection
                     , General.GetNullableDateTime(ucFuelCOTime.Text + " " + timeofFuelCO)
                     , General.GetNullableDecimal(txtEMLogCounter.Text)
                     , chkCounterDefective.Checked==true? 1 : 0
                     , General.GetNullableDecimal(txtEMLogDistance.Text)
                     , General.GetNullableDecimal(txtVoDOCons.Text)
                     , General.GetNullableDateTime(txtExitECADate.Text + " " + timeofexiteta)
                     , chkOffPortLimits.Checked == true ? 1 : 0
                     , General.GetNullableDateTime(txtEntryDate.Text)
                     , General.GetNullableInteger(txtEntryHour.Text)
                     , General.GetNullableDateTime(txtExitDate.Text)
                     , General.GetNullableInteger(txtExitHour.Text)
                     , General.GetNullableInteger(txtShipCalendarIdEntry.Text)
                     , General.GetNullableInteger(txtShipCalendarIdExit.Text)
                     );

            Session["NOONREPORTID"] = noonreportid;
            ucStatus.Text = "Noon Report Created";


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

            Response.Redirect("VesselPositionNoonReport.aspx?NoonReportID=" + noonreportid, false);

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuNRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("NOONREPORTLIST"))
        {
            if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                Response.Redirect("VesselPositionReports.aspx", false);
            else
                Response.Redirect("VesselPositionNoonReportList.aspx", false);
        }
        else if (Session["NOONREPORTID"] != null)
        {
            Response.Redirect("VesselPositionNoonReportEngine.aspx", false);
        }
        else
        {
            ucError.ErrorMessage = "Please save the Report.";
            ucError.Visible = true;
        }
    }
    
    private void BindData()
    {
        string noonreportid = Session["NOONREPORTID"].ToString();
        DataSet ds;
           ds = PhoenixVesselPositionNoonReport.EditNoonReport(General.GetNullableGuid(noonreportid));
           dsPDF = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            ViewState["REPORTSTATUS"] = dt.Rows[0]["FLDCONFIRMEDYN"].ToString();
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["REVIEWDYN"] = dt.Rows[0]["FLDREVIEWDYN"].ToString();
            txtCurrentDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDNOONREPORTDATE"]);
            if (General.GetNullableDateTime(dt.Rows[0]["FLDNOONREPORTDATE"].ToString()) != null)
                txtReportTime.SelectedDate = Convert.ToDateTime( dt.Rows[0]["FLDNOONREPORTDATE"]);
            txtVoyageId.Text = dt.Rows[0]["FLDVOYAGEID"].ToString();
            txtVoyageName.Text = dt.Rows[0]["FLDVOYAGENO"].ToString();
            btnShowVoyage.Attributes.Add("onclick", "return showPickList('spnPickListVoyage', 'codehelp1', '', '../Common/CommonPickListVoyage.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true); ");

            txtETA.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETA"]);
            if (General.GetNullableDateTime(dt.Rows[0]["FLDETA"].ToString()) != null)
                txtTimeOfETA.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDETA"]);
            txtETB.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETB"]);
            if (General.GetNullableDateTime(dt.Rows[0]["FLDETB"].ToString()) != null)
                txtTimeOfETB.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDETB"]);

            txtETC.Text = General.GetDateTimeToString(dt.Rows[0]["FLDETC"]);
            if (General.GetNullableDateTime(dt.Rows[0]["FLDETC"].ToString()) != null)
                txtTimeOfETC.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDETC"]);
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
            if (General.GetNullableDecimal(dt.Rows[0]["FLDLOGSPEED"].ToString()) < General.GetNullableDecimal(dt.Rows[0]["FLDCHARTERSPEED"].ToString()))
                txtLogSpeed.CssClass = "maxhighlight";
            else
                txtLogSpeed.CssClass = "input";
            txtWindForce.Text = dt.Rows[0]["FLDWINDFORCE"].ToString();
            txtCurrentSpeed.Text = dt.Rows[0]["FLDCURRENTSPEED"].ToString();
            txtSeaHeight.Text = dt.Rows[0]["FLDSEAHEIGHT"].ToString();
            ucWindDirection.SelectedDirection = dt.Rows[0]["FLDWINDDIRECTION"].ToString();
            ucCurrentDirection.SelectedDirection = dt.Rows[0]["FLDCURRENTDIRECTION"].ToString();
            ucSeaDirection.SelectedDirection = dt.Rows[0]["FLDSEADIRECTION"].ToString();
            if (General.GetNullableString(dt.Rows[0]["FLDBALLASTYN"].ToString()) != null)
                rbtnBallastLaden.SelectedValue = dt.Rows[0]["FLDBALLASTYN"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
            txtDraftF.Text = dt.Rows[0]["FLDDRAFTF"].ToString();
            txtSwell.Text = dt.Rows[0]["FLDSWELL"].ToString();
            txtBerthLocation.Text = dt.Rows[0]["FLDBERTHLOCATION"].ToString();

            txtShipMeanTime.Text = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Replace("-", "").Replace("+", "");
            txtShipMeanTimeSymbol.SelectedValue = dt.Rows[0]["FLDSHIPMEANTIME"].ToString().Contains("-") == true ? "-" : "+";

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
            ChkUSWaters.Checked = dt.Rows[0]["FLDISUSWATERS"].ToString().Equals("1") ? true : false;
            ChkThroughHRA.Checked = dt.Rows[0]["FLDISPASSINGTHROUGHHRA"].ToString().Equals("1") ? true : false;

            ChkECAyn.Checked = dt.Rows[0]["FLDECAYN"].ToString().Equals("1") ? true : false;
            string oiltype = dt.Rows[0]["FLDECAOILTYPE"].ToString();
            if (oiltype != "")
                ddlECAOilType.SelectedValue = oiltype;
            txtECAEntryDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDECAENTERDATE"]);
            if (General.GetNullableDateTime(dt.Rows[0]["FLDECAENTERDATE"].ToString()) != null)
                txtTimeofECAEntry.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDECAENTERDATE"]);

            txtVoyageAverageSpeed.Text = dt.Rows[0]["FLDVOYAGEAVERAGESPEED"].ToString();
            txtDistancetogo.Text = dt.Rows[0]["FLDDISTANCETOGO"].ToString();
            ChkOilMajorCargoOnboard.Checked = dt.Rows[0]["FLDOILMAJORCARGOONBOARDYN"].ToString().Equals("1") ? true : false;
            ddlOilMajor.SelectedValue = dt.Rows[0]["FLDOILMAJOR"].ToString();

            chkIDLCrossing.Checked = dt.Rows[0]["FLDIDLYN"].ToString().Equals("1") ? true : false;
            chkIDLCrossing.Enabled = false;
            ddlIDLDirection.SelectedValue = General.GetNullableInteger(dt.Rows[0]["FLDIDLDIRECTION"].ToString()) == null ? "Dummy" : dt.Rows[0]["FLDIDLDIRECTION"].ToString();

            txtUTCDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDUTCDATETIME"].ToString());
            if (General.GetNullableDateTime(dt.Rows[0]["FLDUTCDATETIME"].ToString()) != null)
                txtUTCTime.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDUTCDATETIME"]);


            txtEMLogCounter.Text = dt.Rows[0]["FLDEMLOGCOUNTER"].ToString();
            txtEMLogDistance.Text = dt.Rows[0]["FLDEMLOGDISTANCE"].ToString();
           

            ucECAlatitude.TextDegree = dt.Rows[0]["FLDECALAT1"].ToString();
            ucECAlatitude.TextMinute = dt.Rows[0]["FLDECALAT2"].ToString();
            ucECAlatitude.TextSecond = dt.Rows[0]["FLDECALAT3"].ToString();
            ucECAlatitude.TextDirection = dt.Rows[0]["FLDECALATDIRECTION"].ToString();
            ucECALongitude.TextDegree = dt.Rows[0]["FLDECALONG1"].ToString();
            ucECALongitude.TextMinute = dt.Rows[0]["FLDECALONG2"].ToString();
            ucECALongitude.TextSecond = dt.Rows[0]["FLDECALONG3"].ToString();
            ucECALongitude.TextDirection = dt.Rows[0]["FLDECALONGDIRECTION"].ToString();

            ucFuelCOTime.Text = General.GetDateTimeToString(dt.Rows[0]["FLDFUELCOTIME"].ToString());
            if (General.GetNullableDateTime(dt.Rows[0]["FLDFUELCOTIME"].ToString()) != null)
                txtFuelCOTime.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDFUELCOTIME"]);

            chkCounterDefective.Checked = dt.Rows[0]["FLDEMLOGCOUNTERDEFECTIVE"].ToString().Equals("1") ? true : false;
            txtVoDOCons.Text = dt.Rows[0]["FLDVOYAGEORDERDOCONS"].ToString();

            txtExitECADate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDECAEXITDATE"].ToString());
            if (General.GetNullableDateTime(dt.Rows[0]["FLDECAEXITDATE"].ToString()) != null)
                txtExitECATime.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FLDECAEXITDATE"]);

            chkOffPortLimits.Checked = dt.Rows[0]["FLDOFFPORTLIMITYN"].ToString().Equals("1") ? true : false;
            ViewState["OFFPORTLIMITYN"] = dt.Rows[0]["FLDOFFPORTLIMITYN"].ToString();

            //ChkCurrentlyUSWater.Checked = dt.Rows[0]["FLDINUSWATERYN"].ToString().Equals("1") ? true : false;

            txtEntryDate.Text= dt.Rows[0]["FLDUSWATERENTRYDATE"].ToString();
            txtShipCalendarIdEntry.Text = dt.Rows[0]["FLDSHIPCALENDEERENTRYID"].ToString();
            txtEntryHour.Text = dt.Rows[0]["FLDUSWATERENTRYHOUR"].ToString();

            txtExitDate.Text = dt.Rows[0]["FLDUSWATEREXITDATE"].ToString();
            txtShipCalendarIdExit.Text = dt.Rows[0]["FLDSHIPCALENDEEREXITID"].ToString();
            txtExitHour.Text = dt.Rows[0]["FLDUSWATEREXITHOUR"].ToString();

            txtreviewedby.Text= dt.Rows[0]["FLDREVIEWEDBY"].ToString();
            ucReviewedDate.Text = dt.Rows[0]["FLDREVIEWDDATE"].ToString();

            if (General.GetNullableDateTime(dt.Rows[0]["FLDUSWATERENTRYDATE"].ToString()) != null && General.GetNullableInteger(dt.Rows[0]["FLDCURRENTUSWATER"].ToString()) == 0)
                cmdPickListEntry.OnClientClick = "";
            if (General.GetNullableInteger(dt.Rows[0]["FLDREPORTEXISTS"].ToString()) == 1)
            {
                cmdPickListEntry.OnClientClick = "";
                cmdPickListExit.OnClientClick = "";
            }

            if (General.GetNullableString(dt.Rows[0]["FLDECAENTERDATE"].ToString()) != null && General.GetNullableInteger(dt.Rows[0]["FLDCURRENTECA"].ToString())==0)
            {
                txtECAEntryDate.Enabled = false;
                txtTimeofECAEntry.Enabled = false;

                txtECAEntryDate.CssClass = "readonlytextbox";
                txtTimeofECAEntry.CssClass = "readonlytextbox";

                txtExitECADate.Enabled = true;
                txtExitECATime.Enabled = true;

                txtExitECADate.CssClass = "input";
                txtExitECATime.CssClass = "input";

            }
            else
            {
                txtECAEntryDate.Enabled = true;
                txtTimeofECAEntry.Enabled = true;

                txtECAEntryDate.CssClass = "input";
                txtTimeofECAEntry.CssClass = "input";

                txtExitECADate.Enabled = false;
                txtExitECATime.Enabled = false;

                txtExitECADate.CssClass = "readonlytextbox";
                txtExitECATime.CssClass = "readonlytextbox";

            }

            if (General.GetNullableDecimal(dt.Rows[0]["FLDDISTANCEOBSERVED"].ToString()) != null && General.GetNullableDecimal(dt.Rows[0]["FLDEMLOGDISTANCE"].ToString()) != null
                && General.GetNullableDecimal(dt.Rows[0]["FLDEMLOGDISTANCE"].ToString()) < General.GetNullableDecimal(dt.Rows[0]["FLDDISTANCEOBSERVED"].ToString()))
                txtEMLogDistance.CssClass = "maxhighlight";
            

            BindCurrentPort();
            
            if (dt.Rows[0]["FLDCONFIRMEDYN"].ToString() == "1")
            {
                //MenuNewSaveTabStrip.Visible = false;
                lblAlertSenttoOFC.Visible = true;
            }

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

                txtVoyageAverageSpeed.Text = "";
                txtVoyageAverageSpeed.CssClass = "readonlytextbox";
                txtVoyageAverageSpeed.Enabled = false;

                txtEMLogCounter.Text = "";
                txtEMLogCounter.CssClass = "readonlytextbox";
                txtEMLogCounter.Enabled = false;

                txtEMLogDistance.Text = "";
                txtEMLogDistance.CssClass = "readonlytextbox";
                txtEMLogDistance.Enabled = false;

                chkCounterDefective.Enabled = false;

                txtETA.Text = "";
                txtETA.CssClass = "readonlytextbox";
                txtETA.Enabled = false;
                txtTimeOfETA.SelectedDate = null;
                txtTimeOfETA.CssClass = "readonlytextbox";
                txtTimeOfETA.Enabled = false;

                txtETB.Text = "";
                txtETB.CssClass = "readonlytextbox";
                txtETB.Enabled = false;
                txtTimeOfETB.SelectedDate = null;
                txtTimeOfETB.CssClass = "readonlytextbox";
                txtTimeOfETB.Enabled = false;

                txtETC.CssClass = "input";
                txtETC.Enabled = true;
                txtTimeOfETC.CssClass = "input";
                txtTimeOfETC.Enabled = true;

                txtFullSpeed.Text = "";
                txtFullSpeed.CssClass = "readonlytextbox";
                txtFullSpeed.Enabled = false;

                txtFSDistance.Text = "";
                txtFSDistance.CssClass = "readonlytextbox";
                txtFSDistance.Enabled = false;

                txtRSDistance.Text = "";
                txtRSDistance.CssClass = "readonlytextbox";
                txtRSDistance.Enabled=false;

                txtReducedSpeed.Text = "";
                txtReducedSpeed.CssClass = "readonlytextbox";
                txtReducedSpeed.Enabled = false;

                txtStopped.Text = "";
                txtStopped.CssClass = "readonlytextbox";
                txtStopped.Enabled = false;

                //txtDistanceObserved.Text = "";
                //txtDistanceObserved.CssClass = "readonlytextbox";
                //txtDistanceObserved.Enabled = false;

                txtLogSpeed.Text = "";
                if (txtLogSpeed.CssClass != "maxhighlight")
                    txtLogSpeed.CssClass = "readonlytextbox";
                txtLogSpeed.Enabled = false;

                txtNoonSpeed.Text = "";
                txtNoonSpeed.CssClass = "readonlytextbox";
                txtNoonSpeed.Enabled = false;

                //txtVOSpeed.Text = "";
                //txtVOSpeed.CssClass = "readonlytextbox";
                //txtVOSpeed.Enabled = false;

                //txtVoDOCons.Text = "";
                //txtVoDOCons.CssClass = "readonlytextbox";
                //txtVoDOCons.Enabled = false;

                txtCourse.Text = "";
                txtCourse.CssClass = "readonlytextbox";
                txtCourse.Enabled = false;

                txtDraftF.CssClass = "input";
                txtDraftF.Enabled = true;

                txtDraftA.CssClass = "input";
                txtDraftA.Enabled = true;

                //txtDWT.Text = "";
                //txtDWT.CssClass = "readonlytextbox";
                //txtDWT.Enabled = false;

                txtSeaHeight.Text = "";
                txtSeaHeight.CssClass = "readonlytextbox";
                txtSeaHeight.Enabled = false;

                ucSeaDirection.SelectedDirection = "Dummy";
                ucSeaDirection.CssClass = "readonlytextbox";
                ucSeaDirection.Enabled = "false";

                //txtSwell.Text = "";
                //txtSwell.CssClass = "readonlytextbox";
                //txtSwell.Enabled = false;

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

                //txtVOCons.Text = "";
                //txtVOCons.CssClass = "readonlytextbox";
                //txtVOCons.Enabled = false;
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

                txtTimeOfETA.SelectedDate = null;
                txtTimeOfETA.CssClass = "readonlytextbox";
                txtTimeOfETA.Enabled = false;

                txtNoonSpeed.Text = "";
                txtNoonSpeed.CssClass = "readonlytextbox";
                txtNoonSpeed.Enabled = false;

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

                txtFSDistance.Text = "";
                txtFSDistance.CssClass = "readonlytextbox";
                txtFSDistance.Enabled = false;

                txtRSDistance.Text = "";
                txtRSDistance.CssClass = "readonlytextbox";
                txtRSDistance.Enabled = false;

                txtStopped.Text = "";
                txtStopped.CssClass = "readonlytextbox";
                txtStopped.Enabled = false;

                //txtDistanceObserved.Text = "";
                //txtDistanceObserved.CssClass = "readonlytextbox";
                //txtDistanceObserved.Enabled = false;

                txtLogSpeed.Text = "";
                if (txtLogSpeed.CssClass != "maxhighlight")
                    txtLogSpeed.CssClass = "readonlytextbox";
                txtLogSpeed.Enabled = false;

                //txtVOSpeed.Text = "";
                //txtVOSpeed.CssClass = "readonlytextbox";
                //txtVOSpeed.Enabled = false;

                //txtVoDOCons.Text = "";
                //txtVoDOCons.CssClass = "readonlytextbox";
                //txtVoDOCons.Enabled = false;

                txtCourse.Text = "";
                txtCourse.CssClass = "readonlytextbox";
                txtCourse.Enabled = false;

                txtDraftF.CssClass = "input";
                txtDraftF.Enabled = true;

                txtDraftA.CssClass = "input";
                txtDraftA.Enabled = true;

                //txtDWT.Text = "";
                //txtDWT.CssClass = "readonlytextbox";
                //txtDWT.Enabled = false;

                txtSeaHeight.CssClass = "input";
                txtSeaHeight.Enabled = true;

                ucSeaDirection.CssClass = "input";
                ucSeaDirection.Enabled = "true";

                //txtSwell.Text = "";
                //txtSwell.CssClass = "readonlytextbox";
                //txtSwell.Enabled = false;

                txtCurrentSpeed.CssClass = "input";
                txtCurrentSpeed.Enabled = true;

                ucCurrentDirection.CssClass = "input";
                ucCurrentDirection.Enabled = "true";
            }
            else if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATSEA") || dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("DRIFTING"))
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
                chkOffPortLimits.Enabled = false;
                ddlCurrentPort.Enabled = false;
                ddlNextPort.Enabled = true;

                txtETA.CssClass = "input";
                txtETA.Enabled = true;
                txtTimeOfETA.CssClass = "input";
                txtTimeOfETA.Enabled = true;

                txtETB.Text = "";
                txtETB.CssClass = "readonlytextbox";
                txtETB.Enabled = false;
                txtTimeOfETB.SelectedDate = null;
                txtTimeOfETB.CssClass = "readonlytextbox";
                txtTimeOfETB.Enabled = false;

                txtETC.Text = "";
                txtETC.CssClass = "readonlytextbox";
                txtETC.Enabled = false;
                txtTimeOfETC.SelectedDate = null;
                txtTimeOfETC.CssClass = "readonlytextbox";
                txtTimeOfETC.Enabled = false;

                txtFullSpeed.CssClass = "input";
                txtFullSpeed.Enabled = true;

                txtReducedSpeed.CssClass = "input";
                txtReducedSpeed.Enabled = true;

                txtFSDistance.CssClass = "input";
                txtFSDistance.Enabled = true;

                txtRSDistance.CssClass = "input";
                txtRSDistance.Enabled = true;

                txtStopped.CssClass = "input";
                txtStopped.Enabled = true;

                //txtDistanceObserved.CssClass = "input";
                //txtDistanceObserved.Enabled = true;
                if (!txtLogSpeed.CssClass.Contains("maxhighlight"))
                    txtLogSpeed.CssClass = "input";
                txtLogSpeed.Enabled = true;

                txtNoonSpeed.CssClass = "input";
                txtNoonSpeed.Enabled = true;

                txtVOSpeed.CssClass = "input";
                txtVOSpeed.Enabled = true;

                txtVoDOCons.CssClass = "input";
                txtVoDOCons.Enabled = true;

                txtCourse.CssClass = "input";
                txtCourse.Enabled = true;

                //txtDraftF.Text = "";
                txtDraftF.CssClass = "input";
                txtDraftF.Enabled = true;

                //txtDraftA.Text = "";
                txtDraftA.CssClass = "input";
                txtDraftA.Enabled = true;

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

            if (Session["NOONREPORTID"]== null)
            {
                DataSet Location = PhoenixVesselPositionNoonReport.NoonReportLocationEdit(int.Parse(ViewState["VESSELID"].ToString()), General.GetNullableDateTime(txtCurrentDate.Text));
                DataTable Locationdt = Location.Tables[0];
                if (Location.Tables[0].Rows.Count > 0)
                {
                    ucLatitude.TextDegree = Locationdt.Rows[0]["FLDLAT1"].ToString();
                    ucLatitude.TextMinute = Locationdt.Rows[0]["FLDLAT2"].ToString();
                    ucLatitude.TextSecond = Locationdt.Rows[0]["FLDLAT3"].ToString();
                    ucLatitude.TextDirection = Locationdt.Rows[0]["FLDLATDIRECTION"].ToString();
                    ucLongitude.TextDegree = Locationdt.Rows[0]["FLDLONG1"].ToString();
                    ucLongitude.TextMinute = Locationdt.Rows[0]["FLDLONG2"].ToString();
                    ucLongitude.TextSecond = Locationdt.Rows[0]["FLDLONG3"].ToString();
                    ucLongitude.TextDirection = Locationdt.Rows[0]["FLDLONGDIRECTION"].ToString();
                }
            }
            chkOffPortLimits.Checked = ViewState["OFFPORTLIMITYN"].ToString().Equals("1") ? true : false;
            chkOffPortLimits.Enabled = true;

            txtBerthLocation.CssClass = "input";
            txtBerthLocation.Enabled = true;

            txtETA.Text = "";
            txtETA.CssClass = "readonlytextbox";
            txtETA.Enabled = false;
            txtTimeOfETA.SelectedDate = null;
            txtTimeOfETA.CssClass = "readonlytextbox";
            txtTimeOfETA.Enabled = false;

            txtETB.Text = "";
            txtETB.CssClass = "readonlytextbox";
            txtETB.Enabled = false;
            txtTimeOfETB.SelectedDate = null;
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

            txtFSDistance.Text = "";
            txtFSDistance.CssClass = "readonlytextbox";
            txtFSDistance.Enabled = false;

            txtRSDistance.Text = "";
            txtRSDistance.CssClass = "readonlytextbox";
            txtRSDistance.Enabled = false;

            txtStopped.Text = "";
            txtStopped.CssClass = "readonlytextbox";
            txtStopped.Enabled = false;

            //txtDistanceObserved.Text = "";
            //txtDistanceObserved.CssClass = "readonlytextbox";
            //txtDistanceObserved.Enabled = false;

            txtLogSpeed.Text = "";
            if (txtLogSpeed.CssClass != "maxhighlight")
                txtLogSpeed.CssClass = "readonlytextbox";
            txtLogSpeed.Enabled = false;

            //txtVOSpeed.Text = "";
            //txtVOSpeed.CssClass = "readonlytextbox";
            //txtVOSpeed.Enabled = false;

            txtNoonSpeed.Text = "";
            txtNoonSpeed.CssClass = "readonlytextbox";
            txtNoonSpeed.Enabled = false;

            //txtVoDOCons.Text = "";
            //txtVoDOCons.CssClass = "readonlytextbox";
            //txtVoDOCons.Enabled = false;

            txtCourse.Text = "";
            txtCourse.CssClass = "readonlytextbox";
            txtCourse.Enabled = false;

            txtDraftF.CssClass = "input";
            txtDraftF.Enabled = true;

            txtDraftA.CssClass = "input";
            txtDraftA.Enabled = true;

            //txtDWT.Text = "";
            //txtDWT.CssClass = "readonlytextbox";
            //txtDWT.Enabled = false;

            txtSeaHeight.Text = "";
            txtSeaHeight.CssClass = "readonlytextbox";
            txtSeaHeight.Enabled = false;

            ucSeaDirection.SelectedDirection = "Dummy";
            ucSeaDirection.CssClass = "readonlytextbox";
            ucSeaDirection.Enabled = "false";

            //txtSwell.Text = "";
            //txtSwell.CssClass = "readonlytextbox";
            //txtSwell.Enabled = false;

            txtCurrentSpeed.Text = "";
            txtCurrentSpeed.CssClass = "readonlytextbox";
            txtCurrentSpeed.Enabled = false;

            ucCurrentDirection.SelectedDirection= "Dummy";
            ucCurrentDirection.CssClass = "readonlytextbox";
            ucCurrentDirection.Enabled = "false";

            txtFSDistance.Text = "";
            txtFSDistance.CssClass = "readonlytextbox";
            txtFSDistance.Enabled = false;

            txtRSDistance.Text = "";
            txtRSDistance.CssClass = "readonlytextbox";
            txtRSDistance.Enabled = false;

            //txtVOCons.Text = "";
            //txtVOCons.CssClass = "readonlytextbox";
            //txtVOCons.Enabled = false;

            txtEMLogCounter.Text = "";
            txtEMLogCounter.CssClass = "readonlytextbox";
            txtEMLogCounter.Enabled = false;

            txtEMLogDistance.Text = "";
            txtEMLogDistance.CssClass = "readonlytextbox";
            txtEMLogDistance.Enabled = false;

            chkCounterDefective.Enabled = false;
  
        }
        else if (ddlVesselStatus.SelectedValue == "ATANCHOR")
        {
             if (Session["NOONREPORTID"]== null)
            {
                 ucLatitude.TextDegree = "";
                ucLatitude.TextMinute = "";
                ucLatitude.TextSecond = "";
                ucLatitude.TextDirection=""; 
                ucLongitude.TextDegree =""; 
                ucLongitude.TextMinute = "";
                ucLongitude.TextSecond = "";
                ucLongitude.TextDirection = "";
            }
            chkOffPortLimits.Checked = ViewState["OFFPORTLIMITYN"].ToString().Equals("1") ? true : false;
            chkOffPortLimits.Enabled = true;

            txtBerthLocation.CssClass = "input";
            txtBerthLocation.Enabled = true;

            txtETA.Text = "";
            txtETA.CssClass = "readonlytextbox";
            txtETA.Enabled = false;
            txtTimeOfETA.SelectedDate = null;
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

            txtFSDistance.Text = "";
            txtFSDistance.CssClass = "readonlytextbox";
            txtFSDistance.Enabled = false;

            txtRSDistance.Text = "";
            txtRSDistance.CssClass = "readonlytextbox";
            txtRSDistance.Enabled = false;

            txtStopped.Text = "";
            txtStopped.CssClass = "readonlytextbox";
            txtStopped.Enabled = false;

            //txtDistanceObserved.Text = "";
            //txtDistanceObserved.CssClass = "readonlytextbox";
            //txtDistanceObserved.Enabled = false;

            txtLogSpeed.Text = "";
            if (txtLogSpeed.CssClass != "maxhighlight")
                txtLogSpeed.CssClass = "readonlytextbox";
            txtLogSpeed.Enabled = false;

            //txtVOSpeed.Text = "";
            //txtVOSpeed.CssClass = "readonlytextbox";
            //txtVOSpeed.Enabled = false;

            //txtVoDOCons.Text = "";
            //txtVoDOCons.CssClass = "readonlytextbox";
            //txtVoDOCons.Enabled = false;

            txtCourse.Text = "";
            txtCourse.CssClass = "readonlytextbox";
            txtCourse.Enabled = false;

            txtDraftF.CssClass = "input";
            txtDraftF.Enabled = true;

            txtDraftA.CssClass = "input";
            txtDraftA.Enabled = true;

            //txtDWT.Text = "";
            //txtDWT.CssClass = "readonlytextbox";
            //txtDWT.Enabled = false;

            txtSeaHeight.CssClass = "input";
            txtSeaHeight.Enabled = true;

            ucSeaDirection.CssClass = "input";
            ucSeaDirection.Enabled = "true";

            //txtSwell.Text = "";
            //txtSwell.CssClass = "readonlytextbox";
            //txtSwell.Enabled = false;

            txtCurrentSpeed.CssClass = "input";
            txtCurrentSpeed.Enabled = true;

            ucCurrentDirection.CssClass = "input";
            ucCurrentDirection.Enabled = "true";

            //rbtnBallastLaden.Enabled = false;
            //foreach (ListItem li in rbtnBallastLaden.Items)
            //{
            //    li.Selected = false;
            //}
        }
        else if (ddlVesselStatus.SelectedValue == "ATSEA" || ddlVesselStatus.SelectedValue == "DRIFTING")
        {

            if (Session["NOONREPORTID"] == null)
            {
                ucLatitude.TextDegree = "";
                ucLatitude.TextMinute = "";
                ucLatitude.TextSecond = "";
                ucLatitude.TextDirection = "";
                ucLongitude.TextDegree = "";
                ucLongitude.TextMinute = "";
                ucLongitude.TextSecond = "";
                ucLongitude.TextDirection = "";
            }
            chkOffPortLimits.Enabled = false;
            chkOffPortLimits.Checked = false;

            txtBerthLocation.Text = "";
            txtBerthLocation.CssClass = "readonlytextbox";
            txtBerthLocation.Enabled = false;

            //txtNextPort.CssClass = "input";
            //txtNextPort.Enabled = true;

            txtETA.CssClass = "input";
            txtETA.Enabled = true;
            txtTimeOfETA.CssClass = "input";
            txtTimeOfETA.Enabled = true;

            txtETB.Text = "";
            txtETB.CssClass = "readonlytextbox";
            txtETB.Enabled = false;
            txtTimeOfETB.SelectedDate = null;
            txtTimeOfETB.CssClass = "readonlytextbox";
            txtTimeOfETB.Enabled = false;

            txtETC.Text = "";
            txtETC.CssClass = "readonlytextbox";
            txtETC.Enabled = false;
            txtTimeOfETC.SelectedDate = null;
            txtTimeOfETC.CssClass = "readonlytextbox";
            txtTimeOfETC.Enabled = false;

            txtFullSpeed.CssClass = "input";
            txtFullSpeed.Enabled = true;

            txtReducedSpeed.CssClass = "input";
            txtReducedSpeed.Enabled = true;

            txtFSDistance.CssClass = "input";
            txtFSDistance.Enabled = true;

            txtRSDistance.CssClass = "input";
            txtRSDistance.Enabled = true;

            txtStopped.CssClass = "input";
            txtStopped.Enabled = true;

            //txtDistanceObserved.CssClass = "input";
            //txtDistanceObserved.Enabled = true;
            if (txtLogSpeed.CssClass != "maxhighlight")
                txtLogSpeed.CssClass = "input";
            txtLogSpeed.Enabled = true;

            txtNoonSpeed.CssClass = "input";
            txtNoonSpeed.Enabled = true;

            txtVOSpeed.CssClass = "input";
            txtVOSpeed.Enabled = true;

            txtVoDOCons.CssClass = "input";
            txtVoDOCons.Enabled = true;

            txtCourse.CssClass = "input";
            txtCourse.Enabled = true;

            //txtDraftF.Text = "";
            txtDraftF.CssClass = "input";
            txtDraftF.Enabled = true;

            //txtDraftA.Text = "";
            txtDraftA.CssClass = "input";
            txtDraftA.Enabled = true;

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

            txtEMLogCounter.CssClass = "input";
            txtEMLogCounter.Enabled = true;

            chkCounterDefective.Enabled = true;
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
                txtTimeOfETA.SelectedDate = null;
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
                //minvalue = General.GetNullableDecimal(dr["FLDMINVALUE"].ToString());
                //maxvalue = General.GetNullableDecimal(dr["FLDMAXVALUE"].ToString());
                minvalue = General.GetNullableDecimal(dr["FLDALERTLEVEL"].ToString());
                maxvalue = General.GetNullableDecimal(dr["FLDMAXALERTLEVEL"].ToString());

                switch (dr["FLDCOLUMNNAME"].ToString())
                {                      
                    case "FLDDISTANCEOBSERVED":
                        {
                            if (General.GetNullableDecimal(txtDistanceObserved.Text) < minvalue || General.GetNullableDecimal(txtDistanceObserved.Text) > maxvalue)
                                txtDistanceObserved.CssClass = "maxhighlight";
                            else
                                txtDistanceObserved.CssClass = "readonlytextbox";                            
                            break;
                        }
                    //case "FLDSPEED":
                    //    {
                    //        if (General.GetNullableDecimal(txtLogSpeed.Text) < minvalue || General.GetNullableDecimal(txtLogSpeed.Text) > maxvalue)
                    //            txtLogSpeed.CssClass = "maxhighlight";
                    //        else
                    //            txtLogSpeed.CssClass = "input";
                    //        break;
                    //    }
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

     private void ViewMenu()
    {
        PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();
        
       
        if (Request.QueryString["mode"] != null)
            toolbarNoonReporttap.AddButton("Copy", "COPY", ToolBarDirection.Right);
        toolbarNoonReporttap.AddButton("Export PDF", "PDF", ToolBarDirection.Right);
        if (ViewState["REPORTSTATUS"] != null && ViewState["REPORTSTATUS"].ToString() != "1")
        {
            toolbarNoonReporttap.AddButton("Send To office", "SENDTOOFFICE", ToolBarDirection.Right);
            toolbarNoonReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            
        }
        if(ViewState["REVIEWDYN"].ToString()=="0" && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbarNoonReporttap.AddButton("Review", "REVIEW", ToolBarDirection.Right);
        }
        MenuNewSaveTabStrip.AccessRights = this.ViewState;
        MenuNewSaveTabStrip.MenuList = toolbarNoonReporttap.Show();
        ScriptManager.GetCurrent(this).RegisterPostBackControl(MenuNewSaveTabStrip);

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            trreviewby.Visible = false;
            trreviewdate.Visible = false;
        }
    }

    protected void chkIDLCrossing_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkIDLCrossing.Checked == true)
            ddlIDLDirection.Enabled = true;
        else
        {
            ddlIDLDirection.Enabled = false;
            ddlIDLDirection.SelectedValue = "Dummy";
            DataSet dsReportDate = PhoenixVesselPositionNoonReport.GetNoonReportDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dsReportDate.Tables.Count > 0)
            {
                if (dsReportDate.Tables[0].Rows[0]["FLDREPORTDATE"] != null && dsReportDate.Tables[0].Rows[0]["FLDREPORTDATE"].ToString() != string.Empty)
                    txtCurrentDate.Text = General.GetDateTimeToString(dsReportDate.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
            }

        }
    }
    protected void ddlIDLDirection_OnTextChanged(object sender, EventArgs e)
    {        
        DateTime reportDate=new DateTime();
        DataSet dsReportDate = PhoenixVesselPositionNoonReport.GetNoonReportDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dsReportDate.Tables.Count > 0)
        {
            if (dsReportDate.Tables[0].Rows[0]["FLDREPORTDATE"] != null && dsReportDate.Tables[0].Rows[0]["FLDREPORTDATE"].ToString() != string.Empty)
                reportDate = Convert.ToDateTime(dsReportDate.Tables[0].Rows[0]["FLDREPORTDATE"].ToString());
            else
            {
                if (General.GetNullableDateTime(txtCurrentDate.Text) != null)
                    reportDate = Convert.ToDateTime(txtCurrentDate.Text);
            }
        }
        if (ddlIDLDirection.SelectedValue == "1")
            reportDate = reportDate.AddDays(1);
        if (ddlIDLDirection.SelectedValue == "2")
            reportDate = reportDate.AddDays(-1);
        txtCurrentDate.Text = reportDate.ToString();

    }
    protected void chkCounterDefective_OnCheckedChanged(object sender, EventArgs e)
    {
        //if (chkCounterDefective.Checked == true)
        //{
        //    txtNoonSpeed.CssClass = "input";
        //    txtNoonSpeed.Enabled = true;

        //    txtEMLogDistance.CssClass = "input";
        //    txtEMLogDistance.Enabled = true;

        //}
        //else
        //{
        //    txtNoonSpeed.CssClass = "readonlytextbox";
        //    txtNoonSpeed.Enabled = false;

        //    txtEMLogDistance.CssClass = "readonlytextbox";
        //    txtEMLogDistance.Enabled = false;
        //}
    }

    private string PrepareHtmlDoc()
    {
        BindData();
        StringBuilder DsHtmlcontent = new StringBuilder();

        if (dsPDF.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = dsPDF.Tables[0].Rows[0];
            string idlcrossing = General.GetNullableInteger(dr1["FLDIDLYN"].ToString()) == 1 ? "Yes" : "No";
            string BallastLaden = General.GetNullableInteger(dr1["FLDBALLASTYN"].ToString()) == null || General.GetNullableInteger(dr1["FLDBALLASTYN"].ToString()) == 0 ? "Ballast" : "Laden";
            string emlogcounterdefrective = General.GetNullableInteger(dr1["FLDEMLOGCOUNTERDEFECTIVE"].ToString()) == 1 ? "Yes" : "No";
            string icingOnDec = dr1["FLDICINGONDECK"].ToString().Equals("1") ? "Yes" : "No";
            string UsPortin7Days = dr1["FLDISUSWATERS"].ToString().Equals("1") ? "Yes" : "No";
            string TransittingGulf = dr1["FLDISPASSINGTHROUGHHRA"].ToString().Equals("1") ? "Yes" : "No";
            string EntryintoECA = dr1["FLDECAYN"].ToString().Equals("1") ? "Yes" : "No";
            string OilType = General.GetNullableGuid(dr1["FLDECAOILTYPE"].ToString()) == null ? "" : ddlECAOilType.SelectedItem.ToString();

            DsHtmlcontent.Append("<html><table><tr><td align=\"center\"><b>NOON REPORT DECK DEPARTMENT DETAIL</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\" >Vessel</td><td colspan=\"4\">" + dr1["FLDVESSELNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Ship Mean Time</td><td colspan=\"4\"><b>UTC : </b>" + dr1["FLDSHIPMEANTIME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Report Date</td><td colspan=\"4\"><b>LT : </b>" + General.GetDateTimeToString(dr1["FLDNOONREPORTDATE"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDNOONREPORTDATE"]) + " <b>UTC : </b>" + General.GetDateTimeToString(dr1["FLDUTCDATETIME"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDUTCDATETIME"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">IDL Crossing</td><td colspan=\"4\">" + idlcrossing + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Voyage No</td><td colspan=\"4\">" + dr1["FLDVOYAGENO"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Vessel's Status </td><td colspan=\"4\">" + ddlVesselStatus.Text.ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Current Port </td><td colspan=\"4\">" + dr1["FLDCURRENTPORTNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Berth / Location </td><td colspan=\"4\">" + dr1["FLDBERTHLOCATION"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Latitude</td><td colspan=\"4\">" + dr1["FLDLAT1"].ToString() + "  :  " + dr1["FLDLAT2"].ToString() + "  :  " + dr1["FLDLAT3"].ToString() + "  :  " + dr1["FLDLATDIRECTION"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Longitude</td><td colspan=\"4\">" + dr1["FLDLONG1"].ToString() + "  :  " + dr1["FLDLONG2"].ToString() + "  :  " + dr1["FLDLONG3"].ToString() + "  :  " + dr1["FLDLONGDIRECTION"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Next Port</td><td colspan=\"4\">" + dr1["FLDNEXTPORTNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">ETA</td><td colspan=\"4\">" + General.GetDateTimeToString(dr1["FLDETA"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDETA"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">ETB</td><td colspan=\"4\">" + General.GetDateTimeToString(dr1["FLDETB"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDETB"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">ETC/D</td><td colspan=\"4\">" + General.GetDateTimeToString(dr1["FLDETC"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDETC"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Ballast/Laden</td><td colspan=\"4\">" + BallastLaden + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Full Speed</td><td colspan=\"2\">" + dr1["FLDFULLSPEED"].ToString() + " hrs</td><td colspan=\"2\">" + dr1["FLDFULLSPEEDDISTANCE"].ToString() + "  nm</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Reduced Speed</td><td colspan=\"2\">" + dr1["FLDREDUCEDSPEED"].ToString() + " hrs</td><td colspan=\"2\">" + dr1["FLDREDUCEDDISTANCE"].ToString() + "  nm</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Stopped</td><td colspan=\"4\">" + dr1["FLDSTOPPED"].ToString() + "   hrs</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Distance Observed</td><td colspan=\"4\">" + dr1["FLDDISTANCEOBSERVED"].ToString() + "   nm</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Obs Speed</td><td colspan=\"4\">" + dr1["FLDLOGSPEED"].ToString() + "   hrs</td><tr>");
            //DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">EM Log Counter</td><td colspan=\"2\">" + dr1["FLDEMLOGCOUNTER"].ToString() + "</td><td colspan=\"2\">Counter Defective : " + emlogcounterdefrective + "</td><tr>");
            //DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">EM Log Distance </td><td colspan=\"4\">" + dr1["FLDEMLOGDISTANCE"].ToString() + "   nm</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">EM Log Speed</td><td colspan=\"4\">" + dr1["FLDNOONSPEED"].ToString() + "   hrs</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Voyage Average Speed</td><td colspan=\"4\">" + dr1["FLDVOYAGEAVERAGESPEED"].ToString() + "   kts</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Distance To Go</td><td colspan=\"4\">" + dr1["FLDDISTANCETOGO"].ToString() + "   nm</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Voyage Order Speed</td><td colspan=\"4\">" + dr1["FLDVOYAGEORDERSPEED"].ToString() + "   kts</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Voyage Order Cons</td><td colspan=\"4\">" + dr1["FLDVOYAGEORDERCONS"].ToString() + "   mt</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Course</td><td colspan=\"4\">" + dr1["FLDCOURSE"].ToString() + "   T</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Draft F</td><td colspan=\"4\">" + dr1["FLDDRAFTF"].ToString() + "   m</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Draft A</td><td colspan=\"4\">" + dr1["FLDDRAFT"].ToString() + "   m</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Displacement </td><td colspan=\"4\">" + dr1["FLDDWT"].ToString() + "   mt</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Wind Direction</td><td colspan=\"4\">" + dr1["FLDWINDDIRECTIONNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Wind Force</td><td colspan=\"4\">" + dr1["FLDWINDFORCE"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Sea Height</td><td colspan=\"4\">" + dr1["FLDSEAHEIGHT"].ToString() + "   m</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Sea Direction</td><td colspan=\"4\">" + dr1["FLDSEADIRECTIONNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Swell Height</td><td colspan=\"4\">" + dr1["FLDSWELL"].ToString() + "   m</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Current Set and Drift</td><td colspan=\"2\">" + dr1["FLDCURRENTSPEED"].ToString() + "  kts</td><td colspan=\"2\">" + dr1["FLDCURRENTDIRECTIONNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Air Temp</td><td colspan=\"4\">" + dr1["FLDAIRTEMP"].ToString() + "   ° C</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Icing on Deck?</td><td colspan=\"4\">" + icingOnDec + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Is vessel calling US Port within next 7 days or is vessel in US waters?</td><td colspan=\"4\">" + UsPortin7Days + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Is vessel Transiting Gulf of Aden or passing through HRA in next 14 days?</td><td colspan=\"4\">" + TransittingGulf + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Entry into ECA</td><td colspan=\"4\">" + EntryintoECA + "   " + General.GetDateTimeToString(dr1["FLDECAENTERDATE"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDECAENTERDATE"]) + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Latitude</td><td colspan=\"4\">" + dr1["FLDECALAT1"].ToString() + "  :  " + dr1["FLDECALAT2"].ToString() + "  :  " + dr1["FLDECALAT3"].ToString() + "  :  " + dr1["FLDECALATDIRECTION"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Longitude</td><td colspan=\"4\">" + dr1["FLDECALONG1"].ToString() + "  :  " + dr1["FLDECALONG2"].ToString() + "  :  " + dr1["FLDECALONG3"].ToString() + "  :  " + dr1["FLDECALONGDIRECTION"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Fuel used in ECA</td><td colspan=\"4\">" + OilType + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Fuel C/O Time</td><td colspan=\"4\">" + General.GetDateTimeToString(dr1["FLDFUELCOTIME"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDFUELCOTIME"]) + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Master Remarks</td><td colspan=\"4\">" + dr1["FLDREMARKS"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("</table></html>");

        }

        return DsHtmlcontent.ToString();

    }

    public void ConvertToPdf(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetMargins(36f, 36f, 36f, 0f);
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    string filefullpath = "DeckDepartment" + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();

                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                    }
                    document.Close();
                    Response.Buffer = true;
                    var bytes = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
