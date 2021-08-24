using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;

public partial class OwnersVessel : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        imgViewBillingParties.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', 'OwnersMoreInfoBillingParties.aspx?ownerid=" + ucAddrOwner.SelectedAddress + "', 'small')");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        ucAddrOwner.AddressType = ((int)PhoenixAddressType.OWNER).ToString();
        ucAddrPrimaryManager.AddressType = ((int)PhoenixAddressType.MANAGER).ToString();
        ucAddrYard.AddressType = ((int)PhoenixAddressType.YARD).ToString();
        ucCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
        ucClassName.AddressType = ((int)PhoenixAddressType.CLASSIFICATIONSOCIETY).ToString();
        ucDisponentOwner.AddressType = ((int)PhoenixAddressType.OWNER).ToString();
        ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();

        if (!IsPostBack)
        {
            //if (Request.QueryString["NewMode"] == null && Filter.CurrentOwnerVesselMasterFilter == null)
            //{
            //    Session["NEWMODE"] = 0;
            //}

            //if (Request.QueryString["NewMode"] != null)
            //{
            //    Filter.CurrentOwnerVesselMasterFilter = null;
            //    Session["NEWMODE"] = 1;

            //    string Script = "";
            //    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            //    Script += "parent.selectTab('MenuVesselMaster', 'Particulars');";
            //    Script += "</script>" + "\n";
            //    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            //}

            if (Filter.CurrentOwnerVesselMasterFilter != null)
            {
                EditVessel(Int16.Parse(Filter.CurrentOwnerVesselMasterFilter.ToString()));
                DisableControls();
            }
            else
            {
                Reset();
            }
        }
    }

    private void DisableControls()
    {
        txtVesselName.Enabled = false;
        txtVesselCode.Enabled = false;
        ucAddrOwner.Enabled = false;
        ucAddrPrimaryManager.Enabled = false;
        ucPrincipal.Enabled = false;
        ucFlag.Enabled = false;
    }

    protected void EditVessel(int vesselId)
    {
        DataSet ds = PhoenixOwnersVessel.EditVessel(vesselId);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucVesselType.SelectedVesseltype = dr["FLDTYPE"].ToString();
            ucFlag.SelectedFlag =  dr["FLDFLAG"].ToString();
            ucAddrOwner.SelectedAddress =  dr["FLDOWNER"].ToString();

            ucAddrPrimaryManager.SelectedAddress =  dr["FLDPRIMARYMANAGER"].ToString();
            ucAddrYard.SelectedAddress = dr["FLDYARD"].ToString();
            ucPortRegistered.SelectedSeaport = dr["FLDPORTREGISTERED"].ToString();
            ucDisponentOwner.SelectedAddress = dr["FLDDISPONENTOWNER"].ToString();
            ucPrincipal.SelectedAddress = dr["FLDPRINCIPAL"].ToString();

            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            txtVesselCode.Text = dr["FLDVESSELCODE"].ToString();
            txtCallSign.Text = dr["FLDCALLSIGN"].ToString();
            txtOfficialNumber.Text = dr["FLDOFFICIALNUMBER"].ToString();
            txtIMONumber.Text = dr["FLDIMONUMBER"].ToString();
            txtLifeBoatCapacity.Text = dr["FLDLIFEBOATCAPACITY"].ToString();
            txtHullNo.Text = dr["FLDHULLNUMBER"].ToString();
            txtNavigationArea.Text = dr["FLDNAVIGATIONAREA"].ToString();
            ucCharterer.SelectedAddress = dr["FLDCHARTERER"].ToString();
            txtClassNotation.Text = dr["FLDCLASSNOTATION"].ToString();
            txtMMSINo.Text = dr["FLDMMSINUMBER"].ToString();
            txtLoa.Text = dr["FLDLOA"].ToString();
            txtLBP.Text = dr["FLDLBP"].ToString();
            
            txtKeelLaidDate.Text = dr["FLDDATEENTERED"].ToString();
            txtLaunchedDate.Text = dr["FLDDATELEFT"].ToString();
            txtDeliveryDate.Text = dr["FLDORGDATEENTERED"].ToString();
            txtTakeoverDateByESM.Text = dr["FLDORGDATELEFT"].ToString();

            ucEngineType.SelectedEngineName = dr["FLDENGINETYPE"].ToString();
            txtEngineModel.Text = dr["FLDENGINEMODEL"].ToString();
            ucClassName.SelectedAddress = dr["FLDCLASSNAME"].ToString();

            txtSpeed.Text = dr["FLDSPEED"].ToString();
            txtBreath.Text = dr["FLDBREADTH"].ToString();
            txtDepth.Text = dr["FLDDEPTH"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();

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
            txtMainEngine.Text = dr["FLDMAINENGINE"].ToString();
            txtMCR.Text = dr["FLDMCR"].ToString();
            txtAuxEngine.Text = dr["FLDAUXENGINE"].ToString();
            txtAuxBoiler.Text = dr["FLDAUXBOILER"].ToString();
            txtLifeBoatQuantity.Text = dr["FLDLIFEBOATQUANTITY"].ToString();
            txtHeight.Text = dr["FLDHEIGHT"].ToString();
			txtClassNo.Text = dr["FLDCLASSNO"].ToString();

            ddlIceClassed.SelectedValue = dr["FLDICECLASSED"].ToString();
            ddlFittedwithFramo.SelectedValue = dr["FLDFITWITHFRAMOYN"].ToString();

            txtESMHandoverDate.Text = dr["FLDESMHANDOVERDATE"].ToString();
            txtBHP.Text = dr["FLDBHP"].ToString();
            txtKW.Text = dr["FLDKW"].ToString();           
        }
    }

    private void Reset()
    {
        Filter.CurrentOwnerVesselMasterFilter = null;

        ucVesselType.SelectedVesseltype = "";
        ucFlag.SelectedFlag = "";
        ucAddrOwner.SelectedAddress = "";

        ucAddrPrimaryManager.SelectedAddress = "";
        ucAddrYard.SelectedAddress = "";
        ucPortRegistered.SelectedSeaport = "";
        ucPrincipal.SelectedAddress = "";

        txtVesselName.Text = "";
        txtVesselCode.Text = "";
        txtCallSign.Text = "";
        txtOfficialNumber.Text = "";
        txtIMONumber.Text = "";
        txtLifeBoatCapacity.Text = "";
        txtHullNo.Text = "";
        txtNavigationArea.Text = "";
        ucCharterer.SelectedAddress = "";
        txtClassNotation.Text = "";
        txtMMSINo.Text = "";
        txtLoa.Text = "";
        txtLBP.Text = "";

        txtKeelLaidDate.Text = "";
        txtLaunchedDate.Text = "";
        txtDeliveryDate.Text = "";
        txtTakeoverDateByESM.Text = "";

        ucEngineType.SelectedEngineName = "";
        txtEngineModel.Text = "";
        ucClassName.SelectedAddress = "";

        txtSpeed.Text = "";
        txtBreath.Text = "";
        txtDepth.Text = "";
        txtRemarks.Text = "";

        ucDisponentOwner.SelectedAddress = "";
        txtRegisteredGT.Text = "";
        txtSuezGT.Text = "";
        txtPanamaGT.Text = "";
        txtRegisteredNT.Text = "";
        txtSuezNT.Text = "";
        txtPanamaNT.Text = "";
        txtFreeboardTropical.Text = "";
        txtDraftTropical.Text = "";
        txtDWTTropical.Text = "";
        txtFreeboardSummer.Text = "";
        txtDraftSummer.Text = "";
        txtDWTSummer.Text = "";
        txtFreeboardWinter.Text = "";
        txtDraftWinter.Text = "";
        txtDWTWinter.Text = "";
        txtFreeboardLightship.Text = "";
        txtDraftLightship.Text = "";
        txtDWTLightship.Text = "";
        txtFreeboardBallastCond.Text = "";
        txtDraftBallastCond.Text = "";
        txtDWTBallastCond.Text = "";
        txtMainEngine.Text = "";
        txtMCR.Text = "";
        txtAuxEngine.Text = "";
        txtAuxBoiler.Text = "";
        txtLifeBoatQuantity.Text = "";
        txtHeight.Text = "";
    }    

    protected void CalculateBHP(object sender, EventArgs e)
    {
        if (txtKW.Text != "")
        {
            Double bhp = (Convert.ToDouble(txtKW.Text)) * 1.341;
            txtBHP.Text = Convert.ToString(Math.Ceiling(bhp));
        }
    }

    protected void CalculateKW(object sender, EventArgs e)
    {
        if (txtBHP.Text != "")
        {
            Double kwt = (Convert.ToDouble(txtBHP.Text)) / 1.341;
            txtKW.Text = Convert.ToString(Math.Ceiling(kwt));
        }
    }  
}
