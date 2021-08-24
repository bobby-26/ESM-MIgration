using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseRulesConfigEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuPurchaseRuleConfigEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            vessellist();

            Guid? Id = General.GetNullableGuid(Request.QueryString["CONFIGID"].ToString());
            DataTable dT;
            dT = PhoenixPurchaseRules.PurchaseRulesConfigEdit(Id);
            if (dT.Rows.Count > 0)
            {
                DataRow dr = dT.Rows[0];
                txtRules.Text = dr["FLDRULENAME"].ToString();
                txtStockType.Text = dr["FLDSTOCKTYPE"].ToString();
                if (dr["FLDREQUIRED"].ToString() == "1")
                    chkRequiredYN.Checked = true;
                if (dr["FLDNEXTLEVELREQUIRED"].ToString() == "1")
                    chkNLRequiredYN.Checked = true;              
                txtNumberOfQuote.Text = dr["FLDNUMBER"].ToString();
                txtAmount.Text = dr["FLDAMOUNT"].ToString();
               SetCsvValue(ddlVessel,dr["FLDVESSELLIST"].ToString());
                
            }
        }
    }

    private void vessellist()
    {
        ddlVessel.DataSource = PhoenixPurchaseRules.PurchaseConfigVesselList();
        ddlVessel.DataBind();
    }


    protected void MenuPurchaseRuleConfigEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
                  
            if (CommandName.ToUpper().Equals("SAVE"))
            {
               
                PhoenixPurchaseRules.PurchaseRulesConfigUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , General.GetNullableGuid(Request.QueryString["CONFIGID"].ToString())
                   , chkRequiredYN.Checked.Equals(true) ? 1 : 0
                   , chkNLRequiredYN.Checked.Equals(true) ? 1 : 0
                   , General.GetNullableInteger(txtNumberOfQuote.Text)
                   , General.GetNullableDecimal(txtAmount.Text)
                   , General.GetNullableString(GetCsvValue(ddlVessel))
                   );

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                              "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }


}