using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using OfficeOpenXml;
using Telerik.Web.UI;
using System.Web;

public partial class RegistersVessel : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        //imgViewBillingParties.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', 'RegistersMoreInfoBillingParties.aspx?ownerid=" + ucAddrOwner.SelectedAddress + "', 'small')");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        // toolbar.AddButton("Vessel Search", "VESSELSEARCH", ToolBarDirection.Right);
        toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

        toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
        toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
        //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
        toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
        //toolbar.AddButton("List", "LIST", ToolBarDirection.Right);

        MenuVesselList.AccessRights = this.ViewState;
        MenuVesselList.MenuList = toolbar.Show();

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            MenuVesselList.SelectedMenuIndex = 7;
        else
            MenuVesselList.SelectedMenuIndex = 6;
        //Controls.IndexOf();
        //MenuVesselList.FindControl("PARTICULARS");

        ucAddrOwner.AddressType = ((int)PhoenixAddressType.OWNER).ToString();
        ucAddrPrimaryManager.AddressType = ((int)PhoenixAddressType.MANAGER).ToString();
        ucAddrYard.AddressType = ((int)PhoenixAddressType.YARD).ToString();
        ucCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
        ucOffshoreCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
        ucClassName.AddressType = ((int)PhoenixAddressType.CLASSIFICATIONSOCIETY).ToString();
        ucDisponentOwner.AddressType = ((int)PhoenixAddressType.OWNER).ToString();
        ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();

        if (!IsPostBack)
        {
            //txtECDISMakerId.Attributes.Add("style", "visibility:hidden");
            try
            {
                ViewState["PARTICULARPATH"] = "";
                BindEntitytype();
                bindEGCSType();
                bindFuelComplianceMethod();

                if (Request.QueryString["Vesselid"] != null)
                {
                    Filter.CurrentVesselMasterFilter = Request.QueryString["Vesselid"].ToString();
                }

                if (Request.QueryString["NewMode"] == null)
                {
                    Session["NEWMODE"] = 0;

                    //Filter.CurrentVesselMasterFilter = null;
                }

                if (Request.QueryString["NewMode"] != null)
                {
                    //lnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/registers/VesselParticulars/DockingVesselParticulars.xlsx";
                    lnkParticulars.Visible = false;

                    MenuVesselList.Visible = false;

                    Filter.CurrentVesselMasterFilter = null;

                    Session["NEWMODE"] = 1;

                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "parent.selectTab('MenuVesselMaster', 'Particulars');";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
                if (Filter.CurrentVesselMasterFilter != null)
                {
                    EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
                    DisableControls();
                }
                else
                {
                    PhoenixToolbar toolbar1 = new PhoenixToolbar();
                    toolbar1.AddButton("Back", "BACK", ToolBarDirection.Right);
                    toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    MenuVessel.AccessRights = this.ViewState;
                    MenuVessel.MenuList = toolbar1.Show();

                    //lnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/registers/VesselParticulars/DockingVesselParticulars.xlsx";
                    lnkParticulars.Visible = false;
                    Reset();
                }

                BindIceClass();

                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                {
                    lblCharterer.Text = "Training Matrix Identifier";
                    ucOffshoreCharterer.Visible = true;
                    txtMatrixStandard.Visible = true;
                    lblOffshoreCharterer.Visible = true;
                    ucOffshoreCharterer.Visible = true;
                    ucPropulsion.Visible = true;
                    lblPropulsion.Visible = true;
                    lblESMTakeover.Text = "EO Takeover date";
                    lblESMHandoverdate.Text = "EO Handover date";
                    DPClass.Visible = true;
                    lblDPClass.Visible = true;
                    lblDPMakeandModel.Visible = true;
                    PMakeandModel.Visible = true;
                    lblVoltage.Visible = true;
                    Voltage.Visible = true;
                }
                else
                {
                    ucCharterer.OffshoreStandardyn = "0";
                    ucCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
                    //ucCharterer.DataBind();
                    ucCharterer.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ucError.Text = ex.Message;
                ucError.Visible = true;
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
        ucManagementType.Enabled = false;
    }


    protected void lnkfilename_Click(object sender, EventArgs e)
    {
        string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\Registers\\VesselParticulars\\DockingVesselParticulars.xlsx";
        string archivedpath = PhoenixGeneralSettings.CurrentGeneralSetting.ArchivedAttachmentPath + "\\Registers\\VesselParticulars\\DockingVesselParticulars.xlsx";

        if (File.Exists(strpath))
        {
            FileInfo newFile = new FileInfo(strpath);
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                HttpContext.Current.Response.Clear();
                pck.SaveAs(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=DockingVesselParticulars.xlsx");
                HttpContext.Current.Response.End();
            }
        }
        else if (File.Exists(archivedpath))
        {
            FileInfo newFile = new FileInfo(archivedpath);
            using (ExcelPackage pck = new ExcelPackage(newFile))
            {
                HttpContext.Current.Response.Clear();
                pck.SaveAs(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=DockingVesselParticulars.xlsx");
                HttpContext.Current.Response.End();
            }
        }

    }

    protected void lnkParticulars_Click(object sender, EventArgs e)
    {
        if (ViewState["PARTICULARPATH"] != null && ViewState["PARTICULARPATH"].ToString() != "")
        {
            string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\" + ViewState["PARTICULARPATH"].ToString();
            string archivedpath = PhoenixGeneralSettings.CurrentGeneralSetting.ArchivedAttachmentPath + "\\" + ViewState["PARTICULARPATH"].ToString();

            if (File.Exists(strpath))
            {
                FileInfo newFile = new FileInfo(strpath);
                using (ExcelPackage pck = new ExcelPackage(newFile))
                {
                    HttpContext.Current.Response.Clear();
                    pck.SaveAs(HttpContext.Current.Response.OutputStream);
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=DockingVesselParticulars.xlsx");
                    HttpContext.Current.Response.End();
                }
            }
            else if(File.Exists(archivedpath))
            {
                FileInfo newFile = new FileInfo(archivedpath);
                using (ExcelPackage pck = new ExcelPackage(newFile))
                {
                    HttpContext.Current.Response.Clear();
                    pck.SaveAs(HttpContext.Current.Response.OutputStream);
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=DockingVesselParticulars.xlsx");
                    HttpContext.Current.Response.End();
                }
            }
        }
    }

    protected void EditVessel(int vesselId)
    {
        DataSet ds = PhoenixRegistersVessel.EditVessel(vesselId);
        //lnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/registers/VesselParticulars/DockingVesselParticulars.xlsx";

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["FLDPARTICULARSPATH"].ToString() != string.Empty)
            {
                lnkParticulars.Visible = true;
                ViewState["PARTICULARPATH"] = dr["FLDPARTICULARSPATH"].ToString();
                //lnkParticulars.NavigateUrl = Session["sitepath"] + "/attachments/" + dr["FLDPARTICULARSPATH"].ToString();
            }
            else
                lnkParticulars.Visible = false;

            ucVesselType.SelectedVesseltype = dr["FLDTYPE"].ToString();
            ucFlag.SelectedFlag = dr["FLDFLAG"].ToString();
            ucAddrOwner.SelectedValue = dr["FLDOWNER"].ToString();
            ucAddrOwner.Text = dr["FLDOWNERNAME"].ToString();

            ucManagementType.SelectedHard = dr["FLDMANAGEMENT"].ToString();

            ucAddrPrimaryManager.SelectedValue = dr["FLDPRIMARYMANAGER"].ToString();
            ucAddrPrimaryManager.Text = dr["FLDPRIMARYMANAGERNAME"].ToString();

            ucAddrYard.SelectedValue = dr["FLDYARD"].ToString();
            ucAddrYard.Text = dr["FLDYARDNAME"].ToString();

            ucPortRegistered.SelectedValue = dr["FLDPORTREGISTERED"].ToString();
            ucPortRegistered.Text = dr["FLDREGISTEREDPORTNAME"].ToString();
            ucDisponentOwner.SelectedValue = dr["FLDDISPONENTOWNER"].ToString();
            ucDisponentOwner.Text = dr["FLDDISPONENTOWNERNAME"].ToString();

            ucPrincipal.SelectedValue = dr["FLDPRINCIPAL"].ToString();
            ucPrincipal.Text = dr["FLDPRINCIPALNAME"].ToString();

            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            txtVesselCode.Text = dr["FLDVESSELCODE"].ToString();
            txtCallSign.Text = dr["FLDCALLSIGN"].ToString();
            txtOfficialNumber.Text = dr["FLDOFFICIALNUMBER"].ToString();
            txtIMONumber.Text = dr["FLDIMONUMBER"].ToString();
            txtLifeBoatCapacity.Text = dr["FLDLIFEBOATCAPACITY"].ToString();
            txtHullNo.Text = dr["FLDHULLNUMBER"].ToString();
            ucCharterer.SelectedValue = dr["FLDCHARTERER"].ToString();
            ucOffshoreCharterer.SelectedAddress = dr["FLDOFFSHORECHARTERER"].ToString();
            ucCharterer.Text = dr["FLDCHARTERERNAME"].ToString();
            ucNavigationArea.SelectedText = dr["FLDNAVIGATIONAREA"].ToString();

            txtClassNotation.Text = dr["FLDCLASSNOTATION"].ToString();
            txtMMSINo.Text = dr["FLDMMSINUMBER"].ToString();
            txtLoa.Text = dr["FLDLOA"].ToString();
            txtLBP.Text = dr["FLDLBP"].ToString();

            txtKeelLaidDate.Text = dr["FLDDATEENTERED"].ToString();
            txtLaunchedDate.Text = dr["FLDDATELEFT"].ToString();
            txtDeliveryDate.Text = dr["FLDORGDATEENTERED"].ToString();
            txtTakeoverDateByESM.Text = dr["FLDORGDATELEFT"].ToString();

            ucEngineType.SelectedEngineName = dr["FLDENGINETYPE"].ToString();
            //txtEngineModel.Text = dr["FLDENGINEMODEL"].ToString();
            ucClassName.SelectedValue = dr["FLDCLASSNAME"].ToString();
            ucClassName.Text = dr["FLDCLASSNAMEVALUE"].ToString();

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
            txtMatrixStandard.Text = dr["FLDOFFSHORECHARTERERSTANDARD"].ToString() == null || dr["FLDOFFSHORECHARTERERSTANDARD"].ToString() == "" ? dr["FLDOFFSHORECHARTERERNAME"].ToString() : dr["FLDOFFSHORECHARTERERSTANDARDNAME"].ToString();
            lblMatrixStandardId.Text = dr["FLDOFFSHORECHARTERERSTANDARD"].ToString() == null || dr["FLDOFFSHORECHARTERERSTANDARD"].ToString() == "" ? dr["FLDOFFSHORECHARTERER"].ToString() : dr["FLDOFFSHORECHARTERERSTANDARD"].ToString();
            if (General.GetNullableInteger(dr["FLDICECLASSED"].ToString()) != null)
                ddlIceClassed.SelectedValue = dr["FLDICECLASSED"].ToString();
            ddlFittedwithFramo.SelectedValue = dr["FLDFITWITHFRAMOYN"].ToString();

            txtESMHandoverDate.Text = dr["FLDESMHANDOVERDATE"].ToString();
            txtBHP.Text = dr["FLDBHP"].ToString();
            txtKW.Text = dr["FLDKW"].ToString();
            txtSecCapacity.Text = dr["FLDSECCAPACITY"].ToString();

            txtNoOfECDIS.Text = dr["FLDNOOFECDIS"].ToString();
            txtECDISMaker.SelectedValue = dr["FLDECDISMAKER"].ToString();
            //txtECDISMakerCode.Text = dr["FLDECDISMAKERCODE"].ToString();
            txtECDISMaker.Text = dr["FLDECDISMAKERNAME"].ToString();
            txtECDISModel.Text = dr["FLDECDISMODEL"].ToString();

            ucAnniversaryDate.Text = dr["FLDANNIVERSARYDATE"].ToString();

            ucPropulsion.SelectedPropulsion = dr["FLDPROPULSIONID"].ToString();
            Voltage.Text = dr["FLDVOLTAGE"].ToString();
            PMakeandModel.Text = dr["FLDCOMPONENTNAME"].ToString();
            DPClass.SelectedDPClass = dr["FLDDPCLASS"].ToString();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersVesselParticularsPrint.aspx')", "Print", "", "PRINT", ToolBarDirection.Right);
            toolbar.AddImageLink("javascript:parent.openNewWindow('NAFA','File Attachment','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + dr["FLDDTKEY"].ToString() + "&mod="
                                + PhoenixModule.REGISTERS + "'); return false;", "Attachment", "", "ATTACHMENT", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuVessel.MenuList = toolbar.Show();

            txtMEQuantity.Text = dr["FLDMEQUANTITY"].ToString();
            txtAEQuantity.Text = dr["FLDAEQUANTITY"].ToString();
            txtABQuantity.Text = dr["FLDABQUANTITY"].ToString();
            txtTOBQuantity.Text = dr["FLDTOBQUANTITY"].ToString();
            txtIGGQuantity.Text = dr["FLDIGGQUANTITY"].ToString();
            ucAEEngineType.SelectedEngineName = dr["FLDAEENGINETYPE"].ToString();
            txtTOBModel.Text = dr["FLDTOBMODEL"].ToString();
            txtIGGModel.Text = dr["FLDIGGMODEL"].ToString();
            txtAEKW.Text = dr["FLDAEKW"].ToString();
            txtMESFOC.Text = dr["FLDMESFOC"].ToString();
            txtAESFOC.Text = dr["FLDAESFOC"].ToString();

            txtMEMaker.SelectedValue = dr["FLDMEMAKER"].ToString();
            txtMEMaker.Text = dr["FLDMEMAKERNAME"].ToString();

            txtAEMaker.SelectedValue = dr["FLDAEMAKER"].ToString();
            txtAEMaker.Text = dr["FLDAEMAKERNAME"].ToString();

            txtABMaker.SelectedValue = dr["FLDABMAKER"].ToString();
            txtABMaker.Text = dr["FLDABMAKERNAME"].ToString();

            txtTOBMaker.SelectedValue = dr["FLDTOBMAKER"].ToString();
            txtTOBMaker.Text = dr["FLDTOBMAKERNAME"].ToString();

            txtIGGMaker.SelectedValue = dr["FLDIGGMAKER"].ToString();
            txtIGGMaker.Text = dr["FLDIGGMAKERNAME"].ToString();

            txtEEDI.Text = dr["FLDEEDI"].ToString();
            txtEIV.Text = dr["FLDEIV"].ToString();
            ddlIceClass.SelectedValue = dr["FLDICECLASS"].ToString();

            txtPropDiameter.Text = dr["FLDPROPDIAMETER"].ToString();
            txtPropPitch.Text = dr["FLDPROPPITCH"].ToString();
            txtMCRRPM.Text = dr["FLDMCRRPM"].ToString();
            txtMCRgbhph.Text = dr["FLDMCRGBHPH"].ToString();
            txtMCRFOCons.Text = dr["FLDMCRFOCONS"].ToString();
            txtNCRBHP.Text = dr["FLDNCRBHP"].ToString();
            txtNCRkW.Text = dr["FLDNCRKW"].ToString();
            txtNCRRPM.Text = dr["FLDNCRRPM"].ToString();
            txtNCRgbhph.Text = dr["FLDNCRGBHPH"].ToString();
            txtNCRgkWh.Text = dr["FLDNCRGKWH"].ToString();
            txtAERPM.Text = dr["FLDAERPM"].ToString();
            txtAEFOCons.Text = dr["FLDAEFOCONS"].ToString();
            txtDisplacement.Text = dr["FLDDISPLACEMENT"].ToString();
            txtServiceBHP.Text = dr["FLDSERVICEBHP"].ToString();
            txtNCRSpeed.Text = dr["FLDNCRSPEED"].ToString();
            txtSeaTrialNCRBHP.Text = dr["FLDSEATRIALNCRBHP"].ToString();
            txtPowerkWAE1.Text = dr["FLDPOWERKWAE1"].ToString();
            txtPowerkWAE2.Text = dr["FLDPOWERKWAE2"].ToString();
            txtPowerkWAE3.Text = dr["FLDPOWERKWAE3"].ToString();
            txtPowerkWAE4.Text = dr["FLDPOWERKWAE4"].ToString();
            txtRPMAE1.Text = dr["FLDRPMAE1"].ToString();
            txtRPMAE2.Text = dr["FLDRPMAE2"].ToString();
            txtRPMAE3.Text = dr["FLDRPMAE3"].ToString();
            txtRPMAE4.Text = dr["FLDRPMAE4"].ToString();
            txtSFOCAE1.Text = dr["FLDSFOCAE1"].ToString();
            txtSFOCAE2.Text = dr["FLDSFOCAE2"].ToString();
            txtSFOCAE3.Text = dr["FLDSFOCAE3"].ToString();
            txtSFOCAE4.Text = dr["FLDSFOCAE4"].ToString();
            txtFOConsAE1.Text = dr["FLDFOCONSAE1"].ToString();
            txtFOConsAE2.Text = dr["FLDFOCONSAE2"].ToString();
            txtFOConsAE3.Text = dr["FLDFOCONSAE3"].ToString();
            txtFOConsAE4.Text = dr["FLDFOCONSAE4"].ToString();
            txtNCRFOCons.Text = dr["FLDNCRFOCONS"].ToString();
            txtCEMaker.SelectedValue = dr["FLDCEMAKER"].ToString();
            txtCEMaker.Text = dr["FLDCEMAKERNAME"].ToString();
            txtCEModel.Text = dr["FLDCEMODEL"].ToString();
            txtCEQuantity.Text = dr["FLDCEQUANTITY"].ToString();

            txtABsfoc.Text = dr["FLDABSFOC"].ToString();
            txtTOBsfoc.Text = dr["FLDTOBSFOC"].ToString();
            txtIGGsfoc.Text = dr["FLDIGGSFOC"].ToString();
            txtPMECkw.Text = dr["FLDPMECKW"].ToString();
            txtPMECrpm.Text = dr["FLDPMECRPM"].ToString();
            txtPMECgbhph.Text = dr["FLDPMECGBHPH"].ToString();
            txtPMECgkwh.Text = dr["FLDPMECSFOC"].ToString();

            ucPMECEngineType.SelectedEngineName = dr["FLDPMECENGINETYPE"].ToString();
            txtPMECFOCons.Text = dr["FLDPMECFOCONS"].ToString();
            txtABFOCons.Text = dr["FLDABFOCONS"].ToString();
            txtTOBFOCons.Text = dr["FLDTOBFOCONS"].ToString();
            txtIGGFOCons.Text = dr["FLDIGGFOCONS"].ToString();

            txtABkW.Text = dr["FLDABKW"].ToString();
            txtTOBkW.Text = dr["FLDTOBKW"].ToString();
            txtIGGkW.Text = dr["FLDIGGKW"].ToString();
            if (General.GetNullableInteger(dr["FLDTIER"].ToString()) != null)
                ddlTier.SelectedValue = dr["FLDTIER"].ToString();

            if (General.GetNullableInteger(dr["FLDENTITYTYPE"].ToString()) != null)
                ddlentitytype.SelectedValue = dr["FLDENTITYTYPE"].ToString();

            txtTEU.Text = dr["FLDVESSELTEU"].ToString();

            chkEGCS.Checked = General.GetNullableInteger(dr["FLDEGCSINSTALLED"].ToString()) == 1 ? true : false;
            rdlEGCS.SelectedValue = General.GetNullableInteger(dr["FLDEGCSTYPEID"].ToString()) != null && General.GetNullableInteger(dr["FLDEGCSTYPEID"].ToString()) != 0 ? dr["FLDEGCSTYPEID"].ToString() : "0";
            ddlFuelComplianceMethod.SelectedValue = General.GetNullableInteger(dr["FLDFUELCOMPLIANCEMETHOD"].ToString()) == null ? "Dummy" : dr["FLDFUELCOMPLIANCEMETHOD"].ToString();
            txtFuelPipingVolume.Text = dr["FLDFUELPIPINGVOLUME"].ToString();
            ucpscalert.Text = dr["FLDPSCALERT"].ToString();
            ucsplsurveylastdate.Text = dr["FLDSPECIALSURVEYHULLLASTDATE"].ToString();
            ucsplsurveynextdate.Text = dr["FLDSPECIALSURVEYHULLNEXTDATE"].ToString();
            ucdrydocksurveydate.Text = dr["FLDDRYDOCKSURVEYLASTDATE"].ToString();
            ucclassddwindownextdate.Text = dr["FLDCLASSDDNEXTDATE"].ToString();

            chkusvisa.Checked = dr["FLDUSVISAYN"].ToString() == "1" ? true : false;
            chkaustralianvisa.Checked = dr["FLDAUSTRALIANVISAYN"].ToString() == "1" ? true : false;

        }
        else
            lnkParticulars.Visible = false;

    }
    protected void OffshoreChartererStanderdbind(object sender, EventArgs e)
    {
        if (General.GetNullableString(ucOffshoreCharterer.SelectedAddress) == null)
        {
            txtMatrixStandard.Text = "";
            lblMatrixStandardId.Text = "";
        }
        else
        {
            DataSet ds = PhoenixCrewOffshoreTrainingMatrixStandard.EditmatrixStander(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int64.Parse(ucOffshoreCharterer.SelectedAddress));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtMatrixStandard.Text = dr["FLDADDRESSCODE"].ToString() == null || dr["FLDADDRESSCODE"].ToString() == "" ? dr["FLDCHARTERERNAME"].ToString() : dr["FLDNAME"].ToString();
                lblMatrixStandardId.Text = dr["FLDADDRESSCODE"].ToString() == null || dr["FLDADDRESSCODE"].ToString() == "" ? dr["FLDCHARTERERID"].ToString() : dr["FLDADDRESSCODE"].ToString();
            }
            else
            {
                txtMatrixStandard.Text = "";
                lblMatrixStandardId.Text = "";
            }
        }
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

    protected void Vessel_TabStripCommand(object sender, EventArgs e)
    {
        //DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Registers/RegistersVesselMaster.aspx");
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidVessel())
                {
                    if (Filter.CurrentVesselMasterFilter != null)
                    {
                        string filename = string.Empty;
                        if (FileUpload.PostedFile.ContentLength > 0)
                        {
                            filename = txtIMONumber.Text.Trim() + Path.GetExtension(FileUpload.FileName);
                            string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\Registers\\VesselParticulars\\";
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string strpath = path + filename;
                            FileUpload.PostedFile.SaveAs(strpath);
                            filename = "\\Registers\\VesselParticulars\\" + filename;
                        }

                        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
                        {
                            PhoenixRegistersVessel.VesselUpdate(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , Int16.Parse(Filter.CurrentVesselMasterFilter)
                                , Int16.Parse(ucVesselType.SelectedVesseltype.ToString())
                                , txtCallSign.Text
                                , txtOfficialNumber.Text
                                , txtIMONumber.Text
                                , General.GetNullableInteger(ucAddrYard.SelectedValue)
                                , General.GetNullableInteger(txtLifeBoatCapacity.Text)
                                , General.GetNullableInteger(ucPortRegistered.SelectedValue)
                                , General.GetNullableDateTime(txtKeelLaidDate.Text)
                                , General.GetNullableDateTime(txtLaunchedDate.Text)
                                , General.GetNullableDateTime(txtDeliveryDate.Text)
                                , General.GetNullableDateTime(txtTakeoverDateByESM.Text)
                                , General.GetNullableInteger(txtHullNo.Text)
                                , General.GetNullableInteger(txtMMSINo.Text)
                                , txtClassNotation.Text
                                , ucNavigationArea.SelectedText
                                , General.GetNullableInteger(ucCharterer.SelectedValue)
                                , General.GetNullableDecimal(txtLBP.Text)
                                , Int16.Parse(ucEngineType.SelectedEngineName.ToString())
                                , null
                                , General.GetNullableInteger(ucClassName.SelectedValue)
                                , General.GetNullableDecimal(txtSpeed.Text)
                                , General.GetNullableDecimal(txtBreath.Text)
                                , General.GetNullableDecimal(txtDepth.Text)
                                , General.GetNullableDecimal(txtLoa.Text)
                                , txtRemarks.Text
                                , General.GetNullableInteger(ucDisponentOwner.SelectedValue)
                                , General.GetNullableDecimal(txtRegisteredGT.Text)
                                , General.GetNullableDecimal(txtSuezGT.Text)
                                , General.GetNullableDecimal(txtPanamaGT.Text)
                                , General.GetNullableDecimal(txtRegisteredNT.Text)
                                , General.GetNullableDecimal(txtSuezNT.Text)
                                , General.GetNullableDecimal(txtPanamaNT.Text)
                                , General.GetNullableDecimal(txtFreeboardTropical.Text)
                                , General.GetNullableDecimal(txtDraftTropical.Text)
                                , General.GetNullableDecimal(txtDWTTropical.Text)
                                , General.GetNullableDecimal(txtFreeboardSummer.Text)
                                , General.GetNullableDecimal(txtDraftSummer.Text)
                                , General.GetNullableDecimal(txtDWTSummer.Text)
                                , General.GetNullableDecimal(txtFreeboardWinter.Text)
                                , General.GetNullableDecimal(txtDraftWinter.Text)
                                , General.GetNullableDecimal(txtDWTWinter.Text)
                                , General.GetNullableDecimal(txtFreeboardLightship.Text)
                                , General.GetNullableDecimal(txtDraftLightship.Text)
                                , General.GetNullableDecimal(txtDWTLightship.Text)
                                , General.GetNullableDecimal(txtFreeboardBallastCond.Text)
                                , General.GetNullableDecimal(txtDraftBallastCond.Text)
                                , General.GetNullableDecimal(txtDWTBallastCond.Text)
                                , txtMainEngine.Text
                                , General.GetNullableDecimal(txtMCR.Text)
                                , txtAuxEngine.Text
                                , txtAuxBoiler.Text
                                , General.GetNullableInteger(txtLifeBoatQuantity.Text)
                                , General.GetNullableDecimal(txtHeight.Text)
                                , General.GetNullableInteger(ddlIceClassed.SelectedValue)
                                , General.GetNullableInteger(ddlFittedwithFramo.SelectedValue)
                                , General.GetNullableString(txtClassNo.Text)
                                , General.GetNullableDecimal(txtKW.Text)
                                , General.GetNullableDecimal(txtBHP.Text)
                                , General.GetNullableDecimal(txtSecCapacity.Text)
                                , General.GetNullableInteger(txtNoOfECDIS.Text)
                                , General.GetNullableInteger(txtECDISMaker.SelectedValue)
                                , txtECDISModel.Text
                                , filename
                                , General.GetNullableDateTime(ucAnniversaryDate.Text)
                                , null, null, null, null
                                , General.GetNullableInteger(txtMEQuantity.Text)
                                , General.GetNullableInteger(txtAEQuantity.Text)
                                , General.GetNullableInteger(txtABQuantity.Text)
                                , General.GetNullableInteger(txtTOBQuantity.Text)
                                , General.GetNullableInteger(txtIGGQuantity.Text)
                                , General.GetNullableInteger(ucAEEngineType.SelectedEngineName)
                                , General.GetNullableInteger(txtMEMaker.SelectedValue)
                                , General.GetNullableInteger(txtAEMaker.SelectedValue)
                                , General.GetNullableInteger(txtABMaker.SelectedValue)
                                , General.GetNullableInteger(txtTOBMaker.SelectedValue)
                                , General.GetNullableInteger(txtIGGMaker.SelectedValue)
                                , General.GetNullableString(txtTOBModel.Text)
                                , General.GetNullableString(txtIGGModel.Text)
                                , General.GetNullableDecimal(txtAEKW.Text)
                                , General.GetNullableDecimal(txtMESFOC.Text)
                                , General.GetNullableDecimal(txtAESFOC.Text)
                                , General.GetNullableDecimal(txtEEDI.Text)
                                , General.GetNullableDecimal(txtEIV.Text)
                                , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                , General.GetNullableDecimal(txtPropDiameter.Text)
                                , General.GetNullableDecimal(txtPropPitch.Text)
                                , General.GetNullableInteger(txtMCRRPM.Text)
                                , General.GetNullableDecimal(txtMCRgbhph.Text)
                                , General.GetNullableDecimal(txtMCRFOCons.Text)
                                , General.GetNullableInteger(txtNCRBHP.Text)
                                , General.GetNullableInteger(txtNCRkW.Text)
                                , General.GetNullableInteger(txtNCRRPM.Text)
                                , General.GetNullableDecimal(txtNCRgbhph.Text)
                                , General.GetNullableDecimal(txtNCRgkWh.Text)
                                , General.GetNullableInteger(txtAERPM.Text)
                                , General.GetNullableDecimal(txtAEFOCons.Text)
                                , General.GetNullableInteger(txtDisplacement.Text)
                                , General.GetNullableInteger(txtServiceBHP.Text)
                                , General.GetNullableDecimal(txtNCRSpeed.Text)
                                , General.GetNullableInteger(txtSeaTrialNCRBHP.Text)
                                , General.GetNullableDecimal(txtPowerkWAE1.Text)
                                , General.GetNullableDecimal(txtPowerkWAE2.Text)
                                , General.GetNullableDecimal(txtPowerkWAE3.Text)
                                , General.GetNullableDecimal(txtPowerkWAE4.Text)
                                , General.GetNullableInteger(txtRPMAE1.Text)
                                , General.GetNullableInteger(txtRPMAE2.Text)
                                , General.GetNullableInteger(txtRPMAE3.Text)
                                , General.GetNullableInteger(txtRPMAE4.Text)
                                , General.GetNullableDecimal(txtSFOCAE1.Text)
                                , General.GetNullableDecimal(txtSFOCAE2.Text)
                                , General.GetNullableDecimal(txtSFOCAE3.Text)
                                , General.GetNullableDecimal(txtSFOCAE4.Text)
                                , General.GetNullableDecimal(txtFOConsAE1.Text)
                                , General.GetNullableDecimal(txtFOConsAE2.Text)
                                , General.GetNullableDecimal(txtFOConsAE3.Text)
                                , General.GetNullableDecimal(txtFOConsAE4.Text)
                                , General.GetNullableDecimal(txtNCRFOCons.Text)
                                , General.GetNullableInteger(txtCEQuantity.Text)
                                , General.GetNullableInteger(txtCEMaker.SelectedValue)
                                , General.GetNullableString(txtCEModel.Text)
                                , General.GetNullableDecimal(txtABsfoc.Text)
                                , General.GetNullableDecimal(txtTOBsfoc.Text)
                                , General.GetNullableDecimal(txtIGGsfoc.Text)
                                , General.GetNullableInteger(txtPMECkw.Text)
                                , General.GetNullableInteger(txtPMECrpm.Text)
                                , General.GetNullableDecimal(txtPMECgbhph.Text)
                                , General.GetNullableDecimal(txtPMECgkwh.Text)
                                , General.GetNullableInteger(ucPMECEngineType.SelectedEngineName)
                                , General.GetNullableDecimal(txtPMECFOCons.Text)
                                , General.GetNullableDecimal(txtABFOCons.Text)
                                , General.GetNullableDecimal(txtTOBFOCons.Text)
                                , General.GetNullableDecimal(txtIGGFOCons.Text)
                                , General.GetNullableInteger(txtABkW.Text)
                                , General.GetNullableInteger(txtTOBkW.Text)
                                , General.GetNullableInteger(txtIGGkW.Text)
                                , General.GetNullableInteger(ddlTier.SelectedValue)
                                , General.GetNullableInteger(txtTEU.Text)
                                , General.GetNullableInteger(ddlentitytype.SelectedValue)
                                , chkEGCS.Checked == true ? 1 : 0
                                , General.GetNullableInteger(rdlEGCS.SelectedIndex == -1 ? null : rdlEGCS.SelectedValue)
                                , General.GetNullableInteger(ddlFuelComplianceMethod.SelectedValue)
                                , General.GetNullableDecimal(txtFuelPipingVolume.Text)
                                , General.GetNullableInteger(ucpscalert.Text)
                                , General.GetNullableDateTime(ucsplsurveylastdate.Text)
                                , General.GetNullableDateTime(ucsplsurveynextdate.Text)
                                , General.GetNullableDateTime(ucdrydocksurveydate.Text)
                                , General.GetNullableDateTime(ucclassddwindownextdate.Text)
                                , General.GetNullableInteger(chkusvisa.Checked == true ? "1" : "0")
                                , General.GetNullableInteger(chkaustralianvisa.Checked == true ? "1" : "0")
                                );
                            EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
                            ucStatus.Text = "Vessel Information is updated.";
                            ucStatus.Visible = true;
                        }
                        else
                        {
                            PhoenixRegistersVessel.VesselUpdate(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , Int16.Parse(Filter.CurrentVesselMasterFilter)
                                , Int16.Parse(ucVesselType.SelectedVesseltype.ToString())
                                , txtCallSign.Text
                                , txtOfficialNumber.Text
                                , txtIMONumber.Text
                                , General.GetNullableInteger(ucAddrYard.SelectedValue)
                                , General.GetNullableInteger(txtLifeBoatCapacity.Text)
                                , General.GetNullableInteger(ucPortRegistered.SelectedValue)
                                , General.GetNullableDateTime(txtKeelLaidDate.Text)
                                , General.GetNullableDateTime(txtLaunchedDate.Text)
                                , General.GetNullableDateTime(txtDeliveryDate.Text)
                                , General.GetNullableDateTime(txtTakeoverDateByESM.Text)
                                , General.GetNullableInteger(txtHullNo.Text)
                                , General.GetNullableInteger(txtMMSINo.Text)
                                , txtClassNotation.Text
                                , ucNavigationArea.SelectedText
                                , General.GetNullableInteger(lblMatrixStandardId.Text)              //OFFSHORE
                                , General.GetNullableDecimal(txtLBP.Text)
                                , Int16.Parse(ucEngineType.SelectedEngineName.ToString())
                                , null
                                , General.GetNullableInteger(ucClassName.SelectedValue)
                                , General.GetNullableDecimal(txtSpeed.Text)
                                , General.GetNullableDecimal(txtBreath.Text)
                                , General.GetNullableDecimal(txtDepth.Text)
                                , General.GetNullableDecimal(txtLoa.Text)
                                , txtRemarks.Text
                                , General.GetNullableInteger(ucDisponentOwner.SelectedValue)
                                , General.GetNullableDecimal(txtRegisteredGT.Text)
                                , General.GetNullableDecimal(txtSuezGT.Text)
                                , General.GetNullableDecimal(txtPanamaGT.Text)
                                , General.GetNullableDecimal(txtRegisteredNT.Text)
                                , General.GetNullableDecimal(txtSuezNT.Text)
                                , General.GetNullableDecimal(txtPanamaNT.Text)
                                , General.GetNullableDecimal(txtFreeboardTropical.Text)
                                , General.GetNullableDecimal(txtDraftTropical.Text)
                                , General.GetNullableDecimal(txtDWTTropical.Text)
                                , General.GetNullableDecimal(txtFreeboardSummer.Text)
                                , General.GetNullableDecimal(txtDraftSummer.Text)
                                , General.GetNullableDecimal(txtDWTSummer.Text)
                                , General.GetNullableDecimal(txtFreeboardWinter.Text)
                                , General.GetNullableDecimal(txtDraftWinter.Text)
                                , General.GetNullableDecimal(txtDWTWinter.Text)
                                , General.GetNullableDecimal(txtFreeboardLightship.Text)
                                , General.GetNullableDecimal(txtDraftLightship.Text)
                                , General.GetNullableDecimal(txtDWTLightship.Text)
                                , General.GetNullableDecimal(txtFreeboardBallastCond.Text)
                                , General.GetNullableDecimal(txtDraftBallastCond.Text)
                                , General.GetNullableDecimal(txtDWTBallastCond.Text)
                                , txtMainEngine.Text
                                , General.GetNullableDecimal(txtMCR.Text)
                                , txtAuxEngine.Text
                                , txtAuxBoiler.Text
                                , General.GetNullableInteger(txtLifeBoatQuantity.Text)
                                , General.GetNullableDecimal(txtHeight.Text)
                                , General.GetNullableInteger(ddlIceClassed.SelectedValue)
                                , General.GetNullableInteger(ddlFittedwithFramo.SelectedValue)
                                , General.GetNullableString(txtClassNo.Text)
                                , General.GetNullableDecimal(txtKW.Text)
                                , General.GetNullableDecimal(txtBHP.Text)
                                , General.GetNullableDecimal(txtSecCapacity.Text)
                                , General.GetNullableInteger(txtNoOfECDIS.Text)
                                , General.GetNullableInteger(txtECDISMaker.SelectedValue)
                                , txtECDISModel.Text
                                , filename
                                , General.GetNullableDateTime(ucAnniversaryDate.Text)
                                , General.GetNullableInteger(ucOffshoreCharterer.SelectedAddress)
                                , General.GetNullableInteger(ucPropulsion.SelectedPropulsion)
                                , General.GetNullableInteger(Voltage.Text)
                                , General.GetNullableInteger(DPClass.SelectedDPClass)
                                , General.GetNullableInteger(txtMEQuantity.Text)
                                , General.GetNullableInteger(txtAEQuantity.Text)
                                , General.GetNullableInteger(txtABQuantity.Text)
                                , General.GetNullableInteger(txtTOBQuantity.Text)
                                , General.GetNullableInteger(txtIGGQuantity.Text)
                                , General.GetNullableInteger(ucAEEngineType.SelectedEngineName)
                                , General.GetNullableInteger(txtMEMaker.SelectedValue)
                                , General.GetNullableInteger(txtAEMaker.SelectedValue)
                                , General.GetNullableInteger(txtABMaker.SelectedValue)
                                , General.GetNullableInteger(txtTOBMaker.SelectedValue)
                                , General.GetNullableInteger(txtIGGMaker.SelectedValue)
                                , General.GetNullableString(txtTOBModel.Text)
                                , General.GetNullableString(txtIGGModel.Text)
                                , General.GetNullableDecimal(txtAEKW.Text)
                                , General.GetNullableDecimal(txtMESFOC.Text)
                                , General.GetNullableDecimal(txtAESFOC.Text)
                                , General.GetNullableDecimal(txtEEDI.Text)
                                , General.GetNullableDecimal(txtEIV.Text)
                                , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                , General.GetNullableDecimal(txtPropDiameter.Text)
                                , General.GetNullableDecimal(txtPropPitch.Text)
                                , General.GetNullableInteger(txtMCRRPM.Text)
                                , General.GetNullableDecimal(txtMCRgbhph.Text)
                                , General.GetNullableDecimal(txtMCRFOCons.Text)
                                , General.GetNullableInteger(txtNCRBHP.Text)
                                , General.GetNullableInteger(txtNCRkW.Text)
                                , General.GetNullableInteger(txtNCRRPM.Text)
                                , General.GetNullableDecimal(txtNCRgbhph.Text)
                                , General.GetNullableDecimal(txtNCRgkWh.Text)
                                , General.GetNullableInteger(txtAERPM.Text)
                                , General.GetNullableDecimal(txtAEFOCons.Text)
                                , General.GetNullableInteger(txtDisplacement.Text)
                                , General.GetNullableInteger(txtServiceBHP.Text)
                                , General.GetNullableDecimal(txtNCRSpeed.Text)
                                , General.GetNullableInteger(txtSeaTrialNCRBHP.Text)
                                , General.GetNullableDecimal(txtPowerkWAE1.Text)
                                , General.GetNullableDecimal(txtPowerkWAE2.Text)
                                , General.GetNullableDecimal(txtPowerkWAE3.Text)
                                , General.GetNullableDecimal(txtPowerkWAE4.Text)
                                , General.GetNullableInteger(txtRPMAE1.Text)
                                , General.GetNullableInteger(txtRPMAE2.Text)
                                , General.GetNullableInteger(txtRPMAE3.Text)
                                , General.GetNullableInteger(txtRPMAE4.Text)
                                , General.GetNullableDecimal(txtSFOCAE1.Text)
                                , General.GetNullableDecimal(txtSFOCAE2.Text)
                                , General.GetNullableDecimal(txtSFOCAE3.Text)
                                , General.GetNullableDecimal(txtSFOCAE4.Text)
                                , General.GetNullableDecimal(txtFOConsAE1.Text)
                                , General.GetNullableDecimal(txtFOConsAE2.Text)
                                , General.GetNullableDecimal(txtFOConsAE3.Text)
                                , General.GetNullableDecimal(txtFOConsAE4.Text)
                                , General.GetNullableDecimal(txtNCRFOCons.Text)
                                , General.GetNullableInteger(txtCEQuantity.Text)
                                , General.GetNullableInteger(txtCEMaker.SelectedValue)
                                , General.GetNullableString(txtCEModel.Text)
                                , General.GetNullableDecimal(txtABsfoc.Text)
                                , General.GetNullableDecimal(txtTOBsfoc.Text)
                                , General.GetNullableDecimal(txtIGGsfoc.Text)
                                , General.GetNullableInteger(txtPMECkw.Text)
                                , General.GetNullableInteger(txtPMECrpm.Text)
                                , General.GetNullableDecimal(txtPMECgbhph.Text)
                                , General.GetNullableDecimal(txtPMECgkwh.Text)
                                , General.GetNullableInteger(ucPMECEngineType.SelectedEngineName)
                                , General.GetNullableDecimal(txtPMECFOCons.Text)
                                , General.GetNullableDecimal(txtABFOCons.Text)
                                , General.GetNullableDecimal(txtTOBFOCons.Text)
                                , General.GetNullableDecimal(txtIGGFOCons.Text)
                                , General.GetNullableInteger(txtABkW.Text)
                                , General.GetNullableInteger(txtTOBkW.Text)
                                , General.GetNullableInteger(txtIGGkW.Text)
                                , General.GetNullableInteger(ddlTier.SelectedValue)
                                , General.GetNullableInteger(txtTEU.Text)
                                , General.GetNullableInteger(ddlentitytype.SelectedValue)
                                , chkEGCS.Checked == true ? 1 : 0
                                , General.GetNullableInteger(rdlEGCS.SelectedIndex == -1 ? null : rdlEGCS.SelectedValue)
                                , General.GetNullableInteger(ddlFuelComplianceMethod.SelectedValue)
                                , General.GetNullableDecimal(txtFuelPipingVolume.Text)
                                , General.GetNullableInteger(ucpscalert.Text)
                                , General.GetNullableDateTime(ucsplsurveylastdate.Text)
                                , General.GetNullableDateTime(ucsplsurveynextdate.Text)
                                , General.GetNullableDateTime(ucdrydocksurveydate.Text)
                                , General.GetNullableDateTime(ucclassddwindownextdate.Text)
                                );
                            EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
                            ucStatus.Text = "Vessel Information is updated.";
                            ucStatus.Visible = true;
                        }
                    }
                    else
                    {
                        string filename = string.Empty;
                        if (FileUpload.PostedFile.ContentLength > 0)
                        {

                            filename = txtIMONumber.Text.Trim() + Path.GetExtension(FileUpload.FileName);
                            string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\Registers\\VesselParticulars\\";

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string strpath = path + filename;
                            FileUpload.PostedFile.SaveAs(strpath);
                            filename = "\\Registers\\VesselParticulars\\" + filename;

                        }
                        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "PHOENIX")
                        {
                            PhoenixRegistersVessel.VesselInsert(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , txtVesselName.Text
                                , Int16.Parse(ucFlag.SelectedFlag.ToString())
                                , Int16.Parse(ucVesselType.SelectedVesseltype.ToString())
                                , General.GetNullableInteger(ucAddrPrimaryManager.SelectedValue)
                                , General.GetNullableInteger(ucAddrOwner.SelectedValue)
                                , General.GetNullableInteger(ucPrincipal.SelectedValue)
                                , txtVesselCode.Text
                                , txtCallSign.Text
                                , txtOfficialNumber.Text
                                , txtIMONumber.Text
                                , General.GetNullableInteger(ucAddrYard.SelectedValue)
                                , General.GetNullableInteger(txtLifeBoatCapacity.Text)
                                , General.GetNullableInteger(ucPortRegistered.SelectedValue)
                                , General.GetNullableDateTime(txtKeelLaidDate.Text)
                                , General.GetNullableDateTime(txtLaunchedDate.Text)
                                , General.GetNullableDateTime(txtDeliveryDate.Text)
                                , General.GetNullableDateTime(txtTakeoverDateByESM.Text)
                                , General.GetNullableInteger(txtHullNo.Text)
                                , General.GetNullableInteger(txtMMSINo.Text)
                                , txtClassNotation.Text
                                , ucNavigationArea.SelectedText
                                , General.GetNullableInteger(ucCharterer.SelectedValue)
                                , General.GetNullableDecimal(txtLBP.Text)
                                , Int16.Parse(ucEngineType.SelectedEngineName.ToString())
                                , null
                                , General.GetNullableInteger(ucClassName.SelectedValue)
                                , General.GetNullableDecimal(txtSpeed.Text)
                                , General.GetNullableDecimal(txtBreath.Text)
                                , General.GetNullableDecimal(txtDepth.Text)
                                , General.GetNullableDecimal(txtLoa.Text)
                                , txtRemarks.Text
                                , General.GetNullableInteger(ucDisponentOwner.SelectedValue)
                                , General.GetNullableDecimal(txtRegisteredGT.Text)
                                , General.GetNullableDecimal(txtSuezGT.Text)
                                , General.GetNullableDecimal(txtPanamaGT.Text)
                                , General.GetNullableDecimal(txtRegisteredNT.Text)
                                , General.GetNullableDecimal(txtSuezNT.Text)
                                , General.GetNullableDecimal(txtPanamaNT.Text)
                                , General.GetNullableDecimal(txtFreeboardTropical.Text)
                                , General.GetNullableDecimal(txtDraftTropical.Text)
                                , General.GetNullableDecimal(txtDWTTropical.Text)
                                , General.GetNullableDecimal(txtFreeboardSummer.Text)
                                , General.GetNullableDecimal(txtDraftSummer.Text)
                                , General.GetNullableDecimal(txtDWTSummer.Text)
                                , General.GetNullableDecimal(txtFreeboardWinter.Text)
                                , General.GetNullableDecimal(txtDraftWinter.Text)
                                , General.GetNullableDecimal(txtDWTWinter.Text)
                                , General.GetNullableDecimal(txtFreeboardLightship.Text)
                                , General.GetNullableDecimal(txtDraftLightship.Text)
                                , General.GetNullableDecimal(txtDWTLightship.Text)
                                , General.GetNullableDecimal(txtFreeboardBallastCond.Text)
                                , General.GetNullableDecimal(txtDraftBallastCond.Text)
                                , General.GetNullableDecimal(txtDWTBallastCond.Text)
                                , txtMainEngine.Text
                                , General.GetNullableDecimal(txtMCR.Text)
                                , txtAuxEngine.Text
                                , txtAuxBoiler.Text
                                , General.GetNullableInteger(txtLifeBoatQuantity.Text)
                                , General.GetNullableDecimal(txtHeight.Text)
                                , General.GetNullableInteger(ddlIceClassed.SelectedValue)
                                , General.GetNullableInteger(ddlFittedwithFramo.SelectedValue)
                                , General.GetNullableString(txtClassNo.Text)
                                , 1
                                , General.GetNullableDecimal(txtKW.Text)
                                , General.GetNullableDecimal(txtBHP.Text)
                                , General.GetNullableInteger(ucManagementType.SelectedHard)
                                , General.GetNullableDecimal(txtSecCapacity.Text)
                                , General.GetNullableInteger(txtNoOfECDIS.Text)
                                , General.GetNullableInteger(txtECDISMaker.SelectedValue)
                                , txtECDISModel.Text
                                , filename
                                , General.GetNullableDateTime(ucAnniversaryDate.Text)
                                , null, null, null, null
                                , General.GetNullableInteger(txtMEQuantity.Text)
                                , General.GetNullableInteger(txtAEQuantity.Text)
                                , General.GetNullableInteger(txtABQuantity.Text)
                                , General.GetNullableInteger(txtTOBQuantity.Text)
                                , General.GetNullableInteger(txtIGGQuantity.Text)
                                , General.GetNullableInteger(ucAEEngineType.SelectedEngineName)
                                , General.GetNullableInteger(txtMEMaker.SelectedValue)
                                , General.GetNullableInteger(txtAEMaker.SelectedValue)
                                , General.GetNullableInteger(txtABMaker.SelectedValue)
                                , General.GetNullableInteger(txtTOBMaker.SelectedValue)
                                , General.GetNullableInteger(txtIGGMaker.SelectedValue)
                                , General.GetNullableString(txtTOBModel.Text)
                                , General.GetNullableString(txtIGGModel.Text)
                                , General.GetNullableDecimal(txtAEKW.Text)
                                , General.GetNullableDecimal(txtMESFOC.Text)
                                , General.GetNullableDecimal(txtAESFOC.Text)
                                , General.GetNullableDecimal(txtEEDI.Text)
                                , General.GetNullableDecimal(txtEIV.Text)
                                , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                 , General.GetNullableDecimal(txtPropDiameter.Text)
                                , General.GetNullableDecimal(txtPropPitch.Text)
                                , General.GetNullableInteger(txtMCRRPM.Text)
                                , General.GetNullableDecimal(txtMCRgbhph.Text)
                                , General.GetNullableDecimal(txtMCRFOCons.Text)
                                , General.GetNullableInteger(txtNCRBHP.Text)
                                , General.GetNullableInteger(txtNCRkW.Text)
                                , General.GetNullableInteger(txtNCRRPM.Text)
                                , General.GetNullableDecimal(txtNCRgbhph.Text)
                                , General.GetNullableDecimal(txtNCRgkWh.Text)
                                , General.GetNullableInteger(txtAERPM.Text)
                                , General.GetNullableDecimal(txtAEFOCons.Text)
                                , General.GetNullableInteger(txtDisplacement.Text)
                                , General.GetNullableInteger(txtServiceBHP.Text)
                                , General.GetNullableDecimal(txtNCRSpeed.Text)
                                , General.GetNullableInteger(txtSeaTrialNCRBHP.Text)
                                , General.GetNullableDecimal(txtPowerkWAE1.Text)
                                , General.GetNullableDecimal(txtPowerkWAE2.Text)
                                , General.GetNullableDecimal(txtPowerkWAE3.Text)
                                , General.GetNullableDecimal(txtPowerkWAE4.Text)
                                , General.GetNullableInteger(txtRPMAE1.Text)
                                , General.GetNullableInteger(txtRPMAE2.Text)
                                , General.GetNullableInteger(txtRPMAE3.Text)
                                , General.GetNullableInteger(txtRPMAE4.Text)
                                , General.GetNullableDecimal(txtSFOCAE1.Text)
                                , General.GetNullableDecimal(txtSFOCAE2.Text)
                                , General.GetNullableDecimal(txtSFOCAE3.Text)
                                , General.GetNullableDecimal(txtSFOCAE4.Text)
                                , General.GetNullableDecimal(txtFOConsAE1.Text)
                                , General.GetNullableDecimal(txtFOConsAE2.Text)
                                , General.GetNullableDecimal(txtFOConsAE3.Text)
                                , General.GetNullableDecimal(txtFOConsAE4.Text)
                                , General.GetNullableDecimal(txtNCRFOCons.Text)
                                , General.GetNullableInteger(txtCEQuantity.Text)
                                , General.GetNullableInteger(txtCEMaker.SelectedValue)
                                , General.GetNullableString(txtCEModel.Text)
                                , General.GetNullableDecimal(txtABsfoc.Text)
                                , General.GetNullableDecimal(txtTOBsfoc.Text)
                                , General.GetNullableDecimal(txtIGGsfoc.Text)
                                , General.GetNullableInteger(txtPMECkw.Text)
                                , General.GetNullableInteger(txtPMECrpm.Text)
                                , General.GetNullableDecimal(txtPMECgbhph.Text)
                                , General.GetNullableDecimal(txtPMECgkwh.Text)
                                , General.GetNullableInteger(ucPMECEngineType.SelectedEngineName)
                                , General.GetNullableDecimal(txtPMECFOCons.Text)
                                , General.GetNullableDecimal(txtABFOCons.Text)
                                , General.GetNullableDecimal(txtTOBFOCons.Text)
                                , General.GetNullableDecimal(txtIGGFOCons.Text)
                                , General.GetNullableInteger(txtABkW.Text)
                                , General.GetNullableInteger(txtTOBkW.Text)
                                , General.GetNullableInteger(txtIGGkW.Text)
                                , General.GetNullableInteger(ddlTier.SelectedValue)
                                , General.GetNullableInteger(txtTEU.Text)
                                , General.GetNullableInteger(ddlentitytype.SelectedValue)
                                , chkEGCS.Checked == true ? 1 : 0
                                , General.GetNullableInteger(rdlEGCS.SelectedIndex == -1 ? null : rdlEGCS.SelectedValue)
                                , General.GetNullableInteger(ddlFuelComplianceMethod.SelectedValue)
                                , General.GetNullableDecimal(txtFuelPipingVolume.Text)
                                , General.GetNullableInteger(chkusvisa.Checked==true?"1":"0")
                                , General.GetNullableInteger(chkaustralianvisa.Checked == true ? "1" : "0")
                                );
                            //ucStatus.Text = "Vessel Information is saved.";                       
                            Reset();
                        }
                        else
                        {
                            PhoenixRegistersVessel.VesselInsert(
                           PhoenixSecurityContext.CurrentSecurityContext.UserCode
                           , txtVesselName.Text
                           , Int16.Parse(ucFlag.SelectedFlag.ToString())
                           , Int16.Parse(ucVesselType.SelectedVesseltype.ToString())
                           , General.GetNullableInteger(ucAddrPrimaryManager.SelectedValue)
                           , General.GetNullableInteger(ucAddrOwner.SelectedValue)
                           , General.GetNullableInteger(ucPrincipal.SelectedValue)
                           , txtVesselCode.Text
                           , txtCallSign.Text
                           , txtOfficialNumber.Text
                           , txtIMONumber.Text
                           , General.GetNullableInteger(ucAddrYard.SelectedValue)
                           , General.GetNullableInteger(txtLifeBoatCapacity.Text)
                           , General.GetNullableInteger(ucPortRegistered.SelectedValue)
                           , General.GetNullableDateTime(txtKeelLaidDate.Text)
                           , General.GetNullableDateTime(txtLaunchedDate.Text)
                           , General.GetNullableDateTime(txtDeliveryDate.Text)
                           , General.GetNullableDateTime(txtTakeoverDateByESM.Text)
                           , General.GetNullableInteger(txtHullNo.Text)
                           , General.GetNullableInteger(txtMMSINo.Text)
                           , txtClassNotation.Text
                           , ucNavigationArea.SelectedText
                           , General.GetNullableInteger(lblMatrixStandardId.Text)                                      //OFFSHORE
                           , General.GetNullableDecimal(txtLBP.Text)
                           , Int16.Parse(ucEngineType.SelectedEngineName.ToString())
                           , null
                           , General.GetNullableInteger(ucClassName.SelectedValue)
                           , General.GetNullableDecimal(txtSpeed.Text)
                           , General.GetNullableDecimal(txtBreath.Text)
                           , General.GetNullableDecimal(txtDepth.Text)
                           , General.GetNullableDecimal(txtLoa.Text)
                           , txtRemarks.Text
                           , General.GetNullableInteger(ucDisponentOwner.SelectedValue)
                           , General.GetNullableDecimal(txtRegisteredGT.Text)
                           , General.GetNullableDecimal(txtSuezGT.Text)
                           , General.GetNullableDecimal(txtPanamaGT.Text)
                           , General.GetNullableDecimal(txtRegisteredNT.Text)
                           , General.GetNullableDecimal(txtSuezNT.Text)
                           , General.GetNullableDecimal(txtPanamaNT.Text)
                           , General.GetNullableDecimal(txtFreeboardTropical.Text)
                           , General.GetNullableDecimal(txtDraftTropical.Text)
                           , General.GetNullableDecimal(txtDWTTropical.Text)
                           , General.GetNullableDecimal(txtFreeboardSummer.Text)
                           , General.GetNullableDecimal(txtDraftSummer.Text)
                           , General.GetNullableDecimal(txtDWTSummer.Text)
                           , General.GetNullableDecimal(txtFreeboardWinter.Text)
                           , General.GetNullableDecimal(txtDraftWinter.Text)
                           , General.GetNullableDecimal(txtDWTWinter.Text)
                           , General.GetNullableDecimal(txtFreeboardLightship.Text)
                           , General.GetNullableDecimal(txtDraftLightship.Text)
                           , General.GetNullableDecimal(txtDWTLightship.Text)
                           , General.GetNullableDecimal(txtFreeboardBallastCond.Text)
                           , General.GetNullableDecimal(txtDraftBallastCond.Text)
                           , General.GetNullableDecimal(txtDWTBallastCond.Text)
                           , txtMainEngine.Text
                           , General.GetNullableDecimal(txtMCR.Text)
                           , txtAuxEngine.Text
                           , txtAuxBoiler.Text
                           , General.GetNullableInteger(txtLifeBoatQuantity.Text)
                           , General.GetNullableDecimal(txtHeight.Text)
                           , General.GetNullableInteger(ddlIceClassed.SelectedValue)
                           , General.GetNullableInteger(ddlFittedwithFramo.SelectedValue)
                           , General.GetNullableString(txtClassNo.Text)
                           , 1
                           , General.GetNullableDecimal(txtKW.Text)
                           , General.GetNullableDecimal(txtBHP.Text)
                           , General.GetNullableInteger(ucManagementType.SelectedHard)
                           , General.GetNullableDecimal(txtSecCapacity.Text)
                           , General.GetNullableInteger(txtNoOfECDIS.Text)
                           , General.GetNullableInteger(txtECDISMaker.SelectedValue)
                           , txtECDISModel.Text
                           , filename
                           , General.GetNullableDateTime(ucAnniversaryDate.Text)
                           , General.GetNullableInteger(ucOffshoreCharterer.SelectedAddress)
                           , General.GetNullableInteger(ucPropulsion.SelectedPropulsion)
                           , General.GetNullableInteger(Voltage.Text)
                           , General.GetNullableInteger(DPClass.SelectedDPClass)
                           , General.GetNullableInteger(txtMEQuantity.Text)
                            , General.GetNullableInteger(txtAEQuantity.Text)
                            , General.GetNullableInteger(txtABQuantity.Text)
                            , General.GetNullableInteger(txtTOBQuantity.Text)
                            , General.GetNullableInteger(txtIGGQuantity.Text)
                            , General.GetNullableInteger(ucAEEngineType.SelectedEngineName)
                            , General.GetNullableInteger(txtMEMaker.SelectedValue)
                            , General.GetNullableInteger(txtAEMaker.SelectedValue)
                            , General.GetNullableInteger(txtABMaker.SelectedValue)
                            , General.GetNullableInteger(txtTOBMaker.SelectedValue)
                            , General.GetNullableInteger(txtIGGMaker.SelectedValue)
                            , General.GetNullableString(txtTOBModel.Text)
                            , General.GetNullableString(txtIGGModel.Text)
                            , General.GetNullableDecimal(txtAEKW.Text)
                            , General.GetNullableDecimal(txtMESFOC.Text)
                            , General.GetNullableDecimal(txtAESFOC.Text)
                            , General.GetNullableDecimal(txtEEDI.Text)
                            , General.GetNullableDecimal(txtEIV.Text)
                            , General.GetNullableInteger(ddlIceClass.SelectedValue)
                            , General.GetNullableDecimal(txtPropDiameter.Text)
                            , General.GetNullableDecimal(txtPropPitch.Text)
                            , General.GetNullableInteger(txtMCRRPM.Text)
                            , General.GetNullableDecimal(txtMCRgbhph.Text)
                            , General.GetNullableDecimal(txtMCRFOCons.Text)
                            , General.GetNullableInteger(txtNCRBHP.Text)
                            , General.GetNullableInteger(txtNCRkW.Text)
                            , General.GetNullableInteger(txtNCRRPM.Text)
                            , General.GetNullableDecimal(txtNCRgbhph.Text)
                            , General.GetNullableDecimal(txtNCRgkWh.Text)
                            , General.GetNullableInteger(txtAERPM.Text)
                            , General.GetNullableDecimal(txtAEFOCons.Text)
                            , General.GetNullableInteger(txtDisplacement.Text)
                            , General.GetNullableInteger(txtServiceBHP.Text)
                            , General.GetNullableDecimal(txtNCRSpeed.Text)
                            , General.GetNullableInteger(txtSeaTrialNCRBHP.Text)
                            , General.GetNullableDecimal(txtPowerkWAE1.Text)
                            , General.GetNullableDecimal(txtPowerkWAE2.Text)
                            , General.GetNullableDecimal(txtPowerkWAE3.Text)
                            , General.GetNullableDecimal(txtPowerkWAE4.Text)
                            , General.GetNullableInteger(txtRPMAE1.Text)
                            , General.GetNullableInteger(txtRPMAE2.Text)
                            , General.GetNullableInteger(txtRPMAE3.Text)
                            , General.GetNullableInteger(txtRPMAE4.Text)
                            , General.GetNullableDecimal(txtSFOCAE1.Text)
                            , General.GetNullableDecimal(txtSFOCAE2.Text)
                            , General.GetNullableDecimal(txtSFOCAE3.Text)
                            , General.GetNullableDecimal(txtSFOCAE4.Text)
                            , General.GetNullableDecimal(txtFOConsAE1.Text)
                            , General.GetNullableDecimal(txtFOConsAE2.Text)
                            , General.GetNullableDecimal(txtFOConsAE3.Text)
                            , General.GetNullableDecimal(txtFOConsAE4.Text)
                            , General.GetNullableDecimal(txtNCRFOCons.Text)
                            , General.GetNullableInteger(txtCEQuantity.Text)
                            , General.GetNullableInteger(txtCEMaker.SelectedValue)
                            , General.GetNullableString(txtCEModel.Text)
                            , General.GetNullableDecimal(txtABsfoc.Text)
                            , General.GetNullableDecimal(txtTOBsfoc.Text)
                            , General.GetNullableDecimal(txtIGGsfoc.Text)
                            , General.GetNullableInteger(txtPMECkw.Text)
                            , General.GetNullableInteger(txtPMECrpm.Text)
                            , General.GetNullableDecimal(txtPMECgbhph.Text)
                            , General.GetNullableDecimal(txtPMECgkwh.Text)
                            , General.GetNullableInteger(ucPMECEngineType.SelectedEngineName)
                            , General.GetNullableDecimal(txtPMECFOCons.Text)
                            , General.GetNullableDecimal(txtABFOCons.Text)
                            , General.GetNullableDecimal(txtTOBFOCons.Text)
                            , General.GetNullableDecimal(txtIGGFOCons.Text)
                            , General.GetNullableInteger(txtABkW.Text)
                            , General.GetNullableInteger(txtTOBkW.Text)
                            , General.GetNullableInteger(txtIGGkW.Text)
                            , General.GetNullableInteger(ddlTier.SelectedValue)
                            , General.GetNullableInteger(txtTEU.Text)
                            , General.GetNullableInteger(ddlentitytype.SelectedValue)
                            , chkEGCS.Checked == true ? 1 : 0
                            , General.GetNullableInteger(rdlEGCS.SelectedIndex == -1 ? null : rdlEGCS.SelectedValue)
                            , General.GetNullableInteger(ddlFuelComplianceMethod.SelectedValue)
                            , General.GetNullableDecimal(txtFuelPipingVolume.Text)
                        );
                            //ucStatus.Text = "Vessel Information is saved.";                       
                            Reset();
                        }
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidVessel()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtVesselName.Text.Equals(""))
            ucError.ErrorMessage = "Vessel Name is required.";

        if (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null)
            ucError.ErrorMessage = "Vessel Type is required.";

        if (txtVesselCode.Text.Equals(""))
            ucError.ErrorMessage = "Vessel code is required.";

        if (General.GetNullableInteger(ucFlag.SelectedFlag) == null)
            ucError.ErrorMessage = "Flag is required.";

        if (General.GetNullableInteger(ucAddrOwner.SelectedValue) == null)
            ucError.ErrorMessage = "Owner is required.";

        if (General.GetNullableInteger(ucAddrPrimaryManager.SelectedValue) == null)
            ucError.ErrorMessage = "Manager is required.";

        if (General.GetNullableInteger(ucEngineType.SelectedEngineName) == null)
            ucError.ErrorMessage = "Main Engine Type is required.";

        if (General.GetNullableInteger(ucPrincipal.SelectedValue) == null)
            ucError.ErrorMessage = "Principal is required.";

        if (General.GetNullableInteger(ucManagementType.SelectedHard) == null)
            ucError.ErrorMessage = "Management Type is required.";

        if (General.GetNullableInteger(ucAEEngineType.SelectedEngineName) == null)
            ucError.ErrorMessage = "Aux Engine Type is required.";

        //if (!Int16.TryParse(ucEngineModel.SelectedModelName.ToString(), out resultInt16))
        //    ucError.ErrorMessage = "Engine Model is required.";

        //if (txtDateEntered.Text != null && (!DateTime.TryParse(txtDateEntered.Text, out resultDate)))
        //    ucError.ErrorMessage = "Valid Date Entered is required.";

        //if (txtDateLeft.Text != null && (!DateTime.TryParse(txtDateLeft.Text, out resultDate)))
        //    ucError.ErrorMessage = "Valid Date Left is required.";

        //if (txtOrgDateEntered.Text != null && (!DateTime.TryParse(txtOrgDateEntered.Text, out resultDate)))
        //    ucError.ErrorMessage = "Valid Org. Date Entered is required.";

        //if (txtOrgDateLeft.Text != null && (!DateTime.TryParse(txtOrgDateLeft.Text, out resultDate)))
        //    ucError.ErrorMessage = "Valid Org. Date Left is required.";

        //if (txtDateEntered.Text != null && txtDateLeft.Text != null)
        //{
        //    if ((DateTime.TryParse(txtDateEntered.Text, out resultDate)) && (DateTime.TryParse(txtDateLeft.Text, out resultDate)))
        //        if ((DateTime.Parse(txtDateEntered.Text)) >= (DateTime.Parse(txtDateLeft.Text)))
        //            ucError.ErrorMessage = "'Date Left' should be greater than 'Date Entered'";
        //}

        //if (txtOrgDateEntered.Text != null && txtOrgDateLeft.Text != null)
        //{
        //    if ((DateTime.TryParse(txtOrgDateEntered.Text, out resultDate)) && (DateTime.TryParse(txtOrgDateLeft.Text, out resultDate)))
        //        if ((DateTime.Parse(txtOrgDateEntered.Text)) >= (DateTime.Parse(txtOrgDateLeft.Text)))
        //            ucError.ErrorMessage = "'Original Date Left' should be greater than 'Original Date Entered'";
        //}
        if (FileUpload.PostedFile.ContentLength > 0)
        {
            string extension = Path.GetExtension(FileUpload.FileName.ToString());
            if (!extension.Contains(".xl"))
            {
                ucError.ErrorMessage = "Upload valid Excel file.";
            }
            if (extension.Contains(".xl"))
            {
                using (ExcelPackage pck = new ExcelPackage(FileUpload.FileContent))
                {
                    if (pck.Workbook.Worksheets.Count == 0 || pck.Workbook.Worksheets[1].Name.ToString().Trim() != txtIMONumber.Text.Trim())
                        ucError.ErrorMessage = "Cannot Find the Docking Particulars Sheet, Sheet Name should be the Vessel IMO Number.";
                }
            }
        }
        return (!ucError.IsError);

    }

    private void Reset()
    {
        Filter.CurrentVesselMasterFilter = null;

        ucVesselType.SelectedVesseltype = "";
        ucFlag.SelectedFlag = "";
        ucAddrOwner.SelectedValue = "";
        ucAddrOwner.Text = "";
        ucManagementType.SelectedHard = "";

        ucAddrPrimaryManager.SelectedValue = "";
        ucAddrPrimaryManager.Text = "";

        ucAddrYard.SelectedValue = "";
        ucAddrYard.Text = "";

        ucPortRegistered.SelectedValue = "";
        ucPortRegistered.Text = "";
        ucPrincipal.SelectedValue = "";
        ucPrincipal.Text = "";

        txtVesselName.Text = "";
        txtVesselCode.Text = "";
        txtCallSign.Text = "";
        txtOfficialNumber.Text = "";
        txtIMONumber.Text = "";
        txtLifeBoatCapacity.Text = "";
        txtHullNo.Text = "";
        ucNavigationArea.SelectedText = "--Select--";
        ucCharterer.SelectedValue = "";
        ucCharterer.Text = "";
        ucOffshoreCharterer.SelectedAddress = "";
        txtClassNotation.Text = "";
        txtMMSINo.Text = "";
        txtLoa.Text = "";
        txtLBP.Text = "";

        txtKeelLaidDate.Text = "";
        txtLaunchedDate.Text = "";
        txtDeliveryDate.Text = "";
        txtTakeoverDateByESM.Text = "";

        ucEngineType.SelectedEngineName = "";
        //txtEngineModel.Text = "";
        ucClassName.SelectedValue = "";
        ucClassName.Text = "";

        txtSpeed.Text = "";
        txtBreath.Text = "";
        txtDepth.Text = "";
        txtRemarks.Text = "";

        ucDisponentOwner.SelectedValue = "";
        ucDisponentOwner.Text = "";
        ucPropulsion.SelectedPropulsion = "";

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

        txtNoOfECDIS.Text = "";
        txtECDISMaker.Text = "";
        //txtECDISMakerCode.Text = "";
        txtECDISMaker.SelectedValue = "";
        txtECDISModel.Text = "";
        Voltage.Text = "";
        DPClass.SelectedDPClass = "";

    }

    private void BindIceClass()
    {
        DataSet ds = PhoenixRegistersVessel.ListIceClass();
        ddlIceClass.DataTextField = "FLDICECLASS";
        ddlIceClass.DataValueField = "FLDID";
        ddlIceClass.DataSource = ds;
        ddlIceClass.DataBind();
    }

    private void BindEntitytype()
    {
        DataSet ds = PhoenixRegistersVessel.ListEntitytype();
        ddlentitytype.DataTextField = "FLDNAME";
        ddlentitytype.DataValueField = "FLDENTITYTYPEID";
        ddlentitytype.DataSource = ds;
        ddlentitytype.DataBind();
        ddlentitytype.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    //Response.Redirect( "../Registers/RegistersVessel.aspx";
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }

    private void bindEGCSType()
    {
        DataSet ds = PhoenixRegistersVessel.ListEGCSType();
        rdlEGCS.DataBindings.DataTextField = "FLDQUICKNAME";
        rdlEGCS.DataBindings.DataValueField = "FLDQUICKCODE";
        rdlEGCS.DataSource = ds;
        rdlEGCS.DataBind();
        rdlEGCS.Items.Insert(0, new ButtonListItem("Not Applicable", "0"));
    }

    private void bindFuelComplianceMethod()
    {
        DataSet ds = PhoenixRegistersVessel.ListFuelComplianceMethod();
        ddlFuelComplianceMethod.DataTextField = "FLDQUICKNAME";
        ddlFuelComplianceMethod.DataValueField = "FLDQUICKCODE";
        ddlFuelComplianceMethod.DataSource = ds;
        ddlFuelComplianceMethod.DataBind();
        ddlFuelComplianceMethod.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void chkEGCS_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEGCS.Checked == true)
            rdlEGCS.Visible = true;
        else
            rdlEGCS.Visible = false;
    }


}