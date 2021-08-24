using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class WorkflowProcessTransitionCheckEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkFlowTransitionCheckEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            Guid? Id = General.GetNullableGuid(Request.QueryString["PROCESSTRANSITIONCHECK"].ToString());
            DataTable ds2;
            ds2 = PhoenixWorkflowRequest.ProcessTransitionCheckEdit(Id);
            if (ds2.Rows.Count > 0)
            {
                DataRow dr = ds2.Rows[0];
                txtProcess.Text = dr["PROCESS"].ToString();
                txtTransition.Text = dr["TRANSITION"].ToString();
                txtShortCode.Text = dr["FLDCHECKSHORTCODE"].ToString();
                txtName.Text = dr["FLDCHECKNAME"].ToString();
            }
        }

    }

    protected void MenuWorkFlowTransitionCheckEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Id = General.GetNullableGuid(Request.QueryString["PROCESSTRANSITIONCHECK"].ToString());
            string Name = General.GetNullableString(txtName.Text);
         
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixWorkflowRequest.ProcessTransitionCheckUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id, Name);

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
}