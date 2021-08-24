using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class WorkflowEmailAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
         
            ViewState["TRANSITIONID"] = General.GetNullableGuid(Request.QueryString["TRANSITIONID"].ToString());
            ViewState["DTKEY"] = General.GetNullableGuid(Request.QueryString["DTKEY"].ToString());

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWorkflowEmail.MenuList = toolbarmain.Show();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuWorkflowEmail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
              RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
              string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        
            string To       = General.GetNullableString(txtTo.Text);
            string CC       = General.GetNullableString(txtCC.Text);
            string Subject  = General.GetNullableString(txtSubject.Text);
            string Body     = General.GetNullableString(txtbody.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidEmail(To,CC,Subject,Body))
                {
                    ucError.Visible = true;
                    return;
                }
              
                PhoenixWorkflow.WorkFlowEmailInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    General.GetNullableGuid(ViewState["TRANSITIONID"].ToString()),
                                                    To,
                                                    CC,
                                                    Subject,
                                                    Body,
                                                    General.GetNullableGuid(ViewState["DTKEY"].ToString())
                                                    );

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

    private bool IsValidEmail(   string To, string CC, string Subject, string Body)
    {
        ucError.HeaderMessage = "Please provide the following required information";
      
        if (To == null)
            ucError.ErrorMessage = "To is required.";

        if (CC == null)
            ucError.ErrorMessage = "CC is required.";

        if (Subject == null)
            ucError.ErrorMessage = "Subject is required.";

        if (Body == null)
            ucError.ErrorMessage = "Body is required.";

        return (!ucError.IsError);
    }




}