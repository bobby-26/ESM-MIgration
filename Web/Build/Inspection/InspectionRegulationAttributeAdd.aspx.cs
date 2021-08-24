using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationAttributeAdd : PhoenixBasePage
{
    Guid? RegulationId;
    Guid? RuleId;
    Guid? AttributeId;
    //string lblVesselType = "Vessel Type";
    string lblSurveyType = "Survey";
    string lblYearBuild = "Year Built";
    string lblCertificate = "Certificate";
    string lblDocking = "Next Docking";
    string lblKeelaid = "Keel Laid";
    bool IsEditable = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        RegulationId = string.IsNullOrWhiteSpace(Request.QueryString["RuleId"]) == false ? (Guid?)new Guid(Request.QueryString["RegulationId"]) : null;
        RuleId = string.IsNullOrWhiteSpace(Request.QueryString["RuleId"]) == false ? (Guid?)new Guid(Request.QueryString["RuleId"]) : null;
        AttributeId = string.IsNullOrWhiteSpace(Request.QueryString["AttributeId"]) == false ? (Guid?)new Guid(Request.QueryString["AttributeId"]) : null;
        IsEditable = string.IsNullOrWhiteSpace(Request.QueryString["AttributeId"]) == false ? true : false;

        if (IsPostBack == false)
        {
            PopulateAttributes();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            NewAttribute.AccessRights = this.ViewState;
            NewAttribute.MenuList = toolbar.Show();
            PopulateComboBox();
            GetAttributeDetail();
        }
    }

    private void GetAttributeDetail()
    {
        if (AttributeId == null) return;

        DataSet ds = PhoenixInspectionNewRegulation.AttributeEdit(AttributeId);
        DataRow dr = ds.Tables[0].Rows[0];

        txtSortOrder.Text = dr["FLDSORTORDER"].ToString();
        ddlConditionAdd.SelectedItem.Text = dr["FLDCONDITION"].ToString();
        ddlFieldNameAdd.SelectedItem.Text = dr["FLDATTRIBUTENAME"].ToString();
        ddlFieldNameAdd.SelectedItem.Value = dr["FLDATTRIBUTEREGISTERID"].ToString();
        txtValueAdd.Text = dr["FLDVALUE"].ToString();
        if (dr["FLDATTRIBUTENAME"].ToString() == lblYearBuild || dr["FLDATTRIBUTENAME"].ToString() == lblDocking || dr["FLDATTRIBUTENAME"].ToString() == lblKeelaid)
        {
            ddlYearAdd.SelectedDate = Convert.ToDateTime(dr["FLDVALUE"].ToString());
        }
        RadComboSurvey.Text = dr["FLDPICKLISTNAME"].ToString();
        RadComboSurvey.SelectedValue = dr["FLDVALUE"].ToString();
        RadComboCertificate.Text = dr["FLDPICKLISTNAME"].ToString();
        RadComboCertificate.SelectedValue = dr["FLDVALUE"].ToString();
        RadComboSurveyType.Text = dr["FLDSURVEYTYPENAME"].ToString();
        RadComboSurveyType.SelectedValue = dr["FLDVALUE"].ToString();
        chkEarlierLaterAdd.SelectedValue = dr["FLDCONDITION"].ToString();
        chkBeforeAfterAdd.SelectedValue = dr["FLDCONDITION"].ToString();
        displayControl(dr["FLDATTRIBUTENAME"].ToString());
    }

    private void PopulateComboBox()
    {
        DataSet ds = new DataSet();
        ds = PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1);
        RadComboSurveyType.DataValueField = "FLDSURVEYTYPEID";
        RadComboSurveyType.DataTextField = "FLDSURVEYTYPENAME";

        RadComboSurveyType.DataSource = ds;
        RadComboSurveyType.DataBind();

        string certificatename = string.Empty;
        string certificatecode = string.Empty;
        ds = PhoenixInspectionNewRegulation.GetNonTechinicalCertificateList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, certificatecode, certificatename);
        RadComboCertificate.DataValueField = "FLDCERTIFICATEID";
        RadComboCertificate.DataTextField = "FLDCERTIFICATENAME";
        RadComboCertificate.DataSource = ds;
        RadComboCertificate.DataBind();

        int certificatecategoryid = 1;
        //ds = PhoenixCommonPlannedMaintenance.CertificateSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, certificatecode, certificatename, certificatecategoryid, (int)ViewState["PAGENUMBER"], gvCertificate.PageSize, ref iRowCount, ref iTotalPageCount);

        ds = PhoenixInspectionNewRegulation.GetSurveyList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null, certificatecategoryid);
        RadComboSurvey.DataValueField = "FLDCERTIFICATEID";
        RadComboSurvey.DataTextField = "FLDCERTIFICATENAME";
        RadComboSurvey.DataSource = ds;
        RadComboSurvey.DataBind();
    }

    private void PopulateAttributes()
    {
        RadComboBox attibuteDropDown = ddlFieldNameAdd;
        attibuteDropDown.DataSource = PhoenixInspectionNewRegulation.RuleAttributeDropDown();
        attibuteDropDown.DataTextField = "FLDATTRIBUTENAME";
        attibuteDropDown.DataValueField = "FLDATTRIBUTEREGISTERID";
        attibuteDropDown.DataBind();
    }

    protected void NewAttribute_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName == "CLEAR")
            {
                ClearUserInput();
            }
            if (CommandName == "SAVE")
            {
                string value = "";
                string DispalyCode = "";
                string DispalyName = "";
                int? SurveyType = null;
                int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                RadDropDownList condition = ddlConditionAdd;
                RadComboBox attribute = ddlFieldNameAdd;
                RadTextBox valueField = txtValueAdd;
                RadTextBox sort = txtSortOrder;
                RadDatePicker ddlYear = ddlYearAdd;
                RadComboBox txtsurvey = RadComboSurvey;
                RadComboBox txtcertificate = RadComboCertificate;
                RadComboBox txtSurveyType = RadComboSurveyType;
                RadRadioButtonList earlierLater = (RadRadioButtonList)chkEarlierLaterAdd;
                RadRadioButtonList beforeAfter = (RadRadioButtonList)chkBeforeAfterAdd;

                if (attribute.SelectedItem.Text == lblYearBuild)
                {
                    value = ddlYear.SelectedDate.Value.ToString("yyyy-MM-dd");
                    condition.SelectedText = beforeAfter.SelectedValue;
                }
                else if (attribute.SelectedItem.Text == lblSurveyType)
                {
                    value = txtsurvey.SelectedValue;
                    DispalyCode = string.Empty;
                    DispalyName = txtsurvey.Text;
                    condition.SelectedText = earlierLater.SelectedValue;
                }
                else if (attribute.SelectedItem.Text == lblCertificate)
                {
                    value = txtcertificate.SelectedValue;
                    DispalyCode = String.Empty;
                    DispalyName = txtcertificate.Text;
                    condition.SelectedText = earlierLater.SelectedValue;
                    SurveyType = Convert.ToInt32(txtSurveyType.SelectedValue);
                }
                else if (attribute.SelectedItem.Text == lblDocking)
                {
                    value = ddlYear.SelectedDate.Value.ToString("yyyy-MM-dd");
                    condition.SelectedText = earlierLater.SelectedValue;
                }
                else if (attribute.SelectedItem.Text == lblKeelaid)
                {
                    value = ddlYear.SelectedDate.Value.ToString("yyyy-MM-dd");
                    condition.SelectedText = beforeAfter.SelectedValue;
                }
                else
                {
                    value = valueField.Text;
                }

                if (IsEditable == false) SaveAttribute(txtSortOrder.Text, attribute.SelectedItem.Text, new Guid(attribute.SelectedItem.Value), value, condition.SelectedText, SurveyType, DispalyName, DispalyCode, usercode);
                if (IsEditable == true) UpdateAttribute(txtSortOrder.Text, attribute.SelectedItem.Text, new Guid(attribute.SelectedItem.Value), value, condition.SelectedText, SurveyType, DispalyName, DispalyCode, usercode);
                ClearUserInput();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);

            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }



    private void ShowError(string Message)
    {
        ucError.ErrorMessage = Message;
        ucError.Visible = true;
    }
    private void UpdateAttribute(string sort, string attributeName, Guid attributeRegisterId, string value, string condition, int? surveyTypeId, string displayName, string displayCode, int usercode)
    {
        if (ValidateAttribute(RuleId, sort, attributeName, condition, value))
        {
            int sortorder = Convert.ToInt32(sort);
            PhoenixInspectionNewRegulation.RuleAttributeUpdate(AttributeId.Value, attributeRegisterId, value, sortorder, condition, usercode, displayName, displayCode, surveyTypeId);
        }
    }
    private void SaveAttribute(string sort, string attributeName, Guid attributeRegisterId, string value, string condition, int? surveyTypeId, string displayName, string displayCode, int usercode)
    {
        if (ValidateAttribute(RuleId, sort, attributeName, condition, value))
        {
            int sortorder = Convert.ToInt32(sort);
            PhoenixInspectionNewRegulation.RuleAttributeInsert(RegulationId, RuleId, attributeRegisterId, value, sortorder, condition, usercode, displayName, displayCode, surveyTypeId);
        }
        else
        {
            throw new ArgumentException("");
        }
    }

    private bool ValidateAttribute(Guid? RuleId, string sortorder, string attrbute, string condition, string value)
    {
        bool validate = true;
        ucError.HeaderMessage = "Please provide the following required information";
        if (String.IsNullOrEmpty(sortorder))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide sortorder";
        }
        if (String.IsNullOrEmpty(attrbute))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide attribtue";
        }
        if (String.IsNullOrWhiteSpace(condition))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide condition";
        }
        if (string.IsNullOrWhiteSpace(value))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide value";
        }
        if (RuleId.HasValue == false)
        {
            validate = false;
            ucError.ErrorMessage = "Please select a rule and add attributes ";
        }
        return validate;
    }

    private void ClearUserInput()
    {
        txtSortOrder.Text = null;
        ddlFieldNameAdd.SelectedIndex = 0;
        ddlConditionAdd.SelectedIndex = 0;
        chkEarlierLaterAdd.SelectedIndex = 0;
        chkBeforeAfterAdd.SelectedIndex = 0;
        txtValueAdd.Text = null;
        RadComboCertificate.SelectedIndex = 0;
        RadComboSurveyType.SelectedIndex = 0;
        RadComboSurvey.SelectedIndex = 0;
        ddlYearAdd.SelectedDate = null;
        displayControl(null);
    }

    protected void ddlFieldNameAdd_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        displayControl(e.Text);
    }

    private void displayControl(string context)
    {
        RadTextBox valueText = txtValueAdd;
        RadComboBox CertificateComboBox = RadComboCertificate;
        RadComboBox SurveyTypeComboBox = RadComboSurveyType;
        RadComboBox SurveyComboBox = RadComboSurvey;
        RadLabel lblCombosurveyType = lblComboSurveyType;

        RadDatePicker ddlYear = ddlYearAdd;
        RadDropDownList condition = ddlConditionAdd;
        RadRadioButtonList earlier = chkEarlierLaterAdd;
        RadRadioButtonList before = chkBeforeAfterAdd;

        if (context == lblYearBuild)
        {
            ddlYear.Visible = true;
            valueText.Visible = false;
            CertificateComboBox.Visible = false;
            SurveyTypeComboBox.Visible = false;
            SurveyComboBox.Visible = false;
            lblCombosurveyType.Visible = false;

            before.Visible = true;
            earlier.Visible = false;
            condition.Visible = false;
        }
        else if (context == lblSurveyType)
        {
            CertificateComboBox.Visible = false;
            SurveyTypeComboBox.Visible = false;
            SurveyComboBox.Visible = true;
            lblCombosurveyType.Visible = false;

            valueText.Visible = false;
            ddlYear.Visible = false;


            before.Visible = false;
            earlier.Visible = true;
            condition.Visible = false;
        }
        else if (context == lblCertificate)
        {
            valueText.Visible = false;
            ddlYear.Visible = false;
            CertificateComboBox.Visible = true;
            SurveyTypeComboBox.Visible = true;
            SurveyComboBox.Visible = false;
            lblCombosurveyType.Visible = true;

            before.Visible = false;
            earlier.Visible = true;
            condition.Visible = false;
        }
        else if (context == lblDocking)
        {
            ddlYear.Visible = true;
            valueText.Visible = false;
            CertificateComboBox.Visible = false;
            SurveyTypeComboBox.Visible = false;
            SurveyComboBox.Visible = false;
            lblCombosurveyType.Visible = false;

            before.Visible = false;
            earlier.Visible = true;
            condition.Visible = false;
        }
        else if (context == lblKeelaid)
        {
            ddlYear.Visible = true;
            valueText.Visible = false;
            CertificateComboBox.Visible = false;
            SurveyTypeComboBox.Visible = false;
            SurveyComboBox.Visible = false;
            lblCombosurveyType.Visible = false;

            before.Visible = true;
            earlier.Visible = false;
            condition.Visible = false;
        }
        else
        {
            valueText.Visible = true;
            ddlYear.Visible = false;
            CertificateComboBox.Visible = false;
            SurveyTypeComboBox.Visible = false;
            SurveyComboBox.Visible = false;
            lblCombosurveyType.Visible = false;
            before.Visible = false;
            earlier.Visible = false;
            condition.Visible = true;
        }
    }

    protected void RadMCUser_TextChanged(object sender, EventArgs e)
    {
        ViewState["CertificateSelectedValue"] = RadComboCertificate.SelectedValue;
    }


}