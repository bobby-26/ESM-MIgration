using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class WorkflowProcessTransitionEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["TRANSITIONID"] = "";
            ViewState["TRANSITIONID"] = General.GetNullableGuid(Request.QueryString["TRANSITIONID"].ToString());

            ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuWFProcessTransitionEdit.MenuList = toolbarmain.Show();
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(MenuWFProcessTransitionEdit);
            MenuWFProcessTransitionEdit.SelectedMenuIndex = 0;

            PhoenixToolbar tool = new PhoenixToolbar();
            tool.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessActivityList.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "&TRANSITIONID=" + General.GetNullableGuid(ViewState["TRANSITIONID"].ToString()) + " ')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuWFTransitionActivity.MenuList = tool.Show();

            PhoenixToolbar t = new PhoenixToolbar();
            t.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessTransitionCheckAdd.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "&TRANSITIONID=" + General.GetNullableGuid(ViewState["TRANSITIONID"].ToString()) + " ')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuWFTransitionCheck.MenuList = t.Show();

            if (!IsPostBack)
            {

                Guid? Id = General.GetNullableGuid(ViewState["TRANSITIONID"].ToString());
                DataTable ds2;
                ds2 = PhoenixWorkflow.ProcessTransitionEdit(Id);
                if (ds2.Rows.Count > 0)
                {
                    DataRow dr = ds2.Rows[0];
                    txtProcess.Text = dr["NAME"].ToString();
                    txtShortCode.Text = dr["FLDSHORTCODE"].ToString();
                    txtName.Text = dr["FLDNAME"].ToString();
                    txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                    CurrentState.Text = dr["CURRENTSTATENAME"].ToString();
                    NextState.Text = dr["NEXTSTATENAME"].ToString();
                    UcProcessGroup.SelectedProcessGroup = dr["FLDGROUPID"].ToString();
                    UcProcessTarget.SelectedProcessTarget = dr["FLDTARGETID"].ToString();
                }
            }

            UcProcessGroup.ProcessId = (Request.QueryString["PROCESSID"].ToString());
            UcProcessTarget.ProcessId = (Request.QueryString["PROCESSID"].ToString());


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTransitionActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable T = PhoenixWorkflow.WFTransitionActivityList(General.GetNullableGuid(ViewState["TRANSITIONID"].ToString()));
            gvTransitionActivity.DataSource = T;
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
            gvTransitionActivity.Rebind();
            gvTransitionCheck.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWFProcessTransitionEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Name = General.GetNullableString(txtName.Text);
            string Description = General.GetNullableString(txtDescription.Text);
            Guid? GroupId = General.GetNullableGuid(UcProcessGroup.SelectedProcessGroup);
            int? TargetId = General.GetNullableInteger(UcProcessTarget.SelectedProcessTarget);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidProcessTransition(Name, Description, GroupId, TargetId))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixWorkflow.ProcessTransitionUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRANSITIONID"].ToString()),
                                    Name, Description, GroupId, TargetId);

            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                MenuWFProcessTransitionEdit.SelectedMenuIndex = 1;
                Response.Redirect("WorkflowProcessEdit.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidProcessTransition(string Name, string Description, Guid? GroupId, int? TargetId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        if (GroupId == null)
            ucError.ErrorMessage = "Group is required.";

        if (TargetId == null)
            ucError.ErrorMessage = "Target is required.";

        return (!ucError.IsError);
    }

    protected void Rebind()
    {
        gvTransitionActivity.SelectedIndexes.Clear();
        gvTransitionActivity.EditIndexes.Clear();
        gvTransitionActivity.DataSource = null;
        gvTransitionActivity.Rebind();


        gvTransitionCheck.SelectedIndexes.Clear();
        gvTransitionCheck.EditIndexes.Clear();
        gvTransitionCheck.DataSource = null;
        gvTransitionCheck.Rebind();

    }

    protected void gvTransitionActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? DtKey = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                PhoenixWorkflow.DeleteWorkflowEmail(PhoenixSecurityContext.CurrentSecurityContext.UserCode, DtKey);

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTransitionActivity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                Guid? DtKey = General.GetNullableGuid(((RadLabel)item.FindControl("lbldtkey")).Text);

                LinkButton Add = ((LinkButton)item.FindControl("lblActivityName"));
                if (Add != null)
                {
                    Add.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Email','Purchase/WorkflowEmailAdd.aspx?DTKEY=" + DtKey + "&TRANSITIONID=" + General.GetNullableGuid(Request.QueryString["TRANSITIONID"].ToString()) + "');return false");

                }

                LinkButton Edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (Edit != null)
                {
                    Edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Email Edit','Purchase/WorkflowEmailEdit.aspx?DTKEY=" + DtKey + "');return false");
                }

                LinkButton db = (LinkButton)item.FindControl("cmdActivityDelete");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTransitionCheck_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable ds = PhoenixWorkflowRequest.ProcessTransitionCheckList(
                General.GetNullableGuid(ViewState["PROCESSID"].ToString()),
                General.GetNullableGuid(ViewState["TRANSITIONID"].ToString())
                );

            gvTransitionCheck.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTransitionCheck_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                Guid? CheckId = General.GetNullableGuid(((RadLabel)item.FindControl("lblProcessTransitionCheckId")).Text);
                LinkButton Edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (Edit != null)
                {
                    Edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Check Edit','Purchase/WorkflowProcessTransitionCheckEdit.aspx?PROCESSTRANSITIONCHECK=" + CheckId + "');return false");
                }

                LinkButton db = (LinkButton)item.FindControl("cmdCheckDelete");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTransitionCheck_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid? CheckId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblProcessTransitionCheckId")).Text);
                PhoenixWorkflowRequest.ProcessTransitionCheckDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, CheckId);

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}