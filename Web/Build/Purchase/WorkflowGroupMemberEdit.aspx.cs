using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowGroupMemberEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("List", "BACK", ToolBarDirection.Right);
        MenuGroupMemberEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            Guid? Id = General.GetNullableGuid(Request.QueryString["GROUPMEMBERID"].ToString());
            DataTable dt;
            dt = PhoenixWorkForm.GroupMemberEdit(Id);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                UcGroup.SelectedGroup = dr["FLDGROUPID"].ToString();
                UcTarget.SelectedTarget = dr["FLDTARGETID"].ToString();
                txtUserName.Text = dr["NAME"].ToString();
                txtuserid.Text = dr["FLDUSERCODE"].ToString();
            }
        }

    }
    protected void MenuGroupMemberEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Id = General.GetNullableGuid(Request.QueryString["GROUPMEMBERID"].ToString());
            Guid? Group = General.GetNullableGuid(UcGroup.SelectedGroup);
            int? Target = General.GetNullableInteger(UcTarget.SelectedTarget);
            int? user = General.GetNullableInteger(txtuserid.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidGroupMember(Group, Target, user))
                {
                    ucError.Visible = true;
                    return;
                }


                PhoenixWorkForm.GroupMemberUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 Group, Target, user, Id);

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

    private bool IsValidGroupMember(Guid? Group, int? Target, int? user)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Group == null)
            ucError.ErrorMessage = "Group is required.";

        if (Target == null)
            ucError.ErrorMessage = "Target is required.";

        if (user == null)
            ucError.ErrorMessage = "user is required.";

        return (!ucError.IsError);
    }

}