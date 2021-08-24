using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

            ViewState["PROCESSID"]= "";
            ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());
        
        PhoenixToolbar tool = new PhoenixToolbar();
        tool.AddButton("Save", "SAVE", ToolBarDirection.Right);
        tool.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuWorkflowProcessEdit.MenuList = tool.Show();
        MenuWorkflowProcessEdit.SelectedMenuIndex = 0;

        PhoenixToolbar p = new PhoenixToolbar();
        p.AddButton("Transition", "TRANSITION", ToolBarDirection.Left);
        p.AddButton("State", "STATE", ToolBarDirection.Left);
        p.AddButton("Action", "ACTION", ToolBarDirection.Left);
        p.AddButton("Activity", "ACTIVITY", ToolBarDirection.Left); 
        p.AddButton("Group", "GROUP", ToolBarDirection.Left);
        p.AddButton("Target", "TARGET", ToolBarDirection.Left);
        MenuWFProcess.MenuList = p.Show();
        MenuWFProcess.SelectedMenuIndex = 0;

        PhoenixToolbar PS = new PhoenixToolbar();
        PS.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessStateAdd.aspx?PROCESSID="+ General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuWorkProcessState.MenuList = PS.Show();

        PhoenixToolbar PA = new PhoenixToolbar();
        PA.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessActionAdd.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuWorkProcessAction.MenuList = PA.Show();

        PhoenixToolbar ACT = new PhoenixToolbar();
        ACT.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessActivityAdd.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuWorkProcessActivity.MenuList = ACT.Show();

        PhoenixToolbar PT = new PhoenixToolbar();
        PT.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessTransitionAdd.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuWorkProcessTransition.MenuList = PT.Show();

        PhoenixToolbar PG = new PhoenixToolbar();
        PG.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessGroupAdd.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuWorkProcessGroup.MenuList = PG.Show();

        PhoenixToolbar PTR= new PhoenixToolbar();
        PTR.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/WorkflowProcessTargetAdd.aspx?PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + "')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        MenuWorkProcessTarget.MenuList = PTR.Show();

        if (!IsPostBack)
        {
            Guid? Id = General.GetNullableGuid(ViewState["PROCESSID"].ToString());
            DataTable ds1 = PhoenixWorkflow.ProcessEdit(Id);
            if (ds1.Rows.Count > 0)
            {
                DataRow dr = ds1.Rows[0];
                txtAdministrator.Text = dr["FLDADMINISTRATOR"].ToString();
                txtUniqueName.Text = dr["FLDUNIQUENAME"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtdate.Text = dr["FLDCREATEDNAME"].ToString() + "  " + String.Format("{0:dd/MMM/yyyy}",  dr["FLDCREATEDDATE"]).ToString();
                txtprocedureName.Text = dr["FLDPROCEDURENAME"].ToString();
            }
        }

        if (!IsPostBack)
        {

            gvProcessTransition.Visible = true;
            MenuWorkProcessTransition.Visible = true;


            gvProcessState.Visible = false;
            MenuWorkProcessState.Visible = false;

            gvProcessAction.Visible = false;
            MenuWorkProcessAction.Visible = false;

            gvProcessActivity.Visible = false;
            MenuWorkProcessActivity.Visible = false;

            gvProcessGroup.Visible = false;
            MenuWorkProcessGroup.Visible = false;

            gvProcessTarget.Visible = false;
            MenuWorkProcessTarget.Visible = false;
        }
      
    }


    protected void MenuWorkflowProcessEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
      
            string Name = General.GetNullableString(txtName.Text);
            string Description = General.GetNullableString(txtDescription.Text);
        
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidProcess(Name, Description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkflow.ProcessUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["PROCESSID"].ToString()), Name, Description);               
            }

            else if (CommandName.ToUpper().Equals("BACK"))
            {
                MenuWorkflowProcessEdit.SelectedMenuIndex = 1;
                Response.Redirect("WorkflowProcess.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidProcess(string Name, string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    
    protected void gvProcessState_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable DS = PhoenixWorkflow.ProcessStateList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessState.DataSource = DS;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
  

    protected void gvProcessAction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            DataTable DS1 = PhoenixWorkflow.ProcessActionList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessAction.DataSource = DS1;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable DS2 = PhoenixWorkflow.ProcessActivityList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
           gvProcessActivity.DataSource = DS2;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessTransition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            DataTable s1 = PhoenixWorkflow.ProcessTransitionList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessTransition.DataSource = s1;
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
            gvProcessState.Rebind();
            gvProcessAction.Rebind();
            gvProcessActivity.Rebind();
            gvProcessTransition.Rebind();
            gvProcessGroup.Rebind();
            gvProcessTarget.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvProcessState.SelectedIndexes.Clear();
        gvProcessState.EditIndexes.Clear();
        gvProcessState.DataSource = null;
        gvProcessState.Rebind();

        gvProcessAction.SelectedIndexes.Clear();
        gvProcessAction.EditIndexes.Clear();
        gvProcessAction.DataSource = null;
        gvProcessAction.Rebind();


        gvProcessActivity.SelectedIndexes.Clear();
        gvProcessActivity.EditIndexes.Clear();
        gvProcessActivity.DataSource = null;
        gvProcessActivity.Rebind();

        gvProcessTransition.SelectedIndexes.Clear();
        gvProcessTransition.EditIndexes.Clear();
        gvProcessTransition.DataSource = null;
        gvProcessTransition.Rebind();

        gvProcessGroup.SelectedIndexes.Clear();
        gvProcessGroup.EditIndexes.Clear();
        gvProcessGroup.DataSource = null;
        gvProcessGroup.Rebind();

        gvProcessTarget.SelectedIndexes.Clear();
        gvProcessTarget.EditIndexes.Clear();
        gvProcessTarget.DataSource = null;
        gvProcessTarget.Rebind();

    }

    protected void gvProcessState_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("STATEDELETE"))
            {

                int? StateId = General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblStateId")).Text);
                PhoenixWorkflow.DeleteProcessState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, StateId);

                Rebind(); 
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvProcessState_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdStateDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
    }

    protected void gvProcessAction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ACTIONDELETE"))
            {
              
                Guid? ActionId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblActionId")).Text);
                PhoenixWorkflow.DeleteProcessAction(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ActionId);
                 Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvProcessAction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdActionDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
    }

    protected void gvProcessActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ACTIVITYDELETE"))
            {
                Guid? ActivityId =  General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblActivityId")).Text);                              
                PhoenixWorkflow.DeleteProcessActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ActivityId);
                 Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessActivity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdActivityDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
    }

    protected void gvProcessTransition_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "EDIT")
            {
                Guid? TransitionId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTransition")).Text);
                 
                Response.Redirect("WorkflowProcessTransitionEdit.aspx?TRANSITIONID=" + TransitionId + "&PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + " ");
            }
           
            if (e.CommandName.ToUpper().Equals("TRANSITIONDELETE"))
            {
              
                Guid? TransitionId= General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTransition")).Text);
                PhoenixWorkflow.DeleteProcessTransition(PhoenixSecurityContext.CurrentSecurityContext.UserCode, TransitionId);
                 gvProcessTransition.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvProcessTransition_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdTransitionDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
      
        }
    }



    protected void gvProcessGroup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable T1 = PhoenixWorkflow.ProcessGroupList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessGroup.DataSource = T1;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessGroup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper() == "EDIT")
            {
                Guid? GroupId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblGroupid")).Text);

                Response.Redirect("WorkflowProcessGroupEdit.aspx?GROUPID=" + GroupId + "&PROCESSID=" + General.GetNullableGuid(ViewState["PROCESSID"].ToString()) + " ");
            }

            if (e.CommandName.ToUpper().Equals("GROUPDELETE"))
            {              
                Guid? GroupId =  General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblGroupid")).Text);
                PhoenixWorkflow.DeleteProcessGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode, GroupId);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessGroup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdGroupDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
    }

    protected void MenuWFProcess_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
         
            if (CommandName.ToUpper().Equals("STATE"))
            {
                MenuWFProcess.SelectedMenuIndex = 1;
                gvProcessState.Visible = true;
                gvProcessAction.Visible = false;
                gvProcessActivity.Visible = false;
                gvProcessTransition.Visible = false;
                gvProcessGroup.Visible = false;
                gvProcessTarget.Visible = false;

                MenuWorkProcessState.Visible = true;
                MenuWorkProcessAction.Visible = false;
                MenuWorkProcessActivity.Visible = false;
                MenuWorkProcessTransition.Visible = false;
                MenuWorkProcessGroup.Visible = false;
                MenuWorkProcessTarget.Visible = false;

            }
            else if (CommandName.ToUpper().Equals("ACTION"))
            {
                MenuWFProcess.SelectedMenuIndex = 2;
                gvProcessState.Visible = false;
                gvProcessAction.Visible = true;
                gvProcessActivity.Visible = false;
                gvProcessTransition.Visible = false;
                gvProcessGroup.Visible = false;
                gvProcessTarget.Visible = false;

                MenuWorkProcessState.Visible = false;
                MenuWorkProcessAction.Visible = true;
                MenuWorkProcessActivity.Visible = false;
                MenuWorkProcessTransition.Visible = false;
                MenuWorkProcessGroup.Visible = false;
                MenuWorkProcessTarget.Visible = false;

            }
            else if (CommandName.ToUpper().Equals("ACTIVITY"))
            {
                MenuWFProcess.SelectedMenuIndex = 3;
                gvProcessState.Visible = false;
                gvProcessAction.Visible = false;
                gvProcessActivity.Visible = true;
                gvProcessTransition.Visible = false;
                gvProcessGroup.Visible = false;
                gvProcessTarget.Visible = false;

                MenuWorkProcessState.Visible = false;
                MenuWorkProcessAction.Visible = false;
                MenuWorkProcessActivity.Visible = true;
                MenuWorkProcessTransition.Visible = false;
                MenuWorkProcessGroup.Visible = false;
                MenuWorkProcessTarget.Visible = false;

            }
            else if (CommandName.ToUpper().Equals("TRANSITION"))
            {
               
                gvProcessState.Visible = false;
                gvProcessAction.Visible = false;
                gvProcessActivity.Visible = false;
                gvProcessTransition.Visible = true;
                gvProcessGroup.Visible = false;
                gvProcessTarget.Visible = false;

                MenuWorkProcessState.Visible = false;
                MenuWorkProcessAction.Visible = false;
                MenuWorkProcessActivity.Visible = false;
                MenuWorkProcessTransition.Visible = true;
                MenuWorkProcessGroup.Visible = false;
                MenuWorkProcessTarget.Visible = false;

            }
            else if (CommandName.ToUpper().Equals("GROUP"))
            {
                MenuWFProcess.SelectedMenuIndex = 4;

                gvProcessState.Visible = false;
                gvProcessAction.Visible = false;
                gvProcessActivity.Visible = false;
                gvProcessTransition.Visible = false;
                gvProcessGroup.Visible = true;
                gvProcessTarget.Visible = false;

                MenuWorkProcessState.Visible = false;
                MenuWorkProcessAction.Visible = false;
                MenuWorkProcessActivity.Visible = false;
                MenuWorkProcessTransition.Visible = false;
                MenuWorkProcessGroup.Visible = true;
                MenuWorkProcessTarget.Visible = false;
            }

            else if (CommandName.ToUpper().Equals("TARGET"))
            {
                MenuWFProcess.SelectedMenuIndex = 5;
                gvProcessState.Visible = false;
                gvProcessAction.Visible = false;
                gvProcessActivity.Visible = false;
                gvProcessTransition.Visible = false;
                gvProcessGroup.Visible = false;
                gvProcessTarget.Visible = true;

                MenuWorkProcessState.Visible = false;
                MenuWorkProcessAction.Visible = false;
                MenuWorkProcessActivity.Visible = false;
                MenuWorkProcessTransition.Visible = false;
                MenuWorkProcessGroup.Visible = false;
                MenuWorkProcessTarget.Visible = true;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




    protected void gvProcessTarget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
           DataTable DS = PhoenixWorkflow.ProcessTargetList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
           gvProcessTarget.DataSource = DS;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessTarget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (e.CommandName.ToUpper().Equals("TARGETDELETE"))
            {

                int? TargetId = General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblTargetid")).Text);
                PhoenixWorkflow.DeleteProcessTarget(PhoenixSecurityContext.CurrentSecurityContext.UserCode, TargetId);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessTarget_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdTargetDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
    }
}
