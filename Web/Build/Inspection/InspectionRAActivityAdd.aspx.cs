using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionRAActivityAdd : PhoenixBasePage
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
                ViewState["ACTIVITYID"] = "";

                if (Request.QueryString["ACTIVITYID"] != null && Request.QueryString["ACTIVITYID"].ToString() != string.Empty)
                    ViewState["ACTIVITYID"] = Request.QueryString["ACTIVITYID"].ToString();

                BindActivity();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindActivity()
    {
        if (ViewState["ACTIVITYID"] != null && ViewState["ACTIVITYID"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixInspectionRiskAssessmentActivityExtn.EditRiskAssessmentActivity(int.Parse(ViewState["ACTIVITYID"].ToString()));
            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtcode.Text = dr["FLDSHORTCODE"].ToString();
                txtlActivity.Text = dr["FLDNAME"].ToString();                
                ucCategory.SelectedCategory = dr["FLDCATEGORYID"].ToString();
                ucNatureofWork.SelectedQuick = dr["FLDNATUREOFWORK"].ToString();

                chkevent.DataSource = PhoenixInspectionStandardEvent.ProcessStandardEventList(General.GetNullableInteger(dr["FLDCATEGORYID"].ToString()));
                chkevent.DataTextField = "FLDEVENTNAME";
                chkevent.DataValueField = "FLDSTANDARDEVENTID";
                chkevent.DataBind();

                General.BindCheckBoxList(chkevent, dr["FLDEVENTLIST"].ToString());
                chkDMRYN.Checked =dr["FLDDMRTIMESHEETYN"].ToString() == "1" ? true : false;
                chkOperatiponalsummary.Checked = dr["FLDOPERATIONALSUMMARYYN"].ToString() == "1" ? true : false;
            }
        }
    }

    protected void Bindevent()
    {
        chkevent.Items.Clear();

        chkevent.DataSource = PhoenixInspectionStandardEvent.ProcessStandardEventList(General.GetNullableInteger(ucCategory.SelectedCategory));
        chkevent.DataTextField = "FLDEVENTNAME";
        chkevent.DataValueField = "FLDSTANDARDEVENTID";
        chkevent.DataBind();
    }
    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["ACTIVITYID"] != null && ViewState["ACTIVITYID"].ToString() != string.Empty)
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
        PhoenixInspectionRiskAssessmentActivityExtn.InsertRiskAssessmentActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   txtlActivity.Text.Trim(), Convert.ToInt32(ucCategory.SelectedCategory), txtcode.Text.Trim(), General.GetNullableInteger(null), General.GetNullableString(General.ReadCheckBoxList(chkevent))
                   , General.GetNullableInteger(ucNatureofWork.SelectedQuick)
                   , (chkDMRYN.Checked == true) ? 1 : 0
                  , (chkOperatiponalsummary.Checked == true) ? 1 : 0);

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

        PhoenixInspectionRiskAssessmentActivityExtn.UpdateRiskAssessmentActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,int.Parse(ViewState["ACTIVITYID"].ToString()),
                  txtlActivity.Text.Trim(), Convert.ToInt32(ucCategory.SelectedCategory), txtcode.Text.Trim(), General.GetNullableInteger(null),General.GetNullableString(General.ReadCheckBoxList(chkevent))
                  ,General.GetNullableInteger(ucNatureofWork.SelectedQuick)
                  ,(chkDMRYN.Checked == true) ? 1 : 0
                  , (chkOperatiponalsummary.Checked == true) ? 1 : 0);

        ucStatus.Text = "Information updated.";
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidRAActivity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtcode.Text.Trim()) == null)
            ucError.ErrorMessage = "Short Code is required.";

        if (General.GetNullableString(txtlActivity.Text.Trim()) == null)
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableInteger(ucCategory.SelectedCategory) == null)
            ucError.ErrorMessage = "Process is required.";

        return (!ucError.IsError);
    }

    private void InsertRAActivity(string name, string categoryid, string shortcode, string companyid)
    {
        try
        {
            PhoenixInspectionRiskAssessmentActivityExtn.InsertRiskAssessmentActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                  name, Convert.ToInt32(categoryid), shortcode, General.GetNullableInteger(companyid));
        }
        catch (Exception e)
        {
            ucError.ErrorMessage = e.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ucCategory_TextChangedEvent(object sender, EventArgs e)
    {
        Bindevent();
    }
}