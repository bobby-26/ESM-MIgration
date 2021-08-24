using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRANavigation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            rblControlsAdequate.Enabled = false;
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        ViewState["COPYTYPE"] = "0";
        ViewState["SHOWALL"] = "0";
        ViewState["DASHBOARDYN"] = "0";
        if ((Request.QueryString["showall"] != null) && (Request.QueryString["showall"] != ""))
        {
            ViewState["SHOWALL"] = Request.QueryString["showall"].ToString();
        }
        if (Request.QueryString["CopyType"] == "1" || Request.QueryString["CopyType"] == "2")
        {
            toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
            ViewState["COPYTYPE"] = Request.QueryString["CopyType"].ToString();
            ucTitle.ShowMenu = "false";
        }
        else
        {
            if (Request.QueryString["RevYN"] != "1")
            {
                if (Request.QueryString["status"].ToString() != "3" && Request.QueryString["status"].ToString() != "7" && Request.QueryString["status"].ToString() != "5" && Request.QueryString["status"].ToString() != "6") //completed
                {
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                }

                if (Request.QueryString["status"].ToString() == "5") //completed
                {
                    toolbar.AddButton("Save", "RESAVE", ToolBarDirection.Right);
                }

                if ((Request.QueryString["RAType"] != null && Request.QueryString["RAType"].Equals("3")))
                {
                    toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
                }
                if ((Request.QueryString["Dashboardyn"] != null) && (Request.QueryString["Dashboardyn"] != ""))
                    ViewState["DASHBOARDYN"] = Request.QueryString["Dashboardyn"].ToString();
                if (ViewState["DASHBOARDYN"].ToString() == "0")
                {
                    toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
                }
            }
        }
        if (Request.QueryString["RevYN"] != "1")
        {
            MenuNavigation.AccessRights = this.ViewState;
            MenuNavigation.MenuList = toolbar.Show();
        }

        if (!IsPostBack)
        {
            DateTime dt = DateTime.Today;
            txtDate.Text = dt.ToString();
            ViewState["RISKASSESSMENTNAVIGATIONID"] = "";
            ViewState["VESSELID"] = "";

            ViewState["QUALITYCOMPANYID"] = "";

            ViewState["MOCRequestid"] = string.IsNullOrEmpty(Request.QueryString["MOCRequestid"]) ? "" : Request.QueryString["MOCRequestid"];
            ViewState["MOCID"] = string.IsNullOrEmpty(Request.QueryString["MOCID"]) ? "" : Request.QueryString["MOCID"];
            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RAType"]) ? "" : Request.QueryString["RAType"];
            ViewState["mocextention"] = string.IsNullOrEmpty(Request.QueryString["mocextention"]) ? "" : Request.QueryString["mocextention"];
            ViewState["VESSELID"] = string.IsNullOrEmpty(Request.QueryString["Vesselid"]) ? "" : Request.QueryString["Vesselid"];
            ViewState["Vesselname"] = string.IsNullOrEmpty(Request.QueryString["Vesselname"]) ? "" : Request.QueryString["Vesselname"];
            if (ViewState["RATYPE"].ToString() == "3")
            {
                txtVessel.Text = ViewState["Vesselname"].ToString();
                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(ViewState["VESSELID"].ToString());
            }
            else
            {
                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            }

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["QUALITYCOMPANYID"] = nvc.Get("QMS");
                ucCompany.SelectedCompany = ViewState["QUALITYCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;

            BindData();
            BindCategory();
            if (Request.QueryString["navigationid"] != null)
            {
                ViewState["RISKASSESSMENTNAVIGATIONID"] = Request.QueryString["navigationid"].ToString();
                RiskAssessmentNavigationEdit();
                BindScore();
            }
            if (imgShowRAEdit != null)
            {
                imgShowRAEdit.Attributes.Add("onclick", "return showPickList('spnRAEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListRA.aspx?opt=N&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&activity=" + "', true); ");
                if (!SessionUtil.CanAccess(this.ViewState, imgShowRAEdit.CommandName)) imgShowRAEdit.Visible = false;
            }
        }
        // BindGrid();
        BindComapany();
    }
    protected void ucEconomicHazardType_OnTextChangedEvent(object sender, EventArgs e)
    {
        gvEconomicRisk.Rebind();
        GridFooterItem gvEconomicRiskfooteritem = (GridFooterItem)gvEconomicRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlEcoSubHazardType = (RadComboBox)gvEconomicRiskfooteritem.FindControl("ddlSubHazardType");
        ddlEcoSubHazardType.DataTextField = "FLDNAME";
        ddlEcoSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEcoSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEconomicHazardType.SelectedHazardType));
        ddlEcoSubHazardType.DataBind();
    }

    protected void ucEnvHazardType_OnTextChangedEvent(object sender, EventArgs e)
    {
        gvEnvironmentalRisk.Rebind();
        GridFooterItem gvEnvironmentalRiskfooteritem = (GridFooterItem)gvEnvironmentalRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddllEnvSubHazardType = (RadComboBox)gvEnvironmentalRiskfooteritem.FindControl("ddlSubHazardType");
        ddllEnvSubHazardType.DataTextField = "FLDNAME";
        ddllEnvSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddllEnvSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEnvHazardType.SelectedHazardType));
        ddllEnvSubHazardType.DataBind();
    }

    protected void ucHazardType_TextChangedEvent(object sender, EventArgs e)
    {
        gvHealthSafetyRisk.Rebind();
        GridFooterItem gvHealthSafetyRiskfooteritem = (GridFooterItem)gvHealthSafetyRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlHealthSubHazardType = (RadComboBox)gvHealthSafetyRiskfooteritem.FindControl("ddlSubHazardType");
        ddlHealthSubHazardType.DataTextField = "FLDNAME";
        ddlHealthSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHazardType.SelectedHazardType));
        ddlHealthSubHazardType.DataBind();
    }
    protected void BindComapany()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            DataSet ds = PhoenixInspectionRiskAssessmentProcess.MappedVesselCompany(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        }
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        //ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(1);
        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(1, General.GetNullableInteger(ViewState["QUALITYCOMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    private void RiskAssessmentNavigationEdit()
    {
        DataTable dt = PhoenixInspectionRiskAssessmentNavigation.EditRiskAssessmentNavigation(
            General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()));

        foreach (DataRow dr in dt.Rows)
        {
            txtRefNo.Text = dr["FLDNUMBER"].ToString();
            txtRevNO.Text = dr["FLDREVISIONNO"].ToString();
            txtpreparedby.Text = dr["FLDPREPAREDBYNAME"].ToString();
            ucCreatedDate.Text = dr["FLDPREPAREDDATE"].ToString();
            txtApprovedby.Text = dr["FLDAPPROVEDBYNAME"].ToString();
            ucApprovedDate.Text = dr["FLDAPPROVEDDATE"].ToString();
            txtIssuedBy.Text = dr["FLDISSUEDBYNAME"].ToString();
            ucIssuedDate.Text = dr["FLDISSUEDDATE"].ToString();

            txtDate.Text = dr["FLDDATE"].ToString();
            //ddlActivity.SelectedMiscellaneous = dr["FLDACTIVITYID"].ToString();
            txtActivityCondition.Text = dr["FLDACTIVITYCONDITIONS"].ToString();
            txtWorkDetails.Content = dr["FLDWORKACTIVITY"].ToString();
            txtIntendedWorkDate.Text = dr["FLDINTENDEDWORKDATE"].ToString();
            if (General.GetNullableInteger(dr["FLDNUMBEROFPEOPLE"].ToString()) != null)
                rblPeopleInvolved.SelectedValue = dr["FLDNUMBEROFPEOPLE"].ToString();
            BindCheckBoxList(cblReason, dr["FLDREASON"].ToString());
            txtOtherReason.Text = dr["FLDOTHERREASON"].ToString();
            if (General.GetNullableInteger(dr["FLDWORKDURATION"].ToString()) != null)
                rblWorkDuration.SelectedValue = dr["FLDWORKDURATION"].ToString();
            if (General.GetNullableInteger(dr["FLDWORKFREQUENCY"].ToString()) != null)
                rblWorkFrequency.SelectedValue = dr["FLDWORKFREQUENCY"].ToString();
            BindCheckBoxList(cblOtherRisk, dr["FLDOTHERRISK"].ToString());
            txtOtherDetails.Text = dr["FLDOTHERRISKDETAILS"].ToString();
            if (dr["FLDOTHERRISKCONTROL"] != null && dr["FLDOTHERRISKCONTROL"].ToString() != "")
                rblOtherRiskControl.SelectedValue = dr["FLDOTHERRISKCONTROL"].ToString();
            //txtOtherRisk.Text = dr["FLDOTHERRISKPROPOSEDCONTROL"].ToString();
            rblStandByUnit.SelectedValue = dr["FLDAVALIABLESTBYUNIT"].ToString();
            BindCheckBoxList(cbllStandByUnitDetails, dr["FLDSTBYUNIT"].ToString());
            txtStandByUnitDetails.Text = dr["FLDSTBYUNITDETAIL"].ToString();
            //rblStandByEffective.SelectedValue = dr["FLDSTBYFACILITYYN"].ToString();
            BindCheckBoxList(cblProposedControl, dr["FLDSTBYCONTROL"].ToString());
            txtProposedControlDetails.Text = dr["FLDSTBYDETAIL"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            BindCheckBoxList(cblImpact, dr["FLDENVIRONMENTIMPACT"].ToString());
            txtrejectedby.Text = dr["FLDREJECTEDBYNAME"].ToString();
            ucrejecteddate.Text = dr["FLDREJECTEDDATE"].ToString();
            OtherDetailClick(null, null);

            if (dr["FLDCONTROLSADEQUATE"] != null && dr["FLDCONTROLSADEQUATE"].ToString() != "")
                rblControlsAdequate.SelectedValue = dr["FLDCONTROLSADEQUATE"].ToString();
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

            if (ddlCategory.Items.FindItemByValue(dr["FLDACTIVITYID"].ToString()) != null)
                ddlCategory.SelectedValue = dr["FLDACTIVITYID"].ToString();

            txtMasterRemarks.Text = dr["FLDAPPROVALREMARKSBYVESSEL"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDAPPROVALREMARKSBYVESSEL"].ToString()))
                chkOverrideByMaster.Checked = true;

            if (dr["FLDSTATUS"].ToString() == "2" || dr["FLDSTATUS"].ToString() == "3")
            {
                rblControlsAdequate.Enabled = true;
            }

            if (!string.IsNullOrEmpty(dr["FLDAMENDEDROUTINERAID"].ToString()))
                txtRAIdEdit.Text = dr["FLDAMENDEDROUTINERAID"].ToString();
            txtRANumberEdit.Text = dr["FLDPROCESSRANUMBER"].ToString();
            txtRAEdit.Text = dr["FLDPROCESSNAME"].ToString();

            txtAdditionalSafetyProcedures.Content = dr["FLDADDITIONALSAFETYPROCEDURE"].ToString();
            txtAternativeMethod.Text = dr["FLDALTERNATEWORKMETHOD"].ToString();

            if (dr["FLDNAVIGATIONRAVERIFEDYN"].ToString() == "0" || dr["FLDNAVIGATIONRAVERIFEDYN"].ToString() == "1")
            {
                rblVerifcation.SelectedValue = dr["FLDNAVIGATIONRAVERIFEDYN"].ToString();

                if (dr["FLDNAVIGATIONRAVERIFEDYN"].ToString() == "0")
                    txtVerificationRemarks.CssClass = "input_mandatory";
                else
                    txtVerificationRemarks.CssClass = "input";
            }
            txtVerificationRemarks.Text = dr["FLDVERIFICATIONREMARKS"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDCOMPANYID"].ToString()))
            {
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                txtpreparedby.Enabled = true;
                txtpreparedby.CssClass = "input";
                txtpreparedby.ReadOnly = false;

                ucCreatedDate.Enabled = true;
                ucCreatedDate.ReadOnly = false;
                ucCreatedDate.CssClass = "input";

                if (dr["FLDSTATUS"].ToString() == "5" || dr["FLDSTATUS"].ToString() == "6")
                {
                    txtApprovedby.Enabled = true;
                    txtApprovedby.CssClass = "input";
                    txtApprovedby.ReadOnly = false;
                    txtIssuedBy.ReadOnly = false;
                    txtIssuedBy.CssClass = "input";
                    txtIssuedBy.Enabled = true;
                    txtrejectedby.Enabled = true;
                    txtrejectedby.CssClass = "input";
                    txtrejectedby.ReadOnly = false;

                    ucApprovedDate.Enabled = true;
                    ucApprovedDate.ReadOnly = false;
                    ucApprovedDate.CssClass = "input";
                    ucrejecteddate.Enabled = true;
                    ucrejecteddate.CssClass = "input";
                    ucrejecteddate.ReadOnly = false;
                    ucIssuedDate.Enabled = true;
                    ucIssuedDate.ReadOnly = false;
                    ucIssuedDate.CssClass = "input";
                }
            }

            BindScore();
        }
    }

    private void BindData()
    {
        rblPeopleInvolved.DataBindings.DataTextField = "FLDNAME";
        rblPeopleInvolved.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblPeopleInvolved.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(3);
        rblPeopleInvolved.DataBind();

        cblReason.DataBindings.DataTextField = "FLDNAME";
        cblReason.DataBindings.DataValueField = "FLDMISCELLANEOUSID";
        cblReason.DataSource = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(1, null);
        cblReason.DataBind();

        cblOtherRisk.DataBindings.DataTextField = "FLDNAME";
        cblOtherRisk.DataBindings.DataValueField = "FLDHAZARDID";
        cblOtherRisk.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(3, 0);
        cblOtherRisk.DataBind();

        rblWorkDuration.DataBindings.DataTextField = "FLDNAME";
        rblWorkDuration.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblWorkDuration.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(1);
        rblWorkDuration.DataBind();

        rblWorkFrequency.DataBindings.DataTextField = "FLDNAME";
        rblWorkFrequency.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblWorkFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(2, "OFP");
        rblWorkFrequency.DataBind();
        rblWorkFrequency.SelectedIndex = 0;

        rblOtherRiskControl.DataBindings.DataTextField = "FLDNAME";
        rblOtherRiskControl.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblOtherRiskControl.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
        rblOtherRiskControl.DataBind();

        cbllStandByUnitDetails.DataBindings.DataTextField = "FLDITEM";
        cbllStandByUnitDetails.DataBindings.DataValueField = "FLDID";
        cbllStandByUnitDetails.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
                                            Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAD")), 1);
        cbllStandByUnitDetails.DataBind();

        cblProposedControl.DataBindings.DataTextField = "FLDITEM";
        cblProposedControl.DataBindings.DataValueField = "FLDID";
        cblProposedControl.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
                                            Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAP")), 1);
        cblProposedControl.DataBind();

    }
    protected void BindGrid()
    {
        BindGridEconomicRisk();
        BindGridEnvironmentalRisk();
        BindGridHealthSafetyRisk();
        BindGridRiskQuestions();
        BindGridNavigationSafety();
    }

    protected void BindScore()
    {
        decimal minscore = 0, maxscore = 0;

        DataSet ds = PhoenixInspectionRiskAssessmentNavigation.NavigationScores(General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            lblHarzardHealth.Text = dr["FLDHEALTHHAZARD"].ToString();
            lblHazardEnv.Text = dr["FLDENVIOHAZARD"].ToString();
            lblHazardEconomic.Text = dr["FLDECONOMICHAZRD"].ToString();
            lblHazardWorst.Text = dr["FLDWORSTHAZARD"].ToString();

            lblProbhealth.Text = dr["FLDHSPO"].ToString();
            lblProbEnv.Text = dr["FLDENVPO"].ToString();
            lblProbEcomoic.Text = dr["FLDENVPO"].ToString();
            lblProbWorst.Text = dr["FLDENVPO"].ToString();

            lblLikelihoodHealth.Text = dr["FLDHSLH"].ToString();
            lblLikelihoodEnv.Text = dr["FLDENVLH"].ToString();
            lblLikelihoodEconomic.Text = dr["FLDECOLH"].ToString();
            lblLikelihoodWorst.Text = dr["FLDWCLH"].ToString();

            lblLevelOfControlHealth.Text = dr["FLDLEVELOFCONTROL"].ToString();
            lblLevelOfControlEnv.Text = dr["FLDLEVELOFCONTROL"].ToString();
            lblLevelOfControlEconomic.Text = dr["FLDLEVELOFCONTROL"].ToString();
            lblLevelOfControlWorst.Text = dr["FLDLEVELOFCONTROL"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDMINSCORE"].ToString()))
                minscore = decimal.Parse(dr["FLDMINSCORE"].ToString());

            if (!string.IsNullOrEmpty(dr["FLDMAXSCORE"].ToString()))
                maxscore = decimal.Parse(dr["FLDMAXSCORE"].ToString());

            lblLevelofRiskHealth.Text = dr["FLDHSLR"].ToString();

            if (!string.IsNullOrEmpty(lblLevelofRiskHealth.Text))
            {
                if (decimal.Parse(lblLevelofRiskHealth.Text) <= minscore)
                    levelofriskhealth.BgColor = "Lime";
                else if (decimal.Parse(lblLevelofRiskHealth.Text) > minscore && decimal.Parse(lblLevelofRiskHealth.Text) <= maxscore)
                    levelofriskhealth.BgColor = "Yellow";
                else if (decimal.Parse(lblLevelofRiskHealth.Text) > maxscore)
                    levelofriskhealth.BgColor = "Red";
            }
            else
                levelofriskhealth.BgColor = "White";

            lblLevelofRiskEnv.Text = dr["FLDENVLR"].ToString();

            if (!string.IsNullOrEmpty(lblLevelofRiskEnv.Text))
            {
                if (decimal.Parse(lblLevelofRiskEnv.Text) <= minscore)
                    levelofriskenv.BgColor = "Lime";
                else if (decimal.Parse(lblLevelofRiskEnv.Text) > minscore && decimal.Parse(lblLevelofRiskEnv.Text) <= maxscore)
                    levelofriskenv.BgColor = "Yellow";
                else if (decimal.Parse(lblLevelofRiskEnv.Text) > maxscore)
                    levelofriskenv.BgColor = "Red";
            }
            else
                levelofriskenv.BgColor = "White";

            lblLevelofRiskEconomic.Text = dr["FLDECOLR"].ToString();

            if (!string.IsNullOrEmpty(lblLevelofRiskEconomic.Text))
            {
                if (decimal.Parse(lblLevelofRiskEconomic.Text) <= minscore)
                    levelofriskeco.BgColor = "Lime";
                else if (decimal.Parse(lblLevelofRiskEconomic.Text) > minscore && decimal.Parse(lblLevelofRiskEconomic.Text) <= maxscore)
                    levelofriskeco.BgColor = "Yellow";
                else if (decimal.Parse(lblLevelofRiskEconomic.Text) > maxscore)
                    levelofriskeco.BgColor = "Red";
            }
            else
                levelofriskeco.BgColor = "White";

            lblLevelofRiskWorst.Text = dr["FLDWCLR"].ToString();

            if (!string.IsNullOrEmpty(lblLevelofRiskWorst.Text))
            {
                if (decimal.Parse(lblLevelofRiskWorst.Text) <= minscore)
                    levelofriskworst.BgColor = "Lime";
                else if (decimal.Parse(lblLevelofRiskWorst.Text) > minscore && decimal.Parse(lblLevelofRiskWorst.Text) <= maxscore)
                    levelofriskworst.BgColor = "Yellow";
                else if (decimal.Parse(lblLevelofRiskWorst.Text) > maxscore)
                    levelofriskworst.BgColor = "Red";
            }
            else
                levelofriskworst.BgColor = "White";
        }

    }

    protected void MenuNavigation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveRiskAssessmentNavigation();
            }

            if (CommandName.ToUpper().Equals("RESAVE"))
            {
                SaveRiskAssessmentNavigation();
            }

            if (CommandName.ToUpper().Equals("REAPPROVE"))
            {
                if (ViewState["RISKASSESSMENTNAVIGATIONID"] != null && ViewState["RISKASSESSMENTNAVIGATIONID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentNavigation.RequestApprovalNavigation(new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()));
                    ucStatus.Text = "Requested Successfully";
                }
            }

            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["SHOWALL"].ToString().Equals("1"))
                {
                    Response.Redirect("../Inspection/InspectionNonRoutineRiskAssessmentList.aspx", false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionMainFleetRANavigationList.aspx", false);
                }
            }
            if (CommandName.ToUpper().Equals("VERIFY"))
            {
                VerifyRiskAssessmentNavigation();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if ((ViewState["RATYPE"].ToString() == "3") && (ViewState["mocextention"].ToString() == ""))
                {
                    Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTNAVIGATIONID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
                }
                if ((ViewState["RATYPE"].ToString() == "3") && (ViewState["mocextention"].ToString() == "yes"))
                {
                    Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTNAVIGATIONID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
                }
            }

            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (ViewState["COPYTYPE"].ToString() == "1")
                {
                    if (ViewState["RISKASSESSMENTNAVIGATIONID"] != null && ViewState["RISKASSESSMENTNAVIGATIONID"].ToString() != "")
                    {
                        PhoenixInspectionRiskAssessmentNavigation.CopyNavigation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                        ucStatus.Text = "Copied Successfully";

                        String script = String.Format("javascript:fnReloadList('wo','ifMoreInfo','','');");
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                    }
                }
                if (ViewState["COPYTYPE"].ToString() == "2")
                {
                    if (ViewState["RISKASSESSMENTNAVIGATIONID"] != null && ViewState["RISKASSESSMENTNAVIGATIONID"].ToString() != "")
                    {
                        PhoenixInspectionRiskAssessmentNavigation.NavigationProposeTemplate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                        ucStatus.Text = "Copied Successfully";

                        String script = String.Format("javascript:fnReloadList('wo','ifMoreInfo','','');");
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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


    private void SaveRiskAssessmentNavigation()
    {
        try
        {
            string recorddate = txtDate.Text;
            string assessmentdate = txtIntendedWorkDate.Text;
            string peopleinvolved = rblPeopleInvolved.SelectedValue;
            string reasonforassessment = ReadCheckBoxList(cblReason);
            string otherreason = txtOtherReason.Text;
            string aternativemethod = txtAternativeMethod.Text;
            string durationofworkactivity = rblWorkDuration.SelectedValue;
            string frequencyofworkactivity = rblWorkFrequency.SelectedValue;
            string otherrisk = ReadCheckBoxList(cblOtherRisk);
            string otherriskdetails = txtOtherDetails.Text;
            string otherriskcontrol = rblOtherRiskControl.SelectedValue;
            string stbyunit = ReadCheckBoxList(cbllStandByUnitDetails);
            string stbycontrols = ReadCheckBoxList(cblProposedControl);


            if (!IsValidNavigationTemplate())
            {
                ucError.Visible = true;
                return;
            }
            Guid? riskassessmentNavigationidout = Guid.NewGuid();
            PhoenixInspectionRiskAssessmentNavigation.InsertRiskAssessmentNavigation(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
                General.GetNullableDateTime(recorddate),
                General.GetNullableDateTime(assessmentdate),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                HttpUtility.HtmlDecode(txtWorkDetails.Content), durationofworkactivity, frequencyofworkactivity, General.GetNullableInteger(rblPeopleInvolved.SelectedValue),
                null, null, reasonforassessment, otherreason,
                null, null, null, null, null, null,
                otherrisk, otherriskdetails, null, null,
                General.GetNullableInteger(rblStandByUnit.SelectedValue),
                stbyunit, txtStandByUnitDetails.Text, General.GetNullableInteger(null),
                stbycontrols, txtProposedControlDetails.Text, txtRemarks.Text, ReadCheckBoxList(cblImpact),
                //General.GetNullableInteger(ddlActivity.SelectedMiscellaneous),
                General.GetNullableString(txtActivityCondition.Text),
                ref riskassessmentNavigationidout,
                General.GetNullableInteger(rblControlsAdequate.SelectedValue),
                General.GetNullableInteger(ddlCategory.SelectedValue),
                General.GetNullableGuid(txtRAIdEdit.Text),
                General.GetNullableString(HttpUtility.HtmlDecode(txtAdditionalSafetyProcedures.Content)),
                General.GetNullableInteger(ucCompany.SelectedCompany),
                General.GetNullableString(aternativemethod),
                General.GetNullableString(txtpreparedby.Text.Trim()),
                General.GetNullableDateTime(ucCreatedDate.Text),
                General.GetNullableString(txtApprovedby.Text.Trim()),
                General.GetNullableDateTime(ucApprovedDate.Text),
                General.GetNullableString(txtIssuedBy.Text.Trim()),
                General.GetNullableDateTime(ucIssuedDate.Text),
                General.GetNullableString(txtrejectedby.Text.Trim()),
                General.GetNullableDateTime(ucrejecteddate.Text)
                );

            ucStatus.Text = "Navigation Template updated.";
            ViewState["RISKASSESSMENTNAVIGATIONID"] = riskassessmentNavigationidout.ToString();
            Filter.CurrentSelectedNavigationRA = riskassessmentNavigationidout.ToString();
            BindData();
            RiskAssessmentNavigationEdit();
            BindGridNavigationSafety();
            BindGrid();
            BindScore();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VerifyRiskAssessmentNavigation()
    {
        try
        {
            PhoenixInspectionRiskAssessmentNavigation.RANavigationVerify(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"] != null ? ViewState["RISKASSESSMENTNAVIGATIONID"].ToString() : "")
                , General.GetNullableInteger(rblVerifcation.SelectedValue)
                , General.GetNullableString(txtVerificationRemarks.Text));

            ucStatus.Text = "RA Completed";
            ucStatus.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void OtherDetailClick(object sender, EventArgs e)
    {
        string reason = ReadCheckBoxList(cblReason);
        string otherrisk = ReadCheckBoxList(cblOtherRisk);
        string standbyunit = ReadCheckBoxList(cbllStandByUnitDetails);
        string proposedcontrol = ReadCheckBoxList(cblProposedControl);
        string proposedcontrolname = ReadCheckBoxName(cblProposedControl);
        string standbyunitname = ReadCheckBoxName(cbllStandByUnitDetails);

        if (reason.Contains("100"))
        {

            txtOtherReason.CssClass = "input";
            txtOtherReason.ReadOnly = false;
        }
        else
        {
            txtOtherReason.Text = "";
            txtOtherReason.ReadOnly = true;
            txtOtherReason.CssClass = "readonlyRadTextBox";
        }
        if (otherrisk.Contains("100"))
        {

            txtOtherDetails.CssClass = "input";
            txtOtherDetails.ReadOnly = false;
        }
        else
        {
            txtOtherDetails.Text = "";
            txtOtherDetails.ReadOnly = true;
            txtOtherDetails.CssClass = "readonlyRadTextBox";
        }
        if (standbyunitname.ToUpper().Contains("OTHER"))
        {

            txtStandByUnitDetails.CssClass = "input";
            txtStandByUnitDetails.ReadOnly = false;
        }
        else
        {
            txtStandByUnitDetails.Text = "";
            txtStandByUnitDetails.ReadOnly = true;
            txtStandByUnitDetails.CssClass = "readonlyRadTextBox";
        }
        if (proposedcontrolname.ToUpper().Contains("OTHER"))
        {

            txtProposedControlDetails.CssClass = "input";
            txtProposedControlDetails.ReadOnly = false;
        }
        else
        {
            txtProposedControlDetails.Text = "";
            txtProposedControlDetails.ReadOnly = true;
            txtProposedControlDetails.CssClass = "readonlyRadTextBox";
        }
    }

    private string ReadCheckBoxName(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Text.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                cbl.SelectedValue = item;
            }
        }
    }

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void gvHealthSafetyRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGridHealthSafetyRisk();
    }

    private void BindGridHealthSafetyRisk()
    {

        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentNavigation.ListNavigationCategory(1, General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        gvHealthSafetyRisk.DataSource = ds;

    }

    protected void gvEnvironmentalRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGridEnvironmentalRisk();
    }

    private void BindGridEnvironmentalRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentNavigation.ListNavigationCategory(2, General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        gvEnvironmentalRisk.DataSource = ds;
    }

    protected void gvEconomicRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGridEconomicRisk();
    }

    private void BindGridEconomicRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentNavigation.ListNavigationCategory(4, General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        gvEconomicRisk.DataSource = ds;

    }

    protected void gvPersonal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGridRiskQuestions();
    }

    private void BindGridNavigationSafety()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentNavigation.ListNavigationSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString())
            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , null);
        gvNavigationSafety.DataSource = ds;

    }
    private bool IsValidNavigationTemplate()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableDateTime(txtIntendedWorkDate.Text) == null)
            ucError.ErrorMessage = "Date of intended work activity is required.";

        if (General.GetNullableInteger(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableString(txtActivityCondition.Text) == null)
            ucError.ErrorMessage = "Activity is required.";

        if (General.GetNullableString(txtWorkDetails.Content) == null)
            ucError.ErrorMessage = "Risks/Aspects is required.";

        return (!ucError.IsError);
    }

    private void BindGridRiskQuestions()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentNavigation.ListNavigationControl(General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        gvPersonal.DataSource = ds;

    }
    protected void gvPersonal_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("RDELETE"))
            {
                string controlid = ((RadLabel)gce.Item.FindControl("lblId")).Text;
                if (General.GetNullableGuid(controlid) != null)
                {
                    PhoenixInspectionRiskAssessmentNavigation.DeleteNavigationControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(controlid));
                }
                BindGridRiskQuestions();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvPersonal_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = e.NewEditIndex;
    //    _gridView.SelectedIndex = e.NewEditIndex;
    //    BindGridRiskQuestions();
    //}

    protected void gvPersonal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidSpareTools(((RadRadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblOptionEdit")).SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            string lblId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblId")).Text;
            RadLabel lblItemid = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblItemid");
            if (lblId != null && lblId != "")
            {
                PhoenixInspectionRiskAssessmentNavigation.UpdateNavigationControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(lblId), new Guid(lblItemid.Text),
                                                            int.Parse(((RadRadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblOptionEdit")).SelectedValue));
            }
            else
            {
                PhoenixInspectionRiskAssessmentNavigation.InsertNavigationControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(lblItemid.Text), int.Parse(((RadRadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblOptionEdit")).SelectedValue),
                                                            new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
                                                            int.Parse(ViewState["VESSELID"].ToString()));
            }
            _gridView.EditIndex = -1;
            BindGridRiskQuestions();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPersonal_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadRadioButtonList rblOptionEdit = (RadRadioButtonList)e.Item.FindControl("rblOptionEdit");
            if (rblOptionEdit != null)
            {
                rblOptionEdit.SelectedValue = dr["FLDOPTIONID"].ToString();
            }
        }
    }

    //protected void gvPersonal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindGridRiskQuestions();
    //}

    //protected void gvPersonal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    BindGridRiskQuestions();
    //}

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

    protected void gvEconomicRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }
    protected void gvEconomicRisk_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("CADD"))
            {

                if (!IsValidHazard(ucEconomicHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentNavigation.InsertNavigationCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEconomicHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucEconomicHazardType.SelectedHazardType = "";
                ucStatus.Text = "Hazard Added.";

            }
            else if (gce.CommandName.ToUpper().Equals("CDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentNavigation.DeleteNavigationCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                ucStatus.Text = "Hazard Deleted.";
            }
            BindGridEconomicRisk();
            gvEconomicRisk.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("EADD"))
            {

                if (!IsValidHazard(ucEnvHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, 2,
                                        ((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentNavigation.InsertNavigationCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEnvHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text),
                        General.GetNullableInteger(((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous)
                        );

                ucEnvHazardType.SelectedHazardType = "";
                BindGridEnvironmentalRisk();
                gvEnvironmentalRisk.Rebind();
                ucStatus.Text = "Hazard Added.";

            }
            else if (gce.CommandName.ToUpper().Equals("EDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentNavigation.DeleteNavigationCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridEnvironmentalRisk();
                gvEnvironmentalRisk.Rebind();
                ucStatus.Text = "Hazard Deleted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEnvironmentalRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
            UserControlRAMiscellaneous ucImpactType = (UserControlRAMiscellaneous)ge.Item.FindControl("ucImpactType");
            if (ucImpactType != null)
            {
                ucImpactType.MiscellaneousList = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(3, 0);
                ucImpactType.DataBind();
            }

            LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }
    protected void gvHealthSafetyRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("HADD"))
            {

                if (!IsValidHazard(ucHazardType.SelectedHazardType,
                                        ((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentNavigation.InsertNavigationCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)e.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucHazardType.SelectedHazardType = "";
                BindGridHealthSafetyRisk();
                gvHealthSafetyRisk.Rebind();
                ucStatus.Text = "Hazard Added.";

            }
            else if (e.CommandName.ToUpper().Equals("HDELETE"))
            {
                string categoryid = ((RadLabel)e.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentNavigation.DeleteNavigationCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridHealthSafetyRisk();
                gvHealthSafetyRisk.Rebind();
                ucStatus.Text = "Hazard Deleted.";
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvNavigationSafety_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindGridNavigationSafety();
    }

    protected void gvNavigationSafety_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton btnPICEdit = (LinkButton)e.Item.FindControl("btnPICEdit");
            if (btnPICEdit != null)
            {
                btnPICEdit.Attributes.Add("onclick", "return showPickList('spnPICEdit', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + ViewState["VESSELID"].ToString() + "', true); ");
            }
            RadTextBox txtEstimatedStartTimeEdit = (RadTextBox)e.Item.FindControl("txtEstimatedStartTimeEdit");
            RadTextBox txtEstimatedFinishTimeEdit = (RadTextBox)e.Item.FindControl("txtEstimatedFinishTimeEdit");

            RadTextBox txtActualStartTimeEdit = (RadTextBox)e.Item.FindControl("txtActualStartTimeEdit");
            RadTextBox txtActualFinishTimeEdit = (RadTextBox)e.Item.FindControl("txtActualFinishTimeEdit");

            if (txtEstimatedStartTimeEdit != null)
                txtEstimatedStartTimeEdit.Text = drv["FLDESTIMATEDSTARTTIME"].ToString();

            if (txtEstimatedFinishTimeEdit != null)
                txtEstimatedFinishTimeEdit.Text = drv["FLDESTIMATEDFINISHTIME"].ToString();

            if (txtActualStartTimeEdit != null)
                txtActualStartTimeEdit.Text = drv["FLDACTUALSTARTTIME"].ToString();

            if (txtActualFinishTimeEdit != null)
                txtActualFinishTimeEdit.Text = drv["FLDACTUALFINISHTIME"].ToString();

            RadTextBox txtPICNameEdit = (RadTextBox)e.Item.FindControl("txtPICNameEdit");
            RadTextBox txtPICIdEdit = (RadTextBox)e.Item.FindControl("txtPICIdEdit");
            RadTextBox txtPICRankEdit = (RadTextBox)e.Item.FindControl("txtPICRankEdit");

            if (txtPICNameEdit != null)
            {
                txtPICNameEdit.Text = drv["FLDPICNAME"].ToString();
            }
            if (txtPICIdEdit != null)
            {
                txtPICIdEdit.Text = drv["FLDPIC"].ToString();
            }
            if (txtPICRankEdit != null)
            {
                txtPICRankEdit.Text = drv["FLDRANKNAME"].ToString();
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton btnPICAdd = (LinkButton)e.Item.FindControl("btnPICAdd");

            if (btnPICAdd != null)
            {
                btnPICAdd.Attributes.Add("onclick", "return showPickList('spnPICAdd', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + ViewState["VESSELID"].ToString() + "', true); ");
            }

        }
    }
    protected void gvNavigationSafety_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("SADD"))
            {
                string serialno = ((UserControlMaskNumber)gce.Item.FindControl("ucSNoAdd")).Text;
                string task = ((RadTextBox)gce.Item.FindControl("txtTaskAdd")).Text;
                string pic = ((RadTextBox)gce.Item.FindControl("txtPICIdAdd")).Text;

                string estimatedstartdate = ((UserControlDate)gce.Item.FindControl("ucEstimatedStartDateAdd")).Text;
                string estimatedfinishdate = ((UserControlDate)gce.Item.FindControl("ucEstimatedFinishDateAdd")).Text;

                string estimatedstarttime = ((RadTextBox)gce.Item.FindControl("txtEstimatedStartTimeAdd")).Text;
                string estimatedfinishtime = ((RadTextBox)gce.Item.FindControl("txtEstimatedFinishTimeAdd")).Text;

                estimatedstarttime = estimatedstarttime.Trim() == "__:__" ? string.Empty : estimatedstarttime;
                estimatedfinishtime = estimatedfinishtime.Trim() == "__:__" ? string.Empty : estimatedfinishtime;

                estimatedstarttime = (estimatedstarttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : estimatedstarttime;
                estimatedfinishtime = (estimatedfinishtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : estimatedfinishtime;

                string estimatedstartdatetime = estimatedstartdate + " " + estimatedstarttime;
                string estimatedfinishdatetime = estimatedfinishdate + " " + estimatedfinishtime;

                if (!IsValidSafety(task, estimatedstartdate, estimatedstarttime, estimatedfinishdate, estimatedfinishtime))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionRiskAssessmentNavigation.InsertNavigationSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString())
                        , null
                        , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                        , General.GetNullableInteger(serialno)
                        , General.GetNullableString(task)
                        , General.GetNullableInteger(pic)
                        , General.GetNullableDateTime(estimatedstartdatetime)
                        , General.GetNullableDateTime(estimatedfinishdatetime)
                        , null
                        , null
                        );
                    BindGridNavigationSafety();
                    gvNavigationSafety.Rebind();
                }
            }
            else if (gce.CommandName.ToUpper().Equals("SDELETE"))
            {
                string navigationsafteyid = ((RadLabel)gce.Item.FindControl("lblNavigationSafetyId")).Text;
                PhoenixInspectionRiskAssessmentNavigation.DeleteNavigationSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString())
                    , General.GetNullableGuid(navigationsafteyid)
                    );
                BindGridNavigationSafety();
                gvNavigationSafety.Rebind();
            }
            else if (gce.CommandName.ToUpper().Equals("SUPDATE"))
            {
                string estimatedstartdate = "";
                string estimatedfinishdate = "";
                string actualstartdate = "";
                string actualfinishdate = "";

                string estimatedstarttime = "";
                string estimatedfinishtime = "";
                string actualstarttime = "";
                string actualfinishtime = "";

                string navigationsafteyid = ((RadLabel)gce.Item.FindControl("lblNavigationSafetyIdEdit")).Text;
                string serialno = ((UserControlMaskNumber)gce.Item.FindControl("ucSNoEdit")).Text;
                string task = ((RadTextBox)gce.Item.FindControl("txtTaskEdit")).Text;
                string pic = ((RadTextBox)gce.Item.FindControl("txtPICIdEdit")).Text;

                estimatedstartdate = ((UserControlDate)gce.Item.FindControl("ucEstimatedStartDateEdit")).Text;
                estimatedfinishdate = ((UserControlDate)gce.Item.FindControl("ucEstimatedFinishDateEdit")).Text;

                actualstartdate = ((UserControlDate)gce.Item.FindControl("ucActualStartDateEdit")).Text;
                actualfinishdate = ((UserControlDate)gce.Item.FindControl("ucActualFinishDateEdit")).Text;

                estimatedstarttime = ((RadTextBox)gce.Item.FindControl("txtEstimatedStartTimeEdit")).Text;
                estimatedfinishtime = ((RadTextBox)gce.Item.FindControl("txtEstimatedFinishTimeEdit")).Text;

                estimatedstarttime = estimatedstarttime.Trim() == "__:__" ? string.Empty : estimatedstarttime;
                estimatedfinishtime = estimatedfinishtime.Trim() == "__:__" ? string.Empty : estimatedfinishtime;

                actualstarttime = ((RadTextBox)gce.Item.FindControl("txtActualStartTimeEdit")).Text;
                actualfinishtime = ((RadTextBox)gce.Item.FindControl("txtActualFinishTimeEdit")).Text;

                actualstarttime = actualstarttime.Trim() == "__:__" ? string.Empty : actualstarttime;
                actualfinishtime = actualfinishtime.Trim() == "__:__" ? string.Empty : actualfinishtime;

                estimatedstarttime = (estimatedstarttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : estimatedstarttime;
                estimatedfinishtime = (estimatedfinishtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : estimatedfinishtime;

                actualstarttime = (actualstarttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : actualstarttime;
                actualfinishtime = (actualfinishtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : actualfinishtime;

                string estimatedstartdatetime = estimatedstartdate + " " + estimatedstarttime;
                string estimatedfinishdatetime = estimatedfinishdate + " " + estimatedfinishtime;

                string actualstartdatetime = actualstartdate + " " + actualstarttime;
                string actualfinishdatetime = actualfinishdate + " " + actualfinishtime;

                if (!IsValidSafety(task, estimatedstartdate, estimatedstarttime, estimatedfinishdate, estimatedfinishtime))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionRiskAssessmentNavigation.InsertNavigationSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString())
                        , General.GetNullableGuid(navigationsafteyid)
                        , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                        , General.GetNullableInteger(serialno)
                        , General.GetNullableString(task)
                        , General.GetNullableInteger(pic)
                        , General.GetNullableDateTime(estimatedstartdatetime)
                        , General.GetNullableDateTime(estimatedfinishdatetime)
                        , General.GetNullableDateTime(actualstartdatetime)
                        , General.GetNullableDateTime(actualfinishdatetime)
                        );
                    BindGridNavigationSafety();
                    gvNavigationSafety.Rebind();
                }
            }
            gvNavigationSafety.Rebind();
            BindGridNavigationSafety();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSafety(string task, string estimatedstartdate, string estimatedstarttime, string estimatedfinishdate, string estimatedfinishtime)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableString(task) == null)
        {
            ucError.ErrorMessage = "Task is required";
        }
        if (General.GetNullableDateTime(estimatedstartdate) == null)
        {
            ucError.ErrorMessage = "Estimated start date is required";
        }
        if (General.GetNullableDateTime(estimatedfinishdate) == null)
        {
            ucError.ErrorMessage = "Estimated finish date is required";
        }
        else if (General.GetNullableDateTime(estimatedstartdate) != null && General.GetNullableDateTime(estimatedfinishdate) != null)
        {
            string estimatedstartdatetime = estimatedstartdate + " " + estimatedstarttime;
            string estimatedfinishdatetime = estimatedfinishdate + " " + estimatedfinishtime;

            if (General.GetNullableDateTime(estimatedstartdatetime) == null)
            {
                ucError.ErrorMessage = "Invalid Estimated start datetime";
            }
            if (General.GetNullableDateTime(estimatedfinishdatetime) == null)
            {
                ucError.ErrorMessage = "Invalid Estimated finish datetime";
            }
            else if (General.GetNullableDateTime(estimatedstartdatetime) != null && General.GetNullableDateTime(estimatedfinishdatetime) != null)
            {
                if (DateTime.TryParse(estimatedstartdatetime, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(estimatedfinishdatetime)) > 0)
                    ucError.ErrorMessage = "Estimated start date be earlier than Estimated finish date";
            }
        }
        return (!ucError.IsError);
    }
    protected void rblVerifcation_Changed(object sender, EventArgs e)
    {
        if (rblVerifcation != null)
        {
            if (rblVerifcation.SelectedItem.Value == "0")
                txtVerificationRemarks.CssClass = "input_mandatory";
            else
                txtVerificationRemarks.CssClass = "input";
        }
    }

    private bool IsValidHazard(string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()) != null)
        {
            if (General.GetNullableInteger(hazardtypeid) == null)
                ucError.ErrorMessage = "Hazard Type is required.";

            if (type != null && type.ToString() == "2")
            {
                if (General.GetNullableInteger(impacttype) == null)
                    ucError.ErrorMessage = "Impact Type is required.";
            }

            if (General.GetNullableGuid(subhazardid) == null)
                ucError.ErrorMessage = "Impact is required.";
        }

        return (!ucError.IsError);

    }
    private bool IsValidSpareTools(string option)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try answering.";

        return (!ucError.IsError);
    }

    public void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadRadioButtonList rblOptions = (RadRadioButtonList)sender;

            GridDataItem gvrow = (GridDataItem)rblOptions.Parent.Parent;

            string lblId = ((RadLabel)gvrow.FindControl("lblId")).Text;

            RadLabel lblItemid = (RadLabel)gvrow.FindControl("lblItemid");

            if (lblId != null && lblId != "")
            {
                if (!IsValidSpareTools(lblItemid.Text))
                {
                    ucError.Visible = true;
                    BindGridRiskQuestions();
                    return;
                }
                PhoenixInspectionRiskAssessmentNavigation.UpdateNavigationControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(lblId), new Guid(lblItemid.Text),
                                                            int.Parse(((RadRadioButtonList)gvrow.FindControl("rblOptionEdit")).SelectedValue));
            }
            else
            {
                if (!IsValidSpareTools(lblItemid.Text))
                {
                    ucError.Visible = true;
                    BindGridRiskQuestions();
                    return;
                }
                PhoenixInspectionRiskAssessmentNavigation.InsertNavigationControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(lblItemid.Text), int.Parse(((RadRadioButtonList)gvrow.FindControl("rblOptionEdit")).SelectedValue),
                                                            new Guid(ViewState["RISKASSESSMENTNAVIGATIONID"].ToString()),
                                                            int.Parse(ViewState["VESSELID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindGridRiskQuestions();
        }
    }
}
