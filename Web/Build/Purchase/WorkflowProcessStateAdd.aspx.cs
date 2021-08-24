using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Collections;

public partial class WorkflowProcessStateAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
           
            MenuWorkflowProcessStateAdd.AccessRights = this.ViewState;
            MenuWorkflowProcessStateAdd.MenuList = toolbar.Show();

            ViewState["PROCESSID"] = "";
            ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuWorkflowProcessStateAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvProcessStateAdd.Items)
                {
                    if (((RadCheckBox)(gvr.FindControl("ChkState"))).Checked == true)
                    {
                        int? StateType = General.GetNullableInteger(((RadLabel)gvr.FindControl("lblStateTypeId")).Text);
                        int? State = General.GetNullableInteger(((RadLabel)gvr.FindControl("lblStateId")).Text);
                        PhoenixWorkflow.ProcessStateInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, State, StateType, General.GetNullableGuid(ViewState["PROCESSID"].ToString()));

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                    }
                }        
            }
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessStateAdd_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable data = PhoenixWorkflow.WorkflowStatePickList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessStateAdd.DataSource = data;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}