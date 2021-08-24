using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessTransitionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowProcessTransitionAdd.MenuList = toolbarmain.Show();

        UcProcessCurrentState.ProcessId = (Request.QueryString["PROCESSID"].ToString());
        UcProcessNextState.ProcessId = (Request.QueryString["PROCESSID"].ToString());
        UcProcessGroupAdd.ProcessId = (Request.QueryString["PROCESSID"].ToString());
        UcProcessTargetAdd.ProcessId = (Request.QueryString["PROCESSID"].ToString());

    }

    protected void MenuWorkflowProcessTransitionAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Name = General.GetNullableString(txtName.Text);
            string Shortcode = General.GetNullableString(txtShortCode.Text);
            string Description = General.GetNullableString(txtDescription.Text);
            int? Rownumber = General.GetNullableInteger(txtrownumber.Text);
            Guid? Process = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());
            int? CurrentState = General.GetNullableInteger(UcProcessCurrentState.SelectedProcessState);
            int? NextState = General.GetNullableInteger(UcProcessNextState.SelectedProcessState);
            Guid? GroupId = General.GetNullableGuid(UcProcessGroupAdd.SelectedProcessGroup);
            int? TargetId = General.GetNullableInteger(UcProcessTargetAdd.SelectedProcessTarget);

            if (!IsValidProcessTransition(Process, Name, Shortcode, Description, CurrentState, NextState,Rownumber))
            {
                ucError.Visible = true;
                return;
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixWorkflow.ProcessTransitionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                    Process, Shortcode, Name, Description, CurrentState, NextState, Rownumber, GroupId,TargetId);

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


    private bool IsValidProcessTransition(Guid? Process, string Name, string Shortcode, string Description, int? CurrentState, int? NextState,int? RowNumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Process == null)
            ucError.ErrorMessage = "Process is required.";

        if (Shortcode == null)
            ucError.ErrorMessage = "Shortcode is required.";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        if (CurrentState == null)
            ucError.ErrorMessage = "CurrentState is required.";

        if (NextState == null)
            ucError.ErrorMessage = "NextState is required.";

        if (RowNumber == null)
            ucError.ErrorMessage = "RowNumber is required.";

        return (!ucError.IsError);
    }


}