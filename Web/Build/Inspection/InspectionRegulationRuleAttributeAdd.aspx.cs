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

public partial class Inspection_InspectionRegulationRuleAttributeAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        NewAttribute.AccessRights = this.ViewState;
        NewAttribute.MenuList = toolbarmain.Show();

    }

    protected void NewAttribute_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                RadTextBox AttributeCode = txtAttributeCodeAdd;
                RadTextBox AttributeName = txtAttributeNameAdd;
                RadRadioButtonList IncludeYN = chkIncludeYNAdd;
                bool includeYN = Convert.ToBoolean(IncludeYN.SelectedValue);
                bool isDeletable = false;

                if (IsValidInput(AttributeCode.Text, AttributeName.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionNewRegulation.AttributeInsert(AttributeCode.Text, AttributeName.Text, includeYN, isDeletable);
                    ucStatus.Text = "Information Added";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
                }
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

    public bool IsValidInput(string AttributeCode, string AttributeName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (AttributeCode == "")
            ucError.ErrorMessage = "Code is required";

        if (AttributeName == "")
            ucError.ErrorMessage = "Attribute Name is required";

        return (ucError.IsError);
    }
    private void ClearUserInput()
    {
        txtAttributeCodeAdd.Text = null;
        txtAttributeNameAdd = null;
        chkIncludeYNAdd.SelectedIndex = 0;
    }

}