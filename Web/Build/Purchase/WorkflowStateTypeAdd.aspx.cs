using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowStateTypeAdd : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowStateTypeAdd.MenuList = toolbarmain.Show();
    }


    protected void MenuWorkflowStateTypeAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Name = General.GetNullableString(txtName.Text);
            string Shortcode = General.GetNullableString(txtShortCode.Text);
            string Description = General.GetNullableString(txtDescription.Text);

          

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidStockType(Name, Shortcode, Description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkForm.StateTypeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Name, Shortcode, Description);
                ucStatus.Text = "StateType Insert";         
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


    private bool IsValidStockType(string Name, string Shortcode, string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Shortcode == null)
            ucError.ErrorMessage = "Shortcode is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }
}
