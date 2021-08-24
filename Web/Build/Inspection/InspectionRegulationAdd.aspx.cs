using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationAdd : PhoenixBasePage
{
    Guid MocId;
    int usercode;

    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (Request.QueryString["Mocid"] != null)
        {
            MocId = new Guid(Request.QueryString["Mocid"]);
        }
        if (Request.QueryString["Regulationid"] != null)
        {
            ViewState["Regulationid"] = new Guid(Request.QueryString["Regulationid"]);
        }

        if (!IsPostBack)
        {
            ViewState["Regulationid"] = "";
            ViewState["RegulationStatus"] = "";
            ViewState["valuetype"] = "";
            GetRegulationDetail();
            binddropwown();
            tblRule.Visible = false;
            clear();
            ucValue.Visible = false;
            txtValue.Visible = true;
            ddlValue.Visible = false;

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                divRule.Visible = false;
                divRule.Attributes.Add("Style", "display:none;");
            }
        }
        ShowToolBar();

    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        if (MocId != null && (Request.QueryString["LaunchFrom"] == null || General.GetNullableInteger(Request.QueryString["LaunchFrom"].ToString()) == null))
        {
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        }
        if (Request.QueryString["LaunchFrom"] != null && General.GetNullableInteger(Request.QueryString["LaunchFrom"].ToString()) != null)
        {
            toolbarmain.AddButton("List", "LIST", ToolBarDirection.Right);
        }
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            if (ViewState["Regulationid"] != null && ViewState["Regulationid"].ToString() != "")
                toolbarmain.AddButton("Action Plan", "ACTION", ToolBarDirection.Right);
            toolbarmain.AddButton("Approve", "APPLY", ToolBarDirection.Right);
            if (ViewState["RegulationStatus"].ToString() == "Issued")
            {
                toolbarmain.AddButton("Apply Rule for New Vessel", "APPLYNEWVESSEL", ToolBarDirection.Right);
            }
        }
        NewRegulation.AccessRights = this.ViewState;
        NewRegulation.MenuList = toolbarmain.Show();

        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../Inspection/InspectionRegulationAdd.aspx?Mocid="+ MocId + "", "Add", "<i class=\"fas fa-plus\"></i>", "ADDRULE");
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        //toolbarmain.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionRegulationAdd.aspx'); return false;", "Add", "<i class=\"fas fa-plus\"></i>", "ADDRULE");
        ruleTabstrip.AccessRights = this.ViewState;
        ruleTabstrip.MenuList = toolbarmain.Show();
    }
    private void GetRegulationDetail()
    {
        Guid? regid = null;
        Guid? moc = null;
        if (Request.QueryString["Regulationid"] != null)
            regid = General.GetNullableGuid(Request.QueryString["Regulationid"].ToString());
        if (MocId != null && MocId.ToString() != "00000000-0000-0000-0000-000000000000")
            moc = MocId;
    DataSet ds = PhoenixInspectionNewRegulation.RegulationEdit(regid, moc);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            txtIssuedDate.Text = row["FLDISSUEDATE"].ToString();
            txtIssuedBy.Text = row["FLDISSUEDBYNAME"].ToString();
            txtTitle.Text = row["FLDTITLE"].ToString();
            txtDescription.Text = row["FLDDESCRIPTION"].ToString();
            txtActionRequired.Text = row["FLDACTIONREQUIRED"].ToString();
            txtRemarks.Text = row["FLDREMARKS"].ToString();
            txtregulationDueDate.Text = row["FLDDUEDATE"].ToString();
            ViewState["Regulationid"] = row["FLDREGULATIONID"].ToString();
            txtRuleName.Text = "Rule "+ row["FLDRULENUMBER"].ToString();
            ViewState["RegulationStatus"] = row["FLDSTATUS"].ToString();
            MocId = new Guid(row["FLDMOCID"].ToString());
        }
    }
    protected void NewRegulation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE") )
            {
                string IssuedDate = txtIssuedDate.Text;
                string IssuedBy = txtIssuedBy.Text;
                string Title = txtTitle.Text;
                string Description = txtDescription.Text;
                string ActionRequired = txtActionRequired.Text;
                string DueDate = txtregulationDueDate.Text;
                //string VesselType = chkvesselList.SelectedVesseltype;
                string Remarks = txtRemarks.Text;
                string VesselType = "";
              
                if (IsValidReport(IssuedDate, IssuedBy, Title, Description, ActionRequired, DueDate, VesselType))
                {
                    Guid RegulationId = new Guid();
                    if (General.GetNullableGuid(ViewState["Regulationid"].ToString()) == null)
                    {
                        PhoenixInspectionNewRegulation.RegulationInsert(IssuedDate, IssuedBy, Title, Description, DueDate, VesselType, ActionRequired, PhoenixSecurityContext.CurrentSecurityContext.UserCode, Remarks, MocId, ref RegulationId);
                        ViewState["Regulationid"] = RegulationId;
                    }
                    else
                    {
                        PhoenixInspectionNewRegulation.RegulationUpdate(General.GetNullableGuid(ViewState["Regulationid"].ToString()), IssuedDate, IssuedBy, Title, Description, ActionRequired, PhoenixSecurityContext.CurrentSecurityContext.UserCode, Remarks, VesselType, DueDate);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
                    ucStatus.Text = "Saved Successfully.";
                }
                else
                {
                    ucError.Visible = true;
                }
            }
            else if(CommandName.ToUpper().Equals("CLEAR"))
            {
              //  ClearUserInput();
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?MOCID=" + MocId);
            }
            else if (CommandName == "ACTION")
            {
                if (ViewState["Regulationid"] != null && ViewState["Regulationid"].ToString() != "")
                {
                    Response.Redirect("../Inspection/InspectionRegulationActionPlan.aspx?REGULATIONID=" + ViewState["Regulationid"] + "&MOCID=" + MocId + "&LaunchFrom="+Request.QueryString["LaunchFrom"], false);
                }
            }
            else if (CommandName == "APPLYNEWVESSEL")
            {
                if (ViewState["Regulationid"] != null && ViewState["Regulationid"].ToString() != "")
                {
                    RegulationVesselPopulateForNewVessel();
                    ucStatus.Text = "Applied Successfully";
                }
            }
            else if (CommandName == "APPLY")
            {
                if (ViewState["Regulationid"] != null && ViewState["Regulationid"].ToString() != "")
                {
                    RegulationVesselPopulate();
                    ucStatus.Text = "Approved Successfully";
                }
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionRegulation.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void RegulationVesselPopulate()
    {
        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixInspectionNewRegulation.RuleAppliedVesselPopulate(General.GetNullableGuid( ViewState["Regulationid"].ToString()), usercode);
    }
    private void RegulationVesselPopulateForNewVessel()
    {
        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        PhoenixInspectionNewRegulation.RuleAppliedVesselPopulateNewVessel(usercode);
    }
    private bool IsValidReport(string IssuedDate, string IssuedBy, string Title, string Description, string ActionRequired, string DueDate, string VesselType)
    {
        bool validatePass = true;
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrWhiteSpace(IssuedDate))
        {
            ucError.ErrorMessage = "Issued Date Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(IssuedBy))
        {
            ucError.ErrorMessage = "Issued By Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            ucError.ErrorMessage = "Title Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            ucError.ErrorMessage = "Description Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(ActionRequired))
        {
            ucError.ErrorMessage = "Action Required Cannot be Empty";
            validatePass = false;
        }

        //if (String.IsNullOrWhiteSpace(VesselType))
        //{
        //    ucError.ErrorMessage = "Please select a vessel type";
        //    validatePass = false;
        //}

        //if (DueDate == null)
        //{
        //    ucError.ErrorMessage = "Please select a Due Date";
        //    validatePass = false;
        //}

        return validatePass;
    }

    private void ClearUserInput()
    {
        txtTitle.Text = String.Empty;
        txtIssuedDate.Text = null;
        txtIssuedBy.Text = null;
        txtDescription.Text = null;
        txtActionRequired.Text = null;
        txtRemarks.Text = null;
        //txtDuedate.Text = null;
        //chkvesselList.SelectedVesseltype = null;
    }

    protected void ruleTabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ADDRULE") && General.GetNullableGuid(ViewState["Regulationid"].ToString()) != null)
            {
                tblRule.Visible = true;
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["Regulationid"] != null && General.GetNullableGuid(ViewState["Regulationid"].ToString()) != null)
                {
                    if (tblRule.Visible == true)
                        AddRule(new Guid(ViewState["Regulationid"].ToString()), "");
                }
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                clear();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void clear()
    {
        chkApply.Checked = false;
        txtRuleOrder.Text = string.Empty;
        ddlVesselType.SelectedIndex = 0;
        txtDateBuilt.Text = "";
        chkBeforeAfter.SelectedIndex = 0;
        ddlFieldNameAdd.SelectedIndex = 0;
        chkGreaterLesser.SelectedIndex = 0;
        txtDuedate.Text = "";
        //ddSurveyType.SelectedIndex = -1;
        //ddlCertificate.SelectedIndex = -1;
        chkEarlierLater.SelectedIndex = 0;
        tblRule.Visible = false;
    }

    protected void ddlFieldNameAdd_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
       DataSet ds= PhoenixInspectionNewRegulation.AttributeDetail(General.GetNullableGuid(ddlFieldNameAdd.SelectedValue));

        if (ds.Tables[0].Rows[0]["FLDTYPE"].ToString() == "2")
        {
            ucValue.Visible = true;
            txtValue.Visible = false;
            ddlValue.Visible = false;
        }
        else if (ds.Tables[0].Rows[0]["FLDTYPE"].ToString() == "3")
        {
            ucValue.Visible = false;
            txtValue.Visible = false;
            ddlValue.Visible = true;
        }
        else
        {
            ucValue.Visible = false;
            txtValue.Visible = true;
            ddlValue.Visible = false;
        }
    }

    protected void ddlFieldNameAdd_SelectedIndexChangedGrid(object sender, EventArgs e)
    {
        RadComboBox FieldNameAdd = (RadComboBox)sender;
        GridDataItem row = (GridDataItem)FieldNameAdd.Parent.Parent;
        RadComboBox flag = (RadComboBox)row.FindControl("ddlValue");
        UserControlDate date = (UserControlDate)row.FindControl("ucValue");
        UserControlMaskNumber number = (UserControlMaskNumber)row.FindControl("txtValue");

        DataSet ds = PhoenixInspectionNewRegulation.AttributeDetail(General.GetNullableGuid(FieldNameAdd.SelectedValue));

        if (ds.Tables[0].Rows[0]["FLDTYPE"].ToString() == "2")
        {
            date.Visible = true;
            number.Visible = false;
            flag.Visible = false;
        }
        else if (ds.Tables[0].Rows[0]["FLDTYPE"].ToString() == "3")
        {
            date.Visible = false;
            number.Visible = false;
            flag.Visible = true;
        }
        else
        {
            date.Visible = false;
            number.Visible = true;
            flag.Visible = false;
        }
        ViewState["valuetype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
    }
    private void AddRule(Guid RegulationId, string RuleName)
    {
        try
        {
            if (ValidateRule(RuleName))
            {
                Guid? RuleId  = null;
                bool Apply = Convert.ToBoolean( chkApply.Checked == true ? 1 : 0);

                string type = "";
                DataSet ds = PhoenixInspectionNewRegulation.AttributeDetail(General.GetNullableGuid(ddlFieldNameAdd.SelectedValue));

                type = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();

                string value = "";
                if (type == "3")
                    value = ddlValue.SelectedValue;
                else if (type == "2")
                    value = ucValue.Text;
                else
                    value = txtValue.Text;

                PhoenixInspectionNewRegulation.RuleInsert(RegulationId, RuleName, Apply, ref RuleId
                    ,General.GetNullableInteger(txtRuleOrder.Text)
                    ,General.GetNullableInteger(ddlVesselType.SelectedValue)
                    ,General.GetNullableDateTime(txtDateBuilt.Text)
                    ,General.GetNullableString(chkBeforeAfter.SelectedValue)
                    ,General.GetNullableGuid((ddlFieldNameAdd.SelectedValue))
                    ,General.GetNullableString(value)
                    ,General.GetNullableString(chkGreaterLesser.SelectedValue)
                    ,General.GetNullableDateTime(txtDuedate.Text)
                    ,General.GetNullableInteger(ddSurveyType.SelectedValue)
                    ,General.GetNullableString(chkEarlierLater.SelectedValue)
                    ,General.GetNullableInteger(ddlCertificate.SelectedValue));
                ViewState["Ruleid"]= RuleId;
                clear();
                gvRule.Rebind();


                ucStatus.Text = "Saved Successfully.";
            }
            else
            {
                throw new ArgumentException("");
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private bool ValidateRule(string rulename)
    {
        bool validate = true;
        ucError.HeaderMessage = "Please provide the following required information";
        //if (String.IsNullOrEmpty(rulename))
        //{
        //    validate = false;
        //    ucError.ErrorMessage = "Please provide rule name";
        //}
        return validate;
    }
    private void ShowError(string Message)
    {
        ucError.ErrorMessage = Message;
        ucError.Visible = true;
    }

    protected void gvRule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        ViewState["PAGENUMBER"] = 1;
        ds = PhoenixInspectionNewRegulation.RuleList(General.GetNullableGuid(ViewState["Regulationid"].ToString()), int.Parse(ViewState["PAGENUMBER"].ToString()), gvRule.PageSize, ref iRowCount, ref iTotalPageCount);    
        gvRule.DataSource = ds;
    }

    protected void gvRule_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "UPDATE")
            {
                bool Apply = Convert.ToBoolean(((RadCheckBox)e.Item.FindControl("chkApply")).Checked == true ? 1 : 0);
                string type = ((RadLabel)e.Item.FindControl("valuetype")).Text;

                if (ViewState["valuetype"].ToString() != "")
                    type = ViewState["valuetype"].ToString();
                string value = "";
                if (type == "3")
                    value = ((RadComboBox)e.Item.FindControl("ddlValue")).SelectedValue;
                else if (type == "2")
                    value = ((UserControlDate)e.Item.FindControl("ucValue")).Text;
                else
                    value = ((UserControlMaskNumber)e.Item.FindControl("txtValue")).Text;

                PhoenixInspectionNewRegulation.RuleUpdate(new Guid(((RadLabel)e.Item.FindControl("lblRuleID")).Text), "", Apply
                       , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("txtRuleOrder")).Text)
                       , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlVesselType")).SelectedValue)
                       , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDateBuilt")).Text)
                       , General.GetNullableString(((RadRadioButtonList)e.Item.FindControl("chkBeforeAfter")).SelectedValue)
                       , General.GetNullableGuid(((RadComboBox)e.Item.FindControl("ddlFieldNameAdd")).SelectedValue)
                       , General.GetNullableString(value)
                       , General.GetNullableString(((RadRadioButtonList)e.Item.FindControl("chkGreaterLesser")).SelectedValue)
                       , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDuedate")).Text)
                       , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddSurveyType")).SelectedValue)
                       , General.GetNullableString(((RadRadioButtonList)e.Item.FindControl("chkEarlierLater")).SelectedValue)
                       , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlCertificate")).SelectedValue));

                ViewState["valuetype"] = "";
                ucStatus.Text = "Saved Successfully";
                gvRule.Rebind();
            }
            else if(e.CommandName == "DELETE")
            {
                PhoenixInspectionNewRegulation.RuleDelete(new Guid(((RadLabel)e.Item.FindControl("lblRegulationId")).Text), new Guid(((RadLabel)e.Item.FindControl("lblRuleID")).Text), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                gvRule.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void binddropwown()
    {

        ddlVesselType.DataSource = PhoenixRegistersHard.ListHard(1, 81);
        ddlVesselType.DataTextField = "FLDHARDNAME";
        ddlVesselType.DataValueField = "FLDHARDCODE";
        ddlVesselType.DataBind();

        ddlFieldNameAdd.DataSource = PhoenixInspectionNewRegulation.RuleAttributeDropDown();
        ddlFieldNameAdd.DataTextField = "FLDATTRIBUTENAME";
        ddlFieldNameAdd.DataValueField = "FLDATTRIBUTEREGISTERID";
        ddlFieldNameAdd.DataBind();

        ddSurveyType.DataSource = PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1);
        ddSurveyType.DataValueField = "FLDSURVEYTYPEID";
        ddSurveyType.DataTextField = "FLDSURVEYTYPENAME";
        ddSurveyType.DataBind();

        ddlCertificate.DataValueField = "FLDCERTIFICATEID";
        ddlCertificate.DataTextField = "FLDCERTIFICATENAME";
        ddlCertificate.DataSource = PhoenixInspectionNewRegulation.GetNonTechinicalCertificateList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null);
        ddlCertificate.DataBind();

        ddlValue.DataSource = PhoenixRegistersFlag.ListFlag(null);
        ddlValue.DataTextField = "FLDFLAGNAME";
        ddlValue.DataValueField = "FLDFLAGID";
        ddlValue.DataBind();

    }
    protected void gvRule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadComboBox ucVesselTypeList = (RadComboBox)e.Item.FindControl("ddlVesselType");
            if (ucVesselTypeList != null)
            {
                ucVesselTypeList.DataSource = PhoenixRegistersHard.ListHard(1, 81);
                ucVesselTypeList.DataTextField = "FLDHARDNAME";
                ucVesselTypeList.DataValueField = "FLDHARDCODE";
                ucVesselTypeList.DataBind();
                ucVesselTypeList.SelectedValue = dr["FLDVESSELTYPE"].ToString();
            }
            RadComboBox ddlFieldNameAdd = (RadComboBox)e.Item.FindControl("ddlFieldNameAdd");
            if (ddlFieldNameAdd != null)
            {
                ddlFieldNameAdd.DataSource = PhoenixInspectionNewRegulation.RuleAttributeDropDown();
                ddlFieldNameAdd.DataTextField = "FLDATTRIBUTENAME";
                ddlFieldNameAdd.DataValueField = "FLDATTRIBUTEREGISTERID";
                ddlFieldNameAdd.DataBind();
                ddlFieldNameAdd.SelectedValue = dr["FLDATTRIBUTE"].ToString();
                ddlFieldNameAdd_SelectedIndexChangedGrid(ddlFieldNameAdd, null);
            }
            RadComboBox ddSurveyType = (RadComboBox)e.Item.FindControl("ddSurveyType");
            if (ddSurveyType != null)
            {
                ddSurveyType.DataSource = PhoenixRegisterVesselSurveyConfiguration.SurveyTypeList(1);
                ddSurveyType.DataValueField = "FLDSURVEYTYPEID";
                ddSurveyType.DataTextField = "FLDSURVEYTYPENAME";
                ddSurveyType.DataBind();
                ddSurveyType.SelectedValue = dr["FLDSURVEYTYPE"].ToString();
            }
            RadComboBox ddlCertificate = (RadComboBox)e.Item.FindControl("ddlCertificate");
            {
                ddlCertificate.DataValueField = "FLDCERTIFICATEID";
                ddlCertificate.DataTextField = "FLDCERTIFICATENAME";
                ddlCertificate.DataSource = PhoenixInspectionNewRegulation.GetNonTechinicalCertificateList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null);
                ddlCertificate.DataBind();
                ddlCertificate.SelectedValue = dr["FLDCERTIFICATEID"].ToString();
            }
            RadRadioButtonList chkBeforeAfter = (RadRadioButtonList)e.Item.FindControl("chkBeforeAfter");
            if(chkBeforeAfter!=null)
            {
                chkBeforeAfter.SelectedValue = dr["FLDDATEBUILTCOND"].ToString();
            }
            RadRadioButtonList chkGreaterLesser = (RadRadioButtonList)e.Item.FindControl("chkGreaterLesser");
            if (chkGreaterLesser != null)
            {
                chkGreaterLesser.SelectedValue = dr["FLDCONDITION"].ToString();
            }
            RadRadioButtonList chkEarlierLater = (RadRadioButtonList)e.Item.FindControl("chkEarlierLater");
            if (chkEarlierLater != null)
            {
                chkEarlierLater.SelectedValue = dr["FLDSURVEYCONDITION"].ToString();
            }
            RadCheckBox chkApply = (RadCheckBox)e.Item.FindControl("chkApply");
            if (chkApply != null)
            {
                chkApply.Checked = Convert.ToBoolean( dr["FLDAPPLY"].ToString());
            }

            LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null)
            {
                cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);
            }

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
            UserControlMaskNumber txtValue = (UserControlMaskNumber)e.Item.FindControl("txtValue");
            UserControlDate ucValue = (UserControlDate)e.Item.FindControl("ucValue");
            RadComboBox ddlValue = (RadComboBox)e.Item.FindControl("ddlValue");

            if (dr["FLDTYPE"].ToString() == "2")
            {
                if (ucValue != null)
                {
                    ucValue.Visible = true;
                    ucValue.Text = dr["FLDVALUE"].ToString();
                }
                if (txtValue != null)
                    txtValue.Visible = false;
                ddlValue.Visible = false;
            }
            else if (dr["FLDTYPE"].ToString() == "3")
            {
                if (ucValue != null)
                    ucValue.Visible = false;
                if (txtValue != null)
                    txtValue.Visible = false;
                if (ddlValue != null)
                {
                    ddlValue.Visible = true;
                   
                    ddlValue.DataSource =  PhoenixRegistersFlag.ListFlag(null);
                    ddlValue.DataTextField = "FLDFLAGNAME";
                    ddlValue.DataValueField = "FLDFLAGID";
                    ddlValue.DataBind();
                    ddlValue.SelectedValue = dr["FLDVALUE"].ToString();
                }
            }
            else
            {
                if (ucValue != null)
                    ucValue.Visible = false;
                if (txtValue != null)
                {
                    txtValue.Visible = true;
                    txtValue.Text= dr["FLDVALUE"].ToString();
                }
                if (ddlValue != null)
                    ddlValue.Visible = false;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}