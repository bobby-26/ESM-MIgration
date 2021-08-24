using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseRulesConfigAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuPurchaseRuleConfigAdd.MenuList = toolbarmain.Show();
        MenuPurchaseRuleConfigAdd.SelectedMenuIndex = 0;
        if (!IsPostBack)
        {
            vessellist();
        }
    }

    private void vessellist()
    {
        ddlVessel.DataSource = PhoenixPurchaseRules.PurchaseConfigVesselList();
        ddlVessel.DataBind();
    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }


    protected void MenuPurchaseRuleConfigAdd_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Rules = General.GetNullableGuid(ucRules.SelectedPurchaseRules);
            string StockType = General.GetNullableString(ddlStockType.SelectedValue);
            int? Number = General.GetNullableInteger(txtNumberOfQuote.Text);
            decimal? Amount = General.GetNullableDecimal(txtAmount.Text);
            string Vessel = General.GetNullableString(GetCsvValue(ddlVessel));

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidRules(Rules, StockType))
                {
                    ucError.Visible = true;
                    return;
                }


                PhoenixPurchaseRules.PurchaseRulesConfigInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , Rules
                   , StockType
                   , chkRequiredYN.Checked.Equals(true) ? 1 : 0
                   , chkNLRequiredYN.Checked.Equals(true) ? 1 : 0
                   , Number
                   , Amount
                   , Vessel
                 
                   );

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList('Add', 'ifMoreInfo', '','');", true);
            }

            if (CommandName.ToUpper().Equals("NEW"))
            {
                MenuPurchaseRuleConfigAdd.SelectedMenuIndex = 1;

                ucRules.SelectedPurchaseRules = "";
                ddlStockType.SelectedValue = "";
                chkRequiredYN.Checked = false;
                chkNLRequiredYN.Checked = false;
                txtNumberOfQuote.Text = "";
                txtAmount.Text = "";
                ddlVessel.ClearCheckedItems();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRules(Guid? Rules, string StockType) 
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Rules == null)
            ucError.ErrorMessage = "Rules is required.";

        if (StockType == null)
            ucError.ErrorMessage = "StockType is required.";
         
        return (!ucError.IsError);
    }




}