using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web;
public partial class Inspection_InspectionRACargoTemplateDetails : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvHealthSafetyRisk.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvHealthSafetyRisk.UniqueID, "Edit$" + r.RowIndex.ToString());
            }

        }

        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            rblControlsAdequate.Enabled = false;
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        ViewState["COPYTYPE"] = "0";
        if (Request.QueryString["CopyType"] == "1" || Request.QueryString["CopyType"] == "2")
        {
            toolbar.AddButton("Confirm", "CONFIRM");
            ViewState["COPYTYPE"] = Request.QueryString["CopyType"].ToString();
            ucTitle.ShowMenu = "false";
        }
        else
        {
            toolbar.AddButton("List", "LIST");
            if (Request.QueryString["status"].ToString() != "3" && Request.QueryString["status"].ToString() != "5" && Request.QueryString["status"].ToString() != "6" && Request.QueryString["status"].ToString() != "7") //Issued
            {
                toolbar.AddButton("Save", "SAVE");
            }
        }
        MenuCargo.AccessRights = this.ViewState;
        MenuCargo.MenuList = toolbar.Show();

        if (!IsPostBack)
        {            
            DateTime dt = DateTime.Today;
            txtDate.Text = dt.ToString();
            ViewState["RISKASSESSMENTCARGOID"] = "";
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
                ViewState["RISKASSESSMENTCARGOID"] = Request.QueryString["genericid"].ToString();
                RiskAssessmentCargoEdit();
                if (Request.QueryString["genericid"].ToString() != "")
                    BindScore();

            }
        }

        BindGrid();
        DropDownList ddlHealthSubHazardType = (DropDownList)gvHealthSafetyRisk.FooterRow.FindControl("ddlSubHazardType");
        ddlHealthSubHazardType.DataTextField = "FLDNAME";
        ddlHealthSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHazardType.SelectedHazardType));
        ddlHealthSubHazardType.DataBind();

        DropDownList ddllEnvSubHazardType = (DropDownList)gvEnvironmentalRisk.FooterRow.FindControl("ddlSubHazardType");
        ddllEnvSubHazardType.DataTextField = "FLDNAME";
        ddllEnvSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddllEnvSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEnvHazardType.SelectedHazardType));
        ddllEnvSubHazardType.DataBind();

        DropDownList ddlEcoSubHazardType = (DropDownList)gvEconomicRisk.FooterRow.FindControl("ddlSubHazardType");
        ddlEcoSubHazardType.DataTextField = "FLDNAME";
        ddlEcoSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEcoSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEconomicHazardType.SelectedHazardType));
        ddlEcoSubHazardType.DataBind();

        BindComapany();
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
        BindGridCargoSafety();
    }

    protected void BindScore()
    {
        decimal minscore = 0, maxscore = 0;

        DataSet ds = PhoenixInspectionRiskAssessmentCargo.CargoScores(General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()));

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

        //ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivityByCategory(2);
        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(4, General.GetNullableInteger(ViewState["QUALITYCOMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }

    private void RiskAssessmentCargoEdit()
    {
        DataTable dt = PhoenixInspectionRiskAssessmentCargo.EditPhoenixInspectionRiskAssessmentCargo(
            General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()));

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

            if (ddlCategory.Items.FindByValue(dr["FLDACTIVITYID"].ToString()) != null)
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
            if (dr["FLDCARGORAVERIFEDYN"].ToString() == "0" || dr["FLDCARGORAVERIFEDYN"].ToString() == "1")
            {
                rblVerifcation.SelectedValue = dr["FLDCARGORAVERIFEDYN"].ToString();

                if (dr["FLDCARGORAVERIFEDYN"].ToString() == "0")
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

        foreach (ListItem item in cblProposedControl.Items)
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

        rblPeopleInvolved.DataTextField = "FLDNAME";
        rblPeopleInvolved.DataValueField = "FLDFREQUENCYID";
        rblPeopleInvolved.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(3);
        rblPeopleInvolved.DataBind();

        cblReason.DataTextField = "FLDNAME";
        cblReason.DataValueField = "FLDMISCELLANEOUSID";
        cblReason.DataSource = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(1, null);
        cblReason.DataBind();

        cblOtherRisk.DataTextField = "FLDNAME";
        cblOtherRisk.DataValueField = "FLDHAZARDID";
        cblOtherRisk.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(3, 0);
        cblOtherRisk.DataBind();

        rblWorkDuration.DataTextField = "FLDNAME";
        rblWorkDuration.DataValueField = "FLDFREQUENCYID";
        rblWorkDuration.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(1);
        rblWorkDuration.DataBind();

        rblWorkFrequency.DataTextField = "FLDNAME";
        rblWorkFrequency.DataValueField = "FLDFREQUENCYID";
        rblWorkFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(2, "OFP");
        rblWorkFrequency.DataBind();
        rblWorkFrequency.SelectedIndex = 0;

        rblOtherRiskControl.DataTextField = "FLDNAME";
        rblOtherRiskControl.DataValueField = "FLDFREQUENCYID";
        rblOtherRiskControl.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
        rblOtherRiskControl.DataBind();

        cblProposedControl.DataTextField = "FLDITEM";
        cblProposedControl.DataValueField = "FLDID";
        cblProposedControl.DataSource = PhoenixInspectionRiskAssessmentType.ListRiskAssessmentType(
                                            Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RAP")), 3);
        cblProposedControl.DataBind();

        ddlAmendedTo.DataTextField = "FLDNUMBER";
        ddlAmendedTo.DataValueField = "FLDRISKASSESSMENTPROCESSID";
        ddlAmendedTo.DataSource = PhoenixInspectionRiskAssessmentProcess.ListRiskAssessmentProcess(PhoenixSecurityContext.CurrentSecurityContext.InstallCode
            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(ViewState["QUALITYCOMPANYID"].ToString()));
        ddlAmendedTo.DataBind();

        ddlAmendedTo.Items.Insert(0, new ListItem("--Select--", "DUMMY"));
    }

    protected void MenuCargo_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            SaveRiskAssessmentCargo();
        }

        if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Inspection/InspectionNonRoutineRACargoTemplate.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("VERIFY"))
        {
            VerifyRiskAssessmentCargo();
        }
        if (dce.CommandName.ToUpper().Equals("CONFIRM"))
        {
            if (ViewState["COPYTYPE"].ToString() == "1")
            {
                if (ViewState["RISKASSESSMENTCARGOID"] != null && ViewState["RISKASSESSMENTCARGOID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargo.CopyCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["RISKASSESSMENTCARGOID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    ucStatus.Text = "Copied Successfully";

                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
            if (ViewState["COPYTYPE"].ToString() == "2")
            {
                if (ViewState["RISKASSESSMENTCARGOID"] != null && ViewState["RISKASSESSMENTCARGOID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargo.ProposeCargoTemplate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["RISKASSESSMENTCARGOID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    ucStatus.Text = "Standard template proposed";

                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
        }
    }

    private void SaveRiskAssessmentCargo()
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

            if (!IsValidCargoTemplate())
            {
                ucError.Visible = true;
                return;
            }
            Guid? riskassessmentgenericidout = new Guid();
            PhoenixInspectionRiskAssessmentCargo.InsertPhoenixInspectionRiskAssessmentCargo(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()),
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
                General.GetNullableString(aternativemethod),
                1               //Standard Template
                );
            ucStatus.Text = "Cargo Template updated.";
            ViewState["RISKASSESSMENTCARGOID"] = riskassessmentgenericidout.ToString();
            Filter.CurrentSelectedCargoRA = riskassessmentgenericidout.ToString();
            RiskAssessmentCargoEdit();
            BindGridCargoSafety();
            BindScore();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void VerifyRiskAssessmentCargo()
    {
        try
        {
            PhoenixInspectionRiskAssessmentCargo.RACargoVerify(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"] != null ? ViewState["RISKASSESSMENTCARGOID"].ToString() : "")
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
    private void BindCheckBoxList(CheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.Items.FindByValue(item) != null)
                    cbl.Items.FindByValue(item).Selected = true;
            }
        }
    }
    private string ReadCheckBoxList(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
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
        ds = PhoenixInspectionRiskAssessmentCargo.ListCargoCategory(1, General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvHealthSafetyRisk.DataSource = ds;
            gvHealthSafetyRisk.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvHealthSafetyRisk);
        }
    }
    private void BindGridCargoSafety()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentCargo.ListCargoSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString())
            , General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , null);

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvCargoSafety.DataSource = ds;
            gvCargoSafety.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCargoSafety);
        }
    }
    private void BindGridEnvironmentalRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentCargo.ListCargoCategory(2, General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvEnvironmentalRisk.DataSource = ds;
            gvEnvironmentalRisk.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvEnvironmentalRisk);
        }
    }
    private void BindGridEconomicRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentCargo.ListCargoCategory(4, General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {

            gvEconomicRisk.DataSource = ds;
            gvEconomicRisk.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvEconomicRisk);
        }
    }

    private bool IsValidCargoTemplate()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        //if (General.GetNullableDateTime(txtIntendedWorkDate.Text) == null)
        //    ucError.ErrorMessage = "Date of intended work activitys is required.";

        if (General.GetNullableInteger(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableString(txtActivityCondition.Text) == null)
            ucError.ErrorMessage = "Activity/Conditions is required.";

        if (General.GetNullableString(txtWorkDetails.Content) == null)
            ucError.ErrorMessage = "Risks/Aspects is required.";

        return (!ucError.IsError);
    }
    protected void gvEconomicRisk_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        if (ge.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)ge.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton cmdAdd = (ImageButton)ge.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }
    protected void gvEconomicRisk_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(gce.CommandArgument.ToString());
            if (gce.CommandName.ToUpper().Equals("CADD"))
            {

                if (!IsValidHazard(ucEconomicHazardType.SelectedHazardType,
                                        ((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue, null, null
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentCargo.InsertCargoCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEconomicHazardType.SelectedHazardType),
                        new Guid(((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTCARGOID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)_gridView.FooterRow.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucEconomicHazardType.SelectedHazardType = "";
                BindGridEconomicRisk();
                BindScore();

            }
            else if (gce.CommandName.ToUpper().Equals("CDELETE"))
            {
                string categoryid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentCargo.DeleteCargoCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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

    protected void gvEnvironmentalRisk_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(gce.CommandArgument.ToString());
            if (gce.CommandName.ToUpper().Equals("EADD"))
            {

                if (!IsValidHazard(ucEnvHazardType.SelectedHazardType,
                                        ((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue, 2,
                                        ((UserControlRAMiscellaneous)_gridView.FooterRow.FindControl("ucImpactType")).SelectedMiscellaneous))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentCargo.InsertCargoCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEnvHazardType.SelectedHazardType),
                        new Guid(((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTCARGOID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)_gridView.FooterRow.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtProposedControlAdd")).Text),
                        General.GetNullableInteger(((UserControlRAMiscellaneous)_gridView.FooterRow.FindControl("ucImpactType")).SelectedMiscellaneous)
                        );

                ucEnvHazardType.SelectedHazardType = "";
                BindGridEnvironmentalRisk();
                BindScore();

            }
            else if (gce.CommandName.ToUpper().Equals("EDELETE"))
            {
                string categoryid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentCargo.DeleteCargoCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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
    protected void gvEnvironmentalRisk_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        if (ge.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)ge.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Row.RowType == DataControlRowType.Footer)
        {
            UserControlRAMiscellaneous ucImpactType = (UserControlRAMiscellaneous)ge.Row.FindControl("ucImpactType");
            if (ucImpactType != null)
            {
                ucImpactType.MiscellaneousList = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(3, 0);
                ucImpactType.DataBind();
            }

            ImageButton cmdAdd = (ImageButton)ge.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk_RowDataBound(object sender, GridViewRowEventArgs ge)
    {
        if (ge.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)ge.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton cmdAdd = (ImageButton)ge.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk__RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("HADD"))
            {

                if (!IsValidHazard(ucHazardType.SelectedHazardType,
                                        ((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue, null, null
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentCargo.InsertCargoCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucHazardType.SelectedHazardType),
                        new Guid(((DropDownList)_gridView.FooterRow.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(ViewState["RISKASSESSMENTCARGOID"].ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)_gridView.FooterRow.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucHazardType.SelectedHazardType = "";
                BindGridHealthSafetyRisk();
                BindScore();

            }
            else if (e.CommandName.ToUpper().Equals("HDELETE"))
            {
                string categoryid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentCargo.DeleteCargoCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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

    protected void gvCargoSafety_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton btnPICEdit = (ImageButton)e.Row.FindControl("btnPICEdit");
            if (btnPICEdit != null)
            {
                btnPICEdit.Attributes.Add("onclick", "return showPickList('spnPICEdit', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + ViewState["VESSELID"].ToString() + "', true); ");
            }

            TextBox txtActualFinishTimeEdit = (TextBox)e.Row.FindControl("txtActualFinishTimeEdit");

            if (txtActualFinishTimeEdit != null)
                txtActualFinishTimeEdit.Text = drv["FLDACTUALFINISHTIME"].ToString();

            TextBox txtPICNameEdit = (TextBox)e.Row.FindControl("txtPICNameEdit");
            TextBox txtPICIdEdit = (TextBox)e.Row.FindControl("txtPICIdEdit");
            TextBox txtPICRankEdit = (TextBox)e.Row.FindControl("txtPICRankEdit");

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
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton btnPICAdd = (ImageButton)e.Row.FindControl("btnPICAdd");

            if (btnPICAdd != null)
            {
                btnPICAdd.Attributes.Add("onclick", "return showPickList('spnPICAdd', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + ViewState["VESSELID"].ToString() + "', true); ");
            }

        }
    }
    protected void gvCargoSafety_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(gce.CommandArgument.ToString());
            if (gce.CommandName.ToUpper().Equals("SADD"))
            {
                string serialno = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("ucSNoAdd")).Text;
                string task = ((TextBox)_gridView.FooterRow.FindControl("txtTaskAdd")).Text;
                string pic = ((TextBox)_gridView.FooterRow.FindControl("txtPICIdAdd")).Text;

                if (!IsValidSafety(task))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionRiskAssessmentCargo.InsertCargoSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString())
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

                    BindGridCargoSafety();
                }

            }
            else if (gce.CommandName.ToUpper().Equals("SDELETE"))
            {
                string genericsafteyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCargoSafetyId")).Text;
                PhoenixInspectionRiskAssessmentCargo.DeleteCargoSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString())
                    , General.GetNullableGuid(genericsafteyid)
                    );
                BindGridCargoSafety();
            }
            else if (gce.CommandName.ToUpper().Equals("SUPDATE"))
            {
                string actualfinishdate = "";
                string actualfinishtime = "";

                string genericsafteyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCargoSafetyIdEdit")).Text;
                string serialno = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucSNoEdit")).Text;
                string task = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTaskEdit")).Text;
                string pic = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPICIdEdit")).Text;
                string remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text;

                actualfinishdate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucActualFinishDateEdit")).Text;
                actualfinishtime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtActualFinishTimeEdit")).Text;
                actualfinishtime = actualfinishtime.Trim() == "__:__" ? string.Empty : actualfinishtime;

                actualfinishtime = (actualfinishtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : actualfinishtime;

                string actualfinishdatetime = actualfinishdate + " " + actualfinishtime;

                if (!IsValidSafety(task, actualfinishdate, remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionRiskAssessmentCargo.InsertCargoSafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString())
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

                    _gridView.EditIndex = -1;
                    BindGridCargoSafety();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCargoSafety_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;

            BindGridCargoSafety();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCargoSafety_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindGridCargoSafety();
    }

    private bool IsValidHazard(string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()) != null)
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

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()) == null)
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

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTCARGOID"].ToString()) == null)
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
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
