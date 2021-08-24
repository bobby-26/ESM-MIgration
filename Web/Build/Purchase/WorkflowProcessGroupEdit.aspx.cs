using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessGroupEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["GROUPID"] = General.GetNullableGuid(Request.QueryString["GROUPID"].ToString());
            ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuWorkflowProcessGroupEdit.MenuList = toolbarmain.Show();

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessGroupTargetAdd.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "&GROUPID=" + General.GetNullableGuid(Request.QueryString["GROUPID"].ToString()) + "')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuWFGroupTarget.MenuList = tool.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvGroupTarget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkflowProcessGroupEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
                            
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("WorkflowProcessEdit.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "");
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
      
        gvGroupTarget.SelectedIndexes.Clear();
        gvGroupTarget.EditIndexes.Clear();
        gvGroupTarget.DataSource = null;
        gvGroupTarget.Rebind();
    }

    protected void gvGroupTarget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGroupTarget.CurrentPageIndex + 1;
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


            DataTable ds = PhoenixWorkflow.ProcessGroupTargetList(General.GetNullableGuid(ViewState["GROUPID"].ToString()),
                General.GetNullableGuid(ViewState["PROCESSID"].ToString()),               
                  gvGroupTarget.CurrentPageIndex + 1,
                  gvGroupTarget.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

            gvGroupTarget.DataSource = ds;
            gvGroupTarget.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvGroupTarget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {      
             if (e.CommandName.ToUpper() == "EDIT")
            {
                int? TargetId = General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblTargetId")).Text);
                Response.Redirect("WorkflowProcessGroupTargetEdit.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "&GROUPID=" + General.GetNullableGuid(ViewState["GROUPID"].ToString()) + "&TARGETID=" + TargetId + " ");

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblTargetId")).Text;
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


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvGroupTarget.Rebind();
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
            PhoenixWorkflow.DeleteProcessGroupTarget(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["ID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}