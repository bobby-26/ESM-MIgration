using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFalRulesAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);     
        MenuFalRuleAdd.MenuList = toolbarmain.Show();
        MenuFalRuleAdd.SelectedMenuIndex = 0;
    }


    protected void MenuFalRuleAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Name = General.GetNullableString(txtName.Text);
            string Shortcode = General.GetNullableString(txtShortCode.Text);
         
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPurchaseRule(Name, Shortcode))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPurchaseFalRules.PurchaseFalRuleInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     Shortcode,
                     Name,
                     chkRequiredYN.Checked.Equals(true) ? 1 : 0);
                                  
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "fnReloadList('Add', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidPurchaseRule(string Name, string Shortcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Shortcode == null)
            ucError.ErrorMessage = "Shortcode is required.";

        return (!ucError.IsError);
    }










}