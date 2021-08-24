using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web;
using Telerik.Web.UI;
public partial class InspectionRAMachineryComparison : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["machneryid"] != null)
        {
            ViewState["RISKASSESSMENTMACHINERYID"] = Request.QueryString["machneryid"].ToString();
            RiskAssessmentGenericEdit();
        }
    }

    private void RiskAssessmentGenericEdit()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentGeneric.InspectionRiskAssessmentmachineryComparison(
            General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()));

        DataTable dt = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];

        if (dt.Rows.Count > 0)
        {
            txtgenericnumber.Text = dt.Rows[0]["FLDNUMBER"].ToString();
            txtReason.Content = dt.Rows[0]["FLDREASONFORASSESSMENT"].ToString();
            txtalternativework.Content = dt.Rows[0]["FLDALTERNATEWORKMETHOD"].ToString();
            txtnoofpeople.Text = dt.Rows[0]["FLDNOOFPEOPLENAME"].ToString();
            txtduartaion.Text = dt.Rows[0]["FLDDURATIONNAME"].ToString();
            txtFrequency.Text = dt.Rows[0]["FLDFREQUENCYNAME"].ToString();
            txtRisks.Content = dt.Rows[0]["FLDRISKSASPECTS"].ToString();
            txtEnvironmental.Content = dt.Rows[0]["FLDENVIRONMENTALHAZARD"].ToString();
            txtEconomic.Content = dt.Rows[0]["FLDECONOMICHAZARD"].ToString();
            txthealth.Content = dt.Rows[0]["FLDHEALTHANDSAFETYHAZARD"].ToString();
            txtWorstCase.Content = dt.Rows[0]["FLDWORSTCASE"].ToString();
            txtProposedControls.Content = dt.Rows[0]["FLDPROPOSEDCONTROLS"].ToString();
            txtAdditionalSafety.Content = dt.Rows[0]["FLDADDITIONALSAFETYPROCEDURE"].ToString();
            txtRisks.Content = dt.Rows[0]["FLDRISKSASPECTS"].ToString();
        }

        if (dt2.Rows.Count > 0)
        {
            txtstnumber.Text = dt2.Rows[0]["FLDNUMBER"].ToString();
            txtstReason.Content = dt2.Rows[0]["FLDSTANDARDREASONFORASSESSMENT"].ToString();
            txtstalternativework.Content = dt2.Rows[0]["FLDALTERNATEWORKMETHOD"].ToString();
            txtstnoofpeople.Text = dt2.Rows[0]["FLDSTANDARDNOOFPEOPLENAME"].ToString();
            txtstduartaion.Text = dt2.Rows[0]["FLDSTANDARDDURATIONNAME"].ToString();
            txtstFrequency.Text = dt2.Rows[0]["FLDSTANDARDFREQUENCYNAME"].ToString();
            txtstRisks.Content = dt2.Rows[0]["FLDSTANDARDRISKSASPECTS"].ToString();
            txtsthealth.Content = dt2.Rows[0]["FLDSTANDARDHEALTHANDSAFETYHAZARD"].ToString();
            txtstEnvironmental.Content = dt2.Rows[0]["FLDSTANDARDENVIRONMENTALHAZARD"].ToString();
            txtstEconomic.Content = dt2.Rows[0]["FLDSTANDARDECONOMICHAZARD"].ToString();
            txtstWorstCase.Content = dt2.Rows[0]["FLDSTANDARDWORSTCASE"].ToString();
            txtstProposedControls.Content = dt2.Rows[0]["FLDSTANDARDPROPOSEDCONTROLS"].ToString();
            txtstAdditionalSafety.Content = dt2.Rows[0]["FLDSTANDARDADDITIONALSAFETYPROCEDURE"].ToString();
        }

        PhoenixCommonDiffUtil.Item[] diffReason = PhoenixCommonDiffUtil.DiffText(txtReason.Content, txtstReason.Content, false, true, false);
        if (diffReason.Length > 0)
        {
            txtReason.Content = getchangedtext(txtReason.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffalternativework = PhoenixCommonDiffUtil.DiffText(txtalternativework.Content, txtstalternativework.Content, false, true, false);
        if (diffalternativework.Length > 0)
        {
            txtalternativework.Content = getchangedtext(txtalternativework.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffnoofpeople = PhoenixCommonDiffUtil.DiffText(txtnoofpeople.Text, txtstnoofpeople.Text, false, true, false);
        if (diffnoofpeople.Length > 0)
        {
            txtnoofpeople.Attributes["style"] = "color:red; font-weight:bold;";
        }

        PhoenixCommonDiffUtil.Item[] diffduartaion = PhoenixCommonDiffUtil.DiffText(txtduartaion.Text, txtstduartaion.Text, false, true, false);
        if (diffduartaion.Length > 0)
        {
            txtduartaion.Attributes["style"] = "color:red; font-weight:bold;";
        }

        PhoenixCommonDiffUtil.Item[] diffFrequency = PhoenixCommonDiffUtil.DiffText(txtFrequency.Text, txtstFrequency.Text, false, true, false);
        if (diffFrequency.Length > 0)
        {
            txtFrequency.Attributes["style"] = "color:red; font-weight:bold;";
        }

        PhoenixCommonDiffUtil.Item[] diffRisks = PhoenixCommonDiffUtil.DiffText(txtRisks.Content, txtstRisks.Content, false, true, false);
        if (diffRisks.Length > 0)
        {
            txtRisks.Content = getchangedtext(txtRisks.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffEnvironmental = PhoenixCommonDiffUtil.DiffText(txtEnvironmental.Content, txtstEnvironmental.Content, false, true, false);
        if (diffEnvironmental.Length > 0)
        {
            txtEnvironmental.Content = getchangedtext(txtEnvironmental.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffhealth = PhoenixCommonDiffUtil.DiffText(txthealth.Content, txtsthealth.Content, false, true, false);
        if (diffhealth.Length > 0)
        {
            txthealth.Content = getchangedtext(txthealth.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffEconomic = PhoenixCommonDiffUtil.DiffText(txtEconomic.Content, txtstEconomic.Content, false, true, false);
        if (diffEconomic.Length > 0)
        {
            txtEconomic.Content = getchangedtext(txtEconomic.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffWorstCase = PhoenixCommonDiffUtil.DiffText(txtWorstCase.Content, txtstWorstCase.Content, false, true, false);
        if (diffWorstCase.Length > 0)
        {
            txtWorstCase.Content = getchangedtext(txtWorstCase.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffProposedControls = PhoenixCommonDiffUtil.DiffText(txtProposedControls.Content, txtstProposedControls.Content, false, true, false);
        if (diffProposedControls.Length > 0)
        {
            txtProposedControls.Content = getchangedtext(txtProposedControls.Content);
        }

        PhoenixCommonDiffUtil.Item[] diffAdditionalSafety = PhoenixCommonDiffUtil.DiffText(txtAdditionalSafety.Content, txtstAdditionalSafety.Content, false, true, false);
        if (diffAdditionalSafety.Length > 0)
        {
            txtAdditionalSafety.Content = getchangedtext(txtAdditionalSafety.Content);
        }
    }

    public string getchangedtext(string text)
    {
        string modifiedtext = "<span style = " + "color:red; font-weight:bold;" + ">" + text + "</span>";
        return modifiedtext;
    }
}