using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewoffshoreOtherCompanyExperienceAdd : PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);

        empid = Request.QueryString["empid"].ToString();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewOtherExperienceList.AccessRights = this.ViewState;
        MenuCrewOtherExperienceList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["type"] == "p")
            {
                chkPromtedyn.Visible = false;
                lblpromotion.Visible = false;

            }
            ucNatlyOfficers.SelectedNationality = "97";
            ucNatlyRatings.SelectedNationality = "97";

            ViewState["TYPEOFANCORHANDLED"] = "";
            ViewState["ROVCLASS"] = "";

          

            ddlUMS.HardTypeCode = ((int)PhoenixHardTypeCode.PASSPORTECNR).ToString();

            BindddType();
            Bindrovclass();
            if (Request.QueryString["CREWOTHEREXPERIENCEID"] != null)
            {
                EditCrewOtherExperience(int.Parse(Request.QueryString["CREWOTHEREXPERIENCEID"].ToString()));
            }
            txtManagingCompany.Focus();

        }
        RadComboBox dl = (RadComboBox)ddlRank.FindControl("ddlRank");
        dl.DataTextField = "FLDRANKCODE";
    }

    protected void BindddType()
    {
        //chkValue.Visible = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = PhoenixRegistersDMRAnchorHandilingType.DMRAnchorHandilingTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
        DataTable dt = ds.Tables[0];
        //chkValue.DataSource = dt;
        //chkValue.DataBind();
    }
    protected void Bindrovclass()
    {
        //cblRovType.Visible = true;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = PhoenixRegistersDMRRovType.MDRRovTypeSearch("", 1, 100, ref iRowCount, ref iTotalPageCount);
        DataTable dt = ds.Tables[0];
       // cblRovType.DataSource = dt;
        //cblRovType.DataBind();
    }

    protected void EditCrewOtherExperience(int? Otherexperienceid)
    {
        DataSet ds = PhoenixNewApplicantOtherExperience.ListEmployeeOtherExperienceds(Convert.ToInt32(empid), Otherexperienceid);

        DataTable dt = ds.Tables[0];
        DataTable dt1 = ds.Tables[1];

        if (dt.Rows.Count > 0)
        {
            txtManagingCompany.Text = dt.Rows[0]["FLDMANAGINGCOMPANY"].ToString();
            ddlManningCompany.SelectedOtherCompany = dt.Rows[0]["FLDMANNINGCOMPANY"].ToString();
            ddlRank.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
            txtFrom.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
            txtTo.Text = dt.Rows[0]["FLDTODATE"].ToString();
            txtVessel.Text = dt.Rows[0]["FLDVESSEL"].ToString();
            ddlVesselType.SelectedVesseltype = dt.Rows[0]["FLDVESSELTYPE"].ToString();
            ddlEngineType.SelectedEngineName = dt.Rows[0]["FLDENGINETYPE"].ToString();
            txtDWT.Text = dt.Rows[0]["FLDVESSELDWT"].ToString();
            txtGt.Text = dt.Rows[0]["FLDVESSELGT"].ToString();
            txtKWT.Text = dt.Rows[0]["FLDVESSELKW"].ToString();
            txtBHP.Text = dt.Rows[0]["FLDVESSELBHP"].ToString();
            ddlSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASON"].ToString();
            ddlSignonReason.SelectedSignOnReason = dt.Rows[0]["FLDSIGNONREASON"].ToString();
            ddlUMS.SelectedHard = dt.Rows[0]["FLDVESSELUMS"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
            ucNatlyRatings.SelectedNationality = dt.Rows[0]["FLDRATINGNATIONALITY"].ToString();
            ucNatlyOfficers.SelectedNationality = dt.Rows[0]["FLDOFFICERSNATIONALITY"].ToString();
            txtDuration.Text = dt.Rows[0]["FLDDURATION"].ToString();
            txtGap.Text = dt.Rows[0]["FLDGAP"].ToString();
            DPClass.SelectedDPClass = dt.Rows[0]["FLDDPCLASS"].ToString();
            txtDPMakeandModel.Text = dt.Rows[0]["FLDDPMAKEANDMODEL"].ToString();
            ucPropulsion.SelectedPropulsion = dt.Rows[0]["FLDPROPULSION"].ToString();
            txtVoltage.Text = dt.Rows[0]["FLDVOLTAGE"].ToString();
            ucTradingArea.SelectedTradingArea = dt.Rows[0]["FLDOPERATINGAREA"].ToString();
            txtcharterer.Text = dt.Rows[0]["FLDCHARTERER"].ToString();
            ucInstalationType.SelectedInstallType = dt.Rows[0]["FLDTYPEOFINSTALLATION"].ToString();


            if (dt1.Rows.Count > 0)
            {
                //ddlDryDockingsattended.SelectedValue = dt1.Rows[0]["FLDNOOFDRYDOCKINGSATTENDED"].ToString();
                //uctxtNumberofOVIDInspections.Text = dt1.Rows[0]["FLDNOOFOVIDINSPECTIONS"].ToString();
                //uctctNumberofOSVISInspections.Text = dt1.Rows[0]["FLDNOOFOSVISINSPECTIONS"].ToString();
                //txtNumberofOceanTows.Text = dt1.Rows[0]["FLDNOOFOCEANTOWS"].ToString();
                //txtNumberofRigMoves.Text = dt1.Rows[0]["FLDNOOFRIGMOVES"].ToString();
                //ddlFSIPSC.SelectedValue = dt1.Rows[0]["FLDFSIPSCYN"].ToString();
                //ddlFMEA.SelectedValue = dt1.Rows[0]["FLDFMEAYN"].ToString();
                //ddlDPAnnuals.SelectedValue = dt1.Rows[0]["FLDDPANNUALSYN"].ToString();
                //ddlDeliveryOrTakeover.SelectedValue = dt1.Rows[0]["FLDNEWDELIVERYTAKEOVERYN"].ToString();
                //ddlHeavyLift.SelectedValue = dt1.Rows[0]["FLDHEAVYLIFTPROJECTCARGOESYN"].ToString();
                //ddlDKDMud.SelectedValue = dt1.Rows[0]["FLDDKDMUDYN"].ToString();
                //ddlMethanol.SelectedValue = dt1.Rows[0]["FLDMETHANOLYN"].ToString();
                //ddlGlycol.SelectedValue = dt1.Rows[0]["FLDGLYCOLYN"].ToString();
                //ddlExperienceChainHandling.SelectedValue = dt1.Rows[0]["FLDEXPCHAINHANDLINGYN"].ToString();  
                string Str = dt1.Rows[0]["FLDTYPESOFANCHORSHANDLED"].ToString();

                //txtAnchorHandling.Text= dt1.Rows[0]["FLDANCHORHANDLING"].ToString();
                //txtSupply.Text= dt1.Rows[0]["FLDSUPPLYCOUNT"].ToString();
                //txtDiveSupport.Text= dt1.Rows[0]["FLDDIVESUPPORTCOUNT"].ToString();
                //txtROV.Text= dt1.Rows[0]["FLDROVCOUNT"].ToString();
                //txtFlotel.Text = dt1.Rows[0]["FLDFLOTELCOUNT"].ToString();
                string[] str1 = Str.Split(',');

            //    foreach (ListItem li in chkValue.Items)
            //    {
            //        foreach (string s in str1)
            //        {
            //            if (li.Value.Equals(s))
            //            {
            //                li.Selected = true;
            //            }
            //        }
            //    }
            //    string Strrov = dt1.Rows[0]["FLDCLASSOFROV"].ToString();
            //    string[] strrov1 = Strrov.Split(',');

            //    foreach (ListItem li1 in cblRovType.Items)
            //    {
            //        foreach (string s1 in strrov1)
            //        {
            //            if (li1.Value.Equals(s1))
            //            {
            //                li1.Selected = true;
            //            }
            //        }
            //    }
            //    ddlExperienceingranchors.SelectedValue = dt1.Rows[0]["FLDEXPERIENCEINGRAPPLING"].ToString();
            //    ddlExperiencestowage.SelectedValue = dt1.Rows[0]["FLDEXPERIENCESTOWAGEOFCHAINS"].ToString();
            //    txtSizelengthofchain.Text = dt1.Rows[0]["FLDSIZELENGTHCHAINHANDLEDMM"].ToString();
            //    txtSizelengthofchainmeter.Text = dt1.Rows[0]["FLDSIZELENGTHCHAINHANDLEDMTR"].ToString();
            //    ucAnchorhandlingdepth.Text = dt1.Rows[0]["FLDMAXIMUMANCHORHANDLINGDEPTH"].ToString();
            }
            if (dt.Rows[0]["FLDFRAMOEXP"].ToString() == "1")
            {
                chkFramo.Checked = true;
            }
            else
            {
                chkFramo.Checked = false;
            }
            chkPromtedyn.Checked = dt.Rows[0]["FLDPROMOTEDONBOARDYN"].ToString() == "1" ? true : false;
            ucFlag.SelectedFlag = dt.Rows[0]["FLDFLAG"].ToString();
            ddlIceClass.SelectedValue = dt.Rows[0]["FLDICECLASSYN"].ToString();
            txtEngineModel.Text = dt.Rows[0]["FLDENGINEMODEL"].ToString();
        }
    }

    protected void CalculateBHP(object sender, EventArgs e)
    {
        if (txtKWT.Text != "")
        {
            Double bhp = (Convert.ToDouble(txtKWT.Text)) * 1.341;
            txtBHP.Text = Convert.ToString(Math.Ceiling(bhp));
        }
    }
    protected void CalculateKwt(object sender, EventArgs e)
    {
        if (txtBHP.Text != "")
        {
            Double kwt = (Convert.ToDouble(txtBHP.Text)) / 1.341;
            txtKWT.Text = Convert.ToString(Math.Ceiling(kwt));
        }
    }
    protected void ResetCrewOtherExperience()
    {
        txtManagingCompany.Text = "";
        ddlManningCompany.SelectedOtherCompany = "";
        ddlRank.SelectedRank = "";
        txtFrom.Text = "";
        txtTo.Text = "";
        txtVessel.Text = "";
        ddlVesselType.SelectedVesseltype = "";
        ddlEngineType.SelectedEngineName = "";
        txtDWT.Text = "";
        txtGt.Text = "";
        txtKWT.Text = "";
        txtBHP.Text = "";
        ddlSignOffReason.SelectedSignOffReason = "";
        ddlUMS.SelectedHard = "";
        txtRemarks.Text = "";
        ucNatlyRatings.SelectedNationality = "";
        ucNatlyOfficers.SelectedNationality = "";
        chkPromtedyn.Checked = false;
        ucFlag.SelectedFlag = "";
        txtEngineModel.Text = "";
        DPClass.SelectedDPClass = "";
        txtDPMakeandModel.Text = "";
        ucPropulsion.SelectedPropulsion = "";
        txtVoltage.Text = "";
        ucTradingArea.SelectedTradingArea = "";
        txtcharterer.Text = "";
        ucInstalationType.SelectedInstallType = "";
        //ddlDryDockingsattended.SelectedValue = "";
        //ddlDryDockingsattended.SelectedValue = "";
        //uctxtNumberofOVIDInspections.Text = "";
        //uctctNumberofOSVISInspections.Text = "";
        //txtNumberofOceanTows.Text = "";
        //txtNumberofRigMoves.Text = "";
        //ddlFSIPSC.SelectedValue = "";
        //ddlFMEA.SelectedValue = "";
        //ddlDPAnnuals.SelectedValue = "";
        //ddlDeliveryOrTakeover.SelectedValue = "";
        //ddlHeavyLift.SelectedValue = "";
        //ddlDKDMud.SelectedValue = "";
        //ddlMethanol.SelectedValue = "";
        //ddlGlycol.SelectedValue = "";
        //ddlExperienceingranchors.SelectedValue = "";
        //ddlExperiencestowage.SelectedValue = "";
        //txtSizelengthofchain.Text = "";
        //txtSizelengthofchainmeter.Text = "";
        //ucAnchorhandlingdepth.Text = "";
    }
    protected void CrewOtherExperienceList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        string type = Request.QueryString["type"].ToString();
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string ancorhandle = ViewState["TYPEOFANCORHANDLED"].ToString();

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidOtherExperience())
                {

                    System.Text.StringBuilder strROVClass = new System.Text.StringBuilder();

                    //foreach (ListItem item in cblRovType.Items)
                    //{
                    //    if (item.Selected == true)
                    //    {
                    //        if (item.Value.ToString() != strROVClass.ToString())
                    //        {
                    //            strROVClass.Append(item.Value.ToString());
                    //            strROVClass.Append(",");
                    //        }
                    //    }
                    //}
                    ViewState["ROVCLASS"] = strROVClass.ToString();

                    System.Text.StringBuilder strValue = new System.Text.StringBuilder();

                    //foreach (ListItem item in chkValue.Items)
                    //{
                    //    if (item.Selected == true)
                    //    {
                    //        if (item.Value.ToString() != strValue.ToString())
                    //        {
                    //            strValue.Append(item.Value.ToString());
                    //            strValue.Append(",");
                    //        }
                    //    }
                    //}
                    ViewState["TYPEOFANCORHANDLED"] = strValue.ToString();

                    if (Request.QueryString["CREWOTHEREXPERIENCEID"] != null)
                    {

                        PhoenixCrewOffshoreOtherExperience.UpdateEmployeeOtherExperience(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                         , Convert.ToInt32(empid)
                                                         , txtManagingCompany.Text
                                                         , int.Parse(ddlManningCompany.SelectedOtherCompany)
                                                         , ddlRank.SelectedValue
                                                         , Convert.ToDateTime(txtFrom.Text)
                                                         , Convert.ToDateTime(txtTo.Text)
                                                         , txtVessel.Text
                                                         , Convert.ToInt32(ddlVesselType.SelectedVesseltype)
                                                         , General.GetNullableInteger(ddlEngineType.SelectedEngineName)
                                                         , General.GetNullableDecimal(txtDWT.Text)
                                                         , General.GetNullableDecimal(txtGt.Text)
                                                         , General.GetNullableDecimal(txtKWT.Text)
                                                         , General.GetNullableDecimal(txtBHP.Text)
                                                         , General.GetNullableString(string.Empty)
                                                         , General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
                                                         , General.GetNullableInteger(ddlUMS.SelectedHard)
                                                         , txtRemarks.Text
                                                         , Convert.ToInt32(Request.QueryString["CREWOTHEREXPERIENCEID"])
                                                         , General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
                                                         , General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
                                                         , Convert.ToByte(chkFramo.Checked ? 1 : 0)
                                                         , General.GetNullableInteger(ddlSignonReason.SelectedSignOnReason)
                                                         , General.GetNullableInteger(ucFlag.SelectedFlag)
                                                         , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                                         , General.GetNullableString(txtEngineModel.Text)
                                                         , General.GetNullableInteger(DPClass.SelectedDPClass)
                                                         , General.GetNullableString(txtDPMakeandModel.Text)
                                                         , General.GetNullableInteger(ucPropulsion.SelectedPropulsion)
                                                         , General.GetNullableString(txtVoltage.Text)
                                                         , General.GetNullableInteger(ucTradingArea.SelectedTradingArea)
                                                         , General.GetNullableString(txtcharterer.Text)
                                                         , General.GetNullableInteger(ucInstalationType.SelectedInstallType)
                                                         //, General.GetNullableInteger(ddlDryDockingsattended.SelectedValue)
                                                         //, General.GetNullableInteger(uctxtNumberofOVIDInspections.Text)
                                                         //, General.GetNullableInteger(uctctNumberofOSVISInspections.Text)
                                                         //, General.GetNullableString(ViewState["ROVCLASS"].ToString())
                                                         //, General.GetNullableInteger(txtNumberofOceanTows.Text)
                                                         //, General.GetNullableInteger(txtNumberofRigMoves.Text)
                                                         //, General.GetNullableInteger(ddlFSIPSC.SelectedValue)
                                                         //, General.GetNullableInteger(ddlFMEA.SelectedValue)
                                                         //, General.GetNullableInteger(ddlDPAnnuals.SelectedValue)
                                                         //, General.GetNullableInteger(ddlDeliveryOrTakeover.SelectedValue)
                                                         //, General.GetNullableInteger(ddlHeavyLift.SelectedValue)
                                                         //, General.GetNullableInteger(ddlDKDMud.SelectedValue)
                                                         //, General.GetNullableInteger(ddlMethanol.SelectedValue)
                                                         //, General.GetNullableInteger(ddlGlycol.SelectedValue)
                                                         //, General.GetNullableString(ViewState["TYPEOFANCORHANDLED"].ToString())
                                                         //, General.GetNullableInteger(ddlExperienceingranchors.SelectedValue)
                                                         //, General.GetNullableInteger(ddlExperiencestowage.SelectedValue)
                                                         //, General.GetNullableInteger(txtSizelengthofchain.Text)
                                                         //, General.GetNullableDecimal(txtSizelengthofchainmeter.Text)
                                                         //, General.GetNullableDecimal(ucAnchorhandlingdepth.Text)
                                                         //, General.GetNullableInteger(ddlExperienceChainHandling.SelectedValue)
                                                         //, General.GetNullableInteger(txtAnchorHandling.Text)
                                                         //, General.GetNullableInteger(txtSupply.Text)
                                                         //, General.GetNullableInteger(txtDiveSupport.Text)
                                                         //, General.GetNullableInteger(txtROV.Text)
                                                         //, General.GetNullableInteger(txtFlotel.Text)
                                                         );

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    }
                    else
                    {
                        DataTable dt = PhoenixCrewOffshoreOtherExperience.InsertEmployeeOtherExperience(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
                                                          , Convert.ToInt32(empid)
                                                          , General.GetNullableString(txtManagingCompany.Text)
                                                          , int.Parse(ddlManningCompany.SelectedOtherCompany)
                                                          , Convert.ToInt32(ddlRank.SelectedRank)
                                                          , Convert.ToDateTime(txtFrom.Text)
                                                          , Convert.ToDateTime(txtTo.Text)
                                                          , txtVessel.Text
                                                          , Convert.ToInt32(ddlVesselType.SelectedVesseltype)
                                                          , General.GetNullableInteger(ddlEngineType.SelectedEngineName)
                                                          , General.GetNullableDecimal(txtDWT.Text)
                                                          , General.GetNullableDecimal(txtGt.Text)
                                                          , General.GetNullableDecimal(txtKWT.Text)
                                                          , General.GetNullableDecimal(txtBHP.Text)
                                                          , General.GetNullableString(string.Empty)
                                                          , General.GetNullableInteger(ddlSignOffReason.SelectedSignOffReason)
                                                          , General.GetNullableInteger(ddlUMS.SelectedHard)
                                                          , General.GetNullableString(txtRemarks.Text)
                                                          , Convert.ToInt32(Request.QueryString["CREWOTHEREXPERIENCEID"])
                                                          , General.GetNullableInteger(ucNatlyOfficers.SelectedNationality)
                                                          , General.GetNullableInteger(ucNatlyRatings.SelectedNationality)
                                                          , Convert.ToByte(chkFramo.Checked ? 1 : 0)
                                                          , General.GetNullableInteger(ddlSignonReason.SelectedSignOnReason)
                                                          , General.GetNullableInteger(ucFlag.SelectedFlag)
                                                          , General.GetNullableInteger(ddlIceClass.SelectedValue)
                                                          , General.GetNullableString(txtEngineModel.Text)
                                                         , General.GetNullableInteger(DPClass.SelectedDPClass)
                                                         , General.GetNullableString(txtDPMakeandModel.Text)
                                                         , General.GetNullableInteger(ucPropulsion.SelectedPropulsion)
                                                         , General.GetNullableString(txtVoltage.Text)
                                                         , General.GetNullableInteger(ucTradingArea.SelectedTradingArea)
                                                         , General.GetNullableString(txtcharterer.Text)
                                                         , General.GetNullableInteger(ucInstalationType.SelectedInstallType)
                                                         //, General.GetNullableInteger(ddlDryDockingsattended.SelectedValue)
                                                         //, General.GetNullableInteger(uctxtNumberofOVIDInspections.Text)
                                                         //, General.GetNullableInteger(uctctNumberofOSVISInspections.Text)
                                                         //, General.GetNullableString(ViewState["ROVCLASS"].ToString())
                                                         //, General.GetNullableInteger(txtNumberofOceanTows.Text)
                                                         //, General.GetNullableInteger(txtNumberofRigMoves.Text)
                                                         //, General.GetNullableInteger(ddlFSIPSC.SelectedValue)
                                                         //, General.GetNullableInteger(ddlFMEA.SelectedValue)
                                                         //, General.GetNullableInteger(ddlDPAnnuals.SelectedValue)
                                                         //, General.GetNullableInteger(ddlDeliveryOrTakeover.SelectedValue)
                                                         //, General.GetNullableInteger(ddlHeavyLift.SelectedValue)
                                                         //, General.GetNullableInteger(ddlDKDMud.SelectedValue)
                                                         //, General.GetNullableInteger(ddlMethanol.SelectedValue)
                                                         //, General.GetNullableInteger(ddlGlycol.SelectedValue)
                                                         //, General.GetNullableString(ancorhandle)
                                                         //, General.GetNullableInteger(ddlExperienceingranchors.SelectedValue)
                                                         //, General.GetNullableInteger(ddlExperiencestowage.SelectedValue)
                                                         //, General.GetNullableInteger(txtSizelengthofchain.Text)
                                                         //, General.GetNullableDecimal(txtSizelengthofchainmeter.Text)
                                                         //, General.GetNullableDecimal(ucAnchorhandlingdepth.Text)
                                                         //, General.GetNullableInteger(ddlExperienceChainHandling.SelectedValue)
                                                         //, General.GetNullableInteger(txtAnchorHandling.Text)
                                                         //, General.GetNullableInteger(txtSupply.Text)
                                                         //, General.GetNullableInteger(txtDiveSupport.Text)
                                                         //, General.GetNullableInteger(txtROV.Text)
                                                         //, General.GetNullableInteger(txtFlotel.Text)
                                                          );
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

                        ResetCrewOtherExperience();
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
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidOtherExperience()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        int resultint;
        DateTime resultdate;


        if (!int.TryParse(ddlRank.SelectedRank, out resultint))
            ucError.ErrorMessage = "Rank is required";

        if (txtVessel.Text.Trim() == "")
            ucError.ErrorMessage = "Vessel is required";

        if (!int.TryParse(ddlManningCompany.SelectedOtherCompany, out resultint))
        {
            ucError.ErrorMessage = "Manning Company is required";
        }
        if (string.IsNullOrEmpty(txtFrom.Text))
        {
            ucError.ErrorMessage = "Sign-on Date can not be blank";
        }
        else if (DateTime.TryParse(txtFrom.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign-on Date should be later than current date";
        }

        if (string.IsNullOrEmpty(txtTo.Text))
        {
            ucError.ErrorMessage = "To Date can not be blank";
        }
        else if (!string.IsNullOrEmpty(txtTo.Text) && !string.IsNullOrEmpty(txtFrom.Text)
            && DateTime.TryParse(txtTo.Text, out resultdate) && DateTime.Compare(DateTime.Parse(txtFrom.Text), DateTime.Parse(txtTo.Text)) > 0)
        {
            ucError.ErrorMessage = "To Date should be greater than 'Sign-on Date'";
        }



        if (!int.TryParse(ddlVesselType.SelectedVesseltype, out resultint))
        {
            ucError.ErrorMessage = "Vessel Type is required";
        }

        if ((DateTime.TryParse(txtFrom.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0) && General.GetNullableDateTime(txtFrom.Text) != null)
        {
            ucError.ErrorMessage = "Sign-on Date should be later than current date";
        }
        if (string.IsNullOrEmpty(txtTo.Text) && General.GetNullableDateTime(txtFrom.Text) != null)
        {
            ucError.ErrorMessage = "To Date can not be blank";
        }
        if (!string.IsNullOrEmpty(txtTo.Text) && !string.IsNullOrEmpty(txtFrom.Text)
                && DateTime.TryParse(txtTo.Text, out resultdate) && DateTime.Compare(DateTime.Parse(txtFrom.Text), DateTime.Parse(txtTo.Text)) > 0)
        {
            ucError.ErrorMessage = "To Date should be greater than 'Sign-on Date'";
        }

        return (!ucError.IsError);


    }

}
