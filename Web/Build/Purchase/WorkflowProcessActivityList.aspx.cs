using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowProcessActivityList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWFTransitionActivityList.MenuList = toolbar.Show();

            ViewState["PROCESSID"] =   Request.QueryString["PROCESSID"].ToString();

            ViewState["TRANSITIONID"] = Request.QueryString["TRANSITIONID"].ToString();

            UcProcessActivity.ProcessId = Request.QueryString["PROCESSID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  


    protected void MenuWFTransitionActivityList_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
           RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
           string CommandName = ((RadToolBarButton)dce.Item).CommandName;
           if (CommandName.ToUpper().Equals("SAVE"))
           {


                Guid? Activity = General.GetNullableGuid(UcProcessActivity.SelectedProcessActivity);
                string Description = General.GetNullableString(txtDescription.Text);

                if (!IsValidActivity(Activity, Description))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixWorkflow.WFTransitionActivityInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                                                     General.GetNullableGuid(ViewState["TRANSITIONID"].ToString()),
                                                     General.GetNullableGuid(ViewState["PROCESSID"].ToString()),
                                                     Activity,
                                                     Description

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

    private bool IsValidActivity(Guid? Activity, string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((Activity) == null)
            ucError.ErrorMessage = "ActionTypeId is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }



}