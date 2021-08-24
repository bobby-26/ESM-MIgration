using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InspectionPEARSRiskAssessmentActivityStepsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuActivity.AccessRights = this.ViewState;
            MenuActivity.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["RAACTIVITYID"] = "";
                ViewState["RAID"] = "";

                if (Request.QueryString["RAACTIVITYID"] != null && Request.QueryString["RAACTIVITYID"].ToString() != string.Empty)
                    ViewState["RAACTIVITYID"] = Request.QueryString["RAACTIVITYID"].ToString();

                if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != string.Empty)
                    ViewState["RAID"] = Request.QueryString["RAID"].ToString();

                BindGroupMembers();
                BindActivity();                
                BindSeverity();
                BindLOH();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindSeverity()
    {
        ddlinitpplSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlinitpplSeverity.DataBind();

        ddlinitEnvSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlinitEnvSeverity.DataBind();

        ddlinitAstSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlinitAstSeverity.DataBind();

        ddlinitRepSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlinitRepSeverity.DataBind();

        ddlinitSdlSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlinitSdlSeverity.DataBind();

        ddlResPplSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlResPplSeverity.DataBind();

        ddlResEnvSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlResEnvSeverity.DataBind();

        ddlResAstSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlResAstSeverity.DataBind();

        ddlResRepSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlResRepSeverity.DataBind();

        ddlResSdlSeverity.DataSource = PhoenixInspectionPEARSRiskassessmentSeverity.ListRASeverity();
        ddlResSdlSeverity.DataBind();
    }
    protected void BindLOH()
    {
        ddlinitpplLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlinitpplLOH.DataBind();

        ddlinitEnvLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlinitEnvLOH.DataBind();

        ddlinitAstLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlinitAstLOH.DataBind();

        ddlinitRepLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlinitRepLOH.DataBind();

        ddlinitSdlLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlinitSdlLOH.DataBind();

        ddlResPplLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlResPplLOH.DataBind();

        ddlResEnvLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlResEnvLOH.DataBind();

        ddlResAstLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlResAstLOH.DataBind();

        ddlResRepLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlResRepLOH.DataBind();

        ddlResSdlLOH.DataSource = PhoenixInspectionPEARSRiskassessmentLOH.ListRALOH();
        ddlResSdlLOH.DataBind();
    }
    protected void MenuActivity_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRA())
                {
                    ucError.Visible = true;
                    return;
                }

                InserUpdatetActivity();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InserUpdatetActivity()
    {
        try
        {

            PhoenixInspectionPEARSRiskAssessment.RAActivityStepInsert(General.GetNullableGuid(ViewState["RAACTIVITYID"].ToString())
                , new Guid(ViewState["RAID"].ToString())
                , General.GetNullableString(txtActivityStep.Text)
                , General.GetNullableString(txtHazardDesc.Text)
                , General.GetNullableString(txtImpact.Text)
                , General.ReadCheckBoxList(ChkgroupMem)
                , General.GetNullableString(txtExisting.Text)
                , Int32.Parse(ddlinitpplSeverity.SelectedValue)
                , Int32.Parse(ddlinitpplLOH.SelectedValue)
                , Int32.Parse(ddlinitEnvSeverity.SelectedValue)
                , Int32.Parse(ddlinitEnvLOH.SelectedValue)
                , Int32.Parse(ddlinitAstSeverity.SelectedValue)
                , Int32.Parse(ddlinitAstLOH.SelectedValue)
                , Int32.Parse(ddlinitRepSeverity.SelectedValue)
                , Int32.Parse(ddlinitRepLOH.SelectedValue)
                , Int32.Parse(ddlinitSdlSeverity.SelectedValue)
                , Int32.Parse(ddlinitSdlLOH.SelectedValue)
                , General.GetNullableString(txtAdditional.Text)
                , Int32.Parse(ddlResPplSeverity.SelectedValue)
                , Int32.Parse(ddlResPplLOH.SelectedValue)
                , Int32.Parse(ddlResEnvSeverity.SelectedValue)
                , Int32.Parse(ddlResEnvLOH.SelectedValue)
                , Int32.Parse(ddlResAstSeverity.SelectedValue)
                , Int32.Parse(ddlResAstLOH.SelectedValue)
                , Int32.Parse(ddlResRepSeverity.SelectedValue)
                , Int32.Parse(ddlResRepLOH.SelectedValue)
                , Int32.Parse(ddlResSdlSeverity.SelectedValue)
                , Int32.Parse(ddlResSdlLOH.SelectedValue));

            ucStatus.Text = "Information updated.";

            String script = String.Format("javascript:fnReloadList('ActivityStep',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void BindGroupMembers()
    {
        ChkgroupMem.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ChkgroupMem.DataTextField = "FLDGROUPRANK";
        ChkgroupMem.DataValueField = "FLDGROUPRANKID";
        ChkgroupMem.DataBind();
    }
    protected void BindActivity()
    {
        if (ViewState["RAACTIVITYID"] != null && ViewState["RAACTIVITYID"].ToString() != string.Empty)
        {
            DataSet ds = new DataSet();

            ds = PhoenixInspectionPEARSRiskAssessment.EditRAActivityStep(new Guid(ViewState["RAACTIVITYID"].ToString()));

            txtActivityStep.Text = ds.Tables[0].Rows[0]["FLDACTIVITYSTEP"].ToString();
            txtHazardDesc.Text = ds.Tables[0].Rows[0]["FLDASPECT"].ToString();
            txtImpact.Text = ds.Tables[0].Rows[0]["FLDIMPACT"].ToString();           
            txtExisting.Text = ds.Tables[0].Rows[0]["FLDEXISTINGCONTROLS"].ToString();
            ddlinitpplSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALPEOPLESEVERITYID"].ToString();
            ddlinitpplLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALPEOPLELOHID"].ToString();
            txtinitPpl.Text = ds.Tables[0].Rows[0]["FLDINTIALPEOPLERISKRATING"].ToString();
            ddlinitEnvSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALENVIRONMENTSEVERITYID"].ToString();
            ddlinitEnvLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALENVIRONMENTLOHID"].ToString();
            txtinitEnv.Text = ds.Tables[0].Rows[0]["FLDINTIALENVIRONMENTRISKRATING"].ToString();
            ddlinitAstSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALASSETSEVERITYID"].ToString();
            ddlinitAstLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALASSETLOHID"].ToString();
            txtinitAst.Text = ds.Tables[0].Rows[0]["FLDINTIALASSETRISKRATING"].ToString();
            ddlinitRepSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALREPUTATIONSEVERITYID"].ToString();
            ddlinitRepLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALREPUTATIONLOHID"].ToString();
            txtinitRep.Text = ds.Tables[0].Rows[0]["FLDINTIALREPUTATIONRISKRATING"].ToString();
            ddlinitSdlSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALSCHEDULESEVERITYID"].ToString();
            ddlinitSdlLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDINTIALSCHEDULELOHID"].ToString();
            txtinitSdl.Text = ds.Tables[0].Rows[0]["FLDINTIALSCHEDULERISKRATING"].ToString();
            txtAdditional.Text = ds.Tables[0].Rows[0]["FLDADDITIONALCONTROLS"].ToString();
            ddlResPplSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEPEOPLESEVERITYID"].ToString();
            ddlResPplLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEPEOPLELOHID"].ToString();
            txtResPpl.Text = ds.Tables[0].Rows[0]["FLDRESIDULEPEOPLERISKRATING"].ToString();
            ddlResEnvSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEENVIRONMENTSEVERITYID"].ToString();
            ddlResEnvLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEENVIRONMENTLOHID"].ToString();
            txtResEnv.Text = ds.Tables[0].Rows[0]["FLDRESIDULEENVIRONMENTRISKRATING"].ToString();
            ddlResAstSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEASSETSEVERITYID"].ToString();
            ddlResAstLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEASSETLOHID"].ToString();
            txtResAst.Text = ds.Tables[0].Rows[0]["FLDRESIDULEASSETRISKRATING"].ToString();
            ddlResRepSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEREPUTATIONSEVERITYID"].ToString();
            ddlResRepLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULEREPUTATIONLOHID"].ToString();
            txtResRep.Text = ds.Tables[0].Rows[0]["FLDRESIDULEREPUTATIONRISKRATING"].ToString();
            ddlResSdlSeverity.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULESCHEDULESEVERITYID"].ToString();
            ddlResSdlLOH.SelectedValue = ds.Tables[0].Rows[0]["FLDRESIDULESCHEDULELOHID"].ToString();
            txtResSdl.Text = ds.Tables[0].Rows[0]["FLDRESIDULESCHEDULERISKRATING"].ToString();
            General.BindCheckBoxList(ChkgroupMem, ds.Tables[0].Rows[0]["FLDPERSONSINVOLVED"].ToString());
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindSeverity();
        BindLOH();
        BindActivity();
    }
    private bool IsValidRA()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ViewState["RAID"].ToString()) == null)
        {
            ucError.ErrorMessage = "Please Save the RA first.";
            return (!ucError.IsError);
        }
        if (General.GetNullableString(txtActivityStep.Text) == null)
            ucError.ErrorMessage = "Activity Step is required.";

        if (General.GetNullableString(txtHazardDesc.Text) == null)
            ucError.ErrorMessage = "Hazard Description (Aspect) is required.";

        if (General.GetNullableString(txtImpact.Text) == null)
            ucError.ErrorMessage = "Impact is required.";

        if (General.GetNullableString(General.ReadCheckBoxList(ChkgroupMem).ToString()) == null)
            ucError.ErrorMessage = "Persons Involved is required.";

        if (General.GetNullableString(txtExisting.Text) == null)
            ucError.ErrorMessage = "Existing Controls is required.";

        if (General.GetNullableString(txtAdditional.Text) == null)
            ucError.ErrorMessage = "Additional Controls is required.";

        return (!ucError.IsError);

    }
}