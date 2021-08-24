using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;


public partial class AccountsVesselVisitTravelClaimRejectRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");

                MenuCancel.AccessRights = this.ViewState;
                MenuCancel.MenuList = toolbar.Show();

                ViewState["VisitId"] = Request.QueryString["ClaimLineitemId"];
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCancel_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (!IsValidData(txtCancel.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemReject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(Request.QueryString["ClaimLineitemId"])
                     ,txtCancel.Text);
                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string remark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remark.Trim().Equals(""))
            ucError.ErrorMessage = "Remark is required.";
        return (!ucError.IsError);

    }

}
