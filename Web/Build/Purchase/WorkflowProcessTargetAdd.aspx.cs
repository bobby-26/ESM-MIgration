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
public partial class WorkflowProcessTargetAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);       
            MenuWorkflowProcessTargetAdd.AccessRights = this.ViewState;
            MenuWorkflowProcessTargetAdd.MenuList = toolbar.Show();

            ViewState["PROCESSID"] = "";
            ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvProcessTargetAdd_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable data = PhoenixWorkflow.WorkflowTargetPickList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessTargetAdd.DataSource = data;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkflowProcessTargetAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvProcessTargetAdd.Items)
                {
                    if (((RadCheckBox)(gvr.FindControl("ChkTarget"))).Checked == true)
                    {
                        int? TargetId = General.GetNullableInteger(((RadLabel)gvr.FindControl("lblTargetId")).Text);
                     
                        PhoenixWorkflow.ProcessTargetInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, TargetId, General.GetNullableGuid(ViewState["PROCESSID"].ToString()));

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
}