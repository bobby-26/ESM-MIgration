using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class WorkflowProcessActionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
          
            MenuProcessActionAdd.MenuList = toolbarmain.Show();

            ViewState["PROCESSID"] = "";
            ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuProcessActionAdd_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gv in gvProcessActionAdd.Items)
                {
                    if (((RadCheckBox)(gv.FindControl("ChkAction"))).Checked == true)
                    {
                        Guid? ActionType = General.GetNullableGuid(((RadLabel)gv.FindControl("lblActiontypeId")).Text);
                        Guid? Action = General.GetNullableGuid(((RadLabel)gv.FindControl("lblActionId")).Text);

                        PhoenixWorkflow.ProcessActionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Action, ActionType, General.GetNullableGuid(ViewState["PROCESSID"].ToString()));

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

    protected void gvProcessActionAdd_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable data1 = PhoenixWorkflow.WorkflowActionPickList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessActionAdd.DataSource = data1;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}