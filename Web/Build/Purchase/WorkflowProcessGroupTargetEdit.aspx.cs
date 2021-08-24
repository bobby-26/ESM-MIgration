using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System;

public partial class WorkflowProcessGroupTargetEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["GROUPID"] = General.GetNullableGuid(Request.QueryString["GROUPID"].ToString());
        ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());
        ViewState["TARGETID"] = General.GetNullableInteger(Request.QueryString["TARGETID"].ToString());

        PhoenixToolbar tl= new PhoenixToolbar();
        tl.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuWfProcessGroupTargetEdit.MenuList = tl.Show();

        PhoenixToolbar tool = new PhoenixToolbar();
        tool.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessGroupMemberAdd.aspx?GROUPID=" + ViewState["GROUPID"] + "&TARGETID="+ ViewState["TARGETID"] +"&PROCESSID="+ ViewState["PROCESSID"] + " ')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuWFProcessGroupMember.MenuList = tool.Show();

        if (!IsPostBack)
        {
            int? TargetId = General.GetNullableInteger(ViewState["TARGETID"].ToString());
            Guid? GroupId = General.GetNullableGuid(ViewState["GROUPID"].ToString());
            DataTable ds1 = PhoenixWorkflow.ProcessGroupTargetEdit(TargetId,GroupId);
            if (ds1.Rows.Count > 0)
            {
                DataRow dr = ds1.Rows[0];
                txtProcess.Text = dr["PROCESSNAME"].ToString();
                txtGroup.Text = dr["GROUPNAME"].ToString();
                txtTarget.Text = dr["TARGETNAME"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            gvGroupMember.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

     
    }

    protected void gvGroupMember_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGroupMember.CurrentPageIndex + 1;

            DataTable T = PhoenixWorkflow.ProcessGroupMemberList(General.GetNullableGuid(ViewState["GROUPID"].ToString()),
                General.GetNullableGuid(ViewState["PROCESSID"].ToString()),
                General.GetNullableInteger(ViewState["TARGETID"].ToString()),
                 gvGroupMember.CurrentPageIndex + 1,
                  gvGroupMember.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
             
            gvGroupMember.DataSource = T;
            gvGroupMember.VirtualItemCount = iRowCount;
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

            gvGroupMember.Rebind();
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvGroupMember.SelectedIndexes.Clear();
        gvGroupMember.EditIndexes.Clear();
        gvGroupMember.DataSource = null;
        gvGroupMember.Rebind();
    }




    protected void MenuWfProcessGroupTargetEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
      
          if (CommandName.ToUpper().Equals("BACK"))
            {             
                Response.Redirect("WorkflowProcessGroupEdit.aspx?GROUPID=" + General.GetNullableGuid(ViewState["GROUPID"].ToString()) + "&PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvGroupMember_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
      {
          if (e.CommandName.ToUpper().Equals("DELETE"))
          {
              ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblGroupMemberid")).Text;
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
              PhoenixWorkflow.DeleteGroupMember(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["ID"].ToString()));
              Rebind();
          }
          catch (Exception ex)
          {
              ucError.ErrorMessage = ex.Message;
              ucError.Visible = true;
          }
      }
}