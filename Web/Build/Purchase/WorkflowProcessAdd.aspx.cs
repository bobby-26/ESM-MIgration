using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowProcessAdd.MenuList = toolbarmain.Show();
    }


    protected void MenuWorkflowProcessAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string UniqueName = General.GetNullableString(txtUniqueName.Text);
            string Name = General.GetNullableString(txtName.Text);        
            string Description = General.GetNullableString(txtDescription.Text);
            int? Administrator = General.GetNullableInteger(txtAdministrator.Text);
            string ProcedureName = General.GetNullableString(txtProcedureName.Text);
         
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidProcess(Name, UniqueName, Description, Administrator, ProcedureName))
                {
                    ucError.Visible = true;
                    return;
                }
             
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

    private bool IsValidProcess(string Name, string UniqueName, string Description,int? Administrator,string ProcedureName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (UniqueName == null)
            ucError.ErrorMessage = "UniqueName is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        if (Administrator == null)
            ucError.ErrorMessage = "Administrator is required.";

        if (ProcedureName == null)
            ucError.ErrorMessage = "ProcedureName is required.";

        return (!ucError.IsError);
    }
}