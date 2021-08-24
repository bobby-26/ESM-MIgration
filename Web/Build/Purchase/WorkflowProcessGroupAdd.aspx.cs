using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessGroupAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowProcessGroupAdd.MenuList = toolbarmain.Show();
        ViewState["PROCESSID"] = "";
        ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());

    }

    protected void MenuWorkflowProcessGroupAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

               
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvProcessGroupAdd.Items)
                {
                    if (((RadCheckBox)(gvr.FindControl("ChkGroup"))).Checked == true)
                    {
                        Guid? GroupID = General.GetNullableGuid(((RadLabel)gvr.FindControl("lblGroupId")).Text);

                        PhoenixWorkflow.ProcessGroupInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            GroupID, General.GetNullableGuid(ViewState["PROCESSID"].ToString()));

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

   


    protected void gvProcessGroupAdd_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable data = PhoenixWorkflow.WorkflowGroupPickList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessGroupAdd.DataSource = data;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}