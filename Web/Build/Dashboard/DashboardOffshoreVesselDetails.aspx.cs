using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;
using System.Collections.Specialized;


public partial class Dashboard_DashboardOffshoreVesselDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        //BindToolBar();

        BindData();
    }
    private void BindData()
    {

        int vesselid = int.Parse(ViewState["VESSELID"].ToString());
        DataTable dt = new DataTable();
        if(PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode=="OFFSHORE")
            dt= PhoenixCommonDashboard.VesselOffshoreDetailsEdit(vesselid);
        else
          dt = PhoenixCommonDashboard.VesselDetailsEdit(vesselid);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];// ds.Tables[0].Rows[0];

            imgPhoto.ImageUrl = "../css/" + PhoenixGeneralSettings.CurrentGeneralSetting.Theme + "/images/vessel/" + dt.Rows[0]["FLDIMONUMBER"].ToString() + ".png";
            lblVesselName.Text = dr["FLDVESSELNAME"].ToString();
            // lnkMasterName.Text = dr["FLDMASTERNAME"].ToString();
            lblMasterName.Text = dr["FLDMASTERNAME"].ToString();

            string strmasteremployeeid = dr["FLDMASTEREMPLOYEEID"].ToString();
            if (strmasteremployeeid != null)
            {
                // lnkMasterName.Attributes.Add("onclick", "parent.Openpopup('BioData','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=BIODATA&empid="
                //            + strmasteremployeeid + "&showmenu=1'); return false;");
                //Filter.CurrentMenuCodeSelection = "CRW-PER-PER";
                //lnkMasterName.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + strmasteremployeeid + "'); return false;");
            }
            //lnkChiefEngineer.Text = dr["FLDCENAME"].ToString();
            lblChiefEngineer.Text = dr["FLDCENAME"].ToString();

            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            //{
            //    lnkMasterName.Visible = true;
            //    lnkChiefEngineer.Visible = true;
            //}
            //else
            //{
            //    lblMasterName.Visible = true;
            //    lblChiefEngineer.Visible = true;
            //}


            string strceemployeeid = dr["FLDCEEMPLOYEEID"].ToString();
            //if (strceemployeeid != null)
            //{
            //    lnkChiefEngineer.Attributes.Add("onclick", "parent.Openpopup('BioData','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=BIODATA&empid="
            //                + strceemployeeid + "&showmenu=1'); return false;");

            //    //Filter.CurrentMenuCodeSelection = "CRW-PER-PER";
            //    //lnkChiefEnginner.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + strceemployeeid + "'); return false;");
            //}
            lblLastPort.Text = dr["FLDLOCATION"].ToString();
            lblvesselstatus.Text= dr["FLDVESSELSTATUS"].ToString();
            lblNextPort.Text = dr["FLDNEXTPORT"].ToString();
            lblETA.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDETA"]) + " - " + string.Format("{0:dd/MM/yyyy}", dr["FLDETD"]);
            lblLatLog.Text = "Lat: " + dr["FLDLATITUDE"].ToString() + " Long: " + dr["FLDLONGITUDE"].ToString();
            lblKts.Text = dr["FLDAVGSPEED"].ToString();
           // lblCargo.Text = dr["FLDFUELCONS"].ToString();

            lblpscinspection.Text = dr["FLDPSCDONEBY"].ToString() + " - " + dr["FLDLASTPSCDATE"].ToString();
            lblsireinspection.Text = dr["FLDSIREDONEBY"].ToString() + " - " + dr["FLDLASTSIREDATE"].ToString();

            lbldrydock.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDDRYDOCKDATEFROMORDER"]);

            lbllastexport.Text = dr["FLDEXPORTSEQUENCE"].ToString() + " - " + dr["FLDEXPORTDATE"].ToString();
            lbllastimport.Text = dr["FLDIMPORTSEQUENCE"].ToString() + " - " + dr["FLDIMPORTDATE"].ToString();
            lblsparelifeboat.Text = dr["FLDSPARELIFEBOATCAPACITY"].ToString();
            lblPOB.Text = dr["FLDPOB"].ToString();
            lblfleetmanager.Text = dr["FLDFLEETNAME"].ToString() + " / " + dr["FLDFLEETMANAGERNAME"].ToString();
            lbltechsuptd.Text = dr["FLDTECHSUPTNAME"].ToString();
            lblvesseltype.Text = dr["FLDTYPENAME"].ToString();
            lblhullnum.Text = dr["FLDHULLNUMBER"].ToString();
            lblimonumber.Text = dr["FLDIMONUMBER"].ToString();
            lbloffnum.Text = dr["FLDOFFICIALNUMBER"].ToString();
            lblcallsign.Text = dr["FLDCALLSIGN"].ToString();
            lblmmsino.Text = dr["FLDMMSINUMBER"].ToString();
            lblvesselcode.Text = dr["FLDVESSELCODE"].ToString();
            lblregisterport.Text = dr["FLDPORTNAME"].ToString();
            lblflag.Text = dr["FLDFLAGNAME"].ToString();
            lbldisponentowner.Text = dr["FLDDISPONENTOWNERNAME"].ToString();
            lblowner.Text = dr["FLDOWNERNAME"].ToString();
            lblCharterer.Text = dr["FLDCHARTERERNAME"].ToString();
            lblprincipal.Text = dr["FLDPRINCIPALNAME"].ToString();
            lblmanager.Text = dr["FLDPRIMARYMANAGERNAME"].ToString();
            lblpni.Text = dr["FLDPNICLUBNAME"].ToString();
            lblhnm.Text = "";
            lblclassification.Text = dr["FLDCLASSIFICATION"].ToString();
            lblclassno.Text = dr["FLDCLASSNO"].ToString();
            lblclassnotation.Text = dr["FLDCLASSNOTATION"].ToString();
            lblkeellaid.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDDATEENTERED"]);
            lbllaunched.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDDATELEFT"]);
            lbldelivery.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDORGDATEENTERED"]);
            lblmgmnttype.Text = dr["FLDMANAGEMENTTYPENAME"].ToString();
            lblnavarea.Text = dr["FLDNAVIGATIONAREA"].ToString();
            lbliceclass.Text = dr["FLDICECLASSED"].ToString().Equals("1") ? "Yes" : "No";
            lblbuilder.Text = dr["FLDYARDNAME"].ToString();
            lblfittedframo.Text = dr["FLDFITWITHFRAMOYN"].ToString().Equals("1") ? "Yes" : "No";
            lblesmtakeover.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDORGDATELEFT"]);
            lblesmhandover.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDESMHANDOVERDATE"]);


            //Communication
          //  lblsatbphone.Text = dr["FLDBPHONE"].ToString();
            lblsatcTelex.Text = dr["FLDCTALEX"].ToString();
            //lblsatbfax.Text = dr["FLDBFAX"].ToString();
            lblvsatphone.Text = dr["FLDPHONE"].ToString();
            lblvsatfax.Text = dr["FLDFAX"].ToString();
           // lblfleet77phone.Text = dr["FLDFPHONE"].ToString();
            //lblfleet77fax.Text = dr["FLDFFAX"].ToString();
            lblemail.Text = dr["FLDEMAIL"].ToString();
            lblfbbphone.Text = dr["FLDAPHONE"].ToString();
            lblNotifyEmail.Text = dr["FLDNOTIFICATIONEMAIL"].ToString();
            lblfbbfax.Text = dr["FLDAFAX"].ToString();
            lblmobilenumber.Text = dr["FLDMOBILENUMBER"].ToString();

            //Load Line
            lblFreeboardTropical.Text = dr["FLDFREEBOARDTROPICAL"].ToString();
            lblDraftTropical.Text = dr["FLDDRAFTTROPICAL"].ToString();
            lblDWTTropical.Text = dr["FLDDWTTROPICAL"].ToString();
            lblFreeboardSummer.Text = dr["FLDFREEBOARDSUMMER"].ToString();
            lblDraftSummer.Text = dr["FLDDRAFTSUMMER"].ToString();
            lblDWTSummer.Text = dr["FLDDWTSUMMER"].ToString();
            lblFreeboardWinter.Text = dr["FLDFREEBOARDWINTER"].ToString();
            lblDraftWinter.Text = dr["FLDDRAFTWINTER"].ToString();
            lblDWTWinter.Text = dr["FLDDWTWINTER"].ToString();
            lblFreeboardLightship.Text = dr["FLDFREEBOARDLIGHTSHIP"].ToString();
            lblDraftLightship.Text = dr["FLDDRAFTLIGHTSHIP"].ToString();
            lblDWTLightship.Text = dr["FLDDWTLIGHTSHIP"].ToString();
            lblFreeboardBallastCond.Text = dr["FLDFREEBOARDBALLASTCOND"].ToString();
            lblDraftBallastCond.Text = dr["FLDDRAFTBALLASTCOND"].ToString();
            lblDWTBallastCond.Text = dr["FLDDWTBALLASTCOND"].ToString();

            //Tonneag

            lblRegisteredGT.Text = String.Format("{0:###,###}", General.GetNullableInteger(dr["FLDREGISTEREDGT"].ToString()));
            lblSuezGT.Text = String.Format("{0:###,###}", General.GetNullableInteger(dr["FLDSUEZGT"].ToString()));
            lblPanamaGT.Text = String.Format("{0:###,###}", General.GetNullableInteger(dr["FLDPANAMAGT"].ToString()));
            lblRegisteredNT.Text = String.Format("{0:###,###}", General.GetNullableInteger(dr["FLDREGISTEREDNT"].ToString()));
            lblSuezNT.Text = String.Format("{0:###,###}", General.GetNullableInteger(dr["FLDSUEZNT"].ToString()));
            lblPanamaNT.Text = String.Format("{0:###,###}", General.GetNullableInteger(dr["FLDPANAMANT"].ToString()));

            //txtLoa.Text = dr["FLDLOA"].ToString();
            //txtLBP.Text = dr["FLDLBP"].ToString();
            //txtSpeed.Text = dr["FLDSPEED"].ToString();
            //txtBreath.Text = dr["FLDBREADTH"].ToString();
            //txtDepth.Text = dr["FLDDEPTH"].ToString();
            //txtHeight.Text = dr["FLDHEIGHT"].ToString();









            //txtEngineType.Text = dr["FLDENGINENAME"].ToString();
            //txtEngineModel.Text = dr["FLDENGINEMODEL"].ToString();

            //txtMainEngine.Text = dr["FLDMAINENGINE"].ToString();
            //txtMCR.Text = dr["FLDMCR"].ToString();
            //txtAuxEngine.Text = dr["FLDAUXENGINE"].ToString();
            //txtAuxBoiler.Text = dr["FLDAUXBOILER"].ToString();
            //txtKW.Text = dr["FLDKW"].ToString();
            //txtBhp.Text = dr["FLDBHP"].ToString();

            //txtLifeBoatQuantity.Text = dr["FLDLIFEBOATQUANTITY"].ToString();
            //txtLifeBoatCapacity.Text = dr["FLDLIFEBOATCAPACITY"].ToString();
            //txtSecCapacity.Text = dr["FLDSECCAPACITY"].ToString();

            //txtRemarks.Text = dr["FLDREMARKS"].ToString();






            ////txtAccAdministratorEmail.Text = dr["FLDACCOUNTADMINISTRATOREMAIL"].ToString();

            ////txtFleetManagerEmail.Text = dr["FLDFLEETMANAGEREMAIL"].ToString();
            //txtAccInchargeEmail.Text = dr["FLDACCOUNTINCHARGEEMAIL"].ToString();







            //lblLatitude.Text = dr["FLDLATITUDE"].ToString();
            //lblLongitude.Text = dr["FLDLONGITUDE"].ToString();


            //imgPhoto.ImageUrl = "../css/Theme1/images/" + ds.Tables[0].Rows[0]["FLDIMONUMBER"].ToString() + ".gif";
        }

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ddlVessel", ViewState["VESSELID"].ToString());
        criteria.Add("ddlFleet", "");
        criteria.Add("txtReportFrom", "");
        criteria.Add("txtReportTo", "");
        criteria.Add("UcArrivalPort", "");
        criteria.Add("ddlMonth", "");
        criteria.Add("ddlYear", "");
        criteria.Add("ucVoyage", "");

        Filter.CurrentArrivalReportFilter = criteria;




        criteria = new NameValueCollection();
        criteria.Clear();

        criteria.Add("ddlVessel", ViewState["VESSELID"].ToString());
        criteria.Add("ddlFleet", "");
        criteria.Add("txtReportFrom", "");
        criteria.Add("txtReportTo", "");
        criteria.Add("UcCurrentPort", "");
        criteria.Add("UcNextPort", "");
        criteria.Add("ddlMonth", "");
        criteria.Add("ddlYear", "");
        criteria.Add("ucVoyage", "");

        Filter.CurrentDepartureReportFilter = criteria;
        Filter.CurrentShiftingReportFilter = criteria;



        criteria = new NameValueCollection();
        criteria.Clear();

        criteria.Add("ddlVessel", ViewState["VESSELID"].ToString());
        criteria.Add("ddlFleet", "");
        criteria.Add("txtReportFrom", "");
        criteria.Add("txtReportTo", "");
        criteria.Add("UcPortfrom", "");
        criteria.Add("UcPortTo", "");
        criteria.Add("ddlMonth", "");
        criteria.Add("ddlYear", "");
        criteria.Add("txtETAFrom", "");
        criteria.Add("txtETATo", "");

        Filter.CurrentNoonReportListFilter = criteria;

        //btnArrivalReport.Attributes.Add("onclick", "javascript: top.openNewWindow('VesselPosition','Arrival Report','VesselPosition/VesselPositionArrivalReport.aspx'); return false;");
        //btnDepartureReport.Attributes.Add("onclick", "javascript: top.openNewWindow('VesselPosition','Departure Report','VesselPosition/VesselPositionDepartureReport.aspx'); return false;");
       // btnShiftingreport.Attributes.Add("onclick", "javascript: top.openNewWindow('VesselPosition','Shifting Report','VesselPosition/VesselPositionShiftingReport.aspx'); return false;");
       // btnNoonReport.Attributes.Add("onclick", "javascript: top.openNewWindow('VesselPosition','Noon Report','VesselPosition/VesselPositionNoonReportList.aspx'); return false;");
        btnMonthlyreport.Attributes.Add("onclick", "javascript: top.openNewWindow('DMR','Monthly report','CrewOffshore/CrewOffshoreDMRMonthlyReportList.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "'); return false;");
        btnMidnightreport.Attributes.Add("onclick", "javascript: top.openNewWindow('DMR','Midnight Report','CrewOffshore/CrewOffshoreDMRMidNightReportList.aspx?VesselId=" + ViewState["VESSELID"].ToString() + "&PageNumber=1'); return false;");
        //btnQuarterlyReport.Attributes.Add("onclick", "javascript: top.openNewWindow('VesselPosition','QuarterlyReport','VesselPosition/VesselPositionQuarterlyReportList.aspx'); return false;");
        //btnVoyagereport.Attributes.Add("onclick", "javascript: top.openNewWindow('VesselPosition','Voyage','VesselPosition/VesselPositionVoyage.aspx'); return false;");

        //btncommercialPerformance.Attributes.Add("onclick", "javascript: top.openNewWindow('Dashboard','Commercial Performance','Dashboard/DashboardCommercialPerformanceChart.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&vesselname=" + lblVesselName.Text + "'); return false;");
        //btnMachineryParameters.Attributes.Add("onclick", "javascript: top.openNewWindow('Dashboard','Machinery Parameters','Dashboard/DashboardMachinaryPerformenceChart.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&vesselname=" + lblVesselName.Text + "'); return false;");
        //btnAEPerformance.Attributes.Add("onclick", "javascript: top.openNewWindow('Dashboard','A/E Performance','Dashboard/DashboardTechnicalAuxiliaryPerformance.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&vesselname=" + lblVesselName.Text + "'); return false;");
        //btnExceptionReport.Attributes.Add("onclick", "javascript: top.openNewWindow('Dashboard','Exception Report','Dashboard/DashboardVesselPositionAlerts.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&vesselname=" + lblVesselName.Text + "'); return false;");

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dt.Rows.Count > 0)
            ViewState["FLDDTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";

    }
}