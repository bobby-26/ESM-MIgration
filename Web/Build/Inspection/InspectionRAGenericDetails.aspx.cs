using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web;
using Telerik.Web.UI;
public partial class InspectionRAGenericDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            rblControlsAdequate.Enabled = false;
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["status"].ToString() != "3" && Request.QueryString["status"].ToString() != "5" && Request.QueryString["status"].ToString() != "6" && Request.QueryString["status"].ToString() != "7") //Issued
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
        MenuGeneric.AccessRights = this.ViewState;
        MenuGeneric.MenuList = toolbar.Show();


        if (!IsPostBack)
        { 
            DateTime dt = DateTime.Today;
            txtDate.Text = dt.ToString();
            ViewState["RISKASSESSMENTGENERICID"] = "";
            ViewState["VESSELID"] = "";
            txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;

            ViewState["QUALITYCOMPANYID"] = "";
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
            if (Request.QueryString["genericid"] != null)
            {
                ViewState["RISKASSESSMENTGENERICID"] = Request.QueryString["genericid"].ToString();
                RiskAssessmentGenericEdit();
                if (Request.QueryString["genericid"].ToString() != "")
                    BindScore();
            }
        }
        BindGrid();
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

    protected void BindGrid()
    {
        BindGridEconomicRisk();
        BindGridEnvironmentalRisk();
        BindGridHealthSafetyRisk();
        BindGridGenericSafety();
    }

    protected void BindScore()
    {
        decimal minscore = 0, maxscore = 0;

        DataSet ds = PhoenixInspectionRiskAssessmentGeneric.GenericScores(General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()));

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

    protected void BindCategory()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(2, General.GetNullableInteger(ViewState["QUALITYCOMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.DataValueField = "FLDACTIVITYID";
            ddlCategory.DataTextField = "FLDNAME";
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    private void RiskAssessmentGenericEdit()
    {
        DataTable dt = PhoenixInspectionRiskAssessmentGeneric.EditPhoenixInspectionRiskAssessmentGeneric(
            General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()));

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
            if (dr["FLDCONTROLSADEQUATE"] != null && dr["FLDCONTROLSADEQUATE"].ToString() != "")
                rblControlsAdequate.SelectedValue = dr["FLDCONTROLSADEQUATE"].ToString();
            BindCheckBoxList(cblProposedControl, dr["FLDPROPOSEDCONTROLS"].ToString());
            txtProposedControlDetails.Text = dr["FLDOTHERRISKPROPOSEDCONTROL"].ToString();
            OtherDetailClick(null, null);
            BindCheckBoxList(cblImpact, dr["FLDENVIRONMENTIMPACT"].ToString());
            OtherDetailClick(null, null);
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

            if (ddlCategory.Items.FindItemByValue(dr["FLDACTIVITYID"].ToString()) != null)
                ddlCategory.SelectedValue = dr["FLDACTIVITYID"].ToString();

            txtMasterRemarks.Text = dr["FLDAPPROVALREMARKSBYVESSEL"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDAPPROVALREMARKSBYVESSEL"].ToString()))
                chkOverrideByMaster.Checked = true;

            if (dr["FLDSTATUS"].ToString() == "2" || dr["FLDSTATUS"].ToString() == "3")
                rblControlsAdequate.Enabled = true;
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            if (!string.IsNullOrEmpty(dr["FLDAMENDEDROUTINERAID"].ToString()))
                ddlAmendedTo.SelectedValue = dr["FLDAMENDEDROUTINERAID"].ToString();

            txtAdditionalSafetyProcedures.Content = dr["FLDADDITIONALSAFETYPROCEDURE"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDCOMPANYID"].ToString()))
            {
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;
            txtAternativeMethod.Text = dr["FLDALTERNATEWORKMETHOD"].ToString();
            if (dr["FLDGENERICRAVERIFEDYN"].ToString() == "0" || dr["FLDGENERICRAVERIFEDYN"].ToString() == "1")
            {
                rblVerifcation.SelectedValue = dr["FLDGENERICRAVERIFEDYN"].ToString();

                if (dr["FLDGENERICRAVERIFEDYN"].ToString() == "0")
                    txtVerificationRemarks.CssClass = "input_mandatory";
                else
                    txtVerificationRemarks.CssClass = "input";
            }            
            txtVerificationRemarks.Text = dr["FLDVERIFICATIONREMARKS"].ToString();

            BindScore();
        }
    }

    protected void OtherDetailClick(object sender, EventArgs e)
    {
        string reason = ReadCheckBoxList(cblReason);
        string otherrisk = ReadCheckBoxList(cblOtherRisk);
        string proposedcontrol = ReadCheckBoxList(cblProposedControl);
        string proposedcontrolname = string.Empty;

        foreach (ButtonListItem item in cblProposedControl.Items)
        {
            if (item.Selected == true)
            {
                proposedcontrolname = proposedcontrolname + item.Text.ToString() + ",";
            }
        }
        proposedcontrolname = proposedcontrolname.TrimEnd(',');

        if (reason.Contains("100"))
        {

            txtOtherReason.CssClass = "input";
            txtOtherReason.ReadOnly = false;
        }
        else
        {
            txtOtherReason.Text = "";
            txtOtherReason.ReadOnly = true;
            txtOtherReason.CssClass = "readonlytextbox";
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
            txtOtherDetails.CssClass = "readonlytextbox";
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
            txtProposedControlDetails.CssClass = "readonlytextbox";
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

        cblProposedControl.DataBindings.DataTextField = "FLDITEM";
        cblProposedControl.DataBindings.DataValueField = "FLDID";
        cblProposedControl.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
                                            Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAP")), 3);
        cblProposedControl.DataBind();

        ddlAmendedTo.DataTextField = "FLDNUMBER";
        ddlAmendedTo.DataValueField = "FLDRISKASSESSMENTPROCESSID";
        ddlAmendedTo.DataSource = PhoenixInspectionRiskAssessmentProcess.ListRiskAssessmentProcess(PhoenixSecurityContext.CurrentSecurityContext.InstallCode
            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(ViewState["QUALITYCOMPANYID"].ToString()));
        ddlAmendedTo.DataBind();

        ddlAmendedTo.Items.Insert(0, new RadComboBoxItem("--Select--", "DUMMY"));
    }

    protected void MenuGeneric_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            SaveRiskAssessmentGeneric();
        }

        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Inspection/InspectionRAGenericList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("VERIFY"))
        {
            VerifyRiskAssessmentGeneric();
        }
    }

    private void SaveRiskAssessmentGeneric()
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
            string otherriskproposed = txtProposedControlDetails.Text;
            string proposedcontrols = ReadCheckBoxList(cblProposedControl);

            if (!IsValidGenericTemplate())
            {
                ucError.Visible = true;
                return;
            }
            Guid? riskassessmentgenericidout = new Guid();
            PhoenixInspectionRiskAssessmentGeneric.InsertPhoenixInspectionRiskAssessmentGeneric(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()),
                General.GetNullableDateTime(recorddate),
                General.GetNullableDateTime(assessmentdate),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                HttpUtility.HtmlDecode(txtWorkDetails.Content), durationofworkactivity, frequencyofworkactivity, General.GetNullableInteger(rblPeopleInvolved.SelectedValue),
                null, null, reasonforassessment, otherreason,
                null, null, null, null, null, null,
                otherrisk, otherriskdetails, null, otherriskproposed, ReadCheckBoxList(cblImpact),
                //General.GetNullableInteger(ddlActivity.SelectedMiscellaneous),
                General.GetNullableString(txtActivityCondition.Text),
                ref riskassessmentgenericidout,
                General.GetNullableInteger(rblControlsAdequate.SelectedValue),
                General.GetNullableInteger(ddlCategory.SelectedValue),
                proposedcontrols,
                txtRemarks.Text,
                General.GetNullableGuid(ddlAmendedTo.SelectedValue),
                General.GetNullableString(HttpUtility.HtmlDecode(txtAdditionalSafetyProcedures.Content)),
                General.GetNullableInteger(ucCompany.SelectedCompany),
                General.GetNullableString(aternativemethod)
                );
            ucStatus.Text = "Generic Template updated.";
            ViewState["RISKASSESSMENTGENERICID"] = riskassessmentgenericidout.ToString();
            Filter.CurrentSelectedGenericRA = riskassessmentgenericidout.ToString();
            RiskAssessmentGenericEdit();
            BindGridGenericSafety();
            BindScore();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void VerifyRiskAssessmentGeneric()
    {
        try
        {
            PhoenixInspectionRiskAssessmentGeneric.RAGenericVerify(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"] != null ? ViewState["RISKASSESSMENTGENERICID"].ToString() : "")
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
    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                //if (cbl.Items.FindByValue(item) != null)
                //    cbl.Items.FindByValue(item).Selected = true;
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

    private void BindGridHealthSafetyRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentGeneric.ListGenericCategory(1, General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        gvHealthSafetyRisk.DataSource = ds;
        gvHealthSafetyRisk.DataBind();

        GridFooterItem footerItem = (GridFooterItem)gvHealthSafetyRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlHealthSubHazardType = (RadComboBox)footerItem.FindControl("ddlSubHazardType");
        ddlHealthSubHazardType.DataTextField = "FLDNAME";
        ddlHealthSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHazardType.SelectedHazardType));
        ddlHealthSubHazardType.DataBind();
    }
    private void BindGridGenericSafety()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentGeneric.ListGenericSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString())
            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , null);

        gvGenericSafety.DataSource = ds;
        gvGenericSafety.DataBind();
    }
    private void BindGridEnvironmentalRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentGeneric.ListGenericCategory(2, General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        gvEnvironmentalRisk.DataSource = ds;
        gvEnvironmentalRisk.DataBind();

        GridFooterItem footerItem = (GridFooterItem)gvEnvironmentalRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddllEnvSubHazardType = (RadComboBox)footerItem.FindControl("ddlSubHazardType");
        ddllEnvSubHazardType.DataTextField = "FLDNAME";
        ddllEnvSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddllEnvSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEnvHazardType.SelectedHazardType));
        ddllEnvSubHazardType.DataBind();
    }
    private void BindGridEconomicRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentGeneric.ListGenericCategory(4, General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        gvEconomicRisk.DataSource = ds;
        gvEconomicRisk.DataBind();

        GridFooterItem footerItem = (GridFooterItem)gvEconomicRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlEcoSubHazardType = (RadComboBox)footerItem.FindControl("ddlSubHazardType");
        ddlEcoSubHazardType.DataTextField = "FLDNAME";
        ddlEcoSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEcoSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEconomicHazardType.SelectedHazardType));
        ddlEcoSubHazardType.DataBind();

        BindComapany();
    }

    private bool IsValidGenericTemplate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableDateTime(txtIntendedWorkDate.Text) == null)
            ucError.ErrorMessage = "Date of intended work activitys is required.";

        if (General.GetNullableInteger(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableString(txtActivityCondition.Text) == null)
            ucError.ErrorMessage = "Activity/Conditions is required.";

        if (General.GetNullableString(txtWorkDetails.Content) == null)
            ucError.ErrorMessage = "Risks/Aspects is required.";

        return (!ucError.IsError);
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

                PhoenixInspectionRiskAssessmentGeneric.InsertGenericCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEconomicHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTGENERICID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucEconomicHazardType.SelectedHazardType = "";
                BindGridEconomicRisk();
                BindScore();

            }
            else if (gce.CommandName.ToUpper().Equals("CDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentGeneric.DeleteGenericCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridEconomicRisk();
                BindScore();
            }
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

                PhoenixInspectionRiskAssessmentGeneric.InsertGenericCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEnvHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTGENERICID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text),
                        General.GetNullableInteger(((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous)
                        );

                ucEnvHazardType.SelectedHazardType = "";
                BindGridEnvironmentalRisk();
                BindScore();
            }
            else if (gce.CommandName.ToUpper().Equals("EDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentGeneric.DeleteGenericCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridEnvironmentalRisk();
                BindScore();
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

    protected void gvHealthSafetyRisk__ItemCommand(object sender, GridCommandEventArgs e)
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

                PhoenixInspectionRiskAssessmentGeneric.InsertGenericCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTGENERICID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)e.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucHazardType.SelectedHazardType = "";
                BindGridHealthSafetyRisk();
                BindScore();

            }
            else if (e.CommandName.ToUpper().Equals("HDELETE"))
            {
                string categoryid = ((RadLabel)e.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentGeneric.DeleteGenericCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                BindGridHealthSafetyRisk();
                BindScore();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGenericSafety_ItemDataBound(object sender, GridItemEventArgs e)
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
                btnPICEdit.Attributes.Add("onclick", "return showPickList('spnPICEdit', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + ViewState["VESSELID"].ToString() + "', true); ");
            }

            //string txtActualFinishTimeEdit = ((RadTimePicker)e.Item.FindControl("txtActualFinishTimeEdit")).SelectedTime.ToString();

            //if (txtActualFinishTimeEdit != null)
            //    txtActualFinishTimeEdit = drv["FLDACTUALFINISHTIME"].ToString();

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
                btnPICAdd.Attributes.Add("onclick", "return showPickList('spnPICAdd', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    +  ViewState["VESSELID"].ToString() + "', true); ");
            }

        }
    }
    protected void gvGenericSafety_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            if (gce.CommandName.ToUpper().Equals("SADD"))
            {
                string serialno = ((UserControlMaskNumber)gce.Item.FindControl("ucSNoAdd")).Text;
                string task = ((RadTextBox)gce.Item.FindControl("txtTaskAdd")).Text;
                string pic = ((RadTextBox)gce.Item.FindControl("txtPICIdAdd")).Text;                

                if (!IsValidSafety(task))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionRiskAssessmentGeneric.InsertGenericSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString())
                        , null
                        , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                        , General.GetNullableInteger(serialno)
                        , General.GetNullableString(task)
                        , General.GetNullableInteger(pic)
                        , null
                        , null
                        , null
                        , null
                        , null
                        );

                    gvGenericSafety.SelectedIndexes.Clear();
                    gvGenericSafety.EditIndexes.Clear();
                    BindGridGenericSafety();
                }

            }
            else if (gce.CommandName.ToUpper().Equals("SDELETE"))
            {
                string genericsafteyid = ((RadLabel)gce.Item.FindControl("lblGenericSafetyId")).Text;
                PhoenixInspectionRiskAssessmentGeneric.DeleteGenericSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString())
                    , General.GetNullableGuid(genericsafteyid)
                    );
                gvGenericSafety.SelectedIndexes.Clear();
                gvGenericSafety.EditIndexes.Clear();
                BindGridGenericSafety();
            }
            else if (gce.CommandName.ToUpper().Equals("SUPDATE"))
            {                
                string actualfinishdate = "";
                string actualfinishtime = "";

                string genericsafteyid = ((RadLabel)gce.Item.FindControl("lblGenericSafetyIdEdit")).Text;
                string serialno = ((UserControlMaskNumber)gce.Item.FindControl("ucSNoEdit")).Text;
                string task = ((RadTextBox)gce.Item.FindControl("txtTaskEdit")).Text;
                string pic = ((RadTextBox)gce.Item.FindControl("txtPICIdEdit")).Text;
                string remarks = ((RadTextBox)gce.Item.FindControl("txtRemarksEdit")).Text;

                actualfinishdate = ((UserControlDate)gce.Item.FindControl("ucActualFinishDateEdit")).Text;                
                actualfinishtime = ((RadTimePicker)gce.Item.FindControl("txtActualFinishTimeEdit")).SelectedTime.ToString(); 
                
                string actualfinishdatetime = actualfinishdate + " " + actualfinishtime;

                if (!IsValidSafety(task,actualfinishdate,remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionRiskAssessmentGeneric.InsertGenericSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString())
                        , General.GetNullableGuid(genericsafteyid)
                        , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                        , General.GetNullableInteger(serialno)
                        , General.GetNullableString(task)
                        , General.GetNullableInteger(pic)
                        , null
                        , null
                        , null
                        , General.GetNullableDateTime(actualfinishdatetime)
                        , General.GetNullableString(remarks)
                        );

                    gvGenericSafety.SelectedIndexes.Clear();
                    gvGenericSafety.EditIndexes.Clear();
                    BindGridGenericSafety();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidHazard(string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()) != null)
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

    private bool IsValidSafety(string task)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableString(task) == null)
        {
            ucError.ErrorMessage = "Task is required";
        }        
        return (!ucError.IsError);
    }

    private bool IsValidSafety(string task, string actualfinishdate, string remarks)
    {        
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTGENERICID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableString(task) == null)
        {
            ucError.ErrorMessage = "Task is required";
        }
        if (General.GetNullableDateTime(actualfinishdate) == null)
        {
            ucError.ErrorMessage = "Completion date is required";
        }
        if (General.GetNullableString(remarks) == null)
        {
            ucError.ErrorMessage = "Remarks is required";
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
}
