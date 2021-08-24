using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class PurchaseFalApproveAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuPurchaseFalApproveAdd.MenuList = toolbarmain.Show();
        MenuPurchaseFalApproveAdd.SelectedMenuIndex = 0;
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


    protected void MenuPurchaseFalApproveAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String StockType = General.GetNullableString(ddlStockType.SelectedValue);
            int? Level = General.GetNullableInteger(lblLevel.Text);
            string Name = General.GetNullableString(txtLevelName.Text);        
            decimal? Maximum = General.GetNullableDecimal(lblMaximum.Text);
            Guid? Group = General.GetNullableGuid(UcGroup.SelectedGroup);
            int? Tatrget = General.GetNullableInteger(UcTarget.SelectedTarget);
            string Vessel = General.GetNullableString(GetCsvValue(ddlVessel));

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidApprove(StockType, Level, Name, Maximum, Group, Tatrget, Vessel))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPurchaseFalApprove.PurchaseFalApproveInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , StockType
                    , Level
                    , Name
                    , chkRequiredYN.Checked.Equals(true) ? 1 : 0                  
                    , Maximum
                    , Group
                    , Tatrget
                    , Vessel
                    , "IN-BGT"

                    );

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

    private bool IsValidApprove(string StockType, int? Level, string Name, decimal? Maximum, Guid? Group, int? Tatrget, string Vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((StockType) == null)
            ucError.ErrorMessage = "StockType is required.";

        if (Level == null)
            ucError.ErrorMessage = "Level is required.";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";
   
        if (Maximum == null)
            ucError.ErrorMessage = "Maximum is required.";

        if (Group == null)
            ucError.ErrorMessage = "Group is required.";

        if (Tatrget == null)
            ucError.ErrorMessage = "Tatrget is required.";

        if (Vessel == null)
            ucError.ErrorMessage = "Vessel is required.";
        return (!ucError.IsError);
    }


}