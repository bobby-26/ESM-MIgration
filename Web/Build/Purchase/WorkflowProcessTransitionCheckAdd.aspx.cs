using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class WorkflowProcessTransitionCheckAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWFTransitionCheckAdd.MenuList = toolbarmain.Show();         
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWFTransitionCheckAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
         
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixWorkflowRequest.ProcessTransitionCheckInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString()),
                    General.GetNullableGuid(Request.QueryString["TRANSITIONID"].ToString()),
                    General.GetNullableString(txtShortCode.Text),
                    General.GetNullableString(txtName.Text)
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
}