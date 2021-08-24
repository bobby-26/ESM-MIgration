using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class PurchaseRulesEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuPurchaseRuleEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
      
            Guid? Id = General.GetNullableGuid(Request.QueryString["RULEID"].ToString());
            DataTable dT;
            dT = PhoenixPurchaseRules.PurchaseRulesEdit(Id);
            if (dT.Rows.Count > 0)
            {
                DataRow dr = dT.Rows[0];
                txtShortCode.Text = dr["FLDRULECODE"].ToString();
                txtName.Text = dr["FLDRULENAME"].ToString();
                if (dr["FLDREQUIRED"].ToString() == "1")
                    chkRequiredYN.Checked = true;
            }
        }


    }

    protected void MenuPurchaseRuleEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            Guid? Id = General.GetNullableGuid(Request.QueryString["RULEID"].ToString());
            string Name = General.GetNullableString(txtName.Text);
         
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRule(Name))
                {
                    ucError.Visible = true;
                    return;
                }              
                PhoenixPurchaseRules.PurchaseRulesUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Id,
                    Name,
                    chkRequiredYN.Checked.Equals(true) ? 1 : 0);
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

    private bool IsValidRule(string Name)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";    
        return (!ucError.IsError);
    }

}