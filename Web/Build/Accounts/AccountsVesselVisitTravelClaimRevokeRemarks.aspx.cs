using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelClaimRevokeRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Revoke", "REVOKE", ToolBarDirection.Right);

            MenuRevoke.AccessRights = this.ViewState;
            MenuRevoke.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["VisitId"] = Request.QueryString["visitId"];
                ViewState["TravelClaimId"] = Request.QueryString["TravelClaimId"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRevoke_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("REVOKE"))
            {

                if (!IsValidData(txtRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimRevokeApprovalMaillist(new Guid(ViewState["VisitId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];

                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimRevokeApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , new Guid(ViewState["VisitId"].ToString())
                                                                               , new Guid(ViewState["TravelClaimId"].ToString())
                                                                               , txtRemarks.Text);

                string emailbodytext = "Dear Sir/Madam, </br></br> Following Vessel/Business/Office claims are revoked for your kind review. </br></br>" + dr["FLDFORMNUMBER"].ToString() + "</br> </br> Thanks & Best Regards </br> </br> System Administrative team </br>(Auto email)<br> </br> Please click the below link to view the Revoke Remarks.</br></br>" + dr["FLDURL"].ToString();

                PhoenixMail.SendMail(dr["FLDTOMAIL"].ToString(), null, null, "Vessel/Business/Office claims – Revoke Approval.", emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");

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

    private bool IsValidData(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (remarks.Trim().Length == 0)
        {
            ucError.ErrorMessage = "Revoke Remarks is required.";
        }
        return (!ucError.IsError);
    }
}
