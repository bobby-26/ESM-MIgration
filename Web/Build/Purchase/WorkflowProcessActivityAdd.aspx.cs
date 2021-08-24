using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessActivityAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);        
            MenuProcessActivityAdd.MenuList = toolbarmain.Show();

            ViewState["PROCESSID"] = "";
            ViewState["PROCESSID"] = General.GetNullableGuid(Request.QueryString["PROCESSID"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuProcessActivityAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gv in gvProcessActivityAdd.Items)
                {
                    if (((RadCheckBox)(gv.FindControl("ChkActivity"))).Checked == true)
                    {
                        Guid? ActivityType = General.GetNullableGuid(((RadLabel)gv.FindControl("lblActivitytypeId")).Text);
                        Guid? Activity = General.GetNullableGuid(((RadLabel)gv.FindControl("lblActivityId")).Text);

                        PhoenixWorkflow.ProcessActivityInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Activity, ActivityType, General.GetNullableGuid(ViewState["PROCESSID"].ToString()));

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

    protected void gvProcessActivityAdd_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataTable data2 = PhoenixWorkflow.WorkflowActivityPickList(General.GetNullableGuid(ViewState["PROCESSID"].ToString()));
            gvProcessActivityAdd.DataSource = data2;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}