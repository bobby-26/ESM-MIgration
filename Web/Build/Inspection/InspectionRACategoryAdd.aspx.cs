using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class Inspection_InspectionRACategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CATEGORYID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != string.Empty)
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();

                BindVesselTypeList();
                BindActivity();
                BindRAType();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataBindings.DataTextField = "FLDHARDNAME";
        chkVesselType.DataBindings.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }

    protected void BindActivity()
    {
        if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixInspectionRiskAssessmentCategoryExtn.EditRiskAssessmentCategory(int.Parse(ViewState["CATEGORYID"].ToString()));
            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtcode.Text = dr["FLDCODE"].ToString();
                txtname.Text = dr["FLDNAME"].ToString();
                txtColorAdd.SelectedColor = System.Drawing.ColorTranslator.FromHtml(dr["FLDCOLOR"].ToString());
                ddlRAType.SelectedValue = dr["FLDNONROUTINERATYPE"].ToString();
                General.RadBindCheckBoxList(chkVesselType, dr["FLDVESSELTYPELIST"].ToString());
                if (dr["FLDDAILYWORKPLANYN"].ToString().Equals("1"))
                    chkDailyWorkPlan.Checked = true;  
            }
        }
    }
    protected void BindRAType()
    {
        DataTable dt = new DataTable();
        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentType();

        ddlRAType.DataSource = dt;
        ddlRAType.DataTextField = "FLDNAME";
        ddlRAType.DataValueField = "FLDCATEGORYID";
        ddlRAType.DataBind();
        ddlRAType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() != string.Empty)
                    UpdateActivity();
                else
                    InsertActivity();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InsertActivity()
    {
        if (!IsValidRAActivity())
        {
            ucError.Visible = true;
            return;
        }

        string color = System.Drawing.ColorTranslator.ToHtml(txtColorAdd.SelectedColor);

        PhoenixInspectionRiskAssessmentCategoryExtn.InsertRiskAssessmentCategory(
                                                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , txtname.Text.Trim()
                                                          , txtcode.Text.Trim()
                                                          , General.GetNullableString(color)
                                                          , General.GetNullableInteger(ddlRAType.SelectedValue.ToString())
                                                          , General.GetNullableString(General.RadCheckBoxList(chkVesselType))
                                                          , chkDailyWorkPlan.Checked==true?1:0);

        ucStatus.Text = "Information updated.";

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateActivity()
    {
        if (!IsValidRAActivity())
        {
            ucError.Visible = true;
            return;
        }

        string color = System.Drawing.ColorTranslator.ToHtml(txtColorAdd.SelectedColor);

        PhoenixInspectionRiskAssessmentCategoryExtn.UpdateRiskAssessmentCategory(
                                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , int.Parse(ViewState["CATEGORYID"].ToString())
                                                                         , txtname.Text.Trim()
                                                                         , txtcode.Text.Trim()
                                                                         , General.GetNullableString(color)
                                                                         , General.GetNullableInteger(ddlRAType.SelectedValue.ToString())
                                                                         , General.GetNullableString(General.RadCheckBoxList(chkVesselType))
                                                                         , chkDailyWorkPlan.Checked == true ? 1 : 0);

        ucStatus.Text = "Information updated.";
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidRAActivity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtcode.Text.Trim()) == null)
            ucError.ErrorMessage = "Code is required.";

        if (General.GetNullableString(txtname.Text.Trim()) == null)
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    
}