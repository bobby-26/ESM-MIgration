using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionAuditDetailEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["INSPECTIONID"] = String.Empty;
                ViewState["COMPANYID"] = string.Empty;

                if (Request.QueryString["INSPECTIONID"] != null && Request.QueryString["INSPECTIONID"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["INSPECTIONID"].ToString();

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucCompany.SelectedCompany = ViewState["COMPANYID"].ToString();                    
                }
                
                BindQuestionType();
                BindInspection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void RegistersInspection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if(!IsValidInspection())
                {
                    ucError.Visible = true;
                    return;
                }

               if(ViewState["INSPECTIONID"].ToString() != string.Empty && ViewState["INSPECTIONID"] !=null)
                {
                    UpdateInspection();
                }
                else
                {                    
                    InsertInspection();
                }
                ucStatus.Text = "Inspection updated Successfully";

                String script = String.Format("javascript:fnReloadList('IndpecDetail',null,null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindQuestionType()
    {
        DataSet ds = PhoenixInspection.QuestionType();
        ddlQuestionTypeAdd.DataSource = ds;
        ddlQuestionTypeAdd.DataTextField = "FLDQUICKNAME";
        ddlQuestionTypeAdd.DataValueField = "FLDQUICKTYPECODE";
        ddlQuestionTypeAdd.DataBind();
    }
    private void BindInspection()
    {
        try
        {
            if (ViewState["INSPECTIONID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInspection.EditInspection(new Guid(ViewState["INSPECTIONID"].ToString()));
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                    ucInspectionCategoryAdd.SelectedHard = ds.Tables[0].Rows[0]["FLDINSPECTIONCATEGORYID"].ToString();
                    txtShortcode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["FLDINSPECTIONNAME"].ToString();
                    chkActiveYN.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                    chkAddToScheduleYN.Checked = ds.Tables[0].Rows[0]["FLDADDTOSCHEDULE"].ToString() == "1" ? true : false;
                    chkOfficeAuditYN.Checked = ds.Tables[0].Rows[0]["FLDOFFICEYN"].ToString() == "1" ? true : false;
                    if (ds.Tables[0].Rows[0]["FLDOFFICEYN"].ToString() == "1")
                    {
                        ucCompany.Enabled = true;
                    }
                    else
                    {
                        ucCompany.Enabled = false;
                    }
                    txtFrequency.Text = ds.Tables[0].Rows[0]["FLDFREQUENCYINMONTHS"].ToString();
                    ucWindowBefore.Text = ds.Tables[0].Rows[0]["FLDWINDOWPERIODBEFORE"].ToString();
                    ucWindowAfter.Text = ds.Tables[0].Rows[0]["FLDWINDOWPERIODAFTER"].ToString();
                    txtInspectionType.Text = ds.Tables[0].Rows[0]["FLDINSPECTIONTYPE"].ToString();
                    ddlQuestionTypeAdd.SelectedValue = ds.Tables[0].Rows[0]["FLDQUESTIONTYPE"].ToString();
                    txtAssessmentStd.Text = ds.Tables[0].Rows[0]["FLDASSESSMENTSTANDARDS"].ToString();
                    txtlettercode.Text = ds.Tables[0].Rows[0]["FLDSIXLETTERCODE"].ToString();

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void chkOfficeAuditYN_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOfficeAuditYN.Checked == true)
        {
            ucCompany.Enabled = true;
        }
        else
        {
            ucCompany.Enabled = false;
        }
    }
    private void InsertInspection()
    {
        int typeid = 0;
        typeid = Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"));

        PhoenixInspection.InsertInspection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , typeid
            , Int16.Parse(ucInspectionCategoryAdd.SelectedHard)
            , General.GetNullableString(txtName.Text)
            , General.GetNullableString(txtShortcode.Text)
            , (chkActiveYN.Checked==true)?1:0
            , null
            , General.GetNullableInteger(null)
            , (chkOfficeAuditYN.Checked == true) ? 1 : 0
            , General.GetNullableInteger(ucCompany.SelectedCompany)
            , General.GetNullableInteger(txtFrequency.Text)
            , (chkAddToScheduleYN.Checked == true) ? 1 : 0
            , General.GetNullableInteger(ucWindowBefore.Text)
            , General.GetNullableInteger(ucWindowAfter.Text)
            , General.GetNullableString(txtInspectionType.Text)
            , General.GetNullableInteger(ddlQuestionTypeAdd.SelectedValue)
            , General.GetNullableString(txtAssessmentStd.Text)
            , General.GetNullableString(txtlettercode.Text)
            );
    }

    private void UpdateInspection()
    {
        int typeid = 0;
        typeid = Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(1, 148, "AUD"));

        PhoenixInspection.UpdateInspection(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["INSPECTIONID"].ToString())
            , typeid
            , Int16.Parse(ucInspectionCategoryAdd.SelectedHard)
            , txtName.Text
            , txtShortcode.Text
            , (chkActiveYN.Checked == true) ? 1 : 0
            , null
            , General.GetNullableInteger(null)
            , (chkOfficeAuditYN.Checked == true) ? 1 : 0
            , General.GetNullableInteger(ucCompany.SelectedCompany)
            , General.GetNullableInteger(txtFrequency.Text)
            , (chkAddToScheduleYN.Checked == true) ? 1 : 0
            , General.GetNullableInteger(ucWindowBefore.Text)
            , General.GetNullableInteger(ucWindowAfter.Text)
            , General.GetNullableString(txtInspectionType.Text)
            , General.GetNullableInteger(ddlQuestionTypeAdd.SelectedValue)
            , General.GetNullableString(txtAssessmentStd.Text)
            , General.GetNullableString(txtlettercode.Text)
            );
    }

    private bool IsValidInspection()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";
        //if (inspectiontypeid.Trim().Equals("Dummy") || inspectiontypeid.Trim().Equals(""))
        //    ucError.ErrorMessage = "Type is required.";
        if (ucInspectionCategoryAdd.SelectedHard.Trim().Equals("Dummy") || ucInspectionCategoryAdd.SelectedHard.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";
        if (txtShortcode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Short code is required.";
        if (txtName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        if (General.GetNullableInteger(txtFrequency.Text) == null)
            ucError.ErrorMessage = "Frequency is required.";
        if (General.GetNullableInteger(ddlQuestionTypeAdd.SelectedValue) == null)
            ucError.ErrorMessage = "QuestionType is required.";
        if (General.GetNullableString(txtlettercode.Text) == null)
            ucError.ErrorMessage = "Code is required.";

        return (!ucError.IsError);
    }
}