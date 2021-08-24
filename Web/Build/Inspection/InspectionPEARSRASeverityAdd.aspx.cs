using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPEARSRASeverityAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try { 
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuRASeverityAdd.AccessRights = this.ViewState;
            MenuRASeverityAdd.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["SEVERITYID"] = "";

            if (Request.QueryString["SEVERITYID"] != null && Request.QueryString["SEVERITYID"].ToString() != string.Empty)
                ViewState["SEVERITYID"] = Request.QueryString["SEVERITYID"].ToString();

                BindSeverity();
        }
    }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
protected void BindSeverity()
{
    if (ViewState["SEVERITYID"] != null && ViewState["SEVERITYID"].ToString() != string.Empty)
    {
        DataSet ds = PhoenixInspectionPEARSRiskassessmentSeverity.EditRASeverity(int.Parse(ViewState["SEVERITYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
            txtName.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
            ucScore.Text = ds.Tables[0].Rows[0]["FLDSCORE"].ToString();
            cbActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
        }
    }
}
protected void MenuRASeverityAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InserUpdatetSeverity();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InserUpdatetSeverity()
    {
        try
        {
            if (!IsValidSeverity())
            {
                ucError.Visible = true;
                return;
            }

            int Activeyn = cbActiveyn.Checked == true ? 1 : 0;

            PhoenixInspectionPEARSRiskassessmentSeverity.InsertRASeverity(General.GetNullableInteger(ViewState["SEVERITYID"].ToString())
                                                              , General.GetNullableString(txtCode.Text.Trim())
                                                              , General.GetNullableString(txtName.Text.Trim())
                                                              , Int32.Parse(ucScore.Text)
                                                              , Activeyn);

            ucStatus.Text = "Information updated.";

            String script = String.Format("javascript:fnReloadList('Severity',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    private bool IsValidSeverity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtCode.Text.Trim()) == null)
            ucError.ErrorMessage = "Code is required.";

        if (General.GetNullableString(txtName.Text.Trim()) == null)
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableString(ucScore.Text.Trim()) == null)
            ucError.ErrorMessage = "Score is required.";

        return (!ucError.IsError);
    }
}