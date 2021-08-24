using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPEARSRALOHAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuRALOHAdd.AccessRights = this.ViewState;
            MenuRALOHAdd.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["LOHID"] = "";

                if (Request.QueryString["LOHID"] != null && Request.QueryString["LOHID"].ToString() != string.Empty)
                    ViewState["LOHID"] = Request.QueryString["LOHID"].ToString();

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
    public void BindLOH()
    {
        try
        {
            if (ViewState["LOHID"] != null && ViewState["LOHID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInspectionPEARSRiskassessmentLOH.EditRALOH(Int32.Parse(ViewState["LOHID"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtCode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                    ucScore.Text = ds.Tables[0].Rows[0]["FLDSCORE"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
                    cbActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuRALOHAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InserUpdatetLOH();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InserUpdatetLOH()
    {
        try
        {
            if (!IsValidLoh())
            {
                ucError.Visible = true;
                return;
            }

            int Activeyn = cbActiveyn.Checked == true ? 1 : 0;

            PhoenixInspectionPEARSRiskassessmentLOH.InsertRALOH(General.GetNullableInteger(ViewState["LOHID"].ToString())
                , General.GetNullableString(txtCode.Text.Trim())
                , General.GetNullableString(txtName.Text.Trim())
                , Int32.Parse(ucScore.Text)
                , General.GetNullableString(txtRemarks.Text.Trim())
                , Activeyn);

            ucStatus.Text = "Information updated.";

            String script = String.Format("javascript:fnReloadList('LOH',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    private bool IsValidLoh()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtCode.Text.Trim()) == null)
            ucError.ErrorMessage = "Code is required.";

        if (General.GetNullableString(txtName.Text.Trim()) == null)
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableString(ucScore.Text.Trim()) == null)
            ucError.ErrorMessage = "Score is required.";

        if (General.GetNullableString(txtRemarks.Text.Trim()) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }
}