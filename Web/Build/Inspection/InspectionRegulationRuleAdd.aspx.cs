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

public partial class Inspection_InspectionRegulationRuleAdd : PhoenixBasePage
{
    Guid RegulationId;
    protected void Page_Load(object sender, EventArgs e)
    {
        RegulationId = new Guid(Request.QueryString["RegulationId"]);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        NewRule.AccessRights = this.ViewState;
        NewRule.MenuList = toolbarmain.Show();

    }

    protected void NewRule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string RuleName = txtRuleName.Text;
                bool Apply = Convert.ToBoolean(chkApply.SelectedValue);
                AddRule(RegulationId, RuleName, Apply);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearUserInput();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearUserInput()
    {
        txtRuleName.Text = null;
        chkApply.SelectedIndex = 0;
    }

    private void ShowError(string Message)
    {
        ucError.ErrorMessage = Message;
        ucError.Visible = true;
    }

    private void AddRule(Guid RegulationId, string RuleName, bool Apply)
    {
        try
        {
            if (ValidateRule(RuleName))
            {
               // PhoenixInspectionNewRegulation.RuleInsert(RegulationId, RuleName, PhoenixSecurityContext.CurrentSecurityContext.UserCode, Apply);
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
        if (String.IsNullOrEmpty(rulename))
        {
            validate = false;
            ucError.ErrorMessage = "Please provide rule name";
        }
        return validate;
    }


}