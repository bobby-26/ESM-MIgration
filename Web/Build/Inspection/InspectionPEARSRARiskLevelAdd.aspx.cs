using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionPEARSRARiskLevelAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {        
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuRARiskLevelAdd.AccessRights = this.ViewState;
            MenuRARiskLevelAdd.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["RISKLEVELID"] = "";

                if (Request.QueryString["RISKLEVELID"] != null && Request.QueryString["RISKLEVELID"].ToString() != string.Empty)
                    ViewState["RISKLEVELID"] = Request.QueryString["RISKLEVELID"].ToString();

                BindRiskLevel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindRiskLevel()
    {
        if (ViewState["RISKLEVELID"] != null && ViewState["RISKLEVELID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionPEARSRiskLevel.EditRARiskLevel(int.Parse(ViewState["RISKLEVELID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtCode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                txtName.Text = ds.Tables[0].Rows[0]["FLDRISKLEVEL"].ToString();
                ucMinRange.Text = ds.Tables[0].Rows[0]["FLDMINRANGE"].ToString();
                ucMaxRange.Text = ds.Tables[0].Rows[0]["FLDMAXRANGE"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
                txtColorAdd.SelectedColor = System.Drawing.ColorTranslator.FromHtml(ds.Tables[0].Rows[0]["FLDCOLOR"].ToString());
                cbActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
            }
        }
    }
    protected void MenuRARiskLevel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InserUpdatetRiskLevel();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InserUpdatetRiskLevel()
    {
        try
        {
            if (!IsValidRiskLevel())
            {
                ucError.Visible = true;
                return;
            }

            string color = System.Drawing.ColorTranslator.ToHtml(txtColorAdd.SelectedColor);
            int Activeyn = cbActiveyn.Checked == true ? 1 : 0;

            PhoenixInspectionPEARSRiskLevel.InsertRARiskLevel(General.GetNullableInteger(ViewState["RISKLEVELID"].ToString())
                                                              , General.GetNullableString(txtCode.Text.Trim())
                                                              , Int16.Parse(ucMinRange.Text)
                                                              , Int16.Parse(ucMaxRange.Text)
                                                              , General.GetNullableString(txtName.Text.Trim())
                                                              , General.GetNullableString(color)
                                                              , General.GetNullableString(txtRemarks.Text.Trim())
                                                              , Activeyn);

            ucStatus.Text = "Information updated.";

            String script = String.Format("javascript:fnReloadList('RiskLevel',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    private bool IsValidRiskLevel()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtCode.Text.Trim()) == null)
            ucError.ErrorMessage = "Code is required.";

        if (General.GetNullableString(txtName.Text.Trim()) == null)
            ucError.ErrorMessage = "Risk Level is required.";

        if (General.GetNullableString(ucMinRange.Text.Trim()) == null)
            ucError.ErrorMessage = "Minimum Range is required.";

        if (General.GetNullableString(ucMaxRange.Text.Trim()) == null)
            ucError.ErrorMessage = "Maximum Range is required.";

        if (General.GetNullableString(txtRemarks.Text.Trim()) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

}