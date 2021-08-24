using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardCommonVesselParticulars : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                else
                    ViewState["VESSELID"] = "0"; 
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselParticularsEdit(int.Parse(ViewState["VESSELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            lblVesselName.Text = dr["FLDVESSELNAME"].ToString();
            lnkMasterName.Text = dr["FLDMASTERNAME"].ToString();
            lblMasterName.Text = dr["FLDMASTERNAME"].ToString();

            string strmasteremployeeid = dr["FLDMASTEREMPLOYEEID"].ToString();
            if (strmasteremployeeid != null)
            {
                lnkMasterName.Attributes.Add("onclick", "parent.Openpopup('BioData','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=BIODATA&empid="
                            + strmasteremployeeid + "&showmenu=1'); return false;");
            }
            lnkChiefEnginner.Text = dr["FLDCENAME"].ToString();
            lblChiefEngineer.Text = dr["FLDCENAME"].ToString();

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                lnkMasterName.Visible = true;
                lnkChiefEnginner.Visible = true;
            }
            else
            {
                lblMasterName.Visible = true;
                lblChiefEngineer.Visible = true;
            }


            string strceemployeeid = dr["FLDCEEMPLOYEEID"].ToString();
            if (strceemployeeid != null)
            {
                lnkChiefEnginner.Attributes.Add("onclick", "parent.Openpopup('BioData','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=BIODATA&empid="
                            + strceemployeeid + "&showmenu=1'); return false;");
            }
            lblLastExportSequence.Text = dr["FLDEXPORTSEQUENCE"].ToString();
            lblLastImportSequence.Text = dr["FLDIMPORTSEQUENCE"].ToString();
            lblLastExportDate.Text = dr["FLDEXPORTDATE"].ToString();
            lblLastImportDate.Text = dr["FLDIMPORTDATE"].ToString();
            lblSpareLifeBoatCapacity.Text = dr["FLDSPARELIFEBOATCAPACITY"].ToString();
            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            txtType.Text = dr["FLDTYPENAME"].ToString();
            txtIMONumber.Text = dr["FLDIMONUMBER"].ToString();
            txtOfficialNumber.Text = dr["FLDOFFICIALNUMBER"].ToString();
            txtCallSign.Text = dr["FLDCALLSIGN"].ToString();
            txtMMSINo.Text = dr["FLDMMSINUMBER"].ToString();
            txtClassification.Text = dr["FLDCLASSIFICATION"].ToString();
            txtClassNotation.Text = dr["FLDCLASSNOTATION"].ToString();
            txtVesselCode.Text = dr["FLDVESSELCODE"].ToString();
            txtIceClass.Text = dr["FLDICECLASSED"].ToString().Equals("1") ? "Yes" : "No";
            txtFlag.Text = dr["FLDFLAGNAME"].ToString();
            txtPortofRegistery.Text = dr["FLDPORTNAME"].ToString();
            txtOwner.Text = dr["FLDOWNERNAME"].ToString();
            txtDisponentOwner.Text = dr["FLDDISPONENTOWNERNAME"].ToString();
            txtManager.Text = dr["FLDPRIMARYMANAGERNAME"].ToString();
            txtCharterer.Text = dr["FLDCHARTERERNAME"].ToString();
            txtPrincipal.Text = dr["FLDPRINCIPALNAME"].ToString();
            txtHullNo.Text = dr["FLDHULLNUMBER"].ToString();
            txtKeelLaid.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDDATEENTERED"]);
            txtLaunched.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDDATELEFT"]);
            txtDelivery.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDORGDATEENTERED"]);
            txtESMTakeover.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDORGDATELEFT"]);
            txtNavigationArea.Text = dr["FLDNAVIGATIONAREA"].ToString();
            txtClassNo.Text = dr["FLDCLASSNO"].ToString();
            txtBuilder.Text = dr["FLDYARDNAME"].ToString();
            txtFittedwithFramo.Text = dr["FLDFITWITHFRAMOYN"].ToString().Equals("1") ? "Yes" : "No";
            txtESMHandoverDate.Text = string.Format("{0:dd/MM/yyyy}", dr["FLDESMHANDOVERDATE"]);
            txtManagementType.Text = dr["FLDMANAGEMENTTYPENAME"].ToString();

            txtLoa.Text = dr["FLDLOA"].ToString();
            txtLBP.Text = dr["FLDLBP"].ToString();
            txtSpeed.Text = dr["FLDSPEED"].ToString();
            txtBreath.Text = dr["FLDBREADTH"].ToString();
            txtDepth.Text = dr["FLDDEPTH"].ToString();
            txtHeight.Text = dr["FLDHEIGHT"].ToString();

            txtRegisteredGT.Text = String.Format("{0:###,###}", General.GetNullableInteger(dr["FLDREGISTEREDGT"].ToString()));
            txtSuezGT.Text = dr["FLDSUEZGT"].ToString();
            txtPanamaGT.Text = dr["FLDPANAMAGT"].ToString();
            txtRegisteredNT.Text = dr["FLDREGISTEREDNT"].ToString();
            txtSuezNT.Text = dr["FLDSUEZNT"].ToString();
            txtPanamaNT.Text = dr["FLDPANAMANT"].ToString();

            txtFreeboardTropical.Text = dr["FLDFREEBOARDTROPICAL"].ToString();
            txtDraftTropical.Text = dr["FLDDRAFTTROPICAL"].ToString();
            txtDWTTropical.Text = dr["FLDDWTTROPICAL"].ToString();
            txtFreeboardSummer.Text = dr["FLDFREEBOARDSUMMER"].ToString();
            txtDraftSummer.Text = dr["FLDDRAFTSUMMER"].ToString();
            txtDWTSummer.Text = dr["FLDDWTSUMMER"].ToString();
            txtFreeboardWinter.Text = dr["FLDFREEBOARDWINTER"].ToString();
            txtDraftWinter.Text = dr["FLDDRAFTWINTER"].ToString();
            txtDWTWinter.Text = dr["FLDDWTWINTER"].ToString();
            txtFreeboardLightship.Text = dr["FLDFREEBOARDLIGHTSHIP"].ToString();
            txtDraftLightship.Text = dr["FLDDRAFTLIGHTSHIP"].ToString();
            txtDWTLightship.Text = dr["FLDDWTLIGHTSHIP"].ToString();
            txtFreeboardBallastCond.Text = dr["FLDFREEBOARDBALLASTCOND"].ToString();
            txtDraftBallastCond.Text = dr["FLDDRAFTBALLASTCOND"].ToString();
            txtDWTBallastCond.Text = dr["FLDDWTBALLASTCOND"].ToString();

            txtEngineType.Text = dr["FLDENGINENAME"].ToString();
            txtEngineModel.Text = dr["FLDENGINEMODEL"].ToString();

            txtMainEngine.Text = dr["FLDMAINENGINE"].ToString();
            txtMCR.Text = dr["FLDMCR"].ToString();
            txtAuxEngine.Text = dr["FLDAUXENGINE"].ToString();
            txtAuxBoiler.Text = dr["FLDAUXBOILER"].ToString();
            txtKW.Text = dr["FLDKW"].ToString();
            txtBhp.Text = dr["FLDBHP"].ToString();

            txtLifeBoatQuantity.Text = dr["FLDLIFEBOATQUANTITY"].ToString();
            txtLifeBoatCapacity.Text = dr["FLDLIFEBOATCAPACITY"].ToString();
            txtSecCapacity.Text = dr["FLDSECCAPACITY"].ToString();

            txtRemarks.Text = dr["FLDREMARKS"].ToString();

            txtAPhone.Text = dr["FLDAPHONE"].ToString();
            txtAFax.Text = dr["FLDAFAX"].ToString();
            txtBPhone.Text = dr["FLDBPHONE"].ToString();
            txtBFax.Text = dr["FLDBFAX"].ToString();
            txtEmail.Text = dr["FLDEMAIL"].ToString();
            txtNotificationEmail.Text = dr["FLDNOTIFICATIONEMAIL"].ToString();
            txtAccInchargeEmail.Text = dr["FLDACCOUNTINCHARGEEMAIL"].ToString();

            txtPhone.Text = dr["FLDPHONE"].ToString();
            txtFax.Text = dr["FLDFAX"].ToString();
            txtMobileNumber.Text = dr["FLDMOBILENUMBER"].ToString();
            txtFPhone.Text = dr["FLDFPHONE"].ToString();
            txtFFax.Text = dr["FLDFFAX"].ToString();
            txtCTalex.Text = dr["FLDCTALEX"].ToString();
            lblLatitude.Text = dr["FLDLATITUDE"].ToString();
            lblLongitude.Text = dr["FLDLONGITUDE"].ToString();
            imgPhoto.ImageUrl = "../css/" + PhoenixGeneralSettings.CurrentGeneralSetting.Theme + "/images/vessel/" + ds.Tables[0].Rows[0]["FLDIMONUMBER"].ToString() + ".png";
        }

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(int.Parse(ViewState["VESSELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
            ViewState["FLDDTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";
    }
}
