using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewInspectionSupdtConcernsListClosingRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCrewClosingRemarks.AccessRights = this.ViewState;
        MenuCrewClosingRemarks.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

          
            if (Request.QueryString["EmployeeFeedBackid"] != null)
            {
                ViewState["EmployeeFeedBackid"] = Request.QueryString["EmployeeFeedBackid"].ToString();

            }
            if (Request.QueryString["Date"] != null)
            {
                ViewState["Date"] = Request.QueryString["Date"].ToString();

            }

        }

    }

  
    protected void MenuCrewClosingRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='CrewClosingRemarks'>" + "\n";
            scriptClosePopup += "fnReloadList('CrewClosingRemarks');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='CrewClosingRemarksNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
             if (ViewState["EmployeeFeedBackid"] != null)
                {
             
                    if (!IsValidCrewClosingRemarks())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInspectionEventSupdtFeedback.UpdateEventSupdtEmployeewiseFeedbackClosingRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        ,General.GetNullableGuid(ViewState["EmployeeFeedBackid"].ToString())
                        , General.GetNullableString(txtClosingRemarks.Text));
                    ucStatus.Text = "Information Added";

                }
              

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
           else if (CommandName.ToUpper().Equals("NEW"))
                Reset();
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
           
            ViewState["EmployeeFeedBackid"] = null;
            ViewState["Date"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidCrewClosingRemarks()
    {
        ucError.HeaderMessage = "Please provide the following required information";

      
        if (General.GetNullableString(txtClosingRemarks.Text) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
       
        ViewState["EmployeeFeedBackid"] = null;
        ViewState["Date"] = null;
        txtClosingRemarks.Text = "";
       
    }

}