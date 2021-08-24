using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowRequestFieldAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowRequestFieldAdd.MenuList = toolbarmain.Show();
    }

    protected void MenuWorkflowRequestFieldAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Process = General.GetNullableGuid(UcProcess.SelectedProcess);
            string FieldName = General.GetNullableString(txtFieldName.Text);
            string DataType = General.GetNullableString(txtDataType.Text);
            int? Length = General.GetNullableInteger(txtLength.Text);
            int? Default = General.GetNullableInteger(txtDefault.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidProcess(Process, FieldName, DataType, Length, Default))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkflowRequest.ProcessRequestFieldInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Process, FieldName, DataType, Length, Default);

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

    private bool IsValidProcess(Guid? Process,string FieldName,string DataType,int? Length,int? Default)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Process == null)
            ucError.ErrorMessage = "Process is required.";

        if (FieldName == null)
            ucError.ErrorMessage = "FieldName is required.";

        if (DataType == null)
            ucError.ErrorMessage = "DataType is required.";

        if (Length == null)
            ucError.ErrorMessage = "Length is required.";

        if (Default == null)
            ucError.ErrorMessage = "Default is required.";

        return (!ucError.IsError);
    }
}