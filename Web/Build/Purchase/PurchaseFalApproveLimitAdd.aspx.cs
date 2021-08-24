using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class PurchaseFalApproveLimitAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuPurchaseFalApproveLimitAdd.MenuList = toolbarmain.Show();

    }

    protected void MenuPurchaseFalApproveLimitAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            decimal? Level = General.GetNullableDecimal(lblLevel.Text);
            Guid? Rule = General.GetNullableGuid(UcRules.SelectedPurchaseFalRules);



            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidApproveLimit(Level, Rule))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPurchaseFalApprove.PurchaseFalApproveLimitInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Level
                    , Rule
                    , General.GetNullableGuid(Request.QueryString["APPROVALID"].ToString())
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

    private bool IsValidApproveLimit(decimal? Level, Guid? Rule)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((Level) == null)
            ucError.ErrorMessage = "Level is required.";

        if (Rule == null)
            ucError.ErrorMessage = "Rule is required.";

        return (!ucError.IsError);
    }




}