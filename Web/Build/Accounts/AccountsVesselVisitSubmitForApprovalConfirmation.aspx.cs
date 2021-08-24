using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsVesselVisitSubmitForApprovalConfirmation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["VisitId"] = Request.QueryString["visitId"];
    }
    protected void cmdProceed_Click(object sender, EventArgs e)
    {
        try
        {
            Session["OPTION"] = rblConfirmation.SelectedValue.ToString();
            if (!IsValidSelect(rblConfirmation.SelectedValue))
            {
                ucError.Visible = true;
                return;
            }

            if (rblConfirmation.SelectedValue.ToString() != "3")
            {                
                PhoenixAccountsVesselVisitITSuperintendentRegister.VisitSubmitForApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VisitId"].ToString()));
            }

            string script = "javascript:fnReloadList('codehelp1');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSelect(string option)
    {
        if (option.Trim().Equals(""))
            ucError.ErrorMessage = "Please select any one option";

        return (!ucError.IsError);
    }
}
