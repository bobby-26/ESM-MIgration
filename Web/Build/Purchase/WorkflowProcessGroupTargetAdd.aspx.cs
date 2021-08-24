using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessGroupTargetAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowProcessGroupTargetAdd.MenuList = toolbarmain.Show();


        UcProcessGroupAdd.SelectedProcessGroup = (Request.QueryString["GROUPID"].ToString());
        UcProcess.SelectedProcess = (Request.QueryString["PROCESSID"].ToString());
        UcProcessTargetAdd.ProcessId = (Request.QueryString["PROCESSID"].ToString());
        UcProcessGroupAdd.ProcessId = (Request.QueryString["PROCESSID"].ToString());
    }

    protected void MenuWorkflowProcessGroupTargetAdd_TabStripCommand(object sender, EventArgs e)
    {
            try
            {
                RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
                string CommandName = ((RadToolBarButton)dce.Item).CommandName;

                Guid? Process = General.GetNullableGuid(UcProcess.SelectedProcess);
                Guid? Group = General.GetNullableGuid(UcProcessGroupAdd.SelectedProcessGroup);
                int? TargetId = General.GetNullableInteger(UcProcessTargetAdd.SelectedProcessTarget);

                PhoenixWorkflow.ProcessGroupTargetInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     Process,
                      Group,
                     TargetId
                  );

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
    
