using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using Newtonsoft.Json;

public partial class WorkflowRequestList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["REQUESTID"] = "";
            ViewState["REQUESTID"] = General.GetNullableGuid(Request.QueryString["REQUESTID"].ToString());
            UcProcessGroupAdd.ProcessId = Request.QueryString["PROCESSID"].ToString();
            UcProcessTargetAdd.ProcessId = Request.QueryString["PROCESSID"].ToString();

            bind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void Rebind()
    {
        gvWorkflowRequest.SelectedIndexes.Clear();
        gvWorkflowRequest.EditIndexes.Clear();
        gvWorkflowRequest.DataSource = null;
        gvWorkflowRequest.Rebind();
    }



    protected void gvWorkflowRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable DT = PhoenixWorkflow.WorkflowRequestDataList(General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvWorkflowRequest.DataSource = DT;        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void bind()
    {
        try
        {
            if (!IsPostBack)
            {
                Guid? Id = General.GetNullableGuid(Request.QueryString["REQUESTID"].ToString());
                DataTable DA;
                DA = PhoenixWorkflow.WorkflowRequestEdit(Id);
                if (DA.Rows.Count > 0)
                {
                    DataRow dr = DA.Rows[0];

                    txtRequestName.Text = dr["FLDTITLE"].ToString();
                    txtProcessName.Text = dr["PROCESSNAME"].ToString();
                    txtRequestBy.Text = dr["FLDREQUESTEDBY"].ToString();
                    txtRequestDate.Text = dr["FLDREQUESTEDDATE"].ToString();
                    txtState.Text = dr["CURRENTSTATENAME"].ToString();
                    txtUserCode.Text = dr["FLDUSERCODE"].ToString();
                    txtUserName.Text = dr["FLDUSERNAME"].ToString();
                    
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTransition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {     
            DataTable DS = new DataTable();
            DS = PhoenixWorkflow.WorkflowRequestTransitionActionList(General.GetNullableGuid(Request.QueryString["REQUESTID"].ToString()));
          

            gvTransition.DataSource = DS;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    protected void gvTransition_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                int? Complete =  General.GetNullableInteger(((RadLabel)item.FindControl("lblComplete")).Text);
                LinkButton img = ((LinkButton)item.FindControl("cmdEdit"));
                LinkButton img2 = ((LinkButton)item.FindControl("cmdActivity"));
                LinkButton img3 = ((LinkButton)item.FindControl("cmdSave"));
                if (Complete == 1)
                {
                    img.Visible = false;
                    img2.Visible = false;
                    img3.Visible = false;
                }
                                                                                            
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }     
    }


    protected void gvTransition_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if(e.CommandName.ToUpper().Equals("EDIT"))
            {
                Guid? TransitionId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbltransitionId")).Text);
                Guid? ProcessId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblProcessId")).Text);
                                          
                    DataTable ds2;
                    ds2 = PhoenixWorkflow.WORKFLOWREQUESTTRANSITIONACTIONEDIT(TransitionId);
                    if (ds2.Rows.Count > 0)
                    {

                        DataRow dr = ds2.Rows[0];
                        txtCurrentState.Text = dr["CURRENTNAME"].ToString();
                        txtNextState.Text = dr["NEXTNAME"].ToString();
                        UcProcessGroupAdd.SelectedProcessGroup = dr["FLDGROUPID"].ToString();
                        UcProcessTargetAdd.SelectedProcessTarget = dr["FLDTARGETID"].ToString();
                        lblGroupId.Text = dr["FLDGROUPID"].ToString();
                        lblTargetId.Text = dr["FLDTARGETID"].ToString();

                        ddluser.DataSource = PhoenixWorkflow.WORKFLOWAPPROVEUSER(General.GetNullableGuid(dr["FLDGROUPID"].ToString())
                            , General.GetNullableInteger(dr["FLDTARGETID"].ToString()));
                        ddluser.DataBind();
                    }              
            }


            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? TransitionId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbltransitionId")).Text);
                Guid? ProcessId = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblProcessId")).Text);
                Guid? GroupId = General.GetNullableGuid((UcProcessGroupAdd.SelectedProcessGroup).ToString());
                int? TargetId = General.GetNullableInteger((UcProcessTargetAdd.SelectedProcessTarget).ToString());
                int? UserCode = General.GetNullableInteger((ddluser.SelectedValue).ToString());


                PhoenixWorkflow.WorkflowInboxUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        ProcessId, General.GetNullableGuid((ViewState["REQUESTID"]).ToString()),
                                        GroupId, TargetId, TransitionId, UserCode);

                PhoenixWorkflow.WORKFLOWREQUESTTRANSITIONACTIONAPPROVE(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                               "Purchase_Requisition",General.GetNullableGuid((ViewState["REQUESTID"]).ToString()), TransitionId);

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

    protected void UcProcessGroupAdd_TextChangedEvent(object sender, EventArgs e)
    {
        Guid? GroupId = General.GetNullableGuid((UcProcessGroupAdd.SelectedProcessGroup).ToString());
        int? TargetId = General.GetNullableInteger((UcProcessTargetAdd.SelectedProcessTarget).ToString());

        ddluser.DataSource = PhoenixWorkflow.WORKFLOWAPPROVEUSER(GroupId, TargetId);

        ddluser.DataBind();
    }

    protected void UcProcessTargetAdd_TextChangedEvent(object sender, EventArgs e)
    {
        Guid? GroupId = General.GetNullableGuid((UcProcessGroupAdd.SelectedProcessGroup).ToString());
        int? TargetId = General.GetNullableInteger((UcProcessTargetAdd.SelectedProcessTarget).ToString());

        ddluser.DataSource = PhoenixWorkflow.WORKFLOWAPPROVEUSER(GroupId, TargetId);

        ddluser.DataBind();
    }
}






   

