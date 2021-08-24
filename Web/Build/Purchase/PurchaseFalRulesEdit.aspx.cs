using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseFalRulesEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuFalRuleEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {

            Guid? Id = General.GetNullableGuid(Request.QueryString["APPROVERULEID"].ToString());
            DataTable dT;

            dT = PhoenixPurchaseFalRules.PurchaseFalRuleEdit(Id);
            if (dT.Rows.Count > 0)
            {
                DataRow dr = dT.Rows[0];
                txtShortCode.Text = dr["FLDSHORTCODE"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                if (dr["FLDISACTIVE"].ToString() == "1")
                    chkRequiredYN.Checked = true;
            }
        }

    }

    protected void MenuFalRuleEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            Guid? Id = General.GetNullableGuid(Request.QueryString["APPROVERULEID"].ToString());
            string Name = General.GetNullableString(txtName.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRule(Name))
                {
                    ucError.Visible = true;
                    return;
                }


                PhoenixPurchaseFalRules.PurchaseFalRuleUpdate(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Name
                    , chkRequiredYN.Checked.Equals(true) ? 1 : 0
                    , Id);

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