using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowGroupMember : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Purchase/WorkflowGroupMember.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/WorkflowGroupMember.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
          // toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowGroupMemberAdd.aspx','false','600px','400px' )", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Purchase/WorkflowGroupMemberAdd.aspx", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

            MenuWorkflowGroupMember.AccessRights = this.ViewState;
            MenuWorkflowGroupMember.MenuList = toolbar.Show();

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddButton("Group", "GROUP", ToolBarDirection.Left);
            tool.AddButton("Target", "TARGET", ToolBarDirection.Left);
            tool.AddButton("Group Member", "GROUPMEMBER", ToolBarDirection.Left);
            MenuUser.AccessRights = this.ViewState;
            MenuUser.MenuList = tool.Show();
            MenuUser.SelectedMenuIndex = 2;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                gvWorkflowGroupMember.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkflowGroupMember_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                UcGroup.SelectedGroup = "";
                UcTarget.SelectedTarget = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvWorkflowGroupMember.SelectedIndexes.Clear();
        gvWorkflowGroupMember.EditIndexes.Clear();
        gvWorkflowGroupMember.DataSource = null;
        gvWorkflowGroupMember.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvWorkflowGroupMember.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 

    protected void gvWorkflowGroupMember_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkflowGroupMember.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataTable ds = PhoenixWorkForm.GroupMemberSearch(
                                    General.GetNullableGuid(UcGroup.SelectedGroup),
                                    General.GetNullableInteger(UcTarget.SelectedTarget),
                                       gvWorkflowGroupMember.CurrentPageIndex + 1,
                   gvWorkflowGroupMember.PageSize,
             ref iRowCount,
             ref iTotalPageCount);
            gvWorkflowGroupMember.DataSource = ds;
            gvWorkflowGroupMember.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvWorkflowGroupMember_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }

            if (e.CommandName.ToUpper() == "EDIT")
            {
                Guid? GroupMemberId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblId")).Text);

                Response.Redirect("WorkflowGroupMemberEdit.aspx?GROUPMEMBERID=" + GroupMemberId + " ");
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {

            PhoenixWorkForm.DeleteGroupMember(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               General.GetNullableGuid(ViewState["ID"].ToString()));         
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void gvWorkflowGroupMember_ItemDataBound(object sender, GridItemEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Item is GridDataItem)
    //        {
    //            GridDataItem item = e.Item as GridDataItem;
            
    //            LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
    //            if (edit != null)
    //            {
    //                edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','WorkflowGroup','Purchase/WorkflowGroupMemberEdit.aspx?GROUPMEMBERID=" + GroupMemberId + "','false','600px','400px');return false");
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenuUser_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GROUP"))
            {
                Response.Redirect("WorkflowGroup.aspx");
            }

            if (CommandName.ToUpper().Equals("TARGET"))
            {
                Response.Redirect("WorkflowTarget.aspx");
            }


            if (CommandName.ToUpper().Equals("GROUPMEMBER"))
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

}