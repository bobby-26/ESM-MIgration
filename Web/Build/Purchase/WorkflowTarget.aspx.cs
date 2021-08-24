using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowTarget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Purchase/WorkflowTarget.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/WorkflowTarget.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowTargetAdd.aspx','false','450px','240px')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

            MenuWorkflowTarget.AccessRights = this.ViewState;
            MenuWorkflowTarget.MenuList = toolbar.Show();

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddButton("Group", "GROUP", ToolBarDirection.Left);
            tool.AddButton("Target", "TARGET", ToolBarDirection.Left);
            tool.AddButton("Group Member", "GROUPMEMBER", ToolBarDirection.Left);
            MenuUser.AccessRights = this.ViewState;
            MenuUser.MenuList = tool.Show();
            MenuUser.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkflowTarget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkflowTarget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkflowTarget.CurrentPageIndex + 1;
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());



            DataTable ds = PhoenixWorkForm.TargetSearch(General.GetNullableString(txtshortcode.Text.Trim()),
                    General.GetNullableString(txtname.Text.Trim()),
                  sortexpression, sortdirection,
                  gvWorkflowTarget.CurrentPageIndex + 1,
                  gvWorkflowTarget.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvWorkflowTarget.DataSource = ds;
            gvWorkflowTarget.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkflowTarget_TabStripCommand(object sender, EventArgs e)
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
                txtname.Text = "";
                txtshortcode.Text = "";
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
        gvWorkflowTarget.SelectedIndexes.Clear();
        gvWorkflowTarget.EditIndexes.Clear();
        gvWorkflowTarget.DataSource = null;
        gvWorkflowTarget.Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvWorkflowTarget.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkflowTarget_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                int? TargetId = General.GetNullableInteger(((RadLabel)item.FindControl("lblId")).Text);
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','WorkflowTarget','Purchase/WorkflowTargetEdit.aspx?TARGETID=" + TargetId + "','false','450px','240px');return false");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkflowTarget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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
            PhoenixWorkForm.DeleteTarget(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["ID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

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