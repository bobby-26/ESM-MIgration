using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowGroupMemberAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("List", "BACK", ToolBarDirection.Right);
        MenuGroupMemberAdd.MenuList = toolbarmain.Show();
    }


    protected void MenuGroupMemberAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Group = General.GetNullableGuid(UcGroup.SelectedGroup);
            int? Target = General.GetNullableInteger(UcTarget.SelectedTarget);
            int? UserCode = General.GetNullableInteger(txtuserid.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidGroupMember(Group, Target, UserCode))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkForm.GroupMemberInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Group,
                    UserCode,
                    Target
                    );
                ucStatus.Text = "Group Member Added.";
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("WorkflowGroupMember.aspx");
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidGroupMember(Guid? Group, int? Target, int? UserCode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Group == null)
            ucError.ErrorMessage = "Group is required.";

        if (Target == null)
            ucError.ErrorMessage = "Target is required.";

        if (UserCode == null)
            ucError.ErrorMessage = "UserCode is required.";

        return (!ucError.IsError);
    }


}